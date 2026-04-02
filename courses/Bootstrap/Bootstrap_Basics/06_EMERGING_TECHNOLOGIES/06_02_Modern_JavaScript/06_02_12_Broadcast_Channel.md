---
title: "BroadcastChannel with Bootstrap"
slug: "broadcast-channel-bootstrap"
difficulty: 3
tags: ["bootstrap", "javascript", "broadcast-channel", "cross-tab", "sync"]
prerequisites:
  - "06_02_11_Web_Workers_UI"
  - "06_02_09_Resize_Observer"
related:
  - "06_02_10_Performance_Observer"
  - "06_01_14_AI_Personalization_UI"
duration: "30 minutes"
---

# BroadcastChannel with Bootstrap

## Overview

BroadcastChannel enables same-origin communication between browser tabs, windows, and iframes. For Bootstrap applications, this means syncing theme preferences, cart contents, notification states, and form data across multiple open tabs. When a user changes the theme in one tab, all other tabs update simultaneously. When a form is submitted in one tab, other tabs can refresh their data tables. This creates a cohesive multi-tab experience without server roundtrips.

## Basic Implementation

Sync Bootstrap dark/light theme across all open tabs.

```html
<div class="container mt-4">
  <div class="card">
    <div class="card-body">
      <h5 class="card-title">Theme Sync</h5>
      <div class="btn-group" role="group" aria-label="Theme selector">
        <input type="radio" class="btn-check" name="theme" id="lightTheme" value="light" checked>
        <label class="btn btn-outline-secondary" for="lightTheme">
          <i class="bi bi-sun"></i> Light
        </label>
        <input type="radio" class="btn-check" name="theme" id="darkTheme" value="dark">
        <label class="btn btn-outline-secondary" for="darkTheme">
          <i class="bi bi-moon"></i> Dark
        </label>
        <input type="radio" class="btn-check" name="theme" id="autoTheme" value="auto">
        <label class="btn btn-outline-secondary" for="autoTheme">
          <i class="bi bi-circle-half"></i> Auto
        </label>
      </div>
      <div class="mt-3">
        <span class="badge bg-info" id="syncStatus">Synced across tabs</span>
      </div>
    </div>
  </div>
</div>

<script>
const channel = new BroadcastChannel('app-sync');

channel.onmessage = (e) => {
  if (e.data.type === 'theme-change') {
    document.documentElement.setAttribute('data-bs-theme', e.data.theme);
    document.querySelector(`input[value="${e.data.theme}"]`).checked = true;
    document.getElementById('syncStatus').textContent = `Theme synced from another tab`;
  }
};

document.querySelectorAll('input[name="theme"]').forEach(radio => {
  radio.addEventListener('change', (e) => {
    const theme = e.target.value;
    document.documentElement.setAttribute('data-bs-theme', theme);
    channel.postMessage({ type: 'theme-change', theme });
  });
});
</script>
```

## Advanced Variations

### Cart Synchronization

Keep shopping cart state synchronized across tabs with toast notifications.

```html
<div class="toast-container position-fixed bottom-0 end-0 p-3">
  <div class="toast" id="syncToast" role="alert">
    <div class="toast-header">
      <i class="bi bi-arrow-repeat me-2"></i>
      <strong class="me-auto">Tab Sync</strong>
      <small>Just now</small>
      <button type="button" class="btn-close" data-bs-dismiss="toast"></button>
    </div>
    <div class="toast-body" id="syncToastBody"></div>
  </div>
</div>

<nav class="navbar navbar-light bg-light">
  <div class="container-fluid">
    <span class="navbar-brand">Shop</span>
    <button class="btn btn-outline-primary position-relative" id="cartBtn">
      <i class="bi bi-cart3"></i>
      <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger" id="cartCount">
        0
      </span>
    </button>
  </div>
</nav>

<script>
const cartChannel = new BroadcastChannel('cart-sync');

function updateCartCount(count) {
  document.getElementById('cartCount').textContent = count;
}

function showSyncToast(message) {
  document.getElementById('syncToastBody').textContent = message;
  new bootstrap.Toast(document.getElementById('syncToast')).show();
}

cartChannel.onmessage = (e) => {
  if (e.data.type === 'cart-update') {
    updateCartCount(e.data.count);
    showSyncToast(`Cart updated in another tab: ${e.data.count} items`);
  }
  if (e.data.type === 'cart-cleared') {
    updateCartCount(0);
    showSyncToast('Cart was cleared in another tab');
  }
};

function addToCart(item) {
  const count = parseInt(document.getElementById('cartCount').textContent) + 1;
  updateCartCount(count);
  cartChannel.postMessage({ type: 'cart-update', count, item });
}
</script>
```

### Multi-Tab Form Lock

Prevent the same form from being edited simultaneously in multiple tabs.

