---
title: "SaaS Dashboard"
module: "SaaS Applications"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["04_01_Card_Component", "04_09_Badges", "08_05_01_Admin_Dashboard_Layout"]
---

## Overview

A SaaS dashboard is the product's home screen where users monitor key metrics, take actions, and stay informed. Bootstrap 5 cards, list groups, badges, and grid components enable building KPI cards, activity feeds, quick action panels, and chart placeholder areas that provide a comprehensive overview at a glance.

## Basic Implementation

### KPI Summary Cards

```html
<div class="row g-4 mb-4">
  <div class="col-sm-6 col-xl-3">
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
        <div class="mt-3">
          <span class="badge bg-success bg-opacity-10 text-success">
            <i class="bi bi-arrow-up"></i> 12.5%
          </span>
          <span class="text-muted small ms-1">vs last month</span>
        </div>
      </div>
    </div>
  </div>
  <div class="col-sm-6 col-xl-3">
    <div class="card h-100">
      <div class="card-body">
        <div class="d-flex justify-content-between align-items-start">
          <div>
            <p class="text-muted small mb-1">Active Users</p>
            <h3 class="mb-0">2,847</h3>
          </div>
          <div class="bg-primary bg-opacity-10 rounded p-2">
            <i class="bi bi-people text-primary fs-4"></i>
          </div>
        </div>
        <div class="mt-3">
          <span class="badge bg-success bg-opacity-10 text-success">
            <i class="bi bi-arrow-up"></i> 8.2%
          </span>
          <span class="text-muted small ms-1">vs last month</span>
        </div>
      </div>
    </div>
  </div>
  <div class="col-sm-6 col-xl-3">
    <div class="card h-100">
      <div class="card-body">
        <div class="d-flex justify-content-between align-items-start">
          <div>
            <p class="text-muted small mb-1">Conversion Rate</p>
            <h3 class="mb-0">3.24%</h3>
          </div>
          <div class="bg-warning bg-opacity-10 rounded p-2">
            <i class="bi bi-graph-up text-warning fs-4"></i>
          </div>
        </div>
        <div class="mt-3">
          <span class="badge bg-danger bg-opacity-10 text-danger">
            <i class="bi bi-arrow-down"></i> 1.1%
          </span>
          <span class="text-muted small ms-1">vs last month</span>
        </div>
      </div>
    </div>
  </div>
  <div class="col-sm-6 col-xl-3">
    <div class="card h-100">
      <div class="card-body">
        <div class="d-flex justify-content-between align-items-start">
          <div>
            <p class="text-muted small mb-1">Support Tickets</p>
            <h3 class="mb-0">24</h3>
          </div>
          <div class="bg-danger bg-opacity-10 rounded p-2">
            <i class="bi bi-headset text-danger fs-4"></i>
          </div>
        </div>
        <div class="mt-3">
          <span class="badge bg-success bg-opacity-10 text-success">
            <i class="bi bi-arrow-down"></i> 18.0%
          </span>
          <span class="text-muted small ms-1">vs last month</span>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Chart Placeholder Area

```html
<div class="card mb-4">
  <div class="card-header bg-white d-flex justify-content-between align-items-center">
    <h5 class="mb-0">Revenue Overview</h5>
    <div class="btn-group btn-group-sm" role="group">
      <input type="radio" class="btn-check" name="period" id="7d" checked>
      <label class="btn btn-outline-secondary" for="7d">7D</label>
      <input type="radio" class="btn-check" name="period" id="30d">
      <label class="btn btn-outline-secondary" for="30d">30D</label>
      <input type="radio" class="btn-check" name="period" id="90d">
      <label class="btn btn-outline-secondary" for="90d">90D</label>
    </div>
  </div>
  <div class="card-body">
    <div class="bg-light rounded d-flex align-items-center justify-content-center" style="height:300px">
      <div class="text-center text-muted">
        <i class="bi bi-bar-chart-line fs-1 d-block mb-2"></i>
        <p>Chart placeholder - integrate with Chart.js or similar</p>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Activity Feed

