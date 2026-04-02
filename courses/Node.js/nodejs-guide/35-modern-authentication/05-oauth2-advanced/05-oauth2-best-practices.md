# OAuth2 Best Practices

## What You'll Learn

- Production OAuth2 checklist
- How to handle token lifecycle
- How to implement token revocation
- How to audit OAuth2 usage

## Production Checklist

- [ ] Use PKCE for all public clients
- [ ] Validate redirect_uri strictly (no wildcards)
- [ ] Use short-lived access tokens (15 min)
- [ ] Implement refresh token rotation
- [ ] Store refresh tokens securely (encrypted)
- [ ] Implement token revocation endpoint
- [ ] Log all token issuance and revocation
- [ ] Use RS256 or ES256 for token signing
- [ ] Validate iss, aud, exp claims
- [ ] Implement rate limiting on token endpoint

## Token Revocation

```ts
app.post('/revoke', async (req, res) => {
  const { token } = req.body;

  // Add token to revocation list
  await redis.set(`revoked:${token}`, '1', 'EX', 86400);

  res.json({ revoked: true });
});

// Check revocation on each request
app.use('/api', async (req, res, next) => {
  const token = req.headers.authorization?.replace('Bearer ', '');
  const revoked = await redis.get(`revoked:${token}`);

  if (revoked) {
    return res.status(401).json({ error: 'Token revoked' });
  }

  next();
});
```

## Next Steps

For MFA, continue to [MFA Setup](../06-mfa-biometric/01-mfa-setup.md).
