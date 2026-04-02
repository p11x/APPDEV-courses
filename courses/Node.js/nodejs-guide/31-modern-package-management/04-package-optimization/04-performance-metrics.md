# Performance Metrics

## What You'll Learn

- How to measure package manager performance
- How to benchmark install times
- How to track bundle size over time
- How to set up performance budgets

## Install Time Benchmarking

```bash
# Benchmark npm vs pnpm vs yarn

# Clean install
time rm -rf node_modules && npm install
time rm -rf node_modules && pnpm install
time rm -rf node_modules && yarn install

# Cached install
time npm install
time pnpm install
time yarn install
```

## Bundle Size Tracking

### size-limit

```bash
npm install -D size-limit @size-limit/file
```

```json
// package.json
{
  "size-limit": [
    {
      "path": "dist/index.js",
      "limit": "10 KB"
    }
  ],
  "scripts": {
    "size": "size-limit"
  }
}
```

```bash
# Check size
npm run size

# CI integration — fails if over limit
npx size-limit
```

### bundlesize

```json
// package.json
{
  "bundlesize": [
    {
      "path": "dist/*.js",
      "maxSize": "50 kB"
    }
  ]
}
```

## Performance Budget

```json
// .size-limit.json
[
  {
    "name": "Main bundle",
    "path": "dist/main.js",
    "limit": "100 KB"
  },
  {
    "name": "Vendor bundle",
    "path": "dist/vendor.js",
    "limit": "200 KB"
  },
  {
    "name": "Total",
    "path": "dist/*.js",
    "limit": "500 KB"
  }
]
```

## CI Integration

```yaml
# .github/workflows/perf.yml
name: Performance
on: [pull_request]

jobs:
  size:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: 20
      - run: npm ci
      - run: npm run build
      - run: npx size-limit
      - name: Report size
        uses: andresz1/size-limit-action@v1
        with:
          github_token: ${{ secrets.GITHUB_TOKEN }}
```

## Summary Table

| Metric | Target | Tool |
|--------|--------|------|
| Clean install | < 30s | Manual timing |
| Cached install | < 5s | Manual timing |
| Bundle size | < 200KB | size-limit |
| Largest package | < 50KB | bundlephobia |
| Tree-shaking | 0 unused exports | source-map-explorer |

## Next Steps

This concludes Chapter 31. Return to the [guide index](../../index.html).
