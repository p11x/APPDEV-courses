# Pod Basics

## Overview

A Pod is the smallest deployable unit in Kubernetes. It represents a single instance of a running process in your cluster. Pods can contain one or more containers that share storage and network. Understanding Pods is fundamental to Kubernetes, as all other workloads are built on top of them.

## Prerequisites

- Kubernetes cluster access
- kubectl installed

## Core Concepts

### What is a Pod?

A Pod:
- Is the smallest deployable unit
- Can contain one or more containers
- Shares network and storage between containers
- Has a unique IP address
- Is ephemeral (designed to be replaced)

### Pod Lifecycle

- **Pending**: Being scheduled
- **Running**: Container(s) executing
- **Succeeded**: All containers exited successfully
- **Failed**: At least one container failed
- **Unknown**: Node communication lost

## Examples

### Basic Pod

```yaml
apiVersion: v1          # Kubernetes API version
kind: Pod               # Resource type
metadata:
  name: my-pod         # Unique name within namespace
  labels:
    app: myapp         # Label for selection
spec:
  containers:
  - name: nginx        # Container name
    image: nginx:1.25  # Container image
    ports:
    - containerPort: 80  # Port container exposes
```

### Creating and Managing

```bash
# Create pod
kubectl apply -f pod.yaml

# Get pods
kubectl get pods

# Describe pod
kubectl describe pod my-pod

# View logs
kubectl logs my-pod

# Execute in pod
kubectl exec -it my-pod -- /bin/sh

# Delete pod
kubectl delete pod my-pod
```

## Common Mistakes

- **Direct pod creation**: Use Deployments for production.
- **Not understanding pod lifecycle**: Pods are ephemeral.

## What's Next

Continue to [Multi-Container Pods](./02-multi-container-pods.md)
