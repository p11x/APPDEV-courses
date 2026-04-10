# 🧪 JavaScript Testing Encyclopedia

## Complete Testing Strategies Guide

---

## Table of Contents

1. [Unit Testing](#unit-testing)
2. [Integration Testing](#integration-testing)
3. [E2E Testing](#e2e-testing)
4. [Testing Patterns](#testing-patterns)
5. [Mocking Deep Dive](#mocking-deep-dive)

---

## Unit Testing

### AAA Pattern

```javascript
describe('Calculator', () => {
  describe('add', () => {
    it('should add two numbers', () => {
      // Arrange
      const calculator = new Calculator();
      
      // Act
      const result = calculator.add(2, 3);
      
      // Assert
      expect(result).toBe(5);
    });
    
    it('should handle negative numbers', () => {
      const calculator = new Calculator();
      expect(calculator.add(-2, -3)).toBe(-5);
    });
    
    it('should handle decimals', () => {
      const calculator = new Calculator();
      expect(calculator.add(0.1, 0.2)).toBeCloseTo(0.3);
    });
  });
});
```

### Test Organization

```javascript
describe('User Service', () => {
  let service;
  let mockDatabase;
  
  beforeAll(() => {
    mockDatabase = { /* mock DB */ };
    service = new UserService(mockDatabase);
  });
  
  afterEach(() => {
    jest.clearAllMocks();
  });
  
  describe('createUser', () => {
    it('should create user with valid data', async () => {
      // Test implementation
    });
    
    it('should throw on duplicate email', async () => {
      // Test implementation
    });
  });
});
```

---

## Integration Testing

### API Testing

```javascript
describe('POST /api/users', () => {
  it('should create a new user', async () => {
    const response = await request(app)
      .post('/api/users')
      .send({ name: 'John', email: 'john@example.com' })
      .expect(201);
    
    expect(response.body).toMatchObject({
      name: 'John',
      email: 'john@example.com'
    });
  });
  
  it('should return 400 for invalid email', async () => {
    const response = await request(app)
      .post('/api/users')
      .send({ name: 'John', email: 'invalid' })
      .expect(400);
  });
});
```

### Database Integration

```javascript
describe('UserRepository', () => {
  let repository;
  
  beforeAll(async () => {
    await setupTestDatabase();
    repository = new UserRepository(testDb);
  });
  
  afterAll(async () => {
    await cleanupTestDatabase();
  });
});
```

---

## E2E Testing

### Cypress Tests

```javascript
describe('Login Flow', () => {
  beforeEach(() => {
    cy.visit('/login');
  });
  
  it('should login successfully', () => {
    cy.get('[data-testid="email"]').type('user@example.com');
    cy.get('[data-testid="password"]').type('password123');
    cy.get('[data-testid="submit"]').click();
    
    cy.url().should('include', '/dashboard');
    cy.contains('Welcome').should('be.visible');
  });
  
  it('should show error for invalid credentials', () => {
    cy.get('[data-testid="email"]').type('wrong@example.com');
    cy.get('[data-testid="password"]').type('wrongpass');
    cy.get('[data-testid="submit"]').click();
    
    cy.contains('Invalid credentials').should('be.visible');
  });
});
```

### Playwright Tests

```javascript
import { test, expect } from '@playwright/test';

test('complete checkout flow', async ({ page }) => {
  await page.goto('/cart');
  
  // Add items
  await page.click('[data-testid="add-item-1"]');
  
  // Checkout
  await page.click('[data-testid="checkout"]');
  
  // Fill shipping
  await page.fill('[data-testid="address"]', '123 Main St');
  await page.click('[data-testid="continue"]');
  
  // Payment
  await page.fill('[data-testid="card"]', '4242424242424242');
  await page.click('[data-testid="pay"]');
  
  // Verify success
  await expect(page).toHaveURL('/success');
});
```

---

## Testing Patterns

### Snapshot Testing

```javascript
test('renders component correctly', () => {
  const tree = renderer.create(<MyComponent />).toJSON();
  expect(tree).toMatchSnapshot();
});
```

### Property-Based Testing

```javascript
import { describe, it } from 'vitest';

describe('add', () => {
  it('should be commutative', () => {
    // Test with random values
    for (let i = 0; i < 100; i++) {
      const a = Math.random() * 1000;
      const b = Math.random() * 1000;
      expect(add(a, b)).toBe(add(b, a));
    }
  });
});
```

---

## Mocking Deep Dive

### Function Mocking

```javascript
jest.mock('./api');

import { fetchUser } from './api';

test('fetches user', async () => {
  fetchUser.mockResolvedValue({ name: 'John' });
  
  const user = await getUser(1);
  
  expect(fetchUser).toHaveBeenCalledWith(1);
  expect(user.name).toBe('John');
});
```

### Timer Mocking

```javascript
test('delays execution', () => {
  jest.useFakeTimers();
  
  const callback = jest.fn();
  delayedFunction(callback);
  
  jest.advanceTimersByTime(1000);
  
  expect(callback).toHaveBeenCalled();
  
  jest.useRealTimers();
});
```

---

## Summary

### Key Takeaways

1. **Unit**: Individual functions
2. **Integration**: Multiple components
3. **E2E**: Full user flows

### Testing Best Practices

- Test behavior, not implementation
- Use descriptive test names
- Follow AAA pattern

---

*Last updated: 2024*