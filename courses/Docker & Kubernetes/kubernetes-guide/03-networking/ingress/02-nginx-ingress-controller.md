# NGINX Ingress Controller

## Overview

The NGINX Ingress Controller is the most widely used Ingress implementation for Kubernetes. Built on top of NGINX web server, it provides a production-grade reverse proxy with advanced features like rate limiting, authentication, URL rewriting, and TLS termination. It transforms Kubernetes Ingress resources into NGINX configuration.

## Prerequisites

- Kubernetes cluster with kubectl access
- Helm 3 installed (recommended for installation)
- Basic knowledge of Ingress resources

## Core Concepts

### How It Works

The NGINX Ingress Controller:
1. Watches for Ingress resources in all namespaces
2. Generates NGINX configuration from Ingress rules
3. Reloads NGINX when configuration changes
4. Handles health checking, metrics, and logging
5. Provides performance metrics via Prometheus

### Installation Methods

1. **Helm** (recommended): Reproducible, upgradeable
2. **Manifests**: Direct YAML deployment
3. **Operator**: For OpenShift or advanced management

### Common Annotations

The controller supports many annotations for customization:
- Rate limiting
- Path rewriting
- Proxy timeouts
- Whitelist/blacklist
- CORS configuration

## Step-by-Step Examples

### Installing with Helm

```bash
# Add the NGINX ingress helm chart repository
# Stable contains the official NGINX Ingress Controller
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx

# Update helm repositories to get latest charts
helm repo update

# Install the NGINX Ingress Controller
# namespace=ingress-nginx creates a dedicated namespace
# controller.publishService.enabled publishes the LoadBalancer IP to the ingress status
helm install ingress-nginx ingress-nginx/ingress-nginx \
  --namespace ingress-nginx \
  --create-namespace \
  --set controller.publishService.enabled=true

# Verify the installation
# Waits for the controller pod to be ready before returning
kubectl wait --namespace ingress-nginx \
  --for=condition=ready pod \
  --selector=app.kubernetes.io/component=controller \
  --timeout=120s
```

### Alternative: Install via Manifests

```bash
# Download the manifest
curl -O https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.10.0/deploy/static/provider/cloud/deploy.yaml

# Apply with namespace creation
kubectl apply -f deploy.yaml

# Verify pods are running
kubectl get pods -n ingress-nginx
```

### Verifying Installation

```bash
# Check the LoadBalancer Service
# External IP/Hostname is needed for DNS configuration
kubectl get svc -n ingress-nginx

# Output shows EXTERNAL-IP or ADDRESS (AWS: ARN)
# NAME            TYPE           CLUSTER-IP     EXTERNAL-IP
# ingress-nginx   LoadBalancer   10.0.100.100   a1234567890.elb.amazonaws.com

# Verify IngressClass exists and is default
kubectl get ingressclass
# NAME    CONTROLLER
# nginx   k8s.io/ingress-nginx

# Make nginx the default IngressClass (optional)
kubectl label ingressclass nginx ingressclass.kubernetes.io/is-default-class=true
```

### Common Configuration Examples

#### Rate Limiting

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: rate-limited-api
  annotations:
    # Limit requests per IP
    nginx.ingress.kubernetes.io/limit-rps: "100"          # Requests per second
    nginx.ingress.kubernetes.io/limit-connections: "50"    # Concurrent connections
spec:
  ingressClassName: nginx
  rules:
  - host: api.example.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: api-service
            port:
              number: 8080
```

#### URL Rewrite

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: rewritten-api
  annotations:
    # Strip /api/v1 prefix before forwarding
    nginx.ingress.kubernetes.io/rewrite-target: /$2
    # Capture group for rewrite
spec:
  ingressClassName: nginx
  rules:
  - host: example.com
    http:
      paths:
      - path: /api/v1(/|$)(.*)          # Captures everything after /api/v1
        pathType: ImplementationSpecific
        backend:
          service:
            name: backend-service
            port:
              number: 8080
```

#### Basic Authentication

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: protected-app
  annotations:
    # Enable basic auth
    nginx.ingress.kubernetes.io/auth-type: basic
    # Secret containing htpasswd credentials
    nginx.ingress.kubernetes.io/auth-secret: basic-auth
    # Realm name shown in browser
    nginx.ingress.kubernetes.io/auth-realm: "Authentication Required"
spec:
  ingressClassName: nginx
  rules:
  - host: secure.example.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: web-service
            port:
              number: 80
```

### Debugging

```bash
# View controller logs
# -f follows in real-time, useful for debugging
kubectl logs -n ingress-nginx \
  -l app.kubernetes.io/name=ingress-nginx \
  --tail=100 -f

# Check controller configuration
# Exec into the controller pod and examine nginx.conf
kubectl exec -it -n ingress-nginx \
  ingress-nginx-controller-0 -- cat /etc/nginx/nginx.conf

# Validate Ingress resources
# Shows warnings and errors in Ingress definitions
kubectl get ingress -A

# Check events for errors
kubectl get events -n ingress-nginx --sort-by='.lastTimestamp'

# Test from inside the cluster
kubectl run test --rm -it --image=curlimages/curl --restart=Never \
  -- curl -H "Host: myapp.example.com" http://ingress-nginx-controller.ingress-nginx.svc
```

## Gotchas for Docker Users

- **Two-layer routing**: Traffic goes Client → Ingress → Service → Pod (not direct like Docker port mapping)
- **No automatic SSL**: Must configure TLS manually or use cert-manager unlike Docker Swarm's letsencrypt integration
- **Annotation syntax**: Features configured via annotations, not environment variables like Docker labels
- **Controller upgrade complexity**: Unlike Docker, upgrading requires careful planning for zero-downtime
- **Namespace isolation**: Ingress controller runs in its own namespace, separate from your apps

## Common Mistakes

- **Wrong IngressClass name**: Not specifying ingressClassName: nginx
- **Missing TLS config**: Not adding tls section when needed
- **Wrong path regex**: Using wrong regex in path matching
- **Not checking external IP**: Forgetting to configure DNS to ingress IP
- **Annotation typos**: Small typos in annotations cause silent failures

## Quick Reference

| Command | Description |
|---------|-------------|
| `helm install ingress-nginx ingress-nginx/ingress-nginx` | Install via Helm |
| `kubectl get svc -n ingress-nginx` | Get LoadBalancer IP |
| `kubectl logs -n ingress-nginx -l app.kubernetes.io/name=ingress-nginx` | View logs |
| `kubectl exec -it -n ingress-nginx ingress-nginx-controller-* -- nginx -T` | Full config dump |

| Annotation | Description |
|------------|-------------|
| `nginx.ingress.kubernetes.io/limit-rps` | Requests per second |
| `nginx.ingress.kubernetes.io/rewrite-target` | URL rewrite |
| `nginx.ingress.kubernetes.io/proxy-read-timeout` | Timeout |

## What's Next

Continue to [TLS Termination](./03-tls-termination.md) to learn how to secure your Ingress with TLS certificates.
