---
title: "Delivery Estimate"
description: "Build shipping calculators with delivery date displays, location selectors, and shipping option cards using Bootstrap 5."
difficulty: 2
estimated_time: "30 minutes"
prerequisites:
  - "Bootstrap 5 Forms"
  - "Bootstrap 5 Cards"
  - "Bootstrap 5 Accordion"
---

## Overview

Delivery estimate components inform customers about shipping costs and expected arrival dates before checkout. Bootstrap 5 provides form controls for postal code input, card layouts for shipping options, and accordion components for collapsible delivery details. A strong delivery estimate reduces cart abandonment by setting clear expectations.

The component typically includes a location input, shipping method selection, calculated costs, and estimated delivery dates. Real-time calculation via API calls provides accurate estimates based on the customer's location and selected shipping speed.

## Basic Implementation

### Postal Code Delivery Check

```html
<div class="card mb-3">
  <div class="card-body">
    <h6 class="card-title"><i class="bi bi-truck me-1"></i> Delivery Estimate</h6>
    <div class="input-group">
      <span class="input-group-text"><i class="bi bi-geo-alt"></i></span>
      <input type="text" class="form-control" placeholder="Enter ZIP / Postal code" id="zipInput" maxlength="10">
      <button class="btn btn-primary" type="button" id="checkDelivery">Check</button>
    </div>
    <div class="mt-3 d-none" id="deliveryResult">
      <div class="d-flex align-items-center text-success">
        <i class="bi bi-check-circle-fill me-2"></i>
        <span>Delivery available to <strong id="locationName">New York, NY 10001</strong></span>
      </div>
    </div>
  </div>
</div>
```

### Shipping Options Radio Group

```html
<div class="list-group">
  <label class="list-group-item d-flex justify-content-between align-items-center">
    <div class="d-flex align-items-center">
      <input class="form-check-input me-3" type="radio" name="shipping" id="standard" checked>
      <div>
        <strong>Standard Shipping</strong>
        <div class="text-muted small">Delivery in 5-7 business days</div>
      </div>
    </div>
    <span class="fw-bold">$5.99</span>
  </label>
  <label class="list-group-item d-flex justify-content-between align-items-center">
    <div class="d-flex align-items-center">
      <input class="form-check-input me-3" type="radio" name="shipping" id="express">
      <div>
        <strong>Express Shipping</strong>
        <div class="text-muted small">Delivery in 2-3 business days</div>
      </div>
    </div>
    <span class="fw-bold">$12.99</span>
  </label>
  <label class="list-group-item d-flex justify-content-between align-items-center">
    <div class="d-flex align-items-center">
      <input class="form-check-input me-3" type="radio" name="shipping" id="overnight">
      <div>
        <strong>Overnight Shipping</strong>
        <div class="text-muted small">Delivery by tomorrow</div>
      </div>
    </div>
    <span class="fw-bold">$24.99</span>
  </label>
</div>
```

### Delivery Date Display

```html
<div class="alert alert-info d-flex align-items-center" role="alert">
  <i class="bi bi-calendar-event me-2 fs-4"></i>
  <div>
    <strong>Estimated Delivery</strong>
    <div>Wednesday, April 9 - Friday, April 11, 2026</div>
  </div>
</div>
```

## Advanced Variations

### Location Selector with Autocomplete

```html
<div class="card mb-3">
  <div class="card-body">
    <h6 class="card-title"><i class="bi bi-geo-alt me-1"></i> Select Delivery Location</h6>
    <div class="row g-2 mb-3">
      <div class="col-md-4">
        <select class="form-select" id="countrySelect">
          <option selected>Country</option>
          <option value="US">United States</option>
          <option value="CA">Canada</option>
          <option value="UK">United Kingdom</option>
          <option value="AU">Australia</option>
        </select>
      </div>
      <div class="col-md-4">
        <select class="form-select" id="stateSelect" disabled>
          <option selected>State / Province</option>
        </select>
      </div>
      <div class="col-md-4">
        <input type="text" class="form-control" placeholder="ZIP / Postal Code" id="zipField">
      </div>
    </div>
    <button class="btn btn-primary btn-sm" id="calculateShipping">Calculate Shipping</button>
  </div>
</div>
```

### Shipping Timeline Accordion

