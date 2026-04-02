---
title: "Form Accessibility in Bootstrap"
module: "04_ADVANCED_PATTERNS"
lesson: "04_05_Accessibility_Patterns"
file: "04_05_06_Form_Accessibility.md"
difficulty: 2
description: "Label association, fieldset/legend for groups, error announcement, required field indication, autocomplete attributes"
---

## Overview

Forms are the primary interaction point on most web applications. Accessible forms ensure all users, including those using screen readers, keyboards, and voice input, can complete tasks efficiently. Bootstrap provides accessible form components, but developers must follow specific patterns to maintain accessibility.

Essential form accessibility elements:

| Element | Purpose | Bootstrap Class |
|---------|---------|-----------------|
| `<label>` | Associates text with input | `.form-label` |
| `<fieldset>` + `<legend>` | Groups related controls | `.form-group` or raw |
| `aria-describedby` | Links help text and errors | Manual attribute |
| `aria-invalid` | Indicates erroneous input | Manual attribute |
| `required` | Marks mandatory fields | Native HTML attribute |
| `autocomplete` | Hints for autofill | Manual attribute |
| `aria-required` | Redundant required indication | Manual attribute |

## Basic Implementation

### Labeled Input

```html
<div class="mb-3">
  <label for="firstName" class="form-label">First Name</label>
  <input type="text" class="form-control" id="firstName" name="firstName"
         autocomplete="given-name" required aria-required="true">
</div>

<div class="mb-3">
  <label for="email" class="form-label">Email Address</label>
  <input type="email" class="form-control" id="email" name="email"
         autocomplete="email" required aria-required="true"
         aria-describedby="emailHelp">
  <div id="emailHelp" class="form-text">
    We'll use this to send order confirmations.
  </div>
</div>
```

### Input with Error State

```html
<div class="mb-3">
  <label for="password" class="form-label">Password</label>
  <input type="password" class="form-control is-invalid" id="password"
         aria-invalid="true" aria-describedby="passwordError"
         required>
  <div id="passwordError" class="invalid-feedback" role="alert">
    Password must be at least 8 characters long.
  </div>
</div>
```

### Fieldset for Radio Groups

```html
<fieldset class="mb-3">
  <legend class="col-form-label">Preferred Contact Method</legend>
  <div class="form-check">
    <input class="form-check-input" type="radio" name="contactMethod"
           id="contactEmail" value="email" checked>
    <label class="form-check-label" for="contactEmail">Email</label>
  </div>
  <div class="form-check">
    <input class="form-check-input" type="radio" name="contactMethod"
           id="contactPhone" value="phone">
    <label class="form-check-label" for="contactPhone">Phone</label>
  </div>
  <div class="form-check">
    <input class="form-check-input" type="radio" name="contactMethod"
           id="contactMail" value="mail">
    <label class="form-check-label" for="contactMail">Postal Mail</label>
  </div>
</fieldset>
```

### Checkbox Group

```html
<fieldset class="mb-3">
  <legend class="col-form-label">Notification Preferences</legend>
  <div class="form-check">
    <input class="form-check-input" type="checkbox" id="notifEmail" name="notifications"
           value="email">
    <label class="form-check-label" for="notifEmail">Email notifications</label>
  </div>
  <div class="form-check">
    <input class="form-check-input" type="checkbox" id="notifSMS" name="notifications"
           value="sms">
    <label class="form-check-label" for="notifSMS">SMS notifications</label>
  </div>
</fieldset>
```

## Advanced Variations

### Form with Full Accessibility

