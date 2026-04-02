---
title: "Toast Notifications for Errors"
description: "Using Bootstrap toasts for error alerts with auto-dismiss, action buttons, and stacking"
difficulty: 1
tags: ["error-handling", "toasts", "notifications", "bootstrap"]
prerequisites: ["02_05_Alerts"]
---

## Overview

Bootstrap toasts provide lightweight, non-blocking notifications ideal for error messages that don't warrant a full modal or page-level alert. Toasts appear in the corner of the viewport, auto-dismiss after a timeout, and can include action buttons for retry or undo. They work well for transient errors — a failed save, a timeout on a background task, or a validation warning.

The toast component requires manual positioning (Bootstrap does not include a toast container), stacking logic, and JavaScript initialization. Error toasts use `bg-danger` with auto-dismiss disabled by default to ensure users see the error.

## Basic Implementation

```html
<!-- Toast container positioned in top-right -->
<div class="toast-container position-fixed top-0 end-0 p-3" style="z-index: 1100;">

  <!-- Error toast -->
  <div id="errorToast" class="toast align-items-center text-bg-danger border-0" role="alert"
       aria-live="assertive" aria-atomic="true">
    <div class="d-flex">
      <div class="toast-body">
        <i class="bi bi-exclamation-circle me-2"></i>
        Failed to save changes. Please try again.
      </div>
      <button type="button" class="btn-close btn-close-white me-2 m-auto"
              data-bs-dismiss="toast" aria-label="Close"></button>
    </div>
  </div>

</div>

<!-- Trigger button for demo -->
<button class="btn btn-danger" id="showErrorToast">Trigger Error</button>
```

```js
// Show error toast programmatically
const errorToastEl = document.getElementById('errorToast');
const errorToast = new bootstrap.Toast(errorToastEl, {
  autohide: false // Keep error toasts visible until dismissed
});

document.getElementById('showErrorToast').addEventListener('click', () => {
  errorToast.show();
});

// Dynamic toast creation
function showErrorToast(message, options = {}) {
  const container = document.querySelector('.toast-container');

  const toastEl = document.createElement('div');
  toastEl.className = 'toast align-items-center text-bg-danger border-0';
  toastEl.setAttribute('role', 'alert');
  toastEl.setAttribute('aria-live', 'assertive');
  toastEl.setAttribute('aria-atomic', 'true');

  toastEl.innerHTML = `
    <div class="d-flex">
      <div class="toast-body">
        <i class="bi bi-exclamation-circle me-2"></i>
        ${message}
      </div>
      ${options.retry ? `<button class="btn btn-sm btn-light me-2 my-auto" data-action="retry">Retry</button>` : ''}
      <button type="button" class="btn-close btn-close-white me-2 m-auto"
              data-bs-dismiss="toast" aria-label="Close"></button>
    </div>
  `;

  container.appendChild(toastEl);

  const toast = new bootstrap.Toast(toastEl, {
    autohide: options.autohide ?? false,
    delay: options.delay ?? 8000
  });

  if (options.retry) {
    toastEl.querySelector('[data-action="retry"]')
      .addEventListener('click', () => {
        toast.hide();
        options.retry();
      });
  }

  toastEl.addEventListener('hidden.bs.toast', () => toastEl.remove());
  toast.show();

  return toast;
}
```

## Advanced Variations

```html
<!-- Error toast with action buttons and details -->
<div class="toast-container position-fixed top-0 end-0 p-3" style="z-index: 1100;">
  <div class="toast align-items-center text-bg-danger border-0" role="alert"
       aria-live="assertive" aria-atomic="true">
    <div class="toast-header text-bg-danger text-white">
      <i class="bi bi-x-circle-fill me-2"></i>
      <strong class="me-auto">Upload Failed</strong>
      <small>Just now</small>
      <button type="button" class="btn-close btn-close-white" data-bs-dismiss="toast"></button>
    </div>
    <div class="toast-body">
      <p class="mb-2">File "report.pdf" exceeds the 10MB limit.</p>
      <div class="d-flex gap-2">
        <button type="button" class="btn btn-sm btn-outline-light">Choose Smaller File</button>
        <button type="button" class="btn btn-sm btn-light" data-bs-dismiss="toast">Dismiss</button>
      </div>
    </div>
  </div>
</div>
```

