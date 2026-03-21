# Pipeline Agents

## What this covers

This guide explains how to configure where your Jenkins Pipeline runs using the `agent` directive. You'll learn about `agent any`, `agent none`, labeled agents, Docker agents, Kubernetes agents, and per-stage agent allocation.

## Prerequisites

- Understanding of Declarative Pipeline syntax
- Completed previous Pipeline guides

## What is an Agent?

An **agent** is where your pipeline or stage runs. It could be:
- The Jenkins master server
- A Jenkins agent (remote worker)
- A Docker container
- A Kubernetes pod
- A cloud instance

> Think of agents as **workers**. The agent directive assigns workers to your pipeline.

---

## Agent Types

### 1. agent any

Run anywhere—the pipeline runs on any available executor.

```groovy
// The simplest agent - runs on any available agent
pipeline {
    agent any  // Any executor will do
    
    stages {
        stage('Build') {
            steps {
                echo 'Running on any agent'
            }
        }
    }
}
```

**When to use**: Quick tests, simple pipelines, single-machine Jenkins

---

### 2. agent none

Don't allocate an agent at pipeline level—require each stage to specify its own agent.

```groovy
// No agent at pipeline level - stages must specify agents
pipeline {
    agent none  // Must specify agent per stage
    
    stages {
        // This stage runs on any agent
        stage('Build') {
            agent any
            steps {
                echo 'Build stage'
            }
        }
        
        // This stage runs on Linux
        stage('Linux Tests') {
            agent {
                label 'linux'  // Only runs on agents with 'linux' label
            }
            steps {
                echo 'Linux tests'
            }
        }
    }
}
```

**When to use**: Different stages need different environments

---

### 3. agent with Label

Run on agents with specific labels:

```groovy
pipeline {
    // Run on any agent with 'linux' label
    agent {
        label 'linux'  // Agent must have label 'linux'
    }
    
    stages {
        stage('Build') {
            steps {
                sh 'uname -a'  // Should show Linux
            }
        }
    }
}
```

#### Label with Multiple Requirements

```groovy
agent {
    label 'linux && docker'  // Must have BOTH labels
}
```

```groovy
agent {
    label 'linux || windows'  // Must have AT LEAST ONE
}
```

**When to use**: Target specific environments (Linux, Windows, macOS, etc.)

---

### 4. agent with Docker

Run pipeline inside a Docker container:

```groovy
pipeline {
    // Run in a Node.js container
    agent {
        docker {
            // image: The Docker image to use
            image 'node:20-alpine'
            
            // label: Agent label (optional, for documentation)
            label 'docker'
            
            // args: Container arguments
            args '-v /home/jenkins/.npm:/root/.npm'
        }
    }
    
    stages {
        stage('Build') {
            steps {
                sh 'node --version'  // Node inside container
                sh 'npm install'
            }
        }
    }
}
```

#### Docker Agent Options Explained

| Option | Example | Description |
|--------|---------|-------------|
| `image` | `'node:20'` | Docker image name |
| `label` | `'docker'` | Jenkins agent label |
| `args` | `'-v /data:/data'` | Container run arguments |
| `registryUrl` | `'https://registry.hub.docker.com'` | Private registry |
| `registryCredentialsId` | `'docker-hub-credentials'` | Auth for private registry |
| `reuseNode` | `false` | Use same workspace as parent |

```groovy
agent {
    docker {
        image 'maven:3.9-eclipse-temurin-17'
        label 'docker'
        args '-v $HOME/.m2:/root/.m2'  // Mount Maven cache
        reuseNode true  // Use the same workspace
    }
}
```

---

### 5. agent with Kubernetes

Run in Kubernetes pods (requires Kubernetes plugin):

```groovy
pipeline {
    agent {
        kubernetes {
            // cloud: Name of Kubernetes cloud (from Jenkins config)
            cloud 'kubernetes'
            
            // label: Pod label
            label 'jenkins-agent'
            
            // defaultContainer: The container to use
            defaultContainer 'jnlp'
            
            // Pod template specification
            yaml '''
                apiVersion: v1
                kind: Pod
                spec:
                  containers:
                  - name: builder
                    image: maven:3.9-eclipse-temurin-17
                    command:
                    - cat
                    tty: true
            '''
        }
    }
    
    stages {
        stage('Build') {
            steps {
                container('builder') {
                    sh 'mvn clean package'
                }
            }
        }
    }
}
```

---

## Per-Stage Agents

Run different stages on different agents:

