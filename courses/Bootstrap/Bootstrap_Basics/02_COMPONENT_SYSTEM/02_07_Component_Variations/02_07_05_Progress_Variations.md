---
title: "Progress Bar Variations"
description: "Create multiple bars, striped and animated progress, labeled bars, height variations, and custom colored progress indicators in Bootstrap 5"
difficulty: 2
tags: [progress, components, variations, loading, indicators]
prerequisites:
  - "Bootstrap 5 progress basics"
  - "CSS background gradients"
---

## Overview

Bootstrap 5 progress bars visualize completion status. Beyond the standard single bar, variations include multiple stacked bars for showing segment breakdowns, striped patterns with optional animation for active processing indicators, inline labels for displaying percentage text, height variations for thin or thick bars, and custom colors beyond Bootstrap's predefined palette. These patterns are essential for dashboards, file uploads, multi-step processes, and data visualization.

## Basic Implementation

### Standard Progress Bar

```html
<div class="progress" role="progressbar" aria-valuenow="65" aria-valuemin="0" aria-valuemax="100">
  <div class="progress-bar" style="width: 65%">65%</div>
</div>
```

### Height Variations

Adjust bar height by setting an explicit height on the `.progress` container.

```html
<div class="progress mb-3" style="height: 4px;" role="progressbar" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100">
  <div class="progress-bar" style="width: 50%"></div>
</div>

<div class="progress mb-3" style="height: 20px;" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100">
  <div class="progress-bar" style="width: 75%">75%</div>
</div>

<div class="progress" style="height: 30px;" role="progressbar" aria-valuenow="40" aria-valuemin="0" aria-valuemax="100">
  <div class="progress-bar fw-bold" style="width: 40%">40% Complete</div>
</div>
```

## Advanced Variations

### Multiple Stacked Bars

Stacked bars show multiple segments within a single progress indicator.

```html
<div class="progress" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100">
  <div class="progress-bar bg-success" style="width: 35%">Completed (35%)</div>
  <div class="progress-bar bg-warning" style="width: 25%">In Progress (25%)</div>
  <div class="progress-bar bg-danger" style="width: 15%">Blocked (15%)</div>
  <div class="progress-bar bg-secondary" style="width: 25%">Not Started (25%)</div>
</div>
```

### Striped and Animated Bars

Striped bars add visual texture, and the `progress-bar-animated` class adds a CSS animation.

```html
<!-- Striped -->
<div class="progress mb-3" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100">
  <div class="progress-bar progress-bar-striped" style="width: 60%"></div>
</div>

<!-- Striped and animated -->
<div class="progress mb-3" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100">
  <div class="progress-bar progress-bar-striped progress-bar-animated" style="width: 75%"></div>
</div>

<!-- Animated with custom color -->
<div class="progress" role="progressbar" aria-valuenow="45" aria-valuemin="0" aria-valuemax="100">
  <div class="progress-bar progress-bar-striped progress-bar-animated bg-info" style="width: 45%">
    Processing...
  </div>
</div>
```

### Labeled Progress Bars

Labels provide context about what the progress bar represents.

```html
<div class="mb-3">
  <label class="form-label d-flex justify-content-between">
    <span>Storage Used</span>
    <span>72 GB / 100 GB</span>
  </label>
  <div class="progress" style="height: 12px;" role="progressbar" aria-valuenow="72" aria-valuemin="0" aria-valuemax="100">
    <div class="progress-bar bg-warning" style="width: 72%"></div>
  </div>
</div>

<div class="mb-3">
  <label class="form-label d-flex justify-content-between">
    <span>Upload Progress</span>
    <span class="text-success">Complete</span>
  </label>
  <div class="progress" style="height: 12px;" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100">
    <div class="progress-bar bg-success" style="width: 100%"></div>
  </div>
</div>
```

### Custom Color Progress Bars

Extend beyond Bootstrap's built-in colors using inline styles or custom CSS.

```html
<div class="progress mb-3" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100">
  <div class="progress-bar" style="width: 60%; background: linear-gradient(90deg, #667eea, #764ba2);">Gradient</div>
</div>

<div class="progress mb-3" role="progressbar" aria-valuenow="80" aria-valuemin="0" aria-valuemax="100">
  <div class="progress-bar" style="width: 80%; background-color: oklch(0.7 0.15 160);">Custom Color</div>
</div>

<style>
  .progress-bar-azure { background-color: #007bff; }
  .progress-bar-coral { background-color: #ff6b6b; }
</style>
<div class="progress" role="progressbar" aria-valuenow="55" aria-valuemin="0" aria-valuemax="100">
  <div class="progress-bar progress-bar-azure" style="width: 55%"></div>
</div>
```

## Best Practices

1. **Always set `role="progressbar"`** with `aria-valuenow`, `aria-valuemin`, and `aria-valuemax`.
2. **Use height classes or inline styles** on the `.progress` container, not the inner bar.
3. **Keep label text concise** - long labels inside thin bars become unreadable.
4. **Use stacked bars** for multi-category breakdowns instead of separate progress elements.
5. **Apply `progress-bar-animated`** only during active operations, not for completed states.
6. **Use `form-label` with `d-flex justify-content-between`** for external label layouts.
7. **Match bar colors to semantic meaning** - green for success, yellow for caution, red for critical.
8. **Set `aria-valuenow` dynamically** via JavaScript when updating progress programmatically.
9. **Use `progress-bar-striped`** for indeterminate or in-progress states.
10. **Prefer thin bars (2-4px)** for background indicators and thick bars (12-20px) for foreground.
11. **Avoid animation on completed bars** to reduce unnecessary visual movement.
12. **Use gradient backgrounds** sparingly and ensure text contrast is maintained.

## Common Pitfalls

1. **Missing ARIA attributes** makes progress bars invisible to assistive technologies.
2. **Setting height on the inner bar** instead of the `.progress` container breaks overflow clipping.
3. **Labels overflowing thin bars** - keep labels outside bars below 16px height.
4. **Animated stripes on 100% complete** bars confuse users about whether work is still happening.
5. **Too many stacked segments** create a confusing rainbow effect - limit to 4-5 segments.
6. **Hardcoding colors** that fail color contrast checks for accessibility.
7. **Not updating `aria-valuenow`** when changing progress via JavaScript.
8. **Using progress bars for non-percentage data** where a chart or meter would be more appropriate.

## Accessibility Considerations

- Progress bars require `role="progressbar"` and all three ARIA value attributes.
- Label text should be associated with the progress bar via `aria-labelledby` when possible.
- Stacked bars should announce the combined total, not individual segments.
- Animated elements should respect `prefers-reduced-motion` media queries.
- Color-blind users should be able to distinguish segments via labels, not just color.
- Screen readers should announce progress updates via `aria-live` regions for dynamic changes.

## Responsive Behavior

- Progress bars are 100% width by default and scale with their container.
- Thin bars (2-4px) remain readable at all viewport sizes.
- Labels inside bars may need to be hidden on very small screens using `.d-none .d-md-inline`.
- Stacked bars with long labels should abbreviate or hide labels on mobile.
- Height variations should not exceed container bounds on narrow screens.
