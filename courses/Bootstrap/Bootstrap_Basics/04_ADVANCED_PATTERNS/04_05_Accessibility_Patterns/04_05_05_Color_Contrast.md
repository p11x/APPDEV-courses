---
title: "Color Contrast in Bootstrap"
module: "04_ADVANCED_PATTERNS"
lesson: "04_05_Accessibility_Patterns"
file: "04_05_05_Color_Contrast.md"
difficulty: 2
description: "WCAG AA/AAA contrast ratios, Bootstrap color contrast calculation, testing tools, ensuring sufficient contrast in custom themes"
---

## Overview

Color contrast ensures text and interactive elements are readable for users with low vision or color blindness. WCAG defines minimum contrast ratios that must be met for conformance. Bootstrap ships with contrast-safe color combinations built in, but custom themes and overrides require manual verification.

WCAG contrast requirements:

| Level | Normal Text | Large Text (18px+ or 14px bold+) | UI Components |
|-------|-------------|----------------------------------|---------------|
| AA | 4.5:1 | 3:1 | 3:1 |
| AAA | 7:1 | 4.5:1 | 3:1 |

Bootstrap's default color palette is designed to meet AA compliance for most use cases. The `color-contrast()` function in Bootstrap's Sass automatically selects black or white text based on the background color.

## Basic Implementation

### Bootstrap's Built-in Contrast

Bootstrap text color utilities pair with backgrounds that meet contrast requirements:

```html
<!-- These combinations are pre-validated for AA compliance -->
<p class="text-primary bg-light p-2">Primary text on light background</p>
<p class="text-white bg-dark p-2">White text on dark background</p>
<p class="text-dark bg-warning p-2">Dark text on warning background</p>
<p class="text-white bg-danger p-2">White text on danger background</p>
<p class="text-white bg-primary p-2">White text on primary background</p>
```

### Custom Color with Contrast Check

```css
/* Custom color pair - verify contrast manually */
.custom-alert {
  background-color: #e8f4fd; /* Light blue */
  color: #0a4c7a;            /* Dark blue - 7.2:1 contrast ratio */
  padding: 1rem;
  border-radius: 0.375rem;
}

.custom-badge {
  background-color: #6f42c1; /* Bootstrap purple */
  color: #ffffff;             /* White - 5.3:1 contrast ratio */
  padding: 0.25rem 0.75rem;
  border-radius: 0.25rem;
}
```

### Testing Contrast with DevTools

```html
<!-- Use browser devtools to verify -->
<div style="background-color: #yourcolor; color: #yourtextcolor;">
  <!-- Inspect element → Computed → Color contrast -->
  Sample text to verify contrast ratio
</div>
```

## Advanced Variations

### Sass Color Contrast Function

Bootstrap uses a `color-contrast()` function to automatically select accessible text colors. You can replicate this in custom Sass:

```scss
// Bootstrap's approach to automatic contrast
@function color-contrast($background, $color-contrast-dark: $color-contrast-dark,
  $color-contrast-light: $color-contrast-light) {
  $luminance: luminance($background);
  @if ($luminance > 0.45) {
    @return $color-contrast-dark;
  } @else {
    @return $color-contrast-light;
  }
}

// Usage in custom component
.custom-card-header {
  background-color: $primary;
  color: color-contrast($primary);  // Auto-selects white or black
  padding: 1rem;
}

.custom-card-accent {
  background-color: $success;
  color: color-contrast($success);
  padding: 0.5rem 1rem;
}
```

### Custom Theme with Verified Contrast

```scss
// Define custom theme colors
$brand-navy: #1a2744;
$brand-gold: #d4a843;
$brand-light: #f5f0e8;

// Verify contrast ratios before use
// Navy on light: 12.5:1 (AAA pass)
// Gold on navy: 5.1:1 (AA pass for large text)
// Gold on light: 2.1:1 (FAIL - do not use for body text)

.theme-primary-bg {
  background-color: $brand-navy;
  color: color-contrast($brand-navy); // #fff
}

.theme-accent-text {
  color: $brand-gold; // Use only for large text on light backgrounds
  font-size: 1.25rem; // Ensure it counts as "large text" (18px+)
}

.theme-accent-button {
  background-color: $brand-gold;
  color: $brand-navy; // 5.1:1 - AA pass
  font-weight: 600;
}
```

### Contrast for Focus Indicators

```css
/* Focus indicators must meet 3:1 contrast against adjacent colors */
.btn-primary:focus-visible {
  outline: 3px solid #0a58ca; /* Darker shade of primary */
  outline-offset: 2px;
}

.btn-light:focus-visible {
  outline: 3px solid #198754; /* Green outline on light button - 3.5:1 */
  outline-offset: 2px;
}

/* High contrast mode support */
@media (forced-colors: active) {
  :focus-visible {
    outline: 2px solid Highlight;
  }
}
```

## Best Practices

