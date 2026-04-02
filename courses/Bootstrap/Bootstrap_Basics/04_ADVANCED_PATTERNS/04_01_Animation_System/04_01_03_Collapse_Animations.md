---
title: Collapse Animations
category: Advanced Patterns
difficulty: 2
time: 20 min
tags: bootstrap5, animation, collapse, accordion, expand, transitions
---

## Overview

Bootstrap 5's collapse component toggles content visibility with smooth height-based transitions. It powers accordions, expandable navigation menus, and toggleable content panels. The collapse animation works by transitioning the `height` property from `0` to `auto` via Bootstrap's JavaScript, which measures the element's scroll height and sets an explicit pixel value for the CSS transition.

By default, collapse uses a 350ms transition defined in Bootstrap's Sass via `$transition-collapse`. Understanding how this works lets you customize timing, add horizontal collapse variants, and chain animations with collapse events.

## Basic Implementation

The simplest collapse uses a trigger button with `data-bs-toggle="collapse"` and `data-bs-target` pointing to the content container:

```html
<button class="btn btn-primary" type="button" data-bs-toggle="collapse"
        data-bs-target="#collapseExample" aria-expanded="false"
        aria-controls="collapseExample">
  Toggle Content
</button>

<div class="collapse" id="collapseExample">
  <div class="card card-body mt-3">
    This content collapses and expands with a smooth height animation.
    The transition duration defaults to 350ms.
  </div>
</div>
```

The `.show` class is toggled on the collapsible element. When present, `height: auto` is applied after the transition completes. During animation, an explicit pixel height is set to enable the CSS transition (you cannot transition to `auto`).

## Advanced Variations

Horizontal collapse uses `.collapse-horizontal` to animate `width` instead of height. This is useful for sidebar toggles:

```html
<button class="btn btn-secondary mb-3" data-bs-toggle="collapse"
        data-bs-target="#horizontalCollapse">
  Toggle Sidebar
</button>

<div class="collapse collapse-horizontal" id="horizontalCollapse">
  <div class="card card-body" style="width: 280px;">
    This panel collapses horizontally, animating its width from 0 to the
    content's natural width.
  </div>
</div>
```

Bootstrap's accordion component wraps collapse in a structured pattern:

```html
<div class="accordion" id="faqAccordion">
  <div class="accordion-item">
    <h2 class="accordion-header">
      <button class="accordion-button" type="button" data-bs-toggle="collapse"
              data-bs-target="#faq1" aria-expanded="true" aria-controls="faq1">
        What is Bootstrap Collapse?
      </button>
    </h2>
    <div id="faq1" class="accordion-collapse collapse show"
         data-bs-parent="#faqAccordion">
      <div class="accordion-body">
        A component for toggling visibility of content with animated transitions.
      </div>
    </div>
  </div>
  <div class="accordion-item">
    <h2 class="accordion-header">
      <button class="accordion-button collapsed" type="button"
              data-bs-toggle="collapse" data-bs-target="#faq2">
        How do I customize the animation?
      </button>
    </h2>
    <div id="faq2" class="accordion-collapse collapse"
         data-bs-parent="#faqAccordion">
      <div class="accordion-body">
        Override the Sass variables or CSS properties directly.
      </div>
    </div>
  </div>
</div>
```

Customize the collapse duration globally via Sass or per-element via CSS:

```css
/* Global override */
:root {
  --bs-collapse-transition: height 0.5s ease;
}

/* Per-element override */
#customCollapse.collapsing {
  transition: height 0.6s cubic-bezier(0.4, 0, 0.2, 1);
}
```

Listen to collapse events to trigger additional animations:

```js
const el = document.getElementById('collapseExample');
el.addEventListener('shown.bs.collapse', () => {
  el.classList.add('fade-in-content');
});
el.addEventListener('hidden.bs.collapse', () => {
  el.classList.remove('fade-in-content');
});
```

## Best Practices

1. Always include `aria-expanded` and `aria-controls` on toggle buttons for accessibility.
2. Use `.accordion-flush` to remove outer borders in accordions for edge-to-edge layouts.
3. Set `data-bs-parent` on accordion panels to enforce single-open behavior — only one panel opens at a time.
4. Override `$transition-collapse` and `$transition-collapse-height` Sass variables for project-wide timing changes.
5. Avoid placing large amounts of content in collapsible sections — users lose context when content disappears.
6. Use `hide.bs.collapse` and `show.bs.collapse` events for pre-animation logic like loading data.
7. Combine collapse with `.card` components for visually structured expandable content.
8. Test collapse with dynamic content — if content height changes after initialization, re-trigger the measurement.
9. Use `data-bs-toggle="collapse"` on `<a>` elements with `href` as an alternative to `data-bs-target`.
10. Keep transition durations between 200ms and 400ms for collapse to feel responsive without being jarring.

## Common Pitfalls

1. **Transitioning to `height: auto` directly**: CSS cannot animate to `auto`. Bootstrap handles this by measuring `scrollHeight` and setting an explicit pixel value during animation, then switching to `auto` after `.collapsing` transitions to `.collapse.show`.
2. **Missing `.collapse` class**: The collapse plugin requires the base `.collapse` class on the target element. Without it, the JavaScript does not attach and nothing happens.
3. **Conflicting overflow styles**: If the collapsible container has `overflow: visible` during animation, content may flash outside the bounds. Bootstrap sets `overflow: hidden` during `.collapsing`.
4. **Nested collapses without unique IDs**: Multiple collapses in the same document must have distinct `id` values. Duplicate IDs cause targeting failures.
5. **Forgetting `data-bs-parent` in accordions**: Without this attribute, multiple panels can open simultaneously, which may not be the intended behavior.

## Accessibility Considerations

Collapse triggers must have `aria-expanded` reflecting their current state (Bootstrap toggles this automatically). Use `aria-controls` to associate the trigger with its collapsible target. For accordions, use `aria-labelledby` on each panel to reference its heading button. Screen readers announce collapse state changes — ensure button labels are descriptive enough to convey what will be revealed or hidden.

## Responsive Behavior

Collapse works across all breakpoints by default. Use Bootstrap's responsive display utilities to conditionally show/hide collapse triggers:

```html
<button class="btn btn-outline-primary d-md-none" data-bs-toggle="collapse"
        data-bs-target="#mobileNav">
  Menu
</button>
<div class="collapse d-md-block" id="mobileNav">
  <nav class="nav flex-column">
    <a class="nav-link" href="#">Home</a>
    <a class="nav-link" href="#">About</a>
  </nav>
</div>
```

On desktop (`md`+), the nav displays inline via `.d-md-block`. On mobile, it collapses behind the toggle button. Horizontal collapse is especially effective for responsive sidebar layouts that slide in from the edge on smaller screens.
