---
title: Rating Stars
category: Component System
difficulty: 2
time: 25 min
tags: bootstrap5, rating, stars, forms, interactive, accessibility
---

## Overview

Star rating components let users select a value from a visual scale of stars. Bootstrap does not include a native star rating, but you can build one using hidden radio inputs styled with CSS and Bootstrap utilities. This pattern supports full stars, half-star increments, read-only display, and keyboard accessibility while maintaining Bootstrap's form integration.

## Basic Implementation

A star rating uses radio buttons with labels styled as stars using CSS and Unicode characters or icon fonts.

```html
<!-- Star rating using radio buttons and CSS -->
<style>
  .star-rating {
    display: flex;
    flex-direction: row-reverse;
    justify-content: flex-end;
    font-size: 1.5rem;
  }
  .star-rating input {
    display: none;
  }
  .star-rating label {
    color: #dee2e6;
    cursor: pointer;
    padding: 0 2px;
  }
  .star-rating input:checked ~ label,
  .star-rating label:hover,
  .star-rating label:hover ~ label {
    color: #ffc107;
  }
</style>

<div class="star-rating">
  <input type="radio" id="star5" name="rating" value="5">
  <label for="star5" title="5 stars">&#9733;</label>
  <input type="radio" id="star4" name="rating" value="4">
  <label for="star4" title="4 stars">&#9733;</label>
  <input type="radio" id="star3" name="rating" value="3">
  <label for="star3" title="3 stars">&#9733;</label>
  <input type="radio" id="star2" name="rating" value="2">
  <label for="star2" title="2 stars">&#9733;</label>
  <input type="radio" id="star1" name="rating" value="1">
  <label for="star1" title="1 star">&#9733;</label>
</div>
```

## Advanced Variations

```html
<!-- Read-only star display -->
<div class="d-flex align-items-center">
  <span class="text-warning fs-5">
    &#9733;&#9733;&#9733;&#9733;&#9734;
  </span>
  <span class="ms-2 text-muted">4.0 out of 5</span>
</div>

<!-- Star rating with Bootstrap Icons -->
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css">

<style>
  .bi-star-rating input { display: none; }
  .bi-star-rating label { color: #dee2e6; cursor: pointer; font-size: 1.5rem; }
  .bi-star-rating input:checked ~ label i.bi-star-fill { color: #ffc107; }
  .bi-star-rating label:hover i,
  .bi-star-rating label:hover ~ label i { color: #ffc107; }
</style>

<div class="bi-star-rating d-flex flex-row-reverse justify-content-end">
  <input type="radio" id="bi5" name="biRating" value="5">
  <label for="bi5"><i class="bi bi-star-fill"></i></label>
  <input type="radio" id="bi4" name="biRating" value="4">
  <label for="bi4"><i class="bi bi-star-fill"></i></label>
  <input type="radio" id="bi3" name="biRating" value="3">
  <label for="bi3"><i class="bi bi-star-fill"></i></label>
  <input type="radio" id="bi2" name="biRating" value="2">
  <label for="bi2"><i class="bi bi-star-fill"></i></label>
  <input type="radio" id="bi1" name="biRating" value="1">
  <label for="bi1"><i class="bi bi-star-fill"></i></label>
</div>
```

```html
<!-- Half-star rating with Bootstrap form integration -->
<form>
  <fieldset class="mb-3">
    <legend class="col-form-label">Rate this product</legend>
    <div class="star-rating">
      <input type="radio" id="half5" name="halfRating" value="5" required>
      <label for="half5" title="5 stars">&#9733;</label>
      <input type="radio" id="half4" name="halfRating" value="4">
      <label for="half4" title="4 stars">&#9733;</label>
      <input type="radio" id="half3" name="halfRating" value="3">
      <label for="half3" title="3 stars">&#9733;</label>
      <input type="radio" id="half2" name="halfRating" value="2">
      <label for="half2" title="2 stars">&#9733;</label>
      <input type="radio" id="half1" name="halfRating" value="1">
      <label for="half1" title="1 star">&#9733;</label>
    </div>
    <div class="form-text">Select a rating from 1 to 5 stars.</div>
  </fieldset>
  <button type="submit" class="btn btn-primary">Submit Review</button>
</form>
```

## Best Practices

1. Use hidden radio inputs so the rating is semantically a form field.
2. Always include `name` attribute on radios so only one star can be selected.
3. Use `flex-direction: row-reverse` with sibling selectors for the CSS hover effect.
4. Provide `title` attributes on labels for tooltip text on hover.
5. Include a visible label or legend describing what the rating represents.
6. Use `required` on one radio input for form validation integration.
7. Ensure each radio input has a unique `id` matching its label's `for`.
8. Use Bootstrap's `text-warning` color class for filled stars.
9. For read-only displays, use static HTML without radio inputs.
10. Test keyboard navigation to confirm arrow keys cycle through star options.
11. Provide a "clear" or "no rating" option when zero stars is valid.

## Common Pitfalls

1. **Using divs instead of radio inputs.** This breaks form submission and keyboard navigation.
2. **Missing `name` attribute.** Without a shared name, users can select multiple stars simultaneously.
3. **Broken CSS sibling selectors.** Incorrect HTML order breaks the hover highlight effect.
4. **No keyboard support.** If radios are hidden with `display: none`, keyboard users cannot navigate; use `position: absolute; opacity: 0` instead.
5. **Hardcoded colors.** Using hex values instead of Bootstrap utilities conflicts with dark mode.
6. **Unclear read-only states.** Interactive-looking stars that are actually disabled confuse users.

## Accessibility Considerations

Each star must be backed by a radio input with a descriptive label so screen readers announce "1 star", "2 stars", etc. Use `<fieldset>` and `<legend>` to group and describe the rating control. Ensure keyboard users can navigate between stars with arrow keys by keeping radio inputs focusable (use `position: absolute; opacity: 0` rather than `display: none`). Provide `aria-label` on the fieldset if a visible legend is not used. Announce the selected rating with `aria-live` for dynamic feedback.

## Responsive Behavior

Star ratings scale with font-size utilities (`fs-5`, `fs-3`). On mobile, increase star size to at least 44px touch targets. Use `d-inline-flex` to prevent star wrapping on narrow screens. In card layouts, place ratings on their own line with `d-block` or `w-100`. For forms, use Bootstrap's grid to position ratings alongside labels responsively with `col-12 col-md-6`.
