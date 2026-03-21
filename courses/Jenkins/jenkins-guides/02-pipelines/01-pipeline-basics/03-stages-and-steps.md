# Stages and Steps in Jenkins Pipeline

## What this covers

This guide provides a deep dive into `stages`, `stage`, `steps`, `parallel`, and `sequential` stage execution. You'll learn how to create multi-stage pipelines with conditional execution, parallel builds, and stage skipping using the `when` directive.

## Prerequisites

- Understanding of Declarative Pipeline syntax
- Completed the Declarative Syntax Overview guide

## Understanding Stages

### What is a Stage?

A **stage** is a logical grouping of steps in your pipeline. Stages appear in:
- Blue Ocean visual pipeline view
- Jenkins build logs
- Pipeline step indicator

Think of stages like chapters in a book—each one represents a distinct phase of your CI/CD process.

### Example Pipeline with Multiple Stages

```groovy
pipeline {
    agent any
    
    stages {
        // Stage 1: Checkout - Get the code
        stage('Checkout') {
            steps {
                echo 'Checking out source code...'
                // git {} - Clone a Git repository
                // url: 'https://github.com/example/myapp.git'
                git 'https://github.com/jenkins-docs/simple-java-maven-app.git'
            }
        }
        
        // Stage 2: Build - Compile the code
        stage('Build') {
            steps {
                echo 'Building the application...'
                // sh {} - Execute shell commands
                sh 'mvn clean package'
            }
        }
        
        // Stage 3: Test - Run tests
        stage('Test') {
            steps {
                echo 'Running tests...'
                sh 'mvn test'
            }
        }
        
        // Stage 4: Deploy - Deploy to environment
        stage('Deploy') {
            steps {
                echo 'Deploying to production...'
                sh './deploy.sh production'
            }
        }
    }
}
```

---

## Understanding Steps

### What is a Step?

A **step** is a single actual command or action that Jenkins performs. Steps are the atomic actions inside stages.

```groovy
stage('Build') {
    steps {
        // These are all steps:
        echo 'Message'              // Print to log
        sh 'command'                // Run shell
        bat 'command'               // Run batch (Windows)
        sh '''                     // Multi-line shell
            echo "Line 1"
            echo "Line 2"
        '''
        dir('/path') { }            // Change directory
        withEnv([]) { }             // Set environment
        timeout(time: 10) { }       // Add timeout
    }
}
```

### Common Steps Reference

| Step | Purpose | Example |
|------|---------|---------|
| `echo` | Print message | `echo 'Hello'` |
| `sh` | Run shell command | `sh 'npm install'` |
| `bat` | Run batch command | `bat 'dir'` |
| `git` | Clone Git repo | `git url: 'https://github.com/...'` |
| `checkout` | Checkout from SCM | `checkout scm` |
| `dir` | Change directory | `dir('subfolder') { ... }` |
| `fileExists` | Check if file exists | `fileExists 'file.txt'` |
| `readFile` | Read file content | `readFile 'file.txt'` |
| `writeFile` | Write to file | `writeFile file: 'out.txt', text: 'content'` |
| `archiveArtifacts` | Save build artifacts | `archiveArtifacts 'target/*.jar'` |

---

## Conditional Stage Execution with `when`

The `when` directive lets you skip stages based on conditions:

```groovy
pipeline {
    agent any
    
    parameters {
        booleanParam(name: 'RUN_INTEGRATION_TESTS', defaultValue: false, description: 'Run integration tests')
    }
    
    stages {
        stage('Build') {
            steps {
                echo 'Building...'
                sh 'mvn clean package'
            }
        }
        
        stage('Unit Tests') {
            steps {
                echo 'Running unit tests...'
                sh 'mvn test'
            }
        }
        
        // This stage ONLY runs when parameter is true
        stage('Integration Tests') {
            when {
                // expression {} - Evaluate a condition
                expression { return params.RUN_INTEGRATION_TESTS }
            }
            steps {
                echo 'Running integration tests...'
                sh 'mvn verify -Pintegration-tests'
            }
        }
        
        // Only run on main branch
        stage('Deploy') {
            when {
                // branch {} - Check current branch
                branch 'main'
            }
            steps {
                echo 'Deploying to production...'
            }
        }
    }
}
```

### when Conditions Reference

| Condition | Example | Description |
|-----------|---------|-------------|
| `branch` | `branch 'main'` | Only for specific branch |
| `branch pattern` | `branch 'release/*'` | Wildcard support |
| `environment` | `env.DEPLOY_ENV == 'prod'` | Check environment variable |
| `expression` | `expression { ... }` | Groovy expression |
| `not` | `not { branch 'main' }` | Negation |
| `allOf` | `allOf { branch 'main'; environment ... }` | AND logic |
| `anyOf` | `anyOf { branch 'main'; branch 'develop' }` | OR logic |

