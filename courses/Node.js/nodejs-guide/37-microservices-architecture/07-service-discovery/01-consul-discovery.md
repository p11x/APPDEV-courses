# Consul Service Discovery

## What You'll Learn

- How Consul service discovery works
- How to register and discover services
- How health checks work with Consul
- How to use Consul DNS for service discovery

## Setup

```bash
# Start Consul
consul agent -dev

# Or Docker
docker run -d -p 8500:8500 -p 8600:8600/udp consul agent -server -bootstrap-expect=1 -ui -client=0.0.0.0
```

## Service Registration

```ts
// register.ts

import { request } from 'undici';

async function registerService() {
  await request('http://localhost:8500/v1/agent/service/register', {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      Name: 'user-service',
      ID: `user-service-${process.pid}`,
      Address: '127.0.0.1',
      Port: 3000,
      Check: {
        HTTP: 'http://127.0.0.1:3000/healthz',
        Interval: '10s',
        Timeout: '2s',
      },
    }),
  });
}

// Deregister on shutdown
process.on('SIGTERM', async () => {
  await request(`http://localhost:8500/v1/agent/service/deregister/user-service-${process.pid}`, {
    method: 'PUT',
  });
  process.exit(0);
});
```

## Service Discovery

```ts
// discover.ts

async function discoverService(name: string) {
  const res = await fetch(`http://localhost:8500/v1/health/service/${name}?passing=true`);
  const data = await res.json();

  // Load balance: random selection
  const service = data[Math.floor(Math.random() * data.length)];

  return {
    address: service.Service.Address,
    port: service.Service.Port,
    url: `http://${service.Service.Address}:${service.Service.Port}`,
  };
}

// Usage
const userService = await discoverService('user-service');
const response = await fetch(`${userService.url}/users`);
```

## Next Steps

For health checks, continue to [Discovery Health Checks](./05-discovery-health-checks.md).
