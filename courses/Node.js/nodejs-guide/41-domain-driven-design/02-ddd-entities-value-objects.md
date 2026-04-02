# DDD Entities and Value Objects

## What You'll Learn

- Entity identity management
- Value Object immutability
- Business rule enforcement
- TypeScript implementations

---

## Entities

```typescript
// Entities with identity
export class Entity<T> {
  constructor(protected readonly props: T) {}
  
  public equals(other?: Entity<T>): boolean {
    if (!other || !other.props) return false;
    return JSON.stringify(this.props) === JSON.stringify(other.props);
  }
}
```

---

## Value Objects

```typescript
// Immutable value objects
export class Money {
  constructor(
    public readonly amount: number,
    public readonly currency: string
  ) {
    if (amount < 0) throw new Error('Amount cannot be negative');
  }
  
  add(other: Money): Money {
    if (this.currency !== other.currency) {
      throw new Error('Currency mismatch');
    }
    return new Money(this.amount + other.amount, this.currency);
  }
}
```