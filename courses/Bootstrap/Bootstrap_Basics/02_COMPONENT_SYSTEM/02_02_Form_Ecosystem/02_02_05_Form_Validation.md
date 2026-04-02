---
title: Form Validation
category: Form Ecosystem
difficulty: 2
time: 40 minutes
tags:
  - bootstrap
  - forms
  - validation
  - feedback
  - was-validated
---

## Overview

Bootstrap provides a comprehensive form validation system that works with both browser-native HTML5 validation attributes and custom JavaScript-driven validation logic. The system uses two primary CSS classes, `.is-valid` and `.is-invalid`, to visually indicate the state of each form control. These classes are paired with `.valid-feedback` and `.invalid-feedback` elements that display contextual messages to the user.

There are two main approaches to validation in Bootstrap. The first uses the browser's built-in validation triggered by adding the `novalidate` attribute to the `<form>` element along with the `.was-validated` class. This approach leverages HTML5 constraint validation API attributes like `required`, `minlength`, `maxlength`, `pattern`, `type="email"`, and `type="url"`. The second approach applies `.is-valid` and `.is-invalid` classes directly via JavaScript, giving you full control over when and how validation states are displayed.

The `.was-validated` approach is the most common pattern. When this class is present on a form, Bootstrap shows validation feedback for all controls based on their native validity state. Controls that pass validation display green borders and success messages, while invalid controls show red borders and error messages. This class is typically added after the user attempts to submit the form, preventing premature feedback during initial data entry.

Bootstrap also supports tooltip-style validation feedback using the `.valid-tooltip` and `.invalid-tooltip` classes instead of the feedback div classes. These display messages as positioned tooltips relative to the form control, which is useful in compact layouts where inline feedback would disrupt the form's vertical rhythm.

## Basic Implementation

The simplest validation pattern uses the `.was-validated` class on the form element with HTML5 validation attributes on each control. The `novalidate` attribute on the form prevents the browser's default validation bubbles, allowing Bootstrap to handle the visual feedback instead.

```html
<form class="was-validated" novalidate>
  <div class="mb-3">
    <label for="valName" class="form-label">Full Name</label>
    <input type="text" class="form-control" id="valName" required>
    <div class="valid-feedback">Looks good!</div>
    <div class="invalid-feedback">Please enter your name.</div>
  </div>

  <div class="mb-3">
    <label for="valEmail" class="form-label">Email</label>
    <input type="email" class="form-control" id="valEmail" required>
    <div class="valid-feedback">Email format is valid.</div>
    <div class="invalid-feedback">Please provide a valid email address.</div>
  </div>

  <button type="submit" class="btn btn-primary">Submit</button>
</form>
```

Applying validation classes manually via JavaScript gives you precise control over when feedback appears. This is useful for real-time validation that responds to user input rather than form submission.

```html
<form id="jsForm">
  <div class="mb-3">
    <label for="jsUsername" class="form-label">Username</label>
    <input type="text" class="form-control" id="jsUsername">
    <div class="invalid-feedback">Username must be at least 3 characters.</div>
  </div>

  <button type="submit" class="btn btn-primary">Submit</button>
</form>

<script>
document.getElementById('jsForm').addEventListener('submit', function(e) {
  e.preventDefault();
  const input = document.getElementById('jsUsername');

  if (input.value.length >= 3) {
    input.classList.remove('is-invalid');
    input.classList.add('is-valid');
  } else {
    input.classList.remove('is-valid');
    input.classList.add('is-invalid');
  }
});
</script>
```

The `required` attribute on select elements ensures a selection is made before the form can be submitted.

```html
<form class="was-validated" novalidate>
  <div class="mb-3">
    <label for="valSelect" class="form-label">Choose a plan</label>
    <select class="form-select" id="valSelect" required>
      <option value="">Select a plan...</option>
      <option value="basic">Basic</option>
      <option value="pro">Pro</option>
      <option value="enterprise">Enterprise</option>
    </select>
    <div class="invalid-feedback">Please select a plan.</div>
  </div>

  <button type="submit" class="btn btn-primary">Continue</button>
</form>
```

