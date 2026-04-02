---
title: "Custom Grid System"
difficulty: 3
category: "Advanced Development"
subcategory: "Custom Frameworks"
prerequisites:
  - CSS Grid Layout
  - Bootstrap 5 Grid Architecture
  - SCSS Map Manipulation
---

## Overview

Building a custom grid system on top of Bootstrap 5 involves extending or replacing Bootstrap's flexbox-based grid with CSS Grid, custom breakpoint systems, or specialized layout patterns. While Bootstrap's 12-column flexbox grid handles most layouts, certain use cases like magazine-style layouts, dashboard grids, or complex responsive designs benefit from a tailored grid approach.

A custom grid system can layer CSS Grid on top of Bootstrap's existing classes, providing features like named grid areas, masonry-like layouts, container queries, and dynamic column sizing based on content rather than viewport width. The extension maintains backward compatibility with Bootstrap's `.row` and `.col-*` classes while offering enhanced capabilities through new class conventions.

The implementation should leverage CSS custom properties for runtime configuration, SCSS mixins for build-time generation, and progressive enhancement so layouts work in browsers without CSS Grid support.

## Basic Implementation

A custom grid layer extends Bootstrap's grid with CSS Grid capabilities using a parallel class system.

```scss
// Custom grid extension for Bootstrap
@import 'bootstrap/scss/functions';
@import 'bootstrap/scss/variables';
@import 'bootstrap/scss/mixins';

// Custom grid configuration
$grid-columns: 12 !default;
$custom-grid-gap: map-get($spacers, 3) !default;
$custom-grid-breakpoints: $grid-breakpoints !default;

// Named grid template areas
$grid-areas: (
  'dashboard': (
    'header header header',
    'sidebar main aside',
    'footer footer footer'
  ),
  'article': (
    'title title',
    'content sidebar',
    'meta sidebar'
  )
) !default;

// CSS Grid container
.g-grid {
  display: grid;
  gap: var(--g-gap, #{$custom-grid-gap});
  grid-template-columns: repeat(var(--g-columns, #{$grid-columns}), 1fr);
}

// Grid area definitions
@each $name, $areas in $grid-areas {
  .g-grid--#{$name} {
    $template: '';
    @each $row in $areas {
      $template: $template + '"' + $row + '" ';
    }
    grid-template-areas: #{$template};
  }
}

// Grid item placement
@for $i from 1 through $grid-columns {
  .g-col-#{$i} {
    grid-column: span $i;
  }
}

// Named area children
@each $name, $areas in $grid-areas {
  $all-areas: '';
  @each $row in $areas {
    @each $area in $row {
      @if str-index($all-areas, $area) == null {
        $all-areas: $all-areas + ' ' + $area;
      }
    }
  }

  .g-grid--#{$name} {
    @each $area in $all-areas {
      > .g-area--#{$area} {
        grid-area: #{$area};
      }
    }
  }
}
```

```html
<!-- Dashboard grid layout -->
<div class="g-grid g-grid--dashboard" style="--g-columns: 12; min-height: 100vh;">
  <header class="g-area--header bg-primary text-white p-3">
    <h1>Dashboard</h1>
  </header>

  <nav class="g-area--sidebar bg-light p-3">
    <ul class="nav flex-column">
      <li class="nav-item"><a class="nav-link" href="#">Home</a></li>
      <li class="nav-item"><a class="nav-link" href="#">Reports</a></li>
      <li class="nav-item"><a class="nav-link" href="#">Settings</a></li>
    </ul>
  </nav>

  <main class="g-area--main p-4">
    <h2>Main Content</h2>
    <p>Your dashboard content goes here.</p>
  </main>

  <aside class="g-area--aside bg-light p-3">
    <h5>Widgets</h5>
  </aside>

  <footer class="g-area--footer bg-dark text-white p-3">
    <small>&copy; 2025 Company</small>
  </footer>
</div>
```

