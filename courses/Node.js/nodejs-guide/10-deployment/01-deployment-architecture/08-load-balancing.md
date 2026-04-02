# Load Balancing

## What You'll Learn

- Why load balancing matters
- Round-robin with nginx
- Sticky sessions
- Health-check-based routing

## nginx Load Balancing

```nginx
upstream nodejs_cluster {
    least_conn;  # Send to server with fewest connections
    server 127.0.0.1:3000;
    server 127.0.0.1:3001;
    server 127.0.0.1:3002;
    server 127.0.0.1:3003;
}
```

## Next Steps

For horizontal scaling, continue to [Horizontal Scaling](./02-horizontal-scaling.md).
