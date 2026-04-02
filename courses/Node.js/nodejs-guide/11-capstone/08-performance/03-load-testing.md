# Load Testing

## What You'll Learn

- Load testing the capstone API
- Identifying performance bottlenecks
- Setting performance baselines

## Autocannon Tests

```bash
# Test bookmark listing
autocannon -c 100 -d 10 http://localhost:3000/api/bookmarks

# Test with authentication
autocannon -c 100 -d 10 \
  -H "Authorization=Bearer <token>" \
  http://localhost:3000/api/bookmarks

# Test bookmark creation
autocannon -c 50 -d 10 \
  -m POST \
  -H "Content-Type=application/json" \
  -H "Authorization=Bearer <token>" \
  -b '{"url":"https://example.com","title":"Test"}' \
  http://localhost:3000/api/bookmarks
```

## Performance Baseline

| Endpoint | Target (req/s) | Target (p99) |
|----------|---------------|--------------|
| GET /api/bookmarks | 5,000 | 20ms |
| POST /api/bookmarks | 2,000 | 50ms |
| GET /api/bookmarks/:id | 8,000 | 10ms |
| GET /healthz | 50,000 | 2ms |

## Next Steps

For deployment, continue to [Dockerfile](../09-deployment/01-dockerfile.md).
