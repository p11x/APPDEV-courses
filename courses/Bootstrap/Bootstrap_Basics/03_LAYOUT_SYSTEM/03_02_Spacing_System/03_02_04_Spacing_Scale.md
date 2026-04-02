---
title: "Spacing Scale"
description: "Understand Bootstrap 5's spacing scale system, the $spacer variable, and how to customize spacing values."
difficulty: 1
estimated_time: "15 minutes"
prerequisites:
  - "Basic HTML knowledge"
  - "Bootstrap 5 setup"
tags:
  - spacing
  - scale
  - sass
  - utilities
  - customization
---

## Overview

Bootstrap 5 uses a consistent spacing scale built on a base `$spacer` variable set to `1rem`. All spacing utilities (margin, padding, gap) derive their values from this scale. Understanding the scale ensures you use spacing classes predictably and can customize them for your project.

The default scale uses 0.25rem increments:
- **0** = `0`
- **1** = `0.25rem`
- **2** = `0.5rem`
- **3** = `1rem` (the `$spacer` base value)
- **4** = `1.5rem`
- **5** = `3rem`

These values apply uniformly to `m-*`, `p-*`, and `gap-*` utilities.

## Basic Implementation

### The Scale in Practice

```html
<!-- Size 0: no spacing -->
<div class="p-0 bg-light">p-0 = 0</div>

<!-- Size 1: smallest -->
<div class="p-1 bg-light">p-1 = 0.25rem</div>

<!-- Size 2: small -->
<div class="p-2 bg-light">p-2 = 0.5rem</div>

<!-- Size 3: base (1rem) -->
<div class="p-3 bg-light">p-3 = 1rem (default spacer)</div>

<!-- Size 4: large -->
<div class="p-4 bg-light">p-4 = 1.5rem</div>

<!-- Size 5: extra large -->
<div class="p-5 bg-light">p-5 = 3rem</div>
```

### Visual Comparison

```html
<div class="d-flex flex-column gap-2">
  <div class="p-0 border">p-0 (0)</div>
  <div class="p-1 border">p-1 (0.25rem)</div>
  <div class="p-2 border">p-2 (0.5rem)</div>
  <div class="p-3 border">p-3 (1rem)</div>
  <div class="p-4 border">p-4 (1.5rem)</div>
  <div class="p-5 border">p-5 (3rem)</div>
</div>
```

## Advanced Variations

### Custom Spacing with Sass

Override the spacing scale by modifying Sass variables before importing Bootstrap:

```scss
// Custom $spacer value
$spacer: 1.25rem;

// Custom spacers map
$spacers: (
  0: 0,
  1: 0.25rem,
  2: 0.5rem,
  3: 1rem,
  4: 1.5rem,
  5: 3rem,
  6: 4rem,
  7: 6rem
);

@import "bootstrap/scss/bootstrap";
```

This generates additional utility classes like `p-6`, `m-7`, etc.

### Mapping Spacers to Specific Utilities

Customize individual utility scales:

```scss
$spacer: 1rem;

$spacers: (
  0: 0,
  1: 0.25rem,
  2: 0.5rem,
  3: 1rem,
  4: 2rem,
  5: 4rem
);

// Use custom spacers for margin utilities
$utilities: (
  "margin": (
    values: map-merge($spacers, (auto: auto))
  ),
  "padding": (
    values: $spacers
  )
);

@import "bootstrap/scss/bootstrap";
```

### CSS Custom Properties Approach

For runtime customization without Sass recompilation:

```css
:root {
  --bs-spacer: 1.25rem;
}

.custom-space {
  padding: var(--bs-spacer);
}
```

## Best Practices

1. **Stick to the default scale** unless your design system requires different values.
2. **Use size 3 (`1rem`) as your baseline** for most content spacing.
3. **Jump by one scale step** for visual hierarchy (e.g., `p-2` for tight, `p-3` for normal, `p-4` for spacious).
4. **Do not skip too many scale steps** between related elements; `p-1` next to `p-5` creates jarring contrast.
5. **Customize `$spacer` early** in the project to avoid refactoring later.
6. **Document your scale** so designers and developers reference the same values.
7. **Use the same scale for margins and padding** to maintain visual consistency.
8. **Extend the scale rather than replace it** to preserve Bootstrap's default utility compatibility.
9. **Test custom scales at all breakpoints** to ensure responsive behavior remains predictable.
10. **Keep fractional values consistent**; do not mix `rem` and `px` in your custom scale.
11. **Use negative spacing sparingly** and only from the same scale values.
12. **Review spacing with stakeholders** early in design to confirm the scale meets the project's needs.

## Common Pitfalls

1. **Assuming spacing values are the same across projects**: Custom builds may redefine `$spacers`. Always verify the active scale.
2. **Confusing the scale index with rem values**: `p-3` means index 3, not 3rem. Index 3 equals 1rem by default.
3. **Overriding `$spacer` without updating `$spacers`**: Changing `$spacer` alone does not regenerate the utility classes; you must also update `$spacers`.
4. **Using pixel values in custom scales**: Bootstrap uses `rem` for accessibility and scaling. Mixing `px` breaks zoom behavior.
5. **Forgetting to import Bootstrap after variable overrides**: Variables must be defined before the `@import` statement.
6. **Creating too many custom scale steps**: Excessive steps (e.g., 15 values) make it hard for developers to choose the right class.

## Accessibility Considerations

- Bootstrap's `rem`-based scale respects user font-size preferences in browsers.
- Custom scales should maintain `rem` units to preserve this behavior.
- Adequate spacing (scale 2-3 minimum) around text improves readability for users with cognitive disabilities.
- Ensure interactive elements maintain minimum 44x44px touch targets regardless of spacing scale choices.
- Test spacing at 200% browser zoom to verify layout integrity.

## Responsive Behavior

The spacing scale applies uniformly across all responsive utility classes:

```html
<!-- Scale increases at larger breakpoints -->
<div class="p-1 p-sm-2 p-md-3 p-lg-4 p-xl-5">
  Progressive spacing using the scale
</div>

<!-- Mix scale values responsively -->
<div class="mt-2 mt-md-4 mb-3 mb-lg-5">
  Different vertical spacing per breakpoint
</div>
```

When customizing the Sass scale, all responsive utilities automatically use the new values since they reference the same `$spacers` map.