```html
<div class="card" id="formCard">
  <div class="card-header d-flex justify-content-between">
    <h5 class="mb-0">Edit Profile</h5>
    <span class="badge bg-success" id="lockStatus">Editing</span>
  </div>
  <div class="card-body">
    <div id="lockOverlay" class="d-none">
      <div class="alert alert-warning">
        <i class="bi bi-lock"></i> This form is being edited in another tab.
        <button class="btn btn-sm btn-warning ms-2" id="takeOver">Take over</button>
      </div>
    </div>
    <form id="profileForm">
      <div class="mb-3">
        <label class="form-label">Name</label>
        <input type="text" class="form-control" id="nameField">
      </div>
      <div class="mb-3">
        <label class="form-label">Email</label>
        <input type="email" class="form-control" id="emailField">
      </div>
      <button type="submit" class="btn btn-primary" id="saveBtn">Save</button>
    </form>
  </div>
</div>

<script>
const formChannel = new BroadcastChannel('form-lock');
const tabId = crypto.randomUUID();

formChannel.onmessage = (e) => {
  if (e.data.type === 'lock-acquired' && e.data.tabId !== tabId) {
    document.getElementById('lockOverlay').classList.remove('d-none');
    document.getElementById('lockStatus').className = 'badge bg-danger';
    document.getElementById('lockStatus').textContent = 'Locked';
    document.getElementById('saveBtn').disabled = true;
  }
  if (e.data.type === 'lock-released' && e.data.tabId !== tabId) {
    document.getElementById('lockOverlay').classList.add('d-none');
    document.getElementById('lockStatus').className = 'badge bg-success';
    document.getElementById('lockStatus').textContent = 'Editing';
    document.getElementById('saveBtn').disabled = false;
  }
};

document.getElementById('nameField').addEventListener('focus', () => {
  formChannel.postMessage({ type: 'lock-acquired', tabId });
});

document.getElementById('takeOver').addEventListener('click', () => {
  formChannel.postMessage({ type: 'lock-acquired', tabId });
  document.getElementById('lockOverlay').classList.add('d-none');
  document.getElementById('lockStatus').className = 'badge bg-success';
  document.getElementById('saveBtn').disabled = false;
});
</script>
```

### Live Notification Counter

Sync notification badge counts across tabs so dismissed notifications disappear everywhere.

```html
<button class="btn btn-outline-secondary position-relative" data-bs-toggle="dropdown" id="notifBtn">
  <i class="bi bi-bell"></i>
  <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger" id="notifBadge">
    5
  </span>
</button>
<ul class="dropdown-menu dropdown-menu-end" id="notifList" style="width: 300px;">
  <li class="dropdown-header">Notifications</li>
</ul>

<script>
const notifChannel = new BroadcastChannel('notifications');

function setNotifCount(count) {
  const badge = document.getElementById('notifBadge');
  badge.textContent = count;
  badge.classList.toggle('d-none', count === 0);
}

notifChannel.onmessage = (e) => {
  if (e.data.type === 'notif-read') {
    setNotifCount(e.data.remaining);
    const item = document.querySelector(`[data-notif-id="${e.data.notifId}"]`);
    if (item) item.classList.add('text-muted', 'text-decoration-line-through');
  }
  if (e.data.type === 'notif-all-read') {
    setNotifCount(0);
  }
};
</script>
```

## Best Practices

1. Use a consistent channel naming convention across the application
2. Include a `tabId` or `source` field in messages to identify the sender
3. Handle the case where BroadcastChannel is not supported (Safari < 15.4)
4. Close channels when pages unload to prevent ghost listeners
5. Use message type enums to avoid string typos across the codebase
6. Debounce rapid state changes before broadcasting to reduce message volume
7. Include timestamps in messages to handle out-of-order delivery
8. Test with multiple tabs open to verify synchronization behavior
9. Use `structuredClone` for complex objects passed through the channel
10. Avoid broadcasting sensitive data like authentication tokens
11. Implement conflict resolution for simultaneous edits across tabs
12. Log cross-tab messages during development for debugging
13. Combine with localStorage as a fallback for older browsers
14. Use separate channels for different data domains (theme, cart, notifications)

## Common Pitfalls

1. **No feature detection**: Calling `new BroadcastChannel()` in unsupported browsers throws an error
2. **Message loops**: Broadcasting changes received from the channel creates infinite loops
3. **Tab identity confusion**: Not distinguishing between own messages and messages from other tabs
4. **Memory leaks**: Not closing channels on page unload
5. **Security**: Trusting message data without validation from other tabs
6. **Message ordering**: Assuming messages arrive in the order they were sent
7. **Cross-origin issues**: BroadcastChannel only works within the same origin

## Accessibility Considerations

Announce cross-tab sync events with `aria-live="polite"` so screen readers inform users of updates. Ensure toast notifications for tab sync are announced to assistive technology. Use `role="status"` on sync indicators. Provide visual feedback (badges, toasts) alongside any state changes received from other tabs. Ensure lock overlays are keyboard-dismissible. Announce form lock status changes to screen readers.

```html
<div aria-live="polite" aria-atomic="true" class="visually-hidden" id="syncAnnounce">
  Theme changed to dark mode by another tab
</div>
```

## Responsive Behavior

Toast notifications should appear in the bottom-right corner on desktop and bottom-center on mobile. Use Bootstrap's `toast-container` positioning classes. Notification dropdowns should be full-width on mobile with `dropdown-menu-end`. Sync status badges remain compact across all screen sizes. Form lock overlays should cover the full card on all breakpoints. Use `position-fixed bottom-0 start-50 translate-middle-x` for mobile toast placement.
