# Environment Variables on Vercel

## Setting Variables

In Vercel Dashboard:
1. Go to Settings → Environment Variables
2. Add your variables
3. Redeploy to apply

## .env on Vercel

```bash
# .env.local (development)
DATABASE_URL=postgres://...

# .env.production (Vercel)
DATABASE_URL=postgres://...
```

## Using Variables

```typescript
// Access in code
const apiKey = process.env.API_KEY;

// Don't expose to client!
```
