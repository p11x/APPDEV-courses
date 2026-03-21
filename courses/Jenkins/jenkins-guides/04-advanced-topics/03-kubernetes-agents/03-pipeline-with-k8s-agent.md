# Complete Pipeline with Kubernetes Agent

## What this covers

This guide provides a complete real-world pipeline using Kubernetes pod agents. You'll see a full CI/CD workflow: checkout code, compile with Maven, build Docker image using Docker-in-Docker sidecar, push to registry, and deploy to Kubernetes cluster.

## Prerequisites

- Kubernetes Plugin configured
- Completed pod templates guide
- kubectl configured with access to target cluster

---

## Complete Kubernetes Pipeline

```groovy
@Library('jenkins-library') _

pipeline {
    agent {
        kubernetes {
            cloud 'kubernetes'
            label 'java-app-build'
            defaultContainer 'maven'
            
            yaml '''
                apiVersion: v1
                kind: Pod
                spec:
                  serviceAccountName: jenkins
                  containers:
                  - name: maven
                    image: maven:3.9-eclipse-temurin-17
                    command:
                    - cat
                    tty: true
                    volumeMounts:
                    - name: maven-cache
                      mountPath: /root/.m2
                  - name: docker
                    image: docker:latest
                    command:
                    - cat
                    tty: true
                    securityContext:
                      privileged: true
                    volumeMounts:
                    - name: docker-socket
                      mountPath: /var/run/docker.sock
                  - name: kubectl
                    image: bitnami/kubectl:latest
                    command:
                    - cat
                    tty: true
                  volumes:
                  - name: maven-cache
                    persistentVolumeClaim:
                      claimName: maven-cache-pvc
                  - name: docker-socket
                    hostPath:
                      path: /var/run/docker.sock
'''
        }
    }
    
    environment {
        REGISTRY = 'docker.io'
        IMAGE_NAME = 'mycompany/java-app'
        BUILD_TAG = "${env.BUILD_NUMBER}"
        K8S_NAMESPACE = 'production'
    }
    
    stages {
        stage('Checkout') {
            steps {
                echo "Checking out code from branch: ${env.GIT_BRANCH}"
                checkout scm
            }
        }
        
        stage('Maven Build') {
            steps {
                container('maven') {
                    echo "Building with Maven..."
                    sh 'mvn clean package -DskipTests'
                }
            }
        }
        
        stage('Run Unit Tests') {
            steps {
                container('maven') {
                    echo "Running tests..."
                    sh 'mvn test'
                }
            }
            post {
                always {
                    container('maven') {
                        junit 'target/surefire-reports/*.xml'
                    }
                }
            }
        }
        
        stage('Build Docker Image') {
            steps {
                container('docker') {
                    echo "Building Docker image..."
                    sh """
                        docker build \
                          --build-arg VERSION=${BUILD_TAG} \
                          --label git-commit=${GIT_COMMIT} \
                          -t ${IMAGE_NAME}:${BUILD_TAG} \
                          -t ${IMAGE_NAME}:latest \
                          .
                    """
                }
            }
        }
        
        stage('Security Scan') {
            steps {
                container('docker') {
                    echo "Scanning Docker image..."
                    sh """
                        docker run --rm \
                          -v /var/run/docker.sock:/var/run/docker.sock \
                          aquasec/trivy:latest image \
                          --severity HIGH,CRITICAL \
                          --exit-code 1 \
                          --no-progress \
                          ${IMAGE_NAME}:${BUILD_TAG} || true
                    """
                }
            }
        }
        
        stage('Push to Registry') {
            when {
                branch 'main'
            }
            steps {
                container('docker') {
                    echo "Pushing to Docker Hub..."
                    withCredentials([string(credentialsId: 'docker-hub-token', variable: 'DOCKER_TOKEN')]) {
                        sh """
                            echo $DOCKER_TOKEN | docker login ${REGISTRY} -u myuser --password-stdin
                            docker push ${IMAGE_NAME}:${BUILD_TAG}
                            docker push ${IMAGE_NAME}:latest
                        """
                    }
                }
            }
        }
        
        stage('Deploy to Kubernetes') {
            when {
                branch 'main'
            }
            steps {
                container('kubectl') {
                    echo "Deploying to Kubernetes..."
                    
                    script {
                        // Create namespace if needed
                        sh "kubectl create namespace ${K8S_NAMESPACE} --dry-run=client -o yaml | kubectl apply -f -"
                        
                        // Apply deployment
                        sh """
                            kubectl set image deployment/java-app \
                                java-app=${IMAGE_NAME}:${BUILD_TAG} \
                                -n ${K8S_NAMESPACE}
                        """
                        
                        // Wait for rollout
                        sh """
                            kubectl rollout status deployment/java-app \
                                -n ${K8S_NAMESPACE} \
                                --timeout=5m
                        """
                    }
                }
            }
        }
        
        stage('Health Check') {
            when {
                branch 'main'
            }
            steps {
                container('kubectl') {
                    echo "Performing health check..."
                    sh """
                        sleep 10
                        kubectl get pods -n ${K8S_NAMESPACE}
                        kubectl get svc java-app -n ${K8S_NAMESPACE}
                    """
                }
            }
        }
    }
    
    post {
        always {
            echo "Cleaning up workspace..."
            cleanWs deleteDirs: true
        }
        
        success {
            echo "Build and deploy succeeded!"
            slackSend channel: '#deployments',
                      color: 'good',
                      message: "✅ Deployed ${IMAGE_NAME}:${BUILD_TAG} to ${K8S_NAMESPACE}"
        }
        
        failure {
            echo "Build or deploy failed!"
            slackSend channel: '#deployments',
                      color: 'danger',
                      message: "❌ Deployment failed for ${IMAGE_NAME}:${BUILD_TAG}"
        }
    }
}
```

