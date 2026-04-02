---
title: "Coupon Code Input"
description: "Build promo code input groups with discount display, validation feedback, and applied coupon management using Bootstrap 5."
difficulty: 1
estimated_time: "20 minutes"
prerequisites:
  - "Bootstrap 5 Input Group"
  - "Bootstrap 5 Alerts"
  - "Bootstrap 5 Badges"
---

## Overview

Coupon code inputs are a standard e-commerce checkout component that allows customers to apply promotional codes for discounts. Bootstrap 5's input group component provides the foundation for combining a text input with an apply button. The component handles validation states, applied discount display, and coupon removal with clear visual feedback.

Effective coupon UI reduces cart abandonment by making discounts easy to apply. The component should provide immediate feedback on code validity, display the discount clearly, and allow removal without friction.

## Basic Implementation

### Basic Coupon Input Group

```html
<div class="input-group mb-3">
  <input type="text" class="form-control" placeholder="Enter coupon code" aria-label="Coupon code" id="couponInput">
  <button class="btn btn-outline-primary" type="button" id="applyCoupon">Apply</button>
</div>
```

### Applied Coupon Display

```html
<div class="alert alert-success d-flex justify-content-between align-items-center" role="alert">
  <div>
    <i class="bi bi-check-circle-fill me-1"></i>
    <strong>SUMMER20</strong> applied - 20% off your order
  </div>
  <button type="button" class="btn-close" aria-label="Remove coupon"></button>
</div>
```

### Coupon with Validation States

```html
<div class="mb-3">
  <label for="couponValid" class="form-label">Promo Code</label>
  <div class="input-group has-validation">
    <input type="text" class="form-control is-valid" id="couponValid" value="SAVE10" aria-describedby="couponFeedback">
    <button class="btn btn-success" type="button">Applied</button>
    <div class="valid-feedback" id="couponFeedback">Code applied! You save $10.00.</div>
  </div>
</div>

<div class="mb-3">
  <label for="couponInvalid" class="form-label">Promo Code</label>
  <div class="input-group has-validation">
    <input type="text" class="form-control is-invalid" id="couponInvalid" value="EXPIRED2024">
    <button class="btn btn-outline-primary" type="button">Apply</button>
    <div class="invalid-feedback">This coupon code has expired.</div>
  </div>
</div>
```

## Advanced Variations

### Coupon with Discount Breakdown

```html
<div class="card bg-light">
  <div class="card-body">
    <h6 class="card-title">Applied Coupons</h6>
    <div class="d-flex justify-content-between align-items-center mb-2">
      <span>
        <span class="badge bg-success me-1">SUMMER20</span>
        20% off items
      </span>
      <div>
        <span class="text-success">-$24.00</span>
        <button class="btn btn-sm btn-link text-danger p-0 ms-2" aria-label="Remove SUMMER20">
          <i class="bi bi-x-lg"></i>
        </button>
      </div>
    </div>
    <div class="d-flex justify-content-between align-items-center mb-2">
      <span>
        <span class="badge bg-info me-1">FREESHIP</span>
        Free shipping
      </span>
      <div>
        <span class="text-success">-$5.99</span>
        <button class="btn btn-sm btn-link text-danger p-0 ms-2" aria-label="Remove FREESHIP">
          <i class="bi bi-x-lg"></i>
        </button>
      </div>
    </div>
    <hr>
    <div class="d-flex justify-content-between">
      <strong>Total Savings</strong>
      <strong class="text-success">-$29.99</strong>
    </div>
  </div>
</div>
```

### Stacked Coupon Input with Loading State

```html
<div class="input-group">
  <span class="input-group-text"><i class="bi bi-ticket-perforated"></i></span>
  <input type="text" class="form-control" placeholder="Enter promo code" id="promoInput" disabled>
  <button class="btn btn-primary" type="button" id="applyBtn" disabled>
    <span class="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true"></span>
    Applying...
  </button>
</div>
<div class="form-text">Only one coupon code can be applied per order.</div>
```

### Auto-Apply Suggested Coupons

