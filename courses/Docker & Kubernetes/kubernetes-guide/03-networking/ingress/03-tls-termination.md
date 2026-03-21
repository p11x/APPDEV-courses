# TLS Termination

## Overview

TLS (Transport Layer Security) termination is the process of decrypting HTTPS traffic at the Ingress controller before forwarding it to backend services. This provides secure communication between clients and your cluster while allowing backend services to use simpler HTTP communication. This guide covers configuring TLS in Kubernetes Ingress.

## Prerequisites

- Understanding of Ingress basics
- Knowledge of TLS certificates (certificate, private key)
- Familiarity with Kubernetes Secrets

## Core Concepts

### How TLS Termination Works

1. Client connects to Ingress controller using HTTPS
2. Ingress controller decrypts the request (TLS termination)
3. Controller forwards unencrypted request to backend service
4. Backend service responds with unencrypted response
5. Ingress controller encrypts response and sends to client

### TLS in Kubernetes

TLS is configured via:
- **Secret**: Contains certificate and private key
- **Ingress resource**: References the Secret

### cert-manager

cert-manager automates certificate management:
- Automatically requests certificates from Let's Encrypt
- Handles renewal before expiration
- Stores certificates as Secrets
- Supports multiple certificate authorities

## Step-by-Step Examples

### Creating a TLS Secret

First, create the TLS Secret from certificate files:

```bash
# Create TLS secret from certificate files
# Requires: server.crt (certificate) and server.key (private key)
# -n specifies the namespace where the Ingress will be created
kubectl create secret tls web-tls \
  --cert=server.crt \
  --key=server.key \
  --namespace production

# Verify secret was created
# TYPE shows kubernetes.io/tls, indicating TLS certificate
kubectl get secret web-tls -n production

# Describe for certificate details
# Shows issuer, expiration, and certificate info
kubectl describe secret web-tls -n production

# Alternative: Create generic secret (less common)
kubectl create secret generic web-credentials \
  --from-literal=username=admin \
  --from-literal=password=secret123
```

### Creating Ingress with TLS

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: secure-web-ingress
  annotations:
    nginx.ingress.kubernetes.io/ssl-redirect: "true"   # Redirect HTTP to HTTPS
    nginx.ingress.kubernetes.io/force-ssl-redirect: "true"  # Force HTTPS
spec:
  ingressClassName: nginx
  tls:
  # -hosts lists hostnames this certificate serves
  # -secretName references the TLS Secret containing cert/key
  - hosts:
    - myapp.example.com
    secretName: web-tls                           # Must exist in same namespace
  rules:
  - host: myapp.example.com
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

### Multiple Hosts with TLS

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: multi-host-tls
spec:
  ingressClassName: nginx
  tls:
  # First certificate serves api and admin
  - hosts:
    - api.example.com
    - admin.example.com
    secretName: wildcard-tls        # Wildcard certificate
  # Second certificate serves shop
  - hosts:
    - shop.example.com
    secretName: shop-tls
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
  - host: admin.example.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: admin-service
            port:
              number: 8080
  - host: shop.example.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: shop-service
            port:
              number: 8080
```

### Installing cert-manager

For automated certificates:

```bash
# Install cert-manager CRDs first
# These define the Certificate resources in Kubernetes
kubectl apply -f https://github.com/cert-manager/cert-manager/releases/download/v1.14.0/cert-manager.crds.yaml

# Install cert-manager via Helm
# Creates cert-manager namespace and deploys the operator
helm install cert-manager jetstack/cert-manager \
  --namespace cert-manager \
  --create-namespace \
  --set installCRDs=true

# Wait for cert-manager to be ready
kubectl wait --for=condition=ready pod \
  -l app.kubernetes.io/instance=cert-manager \
  -n cert-manager \
  --timeout=300s
```

### Using cert-manager for Auto-Renewal

```yaml
apiVersion: cert-manager.io/v1
kind: ClusterIssuer
metadata:
  name: letsencrypt-prod
spec:
  acme:
    # Email for certificate expiration notices
    email: admin@example.com
    # Server URL (Let's Encrypt production)
    server: https://acme-v02.api.letsencrypt.org/directory
    privateKeySecretRef:
      # Secret to store ACME account private key
      name: letsencrypt-prod
    solvers:
    - http01:
        # Use NGINX Ingress for HTTP01 challenges
        ingress:
          class: nginx
---
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: auto-tls-ingress
  annotations:
    cert-manager.io/cluster-issuer: letsencrypt-prod   # Use the ClusterIssuer
spec:
  ingressClassName: nginx
  tls:
  - hosts:
    - myapp.example.com
    # cert-manager creates this secret automatically
    secretName: auto-tls-secret
  rules:
  - host: myapp.example.com
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

### Verifying TLS Configuration

```bash
# Check Ingress shows TLS configured
kubectl describe ingress secure-web-ingress

# Check certificate is issued
# READY should show True after a few moments
kubectl get certificate

# Describe certificate for details
# Shows: issuer,Common Name,expiration,secret
kubectl describe certificate web-tls

# Verify HTTPS works (from inside cluster)
kubectl run test-tls --rm -it --image=curlimages/curl --restart=Never -- \
  https://myapp.example.com -k   # -k accepts self-signed

# Check certificate expiration
# Shows days until expiration
kubectl get secret -o jsonpath='{.data.tls\.crt}' | base64 -d | openssl x509 -noout -dates
```

## Gotchas for Docker Users

- **Two-layer encryption**: Unlike Docker Swarm with built-in encryption, Kubernetes requires explicit TLS setup
- **Certificate management**: Manual renewal needed without cert-manager, unlike some Docker solutions
- **Secret namespace**: TLS Secret must be in same namespace as Ingress
- **Backend communication**: Traffic from Ingress to pod is often unencrypted unless configured with additional TLS
- **Certificate rotation**: Manual certificates need renewal; cert-manager handles automatically

## Common Mistakes

- **Wrong secret name**: TLS secretName must match exactly
- **Missing hosts in TLS section**: Ingress TLS hosts must match or be subset of rule hosts
- **Expired certificates**: Not monitoring certificate expiration
- **Self-signed certs in production**: Use Let's Encrypt or proper CA for production
- **Not redirecting HTTP to HTTPS**: Without annotation, HTTP remains accessible

## Quick Reference

| Command | Description |
|---------|-------------|
| `kubectl create secret tls NAME --cert=FILE --key=FILE` | Create TLS secret |
| `kubectl get secret NAME -o yaml` | View secret details |
| `kubectl get certificate` | Check cert-manager certificates |

| Annotation | Purpose |
|------------|-------------|
| `nginx.ingress.kubernetes.io/ssl-redirect` | Redirect HTTP→HTTPS |
| `cert-manager.io/cluster-issuer` | Use cert-manager issuer |

| TLS Field | Description |
|-----------|-------------|
| `hosts` | Hostnames covered by this certificate |
| `secretName` | Name of TLS Secret containing cert/key |

## What's Next

Continue to [Network Policies](./../policies/01-network-policies.md) to learn about securing pod-to-pod communication.
