---
title: "Equal Width Columns"
topic: "Grid Deep Dive"
subtopic: "Equal Width Columns"
difficulty: 1
duration: "25 minutes"
prerequisites: ["Bootstrap Grid Basics", "Responsive Breakpoints"]
learning_objectives:
  - Understand how col class creates equal-width columns
  - Apply equal-width columns at all breakpoints
  - Master flex-grow behavior in Bootstrap grid
---

## Overview

Bootstrap's `col` class without a breakpoint suffix creates equal-width columns that automatically distribute available space evenly. Unlike fixed-column classes (e.g., `col-md-4`), the plain `col` class uses CSS Flexbox's `flex-grow` to divide space equally regardless of content. This makes it ideal for layouts where uniform column widths are needed without specifying exact sizes.

When you use `col`, each column receives `flex: 1 0 0%`, meaning columns grow equally, don't shrink below their base size, and start from zero width. This behavior differs from `col-auto`, which sizes columns based on their content.

## Basic Implementation

The simplest equal-width layout uses multiple `col` classes inside a `row`. Each column automatically receives the same proportion of available space.

```html
<div class="container">
  <div class="row">
    <div class="col">
      <div class="bg-light p-3 border">Column 1</div>
    </div>
    <div class="col">
      <div class="bg-light p-3 border">Column 2</div>
    </div>
  </div>
</div>
```

Three equal-width columns divide the row into thirds:

```html
<div class="container">
  <div class="row">
    <div class="col">
      <div class="bg-primary text-white p-3">1 of 3</div>
    </div>
    <div class="col">
      <div class="bg-secondary text-white p-3">2 of 3</div>
    </div>
    <div class="col">
      <div class="bg-success text-white p-3">3 of 3</div>
    </div>
  </div>
</div>
```

Four columns distribute space into quarters automatically:

```html
<div class="container">
  <div class="row">
    <div class="col">Column A</div>
    <div class="col">Column B</div>
    <div class="col">Column C</div>
    <div class="col">Column D</div>
  </div>
</div>
```

## Advanced Variations

Equal-width columns work at specific breakpoints too. Using `col-sm`, `col-md`, or `col-lg` without a number means columns become equal-width only at that breakpoint and above, while stacking on smaller screens.

```html
<div class="container">
  <div class="row">
    <div class="col-md">
      <div class="bg-info text-white p-3">
        Stacks on small, equal-width on md+
      </div>
    </div>
    <div class="col-md">
      <div class="bg-warning p-3">
        Stacks on small, equal-width on md+
      </div>
    </div>
    <div class="col-md">
      <div class="bg-danger text-white p-3">
        Stacks on small, equal-width on md+
      </div>
    </div>
  </div>
</div>
```

Mixing `col` with fixed-width columns. Fixed columns take their specified proportion while `col` fills the remaining space:

```html
<div class="container">
  <div class="row">
    <div class="col-4">
      <div class="bg-primary text-white p-3">Fixed 1/3</div>
    </div>
    <div class="col">
      <div class="bg-secondary text-white p-3">Fills remaining</div>
    </div>
  </div>
</div>
```

Nesting equal-width columns inside a parent column:

```html
<div class="container">
  <div class="row">
    <div class="col">
      <div class="row">
        <div class="col bg-light border p-2">Nested 1</div>
        <div class="col bg-light border p-2">Nested 2</div>
      </div>
    </div>
    <div class="col bg-primary text-white p-3">Sibling</div>
  </div>
</div>
```

## Best Practices

1. Use `col` for layouts where content should share space equally without specifying exact breakpoints.
2. Combine `col` with `col-{n}` when one column needs a fixed width and others should fill remaining space.
3. Always wrap columns inside a `.row` container to maintain proper gutter alignment.
4. Use `col-sm`, `col-md`, `col-lg` breakpoint variants to create responsive equal-width layouts that stack on mobile.
5. Apply `g-*` classes on the `.row` to control gutter spacing between equal-width columns.
6. Avoid placing excessive content in one `col` without overflow handling, as it can break the equal-width illusion.
7. Use `.row-cols-*` alongside `col` for automatic column wrapping control.
8. Keep equal-width column counts manageable — more than 6 equal columns produces very narrow layouts.
9. Test equal-width columns with varying content lengths to ensure visual balance.
10. Use `vh-100` or `min-vh-100` on rows for full-viewport equal-height layouts when combined with equal-width columns.

## Common Pitfalls

- **Forgetting the `.row` wrapper**: Placing `col` elements directly inside a `container` without a `row` breaks gutter behavior and alignment.
- **Unequal content heights**: Equal-width columns do not automatically equalize height unless you add `h-100` or use flex utilities on child elements.
- **Overriding `flex-grow`**: Adding custom CSS that overrides `flex-grow: 1` breaks the equal distribution behavior of `col`.
- **Mixing `col` and `col-auto`**: Expecting `col-auto` to behave like `col` causes unexpected sizing since `col-auto` sizes to content, not available space.
- **Ignoring responsive behavior**: Plain `col` columns remain side-by-side at all screen sizes. Use `col-sm` or `col-md` for responsive stacking.
- **Too many columns**: Creating more than 12 equal-width columns in a single row causes wrapping since Bootstrap's grid is 12-based.
- **Gutter confusion**: Applying `g-5` on a row with many equal-width columns can make content overflow due to excessive gutters.

## Accessibility Considerations

- Use semantic HTML elements (`<section>`, `<article>`, `<main>`) inside columns instead of only `<div>` elements.
- Ensure content reading order matches visual order. Equal-width columns placed side-by-side visually may read top-to-bottom then left-to-right in screen readers.
- Add `role="region"` and `aria-label` to column groups when they represent distinct content areas.
- Maintain sufficient color contrast when applying background colors to column content areas.
- Avoid using columns purely for visual layout when the content is a single logical unit — use CSS for layout instead.
- Provide `lang` attributes on text content within columns for multilingual pages.

## Responsive Behavior

Plain `col` columns never stack. At every viewport width, columns share space equally. For responsive stacking, use breakpoint-specific classes:

```html
<div class="container">
  <div class="row">
    <div class="col-12 col-md">
      <div class="bg-primary text-white p-3">Full width on mobile, equal on md+</div>
    </div>
    <div class="col-12 col-md">
      <div class="bg-secondary text-white p-3">Full width on mobile, equal on md+</div>
    </div>
  </div>
</div>
```

The `col-sm` class keeps columns stacked below 576px and switches to equal-width above that breakpoint. Use `col-lg` for desktop-only equal-width behavior. Combining `col-12 col-md` creates a mobile-first stacked layout that becomes equal-width at medium screens.
