---
title: "Responsive Grid Columns"
module: "Responsive Patterns"
lesson: "01_06_03"
difficulty: 1
estimated_time: "15 minutes"
tags: [grid, columns, responsive, col-sm, col-md, stacking]
prerequisites:
  - "01_06_01_Breakpoint_System"
  - "01_06_02_Mobile_First_Philosophy"
---

# Responsive Grid Columns

## Overview

Bootstrap's grid system uses a 12-column layout that responds to viewport changes through breakpoint-suffixed column classes. The responsive column classes — `col-sm-*`, `col-md-*`, `col-lg-*`, `col-xl-*`, and `col-xxl-*` — determine how many of the 12 available grid columns each element occupies at and above their respective breakpoints.

Without a breakpoint suffix, `col-*` classes define the column span at all viewport sizes. When you add a breakpoint suffix, the column span only activates at that breakpoint and above. Below the breakpoint, columns default to their natural block behavior unless another `col-*` base class is specified.

The stacking behavior is central to responsive grid design. On mobile, columns that do not have a `col-*` base class stack vertically, each occupying the full row width. As the viewport crosses breakpoint thresholds, column classes activate and elements rearrange into horizontal rows. This mobile-first column stacking provides natural content flow on small screens while enabling multi-column layouts on larger ones.

Understanding how multiple breakpoint classes interact on a single element is essential for building grids that adapt smoothly across all viewport sizes.

---

## Basic Implementation

The simplest responsive column pattern applies a base `col-12` for mobile and overrides it at larger breakpoints.

**Example 1: Two-column layout that stacks on mobile**

```html
<div class="container">
  <div class="row">
    <div class="col-12 col-md-6">
      <div class="bg-light p-3 border">
        Column 1 — full width on mobile, half on md+
      </div>
    </div>
    <div class="col-12 col-md-6">
      <div class="bg-light p-3 border">
        Column 2 — full width on mobile, half on md+
      </div>
    </div>
  </div>
</div>
```

On viewports below 768px, both columns are full-width and stack vertically. At `md` (768px) and above, `col-md-6` activates and the columns sit side by side, each occupying 6 of the 12 grid columns (50%).

**Example 2: Three-column layout with responsive ratios**

```html
<div class="container">
  <div class="row">
    <div class="col-12 col-sm-6 col-lg-4">
      <div class="p-3 bg-light border">Sidebar</div>
    </div>
    <div class="col-12 col-sm-6 col-lg-5">
      <div class="p-3 bg-light border">Main Content</div>
    </div>
    <div class="col-12 col-lg-3">
      <div class="p-3 bg-light border">Widget Area</div>
    </div>
  </div>
</div>
```

This layout progressively adapts: full-width stacked on `xs`, two columns (6+6) on `sm`, and three columns (4+5+3) on `lg`. Each breakpoint produces a different column arrangement without any CSS overrides beyond Bootstrap's utility classes.

**Example 3: Using `col` (auto) for flexible column widths**

```html
<div class="container">
  <div class="row">
    <div class="col">
      <div class="p-3 bg-primary text-white">Auto</div>
    </div>
    <div class="col">
      <div class="p-3 bg-secondary text-white">Auto</div>
    </div>
    <div class="col">
      <div class="p-3 bg-success text-white">Auto</div>
    </div>
  </div>
</div>
```

The `col` class without a number distributes available width equally among columns. Each column sizes itself based on content and available space. This is useful when you do not need precise column ratios and want the grid to adapt naturally.

---

## Advanced Variations

**Example 4: Combining multiple breakpoints on a single element**

```html
<div class="container">
  <div class="row g-3">
    <div class="col-12 col-sm-6 col-md-4 col-xl-3">
      <div class="card h-100">
        <div class="card-body">Item 1</div>
      </div>
    </div>
    <div class="col-12 col-sm-6 col-md-4 col-xl-3">
      <div class="card h-100">
        <div class="card-body">Item 2</div>
      </div>
    </div>
    <div class="col-12 col-sm-6 col-md-4 col-xl-3">
      <div class="card h-100">
        <div class="card-body">Item 3</div>
      </div>
    </div>
    <div class="col-12 col-sm-6 col-md-4 col-xl-3">
      <div class="card h-100">
        <div class="card-body">Item 4</div>
      </div>
    </div>
  </div>
</div>
```

This product grid uses four breakpoints on each element. At `xs`: one column (stacked). At `sm`: two columns. At `md`: three columns. At `xl`: four columns. The most specific breakpoint class always wins because Bootstrap's CSS follows mobile-first specificity — later (larger) breakpoint rules override earlier (smaller) ones.

