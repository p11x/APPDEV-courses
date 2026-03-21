# Unit Testing in Express.js

## Why Test Your Code?

**Testing** ensures your code works correctly and continues to work as you make changes. It catches bugs early and gives you confidence when deploying.

Types of tests:
- **Unit tests** - Test individual functions in isolation
- **Integration tests** - Test how parts work together
- **End-to-end tests** - Test entire application flows

## Setting Up Testing

### Install Testing Tools

```bash
npm install --save-dev jest supertest
```

### Configure Jest

Add to `package.json`:

```json
{
    "scripts": {
        "test": "jest",
        "test:watch": "jest --watch",
        "test:coverage": "jest --coverage"
    },
    "jest": {
        "testEnvironment": "node",
        "coverageDirectory": "coverage",
        "collectCoverageFrom": ["src/**/*.js"]
    }
}
```

## Unit Testing with Jest

### Example: Testing a Utility Function

```javascript
// utils/validators.js
// Utility functions for validation

// Validate email format
export const validateEmail = (email) => {
    // Simple email regex
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
};

// Validate password strength
export const validatePassword = (password) => {
    const errors = [];
    
    if (password.length < 8) {
        errors.push('Password must be at least 8 characters');
    }
    
    if (!/[A-Z]/.test(password)) {
        errors.push('Password must contain an uppercase letter');
    }
    
    if (!/[0-9]/.test(password)) {
        errors.push('Password must contain a number');
    }
    
    return {
        isValid: errors.length === 0,
        errors
    };
};

// Calculate discount
export const calculateDiscount = (price, discountPercent) => {
    if (price < 0 || discountPercent < 0 || discountPercent > 100) {
        throw new Error('Invalid input');
    }
    
    const discountAmount = price * (discountPercent / 100);
    return price - discountAmount;
};
```

### Write Tests

```javascript
// tests/validators.test.js
// Jest test file for validators

// Import functions to test
import { validateEmail, validatePassword, calculateDiscount } from '../src/utils/validators.js';

describe('validateEmail', () => {
    // Test group
    
    test('valid email returns true', () => {
        expect(validateEmail('test@example.com')).toBe(true);
    });
    
    test('valid email with subdomain returns true', () => {
        expect(validateEmail('user@mail.example.com')).toBe(true);
    });
    
    test('invalid email without @ returns false', () => {
        expect(validateEmail('testexample.com')).toBe(false);
    });
    
    test('invalid email without domain returns false', () => {
        expect(validateEmail('test@')).toBe(false);
    });
    
    test('empty string returns false', () => {
        expect(validateEmail('')).toBe(false);
    });
});

describe('validatePassword', () => {
    test('valid password returns no errors', () => {
        const result = validatePassword('SecurePass123');
        expect(result.isValid).toBe(true);
        expect(result.errors).toHaveLength(0);
    });
    
    test('password too short returns error', () => {
        const result = validatePassword('Short1');
        expect(result.isValid).toBe(false);
        expect(result.errors).toContain('Password must be at least 8 characters');
    });
    
    test('password without uppercase returns error', () => {
        const result = validatePassword('lowercase1');
        expect(result.isValid).toBe(false);
        expect(result.errors).toContain('Password must contain an uppercase letter');
    });
    
    test('password without number returns error', () => {
        const result = validatePassword('NoNumbers');
        expect(result.isValid).toBe(false);
    });
});

describe('calculateDiscount', () => {
    test('calculates 10% discount correctly', () => {
        expect(calculateDiscount(100, 10)).toBe(90);
    });
    
    test('calculates 0% discount correctly', () => {
        expect(calculateDiscount(100, 0)).toBe(100);
    });
    
    test('calculates 100% discount correctly', () => {
        expect(calculateDiscount(100, 100)).toBe(0);
    });
    
    test('throws on negative price', () => {
        expect(() => calculateDiscount(-10, 10)).toThrow('Invalid input');
    });
    
    test('throws on discount over 100%', () => {
        expect(() => calculateDiscount(100, 110)).toThrow('Invalid input');
    });
});
```

## Integration Testing with Supertest

**Supertest** lets you test Express routes without running a server.

### Example: Testing Routes

```javascript
// tests/api.test.js
import request from 'supertest';
import express from 'express';
import { userRouter } from '../src/routes/userRoutes.js';

describe('User API Tests', () => {
    // Create test app instance
    const app = express();
    app.use(express.json());
    app.use('/api/users', userRouter);
    
    describe('GET /api/users', () => {
        test('returns all users', async () => {
            // request() makes HTTP requests to your Express app
            const response = await request(app)
                .get('/api/users')
                .expect('Content-Type', /json/)
                .expect(200);
            
            // Check response body
            expect(response.body).toHaveProperty('users');
            expect(Array.isArray(response.body.users)).toBe(true);
        });
    });
    
    describe('GET /api/users/:id', () => {
        test('returns user by ID', async () => {
            const response = await request(app)
                .get('/api/users/1')
                .expect(200);
            
            expect(response.body).toHaveProperty('id');
        });
        
        test('returns 404 for non-existent user', async () => {
            await request(app)
                .get('/api/users/99999')
                .expect(404);
        });
    });
    
    describe('POST /api/users', () => {
        test('creates new user', async () => {
            const newUser = {
                name: 'Test User',
                email: 'test@example.com'
            };
            
            const response = await request(app)
                .post('/api/users')
                .send(newUser)
                .expect(201);
            
            expect(response.body).toHaveProperty('id');
            expect(response.body.name).toBe(newUser.name);
        });
        
        test('returns 400 for invalid input', async () => {
            const invalidUser = {
                name: 'Test'
                // missing email
            };
            
            await request(app)
                .post('/api/users')
                .send(invalidUser)
                .expect(400);
        });
    });
});
```

### Testing with Authentication

```javascript
describe('Protected Routes', () => {
    let token;
    
    // Get token before tests
    beforeAll(async () => {
        const response = await request(app)
            .post('/api/auth/login')
            .send({ email: 'test@example.com', password: 'password123' });
        
        token = response.body.token;
    });
    
    test('GET /api/profile with valid token', async () => {
        await request(app)
            .get('/api/profile')
            .set('Authorization', `Bearer ${token}`)
            .expect(200);
    });
    
    test('GET /api/profile without token returns 401', async () => {
        await request(app)
            .get('/api/profile')
            .expect(401);
    });
});
```

## Running Tests

```bash
# Run all tests
npm test

# Run tests in watch mode (re-runs on file changes)
npm run test:watch

# Run tests with coverage report
npm run test:coverage
```

## Test Coverage

| Type | What It Tests | Example |
|------|---------------|---------|
| **Unit** | Single functions | Does `validateEmail()` work? |
| **Integration** | Multiple components | Do routes and controllers work together? |
| **E2E** | Full user flows | Can a user register and login? |

## Best Practices

| Practice | Why |
|----------|-----|
| Test edge cases | What happens with empty input? |
| Test error cases | Does error handling work? |
| Keep tests independent | Each test should work alone |
| Use descriptive names | `test('invalid email returns false')` |
| Run tests in CI/CD | Catch issues before deployment |

## What's Next?

- **[Integration Testing](./02_integration_testing.md)** — Testing API integrations
- **[Deployment](../10_Advanced_Topics/01_deployment.md)** — Deploying to production
