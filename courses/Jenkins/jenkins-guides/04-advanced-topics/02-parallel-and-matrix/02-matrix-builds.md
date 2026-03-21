# Matrix Builds in Jenkins

## What this covers

This guide explains the `matrix` directive in Jenkins (available in Jenkins 2.367+). You'll learn how to run the same stage across multiple combinations of configurations like Node.js versions × OS types, using axes and excludes.

## Prerequisites

- Jenkins 2.367 or later
- Understanding of Pipeline syntax
- Completed parallel stages guide

## What is a Matrix Build?

A matrix build runs a stage multiple times with different combinations of values:

```
Matrix:                    Generates:
                            
Node: [16, 18, 20]        ┌─────────────────────────────┐
OS:   [linux, windows]    │ Node=16, OS=linux           │
                           │ Node=16, OS=windows          │
                           │ Node=18, OS=linux           │
                           │ Node=18, OS=windows          │
                           │ Node=20, OS=linux           │
                           │ Node=20, OS=windows          │
                           └─────────────────────────────┘
```

---

## Basic Matrix Syntax

```groovy
pipeline {
    agent any
    
    matrix {
        // Define axes - the dimensions of the matrix
        axes {
            axis {
                name 'NODE_VERSION'
                values '16', '18', '20'
            }
            axis {
                name 'OS'
                values 'linux', 'windows'
            }
        }
        
        // Exclude some combinations (optional)
        exclude {
            axis {
                name 'NODE_VERSION'
                values '16'
            }
            axis {
                name 'OS'
                values 'windows'
            }
        }
        
        // The stage to run for each combination
        stages {
            stage("Build on ${NODE_VERSION}/${OS}") {
                steps {
                    echo "Building on Node ${NODE_VERSION} and ${OS}"
                    
                    script {
                        if (isUnix()) {
                            sh "node --version"
                        } else {
                            bat "node --version"
                        }
                    }
                }
            }
        }
    }
}
```

---

## Complete Example: Node.js × OS Matrix

```groovy
pipeline {
    agent any
    
    stages {
        stage('Setup') {
            steps {
                echo 'Preparing build environment...'
            }
        }
        
        stage('Test Matrix') {
            matrix {
                axes {
                    axis {
                        name 'NODE_VERSION'
                        values '16', '18', '20'
                    }
                    axis {
                        name 'OS'
                        values 'ubuntu', 'windows'
                    }
                }
                
                exclude {
                    axis {
                        name 'NODE_VERSION'
                        values '16'
                    }
                    axis {
                        name 'OS'
                        values 'windows'
                    }
                }
                
                stages {
                    stage("Test Node ${NODE_VERSION} on ${OS}") {
                        agent {
                            label "${OS}"
                        }
                        steps {
                            echo "Testing with Node ${NODE_VERSION} on ${OS}"
                            sh "node --version"
                            sh "npm install"
                            sh "npm test"
                        }
                        post {
                            always {
                                junit 'test-results/*.xml'
                            }
                        }
                    }
                }
            }
        }
    }
}
```

---

## Matrix Excludes

### Single Exclusion

```groovy
exclude {
    axis {
        name 'NODE_VERSION'
        values '16'
    }
}
```

### Multiple Values

```groovy
exclude {
    axis {
        name 'NODE_VERSION'
        values '16', '18'
    }
}
```

### Multiple Axes

```groovy
exclude {
    axis {
        name 'NODE_VERSION'
        values '16'
    }
    axis {
        name 'OS'
        values 'windows'
    }
}
// Excludes: Node=16 AND Windows (not either!)
```

---

## Matrix with Custom Stage Names

```groovy
matrix {
    axes {
        axis {
            name 'BROWSER'
            values 'chrome', 'firefox', 'safari'
        }
    }
    
    stages {
        // Custom name using matrix values
        stage("E2E - ${BROWSER}") {
            steps {
                echo "Running E2E tests on ${BROWSER}"
                sh "playwright test --browser=${BROWSER}"
            }
        }
    }
}
```

---

## Real-World Example: Java Build Matrix

```groovy
pipeline {
    agent any
    
    stages {
        stage('Build') {
            steps {
                checkout scm
                sh 'mvn clean compile -DskipTests'
            }
        }
        
        stage('Test Matrix') {
            matrix {
                axes {
                    axis {
                        name 'JDK_VERSION'
                        values '11', '17', '21'
                    }
                    axis {
                        name 'DATABASE'
                        values 'mysql', 'postgres', 'mariadb'
                    }
                }
                
                exclude {
                    axis {
                        name 'JDK_VERSION'
                        values '21'
                    }
                    axis {
                        name 'DATABASE'
                        values 'mysql'
                    }
                }
                
                stages {
                    stage("Test JDK ${JDK_VERSION} with ${DATABASE}") {
                        steps {
                            echo "Running tests with JDK ${JDK_VERSION} and ${DATABASE}"
                            
                            script {
                                def dbConfig = getDbConfig(DATABASE)
                                sh "mvn test -Ddb.url=${dbConfig.url}"
                            }
                        }
                        post {
                            always {
                                junit "target/surefire-reports/TEST-*.xml"
                            }
                        }
                    }
                }
            }
        }
        
        stage('Package') {
            steps {
                sh 'mvn package -DskipTests'
                archiveArtifacts artifacts: 'target/*.jar'
            }
        }
    }
}

def getDbConfig(String db) {
    switch(db) {
        case 'mysql':
            return [url: 'jdbc:mysql://localhost:3306/test']
        case 'postgres':
            return [url: 'jdbc:postgresql://localhost:5432/test']
        case 'mariadb':
            return [url: 'jdbc:mariadb://localhost:3306/test']
    }
}
```

---

## Matrix vs Parallel

| Feature | Matrix | Parallel |
|---------|--------|----------|
| **Structure** | Grid/combinations | List of branches |
| **Axes** | Multiple dimensions | Single list |
| **Excludes** | Built-in | Manual |
| **Use case** | Config combinations | Independent tasks |

### When to Use Matrix

- Test across versions (Node, Java, Python)
- Test across OS (Linux, Windows, macOS)
- Test across databases
- Build across configurations

---

## Common Mistakes

### Too Many Combinations

```groovy
// ⚠️ Warning: 3 × 4 × 3 × 2 = 72 builds!
axes {
    axis { name 'A' values '1','2','3' }
    axis { name 'B' values '1','2','3','4' }
    axis { name 'C' values '1','2','3' }
    axis { name 'D' values '1','2' }
}

// ✅ Reasonable: 3 × 2 = 6 builds
axes {
    axis { name 'NODE' values '16','18','20' }
    axis { name 'OS' values 'linux','windows' }
}
```

### Forgetting Agent in Matrix

```groovy
// ⚠️ May run on wrong agent
matrix {
    axes { ... }
    stages {
        stage('Test') {
            // No agent specified
        }
    }
}

// ✅ Specify agent per matrix cell
matrix {
    axes { ... }
    stages {
        stage('Test') {
            agent { label "${OS}" }
        }
    }
}
```

---

## Next Steps

- **[Stash and Unstash](03-stash-and-unstash.md)** - Pass files between stages
- **[Kubernetes Agents](03-kubernetes-agents/01-kubernetes-plugin-setup.md)** - Scale with K8s
- **[Shared Libraries](01-shared-libraries/01-what-are-shared-libraries.md)** - Reusable matrix code
