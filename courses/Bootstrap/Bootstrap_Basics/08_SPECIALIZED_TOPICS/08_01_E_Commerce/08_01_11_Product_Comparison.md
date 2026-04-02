---
title: "Product Comparison"
description: "Build product comparison tables, side-by-side cards, and feature checkmark layouts using Bootstrap 5 utilities."
difficulty: 2
estimated_time: "30 minutes"
prerequisites:
  - "Bootstrap 5 Tables"
  - "Bootstrap 5 Cards"
  - "Bootstrap 5 Grid System"
---

## Overview

Product comparison components help shoppers evaluate multiple items simultaneously. Bootstrap 5 enables comparison tables with feature rows, side-by-side card layouts, and checkmark-based feature matrices. These components are critical for electronics, appliances, and software products where feature differentiation drives purchase decisions.

The comparison pattern typically presents 2-4 products in columns with shared feature rows, allowing users to scan differences vertically. Bootstrap's responsive table, card, and grid utilities handle layout across all screen sizes.

## Basic Implementation

### Side-by-Side Comparison Cards

```html
<div class="row g-3">
  <div class="col-md-4">
    <div class="card h-100">
      <img src="product1.jpg" class="card-img-top" alt="Basic Plan">
      <div class="card-body">
        <h5 class="card-title">Basic Plan</h5>
        <p class="display-6 fw-bold">$29<small class="text-muted fs-6">/mo</small></p>
        <ul class="list-unstyled">
          <li><i class="bi bi-check-circle-fill text-success me-1"></i> 5GB Storage</li>
          <li><i class="bi bi-check-circle-fill text-success me-1"></i> Email Support</li>
          <li><i class="bi bi-x-circle-fill text-danger me-1"></i> <s>Priority Support</s></li>
          <li><i class="bi bi-x-circle-fill text-danger me-1"></i> <s>Custom Domain</s></li>
        </ul>
        <button class="btn btn-outline-primary w-100">Select Plan</button>
      </div>
    </div>
  </div>
  <div class="col-md-4">
    <div class="card h-100 border-primary">
      <div class="card-header bg-primary text-white text-center">Most Popular</div>
      <img src="product2.jpg" class="card-img-top" alt="Pro Plan">
      <div class="card-body">
        <h5 class="card-title">Pro Plan</h5>
        <p class="display-6 fw-bold">$59<small class="text-muted fs-6">/mo</small></p>
        <ul class="list-unstyled">
          <li><i class="bi bi-check-circle-fill text-success me-1"></i> 50GB Storage</li>
          <li><i class="bi bi-check-circle-fill text-success me-1"></i> Priority Support</li>
          <li><i class="bi bi-check-circle-fill text-success me-1"></i> Custom Domain</li>
          <li><i class="bi bi-x-circle-fill text-danger me-1"></i> <s>White Label</s></li>
        </ul>
        <button class="btn btn-primary w-100">Select Plan</button>
      </div>
    </div>
  </div>
  <div class="col-md-4">
    <div class="card h-100">
      <img src="product3.jpg" class="card-img-top" alt="Enterprise Plan">
      <div class="card-body">
        <h5 class="card-title">Enterprise</h5>
        <p class="display-6 fw-bold">$99<small class="text-muted fs-6">/mo</small></p>
        <ul class="list-unstyled">
          <li><i class="bi bi-check-circle-fill text-success me-1"></i> Unlimited Storage</li>
          <li><i class="bi bi-check-circle-fill text-success me-1"></i> 24/7 Support</li>
          <li><i class="bi bi-check-circle-fill text-success me-1"></i> Custom Domain</li>
          <li><i class="bi bi-check-circle-fill text-success me-1"></i> White Label</li>
        </ul>
        <button class="btn btn-outline-primary w-100">Select Plan</button>
      </div>
    </div>
  </div>
</div>
```

### Feature Comparison Table

