---
title: "Tab JavaScript API"
module: "JavaScript Components"
lesson: "04_02_04"
difficulty: 2
estimated_time: 20 minutes
bootstrap_version: 5.3
prerequisites:
  - Bootstrap 5 nav/tab markup
  - JavaScript event handling
learning_objectives:
  - Initialize tabs programmatically with bootstrap.Tab()
  - Use the show method to switch tabs
  - Handle tab lifecycle events
  - Build dynamic tab interfaces with the JavaScript API
---

# Tab JavaScript API

## Overview

The Bootstrap Tab JavaScript API allows programmatic tab switching beyond what `data-bs-toggle="tab"` provides. The `bootstrap.Tab` class wraps each tab trigger element and exposes a `show()` method plus four lifecycle events for fine-grained control over tab transitions.

This API is particularly useful for creating dynamic tab interfaces, syncing tab state with URLs, implementing keyboard shortcuts, or building wizard-style multi-step forms.

```js
const tabTrigger = document.querySelector('#myTab button[data-bs-target="#profile"]');
const tab = new bootstrap.Tab(tabTrigger);
tab.show();
```

## Basic Implementation

### HTML Structure

```html
<ul class="nav nav-tabs" id="myTab" role="tablist">
  <li class="nav-item" role="presentation">
    <button class="nav-link active" id="home-tab" data-bs-toggle="tab"
            data-bs-target="#home" type="button" role="tab"
            aria-controls="home" aria-selected="true">Home</button>
  </li>
  <li class="nav-item" role="presentation">
    <button class="nav-link" id="profile-tab" data-bs-toggle="tab"
            data-bs-target="#profile" type="button" role="tab"
            aria-controls="profile" aria-selected="false">Profile</button>
  </li>
  <li class="nav-item" role="presentation">
    <button class="nav-link" id="settings-tab" data-bs-toggle="tab"
            data-bs-target="#settings" type="button" role="tab"
            aria-controls="settings" aria-selected="false">Settings</button>
  </li>
</ul>
<div class="tab-content" id="myTabContent">
  <div class="tab-pane fade show active" id="home" role="tabpanel" aria-labelledby="home-tab">
    <p>Home content</p>
  </div>
  <div class="tab-pane fade" id="profile" role="tabpanel" aria-labelledby="profile-tab">
    <p>Profile content</p>
  </div>
  <div class="tab-pane fade" id="settings" role="tabpanel" aria-labelledby="settings-tab">
    <p>Settings content</p>
  </div>
</div>
```

### Programmatic Tab Switching

```js
// Initialize a specific tab
const profileTab = new bootstrap.Tab(document.getElementById('profile-tab'));

// Show it
profileTab.show();

// Switch to settings tab
const settingsTab = new bootstrap.Tab(document.getElementById('settings-tab'));
settingsTab.show();
```

### External Button Trigger

```html
<button class="btn btn-primary mt-3" id="goToProfile">
  Go to Profile Tab
</button>
```

```js
document.getElementById('goToProfile').addEventListener('click', () => {
  const tab = new bootstrap.Tab(document.getElementById('profile-tab'));
  tab.show();
});
```

## Advanced Variations

### Tab Lifecycle Events

```js
const profileTabEl = document.getElementById('profile-tab');

profileTabEl.addEventListener('show.bs.tab', (event) => {
  // event.target = new tab being shown
  // event.relatedTarget = previous active tab
  console.log('Switching from:', event.relatedTarget?.id);
  console.log('Switching to:', event.target.id);
});

profileTabEl.addEventListener('shown.bs.tab', (event) => {
  console.log('Tab is now active:', event.target.id);
  // Initialize content that requires the tab to be visible
});

profileTabEl.addEventListener('hide.bs.tab', (event) => {
  console.log('Tab is hiding:', event.target.id);
});

profileTabEl.addEventListener('hidden.bs.tab', (event) => {
  console.log('Tab is now hidden:', event.target.id);
  // Cleanup resources from the hidden tab
});
```

### URL Hash Synchronization

```js
// Activate tab from URL hash on page load
const hash = window.location.hash;
if (hash) {
  const trigger = document.querySelector(`button[data-bs-target="${hash}"]`);
  if (trigger) {
    new bootstrap.Tab(trigger).show();
  }
}

// Update URL hash when tabs change
document.querySelectorAll('#myTab button[data-bs-toggle="tab"]').forEach(btn => {
  btn.addEventListener('shown.bs.tab', (event) => {
    const target = event.target.getAttribute('data-bs-target');
    history.replaceState(null, '', target);
  });
});
```

### Dynamic Tab Creation

```js
function addTab(id, title, content) {
  const tabList = document.getElementById('myTab');
  const tabContent = document.getElementById('myTabContent');

  // Create tab trigger
  const li = document.createElement('li');
  li.className = 'nav-item';
  li.role = 'presentation';
  li.innerHTML = `
    <button class="nav-link" id="${id}-tab" data-bs-toggle="tab"
            data-bs-target="#${id}" type="button" role="tab"
            aria-controls="${id}" aria-selected="false">${title}</button>
  `;
  tabList.appendChild(li);

  // Create tab pane
  const pane = document.createElement('div');
  pane.className = 'tab-pane fade';
  pane.id = id;
  pane.role = 'tabpanel';
  pane.setAttribute('aria-labelledby', `${id}-tab`);
  pane.innerHTML = content;
  tabContent.appendChild(pane);

  // Show the new tab
  const tab = new bootstrap.Tab(document.getElementById(`${id}-tab`));
  tab.show();
}

addTab('reports', 'Reports', '<p>Reports content loaded dynamically.</p>');
```

