# Build Verification

## What You'll Learn
- Verify build works
- Caching
- Artifact storage

## Prerequisites
- CI workflow set up

## Do I Need This Right Now?
Build verification ensures your app compiles correctly.

## Build Step

```yaml
build:
  runs-on: ubuntu-latest
  steps:
    - uses: actions/checkout@v4
    - uses: actions/setup-node@v4
      with:
        node-version: '20'
        cache: 'npm'
    - run: npm ci
    - run: npm run build
```

## Summary
- Run build in CI
- Use caching for speed
- Fail if build fails

## Next Steps
- [deploy-to-vercel-from-github.md](../03-deployment-pipeline/deploy-to-vercel-from-github.md) — Deploy to Vercel
