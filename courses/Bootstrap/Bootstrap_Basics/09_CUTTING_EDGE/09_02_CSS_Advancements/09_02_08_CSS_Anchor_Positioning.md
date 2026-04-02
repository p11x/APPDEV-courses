---
title: "CSS Anchor Positioning"
description: "Using anchor() with Bootstrap tooltips/popovers for positioning without Popper.js"
difficulty: 3
tags: [anchor-positioning, tooltips, popovers, css-anchor, popper-alternative]
prerequisites:
  - 06_03_Popovers
  - 06_04_Tooltips
---

## Overview

CSS Anchor Positioning replaces JavaScript-based positioning libraries (Popper.js, Floating UI) with native CSS. Elements declare an `anchor-name` and other elements reference it with `position-anchor` and `anchor()`. This eliminates a JavaScript dependency, reduces bundle size, and moves positioning logic to CSS where it belongs.

Bootstrap 5 currently depends on Popper.js for tooltip and popover positioning. CSS Anchor Positioning provides the same functionality — position a tooltip above, below, left, or right of its trigger — with pure CSS. The `position-area` shorthand replaces manual `top`/`left` calculations, and `position-try-fallbacks` handles overflow flipping automatically.

## Basic Implementation

```html
<button class="btn btn-primary" style="anchor-name: --tooltip-trigger;">
  Hover me
</button>

<div class="tooltip-custom" style="position-anchor: --tooltip-trigger;">
  Tooltip content here
</div>
```

```css
[style*="anchor-name:"] {
  anchor-name: --tooltip-trigger;
}

.tooltip-custom {
  position: fixed;
  position-anchor: --tooltip-trigger;
  position-area: top center;

  /* Fallback if no space above */
  position-try-fallbacks: flip-block;

  /* Visual styling */
  background: var(--bs-dark);
  color: white;
  padding: 0.25rem 0.5rem;
  border-radius: var(--bs-border-radius);
  font-size: 0.875rem;
  white-space: nowrap;
}

/* Popover example */
.popover-anchor {
  anchor-name: --popover-target;
}

.popover-custom {
  position: fixed;
  position-anchor: --popover-target;
  position-area: bottom span-right;
  position-try-fallbacks: flip-block, flip-inline;

  min-width: 200px;
  background: var(--bs-body-bg);
  border: 1px solid var(--bs-border-color);
  border-radius: var(--bs-border-radius);
  box-shadow: 0 0.5rem 1rem rgba(0,0,0,0.15);
  padding: 1rem;
}
```

```js
// Progressive enhancement — fall back to Popper.js
if (!CSS.supports('anchor-name', '--test')) {
  // Load Popper.js dynamically
  import('https://cdn.jsdelivr.net/npm/@popperjs/core@2').then(({ createPopper }) => {
    document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(el => {
      createPopper(el, el.nextElementSibling, { placement: 'top' });
    });
  });
}
```

## Advanced Variations

Multiple anchor references and inset positioning:

```css
.menu-trigger {
  anchor-name: --menu-anchor;
}

.dropdown-menu-custom {
  position: fixed;
  position-anchor: --menu-anchor;
  position-area: bottom span-right;
  position-try-fallbacks: flip-block, flip-inline, flip-start;

  /* Inset from anchor edge */
  top: anchor(--menu-anchor bottom) + 4px;
  left: anchor(--menu-anchor left);
}
```

Scroll-aware visibility:

```css
.tooltip-custom {
  opacity: 0;
  transition: opacity 0.2s;

  &:has(+ [style*="anchor-name"]:hover),
  &:hover {
    opacity: 1;
  }
}
```

## Best Practices

1. Use `anchor-name` on the trigger element, `position-anchor` on the floating element.
2. Use `position-area` shorthand for common placements (top, bottom, left, right).
3. Use `position-try-fallbacks` for automatic overflow handling.
4. Provide Popper.js fallback for browsers without anchor positioning support.
5. Use `position: fixed` on anchored elements (required for anchor positioning).
6. Keep anchor names descriptive (`--tooltip-trigger`, `--popover-target`).
7. Use `anchor()` function for manual positioning calculations when needed.
8. Test overflow behavior at viewport edges.
9. Combine with CSS transitions for smooth show/hide animations.
10. Use `@supports` to detect support and load fallback conditionally.
11. Limit anchor names to one per element (current spec limitation).
12. Document which Bootstrap components use anchor positioning vs Popper.js.

## Common Pitfalls

1. **Browser support** — Chrome 125+ only; Firefox and Safari lack support as of 2024.
2. **`position: fixed` required** — Anchor positioning only works with `position: fixed`.
3. **Single anchor** — An element can only anchor to one other element.
4. **Scroll offset** — Anchored elements may not track anchor movement during scroll without additional configuration.
5. **Z-index** — Anchored elements need explicit `z-index` to appear above other content.
6. **Popper.js coexistence** — Running both Popper.js and anchor positioning on the same element causes conflicts.

## Accessibility Considerations

Maintain `role="tooltip"` and `aria-describedby` relationships between trigger and tooltip. Anchor positioning is purely visual — ARIA semantics must be set independently. Ensure keyboard-triggered tooltips (focus) are positioned correctly.

## Responsive Behavior

Anchor positioning is inherently responsive — elements position relative to their anchor regardless of viewport size. Use `position-try-fallbacks` to handle space constraints at different viewport sizes automatically.
