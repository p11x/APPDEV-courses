# OAuth 2.0 Authorization Code Flow and Token Management

## What You'll Learn

- OAuth 2.0 authorization code flow (full implementation)
- OAuth 2.0 with PKCE for public clients
- OAuth 2.0 client credentials flow
- Token introspection and validation
- OAuth 2.0 error handling

## Authorization Code Flow

```
OAuth 2.0 Authorization Code Flow:
─────────────────────────────────────────────
Client App         Auth Server        Resource Server
    │                   │                    │
    │── /authorize ────►│                    │
    │   ?response_type= │                    │
    │   code&client_id= │                    │
    │   &redirect_uri=  │                    │
    │   &scope=read     │                    │
    │                   │                    │
    │◄── redirect with ─│                    │
    │    ?code=abc123   │                    │
    │                   │                    │
    │── /token ────────►│                    │
    │   grant_type=     │                    │
    │   authorization_  │                    │
    │   code&code=abc123│                    │
    │   &client_secret  │                    │
    │◄── {access_token, │                    │
    │     refresh_token}│                    │
    │                   │                    │
    │── /resource ──────────────────────────►│
    │   Authorization: Bearer token          │
    │◄── {data} ────────────────────────────│
```

```javascript
// OAuth 2.0 Authorization Server implementation
import express from 'express';
import { randomBytes } from 'node:crypto';
import jwt from 'jsonwebtoken';

class AuthorizationServer {
    constructor(pool, redis) {
        this.pool = pool;
        this.redis = redis;
        this.clients = new Map(); // Registered clients
    }

    registerClient(client) {
        this.clients.set(client.id, {
            id: client.id,
            secret: client.secret,
            redirectUris: client.redirectUris,
            grants: client.grants || ['authorization_code', 'refresh_token'],
        });
    }

    // Step 1: Authorization endpoint
    authorize(req, res) {
        const { response_type, client_id, redirect_uri, scope, state } = req.query;

        const client = this.clients.get(client_id);
        if (!client) {
            return res.status(400).json({ error: 'invalid_client' });
        }

        if (!client.redirectUris.includes(redirect_uri)) {
            return res.status(400).json({ error: 'invalid_redirect_uri' });
        }

        if (response_type !== 'code') {
            return res.redirect(`${redirect_uri}?error=unsupported_response_type&state=${state}`);
        }

        // Render login/consent page (simplified)
        res.render('authorize', { client, scope, redirect_uri, state });
    }

    // Step 2: Handle user consent
    async handleConsent(req, res) {
        const { user_id, client_id, redirect_uri, scope, state } = req.body;

        const code = randomBytes(32).toString('hex');

        // Store authorization code (short-lived)
        await this.redis.set(`auth_code:${code}`, JSON.stringify({
            userId: user_id,
            clientId: client_id,
            scope,
            redirectUri: redirect_uri,
        }), { EX: 300 }); // 5 minutes

        res.redirect(`${redirect_uri}?code=${code}&state=${state}`);
    }

    // Step 3: Token endpoint
    async token(req, res) {
        const { grant_type, code, client_id, client_secret, redirect_uri } = req.body;

        if (grant_type === 'authorization_code') {
            const client = this.clients.get(client_id);
            if (!client || client.secret !== client_secret) {
                return res.status(401).json({ error: 'invalid_client' });
            }

            const codeData = await this.redis.get(`auth_code:${code}`);
            if (!codeData) {
                return res.status(400).json({ error: 'invalid_grant' });
            }

            const { userId, scope } = JSON.parse(codeData);
            await this.redis.del(`auth_code:${code}`);

            const accessToken = jwt.sign(
                { sub: userId, scope, client_id },
                process.env.JWT_SECRET,
                { expiresIn: '1h', issuer: 'auth-server' }
            );

            const refreshToken = randomBytes(32).toString('hex');
            await this.redis.set(`refresh:${refreshToken}`, JSON.stringify({
                userId, clientId: client_id, scope,
            }), { EX: 30 * 24 * 60 * 60 }); // 30 days

            res.json({
                access_token: accessToken,
                token_type: 'Bearer',
                expires_in: 3600,
                refresh_token: refreshToken,
                scope,
            });
        }

        if (grant_type === 'refresh_token') {
            const refreshData = await this.redis.get(`refresh:${req.body.refresh_token}`);
            if (!refreshData) {
                return res.status(400).json({ error: 'invalid_grant' });
            }

            const { userId, scope } = JSON.parse(refreshData);
            const accessToken = jwt.sign(
                { sub: userId, scope },
                process.env.JWT_SECRET,
                { expiresIn: '1h' }
            );

            res.json({
                access_token: accessToken,
                token_type: 'Bearer',
                expires_in: 3600,
            });
        }
    }
}
```

