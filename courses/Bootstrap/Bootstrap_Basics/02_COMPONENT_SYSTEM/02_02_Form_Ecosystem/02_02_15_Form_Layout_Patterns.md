---
title: Form Layout Patterns
category: Component System
difficulty: 2
time: 30 min
tags: bootstrap5, form-layout, horizontal-form, inline-form, form-grid, auto-sizing
---

## Overview

Bootstrap provides flexible layout patterns for organizing forms. Horizontal forms pair labels and inputs side by side using the grid system, inline forms place controls on a single row, form grids use `row` and `col` classes for multi-column layouts, and auto-sizing lets inputs expand to fit their content. These patterns accommodate everything from simple contact forms to complex data entry interfaces.

## Basic Implementation

Form grids use Bootstrap's row and column classes to create multi-column layouts.

```html
<!-- Basic form grid -->
<form>
  <div class="row mb-3">
    <div class="col-md-6">
      <label for="gridFirst" class="form-label">First Name</label>
      <input type="text" class="form-control" id="gridFirst">
    </div>
    <div class="col-md-6">
      <label for="gridLast" class="form-label">Last Name</label>
      <input type="text" class="form-control" id="gridLast">
    </div>
  </div>
  <div class="mb-3">
    <label for="gridEmail" class="form-label">Email</label>
    <input type="email" class="form-control" id="gridEmail">
  </div>
  <button type="submit" class="btn btn-primary">Submit</button>
</form>
```

## Advanced Variations

```html
<!-- Horizontal form layout -->
<form>
  <div class="row mb-3">
    <label for="hName" class="col-sm-2 col-form-label">Name</label>
    <div class="col-sm-10">
      <input type="text" class="form-control" id="hName">
    </div>
  </div>
  <div class="row mb-3">
    <label for="hEmail" class="col-sm-2 col-form-label">Email</label>
    <div class="col-sm-10">
      <input type="email" class="form-control" id="hEmail">
    </div>
  </div>
  <div class="row mb-3">
    <legend class="col-form-label col-sm-2 pt-0">Radios</legend>
    <div class="col-sm-10">
      <div class="form-check">
        <input class="form-check-input" type="radio" name="hRadio" id="hRadio1" checked>
        <label class="form-check-label" for="hRadio1">First radio</label>
      </div>
      <div class="form-check">
        <input class="form-check-input" type="radio" name="hRadio" id="hRadio2">
        <label class="form-check-label" for="hRadio2">Second radio</label>
      </div>
    </div>
  </div>
  <button type="submit" class="btn btn-primary">Sign in</button>
</form>
```

```html
<!-- Inline form -->
<form class="row row-cols-lg-auto g-3 align-items-center">
  <div class="col-12">
    <label class="visually-hidden" for="inlineName">Name</label>
    <input type="text" class="form-control" id="inlineName" placeholder="Jane Doe">
  </div>
  <div class="col-12">
    <label class="visually-hidden" for="inlineUsername">Username</label>
    <div class="input-group">
      <span class="input-group-text">@</span>
      <input type="text" class="form-control" id="inlineUsername" placeholder="Username">
    </div>
  </div>
  <div class="col-12">
    <label class="visually-hidden" for="inlineCity">City</label>
    <input type="text" class="form-control" id="inlineCity" placeholder="City">
  </div>
  <div class="col-12">
    <button type="submit" class="btn btn-primary">Submit</button>
  </div>
</form>
```

```html
<!-- Auto-sizing form -->
<form>
  <div class="row g-3 align-items-center mb-3">
    <div class="col-auto">
      <label for="autoName" class="col-form-label">Name</label>
    </div>
    <div class="col-auto">
      <input type="text" id="autoName" class="form-control">
    </div>
  </div>
  <div class="row g-3 align-items-center">
    <div class="col-auto">
      <label for="autoEmail" class="col-form-label">Email</label>
    </div>
    <div class="col-auto">
      <input type="email" id="autoEmail" class="form-control">
    </div>
  </div>
</form>
```

## Best Practices

1. Use `col-sm-2` for labels and `col-sm-10` for inputs in horizontal forms.
2. Apply `col-form-label` to labels in horizontal forms for vertical alignment.
3. Use `row-cols-lg-auto` for inline forms that stack on mobile.
4. Provide `visually-hidden` labels in inline forms for accessibility.
5. Use `g-3` gutter classes for consistent spacing between columns.
6. Wrap multi-column forms in `row` with `col-*` children for grid layouts.
7. Use `col-md-6` to create two-column layouts that stack on small screens.
8. Apply `align-items-center` on inline form rows for vertical alignment.
9. Use `pt-0` on legend elements in horizontal forms to remove top padding.
10. Test forms at each breakpoint to ensure proper stacking behavior.

## Common Pitfalls

1. **Missing row wrapper.** Columns without a `row` parent will not align properly.
2. **Not using `col-form-label`.** Labels without this class misalign vertically in horizontal forms.
3. **Forgetting responsive breakpoints.** Using `col-6` without `col-md-6` prevents mobile stacking.
4. **Missing `visually-hidden` labels.** Inline forms without hidden labels are inaccessible.
5. **Overcrowded inline forms.** Too many controls on one line causes overflow on mobile.
6. **Inconsistent gutters.** Missing `g-*` classes result in cramped or unevenly spaced controls.

## Accessibility Considerations

Every form control needs a visible or `visually-hidden` label with a matching `for` attribute. Horizontal forms use `<legend>` with `col-form-label` for radio and checkbox groups. Inline forms must include hidden labels for screen readers. Use `role="form"` only when the form is not inside a `<form>` element. Ensure keyboard navigation follows visual tab order across multi-column layouts.

## Responsive Behavior

Form grids stack columns vertically on small screens using responsive column classes. Horizontal forms collapse to stacked layout below `sm` breakpoint (`col-sm-2` becomes full-width). Inline forms use `row-cols-lg-auto` to remain horizontal on large screens and stack on smaller ones. Auto-sizing forms adapt naturally. Use `g-*` gutter classes for consistent spacing that adjusts with breakpoints.