```js
// Toast queue manager for stacking multiple error toasts
class ToastManager {
  constructor(container) {
    this.container = container || document.querySelector('.toast-container');
    this.toasts = [];
    this.maxVisible = 5;
  }

  error(message, options = {}) {
    return this.create(message, 'danger', {
      icon: 'bi-exclamation-circle',
      autohide: false,
      ...options
    });
  }

  warning(message, options = {}) {
    return this.create(message, 'warning', {
      icon: 'bi-exclamation-triangle',
      autohide: true,
      delay: 6000,
      ...options
    });
  }

  success(message, options = {}) {
    return this.create(message, 'success', {
      icon: 'bi-check-circle',
      autohide: true,
      delay: 4000,
      ...options
    });
  }

  create(message, type, options) {
    // Remove oldest if at max capacity
    while (this.toasts.length >= this.maxVisible) {
      const oldest = this.toasts.shift();
      oldest.hide();
    }

    const toastEl = document.createElement('div');
    toastEl.className = `toast align-items-center text-bg-${type} border-0`;
    toastEl.setAttribute('role', 'alert');
    toastEl.setAttribute('aria-live', type === 'danger' ? 'assertive' : 'polite');
    toastEl.setAttribute('aria-atomic', 'true');

    let actionsHtml = '';
    if (options.retry) {
      actionsHtml += `<button class="btn btn-sm btn-light me-2" data-action="retry">Retry</button>`;
    }

    toastEl.innerHTML = `
      <div class="d-flex">
        <div class="toast-body">
          <i class="bi ${options.icon} me-2"></i>${message}
        </div>
        ${actionsHtml}
        <button type="button" class="btn-close ${type !== 'warning' ? 'btn-close-white' : ''} me-2 m-auto"
                data-bs-dismiss="toast" aria-label="Close"></button>
      </div>
    `;

    this.container.appendChild(toastEl);

    const toast = new bootstrap.Toast(toastEl, {
      autohide: options.autohide,
      delay: options.delay || 8000
    });

    if (options.retry) {
      toastEl.querySelector('[data-action="retry"]')
        .addEventListener('click', () => {
          toast.hide();
          options.retry();
        });
    }

    toastEl.addEventListener('hidden.bs.toast', () => {
      toastEl.remove();
      this.toasts = this.toasts.filter(t => t !== toast);
    });

    this.toasts.push(toast);
    toast.show();
    return toast;
  }

  clearAll() {
    this.toasts.forEach(t => t.hide());
    this.toasts = [];
  }
}

const toasts = new ToastManager();

// Usage
document.getElementById('saveBtn').addEventListener('click', async () => {
  try {
    await saveData();
    toasts.success('Changes saved successfully.');
  } catch (err) {
    toasts.error('Failed to save changes.', {
      retry: () => document.getElementById('saveBtn').click()
    });
  }
});
```

## Best Practices

1. Use `autohide: false` for error toasts — users must see and acknowledge errors
2. Position toast containers with `position-fixed` in the top-right corner
3. Limit visible toasts to 3-5 to avoid overwhelming the user
4. Include action buttons (Retry, Undo) directly in the toast for immediate resolution
5. Use `aria-live="assertive"` for error toasts so screen readers announce them immediately
6. Remove toast DOM elements after they are hidden to prevent memory leaks
7. Stack toasts vertically with consistent spacing in the container
8. Use `text-bg-danger` for errors, `text-bg-warning` for warnings, `text-bg-success` for confirmations
9. Group related toasts — don't show 10 individual error toasts for a bulk operation
10. Provide a "Dismiss All" option when multiple toasts are visible

## Common Pitfalls

1. **Auto-hiding error toasts** — Errors that disappear after 5 seconds are easily missed; keep them visible until dismissed
2. **No container element** — Toasts without a `toast-container` overlap and stack unpredictably
3. **Too many toasts** — Showing one toast per field validation error floods the screen; batch into a summary
4. **Not removing hidden toasts** — Accumulated toast DOM nodes cause memory leaks in long sessions
5. **Missing `aria-live`** — Screen readers do not announce toasts without live region attributes
6. **Toasts behind modals** — Toast `z-index` (default 1080) can appear behind Bootstrap modals (1055) if not managed

## Accessibility Considerations

Error toasts require `aria-live="assertive"` and `role="alert"` so assistive technology announces them immediately. Toast action buttons must be keyboard-accessible. Do not rely on color alone to convey error severity — use icons and text. Provide sufficient color contrast (4.5:1 minimum) on custom toast backgrounds.

## Responsive Behavior

Toast containers should use `position-fixed` with `top-0 end-0` for consistent placement. On mobile, toasts can span most of the screen width — use `mw-100` and limit toast body text to 2-3 lines. Stack toasts vertically and ensure the close button is touch-friendly (minimum 44x44px).