```js
// Dynamic grid configuration
class CustomGrid {
  constructor(container, options = {}) {
    this.container = typeof container === 'string'
      ? document.querySelector(container)
      : container;

    this.options = {
      columns: 12,
      gap: '1rem',
      minColumnWidth: '200px',
      ...options
    };

    this._applyStyles();
  }

  _applyStyles() {
    this.container.style.setProperty('--g-columns', this.options.columns);
    this.container.style.setProperty('--g-gap', this.options.gap);
    this.container.classList.add('g-grid');
  }

  setColumns(count) {
    this.options.columns = count;
    this._applyStyles();
  }

  setGap(gap) {
    this.options.gap = gap;
    this._applyStyles();
  }

  autoFill() {
    this.container.style.gridTemplateColumns =
      `repeat(auto-fill, minmax(${this.options.minColumnWidth}, 1fr))`;
  }

  autoFit() {
    this.container.style.gridTemplateColumns =
      `repeat(auto-fit, minmax(${this.options.minColumnWidth}, 1fr))`;
  }
}

// Auto-fill responsive grid
const grid = new CustomGrid('.product-grid', {
  minColumnWidth: '250px',
  gap: '1.5rem'
});
```

## Advanced Variations

Advanced custom grids implement container queries, subgrid support, and dynamic layout switching based on content density.

```scss
// Container query grid (progressive enhancement)
@supports (container-type: inline-size) {
  .g-grid--responsive {
    container-type: inline-size;
    container-name: grid-container;
  }
}

@container grid-container (min-width: 400px) {
  .g-grid--responsive > * {
    grid-column: span 6;
  }
}

@container grid-container (min-width: 768px) {
  .g-grid--responsive > * {
    grid-column: span 4;
  }
}

@container grid-container (min-width: 1024px) {
  .g-grid--responsive > * {
    grid-column: span 3;
  }
}

// Masonry-like grid
.g-grid--masonry {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  grid-auto-rows: 10px;

  > * {
    grid-row: span var(--item-rows, 10);
  }
}

// Dense packing variant
.g-grid--dense {
  grid-auto-flow: dense;
}

// Responsive breakpoint grid
@each $breakpoint, $min-width in $custom-grid-breakpoints {
  @include media-breakpoint-up($breakpoint) {
    $infix: breakpoint-infix($breakpoint, $custom-grid-breakpoints);

    @for $i from 1 through $grid-columns {
      .g-col#{$infix}-#{$i} {
        grid-column: span $i;
      }
    }

    .g-row#{$infix}-#{$i} {
      grid-row: span $i;
    }
  }
}
```

```html
<!-- Masonry grid with variable item heights -->
<div class="g-grid g-grid--masonry" style="gap: 1rem;">
  <div class="card" style="--item-rows: 20;">
    <img src="photo.jpg" class="card-img-top" alt="Photo">
    <div class="card-body"><p class="card-text">Tall card</p></div>
  </div>
  <div class="card" style="--item-rows: 15;">
    <div class="card-body"><p class="card-text">Medium card</p></div>
  </div>
  <div class="card" style="--item-rows: 25;">
    <img src="landscape.jpg" class="card-img-top" alt="Landscape">
    <div class="card-body"><p class="card-text">Extra tall card</p></div>
  </div>
</div>
```

```js
// Dynamic grid layout engine
class GridLayoutEngine {
  static layouts = {
    cards: (count) => ({
      columns: Math.min(count, 4),
      gap: '1.5rem',
      rowHeight: 'auto'
    }),
    gallery: (count) => ({
      columns: Math.ceil(Math.sqrt(count)),
      gap: '0.5rem',
      rowHeight: '200px'
    }),
    list: () => ({
      columns: 1,
      gap: '0',
      rowHeight: 'auto'
    })
  };

  static apply(container, layoutName, itemCount) {
    const config = this.layouts[itemName]?.(itemCount)
      || this.layouts.cards(itemCount);

    const grid = container;
    grid.style.display = 'grid';
    grid.style.gap = config.gap;

    if (config.rowHeight !== 'auto') {
      grid.style.gridAutoRows = config.rowHeight;
    } else {
      grid.style.gridTemplateColumns =
        `repeat(${config.columns}, 1fr)`;
    }
  }
}
```

## Best Practices

