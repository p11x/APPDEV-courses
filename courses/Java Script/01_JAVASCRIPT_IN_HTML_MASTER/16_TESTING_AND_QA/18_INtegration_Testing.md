# 🔗 Integration Testing Guide

## 📋 Overview

Integration testing verifies that different modules or services work together correctly. This guide covers strategies for testing JavaScript application integrations.

---

## 🎯 What is Integration Testing?

Integration testing sits between unit testing and end-to-end testing:
- **Unit tests**: Test individual functions in isolation
- **Integration tests**: Test how components work together
- **E2E tests**: Test complete user workflows

### When to Use Integration Tests

```javascript
// Testing database interactions
async function testUserRepository() {
    const userRepo = new UserRepository(database);
    
    // Create user
    const created = await userRepo.create({ name: 'John' });
    expect(created.id).toBeDefined();
    
    // Retrieve user
    const retrieved = await userRepo.findById(created.id);
    expect(retrieved.name).toBe('John');
    
    // Clean up
    await userRepo.delete(created.id);
}

// Testing API integrations
async function testWeatherService() {
    const weather = new WeatherService();
    
    // Test with mock HTTP client
    const mockFetch = jest.fn().mockResolvedValue({
        ok: true,
        json: () => Promise.resolve({ temp: 25 })
    });
    
    weather.fetch = mockFetch;
    
    const result = await weather.getWeather('London');
    expect(result.temp).toBe(25);
    expect(mockFetch).toHaveBeenCalledWith(
        expect.stringContaining('London')
    );
}
```

---

## 🎯 Integration Testing Patterns

### Horizontal Integration (Same Layer)

```javascript
// Test how services at same layer work together
describe('Payment Integration', () => {
    test('processes payment through payment gateway', async () => {
        const paymentProcessor = new PaymentProcessor();
        const paymentGateway = new PaymentGateway();
        
        // Both work together
        const result = await paymentProcessor.process(
            paymentGateway,
            { amount: 100, currency: 'USD' }
        );
        
        expect(result.status).toBe('success');
    });
});
```

### Vertical Integration (Different Layers)

```javascript
// Test how different layers work together
describe('User Management Integration', () => {
    test('creates user across all layers', async () => {
        // Controller → Service → Repository → Database
        const controller = new UserController();
        
        const response = await controller.createUser({
            name: 'John',
            email: 'john@example.com'
        });
        
        expect(response.status).toBe(201);
        expect(response.data.id).toBeDefined();
    });
});
```

---

## 🎯 Testing Database Integration

```javascript
// Database test setup
const { Client } = require('pg');

async function setupTestDatabase() {
    const client = new Client({
        connectionString: process.env.TEST_DB_URL
    });
    
    await client.connect();
    
    // Create tables
    await client.query(`
        CREATE TABLE IF NOT EXISTS users (
            id SERIAL PRIMARY KEY,
            name VARCHAR(255) NOT NULL,
            email VARCHAR(255) UNIQUE
        )
    `);
    
    return client;
}

async function cleanupTestDatabase(client) {
    await client.query('DROP TABLE IF EXISTS users');
    await client.end();
}

// Test with database
describe('User Repository', () => {
    let client;
    
    beforeAll(async () => {
        client = await setupTestDatabase();
    });
    
    afterAll(async () => {
        await cleanupTestDatabase(client);
    });
    
    test('creates and retrieves user', async () => {
        const repo = new UserRepository(client);
        
        const user = await repo.create({
            name: 'John',
            email: 'john@example.com'
        });
        
        const found = await repo.findById(user.id);
        expect(found.name).toBe('John');
    });
});
```

---

## 🎯 API Integration Testing

```javascript
// Supertest for Express apps
const request = require('supertest');
const app = require('../app');

describe('API Integration', () => {
    test('POST /api/users creates user', async () => {
        const response = await request(app)
            .post('/api/users')
            .send({ name: 'John', email: 'john@example.com' })
            .expect(201);
        
        expect(response.body).toHaveProperty('id');
        expect(response.body.name).toBe('John');
    });
    
    test('GET /api/users/:id returns user', async () => {
        // First create a user
        const created = await request(app)
            .post('/api/users')
            .send({ name: 'Jane', email: 'jane@example.com' });
        
        // Then retrieve it
        const response = await request(app)
            .get(`/api/users/${created.body.id}`)
            .expect(200);
        
        expect(response.body.name).toBe('Jane');
    });
    
    test('handles API errors properly', async () => {
        const response = await request(app)
            .get('/api/users/99999')
            .expect(404);
        
        expect(response.body.error).toBeDefined();
    });
});
```

---

## 🎯 Testing External Services

```javascript
// Mock external API calls
const nock = require('nock');

describe('External API Integration', () => {
    beforeEach(() => {
        // Mock external service
        nock('https://api.example.com')
            .get('/users/1')
            .reply(200, { id: 1, name: 'John' })
            .post('/users')
            .reply(201, { id: 2, name: 'Jane' });
    });
    
    test('fetches user from external API', async () => {
        const service = new ExternalUserService();
        
        const user = await service.getUser(1);
        
        expect(user.name).toBe('John');
    });
    
    test('creates user in external API', async () => {
        const service = new ExternalUserService();
        
        const user = await service.createUser({ name: 'Jane' });
        
        expect(user.id).toBe(2);
    });
});
```

---

## 🔗 Related Topics

- [01_Automated_Testing_Framework.md](./01_Automated_Testing_Framework.md)
- [02_Unit_Testing_Master_Class.md](./02_Unit_Testing_Master_Class.md)

---

**Next: [End to End Testing](./19_END_TO_END_TESTING.md)**