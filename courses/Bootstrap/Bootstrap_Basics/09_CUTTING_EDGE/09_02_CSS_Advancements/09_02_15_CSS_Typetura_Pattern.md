---
title: CSS Typetura Pattern
category: [CSS Advancements, Cutting Edge]
difficulty: 3
time: 25 min
tags: bootstrap5, fluid-typography, scroll-driven, typetura, responsive-type
---

## Overview

The Typetura pattern creates fully fluid typography that scales smoothly between minimum and maximum sizes without media queries. Using CSS custom properties with `@property` registration and scroll-driven animations, it provides precise control over type scaling curves that adapt to any container or viewport width.

## Basic Implementation

Implementing fluid typography using `@property` and `calc()` with viewport-relative units.

```html
<style>
  @property --fluid-unit {
    syntax: '<length>';
    initial-value: 1rem;
    inherits: true;
  }

  :root {
    --fluid-min-width: 320;
    --fluid-max-width: 1200;
    --fluid-min-size: 16;
    --fluid-max-size: 24;
  }

  .fluid-text {
    --fluid-unit: clamp(
      calc(var(--fluid-min-size) * 1px),
      calc(
        var(--fluid-min-size) * 1px +
        (var(--fluid-max-size) - var(--fluid-min-size)) *
        (100cqi - var(--fluid-min-width) * 1px) /
        (var(--fluid-max-width) - var(--fluid-min-width))
      ),
      calc(var(--fluid-max-size) * 1px)
    );
    font-size: var(--fluid-unit);
    container-type: inline-size;
  }

  .fluid-heading {
    --fluid-min-size: 24;
    --fluid-max-size: 64;
    font-size: var(--fluid-unit);
    line-height: 1.1;
  }
</style>

<div class="fluid-text">
  <h1 class="fluid-heading">Fluid Typography</h1>
  <p>This text scales smoothly from 16px at 320px viewport to 24px at 1200px.</p>
</div>
```

## Advanced Variations

Using scroll-driven animations to scale typography based on scroll position.

```html
<style>
  @keyframes text-scale {
    from {
      font-size: clamp(1rem, 2vw, 1.5rem);
      letter-spacing: 0.05em;
      opacity: 0.7;
    }
    to {
      font-size: clamp(2rem, 5vw, 4rem);
      letter-spacing: -0.02em;
      opacity: 1;
    }
  }

  .scroll-title {
    animation: text-scale linear both;
    animation-timeline: scroll();
    animation-range: 0vh 50vh;
    font-weight: 700;
    line-height: 1.1;
  }

  @keyframes fade-in-up {
    from {
      opacity: 0;
      transform: translateY(2rem);
    }
    to {
      opacity: 1;
      transform: translateY(0);
    }
  }

  .scroll-fade {
    animation: fade-in-up linear both;
    animation-timeline: view();
    animation-range: entry 0% entry 100%;
  }
</style>

<div style="height: 200vh;">
  <div class="position-sticky top-0 vh-100 d-flex align-items-center justify-content-center">
    <h1 class="scroll-title text-center px-3">Scroll to Reveal</h1>
  </div>
</div>

<div class="container py-5">
  <div class="row g-4">
    <div class="col-md-4 scroll-fade">
      <div class="card p-4">
        <h5>Card One</h5>
        <p>Fades in as it enters the viewport.</p>
      </div>
    </div>
    <div class="col-md-4 scroll-fade">
      <div class="card p-4">
        <h5>Card Two</h5>
        <p>View-driven animation timing.</p>
      </div>
    </div>
  </div>
</div>
```

Creating a complete type scale using container queries and custom properties.

```html
<style>
  .type-scale {
    container-type: inline-size;
  }

  .type-scale > * {
    --scale-ratio: 1.25;
    --base-size: clamp(0.875rem, 1cqi + 0.5rem, 1.125rem);
  }

  .type-scale h1 { font-size: calc(var(--base-size) * var(--scale-ratio) * var(--scale-ratio) * var(--scale-ratio) * var(--scale-ratio)); }
  .type-scale h2 { font-size: calc(var(--base-size) * var(--scale-ratio) * var(--scale-ratio) * var(--scale-ratio)); }
  .type-scale h3 { font-size: calc(var(--base-size) * var(--scale-ratio) * var(--scale-ratio)); }
  .type-scale h4 { font-size: calc(var(--base-size) * var(--scale-ratio)); }
  .type-scale p  { font-size: var(--base-size); }
  .type-scale small { font-size: calc(var(--base-size) / var(--scale-ratio)); }
</style>

<div class="type-scale" style="max-width: 800px; margin: auto; padding: 2rem;">
  <h1>Heading Level 1</h1>
  <h2>Heading Level 2</h2>
  <h3>Heading Level 3</h3>
  <h4>Heading Level 4</h4>
  <p>Body text that scales proportionally with the container width.</p>
  <small>Small text for captions and metadata.</small>
</div>
```

## Best Practices

1. Use `@property` to register custom properties with animation support
2. Define min/max viewport widths and font sizes as custom properties for easy tuning
3. Use `container-type: inline-size` and `cqi` units for container-relative scaling
4. Apply scroll-driven animations with `animation-timeline: scroll()` for parallax typography
5. Use `animation-timeline: view()` for scroll-triggered entrance animations
6. Maintain a consistent scale ratio (e.g., 1.25 or 1.333) across the type hierarchy
7. Test fluid type at extreme viewport widths for readability
8. Combine with Bootstrap's typography utilities for base styles
9. Use `clamp()` to enforce absolute min and max font sizes
10. Prefer `cqi` (container query inline) units over `vw` for component-level fluid type

## Common Pitfalls

1. **Browser support** — Scroll-driven animations require Chrome 115+; `@property` needs Chrome 85+
2. **No fallback** — Older browsers show default font size if `clamp()` is unsupported
3. **Container query context missing** — `cqi` units require `container-type` on an ancestor
4. **Zoom conflicts** — Fluid type may not respond to browser zoom as expected
5. **Line-height breaks** — Fluid font sizes need fluid or unitless line-heights
6. **Performance** — Scroll-driven animations on many elements can cause jank
7. **Over-scaling** — Aggressive min/max ranges create unreadable text

## Accessibility Considerations

Ensure minimum font sizes remain readable (16px for body text). Test at 200% browser zoom — fluid type should still be legible. Respect `prefers-reduced-motion` to disable scroll-driven typography animations. Maintain sufficient line-height at all scaled sizes. Provide fallback font sizes for unsupported browsers.

## Responsive Behavior

Fluid typography inherently adapts to viewport and container width. Use container queries for component-level responsiveness. Combine scroll-driven animations with viewport breakpoints for hybrid effects. Test across Bootstrap's breakpoint range to verify smooth transitions.
