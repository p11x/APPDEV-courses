---
title: "Performance Next-Gen"
description: "CSS containment, content-visibility, incremental loading, and next-gen performance optimizations for Bootstrap"
difficulty: 3
tags: [performance, content-visibility, containment, lazy-loading, css-contain]
prerequisites:
  - 08_03_Performance_Optimization
---

## Overview

Next-generation CSS performance features — `content-visibility`, `contain`, and `contain-intrinsic-size` — let the browser skip rendering of off-screen or hidden content. Combined with Bootstrap's component architecture, these features dramatically reduce initial paint time and memory usage for pages with many components.

`content-visibility: auto` skips layout and painting for elements outside the viewport, then renders them as the user scrolls. This is more efficient than `display: none` (which requires re-layout when shown) and virtual scrolling (which requires JavaScript). For a Bootstrap page with 100 cards, only the visible 5-10 cards are rendered initially.

## Basic Implementation

```css
/* content-visibility for long lists */
.card {
  content-visibility: auto;
  contain-intrinsic-size: auto 300px; /* estimated height */
}

/* Containment for isolated components */
.widget {
  contain: layout style paint; /* strict containment */
}

/* Incremental list rendering */
.product-list > .product-item {
  content-visibility: auto;
  contain-intrinsic-size: 200px;
}

/* Skip rendering off-screen sections */
.section {
  content-visibility: auto;
  contain-intrinsic-size: 0 500px;
}
```

```html
<!-- Cards with content-visibility — browser skips off-screen cards -->
<div class="row">
  <div class="col-md-4" style="content-visibility: auto; contain-intrinsic-size: auto 400px;">
    <div class="card">
      <img src="photo.jpg" class="card-img-top" loading="lazy" alt="...">
      <div class="card-body">
        <h5 class="card-title">Card 1</h5>
        <p class="card-text">Description.</p>
      </div>
    </div>
  </div>
  <!-- ... 100 more cards ... -->
</div>
```

```js
// Measure rendering improvement
const observer = new PerformanceObserver((list) => {
  for (const entry of list.getEntries()) {
    if (entry.name === 'first-contentful-paint') {
      console.log(`FCP: ${entry.startTime.toFixed(0)}ms`);
    }
  }
});

observer.observe({ entryTypes: ['paint'] });

// Dynamic contain-intrinsic-size estimation
document.querySelectorAll('.card').forEach(card => {
  if (card.offsetHeight > 0) {
    card.style.containIntrinsicSize = `auto ${card.offsetHeight}px`;
  }
});
```

## Advanced Variations

Combine with intersection observer for progressive enhancement:

```css
.card-placeholder {
  content-visibility: auto;
  contain-intrinsic-size: auto 350px;
  background: var(--bs-secondary-bg);
  min-height: 350px;
}

.card-placeholder.loaded {
  content-visibility: visible;
  background: transparent;
}
```

```js
const observer = new IntersectionObserver((entries) => {
  entries.forEach(entry => {
    if (entry.isIntersecting) {
      const card = entry.target;
      card.classList.add('loaded');
      observer.unobserve(card);
    }
  });
}, { rootMargin: '200px' });

document.querySelectorAll('.card-placeholder').forEach(el => observer.observe(el));
```

## Best Practices

1. Use `content-visibility: auto` on containers with many children (lists, grids, tables).
2. Set `contain-intrinsic-size` to an estimated height to prevent layout shifts.
3. Use `contain: layout style paint` on isolated components for strict containment.
4. Use `loading="lazy"` on images inside `content-visibility` containers.
5. Measure performance impact with `PerformanceObserver` and Lighthouse.
6. Apply `content-visibility` at the section level, not individual elements.
7. Use `contain-intrinsic-size: auto <height>` to remember last rendered size.
8. Test with scrolling — ensure content appears without jank.
9. Combine with `will-change` for elements that animate frequently.
10. Avoid `content-visibility: hidden` on interactive elements (they become unreachable).
11. Use `contain: content` for moderate containment (layout + style + paint).
12. Profile with Chrome DevTools Performance tab to identify render bottlenecks.

## Common Pitfalls

1. **Layout shift** — Wrong `contain-intrinsic-size` causes content to jump when rendered.
2. **Browser support** — `content-visibility` in Chrome 85+, Firefox 125+, Safari 18+.
3. **Accessibility** — `content-visibility: hidden` removes content from accessibility tree.
4. **Scroll anchoring** — Browser may lose scroll position when content renders above the viewport.
5. **Over-containment** — `contain: strict` prevents elements from affecting parent layout (can break sticky positioning).
6. **SEO impact** — Search engines may not index `content-visibility: hidden` content.

## Accessibility Considerations

`content-visibility: auto` preserves accessibility tree access. Screen readers can navigate to off-screen content, but it won't be rendered until scrolled into view. Use `contain-intrinsic-size` to prevent layout shifts that disorient users.

## Responsive Behavior

`content-visibility` and `contain` work at all viewport sizes. Adjust `contain-intrinsic-size` values with media queries for different estimated heights at different breakpoints.
