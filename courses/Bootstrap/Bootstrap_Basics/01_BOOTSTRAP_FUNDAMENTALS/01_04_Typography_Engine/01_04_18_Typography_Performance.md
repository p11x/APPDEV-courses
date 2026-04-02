---
title: "Typography Performance"
topic: "Typography Engine"
subtopic: "Typography Performance"
difficulty: 2
duration: "25 minutes"
prerequisites: ["Font Stack Customization", "Web Font Integration"]
learning_objectives:
  - Optimize web font loading with font-display strategies
  - Preload critical fonts for faster rendering
  - Reduce font-related layout shift (CLS)
---

## Overview

Web fonts significantly impact page load performance. Each font file adds network requests and download time, potentially causing Flash of Invisible Text (FOIT) or Flash of Unstyled Text (FOUT). Bootstrap's default system font stack avoids this entirely, but custom web fonts require optimization through `font-display`, preloading, subsetting, and strategic loading. The goal is fast, stable typography that minimizes Cumulative Layout Shift (CLS).

## Basic Implementation

Using `font-display: swap` for immediate text visibility:

```html
<style>
  @font-face {
    font-family: 'CustomFont';
    src: url('/fonts/custom-font.woff2') format('woff2');
    font-display: swap;
    font-weight: 400;
    font-style: normal;
  }
  :root {
    --bs-font-sans-serif: 'CustomFont', system-ui, sans-serif;
  }
</style>
<p>Text appears immediately in the fallback font, then swaps to CustomFont when loaded.</p>
```

Preloading critical font files:

```html
<head>
  <link rel="preload" href="/fonts/inter-var.woff2" as="font" type="font/woff2" crossorigin>
  <style>
    @font-face {
      font-family: 'Inter';
      src: url('/fonts/inter-var.woff2') format('woff2-variations');
      font-display: swap;
    }
  </style>
</head>
<body>
  <h2>Preloaded Font</h2>
  <p>The Inter font loads faster due to the preload directive.</p>
</body>
```

System font stack for zero font loading:

```html
<style>
  :root {
    --bs-font-sans-serif: system-ui, -apple-system, "Segoe UI", Roboto,
      "Helvetica Neue", Arial, sans-serif;
  }
</style>
<div class="container">
  <h2>Instant Typography</h2>
  <p>System fonts require no download — zero FOIT/FOUT and fastest possible render.</p>
</div>
```

## Advanced Variations

Variable fonts to reduce file count:

```html
<style>
  @font-face {
    font-family: 'Inter';
    src: url('/fonts/inter-var.woff2') format('woff2-variations');
    font-display: swap;
    font-weight: 100 900;
  }
  .text-light-weight { font-weight: 300; }
  .text-regular { font-weight: 400; }
  .text-bold { font-weight: 700; }
</style>
<div class="container">
  <p class="text-light-weight">Light weight from variable font file</p>
  <p class="text-regular">Regular weight from same file</p>
  <p class="text-bold">Bold weight from same file</p>
</div>
```

Font subsetting and unicode-range loading:

```html
<style>
  @font-face {
    font-family: 'CustomLatin';
    src: url('/fonts/custom-latin.woff2') format('woff2');
    font-display: swap;
    unicode-range: U+0000-00FF, U+0131, U+0152-0153;
  }
  @font-face {
    font-family: 'CustomLatin';
    src: url('/fonts/custom-cyrillic.woff2') format('woff2');
    font-display: swap;
    unicode-range: U+0400-045F, U+0490-0491;
  }
</style>
<p>Only the Latin subset loads for this English text. Cyrillic loads only when needed.</p>
```

Font loading API for JavaScript-controlled loading:

```html
<script>
  if ('fonts' in document) {
    Promise.all([
      document.fonts.load('400 1rem Inter'),
      document.fonts.load('700 1rem Inter')
    ]).then(function() {
      document.documentElement.classList.add('fonts-loaded');
    });
  }
</script>
<style>
  .fonts-loaded {
    --bs-font-sans-serif: 'Inter', sans-serif;
  }
</style>
<div class="container">
  <h2>JavaScript-Controlled Font Loading</h2>
  <p>Fonts load asynchronously without blocking rendering.</p>
</div>
```

## Best Practices

1. Use `font-display: swap` to show fallback text immediately while web fonts load.
2. Preload the 1-2 most critical font files with `<link rel="preload">`.
3. Use variable fonts (single file, multiple weights) instead of separate files per weight.
4. Subset fonts to include only required character ranges (Latin, Cyrillic, etc.).
5. Limit font weights to 3-4 (400, 600, 700) — each additional weight adds download time.
6. Host fonts locally or use a CDN with proper CORS headers for `crossorigin` loading.
7. Use WOFF2 format for best compression and browser support.
8. Measure CLS (Cumulative Layout Shift) before and after font loading optimization.
9. Consider system font stacks for performance-critical pages (landing pages, login).
10. Use `font-display: optional` for non-critical fonts that can be skipped on slow connections.

## Common Pitfalls

- **FOIT (Flash of Invisible Text)**: `font-display: block` hides text until the font loads, hurting perceived performance.
- **Layout shift from FOUT**: `font-display: swap` causes text reflow when the web font replaces the fallback. Mitigate with size-adjusted fallbacks.
- **Loading all weights**: Importing 10 font weights when only 3 are used wastes bandwidth.
- **Missing preload**: Without `<link rel="preload">`, font requests start late in the rendering pipeline.
- **Cross-origin issues**: Fonts from external domains need `crossorigin` attribute on preload links.
- **Not subsetting**: Full font files with thousands of unused glyphs waste download size.
- **Blocking render**: Placing font CSS `@import` in render-blocking positions delays first paint.

## Accessibility Considerations

- Ensure font loading doesn't cause text to appear and disappear (FOIT) — users need immediate text access.
- Use `font-display: swap` so screen readers and users always have text content available.
- Maintain readable fallback fonts with similar metrics to the web font (minimizes CLS).
- Don't remove fallback fonts after web font loads — ensure graceful degradation.
- Test with slow connections (3G) to verify text remains readable during font loading.
- Provide font size controls so users can adjust text to their preference.

## Responsive Behavior

Font performance optimization applies at all viewport sizes. Preload and `font-display` settings work independently of responsive behavior:

```html
<head>
  <link rel="preload" href="/fonts/inter-var.woff2" as="font" type="font/woff2" crossorigin>
</head>
<style>
  @font-face {
    font-family: 'Inter';
    src: url('/fonts/inter-var.woff2') format('woff2-variations');
    font-display: swap;
    font-weight: 100 900;
  }
  :root {
    --bs-font-sans-serif: 'Inter', system-ui, sans-serif;
  }
</style>
<div class="container">
  <h2 class="fs-3 fs-md-2 fs-lg-1">Performance-Optimized Responsive Typography</h2>
  <p class="fs-6 fs-md-5">
    Preloaded variable font with swap display and responsive sizing.
    Font loads once, adapts to all weights, and scales responsively.
  </p>
</div>
```

The variable font file loads once and serves all weights and sizes across all breakpoints, providing both performance and responsive flexibility.
