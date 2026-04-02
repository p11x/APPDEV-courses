---
tags:
  - bootstrap
  - utilities
  - flexbox
  - layout
  - alignment
category: Bootstrap Fundamentals
difficulty: 2
estimated_time: 40 minutes
---

# Flexbox Utilities

## Overview

Bootstrap 5 flexbox utilities provide a complete set of classes for controlling flex container and flex item behavior without writing any custom CSS. These utilities cover direction, wrapping, alignment, sizing, ordering, and responsive breakpoints — making flexbox layouts trivial to construct and maintain.

Flexbox is a one-dimensional layout model: it arranges items along either a row or a column. Bootstrap's flex utilities map directly to CSS flexbox properties with a consistent naming convention that supports responsive prefixes.

The utility categories include:

- **Direction**: `flex-row`, `flex-row-reverse`, `flex-column`, `flex-column-reverse`
- **Wrap**: `flex-wrap`, `flex-nowrap`, `flex-wrap-reverse`
- **Justify content**: `justify-content-start`, `justify-content-end`, `justify-content-center`, `justify-content-between`, `justify-content-around`, `justify-content-evenly`
- **Align items**: `align-items-start`, `align-items-end`, `align-items-center`, `align-items-baseline`, `align-items-stretch`
- **Align self**: `align-self-start`, `align-self-end`, `align-self-center`, `align-self-baseline`, `align-self-stretch`, `align-self-auto`
- **Align content**: `align-content-start`, `align-content-end`, `align-content-center`, `align-content-between`, `align-content-around`, `align-content-stretch`
- **Fill**: `flex-fill` — makes items grow to fill available space equally
- **Grow/Shrink**: `flex-grow-0`, `flex-grow-1`, `flex-shrink-0`, `flex-shrink-1`
- **Order**: `order-0` through `order-5`, `order-first`, `order-last`

All flex utilities support breakpoint prefixes (`sm`, `md`, `lg`, `xl`, `xxl`) for responsive flex behavior.

## Basic Implementation

**Enabling flex and setting direction:**

```html
<!-- Default row direction -->
<div class="d-flex">
  <div class="p-2 bg-light border">Item 1</div>
  <div class="p-2 bg-light border">Item 2</div>
  <div class="p-2 bg-light border">Item 3</div>
</div>

<!-- Column direction -->
<div class="d-flex flex-column">
  <div class="p-2 bg-light border">Item 1</div>
  <div class="p-2 bg-light border">Item 2</div>
  <div class="p-2 bg-light border">Item 3</div>
</div>

<!-- Reversed row -->
<div class="d-flex flex-row-reverse">
  <div class="p-2 bg-light border">Item 1 (right)</div>
  <div class="p-2 bg-light border">Item 2</div>
  <div class="p-2 bg-light border">Item 3 (left)</div>
</div>
```

**Justify content (main axis alignment):**

```html
<div class="d-flex justify-content-start mb-2">
  <div class="p-2 bg-light border">Start</div>
</div>
<div class="d-flex justify-content-center mb-2">
  <div class="p-2 bg-light border">Center</div>
</div>
<div class="d-flex justify-content-end mb-2">
  <div class="p-2 bg-light border">End</div>
</div>
<div class="d-flex justify-content-between mb-2">
  <div class="p-2 bg-light border">Between</div>
  <div class="p-2 bg-light border">Between</div>
  <div class="p-2 bg-light border">Between</div>
</div>
<div class="d-flex justify-content-around mb-2">
  <div class="p-2 bg-light border">Around</div>
  <div class="p-2 bg-light border">Around</div>
  <div class="p-2 bg-light border">Around</div>
</div>
<div class="d-flex justify-content-evenly">
  <div class="p-2 bg-light border">Evenly</div>
  <div class="p-2 bg-light border">Evenly</div>
  <div class="p-2 bg-light border">Evenly</div>
</div>
```

**Align items (cross axis alignment):**

