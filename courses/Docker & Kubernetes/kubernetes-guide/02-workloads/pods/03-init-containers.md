# Init Containers

## Overview

Init containers are specialized containers that run before the main application containers start. They perform initialization tasks like downloading dependencies, setting up configuration, or waiting for services to become available. Init containers run to completion and must succeed before app containers start.

## Prerequisites

- Understanding of Pods

## Example

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: myapp-pod
spec:
  initContainers:
  - name: init-myservice
    image: busybox:1.36
    command: ['sh', '-c', 'echo Initializing...']
  containers:
  - name: myapp-container
    image: nginx:1.25
```

## Key Points

- Run before main containers
- Must complete successfully
- Can contain utilities not in app image
- Run sequentially

## What's Next

Continue to [Creating Deployments](../deployments/01-creating-deployments.md)
