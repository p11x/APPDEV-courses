---
title: CSS-Only Components
category: [Future Bootstrap, Cutting Edge]
difficulty: 3
time: 30 min
tags: bootstrap5, popover-api, anchor-positioning, css-only, modals, tooltips
---

## Overview

Modern CSS APIs like the Popover API, `:popover-open`, and CSS anchor positioning enable fully functional components without JavaScript. These browser-native features reduce script overhead, improve performance, and create progressive enhancement patterns that fall back gracefully when Bootstrap JS is unavailable.

## Basic Implementation

Creating a popover using the native Popover API with Bootstrap styling.

```html
<button class="btn btn-primary" popovertarget="myPopover">
  Show Popover
</button>

<div id="myPopover" popover class="p-3 rounded shadow">
  <h6>Native Popover</h6>
  <p>This uses the Popover API — no JavaScript needed.</p>
  <button class="btn btn-sm btn-outline-secondary" popovertarget="myPopover"
          popovertargetaction="hide">Close</button>
</div>

<style>
  [popover] {
    border: 1px solid var(--bs-border-color);
    background: var(--bs-body-bg);
    max-width: 300px;
  }

  [popover]:popover-open {
    opacity: 1;
    transform: translateY(0);
  }

  @starting-style {
    [popover]:popover-open {
      opacity: 0;
      transform: translateY(-10px);
    }
  }
</style>
```

## Advanced Variations

Using CSS anchor positioning for tooltips attached to trigger elements.

```html
<button class="btn btn-outline-primary" id="anchorBtn" popovertarget="tipPopover">
  Hover or Click Me
</button>

<div id="tipPopover" popover class="px-3 py-2 rounded"
     style="position-anchor: --my-anchor; top: anchor(bottom); left: anchor(center);
            translate: -50% 8px; margin: 0;">
  <small>Positioned relative to the anchor button.</small>
</div>

<style>
  #anchorBtn {
    anchor-name: --my-anchor;
  }

  #tipPopover {
    border: none;
    background: var(--bs-dark);
    color: var(--bs-light);
    font-size: 0.85rem;
    box-shadow: 0 4px 12px rgba(0,0,0,0.2);
  }
</style>
```

Building a CSS-only modal using the `:popover-open` pseudo-class.

```html
<button class="btn btn-success" popovertarget="cssModal">Open CSS-Only Modal</button>

<div id="cssModal" popover class="modal-dialog p-0">
  <div class="modal-content">
    <div class="modal-header">
      <h5 class="modal-title">CSS-Only Modal</h5>
      <button class="btn-close" popovertarget="cssModal"
              popovertargetaction="hide"></button>
    </div>
    <div class="modal-body">
      <p>This modal uses the Popover API with Bootstrap classes.</p>
      <p>No JavaScript required for open, close, or backdrop.</p>
    </div>
    <div class="modal-footer">
      <button class="btn btn-secondary" popovertarget="cssModal"
              popovertargetaction="hide">Cancel</button>
      <button class="btn btn-primary">Confirm</button>
    </div>
  </div>
</div>

<style>
  #cssModal {
    border: none;
    padding: 0;
    background: transparent;
    max-width: 500px;
    width: 90%;
  }

  #cssModal:popover-open::backdrop {
    background: rgba(0, 0, 0, 0.5);
    backdrop-filter: blur(2px);
  }

  @starting-style {
    #cssModal:popover-open {
      opacity: 0;
      transform: scale(0.95);
    }
  }

  #cssModal:popover-open {
    opacity: 1;
    transform: scale(1);
    transition: opacity 0.2s, transform 0.2s;
  }
</style>
```

## Best Practices

1. Use `popover` attribute on the container element to enable the Popover API
2. Use `popovertarget` on trigger buttons to link them declaratively
3. Apply `popovertargetaction="hide"` on close buttons inside popovers
4. Style `:popover-open` for visible state transitions and animations
5. Use `@starting-style` for entry animations without JavaScript
6. Apply `anchor-name` on the trigger and `position-anchor` on the popover for positioning
7. Use Bootstrap classes inside popovers for consistent component styling
8. Provide `::backdrop` styles for modal-like overlays
9. Test with `popover="manual"` for auto-close prevention
10. Layer popovers above other content using the browser's top layer automatically

## Common Pitfalls

1. **Browser support** — Popover API requires Chrome 114+, Firefox 125+, Safari 17+
2. **Anchor positioning** — CSS anchor positioning requires Chrome 125+; no Firefox/Safari yet
3. **No focus management** — Native popovers don't trap focus like Bootstrap modals
4. **Limited animation** — `@starting-style` is newer and has limited browser support
5. **Cannot nest popovers** — Only one popover can be open at a time (unless `popover="manual"`)
6. **Shadow DOM boundary** — Popovers inside Shadow DOM may not work as expected
7. **Missing keyboard dismissal** — Escape key behavior varies across implementations
8. **No JS fallback** — Browsers without support show nothing without a JS polyfill

## Accessibility Considerations

Native popovers get `role="dialog"` or `role="tooltip"` automatically depending on invocation. Ensure trigger buttons have accessible labels. Use `aria-describedby` to associate popover content with triggers. Test keyboard navigation — Tab should move within the popover, Escape should close it. Provide fallback interactive content for browsers without Popover API support.

## Responsive Behavior

Popovers inherit the viewport constraint and reposition automatically. Use anchor positioning with fallback `position: fixed` for mobile layouts. Apply Bootstrap's responsive width utilities (`w-100`, `mw-100`) to popovers for narrow viewports. Test popover placement at various screen sizes to prevent off-screen rendering.
