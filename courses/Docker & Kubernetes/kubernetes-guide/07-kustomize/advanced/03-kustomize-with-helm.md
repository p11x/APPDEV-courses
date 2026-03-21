# Kustomize with Helm

## Overview

Kustomize can work alongside Helm for powerful configuration management. This guide covers combining both tools.

## Prerequisites

- Helm knowledge
- Kustomize knowledge

## Step-by-Step Examples

### Using helmCharts

```yaml
# kustomization.yaml
apiVersion: kustomize.config.k8s.io/v1beta1
kind: Kustomization

helmCharts:
  - name: nginx
    releaseName: my-nginx
    namespace: default
    valuesInline:
      service:
        type: NodePort
```

### Post-Render Patching

```bash
# Render helm template then kustomize
helm template my-release chart | kubectl kustomize - | kubectl apply -f -
```

## Quick Reference

| Approach | Description |
|----------|-------------|
| helmCharts | Built-in Helm integration |
| Pipeline | Template then patch |

## What's Next

Continue to [What is ArgoCD](../../08-gitops/argocd/01-what-is-argocd.md) for GitOps.
