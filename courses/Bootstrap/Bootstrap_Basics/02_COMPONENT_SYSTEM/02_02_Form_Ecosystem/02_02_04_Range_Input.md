---
title: Range Input
category: Form Ecosystem
difficulty: 1
time: 25 minutes
tags:
  - bootstrap
  - forms
  - range
  - input
  - slider
---

## Overview

The Bootstrap range input provides a styled slider control built on the native HTML `<input type="range">` element. It allows users to select a value from a continuous or discrete range by dragging a thumb along a track. Range inputs are ideal for settings where an approximate value is acceptable, such as volume controls, brightness adjustments, zoom levels, or any numeric parameter where the user needs visual feedback on their selection.

Bootstrap wraps the native range element with the `.form-range` class, which normalizes the appearance across browsers while maintaining full functionality. The component supports standard HTML attributes like `min`, `max`, and `step` to define the range boundaries and increments. It also integrates with Bootstrap's disabled state system and can be customized further with CSS custom properties introduced in Bootstrap 5.3+.

Range inputs are inherently difficult to style consistently across browsers because each browser renders the track and thumb differently. Bootstrap solves this by applying custom styles using pseudo-elements (`::-webkit-slider-thumb`, `::-moz-range-thumb`, etc.) that produce a uniform look on Chrome, Firefox, Safari, and Edge. The result is a clean, modern slider that aligns with Bootstrap's design language out of the box.

## Basic Implementation

The simplest range input requires only the `.form-range` class on an `<input type="range">` element. Bootstrap handles the visual styling automatically.

```html
<div class="mb-3">
  <label for="basicRange" class="form-label">Basic Range</label>
  <input type="range" class="form-range" id="basicRange">
</div>
```

To define custom bounds, use the `min` and `max` attributes. The browser will distribute the track evenly between these two values.

```html
<div class="mb-3">
  <label for="customRange" class="form-label">Temperature (10°C - 40°C)</label>
  <input type="range" class="form-range" id="customRange" min="10" max="40">
</div>
```

The `step` attribute controls the granularity of the slider. By default, most browsers use a step of 1 for integer ranges. Setting `step="5"` snaps the thumb to multiples of 5.

```html
<div class="mb-3">
  <label for="steppedRange" class="form-label">Volume (0-100, step 5)</label>
  <input type="range" class="form-range" id="steppedRange" min="0" max="100" step="5">
</div>
```

To set a default starting value, add the `value` attribute to the input.

```html
<div class="mb-3">
  <label for="defaultRange" class="form-label">Brightness</label>
  <input type="range" class="form-range" id="defaultRange" min="0" max="100" value="75">
</div>
```

A disabled range input prevents user interaction and renders with reduced opacity.

```html
<div class="mb-3">
  <label for="disabledRange" class="form-label text-muted">Disabled Range</label>
  <input type="range" class="form-range" id="disabledRange" disabled>
</div>
```

## Advanced Variations

Bootstrap 5.3 introduced CSS custom properties that allow you to override range input colors without writing complex CSS. The `--bs-form-range-thumb-bg` and `--bs-form-range-track-bg` variables control the thumb and track colors respectively.

```html
<div class="mb-3">
  <label for="coloredRange" class="form-label">Custom Color Range</label>
  <input
    type="range"
    class="form-range"
    id="coloredRange"
    style="--bs-form-range-thumb-bg: #dc3545; --bs-form-range-track-bg: #f8d7da;"
    min="0"
    max="100"
  >
</div>
```

Displaying the current value alongside a range input improves usability significantly. Use JavaScript to update an output element in real time as the user drags the slider.

```html
<div class="mb-3">
  <label for="outputRange" class="form-label">Zoom Level: <span id="zoomValue">50</span>%</label>
  <input
    type="range"
    class="form-range"
    id="outputRange"
    min="25"
    max="200"
    value="50"
    oninput="document.getElementById('zoomValue').textContent = this.value"
  >
</div>
```

