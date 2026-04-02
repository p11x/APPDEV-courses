# Kubernetes Ingress

## Overview

Kubernetes Ingress manages external access to services, typically HTTP/HTTPS routing.

## Ingress Configuration

### Basic Ingress

```yaml
# Example 1: Nginx Ingress
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: fastapi-ingress
  annotations:
    nginx.ingress.kubernetes.io/rewrite-target: /
    cert-manager.io/cluster-issuer: letsencrypt-prod
spec:
  tls:
    - hosts:
        - api.example.com
      secretName: api-tls
  rules:
    - host: api.example.com
      http:
        paths:
          - path: /
            pathType: Prefix
            backend:
              service:
                name: fastapi-service
                port:
                  number: 80
```

### Path-Based Routing

```yaml
# Example 2: Multiple services
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: app-ingress
spec:
  rules:
    - host: api.example.com
      http:
        paths:
          - path: /users
            pathType: Prefix
            backend:
              service:
                name: user-service
                port:
                  number: 80
          - path: /orders
            pathType: Prefix
            backend:
              service:
                name: order-service
                port:
                  number: 80
```

## Summary

Ingress provides HTTP/HTTPS routing to Kubernetes services.

## Next Steps

Continue learning about:
- [Kubernetes Services](./15_kubernetes_services.md)
- [Kubernetes Monitoring](./21_kubernetes_monitoring.md)
