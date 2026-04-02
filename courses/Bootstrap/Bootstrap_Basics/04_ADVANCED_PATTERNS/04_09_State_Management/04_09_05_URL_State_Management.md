---
title: "URL State Management"
description: "Using query parameters, hash-based state, and the History API with Bootstrap components"
difficulty: 3
tags: ["state-management", "url", "history-api", "routing", "bootstrap"]
prerequisites: ["04_09_02_Data_Attribute_State", "04_09_03_LocalStorage_State"]
---

## Overview

URL state encodes application state directly in the page URL using query parameters (`?filter=active&sort=name`) or hash fragments (`#tab-security`). This makes state shareable, bookmarkable, and restorable via browser navigation. The History API (`pushState`, `replaceState`) updates the URL without page reloads, enabling SPA-like behavior in vanilla JavaScript Bootstrap applications.

URL state is ideal for filters, search queries, pagination, tab selections, and any state that should survive sharing or bookmarking.

## Basic Implementation

```html
<!-- URL-driven tab state -->
<ul class="nav nav-pills mb-3" id="urlTabs" role="tablist">
  <li class="nav-item">
    <a class="nav-link active" href="#tab-overview" data-url-tab>Overview</a>
  </li>
  <li class="nav-item">
    <a class="nav-link" href="#tab-details" data-url-tab>Details</a>
  </li>
  <li class="nav-item">
    <a class="nav-link" href="#tab-settings" data-url-tab>Settings</a>
  </li>
</ul>

<div class="tab-content">
  <div class="tab-pane fade show active" id="tab-overview">Overview content</div>
  <div class="tab-pane fade" id="tab-details">Details content</div>
  <div class="tab-pane fade" id="tab-settings">Settings content</div>
</div>
```

```js
// Hash-based tab state management
class URLTabManager {
  constructor(container) {
    this.container = container;
    this.tabs = container.querySelectorAll('[data-url-tab]');
    this.init();
  }

  init() {
    this.tabs.forEach(tab => {
      tab.addEventListener('click', (e) => {
        e.preventDefault();
        const hash = tab.getAttribute('href');
        history.pushState(null, '', hash);
        this.activateTab(hash);
      });
    });

    window.addEventListener('hashchange', () => {
      this.activateTab(location.hash || this.tabs[0].getAttribute('href'));
    });

    // Restore from URL on load
    if (location.hash) {
      this.activateTab(location.hash);
    }
  }

  activateTab(hash) {
    this.tabs.forEach(t => t.classList.remove('active'));
    document.querySelectorAll('.tab-pane').forEach(p => {
      p.classList.remove('show', 'active');
    });

    const activeTab = this.container.querySelector(`[href="${hash}"]`);
    const activePane = document.querySelector(hash);

    if (activeTab && activePane) {
      activeTab.classList.add('active');
      activePane.classList.add('show', 'active');
    }
  }
}

new URLTabManager(document.getElementById('urlTabs'));
```

## Advanced Variations

```js
// Query parameter state manager for filters
class URLStateManager {
  constructor() {
    this.params = new URLSearchParams(location.search);
  }

  get(key) {
    return this.params.get(key);
  }

  getAll(key) {
    return this.params.getAll(key);
  }

  set(key, value) {
    if (value === null || value === '') {
      this.params.delete(key);
    } else {
      this.params.set(key, value);
    }
    this.updateURL();
  }

  toggle(key, value) {
    const values = this.params.getAll(key);
    if (values.includes(value)) {
      this.params.delete(key);
      values.filter(v => v !== value).forEach(v => this.params.append(key, v));
    } else {
      this.params.append(key, value);
    }
    this.updateURL();
  }

  updateURL() {
    const url = `${location.pathname}?${this.params.toString()}`;
    history.pushState({}, '', url);
    window.dispatchEvent(new CustomEvent('urlstatechange', {
      detail: Object.fromEntries(this.params)
    }));
  }

  clear() {
    history.pushState({}, '', location.pathname);
    window.dispatchEvent(new CustomEvent('urlstatechange', { detail: {} }));
  }
}

const urlState = new URLStateManager();

// Wire up filter checkboxes to URL state
document.querySelectorAll('[data-filter-param]').forEach(input => {
  const param = input.dataset.filterParam;
  const value = input.dataset.filterValue;

  // Restore state from URL
  const urlValues = urlState.getAll(param);
  if (urlValues.includes(value)) {
    input.checked = true;
  }

  input.addEventListener('change', () => {
    urlState.toggle(param, value);
  });
});

// Wire up sort dropdown to URL state
const sortSelect = document.getElementById('sortSelect');
sortSelect.value = urlState.get('sort') || 'newest';
sortSelect.addEventListener('change', () => {
  urlState.set('sort', sortSelect.value);
});

// Wire up search input to URL state
const searchInput = document.getElementById('searchInput');
searchInput.value = urlState.get('q') || '';
let searchTimeout;
searchInput.addEventListener('input', () => {
  clearTimeout(searchTimeout);
  searchTimeout = setTimeout(() => {
    urlState.set('q', searchInput.value);
  }, 300);
});

// React to URL state changes (including browser back/forward)
window.addEventListener('urlstatechange', (e) => {
  applyFilters(e.detail);
});

// Handle browser back/forward
window.addEventListener('popstate', () => {
  const params = new URLSearchParams(location.search);
  applyFilters(Object.fromEntries(params));
});
```

## Best Practices

1. Use `pushState` for user-initiated navigation and `replaceState` for initial state setup
2. Use query parameters for filters, search, and pagination state
3. Use hash fragments for tab and section navigation within a page
4. Listen for `popstate` to handle browser back/forward navigation
5. Debounce URL updates during rapid input changes (search-as-you-type)
6. Use `URLSearchParams` for clean query parameter manipulation
7. Default to `replaceState` on page load to avoid adding a blank entry to history
8. Encode special characters in URL values to prevent parsing errors
9. Keep URLs readable — use short, meaningful parameter names
10. Provide a "Reset filters" link that clears all query parameters

## Common Pitfalls

1. **Not listening for `popstate`** — Back/forward buttons don't update the UI without a popstate handler
2. **URL too long** — Excessive query parameters hit browser URL length limits (~2000 characters)
3. **Hash vs query confusion** — Hash is for same-page navigation; query is for filter/search state
4. **Duplicate history entries** — Using `pushState` for every keystroke floods browser history
5. **Not encoding values** — Special characters like `&` and `=` in values break parameter parsing
6. **Missing initial state setup** — URL has parameters on load but UI doesn't reflect them without explicit initialization

## Accessibility Considerations

URL state enables accessible deep-linking — sharing a link to a specific tab ensures all users start at the same content. When hash navigation activates a tab, move focus to the tab panel for screen reader announcement. Ensure URL-driven state changes announce updates via `aria-live` regions.

## Responsive Behavior

URL state is viewport-independent but may drive different layouts. A filter panel visible on desktop may be in an offcanvas on mobile. The same URL parameters should activate the correct state in both viewport layouts. Ensure hash-based navigation scrolls to the correct position accounting for fixed navbar height on mobile.
