---
title: "Container Query Integration"
description: "Bootstrap with container queries for component-level responsiveness replacing viewport-only media queries"
difficulty: 3
tags: [container-queries, responsive, component-level, bootstrap-future, @container]
prerequisites:
  - 09_02_01_Container_Queries
  - 02_01_Grid_System
---

## Overview

Container queries are poised to reshape Bootstrap's responsive system. Currently, Bootstrap's responsive utilities (`.col-md-6`, `.d-lg-none`) respond to viewport width. Container query integration would add a parallel system where components respond to their container's width, enabling true modularity — a card behaves the same whether it's in a sidebar, main content area, or modal.

The integration path involves new utility classes (`.cq-col-6`, `.cq-d-sm-none`), container-aware components, and `container-type` applied to grid columns automatically. This doesn't replace viewport media queries; it adds a component-level responsive layer.

## Basic Implementation

```html
<!-- Hypothetical Bootstrap 6 container query utilities -->
<div class="row">
  <div class="col-md-4" style="container-type: inline-size;">
    <!-- This card responds to the col-md-4 width, not the viewport -->
    <div class="cq-card">
      <div class="cq-card-img"><img src="photo.jpg" alt="..."></div>
      <div class="cq-card-body">
        <h5 class="cq-card-title">Title</h5>
        <p class="cq-card-text">Description text.</p>
      </div>
    </div>
  </div>
</div>
```

```css
/* Bootstrap container query utilities (hypothetical) */
.cq-col-12 { container-type: inline-size; }
.cq-col-6  { container-type: inline-size; }
.cq-col-4  { container-type: inline-size; }

/* Component-level responsive utilities */
@container (min-width: 400px) {
  .cq-card { flex-direction: row; }
  .cq-card-img { width: 200px; }
}

@container (max-width: 399px) {
  .cq-card { flex-direction: column; }
  .cq-card-img { width: 100%; }
}

@container (min-width: 300px) {
  .cq-d-flex { display: flex; }
}

@container (max-width: 299px) {
  .cq-d-none { display: none; }
}
```

```js
// Auto-apply container-type to grid columns
document.querySelectorAll('.col-md-4, .col-lg-6').forEach(col => {
  col.style.containerType = 'inline-size';
});
```

## Advanced Variations

Named containers for complex layouts:

```css
.main-content {
  container-type: inline-size;
  container-name: main;
}

.sidebar {
  container-type: inline-size;
  container-name: sidebar;
}

@container main (min-width: 800px) {
  .product-card { grid-template-columns: 1fr 2fr; }
}

@container sidebar (max-width: 250px) {
  .product-card .product-title { font-size: 0.875rem; }
  .product-card .product-meta { display: none; }
}
```

## Best Practices

1. Apply `container-type` to grid columns, not individual components.
2. Use named containers (`container-name: main`) for explicit query targeting.
3. Provide viewport-based fallbacks for browsers without container query support.
4. Use container queries for component layout, viewport queries for page layout.
5. Prefix container query utilities (`.cq-*`) to distinguish from viewport utilities.
6. Test components in multiple container sizes using ResizeObserver.
7. Document which Bootstrap components support container queries.
8. Use `container-type: normal` to opt elements out of containment.
9. Avoid container queries that modify the container's own size.
10. Combine with subgrid for aligned nested layouts.
11. Use container query length units (`cqw`, `cqh`) for fluid sizing.
12. Plan migration from viewport-only responsive patterns to dual system.

## Common Pitfalls

1. **Containment side effects** — `container-type` creates a containment context affecting `position: fixed` and percentage widths.
2. **Double responsive logic** — Maintaining both viewport and container queries increases complexity.
3. **Bootstrap class conflict** — Container query utilities may conflict with existing viewport utilities.
4. **Browser support** — Full support in Chrome 105+, Firefox 110+, Safari 16+.
5. **Performance** — Many container contexts increase layout calculation cost.
6. **Debugging** — Container query contexts are harder to inspect than media queries.

## Accessibility Considerations

Container queries don't affect accessibility. Ensure that component layout changes maintain logical reading order and don't hide essential content. Test with screen readers at different container sizes.

## Responsive Behavior

Container queries ARE responsive behavior. Components respond to their container, creating a composable responsive architecture. Page-level layout uses viewport media queries; component-level layout uses container queries. Both systems coexist.
