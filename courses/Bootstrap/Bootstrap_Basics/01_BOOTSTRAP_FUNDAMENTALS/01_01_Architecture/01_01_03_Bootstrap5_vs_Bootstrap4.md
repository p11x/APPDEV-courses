---
tags: [bootstrap, migration, bootstrap4, bootstrap5, breaking-changes, rtl]
category: Bootstrap Fundamentals
difficulty: 2
estimated_time: 35 minutes
---

# Bootstrap 5 vs Bootstrap 4

## Overview

Bootstrap 5, released in May 2021, represented a major rewrite of the framework with significant breaking changes, new features, and architectural improvements over Bootstrap 4. Understanding these differences is critical for migration planning and for developers maintaining projects across both versions.

The most significant architectural change is the **removal of jQuery as a dependency**. Bootstrap 4 required jQuery for all JavaScript plugins; Bootstrap 5 replaced it with vanilla JavaScript, reducing bundle size and removing a major dependency for modern applications.

Key changes include: **RTL (Right-to-Left) support** built into the core, **CSS custom properties** for runtime theming, a **new utility API** for generating classes programmatically, **expanded grid system** with xxl breakpoint, **dropped support for IE11**, renamed classes and components, restructured JavaScript plugins, and an enhanced form system.

The migration from Bootstrap 4 to 5 requires systematic updates to class names, data attributes, JavaScript initialization, and potentially Sass variable overrides. This guide provides a comprehensive mapping of every breaking change.

```html
<!-- Bootstrap 4 - jQuery dependency -->
<script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.min.js"></script>

<!-- Bootstrap 5 - No jQuery, single bundle -->
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
```

## Basic Implementation

The most visible migration changes are in class naming conventions and data attribute prefixes. Bootstrap 5 replaced `data-toggle`, `data-target`, `data-dismiss`, and `data-parent` with `data-bs-*` prefixed attributes.

```html
<!-- Bootstrap 4 Modal Trigger -->
<button data-toggle="modal" data-target="#myModal">Open Modal</button>
<div class="modal fade" id="myModal">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Title</h5>
        <button type="button" class="close" data-dismiss="modal">
          <span>&times;</span>
        </button>
      </div>
      <div class="modal-body">Content</div>
    </div>
  </div>
</div>

<!-- Bootstrap 5 Modal Trigger -->
<button data-bs-toggle="modal" data-bs-target="#myModal">Open Modal</button>
<div class="modal fade" id="myModal" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="myModalLabel">Title</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">Content</div>
    </div>
  </div>
</div>
```

Component class renames are extensive. Here is a summary of the most common changes:

```html
<!-- Bootstrap 4 → Bootstrap 5 Class Renames -->

<!-- Badges: font-weight utilities replaced -->
<span class="badge badge-primary">BS4</span>
<span class="badge bg-primary">BS5</span>

<!-- Close buttons: text replaced with aria-label -->
<button class="close"><span>&times;</span></button>
<button class="btn-close" aria-label="Close"></button>

<!-- Form controls: custom-* prefix removed -->
<div class="custom-control custom-checkbox">
  <input type="checkbox" class="custom-control-input" id="check1">
  <label class="custom-control-label" for="check1">Check</label>
</div>
<div class="form-check">
  <input class="form-check-input" type="checkbox" id="check1">
  <label class="form-check-label" for="check1">Check</label>
</div>

<!-- Jumbotron removed — use utilities instead -->
<div class="jumbotron"><h1>Hello</h1></div>
<div class="p-5 mb-4 bg-light rounded-3"><h1 class="display-4">Hello</h1></div>

<!-- Media object removed — use flex utilities -->
<div class="media">
  <img class="mr-3" src="img.jpg">
  <div class="media-body">Content</div>
</div>
<div class="d-flex">
  <img class="me-3" src="img.jpg">
  <div>Content</div>
</div>
```

## Advanced Variations

Bootstrap 5 introduced a completely new JavaScript plugin architecture. Each plugin is a standalone ES module with its own instance methods, events, and configuration options.

