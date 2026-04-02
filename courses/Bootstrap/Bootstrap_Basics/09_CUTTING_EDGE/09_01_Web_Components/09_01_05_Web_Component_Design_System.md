---
title: "Web Component Design System"
description: "Building a design system using web components and Bootstrap, with component registry and token architecture"
difficulty: 3
tags: [design-system, web-components, bootstrap, tokens, registry]
prerequisites:
  - 09_01_03_Shadow_DOM_Styling
  - 09_01_04_Slot_Based_Layout
---

## Overview

A web component design system combines Bootstrap's utility foundation with custom elements to create a framework-agnostic, portable component library. The design system defines design tokens (colors, spacing, typography) as CSS custom properties, registers components in a central registry, and provides a consistent API surface across all components.

The architecture separates concerns: tokens define values, components implement structure, and Bootstrap provides layout utilities. A component registry ensures naming consistency, version tracking, and prevents duplicate registrations. This approach yields components that work in React, Vue, Angular, Svelte, or plain HTML with zero framework dependencies.

## Basic Implementation

```js
// design-tokens.css (injected into each shadow root)
const TOKENS = `
  :host {
    --ds-primary: #0d6efd;
    --ds-secondary: #6c757d;
    --ds-success: #198754;
    --ds-danger: #dc3545;
    --ds-spacing-xs: 0.25rem;
    --ds-spacing-sm: 0.5rem;
    --ds-spacing-md: 1rem;
    --ds-spacing-lg: 1.5rem;
    --ds-radius: 0.375rem;
    --ds-font-family: system-ui, -apple-system, sans-serif;
  }
`;

// Component registry
class DesignSystemRegistry {
  constructor() {
    this.components = new Map();
  }

  register(name, klass) {
    if (this.components.has(name)) {
      console.warn(`[DS] Component "${name}" already registered`);
      return;
    }
    customElements.define(name, klass);
    this.components.set(name, { class: klass, version: '1.0.0' });
  }

  get(name) { return this.components.get(name); }
  has(name) { return this.components.has(name); }
  list() { return [...this.components.keys()]; }
}

window.DS = new DesignSystemRegistry();
```

```html
<ds-badge variant="success" pill>v2.1.0</ds-badge>
<ds-badge variant="warning" outline>beta</ds-badge>
```

```js
class DsBadge extends HTMLElement {
  static get observedAttributes() { return ['variant', 'pill', 'outline']; }

  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this.shadowRoot.innerHTML = `
      <style>${TOKENS}</style>
      <span class="badge" part="badge"><slot></slot></span>
    `;
    this._el = this.shadowRoot.querySelector('.badge');
  }

  connectedCallback() { this._sync(); }
  attributeChangedCallback() { this._sync(); }

  _sync() {
    const variant = this.getAttribute('variant') || 'secondary';
    const outline = this.hasAttribute('outline');
    const prefix = outline ? 'text-' : 'bg-';
    this._el.className = `badge ${prefix}${variant}`;
    if (this.hasAttribute('pill')) this._el.classList.add('rounded-pill');
  }
}

DS.register('ds-badge', DsBadge);
```

```css
/* Consumer theming — overrides propagate to all components */
:root {
  --ds-primary: #6f42c1;
  --ds-radius: 1rem;
}
```

## Advanced Variations

Build a shared base class for all components:

```js
class DsElement extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }

  _render(template) {
    const styleSheet = new CSSStyleSheet();
    styleSheet.replaceSync(TOKENS);
    this.shadowRoot.adoptedStyleSheets = [styleSheet];
    this.shadowRoot.innerHTML += template;
  }

  _attr(name) { return this.getAttribute(name); }
  _bool(name) { return this.hasAttribute(name); }
}

class DsAlert extends DsElement {
  constructor() {
    super();
    this._render(`<div class="alert" part="alert"><slot></slot></div>`);
  }
}

DS.register('ds-alert', DsAlert);
```

## Best Practices

1. Define all design tokens as CSS custom properties under `:host` or `:root`.
2. Use a central registry (`DS.register()`) to prevent naming collisions.
3. Prefix all custom element names with a namespace (`ds-`, `bs-`) to avoid conflicts with native elements.
4. Share a `DsElement` base class for common shadow DOM setup, token injection, and attribute helpers.
5. Use `adoptedStyleSheets` for shared styles across components to reduce memory.
6. Version components in the registry for compatibility tracking.
7. Provide a `DS.list()` method to introspect registered components at runtime.
8. Document each component's attributes, slots, events, and CSS parts.
9. Use semantic attribute names (`variant`, `size`, `disabled`) consistent across all components.
10. Test components in isolation and in composition.
11. Provide TypeScript declarations for consumer DX.
12. Ship components as ES modules with tree-shaking support.

## Common Pitfalls

1. **Token conflicts** — Consumer `:root` overrides may not propagate if tokens are defined on `:host` without forwarding.
2. **Duplicate registration** — Calling `customElements.define()` twice with the same name throws; use a registry guard.
3. **Base class coupling** — Over-abstracting the base class leads to brittle inheritance; prefer composition.
4. **Missing namespace prefix** — Unprefixed element names collide with future HTML elements.
5. **No versioning** — Breaking changes without version tracking break consumer applications.
6. **Token sprawl** — Too many tokens become unmaintainable; keep to ~50 core tokens.

## Accessibility Considerations

Build accessibility into the base class: enforce `role` attributes, provide `aria-*` forwarding, and validate accessible names during development. Log warnings when required ARIA attributes are missing.

## Responsive Behavior

Expose responsive token variants (e.g., `--ds-spacing-md-mobile`, `--ds-spacing-md-desktop`) and apply them via media queries in the shared base stylesheet. Components should respond to their container, not just the viewport.
