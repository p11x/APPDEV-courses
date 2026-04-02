# Horizontal Pod Autoscaler

## What You'll Learn

- How to configure HPA for automatic scaling
- How to set up custom metrics
- How to implement scaling policies
- How to monitor autoscaling behavior

---

## Layer 1: Academic Foundation

### Autoscaling Architecture

The Horizontal Pod Autoscaler (HPA) automatically scales the number of pods based on observed metrics like CPU utilization, memory usage, or custom metrics.

---

## Layer 2: Multi-Paradigm Code Evolution

### Paradigm 1 — Basic HPA

```yaml
# hpa.yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: node-api-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: node-api
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

### Paradigm 2 — HPA with Custom Metrics

```yaml
# hpa-custom-metrics.yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: node-api-hpa
spec:
  scaleTargetRef:
    kind: Deployment
    name: node-api
  minReplicas: 2
  maxReplicas: 10
  metrics:
    - type: Pods
      pods:
        metric:
          name: requests_per_second
        target:
          type: AverageValue
          averageValue: 100
```

### Paradigm 3 — Scaling Policies

```yaml
# hpa-policies.yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: node-api-hpa
spec:
  scaleTargetRef:
    kind: Deployment
    name: node-api
  minReplicas: 2
  maxReplicas: 10
  behavior:
    scaleDown:
      stabilizationWindowSeconds: 300
      policies:
        - type: Percent
          value: 10
          periodSeconds: 60
    scaleUp:
      stabilizationWindowSeconds: 0
      policies:
        - type: Percent
          value: 100
          periodSeconds: 15
```

---

## Layer 3: Performance Engineering

### Scaling Metrics

| Metric | Threshold | Description |
|--------|-----------|--------------|
| CPU | 70% | Target utilization |
| Memory | 80% | Target utilization |
| RPS | 100 | Requests per second |

---

## Layer 4: Security

### Security Considerations

- Set minReplicas to ensure availability
- Configure scale-down stabilization to prevent thrashing
- Use multiple metrics for balanced scaling

---

## Layer 5: Testing

### Autoscaling Tests

```typescript
// hpa-test.ts
async function testAutoscaling(deploymentName: string) {
  const hpa = await k8s.autoscaling.readNamespacedHorizontalPodAutoscaler(
    deploymentName + '-hpa',
    'default'
  );
  
  return {
    minReplicas: hpa.spec?.minReplicas,
    maxReplicas: hpa.spec?.maxReplicas,
    metrics: hpa.spec?.metrics
  };
}
```

---

## Next Steps

Continue to [Kubernetes Monitoring](./08-k8s-monitoring.md) for observability setup.