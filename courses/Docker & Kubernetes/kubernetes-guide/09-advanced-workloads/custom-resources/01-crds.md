# CRDs

## Overview

Custom Resource Definitions (CRDs) extend the Kubernetes API. This guide covers creating and using CRDs.

## Prerequisites

- Kubernetes basics

## Core Concepts

### CRD Example

```yaml
apiVersion: apiextensions.k8s.io/v1
kind: CustomResourceDefinition
metadata:
  name: webapps.example.com
spec:
  group: example.com
  names:
    kind: WebApp
    plural: webapps
  scope: Namespaced
  versions:
    - name: v1
      served: true
      storage: true
```

## Quick Reference

| Field | Description |
|-------|-------------|
| group | API group |
| kind | Resource type |
| scope | Namespaced/Cluster |

## What's Next

Continue to [Writing a CRD](./02-writing-a-crd.md)
