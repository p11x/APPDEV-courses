# Firebase Security

## What You'll Learn

- How to secure Firebase Auth
- How to configure Security Rules
- How to prevent unauthorized access
- How to handle token verification

## Server-Side Verification

```ts
import { adminAuth } from '../lib/firebase-admin.js';

// Verify ID token
async function verifyToken(token: string) {
  try {
    const decoded = await adminAuth.verifyIdToken(token);
    return decoded;
  } catch (err) {
    throw new Error('Invalid token');
  }
}

// Middleware
async function authMiddleware(req, res, next) {
  const token = req.headers.authorization?.replace('Bearer ', '');

  if (!token) {
    return res.status(401).json({ error: 'No token' });
  }

  try {
    req.user = await verifyToken(token);
    next();
  } catch (err) {
    res.status(401).json({ error: 'Invalid token' });
  }
}
```

## Security Rules

```json
{
  "rules": {
    "users": {
      "$uid": {
        ".read": "auth.uid === $uid",
        ".write": "auth.uid === $uid"
      }
    },
    "posts": {
      ".read": true,
      ".write": "auth.uid !== null"
    }
  }
}
```

## Next Steps

For custom claims, continue to [Firebase Custom Claims](./05-firebase-custom-claims.md).
