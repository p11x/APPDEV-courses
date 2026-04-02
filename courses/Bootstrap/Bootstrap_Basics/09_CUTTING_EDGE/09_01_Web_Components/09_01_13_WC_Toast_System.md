---
title: Web Component Toast System
category: [Web Components, Cutting Edge]
difficulty: 3
time: 30 min
tags: bootstrap5, web-components, custom-elements, toast, notifications
---

## Overview

A toast manager web component provides a centralized notification system using custom elements. It handles stacking, auto-dismiss timers, queue management, and multiple toast types (success, warning, error, info) while wrapping Bootstrap's toast styles inside an encapsulated Shadow DOM container.

## Basic Implementation

```html
<bs-toast-container position="top-end"></bs-toast-container>

<script>
class BsToastContainer extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._queue = [];
    this._maxVisible = 5;
  }

  connectedCallback() {
    const position = this.getAttribute('position') || 'top-end';
    const [vert, horiz] = position.split('-');

    this.shadowRoot.innerHTML = `
      <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
      <style>
        :host {
          position: fixed; z-index: 1080;
          ${vert === 'top' ? 'top: 1rem; bottom: auto;' : 'bottom: 1rem; top: auto;'}
          ${horiz === 'end' ? 'right: 1rem; left: auto;' : 'left: 1rem; right: auto;'}
        }
        .toast-stack { display: flex; flex-direction: column; gap: 0.5rem; }
      </style>
      <div class="toast-stack" role="log" aria-live="polite"></div>
    `;
  }

  add(message, options = {}) {
    const { type = 'info', duration = 5000, title = '' } = options;
    const toast = document.createElement('bs-toast');
    toast.setAttribute('message', message);
    toast.setAttribute('type', type);
    toast.setAttribute('duration', duration);
    if (title) toast.setAttribute('title', title);
    this.shadowRoot.querySelector('.toast-stack').appendChild(toast);
  }
}
customElements.define('bs-toast-container', BsToastContainer);

class BsToast extends HTMLElement {
  connectedCallback() {
    const type = this.getAttribute('type') || 'info';
    const message = this.getAttribute('message') || '';
    const title = this.getAttribute('title') || '';
    const duration = parseInt(this.getAttribute('duration') || '5000');

    const colors = {
      success: 'text-bg-success',
      danger: 'text-bg-danger',
      warning: 'text-bg-warning',
      info: 'text-bg-info'
    };

    this.shadowRoot.innerHTML = `
      <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
      <div class="toast show ${colors[type] || colors.info}" role="alert">
        <div class="toast-header">
          <strong class="me-auto">${title || type.charAt(0).toUpperCase() + type.slice(1)}</strong>
          <button type="button" class="btn-close" aria-label="Close"></button>
        </div>
        <div class="toast-body">${message}</div>
      </div>
    `;

    this.shadowRoot.querySelector('.btn-close').addEventListener('click', () => this.dismiss());
    if (duration > 0) setTimeout(() => this.dismiss(), duration);
  }

  dismiss() {
    const toast = this.shadowRoot.querySelector('.toast');
    toast.classList.remove('show');
    setTimeout(() => {
      this.dispatchEvent(new CustomEvent('bs-toast-dismissed', { bubbles: true, composed: true }));
      this.remove();
    }, 300);
  }
}
customElements.define('bs-toast', BsToast);
</script>

<!-- Usage -->
<button class="btn btn-success" onclick="document.querySelector('bs-toast-container').add('File saved!', {type:'success', title:'Success'})">
  Show Toast
</button>
```

## Advanced Variations

Adding queue management with stacking limits and programmatic API.

