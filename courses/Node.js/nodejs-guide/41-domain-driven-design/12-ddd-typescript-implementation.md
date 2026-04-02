# DDD TypeScript Implementation

## What You'll Learn

- Framework setup for DDD
- Decorators and metadata
- Dependency injection
- Modular project structure

---

## Project Structure

```
src/
├── domain/
│   ├── entities/
│   ├── value-objects/
│   ├── aggregates/
│   ├── events/
│   └── services/
├── application/
│   ├── commands/
│   ├── queries/
│   └── services/
├── infrastructure/
│   ├── repositories/
│   ├── persistence/
│   └── external/
└── api/
    ├── controllers/
    └── routes/
```

---

## Base Classes

```typescript
// @libs/ddd/index.ts
export abstract class Entity<T> {
  constructor(protected readonly props: T) {}
  
  public abstract equals(other: Entity<T>): boolean;
}

export abstract class AggregateRoot<T> extends Entity<T> {
  private domainEvents: DomainEvent[] = [];
  
  protected addDomainEvent(event: DomainEvent): void {
    this.domainEvents.push(event);
  }
  
  public pullDomainEvents(): DomainEvent[] {
    const events = [...this.domainEvents];
    this.domainEvents = [];
    return events;
  }
}
```

---

## Next Steps

Continue to [Reactive Programming](./42-reactive-programming/01-reactive-introduction.md)