---
title: "Comparison Charts"
description: "Build before/after comparison cards, period-over-period comparisons, and trend indicator displays using Bootstrap 5."
difficulty: 2
estimated_time: "30 minutes"
prerequisites:
  - "Bootstrap 5 Cards"
  - "Bootstrap 5 Grid"
  - "Bootstrap 5 Utilities"
---

## Overview

Comparison charts display metrics side by side for period-over-period analysis, before/after comparisons, and trend visualization. Bootstrap 5's card, grid, and utility components create visual comparison layouts with trend indicators, percentage changes, and color-coded performance markers.

These components are essential for dashboards where users need to quickly assess whether metrics are improving or declining. The pattern uses directional icons, color coding (green for positive, red for negative), and clear numeric comparisons.

## Basic Implementation

### Period Comparison Card

```html
<div class="card">
  <div class="card-body">
    <div class="d-flex justify-content-between align-items-start mb-2">
      <div class="text-muted small">Total Revenue</div>
      <span class="badge bg-success"><i class="bi bi-arrow-up"></i> 12.5%</span>
    </div>
    <div class="display-6 fw-bold">$48,250</div>
    <div class="text-muted small mt-1">
      vs. <span class="text-decoration-line-through">$42,890</span> last month
    </div>
  </div>
</div>
```

### Before/After Side-by-Side

```html
<div class="card">
  <div class="card-body">
    <h6 class="card-title text-center mb-3">Conversion Rate</h6>
    <div class="row text-center">
      <div class="col-5">
        <div class="text-muted small mb-1">Before</div>
        <div class="fs-4 fw-bold text-secondary">2.4%</div>
        <small class="text-muted">Jan 2026</small>
      </div>
      <div class="col-2 d-flex align-items-center justify-content-center">
        <i class="bi bi-arrow-right fs-4 text-primary"></i>
      </div>
      <div class="col-5">
        <div class="text-muted small mb-1">After</div>
        <div class="fs-4 fw-bold text-success">3.8%</div>
        <small class="text-muted">Mar 2026</small>
      </div>
    </div>
    <div class="text-center mt-3">
      <span class="badge bg-success"><i class="bi bi-arrow-up"></i> +1.4pp improvement</span>
    </div>
  </div>
</div>
```

### Trend Indicator with Sparkline

```html
<div class="card">
  <div class="card-body d-flex justify-content-between align-items-center">
    <div>
      <div class="text-muted small">Active Users</div>
      <div class="fs-4 fw-bold">12,847</div>
      <div class="small">
        <span class="text-danger"><i class="bi bi-arrow-down"></i> 3.2%</span>
        <span class="text-muted">vs last week</span>
      </div>
    </div>
    <div style="width: 100px; height: 40px;">
      <svg viewBox="0 0 100 40" class="w-100 h-100">
        <polyline fill="none" stroke="#dc3545" stroke-width="2" points="0,10 20,15 40,8 60,20 80,18 100,30" />
      </svg>
    </div>
  </div>
</div>
```

## Advanced Variations

### Multi-Metric Comparison Grid

