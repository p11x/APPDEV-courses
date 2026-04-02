---
title: "Slot-Based Layout"
description: "Using HTML slots with Bootstrap grid for flexible, composable web component layouts"
difficulty: 3
tags: [slots, web-components, bootstrap-grid, content-projection, composition]
prerequisites:
  - 09_01_03_Shadow_DOM_Styling
---

## Overview

Slots are the web component equivalent of React's `children` or Vue's `<slot>`. They project light DOM content into specific locations within a shadow DOM template. Combined with Bootstrap's grid system, slots enable composable layouts where a parent component defines the grid structure and child components or HTML fills each cell.

Named slots provide explicit placement (`slot="sidebar"`, `slot="main"`), while the default slot catches unmatched content. Fallback content inside a `<slot>` renders when no matching light DOM content is provided. This pattern is ideal for layout shells, card templates, dashboard panels, and any repeated structural pattern.

## Basic Implementation

```html
<bs-dashboard>
  <nav slot="sidebar">
    <a href="/dashboard">Dashboard</a>
    <a href="/settings">Settings</a>
  </nav>
  <main slot="content">
    <h1>Welcome</h1>
    <p>Dashboard content here.</p>
  </main>
  <aside slot="widgets">
    <bs-widget title="Stats">42 active users</bs-widget>
  </aside>
</bs-dashboard>
```

```js
class BsDashboard extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this.shadowRoot.innerHTML = `
      <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
      <div class="container-fluid">
        <div class="row min-vh-100">
          <div class="col-md-3 col-lg-2 bg-light p-3" part="sidebar">
            <slot name="sidebar">
              <p class="text-muted">Navigation placeholder</p>
            </slot>
          </div>
          <div class="col-md-6 col-lg-8 p-3" part="content">
            <slot name="content">
              <div class="alert alert-info">No content provided</div>
            </slot>
          </div>
          <div class="col-md-3 col-lg-2 p-3" part="widgets">
            <slot name="widgets">
              <p class="text-muted">Widgets area</p>
            </slot>
          </div>
        </div>
      </div>
    `;
  }
}

customElements.define('bs-dashboard', BsDashboard);
```

```css
/* Light DOM slotted content styling */
bs-dashboard [slot="sidebar"] a {
  display: block;
  padding: 0.5rem 1rem;
  color: var(--bs-body-color);
  text-decoration: none;
  border-radius: 0.375rem;
}

bs-dashboard [slot="sidebar"] a:hover {
  background: var(--bs-primary-bg-subtle);
}
```

## Advanced Variations

Use the `slotchange` event to react when slotted content changes:

```js
connectedCallback() {
  const contentSlot = this.shadowRoot.querySelector('slot[name="content"]');
  contentSlot.addEventListener('slotchange', () => {
    const nodes = contentSlot.assignedNodes();
    this.dispatchEvent(new CustomEvent('bs:content-changed', {
      detail: { count: nodes.length },
      bubbles: true
    }));
  });
}
```

Create a grid component with dynamic column slots:

```js
class BsGrid extends HTMLElement {
  static get observedAttributes() { return ['columns']; }

  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }

  _render() {
    const cols = parseInt(this.getAttribute('columns')) || 3;
    const colClass = `col-${12 / cols}`;
    let slots = '';
    for (let i = 1; i <= cols; i++) {
      slots += `<div class="${colClass}"><slot name="col-${i}"></slot></div>`;
    }
    this.shadowRoot.innerHTML = `
      <link rel="stylesheet" href="...bootstrap.min.css">
      <div class="row">${slots}</div>
    `;
  }

  connectedCallback() { this._render(); }
  attributeChangedCallback() { if (this.isConnected) this._render(); }
}

customElements.define('bs-grid', BsGrid);
```

## Best Practices

1. Always provide fallback content inside `<slot>` elements for graceful degradation.
2. Use named slots for predictable content placement; avoid relying on default slot ordering.
3. Listen to `slotchange` events to react to dynamic content insertion or removal.
4. Use `assignedNodes({ flatten: true })` to get the flattened assigned node list.
5. Style slotted content from the light DOM, not the shadow DOM (slotted content is in light DOM).
6. Use `::slotted()` pseudo-element for minimal styling of projected content from shadow DOM.
7. Combine slots with Bootstrap grid classes inside the shadow template for responsive layouts.
8. Document slot names and expected content types in component README.
9. Use `slot="*"` attribute in light DOM to assign different children to different slots.
10. Avoid deeply nested slot hierarchies; they complicate debugging and reduce performance.
11. Test with empty slots, partial slots, and full slot sets.
12. Use `<slot name="header">` naming consistent with Bootstrap section names.

## Common Pitfalls

1. **`::slotted()` limitation** — Only targets direct slotted children, not nested elements inside them.
2. **Slot fallback not showing** — Whitespace text nodes in light DOM count as slotted content and suppress fallback.
3. **Styling slotted content** — Light DOM content must be styled from light DOM or via `::slotted()`, not internal shadow selectors.
4. **Named slot mismatch** — Light DOM `slot="foo"` with no matching `<slot name="foo">` renders in the default slot.
5. **Re-rendering destroys slots** — Replacing `shadowRoot.innerHTML` removes existing slot bindings; re-render carefully.
6. **Event bubbling across slots** — Events from slotted content bubble in light DOM, not shadow DOM; use `composed: true`.

## Accessibility Considerations

Slotted content retains its light DOM accessibility properties. Semantic elements (`<nav>`, `<main>`, `<aside>`) projected through slots maintain their ARIA landmarks. Ensure the shadow template does not add duplicate landmarks around slots.

## Responsive Behavior

Bootstrap grid classes inside the shadow template handle responsive behavior. Slot content adapts to the grid cell width. Use container queries on the host element for component-level responsiveness:

```css
bs-dashboard {
  container-type: inline-size;
}

@container (max-width: 768px) {
  /* Stack sidebar above content on narrow containers */
}
```
