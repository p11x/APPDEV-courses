---
title: Progress Bars
category: Component System
difficulty: 1
time: 25 min
tags: bootstrap5, progress, progress-bar, loading, indicators
---

## Overview

Bootstrap progress bars display the completion status of a task using CSS custom properties for flexible styling. The `progress` wrapper provides the background track while `progress-bar` fills with color to represent progress. Progress bars support striped and animated variants, text labels, custom heights, and stacked multiple bars for complex visualizations.

## Basic Implementation

A progress bar requires a `.progress` container and a `.progress-bar` child with an inline `width` or `style` attribute.

```html
<!-- Basic progress bar -->
<div class="progress">
  <div class="progress-bar" role="progressbar" style="width: 25%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100">25%</div>
</div>

<!-- Progress bar without label -->
<div class="progress">
  <div class="progress-bar" role="progressbar" style="width: 50%;" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100"></div>
</div>

<!-- Color variants -->
<div class="progress">
  <div class="progress-bar bg-success" role="progressbar" style="width: 75%;" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100">75%</div>
</div>
<div class="progress">
  <div class="progress-bar bg-danger" role="progressbar" style="width: 40%;" aria-valuenow="40" aria-valuemin="0" aria-valuemax="100">40%</div>
</div>
<div class="progress">
  <div class="progress-bar bg-info" role="progressbar" style="width: 90%;" aria-valuenow="90" aria-valuemin="0" aria-valuemax="100">90%</div>
</div>
```

## Advanced Variations

```html
<!-- Striped and animated progress bars -->
<div class="progress">
  <div class="progress-bar progress-bar-striped" role="progressbar" style="width: 35%;" aria-valuenow="35" aria-valuemin="0" aria-valuemax="100">35%</div>
</div>
<div class="progress">
  <div class="progress-bar progress-bar-striped progress-bar-animated bg-success" role="progressbar" style="width: 65%;" aria-valuenow="65" aria-valuemin="0" aria-valuemax="100">65%</div>
</div>
```

```html
<!-- Custom height progress bars -->
<div class="progress" style="height: 4px;">
  <div class="progress-bar" role="progressbar" style="width: 50%;" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100"></div>
</div>
<div class="progress" style="height: 20px;">
  <div class="progress-bar bg-warning" role="progressbar" style="width: 70%;" aria-valuenow="70" aria-valuemin="0" aria-valuemax="100">70%</div>
</div>
```

```html
<!-- Stacked progress bars -->
<div class="progress">
  <div class="progress-bar bg-success" role="progressbar" style="width: 30%;" aria-valuenow="30" aria-valuemin="0" aria-valuemax="100">30%</div>
  <div class="progress-bar bg-warning" role="progressbar" style="width: 20%;" aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">20%</div>
  <div class="progress-bar bg-danger" role="progressbar" style="width: 15%;" aria-valuenow="15" aria-valuemin="0" aria-valuemax="100">15%</div>
</div>

<!-- Using CSS variables for dynamic width -->
<div class="progress">
  <div class="progress-bar" role="progressbar" style="width: var(--bs-progress-width, 45%);" aria-valuenow="45" aria-valuemin="0" aria-valuemax="100">45%</div>
</div>
```

## Best Practices

1. Always include `role="progressbar"` and `aria-valuenow`, `aria-valuemin`, `aria-valuemax` attributes.
2. Use color variants (`bg-success`, `bg-warning`, `bg-danger`) to convey semantic meaning.
3. Include visible labels for progress bars that represent user-facing metrics.
4. Use `progress-bar-animated` to signal ongoing active processing.
5. Keep progress bar height minimal (4-8px) for most UI contexts.
6. Use stacked bars to show composite progress from multiple sources.
7. Combine with `d-none` or `d-flex` utilities for conditional display.
8. Update `aria-valuenow` dynamically when progress changes via JavaScript.
9. Use CSS custom properties for dynamic width updates without inline styles.
10. Provide text labels inside or beside the bar for context.
11. Reserve `bg-success` color for completed or near-complete states.

## Common Pitfalls

1. **Missing ARIA attributes.** Screen readers cannot convey progress without `role="progressbar"` and value attributes.
2. **Hardcoded widths without JavaScript.** Static progress bars mislead users about actual progress.
3. **Animating completed progress.** Remove `progress-bar-animated` when progress reaches 100%.
4. **Using thin bars without labels.** A 2px progress bar with no text is meaningless without context.
5. **Overflowing labels.** Long text inside narrow bars gets clipped; use external labels instead.
6. **Ignoring color accessibility.** Relying solely on color fails users with color blindness; pair with text or icons.

## Accessibility Considerations

Progress bars require `role="progressbar"` with `aria-valuenow`, `aria-valuemin`, and `aria-valuemax` to communicate state to assistive technology. When progress updates dynamically, use `aria-live="polite"` on a parent element or update ARIA attributes via JavaScript. Provide visible text labels alongside thin bars that cannot contain internal text. Avoid color-only communication by supplementing with percentage values or status text.

## Responsive Behavior

Progress bars fill their parent container by default, making them inherently responsive. Avoid fixed pixel widths. Use Bootstrap utility classes to hide or show progress bars at specific breakpoints (`d-none d-md-block`). For mobile, keep bar height between 4-8px and ensure labels remain readable. Stacked bars should use distinct colors for clarity on small screens where text labels may overlap.
