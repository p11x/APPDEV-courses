# Multi-Container Pods

## Overview

Multi-container pods allow you to run closely related containers together that need to share resources. Common patterns include sidecars for logging, proxies, and adapters. Containers in a pod share network and storage, enabling tight coupling.

## Prerequisites

- Understanding of Pod basics

## Use Cases

- **Sidecar**: Logging, synchronization
- **Adapter**: Format transformation
- **Ambassador**: Client-side proxy

## Example

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: web-server-with-logger
spec:
  containers:
  - name: web
    image: nginx:1.25
    ports:
    - containerPort: 80
  - name: logger
    image: logger-sidecar:latest
```

## What's Next

Continue to [Init Containers](./03-init-containers.md)
