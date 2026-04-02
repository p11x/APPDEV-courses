# Kubernetes vs Docker Compose

## What You'll Learn

- Key differences between Kubernetes and Docker Compose
- When to use each orchestration tool
- Migration strategies from Docker Compose to Kubernetes
- Decision criteria for choosing the right tool

---

## Layer 1: Academic Foundation

### Tool Comparison

| Feature | Docker Compose | Kubernetes |
|---------|---------------|------------|
| Orchestration | Single host | Multi-node cluster |
| Scaling | Manual/replicas | Auto-scaling |
| Service Discovery | Links/environment | DNS-based |
| Load Balancing | Docker proxy | Built-in LB |
| Rolling Updates | Manual | Automated |
| State Management | Volumes | Persistent volumes |
| Complexity | Low | High |
| Learning Curve | Easy | Steep |

---

## Layer 2: Multi-Paradigm Code Evolution

### Docker Compose Example

```yaml
# docker-compose.yaml
version: '3.8'
services:
  api:
    build: ./api
    ports:
      - "3000:3000"
    environment:
      - NODE_ENV=production
      - DATABASE_URL=postgres://db:5432
    depends_on:
      - db
      - redis
    volumes:
      - ./logs:/app/logs
    restart: unless-stopped

  db:
    image: postgres:15
    volumes:
      - postgres_data:/var/lib/postgresql/data
    environment:
      POSTGRES_PASSWORD: secret

  redis:
    image: redis:7-alpine

volumes:
  postgres_data:
```

### Kubernetes Equivalent

```yaml
# k8s deployment
apiVersion: apps/v1
kind: Deployment
metadata:
  name: api
spec:
  replicas: 3
  selector:
    matchLabels:
      app: api
  template:
    metadata:
      labels:
        app: api
    spec:
      containers:
        - name: api
          image: api:latest
          ports:
            - containerPort: 3000
          env:
            - name: NODE_ENV
              value: production
            - name: DATABASE_URL
              valueFrom:
                configMapKeyRef:
                  name: app-config
                  key: database-url
---
# k8s service
apiVersion: v1
kind: Service
metadata:
  name: api
spec:
  type: ClusterIP
  selector:
    app: api
  ports:
    - port: 80
      targetPort: 3000
```

---

## Layer 3: Decision Matrix

### When to Use Docker Compose

- Local development and testing
- Single-node environments
- Small projects with limited scale
- Prototyping and proof-of-concept
- Simple CI/CD pipelines

### When to Use Kubernetes

- Production environments
- Multi-node clusters
- Auto-scaling requirements
- Complex microservice architectures
- Multi-cloud or hybrid deployments
- Enterprise requirements (RBAC, policies)

---

## Layer 4: Migration Strategy

### Migration Steps

1. Containerize the application
2. Create Kubernetes manifests
3. Configure Docker registry
4. Set up namespace and RBAC
5. Deploy and verify
6. Set up monitoring
7. Configure CI/CD

---

## Diagnostic Center

### Troubleshooting Guide

| Issue | Docker Compose | Kubernetes |
|-------|---------------|------------|
| Service not starting | `docker-compose logs` | `kubectl logs` |
| Port conflicts | Check docker-compose ports | Check service ports |
| Network issues | Check networks config | Check network policies |
| Volume issues | Check volume mounts | Check PVCs |

---

## Next Steps

Continue to [AWS Lambda](./../02-aws-serverless/01-aws-lambda.md) for serverless computing.