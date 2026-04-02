---
title: Visibility Utilities
category: Bootstrap Fundamentals
difficulty: 1
time: 15 min
tags: bootstrap5, visibility, display, collapse, hidden, utilities
---

## Overview

Bootstrap 5 visibility utilities control whether elements are visible on the page without removing them from the document flow. The `visible` class makes an element visible, `invisible` hides it while preserving its space in the layout, and `collapse` hides the element and removes it from the layout flow similar to `display: none`. Understanding the difference between these approaches is crucial for creating dynamic interfaces where elements need to appear, disappear, or be conditionally rendered.

## Basic Implementation

The `visible` and `invisible` classes toggle visibility while maintaining layout space.

```html
<!-- Visibility toggle -->
<div class="visible">This element is visible</div>
<div class="invisible">This element is invisible but still occupies space</div>

<!-- Comparing visibility approaches -->
<div class="row">
  <div class="col-4">
    <div class="p-3 bg-primary text-white">Visible column</div>
  </div>
  <div class="col-4 invisible">
    <div class="p-3 bg-secondary text-white">Invisible column (space preserved)</div>
  </div>
  <div class="col-4">
    <div class="p-3 bg-success text-white">Visible column</div>
  </div>
</div>
```

The `collapse` utility removes the element from the layout entirely.

```html
<!-- Collapse utility vs invisible -->
<div class="p-3 mb-2 bg-light">Before element</div>
<div class="collapse">This element is collapsed (no space taken)</div>
<div class="p-3 bg-light">After element (directly follows "Before" when collapsed)</div>

<!-- Invisible: space is preserved -->
<div class="p-3 mb-2 bg-light">Before element</div>
<div class="invisible">This element is invisible (space still taken)</div>
<div class="p-3 bg-light">After element (below the invisible element's space)</div>
```

## Advanced Variations

Visibility utilities can be combined with responsive breakpoints and JavaScript for dynamic show/hide behavior.

```html
<!-- Responsive visibility -->
<div class="visible-md-block invisible-sm">
  Only visible on medium screens and above
</div>

<!-- Using collapse for expandable sections -->
<div>
  <button class="btn btn-primary" type="button" data-bs-toggle="collapse"
          data-bs-target="#detailsPanel">
    Toggle Details
  </button>
  <div class="collapse mt-2" id="detailsPanel">
    <div class="card card-body">
      This content expands and collapses with animation.
    </div>
  </div>
</div>
```

Visibility can be toggled dynamically with JavaScript or conditional rendering in frameworks.

```html
<!-- Screen reader only content -->
<div class="visually-hidden">
  This content is only visible to screen readers
</div>

<!-- Visually hidden but focusable -->
<div class="visually-hidden-focusable">
  Hidden until focused via keyboard navigation
</div>

<!-- Toggle visibility with display utilities -->
<div class="d-none d-md-block">
  Hidden below medium breakpoint using display utility
</div>
```

## Best Practices

1. **Use `invisible` to maintain layout** - When you need to hide an element without shifting surrounding content, use `invisible`.
2. **Use `collapse` for toggled content** - The `collapse` utility with animation is ideal for expandable sections and accordions.
3. **Prefer `d-none` for responsive hiding** - Display utilities like `d-none d-md-block` are more commonly used than visibility for responsive show/hide.
4. **Use `visually-hidden` for screen readers** - Hide content visually while keeping it accessible to assistive technologies.
5. **Avoid `invisible` on interactive elements** - Invisible buttons and links can still be clicked, causing unexpected behavior.
6. **Combine with transitions** - Add CSS transitions to visibility changes for smooth animations.
7. **Use `data-bs-toggle` for Bootstrap components** - Collapse panels, dropdowns, and modals have built-in visibility management.
8. **Test keyboard navigation** - Ensure hidden elements do not receive keyboard focus when they should be completely removed.
9. **Consider animation timing** - The `collapse` class includes animation. Avoid using it for instant visibility changes.
10. **Use ARIA attributes** - Pair visibility states with `aria-hidden`, `aria-expanded`, and `aria-visible` for proper accessibility signaling.

## Common Pitfalls

1. **Confusing `invisible` with `d-none`** - `invisible` preserves space; `d-none` removes the element from flow entirely. Using the wrong one breaks layout.
2. **Hiding focusable elements** - Elements hidden with `invisible` remain focusable. Use `d-none` or `tabindex="-1"` to remove from tab order.
3. **Not toggling ARIA attributes** - Showing/hiding content without updating `aria-expanded` or `aria-hidden` confuses screen reader users.
4. **Overriding visibility unintentionally** - Later CSS or utility classes can override visibility settings. Use `!important` utilities if needed.
5. **Forgetting collapse animation requirements** - The `collapse` class requires Bootstrap's JavaScript plugin. Without it, the class acts as `display: none` without animation.

## Accessibility Considerations

Visibility utilities have direct accessibility implications. The `visually-hidden` class is specifically designed to provide content to screen readers while hiding it from sight. Use it for skip links, form labels that are visually represented by icons, and supplementary instructions. The `visually-hidden-focusable` class is essential for skip navigation links that appear when focused. Always pair visibility changes with appropriate ARIA attributes: `aria-hidden="true"` for elements hidden from assistive technology, and `aria-expanded` for toggleable content regions.

## Responsive Behavior

Bootstrap provides responsive display utilities (`d-none`, `d-block`, `d-md-block`) that are commonly used for responsive visibility. The `invisible` class does not have responsive variants by default. For responsive visibility, use display utilities which control both rendering and visibility. For example, `d-none d-lg-block` hides an element below the `lg` breakpoint and shows it at `lg` and above. The `collapse` utility can be combined with responsive classes for viewport-dependent expandable sections.
