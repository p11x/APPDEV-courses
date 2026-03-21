# Kustomization File

## Overview

The kustomization.yaml file defines how to customize Kubernetes manifests. This guide covers its structure and common fields.

## Prerequisites

- Kubernetes basics

## Core Concepts

### Basic Structure

```yaml
# kustomization.yaml
apiVersion: kustomize.config.k8s.io/v1beta1
kind: Kustomization

resources:
  - deployment.yaml
  - service.yaml

commonLabels:
  app: myapp
```

### Common Fields

- **resources**: List of manifests
- **patches**: Strategic merge patches
- **commonLabels**: Labels applied to all
- **namePrefix**: Prefix for all names

## Quick Reference

| Field | Description |
|-------|-------------|
| resources | Base manifests |
| patches | Modifications |
| commonLabels | Shared labels |

## What's Next

Continue to [Bases and Overlays](./03-bases-and-overlays.md) for environment management.
