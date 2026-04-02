# Advanced Authentication Patterns for NodeMark

## What You'll Build In This File

Multi-factor authentication, role-based access control, refresh token rotation, and API key management for the capstone project.

## Multi-Factor Authentication (TOTP)

```javascript
// src/services/mfa.js — TOTP-based MFA service
import { authenticator } from 'otplib';
import QRCode from 'qrcode';
import { query } from '../db/index.js';

export class MFAService {
    static async generateSecret(userId, email) {
        const secret = authenticator.generateSecret();
        const otpauthUrl = authenticator.keyuri(email, 'NodeMark', secret);
        const qrCode = await QRCode.toDataURL(otpauthUrl);

        // Store secret temporarily (not enabled until verified)
        await query(
            'UPDATE users SET mfa_secret_temp = $1 WHERE id = $2',
            [secret, userId]
        );

        return { secret, otpauthUrl, qrCode };
    }

    static async enable(userId, token) {
        const { rows } = await query(
            'SELECT mfa_secret_temp FROM users WHERE id = $1',
            [userId]
        );

        const secret = rows[0]?.mfa_secret_temp;
        if (!secret) throw new Error('No MFA setup in progress');

        const isValid = authenticator.verify({ token, secret });
        if (!isValid) throw new Error('Invalid verification code');

        // Generate backup codes
        const backupCodes = Array.from({ length: 10 }, () =>
            Math.random().toString(36).slice(2, 8).toUpperCase()
        );

        await query(
            `UPDATE users SET 
                mfa_secret = $1, mfa_enabled = true,
                mfa_secret_temp = NULL, mfa_backup_codes = $2
             WHERE id = $3`,
            [secret, JSON.stringify(backupCodes), userId]
        );

        return { backupCodes };
    }

    static async verify(userId, token) {
        const { rows } = await query(
            'SELECT mfa_secret, mfa_backup_codes FROM users WHERE id = $1',
            [userId]
        );

        const user = rows[0];
        if (!user?.mfa_secret) return false;

        // Check TOTP
        if (authenticator.verify({ token, secret: user.mfa_secret })) {
            return true;
        }

        // Check backup codes
        const backupCodes = JSON.parse(user.mfa_backup_codes || '[]');
        const codeIndex = backupCodes.indexOf(token.toUpperCase());
        if (codeIndex !== -1) {
            backupCodes.splice(codeIndex, 1);
            await query(
                'UPDATE users SET mfa_backup_codes = $1 WHERE id = $2',
                [JSON.stringify(backupCodes), userId]
            );
            return true;
        }

        return false;
    }
}

// MFA verification middleware
export function requireMFA(req, res, next) {
    if (!req.user.mfaEnabled) return next();

    const mfaToken = req.headers['x-mfa-token'];
    if (!mfaToken) {
        return res.status(403).json({
            error: 'MFA required',
            message: 'Provide X-MFA-Token header',
        });
    }

    MFAService.verify(req.user.userId, mfaToken).then(valid => {
        if (!valid) return res.status(403).json({ error: 'Invalid MFA code' });
        next();
    }).catch(next);
}
```

## Role-Based Access Control (RBAC)

```javascript
// src/middleware/rbac.js — Role-based authorization
const ROLES = {
    user: {
        permissions: ['bookmarks:read', 'bookmarks:write', 'tags:read', 'tags:write'],
    },
    premium: {
        inherits: 'user',
        permissions: ['export:csv', 'export:json', 'api:extended'],
    },
    admin: {
        inherits: 'premium',
        permissions: ['users:read', 'users:write', 'system:manage'],
    },
};

function getRolePermissions(roleName) {
    const role = ROLES[roleName];
    if (!role) return [];

    let permissions = [...(role.permissions || [])];
    if (role.inherits) {
        permissions = [...permissions, ...getRolePermissions(role.inherits)];
    }
    return permissions;
}

export function authorize(...requiredPermissions) {
    return (req, res, next) => {
        const userPermissions = getRolePermissions(req.user.role);

        const hasAll = requiredPermissions.every(p => userPermissions.includes(p));
        if (!hasAll) {
            return res.status(403).json({
                error: 'Forbidden',
                message: 'Insufficient permissions',
                required: requiredPermissions,
            });
        }

        next();
    };
}

// Usage in routes
router.post('/bookmarks', authenticate, authorize('bookmarks:write'), createHandler);
router.get('/export/csv', authenticate, authorize('export:csv'), exportHandler);
router.get('/admin/users', authenticate, authorize('users:read'), listUsersHandler);
```