**Example 5: Sidebar layout with asymmetric column widths**

```html
<div class="container-fluid">
  <div class="row g-0">
    <nav class="col-12 col-md-3 col-lg-2 bg-dark text-white p-3">
      <h5 class="text-uppercase small">Navigation</h5>
      <ul class="list-unstyled">
        <li><a href="#" class="text-white text-decoration-none">Dashboard</a></li>
        <li><a href="#" class="text-white text-decoration-none">Settings</a></li>
        <li><a href="#" class="text-white text-decoration-none">Profile</a></li>
      </ul>
    </nav>
    <main class="col-12 col-md-9 col-lg-10 p-4">
      <h1>Main Content</h1>
      <p>The sidebar stacks above content on mobile.</p>
    </main>
  </div>
</div>
```

On mobile, the sidebar and main content stack vertically (`col-12`). At `md`, the sidebar takes 3 columns (25%) and the main area takes 9 columns (75%). At `lg`, the sidebar narrows to 2 columns and the main area expands to 10 columns. This progressive adjustment provides more content space on wider screens.

**Example 6: Row columns shorthand for uniform grids**

```html
<div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4 g-3">
  <div class="col">
    <div class="card h-100"><div class="card-body">1</div></div>
  </div>
  <div class="col">
    <div class="card h-100"><div class="card-body">2</div></div>
  </div>
  <div class="col">
    <div class="card h-100"><div class="card-body">3</div></div>
  </div>
  <div class="col">
    <div class="card h-100"><div class="card-body">4</div></div>
  </div>
  <div class="col">
    <div class="card h-100"><div class="card-body">5</div></div>
  </div>
  <div class="col">
    <div class="card h-100"><div class="card-body">6</div></div>
  </div>
</div>
```

The `row-cols-*` classes set how many columns appear per row at each breakpoint, eliminating the need to specify `col-*-3` on every child. At `lg`, four cards per row; at `md`, three; at `sm`, two; on mobile, one. This approach is cleaner than applying column classes to every child element.

**Example 7: Nested responsive columns**

```html
<div class="row">
  <div class="col-12 col-md-8">
    <div class="bg-light p-3 border mb-3">
      <h5>Main Column (col-md-8)</h5>
      <div class="row mt-3">
        <div class="col-12 col-sm-6">
          <div class="bg-white p-2 border">Nested A</div>
        </div>
        <div class="col-12 col-sm-6">
          <div class="bg-white p-2 border">Nested B</div>
        </div>
      </div>
    </div>
  </div>
  <div class="col-12 col-md-4">
    <div class="bg-light p-3 border">
      <h5>Sidebar (col-md-4)</h5>
    </div>
  </div>
</div>
```

Nesting grids inside grid columns creates sub-layouts that respond independently. The outer grid splits into 8+4 at `md`. Inside the `col-md-8`, a nested row splits into 6+6 at `sm`. Each nested `row` resets the 12-column count, giving you a fresh grid to work with.

---

## Best Practices

1. **Always set a base `col-*` class before breakpoint-specific overrides.** `col-12 col-md-6` ensures full-width stacking on mobile. Omitting `col-12` can cause unexpected column widths below `md`.

2. **Use `row-cols-*` for uniform grids instead of repeating column classes.** When all children have the same width, `row-cols-sm-2 row-cols-md-3` on the row is cleaner than `col-sm-6 col-md-4` on every child.

3. **Apply gutters with `g-*` classes on the row, not padding on columns.** Row gutters use negative margins on the row and padding on columns, which is Bootstrap's intended approach. Manual padding on columns does not cancel the row's negative margins.

4. **Keep total column spans per row at or below 12.** Columns that exceed 12 wrap to a new line. This is intentional behavior but can cause unexpected layout shifts if the total is miscalculated.

5. **Use `h-100` on cards inside columns to equalize heights.** Without `h-100`, cards with different content lengths produce uneven heights. Columns in a row stretch to match the tallest, but their contents do not.

6. **Avoid mixing `col` (auto) with `col-*` (fixed) in the same row.** Auto columns consume remaining space, which interacts unpredictably with fixed-width columns. Use either auto or fixed widths consistently within a row.

7. **Nest grids only when necessary.** Each nested `row` resets the 12-column grid, but excessive nesting creates complex layouts that are difficult to maintain. Consider separate top-level rows instead.

