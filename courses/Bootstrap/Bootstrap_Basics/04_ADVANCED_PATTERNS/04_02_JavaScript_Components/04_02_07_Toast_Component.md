---
title: "Toast Component"
module: "JavaScript Components"
lesson: "04_02_07"
difficulty: 2
estimated_time: 20 minutes
bootstrap_version: 5.3
prerequisites:
  - Bootstrap 5 basics
  - JavaScript DOM manipulation
learning_objectives:
  - Create and display toast notifications
  - Configure auto-hide behavior with delay settings
  - Show/hide toasts programmatically
  - Build toast containers with stacking behavior
  - Handle toast events for custom workflows
---

# Toast Component

## Overview

Bootstrap toasts are lightweight, auto-dismissing notification messages. Unlike alerts, toasts are designed to be non-blocking — they appear temporarily and disappear without requiring user interaction. The `bootstrap.Toast` class manages visibility, auto-hide timing, and lifecycle events.

Toasts are ideal for success confirmations, error notifications, informational messages, and real-time update alerts. They support stacking, custom positioning, and configurable auto-hide timing.

```js
const toastEl = document.getElementById('myToast');
const toast = new bootstrap.Toast(toastEl);
toast.show();
```

## Basic Implementation

### Toast HTML Structure

```html
<div class="toast" role="alert" aria-live="assertive" aria-atomic="true">
  <div class="toast-header">
    <strong class="me-auto">Notification</strong>
    <small>Just now</small>
    <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
  </div>
  <div class="toast-body">
    Your changes have been saved successfully.
  </div>
</div>
```

### Showing and Hiding

```html
<button class="btn btn-primary" id="showToast">Show Toast</button>

<div class="toast-container position-fixed bottom-0 end-0 p-3">
  <div id="liveToast" class="toast" role="alert" aria-live="assertive" aria-atomic="true">
    <div class="toast-header">
      <strong class="me-auto">Bootstrap</strong>
      <small>1 min ago</small>
      <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
    </div>
    <div class="toast-body">
      Hello, world! This is a toast message.
    </div>
  </div>
</div>
```

```js
const toastEl = document.getElementById('liveToast');
const toast = new bootstrap.Toast(toastEl);

document.getElementById('showToast').addEventListener('click', () => {
  toast.show();
});

// Programmatic hide
// toast.hide();
```

## Advanced Variations

### Auto-Hide Configuration

```js
// Auto-hide after 5 seconds (default)
const toast = new bootstrap.Toast(element, {
  autohide: true,
  delay: 5000
});

// Disable auto-hide (persistent toast)
const persistentToast = new bootstrap.Toast(element, {
  autohide: false
});

// Custom delay
const quickToast = new bootstrap.Toast(element, {
  autohide: true,
  delay: 2000  // 2 seconds
});
```

### Toast Lifecycle Events

```js
const toastEl = document.getElementById('liveToast');

toastEl.addEventListener('show.bs.toast', () => {
  console.log('Toast is about to show');
});

toastEl.addEventListener('shown.bs.toast', () => {
  console.log('Toast is now visible');
});

toastEl.addEventListener('hide.bs.toast', () => {
  console.log('Toast is about to hide');
});

toastEl.addEventListener('hidden.bs.toast', () => {
  console.log('Toast is now hidden');
  // Remove from DOM after hiding
  toastEl.remove();
});
```

### Dynamic Toast Creation

```js
function showToast(message, type = 'info', duration = 5000) {
  const container = document.querySelector('.toast-container') || createContainer();

  const toastEl = document.createElement('div');
  toastEl.className = 'toast';
  toastEl.setAttribute('role', 'alert');
  toastEl.setAttribute('aria-live', 'assertive');
  toastEl.setAttribute('aria-atomic', 'true');

  const bgClass = {
    success: 'text-bg-success',
    error: 'text-bg-danger',
    warning: 'text-bg-warning',
    info: 'text-bg-info'
  }[type] || '';

  toastEl.innerHTML = `
    <div class="toast-header ${bgClass}">
      <strong class="me-auto text-capitalize">${type}</strong>
      <small>Just now</small>
      <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
    </div>
    <div class="toast-body">
      ${message}
    </div>
  `;

  container.appendChild(toastEl);

  const toast = new bootstrap.Toast(toastEl, {
    autohide: true,
    delay: duration
  });

  toast.show();

  toastEl.addEventListener('hidden.bs.toast', () => {
    toastEl.remove();
  });
}

function createContainer() {
  const container = document.createElement('div');
  container.className = 'toast-container position-fixed bottom-0 end-0 p-3';
  document.body.appendChild(container);
  return container;
}

// Usage
showToast('File uploaded successfully!', 'success');
showToast('Network error occurred.', 'error', 8000);
showToast('New message received.', 'info');
```

### Stacking Toasts

