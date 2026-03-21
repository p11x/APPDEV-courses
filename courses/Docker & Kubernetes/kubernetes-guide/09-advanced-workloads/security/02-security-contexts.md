# Security Contexts

## Overview

A security context defines privilege and access control settings for a pod or container. It controls user ID, group ID, capabilities, and filesystem permissions.

## Prerequisites

- Understanding of Linux user permissions
- Basic Kubernetes pod concepts

## Core Concepts

### Pod vs Container Security Context

| Level | Scope | Settings |
|-------|-------|----------|
| Pod | All containers in pod | fsGroup, runAsUser, seLinuxOptions |
| Container | Single container | runAsNonRoot, capabilities, readOnlyRootFilesystem |

### Key Settings

| Setting | Description |
|---------|-------------|
| runAsNonRoot | Container must run as non-root user |
| runAsUser | Specific user ID to run as |
| fsGroup | Group ID for volume permissions |
| capabilities | Linux capabilities to add/drop |
| seLinuxOptions | SELinux context |

## Step-by-Step Examples

### Container Security Context

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: secure-pod
spec:
  securityContext:
    runAsNonRoot: true
    runAsUser: 1000
    fsGroup: 2000
  containers:
  - name: app
    image: nginx:1.25
    securityContext:
      # Don't run as root
      runAsNonRoot: true
      # Drop all capabilities
      capabilities:
        drop:
        - ALL
      # Make filesystem read-only
      readOnlyRootFilesystem: true
      # Prevent privilege escalation
      allowPrivilegeEscalation: false
```

### Advanced Capabilities

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: net-capable-pod
spec:
  containers:
  - name: network-tool
    image: tools:latest
    securityContext:
      capabilities:
        # Add NET_ADMIN capability
        add:
        - NET_ADMIN
        # Drop all others
        drop:
        - ALL
```

### Pod Security Context

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: web-deployment
spec:
  replicas: 3
  selector:
    matchLabels:
      app: web
  template:
    metadata:
      labels:
        app: web
    spec:
      securityContext:
        runAsNonRoot: true
        runAsUser: 1000
        runAsGroup: 1000
        fsGroup: 1000
        seLinuxOptions:
          level: "s0:c123,c456"
      containers:
      - name: web
        image: nginx:1.25
        securityContext:
          allowPrivilegeEscalation: false
          readOnlyRootFilesystem: true
```

## Gotchas for Docker Users

- **Similar to Docker**: Security contexts map to Docker's security options
- **runAsUser**: Like `docker run -u 1000`
- **capabilities**: Like `docker run --cap-add`

## Common Mistakes

- **UID mismatch**: Volume ownership issues
- **Capabilities**: Adding more than needed
- **readOnlyRootFilesystem**: Applications needing to write

## Quick Reference

| Security Context Field | Docker Equivalent |
|----------------------|-------------------|
| runAsUser | -u |
| runAsGroup | -u |
| fsGroup | --chown |
| capabilities.add | --cap-add |
| capabilities.drop | --cap-drop |
| readOnlyRootFilesystem | --read-only |

## What's Next

Continue to [Falco Runtime Security](./03-falco-runtime-security.md) for runtime threat detection.