```html
<div class="table-responsive">
  <table class="table table-bordered text-center align-middle">
    <thead class="table-light">
      <tr>
        <th class="text-start">Feature</th>
        <th>Basic</th>
        <th class="table-primary">Pro</th>
        <th>Enterprise</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td class="text-start">Storage</td>
        <td>5GB</td>
        <td>50GB</td>
        <td>Unlimited</td>
      </tr>
      <tr>
        <td class="text-start">Users</td>
        <td>1</td>
        <td>5</td>
        <td>Unlimited</td>
      </tr>
      <tr>
        <td class="text-start">API Access</td>
        <td><i class="bi bi-x-circle-fill text-danger" aria-label="Not included"></i></td>
        <td><i class="bi bi-check-circle-fill text-success" aria-label="Included"></i></td>
        <td><i class="bi bi-check-circle-fill text-success" aria-label="Included"></i></td>
      </tr>
      <tr>
        <td class="text-start">SSO Integration</td>
        <td><i class="bi bi-x-circle-fill text-danger" aria-label="Not included"></i></td>
        <td><i class="bi bi-x-circle-fill text-danger" aria-label="Not included"></i></td>
        <td><i class="bi bi-check-circle-fill text-success" aria-label="Included"></i></td>
      </tr>
    </tbody>
  </table>
</div>
```

## Advanced Variations

### Sticky Header Comparison Table

```html
<div class="table-responsive" style="max-height: 400px; overflow-y: auto;">
  <table class="table table-bordered text-center align-middle">
    <thead class="table-light sticky-top">
      <tr>
        <th class="text-start bg-white">Feature</th>
        <th>Basic</th>
        <th class="table-primary">Pro</th>
        <th>Enterprise</th>
      </tr>
    </thead>
    <tbody>
      <tr><td class="text-start">Storage</td><td>5GB</td><td>50GB</td><td>Unlimited</td></tr>
      <tr><td class="text-start">Bandwidth</td><td>10GB</td><td>100GB</td><td>Unlimited</td></tr>
      <tr><td class="text-start">API Requests</td><td>1,000/mo</td><td>50,000/mo</td><td>Unlimited</td></tr>
      <tr><td class="text-start">Custom Domain</td><td><i class="bi bi-x-circle text-danger"></i></td><td><i class="bi bi-check-circle text-success"></i></td><td><i class="bi bi-check-circle text-success"></i></td></tr>
      <tr><td class="text-start">SSL Certificate</td><td><i class="bi bi-check-circle text-success"></i></td><td><i class="bi bi-check-circle text-success"></i></td><td><i class="bi bi-check-circle text-success"></i></td></tr>
      <tr><td class="text-start">Priority Support</td><td><i class="bi bi-x-circle text-danger"></i></td><td><i class="bi bi-check-circle text-success"></i></td><td><i class="bi bi-check-circle text-success"></i></td></tr>
      <tr><td class="text-start">Dedicated Account Manager</td><td><i class="bi bi-x-circle text-danger"></i></td><td><i class="bi bi-x-circle text-danger"></i></td><td><i class="bi bi-check-circle text-success"></i></td></tr>
    </tbody>
  </table>
</div>
```

### Highlight Differences Only Toggle

```html
<div class="form-check form-switch mb-3">
  <input class="form-check-input" type="checkbox" id="highlightDiff">
  <label class="form-check-label" for="highlightDiff">Show differences only</label>
</div>
```

### Product Spec Comparison Cards