```html
<form id="registrationForm" novalidate aria-label="User registration">
  <div class="mb-3">
    <label for="regUsername" class="form-label">
      Username <span class="text-danger" aria-hidden="true">*</span>
      <span class="visually-hidden">(required)</span>
    </label>
    <input type="text" class="form-control" id="regUsername" name="username"
           autocomplete="username" required aria-required="true"
           aria-describedby="usernameHelp usernameError" aria-invalid="false">
    <div id="usernameHelp" class="form-text">3-20 characters, letters and numbers only.</div>
    <div id="usernameError" class="invalid-feedback" role="alert" aria-live="polite"></div>
  </div>

  <div class="mb-3">
    <label for="regEmail" class="form-label">
      Email <span class="text-danger" aria-hidden="true">*</span>
      <span class="visually-hidden">(required)</span>
    </label>
    <input type="email" class="form-control" id="regEmail" name="email"
           autocomplete="email" required aria-required="true"
           aria-describedby="emailError" aria-invalid="false">
    <div id="emailError" class="invalid-feedback" role="alert" aria-live="polite"></div>
  </div>

  <!-- Error summary at top of form -->
  <div id="errorSummary" class="alert alert-danger d-none" role="alert"
       aria-live="assertive" tabindex="-1">
    <strong>Please fix the following errors:</strong>
    <ul id="errorList"></ul>
  </div>

  <button type="submit" class="btn btn-primary">Register</button>
</form>

<script>
document.getElementById('registrationForm').addEventListener('submit', function(e) {
  e.preventDefault();
  const errors = [];
  const username = document.getElementById('regUsername');
  const email = document.getElementById('regEmail');

  // Validate username
  if (username.value.length < 3) {
    username.classList.add('is-invalid');
    username.setAttribute('aria-invalid', 'true');
    document.getElementById('usernameError').textContent = 'Username must be at least 3 characters.';
    errors.push({ field: username, message: 'Username must be at least 3 characters.' });
  } else {
    username.classList.remove('is-invalid');
    username.setAttribute('aria-invalid', 'false');
    document.getElementById('usernameError').textContent = '';
  }

  // Validate email
  if (!email.value.includes('@')) {
    email.classList.add('is-invalid');
    email.setAttribute('aria-invalid', 'true');
    document.getElementById('emailError').textContent = 'Please enter a valid email address.';
    errors.push({ field: email, message: 'Please enter a valid email address.' });
  } else {
    email.classList.remove('is-invalid');
    email.setAttribute('aria-invalid', 'false');
    document.getElementById('emailError').textContent = '';
  }

  // Show error summary
  const summary = document.getElementById('errorSummary');
  const errorList = document.getElementById('errorList');
  if (errors.length > 0) {
    errorList.innerHTML = errors.map(e =>
      `<li><a href="#${e.field.id}">${e.message}</a></li>`
    ).join('');
    summary.classList.remove('d-none');
    summary.focus();
  } else {
    summary.classList.add('d-none');
  }
});
</script>
```

### Inline Form with Accessible Labels

```html
<form class="row g-3 align-items-end" aria-label="Search filters">
  <div class="col-md-4">
    <label for="searchTerm" class="form-label">Search</label>
    <input type="search" class="form-control" id="searchTerm"
           placeholder="Enter keywords" aria-label="Search keywords">
  </div>
  <div class="col-md-3">
    <label for="category" class="form-label">Category</label>
    <select class="form-select" id="category" aria-label="Select category">
      <option value="">All categories</option>
      <option value="electronics">Electronics</option>
      <option value="clothing">Clothing</option>
    </select>
  </div>
  <div class="col-md-3">
    <label for="sortBy" class="form-label">Sort by</label>
    <select class="form-select" id="sortBy">
      <option value="relevance">Relevance</option>
      <option value="price-asc">Price: Low to High</option>
      <option value="price-desc">Price: High to Low</option>
    </select>
  </div>
  <div class="col-md-2">
    <button type="submit" class="btn btn-primary w-100">Search</button>
  </div>
</form>
```

### Custom Autocomplete Attributes

```html
<form>
  <div class="mb-3">
    <label for="ccName" class="form-label">Name on card</label>
    <input type="text" class="form-control" id="ccName" autocomplete="cc-name">
  </div>
  <div class="mb-3">
    <label for="ccNumber" class="form-label">Card number</label>
    <input type="text" class="form-control" id="ccNumber" autocomplete="cc-number"
           inputmode="numeric" pattern="[0-9\s]{13,19}">
  </div>
  <div class="row">
    <div class="col-6">
      <label for="ccExpiry" class="form-label">Expiry</label>
      <input type="text" class="form-control" id="ccExpiry"
             autocomplete="cc-exp" placeholder="MM/YY">
    </div>
    <div class="col-6">
      <label for="ccCvc" class="form-label">CVC</label>
      <input type="text" class="form-control" id="ccCvc"
             autocomplete="cc-csc" inputmode="numeric">
    </div>
  </div>
</form>
```

## Best Practices

1. **Always associate labels with inputs using `for`/`id`** - Clicking the label should focus the input. Screen readers announce the label text when the input receives focus.
2. **Use `fieldset` and `legend` for grouped controls** - Radio groups, checkbox groups, and related inputs should be wrapped in a `<fieldset>` with a `<legend>` describing the group.
3. **Indicate required fields accessibly** - Use both `required` attribute and `aria-required="true"`. Visually mark with an asterisk and provide a `.visually-hidden` "(required)" text for screen readers.
4. **Announce errors with `aria-live`** - Error messages should be inside a `role="alert"` or `aria-live="assertive"` region so screen readers announce them immediately.
5. **Set `aria-invalid="true"` on erroneous fields** - This tells screen readers which specific fields have problems, not just that there are errors somewhere.
6. **Link errors to inputs with `aria-describedby`** - The input's `aria-describedby` should reference the error message element's `id`.
7. **Provide an error summary at the top of the form** - After submission, display a summary of all errors with links to each field. Move focus to the summary.
8. **Use `autocomplete` attributes** - Help users fill forms faster with browser autofill by specifying the appropriate autocomplete token (e.g., `given-name`, `email`, `cc-number`).
9. **Never use placeholder as a label** - Placeholders disappear on focus and have low contrast. Always use a visible `<label>`.
10. **Use `inputmode` for mobile keyboards** - Set `inputmode="numeric"` for phone numbers and `inputmode="email"` for email addresses to show the appropriate mobile keyboard.
11. **Group related fields with `fieldset`** - Even non-radio groups benefit from fieldset grouping. Address sections, payment details, and shipping information should each be in a fieldset.
12. **Test forms with a screen reader** - Navigate the entire form using only a screen reader and keyboard to verify all labels, errors, and help text are announced correctly.

