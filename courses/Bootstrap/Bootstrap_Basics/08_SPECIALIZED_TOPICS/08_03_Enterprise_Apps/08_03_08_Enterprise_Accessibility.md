---
title: "Enterprise Accessibility"
module: "Enterprise Apps"
difficulty: 2
estimated_time: "30 min"
prerequisites: ["02_02_Utilities", "04_05_Forms", "04_01_Card_Component"]
---

## Overview

Enterprise applications must meet strict accessibility standards including WCAG 2.1 AAA compliance, Section 508 requirements, and organizational accessibility policies. Bootstrap 5 provides accessible components out of the box, but enterprise applications require additional attention to focus management, color contrast, keyboard navigation, and screen reader support.

## Basic Implementation

### Skip Navigation Link

```html
<!-- Skip link should be the first element in the body -->
<a href="#main-content" class="visually-hidden-focusable btn btn-primary position-absolute" style="z-index:9999;top:10px;left:10px">
  Skip to main content
</a>

<!-- Main navigation -->
<nav class="navbar navbar-expand-lg navbar-dark bg-dark" aria-label="Main navigation">
  <div class="container-fluid">
    <a class="navbar-brand" href="#">Enterprise App</a>
    <ul class="navbar-nav">
      <li class="nav-item"><a class="nav-link active" aria-current="page" href="#">Dashboard</a></li>
      <li class="nav-item"><a class="nav-link" href="#">Reports</a></li>
    </ul>
  </div>
</nav>

<!-- Main content with matching ID -->
<main id="main-content" tabindex="-1">
  <div class="container py-4">
    <h1>Dashboard</h1>
  </div>
</main>
```

### Accessible Form with Error Handling

```html
<form novalidate>
  <div class="mb-3">
    <label for="email" class="form-label">
      Email Address <span class="text-danger" aria-hidden="true">*</span>
    </label>
    <input type="email" class="form-control is-invalid" id="email"
           aria-describedby="emailHelp emailError" aria-invalid="true" required>
    <div id="emailHelp" class="form-text">We'll never share your email.</div>
    <div id="emailError" class="invalid-feedback" role="alert">
      Please enter a valid email address.
    </div>
  </div>
  <div class="mb-3">
    <label for="department" class="form-label">Department</label>
    <select class="form-select" id="department" aria-describedby="deptHelp">
      <option value="">Select a department</option>
      <option value="eng">Engineering</option>
      <option value="sales">Sales</option>
    </select>
    <div id="deptHelp" class="form-text">Your department determines dashboard access.</div>
  </div>
  <button type="submit" class="btn btn-primary">Submit</button>
</form>
```

## Advanced Variations

### High Contrast Mode Support

```html
<style>
  @media (prefers-contrast: high) {
    .card { border: 2px solid #000; }
    .badge { border: 1px solid currentColor; }
    .btn-outline-primary { border-width: 2px; }
    .form-control:focus { outline: 3px solid #000; outline-offset: 2px; }
  }
</style>

<!-- High contrast alert -->
<div class="alert alert-primary border-2" role="alert" style="border-color: #0056b3 !important">
  <i class="bi bi-info-circle me-2" aria-hidden="true"></i>
  <strong>Notice:</strong> This system supports high contrast mode for improved visibility.
</div>
```

### Focus Management Pattern

```html
<!-- Focus trap for modals -->
<div class="modal fade" id="accessibleModal" tabindex="-1" aria-labelledby="modalTitle" aria-modal="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="modalTitle">Confirm Action</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close modal"></button>
      </div>
      <div class="modal-body">
        <p>Are you sure you want to proceed with this action?</p>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button type="button" class="btn btn-primary" id="confirmBtn">Confirm</button>
      </div>
    </div>
  </div>
</div>

<!-- Live region for dynamic updates -->
<div aria-live="polite" aria-atomic="true" class="visually-hidden" id="liveRegion"></div>

<script>
  // Announce actions to screen readers
  function announce(message) {
    const region = document.getElementById('liveRegion');
    region.textContent = message;
    setTimeout(() => { region.textContent = ''; }, 1000);
  }
</script>
```

### Accessible Data Table

```html
<table class="table" role="table" aria-label="Employee directory">
  <caption class="visually-hidden">List of employees with their department and status</caption>
  <thead class="table-light">
    <tr>
      <th scope="col" aria-sort="ascending">
        <button class="btn btn-link p-0 text-decoration-none">
          Name <i class="bi bi-arrow-up" aria-hidden="true"></i>
        </button>
      </th>
      <th scope="col">Department</th>
      <th scope="col">Status</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <th scope="row">John Doe</th>
      <td>Engineering</td>
      <td><span class="badge bg-success" role="status">Active</span></td>
    </tr>
  </tbody>
</table>
```

### Reduced Motion Support

```html
<style>
  @media (prefers-reduced-motion: reduce) {
    *, *::before, *::after {
      animation-duration: 0.01ms !important;
      animation-iteration-count: 1 !important;
      transition-duration: 0.01ms !important;
    }
    .carousel-item { transition: none; }
  }
</style>

<!-- Animated element that respects user preference -->
<div class="progress" role="progressbar" aria-valuenow="65" aria-valuemin="0" aria-valuemax="100" aria-label="Upload progress">
  <div class="progress-bar progress-bar-striped progress-bar-animated" style="width: 65%">65%</div>
</div>
```

## Best Practices

1. Include a "Skip to main content" link as the first focusable element
2. Use `aria-current="page"` on active navigation items
3. Associate form errors with inputs using `aria-describedby` and `aria-invalid`
4. Provide visible focus indicators on all interactive elements (3:1 contrast minimum)
5. Support `prefers-reduced-motion` media query for animations
6. Support `prefers-contrast: high` for users needing higher contrast
7. Use `aria-live` regions to announce dynamic content changes
8. Ensure all interactive elements are keyboard-accessible (Tab, Enter, Escape)
9. Provide `aria-label` on icon-only buttons
10. Use semantic HTML (`<main>`, `<nav>`, `<header>`, `<footer>`)
11. Maintain a minimum 4.5:1 contrast ratio for normal text (AAA: 7:1)
12. Test with screen readers (NVDA, JAWS, VoiceOver) regularly
13. Use `role="alert"` for error messages and important notifications
14. Provide a visible focus ring using Bootstrap's `focus-ring` utility

## Common Pitfalls

1. **No skip navigation** - Keyboard users must tab through the entire nav on every page.
2. **Missing aria-labels on icon buttons** - Screen readers announce "button" with no context.
3. **Color-only status indicators** - Color-blind users can't distinguish status. Use icons and text.
4. **Animations without reduced-motion support** - Motion-sensitive users may experience discomfort.
5. **Focus not managed on route changes** - Single-page apps must move focus to the new content.
6. **Insufficient contrast** - Gray text on white backgrounds often fails WCAG AA.
7. **Missing form labels** - Placeholder text alone isn't sufficient. Always use `<label>`.

## Accessibility Considerations

This entire module is about accessibility. Key enterprise requirements include:

- **WCAG 2.1 Level AA** minimum, with AAA for critical workflows
- **Section 508** compliance for US government contracts
- **EN 301 549** for European accessibility requirements
- Regular automated testing with tools like axe-core
- Manual testing with assistive technologies
- Accessibility statement published on the application

## Responsive Behavior

Accessibility patterns are viewport-independent but adapt naturally. Skip links become more important on mobile where keyboard navigation is less common but switch devices are used. Touch targets must be at least 44x44px on mobile. Focus indicators should be visible at all screen sizes. High contrast mode should work at all breakpoints.
