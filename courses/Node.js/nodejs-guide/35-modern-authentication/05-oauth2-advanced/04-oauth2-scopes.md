# OAuth2 Scopes

## What You'll Learn

- How OAuth2 scopes work
- How to define and enforce scopes
- How to request specific scopes
- How scope-based access control works

## Defining Scopes

```ts
// Scopes define what the client can access
const SCOPES = {
  'read:users': 'Read user profiles',
  'write:users': 'Update user profiles',
  'read:posts': 'Read posts',
  'write:posts': 'Create and edit posts',
  'admin': 'Full administrative access',
};

// Request scopes
const authUrl = new URL('https://auth.example.com/authorize');
authUrl.searchParams.set('scope', 'read:users write:posts');
```

## Enforcing Scopes

```ts
function requireScope(requiredScope: string) {
  return (req, res, next) => {
    const token = req.headers.authorization?.replace('Bearer ', '');
    const decoded = jwt.verify(token, publicKey);

    const scopes = decoded.scope?.split(' ') || [];
    if (!scopes.includes(requiredScope)) {
      return res.status(403).json({ error: 'insufficient_scope' });
    }

    next();
  };
}

app.get('/api/users', requireScope('read:users'), (req, res) => {
  res.json(users);
});

app.post('/api/posts', requireScope('write:posts'), (req, res) => {
  res.json({ created: true });
});
```

## Next Steps

For best practices, continue to [OAuth2 Best Practices](./05-oauth2-best-practices.md).
