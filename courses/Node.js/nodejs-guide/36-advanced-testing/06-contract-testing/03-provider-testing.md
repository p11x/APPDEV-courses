# Provider Testing

## What You'll Learn

- How to verify provider fulfills contracts
- How to set up provider states
- How to use Pact with Express

## Provider Verification

```ts
// tests/contract/provider.test.ts

import { Verifier } from '@pact-foundation/pact';
import { describe, it } from 'vitest';

describe('UserAPI Provider', () => {
  it('fulfills the contract', async () => {
    const verifier = new Verifier({
      provider: 'UserAPI',
      providerBaseUrl: 'http://localhost:3000',
      pactBrokerUrl: process.env.PACT_BROKER_URL,
      publishVerificationResult: true,
      providerVersion: process.env.GIT_COMMIT,
      stateHandlers: {
        'user 1 exists': async () => {
          await db.users.create({ id: 1, name: 'Alice', email: 'alice@example.com' });
        },
      },
    });

    await verifier.verifyProvider();
  });
});
```

## Next Steps

For API testing, continue to [API Testing](./04-api-testing.md).
