# Password Hashing, Account Security, and Session Management

## What You'll Learn

- Password hashing comparison (bcrypt, scrypt, argon2)
- Password strength validation
- Account lockout and brute force protection
- Password recovery workflows
- Remember me and persistent sessions

## Password Hashing Comparison

```
Hashing Algorithm Comparison:
─────────────────────────────────────────────
Algorithm  Memory   CPU      Config     Recommendation
─────────────────────────────────────────────
bcrypt     Low      High     Rounds     Good (widely used)
scrypt     High     Medium   Params     Good (memory-hard)
argon2id   Config   High     Params     Best (modern choice)
pbkdf2     Low      High     Rounds     OK (NIST approved)
```

```javascript
import bcrypt from 'bcrypt';
import { scrypt, randomBytes } from 'node:crypto';
import argon2 from 'argon2';

// bcrypt (most common)
class BcryptHasher {
    constructor(saltRounds = 12) {
        this.saltRounds = saltRounds;
    }

    async hash(password) {
        return bcrypt.hash(password, this.saltRounds);
    }

    async verify(password, hash) {
        return bcrypt.compare(password, hash);
    }
}

// Argon2 (recommended for new projects)
class Argon2Hasher {
    async hash(password) {
        return argon2.hash(password, {
            type: argon2.argon2id,
            memoryCost: 65536,  // 64MB
            timeCost: 3,
            parallelism: 4,
        });
    }

    async verify(password, hash) {
        return argon2.verify(hash, password);
    }
}

// scrypt (Node.js built-in)
function scryptHash(password) {
    return new Promise((resolve, reject) => {
        const salt = randomBytes(16);
        scrypt(password, salt, 64, { N: 16384, r: 8, p: 1 }, (err, derived) => {
            if (err) reject(err);
            resolve(`${salt.toString('hex')}:${derived.toString('hex')}`);
        });
    });
}

function scryptVerify(password, hash) {
    return new Promise((resolve, reject) => {
        const [salt, key] = hash.split(':');
        scrypt(password, Buffer.from(salt, 'hex'), 64, { N: 16384, r: 8, p: 1 }, (err, derived) => {
            if (err) reject(err);
            resolve(timingSafeEqual(Buffer.from(key, 'hex'), derived));
        });
    });
}
```

## Password Strength Validation

```javascript
function validatePasswordStrength(password) {
    const checks = {
        minLength: password.length >= 12,
        hasUppercase: /[A-Z]/.test(password),
        hasLowercase: /[a-z]/.test(password),
        hasNumber: /\d/.test(password),
        hasSpecial: /[!@#$%^&*(),.?":{}|<>]/.test(password),
        noCommonPatterns: !/(password|123456|qwerty)/i.test(password),
        noRepeating: !/(.)\1{2,}/.test(password),
    };

    const score = Object.values(checks).filter(Boolean).length;
    const strength = score <= 3 ? 'weak' : score <= 5 ? 'medium' : 'strong';

    return {
        valid: score >= 5,
        strength,
        score,
        checks,
        suggestions: Object.entries(checks)
            .filter(([, v]) => !v)
            .map(([k]) => k),
    };
}
```

## Account Lockout and Brute Force Protection

```javascript
class AccountLocker {
    constructor(redis) {
        this.redis = redis;
        this.maxAttempts = 5;
        this.lockoutDuration = 900; // 15 minutes
        this.attemptWindow = 900; // 15 minutes
    }

    async checkLockout(email) {
        const lockKey = `lockout:${email}`;
        const locked = await this.redis.get(lockKey);
        if (locked) {
            const ttl = await this.redis.ttl(lockKey);
            throw new AuthError(`Account locked. Try again in ${Math.ceil(ttl / 60)} minutes`, 429);
        }
    }

    async recordFailedAttempt(email) {
        const attemptsKey = `attempts:${email}`;
        const attempts = await this.redis.incr(attemptsKey);

        if (attempts === 1) {
            await this.redis.expire(attemptsKey, this.attemptWindow);
        }

        if (attempts >= this.maxAttempts) {
            const lockKey = `lockout:${email}`;
            await this.redis.set(lockKey, '1', { EX: this.lockoutDuration });
            await this.redis.del(attemptsKey);
            throw new AuthError('Account locked due to too many failed attempts', 429);
        }

        return { remaining: this.maxAttempts - attempts };
    }

    async clearAttempts(email) {
        await this.redis.del(`attempts:${email}`);
    }
}
```

