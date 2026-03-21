# Environment Variables in Jenkins Pipeline

## What this covers

This guide explains all ways to use environment variables in Jenkins Pipelines. You'll learn about the `environment` block, `withEnv` step, built-in Jenkins variables, and how to reference them in your pipelines.

## Prerequisites

- Understanding of Declarative Pipeline syntax
- Completed Pipeline basics guides

## What Are Environment Variables?

Environment variables are key-value pairs that provide information to processes running in your pipeline. They're like notes that every step can read.

> Think of environment variables as **sticky notes** on a shared board—anyone can read them, and they're available to all commands.

---

## Built-in Jenkins Variables

Jenkins automatically provides many built-in environment variables:

### Common Built-in Variables

| Variable | Description | Example |
|----------|-------------|---------|
| `BUILD_NUMBER` | Sequential build number | `42` |
| `BUILD_URL` | Full URL to build | `http://jenkins:8080/job/myjob/42/` |
| `WORKSPACE` | Path to build workspace | `/var/lib/jenkins/workspace/myjob` |
| `JOB_NAME` | Name of the job | `my-pipeline` |
| `NODE_NAME` | Name of the agent | `master` or `linux-agent-1` |
| `EXECUTOR_NUMBER` | Number of executor | `0` |
| `JAVA_HOME` | Java installation path | `/usr/lib/jvm/java-17` |
| `LANG` | System language | `en_US.UTF-8` |
| `PATH` | System PATH | `/usr/local/bin:/usr/bin` |

### Git-Related Variables

| Variable | Description | Example |
|----------|-------------|---------|
| `GIT_BRANCH` | Branch being built | `main` |
| `GIT_COMMIT` | Full Git commit SHA | `abc123def456...` |
| `GIT_URL` | Repository URL | `https://github.com/user/repo.git` |
| `GIT_AUTHOR_NAME` | Commit author name | `John Doe` |
| `GIT_COMMITTER_NAME` | Committer name | `John Doe` |
| `GIT_MESSAGE` | Commit message | "Fix bug in login" |

### Using Built-in Variables

```groovy
pipeline {
    agent any
    
    stages {
        stage('Info') {
            steps {
                // Print to console output
                echo "Building: ${env.JOB_NAME}"
                echo "Build #: ${env.BUILD_NUMBER}"
                echo "Branch: ${env.GIT_BRANCH}"
                echo "Commit: ${env.GIT_COMMIT}"
                echo "Workspace: ${env.WORKSPACE}"
                echo "Build URL: ${env.BUILD_URL}"
            }
        }
    }
}
```

---

## Defining Custom Environment Variables

### Method 1: environment Block (Pipeline-Level)

Variables defined in `environment` are available to all stages:

```groovy
pipeline {
    agent any
    
    // Define environment variables at pipeline level
    environment {
        // Custom application variable
        APP_NAME = 'my-spring-app'
        
        // Deployment environment
        DEPLOY_ENV = 'staging'
        
        // Database URL (note: not a secret - just configuration)
        DB_URL = 'jdbc:postgresql://db.example.com:5432/mydb'
        
        // Can reference other variables
        APP_VERSION = '1.0.0'
        FULL_APP_NAME = "${APP_NAME}-${APP_VERSION}"
    }
    
    stages {
        stage('Build') {
            steps {
                echo "Building ${env.APP_NAME}"
                echo "Version: ${env.APP_VERSION}"
                echo "Full name: ${env.FULL_APP_NAME}"
                echo "Deploying to: ${env.DEPLOY_ENV}"
            }
        }
    }
}
```

### Method 2: environment Block (Stage-Level)

Variables available only within that stage:

```groovy
pipeline {
    agent any
    
    stages {
        stage('Build') {
            environment {
                // Only available in Build stage
                BUILD_MODE = 'release'
            }
            steps {
                echo "Build mode: ${env.BUILD_MODE}"
            }
        }
        
        stage('Test') {
            // env.BUILD_MODE would be empty here!
            steps {
                echo "Running tests..."
            }
        }
    }
}
```

### Method 3: withEnv Step

Temporarily set environment variables for specific commands:

```groovy
pipeline {
    agent any
    
    stages {
        stage('Build') {
            steps {
                // withEnv temporarily sets variables
                withEnv(['APP_VERSION=2.0.0', 'BUILD_ID=123']) {
                    sh 'echo "Version: $APP_VERSION"'
                    sh 'echo "Build ID: $BUILD_ID"'
                }
                
                // Variables outside withEnv are not affected
                sh 'echo "Outside withEnv"'
            }
        }
    }
}
```

---

## Environment Variables with Credentials

### Method 1: credentials() Helper

The `credentials()` helper automatically injects credentials as environment variables:

```groovy
pipeline {
    agent any
    
    environment {
        // UsernamePassword credential provides two variables:
        // - MY_CREDS: username:password
        // - MY_CREDS_USR: username
        // - MY_CREDS_PSW: password
        MY_CREDS = credentials('my-username-password')
        
        // Secret text provides:
        // - MY_TOKEN: the secret value
        MY_TOKEN = credentials('my-secret-token')
        
        // SSH credentials provide:
        // - MY_SSH: SSH private key content
        MY_SSH = credentials('my-ssh-key')
    }
    
    stages {
        stage('Use Credentials') {
            steps {
                // Print username (safe)
                echo "Username: ${env.MY_CREDS_USR}"
                
                // Password is available but masked in logs
                // Never echo passwords!
                sh 'some-tool --token=$MY_TOKEN'
            }
        }
    }
}
```

