---
title: "Modal JavaScript API"
module: "JavaScript Components"
lesson: "04_02_02"
difficulty: 2
estimated_time: 25 minutes
bootstrap_version: 5.3
prerequisites:
  - Bootstrap 5 modal HTML structure
  - JavaScript fundamentals
learning_objectives:
  - Initialize modals programmatically with the JavaScript API
  - Configure modal behavior with options (backdrop, keyboard, focus)
  - Handle modal lifecycle events
  - Use handleUpdate for dynamic content repositioning
  - Properly dispose of modal instances
---

# Modal JavaScript API

## Overview

The Bootstrap Modal JavaScript API provides full programmatic control over modal dialogs. Beyond `data-bs-*` attributes, the `bootstrap.Modal` class lets you create, show, hide, and configure modals dynamically. This is essential for SPAs, dynamic content loading, and complex form workflows.

The API supports configuration for backdrop behavior, keyboard dismissal, focus trapping, scroll behavior, and content updates — giving you precise control over the modal experience.

```js
const modalEl = document.getElementById('myModal');
const modal = new bootstrap.Modal(modalEl, {
  backdrop: true,
  keyboard: true,
  focus: true
});
```

## Basic Implementation

### HTML Structure and Initialization

```html
<div class="modal fade" id="confirmModal" tabindex="-1" aria-labelledby="confirmModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="confirmModalLabel">Confirm Action</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <p>Are you sure you want to proceed?</p>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button type="button" class="btn btn-primary" id="confirmBtn">Confirm</button>
      </div>
    </div>
  </div>
</div>
```

```js
const modalEl = document.getElementById('confirmModal');
const modal = new bootstrap.Modal(modalEl);

// Show the modal
document.getElementById('openModal').addEventListener('click', () => {
  modal.show();
});

// Hide the modal
document.getElementById('confirmBtn').addEventListener('click', () => {
  // Process confirmation...
  modal.hide();
});

// Toggle visibility
modal.toggle();
```

### Retrieving Instances

```js
// Get existing instance
const existing = bootstrap.Modal.getInstance(modalEl);

// Get or create
const instance = bootstrap.Modal.getOrCreateInstance(modalEl);
```

## Advanced Variations

### Configuration Options

```js
const modal = new bootstrap.Modal(modalEl, {
  backdrop: 'static',  // true, false, or 'static' (prevents closing on click)
  keyboard: false,      // Prevent Escape key from closing
  focus: true           // Trap focus inside the modal
});
```

- `backdrop: 'static'` — shows the backdrop but prevents clicking it to close.
- `keyboard: false` — disables the Escape key dismissal.
- `focus: true` — automatically moves focus into the modal when opened.

### Lifecycle Events

```js
const modalEl = document.getElementById('confirmModal');

modalEl.addEventListener('show.bs.modal', () => {
  console.log('Modal is about to show');
});

modalEl.addEventListener('shown.bs.modal', () => {
  console.log('Modal is now visible');
  // Good place to initialize form validation, charts, etc.
});

modalEl.addEventListener('hide.bs.modal', () => {
  console.log('Modal is about to hide');
});

modalEl.addEventListener('hidden.bs.modal', () => {
  console.log('Modal is now hidden');
  // Clean up dynamic content, reset forms
});

modalEl.addEventListener('hidePrevented.bs.modal', () => {
  console.log('Close was prevented (static backdrop or keyboard disabled)');
});
```

### handleUpdate for Dynamic Content

When content inside a modal changes height (e.g., loading async data), call `handleUpdate` to recalculate the scroll and positioning:

```js
const modal = bootstrap.Modal.getOrCreateInstance(modalEl);

fetch('/api/details')
  .then(res => res.json())
  .then(data => {
    modalBody.innerHTML = `<p>${data.description}</p>`;
    modal.handleUpdate(); // Repositions the modal
  });
```

## Best Practices

