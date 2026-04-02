---
title: "AI Personalization UI"
slug: "ai-personalization-ui"
difficulty: 3
tags: ["bootstrap", "ai", "personalization", "adaptive", "ux"]
prerequisites:
  - "06_01_09_AI_Form_Autofill"
  - "06_01_13_AI_Dashboard_Insights"
related:
  - "06_01_11_AI_Content_Summary"
  - "06_01_12_AI_Image_Alt_Text"
duration: "35 minutes"
---

# AI Personalization UI

## Overview

AI-personalized layouts adapt the user interface based on learned behavior, preferences, and contextual signals. Bootstrap provides flexible grid and component primitives, while AI determines which components to show, how to order them, and what content to highlight. This creates experiences that feel tailored to each user without requiring manual configuration. Applications range from personalized dashboards and content feeds to adaptive navigation and dynamic feature visibility.

## Basic Implementation

A personalized card layout where AI determines card order and visibility based on user behavior.

```html
<div class="container mt-4">
  <div class="d-flex justify-content-between align-items-center mb-3">
    <h4>Your Personalized View</h4>
    <div class="dropdown">
      <button class="btn btn-sm btn-outline-secondary dropdown-toggle" data-bs-toggle="dropdown">
        <i class="bi bi-gear"></i> Layout
      </button>
      <ul class="dropdown-menu">
        <li><a class="dropdown-item" href="#" data-layout="ai">AI Optimized</a></li>
        <li><a class="dropdown-item" href="#" data-layout="grid">Grid</a></li>
        <li><a class="dropdown-item" href="#" data-layout="list">List</a></li>
      </ul>
    </div>
  </div>
  <div class="row g-4" id="personalizedGrid">
    <div class="col-12 text-center py-5">
      <div class="spinner-border text-primary"></div>
      <p class="text-muted mt-2">Personalizing your experience...</p>
    </div>
  </div>
</div>

<script>
document.addEventListener('DOMContentLoaded', async () => {
  const userId = getCurrentUserId();
  const res = await fetch('/api/ai/personalize', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      userId,
      page: 'dashboard',
      viewport: { width: window.innerWidth, height: window.innerHeight }
    })
  });
  const layout = await res.json();

  const grid = document.getElementById('personalizedGrid');
  grid.innerHTML = layout.components.map(comp => `
    <div class="col-${comp.size}" data-component="${comp.id}" data-priority="${comp.priority}">
      <div class="card h-100">
        <div class="card-header d-flex justify-content-between">
          <h6 class="mb-0">${comp.title}</h6>
          <span class="badge bg-light text-muted">Priority ${comp.priority}</span>
        </div>
        <div class="card-body">${comp.content}</div>
      </div>
    </div>
  `).join('');
});
</script>
```

## Advanced Variations

### Adaptive Navigation

Navigation items reorder and highlight based on user frequency patterns.

```html
<nav class="navbar navbar-expand-lg navbar-light bg-light">
  <div class="container-fluid">
    <a class="navbar-brand" href="#">App</a>
    <button class="navbar-toggler" data-bs-toggle="collapse" data-bs-target="#navContent">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navContent">
      <ul class="navbar-nav" id="adaptiveNav">
        <!-- Populated by AI -->
      </ul>
    </div>
  </div>
</nav>

<script>
async function loadPersonalizedNav() {
  const res = await fetch('/api/ai/nav-personalize', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ userId: getCurrentUserId() })
  });
  const navItems = await res.json();

  const nav = document.getElementById('adaptiveNav');
  nav.innerHTML = navItems.map(item => `
    <li class="nav-item ${item.isFrequent ? 'order-first' : ''}">
      <a class="nav-link ${item.active ? 'active' : ''}" href="${item.href}">
        <i class="bi bi-${item.icon}"></i> ${item.label}
        ${item.isFrequent ? '<span class="badge bg-primary rounded-pill ms-1">Frequent</span>' : ''}
      </a>
    </li>
  `).join('');
}
</script>
```

### Content Feed with Relevance Scoring

A personalized content feed that surfaces items based on AI-calculated relevance.

```html
<div class="list-group" id="personalizedFeed">
  <div class="list-group-item text-center py-4">
    <div class="spinner-border"></div>
  </div>
</div>

<script>
async function loadFeed() {
  const res = await fetch('/api/ai/content-feed', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      userId: getCurrentUserId(),
      limit: 20,
      exclude: getReadItems()
    })
  });
  const feed = await res.json();

  document.getElementById('personalizedFeed').innerHTML = feed.items.map(item => `
    <div class="list-group-item" data-relevance="${item.relevanceScore}">
      <div class="d-flex justify-content-between align-items-start">
        <div class="flex-grow-1">
          <h6 class="mb-1">${item.title}</h6>
          <p class="mb-1 text-muted small">${item.excerpt}</p>
          <div>
            ${item.tags.map(t => `<span class="badge bg-secondary me-1">${t}</span>`).join('')}
          </div>
        </div>
        <div class="text-end ms-3">
          <div class="progress mb-1" style="width:60px;height:6px;">
            <div class="progress-bar bg-${item.relevanceScore > 80 ? 'success' : 'info'}"
                 style="width:${item.relevanceScore}%"></div>
          </div>
          <small class="text-muted">${item.relevanceScore}% match</small>
        </div>
      </div>
    </div>
  `).join('');
}
</script>
```