```html
<div class="d-flex align-items-start mb-2" style="height: 100px; background: #f8f9fa;">
  <div class="p-2 border">Start</div>
</div>
<div class="d-flex align-items-center mb-2" style="height: 100px; background: #f8f9fa;">
  <div class="p-2 border">Center</div>
</div>
<div class="d-flex align-items-end mb-2" style="height: 100px; background: #f8f9fa;">
  <div class="p-2 border">End</div>
</div>
<div class="d-flex align-items-stretch mb-2" style="height: 100px; background: #f8f9fa;">
  <div class="p-2 border">Stretch (fills height)</div>
</div>
```

**Flex wrap:**

```html
<div class="d-flex flex-wrap gap-2">
  <div class="p-2 bg-light border" style="width: 200px;">Item</div>
  <div class="p-2 bg-light border" style="width: 200px;">Item</div>
  <div class="p-2 bg-light border" style="width: 200px;">Item</div>
  <div class="p-2 bg-light border" style="width: 200px;">Item</div>
  <div class="p-2 bg-light border" style="width: 200px;">Item</div>
</div>
```

## Advanced Variations

**Align self for individual item override:**

```html
<div class="d-flex align-items-start" style="height: 120px; background: #f8f9fa;">
  <div class="p-2 border">Start (default)</div>
  <div class="p-2 border align-self-center">Center (override)</div>
  <div class="p-2 border align-self-end">End (override)</div>
  <div class="p-2 border align-self-stretch">Stretch (override)</div>
</div>
```

`align-self` overrides the container's `align-items` for a specific child, allowing mixed alignment within the same flex container.

**Flex fill for equal-width columns:**

```html
<div class="d-flex">
  <div class="flex-fill p-2 bg-light border">Equal width</div>
  <div class="flex-fill p-2 bg-light border">Equal width</div>
  <div class="flex-fill p-2 bg-light border">Equal width</div>
</div>
```

`flex-fill` applies `flex: 1 1 auto`, causing each item to grow and shrink to fill available space equally.

**Flex grow and shrink:**

```html
<!-- Only the middle item grows -->
<div class="d-flex">
  <div class="p-2 bg-light border flex-shrink-0">Fixed width</div>
  <div class="p-2 bg-light border flex-grow-1">Grows to fill space</div>
  <div class="p-2 bg-light border flex-shrink-0">Fixed width</div>
</div>

<!-- Prevent shrinking on overflow -->
<div class="d-flex" style="width: 300px;">
  <div class="p-2 bg-light border flex-shrink-0" style="width: 200px;">
    Does not shrink below 200px
  </div>
  <div class="p-2 bg-light border flex-shrink-0" style="width: 200px;">
    Does not shrink below 200px
  </div>
</div>
```

**Order utilities:**

```html
<div class="d-flex">
  <div class="order-3 p-2 bg-light border">Third visually (1st in DOM)</div>
  <div class="order-1 p-2 bg-light border">First visually (2nd in DOM)</div>
  <div class="order-2 p-2 bg-light border">Second visually (3rd in DOM)</div>
</div>

<div class="d-flex">
  <div class="order-last p-2 bg-light border">Always last</div>
  <div class="p-2 bg-light border">Default order</div>
  <div class="order-first p-2 bg-light border">Always first</div>
</div>
```

Order values range from `order-0` through `order-5`. `order-first` places an item before `order-0`, and `order-last` places it after `order-5`.

**Align content for wrapped multi-line containers:**

```html
<div class="d-flex flex-wrap align-content-between gap-2"
     style="height: 300px; background: #f8f9fa;">
  <div class="p-2 border bg-white" style="width: 45%;">Item</div>
  <div class="p-2 border bg-white" style="width: 45%;">Item</div>
  <div class="p-2 border bg-white" style="width: 45%;">Item</div>
  <div class="p-2 border bg-white" style="width: 45%;">Item</div>
</div>
```

`align-content` only has an effect when there are multiple lines of flex items (i.e., when `flex-wrap` is active and items have wrapped).

**Responsive flex behavior:**

