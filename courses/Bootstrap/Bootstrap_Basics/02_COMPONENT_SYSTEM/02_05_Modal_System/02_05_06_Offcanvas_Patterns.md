---
title: Offcanvas Patterns
category: Component System
difficulty: 2
time: 20 min
tags: bootstrap5, offcanvas, navigation, sidebar, filters, accordion
---

## Overview

Offcanvas components are used in several common UI patterns: mobile navigation menus, persistent sidebars, filter panels, and content drawers. Combining offcanvas with other Bootstrap components like accordion, list group, and navbar creates robust, production-ready patterns. This lesson covers the most common offcanvas patterns including navigation, sidebar layout, filter panels, accordion integration, and dismissible vs non-dismissible configurations.

## Basic Implementation

### Offcanvas Navigation

```html
<nav class="navbar navbar-dark bg-dark">
  <div class="container-fluid">
    <a class="navbar-brand" href="#">My App</a>
    <button class="navbar-toggler" type="button" data-bs-toggle="offcanvas"
            data-bs-target="#navOffcanvas" aria-controls="navOffcanvas">
      <span class="navbar-toggler-icon"></span>
    </button>
  </div>
</nav>

<div class="offcanvas offcanvas-start" tabindex="-1" id="navOffcanvas" aria-labelledby="navOffcanvasLabel">
  <div class="offcanvas-header bg-dark text-white">
    <h5 class="offcanvas-title" id="navOffcanvasLabel">Navigation</h5>
    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="offcanvas" aria-label="Close"></button>
  </div>
  <div class="offcanvas-body">
    <nav class="nav flex-column">
      <a class="nav-link active" href="#">Home</a>
      <a class="nav-link" href="#">Dashboard</a>
      <a class="nav-link" href="#">Projects</a>
      <a class="nav-link" href="#">Settings</a>
    </nav>
  </div>
</div>
```

## Advanced Variations

### Offcanvas Sidebar with Content

A responsive sidebar that is static on desktop and offcanvas on mobile.

```html
<div class="container-fluid">
  <div class="row">
    <div class="col-lg-3 d-none d-lg-block bg-light p-3">
      <h6>Static Sidebar</h6>
      <nav class="nav flex-column">
        <a class="nav-link" href="#">Dashboard</a>
        <a class="nav-link" href="#">Analytics</a>
        <a class="nav-link" href="#">Reports</a>
      </nav>
    </div>
    <div class="offcanvas-lg offcanvas-start" tabindex="-1" id="sidebar" aria-labelledby="sidebarLabel">
      <div class="offcanvas-header bg-light">
        <h5 class="offcanvas-title" id="sidebarLabel">Menu</h5>
        <button type="button" class="btn-close d-lg-none" data-bs-dismiss="offcanvas" aria-label="Close"></button>
      </div>
      <div class="offcanvas-body">
        <nav class="nav flex-column">
          <a class="nav-link" href="#">Dashboard</a>
          <a class="nav-link" href="#">Analytics</a>
          <a class="nav-link" href="#">Reports</a>
        </nav>
      </div>
    </div>
    <div class="col">
      <button class="btn btn-outline-secondary d-lg-none mb-3" data-bs-toggle="offcanvas" data-bs-target="#sidebar">
        Toggle Sidebar
      </button>
      <h1>Main Content</h1>
      <p>Page content goes here.</p>
    </div>
  </div>
</div>
```

### Offcanvas Filter Panel

```html
<button class="btn btn-outline-primary" data-bs-toggle="offcanvas" data-bs-target="#filterPanel">
  Filters
</button>

<div class="offcanvas offcanvas-end" tabindex="-1" id="filterPanel" aria-labelledby="filterPanelLabel">
  <div class="offcanvas-header">
    <h5 class="offcanvas-title" id="filterPanelLabel">Filters</h5>
    <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body">
    <form>
      <div class="mb-3">
        <label class="form-label">Category</label>
        <select class="form-select">
          <option>All</option>
          <option>Electronics</option>
          <option>Clothing</option>
        </select>
      </div>
      <div class="mb-3">
        <label class="form-label">Price Range</label>
        <input type="range" class="form-range" min="0" max="500">
      </div>
      <div class="mb-3">
        <label class="form-label">Rating</label>
        <div class="form-check">
          <input class="form-check-input" type="checkbox" id="rating5">
          <label class="form-check-label" for="rating5">5 Stars</label>
        </div>
        <div class="form-check">
          <input class="form-check-input" type="checkbox" id="rating4">
          <label class="form-check-label" for="rating4">4+ Stars</label>
        </div>
      </div>
      <button type="submit" class="btn btn-primary w-100">Apply Filters</button>
    </form>
  </div>
</div>
```

### Offcanvas with Accordion

