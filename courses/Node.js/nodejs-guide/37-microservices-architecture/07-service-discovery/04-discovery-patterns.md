# Discovery Patterns

## What You'll Learn

- Client-side vs server-side discovery
- How to implement load balancing
- How to cache service locations

## Client-Side Discovery

```ts
// Client discovers service and load balances

class ServiceDiscovery {
  async getService(name: string) {
    const instances = await consul.getHealthyInstances(name);

    // Round-robin selection
    const index = this.counter++ % instances.length;
    return instances[index];
  }
}
```

## Server-Side Discovery

```
Client → Load Balancer → Service Instance
              │
              └── Service Registry
```

## Next Steps

For health checks, continue to [Discovery Health Checks](./05-discovery-health-checks.md).
