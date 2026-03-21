# Creating a Shared Library

## What this covers

This guide walks through creating a complete Shared Library with the `vars/`, `src/`, and `resources/` directories. You'll learn how to create reusable functions, configure the library in Jenkins, and follow best practices.

## Prerequisites

- Understanding of what shared libraries are
- Git repository to store the library
- Access to Jenkins to configure the library

## Step 1: Create Library Repository

Create a new Git repository for your shared library:

```bash
# Clone or create repository
git init my-jenkins-shared-library
cd my-jenkins-shared-library
```

---

## Step 2: Create Folder Structure

```
my-jenkins-shared-library/
├── src/                      # Groovy classes
│   └── org/
│       └── mycompany/
│           └── Deploy.groovy
├── vars/                     # Global functions
│   ├── buildApp.groovy
│   ├── buildApp.txt
│   ├── runTests.groovy
│   └── notify.groovy
├── resources/                # Static files
│   └── org/
│       └── mycompany/
│           └── templates/
│               └── deployment.yaml
├── test/                     # Unit tests (optional)
├── README.md
└── Jenkinsfile               # Test pipeline (optional)
```

---

## Step 3: Create Global Functions

### Basic Function (vars/buildApp.groovy)

```groovy
/**
 * Build a Java application with Maven
 * 
 * Usage: buildApp()
 * 
 * @param config Map with options:
 *        appName - Application name (default: 'app')
 *        version - Version to build (default: 'SNAPSHOT')
 *        javaVersion - Java version (default: '17')
 */
def call(Map config = [:]) {
    def appName = config.appName ?: 'app'
    def version = config.version ?: 'SNAPSHOT'
    def javaVersion = config.javaVersion ?: '17'
    
    echo "=========================================="
    echo "Building: ${appName}"
    echo "Version: ${version}"
    echo "Java: ${javaVersion}"
    echo "=========================================="
    
    stage('Checkout') {
        checkout scm
    }
    
    stage('Build') {
        sh "mvn clean package -Dversion=${version}"
    }
    
    stage('Test') {
        sh 'mvn test'
    }
    
    echo "Build complete for ${appName}:${version}"
}
```

### Add Help Documentation (vars/buildApp.txt)

```groovy
// vars/buildApp.txt

Build a Java application using Maven.

Usage:
    buildApp()
    buildApp(appName: 'myapp', version: '1.0.0')

Parameters:
    appName    - Application name (default: 'app')
    version    - Version to build (default: 'SNAPSHOT')
    javaVersion - Java version (default: '17')

Example:
    buildApp(appName: 'my-api', version: '2.0.0')
```

---

## Step 4: Create More Functions

### Test Runner (vars/runTests.groovy)

```groovy
/**
 * Run tests with coverage
 * 
 * @param config Map with options:
 *        type - Test framework type (junit, jest, pytest)
 *        coverage - Enable coverage (default: true)
 */
def call(Map config = [:]) {
    def testType = config.type ?: 'junit'
    def enableCoverage = config.coverage ?: true
    
    echo "Running ${testType} tests..."
    
    if (testType == 'junit') {
        sh 'mvn test'
        
        junit 'target/surefire-reports/*.xml'
        
        if (enableCoverage) {
            recordCoverage(
                tools: [jacoco(pattern: '**/jacoco.xml')]
            )
        }
    } else if (testType == 'jest') {
        sh 'npm test'
        
        junit 'test-results/junit.xml'
        
        if (enableCoverage) {
            recordCoverage(
                tools: [jacoco(pattern: '**/coverage/cobertura.xml')]
            )
        }
    }
    
    echo "Tests complete!"
}
```

### Notification Function (vars/notify.groovy)

```groovy
/**
 * Send notifications
 * 
 * @param config Map with options:
 *        channel - Slack channel
 *        message - Message to send
 *        status - Build status (SUCCESS, FAILURE, etc.)
 */
def call(Map config = [:]) {
    def channel = config.channel ?: '#builds'
    def message = config.message ?: 'Build notification'
    def status = currentBuild.result ?: 'SUCCESS'
    
    def color = status == 'SUCCESS' ? 'good' : 'danger'
    
    slackSend channel: channel,
              color: color,
              message: "${message} - ${env.JOB_NAME} #${env.BUILD_NUMBER}"
    
    emailext subject: "${status}: ${env.JOB_NAME}",
             body: "Build ${env.BUILD_NUMBER} - ${status}",
             to: 'team@company.com'
}
```

---

## Step 5: Create Groovy Classes (src/)