```html
<!-- Stack on mobile, row on desktop -->
<div class="d-flex flex-column flex-md-row gap-3">
  <div class="flex-fill p-3 bg-light border">Column 1</div>
  <div class="flex-fill p-3 bg-light border">Column 2</div>
  <div class="flex-fill p-3 bg-light border">Column 3</div>
</div>

<!-- Center on mobile, space-between on desktop -->
<div class="d-flex flex-column flex-sm-row justify-content-center justify-content-md-between gap-2">
  <button class="btn btn-outline-secondary">Cancel</button>
  <button class="btn btn-primary">Submit</button>
</div>

<!-- Full-width on mobile, auto-width on desktop -->
<div class="d-flex flex-column flex-md-row gap-2">
  <input type="text" class="form-control flex-md-grow-1" placeholder="Search...">
  <button class="btn btn-primary w-100 w-md-auto">Go</button>
</div>
```

**Navigation bar with flex:**

```html
<nav class="d-flex justify-content-between align-items-center p-3 bg-light rounded">
  <div class="fw-bold">Brand</div>
  <div class="d-flex gap-3">
    <a href="#" class="text-decoration-none">Home</a>
    <a href="#" class="text-decoration-none">About</a>
    <a href="#" class="text-decoration-none">Contact</a>
  </div>
  <div class="d-flex gap-2">
    <button class="btn btn-outline-primary btn-sm">Login</button>
    <button class="btn btn-primary btn-sm">Sign Up</button>
  </div>
</nav>
```

**Card layout with flex fill:**

```html
<div class="d-flex flex-column flex-md-row gap-3">
  <div class="flex-fill d-flex flex-column p-3 border rounded">
    <h5>Free Plan</h5>
    <p class="flex-grow-1">Basic features for personal use.</p>
    <button class="btn btn-outline-primary mt-auto">Choose</button>
  </div>
  <div class="flex-fill d-flex flex-column p-3 border border-primary rounded bg-primary-subtle">
    <h5 class="text-primary-emphasis">Pro Plan</h5>
    <p class="flex-grow-1 text-primary-emphasis">Advanced features for teams.</p>
    <button class="btn btn-primary mt-auto">Choose</button>
  </div>
  <div class="flex-fill d-flex flex-column p-3 border rounded">
    <h5>Enterprise</h5>
    <p class="flex-grow-1">Custom solutions for large organizations.</p>
    <button class="btn btn-outline-primary mt-auto">Contact</button>
  </div>
</div>
```

The `flex-grow-1` on the paragraph combined with `mt-auto` on the button ensures buttons align to the bottom regardless of content length — a common card layout pattern.

**Sidebar layout with flex:**

```html
<div class="d-flex flex-column flex-md-row" style="min-height: 400px;">
  <nav class="flex-shrink-0 p-3 bg-light" style="width: 250px;">
    <h5>Sidebar</h5>
    <ul class="list-unstyled">
      <li><a href="#">Dashboard</a></li>
      <li><a href="#">Settings</a></li>
    </ul>
  </nav>
  <main class="flex-grow-1 p-4">
    <h2>Main Content</h2>
    <p>Main content area that fills remaining space.</p>
  </main>
</div>
```

## Best Practices

1. **Always apply `d-flex` to the parent** before using any flex utility. Flex properties on children have no effect unless the parent is a flex container.

2. **Use `gap` for spacing between flex items** instead of adding margins to individual children. `gap` distributes space evenly and does not affect the outer edges of the container.

3. **Use `justify-content` for main axis alignment** and `align-items` for cross axis alignment. In a row-direction flex container, `justify-content` controls horizontal distribution and `align-items` controls vertical alignment.

4. **Apply `flex-fill` for equal-width columns** instead of setting `width: 33.33%` on each item. `flex-fill` adapts to content while maintaining equal distribution.

5. **Use `flex-grow-1` on a single element** to make it fill remaining space. Combine with `flex-shrink-0` on siblings to keep them at their natural size.

6. **Use responsive flex direction** for mobile-first layouts: `flex-column flex-md-row` stacks items vertically on mobile and horizontally on desktop.

