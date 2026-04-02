# AWS SQS

## What You'll Learn

- How to implement message queuing with SQS
- How to configure dead-letter queues
- How to implement FIFO queues for ordering
- How to integrate SQS with Lambda

---

## Layer 1: Academic Foundation

### Queue Types

| Type | Description | Use Case |
|------|-------------|----------|
| Standard | Best-effort ordering, unlimited throughput | High throughput, async processing |
| FIFO | Guaranteed ordering, exactly-once | Financial, order processing |

---

## Layer 2: Code Evolution

### SQS Consumer

```typescript
// sqs-consumer.ts
import { SQSHandler, SQSEvent } from 'aws-lambda';

export const handler: SQSHandler = async (event: SQSEvent) => {
  for (const record of event.Records) {
    try {
      const message = JSON.parse(record.body);
      const { type, payload } = message;
      
      console.log(`Processing message: ${record.messageId}`);
      
      await processMessage(type, payload);
      
      console.log(`Successfully processed ${record.messageId}`);
    } catch (error) {
      console.error(`Error processing ${record.messageId}:`, error);
      throw error;
    }
  }
};

async function processMessage(type: string, payload: object) {
  switch (type) {
    case 'USER_CREATED':
      await handleUserCreated(payload);
      break;
    case 'ORDER_PLACED':
      await handleOrderPlaced(payload);
      break;
    default:
      console.warn(`Unknown message type: ${type}`);
  }
}
```

---

## Layer 3: Performance

### Batch Processing

```yaml
# serverless.yml
functions:
  processQueue:
    handler: handler.handler
    events:
      - sqs:
          arn: !GetAtt MyQueue.Arn
          batchSize: 10
          functionResponseType: ReportBatchItemFailures
```

---

## Layer 4: Security

### Encryption at Rest

- Server-side encryption enabled by default
- KMS CMK for customer-managed keys

---

## Next Steps

Continue to [AWS SNS](./06-aws-sns.md) for pub/sub messaging.