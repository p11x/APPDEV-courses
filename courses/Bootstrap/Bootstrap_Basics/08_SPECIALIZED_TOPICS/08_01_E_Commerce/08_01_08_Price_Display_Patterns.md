---
title: "Price Display Patterns"
module: "E-Commerce"
difficulty: 1
estimated_time: "15 min"
prerequisites: ["04_09_Badges", "04_05_Forms"]
---

## Overview

Price display is a core e-commerce pattern that communicates value at a glance. Bootstrap 5 provides typography utilities, badges, and form components to display sale prices, original prices, currency formatting, discount badges, and price range sliders. Clear, consistent pricing reduces confusion and builds trust throughout the shopping experience.

## Basic Implementation

### Standard and Sale Prices

```html
<!-- Regular price -->
<p class="fs-4 fw-bold text-primary mb-0">$129.99</p>

<!-- Sale price with original -->
<div class="d-flex align-items-center gap-2">
  <span class="text-decoration-line-through text-muted">$179.99</span>
  <span class="fs-4 fw-bold text-danger">$129.99</span>
  <span class="badge bg-danger">Save 28%</span>
</div>

<!-- Price with tax note -->
<div>
  <span class="fs-4 fw-bold">$129.99</span>
  <small class="text-muted d-block">Excluding tax</small>
</div>
```

### Discount Badge Variations

```html
<span class="badge bg-danger fs-6">28% OFF</span>
<span class="badge bg-success fs-6">Save $50</span>
<span class="badge bg-warning text-dark">Buy 2 Get 1 Free</span>
<span class="badge bg-info">Free Shipping</span>
```

## Advanced Variations

### Price Range Slider

```html
<div class="mb-4">
  <label class="form-label fw-bold">Price Range</label>
  <div class="row g-2 mb-2">
    <div class="col-6">
      <div class="input-group input-group-sm">
        <span class="input-group-text">$</span>
        <input type="number" class="form-control" value="0" min="0" id="priceMin">
      </div>
    </div>
    <div class="col-6">
      <div class="input-group input-group-sm">
        <span class="input-group-text">$</span>
        <input type="number" class="form-control" value="500" max="1000" id="priceMax">
      </div>
    </div>
  </div>
  <input type="range" class="form-range" min="0" max="1000" value="500" id="priceSlider">
  <div class="d-flex justify-content-between small text-muted">
    <span>$0</span>
    <span>$1,000</span>
  </div>
</div>
```

### Multi-Currency Display

```html
<div class="dropdown">
  <button class="btn btn-outline-secondary btn-sm dropdown-toggle" data-bs-toggle="dropdown">
    USD ($)
  </button>
  <ul class="dropdown-menu">
    <li><a class="dropdown-item active" href="#">USD ($)</a></li>
    <li><a class="dropdown-item" href="#">EUR (&euro;)</a></li>
    <li><a class="dropdown-item" href="#">GBP (&pound;)</a></li>
    <li><a class="dropdown-item" href="#">JPY (&yen;)</a></li>
  </ul>
</div>
```

### Tiered / Volume Pricing

```html
<table class="table table-sm table-bordered w-auto">
  <thead class="table-light">
    <tr>
      <th>Quantity</th>
      <th>Price Per Unit</th>
      <th>Savings</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>1-9</td>
      <td>$12.99</td>
      <td>-</td>
    </tr>
    <tr class="table-success">
      <td>10-49</td>
      <td>$10.99</td>
      <td><span class="badge bg-success">Save 15%</span></td>
    </tr>
    <tr>
      <td>50+</td>
      <td>$8.99</td>
      <td><span class="badge bg-success">Save 31%</span></td>
    </tr>
  </tbody>
</table>
```

### Subscription Pricing

```html
<div class="d-flex align-items-baseline gap-2">
  <span class="fs-3 fw-bold">$9</span>
  <span class="text-muted">/ month</span>
</div>
<div class="d-flex align-items-baseline gap-2">
  <span class="fs-3 fw-bold">$89</span>
  <span class="text-muted">/ year</span>
  <span class="badge bg-success">Save 17%</span>
</div>
```

## Best Practices

1. Use `text-decoration-line-through` for original prices, not manual strikethrough characters
2. Always show the currency symbol before the amount for USD/EUR
3. Use `fw-bold` and larger font size for the current price to draw attention
4. Display discount as both a percentage and absolute dollar amount
5. Use color coding: red for sales, green for savings, muted for original prices
6. Include "Excluding/Including tax" labels to prevent checkout surprises
7. Format large numbers with commas (e.g., $1,299.99) for readability
8. Use Bootstrap badges for compact discount indicators
9. Provide a currency selector for international stores
10. Show per-unit pricing for bulk/subscription products
11. Use consistent price formatting across all pages

## Common Pitfalls

1. **Inconsistent formatting** - Mixing "$129.99" with "129.99$" across pages confuses users. Standardize formatting.
2. **No sale indicator** - Showing only the discounted price without the original doesn't communicate value. Always show both.
3. **Hardcoded currency symbol** - International stores must format prices according to locale. Use `Intl.NumberFormat`.
4. **Tiny discount badges** - Badges that are too small get overlooked. Use at least `fs-6` for visibility.
5. **Missing tax information** - Surprise tax at checkout increases abandonment. Indicate tax inclusion/exclusion upfront.
6. **Price range slider without input fallback** - Sliders are imprecise. Always pair with number inputs.

## Accessibility Considerations

- Use `aria-label="Sale price: $129.99, originally $179.99"` for screen reader context
- Mark the original price with `<del>` or `aria-hidden="true"` so screen readers don't read confusing strikethrough text
- Associate the price range slider with its label via `aria-labelledby`
- Use `aria-valuemin`, `aria-valuemax`, `aria-valuenow` on custom range controls
- Ensure discount badges have sufficient color contrast

## Responsive Behavior

Price displays are inherently flexible and work at all sizes. On **mobile**, stack the original and sale prices vertically if they don't fit side by side. Price range sliders should span full width on small screens. Volume pricing tables should use `table-responsive` on mobile. Currency selectors become dropdowns on all screen sizes. Discount badges remain inline with prices at all breakpoints.
