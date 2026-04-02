# Fastly Compute Deployment

## What You'll Learn

- How to deploy Fastly Compute services
- How to configure backends
- How to manage environments

## Deployment

```bash
# Build
fastly compute build

# Deploy to production
fastly compute deploy

# Deploy to staging
fastly compute deploy --service-id=xxx --version=latest
```

## Configuration

```toml
# fastly.toml

[setup]
  [setup.backends]
    [setup.backends.origin]
      address = "origin.example.com"
      port = 443

[local_server]
  [local_server.backends]
    [local_server.backends.origin]
      url = "https://localhost:3000"
```

## Next Steps

For comparison with Workers, continue to [Compute vs Workers](./04-compute-vs-workers.md).
