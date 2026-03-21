# Docker Pipeline Plugin

## What this covers

This guide explains how to install and use the Docker Pipeline plugin to build, run, and push Docker containers from Jenkins pipelines. You'll learn about `docker.build()`, `docker.image()`, `docker.withRegistry()` and how to configure credentials for private registries.

## Prerequisites

- Docker installed on Jenkins server/agent
- Docker Pipeline plugin installed
- Understanding of Pipeline syntax
- Basic Docker knowledge

## Installing Docker Pipeline Plugin

1. Go to **Manage Jenkins** → **Plugin Manager**
2. Search for "Docker Pipeline"
3. Check **Docker Pipeline** plugin
4. Click **Install**

---

## Docker Pipeline Steps

### docker.build()

Builds a Docker image from a Dockerfile:

```groovy
pipeline {
    agent any
    
    stages {
        stage('Build Image') {
            steps {
                // Build image from Dockerfile in current directory
                // Returns image object
                def image = docker.build("my-app:latest")
                
                // Build with custom Dockerfile path
                def image = docker.build("my-app:latest", "-f Dockerfile.prod .")
                
                // Build with build arguments
                def image = docker.build("my-app:latest", "--build-arg VERSION=1.0 .")
            }
        }
    }
}
```

### docker.image()

Uses an existing Docker image without building:

```groovy
stage('Test in Container') {
    steps {
        // Pull and use existing image
        docker.image('node:20-alpine').inside {
            sh 'node --version'
        }
    }
}
```

### docker.withRegistry()

Authenticate with a Docker registry:

```groovy
stage('Push to Registry') {
    steps {
        // Use private registry with credentials
        docker.withRegistry('https://registry.example.com', 'docker-hub-credentials') {
            docker.build('my-app:latest').push()
        }
    }
}
```

---

## Complete Docker Build-Push Pipeline

```groovy
pipeline {
    agent any
    
    environment {
        REGISTRY = 'docker.io'
        IMAGE_NAME = 'myuser/myapp'
        IMAGE_TAG = "${env.BUILD_NUMBER}"
    }
    
    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        
        stage('Build Image') {
            steps {
                script {
                    // Build the Docker image
                    // docker.build(imageName:tag, args)
                    def image = docker.build("${IMAGE_NAME}:${IMAGE_TAG}")
                    
                    // Also tag as latest
                    image.tag('latest')
                }
            }
        }
        
        stage('Test in Container') {
            steps {
                // Run tests inside the container
                script {
                    docker.image("${IMAGE_NAME}:${IMAGE_TAG}").inside {
                        sh '''
                            npm install
                            npm test
                        '''
                    }
                }
            }
        }
        
        stage('Push to Docker Hub') {
            steps {
                script {
                    // Push to Docker Hub
                    docker.withRegistry('https://index.docker.io/v1/', 'docker-hub-credentials') {
                        docker.image("${IMAGE_NAME}:${IMAGE_TAG}").push()
                        docker.image("${IMAGE_NAME}:latest").push()
                    }
                }
            }
        }
        
        stage('Push to Private Registry') {
            when {
                branch 'main'
            }
            steps {
                script {
                    docker.withRegistry('https://registry.example.com', 'ecr-credentials') {
                        docker.image("${IMAGE_NAME}:${IMAGE_TAG}").push()
                    }
                }
            }
        }
    }
    
    post {
        always {
            // Clean up
            sh 'docker system prune -f'
        }
    }
}
```

---

## Using Docker Inside Containers

### docker.inside()

Run commands inside a Docker container:

```groovy
stage('Build') {
    steps {
        // Run inside Node container
        docker.image('node:20').inside {
            sh 'npm install'
            sh 'npm run build'
        }
    }
}
```

### Inside with Custom Settings

```groovy
docker.image('node:20').inside('--network=host -v /data:/data') {
    sh 'node app.js'
}
```

| Argument | Description |
|----------|-------------|
| `--network=host` | Use host network |
| `-v /host/path:/container/path` | Mount volume |
| `-e VAR=value` | Environment variable |
| `--memory=1g` | Limit memory |

---

## Using Docker with Docker (DinD)

