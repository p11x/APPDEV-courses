# Authentication Testing: Unit, Integration, and Security

## What You'll Learn

- Unit testing authentication logic
- Integration testing auth flows
- Security testing methodologies
- Performance testing auth endpoints
- CI/CD integration for auth testing

## Unit Testing Authentication

```javascript
import { describe, it, beforeEach } from 'node:test';
import assert from 'node:assert/strict';
import bcrypt from 'bcrypt';

describe('Password Hasher', () => {
    it('should hash password with bcrypt', async () => {
        const password = 'SecurePass123!';
        const hash = await bcrypt.hash(password, 10);

        assert.ok(hash.startsWith('$2b$'));
        assert.notEqual(hash, password);
        assert.ok(hash.length > 50);
    });

    it('should verify correct password', async () => {
        const password = 'SecurePass123!';
        const hash = await bcrypt.hash(password, 10);

        assert.ok(await bcrypt.compare(password, hash));
    });

    it('should reject wrong password', async () => {
        const hash = await bcrypt.hash('CorrectPassword', 10);

        assert.ok(!(await bcrypt.compare('WrongPassword', hash)));
    });
});

describe('TokenService', () => {
    let tokenService;

    beforeEach(() => {
        tokenService = new TokenService({
            accessSecret: 'test-secret',
            refreshSecret: 'test-refresh-secret',
        });
    });

    it('should generate access token', () => {
        const token = tokenService.generateAccessToken({ id: 1, role: 'user' });
        assert.ok(typeof token === 'string');
        assert.ok(token.split('.').length === 3); // JWT structure
    });

    it('should verify valid token', () => {
        const token = tokenService.generateAccessToken({ id: 1, role: 'user' });
        const decoded = tokenService.verifyAccessToken(token);

        assert.equal(decoded.sub, 1);
        assert.equal(decoded.role, 'user');
    });

    it('should reject expired token', () => {
        const token = jwt.sign({ sub: 1 }, 'test-secret', { expiresIn: '0s' });

        assert.throws(
            () => tokenService.verifyAccessToken(token),
            { name: 'TokenExpiredError' }
        );
    });

    it('should reject token with wrong secret', () => {
        const token = jwt.sign({ sub: 1 }, 'wrong-secret', { expiresIn: '1h' });

        assert.throws(
            () => tokenService.verifyAccessToken(token),
            { name: 'JsonWebTokenError' }
        );
    });
});
```

## Integration Testing Auth Flows

```javascript
import { describe, it, beforeEach } from 'node:test';
import express from 'express';
import request from 'supertest';

describe('Auth Flow Integration', () => {
    let app;
    let testUser;

    beforeEach(async () => {
        app = express();
        app.use(express.json());

        testUser = await db.users.create({
            email: 'test@example.com',
            passwordHash: await bcrypt.hash('TestPass123!', 10),
            role: 'user',
        });

        setupAuthRoutes(app);
    });

    it('should login with valid credentials', async () => {
        const res = await request(app)
            .post('/auth/login')
            .send({ email: 'test@example.com', password: 'TestPass123!' });

        assert.equal(res.status, 200);
        assert.ok(res.body.accessToken);
        assert.ok(res.body.refreshToken);
    });

    it('should reject invalid credentials', async () => {
        const res = await request(app)
            .post('/auth/login')
            .send({ email: 'test@example.com', password: 'WrongPass' });

        assert.equal(res.status, 401);
        assert.ok(!res.body.accessToken);
    });

    it('should access protected route with valid token', async () => {
        const loginRes = await request(app)
            .post('/auth/login')
            .send({ email: 'test@example.com', password: 'TestPass123!' });

        const res = await request(app)
            .get('/api/profile')
            .set('Authorization', `Bearer ${loginRes.body.accessToken}`);

        assert.equal(res.status, 200);
        assert.equal(res.body.email, 'test@example.com');
    });

    it('should reject protected route without token', async () => {
        const res = await request(app).get('/api/profile');
        assert.equal(res.status, 401);
    });

    it('should refresh token', async () => {
        const loginRes = await request(app)
            .post('/auth/login')
            .send({ email: 'test@example.com', password: 'TestPass123!' });

        const res = await request(app)
            .post('/auth/refresh')
            .send({ refreshToken: loginRes.body.refreshToken });

        assert.equal(res.status, 200);
        assert.ok(res.body.accessToken);
    });

    it('should reject rate-limited login attempts', async () => {
        for (let i = 0; i < 5; i++) {
            await request(app)
                .post('/auth/login')
                .send({ email: 'test@example.com', password: 'Wrong' });
        }

        const res = await request(app)
            .post('/auth/login')
            .send({ email: 'test@example.com', password: 'TestPass123!' });

        assert.equal(res.status, 429);
    });
});
```