```html
<div class="accordion" id="shippingTimeline">
  <div class="accordion-item">
    <h2 class="accordion-header">
      <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#timeline1">
        <i class="bi bi-box-seam me-2 text-primary"></i>
        <strong>Standard Shipping</strong>
        <span class="badge bg-secondary ms-2">$5.99</span>
      </button>
    </h2>
    <div id="timeline1" class="accordion-collapse collapse show" data-bs-parent="#shippingTimeline">
      <div class="accordion-body">
        <div class="d-flex align-items-center mb-2">
          <div class="bg-success rounded-circle p-1 me-2"><i class="bi bi-check text-white small"></i></div>
          <div>
            <strong class="d-block">Order Processed</strong>
            <small class="text-muted">April 2, 2026</small>
          </div>
        </div>
        <div class="d-flex align-items-center mb-2">
          <div class="bg-primary rounded-circle p-1 me-2"><i class="bi bi-truck text-white small"></i></div>
          <div>
            <strong class="d-block">In Transit</strong>
            <small class="text-muted">Estimated April 4-5</small>
          </div>
        </div>
        <div class="d-flex align-items-center">
          <div class="bg-light border rounded-circle p-1 me-2"><i class="bi bi-house small"></i></div>
          <div>
            <strong class="d-block text-muted">Delivered</strong>
            <small class="text-muted">Estimated April 7-9</small>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div class="accordion-item">
    <h2 class="accordion-header">
      <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#timeline2">
        <i class="bi bi-lightning-charge me-2 text-warning"></i>
        <strong>Express Shipping</strong>
        <span class="badge bg-warning text-dark ms-2">$12.99</span>
      </button>
    </h2>
    <div id="timeline2" class="accordion-collapse collapse" data-bs-parent="#shippingTimeline">
      <div class="accordion-body">
        <p class="mb-0">Estimated delivery: <strong>April 4-5, 2026</strong></p>
      </div>
    </div>
  </div>
</div>
```

### Free Shipping Progress Bar

```html
<div class="card bg-light border-0 mb-3">
  <div class="card-body py-2">
    <div class="d-flex justify-content-between mb-1">
      <small>Free shipping on orders over $50</small>
      <small class="text-success"><strong>$12.00 away!</strong></small>
    </div>
    <div class="progress" style="height: 8px;">
      <div class="progress-bar bg-success" role="progressbar" style="width: 76%;" aria-valuenow="76" aria-valuemin="0" aria-valuemax="100"></div>
    </div>
  </div>
</div>
```

## Best Practices

1. Show delivery estimates as early as possible, ideally on the product page
2. Use real postal code validation before calculating shipping
3. Display estimated date ranges rather than exact dates for accuracy
4. Highlight the recommended shipping option with visual distinction
5. Show free shipping thresholds with a progress bar to encourage larger orders
6. Include business day clarifications to set proper expectations
7. Cache shipping calculations to avoid redundant API calls for the same location
8. Use `list-group` with radio inputs for accessible shipping method selection
9. Display shipping cost clearly alongside each option
10. Include carrier logos when applicable for brand recognition
11. Provide tracking information links after order placement
12. Handle international shipping with country-specific options
13. Show a loading spinner during shipping calculation API calls

## Common Pitfalls

1. **No loading state during calculation**: Users may submit multiple requests if there is no visual feedback during the API call to calculate shipping rates.
2. **Showing exact delivery dates**: Guaranteeing a specific date creates customer service issues. Always show ranges with "estimated" language.
3. **Ignoring business days**: Displaying delivery dates that include weekends or holidays when carriers do not operate misleads customers.
4. **No fallback for unsupported locations**: Failing to show a clear message when a location is not serviced leaves users confused.
5. **Missing currency formatting**: Raw numbers like "12.99" without dollar signs or proper formatting look unprofessional.
6. **Not caching location results**: Repeated API calls for the same ZIP code waste resources and slow the experience.
7. **Radio buttons without labels**: Shipping options as radio buttons without associated `label` elements are not clickable on the text.

## Accessibility Considerations

- Use `list-group` with `label` elements wrapping radio inputs for full clickability
- Provide `aria-label` on the postal code input describing its purpose
- Use `role="alert"` on delivery result messages
- Associate shipping option descriptions using `aria-describedby`
- Ensure the location selector cascading dropdowns are keyboard navigable
- Use `aria-live="polite"` on delivery date updates so screen readers announce changes
- Include text alongside icon indicators in the shipping timeline

## Responsive Behavior

On mobile, the location selector inputs should stack vertically using `col-12` instead of `col-md-4`. Shipping option cards should display in a single column. The accordion timeline remains functional at all widths. The free shipping progress bar should maintain readable proportions. Cost amounts should remain visible and not truncate on narrow screens. Use `flex-column flex-sm-row` on shipping option layouts for better mobile presentation.
