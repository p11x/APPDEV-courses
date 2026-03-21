# Contract Testing with Pact

## 📌 What You'll Learn

- What contract testing is
- Consumer-driven contracts
- Pact broker integration

## 🧠 Concept Explained (Plain English)

Contract testing is a way to verify that two services (like a frontend and an API) can communicate correctly without running the full integration. Traditional integration testing requires all services to be running together, which becomes slow and fragile as systems grow.

In contract testing, the consumer (like a React app making API calls) writes tests that define what it expects from the provider (Express API). These expectations are saved as a "contract." The provider then verifies it meets all the contracts from its consumers.

Pact is the most popular tool for this. It records HTTP interactions and verifies both sides match.

## 💻 Code Example

```js
// Consumer side (frontend/microservice) tests
// npm install -D @pact-foundation/pact

import { Pact } from '@pact-foundation/pact';
import { describe, it, expect } from 'vitest';

describe('API Contract Tests', () => {
  const pact = new Pact({
    consumer: 'web-app',
    provider: 'express-api',
    port: 3000,
  });

  beforeAll(() => pact.setup());
  afterEach(() => pact.writePact());
  afterAll(() => pact.finalize());

  it('should get user list', async () => {
    // Define expected interaction
    await pact.addInteraction({
      state: 'users exist',
      uponReceiving: 'a request for users',
      withRequest: {
        method: 'GET',
        path: '/users',
      },
      willRespondWith: {
        status: 200,
        headers: { 'Content-Type': 'application/json' },
        body: [
          { id: '1', name: 'Alice', email: 'alice@example.com' },
          { id: '2', name: 'Bob', email: 'bob@example.com' },
        ],
      },
    });

    // Make the actual request
    const response = await fetch('http://localhost:3000/users');
    const users = await response.json();

    expect(response.status).toBe(200);
    expect(users).toHaveLength(2);
  });
});
```

## Provider Side Verification

```js
// Provider side (Express API) verification
// npm install -D @pact-foundation/pact-verifier

// verifier.js
import { Verifier } from '@pact-foundation/pact-verifier';
import express from 'express';

const app = express();

// Set up your API routes first
app.get('/users', (req, res) => {
  res.json([
    { id: '1', name: 'Alice', email: 'alice@example.com' },
    { id: '2', name: 'Bob', email: 'bob@example.com' },
  ]);
});

// Start your provider
const server = app.listen(3000, async () => {
  console.log('Provider running on port 3000');
  
  try {
    const verifier = new Verifier({
      provider: 'express-api',
      providerBaseUrl: 'http://localhost:3000',
      pactUrls: ['./pacts/web-app-express-api.json'],
    });
    
    const result = await verifier.verifyProvider();
    console.log('Pact Verification Result:', result);
  } catch (error) {
    console.error('Verification failed:', error);
  } finally {
    server.close();
  }
});
```

## Pact Broker Integration

```js
// Publishing contracts to Pact Broker
// npm install -D @pact-foundation/pact-core

// In your CI pipeline after consumer tests pass:
import { Publisher } from '@pact-foundation/pact-core';

const publisher = new Publisher({
  pactBroker: 'https://pact-broker.example.com',
  pactBrokerToken: process.env.PACT_BROKER_TOKEN,
  consumerVersion: '1.0.0',
});

await publisher.publishPacts({
  pactFiles: ['./pacts/'],
});
```

## CI Integration

```yaml
# .github/workflows/ci.yml
jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      
      - name: Run Consumer Tests
        run: npm test
        env:
          PACT_BROKER_URL: https://pact-broker.example.com
          PACT_BROKER_TOKEN: ${{ secrets.PACT_BROKER_TOKEN }}
      
      - name: Publish Contracts
        run: npm run publish:pacts
        if: github.ref == 'refs/heads/main'
      
      - name: Verify Provider Contracts
        run: npm run verify:pacts
        if: github.ref == 'refs/heads/main'
```

## ⚠️ Common Mistakes

1. **Not sharing contracts**: Without a broker, teams can't easily share contract files
2. **Testing too much**: Keep contract tests focused on API surface, not business logic
3. **Missing provider states**: Make sure provider can set up test data for each contract

## ✅ Quick Recap

- Contract testing verifies API compatibility without full integration
- Consumer writes expectations, provider verifies it meets them
- Use Pact for HTTP API contract testing
- Publish contracts to Pact Broker for team sharing
- Run verification in CI to catch breaking changes

## 🔗 What's Next

Learn about API mocking for frontend development.