Checkbox validation ensures that required agreements are accepted before submission.

```html
<form class="was-validated" novalidate>
  <div class="mb-3">
    <div class="form-check">
      <input type="checkbox" class="form-check-input" id="termsCheck" required>
      <label class="form-check-label" for="termsCheck">I agree to the terms and conditions</label>
      <div class="invalid-feedback">You must agree before submitting.</div>
    </div>
  </div>

  <button type="submit" class="btn btn-primary">Register</button>
</form>
```

Textarea validation works identically to text input validation. The `minlength` attribute enforces a minimum character count.

```html
<form class="was-validated" novalidate>
  <div class="mb-3">
    <label for="valBio" class="form-label">Bio</label>
    <textarea class="form-control" id="valBio" rows="3" minlength="20" required></textarea>
    <div class="invalid-feedback">Bio must be at least 20 characters.</div>
    <div class="valid-feedback">Thank you for the detailed bio.</div>
  </div>

  <button type="submit" class="btn btn-primary">Save</button>
</form>
```

## Advanced Variations

Tooltip-based validation displays feedback as floating tooltips rather than inline text. This is particularly useful for inline forms or layouts with limited vertical space.

```html
<form class="row g-3 was-validated" novalidate>
  <div class="col-md-6 position-relative">
    <label for="tooltipFirst" class="form-label">First Name</label>
    <input type="text" class="form-control" id="tooltipFirst" required>
    <div class="valid-tooltip">Looks good!</div>
    <div class="invalid-tooltip">First name is required.</div>
  </div>

  <div class="col-md-6 position-relative">
    <label for="tooltipLast" class="form-label">Last Name</label>
    <input type="text" class="form-control" id="tooltipLast" required>
    <div class="valid-tooltip">Looks good!</div>
    <div class="invalid-tooltip">Last name is required.</div>
  </div>

  <div class="col-12">
    <button type="submit" class="btn btn-primary">Submit</button>
  </div>
</form>
```

Custom JavaScript validation with real-time feedback provides a better user experience by validating on input events rather than waiting for form submission. This pattern checks validity as the user types and shows feedback immediately.

```html
<form id="realTimeForm" class="needs-validation" novalidate>
  <div class="mb-3">
    <label for="rtPassword" class="form-label">Password</label>
    <input type="password" class="form-control" id="rtPassword" required minlength="8">
    <div class="invalid-feedback">Password must be at least 8 characters.</div>
  </div>

  <div class="mb-3">
    <label for="rtConfirm" class="form-label">Confirm Password</label>
    <input type="password" class="form-control" id="rtConfirm" required>
    <div class="invalid-feedback">Passwords do not match.</div>
    <div class="valid-feedback">Passwords match.</div>
  </div>

  <button type="submit" class="btn btn-primary">Set Password</button>
</form>

<script>
(function() {
  const form = document.getElementById('realTimeForm');
  const password = document.getElementById('rtPassword');
  const confirm = document.getElementById('rtConfirm');

  function validateConfirm() {
    if (confirm.value === password.value && confirm.value.length > 0) {
      confirm.classList.remove('is-invalid');
      confirm.classList.add('is-valid');
    } else if (confirm.value.length > 0) {
      confirm.classList.remove('is-valid');
      confirm.classList.add('is-invalid');
    }
  }

  password.addEventListener('input', validateConfirm);
  confirm.addEventListener('input', validateConfirm);

  form.addEventListener('submit', function(e) {
    if (!form.checkValidity()) {
      e.preventDefault();
      e.stopPropagation();
    }
    form.classList.add('was-validated');
  });
})();
</script>
```

Server-side validation results can be displayed by applying `.is-valid` or `.is-invalid` classes based on the response. This pattern is common when the server returns validation errors for specific fields after a form submission via AJAX.