### Retrieving Instances

```js
const existing = bootstrap.Tab.getInstance(document.getElementById('profile-tab'));
const instance = bootstrap.Tab.getOrCreateInstance(document.getElementById('profile-tab'));
```

## Best Practices

1. **Always use `<button>` elements** as tab triggers, not `<a>` tags, for semantic correctness and keyboard handling.
2. **Set `role="tablist"`, `role="tab"`, and `role="tabpanel"`** for proper ARIA semantics.
3. **Use `data-bs-target`** to link triggers to panes; avoid relying solely on `href` for tab navigation.
4. **Initialize content in `shown.bs.tab`**, not `show.bs.tab`, to ensure the pane is visible and has layout.
5. **Use `relatedTarget`** in event handlers to reference the previously active tab for cleanup logic.
6. **Dispose of tab instances** when removing tab elements dynamically from the DOM.
7. **Use `fade` class on panes** for smooth transitions; without it, tabs switch instantly.
8. **Keep tab content lightweight** — lazy-load heavy content in `shown.bs.tab` to improve initial page load.
9. **Set `aria-selected` correctly** — Bootstrap manages this automatically, but verify when building custom tab UIs.
10. **Use `getOrCreateInstance`** to prevent duplicate instances on the same trigger element.
11. **Support keyboard navigation** — tabs should respond to arrow keys. Bootstrap handles this partially; ensure custom implementations support it.

## Common Pitfalls

1. **Passing the pane element instead of the trigger** to `new bootstrap.Tab()` — the constructor expects the tab button, not the content pane.
2. **Missing `data-bs-toggle="tab"`** on the trigger — the `show()` method works but auto-initialization won't.
3. **Not waiting for `shown.bs.tab`** before interacting with pane content — the pane may not be visible or have computed styles yet.
4. **Using `href` instead of `data-bs-target`** — works in some cases but `data-bs-target` is the canonical Bootstrap 5 approach.
5. **Creating tabs without proper ARIA roles** — breaks screen reader navigation. Every tab list needs `role="tablist"`.
6. **Not cleaning up dynamic tabs** — removing a tab trigger without disposing leaves event listeners and orphan references.
7. **Overriding tab transition CSS** — custom `transition` rules can conflict with Bootstrap's `.fade` animation timing.

## Accessibility Considerations

- Tab triggers must be `<button>` elements with `role="tab"`.
- The tab list container needs `role="tablist"`.
- Each pane needs `role="tabpanel"` with `aria-labelledby` pointing to its trigger.
- Active tab has `aria-selected="true"`; inactive tabs have `aria-selected="false"`.
- Tab panes should have `tabindex="0"` if they contain focusable content that needs keyboard access.
- Arrow keys should navigate between tabs within the tablist (Left/Right for horizontal, Up/Down for vertical).

```html
<ul class="nav nav-tabs" role="tablist">
  <li class="nav-item" role="presentation">
    <button class="nav-link active" id="tab1" data-bs-toggle="tab"
            data-bs-target="#panel1" type="button" role="tab"
            aria-controls="panel1" aria-selected="true">Tab 1</button>
  </li>
</ul>
<div class="tab-content">
  <div class="tab-pane fade show active" id="panel1" role="tabpanel"
       aria-labelledby="tab1" tabindex="0">
    Content accessible to assistive technology.
  </div>
</div>
```

## Responsive Behavior

Tabs adapt to different screen sizes with Bootstrap utility classes:

```html
<!-- Stack vertically on small screens -->
<ul class="nav nav-tabs flex-column flex-sm-row" role="tablist">
  <li class="nav-item">
    <button class="nav-link active flex-sm-fill" data-bs-toggle="tab"
            data-bs-target="#tab1" type="button" role="tab">Tab 1</button>
  </li>
  <li class="nav-item">
    <button class="nav-link flex-sm-fill" data-bs-toggle="tab"
            data-bs-target="#tab2" type="button" role="tab">Tab 2</button>
  </li>
</ul>
```

- Use **pills** (`nav-pills`) instead of tabs on mobile for a more compact layout.
- Convert tabs to a **select dropdown** on very small screens:

```js
function handleResponsiveTabs() {
  const tabList = document.getElementById('myTab');
  if (window.innerWidth < 480) {
    tabList.classList.add('d-none');
    document.getElementById('tabSelect')?.classList.remove('d-none');
  } else {
    tabList.classList.remove('d-none');
    document.getElementById('tabSelect')?.classList.add('d-none');
  }
}
window.addEventListener('resize', handleResponsiveTabs);
handleResponsiveTabs();
```

- Use `flex-fill` or `flex-sm-fill` on nav items to distribute tab width evenly across available space.
