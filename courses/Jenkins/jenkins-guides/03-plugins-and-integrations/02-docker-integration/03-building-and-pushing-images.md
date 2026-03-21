# Building and Pushing Docker Images

## What this covers

This guide provides a complete walkthrough of a Docker build-tag-push pipeline. You'll learn multi-tag strategies (`:latest` + `:<build-number>`), using registries with authentication (ECR, Docker Hub, private registries), and the complete CI/CD workflow.

## Prerequisites

- Docker Pipeline plugin installed
- Understanding of Docker commands
- Access to Docker registry

---

## Complete Build-Tag-Push Pipeline

```groovy
pipeline {
    agent any
    
    environment {
        // Registry configuration
        REGISTRY = 'docker.io'
        IMAGE_NAME = 'myuser/myapp'
        
        // Tag strategy
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
                    // Build image from Dockerfile
                    // docker.build(imageName:tag, dockerfileArgs)
                    def image = docker.build("${IMAGE_NAME}:${IMAGE_TAG}")
                    
                    // Also tag as latest for main branch builds
                    if (env.GIT_BRANCH == 'main') {
                        image.tag('latest')
                    }
                }
            }
        }
        
        stage('Test Image') {
            steps {
                script {
                    // Run tests inside the container
                    docker.image("${IMAGE_NAME}:${IMAGE_TAG}").inside {
                        sh '''
                            echo "Running tests..."
                            npm install
                            npm test
                        '''
                    }
                }
            }
        }
        
        stage('Push to Registry') {
            when {
                branch 'main'
            }
            steps {
                script {
                    // Push to Docker Hub
                    docker.withRegistry('https://index.docker.io/v1/', 'docker-hub-credentials') {
                        // Push build tag
                        docker.image("${IMAGE_NAME}:${IMAGE_TAG}").push()
                        
                        // Push latest
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
                    docker.withRegistry('https://registry.example.com', 'private-registry-credentials') {
                        docker.image("${IMAGE_NAME}:${IMAGE_TAG}").push()
                    }
                }
            }
        }
    }
    
    post {
        always {
            // Cleanup
            sh "docker rmi ${IMAGE_NAME}:${IMAGE_TAG} || true"
        }
    }
}
```

---

## Multi-Tag Strategy

### Push Multiple Tags

```groovy
script {
    def image = docker.build("${IMAGE_NAME}:${IMAGE_TAG}")
    
    // Tag with multiple tags
    image.tag('latest')
    image.tag('v1.0.0')
    image.tag('v1.0')
    image.tag('staging')
}
```

### Push Multiple Tags to Registry

```groovy
script {
    docker.withRegistry('https://index.docker.io/v1/', 'credentials') {
        def image = docker.image("${IMAGE_NAME}:${IMAGE_TAG}")
        
        // Push all tags
        image.push()
        image.push('latest')
        image.push('v1.0.0')
    }
}
```

### Build Number + Git SHA Tags

```groovy
environment {
    GIT_SHA_SHORT = "${env.GIT_COMMIT?.take(8) ?: 'local'}"
    IMAGE_TAG = "${env.BUILD_NUMBER}-${GIT_SHA_SHORT}"
}
```

---

## Docker Hub Example

```groovy
pipeline {
    agent any
    
    environment {
        DOCKER_HUB_REPO = 'myuser/myapp'
    }
    
    stages {
        stage('Build') {
            steps {
                script {
                    docker.build("${DOCKER_HUB_REPO}:${env.BUILD_NUMBER}")
                }
            }
        }
        
        stage('Push to Docker Hub') {
            steps {
                script {
                    docker.withRegistry('https://index.docker.io/v1/', 'docker-hub-creds') {
                        // Push with build number
                        docker.image("${DOCKER_HUB_REPO}:${env.BUILD_NUMBER}").push()
                        
                        // If main branch, also push latest
                        if (env.GIT_BRANCH == 'main') {
                            docker.image("${DOCKER_HUB_REPO}:latest").push()
                        }
                    }
                }
            }
        }
    }
}
```

---

## AWS ECR Example

### Prerequisites

1. AWS CLI installed on Jenkins
2. AWS credentials configured in Jenkins
3. ECR repository created

```groovy
pipeline {
    agent any
    
    environment {
        AWS_ACCOUNT_ID = '123456789012'
        AWS_REGION = 'us-east-1'
        ECR_REGISTRY = "${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_REGION}.amazonaws.com"
        IMAGE_NAME = 'myapp'
    }
    
    stages {
        stage('Login to ECR') {
            steps {
                sh '''
                    # Get ECR login password and login
                    aws ecr get-login-password --region ${AWS_REGION} | \
                        docker login --username AWS --password-stdin ${ECR_REGISTRY}
                '''
            }
        }
        
        stage('Build') {
            steps {
                sh "docker build -t ${ECR_REGISTRY}/${IMAGE_NAME}:${env.BUILD_NUMBER} ."
            }
        }
        
        stage('Push to ECR') {
            steps {
                sh '''
                    # Push with build number tag
                    docker push ${ECR_REGISTRY}/${IMAGE_NAME}:${env.BUILD_NUMBER}
                    
                    # Push latest if main branch
                    if [ "${GIT_BRANCH}" = "main" ]; then
                        docker tag ${ECR_REGISTRY}/${IMAGE_NAME}:${env.BUILD_NUMBER} \
                                   ${ECR_REGISTRY}/${IMAGE_NAME}:latest
                        docker push ${ECR_REGISTRY}/${IMAGE_NAME}:latest
                    fi
                '''
            }
        }
    }
}
```

