---
title: "Session State Management"
description: "Session-based state, form state preservation, and tab state persistence in Bootstrap applications"
difficulty: 2
tags: ["state-management", "session", "forms", "tabs", "bootstrap"]
prerequisites: ["04_09_03_LocalStorage_State"]
---

## Overview

Session state persists data for the duration of a browser session — until the tab or browser closes. Unlike localStorage, session data is automatically cleared when the session ends, making it ideal for temporary state: multi-step form progress, tab selections, draft content, and navigation history. The `sessionStorage` API mirrors localStorage but is scoped to the current tab.

Bootstrap applications benefit from session state when users navigate between pages and return to find their tab selection, form drafts, or filter settings preserved within the session.

## Basic Implementation

```html
<!-- Tab state preserved across page navigation -->
<ul class="nav nav-tabs" id="settingsTabs" role="tablist">
  <li class="nav-item" role="presentation">
    <button class="nav-link active" id="profile-tab" data-bs-toggle="tab"
            data-bs-target="#profile" type="button" role="tab"
            data-session-tab="profile">Profile</button>
  </li>
  <li class="nav-item" role="presentation">
    <button class="nav-link" id="security-tab" data-bs-toggle="tab"
            data-bs-target="#security" type="button" role="tab"
            data-session-tab="security">Security</button>
  </li>
  <li class="nav-item" role="presentation">
    <button class="nav-link" id="notifications-tab" data-bs-toggle="tab"
            data-bs-target="#notifications" type="button" role="tab"
            data-session-tab="notifications">Notifications</button>
  </li>
</ul>

<div class="tab-content" id="settingsTabContent">
  <div class="tab-pane fade show active p-3" id="profile" role="tabpanel">Profile settings...</div>
  <div class="tab-pane fade p-3" id="security" role="tabpanel">Security settings...</div>
  <div class="tab-pane fade p-3" id="notifications" role="tabpanel">Notification preferences...</div>
</div>
```

```js
// Session-based tab persistence
const TAB_KEY = 'activeSettingsTab';

// Restore active tab from session
function restoreTab() {
  const savedTab = sessionStorage.getItem(TAB_KEY);
  if (savedTab) {
    const tabButton = document.querySelector(`[data-session-tab="${savedTab}"]`);
    if (tabButton) {
      const tab = new bootstrap.Tab(tabButton);
      tab.show();
    }
  }
}

// Save tab selection on change
document.querySelectorAll('#settingsTabs [data-bs-toggle="tab"]').forEach(btn => {
  btn.addEventListener('shown.bs.tab', (e) => {
    sessionStorage.setItem(TAB_KEY, e.target.dataset.sessionTab);
  });
});

restoreTab();
```

## Advanced Variations

```js
// Form draft preservation with sessionStorage
class FormSessionManager {
  constructor(form, key) {
    this.form = form;
    this.key = `form-draft:${key}`;
    this.init();
  }

  init() {
    this.restore();

    // Save on every input change
    this.form.addEventListener('input', () => this.save());

    // Clear on successful submit
    this.form.addEventListener('submit', () => this.clear());
  }

  save() {
    const data = {};
    new FormData(this.form).forEach((value, key) => {
      data[key] = value;
    });

    // Also save checkbox/radio states
    this.form.querySelectorAll('input[type="checkbox"]').forEach(cb => {
      data[cb.name || cb.id] = cb.checked;
    });

    sessionStorage.setItem(this.key, JSON.stringify(data));
  }

  restore() {
    try {
      const data = JSON.parse(sessionStorage.getItem(this.key));
      if (!data) return;

      Object.entries(data).forEach(([name, value]) => {
        const field = this.form.querySelector(`[name="${name}"], #${name}`);
        if (!field) return;

        if (field.type === 'checkbox') {
          field.checked = value;
        } else if (field.type === 'radio') {
          this.form.querySelector(`[name="${name}"][value="${value}"]`)?.click();
        } else if (field.tagName === 'SELECT') {
          field.value = value;
        } else {
          field.value = value;
        }
      });

      // Show restore notification
      this.showRestoreNotice();
    } catch {
      // Corrupted data — clear it
      sessionStorage.removeItem(this.key);
    }
  }

  clear() {
    sessionStorage.removeItem(this.key);
  }

  showRestoreNotice() {
    const notice = document.createElement('div');
    notice.className = 'alert alert-info alert-dismissible fade show mt-3';
    notice.innerHTML = `
      <i class="bi bi-clock-history me-2"></i>
      Your previous draft has been restored.
      <button type="button" class="btn btn-sm btn-outline-info ms-2" id="clearDraft">Discard</button>
      <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    this.form.prepend(notice);

    notice.querySelector('#clearDraft').addEventListener('click', () => {
      this.clear();
      this.form.reset();
      notice.remove();
    });
  }
}

// Usage
const contactForm = document.getElementById('contactForm');
new FormSessionManager(contactForm, 'contact');
```

```js
// Multi-page navigation state tracker
class SessionNavigation {
  constructor() {
    this.key = 'nav-history';
  }

  push(page) {
    const history = this.getHistory();
    history.push({ page, time: Date.now() });
    // Keep last 20 entries
    if (history.length > 20) history.shift();
    sessionStorage.setItem(this.key, JSON.stringify(history));
  }

  getHistory() {
    try {
      return JSON.parse(sessionStorage.getItem(this.key)) || [];
    } catch {
      return [];
    }
  }

  getPrevious() {
    const history = this.getHistory();
    return history.length > 1 ? history[history.length - 2] : null;
  }

  getBackLink() {
    const prev = this.getPrevious();
    return prev ? prev.page : '/';
  }
}

const nav = new SessionNavigation();
nav.push(window.location.pathname);
```

## Best Practices

1. Use sessionStorage for state that should not persist beyond the current session
2. Save form drafts on every input change for crash recovery
3. Clear sessionStorage on successful form submission to prevent stale drafts
4. Wrap sessionStorage access in try/catch for quota and parsing errors
5. Prefix all keys with a namespace to avoid collisions
6. Use the `storage` event to sync state across iframes in the same tab
7. Store tab selections to restore user's last viewed tab on page return
8. Save navigation history within session for smart "Back" buttons
9. Keep stored data minimal — sessionStorage has a ~5MB limit per origin
10. Provide a "Discard draft" option when restoring form state

## Common Pitfalls

1. **Confusing localStorage and sessionStorage** — sessionStorage clears on tab close; localStorage persists forever
2. **Not clearing on submit** — Storing drafts after successful submission shows stale data on revisit
3. **Storing sensitive data** — Session state is accessible to JavaScript; never store tokens or passwords
4. **Missing error handling** — Private browsing and quota limits throw on write operations
5. **Key collisions** — Multiple forms or tabs sharing the same key overwrite each other
6. **JSON parse on corrupted data** — Always wrap `JSON.parse` in try/catch for sessionStorage values

## Accessibility Considerations

When restoring tab state from sessionStorage, ensure focus is managed correctly — the active tab panel should be announced to screen readers. Form draft restoration should notify users that previous input was recovered. Ensure the notification is announced via `aria-live` so assistive technology users know their data was restored.

## Responsive Behavior

Session state is viewport-independent. Form drafts saved on desktop restore correctly on mobile. However, tab layouts may differ across viewports — vertical tabs on mobile vs horizontal on desktop. Ensure restored tab state works regardless of the current viewport layout.
