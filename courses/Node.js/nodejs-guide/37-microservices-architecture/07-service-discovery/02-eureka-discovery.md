# Eureka Service Discovery

## What You'll Learn

- How Eureka works
- How to register services with Eureka
- How to discover services
- How Eureka compares to Consul

## Setup

```bash
# Docker
docker run -d -p 8761:8761 netflixoss/eureka:latest
```

## Registration (Node.js)

```ts
// register.ts

async function registerWithEureka() {
  await fetch('http://localhost:8761/eureka/apps/user-service', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      instance: {
        instanceId: `user-service-${process.pid}`,
        hostName: 'localhost',
        app: 'USER-SERVICE',
        ipAddr: '127.0.0.1',
        port: { $: 3000, '@enabled': true },
        statusPageUrl: 'http://localhost:3000/health',
      },
    }),
  });
}
```

## Comparison

| Feature | Consul | Eureka |
|---------|--------|--------|
| Health checks | Built-in | Client-side |
| Multi-datacenter | Yes | No |
| DNS interface | Yes | No |
| Best for | Polyglot | Java/Spring |

## Next Steps

For etcd, continue to [etcd Discovery](./03-etcd-discovery.md).