### Theme and Density Adaptation

AI adjusts UI density and color scheme based on usage patterns and time of day.

```html
<div class="card">
  <div class="card-body">
    <h6>Display Preferences</h6>
    <div class="btn-group" role="group" aria-label="Density">
      <input type="radio" class="btn-check" name="density" id="compact" value="compact">
      <label class="btn btn-outline-secondary" for="compact">Compact</label>
      <input type="radio" class="btn-check" name="density" id="comfortable" value="comfortable" checked>
      <label class="btn btn-outline-secondary" for="comfortable">Comfortable</label>
      <input type="radio" class="btn-check" name="density" id="spacious" value="spacious">
      <label class="btn btn-outline-secondary" for="spacious">Spacious</label>
    </div>
    <div class="mt-2">
      <small class="text-muted">
        <i class="bi bi-stars"></i> AI recommends: <strong id="aiDensity">comfortable</strong>
        based on your usage pattern
      </small>
    </div>
  </div>
</div>

<script>
async function applyAIPreferences() {
  const res = await fetch('/api/ai/ui-preferences', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ userId: getCurrentUserId() })
  });
  const prefs = await res.json();

  document.getElementById('aiDensity').textContent = prefs.recommendedDensity;
  document.body.classList.remove('density-compact', 'density-comfortable', 'density-spacious');
  document.body.classList.add(`density-${prefs.appliedDensity}`);

  if (prefs.theme === 'dark' || (prefs.theme === 'auto' && isNightTime())) {
    document.documentElement.setAttribute('data-bs-theme', 'dark');
  }
}

document.querySelectorAll('input[name="density"]').forEach(radio => {
  radio.addEventListener('change', (e) => {
    document.body.classList.remove('density-compact', 'density-comfortable', 'density-spacious');
    document.body.classList.add(`density-${e.target.value}`);
    trackPreference('density', e.target.value);
  });
});
</script>
```

## Best Practices

1. Always provide a manual override for AI-determined layouts
2. Show a brief loading state while personalization data loads
3. Use skeleton placeholders matching the expected layout shape during loading
4. Respect user privacy by making personalization opt-in with clear controls
5. Cache personalization results to avoid layout shifts on every page load
6. Provide a "Reset to default" option alongside personalized layouts
7. Track personalization effectiveness with engagement metrics
8. Use progressive enhancement so pages work without JavaScript
9. Avoid dramatic layout changes that disorient users between sessions
10. Implement gradual adaptation rather than sudden layout shifts
11. Log personalization decisions for debugging and auditing
12. Allow users to pin components to fixed positions regardless of AI suggestions
13. Use `data-bs-theme` for AI-driven theme switching to leverage Bootstrap's built-in dark mode
14. Test personalization with diverse user profiles to avoid bias

## Common Pitfalls

1. **Layout thrashing**: Changing layout too frequently causes visual instability
2. **No user control**: Users feel trapped in an AI layout they cannot modify
3. **Privacy concerns**: Collecting behavioral data without transparent disclosure
4. **Filter bubbles**: Personalization that narrows content diversity over time
5. **Cold start problem**: New users receiving poor personalization due to lack of data
6. **Ignoring accessibility**: Personalized layouts that break screen reader navigation
7. **Performance overhead**: Heavy personalization logic slowing initial page render

## Accessibility Considerations

Ensure personalized layouts maintain logical tab order regardless of visual arrangement. Use `aria-label` on dynamically reordered navigation to indicate the personalization reason. Provide a visible toggle to disable personalization for users who prefer consistent layouts. Maintain heading hierarchy in personalized content areas. Announce layout changes with `aria-live="assertive"` only when significant, use `"polite"` otherwise. Ensure personalized theme changes do not break color contrast ratios.

```html
<div id="personalizedGrid" aria-label="Personalized content layout" role="region">
  <button class="btn btn-sm btn-link" id="disablePersonalization" aria-label="Disable personalized layout">
    Use default layout
  </button>
</div>
<div aria-live="polite" class="visually-hidden" id="layoutAnnounce"></div>
```

## Responsive Behavior

On mobile, simplify personalized layouts to a single-column stack ordered by priority. Use `col-12 col-md-6 col-lg-4` for flexible grid sizing across breakpoints. Collapse personalized navigation items beyond the top 5 into a "More" dropdown on small screens. Ensure personalized theme and density settings persist across device sizes. On tablets, use 2-column layouts with AI-determined column assignments. Use `d-none d-lg-block` for secondary personalized widgets that should only appear on larger screens.
