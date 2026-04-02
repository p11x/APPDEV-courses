---
title: "Responsive Column Stacking"
topic: "Grid Deep Dive"
subtopic: "Responsive Column Stacking"
difficulty: 1
duration: "25 minutes"
prerequisites: ["Responsive Breakpoints", "Bootstrap Grid Basics"]
learning_objectives:
  - Understand mobile-first column stacking behavior
  - Apply sm breakpoint classes for progressive display
  - Create layouts that stack on mobile and expand on desktop
---

## Overview

Bootstrap's mobile-first grid stacks columns vertically by default. Without a breakpoint class, `col-12` forces full-width stacking, while `col-sm-6` or `col-md-4` enables side-by-side display only at and above that breakpoint. This progressive approach ensures mobile users see readable, single-column layouts while desktop users benefit from multi-column designs.

## Basic Implementation

Columns that stack on all screen sizes (no breakpoint class):

```html
<div class="container">
  <div class="row">
    <div class="col-12">
      <div class="bg-primary text-white p-3">Full width — always stacked</div>
    </div>
    <div class="col-12">
      <div class="bg-secondary text-white p-3">Full width — always stacked</div>
    </div>
  </div>
</div>
```

Columns that stack on mobile but sit side-by-side at `sm` (576px+):

```html
<div class="container">
  <div class="row">
    <div class="col-sm-6">
      <div class="bg-success text-white p-3">Half on sm+, full on mobile</div>
    </div>
    <div class="col-sm-6">
      <div class="bg-warning p-3">Half on sm+, full on mobile</div>
    </div>
  </div>
</div>
```

Three columns that stack below `md` and display as thirds on desktop:

```html
<div class="container">
  <div class="row">
    <div class="col-md-4">
      <div class="bg-info text-white p-3">1/3 on md+</div>
    </div>
    <div class="col-md-4">
      <div class="bg-danger text-white p-3">1/3 on md+</div>
    </div>
    <div class="col-md-4">
      <div class="bg-dark text-white p-3">1/3 on md+</div>
    </div>
  </div>
</div>
```

## Advanced Variations

Progressive column display — 1 col on mobile, 2 on sm, 3 on md, 4 on lg:

```html
<div class="container">
  <div class="row">
    <div class="col-12 col-sm-6 col-md-4 col-lg-3">
      <div class="bg-primary text-white p-3">Progressive</div>
    </div>
    <div class="col-12 col-sm-6 col-md-4 col-lg-3">
      <div class="bg-secondary text-white p-3">Progressive</div>
    </div>
    <div class="col-12 col-sm-6 col-md-4 col-lg-3">
      <div class="bg-success text-white p-3">Progressive</div>
    </div>
    <div class="col-12 col-sm-6 col-md-4 col-lg-3">
      <div class="bg-warning p-3">Progressive</div>
    </div>
  </div>
</div>
```

Sidebar that stacks below content on mobile:

```html
<div class="container">
  <div class="row">
    <div class="col-12 col-md-8 order-md-1">
      <div class="bg-white p-3 border">Main content (first on mobile, left on desktop)</div>
    </div>
    <div class="col-12 col-md-4 order-md-2">
      <div class="bg-light p-3 border">Sidebar (second on mobile, right on desktop)</div>
    </div>
  </div>
</div>
```

Form layout that stacks labels and inputs on mobile:

```html
<div class="container">
  <div class="row g-3">
    <div class="col-12 col-sm-6">
      <input type="text" class="form-control" placeholder="First name">
    </div>
    <div class="col-12 col-sm-6">
      <input type="text" class="form-control" placeholder="Last name">
    </div>
    <div class="col-12">
      <input type="email" class="form-control" placeholder="Email address">
    </div>
  </div>
</div>
```

## Best Practices

1. Always start with mobile stacking (no column class or `col-12`) and add breakpoint classes for larger screens.
2. Use `col-sm-6` for two-column layouts that stack below 576px — the most common responsive pattern.
3. Apply `col-md-4` for three-column layouts that stack on tablets and mobile.
4. Combine `col-12 col-sm-6 col-lg-4` for progressive column display across all breakpoints.
5. Use `g-*` gutter classes with stacking columns to add spacing when side-by-side.
6. Apply `order-*` classes to rearrange stacked columns on mobile without changing HTML order.
7. Test at each breakpoint boundary (576px, 768px, 992px, 1200px, 1400px) to verify stacking transitions.
8. Use `row-cols-1 row-cols-md-2` as an alternative to individual `col-12 col-md-6` for uniform columns.
9. Keep stacked column content concise on mobile — long paragraphs in narrow stacked columns reduce readability.
10. Use `d-grid gap-2` on the row for stacked columns that need consistent vertical spacing.

## Common Pitfalls

- **Forgetting `col-12` for mobile**: Without `col-12`, columns may not stack properly on mobile if content is narrow.
- **Breakpoint specificity**: `col-sm-6` doesn't override `col-4` — specify both `col-12 col-sm-6` for explicit mobile stacking.
- **Content overflow in stacked columns**: Long URLs or unbroken text in narrow stacked columns causes horizontal overflow.
- **Ignoring touch targets**: Stacked buttons or links need adequate spacing (minimum 44x44px) for touch interaction.
- **Gutters on stacked layouts**: Large gutters (`g-5`) between stacked columns waste vertical space on mobile.
- **Not testing real devices**: Browser responsive mode doesn't always match real device behavior for stacking.
- **Overriding with `flex-nowrap`**: Adding `flex-nowrap` to rows prevents stacking entirely.

## Accessibility Considerations

- Ensure stacked column reading order matches visual order on mobile — users scroll top to bottom.
- Maintain sufficient spacing between stacked columns so touch targets don't overlap.
- Use semantic heading levels (`h2`, `h3`) consistently within stacked columns.
- Provide `aria-label` on navigation columns that stack below main content on mobile.
- Ensure color contrast ratios are maintained when columns stack and backgrounds change.
- Test with screen readers to verify content makes sense in stacked single-column order.

## Responsive Behavior

Bootstrap's five breakpoints (xs, sm, md, lg, xl, xxl) control when columns stack and when they display side-by-side. A column class without a breakpoint (e.g., `col-6`) applies at all sizes. A breakpoint class (e.g., `col-md-6`) only applies at that breakpoint and above, defaulting to stacking below it.

```html
<div class="container">
  <div class="row">
    <div class="col-12 col-md-6 col-xl-4">
      <div class="bg-primary text-white p-3">
        Full (mobile) → Half (md) → Third (xl)
      </div>
    </div>
    <div class="col-12 col-md-6 col-xl-4">
      <div class="bg-secondary text-white p-3">
        Full (mobile) → Half (md) → Third (xl)
      </div>
    </div>
    <div class="col-12 col-xl-4">
      <div class="bg-success text-white p-3">
        Full (mobile/md) → Third (xl)
      </div>
    </div>
  </div>
</div>
```

Each breakpoint class cascades upward — `col-md-6` applies at md, lg, xl, and xxl unless overridden by a larger breakpoint class like `col-xl-4`.
