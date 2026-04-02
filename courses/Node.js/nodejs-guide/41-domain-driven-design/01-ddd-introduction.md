# Domain-Driven Design in Node.js

## What You'll Learn

- Understanding Domain-Driven Design principles
- Implementing DDD patterns in TypeScript/Node.js
- Entity, Value Object, and Aggregate patterns
- Repository and Domain Service implementations
- Event Sourcing and CQRS patterns

---

## Layer 1: Academic Foundation

### What is Domain-Driven Design?

Domain-Driven Design (DDD) is an approach to software development that emphasizes understanding the business domain through intensive collaboration between domain experts and software developers.

**Core Principles:**
- **Ubiquitous Language**: Shared vocabulary between tech and business teams
- **Bounded Contexts**: Explicit boundaries around domain models
- **Aggregates**: Clusters of related objects treated as single units
- **Domain Events**: Capturing significant occurrences in the domain

### Historical Context

- **Eric Evans** published "Domain-Driven Design: Tackling Complexity in the Heart of Software" (2003)
- **Vaughn Vernon** authored "Implementing Domain-Driven Design" (2013)
- Modern Node.js frameworks now support DDD patterns natively

---

## Layer 2: Multi-Paradigm Code Evolution

### Paradigm 1 — Entities

```typescript
// domain/entities/User.ts
import { Entity } from '@libs/ddd';
import { Email } from '../value-objects/Email';
import { Password } from '../value-objects/Password';

interface UserProps {
  id: string;
  email: Email;
  password: Password;
  name: string;
  createdAt: Date;
  updatedAt: Date;
}

export class User extends Entity<UserProps> {
  private constructor(props: UserProps) {
    super(props);
  }

  static create(props: Omit<UserProps, 'id' | 'createdAt' | 'updatedAt'>): User {
    const now = new Date();
    return new User({
      ...props,
      id: crypto.randomUUID(),
      createdAt: now,
      updatedAt: now,
    });
  }

  changeEmail(newEmail: Email): void {
    if (!newEmail.isValid()) {
      throw new DomainError('Invalid email address');
    }
    this.props.email = newEmail;
    this.props.updatedAt = new Date();
  }

  changeName(name: string): void {
    if (name.length < 2 || name.length > 100) {
      throw new DomainError('Name must be between 2 and 100 characters');
    }
    this.props.name = name;
    this.props.updatedAt = new Date();
  }
}
```

### Paradigm 2 — Value Objects

```typescript
// domain/value-objects/Email.ts
export class Email {
  private readonly value: string;
  private readonly pattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

  private constructor(value: string) {
    this.value = value.toLowerCase();
  }

  static create(value: string): Email {
    if (!value || !new Email(value).pattern.test(value)) {
      throw new DomainError('Invalid email format');
    }
    return new Email(value);
  }

  isValid(): boolean {
    return this.pattern.test(this.value);
  }

  equals(other: Email): boolean {
    return this.value === other.value;
  }

  toString(): string {
    return this.value;
  }
}
```

### Paradigm 3 — Aggregates

```typescript
// domain/aggregates/Order.ts
import { AggregateRoot } from '@libs/ddd';
import { OrderItem } from '../entities/OrderItem';
import { Money } from '../value-objects/Money';

interface OrderProps {
  id: string;
  customerId: string;
  items: OrderItem[];
  status: OrderStatus;
  total: Money;
  createdAt: Date;
}

export class Order extends AggregateRoot<OrderProps> {
  private constructor(props: OrderProps) {
    super(props);
  }

  static create(customerId: string): Order {
    return new Order({
      id: crypto.randomUUID(),
      customerId,
      items: [],
      status: OrderStatus.PENDING,
      total: Money.zero(),
      createdAt: new Date(),
    });
  }

  addItem(productId: string, quantity: number, price: Money): void {
    if (this.props.status !== OrderStatus.PENDING) {
      throw new DomainError('Cannot add items to a finalized order');
    }

    const existingItem = this.props.items.find(
      (item) => item.productId === productId
    );

    if (existingItem) {
      existingItem.increaseQuantity(quantity);
    } else {
      this.props.items.push(
        OrderItem.create(productId, quantity, price)
      );
    }

    this.recalculateTotal();
  }

  private recalculateTotal(): void {
    this.props.total = this.props.items.reduce(
      (sum, item) => sum.add(item.subtotal),
      Money.zero()
    );
  }

  confirm(): void {
    if (this.props.items.length === 0) {
      throw new DomainError('Cannot confirm empty order');
    }
    this.props.status = OrderStatus.CONFIRMED;
  }
}
```

---

## Layer 3: Performance Engineering

### Repository Pattern with Caching

```typescript
// infrastructure/repositories/CachedUserRepository.ts
export class CachedUserRepository implements UserRepository {
  private repository: UserRepository;
  private cache: RedisCache<User>;

  async findById(id: string): Promise<User | null> {
    const cached = await this.cache.get(`user:${id}`);
    if (cached) {
      return cached;
    }

    const user = await this.repository.findById(id);
    if (user) {
      await this.cache.set(`user:${id}`, user, { ttl: 3600 });
    }
    return user;
  }

  async save(user: User): Promise<void> {
    await this.repository.save(user);
    await this.cache.invalidate(`user:${user.id}`);
  }
}
```

---

## Layer 4: Security

### Anti-Corruption Layer

```typescript
// domain/anti-corruption/LegacySystemAdapter.ts
export class LegacySystemAdapter implements ExternalSystemPort {
  private readonly mapper: DomainMapper;

  async getCustomer(customerId: string): Promise<Customer> {
    try {
      const legacyData = await this.fetchLegacyCustomer(customerId);
      return this.mapper.toDomain(legacyData);
    } catch (error) {
      throw new AdapterError('Failed to fetch from legacy system', error);
    }
  }

  private async fetchLegacyCustomer(id: string): Promise<LegacyCustomer> {
    const response = await fetch(`${LEGACY_API}/customers/${id}`);
    if (!response.ok) {
      throw new LegacySystemError(response.status);
    }
    return response.json();
  }
}
```

---

## Layer 5: Testing

### Domain Model Testing

```typescript
// tests/domain/User.test.ts
import { describe, it, expect } from 'vitest';
import { User } from '@domain/entities/User';
import { Email } from '@domain/value-objects/Email';

describe('User Entity', () => {
  it('should create a valid user', () => {
    const user = User.create({
      email: Email.create('test@example.com'),
      password: Password.create('SecurePass123!'),
      name: 'John Doe',
    });

    expect(user.props.name).toBe('John Doe');
    expect(user.props.email.toString()).toBe('test@example.com');
  });

  it('should throw on invalid email', () => {
    expect(() => Email.create('invalid-email')).toThrow(DomainError);
  });

  it('should update email and track change', () => {
    const user = User.create({ /* ... */ });
    const previousEmail = user.props.email;
    const newEmail = Email.create('new@example.com');
    
    user.changeEmail(newEmail);

    expect(user.props.email.equals(newEmail)).toBe(true);
    expect(user.props.updatedAt.getTime()).toBeGreaterThan(
      user.props.createdAt.getTime()
    );
  });
});
```

---

## Next Steps

Continue to [DDD Entities & Value Objects](./02-ddd-entities-value-objects.md)