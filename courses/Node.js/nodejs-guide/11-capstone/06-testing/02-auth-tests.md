# Authentication Tests

## What You'll Build In This File

Complete tests for registration and login endpoints using supertest.

## Complete Auth Tests

Create `tests/auth.test.js`:

```javascript
// tests/auth.test.js - Authentication endpoint tests
// Uses supertest for HTTP assertions and node:test for test structure

import { test, describe, before, after } from 'node:test';
import assert from 'node:assert';
import request from 'supertest';

// Import app - assumes Express app exports properly
// We'll need to create a test config for the app

describe('Authentication Endpoints', () => {
  let app;
  let baseUrl;
  
  // Setup: get app and start test server
  before(async () => {
    // Dynamic import to ensure fresh module state
    const appModule = await import('../src/index.js');
    app = appModule.default;
    baseUrl = `http://localhost:${app.locals.port || 3000}`;
  });
  
  describe('POST /auth/register', () => {
    test('should create a new user with valid data', async () => {
      const email = `newuser-${Date.now()}@test.com`;
      const password = 'password123';
      
      const response = await request(baseUrl)
        .post('/auth/register')
        .send({ email, password })
        .expect('Content-Type', /json/);
      
      assert.strictEqual(response.status, 201);
      assert(response.body.user, 'should return user');
      assert(response.body.token, 'should return token');
      assert.strictEqual(response.body.user.email, email);
    });
    
    test('should reject duplicate email', async () => {
      const email = `duplicate-${Date.now()}@test.com`;
      const password = 'password123';
      
      // Create first user
      await request(baseUrl)
        .post('/auth/register')
        .send({ email, password });
      
      // Try to create duplicate
      const response = await request(baseUrl)
        .post('/auth/register')
        .send({ email, password })
        .expect('Content-Type', /json/);
      
      assert.strictEqual(response.status, 409);
      assert(response.body.message, 'Should have error message');
    });
    
    test('should reject invalid email format', async () => {
      const response = await request(baseUrl)
        .post('/auth/register')
        .send({ email: 'not-an-email', password: 'password123' })
        .expect('Content-Type', /json/);
      
      assert.strictEqual(response.status, 400);
    });
    
    test('should reject short password', async () => {
      const response = await request(baseUrl)
        .post('/auth/register')
        .send({ email: 'test@test.com', password: 'short' })
        .expect('Content-Type', /json/);
      
      assert.strictEqual(response.status, 400);
    });
    
    test('should reject missing email', async () => {
      const response = await request(baseUrl)
        .post('/auth/register')
        .send({ password: 'password123' })
        .expect('Content-Type', /json/);
      
      assert.strictEqual(response.status, 400);
    });
  });
  
  describe('POST /auth/login', () => {
    test('should login with valid credentials', async () => {
      const email = `logintest-${Date.now()}@test.com`;
      const password = 'password123';
      
      // Create user first
      await request(baseUrl)
        .post('/auth/register')
        .send({ email, password });
      
      // Login
      const response = await request(baseUrl)
        .post('/auth/login')
        .send({ email, password })
        .expect('Content-Type', /json/);
      
      assert.strictEqual(response.status, 200);
      assert(response.body.token, 'should return token');
      assert.strictEqual(response.body.user.email, email);
    });
    
    test('should reject wrong password', async () => {
      const email = `wrongpass-${Date.now()}@test.com`;
      
      // Create user
      await request(baseUrl)
        .post('/auth/register')
        .send({ email, password: 'correctpassword' });
      
      // Try login with wrong password
      const response = await request(baseUrl)
        .post('/auth/login')
        .send({ email, password: 'wrongpassword' });
      
      assert.strictEqual(response.status, 401);
    });
    
    test('should reject non-existent user', async () => {
      const response = await request(baseUrl)
        .post('/auth/login')
        .send({ email: 'nobody@test.com', password: 'password123' });
      
      assert.strictEqual(response.status, 401);
    });
  });
});
```

## Running the Tests

```bash
# Run all tests
npm test

# Run specific test file
node --test tests/auth.test.js

# Run with coverage (requires coverage tool)
node --test --experimental-coverage tests/
```

## How It Connects

This connects to:
- [09-testing/integration-testing/01-supertest.md](../../../09-testing/integration-testing/01-supertest.md) - supertest for Express

## Common Mistakes

- Not testing error cases
- Using same test data across tests
- Not cleaning up test database

## Try It Yourself

### Exercise 1: Run Tests
Run the auth tests and fix any failures.

### Exercise 2: Add More Tests
Add tests for edge cases.

### Exercise 3: Test JWT
Verify the returned JWT is valid.

## Next Steps

Continue to [03-bookmark-tests.md](./03-bookmark-tests.md) to test bookmarks.
