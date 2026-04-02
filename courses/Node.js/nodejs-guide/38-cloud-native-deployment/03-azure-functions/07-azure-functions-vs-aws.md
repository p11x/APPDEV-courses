# Azure Functions vs AWS Lambda

## What You'll Learn

- Comparison between Azure Functions and AWS Lambda
- Decision criteria for choosing the right platform
- Feature parity analysis
- Migration considerations

---

## Layer 1: Platform Comparison

| Feature | Azure Functions | AWS Lambda |
|---------|-----------------|-------------|
| Runtime | Node.js 18, 20, 22 | Node.js 18, 20 |
| Cold Start | 200-800ms | 100-800ms |
| Max Execution | 5-30 minutes | 15 minutes |
| Memory | 128MB-4GB | 128MB-10GB |
| Free Tier | 1M requests/month | 1M requests/month |
| Scaling | Auto, max 200 | Auto, burst limit |
| Integrations | Azure ecosystem | AWS ecosystem |

---

## Layer 2: Decision Matrix

**Choose Azure Functions when:**
- Existing Azure infrastructure
- Integration with Microsoft services
- Durable Functions for workflows
- Enterprise requirements

**Choose AWS Lambda when:**
- Existing AWS infrastructure
- Lambda + API Gateway maturity
- Cost optimization needs
- Larger ecosystem

---

## Next Steps

Continue to [Azure Functions Best Practices](./08-azure-functions-best-practices.md)