### Method 2: withCredentials Step

More explicit credential binding:

```groovy
pipeline {
    agent any
    
    stages {
        stage('Deploy') {
            steps {
                // Bind credentials to specific variables
                withCredentials([
                    // UsernamePassword binding
                    usernamePassword(
                        credentialsId: 'github-credentials',
                        usernameVariable: 'GITHUB_USER',
                        passwordVariable: 'GITHUB_TOKEN'
                    ),
                    // Secret text binding
                    string(
                        credentialsId: 'aws-secret-key',
                        variable: 'AWS_SECRET_KEY'
                    ),
                    // SSH key binding
                    sshUserPrivateKey(
                        credentialsId: 'ssh-key',
                        usernameVariable: 'SSH_USER',
                        keyFileVariable: 'SSH_KEY_FILE'
                    )
                ]) {
                    // Variables available inside this block
                    sh 'git push https://$GITHUB_USER:$GITHUB_TOKEN@github.com/...'
                    sh 'aws configure set secret_key $AWS_SECRET_KEY'
                }
            }
        }
    }
}
```

---

## Complete Environment Variables Reference

### All Built-in Jenkins Variables

| Variable | Description |
|----------|-------------|
| `BUILD_NUMBER` | Current build number |
| `BUILD_URL` | URL to build details |
| `WORKSPACE` | Workspace directory path |
| `JOB_NAME` | Job name |
| `JENKINS_URL` | Jenkins master URL |
| `BUILD_TAG` | jenkins-${JOB_NAME}-${BUILD_NUMBER} |
| `EXECUTOR_NUMBER` | Number of the executor |
| `NODE_NAME` | Name of the agent |
| `NODE_LABELS` | Labels assigned to the agent |
| `JAVA_HOME` | Java home directory |
| `JENKINS_HOME` | Jenkins home directory |
| `JENKINS_VERSION` | Version of Jenkins |
| `GIT_BRANCH` | Branch being built |
| `GIT_COMMIT` | Git commit SHA |
| `GIT_URL` | Git repository URL |
| `GIT_AUTHOR_NAME` | Commit author |
| `GIT_COMMITTER_NAME` | Commit committer |
| `GIT_MESSAGE` | Commit message |
| `CHANGES` | Changes since last build |
| `CHANGE_AUTHOR` | Author of change |
| `CHANGE_ID` | ID of change |
| `CHANGE_URL` | URL of change |

---

## Dynamic Environment Variables

Use Groovy to set dynamic values:

```groovy
pipeline {
    agent any
    
    environment {
        // Use Groovy to compute value
        CURRENT_TIMESTAMP = "${new Date().format('yyyy-MM-dd HH:mm:ss')}"
        BUILD_USER = "${env.CHANGE_AUTHOR ?: 'manual'}"
        SHORT_COMMIT = "${env.GIT_COMMIT?.take(8) ?: 'none'}"
    }
    
    stages {
        stage('Show Dynamic Vars') {
            steps {
                echo "Timestamp: ${env.CURRENT_TIMESTAMP}"
                echo "User: ${env.BUILD_USER}"
                echo "Commit: ${env.SHORT_COMMIT}"
            }
        }
    }
}
```

---

## Common Mistakes

### Mistake 1: Echoing Secrets

```groovy
// ❌ WRONG - Never print secrets!
environment {
    MY_TOKEN = credentials('my-token')
}

steps {
    echo "Token: ${env.MY_TOKEN}"  // DANGER! Will be masked but not safe
}

// ✅ CORRECT - Use in commands but don't echo
steps {
    sh 'my-tool --token=$MY_TOKEN'
}
```

### Mistake 2: Wrong Variable Syntax

```groovy
// ❌ WRONG - Wrong quotes
environment {
    MY_VAR = "value"  // Double quotes evaluate immediately
}

// ✅ CORRECT - Use single quotes or no quotes
environment {
    MY_VAR = 'value'  // Single quotes - evaluated later
    MY_VAR2 = value   // No quotes - evaluated later
}

// ✅ CORRECT - For dynamic values
environment {
    MY_VAR = "${new Date().format('yyyy-MM-dd')}"
}
```

### Mistake 3: Forgetting env. Prefix

```groovy
// ⚠️ WORKS but not recommended
steps {
    echo "Build: $BUILD_NUMBER"  // Works but implicit
}

// ✅ RECOMMENDED - Explicit prefix
steps {
    echo "Build: ${env.BUILD_NUMBER}"
}
```

---

## Next Steps

- **[Credentials Plugin](02-credentials-plugin.md)** - Store and use secrets securely
- **[Secret Masking](03-secret-masking.md)** - How Jenkins protects your secrets
- **[Post Block Conditions](../03-post-and-notifications/01-post-block-conditions.md)** - Run actions based on results
