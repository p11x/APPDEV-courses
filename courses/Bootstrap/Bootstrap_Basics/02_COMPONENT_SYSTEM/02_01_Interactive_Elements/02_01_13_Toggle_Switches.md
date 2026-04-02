---
title: Toggle Switches
category: Component System
difficulty: 1
time: 15 min
tags: bootstrap5, form-switch, toggle, switches, form-controls
---

## Overview

Bootstrap toggle switches provide a binary on/off control built on top of checkbox inputs. Using the `form-switch` class, switches replace the default checkbox appearance with a sliding toggle. They support sizing, disabled states, custom labels, and integrate seamlessly with Bootstrap's form validation and layout systems.

## Basic Implementation

Wrap a checkbox input and label inside a `div` with the `form-switch` class.

```html
<!-- Basic toggle switch -->
<div class="form-check form-switch">
  <input class="form-check-input" type="checkbox" role="switch" id="switchDefault">
  <label class="form-check-label" for="switchDefault">Default switch</label>
</div>

<!-- Checked toggle switch -->
<div class="form-check form-switch">
  <input class="form-check-input" type="checkbox" role="switch" id="switchChecked" checked>
  <label class="form-check-label" for="switchChecked">Enabled feature</label>
</div>
```

## Advanced Variations

```html
<!-- Sizing: large and small switches -->
<div class="form-check form-switch">
  <input class="form-check-input" type="checkbox" role="switch" id="switchDefaultSize">
  <label class="form-check-label" for="switchDefaultSize">Default size</label>
</div>
<div class="form-check form-switch">
  <input class="form-check-input form-check-input-lg" type="checkbox" role="switch" id="switchLarge">
  <label class="form-check-label" for="switchLarge">Large switch</label>
</div>
<div class="form-check form-switch">
  <input class="form-check-input form-check-input-sm" type="checkbox" role="switch" id="switchSmall">
  <label class="form-check-label" for="switchSmall">Small switch</label>
</div>
```

```html
<!-- Disabled switch -->
<div class="form-check form-switch">
  <input class="form-check-input" type="checkbox" role="switch" id="switchDisabled" disabled>
  <label class="form-check-label text-muted" for="switchDisabled">Disabled switch</label>
</div>

<!-- Disabled but checked -->
<div class="form-check form-switch">
  <input class="form-check-input" type="checkbox" role="switch" id="switchDisabledChecked" checked disabled>
  <label class="form-check-label text-muted" for="switchDisabledChecked">Always on (locked)</label>
</div>
```

```html
<!-- Switch with inline layout and description -->
<div class="form-check form-switch mb-3">
  <input class="form-check-input" type="checkbox" role="switch" id="switchDarkMode">
  <label class="form-check-label" for="switchDarkMode">Dark Mode</label>
  <div class="form-text">Toggle between light and dark themes.</div>
</div>

<!-- Switch in a list group item -->
<div class="list-group">
  <div class="list-group-item d-flex justify-content-between align-items-center">
    <div>
      <strong>Email Notifications</strong>
      <p class="mb-0 text-muted small">Receive email updates</p>
    </div>
    <div class="form-check form-switch mb-0">
      <input class="form-check-input" type="checkbox" role="switch" id="switchEmail" checked>
    </div>
  </div>
  <div class="list-group-item d-flex justify-content-between align-items-center">
    <div>
      <strong>SMS Alerts</strong>
      <p class="mb-0 text-muted small">Get text message alerts</p>
    </div>
    <div class="form-check form-switch mb-0">
      <input class="form-check-input" type="checkbox" role="switch" id="switchSMS">
    </div>
  </div>
</div>
```

## Best Practices

1. Always include `role="switch"` on the checkbox input for accessibility.
2. Associate a `<label>` with the input using matching `for` and `id` attributes.
3. Use `form-switch` alongside `form-check` for proper spacing and alignment.
4. Apply `disabled` attribute and muted label text for unavailable options.
5. Use switches for binary on/off settings only; use radio buttons for multiple choices.
6. Provide contextual help text with `form-text` below the switch when clarification is needed.
7. Group related switches in a `list-group` or card for settings panels.
8. Keep switch labels concise and descriptive.
9. Use `checked` attribute to reflect the current state, not the desired state.
10. Maintain consistent switch placement in your UI (e.g., always right-aligned in settings).

## Common Pitfalls

1. **Missing `role="switch"` attribute.** Screen readers treat the input as a checkbox, not a toggle.
2. **No associated label.** Unlabeled switches are inaccessible to screen readers and hard to click.
3. **Using switches for non-binary choices.** Switches imply on/off; use radios or selects for multiple options.
4. **Inconsistent state representation.** If checked means "enabled," maintain this convention across the app.
5. **Forgetting to style disabled state.** Disabled switches without visual distinction confuse users.
6. **Placing switches inside labels without proper structure.** This creates nested interactive elements that are problematic for accessibility.

## Accessibility Considerations

Toggle switches must include `role="switch"` so assistive technology announces them as switches rather than checkboxes. Each switch needs a visible `<label>` with a matching `for` attribute. Disabled switches should also have muted or dimmed labels to communicate the unavailable state visually. Keyboard users can toggle switches with Space. Provide `aria-describedby` linking to help text when additional context is needed.

## Responsive Behavior

Toggle switches scale naturally with their parent container. In horizontal layouts, use `d-flex align-items-center` to align switches with accompanying text. On mobile, ensure touch targets are at least 44x44px by using larger switch sizes or adding padding. In settings lists, switches should remain right-aligned while text content fills available space using flexbox.
