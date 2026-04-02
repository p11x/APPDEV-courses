---
title: "Order Tracking"
module: "E-Commerce"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["04_01_Card_Component", "04_04_Table", "04_09_Badges"]
---

## Overview

Order tracking pages keep customers informed after purchase. They display order status, timeline milestones, shipping details, and tracking information. Bootstrap 5 provides timeline patterns with list groups, badges for status indicators, tables for order details, and cards for organizing tracking information into scannable sections.

## Basic Implementation

### Order Timeline with List Group

```html
<div class="container py-5">
  <div class="row justify-content-center">
    <div class="col-lg-8">
      <div class="card">
        <div class="card-header bg-white d-flex justify-content-between align-items-center">
          <div>
            <h5 class="mb-0">Order #ORD-2024-7842</h5>
            <small class="text-muted">Placed on March 15, 2024</small>
          </div>
          <span class="badge bg-warning text-dark fs-6">In Transit</span>
        </div>
        <div class="card-body">
          <!-- Status Timeline -->
          <div class="position-relative mb-5">
            <div class="progress" style="height:4px">
              <div class="progress-bar bg-success" style="width:75%"></div>
            </div>
            <div class="d-flex justify-content-between position-relative" style="margin-top:-14px">
              <div class="text-center">
                <div class="rounded-circle bg-success text-white d-inline-flex align-items-center justify-content-center" style="width:32px;height:32px">
                  <i class="bi bi-check"></i>
                </div>
                <div class="small mt-2 fw-semibold">Confirmed</div>
                <div class="small text-muted">Mar 15</div>
              </div>
              <div class="text-center">
                <div class="rounded-circle bg-success text-white d-inline-flex align-items-center justify-content-center" style="width:32px;height:32px">
                  <i class="bi bi-check"></i>
                </div>
                <div class="small mt-2 fw-semibold">Shipped</div>
                <div class="small text-muted">Mar 16</div>
              </div>
              <div class="text-center">
                <div class="rounded-circle bg-primary text-white d-inline-flex align-items-center justify-content-center" style="width:32px;height:32px">
                  <i class="bi bi-truck"></i>
                </div>
                <div class="small mt-2 fw-semibold">In Transit</div>
                <div class="small text-muted">Mar 18</div>
              </div>
              <div class="text-center">
                <div class="rounded-circle bg-light border d-inline-flex align-items-center justify-content-center" style="width:32px;height:32px">
                  <i class="bi bi-house text-muted"></i>
                </div>
                <div class="small mt-2 text-muted">Delivered</div>
                <div class="small text-muted">Est. Mar 20</div>
              </div>
            </div>
          </div>

          <!-- Detailed Timeline -->
          <h6 class="mb-3">Tracking History</h6>
          <ul class="list-group list-group-flush">
            <li class="list-group-item ps-5 position-relative">
              <div class="position-absolute start-0 top-50 translate-middle-y bg-success rounded-circle" style="width:12px;height:12px;left:12px!important"></div>
              <div class="small text-muted">Mar 18, 2024 - 2:30 PM</div>
              <div>Package arrived at local distribution center - San Francisco, CA</div>
            </li>
            <li class="list-group-item ps-5 position-relative">
              <div class="position-absolute start-0 top-50 translate-middle-y bg-success rounded-circle" style="width:12px;height:12px;left:12px!important"></div>
              <div class="small text-muted">Mar 17, 2024 - 8:15 AM</div>
              <div>In transit to destination</div>
            </li>
            <li class="list-group-item ps-5 position-relative">
              <div class="position-absolute start-0 top-50 translate-middle-y bg-success rounded-circle" style="width:12px;height:12px;left:12px!important"></div>
              <div class="small text-muted">Mar 16, 2024 - 4:00 PM</div>
              <div>Shipped from warehouse - Los Angeles, CA</div>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Order Details Table

```html
<div class="card mt-4">
  <div class="card-header bg-white">
    <h6 class="mb-0">Order Items</h6>
  </div>
  <div class="card-body p-0">
    <div class="table-responsive">
      <table class="table align-middle mb-0">
        <tbody>
          <tr>
            <td style="width:80px">
              <img src="product.jpg" alt="Product" class="rounded" width="60" height="60" style="object-fit:cover">
            </td>
            <td>
              <h6 class="mb-0">Wireless Headphones</h6>
              <small class="text-muted">Black / One Size</small>
            </td>
            <td class="text-center">Qty: 1</td>
            <td class="text-end fw-bold">$129.99</td>
          </tr>
        </tbody>
        <tfoot class="bg-light">
          <tr>
            <td colspan="3" class="text-end">Subtotal:</td>
            <td class="text-end fw-bold">$389.97</td>
          </tr>
          <tr>
            <td colspan="3" class="text-end">Shipping:</td>
            <td class="text-end text-success">Free</td>
          </tr>
          <tr>
            <td colspan="3" class="text-end">Tax:</td>
            <td class="text-end">$31.20</td>
          </tr>
          <tr>
            <td colspan="3" class="text-end fw-bold">Total:</td>
            <td class="text-end fw-bold fs-5">$421.17</td>
          </tr>
        </tfoot>
      </table>
    </div>
  </div>
</div>
```

### Shipping Address Card

```html
<div class="row g-4 mt-2">
  <div class="col-md-6">
    <div class="card h-100">
      <div class="card-body">
        <h6 class="card-title"><i class="bi bi-geo-alt me-2"></i>Shipping Address</h6>
        <p class="mb-0">John Doe<br>123 Main Street, Apt 4B<br>San Francisco, CA 94102</p>
      </div>
    </div>
  </div>
  <div class="col-md-6">
    <div class="card h-100">
      <div class="card-body">
        <h6 class="card-title"><i class="bi bi-truck me-2"></i>Shipping Method</h6>
        <p class="mb-1"><strong>Standard Shipping</strong></p>
        <p class="text-muted mb-0">Tracking: <a href="#">1Z999AA10123456784</a></p>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Show a visual progress bar with clear milestone labels
2. Display estimated delivery date prominently
3. Use color-coded badges for different order statuses
4. Provide a tracking number with a link to the carrier
5. Include both a visual timeline and a detailed event log
6. Show order items with thumbnails for easy identification
7. Use a responsive table for order details
8. Group shipping address and method in separate cards
9. Provide a "Need Help?" or "Contact Support" action
10. Show timestamps in the user's local timezone
11. Auto-refresh or provide a "Refresh Status" button

## Common Pitfalls

1. **No estimated delivery date** - Customers need a target date. Always show ETA.
2. **Timeline not accessible** - Screen readers can't follow visual-only progress. Use semantic markup with status text.
3. **Tracking number not clickable** - Deep-link to the carrier's tracking page.
4. **No order summary** - Users forget what they ordered. Include item details on the tracking page.
5. **Timestamps in server timezone** - Display times in the user's local timezone for clarity.
6. **Missing status for delivered orders** - Show a clear "Delivered" state with delivery date and photo if available.

## Accessibility Considerations

- Use `aria-label="Order status: In Transit"` on the status badge
- Provide text alternatives for the visual progress bar
- Use `role="list"` on timeline elements if list styling is removed
- Associate tracking number with a descriptive label
- Announce status updates with `aria-live="polite"` for auto-refresh
- Ensure timeline dots have sufficient color contrast (3:1 minimum)

## Responsive Behavior

On **mobile**, the progress milestones stack vertically or use a compact horizontal bar. The order details table becomes a stacked card layout with labeled rows. Shipping and address cards stack vertically. On **tablet and desktop**, milestones display horizontally with labels below. The table shows all columns. Cards sit side by side in a two-column layout.
