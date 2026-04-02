---
title: "Loading and Error States for Content"
description: "Skeleton screens, error cards, and retry patterns for failed content loading in Bootstrap"
difficulty: 2
tags: ["error-handling", "loading", "skeleton", "ux", "bootstrap"]
prerequisites: ["02_04_Cards", "04_08_03_Network_Error_UI"]
---

## Overview

Content loaded from APIs can be in one of three states: loading, success, or error. Each state requires distinct UI. Bootstrap's card component, placeholder utility, and spinner components map directly to these states. Skeleton screens (placeholders) set expectations during loading, error cards communicate failure with retry options, and success states display the actual content.

Managing these three states per content block prevents blank screens, confusing spinners, and unhelpful error messages. The pattern applies to dashboards, feeds, card grids, and any data-driven layout.

## Basic Implementation

```html
<!-- Three-state content card -->
<div class="card" id="userCard">
  <!-- Loading state with Bootstrap placeholders -->
  <div class="card-body" id="cardLoading">
    <div class="d-flex align-items-center mb-3">
      <div class="placeholder-glow rounded-circle bg-secondary" style="width: 48px; height: 48px;"></div>
      <div class="ms-3 flex-grow-1">
        <div class="placeholder-glow placeholder col-6 mb-1"></div>
        <div class="placeholder-glow placeholder col-4 small"></div>
      </div>
    </div>
    <div class="placeholder-glow placeholder col-12 mb-2"></div>
    <div class="placeholder-glow placeholder col-10 mb-2"></div>
    <div class="placeholder-glow placeholder col-8"></div>
  </div>

  <!-- Error state -->
  <div class="card-body text-center d-none" id="cardError">
    <i class="bi bi-cloud-slash text-danger mb-2" style="font-size: 2rem;"></i>
    <h6 class="card-title">Failed to load user data</h6>
    <p class="card-text text-muted small">The server did not respond in time.</p>
    <button class="btn btn-outline-primary btn-sm" onclick="loadUserCard()">
      <i class="bi bi-arrow-clockwise me-1"></i>Try Again
    </button>
  </div>

  <!-- Success state -->
  <div class="card-body d-none" id="cardSuccess">
    <div class="d-flex align-items-center mb-3">
      <img src="avatar.jpg" class="rounded-circle" width="48" height="48" alt="User avatar">
      <div class="ms-3">
        <h6 class="mb-0">Jane Doe</h6>
        <small class="text-muted">Software Engineer</small>
      </div>
    </div>
    <p class="card-text">Building accessible web applications with Bootstrap.</p>
  </div>
</div>
```

```js
// Three-state content loader
async function loadUserCard() {
  const loading = document.getElementById('cardLoading');
  const error = document.getElementById('cardError');
  const success = document.getElementById('cardSuccess');

  // Show loading, hide others
  loading.classList.remove('d-none');
  error.classList.add('d-none');
  success.classList.add('d-none');

  try {
    const response = await fetch('/api/user/profile');
    if (!response.ok) throw new Error('Failed to fetch');

    const user = await response.json();

    // Populate success state
    success.querySelector('img').src = user.avatar;
    success.querySelector('h6').textContent = user.name;
    success.querySelector('.text-muted').textContent = user.role;
    success.querySelector('.card-text').textContent = user.bio;

    loading.classList.add('d-none');
    success.classList.remove('d-none');
  } catch (err) {
    loading.classList.add('d-none');
    error.classList.remove('d-none');
  }
}

loadUserCard();
```

## Advanced Variations

