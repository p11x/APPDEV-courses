# Pod Templates in Jenkins Kubernetes

## What this covers

This guide explains how to define Pod Templates in pipelines using `podTemplate` and `containerTemplate`. You'll learn about the full pod spec, including containers, volumes, service accounts, and running as specific users.

## Prerequisites

- Kubernetes Plugin installed
- Kubernetes cluster configured
- Completed Kubernetes setup guide

---

## Basic Pod Template

```groovy
pipeline {
    agent {
        kubernetes {
            // Cloud name from Jenkins configuration
            cloud 'kubernetes'
            
            // Label for this pod
            label 'my-pod'
            
            // Default container name
            defaultContainer 'jnlp'
            
            // Pod specification in YAML
            yaml '''
                apiVersion: v1
                kind: Pod
                spec:
                  containers:
                  - name: jnlp
                    image: jenkins/inbound-agent:latest
'''
        }
    }
    
    stages {
        stage('Build') {
            steps {
                sh 'echo Hello from Kubernetes!'
            }
        }
    }
}
```

---

## Multiple Containers

```groovy
agent {
    kubernetes {
        label 'maven-build'
        defaultContainer 'maven'
        
        yaml '''
            apiVersion: v1
            kind: Pod
            spec:
              containers:
              - name: maven
                image: maven:3.9-eclipse-temurin-17
                command:
                - cat
                tty: true
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
    stage('Build with Maven') {
        steps {
            container('maven') {
                sh 'mvn clean package'
            }
        }
    }
    
    stage('Build Docker Image') {
        steps {
            container('docker') {
                sh 'docker build -t myapp .'
            }
        }
    }
}
```

---

## Container Template Options

```groovy
yaml '''
    apiVersion: v1
    kind: Pod
    spec:
      containers:
      - name: builder
        image: node:20
        imagePullPolicy: Always  # Always pull latest
        command: []              # Override entrypoint
        args: []                 # Container arguments
        workingDir: /workspace   # Working directory
        tty: true                # Allocate TTY
        envVars:                  # Environment variables
        - name: NODE_ENV
          value: production
        resources:                # Resource limits
          limits:
            cpu: "1"
            memory: "1Gi"
          requests:
            cpu: "500m"
            memory: "512Mi"
'''
```

---

## Complete Example: Build with Maven and Docker

```groovy
pipeline {
    agent {
        kubernetes {
            cloud 'kubernetes'
            label 'maven-docker'
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
                  volumes:
                  - name: maven-cache
                    persistentVolumeClaim:
                      claimName: maven-pvc
                  - name: docker-socket
                    hostPath:
                      path: /var/run/docker.sock
'''
        }
    }
    
    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        
        stage('Build') {
            steps {
                container('maven') {
                    sh 'mvn clean package -DskipTests'
                }
            }
        }
        
        stage('Build Docker') {
            steps {
                container('docker') {
                    sh 'docker build -t myapp:$BUILD_NUMBER .'
                }
            }
        }
        
        stage('Push Docker') {
            steps {
                container('docker') {
                    sh 'docker push myapp:$BUILD_NUMBER'
                }
            }
        }
    }
}
```

---

## Pod Template Reference

### Cloud Options

| Option | Description |
|--------|-------------|
| `cloud` | Kubernetes cloud name from config |
| `label` | Pod label for matching |
| `defaultContainer` | Default container for steps |
| `yaml` | Pod spec YAML |
| `workspaceVolume` | Workspace volume type |

### Container Options

| Option | Description |
|--------|-------------|
| `name` | Container name |
| `image` | Docker image |
| `command` | Entrypoint command |
| `args` | Container arguments |
| `tty` | Allocate TTY |
| `workingDir` | Working directory |
| `envVars` | Environment variables |
| `resources` | CPU/memory limits |
| `imagePullPolicy` | Pull policy (Always, IfNotPresent) |

### Volume Types

```groovy
// Host path volume
volumes:
- name: data
  hostPath:
    path: /path/on/host

// PVC volume
volumes:
- name: data
  persistentVolumeClaim:
    claimName: my-pvc

// ConfigMap volume
volumes:
- name: config
  configMap:
    name: my-configmap

// Secret volume
volumes:
- name: secrets
  secret:
    secretName: my-secret
```

---

## Running as Non-Root

```groovy
yaml '''
    apiVersion: v1
    kind: Pod
    spec:
      securityContext:
        runAsUser: 1000
        fsGroup: 1000
      containers:
      - name: builder
        image: node:20
        command:
        - cat
        tty: true
'''
```

---

## Service Account

```groovy
yaml '''
    apiVersion: v1
    kind: Pod
    spec:
      serviceAccountName: jenkins-deploy  # Use specific service account
      containers:
      - name: builder
        image: kubectl:latest
        command:
        - cat
        tty: true
'''
```

---

## Common Mistakes

### Forgetting defaultContainer

```groovy
// ❌ Steps might run in wrong container
agent {
    kubernetes {
        yaml '''
            containers:
            - name: maven
              image: maven:3.9
'''
    }
}

// ✅ Specify default
agent {
    kubernetes {
        defaultContainer 'maven'
        yaml '''
            containers:
            - name: maven
              image: maven:3.9
'''
    }
}
```

### Not Switching Containers

```groovy
// ❌ Wrong container!
steps {
    sh 'mvn build'  // Runs in jnlp, which may not have Maven!
}

// ✅ Switch to correct container
steps {
    container('maven') {
        sh 'mvn build'
    }
}
```

---

## Next Steps

- **[Pipeline with K8s Agent](03-pipeline-with-k8s-agent.md)** - Full K8s pipeline example
- **[Security Setup](04-security-and-rbac/01-security-realm-setup.md)** - Secure Jenkins
- **[Role-Based Access Control](04-security-and-rbac/02-role-based-access-control.md)** - Manage permissions
