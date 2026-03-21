# Docker Agent in Pipeline

## What this covers

This guide explains how to use Docker containers as pipeline agents using `agent { docker {} }`. You'll learn about the `image`, `args`, `registryUrl`, and `registryCredentialsId` options, and how to mount the Docker socket for Docker-in-Docker scenarios.

## Prerequisites

- Docker Pipeline plugin installed
- Understanding of Docker concepts
- Completed Docker Pipeline Plugin guide

## Using Docker as Agent

Instead of running on a permanent Jenkins agent, you can use a Docker container as your build environment:

```groovy
pipeline {
    // Use Docker container as the agent
    agent {
        docker {
            // image: The Docker image to use as the agent
            image 'node:20-alpine'
        }
    }
    
    stages {
        stage('Build') {
            steps {
                sh 'node --version'
                sh 'npm install'
            }
        }
    }
}
```

---

## Docker Agent Options

### image

The Docker image to use:

```groovy
agent {
    docker {
        // Image name (required)
        image 'maven:3.9-eclipse-temurin-17'
    }
}
```

### label

A label for this agent:

```groovy
agent {
    docker {
        image 'node:20'
        label 'docker-agent'  // For documentation
    }
}
```

### args

Arguments passed to `docker run`:

```groovy
agent {
    docker {
        image 'node:20'
        
        // Container run arguments
        args '-v /data:/data -p 8080:8080 --network=my-network'
    }
}
```

| Argument | Description |
|----------|-------------|
| `-v host:container` | Mount volume |
| `-p host:container` | Publish port |
| `--network` | Join network |
| `--memory` | Limit memory |
| `--cpus` | Limit CPUs |
| `-e VAR=value` | Environment variable |

### registryUrl

Private registry URL:

```groovy
agent {
    docker {
        image 'my-registry.com/myimage:latest'
        registryUrl 'https://my-registry.com'
        registryCredentialsId 'my-registry-credentials'
    }
}
```

### registryCredentialsId

Credentials for private registry:

```groovy
agent {
    docker {
        image 'private-repo/myimage:latest'
        registryCredentialsId 'docker-credentials'
    }
}
```

### reuseNode

Use the same workspace as the parent node:

```groovy
agent {
    docker {
        image 'node:20'
        
        // Default: false
        // When true, uses parent node's workspace
        // When false, creates new workspace inside container
        reuseNode true
    }
}
```

---

## Full Example with All Options

```groovy
pipeline {
    agent {
        docker {
            // Image from private registry
            image 'registry.example.com/builder:node-20'
            
            // Registry authentication
            registryUrl 'https://registry.example.com'
            registryCredentialsId 'ecr-credentials'
            
            // Agent label
            label 'docker-build-agent'
            
            // Container configuration
            args '-v /workspace:/workspace -v /root/.m2:/root/.m2 --network=host'
            
            // Reuse parent workspace
            reuseNode false
        }
    }
    
    environment {
        NODE_ENV = 'production'
    }
    
    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        
        stage('Install Dependencies') {
            steps {
                sh 'npm ci'
            }
        }
        
        stage('Build') {
            steps {
                sh 'npm run build'
            }
        }
        
        stage('Test') {
            steps {
                sh 'npm test'
            }
        }
    }
    
    post {
        always {
            // Cleanup
            sh 'rm -rf node_modules'
        }
    }
}
```

---

## Multi-Container Agent

Use `dockerfile: true` to build from a Dockerfile in the repository:

```groovy
pipeline {
    agent {
        docker {
            // Build from Dockerfile in repository
            image 'my-builder'
            dockerfile true
            
            // Build args
            args '--build-arg VERSION=1.0'
            
            // Use as agent
            label 'custom-builder'
        }
    }
    
    stages {
        stage('Build') {
            steps {
                sh 'make build'
            }
        }
    }
}
```

---

## Using Sidecar Containers

For Docker-in-Docker, use multiple containers:

