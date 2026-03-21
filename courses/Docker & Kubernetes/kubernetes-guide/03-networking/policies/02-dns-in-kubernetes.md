# DNS in Kubernetes

## Overview

Kubernetes provides a built-in DNS service that allows pods and services to discover each other by name rather than IP addresses. This is essential for service-to-service communication in microservices architectures. Understanding Kubernetes DNS is critical for debugging connectivity issues and configuring applications.

## Prerequisites

- Understanding of Kubernetes Services
- Basic networking knowledge (DNS, IP addresses)
- Familiarity with pods and deployments

## Core Concepts

### How DNS Works in Kubernetes

1. Every Service gets a DNS name
2. Every Pod gets a DNS name (for headless Services)
3. CoreDNS (or kube-dns) resolves these names
4. Applications can use short names or fully-qualified names

### DNS Resolution Flow

```
Pod (myapp) → CoreDNS → Service (web-service)
                ↓
         kube-system namespace
```

### Fully Qualified Domain Names (FQDN)

The full DNS name format:
```
<service-name>.<namespace>.svc.cluster.local
```

Example: `web-service.default.svc.cluster.local`

### Short Names

- `<service-name>` - Within same namespace
- `<service-name>.<namespace>` - Different namespace

## Step-by-Step Examples

### Service DNS Resolution

```bash
# Create a Service
kubectl create deployment web --image=nginx --replicas=2
kubectl expose deployment web --port=80 --name=web-service

# Get the full service details including ClusterIP
kubectl get svc web-service -o wide
# NAME          TYPE       CLUSTER-IP    PORT(S)  AGE
# web-service   ClusterIP  10.0.100.5   80/TCP   5m

# DNS is automatically created:
# FQDN: web-service.default.svc.cluster.local
# Short: web-service (from default namespace)
```

### Resolving Service Names

```bash
# Create a test pod with DNS tools
kubectl run dns-test --image=tutum/dnsutils --restart=Never -- sleep 3600

# Wait for pod to be ready
kubectl wait --for=condition=Ready pod/dns-test --timeout=60s

# Resolve service by short name
# Queries CoreDNS for web-service in default namespace
kubectl exec dns-test -- nslookup web-service

# Output shows:
# Name:   web-service.default.svc.cluster.local
# Address: 10.0.100.5

# Resolve by FQDN
# Fully qualified includes namespace
kubectl exec dns-test -- nslookup web-service.default.svc.cluster.local

# Resolve from different namespace
# First create service in another namespace
kubectl create namespace production
kubectl run web-prod --image=nginx -n production
kubectl expose deployment web-prod --port=80 -n production

# From test pod, resolve production service
kubectl exec dns-test -- nslookup web-service.production.svc.cluster.local
```

### Cross-Namespace DNS

```yaml
# Example: API service in backend namespace accessed from frontend namespace
---
apiVersion: v1
kind: Namespace
metadata:
  name: backend
---
apiVersion: v1
kind: Service
metadata:
  name: api-service
  namespace: backend
spec:
  selector:
    app: api
  ports:
  - port: 8080
    targetPort: 8080
---
apiVersion: v1
kind: Pod
metadata:
  name: frontend-pod
  namespace: frontend
spec:
  containers:
  - name: frontend
    image: myfrontend:latest
    env:
    # Short name won't work from different namespace
    - name: API_URL
      value: "http://api-service.backend.svc.cluster.local:8080"
```

### Pod DNS

For headless Services, pods get DNS names:

```yaml
apiVersion: v1
kind: Service
metadata:
  name: headless-service
spec:
  clusterIP: None                # Makes it headless
  selector:
    app: myapp
  ports:
  - port: 80
---
# Pods get DNS: <pod-ip-dashes>.<headless-service>.<namespace>.svc.cluster.local
# Example: 10-244-1-5.headless-service.default.svc.cluster.local
```

### Customizing CoreDNS

```yaml
# Edit the CoreDNS ConfigMap to add custom DNS configurations
kubectl edit configmap coredns -n kube-system

# Example: Add forward for external domain
apiVersion: v1
kind: ConfigMap
metadata:
  name: coredns
  namespace: kube-system
data:
  Corefile: |
    .:53 {
        errors
        health
        ready
        kubernetes cluster.local in 10.0.0.0/8 {
           pods insecure
           fallthrough
        }
        forward . /etc/resolv.conf
        log
        cache 30
    }
    # Custom forward for company domain
    company.internal:53 {
        forward company.internal 10.0.0.10
    }
```

### DNS Troubleshooting

```bash
# Check CoreDNS pods are running
# Should have 2+ replicas in kube-system
kubectl get pods -n kube-system -l k8s-app=kube-dns

# Check CoreDNS logs
# Shows query logs and any errors
kubectl logs -n kube-system -l k8s-app=kube-dns --tail=50

# Check service endpoints
# Without endpoints, DNS will fail
kubectl get endpoints web-service

# Test DNS resolution with nslookup
kubectl run test-dns --image=busybox:1.36 --restart=Never -- \
  nslookup kubernetes.default

# Check pod's DNS configuration
# Shows nameservers and search domains
kubectl exec test-dns -- cat /etc/resolv.conf

# Output:
# nameserver 10.96.0.10        # CoreDNS service IP
# search default.svc.cluster.local svc.cluster.local cluster.local
# options ndots:5
```

## DNS Search Domain Order

The search domains determine how short names are resolved:
```
1. default.svc.cluster.local
2. svc.cluster.local
3. cluster.local
```

So resolving `api-service` tries:
1. `api-service.default.svc.cluster.local`
2. `api-service.svc.cluster.local`
3. `api-service.cluster.local`

## Gotchas for Docker Users

- **Automatic service discovery**: Unlike Docker Compose where you manually link containers, Kubernetes DNS provides automatic discovery
- **Namespace-based resolution**: Short names only work within same namespace
- **DNS caching**: Applications may cache DNS results; Kubernetes handles this differently than Docker
- **Headless Service DNS**: Unlike Docker networks, pods get individual DNS A records
- **No automatic cleanup**: DNS records persist until Service is deleted

## Common Mistakes

- **Using ClusterIP directly**: Should use DNS name for service discovery
- **Forgetting namespace**: Cross-namespace access requires FQDN
- **Not waiting for DNS**: Newly created services take seconds to be resolvable
- **Wrong service name**: DNS names are case-insensitive but Kubernetes resources are case-sensitive
- **Pod DNS not working**: Requires headless Service, not regular Service

## Quick Reference

| Name Type | Format | Example |
|-----------|--------|---------|
| Service | `<name>.<namespace>.svc.cluster.local` | api.default.svc.cluster.local |
| Pod | `<pod-ip-dashes>.<service>.ns.svc.cluster.local` | 10-244-1-5.api.default.svc.cluster.local |
| Short | `<name>` | api |
| FQDN | `<name>.<namespace>.svc.cluster.local` | api.default.svc.cluster.local |

| Common Issue | Solution |
|-------------|----------|
| Name not resolving | Check Service exists and has endpoints |
| Wrong namespace | Use FQDN for cross-namespace |
| DNS slow | Check CoreDNS resource limits |
| Cache issues | Restart pods or clear cache |

## What's Next

Continue to [Service Mesh Intro](./03-service-mesh-intro.md) to learn about advanced traffic management beyond basic Kubernetes networking.
