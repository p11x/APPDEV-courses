---
title: "Native Details Accordion with Bootstrap"
category: "Cutting Edge"
difficulty: 1
time: "15 min"
tags: bootstrap5, css, accordion, details, summary, native-html
prerequisites: ["09_05_12_CSS_Native_Modals"]
---

## Overview

The `<details>` and `<summary>` HTML elements create native accordions without JavaScript. When styled with Bootstrap classes, they provide accessible, keyboard-navigable expand/collapse functionality with zero JavaScript dependency. The `name` attribute enables exclusive (single-open) accordion behavior natively in modern browsers.

## Basic Implementation

### Basic Details Accordion

```html
<div class="accordion">
  <details class="accordion-item">
    <summary class="accordion-header">
      <span class="accordion-button">What is Bootstrap?</span>
    </summary>
    <div class="accordion-body">
      Bootstrap is a powerful front-end framework for faster and easier web development.
      It includes HTML, CSS, and JavaScript-based design templates.
    </div>
  </details>

  <details class="accordion-item">
    <summary class="accordion-header">
      <span class="accordion-button">How do I install it?</span>
    </summary>
    <div class="accordion-body">
      You can install Bootstrap via npm, CDN, or download the compiled files directly.
      The recommended approach is using npm for production projects.
    </div>
  </details>

  <details class="accordion-item">
    <summary class="accordion-header">
      <span class="accordion-button">Is it free to use?</span>
    </summary>
    <div class="accordion-body">
      Yes! Bootstrap is completely free and open-source under the MIT license.
      You can use it for personal and commercial projects without restrictions.
    </div>
  </details>
</div>
```

### Styling with Bootstrap Classes

```html
<style>
details.accordion-item {
  border: var(--bs-accordion-border-width) solid var(--bs-accordion-border-color);
  border-radius: var(--bs-accordion-border-radius);
  margin-bottom: -1px;
}

details.accordion-item summary {
  list-style: none;
  cursor: pointer;
  padding: var(--bs-accordion-btn-padding-y) var(--bs-accordion-btn-padding-x);
  background: var(--bs-accordion-btn-bg);
  font-weight: var(--bs-accordion-btn-font-weight);
}

details.accordion-item summary::-webkit-details-marker {
  display: none;
}

details.accordion-item[open] summary {
  background: var(--bs-accordion-active-bg);
}

details.accordion-item .accordion-body {
  padding: var(--bs-accordion-body-padding-y) var(--bs-accordion-body-padding-x);
}

details.accordion-item[open] .accordion-body {
  animation: accordion-open 0.3s ease;
}

@keyframes accordion-open {
  from { opacity: 0; transform: translateY(-8px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>
```

## Advanced Variations

### Exclusive Accordion (Single Open)

```html
<!-- Using the name attribute for exclusive behavior (modern browsers) -->
<div class="accordion">
  <details class="accordion-item" name="faq-accordion">
    <summary class="accordion-header">
      <span class="accordion-button">Question 1</span>
    </summary>
    <div class="accordion-body">Answer 1 - Opening this closes others.</div>
  </details>

  <details class="accordion-item" name="faq-accordion">
    <summary class="accordion-header">
      <span class="accordion-button">Question 2</span>
    </summary>
    <div class="accordion-body">Answer 2 - Only one can be open at a time.</div>
  </details>

  <details class="accordion-item" name="faq-accordion">
    <summary class="accordion-header">
      <span class="accordion-button">Question 3</span>
    </summary>
    <div class="accordion-body">Answer 3 - Native exclusive accordion.</div>
  </details>
</div>
```

### Flush Accordion Variant

```html
<div class="accordion accordion-flush">
  <details class="accordion-item border-0 border-bottom">
    <summary class="accordion-header bg-transparent px-0">
      <span class="accordion-button collapsed bg-transparent shadow-none">
        Flush Item 1
      </span>
    </summary>
    <div class="accordion-body px-0">Content without borders.</div>
  </details>
</div>
```

## Best Practices

- **Use semantic HTML** - details/summary is the most accessible accordion
- **Style with CSS variables** - Use Bootstrap's accordion variables for consistency
- **Add animations** - Use @keyframes with [open] for smooth transitions
- **Use name attribute** - For exclusive accordion behavior in modern browsers
- **Remove default marker** - Hide webkit details marker for clean appearance
- **Include proper ARIA** - Though native, add aria-expanded if needed
- **Test keyboard navigation** - Enter/Space should toggle, Tab navigates
- **Provide JS fallback** - For exclusive behavior in older browsers
- **Use Bootstrap variables** - Match existing accordion styling
- **Keep content accessible** - Ensure screen readers can navigate content

## Common Pitfalls

- **Missing list-style removal** - Default triangle marker shows
- **No animation** - Abrupt open/close without transitions
- **Browser inconsistencies** - Safari handles details differently
- **Focus management issues** - Focus may not move to content
- **Styling limitations** - Can't animate height with CSS alone
- **JS conflicts** - Accordion JS plugins may interfere with native behavior
- **Missing name attribute** - Multiple items can be open unexpectedly
- **Overriding native behavior** - Breaking built-in accessibility

## Accessibility Considerations

Native `<details>` elements have built-in keyboard support (Enter/Space to toggle). Screen readers announce expand/collapse state automatically. The `<summary>` element is focusable by default. Content is hidden from assistive technology when collapsed. No ARIA attributes are required for basic accessibility, but `aria-expanded` can enhance the experience.

## Responsive Behavior

Accordions work well across all viewport sizes. Touch targets (summary elements) must be at least 44x44px. Content should reflow naturally within accordion-body. Consider full-width accordion items on mobile. Animation duration may need adjustment for slower mobile devices. Ensure text remains readable when expanded on small screens.
