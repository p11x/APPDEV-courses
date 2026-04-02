# Kubernetes Ingress

## What You'll Learn

- How to configure Kubernetes Ingress for HTTP/HTTPS routing
- How to set up ingress controllers
- How to implement path-based and host-based routing
- How to configure TLS termination

---

## Layer 1: Academic Foundation

### Ingress Architecture

An Ingress is an API object that manages external access to services, typically HTTP/HTTPS. It can provide load balancing, SSL termination, and name-based virtual hosting.

The ingress controller watches for Ingress resources and configures the load balancer accordingly. Popular controllers include nginx, Traefik, and cloud provider-specific solutions.

---

## Layer 2: Multi-Paradigm Code Evolution

### Paradigm 1 — Basic Ingress

```yaml
# ingress.yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: node-api-ingress
  annotations:
    nginx.ingress.kubernetes.io/rewrite-target: /
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
                name: node-api-service
                port:
                  number: 80
```

### Paradigm 2 — Ingress with TLS

```yaml
# ingress-tls.yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: node-api-ingress-tls
  annotations:
    nginx.ingress.kubernetes.io/ssl-redirect: "true"
    cert-manager.io/cluster-issuer: letsencrypt-prod
spec:
  ingressClassName: nginx
  tls:
    - hosts:
        - api.example.com
      secretName: node-api-tls
  rules:
    - host: api.example.com
      http:
        paths:
          - path: /api
            pathType: Prefix
            backend:
              service:
                name: node-api-service
                port:
                  number: 80
```

### Paradigm 3 — Path-Based Routing

```yaml
# ingress-multi-path.yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: node-api-ingress
spec:
  ingressClassName: nginx
  rules:
    - host: example.com
      http:
        paths:
          - path: /api/v1
            pathType: Prefix
            backend:
              service:
                name: node-api-v1
                port:
                  number: 80
          - path: /api/v2
            pathType: Prefix
            backend:
              service:
                name: node-api-v2
                port:
                  number: 80
```

---

## Layer 3: Performance Engineering

### Ingress Performance

| Metric | Target |
|--------|--------|
| Request Latency | < 50ms |
| Throughput | 10k+ req/s |
| SSL Handshake | < 20ms |

---

## Layer 4: Security

### Security Annotations

```yaml
annotations:
  nginx.ingress.kubernetes.io/proxy-body-size: "10m"
  nginx.ingress.kubernetes.io/proxy-connect-timeout: "30"
  nginx.ingress.kubernetes.io/proxy-read-timeout: "60"
  nginx.ingress.kubernetes.io/proxy-send-timeout: "60"
  nginx.ingress.kubernetes.io/limit-connections: "50"
  nginx.ingress.kubernetes.io/limit-rps: "100"
```

---

## Layer 5: Testing

### Ingress Validation

```typescript
// ingress-test.ts
async function validateIngress(ingressName: string, namespace: string) {
  const ingress = await k8s.networking.readNamespacedIngress(ingressName, namespace);
  
  return {
    host: ingress.spec?.rules?.[0]?.host,
    tls: ingress.spec?.tls?.length > 0,
    rules: ingress.spec?.rules?.map(r => ({
      path: r.http?.paths?.[0]?.path,
      service: r.http?.paths?.[0]?.backend?.service?.name
    }))
  };
}
```

---

## Layer 6: DevOps

### Monitoring Ingress

```yaml
# prometheus rule
- alert: IngressHighErrorRate
  expr: rate(nginx_ingress_controller_requests_error_total[5m]) > 0.05
  for: 5m
  labels:
    severity: warning
```

---

## Layer 7: Learning

### Knowledge Graph

- **Prerequisites**: Kubernetes Service basics
- **Related Topics**: TLS, DNS, Load balancing

---

## Next Steps

Continue to [Kubernetes Config](./05-k8s-config.md) for configuration management.