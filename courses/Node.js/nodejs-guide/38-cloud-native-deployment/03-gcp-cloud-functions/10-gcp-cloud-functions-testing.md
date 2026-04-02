# GCP Cloud Functions Testing

## What You'll Learn

- Testing Cloud Functions locally
- Unit and integration testing
- Mocking dependencies
- CI/CD integration

---

## Layer 1: Testing

### Local Testing

```bash
# Test HTTP function
functions-framework --target=helloHttp --port=8080
curl http://localhost:8080?name=Test
```

---

## Next Steps

Continue to [GCP Cloud Functions Optimization](./11-gcp-cloud-functions-optimization.md)