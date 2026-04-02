---
title: "Push Notification UI with Bootstrap"
topic: "Mobile First PWA"
difficulty: 2
duration: "30 minutes"
prerequisites: ["Push API", "Notifications API", "Service Workers", "Bootstrap components"]
tags: ["notifications", "push", "pwa", "toast", "bootstrap"]
---

## Overview

Push notifications in a PWA require two layers: the Push API subscription (managed by the service worker) and the UI for permission requests, notification display, and in-app notification management. Bootstrap 5 provides the visual components — modals for permission prompts, toasts for in-app notifications, cards for notification history, and badges for unread counts — while the Push API handles the server-to-device communication.

The user-facing experience involves: a permission request UI (not the browser's native prompt), notification cards displaying received messages, an in-app notification center (bell icon with badge), and notification preference settings. Bootstrap's component system handles all these UI elements with accessible, mobile-friendly styling.

## Basic Implementation

### Permission Request UI

```html
<!-- Notification permission prompt (shown after user action) -->
<div class="modal fade" id="notificationModal" tabindex="-1">
  <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content">
      <div class="modal-body text-center py-4">
        <div class="bg-primary bg-opacity-10 rounded-circle d-inline-flex p-3 mb-3">
          <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32"
               fill="var(--bs-primary)" viewBox="0 0 16 16">
            <path d="M8 16a2 2 0 0 0 2-2H6a2 2 0 0 0 2 2zM8 1.918l-.797.161A4.002 4.002 0 0 0 4 6c0 .628-.134 2.197-.459 3.742-.16.767-.376 1.566-.663 2.258h10.244c-.287-.692-.502-1.49-.663-2.258C12.134 8.197 12 6.628 12 6a4.002 4.002 0 0 0-3.203-3.92L8 1.917z"/>
          </svg>
        </div>
        <h5>Stay Updated</h5>
        <p class="text-muted">
          Get notified about new messages, updates, and important alerts.
        </p>
        <div class="d-grid gap-2">
          <button class="btn btn-primary" id="enable-notifications">
            Enable Notifications
          </button>
          <button class="btn btn-outline-secondary" data-bs-dismiss="modal">
            Maybe Later
          </button>
        </div>
      </div>
    </div>
  </div>
</div>
```

```js
// notifications.js
document.getElementById('enable-notifications').addEventListener('click', async () => {
  const permission = await Notification.requestPermission();

  if (permission === 'granted') {
    const modal = bootstrap.Modal.getInstance(
      document.getElementById('notificationModal')
    );
    modal.hide();

    await subscribeToPush();
    showToast('Notifications enabled!', 'success');
  }
});

async function subscribeToPush() {
  const registration = await navigator.serviceWorker.ready;
  const subscription = await registration.pushManager.subscribe({
    userVisibleOnly: true,
    applicationServerKey: urlBase64ToUint8Array(VAPID_PUBLIC_KEY),
  });

  await fetch('/api/push/subscribe', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(subscription),
  });
}
```

## Advanced Variations

### In-App Notification Center

```html
<!-- Bell icon with badge -->
<div class="dropdown">
  <button class="btn btn-link nav-link position-relative p-2"
          data-bs-toggle="dropdown" aria-label="Notifications" id="notif-btn">
    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20"
         fill="currentColor" viewBox="0 0 16 16">
      <path d="M8 16a2 2 0 0 0 2-2H6a2 2 0 0 0 2 2zM8 1.918l-.797.161A4.002 4.002 0 0 0 4 6c0 .628-.134 2.197-.459 3.742-.16.767-.376 1.566-.663 2.258h10.244c-.287-.692-.502-1.49-.663-2.258C12.134 8.197 12 6.628 12 6a4.002 4.002 0 0 0-3.203-3.92L8 1.917z"/>
    </svg>
    <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger"
          id="notif-count">
      3
      <span class="visually-hidden">unread notifications</span>
    </span>
  </button>
  <div class="dropdown-menu dropdown-menu-end p-0" style="width: 320px;">
    <div class="p-3 border-bottom d-flex justify-content-between align-items-center">
      <h6 class="mb-0">Notifications</h6>
      <button class="btn btn-sm btn-link text-decoration-none" id="mark-all-read">
        Mark all read
      </button>
    </div>
    <div class="overflow-auto" style="max-height: 400px;" id="notif-list">
      <div class="list-group list-group-flush">
        <a href="#" class="list-group-item list-group-item-action">
          <div class="d-flex">
            <div class="bg-primary bg-opacity-10 rounded p-2 me-3">
              <svg width="16" height="16" fill="var(--bs-primary)" viewBox="0 0 16 16">
                <path d="M0 4a2 2 0 0 1 2-2h12a2 2 0 0 1 2 2v8a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2V4zm2-1a1 1 0 0 0-1 1v.217l7 4.2 7-4.2V4a1 1 0 0 0-1-1H2zm13 2.383-4.708 2.825L15 11.105V5.383zm-.034 6.876-5.64-3.471L8 9.583l-1.326-.795-5.64 3.47A1 1 0 0 0 2 13h12a1 1 0 0 0 .966-.741zM1 11.105l4.708-2.897L1 5.383v5.722z"/>
              </svg>
            </div>
            <div class="flex-grow-1">
              <div class="small fw-semibold">New message received</div>
              <div class="small text-muted">2 minutes ago</div>
            </div>
            <span class="badge bg-primary rounded-pill align-self-start">New</span>
          </div>
        </a>
        <a href="#" class="list-group-item list-group-item-action">
          <div class="d-flex">
            <div class="bg-success bg-opacity-10 rounded p-2 me-3">
              <svg width="16" height="16" fill="var(--bs-success)" viewBox="0 0 16 16">
                <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>
              </svg>
            </div>
            <div class="flex-grow-1">
              <div class="small fw-semibold">Task completed</div>
              <div class="small text-muted">1 hour ago</div>
            </div>
          </div>
        </a>
      </div>
    </div>
    <div class="p-2 border-top text-center">
      <a href="/notifications" class="small">View all notifications</a>
    </div>
  </div>
</div>
```

### In-App Toast Notifications

```js
// Show notification as toast
function showInAppNotification({ title, body, icon, url }) {
  const toastContainer = document.querySelector('.toast-container');
  const id = `toast-${Date.now()}`;

  const toastHtml = `
    <div id="${id}" class="toast" role="alert" aria-live="assertive" aria-atomic="true">
      <div class="toast-header">
        ${icon ? `<img src="${icon}" class="rounded me-2" width="20" height="20" alt="">` : ''}
        <strong class="me-auto">${title}</strong>
        <small class="text-muted">Just now</small>
        <button type="button" class="btn-close" data-bs-dismiss="toast"></button>
      </div>
      <div class="toast-body">
        ${body}
        ${url ? `<a href="${url}" class="btn btn-sm btn-primary mt-2">View</a>` : ''}
      </div>
    </div>
  `;

  toastContainer.insertAdjacentHTML('beforeend', toastHtml);
  const toastEl = document.getElementById(id);
  new bootstrap.Toast(toastEl, { autohide: true, delay: 5000 }).show();

  toastEl.addEventListener('hidden.bs.toast', () => toastEl.remove());
}
```

### Notification Settings Panel

```html
<div class="card">
  <div class="card-header">
    <h5 class="mb-0">Notification Preferences</h5>
  </div>
  <div class="card-body">
    <div class="form-check form-switch mb-3">
      <input class="form-check-input" type="checkbox" id="notif-messages" checked>
      <label class="form-check-label" for="notif-messages">
        <strong>Messages</strong>
        <div class="small text-muted">New messages and replies</div>
      </label>
    </div>
    <div class="form-check form-switch mb-3">
      <input class="form-check-input" type="checkbox" id="notif-updates" checked>
      <label class="form-check-label" for="notif-updates">
        <strong>Updates</strong>
        <div class="small text-muted">Product updates and announcements</div>
      </label>
    </div>
    <div class="form-check form-switch">
      <input class="form-check-input" type="checkbox" id="notif-marketing">
      <label class="form-check-label" for="notif-marketing">
        <strong>Marketing</strong>
        <div class="small text-muted">Tips, offers, and promotions</div>
      </label>
    </div>
  </div>
</div>
```

## Best Practices

1. **Never trigger the browser's native permission prompt on page load** — show a custom Bootstrap modal first, then request permission on user action.
2. **Use Bootstrap toasts** for in-app notifications that auto-dismiss after a few seconds.
3. **Use a dropdown** for the notification center with scrollable list and "Mark all read" action.
4. **Show unread count badge** on the notification bell using Bootstrap's `badge rounded-pill`.
5. **Use `aria-live="assertive"`** on toast containers for screen reader announcement of new notifications.
6. **Include `visually-hidden` text** on the badge count for screen readers: "3 unread notifications".
7. **Cache notification data** in IndexedDB for offline access to notification history.
8. **Group notifications** by type using different icon backgrounds (bg-primary, bg-success, bg-danger).
9. **Use form switches** (`form-check-switch`) for notification preference toggles.
10. **Respect user preferences** — only send notifications for categories the user has enabled.

## Common Pitfalls

1. **Triggering `Notification.requestPermission()` on page load** causes most users to block notifications.
2. **Not handling denied permission gracefully** — provide a way to re-enable via browser settings.
3. **Missing `userVisibleOnly: true`** in `pushManager.subscribe()` causes Chrome to reject the subscription.
4. **Not testing on real devices** — push notifications behave differently on iOS Safari, Android Chrome, and desktop.
5. **Forgetting to unsubscribe** when users disable notifications leads to silent failures.

## Accessibility Considerations

Notification permission modals must be fully accessible with focus trapping (Bootstrap's modal handles this). Notification badges include `visually-hidden` text for screen readers. Toast notifications use `role="alert"` and `aria-live="assertive"`. The notification list uses `role="feed"` or `aria-label="Notifications"`. Mark-all-read button includes `aria-label`. Individual notification items use `aria-label` describing the notification content and timestamp.

## Responsive Behavior

The notification dropdown adapts to screen width with `dropdown-menu-end` alignment. On mobile, notifications display full-width in the dropdown. The notification list scrolls independently with `max-height: 400px` and `overflow-auto`. Toast notifications use `position-fixed bottom-0 end-0 p-3` and adapt to mobile screens. Notification settings use Bootstrap's `card` component that stacks vertically on small screens.