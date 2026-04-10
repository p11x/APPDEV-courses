---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: EKS Containers
Purpose: Hands-on EKS deployment including application deployment, services, and monitoring
Difficulty: intermediate
Prerequisites: 01_Basic_EKS.md, 02_Advanced_EKS.md
RelatedFiles: 01_Basic_EKS.md, 02_Advanced_EKS.md
UseCase: Production Kubernetes deployment
CertificationExam: CKA, AWS Developer
LastUpdated: 2025
---

## 💡 WHY

Hands-on EKS implementation provides practical experience deploying and managing containerized applications.

## 📖 WHAT

### Lab: Microservices Application

```
┌──────────────┐    ┌──────────────┐    ┌──────────────┐
│   Frontend   │───►│   Backend    │───►│   Database   │
│   (React)    │    │   (Node.js)  │    │  (DynamoDB)  │
└──────────────┘    └──────────────┘    └──────────────┘
        │                   │
        └───────────────────┘
             ALB Ingress
```

## 🔧 HOW

### Module 1: Deploy Application

```bash
#!/bin/bash
# EKS Application Deployment

# Deploy frontend
kubectl apply -f - <<EOF
apiVersion: apps/v1
kind: Deployment
metadata:
  name: frontend
  namespace: production
spec:
  replicas: 3
  selector:
    matchLabels:
      app: frontend
  template:
    metadata:
      labels:
        app: frontend
    spec:
      containers:
      - name: frontend
        image: nginx:latest
        ports:
        - containerPort: 80
        resources:
          requests:
            cpu: 100m
            memory: 128Mi
          limits:
            cpu: 200m
            memory: 256Mi
EOF

# Deploy backend
kubectl apply -f - <<EOF
apiVersion: apps/v1
kind: Deployment
metadata:
  name: backend
  namespace: production
spec:
  replicas: 2
  selector:
    matchLabels:
      app: backend
  template:
    metadata:
      labels:
        app: backend
    spec:
      serviceAccountName: backend-sa
      containers:
      - name: backend
        image: node:18
        ports:
        - containerPort: 3000
        env:
        - name: DB_TABLE
          value: "orders"
EOF

# Create service
kubectl expose deployment frontend --type=LoadBalancer --port=80 -n production
kubectl expose deployment backend --type=ClusterIP --port=3000 -n production
```

### Module 2: Configure Ingress

```bash
# Deploy ALB Ingress
kubectl apply -f - <<EOF
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: main-ingress
  namespace: production
  annotations:
    kubernetes.io/ingress.class: alb
    alb.ingress.kubernetes.io/scheme: internet-facing
spec:
  rules:
  - http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: frontend
            port:
              number: 80
      - path: /api
        pathType: Prefix
        backend:
          service:
            name: backend
            port:
              number: 3000
EOF
```

### Module 3: Monitoring

```bash
# Deploy CloudWatch Container Insights
aws eks create-addon \
    --cluster-name production-cluster \
    --addon-name amazon-cloudwatch-obsidian

# Check pods
kubectl get pods -n production
kubectl get svc -n production
kubectl get ingress -n production
```

## VERIFICATION

```bash
# Check deployment status
kubectl rollout status deployment/frontend -n production
kubectl rollout status deployment/backend -n production

# Get ALB address
kubectl get ingress -n production

# Check logs
kubectl logs deployment/frontend -n production
```

## CLEANUP

```bash
kubectl delete namespace production
aws eks delete-cluster --name production-cluster
```

## 🔗 CROSS-REFERENCES

**Related**: IAM, ALB, CloudWatch