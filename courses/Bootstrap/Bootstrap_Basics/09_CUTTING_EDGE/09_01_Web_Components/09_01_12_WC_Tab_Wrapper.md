---
title: Web Component Tab Wrapper
category: [Web Components, Cutting Edge]
difficulty: 3
time: 30 min
tags: bootstrap5, web-components, custom-elements, tabs, keyboard-nav
---

## Overview

A tab web component wraps Bootstrap's tab/pill system as a custom element with reactive attributes, declarative panel slots, and built-in keyboard navigation. This enables encapsulated, reusable tab interfaces that work across frameworks with minimal API surface.

## Basic Implementation

```html
<bs-tabs>
  <bs-tab label="Overview" active>
    <p>Dashboard overview content here.</p>
  </bs-tab>
  <bs-tab label="Analytics">
    <p>Analytics charts and tables here.</p>
  </bs-tab>
  <bs-tab label="Settings">
    <p>Configuration options here.</p>
  </bs-tab>
</bs-tabs>

<script>
class BsTabs extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }

  connectedCallback() {
    this.render();
    this._setupListeners();
  }

  render() {
    this.shadowRoot.innerHTML = `
      <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
      <ul class="nav nav-tabs" role="tablist">
        <slot name="tabs"></slot>
      </ul>
      <div class="tab-content border border-top-0 p-3">
        <slot name="panels"></slot>
      </div>
    `;
  }

  _setupListeners() {
    this.shadowRoot.querySelector('slot[name="tabs"]')
      .addEventListener('slotchange', () => this._initTabs());
  }

  _initTabs() {
    const tabs = this.querySelectorAll('bs-tab');
    tabs.forEach((tab, i) => {
      const li = document.createElement('li');
      li.className = 'nav-item';
      li.setAttribute('role', 'presentation');
      li.innerHTML = `<button class="nav-link ${tab.hasAttribute('active') ? 'active' : ''}"
        data-index="${i}" role="tab">${tab.getAttribute('label')}</button>`;
      li.querySelector('button').slot = 'tabs';
      this.shadowRoot.querySelector('ul').appendChild(li);
    });
  }
}
customElements.define('bs-tabs', BsTabs);

class BsTab extends HTMLElement {
  connectedCallback() {
    this.setAttribute('slot', this.hasAttribute('active') ? 'panels' : 'panels');
    this.style.display = this.hasAttribute('active') ? 'block' : 'none';
  }
}
customElements.define('bs-tab', BsTab);
</script>
```

## Advanced Variations

Reactive active tab with attribute-based switching and keyboard navigation.

```html
<bs-tabs active-tab="1" variant="pills">
  <bs-tab label="Profile">User profile details</bs-tab>
  <bs-tab label="Messages">Inbox messages</bs-tab>
  <bs-tab label="Settings">Account settings</bs-tab>
</bs-tabs>

<script>
class BsTabs extends HTMLElement {
  static get observedAttributes() {
    return ['active-tab', 'variant'];
  }

  get activeTab() {
    return parseInt(this.getAttribute('active-tab') || '0');
  }

  set activeTab(index) {
    this.setAttribute('active-tab', index);
  }

  attributeChangedCallback(name, old, val) {
    if (old !== val && this.shadowRoot) this._updateActive();
  }

  _updateActive() {
    const tabs = this.shadowRoot.querySelectorAll('.nav-link');
    const panels = this.querySelectorAll('bs-tab');
    const idx = this.activeTab;

    tabs.forEach((t, i) => {
      t.classList.toggle('active', i === idx);
      t.setAttribute('aria-selected', i === idx);
    });
    panels.forEach((p, i) => {
      p.style.display = i === idx ? 'block' : 'none';
    });

    this.dispatchEvent(new CustomEvent('bs-tab-change', {
      bubbles: true, composed: true, detail: { index: idx }
    }));
  }

  _setupKeyboard() {
    const nav = this.shadowRoot.querySelector('.nav');
    nav.addEventListener('keydown', (e) => {
      const tabs = [...nav.querySelectorAll('.nav-link')];
      const current = tabs.findIndex(t => t.classList.contains('active'));
      let next;

      if (e.key === 'ArrowRight') next = (current + 1) % tabs.length;
      else if (e.key === 'ArrowLeft') next = (current - 1 + tabs.length) % tabs.length;
      else if (e.key === 'Home') next = 0;
      else if (e.key === 'End') next = tabs.length - 1;
      else return;

      e.preventDefault();
      this.activeTab = next;
      tabs[next].focus();
    });
  }
}
customElements.define('bs-tabs', BsTabs);
</script>
```

## Best Practices

1. Use `role="tablist"`, `role="tab"`, and `role="tabpanel"` for ARIA semantics
2. Support Arrow Left/Right keyboard navigation between tabs
3. Reflect `active-tab` attribute for declarative and programmatic control
4. Dispatch `bs-tab-change` event with the new tab index in `detail`
5. Support both `nav-tabs` and `nav-pills` via a `variant` attribute
6. Use `aria-selected` to indicate the active tab to screen readers
7. Allow lazy panel rendering with a `lazy` attribute to defer content
8. Support `disabled` attribute on individual tabs
9. Use `Home`/`End` keys for jumping to first/last tab
10. Clean up delegated event listeners in `disconnectedCallback`

## Common Pitfalls

1. **No ARIA roles** — Screen readers cannot identify the tab interface
2. **Missing keyboard nav** — Users cannot switch tabs without a mouse
3. **Not syncing `aria-selected`** — Active state is invisible to assistive tech
4. **Panels not hiding** — All panels display simultaneously instead of one at a time
5. **Attribute not reflected** — Setting `active-tab` programmatically doesn't update UI
6. **Eager rendering all panels** — Performance hit from rendering all tab content upfront
7. **No disabled state** — Tabs cannot be marked as non-interactive
8. **Event not composed** — Consumer cannot listen for tab changes outside shadow root

## Accessibility Considerations

Implement WAI-ARIA tab pattern with `role="tablist"`, `role="tab"`, and `role="tabpanel"`. Each tab panel should have `aria-labelledby` pointing to its tab. Support full keyboard navigation including Arrow keys, Home, and End. Manage `tabindex` so only the active tab is in the tab order.

## Responsive Behavior

On small screens, convert horizontal tabs to a stacked vertical list using `flex-column` or a select dropdown. Use Bootstrap's `nav-fill` or `nav-justified` classes via attributes for full-width tabs on mobile. Hide tab labels and show only icons with `d-none d-md-inline` for narrow viewports.
