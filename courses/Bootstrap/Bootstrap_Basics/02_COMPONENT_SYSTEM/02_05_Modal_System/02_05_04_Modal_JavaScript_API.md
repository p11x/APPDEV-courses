---
title: Modal JavaScript API
category: Component System
difficulty: 3
time: 25 min
tags: bootstrap5, modal, javascript, api, events, programmatic
---

## Overview

Bootstrap's Modal JavaScript API allows full programmatic control over modal dialogs. You can create, show, hide, toggle, and dispose modals using the `bootstrap.Modal` constructor. The API exposes lifecycle events (`show.bs.modal`, `shown.bs.modal`, `hide.bs.modal`, `hidden.bs.modal`) for hooking into transitions, and provides methods to retrieve existing instances or dispose of modals entirely. This API is essential for dynamic content, SPA integration, and complex interaction patterns.

## Basic Implementation

### Creating and Showing a Modal

```html
<button type="button" class="btn btn-primary" id="openBtn">Open Modal</button>

<div class="modal fade" id="myModal" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="myModalLabel">Dynamic Modal</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>
      <div class="modal-body">
        <p>This modal is controlled via JavaScript.</p>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
      </div>
    </div>
  </div>
</div>

<script>
  const modalEl = document.getElementById('myModal');
  const modal = new bootstrap.Modal(modalEl, {
    backdrop: true,    // Show backdrop (default)
    keyboard: true,    // Close on Escape (default)
    focus: true        // Focus on modal when shown (default)
  });

  document.getElementById('openBtn').addEventListener('click', function () {
    modal.show();
  });
</script>
```

## Advanced Variations

### Modal Events

```javascript
const modalEl = document.getElementById('myModal');

modalEl.addEventListener('show.bs.modal', function () {
  console.log('Modal is about to show');
});

modalEl.addEventListener('shown.bs.modal', function () {
  console.log('Modal is now visible');
  // Safe to interact with modal DOM here
});

modalEl.addEventListener('hide.bs.modal', function () {
  console.log('Modal is about to hide');
});

modalEl.addEventListener('hidden.bs.modal', function () {
  console.log('Modal is now hidden');
  // Clean up, reset form, remove dynamic content
});
```

### Getting an Existing Modal Instance

```javascript
// Retrieve existing instance or create new one
const modalEl = document.getElementById('myModal');
let modal = bootstrap.Modal.getInstance(modalEl);

if (!modal) {
  modal = new bootstrap.Modal(modalEl);
}

modal.show();
```

### Disposing a Modal

```javascript
const modalEl = document.getElementById('myModal');
const modal = bootstrap.Modal.getInstance(modalEl);

// Remove all event listeners and data
modal.dispose();

// Optionally remove from DOM
modalEl.remove();
```

### Dynamic Modal Creation

```javascript
function createDynamicModal(title, bodyHtml) {
  const modalEl = document.createElement('div');
  modalEl.className = 'modal fade';
  modalEl.tabIndex = -1;
  modalEl.setAttribute('aria-hidden', 'true');
  modalEl.innerHTML = `
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">${title}</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
        </div>
        <div class="modal-body">${bodyHtml}</div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
        </div>
      </div>
    </div>
  `;

  document.body.appendChild(modalEl);
  const modal = new bootstrap.Modal(modalEl);

  modalEl.addEventListener('hidden.bs.modal', function () {
    modal.dispose();
    modalEl.remove();
  });

  return modal;
}

// Usage
const myModal = createDynamicModal('Alert', '<p>Something happened.</p>');
myModal.show();
```

## Best Practices

1. Always use `bootstrap.Modal.getInstance()` before creating a new instance to avoid duplicates.
2. Dispose of dynamically created modals to prevent memory leaks.
3. Use `hidden.bs.modal` for cleanup operations (resetting forms, removing dynamic DOM).
4. Use `show.bs.modal` for setup operations (loading data via fetch).
5. Set `keyboard: false` only when closing would cause data loss, and provide a clear close button.
6. Set `backdrop: 'static'` when the user must complete an action before dismissal.
7. Use `focus: true` (default) to maintain proper keyboard navigation.
8. Chain modal interactions by listening for `hidden.bs.modal` before showing the next modal.
9. Avoid manipulating the modal DOM during `show.bs.modal`; wait for `shown.bs.modal`.
10. Use `getOrCreateInstance()` to safely get or bootstrap a modal element.

## Common Pitfalls

- **Creating multiple instances on the same element:** Use `getInstance()` to check before constructing a new `bootstrap.Modal()`.
- **Not disposing dynamic modals:** Dynamically injected modals that are removed from the DOM without `.dispose()` leak event listeners.
- **Interacting with modal DOM during `show.bs.modal`:** The animation has not completed; use `shown.bs.modal` instead.
- **Forgetting to remove backdrop:** Calling `modal.hide()` and then removing the DOM element without waiting for `hidden.bs.modal` can leave an orphaned backdrop.
- **Using `modal.dispose()` while the modal is visible:** Always hide the modal before disposing.
- **Not handling the case where `getInstance()` returns `null`:** This happens when the modal was never initialized.

## Accessibility Considerations

The JavaScript API manages focus trapping, `aria-hidden` toggling, and `inert` attribute application automatically. When calling `modal.show()`, focus moves into the dialog. On `modal.hide()`, focus returns to the element that had focus before the modal opened. Ensure your event handlers do not interfere with this focus management by focusing elements at the wrong lifecycle stage.

## Responsive Behavior

The JavaScript API itself has no responsive-specific behavior, but modal options interact with CSS sizing. The `backdrop` option always covers the full viewport. When using `keyboard: true`, the Escape key closes modals regardless of screen size. For responsive offcanvas-like behavior triggered programmatically, use `bootstrap.Offcanvas` instead of `bootstrap.Modal`.
