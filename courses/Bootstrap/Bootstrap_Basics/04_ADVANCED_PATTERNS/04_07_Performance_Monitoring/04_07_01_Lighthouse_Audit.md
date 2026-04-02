---
title: "Lighthouse Audit for Bootstrap Sites"
description: "Running Lighthouse on Bootstrap sites, interpreting scores, and fixing common performance issues"
difficulty: 2
tags: ["performance", "lighthouse", "audit", "bootstrap"]
prerequisites: ["01_01_Basic_Template"]
---

## Overview

Lighthouse is Google's automated tool for auditing web page quality. When working with Bootstrap sites, Lighthouse helps identify performance bottlenecks caused by the framework's CSS and JavaScript overhead. This guide covers running audits, interpreting results, and applying Bootstrap-specific fixes to improve your scores.

Bootstrap's comprehensive CSS (~200KB minified) and JavaScript plugins can impact metrics like First Contentful Paint and Total Blocking Time. Understanding how to audit and optimize these assets is critical for production-ready Bootstrap applications.

## Basic Implementation

Run Lighthouse directly from Chrome DevTools or via the command line to get a baseline performance profile of your Bootstrap site.

```html
<!-- Optimized Bootstrap loading for better Lighthouse scores -->
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Lighthouse-Optimized Bootstrap Site</title>

  <!-- Preload critical Bootstrap CSS -->
  <link rel="preload" href="css/bootstrap.min.css" as="style">

  <!-- Use CDN with integrity for caching benefits -->
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"
        rel="stylesheet"
        integrity="sha384-9ndCyUaIbzAi2FUVXJi0CjmCapSmO7icsOvTYhMl5ER2SFIlzMdNnDnG"
        crossorigin="anonymous">

  <!-- Inline critical CSS for above-the-fold content -->
  <style>
    .hero-section { min-height: 60vh; }
    .navbar { position: sticky; top: 0; }
  </style>
</head>
<body>
  <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
    <div class="container">
      <a class="navbar-brand" href="#">Brand</a>
    </div>
  </nav>

  <main class="container py-5">
    <div class="hero-section">
      <h1 class="display-4">Welcome</h1>
      <p class="lead">Optimized Bootstrap site content.</p>
    </div>
  </main>

  <!-- Defer Bootstrap JS -->
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"
          integrity="sha384-geWF76RCwLtnZ8qwWowPQNguL3RmwHVBC9FhGdlKrxdiJJigb/j/68SIy3Te4Bkz"
          crossorigin="anonymous" defer></script>
</body>
</html>
```

```js
// Running Lighthouse programmatically
const lighthouse = require('lighthouse');
const chromeLauncher = require('chrome-launcher');

async function runAudit(url) {
  const chrome = await chromeLauncher.launch({ chromeFlags: ['--headless'] });
  const results = await lighthouse(url, {
    port: chrome.port,
    onlyCategories: ['performance', 'accessibility'],
    throttling: {
      cpuSlowdownMultiplier: 4,
      downloadThroughputKbps: 1638.4,
      uploadThroughputKbps: 675,
      rttMs: 150
    }
  });

  console.log('Performance Score:', results.lhr.categories.performance.score * 100);
  console.log('FCP:', results.lhr.audits['first-contentful-paint'].displayValue);
  console.log('LCP:', results.lhr.audits['largest-contentful-paint'].displayValue);

  await chrome.kill();
  return results;
}

runAudit('http://localhost:3000');
```

## Advanced Variations

```html
<!-- Conditional Bootstrap loading based on page requirements -->
<head>
  <!-- Only load Bootstrap on pages that need it -->
  <script>
    if (document.querySelector('[data-bs-toggle]') || document.querySelector('.modal, .dropdown, .carousel')) {
      const link = document.createElement('link');
      link.rel = 'stylesheet';
      link.href = 'https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css';
      document.head.appendChild(link);
    }
  </script>
</head>
```

```js
// CI integration with Lighthouse CI
// lighthouserc.js
module.exports = {
  ci: {
    collect: {
      url: ['http://localhost:3000/', 'http://localhost:3000/dashboard'],
      numberOfRuns: 3,
      settings: {
        preset: 'desktop'
      }
    },
    assert: {
      assertions: {
        'categories:performance': ['error', { minScore: 0.85 }],
        'categories:accessibility': ['error', { minScore: 0.95 }],
        'render-blocking-resources': ['warn', { maxLength: 3 }],
        'unused-css-rules': ['warn', { maxLength: 10 }]
      }
    },
    upload: {
      target: 'temporary-public-storage'
    }
  }
};
```

## Best Practices

1. Run Lighthouse in incognito mode to avoid extension interference
2. Test with simulated throttling (Slow 4G) for realistic mobile scores
3. Always run 3+ audits and average results for consistency
4. Use `loading="lazy"` on images below the fold in Bootstrap carousels
5. Preload critical Bootstrap font files if using Bootstrap Icons
6. Enable gzip/brotli compression for Bootstrap CSS and JS files
7. Set long cache headers for versioned Bootstrap assets on CDN
8. Eliminate render-blocking CSS by inlining critical styles
9. Move Bootstrap JavaScript to end of body with `defer` attribute
10. Use the `bootstrap.bundle.min.js` to avoid separate Popper.js requests
11. Audit after each Bootstrap component addition to track score changes
12. Configure Lighthouse CI to fail builds below a performance threshold

## Common Pitfalls

1. **Loading full Bootstrap when only using a few components** — Pull in only the grid and utilities you need via Sass imports
2. **Not deferring Bootstrap JS** — Placing `bootstrap.bundle.js` in `<head>` without `defer` blocks rendering and tanks FCP
3. **Ignoring unused CSS warnings** — Bootstrap ships ~80% unused CSS on typical pages; use PurgeCSS to strip it
4. **Running audits with browser extensions active** — Extensions inject scripts that skew Lighthouse scores; always use incognito
5. **Testing only on desktop** — Mobile scores are typically 20-30 points lower; always validate mobile performance
6. **Caching Lighthouse results** — Each run should use a fresh page load; stale caches mask real issues
7. **Focusing only on the performance score** — Accessibility and SEO audits catch Bootstrap misuse like missing `aria-` labels

## Accessibility Considerations

Bootstrap's semantic HTML components generally score well on accessibility audits, but Lighthouse flags issues like missing `alt` attributes on carousel images, insufficient color contrast on badge variants, and missing `aria-label` on icon-only buttons. Always pair Lighthouse accessibility audits with manual keyboard navigation testing, since automated tools cannot detect all focus management issues in Bootstrap modals and dropdowns.

## Responsive Behavior

Lighthouse simulates a Moto G Power viewport (412x732) for mobile audits. Bootstrap's responsive grid and breakpoint classes (`col-sm-6`, `col-lg-4`) should adapt without horizontal overflow. Verify that your Bootstrap layout does not trigger a layout shift penalty by ensuring image dimensions and ad slots have explicit `width` and `height` attributes or CSS aspect ratios.
