# Istio Setup

## What You'll Learn

- What Istio is and how it works
- How to install Istio on Kubernetes
- How to enable Istio for your services
- How Istio compares to other service meshes

## What Is Istio?

Istio is an open-source **service mesh** that manages communication between microservices. It adds traffic management, security, and observability without changing your application code.

| Feature | Without Mesh | With Istio |
|---------|-------------|------------|
| mTLS | Manual | Automatic |
| Load balancing | Basic (kube-proxy) | Advanced (L7) |
| Retries | In application code | Declarative config |
| Tracing | Manual instrumentation | Automatic |
| Circuit breaking | In application code | Declarative config |

## Installation

```bash
# Install Istio CLI
curl -L https://istio.io/downloadIstio | sh -
export PATH=$PWD/istio-*/bin:$PATH

# Install on Kubernetes
istioctl install --set profile=demo

# Verify
istioctl verify-install

# Enable sidecar injection for default namespace
kubectl label namespace default istio-injection=enabled
```

## Enable Istio for a Service

```yaml
# deployment.yaml — No code changes needed

apiVersion: apps/v1
kind: Deployment
metadata:
  name: user-service
spec:
  replicas: 2
  selector:
    matchLabels:
      app: user-service
  template:
    metadata:
      labels:
        app: user-service
    spec:
      containers:
        - name: user-service
          image: myapp/user-service:latest
          ports:
            - containerPort: 3000
```

Istio automatically injects a sidecar proxy (Envoy) alongside each pod.

## Traffic Management

```yaml
# virtual-service.yaml — Route traffic

apiVersion: networking.istio.io/v1beta1
kind: VirtualService
metadata:
  name: user-service
spec:
  hosts:
    - user-service
  http:
    - route:
        - destination:
            host: user-service
            subset: v1
          weight: 90
        - destination:
            host: user-service
            subset: v2
          weight: 10
---
# Destination rule — define subsets
apiVersion: networking.istio.io/v1beta1
kind: DestinationRule
metadata:
  name: user-service
spec:
  host: user-service
  subsets:
    - name: v1
      labels:
        version: v1
    - name: v2
      labels:
        version: v2
  trafficPolicy:
    connectionPool:
      tcp:
        maxConnections: 100
      http:
        h2UpgradePolicy: DEFAULT
        http1MaxPendingRequests: 100
    outlierDetection:
      consecutive5xxErrors: 5
      interval: 30s
      baseEjectionTime: 30s
```

## mTLS Configuration

```yaml
# peer-authentication.yaml — Enforce mTLS

apiVersion: security.istio.io/v1beta1
kind: PeerAuthentication
metadata:
  name: default
  namespace: default
spec:
  mtls:
    mode: STRICT  # All traffic must be mTLS encrypted
```

## Next Steps

For Linkerd, continue to [Linkerd Setup](./02-linkerd-setup.md).
