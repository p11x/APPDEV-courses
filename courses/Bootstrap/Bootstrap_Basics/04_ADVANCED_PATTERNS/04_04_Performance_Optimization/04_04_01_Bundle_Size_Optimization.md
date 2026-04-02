---
title: "Bundle Size Optimization"
module: "Performance Optimization"
difficulty: 2
duration: "30 minutes"
prerequisites: ["Bootstrap SCSS", "npm basics"]
tags: ["performance", "bundle", "purgecss", "tree-shaking"]
---

# Bundle Size Optimization

## Overview

Bootstrap's full CSS bundle is approximately 230KB minified. For production, most projects use only a fraction of Bootstrap's components and utilities. Optimizing bundle size through selective imports, PurgeCSS integration, and tree-shaking can reduce CSS payloads by 70-90%, significantly improving page load performance.

## Basic Implementation

Analyze your current Bootstrap bundle size:

```bash
# Install source-map-explorer
npm install --save-dev source-map-explorer

# Add analysis script
# "scripts": {
#   "analyze": "source-map-explorer dist/css/*.css"
# }

npm run analyze
```

Import only needed Bootstrap components:

```scss
// Selective Bootstrap imports
@import "bootstrap/scss/functions";
@import "bootstrap/scss/variables";
@import "bootstrap/scss/mixins";

// Only import components you use
@import "bootstrap/scss/root";
@import "bootstrap/scss/reboot";
@import "bootstrap/scss/type";
@import "bootstrap/scss/grid";
@import "bootstrap/scss/buttons";
@import "bootstrap/scss/utilities";
@import "bootstrap/scss/utilities/api";
```

## Advanced Variations

Integrate PurgeCSS to remove unused CSS:

```js
// postcss.config.js
const purgecss = require('@fullhuman/postcss-purgecss');

module.exports = {
  plugins: [
    require('autoprefixer'),
    ...(process.env.NODE_ENV === 'production' ? [
      purgecss({
        content: [
          './src/**/*.html',
          './src/**/*.js',
          './src/**/*.jsx',
          './src/**/*.vue'
        ],
        safelist: {
          standard: [/show/, /active/, /modal/, /collaps/],
          deep: [/tooltip/, /popover/],
          greedy: [/bs-tooltip/, /bs-popover/]
        },
        defaultExtractor: content => content.match(/[\w-/:]+(?<!:)/g) || []
      })
    ] : [])
  ]
};
```

Configure PurgeCSS with Vite:

```js
// vite.config.js
import { defineConfig } from 'vite';
import purgecss from 'vite-plugin-purgecss';

export default defineConfig({
  plugins: [
    purgecss({
      content: ['./index.html', './src/**/*.{js,jsx,vue}'],
      safelist: {
        standard: [/^btn/, /^col/, /^row/, /^modal/, /^collapse/, /show$/]
      }
    })
  ]
});
```

Use source-map-explorer for detailed analysis:

```bash
# Generate source map and analyze
npx sass src/scss/custom.scss dist/css/custom.css --source-map
npx source-map-explorer dist/css/custom.css --html bundle-report.html
```

Create a smart purge configuration for dynamic classes:

```js
// Custom extractor for Bootstrap dynamic classes
const bootstrapExtractor = (content) => {
  const broadMatches = content.match(/[^<>"'`\s]*[^<>"'`\s:]/g) || [];
  const innerMatches = content.match(/[^<>"'`\s.()]*[^<>"'`\s.():]/g) || [];
  return broadMatches.concat(innerMatches);
};

module.exports = {
  content: ['./**/*.html'],
  extractors: [
    {
      extractor: bootstrapExtractor,
      extensions: ['html', 'js', 'jsx', 'vue']
    }
  ],
  safelist: {
    standard: [/^modal/, /^dropdown/, /^tooltip/, /^popover/, /^collapse/, /^carousel/, /show$/, /active$/]
  }
};
```

## Best Practices

1. Measure bundle size before and after optimization
2. Use PurgeCSS with proper safelist for dynamic classes
3. Keep Bootstrap's core functions and variables imports
4. Audit component usage quarterly to remove unused imports
5. Use source-map-explorer to visualize CSS composition
6. Configure PurgeCSS safelist for JavaScript-generated classes
7. Enable CSS minification in production builds
8. Monitor bundle size in CI/CD pipeline
9. Document which Bootstrap components are in use
10. Use PurgeCSS's `blocklist` to remove known unused classes
11. Test thoroughly after PurgeCSS runs to catch removed classes
12. Consider CSS-in-JS alternatives for component-scoped styles

## Common Pitfalls

1. Purging dynamic classes added by JavaScript (modals, tooltips)
2. Not safelisting Bootstrap's state classes (.show, .active, .collapsing)
3. Forgetting to include JavaScript template files in PurgeCSS content
4. Over-purging and breaking component functionality
5. Not rebuilding after changing import configuration
6. Ignoring third-party plugin classes that depend on Bootstrap
7. Missing safelist entries for classes generated from data attributes
8. Not testing all pages/components after optimization

## Accessibility Considerations

- Ensure PurgeCSS doesn't remove focus-related styles
- Verify screen reader utility classes are retained
- Test keyboard navigation after CSS optimization
- Confirm ARIA-related styles remain intact

## Responsive Behavior

- Verify all responsive utilities survive optimization
- Test grid layouts after PurgeCSS runs
- Ensure breakpoint-specific classes are retained
- Validate responsive visibility utilities work correctly
