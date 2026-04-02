---
title: "Offline-First UI with Bootstrap"
topic: "Mobile First PWA"
difficulty: 3
duration: "45 minutes"
prerequisites: ["Service Workers", "Cache API", "IndexedDB", "Bootstrap alerts/toasts"]
tags: ["offline", "service-worker", "cache", "sync", "bootstrap"]
---

## Overview

Offline-first architecture ensures applications remain functional without network connectivity. The UI layer must communicate connectivity status, display cached content, queue actions for later synchronization, and handle sync conflicts gracefully. Bootstrap 5 components — alerts, badges, toasts, spinners, and badges — provide the visual language for offline indicators, sync status, and cached content banners.

The strategy involves: monitoring online/offline events, displaying connectivity status with Bootstrap alerts, storing data in IndexedDB for offline access, queuing user actions (form submissions, edits) in a sync queue, and processing the queue when connectivity returns. Service workers handle asset caching, while the application layer manages data caching and synchronization.

## Basic Implementation

### Online/Offline Status Indicator

```js
// utils/connectivity.js
export class ConnectivityMonitor {
  constructor(onChange) {
    this.onChange = onChange;
    this.isOnline = navigator.onLine;

    window.addEventListener('online', () => {
      this.isOnline = true;
      this.onChange(true);
    });

    window.addEventListener('offline', () => {
      this.isOnline = false;
      this.onChange(false);
    });
  }
}
```

```html
<!-- Offline status banner -->
<div id="offline-banner" class="alert alert-warning alert-dismissible d-none mb-0 rounded-0"
     role="alert">
  <div class="container d-flex align-items-center">
    <span class="badge bg-warning text-dark me-2">
      <i class="bi bi-wifi-off"></i> Offline
    </span>
    <span>You're offline. Changes will sync when reconnected.</span>
    <div class="ms-auto">
      <div class="spinner-border spinner-border-sm text-warning" role="status">
        <span class="visually-hidden">Waiting for connection...</span>
      </div>
    </div>
  </div>
</div>

<!-- Online restored toast -->
<div class="toast-container position-fixed bottom-0 end-0 p-3">
  <div id="online-toast" class="toast" role="alert" aria-live="assertive">
    <div class="toast-header bg-success text-white">
      <i class="bi bi-wifi me-2"></i>
      <strong class="me-auto">Back Online</strong>
      <small>Just now</small>
      <button type="button" class="btn-close btn-close-white" data-bs-dismiss="toast"></button>
    </div>
    <div class="toast-body">
      <div class="d-flex align-items-center">
        <span>Syncing <span id="sync-count">0</span> pending changes...</span>
        <div class="spinner-border spinner-border-sm ms-auto text-success" role="status"></div>
      </div>
    </div>
  </div>
</div>

<script>
  import { ConnectivityMonitor } from './utils/connectivity.js';

  const banner = document.getElementById('offline-banner');
  const toastEl = document.getElementById('online-toast');
  const toast = new bootstrap.Toast(toastEl);

  const monitor = new ConnectivityMonitor((online) => {
    if (online) {
      banner.classList.add('d-none');
      document.getElementById('sync-count').textContent = getPendingCount();
      toast.show();
    } else {
      banner.classList.remove('d-none');
    }
  });
</script>
```

## Advanced Variations

### Sync Queue with IndexedDB

```js
// utils/syncQueue.js
const DB_NAME = 'app-sync-queue';
const STORE_NAME = 'pending-actions';

function openDB() {
  return new Promise((resolve, reject) => {
    const request = indexedDB.open(DB_NAME, 1);
    request.onupgradeneeded = () => {
      request.result.createObjectStore(STORE_NAME, { keyPath: 'id', autoIncrement: true });
    };
    request.onsuccess = () => resolve(request.result);
    request.onerror = () => reject(request.error);
  });
}

export async function addToQueue(action) {
  const db = await openDB();
  const tx = db.transaction(STORE_NAME, 'readwrite');
  tx.objectStore(STORE_NAME).add({
    ...action,
    timestamp: Date.now(),
    status: 'pending',
  });
  return tx.complete;
}

export async function processQueue(processor) {
  const db = await openDB();
  const tx = db.transaction(STORE_NAME, 'readwrite');
  const store = tx.objectStore(STORE_NAME);
  const items = await new Promise(r => { store.getAll().onsuccess = e => r(e.target.result); });

  for (const item of items) {
    try {
      await processor(item);
      store.delete(item.id);
    } catch (err) {
      store.put({ ...item, status: 'failed', error: err.message });
    }
  }
}

export async function getPendingCount() {
  const db = await openDB();
  const tx = db.transaction(STORE_NAME, 'readonly');
  const count = await new Promise(r => { tx.objectStore(STORE_NAME).count().onsuccess = e => r(e.target.result); });
  return count;
}
```

