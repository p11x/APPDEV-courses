---
title: "REST API Loading States"
description: "Implement loading spinners, skeleton screens, and error states for REST API calls using Bootstrap 5 components"
difficulty: 2
tags: [api, rest, loading, skeleton, error-handling]
prerequisites:
  - "Bootstrap 5 spinner and alert components"
  - "JavaScript Fetch API"
  - "Async/await fundamentals"
---

## Overview

REST API integration requires clear loading, success, and error states to maintain user confidence. Bootstrap 5 provides spinners for loading indicators, placeholder components for skeleton screens, and alerts for error feedback. This section covers implementing all three states with the Fetch API, handling network timeouts, retry patterns, and creating smooth transitions between states.

## Basic Implementation

### Fetch with Loading Spinner

Toggle a Bootstrap spinner during API requests.

```html
<div id="userContainer">
  <div class="text-center py-5" id="loadingState">
    <div class="spinner-border text-primary" role="status">
      <span class="visually-hidden">Loading users...</span>
    </div>
    <p class="mt-2 text-muted">Loading users...</p>
  </div>
</div>

<script>
  async function fetchUsers() {
    const container = document.getElementById('userContainer');
    const loading = document.getElementById('loadingState');

    try {
      const response = await fetch('https://jsonplaceholder.typicode.com/users');
      if (!response.ok) throw new Error(`HTTP ${response.status}`);
      const users = await response.json();

      loading.remove();
      users.forEach(user => {
        container.innerHTML += `
          <div class="card mb-2">
            <div class="card-body">
              <h6 class="card-title">${user.name}</h6>
              <p class="card-text text-muted small">${user.email}</p>
            </div>
          </div>`;
      });
    } catch (error) {
      loading.remove();
      container.innerHTML = `
        <div class="alert alert-danger d-flex align-items-center">
          <i class="bi bi-exclamation-triangle-fill me-2"></i>
          <div>Failed to load users. <a href="#" onclick="fetchUsers(); return false;">Retry</a></div>
        </div>`;
    }
  }
  fetchUsers();
</script>
```

### Error Alert with Retry

```html
<div class="alert alert-danger d-flex justify-content-between align-items-center" role="alert">
  <div>
    <i class="bi bi-wifi-off me-2"></i>
    <strong>Connection failed.</strong> Unable to reach the server.
  </div>
  <button class="btn btn-sm btn-outline-danger" onclick="retryRequest()">
    <i class="bi bi-arrow-clockwise me-1"></i>Retry
  </button>
</div>
```

## Advanced Variations

### Skeleton Screen

Bootstrap's placeholder component creates skeleton loading UIs that match content structure.

```html
<div id="postContainer">
  <!-- Skeleton shown during loading -->
  <div class="card mb-3" id="skeletonCard">
    <div class="card-body">
      <div class="placeholder-glow">
        <span class="placeholder col-8 mb-2"></span>
        <span class="placeholder col-12 mb-1"></span>
        <span class="placeholder col-10 mb-1"></span>
        <span class="placeholder col-6"></span>
      </div>
    </div>
  </div>
</div>

<script>
  async function loadPost() {
    try {
      const response = await fetch('https://jsonplaceholder.typicode.com/posts/1');
      const post = await response.json();

      document.getElementById('skeletonCard').replaceWith(
        Object.assign(document.createElement('div'), {
          className: 'card mb-3',
          innerHTML: `
            <div class="card-body">
              <h5 class="card-title">${post.title}</h5>
              <p class="card-text">${post.body}</p>
            </div>`
        })
      );
    } catch (error) {
      document.getElementById('skeletonCard').innerHTML = `
        <div class="card-body">
          <div class="alert alert-danger mb-0">
            Failed to load post. <a href="#" onclick="loadPost(); return false;">Retry</a>
          </div>
        </div>`;
    }
  }
  loadPost();
</script>
```

### Multi-State Component with Timeout

```html
<div id="dataContainer" class="position-relative">
  <div id="dataContent"></div>
</div>

<script>
  const controller = new AbortController();
  const timeout = setTimeout(() => controller.abort(), 8000);

  async function loadData() {
    const content = document.getElementById('dataContent');

    content.innerHTML = `
      <div class="d-flex align-items-center justify-content-center py-5">
        <div class="spinner-border text-primary me-3" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
        <span>Loading data...</span>
      </div>`;

    try {
      const response = await fetch('https://jsonplaceholder.typicode.com/todos/1', {
        signal: controller.signal
      });
      clearTimeout(timeout);

      if (!response.ok) throw new Error(`Server error: ${response.status}`);
      const data = await response.json();

      content.innerHTML = `
        <div class="alert alert-success d-flex align-items-center">
          <i class="bi bi-check-circle me-2"></i>
          <strong>${data.title}</strong>
        </div>`;
    } catch (error) {
      const message = error.name === 'AbortError'
        ? 'Request timed out after 8 seconds.'
        : error.message;

      content.innerHTML = `
        <div class="alert alert-danger">
          <i class="bi bi-x-circle me-2"></i>${message}
          <button class="btn btn-sm btn-outline-danger ms-3" onclick="loadData()">Retry</button>
        </div>`;
    }
  }
  loadData();
</script>
```

## Best Practices

1. **Show loading immediately** - render spinner or skeleton before the fetch call starts.
2. **Use skeleton screens** over spinners for content-heavy pages to reduce perceived wait time.
3. **Always implement error states** with clear messaging and retry options.
4. **Use `AbortController`** for request timeouts to prevent indefinite loading.
5. **Replace loading elements** rather than hiding them to avoid empty DOM nodes.
6. **Use `replaceWith()`** for smooth transitions from skeleton to content.
7. **Provide contextual error messages** - distinguish network errors from server errors.
8. **Disable retry buttons** during re-fetch to prevent duplicate requests.
9. **Use `role="status"`** on loading indicators for screen reader announcements.
10. **Log errors to monitoring** services in production, not just the UI.
11. **Use placeholder glow** variant for skeleton screens that feel responsive.
12. **Set reasonable timeout values** - 8-10 seconds for API calls, shorter for critical paths.

## Common Pitfalls

1. **Not handling network errors** - fetch only rejects on network failure, not HTTP errors.
2. **Missing `response.ok` check** lets 4xx/5xx responses pass as successful.
3. **Infinite loading states** when error handling is missing or incomplete.
4. **Not clearing timeouts** after successful responses causes memory leaks.
5. **Showing spinners for instant responses** (<200ms) causes visual flickering.
6. **Hardcoded error messages** that don't reflect the actual failure reason.
7. **Not sanitizing API responses** before inserting into DOM creates XSS vulnerabilities.
8. **Forgetting to re-enable buttons** after failed requests prevents retry.

## Accessibility Considerations

- Loading spinners require `role="status"` and `visually-hidden` descriptive text.
- Error alerts should use `role="alert"` for immediate screen reader announcement.
- Skeleton screens should include `aria-busy="true"` on the container.
- Retry buttons must be keyboard accessible and clearly labeled.
- Timeout errors should be announced via `aria-live` regions.
- Loading state transitions should not cause focus to be lost.

## Responsive Behavior

- Skeleton screens should match the layout structure at each breakpoint.
- Loading spinners should be centered and sized appropriately for mobile.
- Error alerts should stack action buttons vertically on narrow screens.
- Use `flex-column flex-sm-row` for error alert layouts on mobile.
- Ensure touch-friendly retry buttons (minimum 44px touch target).
