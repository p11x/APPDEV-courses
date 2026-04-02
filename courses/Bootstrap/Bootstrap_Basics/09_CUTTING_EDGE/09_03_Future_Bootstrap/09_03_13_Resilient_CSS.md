---
title: Resilient CSS
category: [Future Bootstrap, Cutting Edge]
difficulty: 3
time: 30 min
tags: bootstrap5, css-containment, content-visibility, progressive-rendering, performance
---

## Overview

Resilient CSS uses containment, `content-visibility`, and progressive rendering patterns to improve rendering performance and layout stability. Applied to Bootstrap components, these techniques reduce style recalculation cost, skip off-screen rendering, and prevent layout shifts during page load.

## Basic Implementation

Using `content-visibility: auto` to skip rendering off-screen Bootstrap components.

```html
<style>
  .section-render {
    content-visibility: auto;
    contain-intrinsic-size: auto 500px;
  }

  .card-contain {
    contain: layout style paint;
    content-visibility: auto;
    contain-intrinsic-size: auto 300px;
  }
</style>

<div class="container py-5">
  <div class="row g-4">
    ${Array.from({ length: 20 }, (_, i) => `
      <div class="col-md-6 col-lg-4">
        <div class="card card-contain">
          <div class="card-body">
            <h5 class="card-title">Card ${i + 1}</h5>
            <p class="card-text">Content visibility optimizes rendering of this card.</p>
            <button class="btn btn-primary">Action</button>
          </div>
        </div>
      </div>
    `).join('')}
  </div>
</div>
```

## Advanced Variations

Applying containment to Bootstrap's grid layout for incremental rendering.

```html
<style>
  .row-contained {
    contain: layout;
  }

  .col-contained {
    contain: layout style;
    content-visibility: auto;
    contain-intrinsic-size: 0 400px;
  }

  /* Contain cards for independent paint layers */
  .card-contained {
    contain: layout style paint;
    will-change: auto;
  }

  /* Progressive image loading with containment */
  .img-progressive {
    content-visibility: auto;
    contain-intrinsic-size: auto 300px;
    aspect-ratio: 16 / 9;
    object-fit: cover;
    width: 100%;
  }

  /* Sidebar containment for stable layout */
  .sidebar-contained {
    contain: layout;
    position: sticky;
    top: 1rem;
    align-self: flex-start;
  }
</style>

<div class="container-fluid">
  <div class="row">
    <div class="col-lg-3 sidebar-contained d-none d-lg-block">
      <div class="card">
        <div class="card-header">Filters</div>
        <div class="card-body">
          <div class="form-check mb-2">
            <input class="form-check-input" type="checkbox" checked>
            <label class="form-check-label">Category A</label>
          </div>
          <div class="form-check mb-2">
            <input class="form-check-input" type="checkbox">
            <label class="form-check-label">Category B</label>
          </div>
        </div>
      </div>
    </div>
    <div class="col-lg-9">
      <div class="row row-contained g-3">
        <div class="col-md-6 col-contained">
          <div class="card card-contained">
            <img src="https://placehold.co/600x338" class="card-img-top img-progressive" alt="">
            <div class="card-body">
              <h5 class="card-title">Contained Card</h5>
              <p class="card-text">Layout and paint are contained.</p>
            </div>
          </div>
        </div>
        <div class="col-md-6 col-contained">
          <div class="card card-contained">
            <img src="https://placehold.co/600x338" class="card-img-top img-progressive" alt="">
            <div class="card-body">
              <h5 class="card-title">Another Card</h5>
              <p class="card-text">Rendering is skipped when off-screen.</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
```

Using `@supports` for progressive enhancement of containment features.

```html
<style>
  @supports (content-visibility: auto) {
    .progressive-section {
      content-visibility: auto;
      contain-intrinsic-size: auto 600px;
    }
  }

  @supports not (content-visibility: auto) {
    .progressive-section {
      /* Fallback: no containment, standard rendering */
      min-height: 600px;
    }
  }

  /* Prevent layout shift during lazy load */
  .placeholder-ready {
    aspect-ratio: 16 / 9;
    background: var(--bs-secondary-bg);
    border-radius: 0.375rem;
    content-visibility: auto;
    contain-intrinsic-size: auto 400px;
  }

  /* Contained table for large data sets */
  .table-contained {
    contain: layout style;
    content-visibility: auto;
    contain-intrinsic-size: auto 800px;
  }
</style>

<section class="progressive-section">
  <div class="container py-5">
    <h2>Progressive Section</h2>
    <p>This section renders lazily when scrolled into view.</p>
    <table class="table table-striped table-contained">
      <thead>
        <tr><th>ID</th><th>Name</th><th>Status</th></tr>
      </thead>
      <tbody>
        ${Array.from({ length: 50 }, (_, i) => `
          <tr><td>${i + 1}</td><td>Item ${i + 1}</td><td>Active</td></tr>
        `).join('')}
      </tbody>
    </table>
  </div>
</section>
```

## Best Practices

1. Apply `content-visibility: auto` to large containers with many children
2. Always pair `content-visibility: auto` with `contain-intrinsic-size` to prevent collapse
3. Use `contain: layout style paint` on independent Bootstrap card components
4. Set `contain-intrinsic-size` to approximate content height for stable scrollbars
5. Apply containment to table rows, card grids, and list items
6. Use `@supports` to provide fallbacks for browsers without `content-visibility`
7. Combine with `aspect-ratio` to prevent image layout shifts
8. Use `will-change: auto` (default) — only set `will-change` for known animations
9. Test with Lighthouse to verify CLS (Cumulative Layout Shift) improvements
10. Monitor `contain-intrinsic-size` accuracy — wrong values cause scrollbar jumps

## Common Pitfalls

1. **Missing `contain-intrinsic-size`** — Elements collapse to zero height when off-screen
2. **Wrong intrinsic size** — Scrollbar position shifts when content loads into view
3. **Over-containment** — `contain: strict` breaks Bootstrap's grid overflow and positioning
4. **Sticky elements inside containment** — `position: sticky` doesn't work inside contained ancestors
5. **Tooltip/popover positioning** — Absolute positioned elements break inside contained containers
6. **Browser support** — `content-visibility` requires Chrome 85+, Firefox 125+, Safari 18+
7. **Accessibility regression** — Screen readers may skip content-visibility: auto sections
8. **Fixed positioning breaks** — `position: fixed` children of contained elements position relative to the container

## Accessibility Considerations

Test that `content-visibility: auto` content is discoverable by screen readers when scrolled into view. Provide sufficient `contain-intrinsic-size` so that skip-to-content links land at correct positions. Ensure focus management works correctly when elements are rendered lazily. Verify that `aria-live` announcements fire for dynamically rendered contained content.

## Responsive Behavior

Adjust `contain-intrinsic-size` values at different breakpoints since content height varies by viewport width. Use media queries to disable containment on small screens where the DOM is typically smaller. Test scrollbar behavior at all Bootstrap breakpoints to verify stable layout with containment applied.