## Password Recovery Workflow

```javascript
import { randomBytes } from 'node:crypto';

class PasswordRecovery {
    constructor(pool, redis, emailService) {
        this.pool = pool;
        this.redis = redis;
        this.email = emailService;
    }

    async requestReset(email) {
        const user = await this.pool.query('SELECT id FROM users WHERE email = $1', [email]);
        if (!user.rows[0]) {
            // Don't reveal if user exists
            return { message: 'If the email exists, a reset link was sent' };
        }

        const token = randomBytes(32).toString('hex');
        const key = `password-reset:${token}`;

        await this.redis.set(key, JSON.stringify({
            userId: user.rows[0].id,
            createdAt: Date.now(),
        }), { EX: 3600 }); // 1 hour expiry

        await this.email.send(email, 'Password Reset', {
            resetLink: `https://app.example.com/reset-password?token=${token}`,
            expiresIn: '1 hour',
        });

        return { message: 'If the email exists, a reset link was sent' };
    }

    async resetPassword(token, newPassword) {
        const key = `password-reset:${token}`;
        const data = await this.redis.get(key);

        if (!data) {
            throw new AuthError('Invalid or expired reset token');
        }

        const { userId } = JSON.parse(data);
        const hash = await bcrypt.hash(newPassword, 12);

        await this.pool.query('UPDATE users SET password_hash = $1 WHERE id = $2', [hash, userId]);
        await this.redis.del(key);

        return { message: 'Password updated successfully' };
    }
}
```

## Remember Me and Persistent Sessions

```javascript
import { randomBytes } from 'node:crypto';

class RememberMeService {
    constructor(pool, redis) {
        this.pool = pool;
        this.redis = redis;
    }

    async createToken(userId) {
        const token = randomBytes(32).toString('hex');
        const hash = createHash('sha256').update(token).digest('hex');

        await this.pool.query(
            `INSERT INTO remember_tokens (user_id, token_hash, expires_at)
             VALUES ($1, $2, NOW() + INTERVAL '30 days')`,
            [userId, hash]
        );

        return token;
    }

    async validateToken(token) {
        const hash = createHash('sha256').update(token).digest('hex');
        const { rows } = await this.pool.query(
            `SELECT user_id FROM remember_tokens
             WHERE token_hash = $1 AND expires_at > NOW()`,
            [hash]
        );

        if (rows.length === 0) return null;

        // Rotate token on use (prevents replay)
        await this.pool.query('DELETE FROM remember_tokens WHERE token_hash = $1', [hash]);
        const newToken = await this.createToken(rows[0].user_id);

        return { userId: rows[0].user_id, newToken };
    }

    async revokeAll(userId) {
        await this.pool.query('DELETE FROM remember_tokens WHERE user_id = $1', [userId]);
    }
}
```

## Best Practices Checklist

- [ ] Use argon2id or bcrypt with appropriate cost factors
- [ ] Validate password strength (min 12 chars, complexity)
- [ ] Implement account lockout after 5 failed attempts
- [ ] Use timing-safe comparison for password verification
- [ ] Don't reveal whether email exists during password reset
- [ ] Hash remember-me tokens before storing
- [ ] Rotate remember-me tokens on each use
- [ ] Expire reset tokens within 1 hour

## Cross-References

- See [Fundamentals](./01-fundamentals-architecture.md) for auth concepts
- See [Security](../06-authentication-security/01-security-headers.md) for hardening
- See [JWT](../jwt/04-jwt-refresh-tokens.md) for token-based auth

## Next Steps

Continue to [JWT Comprehensive Guide](../jwt/04-jwt-refresh-tokens.md).
