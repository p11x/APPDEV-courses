---
tags: [bootstrap5, grid, ordering, columns, layout, responsive]
category: Bootstrap Fundamentals
difficulty: 2
estimated_time: 30 minutes
---

# Column Ordering

## Overview

Bootstrap 5 provides ordering utilities that control the visual sequence of columns within a Flexbox row, independent of their DOM order. This is essential for accessibility and SEO, where the DOM order should reflect the logical content hierarchy, while the visual order adapts to different screen sizes.

The ordering system includes three categories of classes: `.order-first` (Flexbox `order: -1`), `.order-last` (Flexbox `order: 13`), and `.order-{1-5}` (Flexbox `order: 1` through `order: 5`). All ordering classes accept breakpoint suffixes (`.order-md-last`, `.order-lg-2`) for responsive reordering.

By default, columns render in DOM order (`order: 0`). The `.order-first` class moves a column before all default-ordered columns. The `.order-last` class moves it after all default-ordered columns. The `.order-{1-5}` classes provide intermediate ordering between `first` and `last`.

Ordering is particularly powerful when combined with responsive breakpoints. A column can be first in the DOM (for mobile-first content priority) but visually last on desktop, achieving the common "sidebar below content on mobile, beside it on desktop" pattern.

## Basic Implementation

### order-first and order-last

Move a column to the beginning or end of the visual sequence.

```html
<div class="container">
  <div class="row">
    <div class="col-4 order-last">
      DOM first, visually last
    </div>
    <div class="col-4">
      Middle (DOM order)
    </div>
    <div class="col-4 order-first">
      DOM last, visually first
    </div>
  </div>
</div>
```

The first column moves to the end with `.order-last`. The last column moves to the front with `.order-first`.

### Numeric Ordering (order-1 through order-5)

Use `.order-{n}` to set explicit order values for fine-grained sequencing.

```html
<div class="container">
  <div class="row">
    <div class="col-4 order-3">Third visually</div>
    <div class="col-4 order-1">First visually</div>
    <div class="col-4 order-2">Second visually</div>
  </div>
</div>
```

Columns render in ascending order: 1, 2, 3. Unordered columns default to `order: 0` and appear before `order-1`.

### Responsive Ordering

Apply ordering only at specific breakpoints to change layout at different screen sizes.

```html
<div class="container">
  <div class="row">
    <div class="col-12 col-md-8 order-last order-md-first">
      Main content — below sidebar on mobile, left on desktop
    </div>
    <div class="col-12 col-md-4 order-first order-md-last">
      Sidebar — above content on mobile, right on desktop
    </div>
  </div>
</div>
```

On mobile, the sidebar appears first (`.order-first`) and the main content appears last (`.order-last`). At `md`, the order reverses.

### Default Order Behavior

Columns without order classes render in their natural DOM order, all with `order: 0`.

```html
<div class="container">
  <div class="row">
    <div class="col-4">First (DOM)</div>
    <div class="col-4">Second (DOM)</div>
    <div class="col-4">Third (DOM)</div>
  </div>
</div>
```

No reordering occurs. Visual sequence matches DOM sequence.

### Combining order-last with Specific Columns

Only apply ordering to the columns that need reordering. Other columns maintain their default position.

```html
<div class="container">
  <div class="row">
    <div class="col-3 bg-primary text-white p-3">Nav</div>
    <div class="col-6 bg-light p-3">Content</div>
    <div class="col-3 order-last bg-secondary text-white p-3">Ad</div>
  </div>
</div>
```

The ad column moves to the end. The nav and content columns maintain their DOM order.

## Advanced Variations

### Responsive Reordering for Mobile-First Layouts

A classic pattern: content first in DOM for mobile priority, then visually reorder on desktop.

```html
<div class="container">
  <div class="row">
    <div class="col-12 col-lg-8 order-last order-lg-first">
      <article class="p-3 bg-light">
        <h2>Article Title</h2>
        <p>Primary content appears first on mobile for SEO and accessibility.</p>
      </article>
    </div>
    <div class="col-12 col-lg-4 order-first order-lg-last">
      <aside class="p-3 bg-secondary text-white">
        <h3>Sidebar</h3>
        <p>Secondary content stacks above article on mobile.</p>
      </aside>
    </div>
  </div>
</div>
```

The `<article>` is first in DOM but last on mobile. At `lg`, the article moves left and the sidebar moves right.

### Multi-Column Reordering with Numeric Values

Reorder three or more columns with precise sequence control.

```html
<div class="container">
  <div class="row">
    <div class="col-4 order-2 order-md-1">
      <div class="p-3 bg-primary text-white">A — 2nd on mobile, 1st on md+</div>
    </div>
    <div class="col-4 order-1 order-md-3">
      <div class="p-3 bg-success text-white">B — 1st on mobile, 3rd on md+</div>
    </div>
    <div class="col-4 order-3 order-md-2">
      <div class="p-3 bg-info text-white">C — 3rd on mobile, 2nd on md+</div>
    </div>
  </div>
</div>
```

Each column has a different order at mobile versus desktop breakpoints.

### Combining Ordering with Offsets

Use ordering and offsets together for complex layout arrangements.

