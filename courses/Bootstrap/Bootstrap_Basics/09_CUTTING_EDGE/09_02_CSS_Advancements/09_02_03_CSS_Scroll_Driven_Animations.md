---
title: "CSS Scroll-Driven Animations"
description: "Using scroll-timeline and view() timelines for scroll-linked Bootstrap animations without JavaScript"
difficulty: 3
tags: [scroll-animations, scroll-timeline, view-timeline, css-animations, bootstrap]
prerequisites:
  - 08_02_CSS_Transitions
---

## Overview

CSS scroll-driven animations link animation progress to scroll position instead of time. Two timeline types exist: `scroll()` tracks the scroll container's scroll position, and `view()` tracks an element's visibility within its scroll container. Combined with Bootstrap's existing animation utilities (`.fade`, `.slide`), scroll-driven animations create reveal-on-scroll effects, progress indicators, and parallax-like effects — all without JavaScript.

The `animation-timeline` property replaces JavaScript scroll listeners for animation purposes. `scroll(root)` ties animation to the document scroll, while `view()` creates scroll-triggered animations based on element visibility (e.g., fade in as element enters viewport).

## Basic Implementation

```html
<!-- Scroll-linked progress bar -->
<nav class="navbar navbar-fixed-top">
  <div class="scroll-progress" style="animation-timeline: scroll();"></div>
</nav>

<!-- Reveal on scroll -->
<div class="container">
  <div class="reveal-on-scroll card mb-3">
    <div class="card-body">Card 1 — reveals as you scroll</div>
  </div>
  <div class="reveal-on-scroll card mb-3">
    <div class="card-body">Card 2 — reveals as you scroll</div>
  </div>
  <div class="reveal-on-scroll card mb-3">
    <div class="card-body">Card 3 — reveals as you scroll</div>
  </div>
</div>
```

```css
/* Scroll progress bar */
.scroll-progress {
  height: 3px;
  background: var(--bs-primary);
  transform-origin: left;
  animation: progress-bar linear;
  animation-timeline: scroll();
}

@keyframes progress-bar {
  from { transform: scaleX(0); }
  to { transform: scaleX(1); }
}

/* View-based reveal animation */
.reveal-on-scroll {
  opacity: 0;
  transform: translateY(30px);
  animation: fade-slide-up linear both;
  animation-timeline: view();
  animation-range: entry 10% entry 90%;
}

@keyframes fade-slide-up {
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
```

```js
// Progressive enhancement fallback for unsupported browsers
if (!CSS.supports('animation-timeline: view()')) {
  const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        entry.target.classList.add('visible');
      }
    });
  }, { threshold: 0.1 });

  document.querySelectorAll('.reveal-on-scroll').forEach(el => {
    observer.observe(el);
  });
}
```

## Advanced Variations

Parallax effect using `scroll(root)`:

```css
.parallax-hero {
  background-image: url('hero.jpg');
  background-size: cover;
  background-position: center;
  height: 60vh;
  animation: parallax linear;
  animation-timeline: scroll();
  animation-range: 0vh 60vh;
}

@keyframes parallax {
  from { background-position: center 0%; }
  to { background-position: center 100%; }
}
```

Staggered card reveals:

```css
.reveal-on-scroll:nth-child(1) { animation-range: entry 5% entry 85%; }
.reveal-on-scroll:nth-child(2) { animation-range: entry 10% entry 90%; }
.reveal-on-scroll:nth-child(3) { animation-range: entry 15% entry 95%; }
```

## Best Practices

1. Use `animation-timeline: scroll()` for page-level scroll-linked animations.
2. Use `animation-timeline: view()` for element-visibility-triggered animations.
3. Set `animation-range` to control when the animation starts and ends relative to scroll.
4. Use `animation-fill-mode: both` to hold the start and end states.
5. Provide JavaScript `IntersectionObserver` fallback for unsupported browsers.
6. Use `prefers-reduced-motion: reduce` to disable scroll animations for accessibility.
7. Keep scroll animations subtle; heavy parallax causes motion sickness.
8. Use `linear` timing for scroll-linked animations (scroll progress is linear).
9. Test on mobile — scroll-driven animations on touch devices may have performance issues.
10. Combine with Bootstrap's `.fade` and `.slide` classes for consistent animation vocabulary.
11. Use named timelines for complex multi-element animations.
12. Avoid scroll-driven animations on critical content; they should enhance, not gate.

## Common Pitfalls

1. **Browser support** — Chrome 115+, Firefox 110+, Safari 17.4+; no support in older browsers.
2. **`animation-range` confusion** — Values like `entry 10% entry 90%` are relative to the view timeline, not the page.
3. **Performance** — Animating `transform` and `opacity` is GPU-accelerated; animating `height` or `margin` is not.
4. **Missing `@keyframes`** — The animation name must reference a defined `@keyframes` rule.
5. **`scroll()` container ambiguity** — `scroll()` defaults to the nearest scroll ancestor; use `scroll(root)` for the document.
6. **Reduced motion ignored** — Without explicit `prefers-reduced-motion` handling, animations play for all users.

## Accessibility Considerations

Respect `prefers-reduced-motion`:

```css
@media (prefers-reduced-motion: reduce) {
  .reveal-on-scroll,
  .scroll-progress {
    animation: none !important;
    opacity: 1;
    transform: none;
  }
}
```

## Responsive Behavior

Scroll-driven animations work at all viewport sizes. Adjust `animation-range` values for different screen sizes using media queries to account for varying scroll distances on mobile.
