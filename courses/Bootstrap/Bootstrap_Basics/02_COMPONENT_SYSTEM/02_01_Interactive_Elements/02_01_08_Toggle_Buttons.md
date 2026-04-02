---
tags: [bootstrap, toggle-buttons, buttons, state-management]
category: Interactive Elements
difficulty: 2
time: 25 minutes
---

# Toggle Buttons

## Overview

Toggle buttons are interactive controls that switch between active and inactive states when clicked. Bootstrap 5 supports two primary toggle patterns: programmatic toggling via the `data-bs-toggle="button"` attribute on any button element, and semantic form-based toggling using checkbox and radio inputs with visually styled button labels. Both patterns maintain visual state through the `.active` class and communicate state to assistive technologies through `aria-pressed` (for button toggles) or native checked semantics (for input-based toggles).

The button-based toggle pattern is suitable for simple on/off switches like favorite, mute, or enable/disable controls. The input-based patterns leverage native HTML checkbox and radio semantics, providing built-in keyboard navigation, form submission, and state management without custom JavaScript. Radio toggle groups enforce mutual exclusivity, making them ideal for toolbar selections, filter options, and segmented controls.

Understanding the distinction between these patterns is critical. Button toggles manage state through Bootstrap's JavaScript plugin and require explicit event handling for integration with application logic. Input toggles manage state through native form semantics, making them automatically compatible with form serialization and submission. Choosing the correct pattern depends on whether the toggle participates in a form and whether mutual exclusivity is required.

## Basic Implementation

The simplest toggle button uses `data-bs-toggle="button"` on a `<button>` element. Clicking toggles the `.active` class and updates `aria-pressed`:

```html
<button type="button" class="btn btn-primary" data-bs-toggle="button">Toggle button</button>
```

Pre-toggled buttons include the `.active` class and `aria-pressed="true"` from the start:

```html
<button type="button" class="btn btn-primary active" data-bs-toggle="button" aria-pressed="true">
  Pre-toggled button
</button>
```

Outline variants work seamlessly with toggle behavior:

```html
<button type="button" class="btn btn-outline-primary" data-bs-toggle="button">Toggle outline</button>
<button type="button" class="btn btn-outline-success active" data-bs-toggle="button" aria-pressed="true">
  Active outline
</button>
```

Checkbox toggle buttons use the `.btn-check` class on a visually hidden `<input type="checkbox">` paired with a `<label>` styled as a button:

```html
<input type="checkbox" class="btn-check" id="btn-check" autocomplete="off">
<label class="btn btn-primary" for="btn-check">Single toggle</label>
```

Radio toggle buttons share the same `name` attribute to enforce mutual exclusivity:

```html
<input type="radio" class="btn-check" name="options" id="option1" autocomplete="off" checked>
<label class="btn btn-outline-primary" for="option1">Radio 1</label>

<input type="radio" class="btn-check" name="options" id="option2" autocomplete="off">
<label class="btn btn-outline-primary" for="option2">Radio 2</label>

<input type="radio" class="btn-check" name="options" id="option3" autocomplete="off">
<label class="btn btn-outline-primary" for="option3">Radio 3</label>
```

## Advanced Variations

Checkbox toggle buttons in button groups create multi-select toolbars:

```html
<div class="btn-group" role="group" aria-label="Basic checkbox toggle button group">
  <input type="checkbox" class="btn-check" id="btncheck1" autocomplete="off">
  <label class="btn btn-outline-primary" for="btncheck1">Checkbox 1</label>

  <input type="checkbox" class="btn-check" id="btncheck2" autocomplete="off">
  <label class="btn btn-outline-primary" for="btncheck2">Checkbox 2</label>

  <input type="checkbox" class="btn-check" id="btncheck3" autocomplete="off">
  <label class="btn btn-outline-primary" for="btncheck3">Checkbox 3</label>
</div>
```

Radio toggle buttons in button groups create single-select segmented controls:

