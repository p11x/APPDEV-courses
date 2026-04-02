# Event Versioning

## What You'll Learn

- Why event versioning matters
- How to handle schema evolution
- How to implement upcasters
- How to handle breaking changes

## Why Version Events?

As your application evolves, event schemas change. Old events stored in your event store must still be processable.

```
V1: UserCreated { name, email }
V2: UserCreated { name, email, phone }
V3: UserCreated { name, email, phone, preferences }
```

## Upcaster Pattern

```ts
// Upcast events from old versions to current version

function upcastUserCreated(event: any): any {
  switch (event.version) {
    case 1:
      // V1 → V2: add phone field
      return { ...event, data: { ...event.data, phone: null }, version: 2 };

    case 2:
      // V2 → V3: add preferences
      return {
        ...event,
        data: { ...event.data, preferences: { theme: 'light', language: 'en' } },
        version: 3,
      };

    default:
      return event;  // Already current version
  }
}

function upcast(event: any): any {
  const upcasters: Record<string, (e: any) => any> = {
    'UserCreated': upcastUserCreated,
    // Add more event type upcasters
  };

  let current = event;
  while (current.version < CURRENT_VERSION) {
    const upcaster = upcasters[current.type];
    if (!upcaster) throw new Error(`No upcaster for ${current.type} v${current.version}`);
    current = upcaster(current);
  }

  return current;
}
```

## Best Practices

- Always include a version field in events
- Never modify stored events — always upcast on read
- Add new fields as optional with defaults
- Remove fields by making them optional first

## Next Steps

For gRPC, continue to [gRPC Node.js](../04-service-communication/01-grpc-node.md).
