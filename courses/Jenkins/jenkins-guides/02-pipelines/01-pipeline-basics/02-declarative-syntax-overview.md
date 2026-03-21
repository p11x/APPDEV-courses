# Declarative Pipeline Syntax Overview

## What this covers

This guide provides a complete breakdown of Declarative Pipeline syntax. You'll learn about all the top-level blocks (`pipeline`, `agent`, `environment`, `options`, `parameters`, `triggers`, `stages`, `post`) with annotated examples showing exactly what each line does.

## Prerequisites

- Understanding of what a Jenkinsfile is
- Completed the "What is a Jenkinsfile?" guide

## Complete Declarative Pipeline Example

Here's a fully annotated Jenkinsfile with every block explained:

```groovy
// Jenkinsfile - Complete Declarative Pipeline Example

// pipeline {} - Top-level block — required for all Declarative Pipelines
pipeline {
    // agent {} - Where to run the pipeline
    // 'any' means run on any available Jenkins agent/executor
    agent any
    
    // environment {} - Declare environment variables available to all stages
    environment {
        // Custom variable — accessible as env.APP_NAME in steps
        APP_NAME = 'my-app'
        
        // Another custom variable
        DEPLOY_ENV = 'staging'
    }
    
    // options {} - Pipeline-level configuration options
    options {
        // timeout {} - Abort the entire pipeline if it runs longer than 30 minutes
        // time: 30 specifies the amount
        // unit: 'MINUTES' specifies the time unit
        timeout(time: 30, unit: 'MINUTES')
        
        // disableConcurrentBuilds {} - Prevent two builds of this job from running simultaneously
        // Useful to prevent race conditions and resource conflicts
        disableConcurrentBuilds()
        
        // buildDiscarder {} - Keep only the last 10 build logs to save disk space
        // logRotator is the classic Jenkins artifact rotation strategy
        // numToKeepStr: '10' means keep logs for last 10 builds
        buildDiscarder(logRotator(numToKeepStr: '10'))
    }
    
    // parameters {} - Define input parameters for the pipeline
    parameters {
        // string {} - A text input parameter
        // name: 'VERSION' - Variable name used to access this
        // defaultValue: '1.0.0' - Default value if user doesn't specify
        string(name: 'VERSION', defaultValue: '1.0.0', description: 'App version to deploy')
        
        // choice {} - A dropdown selection parameter
        // choices: ['staging', 'production'] - Available options
        choice(name: 'ENVIRONMENT', choices: ['staging', 'production'], description: 'Target environment')
        
        // booleanParam {} - A checkbox parameter
        booleanParam(name: 'RUN_TESTS', defaultValue: true, description: 'Run test suite')
    }
    
    // triggers {} - When to automatically run the pipeline
    triggers {
        // cron {} - Run on a schedule using cron syntax
        // H 2 * * * = Random minute at 2 AM daily
        cron('H 2 * * *')
        
        // pollSCM {} - Check for code changes periodically
        // H/5 * * * * = Every 5 minutes (with hash to distribute load)
        pollSCM('H/5 * * * *')
    }
    
    // stages {} - The main section containing all stage blocks
    stages {
        // stage {} - A named section of work
        // Appears as a block in Blue Ocean UI and build logs
        stage('Build') {
            // steps {} - The actual commands to run inside this stage
            steps {
                // echo {} - Print a message to the build log
                // ${env.APP_NAME} reads the environment variable declared above
                echo "Building ${env.APP_NAME} version ${params.VERSION}"
                
                // sh {} - Execute shell commands (Linux/macOS)
                // This runs npm install to get dependencies
                sh 'npm install'
                
                // bat {} - Execute batch commands (Windows)
                // bat 'npm install'  // Uncomment for Windows
            }
        }
        
        stage('Test') {
            // when {} - Conditional execution
            // Only run this stage if RUN_TESTS parameter is true
            when {
                // expression {} - Evaluate a condition
                expression { return params.RUN_TESTS }
            }
            
            steps {
                echo 'Running tests...'
                // Run the test suite
                sh 'npm test'
                
                // Publish test results
                // junit {} - Publish JUnit test results
                // '**/test-results/*.xml' is a glob pattern
                // ** means any directory, * means any filename
                junit '**/test-results/*.xml'
            }
        }
        
        stage('Deploy') {
            // Run only on staging or production branches
            when {
                // branch {} - Run only for specific branches
                // Supports wildcards: main, release/*, feature/*
                branch 'main'
            }
            
            steps {
                echo "Deploying to ${params.ENVIRONMENT}"
                // Deploy commands would go here
            }
        }
    }
    
    // post {} - Runs after all stages complete, regardless of result
    post {
        // always {} - Runs in every situation (success, failure, aborted)
        always {
            echo 'Pipeline finished - cleanup always runs'
            
            // cleanWs {} - Clean up workspace after build
            // deleteDirs: true - Delete subdirectories
            // notFailBuild: true - Don't fail build if cleanup fails
            cleanWs deleteDirs: true, notFailBuild: true
        }
        
        // success {} - Runs only when pipeline succeeds
        success {
            echo 'Pipeline succeeded!'
            
            // slackSend {} - Send Slack notification (requires Slack plugin)
            // channel: '#builds' - Slack channel to post to
            // color: 'good' - Message color (green)
            // message: The notification text
            slackSend channel: '#builds', color: 'good', message: "Build ${env.BUILD_NUMBER} succeeded!"
        }
        
        // failure {} - Runs only when pipeline fails
        failure {
            echo 'Something went wrong!'
            slackSend channel: '#builds', color: 'danger', message: "Build ${env.BUILD_NUMBER} FAILED!"
        }
        
        // unstable {} - Runs when pipeline is marked unstable
        unstable {
            echo 'Pipeline is unstable'
        }
        
        // changed {} - Runs when build status differs from previous build
        changed {
            echo 'Build status changed!'
        }
    }
}
```

