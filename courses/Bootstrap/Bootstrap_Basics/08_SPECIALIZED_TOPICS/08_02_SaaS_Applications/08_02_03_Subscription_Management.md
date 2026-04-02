---
title: "Subscription Management"
module: "SaaS Applications"
difficulty: 3
estimated_time: "35 min"
prerequisites: ["04_01_Card_Component", "04_04_Table", "04_07_Modal"]
---

## Overview

Subscription management UI lets users view, upgrade, downgrade, and manage their billing. It includes plan cards, billing history tables, payment method management, and confirmation modals for plan changes. Bootstrap 5 cards, tables, modals, and form components provide the building blocks for a complete subscription management experience.

## Basic Implementation

### Current Plan Display

```html
<div class="card border-primary mb-4">
  <div class="card-body">
    <div class="d-flex justify-content-between align-items-start flex-wrap gap-3">
      <div>
        <span class="badge bg-primary mb-2">Current Plan</span>
        <h3 class="mb-1">Pro Plan</h3>
        <p class="text-muted mb-0">$29/month &bull; Renews on April 15, 2024</p>
      </div>
      <div class="text-end">
        <a href="#" class="btn btn-outline-primary btn-sm me-2">Change Plan</a>
        <a href="#" class="btn btn-outline-danger btn-sm">Cancel Subscription</a>
      </div>
    </div>
  </div>
  <div class="card-footer bg-transparent">
    <div class="row text-center">
      <div class="col-4">
        <strong>Unlimited</strong>
        <div class="small text-muted">Projects</div>
      </div>
      <div class="col-4">
        <strong>15</strong>
        <div class="small text-muted">Team Members</div>
      </div>
      <div class="col-4">
        <strong>50GB</strong>
        <div class="small text-muted">Storage Used</div>
      </div>
    </div>
    <div class="progress mt-3" style="height:8px">
      <div class="progress-bar bg-primary" style="width:65%"></div>
    </div>
    <small class="text-muted">32.5GB of 50GB used</small>
  </div>
</div>
```

### Upgrade/Downgrade Plan Cards

```html
<div class="row g-4 mb-5">
  <div class="col-md-4">
    <div class="card h-100">
      <div class="card-body text-center p-4">
        <h5 class="card-title">Starter</h5>
        <div class="my-3">
          <span class="display-6 fw-bold">$9</span>
          <span class="text-muted">/mo</span>
        </div>
        <ul class="list-unstyled text-start small mb-4">
          <li class="mb-1"><i class="bi bi-check text-success me-2"></i>5 Projects</li>
          <li class="mb-1"><i class="bi bi-check text-success me-2"></i>3 Team Members</li>
          <li class="mb-1"><i class="bi bi-check text-success me-2"></i>5GB Storage</li>
        </ul>
        <button class="btn btn-outline-secondary w-100" data-bs-toggle="modal" data-bs-target="#downgradeModal">
          Downgrade
        </button>
      </div>
    </div>
  </div>
  <div class="col-md-4">
    <div class="card h-100 border-primary">
      <div class="card-header bg-primary text-white text-center">Current Plan</div>
      <div class="card-body text-center p-4">
        <h5 class="card-title">Pro</h5>
        <div class="my-3">
          <span class="display-6 fw-bold">$29</span>
          <span class="text-muted">/mo</span>
        </div>
        <ul class="list-unstyled text-start small mb-4">
          <li class="mb-1"><i class="bi bi-check text-success me-2"></i>Unlimited Projects</li>
          <li class="mb-1"><i class="bi bi-check text-success me-2"></i>15 Team Members</li>
          <li class="mb-1"><i class="bi bi-check text-success me-2"></i>50GB Storage</li>
        </ul>
        <button class="btn btn-secondary w-100" disabled>Current Plan</button>
      </div>
    </div>
  </div>
  <div class="col-md-4">
    <div class="card h-100">
      <div class="card-body text-center p-4">
        <h5 class="card-title">Enterprise</h5>
        <div class="my-3">
          <span class="display-6 fw-bold">$99</span>
          <span class="text-muted">/mo</span>
        </div>
        <ul class="list-unstyled text-start small mb-4">
          <li class="mb-1"><i class="bi bi-check text-success me-2"></i>Unlimited Everything</li>
          <li class="mb-1"><i class="bi bi-check text-success me-2"></i>Priority Support</li>
          <li class="mb-1"><i class="bi bi-check text-success me-2"></i>SSO / SAML</li>
        </ul>
        <button class="btn btn-primary w-100" data-bs-toggle="modal" data-bs-target="#upgradeModal">
          Upgrade
        </button>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Billing History Table

```html
<div class="card">
  <div class="card-header bg-white d-flex justify-content-between align-items-center">
    <h5 class="mb-0">Billing History</h5>
    <button class="btn btn-outline-secondary btn-sm">
      <i class="bi bi-download me-1"></i>Export All
    </button>
  </div>
  <div class="table-responsive">
    <table class="table table-hover align-middle mb-0">
      <thead class="table-light">
        <tr>
          <th>Date</th>
          <th>Description</th>
          <th>Amount</th>
          <th>Status</th>
          <th>Invoice</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td>Mar 15, 2024</td>
          <td>Pro Plan - Monthly</td>
          <td>$29.00</td>
          <td><span class="badge bg-success">Paid</span></td>
          <td><a href="#" class="btn btn-sm btn-outline-secondary"><i class="bi bi-file-pdf me-1"></i>PDF</a></td>
        </tr>
        <tr>
          <td>Feb 15, 2024</td>
          <td>Pro Plan - Monthly</td>
          <td>$29.00</td>
          <td><span class="badge bg-success">Paid</span></td>
          <td><a href="#" class="btn btn-sm btn-outline-secondary"><i class="bi bi-file-pdf me-1"></i>PDF</a></td>
        </tr>
        <tr>
          <td>Jan 15, 2024</td>
          <td>Pro Plan - Monthly</td>
          <td>$29.00</td>
          <td><span class="badge bg-warning text-dark">Pending</span></td>
          <td><span class="text-muted">-</span></td>
        </tr>
      </tbody>
    </table>
  </div>
