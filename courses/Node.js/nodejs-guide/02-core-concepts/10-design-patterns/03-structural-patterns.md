# Structural Patterns: Adapter, Repository, and Service

## What You'll Learn

- Adapter pattern for integration
- Repository pattern for data access
- Service pattern for business logic
- Layered architecture implementation

## Adapter Pattern

```javascript
// Wrap different payment providers with same interface
class StripeAdapter {
    constructor(stripeClient) { this.client = stripeClient; }

    async charge(amount, currency, token) {
        const payment = await this.client.charges.create({
            amount: amount * 100, // Stripe uses cents
            currency,
            source: token,
        });
        return { id: payment.id, status: payment.status };
    }
}

class PayPalAdapter {
    constructor(paypalClient) { this.client = paypalClient; }

    async charge(amount, currency, token) {
        const payment = await this.client.payment.create({
            total: amount.toString(),
            currency,
            fundingInstrumentId: token,
        });
        return { id: payment.id, status: payment.state };
    }
}

// Consumer uses same interface regardless of provider
const payment = new StripeAdapter(stripe);
await payment.charge(10, 'usd', 'tok_visa');
```

## Repository Pattern

```javascript
class UserRepository {
    constructor(db) {
        this.db = db;
    }

    async findById(id) {
        return this.db.query('SELECT * FROM users WHERE id = $1', [id]);
    }

    async findAll({ limit = 20, offset = 0 } = {}) {
        return this.db.query('SELECT * FROM users LIMIT $1 OFFSET $2', [limit, offset]);
    }

    async create(data) {
        return this.db.query(
            'INSERT INTO users (name, email) VALUES ($1, $2) RETURNING *',
            [data.name, data.email]
        );
    }

    async update(id, data) {
        return this.db.query(
            'UPDATE users SET name = $1 WHERE id = $2 RETURNING *',
            [data.name, id]
        );
    }

    async delete(id) {
        return this.db.query('DELETE FROM users WHERE id = $1', [id]);
    }
}
```

## Service Pattern

```javascript
class UserService {
    constructor(userRepo, emailService) {
        this.userRepo = userRepo;
        this.emailService = emailService;
    }

    async register(data) {
        // Validation
        if (!data.email) throw new AppError('Email required', 400);

        // Check duplicate
        const existing = await this.userRepo.findByEmail(data.email);
        if (existing) throw new AppError('Email already registered', 409);

        // Create user
        const user = await this.userRepo.create(data);

        // Send welcome email
        await this.emailService.sendWelcome(user);

        return user;
    }

    async getUser(id) {
        const user = await this.userRepo.findById(id);
        if (!user) throw new AppError('User not found', 404);
        return user;
    }
}
```

## Best Practices Checklist

- [ ] Use Adapter to integrate external services
- [ ] Use Repository to abstract data access
- [ ] Use Service for business logic
- [ ] Keep layers independent (no direct DB access from routes)

## Cross-References

- See [Creational Patterns](./01-creational-patterns.md) for Singleton/Factory
- See [Behavioral Patterns](./02-behavioral-patterns.md) for Observer/Strategy
- See [Error Handling](../11-error-handling/01-error-propagation.md) for AppError

## Next Steps

Continue to [Error Handling](../11-error-handling/01-error-propagation.md) for error patterns.
