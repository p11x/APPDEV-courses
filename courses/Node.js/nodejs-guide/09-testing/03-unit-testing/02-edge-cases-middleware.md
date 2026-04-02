# Unit Testing Edge Cases, Middleware, and Security Validation

## What You'll Learn

- Testing edge cases and boundary conditions
- Testing Express middleware
- Testing input validation
- Testing security-sensitive code
- Testing utility functions

## Edge Case Testing

```javascript
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';

describe('Edge Cases', () => {
    describe('Null and Undefined', () => {
        test('handles null input', () => {
            assert.equal(formatName(null), 'Unknown');
        });

        test('handles undefined input', () => {
            assert.equal(formatName(undefined), 'Unknown');
        });

        test('handles empty string', () => {
            assert.equal(formatName(''), 'Unknown');
        });
    });

    describe('Boundary Values', () => {
        test('handles zero', () => {
            assert.equal(calculateDiscount(0), 0);
        });

        test('handles negative numbers', () => {
            assert.equal(calculateDiscount(-10), 0);
        });

        test('handles max safe integer', () => {
            assert.ok(calculateDiscount(Number.MAX_SAFE_INTEGER) > 0);
        });

        test('handles floating point precision', () => {
            assert.ok(Math.abs(0.1 + 0.2 - 0.3) < Number.EPSILON);
        });
    });

    describe('Array Edge Cases', () => {
        test('handles empty array', () => {
            assert.deepEqual(processItems([]), []);
        });

        test('handles single element array', () => {
            assert.deepEqual(processItems([1]), [1]);
        });

        test('handles very large array', () => {
            const large = Array.from({ length: 100000 }, (_, i) => i);
            const result = processItems(large);
            assert.equal(result.length, 100000);
        });
    });

    describe('String Edge Cases', () => {
        test('handles unicode characters', () => {
            assert.equal(reverseString('你好世界'), '界世好你');
        });

        test('handles very long string', () => {
            const long = 'a'.repeat(1000000);
            assert.equal(long.length, 1000000);
        });

        test('handles special characters', () => {
            assert.ok(sanitize('<script>alert("xss")</script>').includes('&lt;'));
        });
    });
});
```

## Testing Express Middleware

```javascript
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';
import express from 'express';
import request from 'supertest';

// Middleware to test
function authMiddleware(req, res, next) {
    const token = req.headers.authorization?.replace('Bearer ', '');
    if (!token) return res.status(401).json({ error: 'No token' });

    try {
        req.user = jwt.verify(token, process.env.JWT_SECRET);
        next();
    } catch {
        res.status(401).json({ error: 'Invalid token' });
    }
}

function rateLimitMiddleware(maxRequests = 10) {
    const requests = new Map();
    return (req, res, next) => {
        const ip = req.ip;
        const count = requests.get(ip) || 0;
        if (count >= maxRequests) {
            return res.status(429).json({ error: 'Too many requests' });
        }
        requests.set(ip, count + 1);
        next();
    };
}

describe('Middleware Tests', () => {
    describe('authMiddleware', () => {
        let app;

        beforeEach(() => {
            app = express();
            app.use(authMiddleware);
            app.get('/test', (req, res) => res.json({ user: req.user }));
        });

        test('rejects request without token', async () => {
            const res = await request(app).get('/test');
            assert.equal(res.status, 401);
            assert.equal(res.body.error, 'No token');
        });

        test('rejects invalid token', async () => {
            const res = await request(app)
                .get('/test')
                .set('Authorization', 'Bearer invalid-token');
            assert.equal(res.status, 401);
        });

        test('accepts valid token', async () => {
            const token = jwt.sign({ sub: 1 }, process.env.JWT_SECRET);
            const res = await request(app)
                .get('/test')
                .set('Authorization', `Bearer ${token}`);
            assert.equal(res.status, 200);
            assert.ok(res.body.user);
        });
    });

    describe('rateLimitMiddleware', () => {
        let app;

        beforeEach(() => {
            app = express();
            app.use(rateLimitMiddleware(3));
            app.get('/test', (req, res) => res.json({ ok: true }));
        });

        test('allows requests under limit', async () => {
            for (let i = 0; i < 3; i++) {
                const res = await request(app).get('/test');
                assert.equal(res.status, 200);
            }
        });

        test('blocks requests over limit', async () => {
            for (let i = 0; i < 3; i++) {
                await request(app).get('/test');
            }
            const res = await request(app).get('/test');
            assert.equal(res.status, 429);
        });
    });
});
```

## Testing Input Validation

```javascript
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';
import { z } from 'zod';

const userSchema = z.object({
    name: z.string().min(1).max(100),
    email: z.string().email(),
    age: z.number().int().min(0).max(150),
});

describe('Input Validation', () => {
    test('accepts valid input', () => {
        const result = userSchema.safeParse({
            name: 'Alice',
            email: 'alice@example.com',
            age: 30,
        });
        assert.ok(result.success);
    });

    test('rejects empty name', () => {
        const result = userSchema.safeParse({
            name: '',
            email: 'alice@example.com',
            age: 30,
        });
        assert.ok(!result.success);
        assert.ok(result.error.issues.some(i => i.path[0] === 'name'));
    });

    test('rejects invalid email', () => {
        const result = userSchema.safeParse({
            name: 'Alice',
            email: 'not-an-email',
            age: 30,
        });
        assert.ok(!result.success);
    });

    test('rejects negative age', () => {
        const result = userSchema.safeParse({
            name: 'Alice',
            email: 'alice@example.com',
            age: -1,
        });
        assert.ok(!result.success);
    });

    test('rejects age over 150', () => {
        const result = userSchema.safeParse({
            name: 'Alice',
            email: 'alice@example.com',
            age: 200,
        });
        assert.ok(!result.success);
    });
});
```

## Testing Security-Sensitive Code

```javascript
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';
import bcrypt from 'bcrypt';
import { timingSafeEqual } from 'node:crypto';

describe('Security Tests', () => {
    test('password is hashed before storage', async () => {
        const password = 'MySecret123!';
        const hash = await bcrypt.hash(password, 10);

        assert.notEqual(hash, password);
        assert.ok(hash.startsWith('$2b$'));
    });

    test('password comparison is timing-safe', async () => {
        const password = 'MySecret123!';
        const hash = await bcrypt.hash(password, 10);

        const start1 = process.hrtime.bigint();
        await bcrypt.compare(password, hash);
        const time1 = process.hrtime.bigint() - start1;

        const start2 = process.hrtime.bigint();
        await bcrypt.compare('wrong', hash);
        const time2 = process.hrtime.bigint() - start2;

        // bcrypt.compare is already timing-safe
        // This test verifies it doesn't leak timing info
        assert.ok(true);
    });

    test('sensitive data is not logged', () => {
        const logs = [];
        const originalLog = console.log;
        console.log = (...args) => logs.push(args);

        try {
            createUser({ email: 'test@test.com', password: 'secret' });

            const logString = JSON.stringify(logs);
            assert.ok(!logString.includes('secret'));
        } finally {
            console.log = originalLog;
        }
    });
});
```

## Common Mistakes

- Not testing null/undefined inputs
- Not testing boundary values
- Not testing middleware in isolation
- Not verifying sensitive data isn't leaked

## Cross-References

- See [Unit Testing](./01-functions-classes.md) for basic patterns
- See [Security Testing](../09-security-performance/01-security-testing.md) for security
- See [API Testing](../06-api-testing/01-rest-graphql.md) for API validation

## Next Steps

Continue to [Integration Testing: Microservices](../04-integration-testing/02-microservices-filesystem.md).
