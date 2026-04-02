---
title: "CSS Performance Analysis"
difficulty: 2
category: "Advanced Development"
subcategory: "Performance Engineering"
prerequisites:
  - Chrome DevTools Coverage Tab
  - PurgeCSS
  - CSS Specificity Understanding
---

## Overview

CSS performance analysis identifies unused styles, excessive specificity, and redundant rules in Bootstrap-based projects. The analysis uses Chrome DevTools' Coverage tab to find CSS that's loaded but never applied, PurgeCSS to remove unused rules from production builds, and specificity analyzers to detect overly complex selectors that slow down the browser's style recalculation.

Bootstrap ships with extensive CSS covering all possible component combinations. Most projects use a fraction of this CSS, making unused style removal the highest-impact optimization. A typical Bootstrap project can reduce CSS payload by 40-70% through selective importing and PurgeCSS.

## Basic Implementation

```js
// PurgeCSS configuration for Bootstrap
// postcss.config.js
const purgeCSS = require('@fullhuman/postcss-purgecss');

module.exports = {
  plugins: [
    require('autoprefixer'),
    process.env.NODE_ENV === 'production' && purgeCSS({
      content: [
        './src/**/*.html',
        './src/**/*.js',
        './src/**/*.jsx',
        './src/**/*.vue'
      ],
      safelist: {
        standard: [
          /^show/, /^modal/, /^collapse/, /^collapsing/,
          /^fade/, /^tooltip/, /^popover/, /^bs-tooltip/,
          /^offcanvas/
        ],
        deep: [/^carousel/, /^accordion/],
        greedy: [/data-bs-/]
      },
      defaultExtractor: (content) => content.match(/[\w-/:]+(?<!:)/g) || []
    })
  ].filter(Boolean)
};
```

```bash
# Chrome DevTools Coverage Analysis
# 1. Open DevTools → Sources → Coverage tab
# 2. Click "Reload" to analyze page load coverage
# 3. Filter by CSS to see unused bytes per stylesheet
# 4. Click on a file to see line-by-line coverage

# CLI-based analysis
npx coverage-report --type css --url http://localhost:3000
```

```js
// Custom CSS coverage analyzer
// scripts/css-coverage.js
const puppeteer = require('puppeteer');
const fs = require('fs');

async function analyzeCSSCoverage(url) {
  const browser = await puppeteer.launch();
  const page = await browser.newPage();

  await page.coverage.startCSSCoverage();
  await page.goto(url, { waitUntil: 'networkidle0' });
  const coverage = await page.coverage.stopCSSCoverage();

  let totalBytes = 0;
  let usedBytes = 0;
  const results = [];

  for (const entry of coverage) {
    const total = entry.text.length;
    const used = entry.ranges.reduce((sum, range) => sum + (range.end - range.start), 0);

    totalBytes += total;
    usedBytes += used;

    results.push({
      url: entry.url,
      totalBytes: total,
      usedBytes: used,
      unusedBytes: total - used,
      coveragePercent: ((used / total) * 100).toFixed(1)
    });
  }

  console.log(`Total CSS: ${(totalBytes / 1024).toFixed(1)} KB`);
  console.log(`Used CSS: ${(usedBytes / 1024).toFixed(1)} KB`);
  console.log(`Unused CSS: ${((totalBytes - usedBytes) / 1024).toFixed(1)} KB`);
  console.log(`Overall Coverage: ${((usedBytes / totalBytes) * 100).toFixed(1)}%`);

  results.sort((a, b) => a.coveragePercent - b.coveragePercent);
  results.forEach(r => {
    if (parseFloat(r.coveragePercent) < 50) {
      console.warn(`  ${r.url}: ${r.coveragePercent}% used (${(r.unusedBytes / 1024).toFixed(1)} KB wasted)`);
    }
  });

  await browser.close();
  return results;
}

analyzeCSSCoverage('http://localhost:3000');
```

## Advanced Variations

```js
// Specificity analyzer
// scripts/specificity-analyzer.js
const postcss = require('postcss');
const specificity = require('specificity');
const fs = require('fs');

function analyzeSpecificity(cssFile) {
  const css = fs.readFileSync(cssFile, 'utf8');
  const root = postcss.parse(css);
  const rules = [];

  root.walkRules(rule => {
    const selectors = rule.selectors;
    selectors.forEach(sel => {
      const spec = specificity.calculate(sel);
      const score = spec[0].specificity.split(',').map(Number);
      const total = score[0] * 100 + score[1] * 10 + score[2];

      if (total > 100) {
        rules.push({
          selector: sel,
          specificity: spec[0].specificity,
          score: total,
          file: cssFile,
          line: rule.source?.start?.line
        });
      }
    });
  });

  rules.sort((a, b) => b.score - a.score);
  return rules.slice(0, 20);
}
```

## Best Practices

1. **Run coverage on real pages** - Analyze actual user-facing pages, not component demos.
2. **Use PurgeCSS in production** - Remove unused CSS as part of the build pipeline.
3. **Maintain safelist carefully** - Bootstrap's JS-generated classes must be safelisted.
4. **Selective Bootstrap imports** - Import only the SCSS partials your project uses.
5. **Monitor CSS size in CI** - Set a maximum CSS bundle size budget.
6. **Audit specificity regularly** - High-specificity selectors slow style recalculation.
7. **Avoid deep nesting** - Prefer flat selectors over deeply nested SCSS.
8. **Use BEM for custom CSS** - Low, consistent specificity prevents cascade issues.
9. **Remove dead CSS** - Delete styles for removed features immediately.
10. **Measure with real devices** - Desktop coverage differs from mobile coverage.

## Common Pitfalls

1. **Purging Bootstrap's JS classes** - Modal, tooltip, and collapse classes are added by JavaScript; PurgeCSS removes them without safelisting.
2. **Ignoring CSS coverage in CI** - Unused CSS silently grows without budget enforcement.
3. **Over-safelisting** - Safelisting too many patterns defeats the purpose of purging.
4. **Not testing after purge** - Purged CSS may break dynamic interactions.
5. **Static analysis only** - Coverage at page load misses styles used after user interactions.

## Accessibility Considerations

Never remove focus styles, screen-reader-only classes, or reduced-motion media queries during CSS optimization. These are critical for accessibility.

## Responsive Behavior

CSS coverage varies between mobile and desktop views. Run coverage analysis at multiple viewport sizes to capture responsive styles.

```js
const viewports = [
  { width: 375, height: 667 },
  { width: 1280, height: 800 }
];

for (const vp of viewports) {
  await page.setViewport(vp);
  const coverage = await analyzeCSSCoverage(url);
  console.log(`${vp.width}px coverage: ${coverage.totalPercent}%`);
}
```
