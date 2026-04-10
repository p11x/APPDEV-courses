---
Category: DevOps
Subcategory: Orchestration
Concept: Kubernetes Basics
Purpose: Understanding Kubernetes container orchestration
Difficulty: beginner
Prerequisites: Docker Basics
RelatedFiles: 02_Advanced_Kubernetes.md
UseCase: Container orchestration in production
CertificationExam: CKA (Kubernetes Administrator)
LastUpdated: 2025
---

## WHY

Kubernetes is the standard for container orchestration in production environments.

## WHAT

### Kubernetes Core Concepts

**Pod**: Smallest deployable unit

**Deployment**: Manages pod replicas

**Service**: Network abstraction

**Ingress**: HTTP routing

## HOW

### Example: Deployment

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: myapp
spec:
  replicas: 3
  selector:
    matchLabels:
      app: myapp
  template:
    metadata:
      labels:
        app: myapp
    spec:
      containers:
      - name: myapp
        image: myapp:latest
        ports:
        - containerPort: 80
```

### Example: Service

```yaml
apiVersion: v1
kind: Service
metadata:
  name: myapp
spec:
  type: LoadBalancer
  selector:
    app: myapp
  ports:
  - port: 80
    targetPort: 80
```

## DEPLOYMENT

```bash
# Apply configuration
kubectl apply -f deployment.yaml

# Check status
kubectl get pods
kubectl get services
```

## CROSS-REFERENCES

### Related Services

- EKS: AWS Kubernetes
- AKS: Azure Kubernetes
- GKE: Google Kubernetes