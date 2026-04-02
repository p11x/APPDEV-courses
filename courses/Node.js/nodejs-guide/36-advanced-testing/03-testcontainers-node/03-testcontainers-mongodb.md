# Testcontainers with MongoDB

## What You'll Learn

- How to test with real MongoDB
- How to set up MongoDB container
- How to handle MongoDB-specific testing

## Setup

```ts
// tests/mongo.test.ts

import { describe, it, beforeAll, afterAll } from 'vitest';
import { MongoDBContainer } from '@testcontainers/mongodb';
import { MongoClient, type Db } from 'mongodb';

describe('MongoDB Tests', () => {
  let container;
  let client: MongoClient;
  let db: Db;

  beforeAll(async () => {
    container = await new MongoDBContainer('mongo:7').start();

    client = new MongoClient(container.getConnectionString());
    await client.connect();
    db = client.db('testdb');
  }, 60_000);

  afterAll(async () => {
    await client.close();
    await container.stop();
  });

  it('inserts and finds a document', async () => {
    const collection = db.collection('users');

    await collection.insertOne({ name: 'Alice', email: 'alice@test.com' });

    const user = await collection.findOne({ email: 'alice@test.com' });
    expect(user?.name).toBe('Alice');
  });
});
```

## Next Steps

For Redis, continue to [Testcontainers Redis](./04-testcontainers-redis.md).