```html
<div class="btn-group" role="group" aria-label="Basic radio toggle button group">
  <input type="radio" class="btn-check" name="btnradio" id="btnradio1" autocomplete="off" checked>
  <label class="btn btn-outline-primary" for="btnradio1">Radio 1</label>

  <input type="radio" class="btn-check" name="btnradio" id="btnradio2" autocomplete="off">
  <label class="btn btn-outline-primary" for="btnradio2">Radio 2</label>

  <input type="radio" class="btn-check" name="btnradio" id="btnradio3" autocomplete="off">
  <label class="btn btn-outline-primary" for="btnradio3">Radio 3</label>
</div>
```

Listening to toggle state changes requires event listeners on the input elements:

```html
<div class="btn-group" role="group" aria-label="State change example">
  <input type="checkbox" class="btn-check" id="stateCheck1" autocomplete="off">
  <label class="btn btn-outline-success" for="stateCheck1">Enable Notifications</label>
</div>

<p id="stateOutput">Notifications: Off</p>

<script>
document.getElementById('stateCheck1').addEventListener('change', function() {
  const output = document.getElementById('stateOutput');
  output.textContent = `Notifications: ${this.checked ? 'On' : 'Off'}`;
});
</script>
```

For button-based toggles, listen to Bootstrap events:

```html
<button type="button" class="btn btn-outline-primary" id="eventToggle" data-bs-toggle="button">
  Listen to Toggle
</button>

<p id="toggleState">State: Inactive</p>

<script>
const toggleBtn = document.getElementById('eventToggle');
toggleBtn.addEventListener('click', function() {
  const isActive = this.classList.contains('active');
  document.getElementById('toggleState').textContent = `State: ${isActive ? 'Active' : 'Inactive'}`;
});
</script>
```

Reading toggle state from checkbox inputs is straightforward with the `checked` property:

```html
<input type="checkbox" class="btn-check" id="readState" autocomplete="off">
<label class="btn btn-outline-info" for="readState">Read My State</label>

<button type="button" class="btn btn-secondary" id="readBtn">Check State</button>

<script>
document.getElementById('readBtn').addEventListener('click', function() {
  const isChecked = document.getElementById('readState').checked;
  alert(`Checkbox is ${isChecked ? 'checked' : 'unchecked'}`);
});
</script>
```

## Best Practices

1. **Use checkbox toggles for multi-select and radio toggles for single-select.** This leverages native HTML form semantics and ensures correct behavior without custom JavaScript state management.

2. **Always pair `.btn-check` inputs with `<label>` elements.** The label provides the visible button appearance. Without the label, the input is visually hidden with no clickable target.

3. **Use `autocomplete="off"` on toggle inputs.** This prevents browsers from persisting checked states across page reloads, which can conflict with server-rendered or JavaScript-initialized state.

4. **Group related toggles inside `btn-group` with `role="group"`.** The group role communicates the relationship between toggle buttons to assistive technologies, and the visual connection helps users understand the controls are related.

5. **Provide `aria-label` on toggle button groups.** Describing the group's purpose (e.g., "Text formatting options") gives screen reader users context for the toggle controls.

6. **Use outline variants for toggle groups.** Outline buttons provide clear visual distinction between active (filled) and inactive (outline-only) states, making the toggle state immediately apparent.

7. **Set `checked` on the default radio selection.** Radio groups should have one option selected by default. Without a checked radio, the group starts in an indeterminate state, which can cause validation issues.

8. **Use `change` events on checkbox/radio inputs for state handling.** The `change` event fires only when the checked state actually changes, unlike `click`, which fires even if the input is already checked.

9. **Do not combine `data-bs-toggle="button"` with `.btn-check` inputs.** These patterns manage state independently. Using both creates conflicting state management where the visual class and the input `checked` property may disagree.

10. **Test toggle state persistence in forms.** Verify that checkbox toggles serialize correctly with `FormData` and that radio toggles submit only the selected value.

## Common Pitfalls

1. **Using button-based toggles inside forms without hidden inputs.** Button toggles do not participate in form submission. If the toggle state needs to be submitted, use checkbox/radio inputs instead, or synchronize the state to a hidden input field.

