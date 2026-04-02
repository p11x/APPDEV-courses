# OAuth2 Server

## What You'll Learn

- How to build an OAuth2 authorization server
- How to implement authorization code flow
- How to manage client applications
- How to issue and validate tokens

## Authorization Code Flow

```
1. Client redirects user to /authorize
2. User authenticates and grants permission
3. Server redirects back with authorization code
4. Client exchanges code for access token
5. Client uses access token to access protected resources
```

## Implementation

```ts
// oauth-server.ts

import express from 'express';
import crypto from 'node:crypto';

const app = express();
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// In-memory stores (use database in production)
const clients = new Map([
  ['client-1', { id: 'client-1', secret: 'secret-1', redirectUris: ['http://localhost:3000/callback'] }],
]);

const authorizationCodes = new Map();
const accessTokens = new Map();

// Step 1: Authorization endpoint
app.get('/authorize', (req, res) => {
  const { client_id, redirect_uri, state, scope } = req.query;

  // Validate client
  const client = clients.get(client_id as string);
  if (!client || !client.redirectUris.includes(redirect_uri as string)) {
    return res.status(400).json({ error: 'invalid_client' });
  }

  // Show consent screen (simplified)
  res.send(`
    <h1>Authorize Application</h1>
    <p>Allow access to: ${scope}</p>
    <form method="POST" action="/authorize/confirm">
      <input type="hidden" name="client_id" value="${client_id}">
      <input type="hidden" name="redirect_uri" value="${redirect_uri}">
      <input type="hidden" name="state" value="${state}">
      <input type="hidden" name="scope" value="${scope}">
      <button type="submit">Allow</button>
    </form>
  `);
});

// Step 2: Confirm authorization
app.post('/authorize/confirm', (req, res) => {
  const { client_id, redirect_uri, state, scope } = req.body;

  // Generate authorization code
  const code = crypto.randomBytes(32).toString('hex');
  authorizationCodes.set(code, {
    clientId: client_id,
    redirectUri: redirect_uri,
    scope,
    expiresAt: Date.now() + 600_000,  // 10 minutes
    userId: 'user-1',  // From session
  });

  // Redirect back to client with code
  const url = new URL(redirect_uri);
  url.searchParams.set('code', code);
  if (state) url.searchParams.set('state', state);

  res.redirect(url.toString());
});

// Step 3: Token endpoint
app.post('/token', (req, res) => {
  const { grant_type, code, client_id, client_secret, redirect_uri } = req.body;

  if (grant_type !== 'authorization_code') {
    return res.status(400).json({ error: 'unsupported_grant_type' });
  }

  // Validate client
  const client = clients.get(client_id);
  if (!client || client.secret !== client_secret) {
    return res.status(401).json({ error: 'invalid_client' });
  }

  // Validate authorization code
  const authCode = authorizationCodes.get(code);
  if (!authCode || authCode.expiresAt < Date.now()) {
    return res.status(400).json({ error: 'invalid_grant' });
  }

  // Issue tokens
  const accessToken = crypto.randomBytes(32).toString('hex');
  const refreshToken = crypto.randomBytes(32).toString('hex');

  accessTokens.set(accessToken, {
    userId: authCode.userId,
    scope: authCode.scope,
    expiresAt: Date.now() + 3600_000,  // 1 hour
  });

  // Delete used authorization code
  authorizationCodes.delete(code);

  res.json({
    access_token: accessToken,
    token_type: 'Bearer',
    expires_in: 3600,
    refresh_token: refreshToken,
    scope: authCode.scope,
  });
});

app.listen(3000, () => {
  console.log('OAuth2 server on http://localhost:3000');
});
```

## Next Steps

For flows, continue to [OAuth2 Flows](./02-oauth2-flows.md).