7. **Apply `order-*` utilities for visual reordering** without changing DOM order. This is useful for moving sidebars below content on mobile.

8. **Use `flex-wrap` on containers with many items** to prevent overflow. Items that cannot fit on one line will wrap to the next.

9. **Use `align-self-*` to override container alignment** for specific items. This is cleaner than creating a separate flex container.

10. **Prefer `justify-content-between` over manual margin** for distributing items to opposite ends of a container. It is more maintainable and responsive.

11. **Use `flex-shrink-0` to prevent items from shrinking** below their content or specified width. This is essential for fixed-width sidebars and image containers.

12. **Combine flex utilities with Bootstrap's grid** for complex layouts. Use flex for component-level alignment (navbars, card footers, button groups) and grid for page-level structure.

13. **Use `align-items-center` with a height constraint** for vertical centering. The container needs a defined height (explicit, `min-height`, or inherited from parent).

14. **Apply `flex-column` and `justify-content-between`** to create sticky footer patterns where the footer stays at the bottom regardless of content height.

## Common Pitfalls

**1. Forgetting `d-flex` on the parent.** Flex utilities on children (`flex-grow-1`, `justify-content-between`, etc.) have no effect unless the parent has `display: flex`. This is the most common mistake.

**2. Confusing `justify-content` with `align-items`.** `justify-content` aligns along the main axis (horizontal in `flex-row`, vertical in `flex-column`). `align-items` aligns along the cross axis. Swapping them produces unexpected results.

**3. Using `align-items-center` without a height constraint.** Vertical centering requires the container to have more height than its content. Without `height`, `min-height`, or a parent that provides height, centering is imperceptible.

**4. Expecting `align-content` to work without `flex-wrap`.** `align-content` only applies when flex items wrap onto multiple lines. On a single-line flex container, it has no effect.

**5. Using `order-*` for meaningful content reordering.** `order` changes visual order but not DOM order. Screen readers and keyboard navigation follow DOM order. Do not use `order` to move important content that should be encountered earlier by assistive technology.

**6. Applying `flex-grow-1` to all children expecting equal widths.** While this works, `flex-fill` is the intended utility for equal distribution. `flex-grow-1` alone does not set `flex-basis: 0` like `flex-fill` does, which can produce different results with variable content sizes.

**7. Forgetting that `flex-direction: column` swaps axes.** In `flex-column`, the main axis is vertical and the cross axis is horizontal. This means `justify-content` controls vertical spacing and `align-items` controls horizontal alignment.

**8. Not using `gap` with `flex-wrap`.** When items wrap, margins on children create inconsistent spacing at line breaks. `gap` provides uniform spacing between all items, including across wrap boundaries.

**9. Overriding `flex-shrink` on items inside a constrained container.** If a flex container is smaller than the total width of its items and all items have `flex-shrink-0`, content will overflow horizontally. At least one item should be allowed to shrink.

**10. Mixing flex and grid utilities on the same element.** A container should use either `d-flex` or `d-grid`, not both. The last one declared wins, which can create confusing and unpredictable layouts.

**11. Using flex for two-dimensional layouts.** Flexbox is one-dimensional (row or column). For layouts requiring both row and column control simultaneously (e.g., a dashboard grid), use `d-grid` instead.

**12. Applying `align-items-stretch` expecting it to affect items with explicit heights.** `stretch` cannot override explicit `height` or `min-height` set on children. Remove explicit dimensions for stretch to work.

## Accessibility Considerations

**DOM order vs. visual order:** Flexbox `order` and `flex-direction: row-reverse`/`column-reverse` change visual presentation without changing DOM order. Screen readers, keyboard navigation, and sequential focus order all follow DOM order. This means:

- Do not use `order-last` to visually move a close button to the right if it should be the last element in the focus sequence.
- Do not use `flex-row-reverse` if the reversed order creates a confusing reading sequence for screen reader users.

**Focus management in flex layouts:** When items are reordered with `order-*`, the tab order does not change. Users tabbing through a flex container may focus items in a sequence that does not match their visual arrangement. This is disorienting for keyboard users.

