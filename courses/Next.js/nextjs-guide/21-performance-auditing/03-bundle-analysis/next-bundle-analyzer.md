# Next.js Bundle Analyzer

## What You'll Learn
- Analyze your bundle size
- Find large dependencies
- Optimize bundle

## Prerequisites
- Next.js project

## Do I Need This Right Now?
Bundle analyzer shows exactly what's in your JavaScript bundle. Essential for finding what's making your app slow.

## Setting Up

```bash
# Install package
npm install @next/bundle-analyzer
```

```typescript
// next.config.js
const withBundleAnalyzer = require('@next/bundle-analyzer')({
  enabled: process.env.ANALYZE === 'true',
});

module.exports = withBundleAnalyzer({
  // Your config
});
```

## Running Analysis

```bash
# Run with analyze
ANALYZE=true npm run build
```

This opens a web page showing your bundle visually.

## Summary
- Install @next/bundle-analyzer
- Run ANALYZE=true npm run build
- Visualize what's in your bundle

## Next Steps
- [identifying-large-packages.md](./identifying-large-packages.md) — Finding large packages
