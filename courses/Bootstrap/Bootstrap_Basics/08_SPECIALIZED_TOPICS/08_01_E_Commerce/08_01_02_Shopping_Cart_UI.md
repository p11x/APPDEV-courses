---
title: "Shopping Cart UI"
module: "E-Commerce"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["02_01_Grid_System", "04_01_Card_Component", "04_03_Offcanvas"]
---

## Overview

The shopping cart is a critical conversion point in e-commerce. Bootstrap 5 provides offcanvas, modal, and card components to build cart sidebars, mini carts, and full cart pages. This module covers quantity controls, cart summaries, remove actions, and responsive cart layouts that guide users smoothly toward checkout.

## Basic Implementation

### Cart Sidebar with Offcanvas

```html
<div class="offcanvas offcanvas-end" tabindex="-1" id="cartSidebar">
  <div class="offcanvas-header border-bottom">
    <h5 class="offcanvas-title">Shopping Cart (3)</h5>
    <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body d-flex flex-column p-0">
    <!-- Cart Items -->
    <div class="flex-grow-1 overflow-auto p-3">
      <div class="d-flex mb-3 pb-3 border-bottom">
        <img src="product-thumb.jpg" alt="Product" class="rounded" width="80" height="80" style="object-fit:cover">
        <div class="ms-3 flex-grow-1">
          <h6 class="mb-1">Wireless Headphones</h6>
          <small class="text-muted d-block mb-2">Black / Large</small>
          <div class="d-flex align-items-center">
            <div class="input-group input-group-sm" style="width:110px">
              <button class="btn btn-outline-secondary" type="button">-</button>
              <input type="text" class="form-control text-center" value="1">
              <button class="btn btn-outline-secondary" type="button">+</button>
            </div>
            <span class="ms-auto fw-bold">$129.99</span>
          </div>
        </div>
        <button class="btn btn-link text-danger p-0 ms-2 align-self-start">
          <i class="bi bi-x-lg"></i>
        </button>
      </div>
    </div>
    <!-- Cart Summary -->
    <div class="border-top p-3 bg-light">
      <div class="d-flex justify-content-between mb-2">
        <span>Subtotal</span>
        <span class="fw-bold">$389.97</span>
      </div>
      <div class="d-flex justify-content-between mb-3">
        <span>Shipping</span>
        <span class="text-success">Free</span>
      </div>
      <a href="checkout.html" class="btn btn-primary w-100 mb-2">Proceed to Checkout</a>
      <a href="cart.html" class="btn btn-outline-secondary w-100">View Full Cart</a>
    </div>
  </div>
</div>
```

### Cart Page Layout

```html
<div class="container py-5">
  <h2 class="mb-4">Shopping Cart</h2>
  <div class="row g-4">
    <div class="col-lg-8">
      <div class="card">
        <div class="card-body">
          <div class="table-responsive">
            <table class="table align-middle mb-0">
              <thead>
                <tr>
                  <th>Product</th>
                  <th>Price</th>
                  <th>Quantity</th>
                  <th>Total</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>
                    <div class="d-flex align-items-center">
                      <img src="product.jpg" alt="Product" class="rounded" width="60" style="object-fit:cover">
                      <div class="ms-3">
                        <h6 class="mb-0">Wireless Headphones</h6>
                        <small class="text-muted">Black</small>
                      </div>
                    </div>
                  </td>
                  <td>$129.99</td>
                  <td>
                    <div class="input-group input-group-sm" style="width:100px">
                      <button class="btn btn-outline-secondary">-</button>
                      <input type="text" class="form-control text-center" value="1">
                      <button class="btn btn-outline-secondary">+</button>
                    </div>
                  </td>
                  <td class="fw-bold">$129.99</td>
                  <td><button class="btn btn-link text-danger p-0"><i class="bi bi-trash"></i></button></td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
    <div class="col-lg-4">
      <div class="card">
        <div class="card-body">
          <h5 class="card-title mb-4">Order Summary</h5>
          <div class="d-flex justify-content-between mb-2"><span>Subtotal</span><span>$389.97</span></div>
          <div class="d-flex justify-content-between mb-2"><span>Shipping</span><span class="text-success">Free</span></div>
          <div class="d-flex justify-content-between mb-2"><span>Tax</span><span>$31.20</span></div>
          <hr>
          <div class="d-flex justify-content-between mb-4"><span class="fw-bold">Total</span><span class="fw-bold fs-5">$421.17</span></div>
          <button class="btn btn-primary w-100">Checkout</button>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Mini Cart with Badge Counter

```html
<div class="dropdown">
  <button class="btn btn-outline-dark position-relative" data-bs-toggle="dropdown">
    <i class="bi bi-cart3"></i>
    <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
      3
      <span class="visually-hidden">items in cart</span>
    </span>
  </button>
  <div class="dropdown-menu dropdown-menu-end p-3" style="width:320px">
    <div class="mb-3">
      <div class="d-flex align-items-center mb-2">
        <img src="thumb.jpg" alt="" class="rounded" width="50" height="50" style="object-fit:cover">
        <div class="ms-2 flex-grow-1">
          <div class="small fw-semibold">Product Name</div>
          <div class="small text-muted">Qty: 1 &times; $129.99</div>
        </div>
        <button class="btn btn-sm btn-link text-danger p-0"><i class="bi bi-x"></i></button>
      </div>
    </div>
    <hr>
    <div class="d-flex justify-content-between mb-3">
      <strong>Total:</strong>
      <strong>$129.99</strong>
    </div>
    <a href="cart.html" class="btn btn-primary btn-sm w-100">View Cart</a>
  </div>
