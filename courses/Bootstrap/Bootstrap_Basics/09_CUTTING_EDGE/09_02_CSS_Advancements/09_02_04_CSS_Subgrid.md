---
title: "CSS Subgrid"
description: "Using CSS subgrid with Bootstrap's grid system to align nested grid items to parent tracks"
difficulty: 3
tags: [subgrid, css-grid, bootstrap-grid, nested-alignment, layout]
prerequisites:
  - 02_01_Grid_System
  - 09_02_01_Container_Queries
---

## Overview

CSS subgrid allows a nested grid to inherit track definitions from its parent grid. Without subgrid, nested grids have independent track definitions, causing alignment issues — a common pain point with card grids where cards have varying content heights. Subgrid solves this by letting child grids align their internal tracks to the parent's rows and columns.

Bootstrap's grid is flexbox-based, but you can layer CSS Grid with subgrid on top for specific alignment needs. The `grid-template-rows: subgrid` and `grid-template-columns: subgrid` properties inherit parent tracks, ensuring content in sibling cards aligns perfectly regardless of content length.

## Basic Implementation

```html
<div class="card-grid">
  <div class="card-grid-item">
    <div class="card">
      <img src="photo1.jpg" class="card-img-top" alt="...">
      <div class="card-body">
        <h5 class="card-title">Short Title</h5>
        <p class="card-text">Brief description.</p>
      </div>
      <div class="card-footer">
        <a href="#" class="btn btn-primary">Go</a>
      </div>
    </div>
  </div>
  <div class="card-grid-item">
    <div class="card">
      <img src="photo2.jpg" class="card-img-top" alt="...">
      <div class="card-body">
        <h5 class="card-title">Much Longer Title That Wraps</h5>
        <p class="card-text">Much longer description that takes more space and pushes content down.</p>
      </div>
      <div class="card-footer">
        <a href="#" class="btn btn-primary">Go</a>
      </div>
    </div>
  </div>
</div>
```

```css
.card-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  grid-template-rows: auto auto 1fr auto; /* image | title | body | footer */
  gap: 1.5rem;
}

.card-grid-item {
  display: grid;
  grid-template-rows: subgrid; /* inherits parent rows */
  grid-row: span 4; /* this card spans 4 parent rows */
}

.card {
  display: grid;
  grid-template-rows: subgrid;
  grid-row: 1 / -1;
  margin: 0; /* override Bootstrap card margin */
}

/* Each section aligns to the parent grid track */
.card-img-top { grid-row: 1; }
.card-title { grid-row: 2; }
.card-text { grid-row: 3; }
.card-footer { grid-row: 4; }
```

```js
// Feature detection
if (CSS.supports('grid-template-rows', 'subgrid')) {
  document.documentElement.classList.add('supports-subgrid');
}
```

## Advanced Variations

Combine subgrid with Bootstrap's responsive breakpoints:

```css
.card-grid {
  display: grid;
  grid-template-columns: repeat(12, 1fr);
  gap: var(--bs-gutter-x, 1.5rem);
}

.card-grid-item {
  grid-column: span 4;
  display: grid;
  grid-template-rows: subgrid;
  grid-row: span 4;
}

@media (max-width: 768px) {
  .card-grid-item {
    grid-column: span 12;
  }
}
```

## Best Practices

1. Use `grid-template-rows: subgrid` for vertical alignment of sibling content.
2. Use `grid-template-columns: subgrid` for horizontal alignment across nested containers.
3. Set `grid-row: span N` to declare how many parent tracks the child spans.
4. Define the parent grid's track structure explicitly for subgrid children to inherit.
5. Use subgrid for card grids, data tables, and form layouts where content alignment matters.
6. Provide flexbox fallback for browsers without subgrid support.
7. Use `gap` on the parent grid; subgrid children inherit the gap.
8. Combine with Bootstrap's CSS custom properties for theming.
9. Test subgrid alignment with varying content lengths.
10. Use named grid lines for explicit alignment control.
11. Avoid mixing Bootstrap's flexbox grid with CSS Grid subgrid in the same row.
12. Document which components use subgrid in the design system.

## Common Pitfalls

1. **Browser support** — Chrome 117+, Firefox 71+, Safari 16+; no support in older browsers.
2. **No fallback** — Without explicit fallback, older browsers show unaligned content.
3. **Track count mismatch** — If the child spans more tracks than the parent has, layout breaks.
4. **Bootstrap conflict** — Bootstrap's `.row` and `.col` classes use flexbox; mixing with CSS Grid requires careful scoping.
5. **Infinite scroll issues** — Adding items dynamically may require grid recalculation.
6. **`grid-row: span` required** — Without `span`, the subgrid only occupies one parent track.

## Accessibility Considerations

Subgrid is purely visual. Use semantic HTML structure inside the grid items. Screen readers read content in DOM order, not visual grid order. Avoid reordering content visually that differs from DOM order.

## Responsive Behavior

Subgrid adapts to parent grid changes. When the parent grid switches column counts at breakpoints, subgrid children automatically adjust. Use `grid-template-columns: subgrid` for horizontal alignment across responsive breakpoints.
