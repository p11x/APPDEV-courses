---
title: Collapse
category: Component System
difficulty: 2
time: 25 min
tags: bootstrap5, collapse, toggle, content-display, expandable
---

## Overview

The Bootstrap Collapse plugin provides a simple way to toggle the visibility of content sections. It powers accordions, expandable FAQs, toggle panels, and any UI element that shows or hides content on demand. The plugin uses CSS transitions for smooth expand and collapse animations and supports horizontal expansion in addition to the default vertical behavior.

Collapse relies on the `data-bs-toggle="collapse"` attribute paired with a target selector via `data-bs-target` or `href`. The content container uses the `.collapse` class, with `.show` indicating visibility.

## Basic Implementation

A button toggles a collapsible section by referencing its `id` with `data-bs-target`.

```html
<p>
  <button class="btn btn-primary" type="button" data-bs-toggle="collapse"
    data-bs-target="#collapseExample" aria-expanded="false" aria-controls="collapseExample">
    Toggle Content
  </button>
</p>
<div class="collapse" id="collapseExample">
  <div class="card card-body">
    This is the collapsible content. It starts hidden and reveals on toggle.
  </div>
</div>
```

Use an `<a>` element with `href` as an alternative trigger method.

```html
<a class="btn btn-outline-secondary" data-bs-toggle="collapse" href="#collapseLink"
  role="button" aria-expanded="false" aria-controls="collapseLink">
  Toggle via Link
</a>
<div class="collapse" id="collapseLink">
  <div class="card card-body mt-2">
    Content revealed by the anchor trigger.
  </div>
</div>
```

Pre-show content by adding `.show` to the collapse container.

```html
<button class="btn btn-success mb-2" type="button" data-bs-toggle="collapse"
  data-bs-target="#openByDefault" aria-expanded="true" aria-controls="openByDefault">
  Toggle Open Section
</button>
<div class="collapse show" id="openByDefault">
  <div class="card card-body">
    This section is visible by default.
  </div>
</div>
```

## Advanced Variations

Multiple targets allow a single trigger or multiple triggers to control several collapse regions.

```html
<p>
  <a class="btn btn-primary" data-bs-toggle="collapse" href="#multiCollapse1"
    role="button">Toggle First</a>
  <button class="btn btn-primary" type="button" data-bs-toggle="collapse"
    data-bs-target="#multiCollapse2">Toggle Second</button>
  <button class="btn btn-secondary" type="button" data-bs-toggle="collapse"
    data-bs-target=".multi-collapse">Toggle Both</button>
</p>
<div class="row">
  <div class="col">
    <div class="collapse multi-collapse" id="multiCollapse1">
      <div class="card card-body">First collapsible panel.</div>
    </div>
  </div>
  <div class="col">
    <div class="collapse multi-collapse" id="multiCollapse2">
      <div class="card card-body">Second collapsible panel.</div>
    </div>
  </div>
</div>
```

Horizontal collapse expands content width instead of height.

```html
<p>
  <button class="btn btn-primary" type="button" data-bs-toggle="collapse"
    data-bs-target="#collapseWidth" aria-expanded="false" aria-controls="collapseWidth">
    Toggle Horizontal
  </button>
</p>
<div style="min-height: 120px;">
  <div class="collapse collapse-horizontal" id="collapseWidth">
    <div class="card card-body" style="width: 300px;">
      This content expands horizontally instead of vertically.
    </div>
  </div>
</div>
```

## Best Practices

1. Always provide unique `id` attributes for each collapse target element.
2. Set `aria-expanded` on triggers to reflect the current open or closed state.
3. Use `aria-controls` on each trigger to reference the target collapse element.
4. Prefer `<button>` elements for toggle triggers to avoid navigation side effects.
5. Wrap collapse content in a meaningful container (`.card`, `.card-body`, etc.) for visual structure.
6. Use `.collapse-horizontal` only when horizontal expansion serves the layout purpose.
7. Combine multiple targets with shared class selectors for grouped toggle behavior.
8. Initialize collapse plugins programmatically with `new bootstrap.Collapse(element)` when adding content dynamically.
9. Keep transition durations at default unless overriding globally via Sass variables.
10. Test collapse behavior with keyboard-only navigation to verify Tab and Enter/Space work.
11. Avoid applying `overflow: hidden` to parent containers, which can clip expanded content.

## Common Pitfalls

- Missing `id` on the collapse container or mismatched `data-bs-target` breaks toggle functionality silently.
- Forgetting `.collapse` on the content div causes it to display permanently instead of starting hidden.
- Using `data-bs-parent` outside an accordion context has no effect and causes confusion.
- Applying `.show` alongside `.collapse` without understanding that it makes content visible by default.
- Nesting collapse triggers inside other interactive elements causes event propagation conflicts.
- Setting explicit heights on collapse containers interferes with the CSS transition animation.
- Horizontal collapse requires a fixed or min-width on the inner content; without it, content may not expand visibly.

## Accessibility Considerations

Triggers must communicate state with `aria-expanded`. Screen readers use this attribute to announce whether the collapsible section is open or closed. Content regions benefit from `role="region"` and `aria-labelledby` pointing to the trigger when the collapse contains significant content. Ensure focus management returns logically after toggling, especially when the trigger disappears or changes context.

## Responsive Behavior

Collapse components are inherently responsive. They adapt to any parent container width. Horizontal collapse respects container boundaries but requires a defined width on its inner content. On mobile devices, collapse sections should have adequate padding and tap targets for comfortable interaction. Avoid placing horizontal collapse inside narrow columns where the expanded content cannot fit.
