# Testing Pyramid, Architecture, and Principles

## What You'll Learn

- Testing pyramid and test distribution
- Test architecture patterns
- Testing principles (FIRST, AAA)
- Test organization strategies
- Testing best practices

## Testing Pyramid

```
Testing Pyramid:
─────────────────────────────────────────────
        /  E2E  \          Few, slow, expensive
       / (5-10%)  \
      /─────────────\
     / Integration   \     Moderate, medium speed
    /   (15-25%)      \
   /───────────────────\
  /     Unit Tests       \  Many, fast, cheap
 /      (60-80%)          \
/─────────────────────────\

Cost:     Unit < Integration < E2E
Speed:    Unit > Integration > E2E
Coverage: Unit > Integration > E2E
Confidence: E2E > Integration > Unit
```

## FIRST Principles

```
FIRST Principles:
─────────────────────────────────────────────
F - Fast:       Tests should run quickly (< 100ms for unit)
I - Isolated:   No dependencies between tests
R - Repeatable: Same result every time
S - Self-validating: Pass/fail without manual review
T - Timely:     Written alongside or before code
```

## AAA Pattern (Arrange-Act-Assert)

```javascript
import { describe, it } from 'node:test';
import assert from 'node:assert/strict';

describe('Calculator', () => {
    it('should add two numbers', () => {
        // Arrange: Set up test data and dependencies
        const a = 5;
        const b = 3;
        const calculator = new Calculator();

        // Act: Execute the code under test
        const result = calculator.add(a, b);

        // Assert: Verify the expected outcome
        assert.strictEqual(result, 8);
    });
});
```

## Test Organization

```
Project Test Structure:
─────────────────────────────────────────────
src/
├── services/
│   ├── user.service.js
│   └── order.service.js
├── controllers/
│   ├── user.controller.js
│   └── order.controller.js
├── middleware/
│   └── auth.middleware.js
└── utils/
    └── validation.js

test/
├── unit/
│   ├── services/
│   │   ├── user.service.test.js
│   │   └── order.service.test.js
│   ├── utils/
│   │   └── validation.test.js
│   └── middleware/
│       └── auth.middleware.test.js
├── integration/
│   ├── api/
│   │   ├── users.api.test.js
│   │   └── orders.api.test.js
│   └── database/
│       └── user.repository.test.js
├── e2e/
│   ├── user-journey.test.js
│   └── checkout-flow.test.js
├── fixtures/
│   ├── users.json
│   └── orders.json
└── helpers/
    ├── test-db.js
    ├── test-app.js
    └── factories.js
```

## Test Naming Conventions

```javascript
// Pattern: should [expected behavior] when [condition]
describe('UserService', () => {
    describe('createUser', () => {
        it('should create user when valid data provided', () => {});
        it('should throw error when email already exists', () => {});
        it('should hash password before storing', () => {});
    });

    describe('findById', () => {
        it('should return user when ID exists', () => {});
        it('should return null when ID does not exist', () => {});
    });
});
```

## Testing Design Patterns

```javascript
// 1. Page Object Pattern (for E2E)
class LoginPage {
    constructor(page) {
        this.page = page;
        this.emailInput = '#email';
        this.passwordInput = '#password';
        this.submitButton = '#submit';
    }

    async login(email, password) {
        await this.page.fill(this.emailInput, email);
        await this.page.fill(this.passwordInput, password);
        await this.page.click(this.submitButton);
    }
}

// 2. Builder Pattern (for test data)
class UserBuilder {
    constructor() {
        this.data = {
            name: 'Test User',
            email: 'test@example.com',
            role: 'user',
        };
    }

    withEmail(email) {
        this.data.email = email;
        return this;
    }

    withRole(role) {
        this.data.role = role;
        return this;
    }

    build() {
        return { ...this.data };
    }
}

// Usage
const adminUser = new UserBuilder()
    .withEmail('admin@example.com')
    .withRole('admin')
    .build();

// 3. Factory Pattern
class UserFactory {
    static create(overrides = {}) {
        return {
            id: Math.random().toString(36).slice(2),
            name: 'Test User',
            email: `user-${Date.now()}@test.com`,
            password: 'Password123!',
            ...overrides,
        };
    }

    static createMany(count, overrides = {}) {
        return Array.from({ length: count }, (_, i) =>
            this.create({ email: `user-${i}@test.com`, ...overrides })
        );
    }
}
```

## Best Practices Checklist

- [ ] Follow the testing pyramid distribution
- [ ] Use AAA pattern consistently
- [ ] Name tests descriptively: "should X when Y"
- [ ] Keep tests independent and isolated
- [ ] Use factories/builders for test data
- [ ] One assertion concept per test
- [ ] Test behavior, not implementation
- [ ] Maintain test code quality equal to production code

## Cross-References

- See [Unit Testing](../03-unit-testing/01-functions-classes.md) for unit test patterns
- See [Integration Testing](./01-supertest.md) for API testing
- See [Database Testing](../07-database-testing/01-unit-testing.md) for DB testing

## Next Steps

Continue to [Testing Frameworks](../02-testing-frameworks/01-jest-comprehensive.md).
