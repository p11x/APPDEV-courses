---
title: "Responsive Visibility Patterns"
lesson: "01_06_10"
difficulty: "1"
topics: ["show-hide", "d-print", "visibility", "display-utilities", "responsive"]
estimated_time: "20 minutes"
---

# Responsive Visibility Patterns

## Overview

Bootstrap provides display utilities with responsive prefixes to show or hide elements at specific breakpoints. The `d-{breakpoint}-{value}` classes control `display` property across viewports, while `d-print-*` utilities manage print visibility. Combining these classes creates visibility matrices that show different content on mobile, tablet, and desktop. Understanding these patterns avoids duplicate content, improves mobile performance, and ensures appropriate UI at every screen size.

Bootstrap 5 removed the standalone `.hidden-*` and `.visible-*` classes from v4, replacing them entirely with the more flexible display utility system.

## Basic Implementation

### Show/Hide at Breakpoints

```html
<!-- Hidden on mobile, visible on medium and up -->
<div class="d-none d-md-block">Desktop-only content</div>

<!-- Visible on mobile, hidden on medium and up -->
<div class="d-block d-md-none">Mobile-only content</div>

<!-- Always visible -->
<div class="d-block">Always shown</div>

<!-- Always hidden -->
<div class="d-none">Never shown (but in DOM)</div>
```

### Print Visibility

```html
<!-- Only visible when printing -->
<div class="d-none d-print-block">Print-only header</div>

<!-- Hidden when printing -->
<div class="d-print-none">Screen-only navigation</div>
```

### Responsive Flex Display

```html
<!-- Stacked on mobile, side-by-side on desktop -->
<div class="d-flex d-md-flex flex-column flex-md-row">
  <div>Column 1</div>
  <div>Column 2</div>
</div>
```

## Advanced Variations

### Full Visibility Matrix

```html
<!-- Mobile only (hidden md+) -->
<div class="d-block d-md-none">Shows: xs, sm | Hides: md, lg, xl, xxl</div>

<!-- Tablet only (hidden xs, hidden lg+) -->
<div class="d-none d-md-block d-lg-none">Shows: md | Hides: xs, sm, lg, xl, xxl</div>

<!-- Desktop only (hidden md-) -->
<div class="d-none d-lg-block">Shows: lg, xl, xxl | Hides: xs, sm, md</div>

<!-- Mobile + Tablet (hidden lg+) -->
<div class="d-block d-lg-none">Shows: xs, sm, md | Hides: lg, xl, xxl</div>

<!-- Tablet + Desktop (hidden xs, sm) -->
<div class="d-none d-md-block">Shows: md, lg, xl, xxl | Hides: xs, sm</div>
```

### Responsive Grid Display Toggle

```html
<!-- Grid on desktop, list on mobile -->
<div class="d-block d-md-none">
  <!-- Mobile: stacked list -->
  <div class="list-group">
    <div class="list-group-item">Item 1</div>
    <div class="list-group-item">Item 2</div>
  </div>
</div>
<div class="d-none d-md-block">
  <!-- Desktop: grid -->
  <div class="row">
    <div class="col-md-6">Item 1</div>
    <div class="col-md-6">Item 2</div>
  </div>
</div>
```

### Combined Print and Screen Utilities

```html
<div class="d-none d-md-block d-print-none">
  <!-- Desktop screen only, not in print -->
  <nav>Complex navigation with dropdowns</nav>
</div>
<div class="d-none d-print-block">
  <!-- Print only -->
  <p>See website at example.com for interactive version</p>
</div>
```

### Visibility with Other Display Values

```html
<!-- Flex on desktop, hidden on mobile -->
<div class="d-none d-lg-flex align-items-center gap-3">
  <span>Nav item 1</span>
  <span>Nav item 2</span>
</div>

<!-- Grid on tablet+, hidden on mobile -->
<div class="d-none d-md-grid" style="grid-template-columns: repeat(3, 1fr);">
  <div>Cell 1</div>
  <div>Cell 2</div>
  <div>Cell 3</div>
</div>
```

## Best Practices

1. **Use `d-none` combined with `d-{breakpoint}-block` for responsive show/hide** - The standard pattern.
2. **Prefer hiding content over duplicating it** - Reduces HTML bloat and maintenance burden.
3. **Use `d-print-none` for navigation and interactive elements in print** - They serve no purpose on paper.
4. **Use `d-print-block` for print-optimized content** - URLs, QR codes, simplified layouts.
5. **Test visibility at exact breakpoint boundaries** - 576px, 768px, 992px, etc.
6. **Use `visually-hidden` for screen-reader-only content** - `d-none` hides from screen readers too.
7. **Use `visually-hidden-focusable` for skip links** - Hidden visually but focusable by keyboard.
8. **Combine with `d-print-flex` or `d-print-grid` for print layouts** - Not just block/none.
9. **Avoid hiding essential content on mobile** - All users need access to core functionality.
10. **Audit hidden content regularly** - Ensure `d-none` elements are truly unnecessary on certain viewports.

## Common Pitfalls

1. **Using `d-none` to hide content from screen readers** - Use `aria-hidden="true"` instead; `d-none` removes from accessibility tree.
2. **Hiding the only copy of important content on mobile** - Mobile users need the same information.
3. **Forgetting `d-print-*` utilities** - Printed pages show navigation, modals, and interactive UI.
4. **Duplicating content for mobile and desktop** - Instead of two elements with different `d-*` classes, use responsive layout (flex, grid).
5. **Not testing at 200% browser zoom** - Hidden elements may become necessary when viewport shrinks due to zoom.

## Accessibility Considerations

`d-none` sets `display: none`, which removes elements from both visual rendering AND the accessibility tree. Screen readers cannot access `d-none` content. Use `visually-hidden` for content that should be announced by screen readers but hidden visually. Use `aria-hidden="true"` for decorative elements. When using responsive visibility to show/hide navigation, ensure the visible navigation at each breakpoint is fully accessible and keyboard-navigable.

## Responsive Behavior

Responsive visibility is the core mechanism for showing different UIs at different breakpoints. The pattern `d-none d-{bp}-block` hides below the breakpoint and shows at and above it. This powers mobile hamburger menus (nav links: `d-none d-lg-flex`), responsive tables (full table: `d-none d-md-table`), and context-appropriate CTAs (desktop CTA: `d-none d-lg-inline-block`). Every responsive visibility utility generates a media query matching Bootstrap's breakpoint system.