1. **Layer on top of Bootstrap** - Never replace Bootstrap's grid classes; extend with complementary `g-grid` classes that coexist alongside `.row` and `.col-*`.
2. **Use CSS custom properties** - Expose grid configuration through CSS variables so layouts can be adjusted without JavaScript re-renders.
3. **Provide fallbacks** - Use `@supports (display: grid)` to provide flexbox fallbacks for older browsers.
4. **Follow Bootstrap naming conventions** - Use similar naming patterns (e.g., `g-col-6` mirrors `col-6`) for developer familiarity.
5. **Support Bootstrap breakpoints** - Use Bootstrap's breakpoint map and mixins for consistency with the rest of the framework.
6. **Document grid areas** - Named grid areas must be documented with visual diagrams so designers understand available layouts.
7. **Keep column counts consistent** - Maintain the 12-column convention so existing Bootstrap users have an intuitive mapping.
8. **Use `auto-fill` for dynamic content** - `repeat(auto-fill, minmax())` creates responsive grids without media queries.
9. **Test with real content** - Grid layouts should be validated with varying content lengths, missing items, and dynamic data.
10. **Minimize CSS output** - Generate only the grid classes your project uses through SCSS configuration flags.

## Common Pitfalls

1. **Conflicting with Bootstrap's grid** - Adding CSS Grid to `.row` elements overrides Bootstrap's flexbox properties, breaking alignment utilities.
2. **Forgetting gap fallback** - The `gap` property in CSS Grid doesn't work in older browsers; provide margin-based fallbacks.
3. **Overusing `grid-template-areas`** - Named areas are powerful but create rigid layouts; use for page shells, not content grids.
4. **Not handling overflow** - Grid items with fixed spans can overflow containers; always set `min-width: 0` on grid children.
5. **Ignoring subgrid** - Not leveraging CSS `subgrid` where supported prevents nested grids from aligning with parent grid tracks.

## Accessibility Considerations

Custom grid layouts must maintain logical DOM order that matches visual presentation. CSS Grid allows visual reordering independent of DOM order, which can confuse screen readers and keyboard navigation.

```html
<!-- Correct: DOM order matches visual order -->
<div class="g-grid g-grid--dashboard">
  <header class="g-area--header">...</header>
  <nav class="g-area--sidebar" aria-label="Main navigation">...</nav>
  <main class="g-area--main" id="main-content">...</main>
  <aside class="g-area--aside" aria-label="Supplementary content">...</aside>
  <footer class="g-area--footer">...</footer>
</div>
```

```scss
// Ensure focus order matches visual order
.g-grid {
  // Do NOT use order property to reorder focusable elements
  // Screen readers follow DOM order, not visual order

  &--dashboard {
    // Header first in DOM = first in tab order
    // Content area receives focus before sidebar
  }
}

// Skip navigation link for grid layouts
.skip-nav {
  position: absolute;
  top: -40px;
  left: 0;
  z-index: $zindex-tooltip;

  &:focus {
    top: 0;
  }
}
```

Always ensure that the tab order follows a logical sequence through the grid content, and that `aria-label` attributes clarify the purpose of each grid region.

## Responsive Behavior

Custom grids should respond to breakpoints using Bootstrap's mixin system and support both viewport-based and container-based responsive patterns.

```scss
// Responsive grid columns using Bootstrap mixins
@each $breakpoint, $min-width in $grid-breakpoints {
  @include media-breakpoint-up($breakpoint) {
    $infix: breakpoint-infix($breakpoint, $grid-breakpoints);

    .g-grid#{$infix}--auto {
      grid-template-columns: repeat(
        auto-fill,
        minmax(var(--g-min-col-width, 250px), 1fr)
      );
    }

    @for $i from 1 through 12 {
      .g-cols#{$infix}-#{$i} {
        grid-template-columns: repeat($i, 1fr);
      }
    }
  }
}

// Responsive grid areas
@include media-breakpoint-down(md) {
  .g-grid--dashboard {
    grid-template-areas:
      'header'
      'main'
      'sidebar'
      'aside'
      'footer';
    grid-template-columns: 1fr;
  }
}
```

The grid should collapse gracefully on smaller screens, stacking grid items vertically when the container width falls below the minimum threshold defined by `--g-min-col-width`.
