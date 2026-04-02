# gRPC vs REST

## What You'll Learn

- When to use gRPC vs REST
- Performance comparison
- How to choose between them
- How to use both together

## Comparison

| Feature | REST | gRPC |
|---------|------|------|
| Protocol | HTTP/1.1 | HTTP/2 |
| Format | JSON | Protobuf |
| Size | Larger | 10x smaller |
| Speed | Moderate | Fast |
| Browser support | Native | Needs proxy |
| Streaming | No | Bidirectional |
| Type safety | No | Yes |
| Debugging | Easy (curl) | Harder |
| Best for | Public APIs | Internal services |

## When to Use gRPC

- Service-to-service communication (microservices)
- High-throughput, low-latency requirements
- Streaming data (real-time updates)
- Strong type contracts needed

## When to Use REST

- Public APIs (browser clients)
- Simple CRUD operations
- When debugging simplicity matters
- When ecosystem compatibility matters

## Hybrid Approach

```
Public clients → REST API Gateway → gRPC → Internal services
Internal services → gRPC → gRPC
```

## Next Steps

For distributed tracing, continue to [Distributed Tracing Setup](../05-distributed-tracing/01-distributed-tracing-setup.md).
