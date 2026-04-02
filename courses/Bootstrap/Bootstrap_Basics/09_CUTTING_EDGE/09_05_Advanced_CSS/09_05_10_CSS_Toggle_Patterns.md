---
title: "CSS Toggle Patterns Without JavaScript"
category: "Cutting Edge"
difficulty: 2
time: "15 min"
tags: bootstrap5, css, toggle, checkbox-hack, no-js
prerequisites: ["09_05_09_CSS_Transitions_Native"]
---

## Overview

CSS-only toggle patterns use the checkbox hack, `:target` pseudo-class, and `details/summary` elements to create interactive Bootstrap components without JavaScript. These patterns enable accordions, modals, dropdowns, and tabs using pure CSS, improving performance and providing graceful fallbacks when JavaScript is unavailable.

## Basic Implementation

### Checkbox Hack Accordion

```html
<div class="accordion" id="cssAccordion">
  <div class="accordion-item">
    <h2 class="accordion-header">
      <input type="checkbox" class="btn-check" id="toggle1" autocomplete="off">
      <label class="accordion-button collapsed" for="toggle1">
        Collapsible Item #1
      </label>
    </h2>
    <div class="accordion-collapse-wrapper">
      <div class="accordion-body">
        Content revealed with pure CSS using the checkbox hack.
      </div>
    </div>
  </div>
</div>

<style>
.accordion-collapse-wrapper {
  max-height: 0;
  overflow: hidden;
  transition: max-height 0.35s ease;
}
#toggle1:checked ~ .accordion-collapse-wrapper,
.btn-check:checked + .accordion-button + .accordion-collapse-wrapper {
  max-height: 500px;
}
.btn-check:checked + .accordion-button::after {
  transform: rotate(180deg);
}
</style>
```

## Advanced Variations

### Details/Summary Accordion

```html
<div class="accordion">
  <details class="accordion-item">
    <summary class="accordion-button">Click to expand</summary>
    <div class="accordion-body">Pure CSS accordion using details element.</div>
  </details>
  <details class="accordion-item">
    <summary class="accordion-button">Second item</summary>
    <div class="accordion-body">No JavaScript required.</div>
  </details>
</div>

<style>
details > summary { list-style: none; cursor: pointer; }
details > summary::-webkit-details-marker { display: none; }
details[open] > .accordion-body { animation: slideDown 0.3s ease; }
@keyframes slideDown { from { opacity: 0; transform: translateY(-10px); } }
</style>
```

## Best Practices

- **Use for progressive enhancement** - JS version can enhance CSS baseline
- **Include proper ARIA** - CSS toggles still need accessibility attributes
- **Test keyboard navigation** - Ensure Tab and Enter/Space work correctly
- **Add focus styles** - CSS-only components need visible focus indicators
- **Set reasonable max-height** - Animated height needs a maximum value
- **Use semantic HTML** - details/summary is preferred over checkbox hack
- **Document JS dependency** - Note which features require JavaScript
- **Test screen readers** - CSS toggles may not announce state changes
- **Provide visual feedback** - Clear open/close indicators
- **Consider touch targets** - Labels must be large enough on mobile

## Common Pitfalls

- **Screen reader issues** - Checkbox hack confuses assistive technology
- **Focus management** - CSS can't move focus like JavaScript
- **State not announced** - Toggle state may be invisible to screen readers
- **Animation limitations** - CSS height animation requires max-height hack
- **Nested toggle conflicts** - Multiple checkboxes can interfere
- **Browser inconsistencies** - details/summary behavior varies
- **Missing fallbacks** - Content hidden without CSS enabled
- **SEO concerns** - Hidden content may not be indexed

## Accessibility Considerations

Use `details/summary` which has native keyboard and screen reader support. For checkbox hack, add `role="button"` and `aria-expanded` to labels. Ensure all interactive elements are keyboard accessible. Provide `aria-controls` linking toggles to content. Test with screen readers to verify state announcements.

## Responsive Behavior

Toggle patterns work well across all viewport sizes. Ensure touch targets are 44x44px minimum on mobile. Accordion content should not overflow on small screens. Consider auto-closing other items when one opens on mobile. Dropdown toggles should handle touch events properly.
