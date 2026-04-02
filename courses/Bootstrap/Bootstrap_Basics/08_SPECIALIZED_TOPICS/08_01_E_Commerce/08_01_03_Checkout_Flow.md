---
title: "Checkout Flow"
module: "E-Commerce"
difficulty: 3
estimated_time: "35 min"
prerequisites: ["04_05_Forms", "04_07_Modal", "08_01_02_Shopping_Cart_UI"]
---

## Overview

The checkout flow converts browsing into purchases. A well-designed checkout minimizes friction, builds trust, and guides users through shipping, payment, and confirmation steps. Bootstrap 5 provides stepper patterns using nav pills, form validation utilities, progress indicators, and modal dialogs to build multi-step checkout experiences that work across all devices.

## Basic Implementation

### Progress Stepper with Nav Pills

```html
<div class="container py-5">
  <div class="row justify-content-center mb-5">
    <div class="col-lg-8">
      <ul class="nav nav-pills nav-justified" role="tablist">
        <li class="nav-item" role="presentation">
          <button class="nav-link active rounded-pill" data-bs-toggle="pill"
                  data-bs-target="#shipping" type="button">
            <i class="bi bi-truck me-2"></i>Shipping
          </button>
        </li>
        <li class="nav-item" role="presentation">
          <button class="nav-link rounded-pill" data-bs-toggle="pill"
                  data-bs-target="#payment" type="button" disabled>
            <i class="bi bi-credit-card me-2"></i>Payment
          </button>
        </li>
        <li class="nav-item" role="presentation">
          <button class="nav-link rounded-pill" data-bs-toggle="pill"
                  data-bs-target="#review" type="button" disabled>
            <i class="bi bi-check-circle me-2"></i>Review
          </button>
        </li>
      </ul>
    </div>
  </div>

  <div class="row justify-content-center">
    <div class="col-lg-8">
      <div class="tab-content">
        <!-- Shipping Step -->
        <div class="tab-pane fade show active" id="shipping">
          <div class="card">
            <div class="card-body p-4">
              <h4 class="card-title mb-4">Shipping Information</h4>
              <form class="needs-validation" novalidate>
                <div class="row g-3">
                  <div class="col-sm-6">
                    <label for="firstName" class="form-label">First Name</label>
                    <input type="text" class="form-control" id="firstName" required>
                    <div class="invalid-feedback">First name is required.</div>
                  </div>
                  <div class="col-sm-6">
                    <label for="lastName" class="form-label">Last Name</label>
                    <input type="text" class="form-control" id="lastName" required>
                    <div class="invalid-feedback">Last name is required.</div>
                  </div>
                  <div class="col-12">
                    <label for="address" class="form-label">Address</label>
                    <input type="text" class="form-control" id="address" required>
                    <div class="invalid-feedback">Address is required.</div>
                  </div>
                  <div class="col-sm-5">
                    <label for="city" class="form-label">City</label>
                    <input type="text" class="form-control" id="city" required>
                  </div>
                  <div class="col-sm-4">
                    <label for="state" class="form-label">State</label>
                    <select class="form-select" id="state" required>
                      <option value="">Choose...</option>
                      <option>California</option>
                    </select>
                  </div>
                  <div class="col-sm-3">
                    <label for="zip" class="form-label">Zip</label>
                    <input type="text" class="form-control" id="zip" required>
                  </div>
                </div>
                <div class="mt-4 d-flex justify-content-between">
                  <a href="cart.html" class="btn btn-outline-secondary">Back to Cart</a>
                  <button type="submit" class="btn btn-primary">Continue to Payment</button>
                </div>
              </form>
            </div>
          </div>
        </div>

        <!-- Payment Step -->
        <div class="tab-pane fade" id="payment">
          <div class="card">
            <div class="card-body p-4">
              <h4 class="card-title mb-4">Payment Method</h4>
              <form>
                <div class="mb-3">
                  <label class="form-label">Card Number</label>
                  <div class="input-group">
                    <span class="input-group-text"><i class="bi bi-credit-card"></i></span>
                    <input type="text" class="form-control" placeholder="1234 5678 9012 3456" required>
                  </div>
                </div>
                <div class="row g-3">
                  <div class="col-sm-6">
                    <label class="form-label">Expiry Date</label>
                    <input type="text" class="form-control" placeholder="MM/YY" required>
                  </div>
                  <div class="col-sm-6">
                    <label class="form-label">CVV</label>
                    <input type="text" class="form-control" placeholder="123" required>
                  </div>
                </div>
                <div class="mt-4 d-flex justify-content-between">
                  <button class="btn btn-outline-secondary" data-bs-target="#shipping" data-bs-toggle="pill">Back</button>
                  <button type="submit" class="btn btn-primary">Review Order</button>
                </div>
              </form>
            </div>
          </div>
        </div>

        <!-- Review Step -->
        <div class="tab-pane fade" id="review">
          <div class="card">
            <div class="card-body p-4">
              <h4 class="card-title mb-4">Review & Place Order</h4>
              <div class="border rounded p-3 mb-3">
                <h6 class="mb-2">Shipping to:</h6>
                <p class="mb-0">John Doe, 123 Main St, San Francisco, CA 94102</p>
              </div>
              <div class="border rounded p-3 mb-4">
                <h6 class="mb-2">Payment:</h6>
                <p class="mb-0"><i class="bi bi-credit-card me-2"></i>**** **** **** 3456</p>
              </div>
              <button class="btn btn-success btn-lg w-100">Place Order - $421.17</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Order Summary Sidebar

```html
<div class="col-lg-4">
  <div class="card sticky-top" style="top:20px">
    <div class="card-header bg-white">
      <h5 class="mb-0">Order Summary</h5>
    </div>
    <div class="card-body">
      <div class="d-flex align-items-center mb-3">
        <img src="product.jpg" alt="" class="rounded" width="50" height="50" style="object-fit:cover">
        <div class="ms-3 flex-grow-1">
          <div class="small fw-semibold">Wireless Headphones</div>
          <div class="small text-muted">Qty: 1</div>
        </div>
        <span>$129.99</span>
      </div>
      <div class="input-group mb-3">
        <input type="text" class="form-control" placeholder="Promo code">
        <button class="btn btn-outline-secondary">Apply</button>
      </div>
      <hr>
      <div class="d-flex justify-content-between mb-1"><span>Subtotal</span><span>$389.97</span></div>
      <div class="d-flex justify-content-between mb-1"><span>Shipping</span><span class="text-success">Free</span></div>
      <div class="d-flex justify-content-between mb-1"><span>Tax</span><span>$31.20</span></div>
      <hr>
      <div class="d-flex justify-content-between"><span class="fw-bold">Total</span><span class="fw-bold fs-5">$421.17</span></div>
    </div>
  </div>
