---
title: Vertical Navigation
category: Component System
difficulty: 1
time: 20 min
tags: bootstrap5, vertical-nav, nav, pills, sidebar, stacked
---

## Overview

Bootstrap supports vertical navigation using the `flex-column` utility on `nav` components. Vertical navs work for sidebars, settings menus, and documentation navigation. They support nav links, stacked pills, icons, and collapse sub-menus. Combined with Bootstrap's grid system, vertical navigation creates responsive sidebar layouts.

## Basic Implementation

Add `flex-column` to a `nav` element to stack links vertically.

```html
<!-- Vertical nav links -->
<nav class="nav flex-column">
  <a class="nav-link active" aria-current="page" href="#">Dashboard</a>
  <a class="nav-link" href="#">Orders</a>
  <a class="nav-link" href="#">Products</a>
  <a class="nav-link" href="#">Customers</a>
  <a class="nav-link disabled" href="#" tabindex="-1">Reports</a>
</nav>
```

## Advanced Variations

```html
<!-- Vertical stacked pills -->
<ul class="nav flex-column nav-pills">
  <li class="nav-item">
    <a class="nav-link active" href="#">Account</a>
  </li>
  <li class="nav-item">
    <a class="nav-link" href="#">Security</a>
  </li>
  <li class="nav-item">
    <a class="nav-link" href="#">Notifications</a>
  </li>
  <li class="nav-item">
    <a class="nav-link" href="#">Billing</a>
  </li>
  <li class="nav-item">
    <a class="nav-link" href="#">Integrations</a>
  </li>
</ul>
```

```html
<!-- Vertical nav with icons -->
<nav class="nav flex-column">
  <a class="nav-link active d-flex align-items-center gap-2" href="#">
    <i class="bi bi-house-door"></i> Home
  </a>
  <a class="nav-link d-flex align-items-center gap-2" href="#">
    <i class="bi bi-speedometer2"></i> Dashboard
  </a>
  <a class="nav-link d-flex align-items-center gap-2" href="#">
    <i class="bi bi-cart"></i> Orders
    <span class="badge bg-danger ms-auto">3</span>
  </a>
  <a class="nav-link d-flex align-items-center gap-2" href="#">
    <i class="bi bi-gear"></i> Settings
  </a>
</nav>
```

```html
<!-- Sidebar navigation layout -->
<div class="row">
  <div class="col-md-3">
    <div class="list-group">
      <a href="#" class="list-group-item list-group-item-action active" aria-current="true">
        <div class="d-flex w-100 justify-content-between">
          <h6 class="mb-1">General</h6>
        </div>
        <small>Profile, account, preferences</small>
      </a>
      <a href="#" class="list-group-item list-group-item-action">
        <div class="d-flex w-100 justify-content-between">
          <h6 class="mb-1">Security</h6>
        </div>
        <small>Password, two-factor, sessions</small>
      </a>
      <a href="#" class="list-group-item list-group-item-action">
        <div class="d-flex w-100 justify-content-between">
          <h6 class="mb-1">Billing</h6>
        </div>
        <small>Plans, payment methods, invoices</small>
      </a>
    </div>
  </div>
  <div class="col-md-9">
    <h4>Settings Content</h4>
    <p>Select a category from the sidebar.</p>
  </div>
</div>
```

## Best Practices

1. Use `flex-column` on `nav` elements to create vertical stacking.
2. Apply `nav-pills` for pill-style vertical navigation.
3. Use `active` class to highlight the current page or section.
4. Include icons alongside text for quick visual scanning.
5. Use `list-group` for sidebar navigation with descriptions.
6. Provide `aria-current="page"` on the active navigation link.
7. Use `disabled` class for unavailable navigation items.
8. Add badges for notification counts on nav items.
9. Keep navigation labels concise and descriptive.
10. Use consistent vertical spacing with `gap-*` utilities.

## Common Pitfalls

1. **Missing `flex-column`.** Without it, nav items display horizontally.
2. **No `aria-current` on active link.** Screen readers cannot identify the current page.
3. **Overcrowded sidebars.** Too many items make navigation overwhelming; use collapsible sections.
4. **Missing responsive behavior.** Vertical sidebars need to collapse or hide on mobile.
5. **No hover/focus states.** Custom styled navs lose Bootstrap's built-in focus indicators.
6. **Using nav for non-navigation content.** Reserve `nav` for actual navigation; use `list-group` for general lists.

## Accessibility Considerations

Vertical navigation should use `<nav>` with `aria-label` describing the navigation purpose (e.g., "Main navigation", "Settings menu"). Active links need `aria-current="page"`. Disabled links should include `aria-disabled="true"` and `tabindex="-1"` to remove from tab order. Use `<ul>` and `<li>` structure for semantic navigation lists. Ensure focus indicators are visible on all interactive items.

## Responsive Behavior

Vertical navigation fills its container width by default. Use `col-md-3` with `col-md-9` content area for sidebar layouts that stack on mobile. On small screens, hide the sidebar with `d-none d-md-block` and provide an offcanvas or collapse alternative for mobile access. Vertical pill navigation adapts naturally. Use `position-sticky top-0` for sticky sidebars that scroll with content.
