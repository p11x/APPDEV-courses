---
title: Form Disabled States
category: Component System
difficulty: 1
time: 15 min
tags: bootstrap5, disabled, readonly, forms, fieldset, states
---

## Overview

Bootstrap provides multiple ways to disable form controls and indicate non-interactive states. The `disabled` attribute prevents interaction on individual inputs, `<fieldset disabled>` disables entire groups, and `readonly` allows viewing but not editing. Bootstrap applies visual styling automatically, dimming disabled controls and removing them from form submission.

## Basic Implementation

Apply `disabled` to individual form controls to prevent interaction.

```html
<!-- Disabled text input -->
<div class="mb-3">
  <label for="disabledInput" class="form-label">Disabled input</label>
  <input type="text" class="form-control" id="disabledInput" placeholder="Cannot edit this" disabled>
</div>

<!-- Disabled select -->
<div class="mb-3">
  <label for="disabledSelect" class="form-label">Disabled select</label>
  <select class="form-select" id="disabledSelect" disabled>
    <option>Cannot change this</option>
  </select>
</div>

<!-- Disabled checkbox and radio -->
<div class="form-check">
  <input class="form-check-input" type="checkbox" id="disabledCheck" disabled>
  <label class="form-check-label" for="disabledCheck">Disabled checkbox</label>
</div>
<div class="form-check">
  <input class="form-check-input" type="radio" name="disabledRadio" id="disabledRadio" disabled>
  <label class="form-check-label" for="disabledRadio">Disabled radio</label>
</div>
```

## Advanced Variations

```html
<!-- Disabled fieldset (disables all controls inside) -->
<fieldset disabled>
  <legend class="col-form-label">Disabled form group</legend>
  <div class="mb-3">
    <label for="fsInput" class="form-label">Name</label>
    <input type="text" class="form-control" id="fsInput" placeholder="Full name">
  </div>
  <div class="mb-3">
    <label for="fsEmail" class="form-label">Email</label>
    <input type="email" class="form-control" id="fsEmail" placeholder="email@example.com">
  </div>
  <button type="submit" class="btn btn-primary">Submit</button>
</fieldset>
```

```html
<!-- Readonly input -->
<div class="mb-3">
  <label for="readonlyInput" class="form-label">Readonly input</label>
  <input type="text" class="form-control" id="readonlyInput" value="User ID: 12345" readonly>
</div>

<!-- Readonly textarea -->
<div class="mb-3">
  <label for="readonlyArea" class="form-label">Terms and conditions</label>
  <textarea class="form-control" id="readonlyArea" rows="3" readonly>By using this service, you agree to the following terms...</textarea>
</div>

<!-- Disabled with plain text fallback -->
<div class="mb-3">
  <label class="form-label">Email</label>
  <input type="email" class="form-control-plaintext" value="user@example.com" readonly>
</div>
```

```html
<!-- Conditional disabling with JavaScript -->
<div class="mb-3">
  <div class="form-check mb-2">
    <input class="form-check-input" type="checkbox" id="enableFields">
    <label class="form-check-label" for="enableFields">Enable editing</label>
  </div>
  <label for="condInput" class="form-label">Address</label>
  <input type="text" class="form-control" id="condInput" placeholder="Enter address" disabled>
</div>
```

## Best Practices

1. Use `disabled` on individual controls to prevent interaction and form submission.
2. Use `<fieldset disabled>` to disable an entire group of related controls at once.
3. Use `readonly` when users should see but not modify a value (e.g., IDs, calculated fields).
4. Pair disabled inputs with muted labels to visually reinforce the disabled state.
5. Provide a tooltip or helper text explaining why a control is disabled.
6. Use `form-control-plaintext` for read-only values that need no input styling.
7. Re-enable fields dynamically with JavaScript when prerequisites are met.
8. Do not rely on `disabled` alone for security; validate on the server.
9. Remove `disabled` before serializing forms if you need disabled values submitted.
10. Test keyboard navigation to ensure disabled fields are skipped.

## Common Pitfalls

1. **Disabled fields not submitted.** `disabled` inputs are excluded from form data; use `readonly` if submission is needed.
2. **No visual indicator.** Custom styled inputs may not show the disabled appearance without Bootstrap classes.
3. **Forgetting fieldset wrapping.** Disabling controls individually is error-prone; use `<fieldset disabled>` for groups.
4. **Disabled vs readonly confusion.** `disabled` prevents focus and submission; `readonly` allows focus and submission.
5. **Security reliance on disabled.** Disabled fields can be re-enabled via DevTools; always validate server-side.
6. **Missing explanations.** Users confused about why fields are disabled need contextual help text.

## Accessibility Considerations

Disabled controls are automatically removed from the tab order by the browser. Announce disabled state with `aria-disabled="true"` when using custom controls that cannot use the native `disabled` attribute. Provide `aria-describedby` linking to explanatory text for why a field is disabled. Use `<fieldset disabled>` with a `<legend>` to describe the disabled group. Avoid disabling primary actions without visible explanation.

## Responsive Behavior

Disabled form controls maintain the same layout as enabled controls. In horizontal forms, disabled inputs align with their labels using Bootstrap's grid. On mobile, disabled inputs still receive focus styling from the browser. Use `form-control-plaintext` for read-only values that should not display input borders, maintaining clean layouts at all breakpoints.
