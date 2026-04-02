---
title: "Stat Card Patterns"
module: "Dashboards & Analytics"
difficulty: 1
estimated_time: "15 min"
prerequisites: ["04_01_Card_Component", "04_09_Badges"]
---

## Overview

Stat cards display key performance indicators (KPIs) at a glance. Bootstrap 5 cards, badges, and utility classes create compact, informative metric displays with trend indicators, icons, and comparison data that form the building blocks of any dashboard.

## Basic Implementation

### Simple KPI Card

```html
<div class="card h-100">
  <div class="card-body">
    <div class="d-flex justify-content-between align-items-start">
      <div>
        <p class="text-muted small mb-1">Total Revenue</p>
        <h3 class="mb-0">$48,250</h3>
      </div>
      <div class="bg-success bg-opacity-10 rounded p-2">
        <i class="bi bi-currency-dollar text-success fs-4"></i>
      </div>
    </div>
  </div>
</div>
```

### KPI Card with Trend

```html
<div class="row g-4">
  <div class="col-sm-6 col-xl-3">
    <div class="card h-100">
      <div class="card-body">
        <div class="d-flex justify-content-between align-items-start mb-3">
          <div>
            <p class="text-muted small mb-1">Total Revenue</p>
            <h3 class="mb-0">$48,250</h3>
          </div>
          <div class="bg-success bg-opacity-10 rounded p-2">
            <i class="bi bi-graph-up-arrow text-success fs-5"></i>
          </div>
        </div>
        <div class="d-flex align-items-center gap-2">
          <span class="badge bg-success bg-opacity-10 text-success">
            <i class="bi bi-arrow-up"></i> 12.5%
          </span>
          <span class="text-muted small">vs last month</span>
        </div>
      </div>
    </div>
  </div>
  <div class="col-sm-6 col-xl-3">
    <div class="card h-100">
      <div class="card-body">
        <div class="d-flex justify-content-between align-items-start mb-3">
          <div>
            <p class="text-muted small mb-1">Active Users</p>
            <h3 class="mb-0">2,847</h3>
          </div>
          <div class="bg-primary bg-opacity-10 rounded p-2">
            <i class="bi bi-people text-primary fs-5"></i>
          </div>
        </div>
        <div class="d-flex align-items-center gap-2">
          <span class="badge bg-success bg-opacity-10 text-success">
            <i class="bi bi-arrow-up"></i> 8.2%
          </span>
          <span class="text-muted small">vs last month</span>
        </div>
      </div>
    </div>
  </div>
  <div class="col-sm-6 col-xl-3">
    <div class="card h-100">
      <div class="card-body">
        <div class="d-flex justify-content-between align-items-start mb-3">
          <div>
            <p class="text-muted small mb-1">Conversion Rate</p>
            <h3 class="mb-0">3.24%</h3>
          </div>
          <div class="bg-warning bg-opacity-10 rounded p-2">
            <i class="bi bi-bullseye text-warning fs-5"></i>
          </div>
        </div>
        <div class="d-flex align-items-center gap-2">
          <span class="badge bg-danger bg-opacity-10 text-danger">
            <i class="bi bi-arrow-down"></i> 1.1%
          </span>
          <span class="text-muted small">vs last month</span>
        </div>
      </div>
    </div>
  </div>
  <div class="col-sm-6 col-xl-3">
    <div class="card h-100">
      <div class="card-body">
        <div class="d-flex justify-content-between align-items-start mb-3">
          <div>
            <p class="text-muted small mb-1">Avg Order Value</p>
            <h3 class="mb-0">$127</h3>
          </div>
          <div class="bg-info bg-opacity-10 rounded p-2">
            <i class="bi bi-receipt text-info fs-5"></i>
          </div>
        </div>
        <div class="d-flex align-items-center gap-2">
          <span class="badge bg-success bg-opacity-10 text-success">
            <i class="bi bi-arrow-up"></i> 5.3%
          </span>
          <span class="text-muted small">vs last month</span>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Sparkline Card

```html
<div class="card h-100">
  <div class="card-body">
    <div class="d-flex justify-content-between align-items-start mb-3">
      <div>
        <p class="text-muted small mb-1">Weekly Revenue</p>
        <h3 class="mb-0">$12,450</h3>
      </div>
      <span class="badge bg-success bg-opacity-10 text-success">
        <i class="bi bi-arrow-up"></i> 18%
      </span>
    </div>
    <div class="d-flex align-items-end gap-1" style="height:40px">
      <div class="bg-primary bg-opacity-25 rounded" style="width:12%;height:30%"></div>
      <div class="bg-primary bg-opacity-25 rounded" style="width:12%;height:45%"></div>
      <div class="bg-primary bg-opacity-25 rounded" style="width:12%;height:35%"></div>
      <div class="bg-primary bg-opacity-25 rounded" style="width:12%;height:60%"></div>
      <div class="bg-primary bg-opacity-25 rounded" style="width:12%;height:50%"></div>
      <div class="bg-primary bg-opacity-25 rounded" style="width:12%;height:75%"></div>
      <div class="bg-primary rounded" style="width:12%;height:90%"></div>
    </div>
    <div class="d-flex justify-content-between mt-1">
      <small class="text-muted">Mon</small>
      <small class="text-muted">Sun</small>
    </div>
  </div>
