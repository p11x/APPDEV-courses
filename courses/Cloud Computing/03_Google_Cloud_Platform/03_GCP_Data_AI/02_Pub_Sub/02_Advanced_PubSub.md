---
Category: Google Cloud Platform
Subcategory: GCP Data AI
Concept: Pub/Sub
Purpose: Advanced understanding of GCP Pub/Sub features and configurations
Difficulty: intermediate
Prerequisites: 01_Basic_PubSub.md
RelatedFiles: 01_Basic_PubSub.md, 03_Practical_PubSub.md
UseCase: Enterprise messaging, event-driven architectures
CertificationExam: GCP Data Engineer
LastUpdated: 2025
---

## 💡 WHY

Advanced Pub/Sub knowledge enables building scalable event-driven architectures, implementing reliable messaging, and optimizing message processing.

### Why Advanced Pub/Sub

- **Ordering**: Message ordering and delivery
- **Dead-letter**: Failed message handling
- **Snapshots**: Message replay
- **Streaming**: Real-time data pipelines

## 📖 WHAT

### Advanced Features

**Message Ordering**:
- Enable ordering key
- Per-ordering key delivery
- Exactly-once delivery (beta)

**Dead-letter Topics**:
- Failed messages go here
- Configure retry limits
- Separate processing

**Snapshots**:
- Save subscription state
- Replay messages
- Schema versioning

### Delivery Patterns

| Pattern | Use Case | Configuration |
|---------|----------|---------------|
| Push | Low latency | HTTPS endpoint |
| Pull | High throughput | Async client |
| Pull+ | Combined | Hybrid |

## 🔧 HOW

### Example 1: Ordered Messaging with Dead-letter

```bash
# Create topic with ordering
gcloud pubsub topics create ordered-topic \
    --message-storage-policy-allowed-regions=us-central1,europe-west1 \
    --schema=proto-schema

# Enable ordering keys
gcloud pubsub topics update ordered-topic \
    --enable-message-ordering

# Create subscription with dead-letter
gcloud pubsub subscriptions create ordered-sub \
    --topic=ordered-topic \
    --ack-deadline=60 \
    --message-retention-duration=604800 \
    --dead-letter-topic=projects/my-project/topics/dlq-topic \
    --max-delivery-attempts=5 \
    --retain-acked-messages

# Set retry policy
gcloud pubsub subscriptions update ordered-sub \
    --min-retry-delay=10s \
    --max-retry-delay=600s
```

### Example 2: Push Subscription Configuration

```bash
# Create push subscription
gcloud pubsub subscriptions create push-sub \
    --topic=my-topic \
    --push-endpoint=https://my-function.region.functions.cloud.goog/my-function \
    --push-auth-service-account=push-sa@my-project.iam.gserviceaccount.com \
    --ack-deadline=30

# Configure push authentication
gcloud pubsub subscriptions update push-sub \
    --push-endpoint=https://my-app.example.com/webhook

# Test push subscription
gcloud pubsub topics publish my-topic \
    --message='{"test":"data"}'

# View subscription
gcloud pubsub subscriptions describe push-sub
```

### Example 3: Streaming Data Pipeline

```bash
# Create topic for streaming
gcloud pubsub topics create streaming-topic

# Create BigQuery subscription
gcloud pubsub subscriptions create bq-sub \
    --topic=streaming-topic \
    --bigquery-table=my-project:dataset.table \
    --use-topic-schema \
    --write-metadata

# Create Dataflow subscription
gcloud pubsub subscriptions create df-sub \
    --topic=streaming-topic

# Enable exactly-once delivery (beta)
gcloud pubsub subscriptions update df-sub \
    --enable-exactly-once-delivery

# Create snapshot for replay
gcloud pubsub snapshots create snapshot-$(date +%Y%m%d) \
    --subscription=my-subscription

# List snapshots
gcloud pubsub snapshots list
```

## ⚠️ COMMON ISSUES

### Troubleshooting Pub/Sub Issues

| Issue | Solution |
|-------|----------|
| Message delay | Check subscription backlog |
| Duplicate messages | Enable exactly-once |
| Push failures | Check endpoint auth |
| Ordering issues | Verify ordering keys |

### Performance Tuning

- Use pull for high throughput
- Configure appropriate ack deadline
- Use batching for bulk processing

## 🌐 COMPATIBILITY

### Cross-Platform Comparison

| Feature | GCP Pub/Sub | AWS SQS | Azure Event Hubs |
|---------|-------------|---------|------------------|
| Ordering | Yes | No | Yes |
| Dead-letter | Yes | Yes | Yes |
| Exactly-once | Beta | No | Limited |
| Push/Pull | Both | Pull only | Both |

## 🔗 CROSS-REFERENCES

### Related Topics

- Cloud Functions (triggers)
- Dataflow (streaming)
- BigQuery (sink)

### Study Resources

- Pub/Sub documentation
- Best practices for Pub/Sub

## ✅ EXAM TIPS

- Ordering keys ensure ordered delivery
- Dead-letter for failed messages
- Snapshots for message replay
- Exactly-once (beta) prevents duplicates