```html
<div class="card h-100">
  <div class="card-header bg-white d-flex justify-content-between align-items-center">
    <h5 class="mb-0">Recent Activity</h5>
    <a href="#" class="small">View All</a>
  </div>
  <div class="card-body p-0">
    <ul class="list-group list-group-flush">
      <li class="list-group-item d-flex align-items-center">
        <div class="bg-success bg-opacity-10 rounded-circle p-2 me-3">
          <i class="bi bi-person-plus text-success"></i>
        </div>
        <div class="flex-grow-1">
          <p class="mb-0 small"><strong>Jane Smith</strong> signed up for Pro plan</p>
          <small class="text-muted">2 minutes ago</small>
        </div>
      </li>
      <li class="list-group-item d-flex align-items-center">
        <div class="bg-primary bg-opacity-10 rounded-circle p-2 me-3">
          <i class="bi bi-credit-card text-primary"></i>
        </div>
        <div class="flex-grow-1">
          <p class="mb-0 small"><strong>Payment received</strong> - $299.00</p>
          <small class="text-muted">15 minutes ago</small>
        </div>
      </li>
      <li class="list-group-item d-flex align-items-center">
        <div class="bg-warning bg-opacity-10 rounded-circle p-2 me-3">
          <i class="bi bi-exclamation-triangle text-warning"></i>
        </div>
        <div class="flex-grow-1">
          <p class="mb-0 small"><strong>API rate limit</strong> reached for Acme Corp</p>
          <small class="text-muted">1 hour ago</small>
        </div>
      </li>
    </ul>
  </div>
</div>
```

### Quick Actions Panel

```html
<div class="card">
  <div class="card-header bg-white">
    <h5 class="mb-0">Quick Actions</h5>
  </div>
  <div class="card-body">
    <div class="row g-3">
      <div class="col-6">
        <a href="#" class="btn btn-outline-primary w-100 py-3">
          <i class="bi bi-person-plus d-block fs-4 mb-1"></i>
          <span class="small">Invite User</span>
        </a>
      </div>
      <div class="col-6">
        <a href="#" class="btn btn-outline-primary w-100 py-3">
          <i class="bi bi-file-earmark-plus d-block fs-4 mb-1"></i>
          <span class="small">New Project</span>
        </a>
      </div>
      <div class="col-6">
        <a href="#" class="btn btn-outline-primary w-100 py-3">
          <i class="bi bi-bar-chart d-block fs-4 mb-1"></i>
          <span class="small">View Reports</span>
        </a>
      </div>
      <div class="col-6">
        <a href="#" class="btn btn-outline-primary w-100 py-3">
          <i class="bi bi-gear d-block fs-4 mb-1"></i>
          <span class="small">Settings</span>
        </a>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Display 4-6 KPI cards in the top row for at-a-glance metrics
2. Show trend indicators (up/down arrows) with percentage change
3. Use color-coded icons in KPI cards for visual categorization
4. Provide time period toggles (7D, 30D, 90D) on charts
5. Keep activity feeds chronological with timestamps
6. Use icon backgrounds (bg-opacity-10) for visual consistency
7. Make activity items clickable for drill-down detail
8. Group quick actions into a grid of icon buttons
9. Use `h-100` on cards in the same row for equal height
10. Include "View All" links for expandable sections

## Common Pitfalls

1. **Too many KPIs** - Showing 8+ metrics overwhelms users. Focus on 4-6 key indicators.
2. **No trend context** - Numbers without comparison lack meaning. Always show period-over-period change.
3. **Chart placeholders without data** - Integrate real chart libraries; static images feel unfinished.
4. **Activity feed too long** - Limit to 5-10 items with a "View All" link.
5. **No loading states** - KPIs that take time to load need skeleton placeholders.
6. **Quick actions not keyboard-accessible** - Icon-only buttons need `aria-label`.

## Accessibility Considerations

- Use `aria-label` on all icon-only quick action buttons
- Provide `aria-live="polite"` on KPI values that update in real-time
- Label chart placeholder areas with `aria-label="Revenue chart, loading"`
- Use semantic headings (`h5`) for card titles
- Ensure color-coded trend indicators also include text (up/down arrows)

## Responsive Behavior

On **mobile**, KPI cards stack into 2 columns. The activity feed and quick actions panels stack vertically. Chart areas span full width. On **tablet**, KPI cards can display in a 2x2 grid. On **desktop**, all 4 KPI cards display in a single row. Activity and quick actions sit side by side below the chart.
