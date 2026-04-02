---
title: "Custom Element Button"
description: "Wrapping a Bootstrap button as a web component with shadow DOM styling and attribute reflection"
difficulty: 3
tags: [web-components, custom-elements, shadow-dom, bootstrap-button]
prerequisites:
  - 01_01_Bootstrap_Setup
  - 09_01_00_Web_Components_Intro
---

## Overview

Wrapping Bootstrap buttons inside custom elements enables truly portable, encapsulated UI components. A `<bs-button>` custom element encapsulates Bootstrap's button classes, handles attribute reflection for properties like `variant`, `size`, and `disabled`, and uses Shadow DOM to isolate its styles from the global stylesheet.

Attribute reflection keeps the DOM synchronized with component state. When you set `bsButton.variant = 'danger'`, the reflected attribute updates to `variant="danger"`, enabling CSS attribute selectors and server-side rendering hydration. Shadow DOM prevents Bootstrap's global CSS from leaking in or out, giving you predictable styling regardless of the page context.

The key challenge is mapping Bootstrap's utility-class API (e.g., `btn btn-primary btn-lg`) into a custom element API that feels native. This requires careful attribute-to-property mapping, handling the light DOM fallback for no-JS environments, and ensuring Bootstrap's JavaScript behavior (ripple effect, focus ring) still functions within the shadow boundary.

## Basic Implementation

```html
<template id="bs-button-template">
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
  <button class="btn" part="button">
    <slot></slot>
  </button>
</template>

<bs-button variant="primary" size="lg">Click Me</bs-button>
```

```js
class BsButton extends HTMLElement {
  static get observedAttributes() {
    return ['variant', 'size', 'disabled', 'outline'];
  }

  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    const template = document.getElementById('bs-button-template');
    this.shadowRoot.appendChild(template.content.cloneNode(true));
    this._button = this.shadowRoot.querySelector('button');
  }

  connectedCallback() {
    this._syncAttributes();
  }

  attributeChangedCallback() {
    this._syncAttributes();
  }

  _syncAttributes() {
    const variant = this.getAttribute('variant') || 'secondary';
    const size = this.getAttribute('size');
    const outline = this.hasAttribute('outline');
    const prefix = outline ? 'btn-outline-' : 'btn-';

    this._button.className = `btn ${prefix}${variant}`;
    if (size) this._button.classList.add(`btn-${size}`);
    this._button.disabled = this.hasAttribute('disabled');
  }

  get variant() { return this.getAttribute('variant'); }
  set variant(val) { this.setAttribute('variant', val); }

  get disabled() { return this.hasAttribute('disabled'); }
  set disabled(val) { val ? this.setAttribute('disabled', '') : this.removeAttribute('disabled'); }
}

customElements.define('bs-button', BsButton);
```

```css
/* External styles CANNOT reach inside shadow DOM by default */
/* Use ::part() for controlled penetration */
bs-button::part(button) {
  border-radius: 0.5rem;
  letter-spacing: 0.05em;
}

/* Attribute selectors work on the host element */
bs-button[variant="danger"] {
  animation: shake 0.3s ease-in-out;
}
```

## Advanced Variations

Use CSS custom properties to allow external theming without breaking encapsulation. Bootstrap 5's CSS custom properties (e.g., `--bs-btn-bg`) can be forwarded into the shadow DOM.

```js
_syncStyles() {
  const props = [
    '--bs-primary', '--bs-btn-bg', '--bs-btn-color',
    '--bs-btn-hover-bg', '--bs-btn-border-color'
  ];
  const computed = getComputedStyle(this);
  props.forEach(prop => {
    const val = computed.getPropertyValue(prop);
    if (val) this._button.style.setProperty(prop, val);
  });
}
```

For progressive enhancement, render a plain `<button>` in light DOM and upgrade it with JavaScript. The shadow DOM template replaces the light DOM content on hydration.

## Best Practices

1. Always set `static get observedAttributes()` to list every attribute you watch for changes.
2. Reflect properties to attributes for CSS selector compatibility and SSR hydration.
3. Use `<slot>` for content projection so light DOM children render inside the shadow tree.
4. Forward CSS custom properties from the host into shadow DOM for theming.
5. Use `part="button"` on the inner element to enable `::part()` styling from outside.
6. Keep the shadow root in `mode: 'open'` unless you have a security reason for closed mode.
7. Call attribute sync logic in both `connectedCallback` and `attributeChangedCallback`.
8. Provide a no-JS fallback by rendering default button markup in light DOM.
9. Use `delegatesFocus: true` in the shadow root options to forward focus to the inner button.
10. Test with `customElements.get('bs-button')` to verify registration.
11. Document every reflected attribute with its expected values and default.
12. Use `this.isConnected` guards before DOM operations in async callbacks.

## Common Pitfalls

1. **Forgetting `observedAttributes`** — `attributeChangedCallback` never fires without the static getter listing attribute names.
2. **Shadow DOM style isolation** — Bootstrap's global CSS does not penetrate the shadow boundary; you must explicitly link or embed styles inside the shadow root.
3. **Event retargeting** — Click events from inside shadow DOM are retargeted to the host element; `event.target` becomes `<bs-button>`, not the inner `<button>`.
4. **SSR mismatch** — Server-rendered light DOM content flashes before shadow DOM hydrates; use `:defined` CSS to hide un-upgraded elements.
5. **Attribute vs property desync** — Setting `element.variant` without reflecting to the attribute breaks CSS attribute selectors.
6. **Focus trap issues** — Without `delegatesFocus`, tabbing may skip the inner button entirely.

## Accessibility Considerations

Set `role="button"` and `tabindex="0"` on the host element or ensure the shadow inner button receives focus. Forward `aria-label`, `aria-pressed`, and `aria-disabled` attributes from the host to the inner button programmatically.

```js
_syncA11y() {
  const label = this.getAttribute('aria-label');
  if (label) this._button.setAttribute('aria-label', label);
  this._button.setAttribute('aria-disabled', this.hasAttribute('disabled'));
}
```

Use `delegatesFocus: true` so that keyboard users can tab directly into the inner button. Announce state changes (loading, success) with `aria-live` regions in the light DOM.

## Responsive Behavior

Custom elements inherit container and viewport context but do not automatically respond to Bootstrap's responsive utility classes. Use CSS custom properties to pass responsive values into the shadow tree:

```css
bs-button {
  --bs-btn-padding-x: clamp(0.5rem, 2vw, 1.5rem);
  --bs-btn-padding-y: clamp(0.25rem, 1vw, 0.75rem);
}
```

For component-level responsiveness independent of viewport, pair with container queries (see 09_02_01).
