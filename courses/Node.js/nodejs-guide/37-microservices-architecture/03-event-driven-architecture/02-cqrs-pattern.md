# CQRS Pattern

## What You'll Learn

- What CQRS (Command Query Responsibility Segregation) is
- How to separate read and write models
- How to implement CQRS with Node.js
- How CQRS combines with event sourcing

## What Is CQRS?

CQRS separates **commands** (writes) from **queries** (reads) into different models. Writes go through a command handler that validates and emits events. Reads come from a separate read model optimized for queries.

```
Commands → Command Handler → Event Store → Events → Read Model Projections
Queries → Read Model (optimized for queries)
```

## Implementation

```ts
// cqrs.ts

import { EventEmitter } from 'node:events';

// Command Handler
class CreateUserCommand {
  constructor(
    public readonly userId: string,
    public readonly name: string,
    public readonly email: string
  ) {}
}

class CommandHandler {
  constructor(
    private eventStore: EventStore,
    private eventBus: EventEmitter
  ) {}

  async handle(command: CreateUserCommand) {
    // Validate
    if (!command.email.includes('@')) {
      throw new Error('Invalid email');
    }

    // Store event
    await this.eventStore.append(command.userId, 'UserCreated', {
      name: command.name,
      email: command.email,
    });

    // Publish event for read model projection
    this.eventBus.emit('UserCreated', {
      id: command.userId,
      name: command.name,
      email: command.email,
    });
  }
}

// Read Model
class UserReadModel {
  private users: Map<string, { id: string; name: string; email: string }> = new Map();

  constructor(eventBus: EventEmitter) {
    eventBus.on('UserCreated', (data) => {
      this.users.set(data.id, data);
    });

    eventBus.on('UserUpdated', (data) => {
      const user = this.users.get(data.id);
      if (user) user[data.field] = data.value;
    });
  }

  getUser(id: string) {
    return this.users.get(id);
  }

  getAllUsers() {
    return [...this.users.values()];
  }
}

// Usage
const eventBus = new EventEmitter();
const eventStore = new EventStore();
const commandHandler = new CommandHandler(eventStore, eventBus);
const readModel = new UserReadModel(eventBus);

// Write (command)
await commandHandler.handle(new CreateUserCommand('1', 'Alice', 'alice@example.com'));

// Read (query) — from read model, not event store
const user = readModel.getUser('1');
console.log(user);  // { id: '1', name: 'Alice', email: 'alice@example.com' }
```

## Next Steps

For event store, continue to [Event Store](./03-event-store.md).
