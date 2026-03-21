# Network Policies

## Overview

Network Policies in Kubernetes define how pods are allowed to communicate with each other and with external endpoints. By default, Kubernetes allows all traffic between pods (permissive mode). Network Policies enforce network segmentation, implementing a "deny by default" model that restricts traffic based on defined rules.

## Prerequisites

- Understanding of Kubernetes networking basics
- Knowledge of Services and pod networking
- CNI plugin that supports NetworkPolicy (Calico, Cilium, weave, kube-router)

## Core Concepts

### Default Allow vs Default Deny

**Default Allow** (Kubernetes default):
- All pods can communicate with all other pods
- All pods can reach external IP addresses
- NetworkPolicy is optional

**Default Deny** (with NetworkPolicy):
- All traffic is blocked unless explicitly allowed
- Must define policies for each required communication path
- More secure

### Policy Structure

NetworkPolicy consists of:
- **podSelector**: Selects pods to which the policy applies
- **policyTypes**: Types of traffic to control (Ingress, Egress, or both)
- **ingress**: Rules for incoming traffic
- **egress**: Rules for outgoing traffic

### Selectors

- **podSelector**: Match pods by labels
- **namespaceSelector**: Match namespaces by labels
- **ipBlock**: Match CIDR ranges (for external traffic)

## Step-by-Step Examples

### Default Deny All Ingress

```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: default-deny-ingress
  namespace: production           # Apply to specific namespace
spec:
  podSelector: {}                # Empty selector = all pods in namespace
  policyTypes:
  - Ingress                      # Only affects ingress traffic
```

### Allow Traffic from Specific Namespace

```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: allow-from-frontend
  namespace: backend
spec:
  podSelector:
    matchLabels:
      app: api                   # Apply to pods with label app=api
  policyTypes:
  - Ingress
  ingress:
  # Allow from pods in frontend namespace
  - from:
    - namespaceSelector:
        matchLabels:
          name: frontend
    ports:
    - protocol: TCP
      port: 8080
```

### Database Access Control Example

This policy restricts a database pod to only accept traffic from the API pod:

```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: database-access-policy
  namespace: production
spec:
  podSelector:
    matchLabels:
      app: database              # Target: database pods
  policyTypes:
  - Ingress                     # Control incoming traffic only
  ingress:
  # Only allow from api pods in the same namespace
  - from:
    - podSelector:
        matchLabels:
          app: api              # Source: api pods
    ports:
    - protocol: TCP
      port: 5432               # PostgreSQL port
```

### Complete Multi-Rule Policy

```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: api-network-policy
  namespace: production
spec:
  podSelector:
    matchLabels:
      tier: backend             # Apply to backend tier pods
  policyTypes:
  - Ingress
  - Egress                      # Control both directions
  ingress:
  # Allow from frontend tier
  - from:
    - podSelector:
        matchLabels:
          tier: frontend
    ports:
    - protocol: TCP
      port: 8080
  # Allow from ingress controller namespace
  - from:
    - namespaceSelector:
        matchLabels:
          name: ingress-nginx
    ports:
    - protocol: TCP
      port: 8080
  egress:
  # Allow DNS
  - to:
    - namespaceSelector: {}
      podSelector:
        matchLabels:
          k8s-app: kube-dns
    ports:
    - protocol: UDP
      port: 53
  # Allow to database
  - to:
    - podSelector:
        matchLabels:
          app: database
    ports:
    - protocol: TCP
      port: 5432
```

### Egress Control Example

```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: restrict-egress-policy
spec:
  podSelector:
    matchLabels:
      app: restricted-app
  policyTypes:
  - Egress
  egress:
  # Allow DNS resolution
  - to:
    - namespaceSelector:
        matchLabels:
          kubernetes.io/metadata.name: kube-system
      podSelector:
        matchLabels:
          k8s-app: kube-dns
    ports:
    - protocol: UDP
      port: 53
  # Allow only to specific external service
  - to:
    - ipBlock:
        cidr: 10.0.0.0/8        # Internal network only
    ports:
    - protocol: TCP
      port: 443
```

### Managing Network Policies

```bash
# Create network policy
kubectl apply -f network-policy.yaml

# List network policies
# Shows policies in each namespace
kubectl get networkpolicy -A

# Describe for detailed rules
# Shows: pod selectors, ingress rules, egress rules
kubectl describe networkpolicy database-access-policy -n production

# Delete network policy
kubectl delete networkpolicy database-access-policy -n production
```

## Gotchas for Docker Users

- **No default isolation**: Unlike Docker networks which require explicit connection, Kubernetes defaults to allow-all
- **Explicit rules required**: Every communication path must be explicitly allowed
- **CNI dependency**: NetworkPolicy only works with compatible CNI plugins (Calico, Cilium, etc.)
- **Namespace isolation**: Policies apply within namespace unless namespaceSelector is used
- **External traffic**: ipBlock required for external/CIDR-based rules

## Common Mistakes

- **Not testing policies**: Breaking changes may not be obvious until deployment fails
- **Forgetting egress**: NetworkPolicy defaults to Ingress-only unless policyTypes includes Egress
- **Wrong pod selectors**: Labels must exactly match for policy to apply
- **Not applying to namespace**: Policies only affect pods in the same namespace unless namespaceSelector used
- **CNI not configured**: Policy has no effect without CNI that supports it

## Quick Reference

| Field | Description |
|-------|-------------|
| podSelector | Pods to which policy applies |
| namespaceSelector | Namespaces to match |
| ipBlock | CIDR ranges (for external) |
| policyTypes | Ingress, Egress, or both |
| from | Sources of traffic (Ingress) |
| to | Destinations of traffic (Egress) |

| CNI Plugin | NetworkPolicy Support |
|------------|----------------------|
| Calico | Full |
| Cilium | Full |
| Weave | Full |
| kube-router | Full |
| Flannel | Limited |
| AWS VPC CNI | No |
| Azure CNI | No |

## What's Next

Continue to [DNS in Kubernetes](./02-dns-in-kubernetes.md) to learn how service discovery works.
