# Kubernetes Deployment

## Concept Title and Overview

In this lesson, you'll learn how to deploy containerized applications using Kubernetes for orchestration and scaling.

## Real-World Importance and Context

Kubernetes (K8s) is the industry standard for container orchestration, enabling:
- Automated deployment and scaling
- Load balancing
- Self-healing
- Rolling updates

## Detailed Step-by-Step Explanation

### Kubernetes Concepts

```
┌─────────────────────────────────────────────────────────────────────────┐
│                 KUBERNETES CONCEPTS                                      │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  POD                                                                   │
│  ┌─────────────────────────────────────────────────────────────────┐  │
│  │ Smallest deployable unit                                          │  │
│  │ Contains one or more containers                                   │  │
│  └─────────────────────────────────────────────────────────────────┘  │
│                                                                         │
│  REPLICA SET                                                           │
│  ┌─────────────────────────────────────────────────────────────────┐  │
│  │ Ensures specified number of pods run                              │  │
│  └─────────────────────────────────────────────────────────────────┘  │
│                                                                         │
│  DEPLOYMENT                                                            │
│  ┌─────────────────────────────────────────────────────────────────┐  │
│  │ Manages ReplicaSets, provides rolling updates                    │  │
│  └─────────────────────────────────────────────────────────────────┘  │
│                                                                         │
│  SERVICE                                                               │
│  ┌─────────────────────────────────────────────────────────────────┐  │
│  │ Network abstraction, load balancing                               │  │
│  └─────────────────────────────────────────────────────────────────┘  │
│                                                                         │
│  INGRESS                                                              │
│  ┌─────────────────────────────────────────────────────────────────┐  │
│  │ External access to services                                       │  │
│  └─────────────────────────────────────────────────────────────────┘  │
│                                                                         │
│  CONFIGMAP/SECRET                                                      │
│  ┌─────────────────────────────────────────────────────────────────┐  │
│  │ Configuration data and sensitive information                      │  │
│  └─────────────────────────────────────────────────────────────────┘  │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Deployment YAML

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: task-manager-backend
spec:
  replicas: 3
  selector:
    matchLabels:
      app: task-manager-backend
  template:
    metadata:
      labels:
        app: task-manager-backend
    spec:
      containers:
      - name: backend
        image: myregistry/task-manager-backend:1.0
        ports:
        - containerPort: 8080
        env:
        - name: SPRING_PROFILES_ACTIVE
          value: "prod"
        resources:
          limits:
            memory: "512Mi"
            cpu: "500m"
---
apiVersion: v1
kind: Service
metadata:
  name: task-manager-backend
spec:
  selector:
    app: task-manager-backend
  ports:
  - port: 80
    targetPort: 8080
  type: ClusterIP
```

### Ingress YAML

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: task-manager-ingress
spec:
  rules:
  - host: taskmanager.example.com
    http:
      paths:
      - path: /api
        pathType: Prefix
        backend:
          service:
            name: task-manager-backend
            port:
              number: 80
```

---

## Summary

You've learned Kubernetes fundamentals for deploying Spring Boot applications.
