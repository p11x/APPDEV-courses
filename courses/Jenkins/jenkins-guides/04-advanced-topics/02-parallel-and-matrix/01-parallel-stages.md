# Parallel Stages in Jenkins Pipeline

## What this covers

This guide explains how to run stages in parallel using the `parallel` directive. You'll learn how to speed up builds by running tests across multiple environments, use `failFast`, and dynamically generate parallel branches.

## Prerequisites

- Understanding of Declarative Pipeline
- Completed Pipeline basics

## What is Parallel Execution?

Parallel stages run simultaneously, reducing total build time:

```
Sequential:                    Parallel:
                                  
Build (2 min)                  Build (2 min)
   │                              │
Test (3 min)                   ├─ Test-1 (3 min)
   │                            ├─ Test-2 (3 min)
Deploy (1 min)                 └─ Deploy (1 min)
   │                              │
Total: 6 min                   Total: 5 min
```

---

## Basic Parallel Stages

```groovy
pipeline {
    agent any
    
    stages {
        stage('Build') {
            steps {
                echo 'Building...'
                sh 'make build'
            }
        }
        
        stage('Test') {
            steps {
                parallel {
                    stage('Unit Tests') {
                        steps {
                            echo 'Running unit tests...'
                            sh 'npm run test:unit'
                        }
                    }
                    
                    stage('Integration Tests') {
                        steps {
                            echo 'Running integration tests...'
                            sh 'npm run test:integration'
                        }
                    }
                    
                    stage('E2E Tests') {
                        steps {
                            echo 'Running E2E tests...'
                            sh 'npm run test:e2e'
                        }
                    }
                }
            }
        }
        
        stage('Deploy') {
            steps {
                echo 'Deploying...'
            }
        }
    }
}
```

---

## Using failFast

### failFast: true

If one parallel branch fails, cancel all others:

```groovy
stage('Test') {
    failFast: true  // Cancel other branches on failure
    
    parallel {
        stage('Unit Tests') {
            steps {
                sh 'npm run test:unit'
            }
        }
        
        stage('Integration Tests') {
            steps {
                sh 'npm run test:integration'
            }
        }
    }
}
```

### failFast: false (Default)

Other branches continue even if one fails:

```groovy
stage('Test') {
    failFast: false  // Let all branches finish
    
    parallel {
        stage('Unit Tests') { ... }
        stage('Integration Tests') { ... }
    }
}
```

---

## Parallel Across Multiple OS/Environments

```groovy
pipeline {
    agent any
    
    stages {
        stage('Build') {
            steps {
                sh 'make build'
            }
        }
        
        stage('Test on Multiple OS') {
            parallel {
                stage('Test on Ubuntu') {
                    agent {
                        label 'ubuntu'
                    }
                    steps {
                        echo 'Testing on Ubuntu...'
                        sh 'npm test'
                    }
                }
                
                stage('Test on macOS') {
                    agent {
                        label 'macos'
                    }
                    steps {
                        echo 'Testing on macOS...'
                        sh 'npm test'
                    }
                }
                
                stage('Test on Windows') {
                    agent {
                        label 'windows'
                    }
                    steps {
                        echo 'Testing on Windows...'
                        bat 'npm test'
                    }
                }
            }
        }
    }
}
```

---

## Dynamic Parallel Branches

Generate parallel stages from a list:

```groovy
pipeline {
    agent any
    
    environment {
        TEST_BROWSERS = 'chrome,firefox,safari,edge'
    }
    
    stages {
        stage('Build') {
            steps {
                sh 'make build'
            }
        }
        
        stage('Cross-Browser Tests') {
            steps {
                script {
                    def browsers = env.TEST_BROWSERS.split(',')
                    
                    def branches = [:]
                    
                    browsers.each { browser ->
                        branches["Browser-${browser}"] = {
                            stage("Test on ${browser}") {
                                sh "playwright test --browser=${browser}"
                            }
                        }
                    }
                    
                    parallel branches
                }
            }
        }
    }
}
```

---

## Real-World Example

```groovy
pipeline {
    agent any
    
    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        
        stage('Build') {
            steps {
                sh 'mvn clean package'
            }
        }
        
        stage('Test in Parallel') {
            failFast: false
            
            parallel {
                // Fast tests first
                stage('Unit Tests') {
                    steps {
                        sh 'mvn test -Dtest=*Test'
                        junit 'target/surefire-reports/*.xml'
                    }
                }
                
                // Slow integration tests
                stage('Integration Tests') {
                    steps {
                        sh 'mvn verify -Pit'
                        junit 'target/failsafe-reports/*.xml'
                    }
                }
                
                // Performance tests
                stage('Performance Tests') {
                    steps {
                        sh './run-perf-tests.sh'
                    }
                }
                
                // Security scan
                stage('Security Scan') {
                    steps {
                        sh 'trivy image --severity HIGH myapp:latest || true'
                    }
                }
            }
        }
        
        stage('Deploy') {
            when {
                branch 'main'
            }
            steps {
                echo 'Deploying...'
            }
        }
    }
    
    post {
        always {
            junit '**/test-results/*.xml'
        }
    }
}
```

---

## Performance Considerations

### When to Use Parallel

| Good for Parallel | Bad for Parallel |
|-------------------|------------------|
| Independent tests | Sequential tests |
| Multiple browsers | Dependent stages |
| Multiple configurations | Quick stages |
| Long-running tests | Setup/teardown heavy |

### Balancing Load

```groovy
// Don't parallelize too finely
parallel {
    stage('Test-A') { ... }  // 30 sec
    stage('Test-B') { ... }  // 30 sec
}

// Better: Group related tests
parallel {
    stage('Frontend Tests') { ... }  // 30 sec (all frontend)
    stage('Backend Tests') { ... }    // 30 sec (all backend)
}
```

---

## Common Mistakes

### Forgetting failFast

```groovy
// ⚠️ May waste resources if one branch fails early
stage('Test') {
    parallel {
        stage('Slow Test') {
            steps {
                sleep 60
                error 'Failed!'
            }
        }
        stage('Fast Test') {
            steps {
                sleep 5
            }
        }
    }
}

// ✅ Set failFast based on needs
stage('Test') {
    failFast: true
    parallel { ... }
}
```

### Resource Contention

```groovy
// ⚠️ Too many parallel stages may overload
stage('Test') {
    parallel {
        stage('T1') { ... }  // All competing
        stage('T2') { ... }  // for same
        stage('T3') { ... }  // resources
        stage('T4') { ... }
    }
}
```

---

## Next Steps

- **[Matrix Builds](02-matrix-builds.md)** - Test across combinations
- **[Stash and Unstash](03-stash-and-unstash.md)** - Pass files between stages
- **[Kubernetes Agents](03-kubernetes-agents/01-kubernetes-plugin-setup.md)** - Scale with K8s
