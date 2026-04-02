# Authentication Security Testing and CI/CD Integration

## What You'll Learn

- Security vulnerability testing for auth
- Automated security scanning in CI/CD
- Penetration testing patterns
- Compliance testing automation

## Security Vulnerability Tests

```javascript
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';
import request from 'supertest';

describe('Authentication Security Tests', () => {
    let app;

    before(async () => {
        app = await createTestApp();
    });

    describe('SQL Injection Prevention', () => {
        test('rejects SQL injection in email', async () => {
            const payloads = [
                "'; DROP TABLE users; --",
                "' OR '1'='1",
                "' UNION SELECT * FROM users --",
                "admin'--",
            ];

            for (const payload of payloads) {
                const res = await request(app)
                    .post('/auth/login')
                    .send({ email: payload, password: 'test' });

                assert.ok(res.status >= 400, `Payload "${payload}" should be rejected`);
            }
        });
    });

    describe('XSS Prevention', () => {
        test('sanitizes user input in responses', async () => {
            const xssPayload = '<script>alert("xss")</script>';
            const res = await request(app)
                .post('/auth/register')
                .send({
                    name: xssPayload,
                    email: 'xss@test.com',
                    password: 'SecurePass123!',
                });

            if (res.status === 201) {
                assert.ok(!res.body.name.includes('<script>'));
            }
        });

        test('sets Content-Type to application/json', async () => {
            const res = await request(app).get('/api/profile');
            assert.ok(res.headers['content-type'].includes('application/json'));
            assert.ok(!res.headers['content-type'].includes('text/html'));
        });
    });

    describe('Rate Limiting', () => {
        test('limits login attempts per IP', async () => {
            for (let i = 0; i < 10; i++) {
                await request(app)
                    .post('/auth/login')
                    .send({ email: 'test@test.com', password: 'Wrong' });
            }

            const res = await request(app)
                .post('/auth/login')
                .send({ email: 'test@test.com', password: 'CorrectPass123!' });

            assert.equal(res.status, 429);
        });
    });

    describe('Token Security', () => {
        test('rejects expired tokens', async () => {
            const expired = jwt.sign({ sub: 1 }, 'test', { expiresIn: '0s' });
            const res = await request(app)
                .get('/api/profile')
                .set('Authorization', `Bearer ${expired}`);

            assert.equal(res.status, 401);
        });

        test('rejects tokens with wrong algorithm', async () => {
            const token = jwt.sign({ sub: 1 }, 'wrong-secret');
            const res = await request(app)
                .get('/api/profile')
                .set('Authorization', `Bearer ${token}`);

            assert.equal(res.status, 401);
        });

        test('rejects tokens without issuer claim', async () => {
            const token = jwt.sign({ sub: 1 }, process.env.JWT_SECRET);
            const res = await request(app)
                .get('/api/profile')
                .set('Authorization', `Bearer ${token}`);

            assert.equal(res.status, 401);
        });
    });

    describe('Session Security', () => {
        test('cookies have secure flags', async () => {
            const res = await request(app)
                .post('/auth/login')
                .send({ email: 'user@test.com', password: 'Pass123!' });

            const cookie = res.headers['set-cookie'];
            if (cookie) {
                assert.ok(cookie.some(c => c.includes('HttpOnly')));
                assert.ok(cookie.some(c => c.includes('Secure')));
                assert.ok(cookie.some(c => c.includes('SameSite')));
            }
        });

        test('regenerates session on login', async () => {
            // This test verifies session fixation prevention
            const res1 = await request(app).get('/auth/csrf-token');
            const sessionId1 = res1.headers['set-cookie'];

            const res2 = await request(app)
                .post('/auth/login')
                .send({ email: 'user@test.com', password: 'Pass123!' });
            const sessionId2 = res2.headers['set-cookie'];

            // Session ID should change after login
            assert.notDeepEqual(sessionId1, sessionId2);
        });
    });

    describe('Error Information Leakage', () => {
        test('does not reveal user existence', async () => {
            const existing = await request(app)
                .post('/auth/login')
                .send({ email: 'existing@test.com', password: 'Wrong' });

            const nonExisting = await request(app)
                .post('/auth/login')
                .send({ email: 'nobody@test.com', password: 'Wrong' });

            assert.equal(existing.status, nonExisting.status);
            assert.equal(existing.body.error, nonExisting.body.error);
        });

        test('does not expose stack traces', async () => {
            const res = await request(app)
                .post('/auth/login')
                .send({ email: 'test@test.com', password: 'test' });

            assert.ok(!res.body.stack, 'Should not expose stack trace');
        });
    });
});
```

## CI/CD Security Scanning

```yaml
# .github/workflows/security.yml
name: Auth Security

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]
  schedule:
    - cron: '0 6 * * 1' # Weekly Monday scan

jobs:
  dependency-scan:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - run: npm ci
      - run: npm audit --audit-level=high --json > audit.json || true
      - name: Check for critical vulnerabilities
        run: |
          CRITICAL=$(cat audit.json | jq '.metadata.vulnerabilities.critical')
          if [ "$CRITICAL" -gt 0 ]; then
            echo "Found $CRITICAL critical vulnerabilities"
            exit 1
          fi

  secrets-scan:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
        with:
          fetch-depth: 0
      - name: Gitleaks
        uses: gitleaks/gitleaks-action@v2
        env:
          GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}

  security-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'
      - run: npm ci
      - run: npm run test:security
        env:
          JWT_SECRET: test-secret-for-ci
```

## Common Mistakes

- Not running security tests in CI/CD
- Testing only happy paths, not attack vectors
- Not scanning for committed secrets
- Ignoring npm audit findings

## Cross-References

- See [Unit Testing](./01-unit-testing.md) for test patterns
- See [Security](../06-authentication-security/01-security-headers.md) for implementation
- See [CI/CD](../../26-cicd-github-actions/) for pipeline setup

## Next Steps

Continue to [Performance: Caching and Load Testing](../08-authentication-performance/02-caching-load-testing.md).
