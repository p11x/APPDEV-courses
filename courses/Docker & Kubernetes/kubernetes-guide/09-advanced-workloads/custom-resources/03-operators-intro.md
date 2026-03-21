# Operators Intro

## Overview

Operators are Kubernetes-native applications that extend the API with custom logic. They automate complex operational tasks by combining Custom Resource Definitions with a control loop that manages your applications.

## Prerequisites

- Understanding of CRDs
- Kubernetes basics
- Application deployment knowledge

## Core Concepts

### What is an Operator

An Operator is a method of packaging, deploying, and managing a Kubernetes application. It uses:

- **Custom Resource Definition (CRD)**: Extends Kubernetes API
- **Controller**: Watches the custom resources and takes action
- **Reconciliation Loop**: Continuously ensures desired state matches actual state

### How Operators Work

The Operator pattern implements a control loop:

```
┌─────────────┐     Observe      ┌─────────────┐
│   Custom    │ ───────────────▶│  Controller │
│  Resource   │                 │             │
└─────────────┘                 └──────┬──────┘
       ▲                                 │
       │     Diff/Act                    │
       └─────────────────────────────────┘
```

## Step-by-Step Examples

### Installing an Operator

```bash
# Install Prometheus Operator using Helm
# --set creates CustomResourceDefinitions automatically
helm install prometheus-operator prometheus-community/kube-prometheus-stack \
  --namespace monitoring \
  --create-namespace \
  --set prometheusOperator.create=true
```

### Using an Operator

```yaml
# Create a Prometheus resource managed by the operator
apiVersion: monitoring.coreos.com/v1
kind: Prometheus
metadata:
  name: my-prometheus
  namespace: monitoring
spec:
  replicas: 2
  retention: 15d
  storage:
    volumeClaimTemplate:
      spec:
        resources:
          requests:
            storage: 10Gi
```

## Popular Operators

| Operator | Purpose |
|----------|---------|
| Prometheus Operator | Monitoring |
| cert-manager | TLS certificates |
| CloudNativePG | PostgreSQL database |
| Strimzi | Apache Kafka |
| Rook | Ceph storage |

## Operator SDK

```bash
# Install operator-sdk
# brew install operator-sdk

# Create new operator
operator-sdk init --domain example.com --project-name myoperator

# Add CRD
operator-sdk create api --group webapp --version v1 --kind WebApp
```

## Gotchas for Docker Users

- **No Docker equivalent**: Operators are pure Kubernetes
- **Complexity**: Requires understanding of K8s internals
- **Resource usage**: Controllers consume resources

## Common Mistakes

- **Overengineering**: Don't use Operators for simple apps
- **Version mismatch**: Operator version vs K8s version
- **Missing RBAC**: Controller needs proper permissions

## Quick Reference

| Component | Description |
|-----------|-------------|
| CRD | Custom API definition |
| Controller | Reconciliation logic |
| Custom Resource | Application instance |

## What's Next

Continue to [Taints and Tolerations](./01-taints-and-tolerations.md) for scheduling.