For scenarios where you need multiple related range inputs, such as a filter panel or a settings page, group them inside a card or fieldset for visual cohesion.

```html
<div class="card p-3">
  <h5 class="card-title">Image Adjustments</h5>

  <div class="mb-3">
    <label for="brightness" class="form-label">Brightness</label>
    <input type="range" class="form-range" id="brightness" min="0" max="200" value="100">
  </div>

  <div class="mb-3">
    <label for="contrast" class="form-label">Contrast</label>
    <input type="range" class="form-range" id="contrast" min="0" max="200" value="100">
  </div>

  <div class="mb-0">
    <label for="saturation" class="form-label">Saturation</label>
    <input type="range" class="form-range" id="saturation" min="0" max="200" value="100">
  </div>
</div>
```

Using a datalist element with a range input provides tick marks at specified intervals, giving users visual reference points.

```html
<div class="mb-3">
  <label for="datalistRange" class="form-label">Rating with tick marks</label>
  <input type="range" class="form-range" id="datalistRange" list="ratingList" min="1" max="10">
  <datalist id="ratingList">
    <option value="1" label="1">
    <option value="2">
    <option value="3">
    <option value="4">
    <option value="5" label="5">
    <option value="6">
    <option value="7">
    <option value="8">
    <option value="9">
    <option value="10" label="10">
  </datalist>
</div>
```

## Best Practices

- **Always include a `<label>` element** associated with the range input via the `for` attribute. Screen readers rely on this association to announce the purpose of the control.
- **Set explicit `min` and `max` values** rather than relying on browser defaults (typically 0-100). Explicit bounds make the expected value range clear to both users and assistive technologies.
- **Use the `step` attribute** to constrain values to meaningful increments. Without it, the browser may allow decimal values that your application cannot handle.
- **Display the current value** next to the range input. Users cannot determine the exact numeric value from the slider position alone, especially on small screens.
- **Provide a text input fallback** for users who need to enter precise values. Range inputs are imprecise by nature and should not be the sole input method for critical numeric data.
- **Group related range inputs** with a `<fieldset>` and `<legend>` for semantic structure and screen reader navigation.
- **Use `value` to set sensible defaults.** A slider starting at zero when the expected midpoint is 50 forces unnecessary interaction.
- **Keep labels concise.** Range input labels should describe what the slider controls in a few words, with the numeric value shown separately.
- **Test across browsers.** While Bootstrap normalizes appearance, the behavior of `step` and `value` can vary slightly between browsers. Verify on Chrome, Firefox, Safari, and Edge.
- **Consider touch targets on mobile.** The default Bootstrap range thumb is approximately 16px wide. On touch devices, this can be difficult to grab. Increase the thumb size with CSS if your audience is primarily mobile.
- **Avoid using range inputs for binary or discrete selections.** If the user can only choose between two or three specific values, radio buttons or a select element are more appropriate.
- **Use `aria-valuenow`, `aria-valuemin`, and `aria-valuemax`** only if you are building a custom slider. Native range inputs expose these values to accessibility APIs automatically.

## Common Pitfalls

- **Omitting the label.** A range input without a visible or accessible label is completely opaque to users. The browser shows no indication of what the slider controls. Always pair the input with a `<label>` element.
- **Relying on the default range of 0-100** when your domain uses different bounds. If your application expects values between 10 and 50, set `min="10" max="50"` explicitly. Otherwise, users will interact with values outside your intended range.
- **Not displaying the current value.** Users drag the slider but have no feedback about the exact number they have selected. This leads to confusion and repeated adjustments. Always show the value in real time.
- **Using `step` values that are too small or too large.** A step of 0.001 on a range from 0 to 1000 creates an unusable slider with imperceptible changes between positions. Conversely, a step of 100 on a 0-500 range gives only six positions, which would be better served by radio buttons.
- **Forgetting that `disabled` removes keyboard accessibility.** Disabled range inputs cannot receive focus via Tab navigation. If you want a read-only state, consider using `pointer-events: none` and `aria-readonly="true"` instead of the `disabled` attribute.
- **Placing the output element inside the `<label>`.** While it works visually, some screen readers may announce the label text every time the value changes, creating repetitive audio output. Place the output next to the label as a sibling `<span>` rather than inside the label text.
- **Ignoring the value attribute on form submission.** If the user never moves the slider, the `value` attribute determines what gets submitted. Make sure this default value is meaningful for your application logic.

