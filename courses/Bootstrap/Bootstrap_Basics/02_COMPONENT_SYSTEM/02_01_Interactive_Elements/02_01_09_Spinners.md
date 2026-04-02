---
title: Spinners
category: Component System
difficulty: 1
time: 20 min
tags: bootstrap5, spinners, loading, indicators, animations
---

## Overview

Bootstrap spinners provide lightweight loading indicators built entirely with CSS. They signal an action is processing and help users understand that content is loading. Bootstrap offers two spinner styles: a border spinner (`spinner-border`) that rotates, and a growing spinner (`spinner-grow`) that pulses. Both support sizing, color variants, and can be embedded inside buttons or used inline.

## Basic Implementation

The simplest spinner uses the `spinner-border` class for a rotating ring or `spinner-grow` for a pulsing dot.

```html
<!-- Border spinner (rotating ring) -->
<div class="spinner-border" role="status">
  <span class="visually-hidden">Loading...</span>
</div>

<!-- Grow spinner (pulsing dot) -->
<div class="spinner-grow" role="status">
  <span class="visually-hidden">Loading...</span>
</div>
```

Always include a `role="status"` attribute and a visually hidden text element for screen readers.

## Advanced Variations

Spinners can be resized, colored, and embedded in other components.

```html
<!-- Size variations -->
<div class="spinner-border spinner-border-sm" role="status">
  <span class="visually-hidden">Loading...</span>
</div>
<div class="spinner-border" style="width: 3rem; height: 3rem;" role="status">
  <span class="visually-hidden">Loading...</span>
</div>

<!-- Color variants -->
<div class="spinner-border text-primary" role="status">
  <span class="visually-hidden">Loading...</span>
</div>
<div class="spinner-border text-success" role="status">
  <span class="visually-hidden">Loading...</span>
</div>
<div class="spinner-grow text-warning" role="status">
  <span class="visually-hidden">Loading...</span>
</div>
```

```html
<!-- Spinner inside a button -->
<button class="btn btn-primary" type="button" disabled>
  <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
  Loading...
</button>

<button class="btn btn-success" type="button" disabled>
  <span class="spinner-grow spinner-border-sm" role="status" aria-hidden="true"></span>
  Please wait
</button>
```

```html
<!-- Centered spinner with flexbox -->
<div class="d-flex justify-content-center">
  <div class="spinner-border text-info" role="status">
    <span class="visually-hidden">Loading...</span>
  </div>
</div>

<!-- Float spinner (end-aligned) -->
<div class="clearfix">
  <div class="spinner-border float-end" role="status">
    <span class="visually-hidden">Loading...</span>
  </div>
</div>
```

## Best Practices

1. Always include `role="status"` on spinner elements for accessibility.
2. Provide visually hidden text describing what is loading.
3. Use `spinner-border-sm` for compact UI areas like buttons and table rows.
4. Match spinner color to the context using text utility classes.
5. Disable interactive elements (buttons, links) while a spinner is active.
6. Use `aria-hidden="true"` when the spinner is purely decorative alongside visible text.
7. Place spinners inside buttons to indicate form submission progress.
8. Center spinners with `d-flex justify-content-center` when loading full sections.
9. Use the grow spinner variant for subtle background processing indicators.
10. Limit spinner duration; show progress bars or skeleton screens for long operations.
11. Avoid nesting spinners or using multiple spinners in close proximity.

## Common Pitfalls

1. **Missing accessible text.** Omitting `role="status"` or the visually hidden `<span>` makes spinners invisible to assistive technology.
2. **Not disabling buttons.** Users can click submit buttons repeatedly if the button remains enabled during loading.
3. **Overusing spinners on fast actions.** A spinner that appears for 200ms causes a flicker that feels slower than no feedback at all.
4. **Forgetting color contrast.** Using `text-muted` spinner on a light background fails WCAG contrast requirements.
5. **Customizing without maintaining animation.** Removing or altering the CSS animation keyframes breaks the spinner behavior.
6. **Using spinners for long processes.** Spinners imply short waits; use progress bars or skeleton loaders for operations exceeding a few seconds.

## Accessibility Considerations

Spinners require `role="status"` to announce loading state to screen readers. The `visually-hidden` span provides context about what is loading. When a spinner replaces content, use `aria-live="polite"` on a parent container so screen readers announce the state change. When a spinner appears inside a button, the button text itself provides the label; pair it with `disabled` to prevent interaction.

## Responsive Behavior

Spinners scale naturally with their container. Use `spinner-border-sm` and `spinner-grow-sm` for compact responsive layouts. In flex containers, spinners align with alignment utilities (`justify-content-center`, `align-items-center`). On mobile, avoid large custom-sized spinners that consume excessive screen space; prefer small variants with descriptive text labels beside them.
