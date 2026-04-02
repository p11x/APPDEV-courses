---
title: Form Sizing
category: Component System
difficulty: 1
time: 15 min
tags: bootstrap5, form-sizing, form-control-sm, form-control-lg, forms
---

## Overview

Bootstrap provides size variants for form controls using `form-control-sm` and `form-control-lg` classes for text inputs, `form-select-sm` and `form-select-lg` for selects, and sizing classes for input groups. These classes adjust padding, font size, and border radius to match compact or prominent design contexts. Labels and form groups can also be sized using utility classes.

## Basic Implementation

Apply sizing classes directly to form controls for small, default, and large variants.

```html
<!-- Small input -->
<div class="mb-3">
  <label for="smallInput" class="form-label">Small input</label>
  <input type="text" class="form-control form-control-sm" id="smallInput" placeholder="Small">
</div>

<!-- Default input -->
<div class="mb-3">
  <label for="defaultInput" class="form-label">Default input</label>
  <input type="text" class="form-control" id="defaultInput" placeholder="Default">
</div>

<!-- Large input -->
<div class="mb-3">
  <label for="largeInput" class="form-label">Large input</label>
  <input type="text" class="form-control form-control-lg" id="largeInput" placeholder="Large">
</div>
```

## Advanced Variations

```html
<!-- Sized selects -->
<div class="mb-3">
  <label for="smallSelect" class="form-label">Small select</label>
  <select class="form-select form-select-sm" id="smallSelect">
    <option selected>Small select</option>
    <option value="1">One</option>
    <option value="2">Two</option>
  </select>
</div>
<div class="mb-3">
  <label for="largeSelect" class="form-label">Large select</label>
  <select class="form-select form-select-lg" id="largeSelect">
    <option selected>Large select</option>
    <option value="1">One</option>
    <option value="2">Two</option>
  </select>
</div>
```

```html
<!-- Sized input groups -->
<div class="mb-3">
  <label class="form-label">Small input group</label>
  <div class="input-group input-group-sm">
    <span class="input-group-text">@</span>
    <input type="text" class="form-control" placeholder="Username">
  </div>
</div>
<div class="mb-3">
  <label class="form-label">Large input group</label>
  <div class="input-group input-group-lg">
    <span class="input-group-text">$</span>
    <input type="text" class="form-control" placeholder="Amount">
    <span class="input-group-text">.00</span>
  </div>
</div>
```

```html
<!-- Mixed sizing in a form -->
<form>
  <div class="row g-3">
    <div class="col-md-6">
      <label for="firstNameSm" class="form-label small">First Name</label>
      <input type="text" class="form-control form-control-sm" id="firstNameSm">
    </div>
    <div class="col-md-6">
      <label for="lastNameSm" class="form-label small">Last Name</label>
      <input type="text" class="form-control form-control-sm" id="lastNameSm">
    </div>
  </div>
  <div class="mt-3">
    <label for="emailLg" class="form-label fs-5">Email Address</label>
    <input type="email" class="form-control form-control-lg" id="emailLg" placeholder="you@example.com">
  </div>
</form>
```

## Best Practices

1. Use consistent sizing across all controls within a single form.
2. Apply `form-control-sm` for compact interfaces like table filters and toolbars.
3. Use `form-control-lg` for prominent forms like login pages and hero sections.
4. Match input group sizing with `input-group-sm` or `input-group-lg`.
5. Size labels with utility classes (`small`, `fs-5`) to match control sizing.
6. Avoid mixing small and large controls in the same form row.
7. Use default sizing for most standard forms.
8. Test readability of small controls on high-DPI displays.
9. Ensure small controls still meet minimum touch target sizes on mobile.
10. Apply `form-select-sm/lg` for sized dropdowns independently of input sizing.

## Common Pitfalls

1. **Inconsistent sizing.** Mixing `form-control-sm` and `form-control-lg` in the same form looks unprofessional.
2. **Too-small controls on mobile.** `form-control-sm` may produce touch targets below 44px.
3. **Forgetting input group sizing.** Input groups need their own `input-group-sm/lg` classes.
4. **Label size mismatch.** Large inputs with tiny labels create visual imbalance.
5. **Overusing large controls.** Large variants consume excessive vertical space in dense forms.

## Accessibility Considerations

Sized form controls remain accessible as long as labels are correctly associated with `for` attributes. Ensure small controls maintain adequate color contrast for borders and text. Verify that `form-control-sm` still meets minimum touch target guidelines on mobile (44x44px). Use `aria-describedby` to link help text regardless of control size. Large controls benefit users with motor impairments due to bigger click targets.

## Responsive Behavior

Form sizing classes apply at all breakpoints. Use responsive utility classes to change sizing conditionally: pair `form-control-sm` with `form-control-lg` at different breakpoints if needed. Input groups maintain their sizing across screen sizes. On mobile, consider using default or large sizing to ensure adequate touch targets. Stack small inline forms vertically on narrow screens.
