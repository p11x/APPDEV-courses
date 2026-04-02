---
title: "Bootstrap Code Review Checklist"
module: "Code Review"
difficulty: 2
estimated_time: 20
tags: ["code-review", "checklist", "quality", "standards"]
prerequisites: ["Bootstrap 5 fundamentals", "HTML/CSS proficiency"]
---

## Overview

A structured code review checklist ensures consistent quality across Bootstrap projects. This checklist covers class usage validation, accessibility compliance, responsive behavior verification, and performance considerations. Using a standardized checklist reduces subjective feedback and creates measurable quality gates for every pull request involving Bootstrap components.

## Basic Implementation

**HTML Structure Review**

Verify that Bootstrap components follow the correct markup hierarchy. Incorrect nesting often causes layout and styling issues.

```html
<!-- CORRECT: Proper card structure -->
<div class="card">
  <img src="photo.jpg" class="card-img-top" alt="Product image">
  <div class="card-body">
    <h5 class="card-title">Product Name</h5>
    <p class="card-text">Description here.</p>
  </div>
</div>

<!-- INCORRECT: Missing card-body wrapper -->
<div class="card">
  <h5 class="card-title">Product Name</h5>
  <p class="card-text">Description here.</p>
</div>
```

**Grid System Validation**

Check that column classes add up to 12 within each row and that breakpoints follow mobile-first ordering.

```html
<!-- CORRECT: Columns sum to 12, mobile-first order -->
<div class="row">
  <div class="col-12 col-md-6 col-lg-4">Content</div>
  <div class="col-12 col-md-6 col-lg-4">Content</div>
  <div class="col-12 col-lg-4">Content</div>
</div>

<!-- INCORRECT: Columns exceed 12 -->
<div class="row">
  <div class="col-8 col-md-6">Content</div>
  <div class="col-8 col-md-8">Content</div>
</div>
```

**Utility Class Audit**

Ensure spacing, color, and display utilities are used consistently and do not override component defaults unexpectedly.

```html
<!-- Review: Confirm utility intent -->
<div class="d-flex justify-content-between align-items-center p-3 mb-4 bg-light rounded">
  <span>Left content</span>
  <span>Right content</span>
</div>
```

## Advanced Variations

**Component Composition Review**

When components are nested, verify that the combination is supported and does not create conflicting styles.

```html
<!-- Review modal containing form with validation -->
<div class="modal fade" id="createForm" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Create Item</h5>
      </div>
      <div class="modal-body">
        <form class="needs-validation" novalidate>
          <div class="mb-3">
            <label for="itemName" class="form-label">Name</label>
            <input type="text" class="form-control" id="itemName" required>
            <div class="invalid-feedback">Name is required.</div>
          </div>
        </form>
      </div>
      <div class="modal-footer">
        <button class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button class="btn btn-primary" type="submit">Save</button>
      </div>
    </div>
  </div>
</div>
```

**Custom Theme Consistency**

Review that custom CSS variables align with the Bootstrap theme and do not introduce conflicting specificity.

```scss
// Review: Custom overrides should extend, not replace
$primary: #2563eb;
$theme-colors: (
  "primary": $primary,
  "accent": #7c3aed
);
@import "bootstrap/scss/bootstrap";
```

**JavaScript Integration Check**

Verify that Bootstrap JS plugins are initialized correctly and event listeners are attached properly.

```javascript
// Review: Proper initialization pattern
document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(el => {
  new bootstrap.Tooltip(el);
});
```

## Best Practices

1. **Use semantic HTML elements** (`<nav>`, `<main>`, `<section>`) instead of generic `<div>` wrappers
2. **Validate grid column sums** to ensure they equal 12 at each breakpoint
3. **Follow mobile-first breakpoint order** (`col-12 col-md-6 col-lg-4`)
4. **Avoid overriding Bootstrap component styles** with `!important` declarations
5. **Use Bootstrap variables** for customizations rather than hard-coded values
6. **Check ARIA attributes** on interactive components (modals, dropdowns, tooltips)
7. **Verify keyboard navigation** for all interactive elements
8. **Confirm responsive images** use `img-fluid` or appropriate sizing classes
9. **Validate form markup** includes proper labels, feedback messages, and `novalidate` handling
10. **Ensure spacing consistency** using Bootstrap's spacing scale (`mt-3`, `mb-4`, `p-2`)
11. **Remove unused utility classes** that add unnecessary CSS weight
12. **Test print styles** to ensure content is readable when printed

## Common Pitfalls

1. **Wrapping grid columns in extra divs** - Adding non-column elements directly inside `.row` breaks the flex layout
2. **Forgetting `data-bs-dismiss` on modal close buttons** - Modals cannot be closed without this attribute
3. **Using `col-auto` without content constraints** - Columns may overflow on small screens
4. **Missing `alt` attributes on images** - Breaks accessibility and causes review failures
5. **Hard-coding pixel values** instead of using Bootstrap's spacing and sizing utilities
6. **Nesting `.container` elements** - Creates unexpected horizontal padding and max-width issues
7. **Mixing Bootstrap 4 and 5 class names** - Classes like `ml-3` (v4) vs `ms-3` (v5) are incompatible
8. **Using `btn-outline-*` without sufficient contrast** - Fails WCAG AA on light backgrounds
9. **Placing dropdown menus inside overflow-hidden containers** - Menus get clipped
10. **Ignoring JavaScript console errors** - Bootstrap JS errors often indicate broken component behavior

## Accessibility Considerations

Review all interactive components for proper ARIA roles and attributes. Modals must include `aria-labelledby` pointing to the modal title. Dropdowns require `aria-expanded` toggling. Forms need associated `<label>` elements or `aria-label`/`aria-labelledby` alternatives. Verify focus management works correctly - modals should trap focus, dropdowns should support arrow key navigation, and tab panels should respond to keyboard input. Test with screen readers to confirm announcements match expected behavior. Color contrast ratios must meet WCAG AA standards (4.5:1 for normal text, 3:1 for large text).

## Responsive Behavior

Verify components render correctly at all standard breakpoints: xs (<576px), sm (576px), md (768px), lg (992px), xl (1200px), and xxl (1400px). Check that navigation collapses appropriately on mobile. Confirm tables either scroll horizontally or restructure for small screens using responsive utilities. Validate that images scale properly and do not overflow containers. Test touch interactions on mobile devices for components like carousels and offcanvas. Ensure modals are usable on small viewports and do not exceed screen height.
