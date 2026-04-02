---
title: Pricing Page
category: Professional Practice
difficulty: 2
time: 40 min
tags: bootstrap5, pricing, cards, comparison-table, toggle, responsive
---

## Overview

A pricing page communicates plan options and drives conversions. Bootstrap 5 cards, grid, form switches, and table components enable a professional pricing layout with highlighted plans, monthly/annual toggles, and feature comparison tables.

## Basic Implementation

### Monthly/Annual Toggle

```html
<div class="container py-5 text-center">
  <h2 class="fw-bold mb-2">Choose Your Plan</h2>
  <p class="text-muted mb-4">Simple, transparent pricing that grows with you.</p>
  <div class="d-flex justify-content-center align-items-center gap-3 mb-5">
    <span class="fw-semibold" id="monthlyLabel">Monthly</span>
    <div class="form-check form-switch d-inline-block">
      <input class="form-check-input" type="checkbox" role="switch" id="billingToggle" style="width:3rem;height:1.5rem">
    </div>
    <span class="text-muted" id="annualLabel">Annual <span class="badge bg-success">Save 20%</span></span>
  </div>
```

### Pricing Cards

```html
  <div class="row g-4 justify-content-center">
    <!-- Free Plan -->
    <div class="col-md-6 col-lg-4">
      <div class="card h-100 border-0 shadow-sm">
        <div class="card-body p-4 text-start">
          <h5 class="text-muted">Free</h5>
          <div class="my-3">
            <span class="display-5 fw-bold">$0</span>
            <span class="text-muted">/month</span>
          </div>
          <p class="text-muted">Perfect for getting started and personal projects.</p>
          <ul class="list-unstyled mb-4">
            <li class="mb-2"><i class="bi bi-check-circle-fill text-success me-2"></i>1 project</li>
            <li class="mb-2"><i class="bi bi-check-circle-fill text-success me-2"></i>Basic analytics</li>
            <li class="mb-2"><i class="bi bi-check-circle-fill text-success me-2"></i>Community support</li>
            <li class="mb-2 text-muted"><i class="bi bi-x-circle text-secondary me-2"></i>Custom domain</li>
            <li class="mb-2 text-muted"><i class="bi bi-x-circle text-secondary me-2"></i>Priority support</li>
          </ul>
          <a href="#" class="btn btn-outline-primary w-100">Get Started</a>
        </div>
      </div>
    </div>

    <!-- Pro Plan (Highlighted) -->
    <div class="col-md-6 col-lg-4">
      <div class="card h-100 border-primary shadow">
        <div class="card-header bg-primary text-white text-center py-2">
          <span class="fw-semibold"><i class="bi bi-star-fill me-1"></i>Most Popular</span>
        </div>
        <div class="card-body p-4 text-start">
          <h5 class="text-primary">Pro</h5>
          <div class="my-3">
            <span class="display-5 fw-bold" data-monthly="$29" data-annual="$23">$29</span>
            <span class="text-muted">/month</span>
          </div>
          <p class="text-muted">For professionals and growing teams.</p>
          <ul class="list-unstyled mb-4">
            <li class="mb-2"><i class="bi bi-check-circle-fill text-success me-2"></i>Unlimited projects</li>
            <li class="mb-2"><i class="bi bi-check-circle-fill text-success me-2"></i>Advanced analytics</li>
            <li class="mb-2"><i class="bi bi-check-circle-fill text-success me-2"></i>Custom domain</li>
            <li class="mb-2"><i class="bi bi-check-circle-fill text-success me-2"></i>Priority email support</li>
            <li class="mb-2"><i class="bi bi-check-circle-fill text-success me-2"></i>Team collaboration</li>
          </ul>
          <a href="#" class="btn btn-primary w-100">Start Free Trial</a>
        </div>
      </div>
    </div>

    <!-- Enterprise Plan -->
    <div class="col-md-6 col-lg-4">
      <div class="card h-100 border-0 shadow-sm">
        <div class="card-body p-4 text-start">
          <h5 class="text-muted">Enterprise</h5>
          <div class="my-3">
            <span class="display-5 fw-bold" data-monthly="$99" data-annual="$79">$99</span>
            <span class="text-muted">/month</span>
          </div>
          <p class="text-muted">For large organizations with custom needs.</p>
          <ul class="list-unstyled mb-4">
            <li class="mb-2"><i class="bi bi-check-circle-fill text-success me-2"></i>Everything in Pro</li>
            <li class="mb-2"><i class="bi bi-check-circle-fill text-success me-2"></i>SSO & SAML</li>
            <li class="mb-2"><i class="bi bi-check-circle-fill text-success me-2"></i>Dedicated account manager</li>
            <li class="mb-2"><i class="bi bi-check-circle-fill text-success me-2"></i>99.9% SLA uptime</li>
            <li class="mb-2"><i class="bi bi-check-circle-fill text-success me-2"></i>Custom integrations</li>
          </ul>
          <a href="#" class="btn btn-outline-primary w-100">Contact Sales</a>
        </div>
      </div>
    </div>
  </div>
```

### Feature Comparison Table

