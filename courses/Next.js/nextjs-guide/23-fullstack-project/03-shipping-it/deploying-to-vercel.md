# Deploying to Vercel

## What You'll Learn
- Deploy to production
- Environment configuration
- CI/CD pipeline

## Prerequisites
- Tests passing

## Do I Need This Right Now?
Deploy to production for the world to see!

## Deployment

This references **Section 11 (Deployment)** and **Section 22 (CI/CD)**.

## Deploy Steps

1. Push code to GitHub
2. Import project in Vercel
3. Configure environment variables
4. Deploy!

```bash
# Environment variables to set:
DATABASE_URL=postgres://...
NEXTAUTH_SECRET=your-secret
NEXTAUTH_URL=https://your-app.vercel.app
NEXT_PUBLIC_SENTRY_DSN=https://...
```

## CI/CD

```yaml
# .github/workflows/deploy.yml
name: Deploy

on:
  push:
    branches: [main]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
      - run: npm ci
      - run: npm run build
      - run: npm test
```

## Summary
- Deploy to Vercel from GitHub
- Set environment variables
- CI runs tests before deploy

## Next Steps
- [post-launch-checklist.md](./post-launch-checklist.md) — Final checklist
