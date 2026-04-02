---
title: "Mobile Navigation Patterns"
topic: "Mobile First PWA"
difficulty: 2
duration: "30 minutes"
prerequisites: ["Bootstrap Navbar", "Responsive design", "Mobile UX patterns"]
tags: ["mobile", "navigation", "bottom-nav", "tab-bar", "navbar"]
---

## Overview

Mobile navigation patterns prioritize thumb-reachable controls and clear visual hierarchy. Bootstrap 5 provides the foundational components — navbar, nav, offcanvas, tabs — but adapting them for mobile-first experiences requires bottom navigation bars, hamburger menus with offcanvas drawers, tab-based navigation, and sticky headers. These patterns follow platform conventions from iOS and Android, making web apps feel native on mobile devices.

The primary mobile navigation patterns are: bottom tab bars (5 icons max), top navbars with hamburger menus, offcanvas slide-out drawers, and full-page tab navigation. Bootstrap's `navbar-expand-*` responsive breakpoint, `offcanvas` component, and `nav-tabs`/`nav-pills` system provide all building blocks needed for these patterns.

## Basic Implementation

### Bottom Navigation Bar

```html
<!-- Fixed bottom tab bar -->
<nav class="navbar navbar-light bg-white fixed-bottom border-top d-md-none">
  <div class="container-fluid d-flex justify-content-around py-1">
    <a href="/" class="nav-link text-primary text-center active" aria-current="page">
      <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor"
           viewBox="0 0 16 16" class="d-block mx-auto">
        <path d="M8.707 1.5a1 1 0 0 0-1.414 0L.646 8.146a.5.5 0 0 0 .708.708L8 2.207l6.646 6.647a.5.5 0 0 0 .708-.708L13 5.793V2.5a.5.5 0 0 0-.5-.5h-1a.5.5 0 0 0-.5.5v1.293L8.707 1.5Z"/>
        <path d="m8 3.293 6 6V13.5a1.5 1.5 0 0 1-1.5 1.5h-9A1.5 1.5 0 0 1 2 13.5V9.293l6-6Z"/>
      </svg>
      <small>Home</small>
    </a>
    <a href="/search" class="nav-link text-secondary text-center">
      <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor"
           viewBox="0 0 16 16" class="d-block mx-auto">
        <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z"/>
      </svg>
      <small>Search</small>
    </a>
    <a href="/profile" class="nav-link text-secondary text-center">
      <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor"
           viewBox="0 0 16 16" class="d-block mx-auto">
        <path d="M8 8a3 3 0 1 0 0-6 3 3 0 0 0 0 6Zm2-3a2 2 0 1 1-4 0 2 2 0 0 1 4 0Zm4 8c0 1-1 1-1 1H3s-1 0-1-1 1-4 6-4 6 3 6 4Zm-1-.004c-.001-.246-.154-.986-.832-1.664C11.516 10.68 10.289 10 8 10c-2.29 0-3.516.68-4.168 1.332-.678.678-.83 1.418-.832 1.664h10Z"/>
      </svg>
      <small>Profile</small>
    </a>
  </div>
</nav>

<!-- Add padding for bottom nav -->
<div class="pb-5 mb-3">
  <!-- Page content -->
</div>
```

### Hamburger Menu with Offcanvas

```html
<nav class="navbar navbar-expand-md navbar-light bg-white sticky-top shadow-sm">
  <div class="container-fluid">
    <a class="navbar-brand fw-bold" href="/">MyApp</a>

    <!-- Mobile hamburger -->
    <button class="navbar-toggler border-0" type="button"
            data-bs-toggle="offcanvas" data-bs-target="#mobileMenu"
            aria-controls="mobileMenu" aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
    </button>

    <!-- Desktop navigation -->
    <div class="collapse navbar-collapse d-none d-md-flex justify-content-end">
      <ul class="navbar-nav">
        <li class="nav-item"><a class="nav-link active" href="/">Home</a></li>
        <li class="nav-item"><a class="nav-link" href="/products">Products</a></li>
        <li class="nav-item"><a class="nav-link" href="/about">About</a></li>
        <li class="nav-item">
          <a class="btn btn-primary ms-2" href="/login">Sign In</a>
        </li>
      </ul>
    </div>
  </div>
</nav>

<!-- Mobile offcanvas menu -->
<div class="offcanvas offcanvas-end d-md-none" tabindex="-1" id="mobileMenu">
  <div class="offcanvas-header border-bottom">
    <h5 class="offcanvas-title">Menu</h5>
    <button type="button" class="btn-close" data-bs-dismiss="offcanvas"
            aria-label="Close"></button>
  </div>
  <div class="offcanvas-body p-0">
    <div class="list-group list-group-flush">
      <a href="/" class="list-group-item list-group-item-action active"
         aria-current="page">Home</a>
      <a href="/products" class="list-group-item list-group-item-action">Products</a>
      <a href="/about" class="list-group-item list-group-item-action">About</a>
      <a href="/contact" class="list-group-item list-group-item-action">Contact</a>
    </div>
    <div class="p-3">
      <a href="/login" class="btn btn-primary w-100">Sign In</a>
    </div>
  </div>
</div>
```

