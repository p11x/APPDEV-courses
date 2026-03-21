# SonarQube Integration in Jenkins

## What this covers

This guide explains how to integrate SonarQube for code quality analysis in Jenkins pipelines. You'll learn about installing SonarQube Scanner, configuring the SonarQube server, using `withSonarQubeEnv()` and `waitForQualityGate()` steps.

## Prerequisites

- SonarQube server running (self-hosted or SonarCloud)
- SonarQube Scanner for Jenkins plugin installed
- Understanding of Pipeline syntax
- Code in a supported language (Java, JavaScript, Python, etc.)

---

## Installing SonarQube Scanner Plugin

1. Go to **Manage Jenkins** → **Plugin Manager**
2. Search for "SonarQube"
3. Install:
   - **SonarQube Scanner for Jenkins**
   - **SonarQube Scanner** (optional but recommended)

---

## Configuring SonarQube Server

### Step 1: Add SonarQube Server

1. Go to **Manage Jenkins** → **Configure System**
2. Find **SonarQube servers**
3. Click **Add SonarQube**

```
┌─────────────────────────────────────────────────────────────────────┐
│  SonarQube servers                                                 │
│                                                                     │
│  [+ Add SonarQube]                                                 │
│                                                                     │
│  Name:  [ SonarQube ]                                            │
│       ↓                                                             │
│  Server URL:  [ http://sonarqube.example.com ]                    │
│       ↓                                                             │
│  Server authentication token:  [ sonarqube-token ▼ ]               │
│       ↓                                                             │
└─────────────────────────────────────────────────────────────────────┘
```

### Step 2: Configure SonarQube Scanner

Find **SonarQube Scanner** in **Global Tool Configuration**:

```
┌─────────────────────────────────────────────────────────────────────┐
│  SonarQube Scanner installations                                   │
│                                                                     │
│  [+ Add SonarQube Scanner]                                          │
│                                                                     │
│  Name:  [ Default ]                                                │
│       ↓                                                             │
│  Apply installation from build:  [ ] ✓                           │
│                                                                     │
│  Scanner version / version from CI:                                │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ (●) Use recent version                                        │ │
│  │ ○ Specify version                                             │ │
│  │ ○ Install automatically                                      │ │
│  └───────────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Creating SonarQube Token

### In SonarQube UI

1. Log in to SonarQube
2. Go to **My Account** → **Security**
3. Generate new token:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Generate Token                                                     │
│                                                                     │
│  Name:  [ jenkins-token ]                                          │
│                                                                     │
│  [ Generate ]                                                      │
│                                                                     │
│  Token:  abc123def456...                                           │
│       ↓ Copy this token                                            │
└─────────────────────────────────────────────────────────────────────┘
```

### Add to Jenkins Credentials

1. Go to **Manage Jenkins** → **Credentials**
2. Add **Secret text** credential:
   - Secret: Your SonarQube token
   - ID: `sonarqube-token`

---

## Using SonarQube in Pipeline

