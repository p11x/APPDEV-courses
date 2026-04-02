# Testcontainers with Redis

## What You'll Learn

- How to test with real Redis
- How to set up Redis container
- How to test caching and pub/sub

## Setup

```ts
// tests/redis.test.ts

import { describe, it, beforeAll, afterAll } from 'vitest';
import { RedisContainer } from '@testcontainers/redis';
import Redis from 'ioredis';

describe('Redis Tests', () => {
  let container;
  let redis: Redis;

  beforeAll(async () => {
    container = await new RedisContainer('redis:7-alpine').start();
    redis = new Redis(container.getConnectionUrl());
  }, 60_000);

  afterAll(async () => {
    await redis.quit();
    await container.stop();
  });

  it('sets and gets a value', async () => {
    await redis.set('key', 'value');
    const result = await redis.get('key');
    expect(result).toBe('value');
  });

  it('expires keys', async () => {
    await redis.set('temp', 'data', 'EX', 1);
    const ttl = await redis.ttl('temp');
    expect(ttl).toBeGreaterThan(0);
  });
});
```

## Next Steps

For patterns, continue to [Testcontainers Patterns](./05-testcontainers-patterns.md).
