---
title: "Critical Path Optimization"
difficulty: 3
category: "Advanced Development"
subcategory: "Performance Engineering"
prerequisites:
  - Critical CSS Extraction
  - Render-Blocking Resources
  - Async/Defer Loading
---

## Overview

Critical path optimization identifies and prioritizes the minimum CSS and JavaScript required to render above-the-fold content, then defers everything else. For Bootstrap applications, this means extracting the CSS rules that style the initial viewport (critical CSS) and inlining them in the HTML head, while loading the full Bootstrap CSS asynchronously. JavaScript loads with `defer` or `async` attributes to prevent render blocking.

The critical rendering path is the sequence of resources the browser must process before the first paint. Every render-blocking resource (synchronous CSS and JavaScript in the `<head>`) delays first paint. By inlining only critical CSS and deferring the rest, first paint can occur in under 1 second even on slow connections.

## Basic Implementation

```js
// Critical CSS extraction with critical package
// scripts/extract-critical.js
const critical = require('critical');

async function extractCritical() {
  const { css, html } = await critical.generate({
    src: 'index.html',
    css: ['dist/css/bootstrap.min.css', 'dist/css/custom.css'],
    dimensions: [
      { width: 375, height: 667 },  // mobile
      { width: 768, height: 1024 }, // tablet
      { width: 1280, height: 800 }  // desktop
    ],
    inline: true,
    extract: true,
    penthouse: {
      timeout: 60000
    }
  });

  // Write critical CSS to file
  const fs = require('fs');
  fs.writeFileSync('dist/css/critical.css', css);

  // Generate optimized HTML
  fs.writeFileSync('dist/index.html', html);
}

extractCritical();
```

```html
<!-- Optimized HTML with critical CSS inlined -->
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>My App</title>

  <!-- Critical CSS inlined for instant first paint -->
  <style>
    /* Only rules needed for above-the-fold content */
    :root { --bs-primary: #0d6efd; --bs-body-bg: #fff; }
    *, *::before, *::after { box-sizing: border-box; }
    body { margin: 0; font-family: system-ui, sans-serif; font-size: 1rem; line-height: 1.5; color: #212529; background-color: #fff; }
    .navbar { display: flex; padding: 0.5rem 1rem; background: #f8f9fa; }
    .container { width: 100%; max-width: 1140px; margin: 0 auto; padding: 0 0.75rem; }
    .hero { padding: 3rem 0; text-align: center; }
    .hero h1 { font-size: 2.5rem; font-weight: 700; }
    .btn-primary { background: #0d6efd; color: #fff; padding: 0.375rem 0.75rem; border-radius: 0.375rem; border: none; }
  </style>

  <!-- Full CSS loaded asynchronously -->
  <link rel="preload"
        href="/dist/css/bootstrap.min.css"
        as="style"
        onload="this.onload=null;this.rel='stylesheet'">
  <link rel="preload"
        href="/dist/css/custom.css"
        as="style"
        onload="this.onload=null;this.rel='stylesheet'">
  <noscript>
    <link rel="stylesheet" href="/dist/css/bootstrap.min.css">
    <link rel="stylesheet" href="/dist/css/custom.css">
  </noscript>
</head>
<body>
  <!-- Above-the-fold content renders immediately -->
  <nav class="navbar">
    <div class="container">
      <a class="navbar-brand" href="/">My App</a>
    </div>
  </nav>

  <section class="hero">
    <div class="container">
      <h1>Welcome to My App</h1>
      <p class="lead">Build something amazing.</p>
      <a href="#" class="btn-primary">Get Started</a>
    </div>
  </section>

  <!-- Below-the-fold content loads after critical rendering -->
  <main class="container my-5" id="main-content">
    <!-- Dynamic content loaded here -->
  </main>

  <!-- JavaScript loaded with defer -->
  <script src="/dist/js/bootstrap.bundle.min.js" defer></script>
  <script src="/dist/js/app.min.js" defer></script>
</body>
</html>
```

```js
// Async CSS loader (for non-critical stylesheets)
// Inline this in <head> to load CSS without blocking render
function loadCSS(href, media) {
  const link = document.createElement('link');
  link.rel = 'stylesheet';
  link.href = href;
  link.media = media || 'print';
  link.onload = () => { link.media = 'all'; };
  document.head.appendChild(link);
}

// Preload polyfill for older browsers
(function() {
  const links = document.querySelectorAll('link[rel="preload"][as="style"]');
  links.forEach(link => {
    link.rel = 'stylesheet';
  });
})();
```

## Advanced Variations

```js
// Critical CSS extraction per route
// scripts/critical-per-route.js
const critical = require('critical');
const routes = [
  { path: '/', file: 'index.html' },
  { path: '/dashboard', file: 'dashboard.html' },
  { path: '/settings', file: 'settings.html' }
];

async function extractForRoute(route) {
  const result = await critical.generate({
    src: route.file,
    css: ['dist/css/bootstrap.min.css', 'dist/css/custom.css'],
    dimensions: [{ width: 375, height: 667 }, { width: 1280, height: 800 }],
    inline: false,
    extract: true
  });

  const fs = require('fs');
  const outputName = route.file.replace('.html', '.critical.css');
  fs.writeFileSync(`dist/css/${outputName}`, result.css);
  console.log(`Extracted ${result.css.length} bytes of critical CSS for ${route.path}`);
}

Promise.all(routes.map(extractForRoute))
  .then(() => console.log('All routes processed'));
```

## Best Practices

1. **Extract critical CSS per page** - Different pages have different above-the-fold content.
2. **Inline critical CSS** - Avoids an additional network request for the most important styles.
3. **Load full CSS asynchronously** - Use `preload` + `onload` pattern or `media="print"` trick.
4. **Defer all JavaScript** - Use `defer` for scripts that need DOM; use `async` for independent scripts.
5. **Measure first paint** - Use Lighthouse or WebPageTest to verify optimization impact.
6. **Include font preloading** - Preload web fonts to prevent FOIT (Flash of Invisible Text).
7. **Minimize critical CSS size** - Keep inlined CSS under 14KB for optimal TCP slow start.
8. **Test on real devices** - Desktop simulation doesn't capture mobile rendering constraints.
9. **Automate extraction** - Run critical CSS extraction as part of the build process.
10. **Validate no layout shift** - Ensure async CSS loading doesn't cause CLS.

## Common Pitfalls

1. **Critical CSS too large** - Inlining 100KB of CSS defeats the purpose; extract only above-the-fold rules.
2. **Stale critical CSS** - Not re-extracting after style changes causes layout inconsistencies.
3. **Blocking font loading** - Web fonts without `font-display: swap` block rendering.
4. **Script in head without defer** - Synchronous scripts in `<head>` block HTML parsing.
5. **Missing noscript fallback** - Async CSS loading fails without JavaScript; provide `<noscript>` alternative.

## Accessibility Considerations

Critical CSS must include focus styles and screen-reader-only classes. Don't defer accessibility-critical styles.

```css
/* Include in critical CSS */
.sr-only { position: absolute; width: 1px; height: 1px; overflow: hidden; clip: rect(0,0,0,0); }
:focus-visible { outline: 2px solid #0d6efd; outline-offset: 2px; }
```

## Responsive Behavior

Extract critical CSS at multiple viewport sizes to ensure mobile and desktop both get optimized rendering.

```js
dimensions: [
  { width: 375, height: 667 },   // mobile
  { width: 768, height: 1024 },  // tablet
  { width: 1280, height: 800 }   // desktop
]
```
