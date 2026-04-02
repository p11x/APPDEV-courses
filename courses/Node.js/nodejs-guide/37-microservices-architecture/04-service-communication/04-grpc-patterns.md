# gRPC Patterns

## What You'll Learn

- Streaming patterns with gRPC
- Error handling in gRPC
- Interceptors and middleware
- Load balancing with gRPC

## Server Streaming

```ts
// Server sends multiple responses

server.addService(proto.NotificationService.service, {
  subscribe: (call) => {
    const userId = call.request.userId;

    // Send notifications as they arrive
    const interval = setInterval(() => {
      call.write({
        type: 'notification',
        message: 'New message',
        timestamp: new Date().toISOString(),
      });
    }, 5000);

    call.on('cancelled', () => {
      clearInterval(interval);
    });
  },
});
```

## Client Streaming

```ts
// Client sends multiple requests

import { credentials, loadPackageDefinition } from '@grpc/grpc-js';

const client = new proto.LogService('localhost:50051', credentials.createInsecure());

const call = client.collectLogs((err, response) => {
  console.log('Total logs:', response.total);
});

// Send multiple log entries
for (const log of logs) {
  call.write(log);
}

call.end();
```

## Bidirectional Streaming

```ts
// Both client and server stream

const call = client.chat();

call.on('data', (message) => {
  console.log('Server:', message.text);
});

call.write({ text: 'Hello' });
call.write({ text: 'How are you?' });
call.end();
```

## Next Steps

For comparison, continue to [gRPC vs REST](./05-grpc-vs-rest.md).
