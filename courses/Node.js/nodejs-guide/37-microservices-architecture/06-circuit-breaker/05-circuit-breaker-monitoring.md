# Circuit Breaker Monitoring

## What You'll Learn

- How to monitor circuit breaker state
- How to expose metrics
- How to set up alerts
- How to visualize circuit breaker status

## Metrics

```ts
import { Counter, Gauge } from 'prom-client';

const circuitState = new Gauge({
  name: 'circuit_breaker_state',
  help: 'Circuit breaker state (0=closed, 1=open, 2=half-open)',
  labelNames: ['service'],
});

const circuitTrips = new Counter({
  name: 'circuit_breaker_trips_total',
  help: 'Number of circuit breaker trips',
  labelNames: ['service'],
});

class MonitoredCircuitBreaker extends CircuitBreaker {
  private state = CircuitState.CLOSED;

  setState(newState: CircuitState) {
    super.setState(newState);
    circuitState.set({ service: this.serviceName }, newState === CircuitState.OPEN ? 1 : 0);

    if (newState === CircuitState.OPEN) {
      circuitTrips.inc({ service: this.serviceName });
    }
  }
}
```

## Alert

```yaml
- alert: CircuitBreakerOpen
  expr: circuit_breaker_state == 1
  for: 1m
  labels:
    severity: warning
  annotations:
    summary: "Circuit breaker open for {{ $labels.service }}"
```

## Next Steps

For service discovery, continue to [Consul Discovery](../07-service-discovery/01-consul-discovery.md).
