---
title: "Form Error Display Patterns"
description: "Using is-invalid, invalid-feedback, tooltip errors, and summary error lists in Bootstrap forms"
difficulty: 1
tags: ["error-handling", "forms", "validation", "bootstrap"]
prerequisites: ["02_02_Forms"]
---

## Overview

Bootstrap provides built-in validation styling through the `.is-invalid` class and `.invalid-feedback` container. These classes visually flag errors with red borders, error icons, and descriptive messages directly below the offending input. Beyond the basics, Bootstrap supports tooltip-based errors, custom validation styles, and error summary lists for complex forms.

Effective form error display combines three elements: inline field errors for immediate feedback, accessible error messages linked via `aria-describedby`, and an optional error summary at the top of long forms to help users locate all issues quickly.

## Basic Implementation

```html
<!-- Bootstrap validation with is-invalid and invalid-feedback -->
<form class="needs-validation" novalidate>
  <div class="mb-3">
    <label for="emailInput" class="form-label">Email address</label>
    <input type="email" class="form-control is-invalid" id="emailInput"
           aria-describedby="emailFeedback" required>
    <div id="emailFeedback" class="invalid-feedback">
      Please enter a valid email address.
    </div>
  </div>

  <div class="mb-3">
    <label for="passwordInput" class="form-label">Password</label>
    <input type="password" class="form-control is-invalid" id="passwordInput"
           aria-describedby="passwordFeedback" required minlength="8">
    <div id="passwordFeedback" class="invalid-feedback">
      Password must be at least 8 characters long.
    </div>
  </div>

  <div class="mb-3">
    <label for="roleSelect" class="form-label">Role</label>
    <select class="form-select is-invalid" id="roleSelect" aria-describedby="roleFeedback" required>
      <option value="">Choose...</option>
      <option value="admin">Admin</option>
      <option value="user">User</option>
    </select>
    <div id="roleFeedback" class="invalid-feedback">
      Please select a role.
    </div>
  </div>

  <button type="submit" class="btn btn-primary">Submit</button>
</form>
```

```js
// Bootstrap validation script
(() => {
  'use strict';

  const form = document.querySelector('.needs-validation');

  form.addEventListener('submit', (event) => {
    if (!form.checkValidity()) {
      event.preventDefault();
      event.stopPropagation();

      // Focus first invalid field
      const firstInvalid = form.querySelector('.is-invalid');
      if (firstInvalid) {
        firstInvalid.focus();
      }
    }

    form.classList.add('was-validated');
  });

  // Real-time validation on input
  form.querySelectorAll('.form-control, .form-select').forEach(input => {
    input.addEventListener('input', () => {
      if (input.checkValidity()) {
        input.classList.remove('is-invalid');
        input.classList.add('is-valid');
      } else {
        input.classList.remove('is-valid');
        input.classList.add('is-invalid');
      }
    });
  });
})();
```

## Advanced Variations

```html
<!-- Error summary + inline errors pattern -->
<form id="registrationForm" novalidate>
  <!-- Error summary at top -->
  <div id="errorSummary" class="alert alert-danger d-none" role="alert" tabindex="-1">
    <h6 class="alert-heading"><i class="bi bi-exclamation-circle me-1"></i>Please fix the following errors:</h6>
    <ul id="errorList" class="mb-0 ps-3"></ul>
  </div>

  <div class="row g-3">
    <div class="col-md-6">
      <label for="firstName" class="form-label">First Name</label>
      <input type="text" class="form-control" id="firstName" required>
      <div class="invalid-feedback">First name is required.</div>
    </div>
    <div class="col-md-6">
      <label for="lastName" class="form-label">Last Name</label>
      <input type="text" class="form-control" id="lastName" required>
      <div class="invalid-feedback">Last name is required.</div>
    </div>
    <div class="col-12">
      <label for="phone" class="form-label">Phone</label>
      <input type="tel" class="form-control" id="phone" pattern="[0-9]{10}" required>
      <div class="invalid-feedback">Enter a 10-digit phone number.</div>
    </div>
  </div>

  <button type="submit" class="btn btn-primary mt-3">Register</button>
</form>
```

```js
// Error summary builder
function showErrorSummary(form) {
  const summary = document.getElementById('errorSummary');
  const list = document.getElementById('errorList');
  const invalidFields = form.querySelectorAll('.is-invalid, :invalid');

  list.innerHTML = '';

  invalidFields.forEach(field => {
    const label = form.querySelector(`label[for="${field.id}"]`);
    const labelText = label ? label.textContent : field.name || field.id;
    const message = field.validationMessage;

    const li = document.createElement('li');
    const link = document.createElement('a');
    link.href = `#${field.id}`;
    link.textContent = `${labelText}: ${message}`;
    link.addEventListener('click', (e) => {
      e.preventDefault();
      field.focus();
    });
    li.appendChild(link);
    list.appendChild(li);
  });

  if (invalidFields.length > 0) {
    summary.classList.remove('d-none');
    summary.focus();
  } else {
    summary.classList.add('d-none');
  }
}

// Tooltip-style errors
function showTooltipError(input, message) {
  input.classList.add('is-invalid');
  input.setAttribute('title', message);

  const tooltip = new bootstrap.Tooltip(input, {
    title: message,
    placement: 'right',
    trigger: 'manual',
    customClass: 'tooltip-danger'
  });
  tooltip.show();

  input.addEventListener('input', () => {
    if (input.checkValidity()) {
      tooltip.hide();
      input.classList.remove('is-invalid');
    }
  }, { once: true });
}
```

## Best Practices

1. Use `.is-invalid` on the input and `.invalid-feedback` for the message — Bootstrap handles visibility automatically
2. Link error messages to inputs with `aria-describedby` pointing to the feedback element's ID
3. Focus the first invalid field after form submission fails
4. Provide an error summary at the top of long forms (5+ fields) listing all errors
5. Use real-time validation on `blur` (not `input`) to avoid premature errors while typing
6. Clear invalid state immediately when the user corrects the input
7. Use `novalidate` on the form to rely on Bootstrap's custom validation UI
8. Show specific error messages — "Email must contain @" is better than "Invalid input"
9. Preserve user input on validation failure — never clear the form on error
10. Use `.was-validated` to scope validation styles to post-submission state

## Common Pitfalls

1. **Missing `novalidate` attribute** — Browser-native validation popups conflict with Bootstrap's custom styling
2. **Forgetting `.invalid-feedback` inside the input group** — The feedback div must be a sibling of the input for Bootstrap's CSS to work
3. **Showing errors before user interaction** — Premature validation on first keystroke frustrates users; wait for `blur`
4. **Generic error messages** — "This field is required" on every field provides no guidance; customize per field
5. **Not clearing error state on correction** — Leaving `.is-invalid` after the user fixes the input causes confusion
6. **Error summary not linking to fields** — Summaries without clickable links force users to scroll and hunt for fields

## Accessibility Considerations

Connect every error message to its input using `aria-describedby` and use `aria-invalid="true"` on invalid inputs. Error summaries should be focusable (`tabindex="-1"`) and use `role="alert"` so screen readers announce them immediately. Ensure error messages are announced without requiring the user to navigate away from the field.

## Responsive Behavior

Error messages in narrow columns can break layout on mobile. Use `.invalid-tooltip` instead of `.invalid-feedback` on compact forms where inline messages would push content down. On small screens, the error summary should remain visible at the top of the viewport when scrolled to.
