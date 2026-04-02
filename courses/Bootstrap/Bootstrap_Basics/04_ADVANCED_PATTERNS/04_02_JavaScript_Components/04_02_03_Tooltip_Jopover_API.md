---
title: "Tooltip & Popover JavaScript API"
module: "JavaScript Components"
lesson: "04_02_03"
difficulty: 3
estimated_time: 30 minutes
bootstrap_version: 5.3
prerequisites:
  - Bootstrap 5 basics
  - JavaScript DOM manipulation
  - Understanding of data attributes
learning_objectives:
  - Initialize tooltips and popovers via the JavaScript API
  - Configure placement, triggers, templates, and sanitization
  - Control visibility with show/hide/toggle methods
  - Enable, disable, and dispose of instances
  - Build custom templates for tooltips and popovers
---

# Tooltip & Popover JavaScript API

## Overview

Bootstrap tooltips and popovers require explicit JavaScript initialization — unlike other components, they do not auto-initialize from `data-bs-*` attributes. The `bootstrap.Tooltip` and `bootstrap.Popover` classes provide rich configuration for placement, triggers, content rendering, and sanitization.

Both classes share nearly identical APIs. The key difference is that popovers support a title and richer content, while tooltips display only a single text string.

```js
// Must initialize — data attributes alone won't work
const tooltip = new bootstrap.Tooltip(element);
const popover = new bootstrap.Popover(element);
```

## Basic Implementation

### Tooltip Initialization

```html
<button type="button" class="btn btn-primary"
        id="saveTooltip"
        data-bs-title="Saves your current progress"
        data-bs-placement="top">
  Save
</button>
```

```js
const btn = document.getElementById('saveTooltip');
const tooltip = new bootstrap.Tooltip(btn, {
  trigger: 'hover focus',
  delay: { show: 200, hide: 0 }
});

// Control methods
tooltip.show();
tooltip.hide();
tooltip.toggle();
tooltip.disable();   // Prevents showing
tooltip.enable();    // Re-enables
tooltip.dispose();   // Cleanup
```

### Popover Initialization

```html
<button type="button" class="btn btn-secondary"
        id="detailsPopover"
        data-bs-title="User Details"
        data-bs-content="<strong>Email:</strong> user@example.com"
        data-bs-html="true"
        data-bs-placement="right">
  View Details
</button>
```

```js
const el = document.getElementById('detailsPopover');
const popover = new bootstrap.Popover(el, {
  sanitize: false,  // Required when using HTML content
  trigger: 'click'
});
```

## Advanced Variations

### Configuration Options

```js
const tooltip = new bootstrap.Tooltip(element, {
  title: 'Custom title',           // String or function
  placement: 'auto',               // 'top', 'bottom', 'left', 'right', 'auto', or function
  trigger: 'hover focus',          // 'click', 'hover', 'focus', 'manual', or combination
  container: 'body',               // Appends tooltip to this element
  delay: { show: 100, hide: 200 }, // Number or object
  html: true,                      // Allow HTML in title (deprecated — use sanitize)
  sanitize: true,                  // Strip dangerous HTML
  allowList: { /* custom tags */ },// Override sanitization allowlist
  template: '<div class="tooltip" role="tooltip">...</div>', // Custom template
  fallbackPlacements: ['top', 'right', 'bottom', 'left'],
  customClass: 'my-custom-tooltip',
  popperConfig: {}                 // Direct Popper.js config override
});
```

### Custom Template

```js
const customTooltip = new bootstrap.Tooltip(element, {
  template: `
    <div class="tooltip" role="tooltip">
      <div class="tooltip-arrow"></div>
      <div class="tooltip-inner bg-dark text-warning fw-bold"></div>
    </div>
  `,
  title: 'Warning!'
});
```

```js
const customPopover = new bootstrap.Popover(element, {
  template: `
    <div class="popover" role="tooltip">
      <div class="popover-arrow"></div>
      <h3 class="popover-header bg-primary text-white"></h3>
      <div class="popover-body"></div>
    </div>
  `,
  title: 'Styled Popover',
  content: 'This popover uses a custom template.',
  html: true,
  sanitize: false
});
```

### Dynamic Content with Functions

```js
const popover = new bootstrap.Popover(element, {
  title: function () {
    return `Details for ${this.getAttribute('data-item-id')}`;
  },
  content: function () {
    const id = this.getAttribute('data-item-id');
    return `<ul><li>ID: ${id}</li><li>Status: Active</li></ul>`;
  },
  html: true,
  sanitize: false,
  trigger: 'click',
  placement: 'right'
});
```

