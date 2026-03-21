# Service Accounts

## Overview

Service accounts provide an identity for pods to authenticate with the Kubernetes API. Unlike regular user accounts which are for humans, service accounts are used by applications, scripts, and controllers to access the API securely.

## Prerequisites

- Understanding of RBAC (Roles, RoleBindings)
- Basic kubectl knowledge

## Core Concepts

### What Are Service Accounts

- **ServiceAccount**: Kubernetes resource that provides identity for pods
- Each namespace has a default service account
- Pods use service account credentials to authenticate to the API

### Service Account Tokens

When a service account is created, Kubernetes automatically generates a token stored as a secret:

```yaml
# Token is stored as secret
# Secret name: sa-name-token-xxxxx
apiVersion: v1
kind: Secret
metadata:
  name: myapp-sa-token-abc123
  annotations:
    kubernetes.io/service-account.name: myapp-sa
type: kubernetes.io/service-account-token
```

### Token Usage

```bash
# Token mounted as volume in pod
# Default location: /var/run/secrets/kubernetes.io/serviceaccount/

# Files available:
# /var/run/secrets/kubernetes.io/serviceaccount/namespace
# /var/run/secrets/kubernetes.io/serviceaccount/token
# /var/run/secrets/kubernetes.io/serviceaccount/ca.crt
```

## Step-by-Step Examples

### Creating a Service Account

```bash
# Create service account using kubectl
# -n specifies namespace
kubectl create serviceaccount myapp-sa -n production

# Verify creation
kubectl get serviceaccount myapp-sa -n production

# Check auto-created secret
kubectl get secret -n production | grep myapp-sa
```

### Service Account YAML

```yaml
apiVersion: v1
kind: ServiceAccount
metadata:
  name: myapp-sa              # Unique name in namespace
  namespace: production       # Namespace scope
  annotations:
    description: "Service account for myapp"  # Optional
  labels:
    app: myapp                # Optional labels
```

### Using Service Account in Pod

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: myapp-pod
  namespace: production
spec:
  # Specify service account
  # Without this, uses default service account
  serviceAccountName: myapp-sa
  
  containers:
  - name: app
    image: myapp:1.0
    command: ["/bin/sh", "-c"]
    args:
    # Use the mounted token to call Kubernetes API
    - |
      # Token is at /var/run/secrets/kubernetes.io/serviceaccount/token
      TOKEN=$(cat /var/run/secrets/kubernetes.io/serviceaccount/token)
      NAMESPACE=$(cat /var/run/secrets/kubernetes.io/serviceaccount/namespace)
      curl -H "Authorization: Bearer $TOKEN" \
        https://kubernetes.default.svc/api/v1/namespaces/$NAMESPACE/pods
```

### Using Service Account with RBAC

```yaml
# Step 1: Create ServiceAccount
apiVersion: v1
kind: ServiceAccount
metadata:
  name: cicd-runner
  namespace: ci-cd
---
# Step 2: Create Role for pod listing
apiVersion: rbac.authorization.k8s.io/v1
kind: Role
metadata:
  name: pod-reader
  namespace: production
rules:
- apiGroups: [""]
  resources: ["pods", "pods/log"]
  verbs: ["get", "list", "watch"]
---
# Step 3: Bind Role to ServiceAccount
apiVersion: rbac.authorization.k8s.io/v1
kind: RoleBinding
metadata:
  name: cicd-pod-reader
  namespace: production
subjects:
- kind: ServiceAccount
  name: cicd-runner
  namespace: ci-cd          # Different namespace - must specify
roleRef:
  kind: Role
  name: pod-reader
  apiGroup: rbac.authorization.k8s.io
```

### Programmatic API Access

```yaml
# Python example using service account
# pip install kubernetes

from kubernetes import client, config

# Load in-cluster config (uses service account token)
config.load_incluster_config()

# Or load from file (for local testing)
# config.load_kube_config()

v1 = client.CoreV1Api()

# List pods using service account credentials
pods = v1.list_pod_for_all_namespaces()
for pod in pods.items:
    print(f"{pod.metadata.namespace}: {pod.metadata.name}")
```

### Image Pull Secret with Service Account

```yaml
# Create secret for private registry
kubectl create secret docker-registry myregistry-secret \
  --docker-server=registry.example.com \
  --docker-username=user \
  --docker-password=pass \
  -n production

# Link to service account
apiVersion: v1
kind: ServiceAccount
metadata:
  name: myapp-sa
  namespace: production
imagePullSecrets:
- name: myregistry-secret    # Pods can pull from private registry
```

### Custom Token Secret (Manual)

```yaml
# Create service account without auto-generated token
apiVersion: v1
kind: ServiceAccount
metadata:
  name: myapp-sa
  namespace: production
automountServiceAccountToken: false
---
# Manually create secret with token
apiVersion: v1
kind: Secret
metadata:
  name: myapp-custom-token
  annotations:
    kubernetes.io/service-account.name: myapp-sa