## Security Testing

```javascript
describe('Auth Security Tests', () => {
    it('should not reveal user existence on failed login', async () => {
        const existingUser = await request(app)
            .post('/auth/login')
            .send({ email: 'test@example.com', password: 'Wrong' });

        const nonExistingUser = await request(app)
            .post('/auth/login')
            .send({ email: 'nobody@example.com', password: 'Wrong' });

        assert.equal(existingUser.status, 401);
        assert.equal(nonExistingUser.status, 401);
        assert.equal(existingUser.body.error, nonExistingUser.body.error);
    });

    it('should reject SQL injection in email', async () => {
        const res = await request(app)
            .post('/auth/login')
            .send({ email: "'; DROP TABLE users; --", password: 'test' });

        assert.ok(res.status >= 400);
    });

    it('should reject tokens with algorithm confusion', async () => {
        const publicKey = fs.readFileSync('./public.pem', 'utf-8');
        const maliciousToken = jwt.sign({ sub: 1, role: 'admin' }, publicKey, {
            algorithm: 'HS256',
        });

        const res = await request(app)
            .get('/api/profile')
            .set('Authorization', `Bearer ${maliciousToken}`);

        assert.equal(res.status, 401);
    });

    it('should enforce HTTPS in production', async () => {
        // Test that cookies have secure flag
        const res = await request(app)
            .post('/auth/login')
            .send({ email: 'test@example.com', password: 'TestPass123!' });

        const cookie = res.headers['set-cookie'];
        assert.ok(cookie.some(c => c.includes('Secure')));
        assert.ok(cookie.some(c => c.includes('HttpOnly')));
        assert.ok(cookie.some(c => c.includes('SameSite')));
    });
});
```

## Performance Testing

```javascript
import autocannon from 'autocannon';

describe('Auth Performance', () => {
    it('should handle 100 concurrent login requests', async () => {
        const result = await autocannon({
            url: 'http://localhost:3000/auth/login',
            connections: 100,
            duration: 10,
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                email: 'test@example.com',
                password: 'TestPass123!',
            }),
        });

        assert.ok(result.latency.p95 < 500, 'p95 latency should be under 500ms');
        assert.ok(result.errors === 0, 'Should have no errors');
        assert.ok(result.throughput.average > 100, 'Should handle >100 req/s');
    });

    it('should verify JWT in under 1ms', () => {
        const token = jwt.sign({ sub: 1 }, 'test-secret');

        const iterations = 10000;
        const start = performance.now();
        for (let i = 0; i < iterations; i++) {
            jwt.verify(token, 'test-secret');
        }
        const elapsed = performance.now() - start;

        const perVerify = elapsed / iterations;
        assert.ok(perVerify < 1, `Verify took ${perVerify.toFixed(3)}ms, expected <1ms`);
    });
});
```

## Best Practices Checklist

- [ ] Test login with valid and invalid credentials
- [ ] Test token generation, verification, and expiration
- [ ] Test rate limiting behavior
- [ ] Test session fixation prevention
- [ ] Test timing-safe comparison
- [ ] Test CSRF protection
- [ ] Automate auth tests in CI/CD
- [ ] Test with production-like load

## Cross-References

- See [Security](../06-authentication-security/01-security-headers.md) for security patterns
- See [Performance](../08-authentication-performance/01-performance-optimization.md) for optimization
- See [Monitoring](../10-authentication-monitoring/01-monitoring-metrics.md) for observability

## Next Steps

Continue to [Authentication Performance](../08-authentication-performance/01-performance-optimization.md).
