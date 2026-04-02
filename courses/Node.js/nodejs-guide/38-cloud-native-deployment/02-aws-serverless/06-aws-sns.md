# AWS SNS

## What You'll Learn

- How to implement pub/sub messaging with SNS
- How to configure topic subscriptions
- How to implement fanout patterns
- How to integrate with Lambda and SQS

---

## Layer 1: Academic Foundation

### SNS Architecture

Amazon SNS is a pub/sub messaging service that enables message delivery to subscribers via multiple protocols.

---

## Layer 2: Code Evolution

### SNS Publisher/Subscriber

```typescript
// sns-handler.ts
import { SNSEvent, SNSPublishParams } from 'aws-lambda';

const sns = new SNS();

export const publishOrderEvent = async (order: Order) => {
  const params: SNSPublishParams = {
    TopicArn: process.env.ORDER_TOPIC_ARN,
    Subject: `Order ${order.orderId} Status Update`,
    Message: JSON.stringify({
      orderId: order.orderId,
      customerId: order.customerId,
      status: order.status,
      timestamp: Date.now()
    }),
    MessageAttributes: {
      priority: {
        DataType: 'String',
        StringValue: order.priority || 'normal'
      }
    }
  };
  
  const result = await sns.publish(params).promise();
  return result.MessageId;
};

export const handler = async (event: SNSEvent) => {
  for (const record of event.Records) {
    const message = JSON.parse(record.Sns.Message);
    console.log('Received SNS message:', message);
    
    await processEvent(message);
  }
};
```

---

## Layer 3: Performance

### Fanout Pattern

```
SNS Topic
    ├── Lambda (Real-time processing)
    ├── SQS Queue (Async processing)
    ├── Email (Notifications)
    └── HTTP (Webhooks)
```

---

## Layer 4: Security

### Access Control

```json
{
  "Version": "2012-10-17",
  "Statement": [{
    "Effect": "Allow",
    "Action": ["sns:Publish"],
    "Resource": "arn:aws:sns:us-east-1:123456789012:orders"
  }]
}
```

---

## Next Steps

Continue to [AWS Step Functions](./07-aws-step-functions.md) for workflow orchestration.