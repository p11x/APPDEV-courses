---
title: "Spinner Variations"
description: "Build growing spinners, button spinners, sized spinners, color variants, and text-adjacent spinners in Bootstrap 5"
difficulty: 1
tags: [spinners, components, variations, loading, indicators]
prerequisites:
  - "Bootstrap 5 spinner basics"
  - "CSS animation fundamentals"
---

## Overview

Bootstrap 5 provides two spinner types: border spinners (rotating ring) and growing spinners (pulsing circle). Common variations include embedding spinners inside buttons during form submissions, sizing spinners from extra-small to large, applying contextual and custom colors, and pairing spinners with descriptive loading text. Spinners communicate that a process is ongoing and prevent users from thinking the interface is frozen.

## Basic Implementation

### Border Spinner

```html
<div class="spinner-border" role="status">
  <span class="visually-hidden">Loading...</span>
</div>
```

### Growing Spinner

```html
<div class="spinner-grow" role="status">
  <span class="visually-hidden">Loading...</span>
</div>
```

## Advanced Variations

### Spinner Sizes

Use `spinner-border-sm` or `spinner-grow-sm` for compact spinners, or set explicit dimensions.

```html
<!-- Small spinner -->
<div class="spinner-border spinner-border-sm" role="status">
  <span class="visually-hidden">Loading...</span>
</div>

<!-- Default spinner -->
<div class="spinner-border" role="status">
  <span class="visually-hidden">Loading...</span>
</div>

<!-- Large spinner via custom style -->
<div class="spinner-border" style="width: 3rem; height: 3rem;" role="status">
  <span class="visually-hidden">Loading...</span>
</div>

<!-- Growing variants -->
<div class="spinner-grow spinner-grow-sm" role="status">
  <span class="visually-hidden">Loading...</span>
</div>
```

### Spinner in Buttons

Embedding spinners inside buttons provides immediate feedback during async actions.

```html
<button class="btn btn-primary" type="button" disabled>
  <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
  <span class="ms-1">Loading...</span>
</button>

<button class="btn btn-success" type="button" disabled>
  <span class="spinner-grow spinner-grow-sm" role="status" aria-hidden="true"></span>
  <span class="ms-1">Saving...</span>
</button>

<button class="btn btn-outline-secondary" type="button" disabled>
  <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
</button>
```

### Color Variants

Apply Bootstrap contextual colors to spinners using text color utilities.

```html
<div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div>
<div class="spinner-border text-success" role="status"><span class="visually-hidden">Loading...</span></div>
<div class="spinner-border text-danger" role="status"><span class="visually-hidden">Loading...</span></div>
<div class="spinner-border text-warning" role="status"><span class="visually-hidden">Loading...</span></div>
<div class="spinner-border text-info" role="status"><span class="visually-hidden">Loading...</span></div>

<!-- Custom color -->
<div class="spinner-border" style="color: #6f42c1;" role="status"><span class="visually-hidden">Loading...</span></div>
```

### Spinner with Text

Position spinners alongside descriptive text for better UX.

```html
<!-- Centered loading state -->
<div class="d-flex align-items-center justify-content-center py-5">
  <div class="spinner-border text-primary me-3" role="status">
    <span class="visually-hidden">Loading...</span>
  </div>
  <strong>Loading your dashboard...</strong>
</div>

<!-- Inline loading indicator -->
<div class="d-flex align-items-center text-muted">
  <div class="spinner-border spinner-border-sm me-2" role="status">
    <span class="visually-hidden">Loading...</span>
  </div>
  Fetching results, please wait...
</div>
```

### Full-Page Loading Overlay

```html
<style>
  .loading-overlay {
    position: fixed;
    inset: 0;
    background: rgba(255,255,255,0.85);
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    z-index: 2000;
  }
</style>

<div class="loading-overlay">
  <div class="spinner-border text-primary mb-3" style="width: 3rem; height: 3rem;" role="status">
    <span class="visually-hidden">Loading...</span>
  </div>
  <h5>Please wait...</h5>
</div>
```

## Best Practices

1. **Always include `role="status"`** and a `visually-hidden` loading text for accessibility.
2. **Use `spinner-border-sm`** inside buttons to maintain proper button sizing.
3. **Disable buttons** with `disabled` attribute while spinner is shown to prevent duplicate submissions.
4. **Use contextual colors** (`text-primary`, `text-success`) to match the spinner to its context.
5. **Pair spinners with text** so users understand what is loading.
6. **Use `spinner-grow`** for indeterminate waits and `spinner-border` for process-oriented loading.
7. **Set explicit width/height** on large spinners rather than relying on utility classes.
8. **Center loading spinners** in their container using `d-flex justify-content-center align-items-center`.
9. **Use `aria-hidden="true"`** on the spinner icon when `visually-hidden` text provides the label.
10. **Respect `prefers-reduced-motion`** by disabling spinner animation for users who prefer reduced motion.
11. **Use overlay spinners** for full-page operations to block interaction.
12. **Keep loading text concise** - "Loading..." is sufficient for most cases.

## Common Pitfalls

1. **Missing `visually-hidden` text** leaves spinner completely inaccessible to screen readers.
2. **Forgetting `disabled`** on button allows multiple clicks during loading.
3. **Using default size spinners** inside buttons causes layout shifts.
4. **Not removing spinners** after async completion leaves UI in loading state permanently.
5. **Too many spinners** on one page creates a distracting, busy interface.
6. **Using spinners for fast operations** (<200ms) causes flickering rather than reassurance.
7. **Custom color spinners** that fail contrast checks on their background.
8. **Full-page overlay without `pointer-events: none`** or proper z-index management.

## Accessibility Considerations

- Every spinner must have `role="status"` so assistive technologies announce the loading state.
- Include `visually-hidden` text describing what is loading, not just "Loading".
- Button spinners should announce the button's action has started via `aria-live` region updates.
- Full-page overlays should trap focus and prevent interaction with background content.
- Respect `prefers-reduced-motion: reduce` by replacing animation with a static indicator.
- Loading state changes should be announced via `aria-live="polite"` for dynamic updates.

## Responsive Behavior

- Spinners are inline by default and size proportionally within their containers.
- Full-page overlays adapt to viewport size automatically using `position: fixed`.
- Button spinners should not cause buttons to grow - use `spinner-border-sm`.
- Loading text may be hidden on very small screens with `.d-none .d-sm-inline`.
- Centered loading states maintain alignment across all viewport sizes with flexbox.
