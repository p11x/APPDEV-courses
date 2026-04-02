# GCP Cloud Functions Deployment

## What You'll Learn

- How to deploy Cloud Functions to GCP
- How to configure deployment settings
- How to manage function versions
- How to set up CI/CD for functions

---

## Layer 1: Deployment

### Deploy Command

```bash
# Deploy HTTP function
gcloud functions deploy helloHttp \
  --runtime nodejs18 \
  --trigger-http \
  --allow-unauthenticated \
  --region us-central1

# Deploy Pub/Sub function
gcloud functions deploy processMessage \
  --runtime nodejs18 \
  --trigger-topic my-topic \
  --region us-central1
```

---

## Next Steps

Continue to [GCP Cloud Functions Monitoring](./05-gcp-cloud-functions-monitoring.md)