</div>
```

### Express Checkout Options

```html
<div class="mb-4">
  <h5 class="text-center text-muted mb-3">Express Checkout</h5>
  <div class="row g-2">
    <div class="col-4">
      <button class="btn btn-dark w-100 py-2">
        <i class="bi bi-apple me-1"></i>Pay
      </button>
    </div>
    <div class="col-4">
      <button class="btn btn-warning w-100 py-2">
        <i class="bi bi-google me-1"></i>Pay
      </button>
    </div>
    <div class="col-4">
      <button class="btn btn-primary w-100 py-2">
        <i class="bi bi-paypal me-1"></i>Pay
      </button>
    </div>
  </div>
  <div class="text-center my-3"><span class="text-muted small">or continue below</span></div>
</div>
```

## Best Practices

1. Use a progress stepper so users always know where they are in the flow
2. Validate forms on step completion, not on every keystroke
3. Keep the order summary visible on every step using a sticky sidebar
4. Provide a "Back" button on every step except the first
5. Auto-fill city/state from zip code when possible
6. Show security badges and SSL indicators near payment fields
7. Use `input-group` with icons to clarify field purpose
8. Mark required fields with an asterisk and a legend
9. Disable the "Continue" button until the form is valid
10. Save progress so users can resume if they leave
11. Offer guest checkout alongside account creation
12. Use clear, action-oriented button labels ("Continue to Payment" not just "Next")

## Common Pitfalls

1. **No progress indication** - Users abandon if they don't know how many steps remain. Always show a stepper.
2. **Forcing account creation** - Requiring signup before checkout increases abandonment. Offer guest checkout.
3. **No form validation feedback** - Silent failures frustrate users. Show inline error messages with `invalid-feedback`.
4. **Losing data between steps** - If a user navigates back, their data should persist. Store form state in JS or hidden fields.
5. **Too many steps** - Combining shipping and payment into fewer steps reduces friction. Consider a single-page checkout.
6. **No mobile optimization** - Payment forms with small inputs are hard to use on phones. Use appropriate input types (`tel`, `cc-number`).
7. **Missing order summary on payment step** - Users need to see what they're paying for at the moment of commitment.

## Accessibility Considerations

- Use `role="tablist"` and `role="tab"` on the stepper navigation
- Manage focus when transitioning between steps: move focus to the new step's heading
- Associate error messages with inputs using `aria-describedby`
- Use `aria-invalid="true"` on fields with validation errors
- Announce step transitions with an `aria-live` region
- Ensure all form fields have visible labels
- Provide `aria-label` on payment method buttons (e.g., "Pay with Apple Pay")

## Responsive Behavior

On **mobile**, stack the stepper vertically or use a compact progress bar. The order summary collapses into a collapsible accordion at the top. Form fields go full-width in a single column. On **tablet**, the stepper uses horizontal pills. The layout can shift to a two-column design with the form on the left and order summary on the right. On **desktop**, the full stepper, form, and sticky order summary display side by side with generous spacing.