```html
<div class="mb-3">
  <label class="form-label">Available Coupons</label>
  <div class="d-flex flex-wrap gap-2">
    <button class="btn btn-outline-success btn-sm" data-coupon="WELCOME15">
      <i class="bi bi-tag me-1"></i> WELCOME15 - 15% off
    </button>
    <button class="btn btn-outline-success btn-sm" data-coupon="FREESHIP">
      <i class="bi bi-truck me-1"></i> FREESHIP - Free shipping
    </button>
    <button class="btn btn-outline-success btn-sm" data-coupon="SAVE10">
      <i class="bi bi-currency-dollar me-1"></i> SAVE10 - $10 off
    </button>
  </div>
  <small class="text-muted">Click a coupon to apply it automatically.</small>
</div>
```

### Checkout Summary with Coupon

```html
<div class="card">
  <div class="card-body">
    <h6 class="card-title">Order Summary</h6>
    <div class="d-flex justify-content-between mb-1">
      <span>Subtotal</span><span>$120.00</span>
    </div>
    <div class="d-flex justify-content-between mb-1 text-success">
      <span>SUMMER20 discount</span><span>-$24.00</span>
    </div>
    <div class="d-flex justify-content-between mb-1">
      <span>Shipping</span><span>$5.99</span>
    </div>
    <hr>
    <div class="d-flex justify-content-between fw-bold">
      <span>Total</span><span>$101.99</span>
    </div>
    <div class="input-group input-group-sm mt-3">
      <input type="text" class="form-control" placeholder="Add another code">
      <button class="btn btn-outline-secondary" type="button">Apply</button>
    </div>
  </div>
</div>
```

## Best Practices

1. Use `input-group` to pair the input with the apply button seamlessly
2. Provide clear success and error states using `is-valid` and `is-invalid` classes
3. Show the discount amount immediately after a successful coupon application
4. Allow easy coupon removal with a visible close or remove button
5. Use `has-validation` wrapper to properly position validation feedback
6. Disable the input and show a loading spinner during coupon validation
7. Display the coupon code in the order summary for customer confirmation
8. Support case-insensitive coupon codes by normalizing input on the server
9. Use badges to visually distinguish applied coupon codes from regular text
10. Provide suggested coupons as clickable buttons to reduce typing
11. Limit to one or a clearly stated maximum number of stackable coupons
12. Auto-uppercase coupon input using CSS `text-transform: uppercase`
13. Include a descriptive `form-label` for accessibility alongside the visual placeholder

## Common Pitfalls

1. **No feedback on invalid codes**: Silently failing when a code is wrong frustrates users. Always display a clear error message with `invalid-feedback`.
2. **Missing loading state**: Without a spinner during API validation, users may click Apply multiple times causing duplicate requests.
3. **Case-sensitive code matching**: Requiring exact uppercase frustrates users who type lowercase. Normalize on the server side.
4. **No coupon removal option**: Applied coupons without a remove button force users to reload the page or contact support.
5. **Coupons not reflected in total**: Applying a coupon without updating the order total creates confusion and erodes trust.
6. **Allowing expired codes to be entered without warning**: Validate expiry dates and show specific "expired" messaging rather than generic errors.
7. **No placeholder or label text**: An empty input with no context confuses users about its purpose. Always include descriptive placeholder text.

## Accessibility Considerations

- Associate the input with its label using `for` and `id` attributes
- Use `aria-describedby` to link validation feedback messages to the input
- Apply `role="alert"` on error/success messages so screen readers announce them
- Ensure the remove coupon button has `aria-label` describing which coupon it removes
- Use `aria-live="polite"` on the order total so updates are announced
- Disable the apply button with `disabled` attribute during validation to prevent double submission
- Provide text-based discount information alongside icon-only visual indicators

## Responsive Behavior

On mobile devices, the coupon input group should stack vertically if the input and button feel cramped. Use `flex-column flex-sm-row` on the input group for better mobile ergonomics. Applied coupon alerts should use `flex-wrap` to handle long coupon names. The checkout summary card should remain full-width on all breakpoints. Suggested coupon buttons should wrap naturally using `d-flex flex-wrap gap-2`.
