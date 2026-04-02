# Pact Setup

## What You'll Learn

- What contract testing is
- How Pact works
- How to set up Pact with Node.js
- How contract testing differs from integration testing

## What Is Contract Testing?

Contract testing verifies that two services agree on the API contract. Instead of testing services together (integration), each service tests against a shared contract.

```
Consumer (web app)          Provider (API)
    │                            │
    ├── Defines expected contract │
    │                            │
    │──── Pact contract ────────→│
    │                            │
    │                            ├── Verifies it can fulfill contract
```

## Setup

```bash
npm install @pact-foundation/pact
```

## Consumer Test

```ts
// tests/contract/consumer.test.ts

import { Pact } from '@pact-foundation/pact';
import { describe, it, beforeAll, afterAll } from 'vitest';

const provider = new Pact({
  consumer: 'WebApp',
  provider: 'UserAPI',
  port: 1234,
});

describe('UserAPI Contract', () => {
  beforeAll(() => provider.setup());
  afterAll(() => provider.finalize());

  it('returns a user', async () => {
    await provider.addInteraction({
      state: 'a user with ID 1 exists',
      uponReceiving: 'a request for user 1',
      withRequest: {
        method: 'GET',
        path: '/api/users/1',
      },
      willRespondWith: {
        status: 200,
        body: {
          id: 1,
          name: 'Alice',
          email: 'alice@example.com',
        },
      },
    });

    const response = await fetch(`${provider.mockService.baseUrl}/api/users/1`);
    const user = await response.json();

    expect(user.name).toBe('Alice');
  });
});
```

## Next Steps

For consumer testing, continue to [Consumer Testing](./02-consumer-testing.md).
