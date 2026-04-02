# GCP Cloud Functions vs AWS Lambda

## What You'll Learn

- Comparison between GCP Cloud Functions and AWS Lambda
- Feature parity analysis
- Decision criteria for choosing the right platform
- Migration considerations

---

## Layer 1: Platform Comparison

| Feature | GCP Cloud Functions | AWS Lambda |
|---------|-------------------|------------|
| Runtime | Node.js 18, 20 | Node.js 18, 20 |
| Cold Start | 100-500ms | 100-800ms |
| Max Execution | 9-60 minutes | 15 minutes |
| Memory | 128MB-8GB | 128MB-10GB |
| Free Tier | 2M invocations | 1M requests |
| Scaling | Auto, max 1000 | Auto, burst limit |

---

## Next Steps

Continue to [GCP Cloud Functions Best Practices](./07-gcp-cloud-functions-best-practices.md)