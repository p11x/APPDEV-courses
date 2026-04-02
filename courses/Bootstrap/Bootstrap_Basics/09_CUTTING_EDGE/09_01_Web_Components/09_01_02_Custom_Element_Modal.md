---
title: "Custom Element Modal"
description: "Building a Bootstrap modal as a web component with declarative API and event dispatching"
difficulty: 3
tags: [web-components, custom-elements, modal, bootstrap-modal, events]
prerequisites:
  - 09_01_01_Custom_Element_Button
---

## Overview

A `<bs-modal>` custom element wraps Bootstrap's modal dialog, exposing a declarative HTML API (`open`, `close` attributes), programmatic methods (`show()`, `hide()`, `toggle()`), and custom events (`bs:show`, `bs:shown`, `bs:hide`, `bs:hidden`). Shadow DOM isolates the modal backdrop, animation timing, and internal structure from the host page's styles.

The real power of a modal web component is encapsulating Bootstrap's complex modal logic — backdrop management, focus trap, ESC key handling, scroll lock — behind a simple element that any framework or vanilla HTML can consume. The component manages its own lifecycle, dispatches typed custom events with detail payloads, and exposes reactive properties that reflect to attributes.

## Basic Implementation

```html
<bs-modal id="confirmModal" title="Confirm Action">
  <span slot="body">Are you sure you want to proceed?</span>
  <span slot="footer">
    <button data-bs-dismiss="modal">Cancel</button>
    <button data-bs-action="confirm">Confirm</button>
  </span>
</bs-modal>

<button onclick="document.getElementById('confirmModal').show()">
  Open Modal
</button>
```

```js
class BsModal extends HTMLElement {
  static get observedAttributes() {
    return ['open', 'title', 'centered', 'size', 'static-backdrop'];
  }

  constructor() {
    super();
    this.attachShadow({ mode: 'open', delegatesFocus: true });
    this.shadowRoot.innerHTML = `
      <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
      <div class="modal fade" part="modal" tabindex="-1">
        <div class="modal-dialog" part="dialog">
          <div class="modal-content" part="content">
            <div class="modal-header" part="header">
              <h5 class="modal-title" part="title"><slot name="title"></slot></h5>
              <button type="button" class="btn-close" part="close-btn" aria-label="Close"></button>
            </div>
            <div class="modal-body" part="body"><slot name="body"></slot></div>
            <div class="modal-footer" part="footer"><slot name="footer"></slot></div>
          </div>
        </div>
      </div>
      <div class="modal-backdrop fade" part="backdrop"></div>
    `;
    this._modal = this.shadowRoot.querySelector('.modal');
    this._backdrop = this.shadowRoot.querySelector('.modal-backdrop');
    this._bindEvents();
  }

  _bindEvents() {
    this.shadowRoot.querySelector('.btn-close').addEventListener('click', () => this.hide());
    this._backdrop.addEventListener('click', () => {
      if (!this.hasAttribute('static-backdrop')) this.hide();
    });
    document.addEventListener('keydown', (e) => {
      if (e.key === 'Escape' && this.hasAttribute('open')) this.hide();
    });
  }

  show() {
    this.dispatchEvent(new CustomEvent('bs:show', { bubbles: true }));
    this.setAttribute('open', '');
    this._modal.classList.add('show');
    this._modal.style.display = 'block';
    this._backdrop.classList.add('show');
    document.body.style.overflow = 'hidden';
    this._modal.focus();
    setTimeout(() => this.dispatchEvent(new CustomEvent('bs:shown', { bubbles: true })), 150);
  }

  hide() {
    this.dispatchEvent(new CustomEvent('bs:hide', { bubbles: true }));
    this.removeAttribute('open');
    this._modal.classList.remove('show');
    this._backdrop.classList.remove('show');
    document.body.style.overflow = '';
    setTimeout(() => {
      this._modal.style.display = 'none';
      this.dispatchEvent(new CustomEvent('bs:hidden', { bubbles: true }));
    }, 150);
  }

  toggle() { this.hasAttribute('open') ? this.hide() : this.show(); }

  attributeChangedCallback(name, old, val) {
    if (name === 'open' && val !== null && old === null) this.show();
    if (name === 'open' && val === null && old !== null) this.hide();
  }
}

customElements.define('bs-modal', BsModal);
```

```css
/* External theming via ::part */
bs-modal::part(dialog) {
  max-width: 600px;
}

bs-modal::part(header) {
  background: var(--bs-primary);
  color: white;
}
```

## Advanced Variations

Integrate with Bootstrap's JS Modal class for animation support. Use the light DOM fallback pattern where a standard Bootstrap modal markup exists and the custom element upgrades it.

## Best Practices

1. Dispatch granular lifecycle events (`bs:show`, `bs:shown`, `bs:hide`, `bs:hidden`) matching Bootstrap's naming.
2. Use `delegatesFocus: true` so focus transfers to the first focusable element inside the modal.
3. Implement ESC key and backdrop click dismissal by default; opt out with `static-backdrop`.
4. Lock body scroll when modal is open; restore on close.
5. Use named slots (`slot="body"`, `slot="footer"`) for flexible content projection.
6. Reflect the `open` attribute for declarative state and CSS hooks.
7. Provide `show()`, `hide()`, and `toggle()` methods for programmatic control.
8. Trap focus within the modal while open to meet WCAG requirements.
9. Restore focus to the triggering element on close.
10. Use `part` attributes to expose internal elements for external styling.
11. Guard against multiple rapid open/close calls with a transition flag.
12. Support SSR by rendering a `<dialog>` or `<div>` fallback in light DOM.

## Common Pitfalls

1. **Backdrop z-index conflicts** — Shadow DOM creates a new stacking context; ensure the backdrop has sufficient z-index.
2. **Focus not trapped** — Without explicit focus trap logic, keyboard users can tab behind the modal.
3. **Body scroll not locked** — Forgetting `overflow: hidden` on `<body>` allows background scrolling.
4. **Animation timing** — Removing the element before the CSS transition completes causes visual glitches; use `setTimeout` matching the transition duration.
5. **Multiple modals** — Stacking modals requires managing backdrop and z-index layers; consider a modal manager.
6. **Slot content not reactive** — Light DOM slot changes don't trigger `attributeChangedCallback`; use `MutationObserver` if needed.

## Accessibility Considerations

Set `role="dialog"` and `aria-modal="true"` on the modal container. Use `aria-labelledby` pointing to the title and `aria-describedby` pointing to the body. Implement a focus trap that cycles through focusable elements and returns focus to the trigger on close.

## Responsive Behavior

Use Bootstrap's `modal-fullscreen`, `modal-lg`, `modal-sm` classes reflected via a `size` attribute. On small screens, automatically switch to fullscreen mode:

```css
@media (max-width: 576px) {
  bs-modal::part(dialog) {
    max-width: 100%;
    margin: 0;
    height: 100vh;
  }
}
```