```html
<script>
class BsToastContainer extends HTMLElement {
  add(message, options = {}) {
    const visible = this.shadowRoot.querySelectorAll('bs-toast').length;
    if (visible >= this._maxVisible) {
      this._queue.push({ message, options });
      return;
    }
    this._createToast(message, options);
  }

  _createToast(message, options) {
    const toast = document.createElement('bs-toast');
    toast.setAttribute('message', message);
    toast.setAttribute('type', options.type || 'info');
    toast.setAttribute('duration', options.duration ?? 5000);
    if (options.title) toast.setAttribute('title', options.title);

    toast.addEventListener('bs-toast-dismissed', () => {
      if (this._queue.length > 0) {
        const next = this._queue.shift();
        this._createToast(next.message, next.options);
      }
    });

    this.shadowRoot.querySelector('.toast-stack').appendChild(toast);
  }

  clear() {
    this.shadowRoot.querySelectorAll('bs-toast').forEach(t => t.dismiss());
    this._queue = [];
  }

  get count() {
    return this.shadowRoot.querySelectorAll('bs-toast').length + this._queue.length;
  }
}
</script>
```

Adding progress bar countdown for timed toasts.

```html
<script>
class BsToast extends HTMLElement {
  connectedCallback() {
    const duration = parseInt(this.getAttribute('duration') || '5000');

    this.shadowRoot.innerHTML = `
      <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
      <div class="toast show" role="alert">
        <div class="toast-header">
          <strong class="me-auto">${this.getAttribute('title') || 'Notification'}</strong>
          <button type="button" class="btn-close" aria-label="Close"></button>
        </div>
        <div class="toast-body">${this.getAttribute('message')}</div>
        ${duration > 0 ? `<div class="progress" style="height:3px">
          <div class="progress-bar" role="progressbar" style="width:100%"></div>
        </div>` : ''}
      </div>
    `;

    if (duration > 0) {
      const bar = this.shadowRoot.querySelector('.progress-bar');
      bar.style.transition = `width ${duration}ms linear`;
      requestAnimationFrame(() => { bar.style.width = '0%'; });
      setTimeout(() => this.dismiss(), duration);
    }
  }
}
</script>
```

## Best Practices

1. Use a single container element positioned fixed at a viewport corner
2. Support `position` attribute: `top-end`, `top-start`, `bottom-end`, `bottom-start`
3. Implement queue management to limit visible toasts and buffer overflow
4. Dispatch `bs-toast-dismissed` event when a toast is removed
5. Provide a `clear()` method to dismiss all toasts at once
6. Use `aria-live="polite"` on the toast stack for screen reader announcements
7. Support configurable auto-dismiss duration with `duration` attribute
8. Add progress bar countdown for visual feedback on timed toasts
9. Color-code toast types using Bootstrap's contextual background classes
10. Allow stacking order control (newest on top or bottom)
11. Provide a `count` getter for monitoring toast queue state
12. Clean up timers in `disconnectedCallback` to prevent stale timeouts

## Common Pitfalls

1. **No queue limit** — Unbounded toasts overflow the viewport
2. **Timers not cleaned up** — Dismissed toasts still trigger `remove()` after being gone
3. **Missing `aria-live`** — Screen readers don't announce toast messages
4. **No visual type distinction** — All toasts look identical regardless of severity
5. **Container not fixed** — Toasts scroll with page content instead of staying visible
6. **Shadow DOM blocking global styles** — Custom toast CSS from the app doesn't apply
7. **Race condition on rapid adds** — Queue state gets out of sync with visible count
8. **No pause on hover** — Timed toasts dismiss while user is reading them

## Accessibility Considerations

Set `role="alert"` or `role="status"` on toast elements. Use `aria-live="polite"` on the container for non-critical toasts and `aria-live="assertive"` for errors. Ensure close buttons have accessible labels. Allow sufficient dismiss duration based on message length. Support keyboard dismissal via Escape.

## Responsive Behavior

Position toasts at full-width on small screens using `start-0 end-0` and removing fixed horizontal offsets. Reduce maximum visible toasts on mobile to prevent screen takeover. Use smaller toast padding and font sizes at narrow viewports. Ensure toasts don't overlap critical mobile UI like bottom navigation bars.
