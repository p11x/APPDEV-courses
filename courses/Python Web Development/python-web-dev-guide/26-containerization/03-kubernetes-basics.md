# Kubernetes Basics

## What You'll Learn
- K8s concepts
- Deployments
- Services

## Prerequisites
- Completed optimizing Dockerfiles

## K8s Concepts

- **Pod**: Smallest deployable unit
- **Deployment**: Manages pods
- **Service**: Network access
- **Ingress**: HTTP routing

## Deployment

```yaml
# deployment.yaml
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
        - containerPort: 8000
        resources:
          limits:
            memory: "256Mi"
            cpu: "500m"
```

## Service

```yaml
# service.yaml
apiVersion: v1
kind: Service
metadata:
  name: myapp
spec:
  selector:
    app: myapp
  ports:
  - port: 80
    targetPort: 8000
  type: ClusterIP
```

## Summary
- Kubernetes orchestrates containers
- Use Deployments for management
- Services for networking

## Next Steps
→ Continue to `04-helm-charts.md`
