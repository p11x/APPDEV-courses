# Creating Deployments

## Overview

A Deployment provides declarative updates for Pods and ReplicaSets. It ensures a specified number of pod replicas are running at any time, handles rolling updates, and enables easy rollbacks. Deployments are the primary way to manage stateless applications in Kubernetes.

## Prerequisites

- Understanding of Pods

## Example

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: nginx-deployment
  labels:
    app: nginx
spec:
  replicas: 3
  selector:
    matchLabels:
      app: nginx
  template:
    metadata:
      labels:
        app: nginx
    spec:
      containers:
      - name: nginx
        image: nginx:1.25
        ports:
        - containerPort: 80
```

## Managing Deployments

```bash
# Create deployment
kubectl apply -f deployment.yaml

# Get deployments
kubectl get deployments

# Scale deployment
kubectl scale deployment nginx-deployment --replicas=5

# Update image
kubectl set image deployment/nginx-deployment nginx=nginx:1.26

# View status
kubectl rollout status deployment/nginx-deployment
```

## What's Next

Continue to [Rolling Updates](./02-rolling-updates.md)