---

## Parallel Stages

Run multiple stages simultaneously to speed up your pipeline:

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
        
        // Run multiple test suites in parallel
        stage('Test') {
            parallel {
                // First parallel branch - Unit Tests
                stage('Unit Tests') {
                    steps {
                        echo 'Running unit tests...'
                        sh 'npm run test:unit'
                    }
                }
                
                // Second parallel branch - Integration Tests
                stage('Integration Tests') {
                    steps {
                        echo 'Running integration tests...'
                        sh 'npm run test:integration'
                    }
                }
                
                // Third parallel branch - E2E Tests
                stage('E2E Tests') {
                    steps {
                        echo 'Running E2E tests...'
                        sh 'npm run test:e2e'
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

### failFast Option

Control what happens when one parallel branch fails:

```groovy
stage('Test') {
    // failFast: true = If one branch fails, cancel all others
    // failFast: false (default) = Let other branches finish
    failFast: true
    parallel {
        stage('Unit Tests') { ... }
        stage('Integration Tests') { ... }
    }
}
```

---

## Sequential Stages with Matrix

For complex configurations, use the `matrix` directive (Jenkins 2.367+):

```groovy
pipeline {
    agent any
    
    // Matrix - run same stage with different configurations
    matrix {
        // axes {} - Define the axes of the matrix
        axes {
            // axis {} - Define one dimension
            axis {
                name 'NODE_VERSION'
                values '16', '18', '20'
            }
            axis {
                name 'OS'
                values 'ubuntu', 'windows'
            }
        }
        
        // exclude {} - Exclude certain combinations
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
        
        // stages {} - The actual stages to run
        stages {
            stage("Build on ${NODE_VERSION}/${OS}") {
                steps {
                    echo "Building on Node ${NODE_VERSION} and ${OS}"
                    sh "node --version"
                }
            }
        }
    }
}
```

This generates builds for:
- Node 16 + Ubuntu
- Node 18 + Ubuntu
- Node 18 + Windows
- Node 20 + Ubuntu
- Node 20 + Windows

---

## Reusable Stage Definitions

For repeated stages, use `stage` with `steps` and reuse:

```groovy
pipeline {
    agent any
    
    stages {
        stage('Build Application') {
            steps {
                echo 'Building...'
            }
        }
        
        // Reuse similar stages
        stage('Test on Staging') {
            steps {
                echo 'Testing on staging...'
            }
        }
        
        stage('Deploy to Staging') {
            steps {
                echo 'Deploying to staging...'
            }
        }
        
        stage('Test on Production') {
            steps {
                echo 'Testing on production...'
            }
        }
        
        stage('Deploy to Production') {
            steps {
                echo 'Deploying to production...'
            }
        }
    }
}
```

---

## Stage Naming Best Practices

| Bad Name | Good Name | Why |
|----------|-----------|-----|
| `stage1` | `Checkout` | Descriptive |
| `doStuff` | `Build Application` | Clear action |
| `test` | `Run Unit Tests` | Specific |
| `deploy-prod` | `Deploy to Production` | Clear target |

---

## Common Mistakes

### Mistake 1: Steps Outside of Stages

```groovy
// ❌ WRONG
pipeline {
    agent any
    steps {  // Steps at pipeline level!
        echo 'This is wrong'
    }
}

// ✅ CORRECT
pipeline {
    agent any
    stages {
        stage('Build') {
            steps {  // Steps inside stage
                echo 'This is correct'
            }
        }
    }
}
```

### Mistake 2: Parallel Without failFast Consideration

```groovy
// ⚠️ Consider whether you want failFast
stage('Test') {
    failFast: true  // Consider: do you want to cancel other tests on failure?
    parallel {
        stage('Unit') { ... }
        stage('Integration') { ... }
    }
}
```

### Mistake 3: Missing Stage Names

```groovy
// ❌ WRONG - All stages look the same in logs
stages {
    stage {  // No name!
        steps { ... }
    }
}

// ✅ CORRECT - Clear names
stages {
    stage('Build') {
        steps { ... }
    }
}
```

---

## Next Steps

- **[Pipeline Agents](04-pipeline-agents.md)** - Configure where your pipeline runs
- **[Environment Variables](02-environment-and-credentials/01-environment-variables.md)** - Use environment variables in stages
- **[Parallel and Matrix Builds](04-advanced-topics/02-parallel-and-matrix/01-parallel-stages.md)** - Advanced parallel execution