---

## Google Container Registry (GCR/GAR)

```groovy
pipeline {
    agent any
    
    environment {
        PROJECT_ID = 'my-project'
        GCR_REGISTRY = 'gcr.io/${PROJECT_ID}'
    }
    
    stages {
        stage('Configure Docker') {
            steps {
                sh 'gcloud auth configure-docker'
            }
        }
        
        stage('Build') {
            steps {
                sh "docker build -t ${GCR_REGISTRY}/myapp:${env.BUILD_NUMBER} ."
            }
        }
        
        stage('Push to GCR') {
            steps {
                sh "docker push ${GCR_REGISTRY}/myapp:${env.BUILD_NUMBER}"
            }
        }
    }
}
```

---

## GitHub Container Registry (ghcr.io)

```groovy
pipeline {
    agent any
    
    environment {
        GITHUB_ORG = 'my-org'
        GITHUB_REPO = 'my-app'
        GHR_REGISTRY = 'ghcr.io'
    }
    
    stages {
        stage('Login to GHCR') {
            steps {
                withCredentials([string(credentialsId: 'ghcr-token', variable: 'GITHUB_TOKEN')]) {
                    sh '''
                        echo $GITHUB_TOKEN | docker login ${GHR_REGISTRY} -u $GITHUB_USERNAME --password-stdin
                    '''
                }
            }
        }
        
        stage('Build and Push') {
            steps {
                sh '''
                    docker build -t ${GHR_REGISTRY}/${GITHUB_ORG}/${GITHUB_REPO}:${BUILD_NUMBER} .
                    docker build -t ${GHR_REGISTRY}/${GITHUB_ORG}/${GITHUB_REPO}:latest .
                    
                    docker push ${GHR_REGISTRY}/${GITHUB_ORG}/${GITHUB_REPO}:${BUILD_NUMBER}
                    docker push ${GHR_REGISTRY}/${GITHUB_ORG}/${GITHUB_REPO}:latest
                '''
            }
        }
    }
}
```

---

## Complete Production Pipeline

```groovy
pipeline {
    agent any
    
    environment {
        REGISTRY = 'docker.io'
        IMAGE_NAME = 'mycompany/myapp'
    }
    
    parameters {
        choice(name: 'DEPLOY_ENV', choices: ['dev', 'staging', 'prod'], description: 'Target environment')
    }
    
    stages {
        stage('Initialize') {
            steps {
                script {
                    env.IMAGE_TAG = "${env.BUILD_NUMBER}-${env.GIT_BRANCH?.replace('/', '-')}"
                }
            }
        }
        
        stage('Build') {
            steps {
                script {
                    def image = docker.build("${IMAGE_NAME}:${env.IMAGE_TAG}")
                    image.tag('latest')
                }
            }
        }
        
        stage('Security Scan') {
            steps {
                script {
                    docker.image("${IMAGE_NAME}:${env.IMAGE_TAG}").inside {
                        // Run security scan
                        sh 'trivy image --severity HIGH,CRITICAL . || true'
                    }
                }
            }
        }
        
        stage('Test') {
            parallel {
                stage('Unit Tests') {
                    steps {
                        docker.image("${IMAGE_NAME}:${env.IMAGE_TAG}").inside {
                            sh 'npm run test:unit'
                        }
                    }
                }
                stage('Integration Tests') {
                    steps {
                        docker.image("${IMAGE_NAME}:${env.IMAGE_TAG}").inside {
                            sh 'npm run test:integration'
                        }
                    }
                }
            }
        }
        
        stage('Push') {
            when {
                anyOf {
                    branch 'main'
                    branch 'develop'
                }
            }
            steps {
                script {
                    def registry = params.DEPLOY_ENV == 'prod' ? 
                        'https://index.docker.io/v1/' : 
                        'https://registry.example.com'
                    
                    docker.withRegistry(registry, 'registry-credentials') {
                        docker.image("${IMAGE_NAME}:${env.IMAGE_TAG}").push()
                        docker.image("${IMAGE_NAME}:latest").push()
                    }
                }
            }
        }
    }
    
    post {
        always {
            sh "docker rmi ${IMAGE_NAME}:${env.IMAGE_TAG} || true"
        }
    }
}
```

---

## Common Mistakes

### Mistake 1: Not Logging In

```groovy
// ❌ Will fail for private registry
docker.image('private-repo/myapp').push()

// ✅ Login first
docker.withRegistry('https://private-repo.com', 'credentials') {
    docker.image('private-repo/myapp').push()
}
```

### Mistake 2: Wrong Registry URL

```groovy
// ❌ Wrong Docker Hub URL
docker.withRegistry('https://docker.io', 'creds')

// ✅ Correct
docker.withRegistry('https://index.docker.io/v1/', 'creds')
```

### Mistake 3: Forgetting to Tag

```groovy
// ❌ Image not tagged
docker.build('myapp')  // Only builds, no tag

// ✅ Tag the image
def img = docker.build('myapp:latest')
img.tag('v1.0')
```

---

## Next Steps

- **[JUnit Plugin](03-testing-and-quality/01-junit-plugin.md)** - Publish test results
- **[SonarQube Integration](03-testing-and-quality/02-sonarqube-integration.md)** - Code quality analysis
- **[Kubernetes Agents](04-advanced-topics/03-kubernetes-agents/01-kubernetes-plugin-setup.md)** - Deploy to Kubernetes
