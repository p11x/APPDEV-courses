# Rolling Updates

## Overview

Rolling updates allow you to update Deployment pod templates gradually, with zero downtime. Kubernetes ensures that during an update, enough pods are available to serve requests while new pods are being created. This is essential for production deployments.

## How It Works

1. New ReplicaSet created with updated spec
2. Pods scaled up in new ReplicaSet
3. Pods scaled down in old ReplicaSet
4. Continues until all pods are updated

## Configuration

```yaml
spec:
  replicas: 3
  strategy:
    type: RollingUpdate
    rollingUpdate:
      maxSurge: 1        # Extra pods during update
      maxUnavailable: 0  # Always maintain full capacity
```

## Commands

```bash
# Update deployment
kubectl set image deployment/myapp myapp=myapp:v2

# Check rollout status
kubectl rollout status deployment/myapp

# View rollout history
kubectl rollout history deployment/myapp

# Pause rollout
kubectl rollout pause deployment/myapp

# Resume rollout
kubectl rollout resume deployment/myapp
```

## What's Next

Continue to [Rollbacks](./03-rollbacks.md)
