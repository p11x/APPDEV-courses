# Testcontainers Patterns

## What You'll Learn

- How to compose multiple containers
- How to use Testcontainers with Docker Compose
- How to optimize container startup
- How to share containers across tests

## Docker Compose

```ts
// tests/compose.test.ts

import { DockerComposeEnvironment } from 'testcontainers';

describe('Full Stack', () => {
  let environment;

  beforeAll(async () => {
    environment = await new DockerComposeEnvironment(
      resolve(__dirname, '..'),
      'docker-compose.test.yml'
    ).up();

    // Access containers
    const postgres = environment.getContainer('postgres');
    const redis = environment.getContainer('redis');
  }, 120_000);

  afterAll(async () => {
    await environment.down();
  });
});
```

## Reusable Containers

```ts
// tests/global-setup.ts — Share container across test files

import { PostgreSqlContainer } from '@testcontainers/postgresql';

let container;

export async function setup() {
  container = await new PostgreSqlContainer('postgres:16').start();
  process.env.DATABASE_URL = container.getConnectionUri();
}

export async function teardown() {
  await container?.stop();
}
```

## Next Steps

For Bun testing, continue to [Bun Test Setup](../04-bun-testing/01-bun-test-setup.md).
