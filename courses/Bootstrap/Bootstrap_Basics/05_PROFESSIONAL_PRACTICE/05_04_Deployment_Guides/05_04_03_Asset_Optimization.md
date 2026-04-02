---
title: "Asset Optimization for Bootstrap 5"
section: "05_04_Deployment_Guides"
file: "05_04_03_Asset_Optimization.md"
difficulty: 2
tags: ["optimization", "minification", "compression", "tree-shaking", "fonts"]
duration: "12 minutes"
prerequisites:
  - "Production build pipeline configured"
  - "Understanding of CSS/JS bundling"
learning_objectives:
  - "Implement CSS minification and tree shaking"
  - "Configure Gzip/Brotli compression"
  - "Optimize font loading and image pipelines"
---

# Asset Optimization for Bootstrap 5

## Overview

Asset optimization reduces the total bytes transferred to the browser, directly improving load times, Core Web Vitals scores, and user experience. For Bootstrap projects, the primary optimization targets are CSS (removing unused rules), JavaScript (tree-shaking unused components), images (compression and modern formats), fonts (subsetting), and server compression (Gzip/Brotli).

A typical unoptimized Bootstrap site ships ~250KB of CSS+JS. With proper optimization, this drops to 40-80KB. Combined with Brotli compression, transfer sizes can reach 15-25KB for CSS and 10-20KB for JS.

---

## Basic Implementation

### CSS Minification with PostCSS

```js
// postcss.config.js
module.exports = {
  plugins: [
    require('cssnano')({
      preset: ['default', {
        discardComments: { removeAll: true },
        normalizeWhitespace: true,
      }],
    }),
  ],
};
```

### PurgeCSS for Unused Bootstrap Classes

```js
// purgecss.config.js
const { PurgeCSS } = require('purgecss');

const purgeCSSResults = await new PurgeCSS().content({
  content: ['./src/**/*.html', './src/**/*.js', './src/**/*.php'],
  css: ['./dist/assets/css/*.css'],
  safelist: {
    standard: [/^modal/, /^tooltip/, /^popover/, /^collapse/, /^show/, /^collapsing/],
    greedy: [/^bs-/],
  },
});
```

### Gzip via Nginx

```nginx
# nginx.conf
gzip on;
gzip_types text/css application/javascript text/html;
gzip_min_length 256;
gzip_vary on;
gzip_comp_level 6;
```

---

## Advanced Variations

### Brotli Pre-compression (Build Time)

```js
// vite.config.js (with plugin)
import compression from 'vite-plugin-compression2';

export default {
  plugins: [
    compression({ algorithm: 'brotliCompress', exclude: [/\.(br)$/] }),
    compression({ algorithm: 'gzip', exclude: [/\.(gz)$/] }),
  ],
};
```

### Font Subsetting

```bash
# Subset Bootstrap Icons to only used glyphs
pyftsubset bootstrap-icons.woff2 \
  --text="⚙★❤✓🔍" \
  --output-file=bootstrap-icons-subset.woff2 \
  --flavor=woff2
```

### Image Optimization Pipeline

```js
// scripts/optimize-images.js
const sharp = require('sharp');
const glob = require('glob');

glob('src/images/**/*.{jpg,png}', (err, files) => {
  files.forEach(file => {
    sharp(file)
      .resize(1200, null, { withoutEnlargement: true })
      .webp({ quality: 80 })
      .toFile(file.replace(/\.(jpg|png)$/, '.webp'));
  });
});
```

---

## Best Practices

1. **Enable Brotli over Gzip** — 15-25% better compression ratio; all modern browsers support it
2. **Use PurgeCSS with a comprehensive safelist** — Bootstrap's dynamic classes (modals, tooltips) are added via JS, not present in HTML
3. **Subset fonts to used characters only** — Bootstrap Icons has 2000+ glyphs; most sites use fewer than 50
4. **Convert images to WebP/AVIF** with `<picture>` fallbacks for older browsers
5. **Set `font-display: swap`** to prevent FOIT (Flash of Invisible Text)
6. **Preload critical fonts** with `<link rel="preload" as="font" crossorigin>`
7. **Lazy-load images below the fold** with `loading="lazy"` attribute
8. **Use `contenthash` in asset filenames** — enables aggressive CDN caching with `max-age=31536000`
9. **Measure with Lighthouse CI** in your pipeline to catch regressions automatically
10. **Split CSS into critical and non-critical** — inline above-the-fold CSS, defer the rest
11. **Remove unused Bootstrap JavaScript modules** — import only Modal, Dropdown, etc. individually
12. **Compress SVGs** with SVGO before including them in the build

---

## Common Pitfalls

1. **Purging dynamic Bootstrap classes** — Modals, tooltips, and popovers add classes via JavaScript. Without a safelist, these components break silently in production
2. **Over-compressing images** — quality below 60 for JPEG/WebP creates visible artifacts on retina displays
3. **Not pre-compressing with Brotli** — on-the-fly Brotli is CPU-intensive; pre-compressed `.br` files served directly are far more efficient
4. **Ignoring font `unicode-range`** — loading the full font file when only Latin characters are used wastes 50-200KB
5. **Forgetting `Vary: Accept-Encoding` header** — proxies may serve Gzip to Brotli-capable browsers
6. **Purging CSS without testing all pages** — a class used on `/about` but not `/index` gets removed, breaking the about page

---

## Accessibility Considerations

Font subsetting must preserve accessibility-related characters and icons. If Bootstrap Icons are used for interactive elements (close buttons, navigation arrows), ensure the subset includes those glyphs and that `aria-label` attributes provide text alternatives.

Image optimization should not strip alt text. AVIF/WebP conversion tools preserve metadata, but verify that `alt` attributes remain in the HTML output. Lazy-loaded images should use explicit `width` and `height` attributes to prevent layout shifts (CLS).

---

## Responsive Behavior

Optimization tools must respect Bootstrap's responsive utility classes. PurgeCSS configurations should include patterns like `d-md-none`, `col-lg-6`, and `flex-sm-row` in their safelist. Without this, responsive layouts break in production while working perfectly in development builds where all CSS is present.

Image optimization pipelines should generate responsive image sets (`srcset`) at Bootstrap's breakpoint widths: 576px, 768px, 992px, 1200px, and 1400px.
