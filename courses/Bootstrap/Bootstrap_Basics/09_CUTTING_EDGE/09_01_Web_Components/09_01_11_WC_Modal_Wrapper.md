---
title: Web Component Modal Wrapper
category: [Web Components, Cutting Edge]
difficulty: 3
time: 30 min
tags: bootstrap5, web-components, custom-elements, modal, shadow-dom
---

## Overview

A modal web component encapsulates Bootstrap's modal dialog as a custom element with imperative `show()`/`hide()` methods, event dispatching, and slot-based content. This enables framework-agnostic modals that work with any JavaScript stack while preserving Bootstrap's animation, backdrop, and focus-trap behavior.

## Basic Implementation

```html
<bs-modal id="confirmModal" title="Confirm Action">
  <p>Are you sure you want to delete this item?</p>
  <span slot="footer">
    <button class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
    <button class="btn btn-danger" id="confirmBtn">Delete</button>
  </span>
</bs-modal>

<button class="btn btn-danger" onclick="document.getElementById('confirmModal').show()">
  Delete Item
</button>

<script>
class BsModal extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._isOpen = false;
  }

  connectedCallback() {
    this.render();
    this._setupListeners();
  }

  render() {
    const title = this.getAttribute('title') || 'Modal';
    const size = this.getAttribute('size') || '';

    this.shadowRoot.innerHTML = `
      <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
      <div class="modal fade" tabindex="-1">
        <div class="modal-dialog ${size ? 'modal-' + size : ''}">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title">${title}</h5>
              <button type="button" class="btn-close" aria-label="Close"></button>
            </div>
            <div class="modal-body">
              <slot></slot>
            </div>
            <div class="modal-footer">
              <slot name="footer"></slot>
            </div>
          </div>
        </div>
      </div>
      <div class="modal-backdrop fade" style="display:none"></div>
    `;
  }

  show() {
    this._isOpen = true;
    const modal = this.shadowRoot.querySelector('.modal');
    const backdrop = this.shadowRoot.querySelector('.modal-backdrop');
    modal.style.display = 'block';
    backdrop.style.display = 'block';
    requestAnimationFrame(() => {
      modal.classList.add('show');
      backdrop.classList.add('show');
    });
    document.body.style.overflow = 'hidden';
    this.dispatchEvent(new CustomEvent('bs-modal-shown', { bubbles: true, composed: true }));
  }

  hide() {
    this._isOpen = false;
    const modal = this.shadowRoot.querySelector('.modal');
    const backdrop = this.shadowRoot.querySelector('.modal-backdrop');
    modal.classList.remove('show');
    backdrop.classList.remove('show');
    setTimeout(() => {
      modal.style.display = 'none';
      backdrop.style.display = 'none';
    }, 150);
    document.body.style.overflow = '';
    this.dispatchEvent(new CustomEvent('bs-modal-hidden', { bubbles: true, composed: true }));
  }

  _setupListeners() {
    this.shadowRoot.querySelector('.btn-close').addEventListener('click', () => this.hide());
    this.shadowRoot.querySelector('.modal-backdrop').addEventListener('click', () => this.hide());
  }
}
customElements.define('bs-modal', BsModal);
</script>
```

## Advanced Variations

Adding static methods for one-off dialogs and focus trapping.

```html
<script>
class BsModal extends HTMLElement {
  static alert(message, title = 'Alert') {
    const modal = document.createElement('bs-modal');
    modal.setAttribute('title', title);
    modal.innerHTML = `<p>${message}</p>`;
    modal.innerHTML += `<button slot="footer" class="btn btn-primary"
      onclick="this.closest('bs-modal').hide()">OK</button>`;
    document.body.appendChild(modal);
    modal.addEventListener('bs-modal-hidden', () => modal.remove(), { once: true });
    modal.show();
    return modal;
  }

  static confirm(message, title = 'Confirm') {
    return new Promise((resolve) => {
      const modal = document.createElement('bs-modal');
      modal.setAttribute('title', title);
      modal.innerHTML = `<p>${message}</p>`;
      modal.innerHTML += `
        <span slot="footer">
          <button class="btn btn-secondary" data-action="cancel">Cancel</button>
          <button class="btn btn-primary" data-action="confirm">OK</button>
        </span>`;
      document.body.appendChild(modal);
      modal.shadowRoot.querySelector('[data-action="cancel"]')
        .addEventListener('click', () => { modal.hide(); resolve(false); });
      modal.shadowRoot.querySelector('[data-action="confirm"]')
        .addEventListener('click', () => { modal.hide(); resolve(true); });
      modal.addEventListener('bs-modal-hidden', () => modal.remove(), { once: true });
      modal.show();
    });
  }

  _trapFocus() {
    const modal = this.shadowRoot.querySelector('.modal');
    const focusable = modal.querySelectorAll(
      'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])'
    );
    const first = focusable[0];
    const last = focusable[focusable.length - 1];

    modal.addEventListener('keydown', (e) => {
      if (e.key === 'Escape') this.hide();
      if (e.key !== 'Tab') return;
      if (e.shiftKey && document.activeElement === first) {
        e.preventDefault(); last.focus();
      } else if (!e.shiftKey && document.activeElement === last) {
        e.preventDefault(); first.focus();
      }
    });
  }
}
</script>

<!-- Usage -->
<script>
async function deleteItem() {
  const confirmed = await BsModal.confirm('Delete this item permanently?');
  if (confirmed) {
    await fetch('/api/items/1', { method: 'DELETE' });
    BsModal.alert('Item deleted successfully.');
  }
}
</script>
```

## Best Practices

1. Provide `show()` and `hide()` methods for imperative control
2. Dispatch `bs-modal-shown` and `bs-modal-hidden` custom events
3. Trap focus within the modal when open for keyboard accessibility
4. Restore focus to the triggering element when the modal closes
5. Support `size` attribute for `modal-sm`, `modal-lg`, `modal-xl` variants
6. Use named slots (`slot="footer"`) for flexible header/footer composition
7. Prevent body scroll when modal is open by setting `overflow: hidden`
8. Handle Escape key to close the modal
9. Clean up DOM nodes for static modal factories (`.alert()`, `.confirm()`)
10. Use `requestAnimationFrame` for smooth entrance animations
11. Provide static convenience methods for common dialog patterns
12. Remove backdrop and restore scroll state on `disconnectedCallback`

## Common Pitfalls

1. **No focus trap** — Tab navigation escapes the modal to background content
2. **Focus not restored** — Trigger element loses focus after modal closes
3. **Backdrop not removed** — Body remains dark/unclickable after modal removal
4. **Body scroll not restored** — Page stays locked after modal dismissal
5. **Static modals not removed from DOM** — Memory leaks from accumulating modal elements
6. **No keyboard dismiss** — Escape key does not close the modal
7. **Slot content timing** — Accessing slot content before `slotchange` returns empty
8. **Multiple modals stacking** — Z-index conflicts when opening modals from within modals

## Accessibility Considerations

Use `role="dialog"` and `aria-modal="true"` on the modal container. Set `aria-labelledby` pointing to the modal title. Ensure all interactive elements inside are focusable. Announce the modal opening to screen readers. Support keyboard-only dismissal via Escape and close button.

## Responsive Behavior

Use the `size` attribute to control modal width at different breakpoints. On small screens, modals should expand to near-full-width with margin padding. Apply `modal-fullscreen-sm-down` for full-screen modals on mobile. Ensure modal content scrolls vertically when it exceeds viewport height using `overflow-y: auto` on the modal body.
