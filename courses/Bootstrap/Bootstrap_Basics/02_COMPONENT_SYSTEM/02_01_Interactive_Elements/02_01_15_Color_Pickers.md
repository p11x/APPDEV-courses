---
title: Color Pickers
category: Component System
difficulty: 2
time: 25 min
tags: bootstrap5, color-picker, forms, input, theme, swatches
---

## Overview

Bootstrap does not include a dedicated color picker component, but you can style native HTML color inputs and build custom color swatch patterns using Bootstrap utilities. The native `<input type="color">` provides browser-native color selection, while custom swatch patterns offer curated color palettes for theme selection, branding, or design tools.

## Basic Implementation

The simplest approach uses a native color input styled with Bootstrap form classes.

```html
<!-- Native color input with Bootstrap styling -->
<div class="mb-3">
  <label for="colorPicker" class="form-label">Choose a color</label>
  <input type="color" class="form-control form-control-color" id="colorPicker" value="#563d7c" title="Choose a color">
</div>

<!-- Color input in an input group -->
<div class="input-group">
  <input type="color" class="form-control form-control-color" id="inlineColor" value="#0d6efd" title="Choose a color">
  <span class="input-group-text">#0d6efd</span>
</div>
```

## Advanced Variations

```html
<!-- Color swatch grid pattern -->
<style>
  .color-swatch {
    width: 2rem;
    height: 2rem;
    border-radius: 0.25rem;
    border: 2px solid transparent;
    cursor: pointer;
    transition: border-color 0.15s ease;
  }
  .color-swatch:hover,
  .color-swatch.active {
    border-color: #212529;
  }
  .color-swatch.active {
    box-shadow: 0 0 0 2px #fff, 0 0 0 4px #0d6efd;
  }
</style>

<div class="mb-3">
  <label class="form-label">Theme Color</label>
  <div class="d-flex gap-2 flex-wrap">
    <button type="button" class="color-swatch p-0" style="background-color: #0d6efd;" title="Primary" data-color="#0d6efd"></button>
    <button type="button" class="color-swatch p-0" style="background-color: #198754;" title="Success" data-color="#198754"></button>
    <button type="button" class="color-swatch p-0" style="background-color: #dc3545;" title="Danger" data-color="#dc3545"></button>
    <button type="button" class="color-swatch p-0" style="background-color: #ffc107;" title="Warning" data-color="#ffc107"></button>
    <button type="button" class="color-swatch p-0" style="background-color: #0dcaf0;" title="Info" data-color="#0dcaf0"></button>
    <button type="button" class="color-swatch p-0" style="background-color: #212529;" title="Dark" data-color="#212529"></button>
    <button type="button" class="color-swatch p-0" style="background-color: #6c757d;" title="Secondary" data-color="#6c757d"></button>
  </div>
  <input type="hidden" name="selectedThemeColor" id="selectedThemeColor">
</div>
```

```html
<!-- Theme color picker with preview -->
<div class="card p-3">
  <h6>Brand Settings</h6>
  <div class="row g-3">
    <div class="col-md-6">
      <label for="primaryColor" class="form-label">Primary Color</label>
      <div class="input-group">
        <input type="color" class="form-control form-control-color" id="primaryColor" value="#0d6efd">
        <input type="text" class="form-control" value="#0d6efd" id="primaryHex" maxlength="7">
      </div>
    </div>
    <div class="col-md-6">
      <label for="accentColor" class="form-label">Accent Color</label>
      <div class="input-group">
        <input type="color" class="form-control form-control-color" id="accentColor" value="#6f42c1">
        <input type="text" class="form-control" value="#6f42c1" id="accentHex" maxlength="7">
      </div>
    </div>
  </div>
  <div class="mt-3">
    <button class="btn btn-primary" id="primaryBtn">Primary Button</button>
    <button class="btn" id="accentBtn" style="background-color: #6f42c1; color: #fff;">Accent Button</button>
  </div>
</div>
```

## Best Practices

1. Always include a visible `<label>` associated with the color input.
2. Use `form-control-color` class for consistent sizing with other form controls.
3. Provide the `title` attribute on color inputs for tooltip accessibility.
4. Pair the color input with a hex text input for precise value entry.
5. Use custom swatches for curated color palettes rather than exposing full color pickers.
6. Display the selected color value as text alongside the input.
7. Use `data-*` attributes on swatch buttons to store hex values for JavaScript access.
8. Validate hex input format when accepting manual color entry.
9. Provide visual feedback (active class) for selected swatches.
10. Consider color contrast when suggesting default colors.

## Common Pitfalls

1. **Missing label or title.** Color inputs without labels are inaccessible to screen readers.
2. **No hex value display.** Users cannot verify or communicate the exact selected color.
3. **Swatches without keyboard support.** Button-based swatches must be focusable and activatable with Enter/Space.
4. **Ignoring dark mode.** Fixed border colors on swatches may be invisible in dark themes.
5. **No validation on hex input.** Free-text hex fields accept invalid values without feedback.
6. **Using color inputs for non-color purposes.** Confuses users and assistive technology.

## Accessibility Considerations

Color inputs need associated `<label>` elements and `title` attributes. Custom swatch grids should use `<button>` elements with descriptive `title` attributes (e.g., "Blue (#0d6efd)"). Ensure keyboard users can navigate swatches with Tab and select with Enter. Display selected color as text for users who cannot perceive color differences. Use `aria-label` when the label is not visible. Provide `aria-live` regions that announce the selected color value.

## Responsive Behavior

Color inputs scale with their container using Bootstrap's grid. On mobile, use `col-12` for full-width color pickers. Swatch grids should wrap naturally with `flex-wrap`. Input groups combining color and text inputs stack vertically on small screens with `col-12 col-md-6`. Ensure touch targets for swatches are at least 44x44px on mobile devices.
