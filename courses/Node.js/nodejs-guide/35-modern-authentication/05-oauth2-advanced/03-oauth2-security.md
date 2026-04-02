# OAuth2 Security

## What You'll Learn

- How to secure OAuth2 implementations
- How to prevent common OAuth attacks
- How to validate tokens properly
- How to handle token revocation

## Security Best Practices

```ts
// Always validate:
// 1. Token signature
// 2. Token expiration
// 3. Issuer (iss claim)
// 4. Audience (aud claim)
// 5. Scope

import jwt from 'jsonwebtoken';

function validateToken(token: string) {
  const decoded = jwt.verify(token, publicKey, {
    algorithms: ['RS256'],
    issuer: 'https://auth.example.com',
    audience: 'https://api.example.com',
  });

  return decoded;
}
```

## Common Attacks

| Attack | Mitigation |
|--------|-----------|
| Token theft | Use short-lived tokens + refresh tokens |
| CSRF | Use PKCE, state parameter |
| Redirect hijacking | Validate redirect_uri strictly |
| Token replay | Use jti claim, check revocation list |

## Next Steps

For scopes, continue to [OAuth2 Scopes](./04-oauth2-scopes.md).
