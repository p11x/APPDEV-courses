---
title: "Lit + Bootstrap"
slug: "lit-bootstrap"
difficulty: 3
tags: ["bootstrap", "lit", "web-components", "shadow-dom", "custom-elements"]
prerequisites:
  - "06_04_01_React_Bootstrap"
  - "06_04_10_SolidJS_Bootstrap"
related:
  - "06_04_09_Qwik_Bootstrap"
  - "06_04_10_SolidJS_Bootstrap"
duration: "40 minutes"
---

# Lit + Bootstrap

## Overview

Lit provides a lightweight framework for building web components that encapsulate Bootstrap-styled UI elements. Web components created with Lit are framework-agnostic, reusable across React, Vue, Angular, or vanilla JS projects. The challenge with Bootstrap and Shadow DOM is CSS encapsulation, Bootstrap styles do not penetrate shadow roots by default. Solutions include Constructable Stylesheets, CSS custom properties, and injecting Bootstrap CSS into each shadow root. Lit's reactive properties and directives map naturally to Bootstrap component state.

## Basic Implementation

A Lit web component that wraps a Bootstrap card with encapsulated styling.

```bash
npm install lit bootstrap sass
```

```typescript
// src/components/bs-card.ts
import { LitElement, html, css } from 'lit';
import { customElement, property } from 'lit/decorators.js';
import bootstrapCSS from 'bootstrap/dist/css/bootstrap.min.css?inline';

@customElement('bs-card')
export class BsCard extends LitElement {
  @property() title = '';
  @property() variant: 'primary' | 'success' | 'warning' | 'danger' = 'primary';

  static styles = css`
    ${bootstrapCSS}
    :host { display: block; }
  `;

  render() {
    return html`
      <div class="card border-${this.variant}">
        <div class="card-header bg-${this.variant} bg-opacity-10">
          <h5 class="mb-0">${this.title}</h5>
        </div>
        <div class="card-body">
          <slot></slot>
        </div>
        <div class="card-footer text-muted">
          <slot name="footer"></slot>
        </div>
      </div>
    `;
  }
}
```

```html
<!-- Usage -->
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Lit + Bootstrap</title>
</head>
<body>
  <div class="container mt-4">
    <bs-card title="Hello from Lit" variant="primary">
      <p>This content is inside a Lit web component with Bootstrap styling.</p>
      <span slot="footer">Built with Lit + Bootstrap</span>
    </bs-card>
  </div>
  <script type="module" src="./components/bs-card.js"></script>
</body>
</html>
```

## Advanced Variations

### Bootstrap Modal Web Component

A fully encapsulated modal component using Lit and Bootstrap JavaScript.

```typescript
// src/components/bs-modal.ts
import { LitElement, html, css } from 'lit';
import { customElement, property, query } from 'lit/decorators.js';

@customElement('bs-modal')
export class BsModal extends LitElement {
  @property() title = 'Modal';
  @property({ type: Boolean, reflect: true }) open = false;
  @property() size: 'sm' | 'lg' | 'xl' | '' = '';

  @query('.modal') modalEl!: HTMLDivElement;
  private bsModal: any;

  static styles = css`
    :host { display: contents; }
  `;

  async updated(changed: Map<string, any>) {
    if (changed.has('open')) {
      const { Modal } = await import('bootstrap/js/dist/modal');
      if (!this.bsModal) {
        this.bsModal = new Modal(this.modalEl);
        this.modalEl.addEventListener('hidden.bs.modal', () => {
          this.open = false;
          this.dispatchEvent(new CustomEvent('bs-close'));
        });
      }
      if (this.open) {
        this.bsModal.show();
      } else {
        this.bsModal.hide();
      }
    }
  }

  disconnectedCallback() {
    super.disconnectedCallback();
    this.bsModal?.dispose();
  }

  render() {
    return html`
      <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5/dist/css/bootstrap.min.css">
      <div class="modal fade" tabindex="-1">
        <div class="modal-dialog ${this.size ? `modal-${this.size}` : ''}">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title">${this.title}</h5>
              <button type="button" class="btn-close" @click=${() => this.open = false}></button>
            </div>
            <div class="modal-body">
              <slot></slot>
            </div>
            <div class="modal-footer">
              <slot name="footer">
                <button type="button" class="btn btn-secondary" @click=${() => this.open = false}>Close</button>
              </slot>
            </div>
          </div>
        </div>
      </div>
    `;
  }
}
```

```html
<button class="btn btn-primary" id="openModal">Open Modal</button>
<bs-modal title="Lit Modal" size="lg">
  <p>Content inside a web component modal.</p>
  <span slot="footer">
    <button class="btn btn-secondary">Cancel</button>
    <button class="btn btn-primary">Save</button>
  </span>
</bs-modal>

<script>
document.getElementById('openModal').addEventListener('click', () => {
  document.querySelector('bs-modal').open = true;
});
</script>
```

### Reactive Bootstrap Badge Counter

A notification badge component with Lit reactive properties.