```html
<div class="toast-container position-fixed bottom-0 end-0 p-3">
  <div class="toast" role="alert" aria-live="assertive" aria-atomic="true">
    <div class="toast-header">
      <strong class="me-auto">Email</strong>
      <small>2 min ago</small>
      <button type="button" class="btn-close" data-bs-dismiss="toast"></button>
    </div>
    <div class="toast-body">New email from John.</div>
  </div>
  <div class="toast" role="alert" aria-live="assertive" aria-atomic="true">
    <div class="toast-header">
      <strong class="me-auto">Calendar</strong>
      <small>5 min ago</small>
      <button type="button" class="btn-close" data-bs-dismiss="toast"></button>
    </div>
    <div class="toast-body">Meeting in 15 minutes.</div>
  </div>
  <div class="toast" role="alert" aria-live="assertive" aria-atomic="true">
    <div class="toast-header">
      <strong class="me-auto">System</strong>
      <small>10 min ago</small>
      <button type="button" class="btn-close" data-bs-dismiss="toast"></button>
    </div>
    <div class="toast-body">Update installed successfully.</div>
  </div>
</div>
```

```js
// Show all toasts in a container
document.querySelectorAll('.toast').forEach(el => {
  new bootstrap.Toast(el).show();
});
```

### Instance Retrieval

```js
const existing = bootstrap.Toast.getInstance(toastEl);
const instance = bootstrap.Toast.getOrCreateInstance(toastEl);
```

## Best Practices

1. **Always use `role="alert"`** and `aria-live="assertive"` on toasts for screen reader announcements.
2. **Wrap toasts in a `.toast-container`** for proper positioning and stacking behavior.
3. **Use `position-fixed`** on the container to keep toasts visible regardless of page scroll.
4. **Remove toasts from DOM** in the `hidden.bs.event` handler to prevent memory buildup with dynamic toasts.
5. **Set appropriate delays** — success messages can be shorter (3-5s), errors should be longer (8-10s) or persistent.
6. **Use `autohide: false`** for critical errors that require user acknowledgment.
7. **Use `text-bg-*` classes** for color-coded toast headers that communicate message type at a glance.
8. **Limit concurrent toasts** — implement a queue or max-visible count to prevent overwhelming users.
9. **Include a close button** (`btn-close`) on all toasts, even auto-hiding ones, for user control.
10. **Use `aria-atomic="true"`** so screen readers announce the entire toast content, not just changed parts.
11. **Keep toast content concise** — toasts are temporary and should communicate one message.

## Common Pitfalls

1. **Not positioning the toast container** — toasts render inline without a positioned container, disrupting page layout.
2. **Forgetting `role="alert"`** — screen readers will not announce the toast without this attribute.
3. **Too-short delay** — users may not have time to read messages. Minimum recommended delay is 3 seconds.
4. **Not removing dynamic toasts** — creating toasts without cleanup causes DOM bloat over time.
5. **Using toasts for critical actions** — toasts auto-dismiss; use modals or alerts for messages that require acknowledgment.
6. **Stacking too many toasts** — more than 3-4 visible toasts at once overwhelms the interface.
7. **Missing `aria-live` attribute** — without it, assistive technology may not announce the toast content.

## Accessibility Considerations

- Toasts must have `role="alert"` and `aria-live="assertive"` (or `"polite"` for non-urgent messages).
- `aria-atomic="true"` ensures the entire toast content is announced.
- The close button must have `aria-label="Close"`.
- Toasts should not steal focus — they are non-modal notifications.
- Use `aria-live="polite"` for informational toasts that should not interrupt the current task.
- Ensure sufficient color contrast in toast headers and body text.

```html
<div class="toast" role="alert" aria-live="assertive" aria-atomic="true">
  <div class="toast-header text-bg-danger">
    <strong class="me-auto">Error</strong>
    <small>Just now</small>
    <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
  </div>
  <div class="toast-body">
    Failed to save changes. Please try again.
  </div>
</div>
```

```js
// Use polite for non-critical notifications
const infoToastEl = document.getElementById('infoToast');
infoToastEl.setAttribute('aria-live', 'polite');
const infoToast = new bootstrap.Toast(infoToastEl);
```

## Responsive Behavior

Toast containers adapt to screen size with Bootstrap positioning utilities:

```html
<!-- Bottom-right on desktop, full-width bottom on mobile -->
<div class="toast-container position-fixed bottom-0 end-0 p-3">
  <div class="toast">...</div>
</div>

<!-- Centered on mobile -->
<div class="toast-container position-fixed bottom-0 start-50 translate-middle-x p-3">
  <div class="toast">...</div>
</div>
```

- On small screens, use `start-50 translate-middle-x` to center toasts horizontally.
- Use `w-100` on the toast for full-width notifications on mobile:

```js
function adjustToastForMobile(toastEl) {
  if (window.innerWidth < 576) {
    toastEl.style.maxWidth = '100%';
    toastEl.classList.add('w-100');
  } else {
    toastEl.style.maxWidth = '350px';
    toastEl.classList.remove('w-100');
  }
}
```

- Stack toasts vertically in the container by default. Bootstrap handles vertical stacking automatically within `.toast-container`.
- Use `top-0` instead of `bottom-0` for toast notifications that should appear at the top of the screen (e.g., system-level alerts).
