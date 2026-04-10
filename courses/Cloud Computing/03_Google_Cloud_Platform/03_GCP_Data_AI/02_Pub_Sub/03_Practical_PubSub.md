---
Category: Google Cloud Platform
Subcategory: GCP Data AI
Concept: Pub/Sub
Purpose: Hands-on exercises for Pub/Sub configuration and management
Difficulty: advanced
Prerequisites: 01_Basic_PubSub.md, 02_Advanced_PubSub.md
RelatedFiles: 01_Basic_PubSub.md, 02_Advanced_PubSub.md
UseCase: Event-driven architectures, real-time messaging
CertificationExam: GCP Data Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with Pub/Sub is essential for building event-driven architectures and managing real-time messaging systems.

### Lab Goals

- Configure advanced messaging patterns
- Implement dead-letter handling
- Build streaming pipelines

## 📖 WHAT

### Exercise Overview

1. **Reliable Messaging**: Ordering, dead-letter
2. **Push/Pull**: Different delivery modes
3. **Integration**: BigQuery, Dataflow

## 🔧 HOW

### Exercise 1: Configure Reliable Messaging

```bash
#!/bin/bash
# Configure reliable Pub/Sub messaging

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Create topic with ordering
gcloud pubsub topics create order-topic \
    --message-storage-policy-allowed-regions=us-central1,europe-west1

gcloud pubsub topics update order-topic \
    --enable-message-ordering

# Create dead-letter topic
gcloud pubsub topics create order-dlq

# Create subscription with dead-letter
gcloud pubsub subscriptions create order-sub \
    --topic=order-topic \
    --ack-deadline=60 \
    --dead-letter-topic=order-dlq \
    --max-delivery-attempts=5 \
    --retain-acked-messages \
    --message-retention-duration=604800

# Set retry policy
gcloud pubsub subscriptions update order-sub \
    --min-retry-delay=10s \
    --max-retry-delay=600s

# Publish ordered messages
gcloud pubsub topics publish order-topic \
    --message='{"orderId":"1001","item":"A"}' \
    --ordering-key=1001

gcloud pubsub topics publish order-topic \
    --message='{"orderId":"1001","item":"B"}' \
    --ordering-key=1001

# Pull messages
gcloud pubsub subscriptions pull order-sub \
    --limit=10 \
    --auto-ack

echo "Reliable messaging configured!"
```

### Exercise 2: Configure Push Subscription

```bash
#!/bin/bash
# Configure push subscription

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Create topic
gcloud pubsub topics create webhook-topic

# Create push subscription
gcloud pubsub subscriptions create webhook-sub \
    --topic=webhook-topic \
    --push-endpoint=https://my-function.region.functions.cloud.goog/my-function \
    --ack-deadline=30

# Verify subscription
gcloud pubsub subscriptions describe webhook-sub

# Test publishing
gcloud pubsub topics publish webhook-topic \
    --message='{"webhook":"test"}'

echo "Push subscription configured!"
```

### Exercise 3: Build Streaming Pipeline

```bash
#!/bin/bash
# Build streaming pipeline with BigQuery

PROJECT_ID="my-project-id"
DATASET_NAME="streaming_data"

gcloud config set project $PROJECT_ID

# Create BigQuery dataset
bq mk --dataset $PROJECT_ID:$DATASET_NAME

# Create streaming topic
gcloud pubsub topics create analytics-topic

# Create BigQuery subscription
gcloud pubsub subscriptions create analytics-sub \
    --topic=analytics-topic \
    --bigquery-table=$PROJECT_ID:$DATASET_NAME.events \
    --use-topic-schema \
    --write-metadata

# Publish sample data
for i in {1..100}; do
    gcloud pubsub topics publish analytics-topic \
        --message="{\"userId\":\"user_$i\",\"event\":\"page_view\",\"timestamp\":\"$(date -Iseconds)\"}"
done

# Query data in BigQuery
bq query --use_legacy_sql=false \
    "SELECT * FROM $PROJECT_ID.$DATASET_NAME.events LIMIT 10"

# Check subscription metrics
gcloud pubsub subscriptions describe analytics-sub

echo "Streaming pipeline configured!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Message delay | Check backlog |
| Push failures | Check endpoint |
| Duplicates | Enable exactly-once |

### Validation

```bash
# Check subscription status
gcloud pubsub subscriptions describe my-sub

# View message backlog
gcloud pubsub subscriptions get-acceptance-pattern my-sub
```

## 🌐 COMPATIBILITY

### Integration

- Cloud Functions
- Dataflow
- BigQuery

## 🔗 CROSS-REFERENCES

### Related Labs

- Cloud Functions
- Dataflow
- BigQuery

### Next Steps

- Set up Dataflow streaming job
- Configure monitoring
- Implement alerting

## ✅ EXAM TIPS

- Practice ordering key usage
- Know dead-letter configuration
- Understand push vs pull
- Monitor subscription metrics
