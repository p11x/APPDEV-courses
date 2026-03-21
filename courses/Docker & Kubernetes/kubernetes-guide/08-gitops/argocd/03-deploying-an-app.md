# Deploying an App

## Overview

This guide covers creating and managing applications in ArgoCD.

## Prerequisites

- ArgoCD installed

## Step-by-Step Examples

### Create Application

```yaml
# application.yaml
apiVersion: argoproj.io/v1alpha1
kind: Application
metadata:
  name: myapp
  namespace: argocd
spec:
  project: default
  source:
    repoURL: https://github.com/myorg/myapp
    path: k8s/overlays/prod
  destination:
    server: https://kubernetes.default.svc
    namespace: production
  syncPolicy:
    automated:
      prune: true
      selfHeal: true
```

### CLI Commands

```bash
# Create application
kubectl apply -f application.yaml

# Sync manually
argocd app sync myapp

# View status
argocd app get myapp
```

## Quick Reference

| Command | Description |
|---------|-------------|
| argocd app create | Create app |
| argocd app sync | Sync app |
| argocd app get | View status |

## What's Next

Continue to [What is Flux](../flux/01-what-is-flux.md) for alternative GitOps.
