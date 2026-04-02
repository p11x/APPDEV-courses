---
title: "Container Queries"
description: "Using @container with Bootstrap for component-level responsiveness independent of viewport"
difficulty: 3
tags: [container-queries, responsive, bootstrap-grid, component-design, @container]
prerequisites:
  - 03_01_Responsive_Basics
---

## Overview

Container queries (`@container`) let components respond to their parent container's size instead of the viewport. A card inside a sidebar should render differently than the same card in a full-width hero — container queries make this possible without JavaScript or multiple component variants.

Bootstrap 5.3+ supports container queries through the `container-type` CSS property. Combined with Bootstrap's grid system, you create truly modular components that adapt their layout based on available space. A `<bs-card>` can show a horizontal layout in wide containers and stack vertically in narrow ones, all from a single component definition.

## Basic Implementation

```html
<div class="row">
  <div class="col-md-4">
    <div class="cq-card-container" style="container-type: inline-size;">
      <bs-card title="Sidebar Card" image="photo.jpg">
        This card adapts to its narrow container.
      </bs-card>
    </div>
  </div>
  <div class="col-md-8">
    <div class="cq-card-container" style="container-type: inline-size;">
      <bs-card title="Main Card" image="photo.jpg">
        This card adapts to its wide container.
      </bs-card>
    </div>
  </div>
</div>
```

```css
.cq-card-container {
  container-type: inline-size;
  container-name: card;
}

@container card (min-width: 400px) {
  .card {
    flex-direction: row;
  }

  .card img {
    width: 200px;
    object-fit: cover;
  }
}

@container card (max-width: 399px) {
  .card {
    flex-direction: column;
  }

  .card img {
    width: 100%;
    height: 200px;
    object-fit: cover;
  }
}
```

```js
// JavaScript can also respond to container size
const observer = new ResizeObserver((entries) => {
  for (const entry of entries) {
    const width = entry.contentRect.width;
    entry.target.dataset.layout = width > 400 ? 'horizontal' : 'vertical';
  }
});

document.querySelectorAll('.cq-card-container').forEach(el => observer.observe(el));
```

## Advanced Variations

Named containers enable scoping queries to specific components:

```css
.dashboard {
  container-type: inline-size;
  container-name: dashboard;
}

.sidebar {
  container-type: inline-size;
  container-name: sidebar;
}

@container dashboard (min-width: 1200px) {
  .widget-grid {
    display: grid;
    grid-template-columns: repeat(4, 1fr);
  }
}

@container sidebar (max-width: 300px) {
  .nav-label { display: none; }
  .nav-icon { font-size: 1.5rem; }
}
```

## Best Practices

1. Use `container-type: inline-size` for horizontal responsiveness (most common need).
2. Use named containers (`container-name: card`) for explicit, readable queries.
3. Scope container queries to specific parent elements, not the entire page.
4. Combine with Bootstrap's grid — containers respond to grid cell width naturally.
5. Use `cq-` prefix for container query utility classes to distinguish from viewport media queries.
6. Test components in multiple container sizes using ResizeObserver.
7. Prefer `min-width` queries (mobile-first within containers).
8. Use container query length units (`cqw`, `cqh`) for fluid sizing within containers.
9. Apply `container-type` to the nearest ancestor, not the component itself.
10. Document which components support container queries in the design system.
11. Use `container-type: normal` to opt out of containment on specific elements.
12. Combine with CSS `clamp()` for smooth scaling within container breakpoints.

## Common Pitfalls

1. **Containment side effects** — `container-type` creates a containment context that may break `position: fixed` or `100%` width children.
2. **No query without type** — `@container` rules are ignored if no ancestor has `container-type`.
3. **Browser support** — Supported in all modern browsers (2023+); provide viewport fallback for older browsers.
4. **Infinite loops** — Avoid container queries that change the container's own size.
5. **Overuse** — Not every component needs container queries; use viewport media queries for page-level layout.
6. **Debugging difficulty** — Browser DevTools container query support varies; use explicit names for easier debugging.

## Accessibility Considerations

Container queries don't affect accessibility directly. Ensure that layout changes at different container sizes maintain logical reading order and don't hide interactive elements without equivalent alternatives.

## Responsive Behavior

Container queries ARE responsive behavior. The component adapts to its container, creating a composable responsive system where each component independently handles its layout. Pair with Bootstrap's grid for a two-level responsive strategy: grid handles page layout, container queries handle component layout.