```javascript
// Bootstrap 4 - jQuery plugin pattern
$('#myModal').modal('show');
$('#myModal').on('shown.bs.modal', function () {
  console.log('Modal shown');
});

// Bootstrap 5 - Vanilla JS class-based API
const myModal = new bootstrap.Modal(document.getElementById('myModal'));
myModal.show();
myModal._element.addEventListener('shown.bs.modal', function () {
  console.log('Modal shown');
});

// Bootstrap 5 - Static methods for finding existing instances
const existingModal = bootstrap.Modal.getInstance(document.getElementById('myModal'));

// Bootstrap 5 - getOrCreateInstance creates or returns existing
const modal = bootstrap.Modal.getOrCreateInstance(document.getElementById('myModal'));

// Bootstrap 5 - Programmatic component creation
const toast = new bootstrap.Toast(document.getElementById('myToast'), {
  animation: true,
  autohide: true,
  delay: 5000
});
```

The directional utility renames are extensive. Bootstrap 5 replaced `left`/`right` with `start`/`end` to support RTL layouts:

```html
<!-- Directional utility renames (BS4 → BS5) -->
<!-- Margins -->
<div class="ml-3">Left</div>     → <div class="ms-3">Start</div>
<div class="mr-3">Right</div>    → <div class="me-3">End</div>
<div class="ml-auto">Auto</div>  → <div class="ms-auto">Auto</div>

<!-- Padding -->
<div class="pl-3">Pad left</div>  → <div class="ps-3">Pad start</div>
<div class="pr-3">Pad right</div> → <div class="pe-3">Pad end</div>

<!-- Text alignment -->
<p class="text-left">Left</p>    → <p class="text-start">Start</p>
<p class="text-right">Right</p>  → <p class="text-end">End</p>

<!-- Float -->
<div class="float-left">Left</div>  → <div class="float-start">Start</div>
<div class="float-right">Right</div> → <div class="float-end">End</div>

<!-- Border -->
<div class="border-left"></div>  → <div class="border-start"></div>
<div class="border-right"></div> → <div class="border-end"></div>

<!-- Rounded -->
<div class="rounded-left"></div>  → <div class="rounded-start"></div>
<div class="rounded-right"></div> → <div class="rounded-end"></div>
```

The Sass variable system was restructured with new variable categories and naming:

```scss
// Bootstrap 4 variables that changed in Bootstrap 5

// BS4: $grid-breakpoints
// BS5: $grid-breakpoints (same structure, added xxl)
$grid-breakpoints: (
  xs: 0,
  sm: 576px,
  md: 768px,
  lg: 992px,
  xl: 1200px,
  xxl: 1400px  // NEW in BS5
) !default;

// BS4: $enable-rounded, $enable-shadows
// BS5: Same, plus new flags
$enable-rounded: true !default;
$enable-shadows: false !default;
$enable-gradients: false !default;
$enable-transitions: true !default;
$enable-reduced-motion: true !default;  // NEW
$enable-smooth-scroll: true !default;   // NEW
$enable-grid-classes: true !default;
$enable-cssgrid: false !default;        // NEW in BS5.3
```

## Best Practices

- **Perform a comprehensive audit** before migration — document all Bootstrap 4 class usages, JavaScript plugin initializations, and Sass overrides to create a complete migration scope.
- **Use find-and-replace with regex** for mechanical class renames (e.g., `ml-` → `ms-`, `mr-` → `me-`, `pl-` → `ps-`, `pr-` → `pe-`).
- **Test JavaScript plugins independently** after migration — the API is fundamentally different; jQuery event handlers no longer work.
- **Update all `data-*` attributes** systematically; a single missing `data-bs-*` prefix breaks the associated plugin.
- **Replace removed components early** — jumbotron, media object, and card decks have no direct equivalents; rebuild them with utility classes.
- **Verify RTL behavior** if your project supports multilingual audiences; Bootstrap 5's RTL support is automatic with the correct HTML `dir` attribute.
- **Remove jQuery and Popper.js from dependencies** after completing the migration to realize bundle size savings.
- **Review spacing utility direction changes** carefully; `ml-3` becoming `ms-3` is straightforward, but `ml-auto` becoming `ms-auto` may interact differently with flexbox in RTL contexts.
- **Update form markup** to use the new form control classes (`form-check`, `form-switch`, `form-range`) instead of the removed `custom-*` prefix classes.
- **Run a full visual regression test** across all breakpoints after migration to catch subtle layout differences.
- **Document all migration decisions** in a `MIGRATION.md` file for team reference and future audits.

