# Kubernetes ConfigMaps

## What You'll Learn

- How to create and use ConfigMaps
- How to inject configuration into pods
- How to manage environment-specific configurations
- How to update configurations without rebuilding containers

---

## Layer 1: Academic Foundation

### Configuration Management

ConfigMaps decouple configuration from container images, allowing the same container to be used across different environments (dev, staging, production).

---

## Layer 2: Multi-Paradigm Code Evolution

### Paradigm 1 — ConfigMap from Literal Values

```yaml
# configmap-literal.yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: node-api-config
data:
  DATABASE_HOST: "postgres.default.svc.cluster.local"
  DATABASE_PORT: "5432"
  REDIS_HOST: "redis.default.svc.cluster.local"
  LOG_LEVEL: "info"
  NODE_ENV: "production"
```

### Paradigm 2 — ConfigMap from Files

```yaml
# configmap-file.yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: app-config
data:
  config.json: |
    {
      "database": {
        "host": "postgres",
        "port": 5432,
        "pool": 10
      },
      "redis": {
        "host": "redis",
        "port": 6379
      },
      "logging": {
        "level": "info",
        "format": "json"
      }
    }
```

### Paradigm 3 — ConfigMap as Environment Variables

```yaml
# deployment-with-configmap.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-api
spec:
  replicas: 3
  template:
    spec:
      containers:
        - name: node-api
          image: myregistry/node-api:latest
          envFrom:
            - configMapRef:
                name: node-api-config
          env:
            - name: API_KEY
              valueFrom:
                secretKeyRef:
                  name: api-secrets
                  key: api-key
```

### Paradigm 4 — ConfigMap as Volume Mount

```yaml
# deployment-volume-configmap.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-api
spec:
  template:
    spec:
      containers:
        - name: node-api
          image: myregistry/node-api:latest
          volumeMounts:
            - name: config-volume
              mountPath: /app/config
              readOnly: true
      volumes:
        - name: config-volume
          configMap:
            name: app-config
```

---

## Layer 3: Performance Engineering

### Configuration Update Strategy

| Strategy | Downtime | Complexity |
|----------|----------|------------|
| Restart pods | Yes | Low |
| Rolling update | No | Medium |
| Volume mount reload | No | High |
| Sidecar pattern | No | Medium |

---

## Layer 4: Security

### Security Best Practices

- Use secrets for sensitive data (passwords, API keys)
- Implement RBAC to restrict ConfigMap access
- Enable encryption at rest for etcd

---

## Layer 5: Testing

### Configuration Validation

```typescript
// config-test.ts
function validateConfig(config: Record<string, string>) {
  const required = ['DATABASE_HOST', 'DATABASE_PORT', 'LOG_LEVEL'];
  const missing = required.filter(key => !config[key]);
  
  return {
    valid: missing.length === 0,
    missingKeys: missing
  };
}
```

---

## Next Steps

Continue to [Kubernetes Secrets](./06-k8s-secrets.md) for sensitive data management.