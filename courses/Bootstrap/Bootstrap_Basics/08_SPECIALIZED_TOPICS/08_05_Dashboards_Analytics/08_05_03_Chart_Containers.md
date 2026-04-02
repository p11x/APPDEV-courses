---
title: "Chart Containers"
module: "Dashboards & Analytics"
difficulty: 2
estimated_time: "20 min"
prerequisites: ["04_01_Card_Component", "05_02_Dropdowns"]
---

## Overview

Chart containers provide structured wrappers for data visualizations. Bootstrap 5 cards, dropdowns, and responsive utilities create consistent, interactive chart panels with headers, controls, and legends that integrate with charting libraries like Chart.js, ApexCharts, or D3.

## Basic Implementation

### Chart Card with Controls

```html
<div class="card">
  <div class="card-header bg-white d-flex justify-content-between align-items-center">
    <h5 class="mb-0">Revenue Overview</h5>
    <div class="d-flex gap-2 align-items-center">
      <div class="btn-group btn-group-sm" role="group">
        <input type="radio" class="btn-check" name="period" id="7d" checked>
        <label class="btn btn-outline-secondary" for="7d">7D</label>
        <input type="radio" class="btn-check" name="period" id="30d">
        <label class="btn btn-outline-secondary" for="30d">30D</label>
        <input type="radio" class="btn-check" name="period" id="90d">
        <label class="btn btn-outline-secondary" for="90d">90D</label>
        <input type="radio" class="btn-check" name="period" id="1y">
        <label class="btn btn-outline-secondary" for="1y">1Y</label>
      </div>
      <div class="dropdown">
        <button class="btn btn-sm btn-outline-secondary dropdown-toggle" data-bs-toggle="dropdown">
          <i class="bi bi-three-dots"></i>
        </button>
        <ul class="dropdown-menu dropdown-menu-end">
          <li><a class="dropdown-item" href="#"><i class="bi bi-download me-2"></i>Export PNG</a></li>
          <li><a class="dropdown-item" href="#"><i class="bi bi-file-earmark-excel me-2"></i>Export Data</a></li>
          <li><a class="dropdown-item" href="#"><i class="bi bi-fullscreen me-2"></i>Fullscreen</a></li>
        </ul>
      </div>
    </div>
  </div>
  <div class="card-body">
    <div class="chart-container position-relative" style="height:300px">
      <!-- Chart.js, ApexCharts, or D3 renders here -->
      <div class="bg-light rounded d-flex align-items-center justify-content-center h-100">
        <div class="text-center text-muted">
          <i class="bi bi-bar-chart-line fs-1 d-block mb-2"></i>
          <p class="mb-0">Revenue chart renders here</p>
          <small>Integrate with Chart.js or similar</small>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Responsive Chart Grid

```html
<div class="row g-4">
  <div class="col-lg-8">
    <div class="card h-100">
      <div class="card-header bg-white"><h5 class="mb-0">Revenue Trend</h5></div>
      <div class="card-body">
        <div style="height:300px" class="bg-light rounded d-flex align-items-center justify-content-center">
          <p class="text-muted mb-0">Line chart area</p>
        </div>
      </div>
    </div>
  </div>
  <div class="col-lg-4">
    <div class="card h-100">
      <div class="card-header bg-white"><h5 class="mb-0">Traffic Sources</h5></div>
      <div class="card-body">
        <div style="height:200px" class="bg-light rounded-circle mx-auto d-flex align-items-center justify-content-center" style="width:200px;height:200px">
          <p class="text-muted mb-0">Donut</p>
        </div>
        <!-- Legend -->
        <div class="mt-3">
          <div class="d-flex align-items-center mb-2">
            <div class="bg-primary rounded me-2" style="width:12px;height:12px"></div>
            <span class="flex-grow-1 small">Organic Search</span>
            <strong class="small">45%</strong>
          </div>
          <div class="d-flex align-items-center mb-2">
            <div class="bg-success rounded me-2" style="width:12px;height:12px"></div>
            <span class="flex-grow-1 small">Direct</span>
            <strong class="small">30%</strong>
          </div>
          <div class="d-flex align-items-center mb-2">
            <div class="bg-warning rounded me-2" style="width:12px;height:12px"></div>
            <span class="flex-grow-1 small">Referral</span>
            <strong class="small">15%</strong>
          </div>
          <div class="d-flex align-items-center">
            <div class="bg-info rounded me-2" style="width:12px;height:12px"></div>
            <span class="flex-grow-1 small">Social</span>
            <strong class="small">10%</strong>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Chart with Loading State

```html
<div class="card">
  <div class="card-header bg-white"><h5 class="mb-0">Sales Data</h5></div>
  <div class="card-body">
    <div style="height:300px" class="d-flex align-items-center justify-content-center">
      <div class="text-center">
        <div class="spinner-border text-primary mb-3" role="status">
          <span class="visually-hidden">Loading chart...</span>
        </div>
        <p class="text-muted mb-0">Loading chart data...</p>
      </div>
    </div>
  </div>
</div>
```

### Chart Error State

```html
<div class="card">
  <div class="card-header bg-white"><h5 class="mb-0">Sales Data</h5></div>
  <div class="card-body">
    <div style="height:300px" class="d-flex align-items-center justify-content-center">
      <div class="text-center">
        <i class="bi bi-exclamation-triangle text-warning fs-1 d-block mb-2"></i>
        <h6>Unable to load chart data</h6>
        <p class="text-muted small mb-3">There was an error fetching the data. Please try again.</p>
        <button class="btn btn-outline-primary btn-sm">Retry</button>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Wrap charts in card components for consistent styling
2. Provide time period toggles (7D, 30D, 90D, 1Y) on line/bar charts
3. Include a dropdown menu for export and fullscreen options
4. Show legends alongside donut/pie charts with color swatches
5. Use a fixed `height` on the chart container for stability
6. Provide loading states with spinners while data fetches
7. Show error states with retry buttons on data failure
8. Use `position-relative` on chart containers for tooltip positioning
9. Make charts responsive by using percentage-based widths
10. Include chart type toggle where appropriate (bar/line/area)

## Common Pitfalls

1. **Chart not responsive** - Charts that don't resize break the layout. Use responsive containers.
2. **No loading state** - Blank chart areas confuse users. Show spinners during data fetch.
3. **Missing legends** - Pie/donut charts without legends are unreadable.
4. **No error handling** - Data failures show blank areas. Provide error states with retry.
5. **Fixed pixel heights on mobile** - Charts too tall on mobile waste space. Use responsive heights.
6. **No export option** - Users need to share chart data. Provide PNG/CSV export.

## Accessibility Considerations

- Use `role="img" aria-label="Revenue chart showing monthly trends from January to June"` on chart containers
- Provide a data table alternative for screen readers
- Use `aria-live="polite"` to announce chart data updates
- Label time period toggle groups with `aria-label="Chart time period"`

## Responsive Behavior

On **mobile**, chart containers take full width with reduced height (200px). Legends stack below the chart. On **tablet**, charts can share rows in a 2-column layout. On **desktop**, full-width charts with side-by-side legends and 8+4 column layouts for chart grids.
