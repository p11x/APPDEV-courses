---
title: Navbar Offcanvas Hybrid
category: Component System
difficulty: 2
time: 30 min
tags: bootstrap5, navbar, offcanvas, responsive, navigation, collapse
---

## Overview

Bootstrap's navbar can use an offcanvas component instead of the default collapse for the mobile menu. The offcanvas slides in from the side (start, end, top, or bottom) providing a full-height navigation panel that works well for complex menus with many items. This hybrid approach combines the navbar's responsive breakpoint behavior with offcanvas's richer slide-in experience.

## Basic Implementation

Replace `navbar-collapse` with `offcanvas` to create a slide-in mobile menu.

```html
<nav class="navbar navbar-expand-lg bg-body-tertiary">
  <div class="container-fluid">
    <a class="navbar-brand" href="#">Brand</a>
    <button class="navbar-toggler" type="button" data-bs-toggle="offcanvas"
            data-bs-target="#navbarOffcanvas" aria-controls="navbarOffcanvas"
            aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="offcanvas offcanvas-end" tabindex="-1" id="navbarOffcanvas"
         aria-labelledby="navbarOffcanvasLabel">
      <div class="offcanvas-header">
        <h5 class="offcanvas-title" id="navbarOffcanvasLabel">Menu</h5>
        <button type="button" class="btn-close" data-bs-dismiss="offcanvas"
                aria-label="Close"></button>
      </div>
      <div class="offcanvas-body">
        <ul class="navbar-nav justify-content-end flex-grow-1 pe-3">
          <li class="nav-item">
            <a class="nav-link active" aria-current="page" href="#">Home</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="#">Features</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="#">Pricing</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="#">Contact</a>
          </li>
        </ul>
      </div>
    </div>
  </div>
</nav>
```

## Advanced Variations

```html
<!-- Offcanvas with dropdown menus -->
<nav class="navbar navbar-expand-lg bg-dark navbar-dark">
  <div class="container-fluid">
    <a class="navbar-brand" href="#">App Name</a>
    <button class="navbar-toggler" type="button" data-bs-toggle="offcanvas"
            data-bs-target="#offcanvasNav" aria-controls="offcanvasNav"
            aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="offcanvas offcanvas-end text-bg-dark" tabindex="-1" id="offcanvasNav">
      <div class="offcanvas-header">
        <h5 class="offcanvas-title">Navigation</h5>
        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="offcanvas"
                aria-label="Close"></button>
      </div>
      <div class="offcanvas-body">
        <ul class="navbar-nav flex-grow-1">
          <li class="nav-item">
            <a class="nav-link active" href="#">Dashboard</a>
          </li>
          <li class="nav-item dropdown">
            <a class="nav-link dropdown-toggle" href="#" role="button"
               data-bs-toggle="dropdown" aria-expanded="false">
              Projects
            </a>
            <ul class="dropdown-menu">
              <li><a class="dropdown-item" href="#">Active Projects</a></li>
              <li><a class="dropdown-item" href="#">Archived</a></li>
              <li><hr class="dropdown-divider"></li>
              <li><a class="dropdown-item" href="#">Create New</a></li>
            </ul>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="#">Team</a>
          </li>
        </ul>
        <form class="d-flex mt-3 mt-lg-0" role="search">
          <input class="form-control me-2" type="search" placeholder="Search">
          <button class="btn btn-outline-light" type="submit">Search</button>
        </form>
      </div>
    </div>
  </div>
</nav>
```

```html
<!-- Offcanvas from start (left) side -->
<nav class="navbar navbar-expand-lg bg-body-tertiary">
  <div class="container-fluid">
    <button class="navbar-toggler me-2" type="button" data-bs-toggle="offcanvas"
            data-bs-target="#startOffcanvas" aria-controls="startOffcanvas">
      <span class="navbar-toggler-icon"></span>
    </button>
    <a class="navbar-brand" href="#">App</a>
    <div class="d-none d-lg-flex flex-grow-1">
      <ul class="navbar-nav">
        <li class="nav-item"><a class="nav-link active" href="#">Home</a></li>
        <li class="nav-item"><a class="nav-link" href="#">Docs</a></li>
        <li class="nav-item"><a class="nav-link" href="#">API</a></li>
      </ul>
    </div>
    <div class="offcanvas offcanvas-start" tabindex="-1" id="startOffcanvas">
      <div class="offcanvas-header">
        <h5 class="offcanvas-title">Navigation</h5>
        <button type="button" class="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
      </div>
      <div class="offcanvas-body">
        <ul class="nav flex-column">
          <li class="nav-item"><a class="nav-link active" href="#">Home</a></li>
          <li class="nav-item"><a class="nav-link" href="#">Documentation</a></li>
          <li class="nav-item"><a class="nav-link" href="#">API Reference</a></li>
          <li class="nav-item"><a class="nav-link" href="#">Examples</a></li>
        </ul>
      </div>
    </div>
  </div>
</nav>
```

## Best Practices

1. Use `data-bs-toggle="offcanvas"` on the navbar toggler button.
2. Set `data-bs-target` to match the offcanvas `id`.
3. Use `offcanvas-end` for right-side slide-in or `offcanvas-start` for left-side.
4. Include `tabindex="-1"` on the offcanvas for proper focus management.
5. Use `navbar-expand-lg` to control at which breakpoint the offcanvas activates.
6. Provide an offcanvas header with a close button for clear dismissal.
7. Use `offcanvas-body` for scrollable content when menu items overflow.
8. Apply `text-bg-dark` on offcanvas for dark-themed navigation panels.
9. Keep the navbar brand visible outside the offcanvas at all breakpoints.
10. Include `aria-controls` on the toggler button referencing the offcanvas `id`.

## Common Pitfalls

1. **Missing `tabindex="-1"`.** The offcanvas receives unexpected focus without this attribute.
2. **Wrong `data-bs-target`.** Toggler does not open the offcanvas if the target ID does not match.
3. **No close button in header.** Users need an obvious way to dismiss the offcanvas.
4. **Overflow issues.** Long menus without `offcanvas-body` scroll behavior cause layout problems.
5. **Not hiding desktop nav.** Without `d-none d-lg-flex`, desktop nav and offcanvas both appear.
6. **Forgetting `aria-controls`.** Screen readers cannot link the toggler to the offcanvas panel.

## Accessibility Considerations

The offcanvas includes built-in focus trapping and ARIA attributes. Add `aria-labelledby` pointing to the offcanvas title `id` for screen reader context. The toggler button needs `aria-controls` referencing the offcanvas `id` and `aria-label="Toggle navigation"`. The close button requires `aria-label="Close"`. Ensure focus returns to the toggler button after the offcanvas closes. Use `role="dialog"` on the offcanvas for proper semantics.

## Responsive Behavior

The navbar displays inline links on large screens (`navbar-expand-lg`) and shows the toggler on smaller screens. The offcanvas replaces the collapse behavior on mobile, sliding in from the chosen side. On desktop, the offcanvas is hidden and links display inline. Use `d-none d-lg-flex` to conditionally show/hide navigation elements. The offcanvas adapts to screen height and provides internal scrolling when content exceeds viewport height.
