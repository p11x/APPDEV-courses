# Service Discovery

## What You'll Learn
- Consul
- etcd
- Kubernetes DNS

## Prerequisites
- Completed microservices basics

## Consul

```bash
pip install python-consul
```

```python
import consul

c = consul.Consul()

# Register service
c.agent.service.register(
    'user-service',
    service_id='user-service-1',
    port=8000,
    check= consul.Check.http('http://localhost:8000/health', '10s')
)

# Discover service
_, services = c.agent.services()
user_service = services.get('user-service')
print(user_service['Address'], user_service['Port'])
```

## Kubernetes DNS

In Kubernetes, services are discoverable via DNS:
- `http://service-name.namespace.svc.cluster.local`

## Summary
- Use service discovery for dynamic IPs
- Consul, etcd are popular
- Kubernetes has built-in DNS

## Next Steps
→ Continue to `03-inter-service-communication.md`