```html
<div class="row g-3">
  <div class="col-md-6">
    <div class="card h-100">
      <div class="card-header d-flex justify-content-between align-items-center">
        <strong>iPhone 15 Pro</strong>
        <span class="badge bg-success">Recommended</span>
      </div>
      <ul class="list-group list-group-flush">
        <li class="list-group-item d-flex justify-content-between"><span>Display</span><strong>6.1" OLED</strong></li>
        <li class="list-group-item d-flex justify-content-between"><span>Processor</span><strong>A17 Pro</strong></li>
        <li class="list-group-item d-flex justify-content-between"><span>RAM</span><strong>8GB</strong></li>
        <li class="list-group-item d-flex justify-content-between"><span>Storage</span><strong>256GB</strong></li>
        <li class="list-group-item d-flex justify-content-between"><span>Battery</span><strong>3274mAh</strong></li>
      </ul>
    </div>
  </div>
  <div class="col-md-6">
    <div class="card h-100">
      <div class="card-header d-flex justify-content-between align-items-center">
        <strong>Galaxy S24</strong>
        <span class="badge bg-secondary">Popular</span>
      </div>
      <ul class="list-group list-group-flush">
        <li class="list-group-item d-flex justify-content-between"><span>Display</span><strong>6.2" AMOLED</strong></li>
        <li class="list-group-item d-flex justify-content-between"><span>Processor</span><strong>Snapdragon 8 Gen 3</strong></li>
        <li class="list-group-item d-flex justify-content-between"><span>RAM</span><strong>8GB</strong></li>
        <li class="list-group-item d-flex justify-content-between"><span>Storage</span><strong>256GB</strong></li>
        <li class="list-group-item d-flex justify-content-between"><span>Battery</span><strong>4000mAh</strong></li>
      </ul>
    </div>
  </div>
</div>
```

## Best Practices

1. Limit comparisons to 4 products maximum to avoid overwhelming users
2. Highlight the recommended or most popular option with `table-primary` or a badge
3. Use `table-responsive` to handle horizontal scrolling on mobile
4. Use check/cross icons with `aria-label` for included/not-included features
5. Keep feature labels in the first column consistently aligned with `text-start`
6. Use `sticky-top` on table headers for long comparison tables
7. Allow users to add/remove products from the comparison dynamically
8. Provide a "Highlight differences" toggle to surface key differentiators
9. Use consistent units and formats across all compared products
10. Include a clear CTA button for each compared product
11. Use `h-100` on cards to equalize heights in side-by-side layouts
12. Group related features into labeled sections within the table
13. Avoid comparing more than 6-8 features at once to maintain scannability

## Common Pitfalls

1. **Too many products**: Comparing more than 4 items horizontally creates an unusable table on mobile. Implement horizontal scroll or limit to 2 on small screens.
2. **Missing aria-labels on icons**: Check and cross icons need `aria-label="Included"` or `aria-label="Not included"` for screen readers.
3. **Inconsistent data formats**: Mixing "5GB", "5 GB", and "5 gigabytes" across products creates confusion. Standardize formatting.
4. **No mobile fallback**: Comparison tables without `table-responsive` break layout on mobile devices, requiring horizontal pinch-zoom.
5. **Visual clutter without grouping**: Listing 20+ features without section headers makes comparisons impossible to scan. Group related rows.
6. **Equal visual weight for all options**: Without highlighting the recommended plan, users cannot quickly identify the best value. Use badges or border colors.
7. **Non-sticky headers**: On scrollable tables, users lose column context when headers scroll out of view.

## Accessibility Considerations

- Use proper `th` and `scope` attributes on table headers for screen reader navigation
- Add `aria-label` to all icon-only check/cross indicators
- Use `caption` element or `aria-label` to describe the comparison table purpose
- Ensure comparison cards use semantic heading hierarchy
- Make interactive comparison toggles keyboard accessible
- Use `role="table"` and `role="grid"` appropriately based on interactivity level
- Provide text-based alternative summaries of key differences

## Responsive Behavior

On mobile, stack comparison cards vertically using `col-12` instead of `col-md-4`. For comparison tables, wrap in `table-responsive` for horizontal scrolling. Consider converting the table to stacked card layout on screens below 576px using CSS or JavaScript. Feature checkmark columns should remain visible with abbreviated labels. The "Recommended" badge should stay prominent at all breakpoints. Use `d-none d-md-table-cell` to hide less critical columns on small screens.