**Responsive stacking and reading order:** When using `flex-column` on mobile and `flex-row` on desktop, ensure the DOM order makes sense in both layouts. If the sidebar is in the DOM before the main content, it will be read first on mobile (where it stacks above) — which may or may not be desirable.

**Sufficient spacing for motor impairments:** Flex `gap` and padding should provide enough space between interactive elements. WCAG 2.5.8 (Target Size) requires at least 24x24px for each target, or sufficient spacing between smaller targets.

**Announcing layout changes:** When responsive flex direction changes happen (e.g., stacking to side-by-side), screen readers are not aware of the visual change. Ensure content makes sense regardless of layout direction.

## Responsive Behavior

Responsive flex utilities are the core of Bootstrap's mobile-first layout system. Every flex utility supports breakpoint prefixes.

**Responsive direction:**

```html
<!-- Stack on mobile, row on tablet+ -->
<div class="d-flex flex-column flex-md-row gap-3">
  <div class="flex-fill p-3 bg-light border">Item 1</div>
  <div class="flex-fill p-3 bg-light border">Item 2</div>
  <div class="flex-fill p-3 bg-light border">Item 3</div>
</div>
```

**Responsive wrapping:**

```html
<!-- No wrap on mobile, wrap on desktop -->
<div class="d-flex flex-nowrap flex-lg-wrap gap-2">
  <div class="p-2 bg-light border" style="min-width: 300px;">Item</div>
  <div class="p-2 bg-light border" style="min-width: 300px;">Item</div>
  <div class="p-2 bg-light border" style="min-width: 300px;">Item</div>
</div>
```

**Responsive alignment:**

```html
<!-- Centered on mobile, space-between on desktop -->
<div class="d-flex flex-column flex-sm-row justify-content-center justify-content-md-between align-items-center gap-2 p-3 bg-light rounded">
  <div class="fw-bold">Logo</div>
  <nav class="d-flex gap-3">
    <a href="#">Link 1</a>
    <a href="#">Link 2</a>
  </nav>
  <button class="btn btn-primary btn-sm">Action</button>
</div>
```

**Responsive order:**

```html
<div class="d-flex flex-column flex-md-row">
  <aside class="order-2 order-md-1 p-3 bg-light" style="width: 250px;">
    Sidebar — appears below content on mobile, beside it on desktop.
  </aside>
  <main class="order-1 order-md-2 flex-grow-1 p-3">
    Main content — appears first on mobile, second on desktop.
  </main>
</div>
```

**Responsive grow behavior:**

```html
<div class="d-flex flex-column flex-md-row gap-2">
  <input type="text" class="form-control flex-md-grow-1" placeholder="Search...">
  <select class="form-select" style="width: auto;">
    <option>All</option>
    <option>Active</option>
  </select>
  <button class="btn btn-primary flex-shrink-0">Search</button>
</div>
```

On mobile, the input, select, and button stack vertically. On `md+`, they sit in a row, and the input grows to fill available space while the select and button maintain their natural width.

**Responsive card grid with flex:**

```html
<div class="d-flex flex-column flex-sm-row flex-lg-wrap gap-3">
  <div class="flex-fill p-3 border rounded" style="min-width: 280px;">
    <h5>Card 1</h5>
    <p>Responsive card that wraps at lg breakpoint.</p>
  </div>
  <div class="flex-fill p-3 border rounded" style="min-width: 280px;">
    <h5>Card 2</h5>
    <p>Responsive card that wraps at lg breakpoint.</p>
  </div>
  <div class="flex-fill p-3 border rounded" style="min-width: 280px;">
    <h5>Card 3</h5>
    <p>Responsive card that wraps at lg breakpoint.</p>
  </div>
</div>
```

These responsive patterns form the basis of nearly every Bootstrap layout. By combining `d-flex` with responsive direction, alignment, and sizing utilities, you can create complex adaptive layouts that work across all device sizes without writing a single line of custom CSS.