## Refresh Token Rotation

```javascript
// src/services/tokens.js — Token service with rotation
import jwt from 'jsonwebtoken';
import { randomBytes, createHash } from 'node:crypto';
import { query } from '../db/index.js';
import { config } from '../config/index.js';

export class TokenService {
    static generateAccessToken(user) {
        return jwt.sign(
            { sub: user.id, email: user.email, role: user.role },
            config.jwt.accessSecret,
            { expiresIn: '15m', issuer: 'nodemark' }
        );
    }

    static async generateRefreshToken(userId) {
        const tokenId = randomBytes(16).toString('hex');
        const token = jwt.sign(
            { sub: userId, jti: tokenId, type: 'refresh' },
            config.jwt.refreshSecret,
            { expiresIn: '7d' }
        );

        const hash = createHash('sha256').update(token).digest('hex');
        await query(
            `INSERT INTO refresh_tokens (token_id, user_id, token_hash, expires_at)
             VALUES ($1, $2, $3, NOW() + INTERVAL '7 days')`,
            [tokenId, userId, hash]
        );

        return token;
    }

    static async refresh(refreshToken) {
        const decoded = jwt.verify(refreshToken, config.jwt.refreshSecret, {
            issuer: 'nodemark',
        });

        // Verify token exists and is valid
        const { rows } = await query(
            'SELECT * FROM refresh_tokens WHERE token_id = $1 AND revoked_at IS NULL',
            [decoded.jti]
        );

        if (rows.length === 0) throw new Error('Token revoked');

        const tokenHash = createHash('sha256').update(refreshToken).digest('hex');
        if (rows[0].token_hash !== tokenHash) throw new Error('Token invalid');

        // Rotate: revoke old, issue new
        await query('UPDATE refresh_tokens SET revoked_at = NOW() WHERE token_id = $1', [decoded.jti]);

        const { rows: [user] } = await query('SELECT * FROM users WHERE id = $1', [decoded.sub]);

        return {
            accessToken: this.generateAccessToken(user),
            refreshToken: await this.generateRefreshToken(user.id),
        };
    }

    static async revokeAll(userId) {
        await query(
            'UPDATE refresh_tokens SET revoked_at = NOW() WHERE user_id = $1 AND revoked_at IS NULL',
            [userId]
        );
    }
}
```

## How It Connects

- MFA follows [08-authentication/06-multi-factor-auth.md](../../../08-authentication/06-multi-factor-auth.md)
- JWT follows [08-authentication/jwt/](../../../08-authentication/jwt/) patterns
- RBAC follows [08-authentication/01-authentication-fundamentals/01-fundamentals-architecture.md](../../../08-authentication/01-authentication-fundamentals/01-fundamentals-architecture.md)

## Common Mistakes

- Storing MFA secrets in plaintext
- Not rotating refresh tokens on use
- Not implementing role inheritance
- Missing backup code generation

## Try It Yourself

### Exercise 1: Enable MFA
Register, login, enable MFA, then verify with an authenticator app.

### Exercise 2: Test RBAC
Create users with different roles and test permission checks.

### Exercise 3: Token Rotation
Test that old refresh tokens are invalidated after rotation.

## Next Steps

Continue to [15-advanced-api/01-versioning-rate-limiting.md](../15-advanced-api/01-versioning-rate-limiting.md).