</div>
```

### Cart Modal for Quick View

```html
<div class="modal fade" id="cartModal" tabindex="-1">
  <div class="modal-dialog modal-lg">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Added to Cart</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>
      <div class="modal-body">
        <div class="d-flex align-items-center">
          <img src="product.jpg" alt="Product" class="rounded" width="120" style="object-fit:cover">
          <div class="ms-4">
            <h5>Premium Headphones</h5>
            <p class="text-success mb-2"><i class="bi bi-check-circle me-1"></i>Added to your cart</p>
            <p class="mb-0">Cart total: <strong>$389.97</strong> (3 items)</p>
          </div>
        </div>
      </div>
      <div class="modal-footer">
        <button class="btn btn-outline-secondary" data-bs-dismiss="modal">Continue Shopping</button>
        <a href="checkout.html" class="btn btn-primary">Go to Checkout</a>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Always display item count badge on cart icon for at-a-glance feedback
2. Use offcanvas for side carts to avoid losing browsing context
3. Provide quantity increment/decrement buttons instead of free-text input
4. Show remove confirmation or undo toast before deleting items
5. Display subtotal, shipping, tax, and total clearly separated
6. Use `table-responsive` wrapper for cart tables on small screens
7. Include product thumbnail, name, variant, and unit price in each row
8. Make the checkout button the most prominent action with `btn-primary`
9. Allow "Continue Shopping" link back to the catalog
10. Save cart state to localStorage or server for persistence
11. Show free shipping threshold progress to encourage larger orders
12. Use `align-middle` on table rows for vertical centering

## Common Pitfalls

1. **No mobile cart experience** - A full table layout breaks on phones. Use stacked layouts or offcanvas on mobile.
2. **Quantity allows zero or negative values** - Clamp quantity to a minimum of 1 and validate on input.
3. **Missing remove feedback** - Deleting items without confirmation causes user frustration. Use a toast with undo.
4. **Cart not updating totals dynamically** - Static totals after quantity changes mislead users. Recalculate on every change.
5. **Ignoring keyboard accessibility** - Quantity buttons and remove actions must be keyboard-operable.
6. **No empty cart state** - Display a friendly message and "Continue Shopping" button when the cart is empty.
7. **Slow cart sidebar** - Loading cart items on open causes lag. Pre-fetch or use skeleton loaders.

## Accessibility Considerations

- Use `aria-label="Shopping cart, 3 items"` on the cart button
- Add `aria-live="polite"` to the cart total so updates are announced
- Ensure quantity inputs have associated labels or `aria-label="Quantity"`
- Use `role="dialog"` and proper `aria-labelledby` on cart modals
- Provide focus management: trap focus inside offcanvas/modal and return focus on close
- Announce item removal with an `aria-live` region
- Ensure all interactive elements have visible focus indicators

## Responsive Behavior

On **mobile** (<768px), use an offcanvas cart that slides from the right. Stack cart item details vertically instead of table rows. On **tablets** (768px+), a two-column layout works: cart items on the left, summary on the right. On **desktop** (992px+), use a full table layout or a dedicated cart page with side-by-side item list and order summary. The mini cart dropdown works across all breakpoints but should increase in width on larger screens to accommodate more items.
