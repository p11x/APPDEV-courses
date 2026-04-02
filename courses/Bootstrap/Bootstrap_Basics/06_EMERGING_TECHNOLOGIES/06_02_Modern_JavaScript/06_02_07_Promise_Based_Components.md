---
title: Promise-Based Bootstrap Components
category: Emerging Technologies
difficulty: 3
time: 30 min
tags: bootstrap5, promises, async, deferred-rendering, modal-loading
---

## Overview

Promise-based component patterns handle asynchronous initialization, deferred rendering, and async data loading within Bootstrap components. Modals that load content before displaying, tabs that fetch data on activation, and forms that submit with async validation all benefit from Promise-based patterns. These patterns ensure components are fully ready before user interaction, preventing partial renders and race conditions.

## Basic Implementation

Promise-based modal that loads content before displaying.

```js
// Promise-based modal loader
class AsyncModal {
  constructor(modalSelector) {
    this.element = document.querySelector(modalSelector);
    this.body = this.element.querySelector('.modal-body');
    this.instance = bootstrap.Modal.getInstance(this.element)
      || new bootstrap.Modal(this.element);
  }

  async show(contentPromise) {
    // Show modal with loading state
    this.body.innerHTML = `
      <div class="text-center py-4">
        <div class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Loading content...</span>
        </div>
        <p class="text-body-secondary mt-2 mb-0">Loading...</p>
      </div>
    `;
    this.instance.show();

    try {
      const content = await contentPromise;
      this.body.innerHTML = content;
      this.element.dispatchEvent(new CustomEvent('modal:loaded'));
    } catch (error) {
      this.body.innerHTML = `
        <div class="alert alert-danger mb-0" role="alert">
          <i class="bi bi-exclamation-triangle me-2"></i>
          Failed to load content: ${error.message}
          <button class="btn btn-sm btn-outline-danger ms-2" data-bs-dismiss="modal">Close</button>
        </div>
      `;
    }
  }

  hide() {
    return new Promise(resolve => {
      this.element.addEventListener('hidden.bs.modal', () => resolve(), { once: true });
      this.instance.hide();
    });
  }
}

// Usage
const modal = new AsyncModal('#contentModal');

document.getElementById('loadUserBtn').addEventListener('click', async () => {
  const userData = fetch('/api/users/1').then(r => r.json()).then(user => `
    <div class="row">
      <div class="col-auto">
        <img src="${user.avatar}" class="rounded-circle" width="80" height="80" alt="${user.name}">
      </div>
      <div class="col">
        <h5>${user.name}</h5>
        <p class="text-body-secondary mb-1">${user.email}</p>
        <span class="badge bg-${user.active ? 'success' : 'secondary'}">${user.active ? 'Active' : 'Inactive'}</span>
      </div>
    </div>
  `);
  await modal.show(userData);
});
```

```html
<button class="btn btn-primary" id="loadUserBtn">View User Profile</button>
<div class="modal fade" id="contentModal" tabindex="-1" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">User Profile</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body"></div>
    </div>
  </div>
</div>
```

## Advanced Variations

Async tab loading with caching and promise-based form submission with validation.

```js
// Async tab loader with caching
class AsyncTabs {
  constructor(navSelector, contentSelector) {
    this.nav = document.querySelector(navSelector);
    this.content = document.querySelector(contentSelector);
    this.cache = new Map();
    this.loading = false;

    this.nav.addEventListener('click', (e) => this.handleClick(e));
  }

  async handleClick(e) {
    const tab = e.target.closest('[data-bs-toggle="tab"]');
    if (!tab || this.loading) return;

    e.preventDefault();
    const url = tab.dataset.url;
    const cacheKey = url;

    // Remove active from all tabs
    this.nav.querySelectorAll('.nav-link').forEach(l => l.classList.remove('active'));
    tab.classList.add('active');

    if (this.cache.has(cacheKey)) {
      this.content.innerHTML = this.cache.get(cacheKey);
      return;
    }

    this.loading = true;
    this.content.innerHTML = `
      <div class="text-center py-5">
        <div class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
      </div>
    `;

    try {
      const response = await fetch(url);
      const html = await response.text();
      this.cache.set(cacheKey, html);
      this.content.innerHTML = html;
    } catch (error) {
      this.content.innerHTML = `
        <div class="alert alert-danger" role="alert">Failed to load tab content.</div>
      `;
    } finally {
      this.loading = false;
    }
  }

  clearCache() {
    this.cache.clear();
  }
}
```