1. **Always set `tabindex="-1"`** on the modal element for proper focus management.
2. **Use `aria-labelledby`** pointing to the modal title for screen readers.
3. **Use `backdrop: 'static'`** for modals that require explicit user action (e.g., confirmations, unsaved changes warnings).
4. **Dispose modal instances** when removing modals from the DOM to prevent memory leaks.
5. **Call `handleUpdate()`** after dynamically changing modal body content to fix scroll and centering.
6. **Use `shown.bs.modal` event** to initialize interactive components (charts, rich text editors) inside the modal, not `show.bs.modal`.
7. **Reset forms in `hidden.bs.modal`** to clean state when the modal reopens.
8. **Avoid multiple simultaneous modals** — Bootstrap does not officially support stacked modals and focus trapping will conflict.
9. **Set explicit `aria-hidden="true"`** on the modal element when hidden; Bootstrap manages this but verify in custom implementations.
10. **Use `static` backdrop for critical decisions** — prevents accidental dismissal.
11. **Ensure the modal has a visible close button** (`btn-close`) for mouse users and a `data-bs-dismiss="modal"` attribute.

## Common Pitfalls

1. **Forgetting `tabindex="-1"`** — without it, focus management breaks and the modal may not be announced by screen readers.
2. **Not calling `handleUpdate()`** after async content loads — modal body may overflow or be mispositioned.
3. **Using `backdrop: true` for critical forms** — users can accidentally close the modal and lose data. Use `'static'` instead.
4. **Multiple instances on the same element** — calling `new bootstrap.Modal(el)` twice overwrites the first. Always use `getOrCreateInstance`.
5. **Loading heavy scripts on every show** — initialize once and reuse, or clean up in `hidden.bs.modal` to avoid stacking listeners.
6. **Ignoring `hidePrevented.bs.modal`** — when using `backdrop: 'static'` or `keyboard: false`, this event fires when users try to close. Provide visual feedback.
7. **Not handling modal focus on hide** — if the trigger button is removed from the DOM before the modal closes, focus falls to `<body>`. Ensure the trigger still exists or manually set focus.

## Accessibility Considerations

- The modal must have `role="dialog"` and `aria-modal="true"`.
- The modal title must be referenced by `aria-labelledby`.
- Focus is trapped inside the modal while open — Bootstrap handles this automatically.
- The close button must have `aria-label="Close"`.
- Background content should have `aria-hidden="true"` when the modal is open (Bootstrap manages this on the `body`).
- All interactive elements inside the modal must be reachable via keyboard (Tab/Shift+Tab).

```html
<div class="modal fade" id="a11yModal" tabindex="-1"
     aria-labelledby="a11yModalLabel" aria-modal="true" role="dialog">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="a11yModalLabel">Accessible Modal</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <label for="emailInput" class="form-label">Email</label>
        <input type="email" class="form-control" id="emailInput">
      </div>
    </div>
  </div>
</div>
```

## Responsive Behavior

Modals are responsive out of the box using `.modal-dialog-centered` and `.modal-fullscreen` variants:

```html
<!-- Centered on all screens -->
<div class="modal-dialog modal-dialog-centered">

<!-- Fullscreen below md breakpoint -->
<div class="modal-dialog modal-fullscreen-sm-down">
```

For dynamic responsiveness via JavaScript:

```js
function adjustModal() {
  const dialog = modalEl.querySelector('.modal-dialog');
  if (window.innerWidth < 576) {
    dialog.classList.add('modal-fullscreen-sm-down');
  } else {
    dialog.classList.remove('modal-fullscreen-sm-down');
  }
}

window.addEventListener('resize', adjustModal);
adjustModal();
```

- Use `.modal-dialog-scrollable` for long content on small screens to keep the header and footer fixed.
- Avoid fixed widths on `.modal-dialog`; use Bootstrap sizing utilities (`modal-sm`, `modal-lg`, `modal-xl`) instead.
- On mobile, ensure form inputs inside modals are not obscured by the virtual keyboard by calling `handleUpdate()` on input focus if needed.
