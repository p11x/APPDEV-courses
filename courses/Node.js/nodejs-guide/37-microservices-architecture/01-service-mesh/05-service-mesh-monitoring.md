# Service Mesh Monitoring

## What You'll Learn

- How to monitor service mesh traffic
- How to set up Kiali for Istio
- How to create mesh dashboards
- How to set up mesh alerts

## Kiali (Istio Dashboard)

```bash
# Install Kiali
kubectl apply -f https://raw.githubusercontent.com/kiali/kiali/master/deploy/getting-started/kiali.yaml

# Access
kubectl port-forward svc/kiali 20001:20001 -n istio-system
# Open http://localhost:20001
```

## Prometheus Metrics

```yaml
# Service mesh exposes metrics automatically
# Key metrics to monitor:

# Request rate
istio_requests_total

# Request duration
istio_request_duration_milliseconds

# Request errors
istio_requests_total{response_code=~"5.."}

# TCP connections
istio_tcp_connections_opened_total
istio_tcp_connections_closed_total
```

## Alerts

```yaml
groups:
  - name: service-mesh
    rules:
      - alert: HighErrorRate
        expr: |
          sum(rate(istio_requests_total{response_code=~"5.."}[5m])) by (destination_service)
          /
          sum(rate(istio_requests_total[5m])) by (destination_service)
          > 0.01
        for: 2m

      - alert: HighLatency
        expr: |
          histogram_quantile(0.99, rate(istio_request_duration_milliseconds_bucket[5m])) > 1000
        for: 5m
```

## Next Steps

For API Gateway, continue to [Nginx Gateway](../02-api-gateway/01-nginx-gateway.md).
