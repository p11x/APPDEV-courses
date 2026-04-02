---
title: "State Sync Across Multiple Components"
description: "Synchronizing state across navbar, sidebar, modal, and other Bootstrap components using a global state bus"
difficulty: 3
tags: ["state-management", "sync", "event-bus", "global-state", "bootstrap"]
prerequisites: ["04_09_06_Reactive_State_Patterns", "04_09_07_Form_State_Patterns"]
---

## Overview

In Bootstrap applications, multiple components often need to reflect the same state. The navbar shows the notification count that updates when a modal dismisses a notification. The sidebar filters affect the main content grid. The theme toggle updates every component simultaneously. Without a synchronization mechanism, each component manages its own copy of shared state, leading to inconsistencies.

A global state bus — built with custom events, a shared reactive store, or the observer pattern — provides a single source of truth. When any component updates state, all subscribers re-render with the new value, keeping the UI consistent.

## Basic Implementation

```js
// Simple global state bus with custom events
class StateBus {
  constructor() {
    this.state = {};
    this.channel = new BroadcastChannel('app-state');
    this.init();
  }

  init() {
    // Sync across iframes/tabs
    this.channel.addEventListener('message', (e) => {
      Object.assign(this.state, e.data);
      this.notifyAll(e.data);
    });
  }

  set(key, value) {
    const oldValue = this.state[key];
    this.state[key] = value;

    // Notify local listeners
    window.dispatchEvent(new CustomEvent('bus:change', {
      detail: { key, value, oldValue }
    }));

    // Sync across tabs/iframes
    this.channel.postMessage({ [key]: value });
  }

  get(key) {
    return this.state[key];
  }

  notifyAll(changes) {
    Object.entries(changes).forEach(([key, value]) => {
      window.dispatchEvent(new CustomEvent('bus:change', {
        detail: { key, value, oldValue: undefined }
      }));
    });
  }
}

const bus = new StateBus();
```

```html
<!-- Components subscribing to global state -->
<nav class="navbar bg-body-tertiary" id="mainNav">
  <div class="container-fluid">
    <a class="navbar-brand" href="#">App</a>
    <div class="d-flex align-items-center gap-3">
      <!-- Notification badge synced from modal actions -->
      <button class="btn btn-outline-primary position-relative" data-bus="notifications">
        <i class="bi bi-bell"></i>
        <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger d-none"
              data-bus-badge="notificationCount">0</span>
      </button>

      <!-- Theme toggle synced across all components -->
      <button class="btn btn-outline-secondary" data-bus-action="toggle-theme">
        <i class="bi bi-moon-fill"></i>
      </button>

      <!-- Sidebar toggle -->
      <button class="btn btn-outline-secondary d-lg-none" data-bus-action="toggle-sidebar">
        <i class="bi bi-list"></i>
      </button>
    </div>
  </div>
</nav>

<!-- Sidebar with filter state -->
<div class="offcanvas-lg offcanvas-start" id="sidebar" data-bus-state="sidebarOpen">
  <div class="offcanvas-body">
    <h6>Filters</h6>
    <div class="form-check mb-2">
      <input class="form-check-input" type="checkbox" id="filterActive" data-bus-filter="active">
      <label class="form-check-label" for="filterActive">Active</label>
    </div>
    <div class="form-check mb-2">
      <input class="form-check-input" type="checkbox" id="filterArchived" data-bus-filter="archived">
      <label class="form-check-label" for="filterArchived">Archived</label>
    </div>
  </div>
</div>

<!-- Main content reacting to filter state -->
<main class="container py-4">
  <div id="contentGrid" data-bus-listen="filters">
    <!-- Cards render based on active filters -->
  </div>
</main>
```

