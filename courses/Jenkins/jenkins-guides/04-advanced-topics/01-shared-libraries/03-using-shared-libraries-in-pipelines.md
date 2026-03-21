# Using Shared Libraries in Pipelines

## What this covers

This guide explains how to use Shared Libraries in Jenkins pipelines with various loading methods, version specifications, and advanced patterns. You'll learn about the underscore import syntax, library versioning, and dynamic loading.

## Prerequisites

- Completed creating a shared library
- Library configured in Jenkins
- Understanding of pipeline basics

---

## Loading Methods

### Method 1: Global Library (Automatic)

Configure library in Jenkins globally - automatically available to all pipelines:

```groovy
// No @Library annotation needed - automatically loaded!
pipeline {
    agent any
    stages {
        stage('Build') {
            steps {
                buildApp()  // Available automatically
            }
        }
    }
}
```

### Method 2: Explicit Import with @Library

```groovy
// Load specific library
@Library('my-shared-library') _

pipeline {
    stages {
        stage('Build') {
            steps {
                buildApp()
            }
        }
    }
}
```

### Method 3: Multiple Libraries

```groovy
// Load multiple libraries
@Library('my-library') _
@Library('company-library@main') _

pipeline {
    stages {
        stage('Build') {
            steps {
                buildApp()  // from my-library
                companyDeploy()  // from company-library
            }
        }
    }
}
```

---

## Version Specification

### Use Branch

```groovy
// Use 'main' branch
@Library('my-library@main') _

// Use 'develop' branch
@Library('my-library@develop') _
```

### Use Tag

```groovy
// Use specific release tag
@Library('my-library@v1.0.0') _

// Use semantic versioning
@Library('my-library@v1.2.3') _
```

### Use Commit SHA

```groovy
// Use specific commit
@Library('my-library@abc123def456') _
```

### Use Default

```groovy
// Uses version configured in Jenkins global settings
@Library('my-library') _
```

---

## Implicit vs Explicit Loading

### Implicit (Configured Globally)

```groovy
// In Jenkins Configure System:
// Add library: my-library (default version: main)
//
// Pipeline - no annotation needed:
pipeline {
    stages {
        stage('Build') {
            steps {
                buildApp()  // Available from global config
            }
        }
    }
}
```

### Explicit (Per Pipeline)

```groovy
// In Jenkins Configure System:
// DON'T add the library
//
// Pipeline - must specify:
@Library('my-library') _

pipeline {
    stages {
        stage('Build') {
            steps {
                buildApp()  // Explicitly loaded
            }
        }
    }
}
```

---

## Advanced Library Loading

### Dynamic Library Loading

```groovy
// Load library dynamically at runtime
library 'my-shared-library@main'

pipeline {
    stages {
        stage('Build') {
            steps {
                buildApp()
            }
        }
    }
}
```

### With Library Resource

```groovy
// Load and use library resource file
@Library('my-library') _

pipeline {
    stages {
        stage('Deploy') {
            steps {
                script {
                    def template = libraryResource 'org/mycompany/deploy.yaml'
                    writeFile file: 'deploy.yaml', text: template
                }
            }
        }
    }
}
```

---

## Using Library Classes (src/)

### Importing Classes

```groovy
@Library('my-shared-library') _

import org.mycompany.Deploy

pipeline {
    agent any
    
    stages {
        stage('Deploy') {
            steps {
                script {
                    def deploy = new Deploy(this)
                    deploy.toKubernetes(
                        appName: 'my-app',
                        image: 'myregistry/my-app:latest',
                        namespace: 'production'
                    )
                }
            }
        }
    }
}
```

---

## Complete Examples

### Example 1: Standard Build Pipeline

```groovy
@Library('jenkins-library@main') _

pipeline {
    agent any
    
    parameters {
        choice(name: 'ENV', choices: ['dev', 'staging', 'prod'], description: 'Environment')
        string(name: 'VERSION', defaultValue: '1.0.0', description: 'Version')
    }
    
    stages {
        stage('Build Application') {
            steps {
                buildApp(
                    appName: params.APP_NAME,
                    version: params.VERSION,
                    javaVersion: '17'
                )
            }
        }
        
        stage('Run Tests') {
            steps {
                runTests(type: 'junit', coverage: true)
            }
        }
        
        stage('Deploy') {
            when {
                expression { return params.ENV != 'dev' }
            }
            steps {
                notify(channel: '#deployments', message: "Deploying ${params.VERSION} to ${params.ENV}")
                // deploy() function from library
            }
        }
    }
    
    post {
        always {
            notify(
                channel: '#builds',
                status: currentBuild.result ?: 'SUCCESS'
            )
        }
    }
}
```

### Example 2: Multi-Stage Pipeline with Library

```groovy
@Library('my-library@v2.0.0') _

pipeline {
    agent any
    
    environment {
        DOCKER_REGISTRY = 'docker.io'
    }
    
    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        
        stage('Build') {
            parallel {
                stage('Frontend') {
                    steps {
                        runTests(type: 'jest')
                        buildDockerImage('frontend')
                    }
                }
                stage('Backend') {
                    steps {
                        runTests(type: 'junit')
                        buildDockerImage('backend')
                    }
                }
            }
        }
        
        stage('Deploy') {
            when {
                branch 'main'
            }
            steps {
                deploy(
                    environment: 'production',
                    components: ['frontend', 'backend']
                )
            }
        }
    }
}
```

---

## Library Loading from Different Sources

### GitHub

```groovy
@Library('github.com/owner/repo@main') _

// or
@Library('gh:owner/repo@v1.0.0') _
```

### GitLab

```groovy
@Library('git@gitlab.com:owner/repo.git@main') _
```

### Bitbucket

```groovy
@Library('bitbucket.org/owner/repo@main') _
```

---

## Best Practices

### 1. Use Version Tags

```groovy
// ❌ May break - using moving target
@Library('my-library') _

// ✅ Stable - pinned to version
@Library('my-library@v1.2.3') _
```

### 2. Document Library Functions

```groovy
// vars/deploy.groovy

/**
 * Deploy application to Kubernetes
 * @param config.appName - Application name (required)
 * @param config.namespace - Kubernetes namespace (default: default)
 * @param config.image - Container image (required)
 */
def call(Map config) {
    // implementation
}
```

### 3. Handle Missing Functions Gracefully

```groovy
pipeline {
    stages {
        stage('Build') {
            steps {
                script {
                    // Check if function exists
                    if (this.metaClass.respondsTo(this, 'buildApp')) {
                        buildApp()
                    } else {
                        sh 'mvn clean package'
                    }
                }
            }
        }
    }
}
```

---

## Common Mistakes

### Missing Underscore

```groovy
// ❌ WRONG - missing underscore
@Library('my-library')

pipeline { }

// ✅ CORRECT - underscore required
@Library('my-library') _

pipeline { }
```

### Wrong Version Format

```groovy
// ❌ WRONG
@Library('my-library@version=1.0') _

// ✅ CORRECT - just the version/tag/branch
@Library('my-library@v1.0') _
```

### Function Not Found

```groovy
// ❌ Function not defined in library
buildApp()  // Must exist in vars/buildApp.groovy

// ✅ Make sure it's defined
// vars/buildApp.groovy exists with: def call() { }
```

---

## Next Steps

- **[Parallel Stages](02-parallel-and-matrix/01-parallel-stages.md)** - Run parallel builds
- **[Matrix Builds](02-parallel-and-matrix/02-matrix-builds.md)** - Test across configurations
- **[Kubernetes Agents](03-kubernetes-agents/01-kubernetes-plugin-setup.md)** - Scale with K8s
