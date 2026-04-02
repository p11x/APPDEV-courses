# AWS EventBridge

## What You'll Learn

- How to implement event-driven architecture with EventBridge
- How to create event buses and rules
- How to integrate with Lambda for event processing
- How to implement event routing and filtering

---

## Layer 1: Academic Foundation

### Event-Driven Patterns

EventBridge is a serverless event bus that connects application data to AWS services and custom targets.

**Components:**
- **Event Bus**: Default or custom event routing
- **Rules**: Pattern matching for event routing
- **Targets**: Lambda, SQS, SNS, API Destinations

---

## Layer 2: Code Evolution

### EventBridge Integration

```typescript
// eventbridge-handler.ts
import { EventBridgeEvent, Context } from 'aws-lambda';

type OrderEvent = {
  orderId: string;
  customerId: string;
  status: 'created' | 'paid' | 'shipped' | 'delivered';
  timestamp: number;
};

export const handler = async (
  event: EventBridgeEvent<string, OrderEvent>,
  context: Context
) => {
  const { orderId, status, customerId } = event.detail;
  
  console.log(`Processing order ${orderId} status: ${status}`);
  
  switch (status) {
    case 'created':
      await sendOrderConfirmation(customerId, orderId);
      break;
    case 'paid':
      await initiateFulfillment(orderId);
      break;
    case 'shipped':
      await notifyShipping(customerId, orderId);
      break;
    case 'delivered':
      await completeOrder(orderId);
      break;
  }
};

async function sendOrderConfirmation(customerId: string, orderId: string) {
  const eventbridge = new EventBridge();
  await eventbridge.putEvents({
    Entries: [{
      Source: 'order.service',
      DetailType: 'Notification',
      Detail: JSON.stringify({ customerId, message: 'Order confirmed' }),
      EventBusName: 'notifications'
    }]
  }).promise();
}
```

---

## Layer 3: Performance

### Event Filtering

```json
{
  "detail": {
    "status": ["paid", "shipped"],
    "amount": [{ "numeric": [">=", 100] }]
  }
}
```

---

## Layer 4: Security

### Access Control

```json
{
  "Version": "2012-10-17",
  "Statement": [{
    "Effect": "Allow",
    "Action": ["events:PutEvents"],
    "Resource": "arn:aws:events:us-east-1:123456789012:event-bus/default"
  }]
}
```

---

## Next Steps

Continue to [AWS SQS](./05-aws-sqs.md) for message queuing.