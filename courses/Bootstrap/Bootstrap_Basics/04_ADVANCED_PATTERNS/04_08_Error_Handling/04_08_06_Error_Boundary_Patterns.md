---
title: "Error Boundary Patterns"
description: "Catching JavaScript errors in Bootstrap components and providing fallback rendering"
difficulty: 3
tags: ["error-handling", "javascript", "error-boundary", "bootstrap"]
prerequisites: ["04_08_01_Modal_Error_States", "04_08_04_Loading_Error_States"]
---

## Overview

Error boundaries catch JavaScript runtime errors in a component subtree and prevent the entire page from crashing. While React has a formal `ErrorBoundary` concept, vanilla JavaScript Bootstrap applications need manual error handling patterns. The goal is to isolate failures — a broken chart widget should not crash the navigation bar or other functioning components.

Each Bootstrap component container can be wrapped with try/catch logic that catches initialization errors, runtime exceptions, and unhandled promise rejections, then renders a fallback UI in place of the failed component.

## Basic Implementation

```html
<!-- Error boundary wrapper for Bootstrap components -->
<div class="error-boundary" data-component="user-widget">
  <div class="component-content">
    <div class="card">
      <div class="card-body">
        <h5 class="card-title">User Dashboard</h5>
        <p id="userStatus">Loading...</p>
        <button class="btn btn-primary" id="loadDataBtn">Load Data</button>
      </div>
    </div>
  </div>

  <!-- Fallback UI (hidden by default) -->
  <div class="component-fallback d-none">
    <div class="card border-danger">
      <div class="card-body text-center">
        <i class="bi bi-bug text-danger mb-2" style="font-size: 1.5rem;"></i>
        <h6 class="card-title text-danger">Component Error</h6>
        <p class="card-text text-muted small" data-error-message>This widget failed to load.</p>
        <button class="btn btn-outline-danger btn-sm" data-retry>Reload Widget</button>
      </div>
    </div>
  </div>
</div>
```

```js
// Generic error boundary for Bootstrap components
class BootstrapErrorBoundary {
  constructor(container) {
    this.container = container;
    this.content = container.querySelector('.component-content');
    this.fallback = container.querySelector('.component-fallback');
    this.retryBtn = container.querySelector('[data-retry]');
    this.errorDisplay = container.querySelector('[data-error-message]');
    this.componentName = container.dataset.component || 'Unknown';

    this.retryBtn?.addEventListener('click', () => this.retry());
    window.addEventListener('error', (e) => this.handleGlobalError(e));
  }

  wrap(fn) {
    try {
      return fn();
    } catch (error) {
      this.showError(error);
      return null;
    }
  }

  async wrapAsync(fn) {
    try {
      return await fn();
    } catch (error) {
      this.showError(error);
      return null;
    }
  }

  showError(error) {
    console.error(`[${this.componentName}]`, error);
    this.content.classList.add('d-none');
    this.fallback.classList.remove('d-none');
    this.errorDisplay.textContent = error.message || 'An unexpected error occurred.';
  }

  retry() {
    this.fallback.classList.add('d-none');
    this.content.classList.remove('d-none');
    this.onRetry?.();
  }

  handleGlobalError(event) {
    if (this.container.contains(event.target)) {
      event.preventDefault();
      this.showError(new Error(event.message));
    }
  }
}

// Usage
const widget = new BootstrapErrorBoundary(document.querySelector('[data-component="user-widget"]'));
widget.onRetry = () => loadUserData();

widget.wrapAsync(async () => {
  const response = await fetch('/api/user/dashboard');
  if (!response.ok) throw new Error('Failed to load dashboard data');
  const data = await response.json();
  document.getElementById('userStatus').textContent = `Welcome, ${data.name}`;
});
```

## Advanced Variations

```js
// Multi-component error boundary manager
class ErrorBoundaryManager {
  constructor() {
    this.boundaries = new Map();
    this.init();
  }

  init() {
    // Catch unhandled promise rejections
    window.addEventListener('unhandledrejection', (event) => {
      console.error('Unhandled rejection:', event.reason);
      event.preventDefault();
    });

    // Register all error-boundary components
    document.querySelectorAll('.error-boundary').forEach(el => {
      const name = el.dataset.component;
      this.boundaries.set(name, new BootstrapErrorBoundary(el));
    });
  }

  get(name) {
    return this.boundaries.get(name);
  }

  async initComponent(name, initFn) {
    const boundary = this.get(name);
    if (!boundary) {
      console.warn(`No error boundary found for: ${name}`);
      return initFn();
    }
    return boundary.wrapAsync(initFn);
  }

  resetAll() {
    this.boundaries.forEach(b => b.retry());
  }
}

// Initialize all components with error boundaries
const errorManager = new ErrorBoundaryManager();

errorManager.initComponent('user-widget', async () => {
  const data = await fetch('/api/user').then(r => r.json());
  renderUserWidget(data);
});

errorManager.initComponent('chart-widget', async () => {
  const data = await fetch('/api/stats').then(r => r.json());
  renderChart(data);
});

errorManager.initComponent('activity-feed', async () => {
  const data = await fetch('/api/activity').then(r => r.json());
  renderFeed(data);
});
```

## Best Practices

1. Wrap each independent Bootstrap component in its own error boundary
2. Display a meaningful fallback UI instead of a blank space when a component fails
3. Include a retry button in the fallback that re-attempts initialization
4. Log caught errors with the component name for debugging
5. Use `unhandledrejection` to catch async errors not handled by try/catch
6. Keep error boundaries granular — one broken widget should not crash the page
7. Provide a global error handler as a last resort for uncaught exceptions
8. Preserve existing UI state when a component partially fails
9. Test error boundaries by intentionally throwing errors in development
10. Use `data-*` attributes to mark error boundary containers for programmatic access

## Common Pitfalls

1. **Catching errors too broadly** — A try/catch around the entire page hides errors from developer tools and logging
2. **Silent failures** — Catching errors without logging or displaying anything masks bugs during development
3. **No retry mechanism** — Transient network errors need retry; fallback UI without retry forces page reload
4. **Not cleaning up on error** — Event listeners and intervals from the failed component remain active if not cleaned up
5. **Fallback UI breaks layout** — Error cards with different dimensions shift surrounding components; match dimensions
6. **Catching and re-throwing** — Re-throwing after showing fallback UI creates duplicate error reports

## Accessibility Considerations

Error boundary fallbacks must be accessible. Use `role="alert"` on error messages so screen readers announce them immediately. Ensure the retry button is keyboard-focusable and has an accessible label. The fallback card should maintain the same heading hierarchy as the original component to preserve document structure for assistive technology.

## Responsive Behavior

Error fallback cards should use Bootstrap's responsive grid to maintain layout integrity across breakpoints. On mobile, stack the error icon, message, and retry button vertically. Ensure the fallback component does not cause horizontal overflow on narrow viewports — use `text-break` for long error messages.