## Accessibility Considerations

Range inputs are natively accessible when implemented with proper HTML semantics. The `<input type="range">` element is recognized by all major screen readers and exposes its current value, minimum, maximum, and step increment to assistive technologies.

The most critical accessibility requirement is labeling. The `<label>` element must be programmatically associated with the input using the `for` attribute matching the input's `id`. Without this association, screen readers announce the control as "slider" with no context about its purpose.

For range inputs that represent a value with units (such as temperature in degrees or a percentage), include the unit in the label text or as a visible suffix so users understand what they are adjusting. Avoid relying solely on visual proximity to convey this information.

```html
<div class="mb-3">
  <label for="tempRange" class="form-label">
    Room Temperature: <span id="tempDisplay">22</span>°C
  </label>
  <input
    type="range"
    class="form-range"
    id="tempRange"
    min="16"
    max="30"
    value="22"
    oninput="document.getElementById('tempDisplay').textContent = this.value"
  >
</div>
```

When grouping multiple range inputs, use a `<fieldset>` with a `<legend>` to provide a group-level description. This allows screen reader users to understand the context of multiple related sliders without navigating through each one individually.

```html
<fieldset>
  <legend>Audio Mix Controls</legend>
  <div class="mb-3">
    <label for="musicVol" class="form-label">Music</label>
    <input type="range" class="form-range" id="musicVol" min="0" max="100" value="80">
  </div>
  <div class="mb-3">
    <label for="voiceVol" class="form-label">Voice</label>
    <input type="range" class="form-range" id="voiceVol" min="0" max="100" value="100">
  </div>
</fieldset>
```

Keyboard users can adjust range inputs using the arrow keys. Each press of an arrow key moves the value by one step increment. Page Up and Page Down typically move the value by a larger increment (usually 10% of the range). Ensure that your `step` attribute is set so that keyboard-based adjustments produce meaningful changes.

## Responsive Behavior

Bootstrap's range input is inherently responsive. It stretches to fill the full width of its container at all breakpoints by default. No additional responsive classes are required for basic functionality.

On narrow screens (mobile devices), the full-width slider provides a good touch target for horizontal dragging. However, the thumb itself may be small relative to a finger. If your application is primarily mobile, consider increasing the thumb size with custom CSS.

```html
<style>
  .form-range::-webkit-slider-thumb {
    width: 24px;
    height: 24px;
  }
  .form-range::-moz-range-thumb {
    width: 24px;
    height: 24px;
  }
</style>

<div class="mb-3">
  <label for="mobileRange" class="form-label">Mobile-Friendly Range</label>
  <input type="range" class="form-range" id="mobileRange">
</div>
```

When placing range inputs inside Bootstrap's grid system, they automatically adapt to the column width. A range input inside a `col-md-6` column will be half-width on medium screens and full-width on small screens.

```html
<div class="row">
  <div class="col-md-6 mb-3">
    <label for="rangeCol1" class="form-label">Left Slider</label>
    <input type="range" class="form-range" id="rangeCol1">
  </div>
  <div class="col-md-6 mb-3">
    <label for="rangeCol2" class="form-label">Right Slider</label>
    <input type="range" class="form-range" id="rangeCol2">
  </div>
</div>
```

For range inputs displayed inside input groups or horizontal forms, ensure that the label and input stack vertically on small screens using Bootstrap's grid classes. The `col-form-label` class aligns the label text with the input height in horizontal layouts, and `col-12` forces full-width stacking on extra-small viewports.