```html
<form id="serverForm">
  <div class="mb-3">
    <label for="serverEmail" class="form-label">Email</label>
    <input type="email" class="form-control is-invalid" id="serverEmail" value="taken@example.com">
    <div class="invalid-feedback">This email address is already registered.</div>
  </div>

  <div class="mb-3">
    <label for="serverUsername" class="form-label">Username</label>
    <input type="text" class="form-control is-valid" id="serverUsername" value="available_name">
    <div class="valid-feedback">Username is available.</div>
  </div>

  <button type="submit" class="btn btn-primary">Try Again</button>
</form>
```

Validation with the `pattern` attribute allows you to define custom regular expressions for format validation.

```html
<form class="was-validated" novalidate>
  <div class="mb-3">
    <label for="phoneInput" class="form-label">Phone Number</label>
    <input
      type="tel"
      class="form-control"
      id="phoneInput"
      pattern="^\d{3}-\d{3}-\d{4}$"
      placeholder="123-456-7890"
      required
    >
    <div class="invalid-feedback">Please enter a valid phone number (123-456-7890).</div>
  </div>

  <div class="mb-3">
    <label for="zipInput" class="form-label">ZIP Code</label>
    <input
      type="text"
      class="form-control"
      id="zipInput"
      pattern="^\d{5}(-\d{4})?$"
      placeholder="12345 or 12345-6789"
      required
    >
    <div class="invalid-feedback">Please enter a valid ZIP code.</div>
  </div>

  <button type="submit" class="btn btn-primary">Submit</button>
</form>
```

## Best Practices

- **Always use `novalidate` on the `<form>` element** when using Bootstrap's validation classes. This suppresses the browser's default validation tooltips and allows Bootstrap to control the visual feedback.
- **Use `.was-validated` on form submission**, not on page load. Adding this class immediately creates a poor user experience by flagging every empty field as invalid before the user has started filling in the form.
- **Provide specific, actionable error messages** in `.invalid-feedback` elements. "Please enter a valid email address" is far more helpful than "Invalid input."
- **Include both `.valid-feedback` and `.invalid-feedback`** for each control. Even if you only show validation after submission, having both messages ready ensures the UI is complete.
- **Place `.invalid-feedback` and `.valid-feedback` as direct siblings** of the form control, not nested inside another element. Bootstrap uses the adjacent sibling selector to show and hide these elements.
- **Use `type="email"`, `type="url"`, and `type="tel"`** for format-specific inputs. These types trigger built-in browser validation for common patterns without requiring custom regular expressions.
- **Validate on the server regardless of client-side validation.** Client-side validation can be bypassed. Always perform the same checks on the server before processing the data.
- **Use `.is-valid` and `.is-invalid` with JavaScript** when you need real-time validation feedback. Apply these classes in response to `input`, `change`, or `blur` events for responsive UX.
- **Combine `required`, `minlength`, `maxlength`, `pattern`, and `type` attributes** as needed. Multiple constraints can be active on a single input, and Bootstrap will show the appropriate feedback based on which constraint fails first.
- **Test validation states with screen readers.** Verify that `.invalid-feedback` text is announced when the input receives focus in an invalid state. The `aria-describedby` attribute can help link feedback messages explicitly to their controls.
- **Avoid validation on every keystroke for performance-sensitive forms.** Debounce validation logic to prevent excessive DOM updates while the user is typing rapidly.
- **Reset validation classes when the user corrects an error.** If you apply `.is-invalid` via JavaScript, also apply `.is-valid` or remove the class when the input becomes valid, so the red border disappears.

## Common Pitfalls

