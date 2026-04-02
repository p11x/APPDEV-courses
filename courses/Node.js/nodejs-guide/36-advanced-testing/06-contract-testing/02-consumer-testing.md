# Consumer Testing

## What You'll Learn

- How to write consumer-driven contract tests
- How to define expected API behavior
- How to publish contracts
- How to verify contracts

## Consumer Test

```ts
// tests/contract/user-consumer.test.ts

import { PactV4 } from '@pact-foundation/pact';
import { describe, it, beforeAll, afterAll } from 'vitest';

const provider = new PactV4({
  consumer: 'WebApp',
  provider: 'UserAPI',
});

describe('User API Consumer', () => {
  it('gets a user by ID', async () => {
    await provider
      .addInteraction()
      .given('user 1 exists')
      .uponReceiving('a request for user 1')
      .withRequest('GET', '/api/users/1')
      .willRespondWith(200, (builder) => {
        builder.headers({ 'Content-Type': 'application/json' });
        builder.jsonBody({
          id: 1,
          name: 'Alice',
          email: 'alice@example.com',
        });
      })
      .executeTest(async (mockServer) => {
        const response = await fetch(`${mockServer.url}/api/users/1`);
        const user = await response.json();
        expect(user.name).toBe('Alice');
      });
  });
});
```

## Next Steps

For provider testing, continue to [Provider Testing](./03-provider-testing.md).
