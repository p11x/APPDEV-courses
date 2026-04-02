# AWS Lambda Powertools

## What You'll Learn

- How to use Lambda Powertools for Node.js
- How to implement structured logging
- How to set up tracing with X-Ray
- How to implement middleware patterns

---

## Layer 1: Academic Foundation

### Powertools Overview

AWS Lambda Powertools is a library providing utilities for building serverless applications.

**Core Utilities:**
- **Logger**: Structured logging with correlation IDs
- **Tracer**: Distributed tracing with X-Ray
- **Metrics**: CloudWatch Metrics creation
- **Parameters**: Parameter store and secrets integration

---

## Layer 2: Code Evolution

### Logger Implementation

```typescript
import { Logger, LogLevel } from '@aws-lambda-powertools/logger';

const logger = new Logger({
  serviceName: 'order-service',
  logLevel: LogLevel.INFO,
  sampleRate: 0.1
});

export const handler = async (event: OrderEvent) => {
  logger.addContext({ orderId: event.orderId });
  
  logger.info('Processing order', {
    customerId: event.customerId,
    amount: event.amount
  });
  
  try {
    await processOrder(event);
    logger.info('Order processed successfully');
  } catch (error) {
    logger.error('Failed to process order', { error });
    throw error;
  }
};
```

### Tracer Implementation

```typescript
import { Tracer } from '@aws-lambda-powertools/tracer';

const tracer = new Tracer({ serviceName: 'order-service' });

export const handler = async (event: OrderEvent) => {
  const segment = tracer.getSegment();
  
  const orderSpan = tracer.addSubsegment('order-processing');
  try {
    const order = await getOrder(event.orderId);
    orderSpan.addMetadata('orderStatus', order.status);
    
    const paymentSpan = tracer.addSubsegment('payment');
    await processPayment(order);
    paymentSpan.close();
    
    return { success: true };
  } catch (error) {
    orderSpan.addError(error);
    throw error;
  } finally {
    orderSpan.close();
  }
};
```

---

## Layer 3: Performance

### Impact Metrics

| Feature | Overhead |
|---------|----------|
| Logger | < 1ms |
| Tracer | < 2ms |
| Metrics | < 1ms |

---

## Next Steps

Continue to [Serverless Framework](./09-aws-serverless-framework.md) for deployment tooling.