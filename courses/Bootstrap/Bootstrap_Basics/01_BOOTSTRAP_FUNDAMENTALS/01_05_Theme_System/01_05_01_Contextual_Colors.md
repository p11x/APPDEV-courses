---
tags:
  - bootstrap
  - contextual-colors
  - theming
  - colors
  - utilities
category: Bootstrap Fundamentals
difficulty: 1
time: 30 minutes
---

# Contextual Colors

## Overview

Bootstrap 5 provides a comprehensive set of contextual colors built on a semantic color system. These colors communicate meaning to users through visual cues — red signals danger, green signals success, and blue conveys informational content. Understanding how contextual colors work is foundational to building accessible, visually consistent Bootstrap interfaces.

Bootstrap ships with eight base contextual colors: **primary**, **secondary**, **success**, **danger**, **warning**, **info**, **light**, and **dark**. Each color serves a specific semantic purpose and is applied through utility classes such as `text-{color}`, `bg-{color}`, and `border-{color}`.

The contextual color system is tightly integrated with Bootstrap's CSS custom properties. When you reference `.text-primary`, Bootstrap resolves the color value via `var(--bs-primary)`. This means that changing a theme color propagates automatically to every utility and component that references it.

Understanding semantic meaning is critical. `text-danger` is not simply "red text" — it communicates that the content relates to an error, critical alert, or destructive action. Misusing contextual colors (e.g., using `text-danger` for decorative purposes) confuses users and harms accessibility.

Bootstrap's color system also extends to components. Alert variants (`alert-success`, `alert-warning`), badge backgrounds (`bg-primary`, `bg-info`), and button styles (`btn-danger`, `btn-outline-success`) all draw from the same eight contextual colors, ensuring visual consistency across the entire UI.

## Basic Implementation

The simplest way to apply contextual colors is through text and background utility classes.

```html
<!-- Text colors -->
<p class="text-primary">Primary text — main brand or interactive element</p>
<p class="text-secondary">Secondary text — supporting or muted content</p>
<p class="text-success">Success text — positive outcome or confirmation</p>
<p class="text-danger">Danger text — error, warning, or destructive action</p>
<p class="text-warning">Warning text — caution or something needs attention</p>
<p class="text-info">Info text — neutral information or tips</p>
<p class="text-light bg-dark">Light text on dark background</p>
<p class="dark">Dark text — primary body text or headings</p>
```

Background colors follow the same naming convention:

```html
<div class="p-3 mb-2 bg-primary text-white">Primary background</div>
<div class="p-3 mb-2 bg-secondary text-white">Secondary background</div>
<div class="p-3 mb-2 bg-success text-white">Success background</div>
<div class="p-3 mb-2 bg-danger text-white">Danger background</div>
<div class="p-3 mb-2 bg-warning text-dark">Warning background (dark text)</div>
<div class="p-3 mb-2 bg-info text-dark">Info background (dark text)</div>
<div class="p-3 mb-2 bg-light text-dark">Light background</div>
<div class="p-3 mb-2 bg-dark text-white">Dark background</div>
```

Notice that `bg-warning`, `bg-info`, and `bg-light` pair with `text-dark` rather than `text-white`. These lighter colors fail contrast requirements with white text, so Bootstrap recommends dark text on them.

Contextual colors apply to components as well:

```html
<!-- Buttons -->
<button class="btn btn-primary">Primary</button>
<button class="btn btn-success">Success</button>
<button class="btn btn-danger">Danger</button>

<!-- Alerts -->
<div class="alert alert-success" role="alert">
  Operation completed successfully.
</div>

<div class="alert alert-danger" role="alert">
  An error occurred. Please try again.
</div>

<!-- Badges -->
<span class="badge bg-primary">New</span>
<span class="badge bg-danger">Critical</span>
<span class="badge bg-success">Approved</span>
```

## Advanced Variations

Beyond the base classes, Bootstrap provides outline and subtle variations for each color. Outline variants strip the background and render only a colored border, useful for secondary actions.

```html
<!-- Outline buttons -->
<button class="btn btn-outline-primary">Primary</button>
<button class="btn btn-outline-secondary">Secondary</button>
<button class="btn btn-outline-success">Success</button>
<button class="btn btn-outline-danger">Danger</button>
<button class="btn btn-outline-warning">Warning</button>
<button class="btn btn-outline-info">Info</button>
```

Subtle color variants (introduced in Bootstrap 5.3) provide softer backgrounds for less prominent UI elements:

```html
<div class="p-3 mb-2 bg-primary-subtle border border-primary-subtle">
  Primary subtle — lighter version for backgrounds
</div>
<div class="p-3 mb-2 bg-success-subtle border border-success-subtle">
  Success subtle — soft confirmation styling
</div>
<div class="p-3 mb-2 bg-danger-subtle border border-danger-subtle">
  Danger subtle — gentle error indication
</div>
```

Emphasis text variants provide stronger text colors paired with subtle backgrounds:

```html
<div class="bg-success-subtle p-3">
  <p class="text-success-emphasis fw-bold">Confirmed</p>
  <p class="text-success-emphasis">Your account has been verified.</p>
</div>
```

You can combine text and background colors for inline highlighting:

```html
<p>
  The status is <span class="badge bg-success">Active</span>.
  Review <span class="text-danger fw-semibold">critical items</span> first.
</p>
```

Color utilities also extend to SVGs and icons:

```html
<svg class="text-primary" width="24" height="24">
  <use xlink:href="#icon-check"></use>
</svg>

<i class="bi bi-exclamation-triangle-fill text-warning fs-3"></i>
```