- **Forgetting `novalidate` on the form element.** Without it, the browser shows its native validation popups alongside Bootstrap's feedback messages, creating a confusing double-feedback experience.
- **Adding `.was-validated` on page load instead of after submission.** This marks every empty required field as invalid before the user has interacted with the form. Always add the class only after the user attempts to submit.
- **Placing `.invalid-feedback` inside a wrapper div.** Bootstrap's CSS shows `.invalid-feedback` only when it is an immediate sibling of an invalid form control or when the parent has the `.was-validated` class. Nesting it too deeply breaks this selector chain.
- **Using `.invalid-feedback` with `.form-check` inputs incorrectly.** For checkboxes and radios, the `.invalid-feedback` element must be placed after the `.form-check-input`, not after the label.
- **Not clearing validation state on subsequent submissions.** If a user submits an invalid form, corrects errors, and resubmits, the old `.is-invalid` classes may persist. Always reset validation classes before reapplying them.
- **Relying solely on HTML5 `pattern` attribute without explaining the format.** A `pattern` attribute like `pattern="[A-Za-z]{3}"` silently rejects input without telling the user the expected format. Always provide an explicit `.invalid-feedback` message.
- **Forgetting that `disabled` and `readonly` inputs skip validation.** The browser does not validate disabled or readonly fields. If a field must be validated but not editable, consider making it visually read-only with CSS and `aria-readonly` instead of the `disabled` attribute.
- **Using tooltip validation without `position: relative` on the parent.** `.valid-tooltip` and `.invalid-tooltip` require a positioned parent to calculate their placement. Missing `position-relative` causes tooltips to appear in the wrong location or not at all.

## Accessibility Considerations

Bootstrap's validation feedback is accessible by default when implemented correctly. Invalid form controls gain red borders and display error messages, while valid controls show green borders with success messages. Screen readers can announce these states, but proper markup ensures the best experience.

The `.invalid-feedback` element should be linked to its associated input using the `aria-describedby` attribute. This explicitly tells assistive technologies which element contains the error message for a given control. Without this link, screen readers may not announce the feedback when the user focuses the invalid input.

```html
<form class="was-validated" novalidate>
  <div class="mb-3">
    <label for="accessibleEmail" class="form-label">Email Address</label>
    <input
      type="email"
      class="form-control"
      id="accessibleEmail"
      required
      aria-describedby="emailFeedback"
    >
    <div id="emailFeedback" class="invalid-feedback">
      Please enter a valid email address.
    </div>
  </div>
</form>
```

For forms with multiple validation errors, use `aria-live="polite"` on a summary container to announce the total number of errors after submission. This gives screen reader users an immediate overview without having to tab through each field.

```html
<div id="errorSummary" class="alert alert-danger d-none" role="alert" aria-live="polite"></div>
```

When using tooltip-based validation, be aware that screen readers do not automatically announce tooltip content. You may need to use `role="alert"` or `aria-live` on the tooltip element to ensure it is announced when it appears.

## Responsive Behavior

Bootstrap's validation feedback elements are fully responsive by default. The `.valid-feedback` and `.invalid-feedback` divs span the full width of their parent container, adapting naturally to all screen sizes.

In grid-based forms using Bootstrap's row and column classes, validation feedback works correctly within each column. A feedback message inside a `col-md-6` column will wrap to the next line if the message text is longer than the column width on smaller screens.

```html
<form class="row g-3 was-validated" novalidate>
  <div class="col-md-4">
    <label for="respCity" class="form-label">City</label>
    <input type="text" class="form-control" id="respCity" required>
    <div class="invalid-feedback">City is required.</div>
  </div>

  <div class="col-md-4">
    <label for="respState" class="form-label">State</label>
    <input type="text" class="form-control" id="respState" required>
    <div class="invalid-feedback">State is required.</div>
  </div>

  <div class="col-md-4">
    <label for="respZip" class="form-label">Zip</label>
    <input type="text" class="form-control" id="respZip" required pattern="\d{5}">
    <div class="invalid-feedback">Valid 5-digit ZIP required.</div>
  </div>

  <div class="col-12">
    <button type="submit" class="btn btn-primary">Submit</button>
  </div>
</form>
```

On very narrow screens, long inline feedback messages can cause horizontal overflow. Consider using shorter feedback text on mobile or switching to tooltip-based validation on small viewports by toggling between `.invalid-feedback` and `.invalid-tooltip` classes with media query detection in JavaScript.

For horizontal forms using `.row` and `.col-*-*` for label-input layouts, validation feedback should be placed inside the input column so it aligns with the input rather than spanning the full form width. Use `col-12` on the feedback container if it needs to break below the input on small screens.
