---
title: Form Help Text
category: Component System
difficulty: 1
time: 15 min
tags: bootstrap5, help-text, form-text, tooltips, popovers, forms
---

## Overview

Bootstrap provides multiple mechanisms for displaying help text alongside form controls. The `form-text` class places contextual help below inputs, tooltips offer brief hints on hover, and popovers provide richer expandable help. These tools guide users through form completion, clarify requirements, and reduce input errors.

## Basic Implementation

Use `form-text` below a form control for static contextual help.

```html
<!-- Basic help text -->
<div class="mb-3">
  <label for="passwordInput" class="form-label">Password</label>
  <input type="password" class="form-control" id="passwordInput" aria-describedby="passwordHelp">
  <div id="passwordHelp" class="form-text">
    Must be 8-20 characters long with letters and numbers.
  </div>
</div>

<!-- Help text with muted styling -->
<div class="mb-3">
  <label for="usernameInput" class="form-label">Username</label>
  <input type="text" class="form-control" id="usernameInput" aria-describedby="usernameHelp">
  <div id="usernameHelp" class="form-text text-muted">
    Your unique username cannot be changed later.
  </div>
</div>
```

## Advanced Variations

```html
<!-- Tooltip on input -->
<div class="mb-3">
  <label for="tooltipInput" class="form-label">
    API Key
    <i class="bi bi-info-circle" data-bs-toggle="tooltip" data-bs-placement="top"
       title="Found in your account settings under API section."></i>
  </label>
  <input type="text" class="form-control" id="tooltipInput" placeholder="Enter API key">
</div>

<!-- Popover for detailed field help -->
<div class="mb-3">
  <label for="popoverInput" class="form-label">
    Tax ID
    <button type="button" class="btn btn-link btn-sm p-0" data-bs-toggle="popover"
            data-bs-placement="right" data-bs-title="What is a Tax ID?"
            data-bs-content="Your Tax Identification Number is a 9-digit number issued by the IRS. For individuals, this is typically your Social Security Number (SSN). For businesses, it is your Employer Identification Number (EIN).">
      <i class="bi bi-question-circle"></i>
    </button>
  </label>
  <input type="text" class="form-control" id="popoverInput" placeholder="XX-XXXXXXX">
</div>
```

```html
<!-- Inline help text beside input -->
<div class="row mb-3 align-items-center">
  <div class="col-auto">
    <label for="inlineHelp" class="col-form-label">Amount</label>
  </div>
  <div class="col-auto">
    <input type="number" class="form-control" id="inlineHelp" value="100">
  </div>
  <div class="col-auto">
    <span class="form-text">USD, minimum $10</span>
  </div>
</div>

<!-- Help text with validation state -->
<div class="mb-3">
  <label for="validatedInput" class="form-label">Email</label>
  <input type="email" class="form-control is-invalid" id="validatedInput"
         value="invalid-email" aria-describedby="emailFeedback">
  <div id="emailFeedback" class="invalid-feedback">
    Please enter a valid email address.
  </div>
</div>
```

## Best Practices

1. Use `form-text` for persistent help that should always be visible below the input.
2. Link help text to inputs with `aria-describedby` for screen reader association.
3. Use tooltips for brief hints that do not warrant persistent display.
4. Use popovers for multi-line explanations, examples, or detailed guidance.
5. Keep help text concise; one to two sentences maximum.
6. Place help text consistently (below inputs) across your forms.
7. Use `text-muted` for secondary help text styling.
8. Initialize tooltips and popovers with `new bootstrap.Tooltip(element)`.
9. Provide `data-bs-trigger="focus"` on popovers so keyboard users can access them.
10. Avoid putting critical information only in tooltips; they are not accessible on touch devices.

## Common Pitfalls

1. **Missing `aria-describedby`.** Screen readers cannot link help text to the input without this attribute.
2. **Tooltips for critical information.** Touch device users cannot access hover tooltips.
3. **Popovers without focus trigger.** Keyboard-only users cannot open popovers with default hover trigger.
4. **Overly long help text.** Dense paragraphs below inputs overwhelm users; keep it brief.
5. **Inconsistent placement.** Mixing help text above, below, and beside inputs confuses users.
6. **Not initializing JS components.** Tooltips and popovers require JavaScript initialization to function.

## Accessibility Considerations

Always associate `form-text` with the input using `aria-describedby` matching the help text `id`. For tooltips, use `data-bs-toggle="tooltip"` with descriptive `title` attributes. Popovers should use `data-bs-trigger="focus"` to support keyboard access. Avoid conveying critical errors only through help text; use `invalid-feedback` with `is-invalid` for validation messages. Ensure help text color meets WCAG contrast requirements.

## Responsive Behavior

Help text wraps naturally within its container. In horizontal forms, help text aligns with the input column using Bootstrap's grid. On mobile, ensure help text does not push inputs below the visible viewport by keeping it concise. Inline help text beside inputs should stack on small screens using `col-12` for the help text. Popovers reposition automatically using Popper.js placement logic.