---

## Top-Level Blocks Reference

### 1. pipeline (Required)

The root block that contains all other Declarative Pipeline components.

```groovy
pipeline {
    // All other blocks go here
}
```

### 2. agent

Specifies where the pipeline or stage runs.

```groovy
// Run anywhere
agent any

// Run on specific label
agent { label 'linux' }

// Run in Docker container
agent { docker 'node:20' }

// Don't allocate agent (use for stages that don't need one)
agent none
```

### 3. environment

Defines environment variables accessible via `env.VARIABLE_NAME` or `$VARIABLE_NAME`.

```groovy
environment {
    APP_NAME = 'my-app'
    DB_HOST = 'localhost'
}
```

### 4. options

Configures pipeline-level settings.

```groovy
options {
    timeout(time: 30, unit: 'MINUTES')     // Timeout the entire pipeline
    disableConcurrentBuilds()              // Don't allow parallel builds
    buildDiscarder(logRotator(numToKeepStr: '10'))  // Keep 10 builds
    timestamps()                             // Add timestamps to log output
    skipDefaultCheckout()                   // Don't auto-checkout scm
}
```

### 5. parameters

Defines build parameters that users can input when triggering the build.

```groovy
parameters {
    string(name: 'VERSION', defaultValue: '1.0.0')
    choice(name: 'ENV', choices: ['dev', 'staging', 'prod'])
    booleanParam(name: 'DEPLOY', defaultValue: false)
    password(name: 'API_TOKEN', defaultValue: '')
}
```

### 6. triggers

Defines automatic build triggers.

```groovy
triggers {
    cron('H 2 * * *')              // Daily at 2 AM
    pollSCM('H/5 * * * *')         // Poll every 5 minutes
    upstream('other-job')          // After other job completes
}
```

### 7. stages (Required)

Container for all stage blocks. At least one stage is required.

```groovy
stages {
    stage('Build') { ... }
    stage('Test') { ... }
    stage('Deploy') { ... }
}
```

### 8. post

Defines actions to run after pipeline completes.

```groovy
post {
    always { ... }     // Always runs
    success { ... }    // Runs on success
    failure { ... }    // Runs on failure
    unstable { ... }   // Runs when unstable
    changed { ... }    // Runs when status changed
    cleanup { ... }    // Runs last, after everything
}
```

---

## Key Differences: Steps vs Blocks

| Type | Definition | Example |
|------|------------|---------|
| **steps** | Single actions | `echo`, `sh`, `bat`, `git` |
| **blocks** | Containers | `stages`, `stage`, `environment`, `options` |

---

## Common Mistakes

### Mistake 1: Missing Required Blocks

```groovy
// ❌ WRONG - Missing required 'stages' block
pipeline {
    agent any
    // No stages!
}

// ✅ CORRECT
pipeline {
    agent any
    stages {
        stage('Build') {
            steps {
                echo 'Hello'
            }
        }
    }
}
```

### Mistake 2: Wrong Block Placement

```groovy
// ❌ WRONG - steps outside of stages
pipeline {
    agent any
    steps {
        echo 'This is wrong!'
    }
    stages {
        stage('Build') { }
    }
}

// ✅ CORRECT - steps inside stages
pipeline {
    agent any
    stages {
        stage('Build') {
            steps {
                echo 'This is correct!'
            }
        }
    }
}
```

### Mistake 3: Forgetting agent

```groovy
// ⚠️ WARNING - No agent specified (may fail in some Jenkins configurations)
pipeline {
    stages {
        stage('Build') {
            steps {
                sh 'echo hello'
            }
        }
    }
}

// ✅ CORRECT - specify agent
pipeline {
    agent any
    stages {
        stage('Build') {
            steps {
                sh 'echo hello'
            }
        }
    }
}
```

---

## Next Steps

- **[Stages and Steps](03-stages-and-steps.md)** - Deep dive into stages, steps, parallel, and sequential
- **[Pipeline Agents](04-pipeline-agents.md)** - Advanced agent configuration
- **[Environment Variables](02-environment-and-credentials/01-environment-variables.md)** - Using environment variables
