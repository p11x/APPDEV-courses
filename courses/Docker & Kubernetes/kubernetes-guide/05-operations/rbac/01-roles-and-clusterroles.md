# Roles and ClusterRoles

## Overview

Roles and ClusterRoles define permissions within Kubernetes. They specify which actions (verbs) can be performed on which resources (like pods, services, deployments) in a namespace or across the entire cluster.

## Prerequisites

- Understanding of Kubernetes API
- kubectl basics
- Namespaces concept

## Core Concepts

### Role vs ClusterRole

- **Role**: Namespace-scoped permissions - only works within one namespace
- **ClusterRole**: Cluster-wide permissions - works across all namespaces or on cluster-scoped resources

### When to Use Each

- Use **Role** for permissions limited to one namespace
- Use **ClusterRole** for:
  - Cluster-scoped resources (nodes, PVs, namespaces)
  - Non-resource URLs (/healthz, /version)
  - Permissions that should apply to all namespaces

### API Resources

```bash
# List all API resources with their groups
# Shows: NAME, APIVERSION, NAMESPACED, KIND
kubectl api-resources -o wide

# Examples:
# pods              v1         true       Pod
# deployments       apps/v1    true       Deployment
# services          v1         true       Service
# nodes             v1         false      Node
# namespaces        v1         false      Namespace
```

### Verbs

The verbs available correspond to HTTP methods:

```bash
# Available verbs and their meanings:
# get      - read a specific resource (GET /pods/my-pod)
# list     - list resources (GET /pods)
# watch    - watch for changes (WS /pods)
# create   - create new resources (POST /pods)
# update   - update existing resource (PUT /pods/my-pod)
# patch    - partially update resource (PATCH /pods/my-pod)
# delete   - delete specific resource (DELETE /pods/my-pod)
# deletecollection - delete multiple resources (DELETE /pods)
```

## Step-by-Step Examples

### Creating a Role

```yaml
apiVersion: rbac.authorization.k8s.io/v1
kind: Role
metadata:
  name: pod-reader           # Role name, unique in namespace
  namespace: production      # Namespace where this role applies
rules:
- apiGroups: [""]           # "" = core API group (v1)
  resources: ["pods"]        # Resource types to allow
  verbs: ["get", "list", "watch"]   # Allowed actions
```

```bash
# Apply the role
kubectl apply -f role.yaml

# Verify role was created
kubectl get role pod-reader -n production
```

### Role for Multiple Resources

```yaml
apiVersion: rbac.authorization.k8s.io/v1
kind: Role
metadata:
  name: app-developer
  namespace: development
rules:
# Rule 1: Read pods and logs
- apiGroups: [""]
  resources: ["pods", "pods/log"]
  verbs: ["get", "list", "watch"]
# Rule 2: Manage deployments
- apiGroups: ["apps"]
  resources: ["deployments"]
  verbs: ["get", "list", "watch", "create", "update", "patch"]
# Rule 3: Read configmaps
- apiGroups: [""]
  resources: ["configmaps"]
  verbs: ["get", "list"]
```

### Creating a ClusterRole

```yaml
apiVersion: rbac.authorization.k8s.io/v1
kind: ClusterRole
metadata:
  name: node-reader          # ClusterRoles are cluster-wide
rules:
# Read node resources
- apiGroups: [""]
  resources: ["nodes"]
  verbs: ["get", "list", "watch"]
# Read node metrics
- apiGroups: [""]
  resources: ["nodes/stats"]
  verbs: ["get", "list"]
```

### ClusterRole for Aggregated Permissions

```yaml
# ClusterRoles used by Node authorizer
# Aggregated from ClusterRoleBindings with label matching
apiVersion: rbac.authorization.k8s.io/v1
kind: ClusterRole
metadata:
  name: system:node-reader
  labels:
    rbac.authorization.k8s.io/aggregate-to-node: "true"
rules:
- apiGroups: [""]
  resources: ["nodes"]
  verbs: ["get", "list", "watch"]
```

