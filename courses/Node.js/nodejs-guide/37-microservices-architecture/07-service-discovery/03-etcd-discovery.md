# etcd Service Discovery

## What You'll Learn

- How etcd works for service discovery
- How to use etcd with Node.js
- How etcd compares to Consul

## Setup

```bash
docker run -d -p 2379:2379 quay.io/coreos/etcd:v3.5
```

## Registration

```ts
import { Etcd3 } from 'etcd3';

const client = new Etcd3({ hosts: 'localhost:2379' });

// Register with lease (auto-expires)
const lease = client.lease(30);  // 30 second lease

await lease.put('/services/user-service/instance-1').value(JSON.stringify({
  host: '127.0.0.1',
  port: 3000,
}));

// Discovery
const services = await client.getAll().prefix('/services/user-service/').strings();
```

## Next Steps

For patterns, continue to [Discovery Patterns](./04-discovery-patterns.md).