</div>
```

### Payment Methods Management

```html
<div class="card mt-4">
  <div class="card-header bg-white d-flex justify-content-between align-items-center">
    <h5 class="mb-0">Payment Methods</h5>
    <button class="btn btn-primary btn-sm" data-bs-toggle="modal" data-bs-target="#addCardModal">
      <i class="bi bi-plus me-1"></i>Add Card
    </button>
  </div>
  <div class="card-body">
    <div class="d-flex align-items-center justify-content-between border rounded p-3 mb-3">
      <div class="d-flex align-items-center">
        <i class="bi bi-credit-card-2-front fs-3 text-primary me-3"></i>
        <div>
          <div class="fw-semibold">Visa ending in 4242</div>
          <small class="text-muted">Expires 12/2025</small>
        </div>
      </div>
      <div class="d-flex align-items-center gap-2">
        <span class="badge bg-primary">Default</span>
        <button class="btn btn-sm btn-outline-secondary"><i class="bi bi-pencil"></i></button>
        <button class="btn btn-sm btn-outline-danger"><i class="bi bi-trash"></i></button>
      </div>
    </div>
    <div class="d-flex align-items-center justify-content-between border rounded p-3">
      <div class="d-flex align-items-center">
        <i class="bi bi-credit-card-2-front fs-3 text-secondary me-3"></i>
        <div>
          <div class="fw-semibold">Mastercard ending in 8888</div>
          <small class="text-muted">Expires 08/2026</small>
        </div>
      </div>
      <div class="d-flex align-items-center gap-2">
        <button class="btn btn-sm btn-outline-primary">Set Default</button>
        <button class="btn btn-sm btn-outline-danger"><i class="bi bi-trash"></i></button>
      </div>
    </div>
  </div>
</div>
```

### Confirm Upgrade Modal

```html
<div class="modal fade" id="upgradeModal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Upgrade to Enterprise</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>
      <div class="modal-body">
        <div class="alert alert-info">
          <strong>Prorated charge:</strong> You'll be charged $58.33 today for the remaining 18 days of this billing cycle.
        </div>
        <p>Starting next billing cycle, you'll be charged <strong>$99/month</strong>.</p>
        <h6>New features you'll unlock:</h6>
        <ul>
          <li>Unlimited team members</li>
          <li>Priority support</li>
          <li>SSO / SAML authentication</li>
          <li>Dedicated account manager</li>
        </ul>
      </div>
      <div class="modal-footer">
        <button class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button class="btn btn-primary">Confirm Upgrade</button>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Clearly highlight the current plan with a "Current Plan" badge
2. Disable the current plan button to prevent redundant actions
3. Show prorated charges in upgrade confirmation modals
4. Provide downloadable PDF invoices in billing history
5. Display storage/usage limits with progress bars
6. Allow adding multiple payment methods with a default designation
7. Include a cancellation flow with retention offers
8. Show renewal dates prominently on the current plan card
9. Use modal dialogs for destructive actions (cancel, downgrade)
10. Provide plan comparison so users understand what they gain/lose

## Common Pitfalls

1. **No proration explanation** - Users are confused by unexpected charges. Always explain prorated billing.
2. **Missing cancellation flow** - Direct cancellation loses customers. Offer alternatives (pause, downgrade).
3. **No billing history** - Users need invoices for accounting. Always provide downloadable records.
4. **Payment method errors not handled** - Failed payments need clear error messages and retry options.
5. **Confusing upgrade/downgrade** - Clearly show what changes in features and pricing.
6. **No confirmation for destructive actions** - Always require confirmation before canceling or downgrading.

## Accessibility Considerations

- Use `aria-label="Upgrade to Enterprise plan, $99 per month"` on plan buttons
- Mark the current plan with `aria-current="true"`
- Provide `role="dialog"` and proper labeling on confirmation modals
- Use `aria-describedby` to link prorated charge explanations to the confirm button
- Ensure all table headers use `<th>` elements for screen reader navigation

## Responsive Behavior

On **mobile**, plan cards stack vertically. The billing history table uses `table-responsive`. Payment method cards stack with action buttons below the card details. On **tablet**, plan cards can display in a 2-column layout. On **desktop**, all 3 plan cards display side by side with the current plan visually emphasized.