```js
// Promise-based form with async validation
class AsyncForm {
  constructor(formSelector) {
    this.form = document.querySelector(formSelector);
    this.submitBtn = this.form.querySelector('[type="submit"]');
    this.form.addEventListener('submit', (e) => this.handleSubmit(e));
  }

  async handleSubmit(e) {
    e.preventDefault();

    // Collect form data
    const formData = new FormData(this.form);
    const data = Object.fromEntries(formData.entries());

    // Show loading state
    this.setLoading(true);
    this.clearValidation();

    try {
      // Async validation
      const validation = await this.validate(data);
      if (!validation.valid) {
        this.showErrors(validation.errors);
        return;
      }

      // Submit
      const response = await fetch(this.form.action, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Submission failed');
      }

      const result = await response.json();
      this.showSuccess(result);

    } catch (error) {
      this.showError(error.message);
    } finally {
      this.setLoading(false);
    }
  }

  setLoading(isLoading) {
    this.submitBtn.disabled = isLoading;
    this.submitBtn.innerHTML = isLoading
      ? '<span class="spinner-border spinner-border-sm me-2" role="status"></span>Submitting...'
      : 'Submit';
  }

  showErrors(errors) {
    for (const [field, message] of Object.entries(errors)) {
      const input = this.form.querySelector(`[name="${field}"]`);
      if (input) {
        input.classList.add('is-invalid');
        input.parentElement.querySelector('.invalid-feedback')?.remove();
        const feedback = document.createElement('div');
        feedback.className = 'invalid-feedback';
        feedback.textContent = message;
        input.parentElement.appendChild(feedback);
      }
    }
  }

  clearValidation() {
    this.form.querySelectorAll('.is-invalid').forEach(el => el.classList.remove('is-invalid'));
    this.form.querySelectorAll('.invalid-feedback').forEach(el => el.remove());
    this.form.querySelectorAll('.alert').forEach(el => el.remove());
  }
}
```

```html
<ul class="nav nav-tabs" id="asyncTabs">
  <li class="nav-item">
    <a class="nav-link active" data-bs-toggle="tab" data-url="/api/tab-overview">Overview</a>
  </li>
  <li class="nav-item">
    <a class="nav-link" data-bs-toggle="tab" data-url="/api/tab-settings">Settings</a>
  </li>
</ul>
<div id="tabContent" class="tab-content border border-top-0 p-4"></div>
```

## Best Practices

1. Show loading spinners immediately when async operations begin
2. Use `AbortController` to cancel promises when users navigate away
3. Cache resolved promise results to avoid redundant async calls
4. Provide clear error messages with retry options in failed states
5. Disable interactive elements during async operations to prevent double-submission
6. Use `Promise.allSettled` for parallel async operations that should all complete
7. Set reasonable timeouts for async operations using `Promise.race`
8. Clean up async state when components are removed from the DOM
9. Use async/await over raw Promise chains for readability
10. Log async failures for debugging while showing user-friendly messages

## Common Pitfalls

1. **Unresolved promises** - Modals that never resolve leave the user stuck. Always set timeouts.
2. **State after unmount** - Async callbacks that update removed DOM elements cause errors. Check element existence.
3. **Double submission** - Users clicking submit multiple times create duplicate requests. Disable buttons during submission.
4. **Cache staleness** - Cached async data becomes outdated. Implement cache invalidation strategies.
5. **Unhandled rejections** - Missing `.catch()` on promises causes silent failures. Always handle rejections.

## Accessibility Considerations

Async components must announce state changes to screen readers. Use `aria-busy="true"` during loading, `role="alert"` for error messages, and manage focus when async content appears. Promise-resolved modal content should move focus to the first interactive element. Form validation errors should be announced and associated with their fields via `aria-describedby`.

## Responsive Behavior

Async loading states should match the responsive layout of the final content. Skeleton loaders should use the same grid structure as the loaded content to prevent layout shift. Promise-based modals should respect Bootstrap's responsive modal sizes (`modal-fullscreen-sm-down`) and ensure loaded content is scrollable within the modal on small viewports.
