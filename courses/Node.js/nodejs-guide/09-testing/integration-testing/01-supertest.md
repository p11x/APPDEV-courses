# Testing Express with Supertest

## What You'll Learn

- Installing supertest
- Making HTTP requests in tests

## Installing Supertest

```bash
npm install --save-dev supertest
```

## Using Supertest

```javascript
// supertest-demo.js - Testing Express with supertest

import express from 'express';
import request from 'supertest';

const app = express();

app.get('/api/users', (req, res) => {
  res.json([{ id: 1, name: 'Alice' }]);
});

app.post('/api/users', (req, res) => {
  res.status(201).json({ id: 2, ...req.body });
});

// Test
request(app)
  .get('/api/users')
  .expect(200)
  .then(res => {
    console.log(res.body);
  });
```

## Code Example

```javascript
// test/supertest.test.js - Complete supertest example

import express from 'express';
import request from 'supertest';

const app = express();
app.use(express.json());

// Routes
app.get('/api/hello', (req, res) => {
  res.json({ message: 'Hello!' });
});

app.post('/api/echo', (req, res) => {
  res.json(req.body);
});

describe('API Tests', () => {
  test('GET /api/hello', async () => {
    const res = await request(app).get('/api/hello');
    expect(res.status).toBe(200);
    expect(res.body.message).toBe('Hello!');
  });
  
  test('POST /api/echo', async () => {
    const res = await request(app)
      .post('/api/echo')
      .send({ name: 'Alice' });
    expect(res.status).toBe(200);
    expect(res.body.name).toBe('Alice');
  });
});
```

## Try It Yourself

### Exercise 1: Test Routes
Test your Express routes using supertest.

### Exercise 2: POST Requests
Test POST requests with JSON body.
