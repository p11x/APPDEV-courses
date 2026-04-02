---
title: "Responsive Form Patterns"
lesson: "01_06_15"
difficulty: "1"
topics: ["forms", "inline-forms", "stacked-forms", "form-layout", "responsive"]
estimated_time: "20 minutes"
---

# Responsive Form Patterns

## Overview

Bootstrap forms can be structured as stacked (labels above inputs), horizontal (labels beside inputs), or inline (all elements on one line). Responsive patterns combine these layouts: forms may be stacked on mobile and horizontal on desktop, or inline on desktop and stacked on mobile. The grid system (`row`/`col`) powers horizontal form layouts while display utilities control responsive transitions. Understanding these patterns ensures forms remain usable and accessible across all viewport sizes.

Bootstrap's form controls (`.form-control`, `.form-select`, `.form-check`) provide consistent styling, sizing, and validation states that work responsively without additional markup.

## Basic Implementation

### Stacked Form (Default)

```html
<!-- Stacked: labels above inputs, works at all sizes -->
<form>
  <div class="mb-3">
    <label for="email" class="form-label">Email address</label>
    <input type="email" class="form-control" id="email" placeholder="name@example.com">
  </div>
  <div class="mb-3">
    <label for="password" class="form-label">Password</label>
    <input type="password" class="form-control" id="password">
  </div>
  <div class="mb-3 form-check">
    <input type="checkbox" class="form-check-input" id="remember">
    <label class="form-check-label" for="remember">Remember me</label>
  </div>
  <button type="submit" class="btn btn-primary">Submit</button>
</form>
```

### Horizontal Form

```html
<!-- Labels beside inputs using grid -->
<form>
  <div class="row mb-3">
    <label for="emailH" class="col-sm-2 col-form-label">Email</label>
    <div class="col-sm-10">
      <input type="email" class="form-control" id="emailH">
    </div>
  </div>
  <div class="row mb-3">
    <label for="passwordH" class="col-sm-2 col-form-label">Password</label>
    <div class="col-sm-10">
      <input type="password" class="form-control" id="passwordH">
    </div>
  </div>
  <div class="row mb-3">
    <div class="col-sm-10 offset-sm-2">
      <button type="submit" class="btn btn-primary">Submit</button>
    </div>
  </div>
</form>
```

### Inline Form

```html
<!-- All on one line (wraps on mobile) -->
<form class="row row-cols-auto g-3 align-items-center">
  <div class="col">
    <label class="visually-hidden" for="inlineName">Name</label>
    <input type="text" class="form-control" id="inlineName" placeholder="Name">
  </div>
  <div class="col">
    <label class="visually-hidden" for="inlineEmail">Email</label>
    <input type="email" class="form-control" id="inlineEmail" placeholder="Email">
  </div>
  <div class="col">
    <button type="submit" class="btn btn-primary">Submit</button>
  </div>
</form>
```

## Advanced Variations

### Responsive Horizontal Form (Stacked on Mobile)

```html
<form>
  <!-- Stacked on mobile, horizontal on sm+ -->
  <div class="row mb-3">
    <label for="respName" class="col-12 col-sm-2 col-form-label">Full Name</label>
    <div class="col-12 col-sm-10">
      <input type="text" class="form-control" id="respName">
    </div>
  </div>
  <div class="row mb-3">
    <label for="respEmail" class="col-12 col-sm-2 col-form-label">Email</label>
    <div class="col-12 col-sm-10">
      <input type="email" class="form-control" id="respEmail">
    </div>
  </div>
  <div class="row mb-3">
    <div class="col-12 col-sm-10 offset-sm-2">
      <button type="submit" class="btn btn-primary">Submit</button>
    </div>
  </div>
</form>
```

### Multi-Column Form (Desktop) / Stacked (Mobile)

```html
<form>
  <div class="row g-3">
    <div class="col-12 col-md-6">
      <label class="form-label">First Name</label>
      <input type="text" class="form-control">
    </div>
    <div class="col-12 col-md-6">
      <label class="form-label">Last Name</label>
      <input type="text" class="form-control">
    </div>
    <div class="col-12 col-md-4">
      <label class="form-label">City</label>
      <input type="text" class="form-control">
    </div>
    <div class="col-12 col-md-4">
      <label class="form-label">State</label>
      <select class="form-select"><option>Choose...</option></select>
    </div>
    <div class="col-12 col-md-4">
      <label class="form-label">Zip</label>
      <input type="text" class="form-control">
    </div>
    <div class="col-12">
      <button type="submit" class="btn btn-primary">Submit</button>
    </div>
  </div>
</form>
```

### Responsive Input Group

```html
<!-- Search bar: full width on mobile, constrained on desktop -->
<form class="row justify-content-center">
  <div class="col-12 col-md-8 col-lg-6">
    <div class="input-group">
      <input type="search" class="form-control" placeholder="Search...">
      <button class="btn btn-primary" type="submit">Search</button>
    </div>
  </div>
</form>
```

## Best Practices

1. **Use stacked forms as the default** - Most readable and accessible on all screen sizes.
2. **Use horizontal forms only when labels are short** - Long labels compress inputs on narrow screens.
3. **Use `col-12` on mobile for full-width fields** - Ensures fields don't crowd on small screens.
4. **Always associate labels with inputs using `for`/`id`** - Clicking the label focuses the input.
5. **Use `.form-label` on labels** - Provides consistent spacing and styling.
6. **Group related fields with `<fieldset>` and `<legend>`** - Essential for screen reader context.
7. **Use appropriate input types** - `email`, `tel`, `number`, `date` trigger correct mobile keyboards.
8. **Add `autocomplete` attributes** - Helps browsers and password managers fill forms.
9. **Use `mb-3` between form groups** - Consistent vertical spacing.
10. **Place submit buttons at the bottom of forms** - Natural tab order completion point.
11. **Use `.form-text` for help text** - Styled consistently with Bootstrap.
12. **Test forms with keyboard-only navigation** - Tab through every field and button.

## Common Pitfalls

1. **Using horizontal forms on mobile without `col-12`** - Labels and inputs overlap.
2. **Forgetting `visually-hidden` labels in inline forms** - Screen readers need labels.
3. **Not using `inputmode` on mobile number fields** - Keyboard defaults to letters.
4. **Making form fields too narrow on desktop** - Users can't see what they're typing.
5. **Placing validation messages outside `<div class="mb-3">`** - Breaks Bootstrap's validation styling.

## Accessibility Considerations

Every form input needs a `<label>` with a matching `for` attribute. Use `<fieldset>` and `<legend>` for related groups (radio buttons, checkboxes). Error messages should be associated with inputs via `aria-describedby`. Validation states (`.is-invalid`, `.is-valid`) must be announced by screen readers. Required fields need `aria-required="true"` or the `required` attribute. Responsive form changes should not break the logical tab order.

## Responsive Behavior

Horizontal forms use Bootstrap's grid to reposition labels: `col-sm-2` for labels and `col-sm-10` for inputs on screens above 576px. Below `sm`, both columns become `col-12`, stacking the label above the input. Multi-column forms use `col-md-6` to pair fields on desktop while stacking on mobile. Inline forms use `row-cols-auto` to wrap naturally when space runs out.
