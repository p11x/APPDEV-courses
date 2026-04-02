---
title: "Accessibility Code Review for Bootstrap"
module: "Code Review"
difficulty: 2
estimated_time: 25
tags: ["accessibility", "ARIA", "keyboard", "screen-reader", "WCAG"]
prerequisites: ["WCAG 2.1 basics", "Bootstrap 5 components"]
---

## Overview

Accessibility code review ensures that Bootstrap components are usable by people with disabilities. This involves validating ARIA attributes, keyboard navigation patterns, screen reader compatibility, color contrast ratios, and focus management. Bootstrap 5 includes built-in accessibility features, but developers must implement them correctly and supplement with additional attributes where needed. A thorough accessibility review catches issues that automated tools often miss.

## Basic Implementation

**ARIA Labeling Review**

Verify that interactive elements have accessible names. Bootstrap components often require specific ARIA attributes.

```html
<!-- Modal with proper ARIA -->
<div class="modal fade" id="deleteConfirm" tabindex="-1" aria-labelledby="deleteTitle" aria-modal="true" role="dialog">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="deleteTitle">Confirm Deletion</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close modal"></button>
      </div>
      <div class="modal-body">
        <p>This action cannot be undone.</p>
      </div>
      <div class="modal-footer">
        <button class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button class="btn btn-danger">Delete</button>
      </div>
    </div>
  </div>
</div>
```

**Keyboard Navigation Check**

Ensure all interactive components are reachable and operable via keyboard alone.

```html
<!-- Dropdown with keyboard support (Bootstrap handles this) -->
<div class="dropdown">
  <button class="btn btn-primary dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
    Actions
  </button>
  <ul class="dropdown-menu">
    <li><a class="dropdown-item" href="#">Edit</a></li>
    <li><a class="dropdown-item" href="#">Duplicate</a></li>
    <li><hr class="dropdown-divider"></li>
    <li><a class="dropdown-item text-danger" href="#">Delete</a></li>
  </ul>
</div>
```

**Form Accessibility Audit**

Every input must have an associated label, and validation feedback must be announced to screen readers.

```html
<div class="mb-3">
  <label for="emailInput" class="form-label">Email address</label>
  <input type="email" class="form-control" id="emailInput" aria-describedby="emailHelp emailError" required>
  <div id="emailHelp" class="form-text">We'll never share your email.</div>
  <div id="emailError" class="invalid-feedback">Please enter a valid email address.</div>
</div>
```

## Advanced Variations

**Focus Management in Dynamic Content**

When content changes dynamically (modals, tabs, accordions), verify that focus is managed correctly.

```javascript
// Custom focus management for dynamic content
const liveRegion = document.getElementById('notifications');
function announceUpdate(message) {
  liveRegion.textContent = '';
  setTimeout(() => {
    liveRegion.textContent = message;
  }, 100);
}

// Review: Focus returns to trigger after modal closes
const modal = document.getElementById('settingsModal');
modal.addEventListener('hidden.bs.modal', () => {
  document.querySelector('[data-bs-target="#settingsModal"]').focus();
});
```

**Skip Navigation Links**

Verify that skip links are present and functional for keyboard users.

```html
<body>
  <a href="#main-content" class="visually-hidden-focusable btn btn-primary position-absolute top-0 start-0 z-3">
    Skip to main content
  </a>
  <nav class="navbar">...</nav>
  <main id="main-content" class="container" tabindex="-1">
    ...
  </main>
</body>
```

**Live Region Announcements**

For AJAX content updates, ensure ARIA live regions announce changes to screen readers.

```html
<div id="search-results" aria-live="polite" aria-atomic="true" class="visually-hidden">
  <!-- Screen reader announces updates here -->
</div>
<div id="results-container" class="row">
  <!-- Dynamically loaded results -->
</div>
```

## Best Practices

1. **Add `aria-label` or `aria-labelledby`** to every landmark region
2. **Test with keyboard only** - Tab through every interactive element on each page
3. **Verify focus order matches visual order** - CSS visual reordering must not break tab sequence
4. **Use `aria-expanded` on disclosure triggers** - Accordions, dropdowns, and collapse buttons
5. **Maintain a 4.5:1 contrast ratio** for normal text and 3:1 for large text (WCAG AA)
6. **Provide visible focus indicators** - Never use `outline: none` without a replacement
7. **Use `role="alert"` for error messages** - Ensure validation errors are announced immediately
8. **Test with at least two screen readers** - NVDA/JAWS on Windows and VoiceOver on macOS
9. **Add `aria-hidden="true"` to decorative icons** - Icons inside buttons with text labels are decorative
10. **Use `aria-current="page"` on active navigation links** - Helps screen readers identify current location
11. **Ensure all images have meaningful `alt` text** or `alt=""` if purely decorative
12. **Test with browser zoom at 200%** - Content must remain functional and readable

## Common Pitfalls

1. **Missing `aria-labelledby` on modals** - Screen readers cannot associate the title with the dialog
2. **Using `tabindex` values greater than 0** - Disrupts natural tab order unpredictably
3. **Removing focus outlines** - `outline: none` without replacement makes keyboard navigation invisible
4. **Forgetting `aria-expanded` state toggling** - Dropdowns and accordions show incorrect state
5. **Color-only error indicators** - Using only red text without icons or text fails color-blind users
6. **Auto-playing content without controls** - Carousels must pause on hover/focus and provide stop controls
7. **Missing `alt` text on informative images** - Decorative images should use `alt=""` explicitly
8. **Using `div` for interactive controls** - Custom buttons need `role="button"`, `tabindex="0"`, and keyboard handlers
9. **Trap focus inappropriately** - Modals must trap focus, but other components should not
10. **Ignoring touch target sizes** - Interactive elements should be at least 44x44 CSS pixels

## Accessibility Considerations

Conduct accessibility reviews using a combination of automated tools (axe-core, Lighthouse), manual keyboard testing, and screen reader verification. Document findings with WCAG success criteria references. Prioritize issues by impact: critical (blocks users), major (significantly impairs), and minor (inconvenient). Ensure that Bootstrap's `visually-hidden` class is used correctly for screen-reader-only content, and that all dynamic content updates are announced through ARIA live regions.

## Responsive Behavior

Accessibility must be maintained across all breakpoints. Verify that touch targets remain at least 44px on mobile devices. Ensure that collapsed navigation is fully keyboard accessible. Check that text reflow at 200% zoom does not cause content overlap or loss. Validate that horizontal scrolling is not required at 320px viewport width (WCAG 1.4.10). Test assistive technology interactions at mobile breakpoints where touch and screen reader modes differ from desktop.
