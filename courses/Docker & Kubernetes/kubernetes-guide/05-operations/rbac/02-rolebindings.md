# RoleBindings and ClusterRoleBindings

## Overview

RoleBindings and ClusterRoleBindings connect roles (permissions) to users, groups, or service accounts. They are the "assignment" that grants subjects the permissions defined in a role. Without a binding, a role does nothing.

## Prerequisites

- Understanding of Roles and ClusterRoles
- Basic kubectl knowledge
- Users/groups concept in Kubernetes

## Core Concepts

### Binding Types

- **RoleBinding**: Links a Role to subjects within a namespace
- **ClusterRoleBinding**: Links a ClusterRole to subjects across all namespaces

### Subjects

The entities that receive permissions:

- **Users**: Regular users (managed externally via OIDC/LDAP)
- **Groups**: Collections of users
- **Service Accounts**: Pod identity for applications

## Step-by-Step Examples

### RoleBinding - User

```yaml
apiVersion: rbac.authorization.k8s.io/v1
kind: RoleBinding
metadata:
  name: john-pod-reader         # Unique name in namespace
  namespace: production
subjects:
# Subject type: User
- kind: User
  name: john@example.com       # User identity
  apiGroup: rbac.authorization.k8s.io
roleRef:
  kind: Role                   # References a Role (not ClusterRole)
  name: pod-reader             # Must exist in same namespace
  apiGroup: rbac.authorization.k8s.io
```

```bash
# Apply the rolebinding
kubectl apply -f rolebinding.yaml

# Verify
kubectl get rolebinding -n production
```

### RoleBinding - Group

```yaml
apiVersion: rbac.authorization.k8s.io/v1
kind: RoleBinding
metadata:
  name: developers-pod-access
  namespace: development
subjects:
# Subject type: Group
- kind: Group
  name: developers              # Group name
  apiGroup: rbac.authorization.k8s.io
- kind: Group
  name: qa-team
  apiGroup: rbac.authorization.k8s.io
roleRef:
  kind: Role
  name: pod-reader
  apiGroup: rbac.authorization.k8s.io
```

### RoleBinding - Service Account

```yaml
apiVersion: rbac.authorization.k8s.io/v1
kind: RoleBinding
metadata:
  name: myapp-pod-access
  namespace: production
subjects:
# Subject type: ServiceAccount
- kind: ServiceAccount
  name: myapp-sa               # SA name (not prefixed with system:)
  namespace: production        # SA namespace (must match or use system:default)
- kind: ServiceAccount
  name: default                # Default SA in namespace
  namespace: production
roleRef:
  kind: Role
  name: app-developer
  apiGroup: rbac.authorization.k8s.io
```

### RoleBinding - Referencing ClusterRole

```yaml
# Common pattern: ClusterRole with RoleBinding
# Grants cluster-wide permissions to specific namespace

apiVersion: rbac.authorization.k8s.io/v1
kind: RoleBinding
metadata:
  name: dev-view-all
  namespace: development
subjects:
- kind: Group
  name: developers
  apiGroup: rbac.authorization.k8s.io
roleRef:
  kind: ClusterRole           # References cluster-scoped role
  name: view                  # Built-in view role
  apiGroup: rbac.authorization.k8s.io
```

### ClusterRoleBinding - Cluster-Wide

```yaml
apiVersion: rbac.authorization.k8s.io/v1
kind: ClusterRoleBinding
metadata:
  name: admin-cluster-access
subjects:
# Admin user gets cluster-admin permissions
- kind: User
  name: admin@example.com
  apiGroup: rbac.authorization.k8s.io
roleRef:
  kind: ClusterRole
  name: cluster-admin         # Full cluster access
  apiGroup: rbac.authorization.k8s.io
```

### ClusterRoleBinding - Multiple Subjects

```yaml
apiVersion: rbac.authorization.k8s.io/v1
kind: ClusterRoleBinding
metadata:
  name: monitoring-reader
subjects:
# User
- kind: User
  name: prometheus@example.com
  apiGroup: rbac.authorization.k8s.io
# Service Account
- kind: ServiceAccount
  name: prometheus
  namespace: monitoring
# Group
- kind: Group
  name: ops-team
  apiGroup: rbac.authorization.k8s.io
roleRef:
  kind: ClusterRole
  name: system:prometheus     # Custom cluster role for metrics
  apiGroup: rbac.authorization.k8s.io
```

