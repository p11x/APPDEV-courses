# GCP Cloud Functions Security

## What You'll Learn

- Securing Cloud Functions
- IAM and authentication
- VPC and network security
- Handling sensitive data

---

## Layer 1: Security

### IAM Configuration

```bash
# Deploy with authentication
gcloud functions deploy helloSecure \
  --runtime nodejs18 \
  --trigger-http \
  --require-authentication

# Set IAM policy
gcloud functions add-iam-policy-binding helloSecure \
  --member=serviceAccount:my-app@my-project.iam.gserviceaccount.com \
  --role=cloudfunctions.invoker
```

---

## Next Steps

Continue to [GCP Cloud Functions Performance](./09-gcp-cloud-functions-performance.md)