# Linkerd Setup

## What You'll Learn

- What Linkerd is and how it compares to Istio
- How to install Linkerd
- How to use Linkerd for traffic splitting
- How Linkerd's architecture works

## What Is Linkerd?

Linkerd is a lightweight, security-first service mesh for Kubernetes. It is simpler and lighter than Istio while providing mTLS, observability, and traffic management.

| Feature | Istio | Linkerd |
|---------|-------|---------|
| Proxy | Envoy (C++) | linkerd2-proxy (Rust) |
| Resource usage | Higher | Lower |
| Complexity | Higher | Lower |
| mTLS | Yes | Yes (default on) |
| Dashboard | Kiali | Linkerd Viz |

## Installation

```bash
# Install CLI
curl --proto '=https' -sL https://run.linkerd.io/install | sh
export PATH=$HOME/.linkerd2/bin:$PATH

# Install on Kubernetes
linkerd install --crds | kubectl apply -f -
linkerd install | kubectl apply -f -

# Verify
linkerd check

# Inject sidecar into namespace
kubectl get deploy -o yaml | linkerd inject - | kubectl apply -f -
```

## Traffic Splitting

```yaml
# traffic-split.yaml

apiVersion: split.smi-spec.io/v1alpha4
kind: TrafficSplit
metadata:
  name: user-service-split
spec:
  service: user-service
  backends:
    - service: user-service-v1
      weight: 900  # 90%
    - service: user-service-v2
      weight: 100  # 10%
```

## Next Steps

For Consul, continue to [Consul Mesh](./03-consul-mesh.md).