## Best Practices

1. **Use colors for their semantic meaning, not decoration.** `text-danger` signals errors. Using it for non-erroneous content because you like the color violates semantic intent.

2. **Pair light backgrounds with dark text.** `bg-warning`, `bg-info`, and `bg-light` require `text-dark` for sufficient contrast. Never assume all colors work with white text.

3. **Combine text and background utilities for contrast.** Always verify that text is readable on its background. Use browser dev tools or contrast checkers to validate.

4. **Leverage subtle variants for secondary UI regions.** Reserve strong contextual colors for alerts, errors, and primary actions. Use `bg-*-subtle` for less critical areas like info cards or secondary notifications.

5. **Maintain color consistency across components.** If a form field shows an error, the alert, the input border, and the helper text should all use `danger`. Do not mix colors for the same state.

6. **Use `text-reset` to inherit parent color.** When nesting colored elements, `text-reset` allows child elements to inherit the parent's text color instead of defaulting to the body color.

7. **Apply colors through utility classes, not inline styles.** Using `style="color: red"` bypasses Bootstrap's theme system. When the theme changes, inline styles remain static.

8. **Respect the warning/info lightness.** These colors are intentionally lighter. Do not darken them with custom CSS unless you also update the corresponding text color for contrast.

9. **Test color choices in both light and dark modes.** Colors behave differently under `data-bs-theme="dark"`. A color that looks correct in light mode may be unreadable in dark mode.

10. **Use outline button variants for secondary actions.** Reserve filled button colors for the primary action on a page. Outline buttons reduce visual weight.

11. **Document custom color extensions.** If your project extends the color palette beyond the eight base colors, document the new semantic meanings so the team applies them consistently.

12. **Avoid using color as the sole indicator.** Pair colors with icons, text labels, or patterns. Users with color vision deficiencies may not distinguish between red and green.

## Common Pitfalls

1. **Using `text-danger` or `text-success` as the only way to communicate state.** Colorblind users may not distinguish between red and green. Always include a text label or icon alongside the color change.

2. **Applying `text-white` on `bg-warning` or `bg-info`.** These colors do not meet WCAG AA contrast with white text. Use `text-dark` instead.

3. **Hardcoding hex colors instead of using contextual classes.** Writing `style="background-color: #198754"` instead of `bg-success` disconnects the element from the theme system. If Bootstrap's green changes, your hardcoded value does not update.

4. **Overriding Bootstrap's color definitions in global CSS.** Changing `.text-primary { color: #ff0000 }` globally creates confusion. Override colors through Sass variables or CSS custom properties at the root level instead.

5. **Using too many colors on a single screen.** If every element is a different color, none stands out. Limit the palette to 2–3 contextual colors per view.

6. **Forgetting to update dark mode overrides.** If you customize `--bs-primary` but not `--bs-primary-rgb`, opacity-based utilities (`bg-primary bg-opacity-50`) will break in dark mode because the RGB value falls back to the light mode value.

7. **Inconsistent color usage between form states and feedback.** Adding `is-invalid` (red border) to an input but showing a green alert for the same field confuses users. Keep feedback colors synchronized.

## Accessibility Considerations

Contextual colors play a direct role in accessibility. Bootstrap's default color palette is designed to meet WCAG 2.1 AA contrast ratios when paired correctly, but misuse can violate those standards.

Always verify contrast ratios. Bootstrap's `text-success` on a white background meets AA for normal text (4.5:1), but `text-warning` on white does not. The documentation recommends pairing warning and info backgrounds with dark text.

Do not rely on color alone. The WCAG 1.4.1 guideline (Use of Color) requires that color is not the only visual means of conveying information. For example, an error state should use `text-danger` combined with an icon and descriptive text.

```html
<!-- Correct: color + icon + text -->
<div class="alert alert-danger d-flex align-items-center" role="alert">
  <i class="bi bi-exclamation-triangle-fill me-2"></i>
  <div>Password must be at least 8 characters.</div>
</div>

<!-- Incorrect: color alone -->
<p class="text-danger">Invalid input</p>
```

Screen readers do not interpret visual colors. Always use `role`, `aria-label`, and semantic HTML alongside color utilities to ensure assistive technology communicates the same meaning.

Focus indicators use Bootstrap's primary color by default. If you change `--bs-primary` to a low-contrast color, focus outlines may become invisible. Always test focus visibility after theme changes.

## Responsive Behavior

Contextual color utilities are not responsive by default — they apply at all breakpoints. However, you can combine them with Bootstrap's display and flex utilities to adapt color presentation.

```html
<!-- Different emphasis at different breakpoints -->
<div class="bg-success-subtle bg-lg-danger-subtle p-3">
  This changes emphasis at the large breakpoint.
</div>
```

Note that Bootstrap does not include responsive color utilities out of the box. The above pattern requires a custom Sass extension. The standard approach is to use color utilities on elements that remain consistent across breakpoints.

For responsive layouts, ensure that colored elements remain readable at all sizes. A `bg-primary` hero section with white text may be fine on desktop but overwhelming on mobile if it fills the screen. Consider using subtle variants on smaller viewports.

```html
<!-- Responsive alert sizing -->
<div class="alert alert-info alert-md-success" role="alert">
  Status message that adapts to screen size.
</div>
```

In most projects, contextual colors work responsively without modification. The primary concern is maintaining contrast when background colors meet text at varying font sizes. Larger text (18px+ or 14px+ bold) has a lower contrast requirement (3:1 instead of 4.5:1), which gives you more flexibility at larger breakpoints with larger typography.