## Built-in Roles

### Cluster-Level

```bash
# Admin - full access to namespace
# Built-in ClusterRole (not namespace-scoped but grants namespace admin)
kubectl get clusterrole admin -o yaml

# View - read-only to most resources
kubectl get clusterrole view -o yaml

# Edit - modify resources, not secrets
kubectl get clusterrole edit -o yaml

# Cluster-admin - superuser
kubectl get clusterrole cluster-admin -o yaml
```

### Using Built-in Roles

```yaml
# Grant view access to a user
apiVersion: rbac.authorization.k8s.io/v1
kind: RoleBinding
metadata:
  name: john-viewer
  namespace: production
subjects:
- kind: User
  name: john@example.com
  apiGroup: rbac.authorization.k8s.io
roleRef:
  kind: ClusterRole           # Using cluster-scoped role
  name: view                  # Built-in role
  apiGroup: rbac.authorization.k8s.io
```

## Common Patterns

### Granting Namespace Admin

```yaml
# Give user admin to their namespace
apiVersion: rbac.authorization.k8s.io/v1
kind: RoleBinding
metadata:
  name: user-namespace-admin
  namespace: johns-namespace
subjects:
- kind: User
  name: john@example.com
  apiGroup: rbac.authorization.k8s.io
roleRef:
  kind: ClusterRole
  name: admin                 # ClusterRole admin has namespace admin
  apiGroup: rbac.authorization.k8s.io
```

### Granting Pod Access via Service Account

```yaml
# Pod in namespace A accessing secrets in namespace B
apiVersion: rbac.authorization.k8s.io/v1
kind: RoleBinding
metadata:
  name: cross-namespace-access
  namespace: production
subjects:
- kind: ServiceAccount
  name: myapp
  namespace: production
roleRef:
  kind: ClusterRole
  name: secret-reader         # Custom clusterrole to read secrets
  apiGroup: rbac.authorization.k8s.io
```

## Viewing Bindings

```bash
# List role bindings in namespace
kubectl get rolebinding -n production

# Describe role binding
kubectl describe rolebinding john-pod-reader -n production

# List cluster role bindings
kubectl get clusterrolebinding

# Describe cluster role binding
kubectl describe clusterrolebinding admin-cluster-access
```

## Subject Reference Format

```yaml
# User
- kind: User
  name: username
  apiGroup: rbac.authorization.k8s.io

# Group
- kind: Group
  name: groupname
  apiGroup: rbac.authorization.k8s.io

# Service Account (in same namespace)
- kind: ServiceAccount
  name: servicename
  namespace: namespace

# Service Account (cross namespace)
- kind: ServiceAccount
  name: servicename
  namespace: other-namespace
```

## Gotchas for Docker Users

- **No direct grant**: Must use bindings, not direct user assignment
- **Immutable**: Delete and recreate to change binding
- **Cascading**: ClusterRoleBindings affect all namespaces
- **Audit**: Check bindings to see who has access

## Common Mistakes

- **Wrong namespace**: RoleBinding must be in same namespace as Role
- **Forgetting subjects**: Empty subjects = no one gets permissions
- **Using ClusterRole in wrong binding**: RoleBinding can reference ClusterRole
- **ServiceAccount namespace**: Must specify namespace for SA

## Quick Reference

| Binding Type | Role Type | Scope |
|--------------|------------|-------|
| RoleBinding | Role | Single namespace |
| RoleBinding | ClusterRole | Single namespace |
| ClusterRoleBinding | ClusterRole | All namespaces |

| Subject Kind | Format |
|--------------|--------|
| User | name: user@example.com |
| Group | name: group-name |
| ServiceAccount | name: sa-name, namespace: ns |

| Built-in ClusterRole | Permissions |
|----------------------|-------------|
| admin | Full namespace access |
| edit | Modify most resources |
| view | Read-only (no secrets) |
| cluster-admin | Superuser |

## What's Next

Continue to [Service Accounts](./03-service-accounts.md) for pod identity.
