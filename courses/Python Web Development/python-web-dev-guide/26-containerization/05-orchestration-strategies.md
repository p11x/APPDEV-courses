# Orchestration Strategies

## What You'll Learn
- Rolling updates
- Blue-green deployment
- Canary releases

## Prerequisites
- Completed Helm charts

## Rolling Updates

```yaml
spec:
  replicas: 3
  strategy:
    type: RollingUpdate
    rollingUpdate:
      maxSurge: 1
      maxUnavailable: 0
```

## Blue-Green Deployment

```yaml
# service-blue.yaml
apiVersion: v1
kind: Service
metadata:
  name: myapp
spec:
  selector:
    version: blue
---
# service-green.yaml
apiVersion: v1
kind: Service
metadata:
  name: myapp-green
spec:
  selector:
    version: green
```

## Canary Release

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: myapp
  annotations:
    nginx.ingress.kubernetes.io/canary: "true"
    nginx.ingress.kubernetes.io/canary-weight: "10"
spec:
  rules:
  - host: myapp.example.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: myapp-canary
            port:
              number: 80
```

## Summary
- Rolling updates for gradual rollout
- Blue-green for instant switch
- Canary for gradual percentage

## Next Steps
→ Move to `27-ci-cd/`
