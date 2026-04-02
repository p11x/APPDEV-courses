---
title: "Shadow DOM Styling"
description: "Styling Bootstrap components inside Shadow DOM using CSS custom properties, ::part, and style encapsulation strategies"
difficulty: 3
tags: [shadow-dom, css-custom-properties, part-selector, bootstrap-theming]
prerequisites:
  - 09_01_01_Custom_Element_Button
  - 09_01_02_Custom_Element_Modal
---

## Overview

Shadow DOM provides true style encapsulation — global Bootstrap CSS cannot reach into a shadow tree, and shadow tree styles cannot leak out. This is both a feature and a challenge. You get guaranteed isolation (no class name collisions, no unintended overrides), but you lose the convenience of Bootstrap's global stylesheet.

Three strategies solve this: (1) embedding a `<link>` or `<style>` inside each shadow root, (2) forwarding CSS custom properties from the host into the shadow tree, and (3) exposing internal elements via the `::part()` pseudo-element. Bootstrap 5's increasing use of CSS custom properties (e.g., `--bs-primary`, `--bs-body-bg`) makes strategy (2) particularly effective, as many theme values automatically penetrate shadow boundaries.

## Basic Implementation

```html
<bs-card variant="primary">
  <span slot="header">Card Title</span>
  <p slot="body">Content inside shadow DOM.</p>
</bs-card>
```

```js
class BsCard extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this.shadowRoot.innerHTML = `
      <style>
        /* Embedded Bootstrap scope — only loads inside shadow */
        @import url('https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css');

        :host {
          display: block;
        }

        /* Forward host custom properties */
        .card {
          --bs-card-bg: var(--card-bg, var(--bs-body-bg));
          --bs-card-border-color: var(--card-border, var(--bs-border-color));
        }
      </style>
      <div class="card" part="card">
        <div class="card-header" part="header"><slot name="header"></slot></div>
        <div class="card-body" part="body"><slot name="body"></slot></div>
        <div class="card-footer" part="footer"><slot name="footer"></slot></div>
      </div>
    `;
  }
}

customElements.define('bs-card', BsCard);
```

```css
/* External: CSS custom properties penetrate shadow DOM */
bs-card {
  --card-bg: #f8f9fa;
  --card-border: #dee2e6;
  border-radius: 1rem;
}

/* ::part() reaches exposed internal elements */
bs-card::part(card) {
  box-shadow: 0 4px 6px rgba(0,0,0,0.1);
}

bs-card::part(header) {
  font-weight: 700;
  text-transform: uppercase;
}
```

## Advanced Variations

Use adopted stylesheets (`CSSStyleSheet`) to share a single stylesheet across multiple shadow roots without duplicating `<link>` tags:

```js
const sharedSheet = new CSSStyleSheet();
sharedSheet.replaceSync(`
  @import url('https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css');
`);

class BsCard extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this.shadowRoot.adoptedStyleSheets = [sharedSheet];
  }
}
```

This dramatically reduces memory when you have hundreds of instances. You can also layer additional sheets for component-specific overrides.

## Best Practices

1. Use `@import` or `<link>` inside shadow root for Bootstrap CSS; do not assume global styles penetrate.
2. Forward Bootstrap CSS custom properties (`--bs-*`) from the host element into the shadow tree.
3. Expose key internal elements with `part="name"` for external `::part()` styling.
4. Use `adoptedStyleSheets` to share styles across shadow roots efficiently.
5. Avoid `!important` in shadow styles; it creates cascade wars with external `::part()` overrides.
6. Use `:host` and `:host-context()` for conditional styling based on parent context.
7. Document every `part` name in your component's API.
8. Prefer CSS custom properties over `::part()` for theming (more composable, cascade-friendly).
9. Test with both light and dark Bootstrap themes to verify custom property forwarding.
10. Use `:host([attribute])` for state-based styling (e.g., `:host([variant="danger"])`).
11. Avoid `@import` in production — bundle Bootstrap CSS as an inlined string or use constructable stylesheets.
12. Keep shadow CSS minimal; delegate layout to light DOM where possible.

## Common Pitfalls

1. **`@import` performance** — Each shadow root that uses `@import` triggers a separate network request; use constructable stylesheets or inlined CSS instead.
2. **Custom property not forwarded** — CSS custom properties only cascade into shadow DOM if explicitly applied on `:host` or an internal element.
3. **`::part()` single-level** — You cannot chain `::part()` selectors (e.g., `::part(header) ::part(title)` is invalid).
4. **`:host-context()` browser support** — Not supported in Firefox; avoid for cross-browser code.
5. **Duplicate style loading** — Without `adoptedStyleSheets`, 100 card instances load Bootstrap 100 times.
6. **Specificity surprises** — External `::part()` styles have low specificity; internal styles may override them unintentionally.

## Accessibility Considerations

Shadow DOM does not affect ARIA. Screen readers traverse the flattened DOM tree (light + shadow). Ensure `role`, `aria-*`, and semantic HTML are correct inside the shadow template. Use `<slot>` to project accessible light DOM content.

## Responsive Behavior

CSS media queries inside shadow DOM work normally — the shadow tree responds to viewport size. However, container queries require the shadow root element to be a containment context:

```css
:host {
  container-type: inline-size;
}

@container (min-width: 400px) {
  .card { flex-direction: row; }
}
```