```groovy
pipeline {
    agent {
        docker {
            image 'ubuntu:22.04'
            label 'dind'
        }
    }
    
    stages {
        stage('Setup Docker') {
            steps {
                sh '''
                    # Install Docker in container
                    apt-get update
                    apt-get install -y docker.io
                    
                    # Start Docker daemon
                    dockerd &
                    sleep 5
                '''
            }
        }
        
        stage('Build Image') {
            steps {
                sh 'docker build -t myapp:latest .'
            }
        }
    }
}
```

---

## Mounting Docker Socket

For building Docker images, mount the Docker socket:

```groovy
pipeline {
    agent {
        docker {
            image 'docker:latest'
            
            // Mount Docker socket for DinD
            args '-v /var/run/docker.sock:/var/run/docker.sock'
        }
    }
    
    stages {
        stage('Build') {
            steps {
                sh 'docker build -t myapp:latest .'
            }
        }
        
        stage('Test') {
            steps {
                sh 'docker run myapp:latest npm test'
            }
        }
        
        stage('Push') {
            steps {
                sh 'docker push myapp:latest'
            }
        }
    }
}
```

---

## Per-Stage Docker Agents

Use different Docker containers for different stages:

```groovy
pipeline {
    agent none  // No agent at pipeline level
    
    stages {
        stage('Build with Maven') {
            agent {
                docker {
                    image 'maven:3.9-eclipse-temurin-17'
                    args '-v /root/.m2:/root/.m2'
                }
            }
            steps {
                sh 'mvn clean package'
            }
        }
        
        stage('Test with Node') {
            agent {
                docker {
                    image 'node:20'
                    args '-v /workspace/node_modules'
                }
            }
            steps {
                sh 'npm test'
            }
        }
        
        stage('Scan with Security Tool') {
            agent {
                docker {
                    image 'aquasec/trivy:latest'
                    args '--security-changes vuln'
                }
            }
            steps {
                sh 'trivy image myapp:latest'
            }
        }
    }
}
```

---

## Docker Agent with Kubernetes

Combining Docker agents with Kubernetes:

```groovy
pipeline {
    agent {
        kubernetes {
            label 'docker-agent'
            defaultContainer 'docker'
            yaml '''
                apiVersion: v1
                kind: Pod
                spec:
                  containers:
                  - name: docker
                    image: docker:latest
                    command:
                    - cat
                    tty: true
                    volumeMounts:
                    - name: docker-socket
                      mountPath: /var/run/docker.sock
                  volumes:
                  - name: docker-socket
                    hostPath:
                      path: /var/run/docker.sock
            '''
        }
    }
    
    stages {
        stage('Build Docker') {
            steps {
                sh 'docker build -t myapp:latest .'
            }
        }
    }
}
```

---

## Common Mistakes

### Mistake 1: Wrong Image Name

```groovy
// ❌ Wrong - image must exist
image 'nod:20'  // Typo!

// ✅ Correct
image 'node:20'
```

### Mistake 2: Forgetting Socket for DinD

```groovy
// ❌ Can't build Docker without socket
agent {
    docker {
        image 'docker:latest'
        // Missing socket!
    }
}

// ✅ Mount socket
agent {
    docker {
        image 'docker:latest'
        args '-v /var/run/docker.sock:/var/run/docker.sock'
    }
}
```

### Mistake 3: Port Conflicts

```groovy
// ❌ Might conflict with existing services
args '-p 8080:8080'

// ✅ Use dynamic port
args '-p 8080'
// Or
args '-P'  // Publish all exposed ports
```

---

## Next Steps

- **[Building and Pushing Images](03-building-and-pushing-images.md)** - Complete CI/CD workflow
- **[Kubernetes Agents](04-advanced-topics/03-kubernetes-agents/01-kubernetes-plugin-setup.md)** - Use Kubernetes for agents
- **[JUnit Plugin](03-testing-and-quality/01-junit-plugin.md)** - Publish test results