## Common Pitfalls

1. **Missing label-input association** - A `<label>` without a `for` attribute, or a `for` that doesn't match any `id`, leaves the input unlabeled. Screen readers announce "edit text" with no context.
2. **Using placeholder as the only label** - Placeholder text disappears on input, leaving users without context. It also typically fails contrast requirements and is not reliably announced by all screen readers.
3. **No error announcement** - Adding a red border and error text is not enough. If the error isn't announced via `aria-live`, screen reader users won't know an error occurred.
4. **Missing `fieldset` on radio/checkbox groups** - Without `fieldset` and `legend`, screen readers announce individual options but not the group question (e.g., "email radio button checked" without "Preferred contact method:").
5. **`aria-describedby` referencing nonexistent IDs** - If the error message `id` doesn't match the `aria-describedby` value, the association is broken. Verify IDs are consistent.
6. **Forgetting `autocomplete` on common fields** - Users with cognitive disabilities benefit from autofill. Omitting `autocomplete="email"` or `autocomplete="new-password"` forces manual entry.
7. **Required field indication without `required` attribute** - Visual asterisks without the `required` attribute mean form validation doesn't enforce the requirement and screen readers don't announce it.
8. **Error messages removed on focus** - Clearing error messages when the user focuses the input prevents them from reading the error while correcting it. Keep errors visible until the field is valid.

## Accessibility Considerations

### Screen Reader Form Announcement Order

When a screen reader user navigates a form, they hear elements in this order:

1. Form label/landmark name
2. Fieldset legend (if grouped)
3. Input label
4. Required state
5. Input type and current value
6. Help text (via `aria-describedby`)
7. Error message (via `aria-describedby`)
8. Additional description (via `aria-describedby`)

### Error Recovery Pattern

```html
<!-- Step 1: Show error summary -->
<div id="errorSummary" role="alert" tabindex="-1" class="alert alert-danger">
  <h2>There are 2 errors in this form</h2>
  <ul>
    <li><a href="#field1">Email address is required</a></li>
    <li><a href="#field2">Password must be 8+ characters</a></li>
  </ul>
</div>

<!-- Step 2: Mark individual fields -->
<div class="mb-3">
  <label for="field1" class="form-label">Email</label>
  <input type="email" class="form-control is-invalid" id="field1"
         aria-invalid="true" aria-describedby="field1Error" required>
  <div id="field1Error" class="invalid-feedback">Email address is required.</div>
</div>
```

### Autocomplete Token Reference

| Field Type | Token |
|-----------|-------|
| First name | `given-name` |
| Last name | `family-name` |
| Email | `email` |
| Phone | `tel` |
| Street address | `street-address` |
| City | `address-level2` |
| Postal code | `postal-code` |
| Country | `country-name` |
| New password | `new-password` |
| Current password | `current-password` |
| Credit card number | `cc-number` |
| One-time code | `one-time-code` |

## Responsive Behavior

Form layout changes across breakpoints but accessibility patterns remain constant:

- **Stacked to inline** - When forms go from stacked (mobile) to inline (desktop), ensure labels remain visible or properly associated. Never hide labels on desktop inline forms without adding `aria-label`.
- **Touch targets** - On mobile, ensure inputs, checkboxes, and radio buttons have minimum 44x44px touch targets. Bootstrap's `.form-check` includes adequate padding.
- **Font size** - iOS Safari zooms into inputs with font-size below 16px. Use at least `16px` (1rem) font size on inputs to prevent unwanted zoom.
- **Responsive fieldsets** - When fieldsets use multi-column layouts on desktop, ensure the visual grouping still matches the `<fieldset>` grouping.

```html
<!-- Responsive two-column form -->
<fieldset>
  <legend class="col-form-label">Shipping Address</legend>
  <div class="row">
    <div class="col-md-6 mb-3">
      <label for="shipFirst" class="form-label">First Name</label>
      <input type="text" class="form-control" id="shipFirst"
             autocomplete="shipping given-name">
    </div>
    <div class="col-md-6 mb-3">
      <label for="shipLast" class="form-label">Last Name</label>
      <input type="text" class="form-control" id="shipLast"
             autocomplete="shipping family-name">
    </div>
  </div>
  <div class="mb-3">
    <label for="shipAddress" class="form-label">Street Address</label>
    <input type="text" class="form-control" id="shipAddress"
           autocomplete="shipping street-address">
  </div>
</fieldset>
```