## Authorization Code with PKCE

```javascript
// PKCE for public clients (SPAs, mobile apps)
import { createHash, randomBytes } from 'node:crypto';

function generatePKCE() {
    const verifier = randomBytes(32).toString('base64url');
    const challenge = createHash('sha256').update(verifier).digest('base64url');
    return { verifier, challenge };
}

// Client-side: generate PKCE and start flow
const { verifier, challenge } = generatePKCE();
sessionStorage.setItem('pkce_verifier', verifier);

// Redirect to auth server
window.location = `https://auth.example.com/authorize?` +
    `response_type=code&` +
    `client_id=${clientId}&` +
    `redirect_uri=${redirectUri}&` +
    `code_challenge=${challenge}&` +
    `code_challenge_method=S256&` +
    `scope=read&` +
    `state=${state}`;

// After redirect, exchange code with verifier
async function exchangeCode(code) {
    const verifier = sessionStorage.getItem('pkce_verifier');
    const response = await fetch('https://auth.example.com/token', {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: new URLSearchParams({
            grant_type: 'authorization_code',
            code,
            client_id: clientId,
            redirect_uri: redirectUri,
            code_verifier: verifier,
        }),
    });
    return response.json();
}

// Server-side: verify PKCE
async function verifyPKCE(code, verifier) {
    const codeData = await redis.get(`auth_code:${code}`);
    const { codeChallenge } = JSON.parse(codeData);

    const challenge = createHash('sha256').update(verifier).digest('base64url');
    return challenge === codeChallenge;
}
```

## Client Credentials Flow

```javascript
// Machine-to-machine authentication
app.post('/token', async (req, res) => {
    const { grant_type, client_id, client_secret } = req.body;

    if (grant_type !== 'client_credentials') {
        return res.status(400).json({ error: 'unsupported_grant_type' });
    }

    const client = clients.get(client_id);
    if (!client || client.secret !== client_secret) {
        return res.status(401).json({ error: 'invalid_client' });
    }

    const token = jwt.sign(
        {
            sub: client_id,
            scope: client.scopes.join(' '),
            client_id,
        },
        process.env.JWT_SECRET,
        { expiresIn: '1h', issuer: 'auth-server' }
    );

    res.json({
        access_token: token,
        token_type: 'Bearer',
        expires_in: 3600,
        scope: client.scopes.join(' '),
    });
});
```

## Token Introspection

```javascript
// RFC 7662 Token Introspection
app.post('/introspect', async (req, res) => {
    const { token } = req.body;

    try {
        const decoded = jwt.verify(token, process.env.JWT_SECRET);

        res.json({
            active: true,
            sub: decoded.sub,
            scope: decoded.scope,
            client_id: decoded.client_id,
            exp: decoded.exp,
            iat: decoded.iat,
            iss: decoded.iss,
        });
    } catch {
        res.json({ active: false });
    }
});
```

## Best Practices Checklist

- [ ] Use PKCE for all public clients (SPAs, mobile)
- [ ] Validate redirect_uri strictly
- [ ] Use short-lived authorization codes (5 minutes)
- [ ] Implement state parameter for CSRF protection
- [ ] Store refresh tokens securely
- [ ] Implement token introspection for resource servers
- [ ] Handle all OAuth error codes properly

## Cross-References

- See [JWT Refresh](../jwt/04-jwt-refresh-tokens.md) for token management
- See [Social Login](./02-social-providers.md) for provider integration
- See [Security](../06-authentication-security/01-security-headers.md) for hardening

## Next Steps

Continue to [Social Provider Integration](./02-social-providers.md).
