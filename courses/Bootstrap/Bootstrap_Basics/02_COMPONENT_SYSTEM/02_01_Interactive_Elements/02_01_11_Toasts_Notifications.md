---
title: Toasts and Notifications
category: Component System
difficulty: 2
time: 30 min
tags: bootstrap5, toasts, notifications, alerts, autohide
---

## Overview

Bootstrap toasts are lightweight notifications modeled after the push notifications in mobile OSes. They display brief, auto-dismissable messages using the `toast` component, which includes a header, body, and optional close button. Toasts support auto-hide with configurable delay, stacking via a `toast-container`, and can be positioned in any corner using positioning utilities.

## Basic Implementation

A toast requires a `.toast` wrapper with a `.toast-header` and `.toast-body`.

```html
<!-- Basic toast -->
<div class="toast show" role="alert" aria-live="assertive" aria-atomic="true">
  <div class="toast-header">
    <strong class="me-auto">Notification</strong>
    <small>Just now</small>
    <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
  </div>
  <div class="toast-body">
    Your file has been uploaded successfully.
  </div>
</div>

<!-- Simple body-only toast -->
<div class="toast show" role="alert" aria-live="assertive" aria-atomic="true">
  <div class="toast-body">
    A new update is available.
    <div class="mt-2 pt-2 border-top">
      <button type="button" class="btn btn-primary btn-sm" data-bs-dismiss="toast">Update now</button>
      <button type="button" class="btn btn-secondary btn-sm" data-bs-dismiss="toast">Later</button>
    </div>
  </div>
</div>
```

## Advanced Variations

```html
<!-- Toast with autohide and custom delay -->
<div class="toast show" role="alert" aria-live="assertive" aria-atomic="true"
     data-bs-autohide="true" data-bs-delay="5000">
  <div class="toast-header">
    <i class="bi bi-check-circle text-success me-2"></i>
    <strong class="me-auto">Success</strong>
    <small>2 min ago</small>
    <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
  </div>
  <div class="toast-body">
    Your changes have been saved.
  </div>
</div>
```

```html
<!-- Toast container with stacked toasts -->
<div class="toast-container position-fixed bottom-0 end-0 p-3">
  <div class="toast show" role="alert" aria-live="assertive" aria-atomic="true">
    <div class="toast-header">
      <strong class="me-auto">Error</strong>
      <small>Just now</small>
      <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
    </div>
    <div class="toast-body">
      Failed to connect to server.
    </div>
  </div>
  <div class="toast show" role="alert" aria-live="assertive" aria-atomic="true">
    <div class="toast-header">
      <strong class="me-auto">Warning</strong>
      <small>1 min ago</small>
      <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
    </div>
    <div class="toast-body">
      Storage is 90% full.
    </div>
  </div>
</div>
```

```html
<!-- Trigger toast via JavaScript button -->
<button type="button" class="btn btn-primary" id="liveToastBtn">Show notification</button>

<div class="toast-container position-fixed top-0 end-0 p-3">
  <div id="liveToast" class="toast" role="alert" aria-live="assertive" aria-atomic="true">
    <div class="toast-header">
      <strong class="me-auto">Bootstrap Toast</strong>
      <small>11 mins ago</small>
      <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
    </div>
    <div class="toast-body">
      Hello, world! This is a toast message.
    </div>
  </div>
</div>
```

## Best Practices

1. Always include `role="alert"`, `aria-live="assertive"`, and `aria-atomic="true"` on each toast.
2. Use `toast-container` with positioning classes to manage multiple toasts.
3. Set `data-bs-autohide="true"` with an appropriate `data-bs-delay` for transient notifications.
4. Provide a close button in the header for dismissable toasts.
5. Use semantic colors in headers (success icons, warning icons) to convey notification type.
6. Limit visible toasts to 2-3 at a time to avoid overwhelming the user.
7. Position toasts consistently across your application (top-end or bottom-end).
8. Use action buttons in the toast body for notifications requiring user response.
9. Initialize toasts with JavaScript for dynamic creation and disposal.
10. Destroy toast instances after dismissal to free memory in long-running SPAs.

## Common Pitfalls

1. **Forgetting ARIA attributes.** Without `aria-live="assertive"`, screen readers may not announce toasts.
2. **Stacking without a container.** Placing multiple toasts without `toast-container` causes overlap and layout issues.
3. **No auto-hide on informational toasts.** Persistent toasts clutter the interface and block interaction.
4. **Too-short delay values.** Auto-hide under 3 seconds gives users insufficient time to read the message.
5. **Using toasts for critical errors.** Toasts are dismissable; use modals or persistent alerts for critical system errors.
6. **Not destroying toast instances.** Accumulating toast DOM nodes causes memory leaks in SPAs.

## Accessibility Considerations

Toasts must include `role="alert"` and `aria-live="assertive"` so screen readers announce them immediately. The `aria-atomic="true"` attribute ensures the entire toast content is read, not just changed portions. Each toast should have an accessible close button with `aria-label="Close"`. For auto-hiding toasts, ensure the delay is long enough for all users to read the content. Avoid using toasts for critical information that users must acknowledge.

## Responsive Behavior

Toast containers using `position-fixed` remain anchored to the viewport corner regardless of screen size. On mobile devices, use `start-50 translate-middle-x` for horizontally centered toasts. Reduce toast width with `w-auto` or max-width constraints for narrow screens. Avoid full-width toasts that dominate the mobile viewport; keep them compact and positioned at top-end or bottom-end.