```html
<div class="container">
  <div class="row">
    <div class="col-4 order-2 offset-2">
      Second with offset gap
    </div>
    <div class="col-4 order-1">
      First visually
    </div>
  </div>
</div>
```

The offset maintains spacing after reordering. The second column has a 2-unit gap even though it renders after the first.

### Ordering with Auto Columns

Order `.col-auto` columns alongside fixed-width columns.

```html
<div class="container">
  <div class="row align-items-center">
    <div class="col order-last text-end">
      <h1 class="h4 mb-0">Site Title</h1>
    </div>
    <div class="col-auto order-first">
      <img src="logo.png" alt="Logo" width="40">
    </div>
    <div class="col-auto">
      <button class="btn btn-primary">CTA</button>
    </div>
  </div>
</div>
```

The logo moves to the first position while the title moves to the last, creating a logo-left, title-right navigation bar.

### Stacking Order with order-1 to order-5

Use numeric orders for layouts with multiple reorderable sections.

```html
<div class="container">
  <div class="row">
    <div class="col-md-4 order-3">Widget C (last)</div>
    <div class="col-md-4 order-1">Widget A (first)</div>
    <div class="col-md-4 order-2">Widget B (middle)</div>
  </div>
  <div class="row mt-3">
    <div class="col-md-4 order-2">Widget E</div>
    <div class="col-md-4 order-3">Widget F (last)</div>
    <div class="col-md-4 order-1">Widget D (first)</div>
  </div>
</div>
```

Each row independently orders its columns using numeric values.

## Best Practices

1. **Keep DOM order logical** — The DOM should reflect content hierarchy. Ordering is a visual enhancement, not a structural one.
2. **Use `order-first` and `order-last` for simple swaps** — These are the most readable and maintainable ordering classes.
3. **Apply responsive ordering to match layout needs** — Use `.order-md-last` rather than `.order-last` when reordering should only happen on larger screens.
4. **Do not reorder essential navigation landmarks** — Screen reader users rely on logical DOM order. Reordering `<nav>` elements can create confusion.
5. **Limit reordering to 2-3 columns** — Complex reordering across many columns produces fragile, hard-to-debug layouts.
6. **Combine ordering with responsive column widths** — Reordering is most effective when columns also change size at breakpoints.
7. **Reset ordering at breakpoints when needed** — Use `.order-md-0` to remove an order applied at a smaller breakpoint.
8. **Use `order-first` for mobile-priority content** — Place important content first in the DOM, then reorder it on desktop.
9. **Test with screen readers after reordering** — Verify that the reading sequence makes sense even if the visual order differs from the DOM order.
10. **Avoid negative order values** — Bootstrap does not provide classes for negative order. Use `.order-first` (which sets `order: -1`) instead.
11. **Document complex ordering patterns** — When multiple breakpoints reorder columns differently, add comments to clarify the intended behavior.

## Common Pitfalls

1. **Reordering without considering screen reader experience** — Visual reordering does not affect DOM order. Screen readers announce content in DOM order, which may be confusing if it diverges significantly from the visual layout.

2. **Using `order-last` on all columns** — If every column has `.order-last`, they all share `order: 13` and fall back to DOM order. The class has no differentiating effect.

3. **Forgetting that ordering applies within the row context** — `order-*` only affects the column's position within its parent `.row`. It does not move the column to a different row.

4. **Not resetting responsive ordering** — `.order-sm-first` persists at `md`, `lg`, `xl`, and `xxl`. If the ordering should only apply at `sm`, add `.order-md-0` to reset.

5. **Assuming order changes DOM position** — Ordering is purely visual via CSS Flexbox. The tab order, focus order, and screen reader order all follow the original DOM sequence.

6. **Combining too many order classes on one element** — Applying `.order-1 .order-md-3 .order-lg-last` to a single element can produce unpredictable results at intermediate breakpoints.

7. **Using ordering when restructuring the DOM is better** — If the DOM order is fundamentally wrong for the layout, fix the HTML rather than applying extensive CSS ordering.

## Accessibility Considerations

- Maintain a logical DOM order that reflects the content's semantic hierarchy. Use ordering utilities only for visual enhancements that do not compromise the reading sequence.
- When reordering creates a visual layout that differs from the DOM order, ensure that the content still makes sense when read in DOM order by screen readers.
- Provide skip links or landmark regions so keyboard users can navigate directly to reordered content areas.
- Use `aria-label` on reordered sections to help assistive technology users understand the page structure despite visual rearrangement.
- Test the page with a screen reader after applying responsive reordering to verify that the announced sequence is coherent at each breakpoint.

## Responsive Behavior

Ordering classes with breakpoint suffixes activate only at and above the specified breakpoint. `.order-md-last` applies at ≥768px. Below that threshold, the column retains its default `order: 0` or any smaller breakpoint ordering.

When a row's columns stack vertically on mobile (`.col-12`), ordering still affects their visual sequence — a column with `.order-first` appears at the top of the stack, and `.order-last` appears at the bottom.

Ordering interacts with column wrapping. When columns exceed 12 units and wrap to a new line, ordering affects where each column appears within its wrapped line, not across lines.