```groovy
pipeline {
    // No agent at pipeline level
    agent none
    
    stages {
        // Stage 1: Build on any agent
        stage('Build') {
            agent any
            steps {
                echo 'Building...'
            }
        }
        
        // Stage 2: Test on Linux only
        stage('Test on Linux') {
            agent {
                label 'linux'
            }
            steps {
                sh 'make test'
            }
        }
        
        // Stage 3: Test on Windows only
        stage('Test on Windows') {
            agent {
                label 'windows'
            }
            steps {
                bat 'make test'
            }
        }
        
        // Stage 4: Docker build
        stage('Build Docker') {
            agent {
                docker {
                    image 'docker:latest'
                    args '-v /var/run/docker.sock:/var/run/docker.sock'
                }
            }
            steps {
                sh 'docker build -t myapp:latest .'
            }
        }
    }
}
```

---

## Complete Docker Agent Example

Here's a fully configured Docker agent with registry authentication:

```groovy
pipeline {
    // Run entire pipeline in Docker container
    agent {
        docker {
            // Image with JDK and Maven
            image 'maven:3.9-eclipse-temurin-17'
            
            // Mount Maven repository cache for faster builds
            args '-v $HOME/.m2:/root/.m2'
            
            // Label for the Jenkins agent
            label 'docker'
            
            // Reuse parent node's workspace
            reuseNode true
        }
    }
    
    environment {
        // Environment variables available in the container
        MAVEN_OPTS = '-Xmx1024m'
    }
    
    stages {
        stage('Checkout') {
            steps {
                echo 'Checking out source code...'
                checkout scm
            }
        }
        
        stage('Build') {
            steps {
                echo 'Building with Maven...'
                sh 'mvn clean package -DskipTests'
            }
        }
        
        stage('Test') {
            steps {
                echo 'Running tests...'
                sh 'mvn test'
            }
            
            post {
                always {
                    // Publish test results
                    junit '**/target/surefire-reports/*.xml'
                }
            }
        }
        
        stage('Archive') {
            steps {
                echo 'Archiving artifacts...'
                archiveArtifacts artifacts: 'target/*.jar', fingerprint: true
            }
        }
    }
}
```

---

## Setting Up Agent Labels

### In Jenkins UI

1. Go to **Manage Jenkins** → **Manage Nodes**
2. Click on the agent (or master)
3. In **Labels**, add labels (space-separated):

```
linux docker maven nodejs
```

### In Kubernetes Plugin

In Jenkins → Manage Jenkins → Configure Clouds → Kubernetes:

```yaml
# Kubernetes Pod Template
name: maven-builder
label: maven-builder
containers:
- name: maven
  image: maven:3.9-eclipse-temurin-17
  command: cat
  tty: true
```

---

## Agent vs Node

| Directive | What it Does |
|-----------|--------------|
| `agent` | Declarative Pipeline - where to run |
| `node` | Scripted Pipeline - same as agent |

```groovy
// Declarative (recommended)
pipeline {
    agent any
    stages { ... }
}

// Scripted (older style)
node {
    stage('Build') { ... }
}
```

---

## Common Mistakes

### Mistake 1: Forgetting Agent

```groovy
// ⚠️ WARNING - May fail without agent specified
pipeline {
    // No agent!
    stages {
        stage('Build') {
            steps {
                sh 'echo hello'
            }
        }
    }
}

// ✅ CORRECT
pipeline {
    agent any
    stages { ... }
}
```

### Mistake 2: Docker Image Doesn't Exist

```groovy
// ❌ WRONG - Typo in image name
agent {
    docker {
        image 'nod:20'  // Wrong! Should be 'node'
    }
}

// ✅ CORRECT
agent {
    docker {
        image 'node:20'
    }
}
```

### Mistake 3: Missing Container Switch

```groovy
// ❌ WRONG - Using wrong container
stage('Build') {
    steps {
        sh 'mvn build'  // Maven not in default container!
    }
}

// ✅ CORRECT - Switch to correct container
stage('Build') {
    steps {
        container('maven') {
            sh 'mvn build'
        }
    }
}
```

---

## Next Steps

- **[Environment Variables](02-environment-and-credentials/01-environment-variables.md)** - Use environment variables in pipelines
- **[Credentials Plugin](02-environment-and-credentials/02-credentials-plugin.md)** - Use secrets securely
- **[Kubernetes Agents](04-advanced-topics/03-kubernetes-agents/01-kubernetes-plugin-setup.md)** - Full Kubernetes integration
