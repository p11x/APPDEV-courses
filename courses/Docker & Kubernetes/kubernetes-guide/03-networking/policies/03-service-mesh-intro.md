# Service Mesh Introduction

## Overview

A service mesh is a dedicated infrastructure layer that provides observability, traffic management, and security features for microservices beyond what Kubernetes networking offers. It handles service-to-service communication, providing mTLS encryption, detailed metrics, and sophisticated routing rules. Popular options include Istio and Linkerd.

## Prerequisites

- Understanding of Kubernetes networking
- Knowledge of microservices architecture
- Familiarity with Ingress and network policies

## Core Concepts

### What a Service Mesh Adds

Beyond native Kubernetes networking, a service mesh provides:

1. **Mutual TLS (mTLS)**: Automatic encryption between all services
2. **Traffic Management**: Advanced routing, canary deployments, traffic splitting
3. **Observability**: Distributed tracing, detailed metrics, service dependency maps
4. **Security**: Fine-grained access control, authentication policies
5. **Resilience**: Retries, timeouts, circuit breakers

### Sidecar Proxy Model

Service meshes use sidecar proxies (typically Envoy):
- Each pod gets an additional container (sidecar)
- All traffic goes through the sidecar
- Sidecar handles mTLS, metrics, routing
- Application code doesn't need changes

```
┌─────────────────────────────────────┐
│              Pod                     │
│  ┌─────────────┐  ┌─────────────┐  │
│  │  App       │  │  Sidecar    │  │
│  │  Container │◄─┤  (Envoy)    │  │
│  └─────────────┘  └──────┬──────┘  │
└────────────────────────────┼──────────┘
                            │
                    Network Layer
```

### Istio vs Linkerd

| Feature | Istio | Linkerd |
|---------|-------|--------|
| Complexity | Higher | Lower |
| Resource usage | Higher | Lower |
| Installation | More involved | Simpler |
| Features | Very rich | Focused |
| Maturity | Very mature | Mature |
| Learning curve | Steeper | Gentler |

### When You Need a Service Mesh

- **mTLS everywhere**: Encryption without code changes
- **Traffic splitting**: Canary releases, A/B testing
- **Detailed observability**: Distributed tracing across services
- **Fine-grained policies**: Service-to-service auth
- **When NOT needed**: Simple apps, low security requirements, limited resources

## Step-by-Step Examples

### Installing Istio

```bash
# Download istioctl (Istio's CLI tool)
# Use 1.22+ for latest features
curl -L https://istio.io/downloadIstio | sh -

# Add to PATH
export PATH=$PATH:$HOME/istio-1.22.0/bin

# Install Istio with default profile
# namespace=istio-system creates the control plane namespace
# profile=default enables core features
istioctl install --set profile=default -y

# Wait for Istio components to be ready
# This includes istiod (control plane) and ingress/egress gateways
kubectl wait --for=condition=ready pod -l app=istiod -n istio-system --timeout=300s

# Enable automatic sidecar injection for a namespace
# All pods created in this namespace will get Envoy sidecar
kubectl label namespace default istio-injection=enabled
```

### Installing Linkerd

```bash
# Install Linkerd CLI
# edge or stable channel available
curl -sL https://run.linkerd.io/install | sh

# Add to PATH
export PATH=$PATH:$HOME/.linkerd2/bin

# Install Linkerd control plane
# This creates linkerd namespace and deploys control plane components
linkerd install | kubectl apply -f -

# Wait for control plane to be ready
linkerd check --pre                                  # Pre-install check
linkerd install | kubectl apply -f -                # Install
linkerd check                                       # Verify installation

# Enable automatic proxy injection
# Annotates namespace so pods get Linkerd proxy
kubectl annotate namespace default linkerd.io/inject=enabled
```

### Deploying with Service Mesh

```yaml
# After enabling istio-injection or linkerd.io/inject
# Pods automatically get sidecar proxy
apiVersion: apps/v1
kind: Deployment
metadata:
  name: myapp
spec:
  replicas: 3
  selector:
    matchLabels:
      app: myapp
  template:
    metadata:
      labels:
        app: myapp
      # No need to add sidecar manually - mesh injects it
    spec:
      containers:
      - name: myapp
        image: myapp:v1.0
        ports:
        - containerPort: 8080
```

### Traffic Splitting Example (Istio)

```yaml
# VirtualService for traffic splitting
# Routes 90% to v1, 10% to canary
apiVersion: networking.istio.io/v1beta1
kind: VirtualService
metadata:
  name: myapp-vsvc
spec:
  hosts:
  - myapp
  http:
  - route:
    - destination:
        host: myapp
        subset: v1
      weight: 90
    - destination:
        host: myapp
        subset: canary
      weight: 10
---
# DestinationRule defines subsets (versions)
apiVersion: networking.istio.io/v1beta1
kind: DestinationRule
metadata:
  name: myapp-dest
spec:
  host: myapp
  subsets:
  - name: v1
    labels:
      version: v1.0
  - name: canary
    labels:
      version: canary
```

### Viewing Mesh Metrics

```bash
# Istio: View service graph
# Opens web UI showing service relationships
istioctl dashboard kiali

# Linkerd: View service map
# Opens web UI showing all services and their connections
linkerd viz dashboard

# Get metrics for a specific service
# Shows request rate, success rate, latency
istioctl dashboard prometheus

# Linkerd: top command shows real-time traffic
linkerd viz top -n default deploy
```

## Gotchas for Docker Users

- **Two-layer networking**: Sidecar adds hop between app and network
- **Resource overhead**: Each pod runs additional proxy container
- **Complexity**: Adds operational complexity beyond basic Kubernetes
- **Not for all traffic**: Only handles service-to-service within cluster
- **Debugging challenge**: Network issues become more complex to diagnose

## Common Mistakes

- **Installing on small clusters**: Sidecar overhead is significant
- **Enabling everywhere**: Not all apps need mesh features
- **Forgetting to enable namespace**: Pods won't get sidecars without injection
- **Not understanding injection**: Sidecar injection is automatic but can be opt-out
- **Ignoring mTLS**: Service mesh enables mTLS by default, some apps may have issues

## Quick Reference

| Component | Istio | Linkerd |
|-----------|-------|---------|
| Control Plane | istiod | linkerd-controller |
| Data Plane | Envoy | Linkerd2-proxy |
| UI | Kiali | Linkerd Web |
| CLI | istioctl | linkerd |
| Default Port | 15000 | 4191 |

| Feature | How to Enable |
|---------|---------------|
| mTLS | Automatic with mesh |
| Traffic Splitting | VirtualService (Istio) / ServiceProfile (Linkerd) |
| Tracing | Configure tracing collector |
| Metrics | Built into sidecar |

## What's Next

Continue to [PersistentVolumes and PVCs](./../../storage/persistent-volumes/01-pv-and-pvc.md) to learn about persistent storage.