</div>
```

### Comparison Card

```html
<div class="card h-100">
  <div class="card-body">
    <p class="text-muted small mb-3">Revenue Comparison</p>
    <div class="mb-3">
      <div class="d-flex justify-content-between mb-1">
        <span class="small">This Month</span>
        <strong>$48,250</strong>
      </div>
      <div class="progress" style="height:8px">
        <div class="progress-bar bg-primary" style="width:85%"></div>
      </div>
    </div>
    <div>
      <div class="d-flex justify-content-between mb-1">
        <span class="small">Last Month</span>
        <strong class="text-muted">$42,890</strong>
      </div>
      <div class="progress" style="height:8px">
        <div class="progress-bar bg-secondary" style="width:72%"></div>
      </div>
    </div>
    <div class="mt-3 text-center">
      <span class="badge bg-success bg-opacity-10 text-success">
        <i class="bi bi-arrow-up"></i> 12.5% increase
      </span>
    </div>
  </div>
</div>
```

### Mini Stat with Icon

```html
<div class="d-flex align-items-center p-3 border rounded">
  <div class="bg-primary bg-opacity-10 rounded p-3 me-3">
    <i class="bi bi-cart-check text-primary fs-4"></i>
  </div>
  <div>
    <p class="text-muted small mb-0">Orders Today</p>
    <h5 class="mb-0">847</h5>
  </div>
</div>
```

## Best Practices

1. Show 4-6 KPI cards maximum to avoid information overload
2. Include trend indicators (up/down arrows) with percentage change
3. Use color-coded icons in background containers (bg-opacity-10)
4. Provide comparison context ("vs last month")
5. Use `h-100` on all cards in a row for equal height
6. Include sparkline charts for trend visualization
7. Use progress bars for comparison metrics
8. Keep labels short and descriptive
9. Use consistent icon sizing across all cards
10. Apply `badge` with `bg-opacity-10` for subtle trend indicators

## Common Pitfalls

1. **No trend context** - Numbers alone lack meaning. Always show comparison data.
2. **Too many KPIs** - 10+ cards overwhelm users. Focus on 4-6 key metrics.
3. **Missing labels** - "48,250" without context is meaningless. Always include labels.
4. **Inconsistent styling** - Mixing card styles creates visual clutter. Use a consistent pattern.
5. **No responsive grid** - Cards must stack on mobile. Use `col-sm-6 col-xl-3`.
6. **Color-only trends** - Red/green indicators aren't accessible. Include arrow icons and text.

## Accessibility Considerations

- Use `aria-label="Total Revenue: $48,250, up 12.5% from last month"` on KPI cards
- Mark trend badges with text labels in addition to arrows
- Use `role="status"` for cards displaying live data
- Ensure icon colors meet 3:1 contrast minimum

## Responsive Behavior

On **mobile**, stat cards stack in 2 columns (`col-sm-6`). On **tablet**, they can display in a 2x2 grid. On **desktop** (`col-xl-3`), all 4 cards display in a single row. Sparkline charts scale to fit their container.