### Enable/Disable Toggle

```js
const tooltip = new bootstrap.Tooltip(element);

// Disable — tooltip won't show on interaction
tooltip.disable();

// Re-enable
tooltip.enable();

// Manual control — ignore trigger events
tooltip.show();
setTimeout(() => tooltip.hide(), 2000);
```

### Retrieving Instances

```js
const existing = bootstrap.Tooltip.getInstance(element);
const instance = bootstrap.Tooltip.getOrCreateInstance(element);
```

## Best Practices

1. **Always initialize via JavaScript** — tooltips and popovers do not auto-initialize from `data-bs-*` attributes.
2. **Set `container: 'body'`** when tooltips are inside elements with `overflow: hidden` to prevent clipping.
3. **Use `sanitize: true`** (default) for user-generated content; only disable it for trusted static content.
4. **Use `trigger: 'click'` for popovers** that contain interactive elements; `hover` popovers disappear before users can interact.
5. **Dispose of instances** when removing elements from the DOM to prevent orphaned popper elements.
6. **Use `delay` to prevent flicker** — a small `show` delay (100-200ms) prevents tooltips from flashing during quick mouse movements.
7. **Use functions for dynamic content** instead of setting `data-bs-content` attributes with interpolated strings.
8. **Avoid tooltips on non-focusable elements** like `<span>` — add `tabindex="0"` so keyboard users can access them.
9. **Use `placement: 'auto'`** when the target element may be near screen edges; Popper.js will choose the best position.
10. **Override `allowList`** when you need specific HTML tags that Bootstrap sanitizes by default (e.g., `<iframe>`, `<video>`).
11. **Use `customClass`** instead of overriding Bootstrap tooltip CSS classes directly.

## Common Pitfalls

1. **Forgetting to initialize** — the most common mistake. `data-bs-toggle="tooltip"` alone does nothing without `new bootstrap.Tooltip()`.
2. **Using `html: true` with user input** — creates XSS vulnerabilities. Use `sanitize: false` only with trusted content.
3. **Tooltips inside `overflow: hidden` containers** — they get clipped. Always set `container: 'body'`.
4. **Not disposing instances** — tooltips create DOM elements. Removing the trigger without disposing leaves orphan tooltip elements.
5. **Tooltip on disabled buttons** — disabled elements don't fire events. Wrap the button in a `<span>` or `<div>` and attach the tooltip there.
6. **Confusing tooltip and popover content options** — tooltips use `title`, popovers use both `title` and `content`.
7. **Using `html: true` instead of `sanitize` configuration** — `html` is deprecated in favor of `sanitize` and `allowList`.

## Accessibility Considerations

- Tooltips should add `aria-describedby` pointing to the tooltip element (Bootstrap handles this automatically).
- Use tooltips only for supplementary information — never for essential content.
- Ensure tooltip triggers are focusable: use `<button>` or add `tabindex="0"` to other elements.
- Popovers with interactive content should use `trigger: 'click'` and manage focus inside the popover.
- The tooltip `role="tooltip"` is set automatically; do not override it.
- Keyboard users must be able to trigger the tooltip via focus — use `trigger: 'hover focus'` (default).

```html
<button type="button" class="btn btn-info"
        id="helpBtn"
        data-bs-title="Opens the help documentation in a new tab"
        data-bs-placement="top"
        tabindex="0">
  <i class="bi bi-question-circle"></i>
  <span class="visually-hidden">Help</span>
</button>
```

```js
// Ensure keyboard accessibility
const helpTooltip = new bootstrap.Tooltip(document.getElementById('helpBtn'), {
  trigger: 'hover focus'
});
```

## Responsive Behavior

Tooltips and popovers use Popper.js for positioning, which automatically handles responsive repositioning:

- **Automatic placement adjustment** — when the viewport shrinks, `placement: 'auto'` flips the tooltip to the side with the most space.
- **Scrollable containers** — tooltips reposition on scroll when properly attached. Use `container: 'body'` for scrollable parents.
- **Touch devices** — `hover` trigger does not work reliably on touch. Switch to `click` or `manual` triggers for mobile:

```js
const isTouchDevice = 'ontouchstart' in window;

const tooltip = new bootstrap.Tooltip(element, {
  trigger: isTouchDevice ? 'click' : 'hover focus'
});
```

- **Popper config overrides** for fine-grained responsive behavior:

```js
const popover = new bootstrap.Popover(element, {
  popperConfig: {
    modifiers: [{
      name: 'preventOverflow',
      options: {
        boundary: 'viewport',
        padding: 16
      }
    }]
  }
});
```