```html
<!-- Skeleton grid for multiple cards -->
<div class="row g-4" id="cardGrid">
  <!-- Repeat skeleton cards while loading -->
  <div class="col-md-4 skeleton-card">
    <div class="card h-100">
      <div class="placeholder-glow" style="height: 200px; background: var(--bs-secondary-bg);"></div>
      <div class="card-body">
        <div class="placeholder-glow placeholder col-8 mb-2"></div>
        <div class="placeholder-glow placeholder col-12 mb-1"></div>
        <div class="placeholder-glow placeholder col-10 mb-1"></div>
        <div class="placeholder-glow placeholder col-6"></div>
      </div>
      <div class="card-footer">
        <div class="placeholder-glow placeholder col-4"></div>
      </div>
    </div>
  </div>
  <div class="col-md-4 skeleton-card">
    <div class="card h-100">
      <div class="placeholder-glow" style="height: 200px; background: var(--bs-secondary-bg);"></div>
      <div class="card-body">
        <div class="placeholder-glow placeholder col-7 mb-2"></div>
        <div class="placeholder-glow placeholder col-12 mb-1"></div>
        <div class="placeholder-glow placeholder col-9"></div>
      </div>
      <div class="card-footer">
        <div class="placeholder-glow placeholder col-5"></div>
      </div>
    </div>
  </div>
  <div class="col-md-4 skeleton-card">
    <div class="card h-100">
      <div class="placeholder-glow" style="height: 200px; background: var(--bs-secondary-bg);"></div>
      <div class="card-body">
        <div class="placeholder-glow placeholder col-6 mb-2"></div>
        <div class="placeholder-glow placeholder col-12 mb-1"></div>
        <div class="placeholder-glow placeholder col-11 mb-1"></div>
        <div class="placeholder-glow placeholder col-7"></div>
      </div>
      <div class="card-footer">
        <div class="placeholder-glow placeholder col-3"></div>
      </div>
    </div>
  </div>
</div>
```

```js
// Generic content state manager
class ContentState {
  constructor(container) {
    this.container = container;
    this.states = {
      loading: container.querySelector('[data-state="loading"]'),
      error: container.querySelector('[data-state="error"]'),
      success: container.querySelector('[data-state="success"]')
    };
  }

  show(state) {
    Object.entries(this.states).forEach(([key, el]) => {
      if (el) el.classList.toggle('d-none', key !== state);
    });
  }

  showError(message, retryCallback) {
    const errorEl = this.states.error;
    if (errorEl) {
      errorEl.querySelector('[data-error-message]').textContent = message;
      const retryBtn = errorEl.querySelector('[data-retry]');
      if (retryBtn && retryCallback) {
        retryBtn.onclick = retryCallback;
      }
    }
    this.show('error');
  }
}

// Usage
const card = new ContentState(document.getElementById('userCard'));
card.show('loading');

fetch('/api/user')
  .then(r => r.json())
  .then(data => { /* render and show success */ })
  .catch(err => card.showError(err.message, () => loadUserCard()));
```

## Best Practices

1. Use Bootstrap's `placeholder-glow` or `placeholder-wave` classes for skeleton screens
2. Match skeleton dimensions to actual content to prevent layout shifts on load complete
3. Show skeleton screens immediately on page load, not after a delay
4. Provide specific error messages that explain what went wrong
5. Always include a retry button on error states for recoverable failures
6. Use `aria-busy="true"` on loading containers for screen reader announcements
7. Maintain consistent card dimensions across loading, error, and success states
8. Timeout loading states — show an error if loading exceeds 15 seconds
9. Cache successful responses to show stale data while refreshing
10. Test each state independently — loading, error, and success should all render correctly in isolation

## Common Pitfalls

1. **Skeleton dimensions differ from real content** — Layout shifts when loading completes if skeleton cards are a different height than actual cards
2. **Error state missing retry** — Users must reload the entire page to recover from a single failed card
3. **No loading timeout** — Infinite spinners frustrate users; always fail to an error state after a threshold
4. **Blank screen during loading** — Not showing a skeleton makes the page feel broken; always provide visual feedback
5. **Error message is too generic** — "Error" with no context gives users no path to resolution
6. **Not handling partial failures** — If 3 of 5 cards load, show the 3 successful cards with error states on the 2 that failed

## Accessibility Considerations

Loading states need `aria-busy="true"` and `aria-live="polite"` so screen readers announce content updates. Skeleton placeholders should have `role="status"` with a `visually-hidden` label like "Loading content". Error cards must be focusable and contain actionable retry buttons reachable via keyboard.

## Responsive Behavior

Skeleton cards should use the same grid classes as real content cards (`col-md-4`, `col-lg-3`) so they adapt to viewport width. Error cards should not collapse to unreadable sizes on mobile — use `col-12` on narrow viewports for error states that need more vertical space for messages and retry buttons.
