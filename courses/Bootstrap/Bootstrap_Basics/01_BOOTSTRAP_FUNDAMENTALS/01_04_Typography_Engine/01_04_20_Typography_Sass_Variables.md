---
title: "Typography Sass Variables"
topic: "Typography Engine"
subtopic: "Typography Sass Variables"
difficulty: 2
duration: "30 minutes"
prerequisites: ["Font Stack Customization", "Heading Typography"]
learning_objectives:
  - Customize Bootstrap's typography Sass variables
  - Override $font-size-base, $headings-font-weight, and related variables
  - Create a custom typography scale using Sass configuration
---

## Overview

Bootstrap's typography system is controlled by Sass variables in `_variables.scss`. Key variables include `$font-size-base` (root body size), `$headings-font-weight` (heading boldness), `$line-height-base` (body line height), and font family variables for base and heading text. Overriding these variables before importing Bootstrap creates a consistent, customized typography system without writing custom CSS for every element.

## Basic Implementation

Overriding base typography variables in a custom SCSS file:

```scss
// _custom-variables.scss
$font-size-base: 1rem;             // 16px body text
$font-size-lg: 1.125rem;           // 18px large text
$font-size-sm: 0.875rem;           // 14px small text

$line-height-base: 1.6;            // Body line height
$line-height-lg: 1.5;              // Large text line height
$line-height-sm: 1.4;              // Small text line height

$body-bg: #ffffff;
$body-color: #212529;
```

Customizing heading variables:

```scss
// _custom-variables.scss
$headings-font-family: null;        // Inherit from $font-family-base
$headings-font-weight: 700;         // Bold headings
$headings-line-height: 1.2;         // Tight heading line height
$headings-color: null;              // Inherit from $body-color

$h1-font-size: 2.5rem;             // 40px
$h2-font-size: 2rem;               // 32px
$h3-font-size: 1.75rem;            // 28px
$h4-font-size: 1.5rem;             // 24px
$h5-font-size: 1.25rem;            // 20px
$h6-font-size: 1rem;               // 16px
```

Importing custom variables before Bootstrap:

```scss
// main.scss
@import 'custom-variables';
@import 'bootstrap/scss/bootstrap';

// Custom styles below...
.custom-section {
  // Uses overridden variables
}
```

## Advanced Variations

Complete typography customization with modular scale:

```scss
// Typography scale using a 1.25 ratio (Major Third)
$font-size-base: 1rem;
$type-scale-ratio: 1.25;

$h6-font-size: $font-size-base;                                          // 1rem
$h5-font-size: $font-size-base * $type-scale-ratio;                      // 1.25rem
$h4-font-size: $font-size-base * pow($type-scale-ratio, 2);             // 1.563rem
$h3-font-size: $font-size-base * pow($type-scale-ratio, 3);             // 1.953rem
$h2-font-size: $font-size-base * pow($type-scale-ratio, 4);             // 2.441rem
$h1-font-size: $font-size-base * pow($type-scale-ratio, 5);             // 3.052rem
$display-font-sizes: (
  1: $font-size-base * pow($type-scale-ratio, 8),                       // 6rem
  2: $font-size-base * pow($type-scale-ratio, 7),                       // 4.8rem
  3: $font-size-base * pow($type-scale-ratio, 6),                       // 3.8rem
  4: $font-size-base * pow($type-scale-ratio, 5),                       // 3.1rem
  5: $font-size-base * pow($type-scale-ratio, 4),                       // 2.4rem
  6: $font-size-base * pow($type-scale-ratio, 3),                       // 2rem
);
```

Font weight and style customization:

```scss
$font-weight-lighter: lighter;
$font-weight-light: 300;
$font-weight-normal: 400;
$font-weight-semibold: 600;
$font-weight-bold: 700;
$font-weight-bolder: bolder;

$headings-font-weight: 700;
$lead-font-weight: 400;
$dt-font-weight: $font-weight-bold;
```

Spacing and decoration variables:

```scss
$letter-spacing-base: 0;            // Normal letter spacing
$letter-spacing-tight: -0.02em;     // Tighter for headings
$letter-spacing-wide: 0.05em;       // Wider for labels

$paragraph-margin-bottom: 1.25rem;
$headings-margin-bottom: 0.5rem;
$lead-font-size: 1.25rem;
$lead-font-weight: 300;

$small-font-size: 87.5%;           // 14px from 16px base
$text-muted: #6c757d;
```

## Best Practices

1. Define all typography overrides in a separate `_custom-variables.scss` file for maintainability.
2. Import custom variables before Bootstrap's SCSS to ensure overrides take precedence.
3. Use `rem` units for all font size variables to respect user browser settings.
4. Apply a consistent type scale ratio (1.2, 1.25, or 1.333) for harmonious heading sizes.
5. Set `$headings-font-weight` to 600-700 for clear visual hierarchy.
6. Use `null` to let variables inherit from parent (e.g., `$headings-color: null`).
7. Override display font sizes using the `$display-font-sizes` map for `display-1` through `display-6`.
8. Test typography changes across all breakpoints after variable overrides.
9. Document custom variable values in comments for team reference.
10. Use Bootstrap's built-in variable defaults as a starting point before customization.

## Common Pitfalls

- **Importing variables after Bootstrap**: Variables defined after `@import 'bootstrap'` don't override defaults.
- **Using `px` instead of `rem`**: Fixed pixel values ignore user browser font preferences.
- **Overriding too many variables**: Changing every variable makes the system harder to maintain than custom CSS.
- **Ignoring cascade effects**: Changing `$font-size-base` affects all `rem`-based sizing throughout Bootstrap.
- **Forgetting `$enable-*` flags**: Typography options like `display-*` can be disabled via `$enable-*` Sass flags.
- **Missing `pow()` function**: Custom type scales using `pow()` require the Sass math module (`@use "sass:math"`).
- **Not testing responsive**: Custom heading sizes may need responsive breakpoint adjustments beyond variable overrides.

## Accessibility Considerations

- Ensure `$font-size-base` is at least 1rem (16px) for accessible body text.
- Maintain clear heading size hierarchy — `$h1-font-size` must be larger than `$h2-font-size`.
- Keep `$line-height-base` at 1.5 or above for comfortable reading.
- Verify contrast ratios after customizing `$body-color` and `$headings-color`.
- Test with browser zoom (200-400%) to ensure overridden sizes scale correctly.
- Respect user `prefers-reduced-motion` settings for any typography transitions.

## Responsive Behavior

Bootstrap's typography variables don't have built-in responsive variants, but you can create responsive typography using CSS custom properties alongside Sass overrides:

```scss
// _custom-variables.scss
$font-size-base: clamp(0.9375rem, 1vw + 0.5rem, 1.125rem);

$h1-font-size: clamp(2rem, 3vw + 1rem, 3.5rem);
$h2-font-size: clamp(1.5rem, 2vw + 0.75rem, 2.5rem);
$h3-font-size: clamp(1.25rem, 1.5vw + 0.5rem, 2rem);
```

This produces headings that scale fluidly between minimum and maximum sizes without media queries. The `clamp()` function in Sass variables generates CSS that responds to viewport width while maintaining defined limits.