```html
  <div class="mt-5">
    <h3 class="fw-bold text-center mb-4">Compare Plans</h3>
    <div class="table-responsive">
      <table class="table table-bordered text-center align-middle">
        <thead class="table-light">
          <tr>
            <th class="text-start">Feature</th>
            <th>Free</th>
            <th class="table-primary">Pro</th>
            <th>Enterprise</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td class="text-start">Projects</td>
            <td>1</td>
            <td>Unlimited</td>
            <td>Unlimited</td>
          </tr>
          <tr>
            <td class="text-start">Storage</td>
            <td>5 GB</td>
            <td>50 GB</td>
            <td>500 GB</td>
          </tr>
          <tr>
            <td class="text-start">Team Members</td>
            <td>1</td>
            <td>10</td>
            <td>Unlimited</td>
          </tr>
          <tr>
            <td class="text-start">Analytics</td>
            <td><i class="bi bi-check text-success"></i></td>
            <td><i class="bi bi-check text-success"></i></td>
            <td><i class="bi bi-check text-success"></i></td>
          </tr>
          <tr>
            <td class="text-start">Custom Domain</td>
            <td><i class="bi bi-x text-muted"></i></td>
            <td><i class="bi bi-check text-success"></i></td>
            <td><i class="bi bi-check text-success"></i></td>
          </tr>
          <tr>
            <td class="text-start">Priority Support</td>
            <td><i class="bi bi-x text-muted"></i></td>
            <td><i class="bi bi-check text-success"></i></td>
            <td><i class="bi bi-check text-success"></i></td>
          </tr>
          <tr>
            <td class="text-start">SSO</td>
            <td><i class="bi bi-x text-muted"></i></td>
            <td><i class="bi bi-x text-muted"></i></td>
            <td><i class="bi bi-check text-success"></i></td>
          </tr>
          <tr>
            <td class="text-start">SLA</td>
            <td><i class="bi bi-x text-muted"></i></td>
            <td><i class="bi bi-x text-muted"></i></td>
            <td>99.9%</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</div>
```

### Toggle JavaScript

```javascript
document.getElementById('billingToggle').addEventListener('change', function () {
  const isAnnual = this.checked;
  document.querySelectorAll('[data-monthly]').forEach(el => {
    el.textContent = isAnnual ? el.dataset.annual : el.dataset.monthly;
  });
  document.getElementById('monthlyLabel').classList.toggle('text-muted', isAnnual);
  document.getElementById('monthlyLabel').classList.toggle('fw-semibold', !isAnnual);
  document.getElementById('annualLabel').classList.toggle('text-muted', !isAnnual);
  document.getElementById('annualLabel').classList.toggle('fw-semibold', isAnnual);
});
```

## Advanced Variations

- **Slider Pricing:** Replace the toggle with a slider that adjusts per-seat pricing dynamically.
- **Currency Selector:** Add a dropdown that converts displayed prices to different currencies via JS.
- **Tooltip on Features:** Use `data-bs-toggle="tooltip"` on info icons next to complex features for explanations.
- **Animated Card Hover:** Apply CSS `transform: scale(1.03)` on card hover for emphasis.
- **FAQ Accordion Below Pricing:** Append a Bootstrap accordion with common pricing questions.

## Best Practices

1. Use `col-lg-4` for three equal pricing cards that stack on smaller screens.
2. Highlight the recommended plan with `border-primary` and a `card-header` badge.
3. Apply `h-100` on all cards to equalize heights across the row.
4. Use `display-5 fw-bold` for price figures to make them the visual focal point.
5. Show included features with `bi-check-circle-fill text-success` and excluded with `bi-x-circle text-muted`.
6. Use `table-responsive` around the comparison table for horizontal scrolling on mobile.
7. Apply `table-primary` on the highlighted plan column in the comparison table.
8. Use `data-monthly` and `data-annual` data attributes to toggle prices with JavaScript.
9. Keep the toggle label format clear: "Monthly" | "Annual (Save 20%)".
10. Use `list-unstyled` on feature lists for clean rendering without bullet points.
11. Apply `shadow` on the highlighted card and `shadow-sm` on others for subtle differentiation.
12. Center the entire pricing section with `text-center` on the container.

## Common Pitfalls

1. **No toggle feedback:** Without visual label changes, users don't know which billing period is active.
2. **Missing `h-100` on cards:** Pricing cards of different lengths create an uneven layout.
3. **Too many plans:** More than three or four plans overwhelms visitors; consolidate if possible.
4. **No comparison table:** Power users need feature-by-feature comparison; hiding it reduces trust.
5. **Hardcoded prices in JS:** Use data attributes on elements instead of hardcoding values in JavaScript.
6. **Non-responsive table:** Without `table-responsive`, the comparison table breaks on mobile.
7. **Inaccessible toggle:** Use `role="switch"` and `aria-checked` on the billing toggle checkbox.

## Accessibility Considerations

- Add `role="switch"` and `aria-checked` to the billing toggle checkbox.
- Use `aria-label` on pricing card links (e.g., `aria-label="Start Pro plan free trial"`).
- Apply `scope="col"` on all `<th>` elements in the comparison table.
- Use `aria-describedby` on the toggle to reference the "Save 20%" label.
- Ensure sufficient color contrast on the highlighted card header (`bg-primary text-white`).

## Responsive Behavior

| Breakpoint | Pricing Cards | Comparison Table | Toggle |
|------------|--------------|-----------------|--------|
| `<576px` | 1 column | Horizontal scroll | Centered |
| `≥576px` | 1 column | Horizontal scroll | Centered |
| `≥768px` | 2 columns + 1 below | Horizontal scroll | Centered |
| `≥992px` | 3 columns | Full table | Centered |
| `≥1200px` | 3 columns | Full table | Centered |
