---
title: "Mobile Performance Optimization with Bootstrap"
topic: "Mobile First PWA"
difficulty: 2
duration: "35 minutes"
prerequisites: ["Bootstrap customization", "CSS optimization", "Performance metrics"]
tags: ["performance", "mobile", "critical-css", "lazy-loading", "bootstrap"]
---

## Overview

Mobile performance optimization with Bootstrap 5 focuses on reducing payload size, eliminating render-blocking resources, and respecting device constraints. Key strategies include: extracting critical CSS for above-the-fold content, lazy-loading Bootstrap components and images, reducing motion for accessibility and battery savings, and optimizing the delivery of Bootstrap's CSS and JavaScript bundles.

Bootstrap 5's modular Sass architecture enables selective CSS inclusion — import only the grid, reboot, and utilities for a ~30KB CSS foundation instead of the full ~230KB stylesheet. JavaScript tree-shaking via ES module imports reduces the JS payload. Combined with resource hints, image optimization, and `prefers-reduced-motion` respect, Bootstrap applications can achieve sub-3-second Largest Contentful Paint on 3G networks.

## Basic Implementation

### Selective Bootstrap CSS

```scss
// css/critical.scss — only essential above-the-fold styles
@import 'bootstrap/scss/functions';
@import 'bootstrap/scss/variables';
@import 'bootstrap/scss/mixins';
@import 'bootstrap/scss/root';
@import 'bootstrap/scss/reboot';
@import 'bootstrap/scss/type';
@import 'bootstrap/scss/containers';
@import 'bootstrap/scss/grid';
@import 'bootstrap/scss/utilities/api';
```

### Lazy Loading Images

```html
<!-- Lazy load below-fold images -->
<img src="/images/hero.webp" class="img-fluid" alt="Hero"
     loading="eager" fetchpriority="high" width="1200" height="600">

<img src="/images/feature-1.webp" class="img-fluid" alt="Feature"
     loading="lazy" decoding="async" width="800" height="400">

<img src="/images/feature-2.webp" class="img-fluid" alt="Feature"
     loading="lazy" decoding="async" width="800" height="400">
```

### Critical CSS Inline + Deferred Full CSS

```html
<head>
  <!-- Inline critical CSS -->
  <style>
    /* Paste critical.css output here (under 14KB) */
    :root{--bs-primary:#6366f1}*,*::before,*::after{box-sizing:border-box}
    body{margin:0;font-family:system-ui,-apple-system,sans-serif;line-height:1.5}
    .container{width:100%;max-width:540px;margin:0 auto;padding:0 0.75rem}
    .row{display:flex;flex-wrap:wrap;margin:0 -0.75rem}
    .col{flex:1;padding:0 0.75rem}
    .navbar{padding:0.5rem 1rem;background:#f8f9fa}
  </style>

  <!-- Defer full Bootstrap CSS -->
  <link rel="preload" href="/css/bootstrap.min.css" as="style"
        onload="this.onload=null;this.rel='stylesheet'">
  <noscript>
    <link rel="stylesheet" href="/css/bootstrap.min.css">
  </noscript>
</head>
```

### Reduced Motion Support

```css
/* Respect user's motion preferences */
@media (prefers-reduced-motion: reduce) {
  *,
  *::before,
  *::after {
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
    transition-duration: 0.01ms !important;
    scroll-behavior: auto !important;
  }

  .carousel-item {
    transition: none !important;
  }

  .modal.fade {
    transition: none !important;
  }

  .collapse {
    transition: none !important;
  }
}
```

## Advanced Variations

### Resource Hints

```html
<head>
  <!-- Preconnect to CDN -->
  <link rel="preconnect" href="https://cdn.jsdelivr.net" crossorigin>
  <link rel="dns-prefetch" href="https://cdn.jsdelivr.net">

  <!-- Preload critical fonts -->
  <link rel="preload" href="/fonts/inter-var.woff2" as="font"
        type="font/woff2" crossorigin>

  <!-- Preload critical CSS -->
  <link rel="preload" href="/css/critical.css" as="style">

  <!-- Prefetch next page -->
  <link rel="prefetch" href="/dashboard" as="document">
</head>
```

### Lazy Bootstrap JavaScript

