# Name Prefix and Labels

## Overview

Kustomize provides transformers to modify resource metadata. This guide covers namePrefix, commonLabels, and other transformers.

## Prerequisites

- Kustomize basics

## Core Concepts

### namePrefix and nameSuffix

```yaml
apiVersion: kustomize.config.k8s.io/v1beta1
kind: Kustomization
namePrefix: dev-
nameSuffix: -v1
resources:
  - deployment.yaml
```

### commonLabels

```yaml
commonLabels:
  app: myapp
  environment: production
```

### commonAnnotations

```yaml
commonAnnotations:
  description: "My application"
```

## Quick Reference

| Transformer | Effect |
|------------|--------|
| namePrefix | Adds prefix to names |
| nameSuffix | Adds suffix to names |
| commonLabels | Adds labels to all |
| commonAnnotations | Adds annotations |

## What's Next

Continue to [Config Generators](./03-config-generators.md) for automated configs.
