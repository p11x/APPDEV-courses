---
title: "LocalStorage State Persistence"
description: "Persisting UI state like theme, sidebar, and filters in localStorage with Bootstrap components"
difficulty: 2
tags: ["state-management", "localStorage", "persistence", "theme", "bootstrap"]
prerequisites: ["04_09_02_Data_Attribute_State"]
---

## Overview

LocalStorage persists state across browser sessions, enabling user preferences to survive page reloads and tab closures. Common uses in Bootstrap sites include dark/light theme toggle, sidebar collapse state, filter selections, and table sort preferences. The `localStorage` API is synchronous and simple — `setItem` saves state, `getItem` retrieves it.

Combining localStorage with Bootstrap's data attribute state pattern creates components that remember user preferences while maintaining clean separation between persistence logic and UI rendering.

## Basic Implementation

```html
<!-- Theme toggle persisted in localStorage -->
<nav class="navbar navbar-expand-lg" data-bs-theme="light" id="mainNav">
  <div class="container-fluid">
    <a class="navbar-brand" href="#">MyApp</a>
    <button class="btn btn-outline-secondary" id="themeToggle">
      <i class="bi bi-moon-fill" id="themeIcon"></i>
    </button>
  </div>
</nav>

<div class="container py-4">
  <div class="alert alert-info">
    Current theme: <strong id="themeLabel">light</strong>
  </div>
</div>
```

```js
// Theme persistence with localStorage
const themeToggle = document.getElementById('themeToggle');
const themeIcon = document.getElementById('themeIcon');
const themeLabel = document.getElementById('themeLabel');
const nav = document.getElementById('mainNav');

function getTheme() {
  return localStorage.getItem('theme') || 'light';
}

function setTheme(theme) {
  localStorage.setItem('theme', theme);
  applyTheme(theme);
}

function applyTheme(theme) {
  document.documentElement.setAttribute('data-bs-theme', theme);
  nav.setAttribute('data-bs-theme', theme);
  themeIcon.className = theme === 'dark' ? 'bi bi-sun-fill' : 'bi bi-moon-fill';
  themeLabel.textContent = theme;
}

// Apply saved theme on load
applyTheme(getTheme());

themeToggle.addEventListener('click', () => {
  const current = getTheme();
  setTheme(current === 'dark' ? 'light' : 'dark');
});
```

## Advanced Variations

```js
// Generic localStorage state manager with defaults and validation
class LocalStateManager {
  constructor(prefix = 'app') {
    this.prefix = prefix;
  }

  _key(key) {
    return `${this.prefix}:${key}`;
  }

  get(key, defaultValue = null) {
    try {
      const value = localStorage.getItem(this._key(key));
      if (value === null) return defaultValue;
      return JSON.parse(value);
    } catch {
      return defaultValue;
    }
  }

  set(key, value) {
    try {
      localStorage.setItem(this._key(key), JSON.stringify(value));
      window.dispatchEvent(new CustomEvent('localstate', {
        detail: { key, value }
      }));
    } catch (e) {
      console.warn('LocalStorage write failed:', e.message);
    }
  }

  remove(key) {
    localStorage.removeItem(this._key(key));
  }

  clear() {
    Object.keys(localStorage)
      .filter(k => k.startsWith(this.prefix))
      .forEach(k => localStorage.removeItem(k));
  }
}

const state = new LocalStateManager('myapp');

// Sidebar collapse state
const sidebar = document.getElementById('sidebar');
const sidebarToggle = document.getElementById('sidebarToggle');

// Restore sidebar state
if (state.get('sidebarCollapsed', false)) {
  sidebar.classList.add('collapsed');
}

sidebarToggle.addEventListener('click', () => {
  const collapsed = sidebar.classList.toggle('collapsed');
  state.set('sidebarCollapsed', collapsed);
});

// Table sort preference
const sortSelect = document.getElementById('sortSelect');
sortSelect.value = state.get('sortOrder', 'newest');

sortSelect.addEventListener('change', () => {
  state.set('sortOrder', sortSelect.value);
  applySorting(sortSelect.value);
});

// Filter preferences
const filterCheckboxes = document.querySelectorAll('[data-filter]');
const savedFilters = state.get('filters', []);

filterCheckboxes.forEach(cb => {
  cb.checked = savedFilters.includes(cb.dataset.filter);
  cb.addEventListener('change', () => {
    const active = Array.from(filterCheckboxes)
      .filter(c => c.checked)
      .map(c => c.dataset.filter);
    state.set('filters', active);
  });
});
```

```js
// Listen for state changes from other tabs
window.addEventListener('storage', (e) => {
  if (e.key === 'myapp:theme') {
    applyTheme(JSON.parse(e.newValue));
  }
  if (e.key === 'myapp:sidebarCollapsed') {
    const sidebar = document.getElementById('sidebar');
    sidebar.classList.toggle('collapsed', JSON.parse(e.newValue));
  }
});
```

## Best Practices

1. Namespace all keys with a prefix (`app:theme`) to avoid collisions
2. Use `JSON.stringify`/`JSON.parse` for storing non-string values
3. Wrap localStorage access in try/catch — private browsing and quota limits can throw
4. Apply saved state on `DOMContentLoaded` before user sees the page
5. Use the `storage` event to sync state across browser tabs
6. Provide a "Reset preferences" option that clears localStorage state
7. Set reasonable defaults when localStorage values are missing
8. Keep stored values small — localStorage has a ~5MB limit per origin
9. Debounce rapid writes to localStorage (e.g., during slider dragging)
10. Use sessionStorage instead of localStorage for session-only state

## Common Pitfalls

1. **Not handling missing values** — `localStorage.getItem` returns `null` for missing keys; always provide defaults
2. **Synchronous blocking** — Large localStorage reads block the main thread; keep stored data minimal
3. **Quota exceeded errors** — Catch `QuotaExceededError` and degrade gracefully
4. **Stale state from other tabs** — Without the `storage` event listener, tabs show outdated state
5. **Security concerns** — localStorage is accessible to any script on the page; never store sensitive data
6. **JSON parse errors** — Corrupted localStorage values cause `JSON.parse` to throw; always catch

## Accessibility Considerations

Persisted theme state benefits users who need high contrast or dark mode for visual accessibility. Ensure the theme toggle is keyboard-accessible and announces the current state. When restoring sidebar collapse state, ensure focus management adapts — collapsed sidebars should not trap focus on hidden content.

## Responsive Behavior

LocalStorage state is viewport-independent. Sidebar collapse state may need different defaults on mobile (collapsed by default) vs desktop (expanded by default). Store the state separately per breakpoint or check viewport width when applying saved state: expand sidebar only on desktop even if the user previously collapsed it on mobile.
