# OAuth2 Flows

## What You'll Learn

- Different OAuth2 grant types
- When to use each flow
- How PKCE improves security
- How to implement refresh tokens

## Grant Types

| Grant Type | Use Case | Security |
|-----------|----------|----------|
| Authorization Code | Web apps (with backend) | High (with PKCE) |
| Authorization Code + PKCE | SPAs, mobile apps | Highest |
| Client Credentials | Machine-to-machine | High |
| Device Code | Smart TVs, CLI tools | Medium |

## Authorization Code with PKCE

```ts
// Generate PKCE challenge
function generatePKCE() {
  const verifier = crypto.randomBytes(32).toString('base64url');
  const challenge = crypto
    .createHash('sha256')
    .update(verifier)
    .digest('base64url');
  return { verifier, challenge };
}

// Redirect to authorization server
const { verifier, challenge } = generatePKCE();
const authUrl = new URL('https://auth.example.com/authorize');
authUrl.searchParams.set('response_type', 'code');
authUrl.searchParams.set('client_id', CLIENT_ID);
authUrl.searchParams.set('redirect_uri', REDIRECT_URI);
authUrl.searchParams.set('code_challenge', challenge);
authUrl.searchParams.set('code_challenge_method', 'S256');
```

## Next Steps

For security, continue to [OAuth2 Security](./03-oauth2-security.md).
