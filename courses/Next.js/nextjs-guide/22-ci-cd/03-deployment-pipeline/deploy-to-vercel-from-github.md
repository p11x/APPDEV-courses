# Deploy to Vercel from GitHub

## What You'll Learn
- Automatic deployment
- Preview deployments
- Production deployments

## Prerequisites
- Vercel account

## Do I Need This Right Now?
Vercel + GitHub = automatic deployments. Best for Next.js apps.

## Setup

1. Go to Vercel → Add New → Project
2. Import from GitHub
3. Configure settings
4. Deploy!

## GitHub Actions Alternative

```yaml
name: Deploy to Vercel

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: amondnet/vercel-action@v25
        with:
          vercel-token: ${{ secrets.VERCEL_TOKEN }}
          vercel-org-id: ${{ secrets.ORG_ID }}
          vercel-project-id: ${{ secrets.PROJECT_ID }}
          vercel-args: '--prod'
```

## Summary
- Vercel automatically deploys on push
- Preview deployments for PRs
- Manual or automatic production deploys

## Next Steps
- [deploy-to-vps.md](./deploy-to-vps.md) — Deploy to VPS