---

## Pipeline Breakdown

### Stage 1: Checkout
- Gets source code from Git
- Sets GIT_BRANCH, GIT_COMMIT environment variables

### Stage 2: Maven Build
- Uses Maven container
- Compiles Java code
- Packages as JAR

### Stage 3: Run Unit Tests
- Runs JUnit tests
- Publishes test results

### Stage 4: Build Docker Image
- Uses Docker-in-Docker sidecar
- Builds image with version tag and latest

### Stage 5: Security Scan
- Uses Trivy to scan for vulnerabilities
- Doesn't fail build (|| true), just reports

### Stage 6: Push to Registry
- Only runs on main branch
- Pushes version tag and latest

### Stage 7: Deploy to Kubernetes
- Uses kubectl container
- Updates deployment image
- Waits for rollout completion

### Stage 8: Health Check
- Verifies deployment
- Shows pod and service status

---

## Key Concepts Explained

### Container Switching

```groovy
// Each step runs in defaultContainer unless specified
container('maven') {
    sh 'mvn build'  // Runs in Maven container
}

container('docker') {
    sh 'docker build'  // Runs in Docker container
}
```

### Docker-in-Docker

The Docker sidecar with socket mount allows building images:

```yaml
- name: docker
  image: docker:latest
  securityContext:
    privileged: true  # Required for DinD
  volumeMounts:
  - name: docker-socket
    mountPath: /var/run/docker.sock
```

### kubectl Access

The kubectl container has access to deploy:

```yaml
- name: kubectl
  image: bitnami/kubectl:latest
```

---

## Environment Variables in K8s Pipeline

| Variable | Source | Example |
|----------|--------|---------|
| `GIT_BRANCH` | Jenkins | `main` |
| `GIT_COMMIT` | Jenkins | `abc123...` |
| `BUILD_NUMBER` | Jenkins | `42` |
| `K8S_NAMESPACE` | Custom | `production` |
| `IMAGE_NAME` | Custom | `mycompany/app` |

---

## Common Issues

### Pod Not Starting

```
Reason: ImagePullBackOff
```

**Solution**: Check image name and registry credentials

### kubectl Not Found

```
sh: kubectl: not found
```

**Solution**: Ensure kubectl container is defined in pod template

### Docker Build Fails

```
docker: cannot connect to Docker daemon
```

**Solution**: Check Docker socket is mounted correctly

---

## Next Steps

- **[Security Realm Setup](04-security-and-rbac/01-security-realm-setup.md)** - Configure authentication
- **[Role-Based Access Control](04-security-and-rbac/02-role-based-access-control.md)** - Set up permissions
- **[Script Approval](04-security-and-rbac/03-script-approval-and-sandbox.md)** - Manage pipeline security