```typescript
// src/components/bs-badge-counter.ts
import { LitElement, html, css } from 'lit';
import { customElement, property } from 'lit/decorators.js';
import { classMap } from 'lit/directives/class-map.js';

@customElement('bs-badge-counter')
export class BsBadgeCounter extends LitElement {
  @property({ type: Number }) count = 0;
  @property() variant: 'primary' | 'danger' | 'warning' = 'danger';
  @property({ type: Number }) max = 99;

  static styles = css`
    :host { display: inline-block; position: relative; }
    .badge { position: absolute; top: -8px; right: -8px; font-size: 0.7rem; min-width: 18px; height: 18px; line-height: 18px; padding: 0 4px; }
  `;

  render() {
    const displayCount = this.count > this.max ? `${this.max}+` : this.count.toString();
    const classes = { badge: true, [`bg-${this.variant}`]: true, 'd-none': this.count === 0 };

    return html`
      <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5/dist/css/bootstrap.min.css">
      <slot></slot>
      <span class=${classMap(classes)}>${displayCount}</span>
    `;
  }
}
```

```html
<bs-badge-counter count="5" variant="danger">
  <button class="btn btn-outline-secondary">
    <i class="bi bi-bell"></i> Notifications
  </button>
</bs-badge-counter>

<script>
  const badge = document.querySelector('bs-badge-counter');
  setInterval(() => { badge.count++; }, 3000);
</script>
```

### Bootstrap Toast Container

```typescript
// src/components/bs-toast-container.ts
import { LitElement, html, css } from 'lit';
import { customElement, state } from 'lit/decorators.js';
import { repeat } from 'lit/directives/repeat.js';

interface Toast { id: string; title: string; message: string; variant: string; }

@customElement('bs-toast-container')
export class BsToastContainer extends LitElement {
  @state() private toasts: Toast[] = [];

  static styles = css`
    :host { position: fixed; bottom: 1rem; right: 1rem; z-index: 1080; }
  `;

  addToast(title: string, message: string, variant = 'info') {
    const id = crypto.randomUUID();
    this.toasts = [...this.toasts, { id, title, message, variant }];
    setTimeout(() => this.removeToast(id), 5000);
  }

  removeToast(id: string) {
    this.toasts = this.toasts.filter(t => t.id !== id);
  }

  render() {
    return html`
      <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5/dist/css/bootstrap.min.css">
      ${repeat(this.toasts, t => t.id, t => html`
        <div class="toast show" role="alert">
          <div class="toast-header bg-${t.variant} bg-opacity-10">
            <strong class="me-auto">${t.title}</strong>
            <button type="button" class="btn-close" @click=${() => this.removeToast(t.id)}></button>
          </div>
          <div class="toast-body">${t.message}</div>
        </div>
      `)}
    `;
  }
}
```

## Best Practices

1. Inject Bootstrap CSS into shadow roots using inline styles or `<link>` tags within the component
2. Use Constructable Stylesheets (`adoptedStyleSheets`) for shared CSS across components
3. Expose CSS custom properties from the host element for theme customization
4. Import Bootstrap JavaScript dynamically within components, not globally
5. Use `slot` elements for flexible content projection
6. Dispose Bootstrap component instances in `disconnectedCallback`
7. Use `reflect: true` on properties that should update HTML attributes
8. Keep web components focused on single responsibilities
9. Use Lit directives (`classMap`, `repeat`, `ifDefined`) for conditional rendering
10. Test components in isolation with tools like Storybook
11. Document custom element attributes and events for consumers
12. Use `@state()` for internal reactive state that should not be exposed as attributes
13. Prefix custom element names with a namespace (e.g., `bs-`) to avoid conflicts
14. Provide fallback content inside `<slot>` elements for graceful degradation

## Common Pitfalls

1. **CSS not reaching Shadow DOM**: Bootstrap styles not applying because of encapsulation
2. **Bootstrap JS scope**: Bootstrap JavaScript targeting elements outside the shadow root
3. **Event bubbling**: Custom events not crossing shadow DOM boundaries without `composed: true`
4. **Attribute reflection**: Forgetting `reflect: true` means attribute changes do not sync with properties
5. **Memory leaks**: Not disposing Bootstrap instances in `disconnectedCallback`
6. **Global style conflicts**: Including Bootstrap CSS globally while also injecting it in shadow roots
7. **Slot changes**: Not handling `slotchange` events for dynamic slot content

## Accessibility Considerations

Ensure web components expose proper ARIA roles and attributes. Use `role` and `aria-*` attributes on the host element with `static properties` for accessibility tree integration. Test that screen readers can navigate through shadow DOM boundaries. Use `delegatesFocus: true` in shadow root options for proper focus management. Ensure slotted content maintains its semantic meaning within the component. Provide `aria-label` on interactive custom elements. Test with NVDA and VoiceOver to verify shadow DOM content is announced.

```typescript
static properties = {
  role: { reflect: true },
  'aria-label': { reflect: true },
};

// Set defaults
connectedCallback() {
  super.connectedCallback();
  if (!this.role) this.role = 'region';
}
```

## Responsive Behavior

Bootstrap's responsive CSS must be injected into each shadow root for responsive behavior to work. Include the full Bootstrap CSS or at minimum the grid and responsive utility sections. Use CSS custom properties to pass responsive breakpoint values from the host page. Test responsive layouts by resizing the viewport and verifying shadow DOM content adapts. Consider using CSS `@import` within shadow root stylesheets to pull in responsive styles. Verify that `d-*` responsive display utilities work correctly within shadow roots.
