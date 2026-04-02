---
title: Bootstrap as Web Components
category: Emerging Technologies
difficulty: 3
time: 35 min
tags: bootstrap5, web-components, custom-elements, shadow-dom, slots
---

## Overview

Wrapping Bootstrap components as Web Components creates framework-agnostic, encapsulated UI elements usable in any JavaScript environment. Custom Elements provide a native API for defining new HTML tags, while Shadow DOM isolates Bootstrap's styles from the host page. This approach enables Bootstrap component reuse across React, Vue, Angular, and vanilla JS projects without style conflicts.

## Basic Implementation

Define a custom element that internally renders a Bootstrap component.

```js
// Custom Bootstrap Alert Web Component
class BsAlert extends HTMLElement {
  static get observedAttributes() {
    return ['variant', 'dismissible', 'heading'];
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

  get variant() {
    return this.getAttribute('variant') || 'primary';
  }

  get dismissible() {
    return this.hasAttribute('dismissible');
  }

  get heading() {
    return this.getAttribute('heading') || '';
  }

  render() {
    const dismissBtn = this.dismissible
      ? `<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>`
      : '';

    const headingEl = this.heading
      ? `<h4 class="alert-heading">${this.heading}</h4>`
      : '';

    this.shadowRoot.innerHTML = `
      <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css">
      <div class="alert alert-${this.variant} ${this.dismissible ? 'alert-dismissible' : ''} fade show" role="alert">
        ${headingEl}
        <slot></slot>
        ${dismissBtn}
      </div>
    `;
  }
}

customElements.define('bs-alert', BsAlert);
```

```html
<!-- Usage in any HTML page -->
<bs-alert variant="success" dismissible heading="Success!">
  Your changes have been saved successfully.
</bs-alert>

<bs-alert variant="danger">
  An error occurred while processing your request.
</bs-alert>
```

## Advanced Variations

Complex Web Components with Shadow DOM require careful handling of Bootstrap's JavaScript, as scripts don't cross shadow boundaries automatically.

```js
// Bootstrap Modal Web Component with slot-based content
class BsModal extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._isOpen = false;
  }

  connectedCallback() {
    this.render();
    this.setupListeners();
  }

  static get observedAttributes() {
    return ['title', 'size', 'centered', 'static-backdrop'];
  }

  render() {
    const size = this.getAttribute('size') || '';
    const centered = this.hasAttribute('centered') ? 'modal-dialog-centered' : '';

    this.shadowRoot.innerHTML = `
      <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css">
      <style>
        .modal { display: none; }
        .modal.show { display: block; }
        .modal-backdrop { position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0,0,0,0.5); z-index: 1040; }
      </style>
      <div class="modal fade" tabindex="-1" aria-labelledby="modalTitle" aria-hidden="true">
        <div class="modal-dialog ${size} ${centered}">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title" id="modalTitle">
                <slot name="title">${this.getAttribute('title') || 'Modal'}</slot>
              </h5>
              <button type="button" class="btn-close" aria-label="Close" id="closeBtn"></button>
            </div>
            <div class="modal-body">
              <slot name="body"></slot>
            </div>
            <div class="modal-footer">
              <slot name="footer">
                <button type="button" class="btn btn-secondary" id="cancelBtn">Cancel</button>
                <button type="button" class="btn btn-primary" id="confirmBtn">Confirm</button>
              </slot>
            </div>
          </div>
        </div>
      </div>
    `;
  }

  setupListeners() {
    const backdrop = this.hasAttribute('static-backdrop') ? 'static' : true;

    this.shadowRoot.getElementById('closeBtn').addEventListener('click', () => this.hide());
    this.shadowRoot.getElementById('cancelBtn')?.addEventListener('click', () => this.hide());

    this.shadowRoot.querySelector('.modal').addEventListener('click', (e) => {
      if (e.target.classList.contains('modal') && backdrop !== 'static') {
        this.hide();
      }
    });
  }

  show() {
    const modal = this.shadowRoot.querySelector('.modal');
    const backdrop = document.createElement('div');
    backdrop.className = 'modal-backdrop fade show';
    document.body.appendChild(backdrop);
    document.body.classList.add('modal-open');
    modal.classList.add('show');
    modal.setAttribute('aria-hidden', 'false');
    this._isOpen = true;
    this.dispatchEvent(new CustomEvent('bs:modal:shown'));
  }

  hide() {
    const modal = this.shadowRoot.querySelector('.modal');
    document.querySelector('.modal-backdrop')?.remove();
    document.body.classList.remove('modal-open');
    modal.classList.remove('show');
    modal.setAttribute('aria-hidden', 'true');
    this._isOpen = false;
    this.dispatchEvent(new CustomEvent('bs:modal:hidden'));
  }

  disconnectedCallback() {
    document.querySelector('.modal-backdrop')?.remove();
    document.body.classList.remove('modal-open');
  }
}

customElements.define('bs-modal', BsModal);
```

```html
<!-- Web Component modal usage -->
<button class="btn btn-primary" onclick="document.querySelector('bs-modal').show()">
  Open Web Component Modal
</button>

<bs-modal title="User Details" size="modal-lg" centered>
  <span slot="body">
    <p>Content is projected via slots.</p>
    <div class="mb-3">
      <label for="wc-email" class="form-label">Email</label>
      <input type="email" class="form-control" id="wc-email">
    </div>
  </span>
</bs-modal>
```

## Best Practices

1. Use Shadow DOM (`mode: 'open'`) to isolate Bootstrap styles from the host page
2. Load Bootstrap CSS inside Shadow DOM via `<link>` tag for style encapsulation
3. Use `<slot>` elements for content projection to maintain flexibility
4. Handle Bootstrap JavaScript manually since it doesn't traverse Shadow DOM
5. Dispatch custom events (`CustomEvent`) to communicate state changes to the host page
6. Define `observedAttributes` for reactive attribute changes
7. Clean up event listeners and DOM state in `disconnectedCallback`
8. Prefix custom element names with `bs-` to avoid naming conflicts
9. Provide both imperative API (methods) and declarative API (attributes)
10. Test Web Components across browsers for Shadow DOM support consistency

## Common Pitfalls

1. **Style leakage** - Without Shadow DOM, Bootstrap styles conflict with host page styles. Always use Shadow DOM for isolation.
2. **JavaScript boundary issues** - Bootstrap's auto-initialization doesn't work inside Shadow DOM. Initialize components manually.
3. **CSS specificity conflicts** - Host page styles with `!important` can penetrate Shadow DOM. Use `:host` selectors defensively.
4. **Slot fallback content** - Slot fallback content is only rendered when no slotted content is provided. Don't rely on it for critical UI.
5. **Browser compatibility** - Custom Elements require ES2015+ browsers. Provide polyfills for legacy support if needed.

## Accessibility Considerations

Web Components must maintain Bootstrap's accessibility features. Ensure ARIA attributes are set on elements inside Shadow DOM, manage focus programmatically since `autofocus` doesn't work across shadow boundaries, forward keyboard events from Shadow DOM to the host, and announce dynamic content changes using `aria-live` regions inside the shadow root.

## Responsive Behavior

Bootstrap's responsive utilities loaded inside Shadow DOM apply correctly within the shadow tree. However, responsive behavior based on viewport width requires explicit media query handling within the component. Use `ResizeObserver` or `window.matchMedia` inside the Web Component to respond to viewport changes and adjust internal layout accordingly.