## Advanced Variations

### Tab-Based Page Navigation

```html
<!-- Mobile tab navigation (visible on small screens) -->
<div class="d-md-none">
  <div class="nav nav-tabs nav-fill border-bottom" role="tablist">
    <button class="nav-link active" data-bs-toggle="tab" data-bs-target="#feed"
            type="button" role="tab" aria-selected="true">
      <span class="d-block">Feed</span>
    </button>
    <button class="nav-link" data-bs-toggle="tab" data-bs-target="#explore"
            type="button" role="tab" aria-selected="false">
      <span class="d-block">Explore</span>
    </button>
    <button class="nav-link" data-bs-toggle="tab" data-bs-target="#saved"
            type="button" role="tab" aria-selected="false">
      <span class="d-block">Saved</span>
    </button>
  </div>

  <div class="tab-content">
    <div class="tab-pane fade show active" id="feed" role="tabpanel">
      <div class="p-3">
        <div class="card mb-3"><div class="card-body">Post 1</div></div>
        <div class="card mb-3"><div class="card-body">Post 2</div></div>
      </div>
    </div>
    <div class="tab-pane fade" id="explore" role="tabpanel">
      <div class="p-3">Explore content</div>
    </div>
    <div class="tab-pane fade" id="saved" role="tabpanel">
      <div class="p-3">Saved items</div>
    </div>
  </div>
</div>
```

### Sticky Search Header

```html
<header class="sticky-top bg-white shadow-sm py-2">
  <div class="container-fluid">
    <div class="input-group">
      <span class="input-group-text bg-transparent border-end-0">
        <svg width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
          <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z"/>
        </svg>
      </span>
      <input type="search" class="form-control border-start-0"
             placeholder="Search..." aria-label="Search">
    </div>
  </div>
</header>
```

## Best Practices

1. **Limit bottom navigation to 3-5 items** — more items reduce tap target size below the 48px minimum.
2. **Use `fixed-bottom`** class for bottom tab bars, with `d-md-none` to hide on larger screens.
3. **Add `pb-5 mb-3`** to page content to prevent it from being hidden behind the fixed bottom nav.
4. **Use `aria-current="page"`** on the active navigation item for screen reader context.
5. **Use `sticky-top`** for search bars and filters that should remain visible during scroll.
6. **Use Bootstrap's `offcanvas`** component for hamburger menus — it handles focus trapping, backdrop, and keyboard navigation.
7. **Make tap targets at least 44x44px** using `py-2 px-3` or similar padding on interactive elements.
8. **Use `navbar-expand-md`** to show full navigation on tablet+ and hamburger on mobile.
9. **Use `list-group-flush`** in offcanvas menus for clean, edge-to-edge menu items.
10. **Include `<title>` and `aria-label`** on navigation landmarks for screen reader identification.

## Common Pitfalls

1. **Too many bottom nav items** (6+) makes them too small to tap accurately on mobile.
2. **Forgetting `pb-5`** on page content causes it to scroll behind the fixed bottom nav.
3. **Using `position: fixed` for top nav** without `sticky-top` causes content jump on scroll.
4. **Not hiding desktop nav on mobile** results in duplicate navigation visible on small screens.
5. **Missing `aria-current="page"`** leaves screen reader users without active page context.

## Accessibility Considerations

Use `<nav aria-label="Main navigation">` to distinguish multiple navigation landmarks. Bottom tab bars should use `role="tablist"` with `role="tab"` and `role="tabpanel"` for proper tab semantics. Offcanvas menus trap focus automatically via Bootstrap's component. Ensure all navigation links are keyboard-accessible and have visible focus indicators. Provide a skip-to-content link at the top of the page for keyboard users to bypass navigation.

## Responsive Behavior

Bottom navigation is hidden on `md`+ screens using `d-md-none`, while desktop navigation appears via `navbar-expand-md`. Offcanvas menus are only rendered on mobile (toggled by `navbar-toggler`). Tab-based navigation switches between `nav-tabs` (mobile) and full page layouts (desktop). Bootstrap's responsive grid ensures navigation components reflow correctly: vertical menus become horizontal, bottom bars disappear, and offcanvas drawers become inline sidebar content.