For building Docker images inside Docker:

### Method 1: Mount Docker Socket

```groovy
pipeline {
    agent {
        docker {
            image 'docker:latest'
            args '-v /var/run/docker.sock:/var/run/docker.sock'
        }
    }
    
    stages {
        stage('Build') {
            steps {
                sh 'docker build -t myapp .'
            }
        }
    }
}
```

### Method 2: Docker-in-Docker (dind)

```groovy
agent {
    docker {
        image 'docker:dind'
        args '--privileged'
    }
}
```

---

## Working with Docker Hub

### Push to Docker Hub

```groovy
// Login to Docker Hub
docker.withRegistry('https://index.docker.io/v1/', 'docker-hub-credentials') {
    // Push image
    docker.image('myuser/myapp:tag').push()
    
    // Push multiple tags
    docker.image('myuser/myapp:tag').push('latest')
    docker.image('myuser/myapp:tag').push('v1.0.0')
}
```

### Pull from Private Repository

```groovy
docker.withRegistry('https://registry.example.com', 'docker-credentials') {
    docker.image('private-repo/myapp:latest').pull()
}
```

---

## Working with AWS ECR

### Setup

1. Install AWS CLI in Jenkins
2. Configure AWS credentials in Jenkins
3. Use ECR registry URL

```groovy
pipeline {
    agent any
    
    environment {
        AWS_REGION = 'us-east-1'
        ECR_REGISTRY = "${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_REGION}.amazonaws.com"
    }
    
    stages {
        stage('Login to ECR') {
            steps {
                script {
                    // Get ECR login password
                    def ecrLogin = sh(
                        script: "aws ecr get-login-password --region ${AWS_REGION}",
                        returnStdout: true
                    ).trim()
                    
                    // Login to ECR
                    sh "docker login -u AWS -p ${ecrLogin} ${ECR_REGISTRY}"
                }
            }
        }
        
        stage('Build and Push') {
            steps {
                script {
                    def image = docker.build("${ECR_REGISTRY}/myapp:latest")
                    image.push()
                }
            }
        }
    }
}
```

---

## Docker Pipeline Reference

### Methods

| Method | Description |
|--------|-------------|
| `docker.build(imageName)` | Build image from Dockerfile |
| `docker.build(imageName, args)` | Build with custom args |
| `docker.image(imageName)` | Reference existing image |
| `docker.withRegistry(url, creds) { }` | Authenticate to registry |
| `docker.withTool(toolName) { }` | Use custom Docker tool |

### Image Methods

| Method | Description |
|--------|-------------|
| `image.inside { }` | Run commands inside container |
| `image.push()` | Push to registry |
| `image.pull()` | Pull from registry |
| `image.tag(tag)` | Add tag |
| `image.push(tag)` | Push with specific tag |
| `image.inspect()` | Get container info |
| `image.id` | Get image ID |

---

## Common Mistakes

### Mistake 1: No Docker Socket

```groovy
// ❌ Won't work - can't run docker in docker
agent { docker { image 'docker:latest' } }

// ✅ Mount socket
agent { 
    docker { 
        image 'docker:latest'
        args '-v /var/run/docker.sock:/var/run/docker.sock'
    } 
}
```

### Mistake 2: Wrong Registry URL

```groovy
// ❌ Wrong Docker Hub URL
docker.withRegistry('https://docker.io', 'creds')

// ✅ Correct Docker Hub URL
docker.withRegistry('https://index.docker.io/v1/', 'creds')
```

### Mistake 3: Not Logging In

```groovy
// ❌ Push will fail - not logged in
docker.image('private-repo/myapp').push()

// ✅ Login first
docker.withRegistry('https://private-repo.com', 'creds') {
    docker.image('private-repo/myapp').push()
}
```

---

## Next Steps

- **[Docker Agent in Pipeline](02-docker-agent-in-pipeline.md)** - Use Docker containers as agents
- **[Building and Pushing Images](03-building-and-pushing-images.md)** - Complete CI/CD with Docker
- **[Kubernetes Agents](04-advanced-topics/03-kubernetes-agents/01-kubernetes-plugin-setup.md)** - Run agents in Kubernetes
