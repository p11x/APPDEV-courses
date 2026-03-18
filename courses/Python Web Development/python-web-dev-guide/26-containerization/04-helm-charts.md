# Helm Charts

## What You'll Learn
- Helm package manager
- Writing charts
- Values and templates

## Prerequisites
- Completed Kubernetes basics

## Helm Basics

```bash
# Install Helm
curl https://raw.githubusercontent.com/helm/helm/main/scripts/get-helm-3 | bash

# Create chart
helm create myapp

# Deploy
helm install myapp ./myapp

# Upgrade
helm upgrade myapp ./myapp
```

## Chart Structure

```
myapp/
├── Chart.yaml
├── values.yaml
├── templates/
│   ├── deployment.yaml
│   └── service.yaml
```

## values.yaml

```yaml
replicaCount: 3

image:
  repository: myapp
  tag: latest
  pullPolicy: IfNotPresent

service:
  type: ClusterIP
  port: 80

resources:
  limits:
    cpu: 500m
    memory: 256Mi
  requests:
    cpu: 200m
    memory: 128Mi
```

## Summary
- Helm packages Kubernetes manifests
- Use values.yaml for configuration
- Simplifies deployments

## Next Steps
→ Continue to `05-orchestration-strategies.md`
