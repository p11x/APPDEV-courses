---
title: "Responsive Visibility"
description: "Show and hide elements at specific breakpoints with Bootstrap 5 display utilities"
difficulty: 1
estimated_time: "15 minutes"
tags: ["responsive", "display", "visibility", "d-none", "breakpoints"]
---

# Responsive Visibility

## Overview

Bootstrap 5 controls element visibility across breakpoints using display utilities with responsive prefixes. The `d-none` class hides elements, while `d-{breakpoint}-block`, `d-{breakpoint}-flex`, and similar classes show them from that breakpoint upward. Combined with print utilities like `d-print-none`, this system provides complete control over what appears on each screen size.

This approach replaces JavaScript-based show/hide logic and media query CSS for most responsive visibility needs. It is essential for mobile menus, responsive navigation, showing/hiding sidebar content, and optimizing layouts for different devices.

## Basic Implementation

### Hide at Specific Breakpoints

```html
<!-- Hidden only on extra small screens -->
<div class="d-none d-sm-block bg-light p-3">
  Visible on sm and above, hidden on xs
</div>

<!-- Hidden on medium and above -->
<div class="d-block d-md-none bg-light p-3">
  Visible only on xs/sm (mobile)
</div>

<!-- Hidden on large and above -->
<div class="d-block d-lg-none bg-light p-3">
  Visible only below lg breakpoint
</div>
```

### Show Only at Specific Breakpoints

```html
<!-- Show only on medium screens -->
<div class="d-none d-md-block d-lg-none bg-warning p-3">
  Visible only on md screens
</div>

<!-- Show only on extra small -->
<div class="d-block d-sm-none bg-info text-white p-3">
  Visible only on xs (below 576px)
</div>
```

### Mobile vs Desktop Navigation

```html
<!-- Mobile hamburger menu -->
<nav class="d-flex d-lg-none">
  <button class="btn btn-outline-dark" data-bs-toggle="offcanvas" data-bs-target="#mobileMenu">
    ☰ Menu
  </button>
</nav>

<!-- Desktop navigation links -->
<nav class="d-none d-lg-flex gap-3">
  <a href="#" class="nav-link">Home</a>
  <a href="#" class="nav-link">About</a>
  <a href="#" class="nav-link">Services</a>
  <a href="#" class="nav-link">Contact</a>
</nav>
```

## Advanced Variations

### Responsive Display Types

Show elements with different display types at different breakpoints:

```html
<!-- Block on mobile, flex on medium, grid on large -->
<div class="d-block d-md-flex d-lg-grid gap-3 bg-light p-3" style="grid-template-columns: repeat(3, 1fr);">
  <div class="bg-primary text-white p-2">Item 1</div>
  <div class="bg-success text-white p-2">Item 2</div>
  <div class="bg-danger text-white p-2">Item 3</div>
</div>
```

### Print Visibility

Control what appears when the page is printed:

```html
<!-- Hidden on screen, visible only in print -->
<div class="d-none d-print-block">
  This content only appears when printing
</div>

<!-- Visible on screen, hidden in print -->
<div class="d-block d-print-none">
  This content is hidden when printing
</div>

<!-- Navigation hidden in print -->
<nav class="d-flex d-print-none">
  <a href="#">Home</a>
  <a href="#">About</a>
</nav>
```

### Responsive Sidebar

```html
<div class="container-fluid">
  <div class="row">
    <!-- Sidebar: hidden on mobile, visible on md+ -->
    <aside class="d-none d-md-block col-md-3 bg-light p-3">
      <h5>Sidebar</h5>
      <nav class="nav flex-column">
        <a class="nav-link" href="#">Dashboard</a>
        <a class="nav-link" href="#">Settings</a>
      </nav>
    </aside>
    <!-- Main: always visible -->
    <main class="col-12 col-md-9 p-3">
      <h5>Main Content</h5>
      <p>Content area adjusts based on sidebar visibility.</p>
    </main>
  </div>
</div>
```

### Multi-Breakpoint Visibility Patterns

```html
<!-- Show on xs and xl+ only -->
<div class="d-block d-sm-none d-xl-block bg-success text-white p-3">
  Visible on xs and xl+ (hidden sm through lg)
</div>

<!-- Show on md and lg only -->
<div class="d-none d-md-block d-xl-none bg-info text-white p-3">
  Visible on md and lg only
</div>
```

## Best Practices

1. **Use `d-none` as the hiding mechanism** rather than `invisible` or `opacity-0`. `d-none` removes the element from layout entirely.

2. **Always pair `d-none` with a show class** (`d-sm-block`, `d-md-flex`). Without a show class, the element remains hidden at all sizes.

3. **Use mobile-first approach.** Apply base classes for mobile, then progressively show/hide at larger breakpoints.

4. **Choose the correct display type** when showing: `d-sm-block`, `d-md-flex`, `d-lg-grid`. The display type must match the intended layout behavior.

5. **Use `d-print-none`** to hide navigation, sidebars, and interactive elements when printing.

6. **Use `d-print-block`** for print-specific content like URLs, QR codes, or full addresses.

7. **Avoid hiding critical content** on any breakpoint. Responsive visibility should reorganize layout, not remove essential information.

8. **Test on real devices.** Emulators may not accurately represent how responsive visibility behaves on actual screens.

9. **Combine with `visually-hidden`** for content that should be hidden visually but remain accessible to screen readers.

10. **Use `d-{breakpoint}-none`** to hide elements from a specific breakpoint upward, not just to show them.

11. **Document visibility logic** in complex layouts. Multiple `d-none`/`d-*-block` combinations can be difficult to reason about.

## Common Pitfalls

### Missing display type after d-none
`d-none d-sm-block` works because `d-sm-block` provides a display type. But `d-none d-sm` is invalid and keeps the element hidden at all sizes.

### Forgetting that d-none affects all children
Hiding a parent with `d-none` hides all its children. Screen readers also skip hidden content. Ensure hidden content is available through alternative means.

### Conflicting display types
`d-block d-flex` on the same element without responsive prefixes causes the last class to win. Ensure breakpoint prefixes prevent conflicts.

### Hidden elements still in DOM
`d-none` hides elements visually and from layout but they remain in the HTML. For performance, consider removing heavy hidden elements with JavaScript.

### Print styles not applying
`d-print-none` only applies in print media. Ensure the class is applied correctly and test with browser print preview.

## Accessibility Considerations

`d-none` removes elements from the visual layout and typically from screen reader output. If content should be accessible but visually hidden, use the `visually-hidden` class instead.

When hiding navigation elements on mobile, ensure alternative navigation is available. A hamburger menu must be accessible via keyboard and screen reader, with proper ARIA attributes.

For content that differs between breakpoints (e.g., short text on mobile, full text on desktop), ensure both versions convey the same essential information. Screen readers may encounter both versions in the DOM.

Use `aria-hidden="true"` alongside `d-none` when the hidden content should be completely ignored by assistive technology.

## Responsive Behavior

All display utilities support Bootstrap's five breakpoints. The syntax is `d-{breakpoint}-{value}` where value is any CSS display value: `none`, `block`, `inline`, `inline-block`, `flex`, `inline-flex`, `grid`, `table`, and more.

```html
<!-- Hidden on mobile, flex on tablet, grid on desktop -->
<div class="d-none d-md-flex d-lg-grid gap-3">
  <div>Item 1</div>
  <div>Item 2</div>
  <div>Item 3</div>
</div>
```

The mobile-first approach means un-prefixed classes apply at all sizes, while prefixed classes override from that breakpoint up. This creates a cascade where each breakpoint can modify the display behavior established by smaller breakpoints.
