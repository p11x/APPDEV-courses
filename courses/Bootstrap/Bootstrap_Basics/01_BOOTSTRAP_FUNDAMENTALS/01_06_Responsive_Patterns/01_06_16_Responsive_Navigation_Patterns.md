---
title: "Responsive Navigation Patterns"
lesson: "01_06_16"
difficulty: "2"
topics: ["navbar", "collapse", "offcanvas", "dropdowns", "mobile-nav"]
estimated_time: "30 minutes"
---

# Responsive Navigation Patterns

## Overview

Bootstrap provides three primary responsive navigation components: the **navbar** with collapse toggler, **offcanvas** for slide-in mobile menus, and **dropdowns** for nested navigation. The navbar's `.navbar-expand-{breakpoint}` class controls when the mobile toggle appears and links expand horizontally. Offcanvas replaces traditional mobile menus with a slide-in panel. Understanding how to combine these components creates navigation that adapts from a full desktop menu to a touch-friendly mobile experience.

## Basic Implementation

### Navbar with Collapse Toggler

```html
<nav class="navbar navbar-expand-lg navbar-dark bg-dark">
  <div class="container-fluid">
    <a class="navbar-brand" href="#">Brand</a>
    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#mainNav">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="mainNav">
      <ul class="navbar-nav me-auto">
        <li class="nav-item"><a class="nav-link active" href="#">Home</a></li>
        <li class="nav-item"><a class="nav-link" href="#">About</a></li>
        <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">Services</a>
          <ul class="dropdown-menu">
            <li><a class="dropdown-item" href="#">Web Design</a></li>
            <li><a class="dropdown-item" href="#">Development</a></li>
          </ul>
        </li>
      </ul>
      <form class="d-flex" role="search">
        <input class="form-control me-2" type="search" placeholder="Search">
        <button class="btn btn-outline-light" type="submit">Search</button>
      </form>
    </div>
  </div>
</nav>
```

### Offcanvas Mobile Navigation

```html
<!-- Mobile nav (offcanvas) -->
<nav class="navbar bg-body-tertiary d-lg-none">
  <div class="container-fluid">
    <a class="navbar-brand" href="#">Brand</a>
    <button class="navbar-toggler" data-bs-toggle="offcanvas" data-bs-target="#mobileMenu">
      <span class="navbar-toggler-icon"></span>
    </button>
  </div>
</nav>

<div class="offcanvas offcanvas-start" tabindex="-1" id="mobileMenu">
  <div class="offcanvas-header">
    <h5 class="offcanvas-title">Navigation</h5>
    <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body">
    <ul class="list-unstyled">
      <li><a class="d-block py-2 text-decoration-none" href="#">Home</a></li>
      <li><a class="d-block py-2 text-decoration-none" href="#">About</a></li>
      <li><a class="d-block py-2 text-decoration-none" href="#">Contact</a></li>
    </ul>
  </div>
</div>

<!-- Desktop nav (inline) -->
<nav class="navbar bg-body-tertiary d-none d-lg-block">
  <div class="container">
    <ul class="navbar-nav flex-row gap-3">
      <li class="nav-item"><a class="nav-link" href="#">Home</a></li>
      <li class="nav-item"><a class="nav-link" href="#">About</a></li>
      <li class="nav-item"><a class="nav-link" href="#">Contact</a></li>
    </ul>
  </div>
</nav>
```

## Advanced Variations

### Mega Menu with Responsive Collapse

```html
<nav class="navbar navbar-expand-xl navbar-light bg-light">
  <div class="container">
    <a class="navbar-brand" href="#">Store</a>
    <button class="navbar-toggler" data-bs-toggle="collapse" data-bs-target="#megaNav">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="megaNav">
      <ul class="navbar-nav">
        <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle" data-bs-toggle="dropdown" href="#">Shop</a>
          <div class="dropdown-menu p-4" style="min-width: 400px;">
            <div class="row">
              <div class="col-6">
                <h6 class="dropdown-header">Electronics</h6>
                <a class="dropdown-item" href="#">Phones</a>
                <a class="dropdown-item" href="#">Laptops</a>
              </div>
              <div class="col-6">
                <h6 class="dropdown-header">Clothing</h6>
                <a class="dropdown-item" href="#">Men's</a>
                <a class="dropdown-item" href="#">Women's</a>
              </div>
            </div>
          </div>
        </li>
      </ul>
    </div>
  </div>
</nav>
```

