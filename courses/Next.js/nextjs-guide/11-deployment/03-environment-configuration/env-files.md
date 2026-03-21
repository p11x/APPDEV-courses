# Environment Files in Next.js

## File Types

| File | Purpose |
|------|---------|
| `.env.local` | Development secrets |
| `.env.development` | Dev environment |
| `.env.production` | Production environment |
| `.env` | Default values |

## Creating Files

```bash
# .env.local
DATABASE_URL=postgres://user:pass@localhost:5432/mydb
API_KEY=your-api-key-here
```

## Accessing Variables

```typescript
const dbUrl = process.env.DATABASE_URL;
```
