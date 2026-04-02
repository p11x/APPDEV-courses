---
title: Grid Functions and Mixins
category: Layout System
difficulty: 3
time: 30 min
tags: bootstrap5, grid, sass, mixins, functions, custom-grid
---

## Overview

Bootstrap 5's grid system is built entirely with Sass functions and mixins that generate the CSS for containers, rows, and columns. Understanding these internal tools lets you create custom grid configurations, override default breakpoints, generate specialized column classes, and build grid systems for components that cannot rely on utility classes alone. The primary functions include `make-container`, `make-row`, `make-col`, and `make-col-ready`, each producing specific CSS properties that compose the grid.

## Basic Implementation

The `make-container` mixin generates the container's max-width values at each breakpoint and applies horizontal padding. Use it when building a custom container class with different max-widths or padding.

```scss
// Using make-container mixin
.custom-container {
  @include make-container();
}

// Output generates:
// .custom-container {
//   width: 100%;
//   padding-right: var(--bs-gutter-x, 0.75rem);
//   padding-left: var(--bs-gutter-x, 0.75rem);
//   margin-right: auto;
//   margin-left: auto;
// }
```

The `make-row` mixin creates the negative-margin technique that compensates for column gutters, keeping content flush with the container edges.

```scss
// Using make-row mixin
.custom-row {
  @include make-row();
}

// Output generates:
// .custom-row {
//   --bs-gutter-x: 1.5rem;
//   --bs-gutter-y: 0;
//   display: flex;
//   flex-wrap: wrap;
//   margin-top: calc(-1 * var(--bs-gutter-y));
//   margin-right: calc(-0.5 * var(--bs-gutter-x));
//   margin-left: calc(-0.5 * var(--bs-gutter-x));
// }
```

The `make-col-ready` mixin establishes the base column properties — flex-basis, padding for gutters, and width — without assigning a specific column span.

```scss
// Using make-col-ready mixin
.custom-col {
  @include make-col-ready();
}

// Output generates:
// .custom-col {
//   flex-shrink: 0;
//   width: 100%;
//   max-width: 100%;
//   padding-right: calc(var(--bs-gutter-x) * 0.5);
//   padding-left: calc(var(--bs-gutter-x) * 0.5);
//   margin-top: var(--bs-gutter-y);
// }
```

## Advanced Variations

The `make-col` mixin generates a column with a specific number of grid columns. Combined with a loop, you can produce a full set of column utility classes.

```scss
// Generate custom 16-column grid classes
@for $i from 1 through 16 {
  .col-custom-#{$i} {
    @include make-col($i, 16);
  }
}

// Produces classes like .col-custom-1 through .col-custom-16
// Each calculates flex: 0 0 auto and width as a percentage of 16
```

You can generate responsive column classes at specific breakpoints using `make-col` inside a breakpoint media query mixin.

```scss
// Responsive custom columns
@each $breakpoint, $width in $grid-breakpoints {
  @include media-breakpoint-up($breakpoint, $grid-breakpoints) {
    @for $i from 1 through 6 {
      .col-#{$breakpoint}-#{$i} {
        @include make-col($i, 6);
      }
    }
  }
}

// Generates .col-sm-1 through .col-sm-6,
// .col-md-1 through .col-md-6, etc.
```

The `make-container-max-widths` function retrieves the max-width map and can be overridden for custom container sizes.

```scss
// Override container max-widths
$container-max-widths: (
  sm: 540px,
  md: 720px,
  lg: 960px,
  xl: 1140px,
  xxl: 1320px,
);

// Custom narrower container
.container-narrow {
  @include make-container();
  @each $breakpoint, $max-width in $container-max-widths {
    @include media-breakpoint-up($breakpoint) {
      max-width: $max-width * 0.75;
    }
  }
}
```

## Best Practices

1. Import Bootstrap's `_functions.scss` and `_mixins.scss` before using any grid mixin in custom Sass files.
2. Always define `$grid-columns` and `$grid-gutter-width` variables before importing Bootstrap to override defaults.
3. Use `make-col-ready` as the base for any custom column class, then layer `make-col` for sizing.
4. Keep custom grid generation in a dedicated `_grid-custom.scss` partial to maintain separation of concerns.
5. Override `$grid-breakpoints` only when the project genuinely requires different breakpoint values — changing them affects every responsive utility.
6. Use CSS custom properties (`--bs-gutter-x`, `--bs-gutter-y`) in conjunction with grid mixins for runtime gutter adjustments.
7. Test custom grid output by inspecting the compiled CSS to verify percentage widths and negative margins are correct.
8. Avoid generating more than 16 columns unless the design system explicitly requires it; excessive columns increase CSS bundle size.
9. Document any custom grid mixins or functions with comments explaining the intended column count and breakpoints.
10. Use `@each` loops with `$grid-breakpoints` map to generate responsive variants consistently.
11. Prefer mixin-based grid classes over manually writing `flex` and `width` properties to ensure compatibility with Bootstrap's gutter system.

## Common Pitfalls

1. **Forgetting to import dependencies**: Using `make-col` without importing Bootstrap's mixin file results in a Sass compilation error.
2. **Mismatched column denominator**: Passing a total column count to `make-col` that differs from `$grid-columns` causes misaligned widths (e.g., dividing by 16 when Bootstrap expects 12).
3. **Overriding `$grid-columns` without updating loops**: Changing `$grid-columns` to 16 but leaving `@for` loops at `through 12` generates incomplete column sets.
4. **Placing mixins outside flex containers**: The `make-col` and `make-col-ready` mixins produce flex item properties that have no effect without a parent `display: flex`.
5. **Nesting `make-row` without gutter adjustment**: Nesting rows inside columns with the default gutter causes double-negative-margin stacking; override `--bs-gutter-x` on nested rows.
6. **Ignoring compiled output size**: Generating columns for every breakpoint and large column counts can produce thousands of unused CSS rules.
7. **Hardcoding pixel values in mixins**: Mixing Bootstrap's rem-based system with pixel values breaks responsive scaling.

## Accessibility Considerations

Grid functions and mixins generate purely visual CSS. Ensure that custom grid classes applied to semantic HTML maintain logical reading order. Avoid generating `order-*` utility classes through Sass loops that could reorder content visually away from DOM order, which disrupts screen reader navigation. When creating custom container classes, preserve sufficient horizontal padding so content does not touch viewport edges on zoomed or high-CSS-pixel-density screens.

## Responsive Behavior

The `make-col` mixin calculates widths as percentages of `$grid-columns`, which scale fluidly at any viewport size. When generating custom responsive columns, nest `make-col` inside `media-breakpoint-up` so classes activate only at the intended breakpoint and above. The `make-container` mixin automatically applies the `$container-max-widths` map to constrain content at each breakpoint. Custom grids should follow the same mobile-first pattern: define full-width defaults via `make-col-ready`, then override with `make-col` inside breakpoint wrappers.