### ClusterRole for Non-Resource URLs

```yaml
apiVersion: rbac.authorization.k8s.io/v1
kind: ClusterRole
metadata:
  name: health-check-reader
rules:
# Allow access to health check endpoints
- nonResourceURLs: ["/healthz", "/healthz/*", "/version", "/version/*"]
  verbs: ["get"]
```

### ClusterRole for Cluster-Scoped Resources

```yaml
apiVersion: rbac.authorization.k8s.io/v1
kind: ClusterRole
metadata:
  name: storage-admin
rules:
# Manage PersistentVolumes
- apiGroups: [""]
  resources: ["persistentvolumes"]
  verbs: ["get", "list", "watch", "create", "delete"]
# Manage StorageClasses
- apiGroups: ["storage.k8s.io"]
  resources: ["storageclasses"]
  verbs: ["get", "list", "watch", "create", "update", "delete"]
```

## Viewing Roles

```bash
# List roles in namespace
kubectl get roles -n production

# Describe role to see permissions
kubectl describe role pod-reader -n production

# List cluster roles
kubectl get clusterroles

# Describe cluster role
kubectl describe clusterrole admin

# Show rules for cluster role
kubectl get clusterrole admin -o yaml
```

## Common Patterns

### Read-Only Role

```yaml
apiVersion: rbac.authorization.k8s.io/v1
kind: Role
metadata:
  name: readonly
rules:
- apiGroups: ["*"]
  resources: ["*"]
  verbs: ["get", "list", "watch"]
```

### Full Access Role

```yaml
apiVersion: rbac.authorization.k8s.io/v1
kind: Role
metadata:
  name: admin
rules:
- apiGroups: ["*"]
  resources: ["*"]
  verbs: ["*"]  # Wildcard = all verbs
```

### Resource-Specific Role

```yaml
apiVersion: rbac.authorization.k8s.io/v1
kind: Role
metadata:
  name: secret-manager
rules:
- apiGroups: [""]
  resources: ["secrets"]
  verbs: ["get", "list", "watch", "create", "update", "patch", "delete"]
```

## Checking Permissions

```bash
# Check what permissions you have
# auth can_i shows authorization check
kubectl auth can-i get pods

# Check specific action
kubectl auth can-i create deployments

# Check for another user
kubectl auth can-i delete pods --as=john@example.com

# Check for service account
kubectl auth can-i get pods --as=system:serviceaccount:default:my-sa
```

## Gotchas for Docker Users

- **No Docker equivalent**: RBAC is Kubernetes-specific
- **Namespace-scoped**: Roles only work within their namespace
- **No deny rules**: Only positive permissions (allow list)
- **Default roles**: ClusterRoles starting with "system:" are built-in

## Common Mistakes

- **Wrong API group**: Using wrong apiGroups for resources
- **Missing verbs**: Forgetting required verbs (e.g., watch for kubectl get -w)
- **ClusterRole for namespaced**: Can use ClusterRole but Role is preferred
- **Not checking**: Always test with auth can-i

## Quick Reference

| Type | Scope | Use For |
|------|-------|---------|
| Role | Single namespace | Namespaced resources |
| ClusterRole | All namespaces | Cluster resources, aggregated |

| Verb | HTTP | Meaning |
|------|------|---------|
| get | GET | Read one |
| list | GET | Read many |
| watch | GET | Stream changes |
| create | POST | Create new |
| update | PUT | Replace |
| patch | PATCH | Modify |
| delete | DELETE | Remove one |
| deletecollection | DELETE | Remove many |

| API Group | Examples |
|-----------|----------|
| "" (core) | pods, services, configmaps |
| apps | deployments, statefulsets |
| networking.k8s.io | networkpolicies, ingresses |
| rbac.authorization.k8s.io | roles, rolebindings |

## What's Next

Continue to [RoleBindings](./02-rolebindings.md) to connect roles to users.
