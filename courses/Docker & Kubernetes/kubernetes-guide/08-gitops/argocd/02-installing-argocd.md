# Installing ArgoCD

## Overview

This guide covers installing ArgoCD in a Kubernetes cluster.

## Prerequisites

- kubectl access to cluster

## Step-by-Step Examples

### Install ArgoCD

```bash
# Create namespace
kubectl create namespace argocd

# Install ArgoCD
kubectl apply -n argocd -f https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml

# Get initial admin password
kubectl -n argocd get secret argocd-initial-admin-secret -o jsonpath="{.data.password}" | base64 -d
```

### Access ArgoCD UI

```bash
# Port forward to access UI
kubectl port-forward svc/argocd-server -n argocd 8080:443
```

## Quick Reference

| Command | Description |
|---------|-------------|
| kubectl apply -f | Install ArgoCD |
| port-forward | Access UI |

## What's Next

Continue to [Deploying an App](./03-deploying-an-app.md) for usage.
