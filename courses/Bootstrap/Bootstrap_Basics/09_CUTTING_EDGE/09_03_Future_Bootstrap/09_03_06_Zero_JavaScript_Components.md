---
title: "Zero-JavaScript Components"
description: "CSS-only modals, popovers, and tooltips using :popover-open, anchor positioning, and :has()"
difficulty: 3
tags: [css-only, popover-api, anchor-positioning, :has, zero-js, bootstrap-future]
prerequisites:
  - 06_02_Offcanvas
  - 09_02_08_CSS_Anchor_Positioning
---

## Overview

Zero-JavaScript components use native browser APIs — the Popover API, `:popover-open`, `dialog` element, CSS anchor positioning, and the `:has()` selector — to implement interactive UI without custom JavaScript. Bootstrap's modals, tooltips, dropdowns, and popovers currently require JavaScript for open/close logic, focus trapping, and positioning. CSS-only alternatives reduce bundle size, improve performance, and work when JavaScript is blocked or slow to load.

The Popover API (`popover="auto"`, `popovertarget`) handles show/hide, backdrop, and light-dismiss. The `dialog` element provides native modal behavior with focus trapping. CSS anchor positioning places floating elements relative to triggers. The `:has()` selector enables conditional styling based on child state.

## Basic Implementation

```html
<!-- CSS-only popover (no JavaScript) -->
<button popovertarget="my-popover" class="btn btn-primary">
  Toggle Popover
</button>
<div id="my-popover" popover class="popover-bs">
  <div class="popover-header">Popover Title</div>
  <div class="popover-body">This popover uses zero JavaScript.</div>
</div>

<!-- CSS-only modal (no JavaScript) -->
<dialog id="my-modal" class="modal-bs">
  <form method="dialog">
    <div class="modal-header">
      <h5>Modal Title</h5>
      <button class="btn-close" value="cancel"></button>
    </div>
    <div class="modal-body">
      <p>Native dialog element with built-in focus trap.</p>
    </div>
    <div class="modal-footer">
      <button value="cancel" class="btn btn-secondary">Close</button>
      <button value="confirm" class="btn btn-primary">Save</button>
    </div>
  </form>
</dialog>

<button onclick="document.getElementById('my-modal').showModal()" class="btn btn-primary">
  Open Modal
</button>

<!-- CSS-only dropdown with :has() -->
<div class="dropdown-bs">
  <button class="btn btn-secondary">Dropdown</button>
  <ul class="dropdown-menu-bs">
    <li><a href="#">Action</a></li>
    <li><a href="#">Another action</a></li>
    <li><a href="#">Something else</a></li>
  </ul>
</div>
```

```css
/* Popover styling */
.popover-bs {
  border: 1px solid var(--bs-border-color);
  border-radius: var(--bs-border-radius);
  box-shadow: var(--bs-box-shadow);
  padding: 0;
  inset: unset;
}

.popover-bs::backdrop {
  background: transparent;
}

/* Modal styling */
.modal-bs {
  border: none;
  border-radius: var(--bs-border-radius-lg);
  box-shadow: var(--bs-box-shadow-lg);
  max-width: 500px;
  padding: 0;
}

.modal-bs::backdrop {
  background: rgba(0, 0, 0, 0.5);
}

/* CSS-only dropdown with :has() */
.dropdown-bs {
  position: relative;
  display: inline-block;
}

.dropdown-menu-bs {
  display: none;
  position: absolute;
  top: 100%;
  left: 0;
  background: var(--bs-body-bg);
  border: 1px solid var(--bs-border-color);
  border-radius: var(--bs-border-radius);
  box-shadow: var(--bs-box-shadow);
  list-style: none;
  padding: 0.5rem 0;
  min-width: 200px;
}

/* Show dropdown when button is focused or menu is hovered */
.dropdown-bs:has(button:focus-visible) .dropdown-menu-bs,
.dropdown-bs:has(.dropdown-menu-bs:hover) .dropdown-menu-bs {
  display: block;
}
```

```js
// Feature detection — load JS fallback if needed
if (!HTMLElement.prototype.showPopover) {
  console.log('Popover API not supported, loading JS fallback');
  // import('./popover-fallback.js');
}

if (!HTMLDialogElement.prototype.showModal) {
  console.log('Dialog API not supported, loading JS fallback');
  // import('./modal-fallback.js');
}
```

## Advanced Variations

CSS-only tooltip with anchor positioning:

```css
button[title] {
  anchor-name: --tooltip-trigger;
}

.tooltip-css {
  position: fixed;
  position-anchor: --tooltip-trigger;
  position-area: top center;
  position-try-fallbacks: flip-block;
  opacity: 0;
  pointer-events: none;
  transition: opacity 0.15s;
}

button[title]:hover .tooltip-css {
  opacity: 1;
}
```

## Best Practices

1. Use `<dialog>` for modals — it provides focus trapping and backdrop natively.
2. Use `popover` attribute for popovers and dropdowns — it handles show/hide and light-dismiss.
3. Use `:has()` for conditional styling without JavaScript.
4. Use CSS anchor positioning for tooltip/popover placement.
5. Provide JavaScript fallback for unsupported browsers.
6. Use `method="dialog"` on forms inside `<dialog>` for native close behavior.
7. Style `::backdrop` for modal and popover overlays.
8. Use `popovertarget` attribute to link triggers to popovers.
9. Test keyboard navigation — native APIs provide basic keyboard support.
10. Use `:popover-open` pseudo-class for open-state styling.
11. Keep CSS-only components simple; complex interactions still need JavaScript.
12. Use `@supports` to detect API support in CSS.

## Common Pitfalls

1. **Browser support** — Popover API in Chrome 114+, Dialog in all modern browsers, `:has()` in Chrome 105+.
2. **`top-layer` behavior** — Popovers render in the top layer, above all other content; z-index doesn't apply.
3. **Focus management** — `<dialog>` traps focus; `<popover>` does not.
4. **Form submission** — `<dialog>` with `method="dialog"` closes on any button click.
5. **CSS-only limitations** — Complex interactions (animation, async loading, data fetching) still require JavaScript.
6. **`:has()` performance** — Complex `:has()` selectors can be slow on large DOM trees.

## Accessibility Considerations

`<dialog>` provides `role="dialog"`, focus management, and `aria-modal` automatically. Popovers should use `role="tooltip"` or `role="menu"` manually. Keyboard navigation (Tab, Escape) works natively with these APIs.

## Responsive Behavior

CSS-only components are inherently responsive. Use `max-width`, `max-height`, and media queries for responsive sizing. The `dialog` element supports `max-width: 100vw` for mobile fullscreen modals.
