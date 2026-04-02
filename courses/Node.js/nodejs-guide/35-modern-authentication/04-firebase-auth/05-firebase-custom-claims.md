# Firebase Custom Claims

## What You'll Learn

- How to add custom claims to Firebase tokens
- How to use claims for authorization
- How to check claims on client and server
- How claims propagate

## Setting Custom Claims

```ts
// admin/set-claims.ts

import { adminAuth } from '../lib/firebase-admin.js';

// Set admin claim
await adminAuth.setCustomUserClaims(userId, {
  admin: true,
  role: 'admin',
  permissions: ['read', 'write', 'delete'],
});

// Set user tier
await adminAuth.setCustomUserClaims(userId, {
  tier: 'premium',
  maxApiCalls: 10000,
});
```

## Checking Claims

```ts
// Server-side
const decodedToken = await adminAuth.verifyIdToken(token);
if (decodedToken.admin) {
  // User is admin
}

// Client-side
const idTokenResult = await user.getIdTokenResult();
if (idTokenResult.claims.admin) {
  // User is admin
}
```

## Security Rules with Claims

```json
{
  "rules": {
    "admin-data": {
      ".read": "auth.token.admin === true",
      ".write": "auth.token.admin === true"
    }
  }
}
```

## Next Steps

For OAuth2, continue to [OAuth2 Server](../05-oauth2-advanced/01-oauth2-server.md).
