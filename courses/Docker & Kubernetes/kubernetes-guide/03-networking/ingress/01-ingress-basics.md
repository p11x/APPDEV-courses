# Ingress Basics

## Overview

Ingress is a Kubernetes resource that provides HTTP and HTTPS routing to services based on hostnames and URL paths. It acts as an entry point to your cluster, enabling external access to multiple services through a single load balancer. Ingress is essential for deploying web applications, APIs, and microservices that need URL-based routing.

## Prerequisites

- Understanding of Kubernetes Services
- Basic knowledge of HTTP/HTTPS networking
- Familiarity with DNS configuration

## Core Concepts

### The Problem Ingress Solves

Before Ingress, you needed to expose each service via NodePort or LoadBalancer:
- Each service needs its own public IP/port
- No path-based routing (can't route /api to one service and /web to another)
- No TLS termination at the cluster edge

Ingress solves this by:
- Single entry point (one IP/hostname)
- Host-based and path-based routing
- TLS termination
- Name-based virtual hosting

### Ingress vs Ingress Controller

It's crucial to understand:
- **Ingress resource**: YAML defining routing rules
- **Ingress controller**: Software that implements the routing (NGINX, Traefik, etc.)

You need BOTH:
1. Install an Ingress controller (e.g., NGINX Ingress Controller)
2. Create Ingress resources with routing rules

### IngressClass

Since Kubernetes 1.18+, IngressClass links an Ingress to a specific controller:

```yaml
apiVersion: networking.k8s.io/v1
kind: IngressClass
metadata:
  name: nginx
spec:
  controller: k8s.io/ingress-nginx
```

### Path Types

- **Prefix**: Matches URL path prefix (e.g., `/api` matches `/api`, `/api/v1`)
- **Exact**: Matches exact URL path (case-sensitive)
- **ImplementationSpecific**: Controller decides behavior

## Step-by-Step Examples

### Creating an Ingress

```yaml
apiVersion: networking.k8s.io/v1               # API group for Ingress (stable since k8s 1.19)
kind: Ingress                                    # Resource type for HTTP/S routing rules
metadata:
  name: web-ingress                             # Unique name in this namespace
  annotations:
    nginx.ingress.kubernetes.io/rewrite-target: /   # Strips path prefix before forwarding
spec:
  ingressClassName: nginx                       # Links to the IngressClass installed by the controller
  rules:
  - host: myapp.example.com                    # Hostname the rule applies to (must match DNS)
    http:
      paths:
      - path: /api                             # URL prefix to match
        pathType: Prefix                        # Prefix = matches /api, /api/v1, etc.
        backend:
          service:
            name: api-service                   # Name of the Service to forward traffic to
            port:
              number: 8080                      # Port on the Service (not the Pod)
      - path: /web
        pathType: Prefix
        backend:
          service:
            name: web-service
            port:
              number: 3000
```

### Multi-Path Ingress

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: multi-path-ingress
spec:
  ingressClassName: nginx
  rules:
  - host: api.example.com
    http:
      paths:
      # Exact match for /health
      - path: /health
        pathType: Exact
        backend:
          service:
            name: health-service
            port:
              number: 8080
      # Prefix match for /users (matches /users, /users/123, etc.)
      - path: /users
        pathType: Prefix
        backend:
          service:
            name: users-service
            port:
              number: 8080
      # Default backend (served when no rule matches)
      - path: /
        pathType: Prefix
        backend:
          service:
            name: default-service
            port:
              number: 8080
```

### Managing Ingress

```bash
# Create Ingress
kubectl apply -f ingress.yaml

# List Ingress resources
# Shows host, address, and ports
kubectl get ingress -o wide

# Describe for detailed rules
# Includes: host, paths, backend services, annotations
kubectl describe ingress web-ingress

# Check IngressClass
kubectl get ingressclass

# View ingress controller pods
kubectl get pods -n ingress-nginx

# Check ingress controller logs
kubectl logs -n ingress-nginx -l app.kubernetes.io/name=ingress-nginx

# Delete Ingress
kubectl delete ingress web-ingress
```

## Gotchas for Docker Users

- **Two components required**: Unlike Docker where you expose ports directly, Kubernetes needs both Ingress resource AND controller
- **No automatic TLS**: Must create TLS Secret manually or use cert-manager
- **Path matching complexity**: Unlike simple path routing, Kubernetes has Exact vs Prefix pathTypes
- **Controller-specific features**: Annotations vary between controllers (NGINX vs Traefik vs Ambassador)
- **Default backend**: Missing rules means 404 - must define default backend or wildcard

## Common Mistakes

- **Forgetting IngressClass**: Without ingressClassName, Ingress won't be picked up
- **Wrong pathType**: Using wrong type causes validation errors
- **No DNS**: Host must resolve to ingress controller IP
- **Missing annotations**: Many features require controller-specific annotations
- **Not installing controller**: Ingress resource does nothing without a controller

## Quick Reference

| Field | Description |
|-------|-------------|
| ingressClassName | Links to IngressClass |
| rules | Host/path to service mapping |
| host | Domain name |
| path | URL path prefix or exact |
| pathType | Prefix, Exact, or ImplementationSpecific |
| backend.service.name | Target Service |
| backend.service.port | Target Service port |
| tls | TLS configuration |

| Annotation | Purpose |
|-----------|---------|
| nginx.ingress.kubernetes.io/rewrite-target | Path rewrite |
| nginx.ingress.kubernetes.io/ssl-redirect | Force SSL |
| nginx.ingress.kubernetes.io/proxy-read-timeout | Timeout settings |

## What's Next

Continue to [NGINX Ingress Controller](./02-nginx-ingress-controller.md) to learn how to install and configure the most popular Ingress controller.
