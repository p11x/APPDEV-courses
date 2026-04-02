# Consul Service Mesh

## What You'll Learn

- What Consul Connect is
- How to set up Consul service mesh
- How Consul compares to Istio and Linkerd
- How to use Consul for service discovery + mesh

## What Is Consul Connect?

Consul Connect adds service mesh capabilities to HashiCorp Consul. It provides mTLS, service-to-service authorization, and integrates with Consul's service discovery.

## Installation

```bash
# Install Consul
brew install consul

# Start dev server
consul agent -dev

# Or with Docker
docker run -d --name consul -p 8500:8500 consul:latest
```

## Service Registration

```hcl
# service.hcl

service {
  name = "user-service"
  port = 3000

  connect {
    sidecar_service {}  # Enable sidecar proxy
  }

  check {
    http = "http://localhost:3000/healthz"
    interval = "10s"
  }
}
```

## Comparison

| Feature | Istio | Linkerd | Consul |
|---------|-------|---------|--------|
| Service discovery | No (needs K8s) | No | Built-in |
| mTLS | Yes | Yes | Yes |
| Multi-platform | Kubernetes | Kubernetes | Any (VM, K8s, Nomad) |
| Best for | K8s-native | Simplicity | Multi-platform |

## Next Steps

For patterns, continue to [Service Mesh Patterns](./04-service-mesh-patterns.md).