1. **Test all custom color pairs for AA compliance** - Use a contrast checker tool or browser devtools to verify every combination of text and background color meets at least 4.5:1 for normal text.
2. **Use Bootstrap's default color utilities when possible** - Pre-validated combinations like `text-white bg-primary` are guaranteed to meet AA contrast requirements.
3. **Never rely on color alone to convey information** - Always supplement color with text, icons, or patterns. Example: error states should include an icon and message, not just red text.
4. **Verify contrast at every text size** - Large text (18px regular or 14px bold) requires only 3:1, while body text requires 4.5:1. Design systems often pass for headers but fail for body text.
5. **Test with color blindness simulators** - Tools like Chrome DevTools' rendering emulation or browser extensions simulate protanopia, deuteranopia, and tritanopia to reveal problematic color combinations.
6. **Account for background images** - Text over images may have varying contrast depending on the image content. Use a semi-transparent overlay to ensure consistent contrast.
7. **Check disabled states** - Disabled buttons and inputs often use low-contrast colors. While WCAG exempts disabled controls from contrast requirements, ensure they are still distinguishable.
8. **Use sufficient contrast for borders and icons** - Input borders, checkbox outlines, and icon colors must meet 3:1 contrast against their background.
9. **Document approved color combinations** - Maintain a reference table of validated color pairs in your design system to prevent accidental violations.
10. **Automate contrast testing in CI/CD** - Use tools like axe-core or Lighthouse to catch contrast violations before deployment.
11. **Test in high contrast mode** - Windows High Contrast Mode overrides page colors. Ensure your UI remains functional with `forced-colors: active` media query.
12. **Avoid thin fonts at small sizes** - Light font weights reduce perceived contrast. Use at least `font-weight: 400` for body text below 16px.

## Common Pitfalls

1. **Using placeholder text as labels** - Placeholder text typically uses low contrast gray. When it serves as the only label, users cannot read it, violating contrast requirements.
2. **Text over images without overlays** - White text over a busy photograph may pass in one area and fail in another. Always add a solid or semi-transparent overlay behind text.
3. **Custom brand colors not checked for contrast** - Brand purple or corporate teal may look great in the design comp but fail contrast requirements at body text size.
4. **Light gray text on white backgrounds** - Colors like `#999` on `#fff` yield only 2.8:1 contrast, which fails both AA and AAA for any text size.
5. **Focus indicators invisible on matching backgrounds** - A blue outline on a blue button is invisible. Use contrasting outline colors based on the button's background.
6. **Decorative borders and UI elements with insufficient 3:1 contrast** - Form input borders, dividers, and checkbox outlines must meet 3:1 against adjacent colors.
7. **Ignoring contrast in dark mode** - Inverting colors without rechecking contrast can create combinations like light gray on white that fail requirements.
8. **Relying on WCAG exceptions incorrectly** - WCAG exempts logos and decorative text from contrast rules, but all functional text, including captions and disabled states with informational value, must comply.

## Accessibility Considerations

### WCAG Contrast Quick Reference

Calculate contrast ratio using the relative luminance formula:

```
Contrast Ratio = (L1 + 0.05) / (L2 + 0.05)
```

Where L1 is the lighter color's luminance and L2 is the darker color's luminance.

### Testing Tools

| Tool | Type | Best For |
|------|------|----------|
| WebAIM Contrast Checker | Web | Quick manual checks |
| Chrome DevTools | Browser | Inspecting live elements |
| axe DevTools | Extension | Full page audits |
| Colour Contrast Analyser | Desktop app | Precise measurements |
| Lighthouse | CI/CD | Automated testing |

### High Contrast Mode

```css
/* Ensure UI is functional in Windows High Contrast Mode */
.custom-checkbox {
  border: 2px solid #333;
  forced-color-adjust: none; /* Opt out of automatic adjustment */
}

/* Better approach: respect system colors */
.custom-card {
  border: 1px solid ButtonText;
  background: Canvas;
  color: CanvasText;
}

@media (forced-colors: active) {
  .custom-focus-ring {
    outline: 2px solid Highlight;
    outline-offset: 2px;
  }

  .custom-separator {
    border-color: ButtonText;
  }
}
```

## Responsive Behavior

Color contrast requirements do not change across viewport sizes, but responsive design patterns affect how contrast is experienced:

- **Text resizing** - When users increase text size via browser zoom or OS settings, ensure contrast remains sufficient. Large text has a lower threshold (3:1), but custom line-height and spacing may cause overlap with colored backgrounds.
- **Mobile brightness** - Mobile devices are used in bright sunlight where contrast is harder to perceive. Design for higher contrast than the minimum to ensure readability outdoors.
- **Dark mode** - If your app supports dark mode via `prefers-color-scheme`, verify all color combinations meet contrast requirements in both modes. Bootstrap 5.3+ includes built-in dark mode support.
- **Responsive images as backgrounds** - On small screens, a background image may crop differently, changing which parts of the image are visible behind text. Test overlay contrast at all breakpoints.

```html
<!-- Responsive contrast-safe overlay on hero image -->
<div class="position-relative">
  <img src="hero.jpg" class="img-fluid w-100" alt="" aria-hidden="true">
  <div class="position-absolute top-50 start-0 translate-middle-y p-4
              bg-dark bg-opacity-75 text-white w-50">
    <h1 class="display-5">Welcome</h1>
    <p class="lead">Accessible text with guaranteed contrast.</p>
  </div>
</div>
```