8. **Use `col-12` as the mobile base for all columns that will be overridden.** This ensures predictable stacking. Even if a column should be narrow on mobile, starting with `col-12` and overriding upward is simpler than managing partial widths on small screens.

9. **Test column wrapping behavior at each breakpoint.** Columns that do not fit within 12 units wrap to the next line. On mobile, a row with two `col-6` elements fits perfectly. A row with three `col-6` elements wraps the third to a new line.

10. **Use `offset-*` classes to push columns across breakpoints.** `offset-md-3` on a `col-md-6` column centers it with a 3-column margin on the left. This is cleaner than adding empty spacer columns.

11. **Apply `g-0` to remove gutters when columns need to be edge-to-edge.** Image galleries and full-width sections often require zero gutters. Use `g-0` on the row to remove Bootstrap's default spacing.

12. **Use `order-*` classes to reorder columns across breakpoints.** `order-md-1` and `order-md-2` rearrange the visual order at `md` and above without changing the DOM order. This keeps the mobile stacking order accessible while enabling different desktop arrangements.

---

## Common Pitfalls

**Pitfall 1: Forgetting that columns without a `col-*` class expand to content width.**
An element without `col-*` inside a row does not behave as a grid column. It takes its content width and may break the row layout. Every direct child of a `row` needs a `col-*` class.

**Pitfall 2: Exceeding 12 columns in a row.**
`col-md-8` + `col-md-6` = 14, which wraps the second column. This is often unintentional. Verify that column spans add up to 12 or less at each breakpoint.

**Pitfall 3: Not accounting for gutter width in column calculations.**
Gutters (`g-*` classes) add padding to columns and negative margins to the row. If you apply manual margins or padding to columns, they may conflict with gutter calculations.

**Pitfall 4: Applying `col-*` classes to nested elements instead of direct row children.**
Bootstrap's grid requires `col-*` on the direct child of `row`. Applying `col-*` to a grandchild `<div>` inside a column `<div>` does not create a grid column.

**Pitfall 5: Using `offset-*` on the first column instead of adjusting column widths.**
`offset-md-2 col-md-8` and `col-md-8 offset-md-2` are equivalent, but the offset is unnecessary when a single centered column can be achieved with `col-md-8 mx-auto`.

**Pitfall 6: Ignoring column stacking order on mobile.**
Columns stack in DOM order on mobile. If the sidebar HTML comes before the main content HTML, the sidebar appears first on mobile. Use `order-*` classes to rearrange if the mobile order should differ.

**Pitfall 7: Not testing with odd numbers of columns.**
A three-column layout with `col-md-4` works perfectly (4+4+4=12). Five columns with `col-md-4` wraps two items to a second row, which may look unbalanced. Plan grid item counts to align with your column ratios.

---

## Accessibility Considerations

Column stacking on mobile follows DOM order, which governs screen reader and keyboard navigation order. Ensure that the DOM order reflects a logical content hierarchy. If the sidebar appears before the main content in the DOM, screen readers announce it first on all devices.

When reordering columns with `order-*` classes for visual purposes, remember that the DOM order does not change. Keyboard users still tab through elements in DOM order. Avoid creating large discrepancies between visual order and DOM order, as this disorients keyboard-only users.

Ensure that column content is readable when columns narrow. Text inside `col-md-2` on a 992px viewport is approximately 160px wide. Long words may overflow, and dense text becomes hard to read. Use `text-break` to prevent overflow and consider minimum readable widths for text-heavy columns.

---

## Responsive Behavior

Responsive columns follow Bootstrap's mobile-first breakpoint system. A `col-md-6` class has no effect below 768px. Below that breakpoint, the column takes either its `col-*` base width (if specified) or its natural content width (if not).

When multiple breakpoint classes are applied (`col-12 col-sm-6 col-md-4 col-lg-3`), each class overrides the previous at its breakpoint. At `sm`, `col-sm-6` overrides `col-12`. At `md`, `col-md-4` overrides `col-sm-6`. At `lg`, `col-lg-3` overrides `col-md-4`. The most specific active breakpoint always wins.

Columns that exceed 12 units in a row wrap to a new visual row within the same `row` container. This wrapping respects the current active column widths. A `row` with six `col-md-4` children displays two visual rows of three columns each at `md`.

The `row-cols-*` classes control automatic column wrapping by setting a fixed number of columns per row. `row-cols-md-3` ensures that at `md` and above, exactly three columns fit per row, with excess columns wrapping. This is different from setting `col-md-4` on each child, but the visual result is identical when the number of children is a multiple of the columns-per-row value.