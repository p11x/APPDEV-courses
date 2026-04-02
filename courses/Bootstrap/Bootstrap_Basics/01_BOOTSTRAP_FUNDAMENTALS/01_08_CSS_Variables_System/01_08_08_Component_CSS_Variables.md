---
title: Component CSS Variables
category: Bootstrap Fundamentals
difficulty: 3
time: 30 min
tags: bootstrap5, css-variables, components, theming, modal, button, card
---

## Overview

Bootstrap 5 components expose dedicated CSS custom properties that allow per-component theming without affecting the global system. Each component (buttons, cards, modals, navbars, dropdowns, etc.) has its own set of `--bs-*` variables that control padding, colors, borders, and behavior. This granular control enables creating component-level variations that maintain consistency within a single component type while differing from the global theme. Understanding component variables is essential for advanced theming, white-labeling, and building design systems on top of Bootstrap.

## Basic Implementation

Button components expose variables for styling all button states.

```html
<style>
  /* Override button variables for a custom button group */
  .custom-buttons {
    --bs-btn-padding-x: 1.5rem;
    --bs-btn-padding-y: 0.75rem;
    --bs-btn-font-size: 1.1rem;
    --bs-btn-border-radius: 0.5rem;
  }
</style>

<div class="custom-buttons">
  <button class="btn btn-primary">Custom Primary</button>
  <button class="btn btn-secondary">Custom Secondary</button>
  <button class="btn btn-success">Custom Success</button>
</div>
```

Modal variables control the dialog appearance.

```html
<style>
  /* Custom modal styling */
  .custom-modal {
    --bs-modal-bg: #f8f9fa;
    --bs-modal-border-color: #dee2e6;
    --bs-modal-border-radius: 1rem;
    --bs-modal-header-border-color: transparent;
    --bs-modal-footer-border-color: transparent;
    --bs-modal-header-padding: 2rem 2rem 1rem;
    --bs-modal-inner-border-radius: 1rem;
  }
</style>

<div class="modal-dialog">
  <div class="modal-content custom-modal">
    <div class="modal-header">
      <h5 class="modal-title">Custom Modal</h5>
    </div>
    <div class="modal-body">
      Modal with custom variable overrides
    </div>
  </div>
</div>
```

## Advanced Variations

Card component variables for themed card designs.

```html
<style>
  .dark-card-theme {
    --bs-card-bg: #2d3339;
    --bs-card-color: #e9ecef;
    --bs-card-border-color: #495057;
    --bs-card-cap-bg: #343a40;
    --bs-card-cap-color: #f8f9fa;
    --bs-card-border-radius: 0.75rem;
  }
</style>

<div class="card dark-card-theme">
  <div class="card-header">Dark Themed Header</div>
  <div class="card-body">
    <h5 class="card-title">Dark Card</h5>
    <p class="card-text">This card uses custom CSS variables for a dark theme.</p>
    <a href="#" class="btn btn-primary">Action</a>
  </div>
  <div class="card-footer text-muted">Footer with custom styling</div>
</div>
```

Navbar variables for branded navigation.

```html
<style>
  .brand-navbar {
    --bs-navbar-padding-y: 1rem;
    --bs-navbar-brand-padding-y: 0;
    --bs-navbar-brand-font-size: 1.5rem;
    --bs-navbar-brand-color: #ff6600;
    --bs-navbar-brand-hover-color: #cc5200;
    --bs-navbar-color: rgba(255, 255, 255, 0.85);
    --bs-navbar-hover-color: #fff;
    --bs-navbar-active-color: #ff6600;
    --bs-navbar-toggler-border-color: rgba(255, 255, 255, 0.25);
  }
</style>

<nav class="navbar navbar-expand-lg bg-dark brand-navbar">
  <div class="container">
    <a class="navbar-brand" href="#">BrandName</a>
    <div class="navbar-nav">
      <a class="nav-link active" href="#">Home</a>
      <a class="nav-link" href="#">Products</a>
      <a class="nav-link" href="#">Contact</a>
    </div>
  </div>
</nav>
```

Dropdown and offcanvas variables for consistent theming.

```html
<style>
  .themed-components {
    --bs-dropdown-bg: #1e1e2e;
    --bs-dropdown-color: #cdd6f4;
    --bs-dropdown-border-color: #45475a;
    --bs-dropdown-link-color: #cdd6f4;
    --bs-dropdown-link-hover-bg: #45475a;
    --bs-dropdown-link-active-bg: #ff6600;

    --bs-offcanvas-bg: #1e1e2e;
    --bs-offcanvas-color: #cdd6f4;
    --bs-offcanvas-border-color: #45475a;
  }
</style>
```

## Best Practices

1. **Use component variables over global overrides** - Prefer `--bs-btn-*` over global color variables when changes are button-specific.
2. **Scope overrides to wrapper classes** - Apply component variable overrides within a parent class to avoid global side effects.
3. **Override all related variables** - When customizing a component, update hover, active, and focus state variables for consistency.
4. **Document component customizations** - Maintain a reference of which components use custom variables and their values.
5. **Test all component states** - Verify hover, focus, active, disabled, and open states render correctly with custom variables.
6. **Combine with utility classes** - Use utility classes alongside component variables for fine-tuned adjustments.
7. **Avoid deep nesting of overrides** - Keep component variable overrides at one or two levels to prevent cascade conflicts.
8. **Use variables for white-labeling** - Component variables are ideal for multi-brand systems where each brand needs unique component styling.
9. **Preserve accessibility** - Custom component colors must maintain WCAG contrast requirements.
10. **Inspect components in DevTools** - Use the browser's computed styles to discover available component variables before overriding.

## Common Pitfalls

1. **Overriding only base variables** - Changing `--bs-primary` does not update `--bs-btn-color` or `--bs-btn-bg`. Component variables must be set explicitly.
2. **Ignoring state variables** - Customizing `--bs-btn-bg` without updating `--bs-btn-hover-bg` creates inconsistent hover states.
3. **Variable scope confusion** - Component variables defined on a class only affect elements within that class. Global overrides require `:root`.
4. **Missing component CSS** - Some components must be imported for their CSS variables to be available. Ensure all needed components are included.
5. **Conflicting specificity** - Custom CSS selectors with higher specificity can override component variable declarations. Use equal or lower specificity.

## Accessibility Considerations

Component variable overrides must preserve accessibility standards. Custom button colors need sufficient contrast for all states (default, hover, focus, active). Focus indicators controlled by `--bs-btn-focus-*` variables must remain visible. Modal backgrounds and text colors must meet contrast requirements. Navigation items need clear active and hover states. When customizing component variables, test with keyboard navigation, screen readers, and high contrast mode to ensure all users can interact with the components effectively.

## Responsive Behavior

Component CSS variables do not have responsive variants by default. To create responsive component behavior, override variables within media queries. For example, increase `--bs-modal-dialog-margin` on mobile for better full-screen modal behavior, or adjust `--bs-card-spacer-x` for different padding at various breakpoints. Some components like modals and offcanvas already adapt their variables based on viewport through Bootstrap's internal media queries, providing built-in responsive behavior that custom overrides should complement rather than conflict with.
