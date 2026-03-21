# Introduction to Testing Express Apps

## 📌 What You'll Learn
- Why testing is important
- Types of tests (unit, integration, end-to-end)
- How to test Express routes
- Using Jest and Supertest for testing

## 🧠 Concept Explained (Plain English)

Testing is like having a quality control checklist for your code. Imagine building a car - you don't just hope it works! You test every component: does the engine start? Do the brakes work? Do the lights turn on?

Similarly, when you write code for a web application, you need to verify that:
- Routes work correctly (returning the right data)
- Authentication protects the right endpoints
- Errors are handled properly
- Your code does what it's supposed to do

There are different levels of testing:

1. **Unit Tests**: Test individual functions in isolation
2. **Integration Tests**: Test how parts work together
3. **End-to-End (E2E) Tests**: Test the entire application flow

For Express apps, integration tests are most common - you test your routes, middleware, and how they work together.

Popular testing tools:
- **Jest**: Fast, feature-rich test runner from Facebook
- **Mocha**: Flexible test framework
- **Supertest**: HTTP assertions for testing Express routes
- **Chai**: Assertion library for writing readable tests

## 💻 Code Example

```javascript
// ES Module - Testing Express with Jest and Supertest

// In a real project, you'd have separate test files
// This example shows the concepts

/*
import express from 'express';
import request from 'supertest';
import jest from 'jest';

// Setup Express app for testing
const app = express();

app.use(express.json());

// Simple routes to test
app.get('/api/users', (req, res) => {
    res.json([
        { id: 1, name: 'Alice' },
        { id: 2, name: 'Bob' }
    ]);
});

app.get('/api/users/:id', (req, res) => {
    const id = parseInt(req.params.id);
    if (id === 1) {
        res.json({ id: 1, name: 'Alice' });
    } else {
        res.status(404).json({ error: 'User not found' });
    }
});

app.post('/api/users', (req, res) => {
    const { name } = req.body;
    if (!name) {
        return res.status(400).json({ error: 'Name is required' });
    }
    res.status(201).json({ id: 3, name });
});

export default app;
*/

/*
// ========================================
// TEST FILE EXAMPLE (tests/app.test.js)
// ========================================

// import app from '../app.js';

// describe() groups related tests
describe('GET /api/users', () => {
    
    // test() defines a single test case
    test('should return a list of users', async () => {
        // Use supertest to make HTTP requests to your Express app
        const response = await request(app)
            .get('/api/users')
            .expect(200); // Assert status code is 200
        
        // Check response body
        expect(response.body).toHaveLength(2);
        expect(response.body[0].name).toBe('Alice');
    });
    
    test('should return users as an array', async () => {
        const response = await request(app)
            .get('/api/users');
        
        expect(Array.isArray(response.body)).toBe(true);
    });
});

describe('GET /api/users/:id', () => {
    
    test('should return a single user by ID', async () => {
        const response = await request(app)
            .get('/api/users/1')
            .expect(200);
        
        expect(response.body.id).toBe(1);
        expect(response.body.name).toBe('Alice');
    });
    
    test('should return 404 for non-existent user', async () => {
        const response = await request(app)
            .get('/api/users/999')
            .expect(404);
        
        expect(response.body.error).toBe('User not found');
    });
});

describe('POST /api/users', () => {
    
    test('should create a new user with valid data', async () => {
        const response = await request(app)
            .post('/api/users')
            .send({ name: 'Charlie' })
            .expect(201);
        
        expect(response.body.name).toBe('Charlie');
    });
    
    test('should return 400 when name is missing', async () => {
        const response = await request(app)
            .post('/api/users')
            .send({})
            .expect(400);
        
        expect(response.body.error).toBe('Name is required');
    });
});
*/

/*
// ========================================
// RUNNING TESTS
// ========================================

// Add to package.json scripts:
// "test": "jest",
// "test:watch": "jest --watch",
// "test:coverage": "jest --coverage"

// Run tests: npm test
// Run with coverage: npm run test:coverage
*/

// ========================================
// EXAMPLE: TESTING MIDDLEWARE
// ========================================

/*
// authMiddleware.js
export const authenticate = (req, res, next) => {
    const token = req.headers.authorization;
    if (!token) {
        return res.status(401).json({ error: 'No token provided' });
    }
    // Verify token logic...
    req.user = { id: 1 };
    next();
};

// middleware.test.js
import { authenticate } from '../middleware/authMiddleware.js';

test('should call next() when token is provided', () => {
    const req = { headers: { authorization: 'Bearer token123' } };
    const res = {
        status: jest.fn().mockReturnThis(),
        json: jest.fn()
    };
    const next = jest.fn();
    
    authenticate(req, res, next);
    
    expect(next).toHaveBeenCalled();
    expect(req.user).toBeDefined();
});

test('should return 401 when token is missing', () => {
    const req = { headers: {} };
    const res = {
        status: jest.fn().mockReturnThis(),
        json: jest.fn()
    };
    const next = jest.fn();
    
    authenticate(req, res, next);
    
    expect(res.status).toHaveBeenCalledWith(401);
    expect(next).not.toHaveBeenCalled();
});
*/

console.log(`
// ========================================
// TESTING SUMMARY
// ========================================

// Test Structure:
describe('Route/Feature', () => {
    test('should do something specific', async () => {
        // Arrange - set up test data
        // Act - perform the action
        // Assert - check the result
    });
});

// Common Assertions:
expect(value).toBe(expected)           // Strict equality
expect(value).toEqual(expected)        // Deep equality
expect(value).toBeTruthy()             // Check if truthy
expect(value).toBeFalsy()              // Check if falsy
expect(() => code).toThrow()           // Check if throws
expect(array).toHaveLength(n)          // Check array length
expect(obj).toHaveProperty('key')      // Check property exists

// Testing Tips:
// 1. Test happy paths AND edge cases
// 2. Test error handling
// 3. Keep tests independent
// 4. Use descriptive test names
// 5. Run tests automatically in CI/CD
`);

console.log(`
// ========================================
// JEST CONFIGURATION (jest.config.js)
// ========================================

export default {
    testEnvironment: 'node',
    testMatch: ['**/__tests__/**/*.test.js'],
    collectCoverageFrom: [
        'src/**/*.js',
        '!src/**/*.test.js'
    ],
    coverageDirectory: 'coverage',
    verbose: true
};
`);
```

## 🔍 Testing Concepts

| Concept | Description |
|---------|-------------|
| `describe()` | Groups related tests together |
| `test()` | Defines a single test case |
| `expect()` | Assertion - checks if value matches expectation |
| `request()` | Supertest - makes HTTP requests to app |
| `beforeEach()` | Runs before each test |
| `afterEach()` | Runs after each test |

## ⚠️ Common Mistakes

**1. Not testing error cases**
Only testing happy paths leaves bugs undetected. Always test error handling!

**2. Making tests dependent on each other**
Each test should work independently. Don't let one test affect another.

**3. Testing implementation details**
Test behavior, not implementation. If you change how code works, tests shouldn't break.

**4. Not using descriptive test names**
Test names should describe what they're testing: "should return 404 for invalid ID"

**5. Forgetting to test async code**
Use async/await with promises in tests, or use .then() for callbacks.

## ✅ Quick Recap

- Testing verifies your code works correctly
- Use Jest for test framework and Supertest for HTTP testing
- Use `describe()` to group tests and `test()` for individual cases
- Make assertions with `expect()`
- Test both success and error scenarios
- Keep tests independent and focused
- Run tests automatically in CI/CD pipelines

## 🔗 What's Next

Let's look at deployment strategies for Express applications, including environment configuration.
