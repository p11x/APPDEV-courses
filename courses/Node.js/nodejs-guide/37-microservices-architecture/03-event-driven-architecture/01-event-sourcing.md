# Event Sourcing

## What You'll Learn

- What event sourcing is
- How to implement event sourcing with Node.js
- How event sourcing differs from CRUD
- How to rebuild state from events

## What Is Event Sourcing?

Instead of storing the current state, event sourcing stores **every change as an immutable event**. The current state is derived by replaying events.

```
CRUD:       users table → { id: 1, name: "Alice", email: "alice@new.com" }
Event Sourcing: events table →
  1. UserCreated { id: 1, name: "Alice", email: "alice@old.com" }
  2. UserUpdated { id: 1, field: "email", value: "alice@new.com" }
```

## Implementation

```ts
// event-store.ts

interface Event {
  id: string;
  aggregateId: string;
  type: string;
  data: Record<string, unknown>;
  timestamp: string;
  version: number;
}

class EventStore {
  private events: Event[] = [];

  async append(aggregateId: string, type: string, data: Record<string, unknown>): Promise<Event> {
    const existing = this.events.filter((e) => e.aggregateId === aggregateId);
    const version = existing.length + 1;

    const event: Event = {
      id: crypto.randomUUID(),
      aggregateId,
      type,
      data,
      timestamp: new Date().toISOString(),
      version,
    };

    this.events.push(event);
    return event;
  }

  async getEvents(aggregateId: string): Promise<Event[]> {
    return this.events
      .filter((e) => e.aggregateId === aggregateId)
      .sort((a, b) => a.version - b.version);
  }
}

// Aggregate — rebuilds state from events
class UserAggregate {
  id: string;
  name: string = '';
  email: string = '';
  version: number = 0;

  constructor(id: string) {
    this.id = id;
  }

  apply(event: Event) {
    switch (event.type) {
      case 'UserCreated':
        this.name = event.data.name as string;
        this.email = event.data.email as string;
        break;
      case 'UserUpdated':
        const field = event.data.field as string;
        this[field] = event.data.value;
        break;
    }
    this.version = event.version;
  }
}

// Usage
const store = new EventStore();

// Create user
await store.append('user-1', 'UserCreated', { name: 'Alice', email: 'alice@example.com' });

// Update email
await store.append('user-1', 'UserUpdated', { field: 'email', value: 'alice@new.com' });

// Rebuild state
const events = await store.getEvents('user-1');
const user = new UserAggregate('user-1');
events.forEach((e) => user.apply(e));

console.log(user);  // { id: 'user-1', name: 'Alice', email: 'alice@new.com', version: 2 }
```

## Next Steps

For CQRS, continue to [CQRS Pattern](./02-cqrs-pattern.md).