```js
// main.js — defer non-critical Bootstrap JS
document.addEventListener('DOMContentLoaded', () => {
  // Load Bootstrap JS modules only when needed
  const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        const el = entry.target;

        if (el.hasAttribute('data-bs-toggle')) {
          import('bootstrap/js/dist/' + getModuleName(el)).then(module => {
            new module.default(el);
          });
        }

        observer.unobserve(el);
      }
    });
  }, { rootMargin: '200px' });

  document.querySelectorAll('[data-bs-toggle]').forEach(el => {
    observer.observe(el);
  });
});

function getModuleName(el) {
  const map = {
    'modal': 'modal',
    'dropdown': 'dropdown',
    'collapse': 'collapse',
    'tooltip': 'tooltip',
    'popover': 'popover',
    'toast': 'toast',
    'offcanvas': 'offcanvas',
  };
  return map[el.getAttribute('data-bs-toggle')] || 'modal';
}
```

### Image Optimization Pipeline

```html
<picture>
  <source srcset="/images/hero-400.webp 400w,
                  /images/hero-800.webp 800w,
                  /images/hero-1200.webp 1200w"
          type="image/webp" sizes="(max-width: 576px) 100vw, 800px">
  <source srcset="/images/hero-400.jpg 400w,
                  /images/hero-800.jpg 800w,
                  /images/hero-1200.jpg 1200w"
          type="image/jpeg" sizes="(max-width: 576px) 100vw, 800px">
  <img src="/images/hero-800.jpg" class="img-fluid" alt="Hero"
       loading="eager" fetchpriority="high" width="1200" height="600">
</picture>
```

### Battery-Aware Animations

```js
// Disable animations on low battery
async function checkBattery() {
  if ('getBattery' in navigator) {
    const battery = await navigator.getBattery();
    if (battery.level < 0.2 && !battery.charging) {
      document.documentElement.classList.add('reduce-animations');
    }

    battery.addEventListener('levelchange', () => {
      document.documentElement.classList.toggle(
        'reduce-animations',
        battery.level < 0.2 && !battery.charging
      );
    });
  }
}

checkBattery();
```

```css
.reduce-animations *,
.reduce-animations *::before,
.reduce-animations *::after {
  animation: none !important;
  transition: none !important;
}
```

### Font Loading Optimization

```css
/* Use font-display: swap for web fonts */
@font-face {
  font-family: 'Inter';
  src: url('/fonts/inter-var.woff2') format('woff2');
  font-display: swap;
  font-weight: 100 900;
}

/* System font fallback that matches Inter metrics */
body {
  font-family: 'Inter', system-ui, -apple-system, 'Segoe UI', Roboto,
    'Helvetica Neue', Arial, sans-serif;
}
```

## Best Practices

1. **Extract critical CSS** for above-the-fold content — keep it under 14KB (one TCP round trip).
2. **Use `loading="lazy"`** on all below-fold images to defer loading.
3. **Use `fetchpriority="high"`** on the hero/LCP image for faster first paint.
4. **Set explicit `width` and `height`** on images to prevent Cumulative Layout Shift (CLS).
5. **Preconnect to CDN origins** (`<link rel="preconnect">`) to save DNS + TLS time.
6. **Use WebP/AVIF image formats** with `<picture>` fallbacks for 25-35% size reduction.
7. **Import only needed Bootstrap Sass partials** — the full stylesheet is ~230KB; selective import can be ~30KB.
8. **Defer non-critical JavaScript** with `defer` attribute or dynamic `import()`.
9. **Respect `prefers-reduced-motion`** to disable animations for users who need it and save battery.
10. **Use `font-display: swap`** to prevent invisible text during web font loading.
11. **Monitor Core Web Vitals** (LCP, FID, CLS) with `web-vitals` library in production.

## Common Pitfalls

1. **Not setting image dimensions** causes layout shift (CLS) as images load.
2. **Loading full Bootstrap CSS** (~230KB) when only grid + utilities (~30KB) are used.
3. **Not deferring below-fold images** loads all images on page load, wasting bandwidth.
4. **Missing `prefers-reduced-motion`** excludes users with vestibular disorders and drains battery.
5. **Blocking font loading** with `font-display: block` causes invisible text (FOIT).

## Accessibility Considerations

Performance optimizations must not compromise accessibility. `prefers-reduced-motion` media queries disable animations for users with motion sensitivity. Lazy-loaded images must have `alt` text. Critical CSS must include accessible color contrast ratios. Skip-to-content links must be in the critical CSS. Font loading with `font-display: swap` ensures text is always readable. Battery-aware animation reduction respects user device constraints.

## Responsive Behavior

Critical CSS extraction should include responsive container and grid rules for the first viewport. Use `sizes` attribute on images to inform the browser of responsive display widths. Bootstrap's responsive utility classes (`d-none`, `d-md-block`) should be in the critical CSS if they affect above-fold content. Mobile-first CSS means the smallest breakpoint styles are in the critical path — override media queries are loaded with the full CSS. Lazy loading thresholds should account for mobile scroll velocity to preload content before it enters the viewport.