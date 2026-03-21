# Web Vitals Explained

## What You'll Learn
- Core Web Vitals in depth
- How each metric is measured
- Optimization strategies

## Prerequisites
- Basic performance knowledge

## Do I Need This Right Now?
Core Web Vitals are Google's ranking factors. Understanding them is essential for SEO and user experience.

## Core Web Vitals

### Largest Contentful Paint (LCP)

Measures when the largest content element becomes visible.

**Good:** < 2.5 seconds
**Needs Improvement:** 2.5-4 seconds  
**Poor:** > 4 seconds

**What affects LCP:**
- Server response time
- Render-blocking resources
- Large hero images
- CSS/JS blocking render

**How to optimize:**
- Use next/image with priority
- Enable text compression
- Use streaming SSR
- Optimize images

### First Input Delay (FID)

Time between first user interaction and browser response.

**Good:** < 100ms
**Needs Improvement:** 100-300ms
**Poor:** > 300ms

**What affects FID:**
- Heavy JavaScript execution
- Many event handlers
- Main thread blocking

**How to optimize:**
- Code splitting
- Reduce JavaScript
- Defer non-critical JS

### Cumulative Layout Shift (CLS)

Measures visual stability - how much content shifts unexpectedly.

**Good:** < 0.1
**Needs Improvement:** 0.1-0.25
**Poor:** > 0.25

**What affects CLS:**
- Images without dimensions
- Dynamic content loading
- Late-loading fonts
- Ads/embeds without space

**How to optimize:**
- Always set image dimensions
- Reserve space for ads
- Use font-display: swap
- Preload important assets

### Interaction to Next Paint (INP)

Measures overall page responsiveness.

**Good:** < 200ms
**Needs Improvement:** 200-500ms
**Poor:** > 500ms

**What affects INP:**
- Long tasks in JavaScript
- Event handlers
- Main thread blocking

**How to optimize:**
- Break up long tasks
- Use requestIdleCallback
- Optimize event handlers

## Additional Vitals

### Time to First Byte (TTFB)

Time for browser to receive first byte.

**Good:** < 800ms

### First Contentful Paint (FCP)

Time for first content to render.

**Good:** < 1.8 seconds

## Summary
- LCP: Page load speed (< 2.5s good)
- FID: Interactivity (< 100ms good)
- CLS: Visual stability (< 0.1 good)
- INP: Overall responsiveness (< 200ms good)
- All three Core Web Vitals affect SEO rankings

## Next Steps
- [custom-vitals-reporting.md](./custom-vitals-reporting.md) — Custom metrics
