# Test Setup

## What You'll Build In This File

Test helpers and setup for NodeMark using the built-in node:test runner.

## Complete Test Setup

Create `tests/helpers.js`:

```javascript
// tests/helpers.js - Test utilities and setup
// Provides test-specific database and app configuration

import { spawn } from 'child_process';
import request from 'supertest';

/**
 * Test configuration - uses separate test database
 * This ensures tests don't affect development data
 */
export const testConfig = {
  port: 3001,  // Different port than dev (3000)
  nodeEnv: 'test',
  db: {
    host: process.env.TEST_DB_HOST || 'localhost',
    port: parseInt(process.env.TEST_DB_PORT || '5432'),
    database: process.env.TEST_DB_NAME || 'nodemark_test',
    user: process.env.TEST_DB_USER || 'postgres',
    password: process.env.TEST_DB_PASSWORD || 'postgres',
  },
  jwt: {
    secret: 'test-secret-key',
    expiresIn: '1h',
  },
};

/**
 * Create a test application instance
 * Import your app and configure it for testing
 * @param {Object} overrides - Override app configuration
 */
export async function createTestApp(overrides = {}) {
  // Dynamic import to get app with test config
  const { default: app } = await import('../src/index.js');
  
  // Override config
  Object.assign(app.config, overrides);
  
  return app;
}

/**
 * Make an authenticated request
 * @param {Object} app - Express app
 * @param {string} token - JWT token
 * @returns {Function} - Function to make authenticated requests
 */
export function authenticatedRequest(app, token) {
  return {
    get: (path) => request(app).get(path).set('Authorization', `Bearer ${token}`),
    post: (path) => request(app).post(path).set('Authorization', `Bearer ${token}`),
    patch: (path) => request(app).patch(path).set('Authorization', `Bearer ${token}`),
    delete: (path) => request(app).delete(path).set('Authorization', `Bearer ${token}`),
  };
}

/**
 * Create a test user and return their token
 * @param {Object} app - Express app
 */
export async function createTestUser(app) {
  const email = `test-${Date.now()}@example.com`;
  const password = 'testpassword123';
  
  const response = await request(app)
    .post('/auth/register')
    .send({ email, password });
  
  return {
    email,
    password,
    token: response.body.token,
    userId: response.body.user.id,
  };
}

/**
 * Wait for a condition with timeout
 * @param {Function} condition - Function that returns Promise<boolean>
 * @param {number} timeout - Timeout in milliseconds
 * @param {number} interval - Check interval in milliseconds
 */
export async function waitFor(condition, timeout = 5000, interval = 100) {
  const start = Date.now();
  
  while (Date.now() - start < timeout) {
    if (await condition()) {
      return true;
    }
    await new Promise(resolve => setTimeout(resolve, interval));
  }
  
  throw new Error('Timeout waiting for condition');
}
```

## Test Setup for Each Test File

Create `tests/setup.js`:

```javascript
// tests/setup.js - Global test setup
// Runs before all tests

import { test, describe, before, after } from 'node:test';
import assert from 'node:assert';

// Set test environment before importing app
process.env.NODE_ENV = 'test';
process.env.DB_NAME = 'nodemark_test';

describe('Global Test Setup', () => {
  test('should have test environment configured', () => {
    assert.strictEqual(process.env.NODE_ENV, 'test');
  });
  
  test('should be able to import app', async () => {
    // This verifies imports work
    const { query } = await import('../src/db/index.js');
    assert(query, 'db.query should be exported');
  });
});
```

## How It Connects

This connects to concepts from:
- [09-testing/node-test-runner/01-test-basics.md](../../../09-testing/node-test-runner/01-test-basics.md) - node:test basics
- [09-testing/integration-testing/01-supertest.md](../../../09-testing/integration-testing/01-supertest.md) - supertest for Express

## Common Mistakes

- Using same database for tests and development
- Not cleaning up test data between tests
- Not handling async test setup properly

## Try It Yourself

### Exercise 1: Create Helper
Create a test helper to generate test data.

### Exercise 2: Add BeforeEach
Add a beforeEach hook to clean up between tests.

### Exercise 3: Test Config
Verify test config is used in tests.

## Next Steps

Continue to [02-auth-tests.md](./02-auth-tests.md) to write auth tests.
