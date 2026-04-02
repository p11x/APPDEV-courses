# Kubernetes Best Practices

## What You'll Learn

- Production-ready Kubernetes configurations
- Security hardening guidelines
- Resource optimization strategies
- High availability patterns

---

## Layer 1: Best Practices Overview

### Security Hardening

| Practice | Implementation |
|----------|----------------|
| Run non-root | `securityContext.runAsNonRoot: true` |
| Read-only root filesystem | `securityContext.readOnlyRootFilesystem: true` |
| Drop capabilities | `securityContext.capabilities.drop: [ALL]` |
| Network policies | Restrict pod-to-pod communication |
| RBAC | Least privilege access control |

---

## Layer 2: Production Patterns

### Resource Limits Best Practice

```yaml
# deployment-best-practices.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-api
spec:
  replicas: 3
  strategy:
    type: RollingUpdate
    rollingUpdate:
      maxSurge: 1
      maxUnavailable: 0
  template:
    spec:
      securityContext:
        runAsNonRoot: true
        runAsUser: 1000
        fsGroup: 1000
      containers:
        - name: node-api
          securityContext:
            readOnlyRootFilesystem: true
            allowPrivilegeEscalation: false
            capabilities:
              drop:
                - ALL
          resources:
            requests:
              memory: "256Mi"
              cpu: "200m"
            limits:
              memory: "512Mi"
              cpu: "500m"
          livenessProbe:
            httpGet:
              path: /health
              port: 3000
            initialDelaySeconds: 30
            periodSeconds: 10
          readinessProbe:
            httpGet:
              path: /ready
              port: 3000
            initialDelaySeconds: 5
            periodSeconds: 5
```

### Pod Disruption Budget

```yaml
# pdb.yaml
apiVersion: policy/v1
kind: PodDisruptionBudget
metadata:
  name: node-api-pdb
spec:
  minAvailable: 2
  selector:
    matchLabels:
      app: node-api
```

---

## Layer 3: Performance

### Optimization Checklist

- Set appropriate resource requests/limits
- Use readiness probes to avoid traffic during startup
- Configure HPA with multiple metrics
- Use caching for frequently accessed data

---

## Layer 4: Security

### Security Checklist

- [ ] Enable RBAC
- [ ] Use network policies
- [ ] Enable etcd encryption
- [ ] Configure pod security policies
- [ ] Use secrets for sensitive data
- [ ] Enable audit logging
- [ ] Regular security updates

---

## Layer 5: Testing

### K8s Validation Tests

```typescript
// k8s-best-practices-test.ts
async function validateBestPractices(deploymentName: string) {
  const deployment = await k8s.apps.readNamespacedDeployment(deploymentName, 'default');
  const podSpec = deployment.spec?.template?.spec;
  
  const checks = {
    hasSecurityContext: !!podSpec?.securityContext,
    hasResourceLimits: !!podSpec?.containers?.[0]?.resources?.limits,
    hasLivenessProbe: !!podSpec?.containers?.[0]?.livenessProbe,
    hasReadinessProbe: !!podSpec?.containers?.[0]?.readinessProbe,
    isNonRoot: podSpec?.securityContext?.runAsNonRoot === true
  };
  
  return {
    compliant: Object.values(checks).every(v => v),
    checks
  };
}
```

---

## Next Steps

Continue to [Kubernetes vs Docker Compose](./10-k8s-vs-docker-compose.md) for comparison.