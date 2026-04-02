---
title: "Bootstrap Load Order"
lesson: "01_01_10"
difficulty: "2"
topics: ["css-load-order", "js-dependencies", "popper.js", "initialization"]
estimated_time: "25 minutes"
---

# Bootstrap Load Order

## Overview

Bootstrap's CSS and JavaScript must be loaded in a specific order to function correctly. CSS must load before page content renders, JavaScript requires Popper.js as a dependency for interactive components, and the initialization sequence determines when components become active. Understanding this load order prevents broken layouts, non-functional modals, and silent JS errors.

The load order matters at three levels: CSS dependencies (imports), JavaScript dependencies (Popper.js), and DOM readiness (initialization timing). Each layer has specific requirements that, when violated, produce subtle bugs that are difficult to diagnose.

## Basic Implementation

### Standard HTML Load Order

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <!-- 1. CSS must load first -->
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
  <!-- 2. Your HTML content -->
  <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
    Launch modal
  </button>

  <div class="modal fade" id="exampleModal" tabindex="-1">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Modal title</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
        </div>
        <div class="modal-body">Modal content here.</div>
      </div>
    </div>
  </div>

  <!-- 3. JS loads at end of body -->
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

### JavaScript-Only Components

```html
<!-- Bootstrap JS bundle (includes Popper.js) -->
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

<script>
  // Components are now available
  const modal = new bootstrap.Modal(document.getElementById('exampleModal'));
  modal.show();
</script>
```

### Standalone JS with Separate Popper.js

```html
<!-- 1. Popper.js must load BEFORE bootstrap.js -->
<script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.8/dist/umd/popper.min.js"></script>
<!-- 2. Bootstrap JS after Popper -->
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.min.js"></script>
```

## Advanced Variations

### Deferred Loading with `defer`

```html
<head>
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
  <!-- Content here -->
  <script defer src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
  <script defer src="/js/app.js"></script>
</body>
```

### ES Module Import Order

```javascript
// app.js - when using modules
import 'bootstrap/dist/css/bootstrap.min.css'; // CSS via bundler
import * as bootstrap from 'bootstrap';         // All JS components

// DOM must be ready before initializing components
document.addEventListener('DOMContentLoaded', () => {
  const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
  tooltipTriggerList.forEach(el => new bootstrap.Tooltip(el));
});
```

### Dynamic Import for Lazy Loading

```javascript
// Load Bootstrap JS only when needed
async function openModal() {
  const { Modal } = await import('bootstrap');
  const modal = new Modal(document.getElementById('myModal'));
  modal.show();
}
```

### SCSS Import Order Matters

```scss
// Correct order - functions and variables first
@import "bootstrap/scss/functions";
@import "bootstrap/scss/variables";
@import "bootstrap/scss/mixins";
@import "bootstrap/scss/root";
@import "bootstrap/scss/reboot";
// Components after root/reboot
@import "bootstrap/scss/buttons";
@import "bootstrap/scss/modal";
```

## Best Practices

1. **Always load CSS in the `<head>`** - Prevents unstyled content flash (FOUC).
2. **Place Bootstrap JS before closing `</body>`** - DOM must exist before JS queries it.
3. **Use `bootstrap.bundle.min.js` to avoid Popper.js version mismatches** - Bundled and tested together.
4. **Load custom CSS after Bootstrap CSS** - Ensures your overrides take precedence.
5. **Load custom JS after Bootstrap JS** - Your code can reference `bootstrap.*` classes.
6. **Use `defer` attribute for non-critical scripts** - Parallel download, sequential execution.
7. **Initialize tooltips/popovers after DOMContentLoaded** - Elements must exist in DOM.
8. **Import SCSS partials in dependency order** - Functions before variables before mixins before components.
9. **Test load order in slow network conditions** - Race conditions surface under throttling.
10. **Avoid `async` for Bootstrap JS** - Execution order becomes unpredictable.
11. **Use module bundler's CSS extraction** - Ensures CSS loads in `<head>` even from JS imports.
12. **Keep Bootstrap and Popper.js versions synchronized** - Check Bootstrap's peer dependency requirements.

## Common Pitfalls

1. **Loading Bootstrap JS in `<head>` without `defer`** - Blocks rendering and DOM is unavailable, causing components to fail silently.
2. **Using `bootstrap.js` without Popper.js** - Dropdowns, tooltips, and popovers throw `ReferenceError: Popper is not defined`.
3. **Placing custom CSS before Bootstrap CSS** - Bootstrap's specificity overrides your customizations instead of the reverse.
4. **Initializing components before DOMContentLoaded** - `document.querySelector()` returns null, components cannot bind to elements.
5. **Importing Bootstrap SCSS partials before `_functions.scss`** - Compilation errors because variables and mixins reference undefined functions.
6. **Mixing CDN and local Bootstrap versions** - CSS and JS version mismatches cause `data-bs-*` attribute conflicts.

## Accessibility Considerations

Bootstrap's JavaScript handles ARIA attribute updates, keyboard navigation, and focus management dynamically during initialization. If JavaScript fails to load or initializes before the DOM, these accessibility features will not activate. Components like modals set `aria-hidden`, `role="dialog"`, and trap focus automatically - but only when JS loads correctly. Always ensure your fallback content is accessible without JavaScript.

## Responsive Behavior

Bootstrap's CSS load order does not affect responsive behavior - all responsive utilities and breakpoint-dependent styles are defined in the compiled CSS regardless of external load order. However, JavaScript-dependent responsive components (like navbar collapse and offcanvas) require Bootstrap JS to be loaded for their toggle behavior to work at all screen sizes.
