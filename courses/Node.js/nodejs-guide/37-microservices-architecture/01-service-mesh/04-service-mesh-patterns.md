# Service Mesh Patterns

## What You'll Learn

- Common service mesh deployment patterns
- How to implement canary deployments
- How to implement blue-green deployments
- How to handle mesh migration

## Canary Deployment

```yaml
# 90% traffic to stable, 10% to canary
apiVersion: networking.istio.io/v1beta1
kind: VirtualService
metadata:
  name: my-service
spec:
  hosts:
    - my-service
  http:
    - route:
        - destination:
            host: my-service
            subset: stable
          weight: 90
        - destination:
            host: my-service
            subset: canary
          weight: 10
```

## Circuit Breaking

```yaml
apiVersion: networking.istio.io/v1beta1
kind: DestinationRule
metadata:
  name: my-service
spec:
  host: my-service
  trafficPolicy:
    connectionPool:
      tcp:
        maxConnections: 100
      http:
        http1MaxPendingRequests: 50
        maxRequestsPerConnection: 10
    outlierDetection:
      consecutive5xxErrors: 3
      interval: 10s
      baseEjectionTime: 30s
      maxEjectionPercent: 50
```

## Next Steps

For monitoring, continue to [Service Mesh Monitoring](./05-service-mesh-monitoring.md).
