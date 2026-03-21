# Liveness and Readiness Probes

## Overview

Probes are health checks that Kubernetes uses to determine if your application is healthy and ready to serve traffic. They are essential for self-healing applications and ensuring that traffic is only sent to healthy pods.

## Types of Probes

1. **Liveness Probe**: Checks if container is running
2. **Readiness Probe**: Checks if container can serve traffic
3. **Startup Probe**: Checks if application started (for slow starters)

## Example

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: my-pod
spec:
  containers:
  - name: my-container
    image: myapp
    livenessProbe:
      httpGet:
        path: /health
        port: 8080
      initialDelaySeconds: 30
      periodSeconds: 10
    readinessProbe:
      httpGet:
        path: /ready
        port: 8080
      initialDelaySeconds: 5
      periodSeconds: 5
```

## Commands

```bash
# Check probe status
kubectl describe pod my-pod

# View pod events
kubectl get events --field-selector involvedObject.name=my-pod
```

## Common Mistakes

- **Too aggressive**: Can cause unnecessary restarts.
- **Missing probes**: Always add probes for production.

## What's Next

This concludes the Kubernetes guide basics. Now complete the task with a summary.
