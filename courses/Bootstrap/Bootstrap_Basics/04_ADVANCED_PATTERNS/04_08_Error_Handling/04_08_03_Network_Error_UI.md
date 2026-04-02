---
title: "Network Error UI Patterns"
description: "Connection lost banners, retry mechanisms, and offline fallback UI with Bootstrap components"
difficulty: 2
tags: ["error-handling", "network", "offline", "retry", "bootstrap"]
prerequisites: ["04_08_01_Modal_Error_States"]
---

## Overview

Network failures are inevitable — servers go down, connections drop, and requests time out. Bootstrap provides the components (alerts, spinners, badges, buttons) to build informative network error UI that keeps users informed and offers recovery options. A well-designed network error experience shows a persistent banner when offline, offers retry buttons on failed operations, and gracefully degrades content when data cannot load.

The `navigator.onLine` API and `fetch` error handling form the foundation for detecting network issues, while Bootstrap's alert and toast components provide the visual framework for communicating status.

## Basic Implementation

```html
<!-- Network status banner -->
<div id="offlineBanner" class="alert alert-danger alert-dismissible d-none mb-0 rounded-0"
     role="alert" aria-live="assertive">
  <div class="container d-flex align-items-center justify-content-between">
    <div>
      <i class="bi bi-wifi-off me-2"></i>
      <strong>No internet connection.</strong> Some features may be unavailable.
    </div>
    <button type="button" class="btn btn-sm btn-outline-danger" id="retryConnectionBtn">
      <i class="bi bi-arrow-clockwise me-1"></i>Retry
    </button>
  </div>
</div>
```

```js
// Network status detection and UI updates
const offlineBanner = document.getElementById('offlineBanner');
const retryBtn = document.getElementById('retryConnectionBtn');

function updateNetworkStatus() {
  if (navigator.onLine) {
    offlineBanner.classList.add('d-none');
    document.body.style.paddingTop = '0';
  } else {
    offlineBanner.classList.remove('d-none');
    document.body.style.paddingTop = offlineBanner.offsetHeight + 'px';
  }
}

window.addEventListener('online', updateNetworkStatus);
window.addEventListener('offline', updateNetworkStatus);

// Initial check
updateNetworkStatus();

// Retry connection check
retryBtn.addEventListener('click', async () => {
  retryBtn.disabled = true;
  retryBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-1"></span>Checking...';

  try {
    await fetch('/api/health', { cache: 'no-store', signal: AbortSignal.timeout(5000) });
    offlineBanner.classList.add('d-none');
    document.body.style.paddingTop = '0';
  } catch {
    retryBtn.disabled = false;
    retryBtn.innerHTML = '<i class="bi bi-arrow-clockwise me-1"></i>Retry';
  }
});
```

## Advanced Variations

```html
<!-- Network-aware data loading card -->
<div class="card" id="dashboardCard">
  <div class="card-header d-flex justify-content-between align-items-center">
    <span>Recent Activity</span>
    <span id="connectionBadge" class="badge bg-success">
      <i class="bi bi-wifi me-1"></i>Online
    </span>
  </div>
  <div class="card-body">
    <!-- Loading state -->
    <div id="cardLoading" class="text-center py-4">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
      <p class="text-muted mt-2 mb-0">Loading activity data...</p>
    </div>

    <!-- Error state -->
    <div id="cardError" class="d-none text-center py-4">
      <i class="bi bi-cloud-slash text-danger" style="font-size: 2rem;"></i>
      <p class="mt-2 mb-1">Unable to load activity data.</p>
      <p class="text-muted small">Check your connection and try again.</p>
      <button class="btn btn-outline-primary btn-sm" id="retryLoadBtn">
        <i class="bi bi-arrow-clockwise me-1"></i>Retry
      </button>
    </div>

    <!-- Success state -->
    <div id="cardContent" class="d-none">
      <ul class="list-group list-group-flush" id="activityList"></ul>
    </div>
  </div>
</div>
```

```js
// Resilient fetch with retry logic
async function fetchWithRetry(url, options = {}, maxRetries = 3) {
  let lastError;

  for (let attempt = 1; attempt <= maxRetries; attempt++) {
    try {
      const controller = new AbortController();
      const timeout = setTimeout(() => controller.abort(), 10000);

      const response = await fetch(url, {
        ...options,
        signal: controller.signal
      });

      clearTimeout(timeout);

      if (!response.ok) {
        throw new Error(`HTTP ${response.status}: ${response.statusText}`);
      }

      return await response.json();
    } catch (error) {
      lastError = error;

      if (attempt < maxRetries) {
        const delay = Math.min(1000 * Math.pow(2, attempt - 1), 10000);
        await new Promise(resolve => setTimeout(resolve, delay));
      }
    }
  }

  throw lastError;
}

// Usage with Bootstrap UI states
async function loadDashboard() {
  const loading = document.getElementById('cardLoading');
  const error = document.getElementById('cardError');
  const content = document.getElementById('cardContent');
  const badge = document.getElementById('connectionBadge');

  loading.classList.remove('d-none');
  error.classList.add('d-none');
  content.classList.add('d-none');

  try {
    const data = await fetchWithRetry('/api/activity');
    renderActivity(data);
    loading.classList.add('d-none');
    content.classList.remove('d-none');
    badge.className = 'badge bg-success';
    badge.innerHTML = '<i class="bi bi-wifi me-1"></i>Online';
  } catch (err) {
    loading.classList.add('d-none');
    error.classList.remove('d-none');
    badge.className = 'badge bg-danger';
    badge.innerHTML = '<i class="bi bi-wifi-off me-1"></i>Offline';
  }
}
```

## Best Practices

1. Detect network status with `navigator.onLine` and the `online`/`offline` events
2. Use a persistent banner for connection loss, not a dismissible toast that users might miss
3. Implement exponential backoff for retry attempts to avoid hammering the server
4. Show the offline banner immediately without waiting for a failed request
5. Provide a manual retry button alongside automatic reconnection detection
6. Use `AbortController` with timeouts to fail fast on unresponsive endpoints
7. Cache recent data in memory or localStorage for offline display
8. Disable actions that require network connectivity when offline
9. Show a network status badge in headers so users know their connection state
10. Test error UI by simulating offline mode in Chrome DevTools (Network tab)

## Common Pitfalls

1. **Relying solely on `navigator.onLine`** — This only detects hardware connectivity, not actual internet access; always verify with a health endpoint
2. **No timeout on fetch requests** — Requests to dead servers hang indefinitely without an `AbortController` timeout
3. **Retrying non-retryable errors** — 401/403 responses will never succeed on retry; only retry 5xx and network errors
4. **Retry button without state** — Clicking retry multiple times triggers overlapping requests; disable during retry
5. **Missing loading-to-error transition** — Showing both spinner and error message simultaneously is confusing
6. **Not preserving scroll position** — Re-rendering content after retry can lose the user's scroll position

## Accessibility Considerations

Network status changes must be announced to screen readers using `aria-live="assertive"` on the offline banner. The retry button should be keyboard-accessible and clearly labeled. Error messages should explain what happened and what the user can do about it. Do not rely solely on color to convey online/offline status — use icons and text labels as well.

## Responsive Behavior

The offline banner should span the full width on mobile without horizontal scrolling. Use `alert-dismissible` only on desktop where users can reasonably monitor connection status. On mobile, keep the banner persistent. Ensure the retry button remains tappable (minimum 44x44px touch target) and does not overflow the banner on narrow screens.
