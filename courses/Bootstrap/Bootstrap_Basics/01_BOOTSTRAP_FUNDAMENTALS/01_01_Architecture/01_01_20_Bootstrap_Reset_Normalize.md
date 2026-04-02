---
title: "Bootstrap Reset & Normalize"
lesson: "01_01_20"
difficulty: "1"
topics: ["reboot", "normalize", "box-sizing", "reset", "element-defaults"]
estimated_time: "20 minutes"
---

# Bootstrap Reset & Normalize

## Overview

Bootstrap uses **Reboot**, its own CSS reset built on top of Normalize.css, to establish consistent cross-browser defaults. Reboot goes beyond normalization by opinionatedly resetting margins, font sizes, and box-sizing to provide a clean foundation. The `box-sizing: border-box` rule is applied globally, margin is removed from headings and paragraphs, and form elements receive consistent base styling. Understanding Reboot's behavior prevents unexpected spacing, sizing, and rendering issues across browsers.

Reboot is automatically included when you import the full Bootstrap CSS or SCSS. It can also be imported independently as `bootstrap-reboot.css` for projects that want only the reset layer without Bootstrap's component styles.

## Basic Implementation

### Including Reboot Only

```html
<!-- Just the reset, no Bootstrap components -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap-reboot.min.css" rel="stylesheet">
```

### Reboot via SCSS

```scss
// Import only reboot
@import "node_modules/bootstrap/scss/functions";
@import "node_modules/bootstrap/scss/variables";
@import "node_modules/bootstrap/scss/mixins";
@import "node_modules/bootstrap/scss/root";
@import "node_modules/bootstrap/scss/reboot";
```

### Key Reboot Rules

```css
/* Global box-sizing */
*, *::before, *::after {
  box-sizing: border-box;
}

/* Root defaults */
html {
  -webkit-text-size-adjust: 100%;
  scroll-behavior: smooth;
}

/* Body defaults */
body {
  margin: 0;
  font-family: var(--bs-body-font-family);
  font-size: var(--bs-body-font-size);
  line-height: var(--bs-body-line-height);
  color: var(--bs-body-color);
  background-color: var(--bs-body-bg);
  -webkit-text-size-adjust: 100%;
  -webkit-tap-highlight-color: transparent;
}
```

## Advanced Variations

### Custom Reboot Overrides

```scss
// Override before importing Bootstrap
$font-family-base: 'Inter', sans-serif;
$font-size-base: 1rem;
$line-height-base: 1.6;
$body-bg: #f8f9fa;
$body-color: #212529;

// Reboot will use these values
@import "node_modules/bootstrap/scss/bootstrap";
```

### Headings Reset

```css
/* Reboot removes margin from all headings */
h1, h2, h3, h4, h5, h6 {
  margin-top: 0;
  margin-bottom: 0.5rem;
  font-weight: 500;
  line-height: 1.2;
}

/* Relative sizing */
h1 { font-size: calc(1.375rem + 1.5vw); }
h2 { font-size: calc(1.325rem + 0.9vw); }
h3 { font-size: calc(1.3rem + 0.6vw); }
```

### List Reset

```css
/* Lists lose bullets and padding */
ul, ol {
  padding-left: 2rem; /* Bootstrap keeps some padding */
}

/* Unstyled lists */
.list-unstyled {
  padding-left: 0;
  list-style: none;
}
```

### Form Element Resets

```css
/* Consistent form element styling */
input, button, select, optgroup, textarea {
  margin: 0;
  font-family: inherit;
  font-size: inherit;
  line-height: inherit;
}

/* Remove default appearance */
button, select {
  text-transform: none;
}

/* Buttons inherit font */
button, [type="button"], [type="reset"], [type="submit"] {
  -webkit-appearance: button;
}
```

### Table Resets

```css
/* Tables get border-collapse */
table {
  caption-side: bottom;
  border-collapse: collapse;
}

/* Consistent cell spacing */
th {
  text-align: inherit;
  text-align: -webkit-match-parent;
}
```

## Best Practices

1. **Always include Reboot** - Prevents cross-browser rendering inconsistencies.
2. **Import Reboot before any custom CSS** - Ensures your styles override defaults.
3. **Use `bootstrap-reboot.css` when you only need the reset** - Avoid loading unnecessary component CSS.
4. **Do not fight Reboot's box-sizing reset** - `border-box` everywhere is the correct default.
5. **Use `.list-unstyled` for navigation-style lists** - Don't manually reset list styles.
6. **Understand that Reboot removes heading margins** - Add them back with utilities (`.mb-3`) or custom CSS.
7. **Test in multiple browsers after applying Reboot** - Especially Safari and older Edge.
8. **Keep Reboot's focus styles** - The `:focus-visible` outline is accessible by default.
9. **Do not override `html { scroll-behavior: smooth }`** unless causing accessibility issues - Some screen readers warn against smooth scroll.
10. **Use Reboot's CSS custom properties** - `--bs-body-*` variables make runtime theming easier.
11. **Review Reboot after Bootstrap upgrades** - New resets are added occasionally.
12. **Check that `prefers-reduced-motion` is respected** - Reboot disables smooth scroll for users who prefer reduced motion.

## Common Pitfalls

1. **Forgetting that Reboot removes heading margins** - Headings stack without vertical spacing; add `.mb-3` or similar.
2. **Overriding `box-sizing` after Reboot** - Mixing `content-box` and `border-box` causes layout calculation bugs.
3. **Expecting lists to have bullets when using Bootstrap** - Some list contexts are reset; use explicit `list-style` if needed.
4. **Not testing form elements across browsers** - Reboot normalizes them, but OS-level styling can still leak through.
5. **Removing `outline` on `:focus-visible`** - Creates inaccessible focus indicators; always provide an alternative.

## Accessibility Considerations

Reboot includes accessible defaults: focus indicators use `outline` (visible to keyboard users), `prefers-reduced-motion` disables smooth scrolling and transitions, and form elements inherit fonts for consistency. The `[tabindex="-1"]:focus` rule removes the focus outline only for programmatically focused elements, not for keyboard navigation. When overriding Reboot, never remove focus indicators entirely and always test keyboard navigation.

## Responsive Behavior

Reboot itself is not responsive - it applies consistent base styles at all viewport sizes. However, Bootstrap's responsive typography (fluid `h1`-`h6` sizing via `calc()` and `vw` units) is defined in Reboot, making headings naturally scale with viewport width. The `html { scroll-behavior: smooth }` rule and `-webkit-text-size-adjust: 100%` prevent mobile browsers from applying their own text scaling overrides.