### Offline-Aware Form

```html
<form id="offline-form" class="needs-validation" novalidate>
  <div class="mb-3">
    <label class="form-label">Task Title</label>
    <input type="text" class="form-control" name="title" required>
  </div>
  <div class="mb-3">
    <label class="form-label">Description</label>
    <textarea class="form-control" name="description" rows="3"></textarea>
  </div>

  <div id="form-status" class="d-none mb-3">
    <div class="alert alert-info d-flex align-items-center py-2">
      <div class="spinner-border spinner-border-sm me-2"></div>
      <small>Saved locally. Will sync when online.</small>
    </div>
  </div>

  <button type="submit" class="btn btn-primary" id="submit-btn">
    <span class="submit-text">Save Task</span>
    <span class="spinner-border spinner-border-sm d-none" role="status"></span>
  </button>
</form>

<script type="module">
  import { addToQueue } from './utils/syncQueue.js';

  const form = document.getElementById('offline-form');
  const status = document.getElementById('form-status');
  const submitBtn = document.getElementById('submit-btn');

  form.addEventListener('submit', async (e) => {
    e.preventDefault();

    const data = Object.fromEntries(new FormData(form));
    submitBtn.querySelector('.spinner-border').classList.remove('d-none');
    submitBtn.disabled = true;

    if (navigator.onLine) {
      await fetch('/api/tasks', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data),
      });
    } else {
      await addToQueue({ type: 'CREATE_TASK', data });
      status.classList.remove('d-none');
    }

    submitBtn.querySelector('.spinner-border').classList.add('d-none');
    submitBtn.disabled = false;
    form.reset();
  });
</script>
```

### Cached Content Badge

```html
<div class="card mb-3">
  <div class="card-body">
    <div class="d-flex justify-content-between align-items-start">
      <h5 class="card-title">Cached Article</h5>
      <span class="badge bg-secondary">
        <i class="bi bi-clock-history"></i> Cached 2h ago
      </span>
    </div>
    <p class="card-text">This content is available offline.</p>
    <small class="text-muted">
      <i class="bi bi-database"></i> Stored in local cache
    </small>
  </div>
</div>
```

## Best Practices

1. **Monitor `navigator.onLine`** and `online`/`offline` events for real-time connectivity detection.
2. **Use Bootstrap alerts** for persistent offline banners and toasts for transient status updates.
3. **Queue user actions in IndexedDB** rather than losing them when offline.
4. **Show pending action count** in the offline banner to inform users of queued changes.
5. **Use `aria-live="polite"`** on status regions so screen readers announce connectivity changes.
6. **Disable submit buttons during sync** with spinner feedback to prevent duplicate submissions.
7. **Cache API responses** in service workers with network-first strategy for fresh-when-available behavior.
8. **Use Bootstrap badges** to indicate cached content age and sync status.
9. **Implement retry with exponential backoff** for failed sync operations.
10. **Provide manual sync trigger** button so users can force synchronization.

## Common Pitfalls

1. **Relying solely on `navigator.onLine`** — it only detects network interface status, not actual internet connectivity.
2. **Not storing form data before attempting network requests** causes data loss if the request fails.
3. **Infinite sync loops** occur when failed items are re-queued without error limits.
4. **Not showing sync progress** leaves users uncertain about pending changes.
5. **Forgetting to clear processed queue items** causes duplicate operations on reconnect.

## Accessibility Considerations

Offline status must be announced to assistive technologies. Use `role="alert"` and `aria-live="assertive"` for offline banners. Use `aria-live="polite"` for sync completion notifications. Ensure offline-cached content maintains the same ARIA markup as online content. Sync status spinners should include `visually-hidden` text. Form submission feedback must be accessible even in offline mode — use Bootstrap's validation feedback classes for offline-persisted validation states.

## Responsive Behavior

Offline banners use Bootstrap's `alert` component with full-width layout on mobile. Toast notifications position in the bottom-right corner (`position-fixed bottom-0 end-0`) and adapt to screen width. Sync status indicators use `badge` components that scale within their containers. Mobile-first design ensures offline UI is prominent on small screens with appropriate padding and tap targets. The `container` class constrains offline banner content width on larger screens.