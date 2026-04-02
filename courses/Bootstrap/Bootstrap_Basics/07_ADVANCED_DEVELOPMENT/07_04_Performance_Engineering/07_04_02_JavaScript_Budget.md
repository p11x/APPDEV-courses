---
title: "JavaScript Bundle Budget"
difficulty: 2
category: "Advanced Development"
subcategory: "Performance Engineering"
prerequisites:
  - Webpack Bundle Analyzer
  - size-limit
  - CI/CD Pipeline Configuration
---

## Overview

A JavaScript bundle budget sets maximum size limits for Bootstrap JS and custom component code, enforced in CI to prevent bundle growth. The budget accounts for Bootstrap's bundled JavaScript (Popper.js, all plugins), custom application code, and third-party dependencies. Budgets are expressed in both raw and gzip-compressed sizes with different limits for initial load (critical path) and lazy-loaded chunks.

Bootstrap 5's JavaScript is modular; importing only needed plugins (Modal, Dropdown, Tooltip) instead of the full bundle significantly reduces payload. Combined with tree-shaking via ES modules and dynamic import for heavy components, projects can keep their JS budget under 100KB gzipped.

## Basic Implementation

```js
// size-limit configuration
// .size-limit.json
[
  {
    "name": "JS Bundle (initial load)",
    "path": "dist/js/main.*.js",
    "limit": "80 kB",
    "gzip": true
  },
  {
    "name": "JS Bundle (all chunks)",
    "path": "dist/js/**/*.js",
    "limit": "150 kB",
    "gzip": true
  },
  {
    "name": "CSS Bundle",
    "path": "dist/css/main.*.css",
    "limit": "50 kB",
    "gzip": true
  }
]
```

```json
// package.json scripts
{
  "scripts": {
    "build": "webpack --mode production",
    "size": "size-limit",
    "size:check": "size-limit --json | node scripts/check-budget.js"
  }
}
```

```js
// scripts/check-budget.js
const results = JSON.parse(require('fs').readFileSync('/dev/stdin', 'utf8'));
let failed = false;

results.forEach(r => {
  const status = r.pass ? 'PASS' : 'FAIL';
  console.log(`${status}: ${r.name} - ${(r.size / 1024).toFixed(1)}KB (limit: ${r.limit})`);
  if (!r.pass) failed = true;
});

if (failed) {
  console.error('\nBundle budget exceeded. Optimize or increase budget.');
  process.exit(1);
}
```

```js
// webpack.config.js with budget enforcement
module.exports = {
  performance: {
    hints: 'error',
    maxEntrypointSize: 80 * 1024,
    maxAssetSize: 50 * 1024
  },
  optimization: {
    splitChunks: {
      chunks: 'all',
      maxInitialRequests: 5,
      maxSize: 50 * 1024
    }
  }
};
```

## Advanced Variations

```yaml
# CI budget enforcement
# .github/workflows/budget.yml
name: Bundle Budget
on: [pull_request]
jobs:
  budget:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
      - run: npm ci
      - run: npm run build
      - run: npx size-limit --json > size-report.json
      - name: Check budget
        run: node scripts/check-budget.js < size-report.json
      - name: Comment PR
        uses: marocchino/sticky-pull-request-comment@v2
        with:
          header: size
          message: |
            ## Bundle Size Report
            | File | Size | Budget | Status |
            |------|------|--------|--------|
            $(node scripts/format-size-table.js)
```

## Best Practices

1. **Set realistic budgets** - Start generous, tighten gradually as you optimize.
2. **Budget both gzip and raw** - Gzip for network transfer, raw for parsing time.
3. **Separate initial vs lazy budgets** - Initial load should be tight; lazy chunks can be larger.
4. **Automate in CI** - Budget enforcement must be automated, not manual.
5. **Report on PRs** - Show bundle size change compared to main branch.
6. **Use webpack performance hints** - Webpack warns/errors when assets exceed limits.
7. **Analyze before optimizing** - Use bundle analyzer to find the biggest contributors.
8. **Split vendor code** - Separate Bootstrap from app code for better caching.
9. **Import selectively** - `import { Modal } from 'bootstrap'` not `import 'bootstrap'`.
10. **Track trends over time** - Plot bundle size over releases to catch gradual growth.

## Common Pitfalls

1. **Budget too tight** - Unrealistic budgets cause constant CI failures and developer frustration.
2. **Only measuring raw size** - Raw size doesn't reflect actual transfer size after compression.
3. **Not accounting for Popper.js** - Bootstrap's Popper dependency adds ~20KB to bundle size.
4. **Ignoring lazy chunks** - Only budgeting initial load misses oversized lazy-loaded components.
5. **No trend tracking** - Budget passes but size creeps up slowly over many PRs.

## Accessibility Considerations

Bundle budget optimization should not remove accessibility-related JavaScript like focus management, keyboard navigation, or ARIA live region updates.

## Responsive Behavior

Consider conditional loading: heavy desktop-only components should be lazy-loaded only on larger viewports.

```js
if (window.matchMedia('(min-width: 768px)').matches) {
  import('./components/DataTable');
}
```
