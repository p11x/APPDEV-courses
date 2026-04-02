# Modern Authentication Patterns

## What You'll Learn

- Passwordless authentication (WebAuthn/FIDO2)
- Single Sign-On (SSO) implementation
- API key authentication
- Microservices authentication patterns
- Zero-trust authentication

## Passwordless Authentication with WebAuthn

```javascript
import {
    generateRegistrationOptions,
    verifyRegistrationResponse,
    generateAuthenticationOptions,
    verifyAuthenticationResponse,
} from '@simplewebauthn/server';

// Registration
app.post('/auth/webauthn/register', async (req, res) => {
    const user = req.user; // Already authenticated

    const options = await generateRegistrationOptions({
        rpName: 'My App',
        rpID: 'example.com',
        userID: user.id,
        userName: user.email,
        attestationType: 'none',
        authenticatorSelection: {
            residentKey: 'preferred',
            userVerification: 'preferred',
        },
    });

    // Store challenge in session
    req.session.challenge = options.challenge;

    res.json(options);
});

app.post('/auth/webauthn/register/verify', async (req, res) => {
    const expectedChallenge = req.session.challenge;

    const verification = await verifyRegistrationResponse({
        response: req.body,
        expectedChallenge,
        expectedOrigin: 'https://example.com',
        expectedRPID: 'example.com',
    });

    if (verification.verified) {
        // Store credential
        await db.credentials.create({
            userId: req.user.id,
            credentialID: verification.registrationInfo.credentialID,
            publicKey: verification.registrationInfo.credentialPublicKey,
            counter: verification.registrationInfo.counter,
        });
        res.json({ verified: true });
    } else {
        res.status(400).json({ verified: false });
    }
});

// Authentication
app.post('/auth/webauthn/authenticate', async (req, res) => {
    const options = await generateAuthenticationOptions({
        rpID: 'example.com',
        userVerification: 'preferred',
    });

    req.session.challenge = options.challenge;
    res.json(options);
});

app.post('/auth/webauthn/authenticate/verify', async (req, res) => {
    const credential = await db.credentials.findOne({
        credentialID: req.body.id,
    });

    const verification = await verifyAuthenticationResponse({
        response: req.body,
        expectedChallenge: req.session.challenge,
        expectedOrigin: 'https://example.com',
        expectedRPID: 'example.com',
        authenticator: {
            credentialPublicKey: credential.publicKey,
            credentialID: credential.credentialID,
            counter: credential.counter,
        },
    });

    if (verification.verified) {
        // Update counter
        await db.credentials.update(credential.id, {
            counter: verification.authenticationInfo.newCounter,
        });

        // Issue session/token
        const token = generateToken(credential.userId);
        res.json({ verified: true, token });
    } else {
        res.status(400).json({ verified: false });
    }
});
```

## Single Sign-On (SAML)

```javascript
import { SAML } from '@node-saml/node-saml';

const saml = new SAML({
    cert: fs.readFileSync('/path/to/idp-cert.pem', 'utf-8'),
    issuer: 'https://app.example.com',
    callbackUrl: 'https://app.example.com/auth/saml/callback',
    entryPoint: 'https://idp.example.com/sso/saml',
    identifierFormat: 'urn:oasis:names:tc:SAML:1.1:nameid-format:emailAddress',
});

// SP-initiated SSO
app.get('/auth/saml/login', async (req, res) => {
    const loginUrl = await saml.getAuthorizeUrlAsync();
    res.redirect(loginUrl);
});

// Handle SAML response
app.post('/auth/saml/callback', async (req, res) => {
    try {
        const { profile } = await saml.validatePostResponseAsync(req.body);

        let user = await db.users.findByEmail(profile.nameID);
        if (!user) {
            user = await db.users.create({
                email: profile.nameID,
                name: profile.displayName || profile.nameID,
                ssoProvider: 'saml',
            });
        }

        req.session.userId = user.id;
        res.redirect('/dashboard');
    } catch (err) {
        console.error('SAML validation error:', err);
        res.redirect('/login?error=saml_failed');
    }
});

// SP metadata
app.get('/auth/saml/metadata', (req, res) => {
    res.type('application/xml');
    res.send(saml.generateServiceProviderMetadata());
});
```

## API Key Authentication

