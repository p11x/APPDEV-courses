# Message Patterns

## What You'll Learn

- Common messaging patterns for microservices
- How to implement publish/subscribe
- How to implement request/reply
- How to handle message ordering

## Publish/Subscribe

```ts
// pubsub.ts

import { EventEmitter } from 'node:events';

class EventBus {
  private emitter = new EventEmitter();

  publish(topic: string, data: unknown) {
    this.emitter.emit(topic, data);
  }

  subscribe(topic: string, handler: (data: unknown) => void) {
    this.emitter.on(topic, handler);
  }
}

// Usage
const bus = new EventBus();

bus.subscribe('user.created', (data) => {
  console.log('Send welcome email to:', data);
});

bus.publish('user.created', { email: 'alice@example.com' });
```

## Request/Reply

```ts
// request-reply.ts

import { randomUUID } from 'node:crypto';

class RequestReplyClient {
  private pending = new Map<string, { resolve: Function; reject: Function }>();

  async request(service: string, data: unknown): Promise<unknown> {
    const correlationId = randomUUID();

    return new Promise((resolve, reject) => {
      this.pending.set(correlationId, { resolve, reject });

      // Send request with correlation ID
      this.send(service, { correlationId, data });

      // Timeout after 5 seconds
      setTimeout(() => {
        if (this.pending.has(correlationId)) {
          this.pending.delete(correlationId);
          reject(new Error('Request timeout'));
        }
      }, 5000);
    });
  }

  handleReply(correlationId: string, result: unknown) {
    const pending = this.pending.get(correlationId);
    if (pending) {
      this.pending.delete(correlationId);
      pending.resolve(result);
    }
  }
}
```

## Next Steps

For event versioning, continue to [Event Versioning](./05-event-versioning.md).
