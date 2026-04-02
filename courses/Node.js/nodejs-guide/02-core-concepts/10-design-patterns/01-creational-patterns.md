# Creational Design Patterns: Singleton and Factory

## What You'll Learn

- Singleton pattern implementation
- Factory pattern for object creation
- When to use each pattern
- Testing considerations

## Singleton Pattern

```javascript
// Module-based singleton (Node.js natural pattern)
// config.js — Module is cached, acts as singleton
const config = {
    port: process.env.PORT || 3000,
    db: { host: process.env.DB_HOST || 'localhost' },
};

Object.freeze(config); // Prevent modification
export default config;

// Class-based singleton
class Database {
    static #instance = null;

    constructor(url) {
        if (Database.#instance) {
            return Database.#instance;
        }
        this.url = url;
        this.connected = false;
        Database.#instance = this;
    }

    static getInstance(url) {
        if (!Database.#instance) {
            Database.#instance = new Database(url);
        }
        return Database.#instance;
    }

    async connect() {
        if (this.connected) return;
        this.connected = true;
        console.log(`Connected to ${this.url}`);
    }
}

const db1 = Database.getInstance('postgres://localhost/mydb');
const db2 = Database.getInstance('postgres://localhost/other');
console.log(db1 === db2); // true — same instance
```

## Factory Pattern

```javascript
// Factory for creating different notification types
class NotificationFactory {
    static create(type, options) {
        switch (type) {
            case 'email':
                return new EmailNotification(options);
            case 'sms':
                return new SMSNotification(options);
            case 'push':
                return new PushNotification(options);
            default:
                throw new Error(`Unknown type: ${type}`);
        }
    }
}

class EmailNotification {
    constructor({ to, subject, body }) {
        this.to = to;
        this.subject = subject;
        this.body = body;
    }

    async send() {
        console.log(`Email to ${this.to}: ${this.subject}`);
    }
}

class SMSNotification {
    constructor({ to, message }) {
        this.to = to;
        this.message = message;
    }

    async send() {
        console.log(`SMS to ${this.to}: ${this.message}`);
    }
}

// Usage
const notification = NotificationFactory.create('email', {
    to: 'alice@example.com',
    subject: 'Welcome',
    body: 'Hello!',
});
await notification.send();
```

## Best Practices Checklist

- [ ] Use module exports for natural singletons in Node.js
- [ ] Use factory pattern when object type varies at runtime
- [ ] Make singletons testable (allow dependency injection)
- [ ] Freeze singleton configuration objects

## Cross-References

- See [Behavioral Patterns](./02-behavioral-patterns.md) for Observer/Strategy
- See [Structural Patterns](./03-structural-patterns.md) for Adapter/Decorator
- See [Error Handling](../11-error-handling/01-error-propagation.md) for error patterns

## Next Steps

Continue to [Behavioral Patterns](./02-behavioral-patterns.md) for Observer and Strategy.
