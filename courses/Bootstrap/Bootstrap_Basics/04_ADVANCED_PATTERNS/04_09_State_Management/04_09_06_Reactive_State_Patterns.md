---
title: "Reactive State Patterns"
description: "Proxy-based reactivity, custom event-driven state, and observer patterns for Bootstrap components"
difficulty: 3
tags: ["state-management", "reactive", "proxy", "observer", "events", "bootstrap"]
prerequisites: ["04_09_02_Data_Attribute_State", "04_09_05_URL_State_Management"]
---

## Overview

Reactive state automatically updates the UI when data changes. JavaScript's `Proxy` object intercepts property access and assignment, enabling a reactive system that re-renders Bootstrap components whenever state mutations occur. Combined with custom events and the observer pattern, this creates a lightweight state management layer without external libraries.

This pattern suits dashboards, real-time feeds, and interactive forms where multiple components must react to shared state changes — a notification count updates the navbar badge, a filter change re-renders the card grid, and a theme switch updates all themed elements simultaneously.

## Basic Implementation

```js
// Proxy-based reactive state
function createState(initial, onChange) {
  const handler = {
    set(target, property, value) {
      const oldValue = target[property];
      target[property] = value;

      if (oldValue !== value) {
        onChange(property, value, oldValue);
      }
      return true;
    },

    get(target, property) {
      const value = target[property];
      if (typeof value === 'object' && value !== null) {
        return new Proxy(value, handler);
      }
      return value;
    }
  };

  return new Proxy({ ...initial }, handler);
}

// Application state
const state = createState({
  theme: 'light',
  notificationCount: 0,
  filters: { status: 'all', search: '' },
  sidebarOpen: true
}, (property, value, oldValue) => {
  console.log(`State changed: ${property}`, oldValue, '→', value);

  // Dispatch event for components to react
  document.dispatchEvent(new CustomEvent('statechange', {
    detail: { property, value, oldValue }
  }));
});
```

```html
<!-- Components bound to reactive state -->
<nav class="navbar navbar-expand-lg bg-body-tertiary" id="mainNav">
  <div class="container-fluid">
    <a class="navbar-brand" href="#">Dashboard</a>
    <div class="d-flex align-items-center gap-3">
      <button class="btn btn-outline-primary position-relative" id="notifBtn">
        <i class="bi bi-bell"></i>
        <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger d-none"
              id="notifBadge">0</span>
      </button>
      <button class="btn btn-outline-secondary" id="themeBtn">
        <i class="bi bi-moon-fill"></i>
      </button>
    </div>
  </div>
</nav>
```

```js
// UI bindings
document.addEventListener('statechange', (e) => {
  const { property, value } = e.detail;

  switch (property) {
    case 'notificationCount':
      const badge = document.getElementById('notifBadge');
      badge.textContent = value;
      badge.classList.toggle('d-none', value === 0);
      break;

    case 'theme':
      document.documentElement.setAttribute('data-bs-theme', value);
      document.getElementById('themeBtn').querySelector('i').className =
        value === 'dark' ? 'bi bi-sun-fill' : 'bi bi-moon-fill';
      break;

    case 'sidebarOpen':
      document.getElementById('sidebar')?.classList.toggle('collapsed', !value);
      break;
  }
});

// Trigger state changes
document.getElementById('themeBtn').addEventListener('click', () => {
  state.theme = state.theme === 'dark' ? 'light' : 'dark';
});

// Simulate notification
setInterval(() => {
  state.notificationCount = state.notificationCount + 1;
}, 10000);
```

## Advanced Variations

```js
// Observer pattern for component-level reactivity
class EventBus {
  constructor() {
    this.listeners = new Map();
  }

  on(event, callback) {
    if (!this.listeners.has(event)) this.listeners.set(event, new Set());
    this.listeners.get(event).add(callback);
    return () => this.off(event, callback);
  }

  off(event, callback) {
    this.listeners.get(event)?.delete(callback);
  }

  emit(event, data) {
    this.listeners.get(event)?.forEach(cb => cb(data));
  }
}

const bus = new EventBus();

// Reactive store with computed properties
class ReactiveStore {
  constructor(initialState) {
    this.state = { ...initialState };
    this.computed = {};
    this.subscribers = new Set();

    return new Proxy(this, {
      get(target, prop) {
        if (prop in target.computed) {
          return target.computed[prop](target.state);
        }
        if (prop in target.state) {
          return target.state[prop];
        }
        return target[prop];
      },

      set(target, prop, value) {
        if (prop in target.state) {
          const old = target.state[prop];
          target.state[prop] = value;
          if (old !== value) {
            target.subscribers.forEach(cb => cb(prop, value, old));
            bus.emit(`state:${prop}`, value);
          }
          return true;
        }
        return false;
      }
    });
  }

  subscribe(callback) {
    this.subscribers.add(callback);
    return () => this.subscribers.delete(callback);
  }

  addComputed(name, fn) {
    this.computed[name] = fn;
  }
}

// Usage
const store = new ReactiveStore({
  items: [],
  currentPage: 1,
  perPage: 10,
  filter: ''
});

store.addComputed('filteredItems', (state) => {
  return state.items.filter(item =>
    item.name.toLowerCase().includes(state.filter.toLowerCase())
  );
});

store.addComputed('totalPages', (state) => {
  const filtered = state.items.filter(item =>
    item.name.toLowerCase().includes(state.filter.toLowerCase())
  );
  return Math.ceil(filtered.length / state.perPage);
});

store.subscribe((prop, value) => {
  if (prop === 'items' || prop === 'filter') {
    renderCardGrid(store.filteredItems);
    renderPagination(store.currentPage, store.totalPages);
  }
  if (prop === 'currentPage') {
    renderPagination(value, store.totalPages);
  }
});

// Wire to Bootstrap components
bus.on('state:items', () => {
  renderCardGrid(store.filteredItems);
});
```

## Best Practices

1. Use `Proxy` for transparent property interception without explicit getter/setter calls
2. Dispatch custom events on state change so multiple components can react independently
3. Keep the reactive state object flat to simplify change detection
4. Use an event bus for cross-component communication unrelated to shared state
5. Unsubscribe from events when components are destroyed to prevent memory leaks
6. Batch state updates to avoid multiple re-renders from rapid sequential changes
7. Use computed/derived values that recalculate automatically on dependency changes
8. Log state changes in development for debugging complex interactions
9. Keep the state tree small — only store data that drives UI rendering
10. Separate state management from DOM manipulation for testability

## Common Pitfalls

1. **Proxy performance on large objects** — Deep proxying of large arrays or objects adds overhead; keep state shallow
2. **Infinite update loops** — State change triggers re-render which triggers state change; use dirty checks
3. **Missing cleanup** — Event listeners and subscriptions accumulate if not removed on component teardown
4. **Over-engineering simple state** — A single toggle does not need a reactive store; use it for shared/complex state
5. **Proxy traps breaking expectations** — `JSON.stringify` on a Proxy may not serialize correctly without a `get` trap
6. **No batched updates** — Setting 10 state properties in sequence triggers 10 re-renders; batch into one update

## Accessibility Considerations

Reactive state changes that modify visible content must trigger appropriate ARIA announcements. When a filtered list re-renders, update `aria-live` regions with the new count. Ensure focus is not lost when state changes remove the currently focused element from the DOM.

## Responsive Behavior

Reactive state drives UI updates regardless of viewport. However, the same state may need different rendering on mobile vs desktop. Subscribe to viewport changes alongside state changes to ensure reactive re-renders produce responsive layouts. A sidebar state change renders as an offcanvas on mobile and a collapsible panel on desktop.
