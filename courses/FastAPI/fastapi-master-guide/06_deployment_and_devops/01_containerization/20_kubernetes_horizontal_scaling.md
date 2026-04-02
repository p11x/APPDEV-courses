# Kubernetes Horizontal Pod Autoscaling

## Overview

Horizontal Pod Autoscaler (HPA) automatically scales pods based on metrics.

## HPA Configuration

### Basic HPA

```yaml
# Example 1: HPA configuration
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

### Custom Metrics

```yaml
# Example 2: Custom metrics HPA
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: fastapi-hpa-custom
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: fastapi-app
  minReplicas: 2
  maxReplicas: 20
  metrics:
    - type: Pods
      pods:
        metric:
          name: http_requests_per_second
        target:
          type: AverageValue
          averageValue: "100"
```

## Summary

HPA enables automatic scaling based on resource usage.

## Next Steps

Continue learning about:
- [Kubernetes Monitoring](./21_kubernetes_monitoring.md)
- [Kubernetes Best Practices](./24_kubernetes_best_practices.md)
