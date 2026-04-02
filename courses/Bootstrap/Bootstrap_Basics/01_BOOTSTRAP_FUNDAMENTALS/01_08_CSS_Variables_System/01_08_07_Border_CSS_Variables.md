---
title: Border CSS Variables
category: Bootstrap Fundamentals
difficulty: 1
time: 15 min
tags: bootstrap5, css-variables, border, border-radius, border-width, utilities
---

## Overview

Bootstrap 5 controls all border styling through CSS custom properties, including color (`--bs-border-color`), width (`--bs-border-width`), style (`--bs-border-style`), and radius (`--bs-border-radius`, `--bs-border-radius-sm`, `--bs-border-radius-lg`, `--bs-border-radius-pill`, `--bs-border-radius-circle`). These variables ensure consistent border styling across all components and can be overridden globally or scoped to specific sections. The border system also includes translucent variants (`--bs-border-color-translucent`) for subtle divider effects.

## Basic Implementation

Border variables define the default appearance of borders throughout Bootstrap.

```html
<style>
  /* Default border variable values */
  /* --bs-border-color: #dee2e6; */
  /* --bs-border-width: 1px; */
  /* --bs-border-style: solid; */
  /* --bs-border-radius: 0.375rem; */
  /* --bs-border-radius-sm: 0.25rem; */
  /* --bs-border-radius-lg: 0.5rem; */
  /* --bs-border-radius-pill: 50rem; */
  /* --bs-border-radius-circle: 50%; */
</style>

<!-- Elements using border variables -->
<div class="border p-3 mb-3" style="border-color: var(--bs-border-color);">
  Default border color and width
</div>

<div class="p-3 mb-3" style="border: var(--bs-border-width) solid var(--bs-border-color); border-radius: var(--bs-border-radius);">
  Custom element with Bootstrap border variables
</div>
```

Border radius utilities are powered by these variables.

```html
<!-- Border radius variations -->
<div class="rounded-sm bg-light p-3 mb-2">Small radius (sm)</div>
<div class="rounded bg-light p-3 mb-2">Default radius</div>
<div class="rounded-lg bg-light p-3 mb-2">Large radius (lg)</div>
<div class="rounded-pill bg-primary text-white p-3 mb-2 px-4">Pill radius</div>
<div class="rounded-circle bg-success text-white d-flex align-items-center justify-content-center mb-2" style="width: 80px; height: 80px;">Circle</div>
```

## Advanced Variations

Override border variables for section-specific styling.

```html
<style>
  .dark-border-section {
    --bs-border-color: #495057;
    --bs-border-color-translucent: rgba(255, 255, 255, 0.15);
    background-color: #212529;
    color: #fff;
    padding: 1.5rem;
  }
</style>

<div class="dark-border-section">
  <div class="border p-3 mb-3">
    Border adapts to dark section via variable override
  </div>
  <div class="border-top pt-3">
    Top border also respects the custom color
  </div>
</div>
```

Creating design systems with custom border radius scales.

```html
<style>
  .design-system {
    --bs-border-radius: 0.5rem;
    --bs-border-radius-sm: 0.25rem;
    --bs-border-radius-lg: 1rem;
    --bs-border-radius-pill: 100rem;
  }
</style>

<div class="design-system">
  <button class="btn btn-primary rounded me-2">Default</button>
  <button class="btn btn-primary rounded-sm me-2">Small</button>
  <button class="btn btn-primary rounded-lg me-2">Large</button>
  <button class="btn btn-primary rounded-pill">Pill</button>
</div>
```

Combining border variables with component overrides.

```html
<style>
  /* Sharp design: no border radius */
  .sharp-theme {
    --bs-border-radius: 0;
    --bs-border-radius-sm: 0;
    --bs-border-radius-lg: 0;
    --bs-border-radius-pill: 0;
  }
</style>

<div class="sharp-theme">
  <div class="card mb-3">
    <div class="card-body">Sharp card with no rounded corners</div>
  </div>
  <button class="btn btn-primary">Sharp Button</button>
</div>
```

## Best Practices

1. **Override border variables globally** - Change `--bs-border-color` on `:root` to update all borders at once.
2. **Use translucent borders for subtlety** - Apply `--bs-border-color-translucent` for soft divider lines that blend with backgrounds.
3. **Maintain consistent border radius** - Use Bootstrap's radius variables (`sm`, default, `lg`) for proportional corner rounding.
4. **Update border colors for dark themes** - Light borders are invisible on dark backgrounds. Override `--bs-border-color` with theme changes.
5. **Use `border-0` to remove borders** - Bootstrap's `border-0` utility removes all borders. Use it alongside border variables for explicit control.
6. **Combine with shadow for depth** - Borders and shadows together create clear element boundaries and perceived depth.
7. **Apply pill radius for tags and badges** - `rounded-pill` creates fully rounded ends ideal for tags and status badges.
8. **Use circle radius for avatars** - `rounded-circle` with equal width/height creates circular profile images.
9. **Test border visibility** - Verify that borders are visible against all background colors in your theme.
10. **Document custom border values** - Record any overridden border variables in your design system documentation.

## Common Pitfalls

1. **Invisible borders on dark backgrounds** - Default `--bs-border-color` (#dee2e6) is nearly invisible on dark backgrounds. Always override for dark themes.
2. **Border radius inconsistency** - Using `rounded` on some elements and `rounded-lg` on others without a systematic approach creates visual inconsistency.
3. **Borders adding to element width** - With `border-box` sizing, borders are included in the declared width. A `w-100` element with borders is narrower inside.
4. **Not removing default borders** - Some elements like inputs have default borders. Forgetting to use `border-0` when needed creates unexpected styling.
5. **Pill radius on narrow elements** - `rounded-pill` on very narrow elements creates disproportionately large curves. Use fixed-width containers instead.

## Accessibility Considerations

Borders are important visual indicators for interactive elements and form fields. Ensure that form inputs have visible borders to indicate their boundaries. When overriding border colors, maintain sufficient contrast with the background. Focus indicators often use borders; ensure custom border styles do not conflict with focus ring visibility. Borders around clickable regions help users with motor impairments identify interactive areas. Avoid removing borders from form controls without providing alternative visual indicators.

## Responsive Behavior

Border CSS variables do not change with viewport size by default. Create responsive border behavior by overriding variables within media queries. For example, use `--bs-border-radius: 0` on mobile for a flat design and increase radius at larger breakpoints. Border width can also scale responsively: thicker borders on desktop for visual emphasis, thinner on mobile to save space. Combine responsive border variables with Bootstrap's responsive border utility classes (`border-md-top`) for adaptive layouts.
