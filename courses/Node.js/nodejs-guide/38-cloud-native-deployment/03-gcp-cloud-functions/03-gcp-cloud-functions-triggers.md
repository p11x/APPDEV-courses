# GCP Cloud Functions Triggers

## What You'll Learn

- Different trigger types in GCP Cloud Functions
- How to configure HTTP, Cloud Storage, and Pub/Sub triggers
- How to handle event-driven functions
- How to implement trigger-specific logic

---

## Layer 1: Trigger Types

| Trigger | Description | Use Case |
|---------|-------------|----------|
| HTTP | Direct HTTP requests | REST APIs |
| Cloud Storage | Object changes | File processing |
| Pub/Sub | Message events | Async processing |
| Firestore | Database changes | Real-time triggers |
| Cloud Scheduler | Scheduled jobs | Cron tasks |

---

## Layer 2: Pub/Sub Trigger

```javascript
exports.processMessage = (message, context) => {
  const data = Buffer.from(message.data, 'base64').toString();
  const json = JSON.parse(data);
  
  console.log('Message ID:', context.eventId);
  console.log('Timestamp:', context.timestamp);
  console.log('Data:', json);
  
  message.ack();
};
```

---

## Next Steps

Continue to [GCP Cloud Functions Deployment](./04-gcp-cloud-functions-deployment.md)