### Offcanvas with Accordion Sub-Navigation

```html
<div class="offcanvas offcanvas-start" id="navAccordion">
  <div class="offcanvas-header">
    <h5 class="offcanvas-title">Menu</h5>
    <button class="btn-close" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body p-0">
    <div class="accordion accordion-flush" id="navMenu">
      <div class="accordion-item">
        <h2 class="accordion-header">
          <button class="accordion-button collapsed" data-bs-toggle="collapse" data-bs-target="#navProducts">
            Products
          </button>
        </h2>
        <div id="navProducts" class="accordion-collapse collapse" data-bs-parent="#navMenu">
          <div class="accordion-body ps-4">
            <a href="#" class="d-block py-1">All Products</a>
            <a href="#" class="d-block py-1">New Arrivals</a>
            <a href="#" class="d-block py-1">Sale</a>
          </div>
        </div>
      </div>
    </div>
    <hr class="m-0">
    <a href="#" class="d-block p-3">About Us</a>
    <a href="#" class="d-block p-3">Contact</a>
  </div>
</div>
```

## Best Practices

1. **Use `.navbar-expand-lg` for typical responsive breakpoints** - Collapse below 992px.
2. **Place toggler button and brand on the same row** - `container-fluid` with flex utilities.
3. **Use `me-auto` on nav items to push right-side content** - Login button, search form.
4. **Use offcanvas for deeply nested navigation** - Better UX than collapsed accordion menus.
5. **Add `aria-expanded` and `aria-controls` to toggler buttons** - Communicates state to screen readers.
6. **Use `data-bs-dismiss="offcanvas"` on the close button** - Proper event handling.
7. **Include `.navbar-toggler` only when needed** - `navbar-expand-lg` hides it above `lg`.
8. **Test dropdown menus on touch devices** - Taps open/close, not hover.
9. **Keep mobile navigation to 5-7 primary items** - Cognitive load limit on small screens.
10. **Use consistent colors across navbar variants** - `navbar-dark` with dark backgrounds, `navbar-light` with light.

## Common Pitfalls

1. **Not including Popper.js for dropdowns** - Menus don't position correctly; use the bundle build.
2. **Using `navbar-expand` without a breakpoint** - Never collapses; always horizontal.
3. **Nesting dropdowns inside offcanvas without accordion parent** - Multiple panels open simultaneously.
4. **Forgetting `tabindex="-1"` on offcanvas** - Breaks focus management.
5. **Using `data-bs-toggle="collapse"` on offcanvas triggers** - Conflicting Bootstrap plugins; use `data-bs-toggle="offcanvas"`.

## Accessibility Considerations

Navigation must be keyboard-accessible: Tab through links, Enter to activate, Escape to close dropdowns and offcanvas. Bootstrap manages `aria-expanded`, `aria-controls`, and focus trapping automatically when using `data-bs-*` attributes. The navbar should use `<nav aria-label="Main navigation">` for landmark identification. Mobile toggler must be reachable by keyboard and announce its state. Offcanvas traps focus within the panel when open and returns focus to the trigger when closed.

## Responsive Behavior

`.navbar-expand-lg` switches from collapsed (hamburger) to horizontal at the `lg` breakpoint (992px). Below `lg`, the `.navbar-collapse` is hidden until toggled. Offcanvas components can be responsive: `offcanvas-start offcanvas-lg-start` makes the panel persistent on `lg+` screens while remaining a slide-in on smaller screens. Dropdown menus inside the navbar automatically stack vertically when collapsed and display horizontally when expanded.