```html
<div class="row g-3">
  <div class="col-md-3">
    <div class="card h-100">
      <div class="card-body text-center">
        <div class="text-muted small mb-1">Revenue</div>
        <div class="fs-5 fw-bold">$48,250</div>
        <div class="small text-success"><i class="bi bi-arrow-up"></i> 12.5%</div>
        <div class="progress mt-2" style="height: 4px;">
          <div class="progress-bar bg-success" style="width: 80%;"></div>
        </div>
      </div>
    </div>
  </div>
  <div class="col-md-3">
    <div class="card h-100">
      <div class="card-body text-center">
        <div class="text-muted small mb-1">Orders</div>
        <div class="fs-5 fw-bold">1,247</div>
        <div class="small text-success"><i class="bi bi-arrow-up"></i> 8.3%</div>
        <div class="progress mt-2" style="height: 4px;">
          <div class="progress-bar bg-success" style="width: 65%;"></div>
        </div>
      </div>
    </div>
  </div>
  <div class="col-md-3">
    <div class="card h-100">
      <div class="card-body text-center">
        <div class="text-muted small mb-1">Avg Order</div>
        <div class="fs-5 fw-bold">$38.70</div>
        <div class="small text-danger"><i class="bi bi-arrow-down"></i> 2.1%</div>
        <div class="progress mt-2" style="height: 4px;">
          <div class="progress-bar bg-danger" style="width: 45%;"></div>
        </div>
      </div>
    </div>
  </div>
  <div class="col-md-3">
    <div class="card h-100">
      <div class="card-body text-center">
        <div class="text-muted small mb-1">Returns</div>
        <div class="fs-5 fw-bold">42</div>
        <div class="small text-success"><i class="bi bi-arrow-down"></i> 15%</div>
        <div class="progress mt-2" style="height: 4px;">
          <div class="progress-bar bg-success" style="width: 90%;"></div>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Year-over-Year Comparison Table

```html
<div class="table-responsive">
  <table class="table table-hover align-middle">
    <thead class="table-light">
      <tr>
        <th>Metric</th>
        <th class="text-end">2025</th>
        <th class="text-end">2026</th>
        <th class="text-end">Change</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td>Total Revenue</td>
        <td class="text-end">$385,000</td>
        <td class="text-end">$462,000</td>
        <td class="text-end text-success"><i class="bi bi-arrow-up"></i> +20%</td>
      </tr>
      <tr>
        <td>Active Subscribers</td>
        <td class="text-end">8,420</td>
        <td class="text-end">11,350</td>
        <td class="text-end text-success"><i class="bi bi-arrow-up"></i> +35%</td>
      </tr>
      <tr>
        <td>Churn Rate</td>
        <td class="text-end">5.2%</td>
        <td class="text-end">4.1%</td>
        <td class="text-end text-success"><i class="bi bi-arrow-down"></i> -1.1pp</td>
      </tr>
      <tr>
        <td>Support Tickets</td>
        <td class="text-end">2,340</td>
        <td class="text-end">2,890</td>
        <td class="text-end text-danger"><i class="bi bi-arrow-up"></i> +24%</td>
      </tr>
    </tbody>
  </table>
</div>
```

### Period Selector

```html
<div class="btn-group btn-group-sm mb-3" role="group" aria-label="Time period">
  <input type="radio" class="btn-check" name="period" id="period7d">
  <label class="btn btn-outline-secondary" for="period7d">7D</label>
  <input type="radio" class="btn-check" name="period" id="period30d" checked>
  <label class="btn btn-outline-secondary" for="period30d">30D</label>
  <input type="radio" class="btn-check" name="period" id="period90d">
  <label class="btn btn-outline-secondary" for="period90d">90D</label>
  <input type="radio" class="btn-check" name="period" id="period1y">
  <label class="btn btn-outline-secondary" for="period1y">1Y</label>
</div>
```

## Best Practices

1. Use green arrows for positive trends and red for negative trends consistently
2. Show both absolute values and percentage changes for context
3. Include period labels ("vs last month", "vs last year") for clarity
4. Use `text-success` and `text-danger` classes for trend coloring
5. Display previous period values with strikethrough for visual comparison
6. Use progress bars to show relative performance against targets
7. Include sparkline SVGs for quick visual trend assessment
8. Group related comparison metrics in a responsive grid
9. Provide period selector controls (7D, 30D, 90D, 1Y) for flexibility
10. Use `font-monospace` for numeric values to align digits
11. Show comparison direction clearly with arrow icons
12. Use badges to highlight significant percentage changes
13. Include a "No change" indicator for flat metrics

## Common Pitfalls

1. **Inconsistent trend colors**: Using green for "down" on some metrics (like returns) and red on others without context confuses users.
2. **Missing period context**: Showing "12.5% increase" without specifying the comparison period is ambiguous.
3. **No baseline reference**: Comparing numbers without showing the previous value leaves users guessing about magnitude.
4. **Overloading with too many metrics**: Showing 20 comparison cards at once reduces scannability. Group related metrics.
5. **Missing units**: "$48,250" vs "48,250 users" vs "48.25%" look similar without unit labels.
6. **No visual hierarchy**: All metrics at the same visual level prevent users from identifying the most important ones.
7. **Rounding inconsistencies**: Mixing "$48,250.00" and "$48.3k" across comparison pairs creates confusion.

## Accessibility Considerations

- Use `aria-label` on trend arrows describing the direction and magnitude
- Provide text-based trend descriptions alongside visual indicators
- Use `role="group"` on period selector button groups
- Ensure comparison tables have proper `th` scope attributes
- Announce period changes using `aria-live="polite"` regions
- Include text alternatives for sparkline SVGs
- Use `aria-describedby` to link trend indicators to their metrics

## Responsive Behavior

On mobile, comparison cards should use `col-6` for a 2-column grid. Before/after layouts should stack vertically using `col-12`. Multi-metric grids should reduce to 2 columns. Tables should use `table-responsive`. Period selector buttons should remain accessible at all sizes. Sparkline charts should maintain aspect ratio at smaller widths.
