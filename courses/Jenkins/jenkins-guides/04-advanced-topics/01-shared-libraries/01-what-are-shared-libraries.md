# What Are Shared Libraries?

## What this covers

This guide explains what Jenkins Shared Libraries are, why they're useful for reusable pipeline code, and the folder structure required. You'll understand how to store common pipeline code in a separate Git repository and load it into any pipeline.

## Prerequisites

- Understanding of Declarative and Scripted Pipeline syntax
- Experience with Jenkins pipelines
- Git repository for storing shared code

## What is a Shared Library?

A **Shared Library** is a collection of reusable Groovy code that can be loaded into any Jenkins pipeline. Instead of copying the same code across multiple Jenkinsfiles, you define it once in a shared library.

> Think of shared libraries as **functions** you can call from any pipeline—like a utility library.

---

## Why Use Shared Libraries?

### Without Shared Libraries

```groovy
// In my-app/Jenkinsfile
pipeline {
    agent any
    stages {
        stage('Build') {
            steps {
                sh 'echo "Building..."'
                sh 'mvn clean package'
                sh 'echo "Build complete"'
            }
        }
    }
}

// In other-app/Jenkinsfile - DUPLICATE!
pipeline {
    agent any
    stages {
        stage('Build') {
            steps {
                sh 'echo "Building..."'
                sh 'mvn clean package'
                sh 'echo "Build complete"'
            }
        }
    }
}
```

### With Shared Libraries

```groovy
// In my-app/Jenkinsfile - Clean!
@Library('my-shared-library') _

pipeline {
    agent any
    stages {
        stage('Build') {
            steps {
                buildApp()  // Reusable function!
            }
        }
    }
}

// In other-app/Jenkinsfile - Same function!
@Library('my-shared-library') _

pipeline {
    agent any
    stages {
        stage('Build') {
            steps {
                buildApp()  // Same function!
            }
        }
    }
}
```

---

## Library Folder Structure

A shared library has this structure:

```
my-shared-library/
├── src/                      # Groovy source files (optional)
│   └── org/
│       └── example/
│           └── Utils.groovy   # Class definitions
├── vars/                     # Global variables (functions)
│   ├── buildApp.groovy       # Call via buildApp()
│   ├── buildApp.txt          # Help documentation (optional)
│   ├── runTests.groovy       # Call via runTests()
│   └── deploy.groovy         # Call via deploy()
├── resources/                # External resources (optional)
│   └── org/example/
│       └── template.json     # Read via libraryResource()
└── README.md                # Library documentation
```

---

## Types of Library Code

### 1. Global Variables (vars/)

These become global functions in your pipeline:

**vars/buildApp.groovy:**
```groovy
// This becomes buildApp() in pipelines
def call(Map config = [:]) {
    echo "Building ${config.appName ?: 'app'}"
    sh "mvn clean package -Dversion=${config.version ?: 'SNAPSHOT'}"
}
```

**Usage in pipeline:**
```groovy
@Library('my-library') _

pipeline {
    stages {
        stage('Build') {
            steps {
                buildApp(appName: 'my-app', version: '1.0.0')
            }
        }
    }
}
```

### 2. Source Files (src/)

Groovy classes in packages:

**src/org/example/Utils.groovy:**
```groovy
package org.example

class Utils {
    static def buildApp(String name) {
        echo "Building ${name}"
        sh "mvn clean package"
    }
}
```

**Usage in pipeline:**
```groovy
@Library('my-library') _

import org.example.Utils

pipeline {
    stages {
        stage('Build') {
            steps {
                script {
                    Utils.buildApp('my-app')
                }
            }
        }
    }
}
```

### 3. Resources (resources/)

Files loaded at runtime:

**resources/org/example/deploy-template.json:**
```json
{
    "app": "${APP_NAME}",
    "version": "${VERSION}"
}
```

**Usage in pipeline:**
```groovy
@Library('my-library') _

pipeline {
    stages {
        stage('Deploy') {
            steps {
                script {
                    def template = libraryResource 'org/example/deploy-template.json'
                    // Use template with variable substitution
                }
            }
        }
    }
}
```

---

## Configuring Shared Library in Jenkins

### Step 1: Configure Library

1. Go to **Manage Jenkins** → **Configure System**
2. Find **Global Pipeline Libraries**
3. Click **Add**

```
┌─────────────────────────────────────────────────────────────────────┐
│  Global Pipeline Libraries                                          │
│                                                                     │
│  Name:  [ my-shared-library ]                                      │
│       ↓                                                             │
│  Default version:  [ main ]                                        │
│       ↓                                                             │
│  Retrieval method:  ☑ Modern SCM                                   │
│       ↓                                                             │
│  Source Code Management:  Git                                      │
│       ↓                                                             │
│  Project repository:  [ https://github.com/user/shared-lib.git ]   │
│       ↓                                                             │
│  Credentials:  [ github-credentials ▼ ]                            │
│       ↓                                                             │
│  Library path:  [ ] (leave empty for root)                         │
└─────────────────────────────────────────────────────────────────────┘
```

### Step 2: Add Credentials

If repository is private:
1. Add Git credentials in Jenkins
2. Select them in the library configuration

---

## Using Shared Library in Pipeline

### Basic Usage

```groovy
// Load library globally configured
@Library('my-shared-library') _

pipeline {
    agent any
    stages {
        stage('Build') {
            steps {
                buildApp()
            }
        }
    }
}
```

### With Version/Tag

```groovy
// Use specific version
@Library('my-shared-library@v1.0.0') _

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

### Underscore Import

The `_` (underscore) is required:

```groovy
// ✅ Correct - underscore loads library
@Library('my-library') _

// ❌ Wrong - without underscore
@Library('my-library')
```

---

## Benefits of Shared Libraries

| Benefit | Description |
|---------|-------------|
| **DRY** | Don't Repeat Yourself |
| **Single Source** | Update in one place |
| **Versioning** | Pin to specific versions |
| **Testing** | Test common code once |
| **Collaboration** | Share across teams |
| **Standardization** | Enforce best practices |

---

## Common Patterns

### Standard Build Steps

```groovy
// In vars/standardBuild.groovy
def call(String language = 'java') {
    stage('Checkout') {
        checkout scm
    }
    
    stage('Build') {
        if (language == 'java') {
            sh 'mvn clean package'
        } else if (language == 'node') {
            sh 'npm ci && npm run build'
        }
    }
    
    stage('Test') {
        sh 'mvn test'  // or npm test
    }
}
```

### Deployment Functions

```groovy
// In vars/deploy.groovy
def call(String environment, Map config = [:]) {
    echo "Deploying to ${environment}"
    
    def configFile = libraryResource "org/example/${environment}-config.json"
    
    sh """
        kubectl apply -f ${configFile}
        kubectl set image deployment/${config.app} ${config.app}=${config.image}
    """
}
```

---

## Best Practices

1. **Version Control** - Use tags for stable versions
2. **Documentation** - Add `.txt` help files
3. **Testing** - Test library code in isolation
4. **Naming** - Clear function names
5. **Error Handling** - Proper exception handling

---

## Next Steps

- **[Creating a Shared Library](02-creating-a-shared-library.md)** - Build your first library
- **[Using Shared Libraries in Pipelines](03-using-shared-libraries-in-pipelines.md)** - Advanced usage
- **[Parallel Stages](02-parallel-and-matrix/01-parallel-stages.md)** - Speed up builds