```html
<div class="offcanvas offcanvas-start" tabindex="-1" id="accordionOffcanvas" aria-labelledby="accordionLabel">
  <div class="offcanvas-header">
    <h5 class="offcanvas-title" id="accordionLabel">Help Topics</h5>
    <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body">
    <div class="accordion" id="helpAccordion">
      <div class="accordion-item">
        <h2 class="accordion-header">
          <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#helpOne">
            Getting Started
          </button>
        </h2>
        <div id="helpOne" class="accordion-collapse collapse show" data-bs-parent="#helpAccordion">
          <div class="accordion-body">
            <p>Welcome! Follow these steps to set up your account.</p>
          </div>
        </div>
      </div>
      <div class="accordion-item">
        <h2 class="accordion-header">
          <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#helpTwo">
            Account Settings
          </button>
        </h2>
        <div id="helpTwo" class="accordion-collapse collapse" data-bs-parent="#helpAccordion">
          <div class="accordion-body">
            <p>Manage your profile, notifications, and privacy settings.</p>
          </div>
        </div>
      </div>
      <div class="accordion-item">
        <h2 class="accordion-header">
          <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#helpThree">
            Billing
          </button>
        </h2>
        <div id="helpThree" class="accordion-collapse collapse" data-bs-parent="#helpAccordion">
          <div class="accordion-body">
            <p>View invoices, update payment methods, and manage subscriptions.</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Dismissible vs Non-Dismissible

```html
<!-- Dismissible: click backdrop or press Escape to close -->
<div class="offcanvas offcanvas-end" tabindex="-1" id="dismissibleCanvas" aria-labelledby="dismissibleLabel">
  <div class="offcanvas-header">
    <h5 class="offcanvas-title" id="dismissibleLabel">Dismissible</h5>
    <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body">
    <p>Click outside or press Escape to close.</p>
  </div>
</div>

<!-- Non-dismissible: must click close button -->
<button class="btn btn-warning" data-bs-toggle="offcanvas" data-bs-target="#staticCanvas"
        data-bs-backdrop="static" data-bs-scroll="true">
  Open Non-Dismissible
</button>

<div class="offcanvas offcanvas-end" tabindex="-1" id="staticCanvas" aria-labelledby="staticLabel"
     data-bs-backdrop="static" data-bs-scroll="true">
  <div class="offcanvas-header">
    <h5 class="offcanvas-title" id="staticLabel">Non-Dismissible</h5>
    <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body">
    <p>Backdrop click does not close this. Scroll is allowed on the body.</p>
  </div>
</div>
```

## Best Practices

1. Use responsive offcanvas classes (`.offcanvas-lg`) for sidebar patterns that persist on desktop.
2. Hide the close button on breakpoints where the offcanvas is static (e.g., `d-lg-none`).
3. Use `data-bs-backdrop="static"` for filter or form panels where accidental dismissal is disruptive.
4. Use `data-bs-scroll="true"` when the offcanvas content is short and the page behind it should remain scrollable.
5. Combine offcanvas with accordion for collapsible content sections within drawers.
6. Keep offcanvas navigation items concise; use nested accordions for deep hierarchies.
7. Apply `aria-labelledby` on every offcanvas for screen reader identification.
8. Test the transition between offcanvas and static sidebar at breakpoint boundaries.
9. Avoid using offcanvas for critical actions that require immediate user attention (use modals instead).
10. Style the offcanvas header to match the triggering navigation context for visual continuity.

## Common Pitfalls

- **Not hiding the close button on desktop:** When using responsive offcanvas, the close button is unnecessary on larger screens; hide it with display utilities.
- **Placing offcanvas inside navbar markup:** This can break z-index and cause the offcanvas to appear behind content.
- **Forgetting `data-bs-backdrop="static"` for non-dismissible patterns:** Without it, clicking outside closes the offcanvas unintentionally.
- **Using offcanvas for critical confirmations:** Modals are more appropriate for focused, required interactions.
- **Not testing responsive breakpoint behavior:** The offcanvas-to-static transition can cause layout shifts if not tested.
- **Accordions inside offcanvas losing scroll position:** When accordion sections expand, the offcanvas scroll position may shift unexpectedly.

## Accessibility Considerations

Offcanvas navigation must include `aria-labelledby` linking to its title. Use `aria-label` on toggle buttons. When using offcanvas for navigation, ensure all links are keyboard-accessible and screen readers announce the navigation region. Non-dismissible offcanvas should communicate its dismissal requirement through visible instructions or `aria-describedby`. Accordion items inside offcanvas follow the same accessibility rules as standalone accordions.

## Responsive Behavior

Responsive offcanvas classes (`.offcanvas-sm`, `.offcanvas-md`, `.offcanvas-lg`, `.offcanvas-xl`, `.offcanvas-xxl`) render the component as a static sidebar above the specified breakpoint. Below the breakpoint, it behaves as a sliding drawer. This pattern is ideal for dashboard sidebars, e-commerce filter panels, and navigation menus that need to adapt between mobile and desktop layouts without duplicating markup.
