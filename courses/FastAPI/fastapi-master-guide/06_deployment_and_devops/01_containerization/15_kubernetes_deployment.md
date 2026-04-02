# Kubernetes Deployment

## Overview

Kubernetes orchestrates containerized FastAPI applications for production-grade scalability, reliability, and management.

## Basic Deployment

### Deployment Manifest

```yaml
# Example 1: Kubernetes Deployment
apiVersion: apps/v1
kind: Deployment
metadata:
  name: fastapi-app
  labels:
    app: fastapi
spec:
  replicas: 3
  selector:
    matchLabels:
      app: fastapi
  template:
    metadata:
      labels:
        app: fastapi
    spec:
      containers:
        - name: fastapi
          image: myregistry/fastapi-app:latest
          ports:
            - containerPort: 8000
          env:
            - name: DATABASE_URL
              valueFrom:
                secretKeyRef:
                  name: app-secrets
                  key: database-url
          resources:
            requests:
              memory: "128Mi"
              cpu: "100m"
            limits:
              memory: "256Mi"
              cpu: "200m"
          livenessProbe:
            httpGet:
              path: /health
              port: 8000
            initialDelaySeconds: 30
            periodSeconds: 10
          readinessProbe:
            httpGet:
              path: /health/ready
              port: 8000
            initialDelaySeconds: 5
            periodSeconds: 5
```

### Service

```yaml
# Example 2: Kubernetes Service
apiVersion: v1
kind: Service
metadata:
  name: fastapi-service
spec:
  selector:
    app: fastapi
  ports:
    - protocol: TCP
      port: 80
      targetPort: 8000
  type: ClusterIP
```

### Ingress

```yaml
# Example 3: Ingress Configuration
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

## ConfigMap and Secrets

### Configuration Management

```yaml
# Example 4: ConfigMap
apiVersion: v1
kind: ConfigMap
metadata:
  name: app-config
data:
  ENVIRONMENT: production
  LOG_LEVEL: info
  DEBUG: "false"
```

```yaml
# Example 5: Secrets
apiVersion: v1
kind: Secret
metadata:
  name: app-secrets
type: Opaque
stringData:
  database-url: postgresql://user:pass@db:5432/mydb
  secret-key: my-secret-key
```

## Horizontal Pod Autoscaler

### Auto-scaling

```yaml
# Example 6: HPA Configuration
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: fastapi-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: fastapi-app
  minReplicas: 2
  maxReplicas: 10
  metrics:
    - type: Resource
      resource:
        name: cpu
        target:
          type: Utilization
          averageUtilization: 70
    - type: Resource
      resource:
        name: memory
        target:
          type: Utilization
          averageUtilization: 80
```

## Kustomize

### Environment Management

```yaml
# Example 7: kustomization.yaml
apiVersion: kustomize.config.k8s.io/v1beta1
kind: Kustomization

resources:
  - deployment.yaml
  - service.yaml
  - ingress.yaml

commonLabels:
  app: fastapi
  team: backend

images:
  - name: myregistry/fastapi-app
    newTag: v1.2.0
```

```bash
# Deploy with Kustomize
kubectl apply -k overlays/production
```

## Helm Charts

### Helm Values

```yaml
# Example 8: values.yaml
replicaCount: 3

image:
  repository: myregistry/fastapi-app
  tag: latest
  pullPolicy: IfNotPresent

service:
  type: ClusterIP
  port: 80

ingress:
  enabled: true
  hosts:
    - host: api.example.com
      paths:
        - path: /
          pathType: Prefix

resources:
  requests:
    memory: 128Mi
    cpu: 100m
  limits:
    memory: 256Mi
    cpu: 200m

autoscaling:
  enabled: true
  minReplicas: 2
  maxReplicas: 10
  targetCPUUtilizationPercentage: 70
```

## Deployment Commands

### kubectl Commands

```bash
# Example 9: Common kubectl commands

# Apply manifests
kubectl apply -f deployment.yaml
kubectl apply -k .

# View resources
kubectl get pods
kubectl get deployments
kubectl get services

# View logs
kubectl logs -f deployment/fastapi-app

# Scale deployment
kubectl scale deployment fastapi-app --replicas=5

# Rolling update
kubectl set image deployment/fastapi-app fastapi=myapp:v2

# Rollback
kubectl rollout undo deployment/fastapi-app

# Port forward (local testing)
kubectl port-forward service/fastapi-service 8000:80

# Execute into pod
kubectl exec -it pod/fastapi-app-xxx -- /bin/sh
```

## Best Practices

### Kubernetes Guidelines

```yaml
# Example 10: Production best practices
"""
Kubernetes Best Practices:

1. Resource Management
   ✓ Set resource requests and limits
   ✓ Use Horizontal Pod Autoscaler
   ✓ Monitor resource usage

2. Health Checks
   ✓ Configure liveness probes
   ✓ Configure readiness probes
   ✓ Set appropriate timeouts

3. Security
   ✓ Use non-root containers
   ✓ Enable Pod Security Standards
   ✓ Use secrets for sensitive data

4. Configuration
   ✓ Use ConfigMaps for config
   ✓ Externalize configuration
   ✓ Use environment-specific overlays

5. Deployment Strategy
   ✓ Use rolling updates
   ✓ Configure max surge/unavailable
   ✓ Implement blue-green or canary
"""
```

## Summary

| Resource | Purpose | Example |
|----------|---------|---------|
| Deployment | Run containers | `kubectl apply -f deployment.yaml` |
| Service | Expose pods | ClusterIP, LoadBalancer |
| Ingress | Route traffic | nginx-ingress |
| HPA | Auto-scale | CPU/Memory based scaling |
| ConfigMap | Configuration | Environment variables |
| Secrets | Sensitive data | Database credentials |

## Next Steps

Continue learning about:
- [Kubernetes Services](./16_kubernetes_services.md) - Service types
- [Kubernetes Ingress](./17_kubernetes_ingress.md) - Traffic routing
- [Monitoring](../05_monitoring_and_observability/01_monitoring_overview.md)
