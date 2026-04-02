---
title: Bootstrap 4 to 5 Migration Guide
category: Professional
difficulty: 3
time: 45 min
tags: bootstrap5, migration, upgrade, class-changes, data-attributes, rtl
---

# Bootstrap 4 to 5 Migration Guide

## Overview

Migrating from Bootstrap 4 to Bootstrap 5 involves significant changes across CSS class names, JavaScript plugins, data attributes, and the removal of jQuery dependency. Bootstrap 5 introduces RTL support, drops IE11 compatibility, renames numerous utility classes, and replaces `data-` attributes with `data-bs-` prefixed versions. A systematic approach prevents regressions and ensures the upgrade completes without breaking existing functionality.

## Basic Implementation

The most immediate change is replacing jQuery-dependent Bootstrap 4 code with vanilla JavaScript equivalents:

```html
<!-- Bootstrap 4: jQuery dependency -->
<button data-toggle="modal" data-target="#exampleModal">Open</button>
<div class="modal" id="exampleModal">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Title</h5>
        <button type="button" class="close" data-dismiss="modal">
          <span>&times;</span>
        </button>
      </div>
    </div>
  </div>
</div>

<!-- Bootstrap 5: Vanilla JS, data-bs-* attributes -->
<button data-bs-toggle="modal" data-bs-target="#exampleModal">Open</button>
<div class="modal" id="exampleModal">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Title</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>
    </div>
  </div>
</div>
```

Key class renames:

```html
<!-- Bootstrap 4 → Bootstrap 5 -->
<!-- Badge -->
<span class="badge badge-primary">Tag</span>
<span class="badge bg-primary">Tag</span>

<!-- Close button -->
<button class="close">&times;</button>
<button class="btn-close"></button>

<!-- Float -->
<div class="float-left"></div>
<div class="float-start"></div>

<!-- Margin/Padding directional -->
<div class="ml-3 mr-2 pl-4 pr-1"></div>
<div class="ms-3 me-2 ps-4 pe-1"></div>

<!-- Text alignment -->
<div class="text-left text-right"></div>
<div class="text-start text-end"></div>

<!-- Font weight -->
<p class="font-weight-bold font-italic"></p>
<p class="fw-bold fst-italic"></p>
```

## Advanced Variations

Automated migration with find-and-replace patterns:

```bash
# Data attribute migration
find . -name "*.html" -exec sed -i 's/data-toggle=/data-bs-toggle=/g' {} +
find . -name "*.html" -exec sed -i 's/data-target=/data-bs-target=/g' {} +
find . -name "*.html" -exec sed -i 's/data-dismiss=/data-bs-dismiss=/g' {} +
find . -name "*.html" -exec sed -i 's/data-ride=/data-bs-ride=/g' {} +
find . -name "*.html" -exec sed -i 's/data-spy=/data-bs-spy=/g' {} +

# Class renames
find . -name "*.html" -exec sed -i 's/badge badge-/badge bg-/g' {} +
find . -name "*.html" -exec sed -i 's/class="close"/class="btn-close"/g' {} +
find . -name "*.html" -exec sed -i 's/ml-/ms-/g' {} +
find . -name "*.html" -exec sed -i 's/mr-/me-/g' {} +
find . -name "*.html" -exec sed -i 's/pl-/ps-/g' {} +
find . -name "*.html" -exec sed -i 's/pr-/pe-/g' {} +
```

JavaScript plugin migration:

```javascript
// Bootstrap 4 (jQuery)
$('#myModal').modal('show');
$('#myTooltip').tooltip();
$('#myDropdown').dropdown();

// Bootstrap 5 (Vanilla JS)
const modal = new bootstrap.Modal(document.getElementById('myModal'));
modal.show();

const tooltip = new bootstrap.Tooltip(document.querySelector('[data-bs-toggle="tooltip"]'));

const dropdown = new bootstrap.Dropdown(document.getElementById('myDropdown'));
```

Sass variable changes:

```scss
// Bootstrap 4
$theme-colors: (
  "primary": #007bff,
  "secondary": #6c757d,
  "danger": #dc3545
);
$grid-breakpoints: (
  xs: 0,
  sm: 576px,
  md: 768px,
  lg: 992px,
  xl: 1200px
);

// Bootstrap 5 - same map structure, but adds xxl breakpoint
$grid-breakpoints: (
  xs: 0,
  sm: 576px,
  md: 768px,
  lg: 992px,
  xl: 1200px,
  xxl: 1400px  // New breakpoint
);
```

## Best Practices

1. **Migrate incrementally** - Update one component or page at a time rather than attempting a full rewrite
2. **Run both versions temporarily** - Use module aliases or scoped imports to test components side by side
3. **Automate data attribute changes** - Use sed or project-wide find-replace for `data-bs-*` prefix updates
4. **Audit custom Sass variables** - Compare your `$theme-colors`, `$grid-breakpoints`, and spacing variables against Bootstrap 5 defaults
5. **Update JavaScript plugins systematically** - Replace every jQuery plugin call with its Bootstrap 5 vanilla JS equivalent
6. **Test RTL support after migration** - Bootstrap 5's logical properties may affect custom layouts
7. **Review third-party plugin compatibility** - Ensure plugins like bootstrap-select, bootstrap-table support Bootstrap 5
8. **Update build tooling** - Verify PostCSS, Sass compiler, and bundler versions are compatible
9. **Remove jQuery dependency** - Delete jQuery and any jQuery plugins that Bootstrap 5 no longer needs
10. **Validate with Bootstrap 5 documentation** - Cross-reference every class name against the official docs

## Common Pitfalls

1. **Missing `data-bs-` prefix** - Forgetting the `bs-` prefix on data attributes silently breaks all JavaScript plugins
2. **Using deprecated `close` class** - The `close` class no longer exists; must use `btn-close`
3. **Forgetting directional property changes** - `ml-`/`mr-` to `ms-`/`me-` and `pl-`/`pr-` to `ps-`/`pe-` is easy to miss
4. **Ignoring new `xxl` breakpoint** - Custom grid configurations may need the 1400px `xxl` breakpoint
5. **Not removing jQuery** - Keeping unused jQuery inflates bundle size and creates confusion about dependencies
6. **Assuming drop-in replacement** - Some components have structural HTML changes, not just class renames
7. **Overlooking JavaScript API changes** - Plugin options, methods, and events have renamed or restructured

## Accessibility Considerations

Bootstrap 5 significantly improves accessibility. The new `visually-hidden` class replaces `sr-only`. Close buttons use `btn-close` with proper ARIA. Modal focus management is improved. Ensure all `aria-*` attributes remain intact during migration and update `aria-label` values for the new button markup where applicable.

## Responsive Behavior

Bootstrap 5 adds the `xxl` breakpoint at 1400px. Verify that custom responsive utilities account for this. The new logical property utilities (`ms-`, `me-`, `ps-`, `pe-`) are RTL-aware, so layouts built with these utilities automatically adapt when `dir="rtl"` is applied to the document root.
