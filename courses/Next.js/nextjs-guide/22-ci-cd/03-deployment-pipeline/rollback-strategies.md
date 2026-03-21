# Rollback Strategies

## What You'll Learn
- Rollback when things break
- Vercel rollbacks
- Docker rollbacks

## Prerequisites
- Deployment set up

## Do I Need This Right Now?
When deployment breaks, you need to rollback fast.

## Vercel Rollback

In Vercel dashboard:
1. Go to Deployments
2. Find last working deployment
3. Click "..." → Promote to Production

## GitHub Actions Rollback

```yaml
name: Rollback

on:
  workflow_dispatch:
    inputs:
      deployment-id:
        description: 'Deployment ID to rollback to'
        required: true

jobs:
  rollback:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: amondnet/vercel-action@v25
        with:
          vercel-token: ${{ secrets.VERCEL_TOKEN }}
          vercel-org-id: ${{ secrets.ORG_ID }}
          vercel-project-id: ${{ secrets.PROJECT_ID }}
          vercel-args: '--prod'
          deployment-id: ${{ github.event.inputs.deployment-id }}
```

## Summary
- Vercel: Use dashboard to rollback
- GitHub Actions: Use previous deployment ID
- Always test deployments before promoting