### src/org/mycompany/Deploy.groovy

```groovy
package org.mycompany

class Deploy implements Serializable {
    def script
    
    Deploy(script) {
        this.script = script
    }
    
    /**
     * Deploy to Kubernetes
     */
    def toKubernetes(Map config) {
        def appName = config.appName
        def image = config.image
        def namespace = config.namespace ?: 'default'
        def environment = config.environment ?: 'staging'
        
        script.echo "Deploying ${appName} to ${namespace}/${environment}"
        
        script.sh """
            kubectl set image deployment/${appName} \\
                ${appName}=${image} \\
                -n ${namespace}
        """
        
        // Wait for rollout
        script.sh """
            kubectl rollout status deployment/${appName} \\
                -n ${namespace} \\
                --timeout=5m
        """
    }
    
    /**
     * Deploy to Docker Compose
     */
    def toDockerCompose(Map config) {
        def composeFile = config.file ?: 'docker-compose.yml'
        
        script.echo "Deploying with Docker Compose..."
        
        script.sh """
            docker-compose -f ${composeFile} up -d
        """
    }
}
```

---

## Step 6: Create Resource Files

### resources/org/mycompany/templates/deployment.yaml

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: APP_NAME
  namespace: NAMESPACE
spec:
  replicas: REPLICAS
  selector:
    matchLabels:
      app: APP_NAME
  template:
    metadata:
      labels:
        app: APP_NAME
    spec:
      containers:
      - name: APP_NAME
        image: IMAGE
        ports:
        - containerPort: PORT
```

### Using Resources in Functions

```groovy
// In vars/deployK8s.groovy
def call(Map config) {
    def template = libraryResource 'org/mycompany/templates/deployment.yaml'
    
    // Replace placeholders
    def deployment = template
        .replace('APP_NAME', config.appName)
        .replace('IMAGE', config.image)
        .replace('NAMESPACE', config.namespace)
        .replace('REPLICAS', config.replicas ?: '3')
        .replace('PORT', config.port ?: '8080')
    
    // Write to file
    writeFile file: 'deployment.yaml', text: deployment
    
    // Apply
    sh 'kubectl apply -f deployment.yaml'
}
```

---

## Step 7: Configure in Jenkins

1. Go to **Manage Jenkins** → **Configure System**
2. Find **Global Pipeline Libraries**
3. Add library:

```
Name: my-shared-library
Default version: main
Repository URL: https://github.com/your-org/jenkins-shared-library.git
Credentials: (if private repo)
```

---

## Step 8: Test the Library

### Create Test Pipeline

```groovy
// Test in a separate job
@Library('my-shared-library') _

pipeline {
    agent any
    
    stages {
        stage('Build') {
            steps {
                buildApp(appName: 'test-app', version: '1.0.0')
            }
        }
        
        stage('Test') {
            steps {
                runTests(type: 'junit')
            }
        }
        
        stage('Notify') {
            steps {
                notify(
                    channel: '#test',
                    message: 'Test build complete'
                )
            }
        }
    }
}
```

---

## Best Practices

### 1. Use @NonCPS for Performance

```groovy
import groovy.transform.NonCPS

@NonCPS
def parseJson(String jsonText) {
    return new groovy.json.JsonSlurper().parseText(jsonText)
}
```

### 2. Handle Errors

```groovy
def call(Map config) {
    try {
        // Main logic
        sh "mvn clean package"
    } catch (Exception e) {
        error "Build failed: ${e.message}"
    } finally {
        // Cleanup
        sh 'mvn clean'
    }
}
```

### 3. Add Logging

```groovy
def call(Map config = [:]) {
    echo "[buildApp] Starting build for ${config.appName}"
    // ... logic
    echo "[buildApp] Build complete"
}
```

---

## Common Mistakes

### Wrong Method Signature

```groovy
// ❌ Wrong - no 'call' method
def buildApp() { }

// ✅ Correct - 'call' makes it a function
def call() { }
```

### Missing Underscore

```groovy
// ❌ WON'T WORK - no underscore
@Library('my-library')

pipeline { }

// ✅ Correct
@Library('my-library') _

pipeline { }
```

---

## Next Steps

- **[Using Shared Libraries in Pipelines](03-using-shared-libraries-in-pipelines.md)** - Advanced usage
- **[Parallel Stages](02-parallel-and-matrix/01-parallel-stages.md)** - Combine with parallel execution
- **[Kubernetes Agents](03-kubernetes-agents/01-kubernetes-plugin-setup.md)** - Deploy to K8s