### Basic Pipeline

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
        
        stage('SonarQube Analysis') {
            steps {
                // withSonarQubeEnv prepares the environment
                // Must match server name in Jenkins config
                withSonarQubeEnv('SonarQube') {
                    sh '''
                        # Run SonarQube Scanner
                        # Use scanner jar from configured installation
                        sonar-scanner \
                          -Dsonar.projectKey=my-project \
                          -Dsonar.sources=src \
                          -Dsonar.java.binaries=target/classes
                    '''
                }
            }
        }
        
        stage('Quality Gate') {
            steps {
                // Wait for SonarQube quality gate
                // This blocks until analysis is complete
                waitForQualityGate()
            }
        }
    }
}
```

---

## Complete Pipeline with SonarQube

```groovy
pipeline {
    agent any
    
    environment {
        SONAR_PROJECT_KEY = 'my-app'
    }
    
    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        
        stage('Build') {
            steps {
                sh 'mvn clean compile'
            }
        }
        
        stage('SonarQube Analysis') {
            steps {
                withSonarQubeEnv('SonarQube') {
                    sh """
                        sonar-scanner \
                          -Dsonar.projectKey=${SONAR_PROJECT_KEY} \
                          -Dsonar.projectName=My-App \
                          -Dsonar.projectVersion=${env.BUILD_NUMBER} \
                          -Dsonar.sources=src \
                          -Dsonar.language=java \
                          -Dsonar.java.binaries=target/classes \
                          -Dsonar.sourceEncoding=UTF-8
                    """
                }
            }
        }
        
        stage('Quality Gate Check') {
            steps {
                // Store quality gate result
                script {
                    def qg = waitForQualityGate()
                    // Optionally fail the build if not passed
                    if (qg.status != 'OK') {
                        error "Quality Gate failed: ${qg.status}"
                    }
                }
            }
        }
        
        stage('Test') {
            steps {
                sh 'mvn test'
            }
            post {
                always {
                    junit 'target/surefire-reports/*.xml'
                }
            }
        }
        
        stage('Package') {
            steps {
                sh 'mvn package -DskipTests'
            }
        }
    }
    
    post {
        always {
            // Archive artifacts
            archiveArtifacts artifacts: 'target/*.jar', fingerprint: true
        }
        success {
            echo 'Build and SonarQube analysis succeeded!'
        }
    }
}
```

---

## SonarQube Scanner Parameters

### Required Parameters

| Parameter | Description | Example |
|-----------|-------------|---------|
| `sonar.projectKey` | Unique project key | `my-app` |
| `sonar.sources` | Source code location | `src` |

### Common Parameters

| Parameter | Description | Example |
|-----------|-------------|---------|
| `sonar.projectName` | Display name | `My Application` |
| `sonar.projectVersion` | Version | `1.0.0` |
| `sonar.language` | Language | `java`, `js` |
| `sonar.java.binaries` | Compiled classes | `target/classes` |
| `sonar.sourceEncoding` | Source encoding | `UTF-8` |

---

## Java with Maven Example

Using the SonarQube Maven plugin:

```groovy
pipeline {
    agent any
    
    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        
        stage('Build & Analyze') {
            steps {
                withSonarQubeEnv('SonarQube') {
                    sh 'mvn clean verify sonar:sonar'
                }
            }
        }
        
        stage('Quality Gate') {
            steps {
                waitForQualityGate()
            }
        }
    }
}
```

---

## JavaScript Example

```groovy
pipeline {
    agent any
    
    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        
        stage('Install') {
            steps {
                sh 'npm install'
            }
        }
        
        stage('SonarQube Analysis') {
            steps {
                withSonarQubeEnv('SonarQube') {
                    sh '''
                        sonar-scanner \
                          -Dsonar.projectKey=my-js-app \
                          -Dsonar.sources=src \
                          -Dsonar.tests=test \
                          -Dsonar.typescript.libraries=node_modules
                    '''
                }
            }
        }
        
        stage('Quality Gate') {
            steps {
                waitForQualityGate()
            }
        }
    }
}
```

---

## waitForQualityGate Options

### Basic

```groovy
waitForQualityGate()
```

### With Timeout

```groovy
waitForQualityGate timeout: 5  // 5 minutes
```

### Store Result

```groovy
script {
    def qg = waitForQualityGate()
    echo "Quality Gate Status: ${qg.status}"
    // qg.status can be: OK, WARNING, ERROR, NONE
}
```

---

## Common Mistakes

### Mistake 1: Wrong Server Name

```groovy
// ❌ Wrong - name doesn't match Jenkins config
withSonarQubeEnv('My SonarQube') { }

// ✅ Must match configured server name
withSonarQubeEnv('SonarQube') { }
```

### Mistake 2: Missing Quality Gate

```groovy
// ❌ Build might fail because analysis isn't done
withSonarQubeEnv('SonarQube') {
    sh 'sonar-scanner ...'
}
sh 'continue to next stage'  // Analysis might not be done!

// ✅ Wait for quality gate
withSonarQubeEnv('SonarQube') {
    sh 'sonar-scanner ...'
}
waitForQualityGate()  // Wait for analysis to complete
```

### Mistake 3: Wrong Project Key

```groovy
// ❌ Each project must have unique key
-Dsonar.projectKey=my-app  // Already exists in SonarQube!

// ✅ Use unique key per project
-Dsonar.projectKey=my-app-${env.BUILD_NUMBER}
```

---

## SonarQube Quality Gates

In SonarQube, configure quality gates to:

- Require minimum coverage (e.g., 80%)
- Block on new bugs
- Require maintainability rating

```
┌─────────────────────────────────────────────────────────────────────┐
│  Quality Gate: Jenkins Default                                      │
│                                                                     │
│  Conditions:                                                        │
│  ├── New Code Coverage ≥ 80%                                        │
│  ├── New Code Bugs = 0                                              │
│  ├── New Code Vulnerabilities = 0                                   │
│  └── Maintainability Rating = A                                     │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Next Steps

- **[Code Coverage Reports](03-code-coverage-reports.md)** - Publish coverage
- **[Pipeline Best Practices](.../02-pipelines/... )** - Improve your pipelines
- **[Shared Libraries](04-advanced-topics/01-shared-libraries/01-what-are-shared-libraries.md)** - Reusable code