```js
// Wire up all bus-connected components
window.addEventListener('bus:change', (e) => {
  const { key, value } = e.detail;

  switch (key) {
    case 'notificationCount':
      const badge = document.querySelector('[data-bus-badge="notificationCount"]');
      badge.textContent = value;
      badge.classList.toggle('d-none', value === 0);
      break;

    case 'theme':
      document.documentElement.setAttribute('data-bs-theme', value);
      break;

    case 'filters':
      applyFilters(value);
      break;

    case 'sidebarOpen':
      const sidebar = document.getElementById('sidebar');
      if (window.innerWidth >= 992) {
        sidebar.classList.toggle('show', value);
      }
      break;
  }
});

// Actions from UI
document.querySelector('[data-bus-action="toggle-theme"]').addEventListener('click', () => {
  const current = bus.get('theme') || 'light';
  bus.set('theme', current === 'dark' ? 'light' : 'dark');
});

document.querySelector('[data-bus-action="toggle-sidebar"]').addEventListener('click', () => {
  bus.set('sidebarOpen', !bus.get('sidebarOpen'));
});

// Filter state from sidebar
document.querySelectorAll('[data-bus-filter]').forEach(cb => {
  cb.addEventListener('change', () => {
    const active = Array.from(document.querySelectorAll('[data-bus-filter]:checked'))
      .map(c => c.dataset.busFilter);
    bus.set('filters', active);
  });
});
```

## Advanced Variations

```js
// Component synchronization with lifecycle management
class SyncedComponent {
  constructor(element, bus) {
    this.el = element;
    this.bus = bus;
    this.subscriptions = [];
  }

  listen(key, callback) {
    const handler = (e) => {
      if (e.detail.key === key) callback(e.detail.value);
    };
    window.addEventListener('bus:change', handler);
    this.subscriptions.push(() => window.removeEventListener('bus:change', handler));
  }

  emit(key, value) {
    this.bus.set(key, value);
  }

  destroy() {
    this.subscriptions.forEach(unsub => unsub());
  }
}

// Notification modal synced with navbar badge
class NotificationModal extends SyncedComponent {
  constructor(el, bus) {
    super(el, bus);
    this.init();
  }

  init() {
    // Update badge count from modal actions
    this.el.addEventListener('hidden.bs.modal', () => {
      const remaining = this.el.querySelectorAll('.notification-item').length;
      this.emit('notificationCount', remaining);
    });

    this.el.querySelectorAll('[data-dismiss-notification]').forEach(btn => {
      btn.addEventListener('click', () => {
        btn.closest('.notification-item').remove();
        const remaining = this.el.querySelectorAll('.notification-item').length;
        this.emit('notificationCount', remaining);
      });
    });
  }
}

// Initialize synced components
const notifModal = new NotificationModal(
  document.getElementById('notificationModal'),
  bus
);

// Clean up on SPA navigation
window.addEventListener('beforeunload', () => {
  notifModal.destroy();
});
```

## Best Practices

1. Maintain a single source of truth for each piece of shared state
2. Use custom events (`bus:change`) to broadcast state updates to all components
3. Components should only read from state and dispatch changes — never mutate directly
4. Unsubscribe from state changes when components are destroyed to prevent memory leaks
5. Use the BroadcastChannel API to sync state across tabs and iframes
6. Batch multiple state changes into a single update cycle to reduce re-renders
7. Log state changes in development to trace synchronization issues
8. Keep the global state minimal — only store data shared by multiple components
9. Namespace events and state keys to avoid collisions between feature areas
10. Provide a `destroy` method on synced components that cleans up subscriptions

## Common Pitfalls

1. **Duplicate state** — Storing the same value in both the bus and local component state causes sync drift
2. **Missing cross-tab sync** — Without BroadcastChannel, changes in one tab don't reflect in others
3. **Event listener accumulation** — Adding listeners without removal creates memory leaks in SPA navigation
4. **Circular updates** — Component A updates state, which triggers B, which updates state again, looping infinitely
5. **Too many global keys** — Global state becomes hard to manage with 50+ keys; scope by feature
6. **No state reset** — Navigating between features leaves stale state from the previous feature

## Accessibility Considerations

When synchronized state changes modify visible content, update `aria-live` regions so screen readers announce the change. Notification count changes should be announced via an `aria-live="polite"` region. Theme changes should not cause focus loss. Sidebar state changes on mobile should manage focus correctly — opening the offcanvas should trap focus inside it.

## Responsive Behavior

State synchronization must account for viewport-specific rendering. The sidebar state `sidebarOpen` renders as an offcanvas on mobile and a collapsible panel on desktop. The same state key drives different UI behaviors depending on viewport width. Listen for viewport changes alongside state changes to apply the correct rendering strategy.
