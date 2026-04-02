---
title: "Template Element Patterns"
description: "Using HTML <template> elements with Bootstrap for lazy instantiation, clone patterns, and deferred rendering"
difficulty: 2
tags: [template-element, lazy-instantiation, clone-node, bootstrap, performance]
prerequisites:
  - 09_01_01_Custom_Element_Button
---

## Overview

The `<template>` element holds inert HTML that the browser does not render, parse images from, or execute scripts from until explicitly activated. Combined with Bootstrap, templates enable deferred component instantiation — define a card, modal, or table row once in a `<template>`, then clone and populate it programmatically as needed. This avoids innerHTML parsing overhead and prevents XSS from string concatenation.

Three clone patterns dominate: `cloneNode(true)` for simple duplication, `document.importNode()` for cross-document cloning, and structured cloning via the `content` property. Lazy template instantiation defers rendering until the element scrolls into view or an event fires, reducing initial page weight.

## Basic Implementation

```html
<template id="alert-template">
  <div class="alert alert-dismissible fade show" role="alert" part="alert">
    <strong class="alert-heading" part="heading"></strong>
    <span class="alert-body" part="body"></span>
    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close" part="close"></button>
  </div>
</template>

<div id="alert-container"></div>
<button id="add-alert">Add Alert</button>
```

```js
function createAlert(heading, message, variant = 'info') {
  const template = document.getElementById('alert-template');
  const clone = template.content.cloneNode(true);
  const alert = clone.querySelector('.alert');

  alert.classList.add(`alert-${variant}`);
  alert.querySelector('.alert-heading').textContent = heading;
  alert.querySelector('.alert-body').textContent = message;

  document.getElementById('alert-container').appendChild(clone);
  return alert;
}

document.getElementById('add-alert').addEventListener('click', () => {
  createAlert('Success!', 'Operation completed.', 'success');
});
```

```css
/* Template content is inert until cloned — no styling needed until activation */
#alert-container .alert {
  animation: slideIn 0.3s ease-out;
}

@keyframes slideIn {
  from { opacity: 0; transform: translateY(-10px); }
  to { opacity: 1; transform: translateY(0); }
}
```

## Advanced Variations

Integrate templates with web components for a declarative API:

```js
class BsAlert extends HTMLElement {
  static get template() {
    if (!this._template) {
      this._template = document.createElement('template');
      this._template.innerHTML = `
        <div class="alert alert-dismissible fade show" role="alert">
          <strong class="alert-heading"></strong>
          <p class="mb-0"></p>
          <button type="button" class="btn-close" aria-label="Close"></button>
        </div>
      `;
    }
    return this._template;
  }

  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    const clone = BsAlert.template.content.cloneNode(true);
    this.shadowRoot.appendChild(clone);
  }

  connectedCallback() {
    const variant = this.getAttribute('variant') || 'info';
    this.shadowRoot.querySelector('.alert').classList.add(`alert-${variant}`);
    this.shadowRoot.querySelector('.alert-heading').textContent = this.getAttribute('heading') || '';
    this.shadowRoot.querySelector('p').textContent = this.textContent;
    this.shadowRoot.querySelector('.btn-close').addEventListener('click', () => this.remove());
  }
}

customElements.define('bs-alert', BsAlert);
```

Use `<template>` for lazy image galleries — define cards with placeholder images, populate `src` on intersection:

```js
const observer = new IntersectionObserver((entries) => {
  entries.forEach(entry => {
    if (entry.isIntersecting) {
      const card = entry.target;
      const img = card.querySelector('img[data-src]');
      if (img) { img.src = img.dataset.src; img.removeAttribute('data-src'); }
      observer.unobserve(card);
    }
  });
});
```

## Best Practices

1. Always use `template.content.cloneNode(true)` — never `innerHTML` on the template element itself.
2. Prefer `<template>` over string-based HTML generation to prevent XSS vulnerabilities.
3. Use `document.importNode(template.content, true)` when cloning across documents (e.g., iframes).
4. Store templates in `<head>` or hidden containers to avoid accidental rendering.
5. Populate cloned content via `querySelector` + `textContent` — never `innerHTML` on cloned nodes.
6. Use templates for repeated structures: table rows, card grids, list items.
7. Combine with `IntersectionObserver` for lazy instantiation on scroll.
8. Cache the template reference (`document.getElementById`) to avoid repeated DOM lookups.
9. Use `adoptedStyleSheets` alongside templates for efficient style sharing.
10. Test cloning performance with large template sets (1000+ clones).
11. Name templates with descriptive IDs (`alert-template`, `card-template`).
12. Remove unused cloned elements to free memory.

## Common Pitfalls

1. **Scripts don't execute** — `<script>` inside `<template>` is inert; you must manually execute it after cloning.
2. **Images still load** — `<img src>` inside templates may still trigger downloads in some browsers; use `data-src`.
3. **`cloneNode` vs `importNode`** — `cloneNode` is same-document only; `importNode` is required for cross-document scenarios.
4. **Shallow clone missing content** — `cloneNode(false)` copies only the element, not its children; always use `true`.
5. **Template pollution** — Modifying template content after cloning affects all future clones.
6. **No scoped styles** — Template content shares the document's global CSS; use web component shadow DOM for isolation.

## Accessibility Considerations

Ensure cloned elements include proper ARIA attributes. If the template has `role="alert"`, the clone inherits it. Populate `aria-label` and `aria-describedby` programmatically after cloning. Announce dynamically inserted alerts with `aria-live="polite"`.

## Responsive Behavior

Template-cloned Bootstrap components inherit responsive behavior from Bootstrap's global CSS. No special handling needed unless using Shadow DOM (see 09_01_03).
