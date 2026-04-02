---
title: "Performance Budgets for Bootstrap Projects"
description: "Setting CSS/JS size budgets, CI enforcement, and alerting on performance regressions"
difficulty: 2
tags: ["performance", "budgets", "ci", "bundling", "bootstrap"]
prerequisites: ["04_07_03_CSS_Coverage_Analysis", "04_07_06_Network_Waterfall_Analysis"]
---

## Overview

A performance budget sets hard limits on metrics like total page weight, JavaScript bundle size, CSS size, and load time. For Bootstrap projects, budgets prevent the framework's generous component library from inflating your bundle over time. Without budgets, adding a carousel here and a modal there silently bloats the payload until performance degrades.

Budgets enforced in CI/CD pipelines catch regressions before they reach production. When a Bootstrap component pushes the bundle over the limit, the build fails and the team must optimize or consciously raise the budget.

## Basic Implementation

```json
// performance-budget.json
{
  "budgets": [
    {
      "resourceType": "stylesheet",
      "budget": 80000
    },
    {
      "resourceType": "script",
      "budget": 150000
    },
    {
      "resourceType": "image",
      "budget": 300000
    },
    {
      "resourceType": "font",
      "budget": 100000
    },
    {
      "resourceType": "total",
      "budget": 600000
    }
  ],
  "timings": [
    {
      "metric": "first-contentful-paint",
      "budget": 2000
    },
    {
      "metric": "largest-contentful-paint",
      "budget": 2500
    }
  ]
}
```

```js
// Webpack bundle size budget enforcement
// webpack.config.js
module.exports = {
  performance: {
    hints: 'error',
    maxAssetSize: 250000,        // 250KB per asset
    maxEntrypointSize: 400000,   // 400KB total entrypoint
    assetFilter: (assetFilename) => {
      // Only check CSS and JS against budget
      return /\.(js|css)$/.test(assetFilename);
    }
  }
};
```

## Advanced Variations

```js
// GitHub Actions CI budget enforcement
// .github/workflows/performance-budget.yml
// name: Performance Budget
// on: [pull_request]
// jobs:
//   budget-check:
//     runs-on: ubuntu-latest
//     steps:
//       - uses: actions/checkout@v4
//       - run: npm ci
//       - run: npm run build
//       - name: Check bundle size
//         run: |
//           BOOTSTRAP_CSS=$(stat -f%z dist/css/bootstrap.min.css 2>/dev/null || stat -c%s dist/css/bootstrap.min.css)
//           CUSTOM_CSS=$(stat -f%z dist/css/app.min.css 2>/dev/null || stat -c%s dist/css/app.min.css)
//           TOTAL_CSS=$((BOOTSTRAP_CSS + CUSTOM_CSS))
//
//           echo "Bootstrap CSS: ${BOOTSTRAP_CSS} bytes"
//           echo "Custom CSS: ${CUSTOM_CSS} bytes"
//           echo "Total CSS: ${TOTAL_CSS} bytes"
//
//           if [ $TOTAL_CSS -gt 80000 ]; then
//             echo "::error::CSS budget exceeded: ${TOTAL_CSS} > 80000 bytes"
//             exit 1
//           fi

// bundlewatch integration for automated budget checks
const bundlewatch = require('bundlewatch');

bundlewatch({
  files: [
    {
      path: './dist/css/bootstrap.min.css',
      maxSize: '60kb',
      compression: 'gzip'
    },
    {
      path: './dist/js/bootstrap.bundle.min.js',
      maxSize: '30kb',
      compression: 'gzip'
    },
    {
      path: './dist/js/app.min.js',
      maxSize: '50kb',
      compression: 'gzip'
    }
  ],
  ci: {
    trackBranches: ['main'],
    repoBranchBase: 'main'
  }
});
```

```js
// Custom budget checker script
// scripts/check-budget.js
const fs = require('fs');
const zlib = require('zlib');
const path = require('path');

const BUDGETS = {
  'css/bootstrap.min.css': 60 * 1024,
  'js/bootstrap.bundle.min.js': 30 * 1024,
  'css/app.css': 20 * 1024,
  'js/app.js': 50 * 1024
};

let passed = true;

for (const [filePath, maxBytes] of Object.entries(BUDGETS)) {
  const fullPath = path.join(__dirname, '..', 'dist', filePath);

  if (!fs.existsSync(fullPath)) {
    console.log(`SKIP: ${filePath} (not found)`);
    continue;
  }

  const content = fs.readFileSync(fullPath);
  const gzipped = zlib.gzipSync(content);
  const sizeKB = (gzipped.length / 1024).toFixed(1);
  const limitKB = (maxBytes / 1024).toFixed(0);

  if (gzipped.length > maxBytes) {
    console.error(`FAIL: ${filePath} — ${sizeKB}KB gzipped exceeds ${limitKB}KB budget`);
    passed = false;
  } else {
    console.log(`PASS: ${filePath} — ${sizeKB}KB gzipped (budget: ${limitKB}KB)`);
  }
}

if (!passed) {
  console.error('\nPerformance budget exceeded. Optimize assets before merging.');
  process.exit(1);
}

console.log('\nAll budgets passed.');
```

## Best Practices

1. Set separate budgets for CSS, JS, images, and fonts — not just a total budget
2. Measure gzipped size, not raw size, since servers compress responses
3. Base Bootstrap CSS budget on PurgeCSS output, not the full 200KB library
4. Include performance budgets in PR checks so regressions block merges
5. Review and adjust budgets quarterly as the application grows
6. Use tooling like `bundlewatch`, `size-limit`, or `bundlesize` for automation
7. Budget for both initial load and total page weight including lazy-loaded assets
8. Track budget trends over time to catch gradual bloat
9. Set stricter budgets for mobile-specific pages and landing pages
10. Document why each budget exists and what triggers a budget increase

## Common Pitfalls

1. **Setting budgets too loose** — A 500KB CSS budget on a Bootstrap page provides no protection; match budgets to actual usage
2. **Measuring uncompressed size** — Gzipped Bootstrap CSS is ~30KB, not 200KB; budgets should reflect transfer size
3. **No budget for third-party scripts** — Analytics, ads, and chat widgets often exceed Bootstrap's own size; budget them too
4. **Budgets without CI enforcement** — Manual budget checks get forgotten; automate in the build pipeline
5. **Ignoring budget for lazy-loaded content** — Images and components loaded after initial render still consume bandwidth
6. **One budget for all pages** — A dashboard with many components needs a higher budget than a landing page; use per-route budgets

## Accessibility Considerations

Performance budgets indirectly protect accessibility. Bloated pages load slowly on assistive devices with limited processing power. Budgets that enforce smaller bundles improve the experience for users on older hardware, low-bandwidth connections, and data-limited mobile plans.

## Responsive Behavior

Consider viewport-specific budgets. Mobile pages should have stricter budgets than desktop because mobile users face slower networks and higher latency. Track Bootstrap's responsive CSS separately — the grid utilities, visibility classes, and breakpoint modifiers add measurable weight that scales differently across devices.