2. **Forgetting `name` attribute on radio inputs.** Radio inputs without the same `name` do not enforce mutual exclusivity. All radios can be selected simultaneously, which defeats the purpose of a radio group.

3. **Missing `for` attribute on labels.** The `for` attribute must match the `id` of the input. Without it, clicking the label does not toggle the input, and the visual button becomes non-interactive.

4. **Not including `autocomplete="off"`.** Browsers may restore checked states from previous sessions, causing toggles to start in unexpected states that conflict with application logic.

5. **Using `data-bs-toggle="button"` on disabled buttons.** Disabled buttons do not fire click events. The toggle behavior is inactive on disabled elements. This is correct behavior but can confuse developers expecting toggling on disabled controls.

6. **Not handling state changes in application logic.** Bootstrap manages visual state only. If the toggle controls an application feature (e.g., dark mode, notifications), JavaScript must listen for changes and update the application state accordingly.

7. **Using button toggles for options that should be form-submittable.** Button toggles use `aria-pressed` for accessibility but do not serialize for form submission. Use input-based toggles when the state is part of form data.

8. **Mixing toggle patterns inconsistently.** Using `data-bs-toggle="button"` in one place and `.btn-check` inputs in another for the same type of control creates inconsistency. Choose one pattern per use case.

## Accessibility Considerations

Button-based toggles must have `aria-pressed` set to `"true"` or `"false"` to communicate the toggle state to screen readers. Bootstrap manages this automatically for elements with `data-bs-toggle="button"` when toggled. Pre-toggled buttons must include `aria-pressed="true"` manually.

Checkbox and radio toggle inputs use native checked semantics, which are fully accessible by default. Screen readers announce checkbox toggles as "checkbox, checked" or "checkbox, not checked" and radio toggles as "radio button, selected" or "radio button, not selected."

Toggle button groups require `role="group"` and an accessible name via `aria-label` or `aria-labelledby`. Without these attributes, screen readers announce the toggles as isolated controls with no group context.

For keyboard navigation, checkbox toggles are individually focusable with Tab. Radio toggles within a group use a single Tab stop: the selected radio is in the tab order, and arrow keys move selection between radios within the group.

Visual state must not rely solely on color. Active toggle buttons typically change background color. Pair this with a visual indicator like a border change, icon, or text change to ensure the state is perceivable by users with color vision deficiencies.

## Responsive Behavior

Toggle buttons follow the same responsive behavior as standard buttons. They do not change size at breakpoints by default. Use `btn-lg` or `btn-sm` classes for fixed size adjustments.

For responsive toggle button groups, combine `.btn-group` with Bootstrap's flex utilities:

```html
<div class="btn-group flex-wrap" role="group" aria-label="Responsive toggle group">
  <input type="checkbox" class="btn-check" id="rt1" autocomplete="off">
  <label class="btn btn-outline-primary" for="rt1">Option A</label>
  <input type="checkbox" class="btn-check" id="rt2" autocomplete="off">
  <label class="btn btn-outline-primary" for="rt2">Option B</label>
  <input type="checkbox" class="btn-check" id="rt3" autocomplete="off">
  <label class="btn btn-outline-primary" for="rt3">Option C</label>
  <input type="checkbox" class="btn-check" id="rt4" autocomplete="off">
  <label class="btn btn-outline-primary" for="rt4">Option D</label>
</div>
```

The `flex-wrap` class allows the toggle group to wrap to the next line on narrow viewports, preventing horizontal overflow.

For segmented controls that need to span full width on mobile, use `.d-grid` with `.w-100` on labels:

```html
<div class="btn-group d-flex" role="group" aria-label="Full-width toggle group">
  <input type="radio" class="btn-check" name="view" id="listView" autocomplete="off" checked>
  <label class="btn btn-outline-primary flex-fill" for="listView">List</label>
  <input type="radio" class="btn-check" name="view" id="gridView" autocomplete="off">
  <label class="btn btn-outline-primary flex-fill" for="gridView">Grid</label>
</div>
```

The `d-flex` on the group and `flex-fill` on labels ensure equal-width buttons that fill the available container width, creating a responsive segmented control that adapts to any viewport size.
