---
Category: Google Cloud Platform
Subcategory: GCP Data AI
Concept: Pub/Sub
Purpose: Understanding GCP Pub/Sub messaging service
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_PubSub.md, 03_Practical_PubSub.md
UseCase: Event-driven messaging on GCP
CertificationExam: GCP Data Engineer
LastUpdated: 2025
---

## 💡 WHY

Pub/Sub provides global, scalable messaging on GCP. Understanding pub/sub is essential for event-driven architectures.

## 📖 WHAT

### Pub/Sub Features

- **Topics**: Message channels
- **Subscriptions**: Message receivers
- **Push/Pull**: Delivery methods
- **Ordering**: Message ordering
- **Dead-letter**: Failed message handling

## 🔧 HOW

### Example: Create Topic

```bash
# Create topic
gcloud pubsub topics create my-topic

# Create subscription
gcloud pubsub subscriptions create my-sub \
    --topic=my-topic

# Publish message
gcloud pubsub topics publish my-topic \
    --message "Hello World"

# Pull messages
gcloud pubsub subscriptions pull my-sub \
    --auto-ack
```

## ✅ EXAM TIPS

- Global messaging service
- Push or pull delivery
- At-least-once delivery by default