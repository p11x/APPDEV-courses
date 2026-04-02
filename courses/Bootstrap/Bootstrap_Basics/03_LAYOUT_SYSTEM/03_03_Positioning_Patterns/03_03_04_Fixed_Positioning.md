---
title: "Fixed Positioning"
description: "Implement fixed navigation bars, floating buttons, and viewport-pinned elements using Bootstrap 5 fixed positioning utilities."
difficulty: 2
estimated_time: "20 minutes"
prerequisites:
  - "Position utilities"
  - "Z-index layers"
tags:
  - positioning
  - fixed
  - navbar
  - layout
  - viewport
---

## Overview

Fixed positioning pins elements to the viewport so they remain visible during scrolling. Bootstrap 5 provides `fixed-top` and `fixed-bottom` utility classes for this purpose. These are commonly used for navigation bars, cookie consent banners, floating action buttons, and persistent footers.

Fixed elements are removed from normal document flow, so surrounding content must compensate with padding or margin to avoid overlap. Bootstrap's navbar component integrates fixed positioning seamlessly with its `navbar-fixed-top` and `navbar-fixed-bottom` variants.

## Basic Implementation

### Fixed Top Navigation

```html
<nav class="navbar navbar-expand-lg navbar-dark bg-dark fixed-top">
  <div class="container">
    <a class="navbar-brand" href="#">MyApp</a>
    <ul class="navbar-nav">
      <li class="nav-item"><a class="nav-link" href="#">Home</a></li>
      <li class="nav-item"><a class="nav-link" href="#">About</a></li>
    </ul>
  </div>
</nav>

<!-- Add padding to body to prevent content overlap -->
<main class="container" style="padding-top: 56px;">
  <h1>Page Content</h1>
  <p>Content starts below the fixed navbar.</p>
</main>
```

### Fixed Bottom Bar

```html
<div class="fixed-bottom bg-primary text-white p-3">
  <div class="container d-flex justify-content-between align-items-center">
    <span>Cookie Notice</span>
    <button class="btn btn-light btn-sm">Accept</button>
  </div>
</div>
```

### Fixed Floating Action Button

```html
<button class="btn btn-primary rounded-circle position-fixed bottom-0 end-0 m-4"
        style="width: 56px; height: 56px; z-index: 1050;">
  +
</button>
```

## Advanced Variations

### Body Padding Helper

Bootstrap provides `.fixed-top` and `.fixed-bottom` body padding helpers. Apply the class to `<body>`:

```html
<body class="fixed-top-body">
  <nav class="navbar fixed-top navbar-dark bg-dark">
    <div class="container">
      <a class="navbar-brand" href="#">App</a>
    </div>
  </nav>
  <main class="container">
    <h1>Content</h1>
  </main>
</body>
```

Manually add body padding in your CSS:

```css
.fixed-top-body {
  padding-top: 56px; /* Match navbar height */
}

.fixed-bottom-body {
  padding-bottom: 56px;
}
```

### Z-Index Management

Fixed elements stack above normal content but may conflict with modals and dropdowns:

```html
<!-- Standard navbar z-index -->
<nav class="navbar fixed-top navbar-dark bg-dark" style="z-index: 1030;">
  <div class="container">
    <a class="navbar-brand" href="#">App</a>
  </div>
</nav>

<!-- Toast above navbar -->
<div class="position-fixed top-0 end-0 p-3" style="z-index: 1070;">
  <div class="toast show">
    <div class="toast-body">Notification message</div>
  </div>
</div>
```

### Responsive Fixed Navigation

Make navigation fixed only on larger screens:

```html
<nav class="navbar navbar-expand-lg navbar-dark bg-dark sticky-top">
  <div class="container">
    <a class="navbar-brand" href="#">Responsive Nav</a>
  </div>
</nav>
```

Use `sticky-top` instead of `fixed-top` for behavior that stays within the document flow on mobile but pins on desktop, or add custom CSS:

```css
@media (min-width: 992px) {
  .navbar {
    position: fixed;
    top: 0;
    width: 100%;
    z-index: 1030;
  }
  body {
    padding-top: 56px;
  }
}
```

## Best Practices

1. **Always add body padding** matching the fixed element's height to prevent content from hiding behind it.
2. **Set explicit `z-index`** on fixed elements to control stacking order relative to modals and dropdowns.
3. **Use `fixed-top` on `<nav>` elements** for consistent navigation behavior.
4. **Reserve `fixed-bottom` for actionable elements** like CTAs, consent bars, or chat widgets.
5. **Keep fixed elements compact** to minimize viewport space consumption, especially on mobile.
6. **Use `navbar-fixed-top` pattern** from Bootstrap's navbar component for pre-configured fixed navigation.
7. **Test on mobile devices** to ensure fixed elements do not obstruct touch interactions.
8. **Provide dismiss options** for fixed-bottom banners to let users reclaim viewport space.
9. **Use `z-index: 1030`** for navbars (Bootstrap's default) and `1050+` for elements that must appear above modals.
10. **Avoid fixed positioning on content areas**; it is intended for chrome and navigation, not primary content.
11. **Use `role="banner"`** on fixed-top navigation and `role="contentinfo"` on fixed-bottom bars for semantic HTML.
12. **Test scroll behavior** to ensure fixed elements do not cause jank or repaint issues on low-powered devices.

## Common Pitfalls

1. **Content hidden behind fixed nav**: Without body padding, the top of the page content is obscured.
2. **Z-index conflicts with modals**: Bootstrap modals use `z-index: 1055`. Fixed elements with higher z-index appear above modals unintentionally.
3. **Fixed elements on mobile consuming screen space**: A fixed navbar and footer together can consume 30%+ of a small viewport.
4. **Print layout issues**: Fixed elements appear on every printed page or cause clipping. Use `@media print` to hide them.
5. **Fixed inside transform container**: If any ancestor has `transform`, `perspective`, or `filter`, fixed positioning behaves like absolute relative to that ancestor.
6. **Double scrollbars**: Fixed elements combined with overflow settings can create unexpected scrollbar behavior.

## Accessibility Considerations

- Use `aria-label` on fixed navigation landmarks so screen readers identify them correctly.
- Ensure fixed elements do not cover focused elements during keyboard navigation.
- Provide a skip link (`<a class="visually-hidden-focusable" href="#main">Skip to content</a>`) to bypass fixed navigation.
- Fixed bottom bars should be dismissible so they do not permanently reduce content area.
- Maintain minimum 44x44px touch targets on fixed interactive elements.

## Responsive Behavior

Fixed positioning can be conditionally applied using custom CSS with Bootstrap breakpoints:

```css
/* Fixed only on large screens */
@media (min-width: 992px) {
  .navbar {
    position: fixed;
    top: 0;
    width: 100%;
    z-index: 1030;
  }
  body { padding-top: 56px; }
}

@media (max-width: 991.98px) {
  .navbar {
    position: relative;
  }
  body { padding-top: 0; }
}
```

Alternatively, use `sticky-top` which naturally adapts to container bounds and provides a similar experience without viewport anchoring issues on mobile.
