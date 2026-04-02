# Testing Frameworks Comparison

## What You'll Learn

- Node.js built-in test runner
- Jest vs Vitest comparison
- Test organization patterns
- CI/CD test integration

## Node.js Built-In Test Runner (Node.js 20+)

```javascript
// tests/api.test.js — Using node:test

import { describe, it, before, after, mock } from 'node:test';
import assert from 'node:assert/strict';

describe('User API', () => {
    let server;
    
    before(async () => {
        server = await startServer();
    });
    
    after(async () => {
        await server.close();
    });
    
    it('should return user list', async () => {
        const res = await fetch('http://localhost:3000/api/users');
        assert.equal(res.status, 200);
        const body = await res.json();
        assert.ok(Array.isArray(body.data));
    });
    
    it('should create user', async () => {
        const res = await fetch('http://localhost:3000/api/users', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name: 'Test', email: 'test@example.com' }),
        });
        assert.equal(res.status, 201);
    });
    
    it('should mock external service', async () => {
        const fn = mock.fn(() => Promise.resolve({ data: 'mocked' }));
        const result = await fn();
        assert.equal(fn.mock.calls.length, 1);
        assert.deepEqual(result, { data: 'mocked' });
    });
});
```

```bash
# Run tests
node --test tests/**/*.test.js

# With coverage (Node.js 20+)
node --test --experimental-test-coverage tests/**/*.test.js

# Watch mode
node --test --watch tests/**/*.test.js
```

## Jest vs Vitest Comparison

```
Jest vs Vitest:
─────────────────────────────────────────────
Feature          Jest            Vitest
─────────────────────────────────────────────
Speed            Good            Excellent (Vite)
ESM Support      Limited         Native
TypeScript       Via transform   Native
Watch Mode       Good            Excellent
API              describe/it     describe/it (compatible)
Config           jest.config     vitest.config
UI               None            Built-in UI
─────────────────────────────────────────────
Choose Jest:     Large existing codebase
Choose Vitest:   New projects, ESM-first
Choose node:test No external dependencies
```

## Test Organization

```
project/
├── src/
│   ├── services/
│   │   └── user.service.ts
│   └── routes/
│       └── users.routes.ts
├── tests/
│   ├── unit/
│   │   └── user.service.test.ts
│   ├── integration/
│   │   └── users.api.test.ts
│   └── e2e/
│       └── registration.test.ts
└── package.json
```

## Best Practices Checklist

- [ ] Use built-in test runner for simple projects
- [ ] Use Vitest for TypeScript/ESM projects
- [ ] Separate unit, integration, and e2e tests
- [ ] Mock external dependencies in unit tests
- [ ] Run tests in CI/CD pipeline
- [ ] Aim for >80% code coverage on critical paths

## Cross-References

- See [TypeScript Integration](./01-typescript-integration.md) for TypeScript setup
- See [ESLint Prettier](./02-eslint-prettier.md) for code quality
- See [Testing](../../../09-testing/) for comprehensive testing guide

## Next Steps

Continue to [Security Best Practices](../21-security-modern/01-security-headers-deps.md) for security hardening.