## Common Pitfalls

- **Mixing Bootstrap 4 and 5 classes** — using `ml-3` alongside `ms-3` creates conflicting or redundant styles with unpredictable results.
- **Forgetting to add `tabindex="-1"` and `aria-hidden="true"`** on modals — Bootstrap 5 requires these for accessibility; Bootstrap 4 did not enforce them.
- **Using jQuery event binding with Bootstrap 5 plugins** — `$('#modal').on('shown.bs.modal', ...)` no longer works; use `addEventListener` or the Bootstrap 5 API.
- **Expecting `card-deck` to work** — it was removed in Bootstrap 5; use the grid system (`row`/`col`) with `h-100` on cards instead.
- **Not updating close buttons** — `<button class="close">&times;</button>` must become `<button class="btn-close" aria-label="Close"></button>`; the `&times;` character is removed.
- **Ignoring the xxl breakpoint** — layouts using `col-xl-3` will display differently if the design assumed a max container width of 1140px (now 1320px at xxl).
- **Missing ARIA attributes on interactive components** — Bootstrap 5 enforces stricter accessibility patterns that require `aria-label`, `aria-expanded`, and `aria-controls`.

## Accessibility Considerations

Bootstrap 5 made accessibility a first-class concern. Every interactive component now includes proper ARIA attributes, keyboard navigation support, and screen reader announcements by default.

Key accessibility changes from Bootstrap 4:
- Close buttons use `aria-label="Close"` instead of visible `&times;` characters.
- Modals require `aria-labelledby` and `aria-hidden="true"` attributes.
- Dropdowns include `aria-expanded` state management.
- Form validation messages use `aria-describedby` to associate error text with inputs.
- Focus trapping is built into modals and offcanvas components.

```html
<!-- Bootstrap 5 accessible dropdown -->
<div class="dropdown">
  <button class="btn btn-secondary dropdown-toggle" 
          type="button" 
          data-bs-toggle="dropdown" 
          aria-expanded="false"
          aria-haspopup="true">
    Dropdown
  </button>
  <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton">
    <li><a class="dropdown-item" href="#">Action</a></li>
    <li><a class="dropdown-item" href="#">Another action</a></li>
  </ul>
</div>
```

## Responsive Behavior

Bootstrap 5 added the `xxl` breakpoint (≥1400px) and expanded container maximum widths. This affects how layouts render on large monitors. The container max widths in Bootstrap 5 are: sm: 540px, md: 720px, lg: 960px, xl: 1140px, xxl: 1320px.

The new `xxl` breakpoint means that responsive classes like `col-xxl-2` and `d-xxl-none` are available. Projects migrating from Bootstrap 4 should verify that layouts don't break on ultra-wide screens where the new breakpoint takes effect.

Bootstrap 5.3+ also introduced CSS Grid support alongside the existing Flexbox grid, enabling two different layout systems to coexist. The `.cssgrid` container and `.g-col-*` classes provide an alternative grid system with features like `gap` support without margin hacks.

```html
<!-- Bootstrap 5 CSS Grid (5.3+) -->
<div class="grid gap-3">
  <div class="g-col-4">Column 1</div>
  <div class="g-col-4">Column 2</div>
  <div class="g-col-4">Column 3</div>
</div>

<!-- Responsive CSS Grid -->
<div class="grid">
  <div class="g-col-6 g-col-md-4">Responsive</div>
  <div class="g-col-6 g-col-md-8">Responsive</div>
</div>
```
