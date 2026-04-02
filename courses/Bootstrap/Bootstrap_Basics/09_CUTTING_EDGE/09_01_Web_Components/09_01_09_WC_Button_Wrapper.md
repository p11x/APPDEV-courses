---
title: Web Component Button Wrapper
category: [Web Components, Cutting Edge]
difficulty: 3
time: 30 min
tags: bootstrap5, web-components, custom-elements, shadow-dom, button
---

## Overview

Wrapping Bootstrap buttons in a web component provides a reusable, encapsulated element with attribute reflection and custom events. This pattern allows teams to distribute Bootstrap-styled buttons as framework-agnostic custom elements while preserving Bootstrap's design tokens and behavior.

## Basic Implementation

Define a custom element that applies Bootstrap button classes and reflects variant attributes.

```html
<bs-button variant="primary" size="lg" disabled>Click Me</bs-button>

<script>
class BsButton extends HTMLElement {
  static get observedAttributes() {
    return ['variant', 'size', 'disabled', 'outline'];
  }

  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }

  connectedCallback() {
    this.render();
  }

  attributeChangedCallback() {
    this.render();
  }

  render() {
    const variant = this.getAttribute('variant') || 'primary';
    const size = this.getAttribute('size') || '';
    const outline = this.hasAttribute('outline');
    const disabled = this.hasAttribute('disabled');
    const prefix = outline ? 'btn-outline-' : 'btn-';
    const sizeClass = size ? `btn-${size}` : '';

    this.shadowRoot.innerHTML = `
      <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
      <button class="btn ${prefix}${variant} ${sizeClass}" ${disabled ? 'disabled' : ''}>
        <slot></slot>
      </button>
    `;

    this.shadowRoot.querySelector('button').addEventListener('click', (e) => {
      if (disabled) return;
      this.dispatchEvent(new CustomEvent('bs-click', {
        bubbles: true, composed: true, detail: { variant }
      }));
    });
  }
}
customElements.define('bs-button', BsButton);
</script>
```

## Advanced Variations

Adding attribute reflection so properties sync bidirectionally between JS and HTML.

```html
<bs-button variant="success" loading>Save</bs-button>

<script>
class BsButton extends HTMLElement {
  static get observedAttributes() {
    return ['variant', 'size', 'disabled', 'outline', 'loading'];
  }

  get variant() { return this.getAttribute('variant') || 'primary'; }
  set variant(v) { this.setAttribute('variant', v); }

  get loading() { return this.hasAttribute('loading'); }
  set loading(v) { v ? this.setAttribute('loading', '') : this.removeAttribute('loading'); }

  connectedCallback() {
    this.render();
  }

  attributeChangedCallback(name, oldVal, newVal) {
    if (oldVal !== newVal) this.render();
  }

  render() {
    const variant = this.variant;
    const size = this.getAttribute('size') || '';
    const outline = this.hasAttribute('outline');
    const disabled = this.hasAttribute('disabled');
    const loading = this.loading;
    const prefix = outline ? 'btn-outline-' : 'btn-';
    const sizeClass = size ? `btn-${size}` : '';

    this.shadowRoot.innerHTML = `
      <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
      <style>
        :host { display: inline-block; }
        .spinner-border { width: 1em; height: 1em; border-width: 0.15em; }
      </style>
      <button class="btn ${prefix}${variant} ${sizeClass}"
              ${disabled || loading ? 'disabled' : ''}>
        ${loading ? '<span class="spinner-border me-1"></span>' : ''}
        <slot></slot>
      </button>
    `;
  }
}
customElements.define('bs-button', BsButton);

// Usage with property access
const btn = document.querySelector('bs-button');
btn.variant = 'danger';
btn.loading = false;
</script>
```

Composing multiple button variants with a shared web component base.

```html
<div class="btn-group">
  <bs-button variant="primary" size="sm">
    <i class="bi bi-save"></i> Save
  </bs-button>
  <bs-button variant="outline-secondary" size="sm">
    <i class="bi bi-x"></i> Cancel
  </bs-button>
  <bs-button variant="danger" size="sm" outline>
    <i class="bi bi-trash"></i> Delete
  </bs-button>
</div>
```

## Best Practices

1. Use `mode: 'open'` on Shadow DOM initially for easier debugging during development
2. Load Bootstrap CSS inside Shadow DOM via `<link>` for style encapsulation
3. Reflect attributes to properties for ergonomic JS API access
4. Dispatch `composed: true` custom events so they cross Shadow DOM boundaries
5. Use `observedAttributes` static getter for efficient change detection
6. Prefix custom events with `bs-` to avoid collisions with native events
7. Handle `disabled` attribute to prevent event dispatch from the component
8. Use `<slot>` for content projection so inner HTML remains light
9. Clean up event listeners in `disconnectedCallback` to prevent memory leaks
10. Document the public API (attributes, properties, events) for consumers
11. Provide sensible defaults for all attributes to reduce boilerplate
12. Use CSS custom properties to allow theme overrides without attribute changes

## Common Pitfalls

1. **Not loading CSS inside Shadow DOM** — Bootstrap styles fail to apply to encapsulated markup
2. **Missing `composed: true`** — Custom events don't propagate outside the shadow root
3. **Not reflecting properties to attributes** — `el.variant = 'danger'` doesn't trigger re-render
4. **Forgetting `disconnectedCallback`** — Event listeners accumulate and leak memory
5. **Using `innerHTML` without sanitization** — Potential XSS if slot content includes user input
6. **Not handling re-entrancy** — `attributeChangedCallback` fires during `connectedCallback`, causing double renders
7. **Shadow DOM prevents global style overrides** — Consumer CSS cannot restyle internal elements without `::part`
8. **Slot timing issues** — Accessing slotted content before `slotchange` event fires returns empty

## Accessibility Considerations

The button element inside Shadow DOM inherits native button accessibility. Ensure focus delegation works by calling `focus()` on the host element. Use `role="button"` on the host if the shadow root doesn't contain a native button. Support `aria-label`, `aria-disabled`, and `aria-pressed` attributes by reflecting them to the internal button element.

## Responsive Behavior

Web component buttons inherit Bootstrap's responsive sizing through utility classes applied via attributes. Use the `size` attribute to switch between `btn-sm` and `btn-lg` at different breakpoints by observing a `ResizeObserver` on the host. For mobile-first designs, default to `btn-sm` and upgrade to standard size on `md+` viewports.