```javascript
import { randomBytes, createHash } from 'node:crypto';

class ApiKeyService {
    constructor(pool) {
        this.pool = pool;
    }

    async createKey(userId, name, scopes = []) {
        const key = `sk_${randomBytes(32).toString('hex')}`;
        const hash = createHash('sha256').update(key).digest('hex');

        await this.pool.query(
            `INSERT INTO api_keys (user_id, name, key_hash, scopes, created_at)
             VALUES ($1, $2, $3, $4, NOW())`,
            [userId, name, hash, JSON.stringify(scopes)]
        );

        return key; // Only shown once
    }

    async validateKey(key) {
        const hash = createHash('sha256').update(key).digest('hex');

        const { rows } = await this.pool.query(
            `SELECT ak.*, u.email, u.role
             FROM api_keys ak
             JOIN users u ON u.id = ak.user_id
             WHERE ak.key_hash = $1 AND ak.revoked_at IS NULL`,
            [hash]
        );

        if (rows.length === 0) return null;

        // Update last used
        await this.pool.query(
            'UPDATE api_keys SET last_used_at = NOW() WHERE key_hash = $1',
            [hash]
        );

        return rows[0];
    }

    async revokeKey(keyId, userId) {
        await this.pool.query(
            'UPDATE api_keys SET revoked_at = NOW() WHERE id = $1 AND user_id = $2',
            [keyId, userId]
        );
    }
}

// Middleware
function apiKeyAuth(apiKeyService) {
    return async (req, res, next) => {
        const key = req.headers['x-api-key'] ||
            req.query.api_key ||
            req.headers.authorization?.replace('Bearer ', '');

        if (!key) {
            return res.status(401).json({ error: 'API key required' });
        }

        const keyData = await apiKeyService.validateKey(key);
        if (!keyData) {
            return res.status(401).json({ error: 'Invalid API key' });
        }

        req.apiKey = keyData;
        req.user = { id: keyData.user_id, email: keyData.email, role: keyData.role };
        next();
    };
}
```

## Microservices Authentication

```javascript
// Service-to-service JWT (asymmetric keys)
import jwt from 'jsonwebtoken';
import { readFileSync } from 'node:fs';

class ServiceAuth {
    constructor() {
        this.privateKey = readFileSync('./keys/service-private.pem');
        this.publicKeys = new Map(); // Service ID → public key
    }

    createServiceToken(serviceId, scopes) {
        return jwt.sign(
            {
                sub: serviceId,
                type: 'service',
                scope: scopes.join(' '),
            },
            this.privateKey,
            {
                algorithm: 'RS256',
                expiresIn: '5m',
                issuer: serviceId,
            }
        );
    }

    registerPublicKey(serviceId, publicKey) {
        this.publicKeys.set(serviceId, publicKey);
    }

    verifyServiceToken(token) {
        const decoded = jwt.decode(token, { complete: true });
        const publicKey = this.publicKeys.get(decoded.payload.iss);

        if (!publicKey) {
            throw new Error(`Unknown service: ${decoded.payload.iss}`);
        }

        return jwt.verify(token, publicKey, {
            algorithms: ['RS256'],
            clockTolerance: 10,
        });
    }
}

// Middleware
function serviceAuthMiddleware(serviceAuth) {
    return (req, res, next) => {
        const token = req.headers.authorization?.replace('Bearer ', '');
        if (!token) return res.status(401).json({ error: 'No token' });

        try {
            req.service = serviceAuth.verifyServiceToken(token);
            next();
        } catch (err) {
            res.status(401).json({ error: 'Invalid service token' });
        }
    };
}
```

## Best Practices Checklist

- [ ] Use WebAuthn for passwordless auth on supported platforms
- [ ] Implement SAML for enterprise SSO
- [ ] Hash API keys before storing
- [ ] Use asymmetric JWT for service-to-service auth
- [ ] Implement rate limiting per API key
- [ ] Rotate API keys regularly
- [ ] Log all authentication events

## Cross-References

- See [OAuth2](../04-oauth2-oidc/01-authorization-code-flow.md) for OAuth flows
- See [Security](../06-authentication-security/01-security-headers.md) for hardening
- See [Testing](../07-authentication-testing/01-unit-testing.md) for auth testing

## Next Steps

Continue to [Authentication Security](../06-authentication-security/01-security-headers.md).