type: kubernetes.io/service-account-token
data:
  token: <base64-encoded-token>
  ca.crt: <base64-encoded-ca>
---
# Reference in pod
apiVersion: v1
kind: Pod
metadata:
  name: myapp-pod
spec:
  serviceAccountName: myapp-sa
  volumes:
  - name: token
    secret:
      secretName: myapp-custom-token
  containers:
  - name: app
    image: myapp:1.0
    volumeMounts:
    - name: token
      mountPath: "/custom/token"
      readOnly: true
```

## Managing Service Accounts

```bash
# List service accounts
kubectl get serviceaccount -n production

# Describe service account
kubectl describe serviceaccount myapp-sa -n production

# Check what secrets are used
kubectl get serviceaccount myapp-sa -n production -o yaml

# Delete service account (also deletes token secret)
kubectl delete serviceaccount myapp-sa -n production
```

## Disabling Auto-Mounting

```yaml
# Disable for specific pod
apiVersion: v1
kind: Pod
metadata:
  name: no-token-pod
spec:
  # Disable service account token mount
  automountServiceAccountToken: false
  containers:
  - name: app
    image: myapp:1.0

# Disable for entire service account
apiVersion: v1
kind: ServiceAccount
metadata:
  name: no-automount-sa
automountServiceAccountToken: false
```

## Use Cases

### CI/CD Pipeline

```yaml
# CI/CD runner service account with deployment permissions
apiVersion: v1
kind: ServiceAccount
metadata:
  name: gitlab-runner
  namespace: ci
---
apiVersion: rbac.authorization.k8s.io/v1
kind: Role
metadata:
  name: gitlab-deployer
rules:
- apiGroups: ["apps"]
  resources: ["deployments"]
  verbs: ["get", "list", "watch", "update", "patch"]
- apiGroups: [""]
  resources: ["pods"]
  verbs: ["get", "list", "watch", "delete"]
---
apiVersion: rbac.authorization.k8s.io/v1
kind: RoleBinding
metadata:
  name: gitlab-deployer-binding
subjects:
- kind: ServiceAccount
  name: gitlab-runner
  namespace: ci
roleRef:
  kind: Role
  name: gitlab-deployer
  apiGroup: rbac.authorization.k8s.io
```

### Metrics Collection

```yaml
# Prometheus service account
apiVersion: v1
kind: ServiceAccount
metadata:
  name: prometheus
  namespace: monitoring
---
apiVersion: rbac.authorization.k8s.io/v1
kind: ClusterRoleBinding
metadata:
  name: prometheus-cluster
subjects:
- kind: ServiceAccount
  name: prometheus
  namespace: monitoring
roleRef:
  kind: ClusterRole
  name: system:prometheus
  apiGroup: rbac.authorization.k8s.io
```

## Default Service Account

```bash
# Each namespace has a default service account
# Used when no serviceAccountName is specified

# Check default SA
kubectl get serviceaccount default -n production

# Pods automatically mount default SA token
# Unless automountServiceAccountToken: false
```

## Security Best Practices

```bash
# Principle of least privilege:
# 1. Create dedicated service accounts per application
# 2. Grant minimal RBAC permissions
# 3. Disable automount when not needed
# 4. Rotate tokens regularly
# 5. Don't use default service account for apps
```

## Gotchas for Docker Users

- **No Docker equivalent**: Service accounts are Kubernetes-specific
- **Auto-mounting**: Tokens mounted by default in every pod
- **Token lifetime**: Tokens expire (1 year default, can rotate)
- **Namespaced**: Service accounts are namespace-scoped

## Common Mistakes

- **Using default SA**: Should create dedicated service accounts
- **Over-permission**: Granting admin instead of minimal permissions
- **Missing namespace**: Forgetting namespace in RoleBinding for cross-namespace
- **Token exposure**: Tokens visible in pod logs/secrets

## Quick Reference

| Command | Description |
|---------|-------------|
| `kubectl create serviceaccount NAME` | Create SA |
| `kubectl get serviceaccount` | List SAs |
| `kubectl describe serviceaccount NAME` | Show details |

| Field | Description |
|-------|-------------|
| serviceAccountName | Pod spec field to use SA |
| automountServiceAccountToken | Enable/disable token mount |
| imagePullSecrets | Registry credentials |

| Token Location | File |
|----------------|------|
| Token | /var/run/secrets/kubernetes.io/serviceaccount/token |
| CA cert | /var/run/secrets/kubernetes.io/serviceaccount/ca.crt |
| Namespace | /var/run/secrets/kubernetes.io/serviceaccount/namespace |

## What's Next

This completes the Kubernetes guide. Review what you've learned about pods, deployments, services, storage, and RBAC. Consider exploring specific areas in more depth based on your use case.
