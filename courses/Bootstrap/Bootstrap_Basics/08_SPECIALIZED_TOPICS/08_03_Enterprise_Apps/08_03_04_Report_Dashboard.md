---
title: "Report Dashboard"
module: "Enterprise Apps"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["04_01_Card_Component", "04_05_Forms", "05_02_Dropdowns"]
---

## Overview

Report dashboards let users filter, visualize, and export business data. Bootstrap 5 cards, form controls, dropdowns, and grid components provide the building blocks for report filters, date range pickers, chart placeholder areas, and export options that enable data-driven decision making in enterprise applications.

## Basic Implementation

### Report Filter Bar

```html
<div class="card mb-4">
  <div class="card-body">
    <div class="row g-3 align-items-end">
      <div class="col-md-3">
        <label class="form-label">Date Range</label>
        <div class="input-group">
          <input type="date" class="form-control" value="2024-01-01">
          <span class="input-group-text">to</span>
          <input type="date" class="form-control" value="2024-03-31">
        </div>
      </div>
      <div class="col-md-2">
        <label class="form-label">Department</label>
        <select class="form-select">
          <option selected>All Departments</option>
          <option>Engineering</option>
          <option>Sales</option>
          <option>Marketing</option>
        </select>
      </div>
      <div class="col-md-2">
        <label class="form-label">Region</label>
        <select class="form-select">
          <option selected>All Regions</option>
          <option>North America</option>
          <option>Europe</option>
          <option>Asia Pacific</option>
        </select>
      </div>
      <div class="col-md-3">
        <label class="form-label">Quick Ranges</label>
        <div class="btn-group w-100" role="group">
          <input type="radio" class="btn-check" name="range" id="today">
          <label class="btn btn-outline-secondary btn-sm" for="today">Today</label>
          <input type="radio" class="btn-check" name="range" id="week" checked>
          <label class="btn btn-outline-secondary btn-sm" for="week">This Week</label>
          <input type="radio" class="btn-check" name="range" id="month">
          <label class="btn btn-outline-secondary btn-sm" for="month">This Month</label>
          <input type="radio" class="btn-check" name="range" id="quarter">
          <label class="btn btn-outline-secondary btn-sm" for="quarter">Quarter</label>
        </div>
      </div>
      <div class="col-md-2">
        <button class="btn btn-primary w-100">Apply Filters</button>
      </div>
    </div>
  </div>
</div>
```

### Report Summary Cards

```html
<div class="row g-4 mb-4">
  <div class="col-sm-6 col-xl-3">
    <div class="card h-100">
      <div class="card-body">
        <p class="text-muted small mb-1">Total Revenue</p>
        <h3 class="mb-0">$1.24M</h3>
        <span class="badge bg-success bg-opacity-10 text-success mt-2">
          <i class="bi bi-arrow-up"></i> 18.2%
        </span>
      </div>
    </div>
  </div>
  <div class="col-sm-6 col-xl-3">
    <div class="card h-100">
      <div class="card-body">
        <p class="text-muted small mb-1">New Customers</p>
        <h3 class="mb-0">1,428</h3>
        <span class="badge bg-success bg-opacity-10 text-success mt-2">
          <i class="bi bi-arrow-up"></i> 12.5%
        </span>
      </div>
    </div>
  </div>
  <div class="col-sm-6 col-xl-3">
    <div class="card h-100">
      <div class="card-body">
        <p class="text-muted small mb-1">Avg. Order Value</p>
        <h3 class="mb-0">$867</h3>
        <span class="badge bg-danger bg-opacity-10 text-danger mt-2">
          <i class="bi bi-arrow-down"></i> 3.1%
        </span>
      </div>
    </div>
  </div>
  <div class="col-sm-6 col-xl-3">
    <div class="card h-100">
      <div class="card-body">
        <p class="text-muted small mb-1">Churn Rate</p>
        <h3 class="mb-0">2.4%</h3>
        <span class="badge bg-success bg-opacity-10 text-success mt-2">
          <i class="bi bi-arrow-down"></i> 0.8%
        </span>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Chart Report Cards

```html
<div class="row g-4 mb-4">
  <div class="col-lg-8">
    <div class="card h-100">
      <div class="card-header bg-white d-flex justify-content-between align-items-center">
        <h5 class="mb-0">Revenue Trend</h5>
        <div class="dropdown">
          <button class="btn btn-sm btn-outline-secondary dropdown-toggle" data-bs-toggle="dropdown">
            <i class="bi bi-three-dots"></i>
          </button>
          <ul class="dropdown-menu dropdown-menu-end">
            <li><a class="dropdown-item" href="#"><i class="bi bi-download me-2"></i>Export PNG</a></li>
            <li><a class="dropdown-item" href="#"><i class="bi bi-file-earmark-excel me-2"></i>Export Data</a></li>
          </ul>
        </div>
      </div>
      <div class="card-body">
        <div class="bg-light rounded d-flex align-items-center justify-content-center" style="height:300px">
          <p class="text-muted mb-0">Revenue trend chart area</p>
        </div>
      </div>
    </div>
  </div>
  <div class="col-lg-4">
    <div class="card h-100">
      <div class="card-header bg-white"><h5 class="mb-0">By Region</h5></div>
      <div class="card-body">
        <div class="bg-light rounded d-flex align-items-center justify-content-center mb-3" style="height:200px">
          <p class="text-muted mb-0">Pie chart area</p>
        </div>
        <div class="d-flex align-items-center mb-2">
          <div class="bg-primary rounded me-2" style="width:12px;height:12px"></div>
          <span class="flex-grow-1 small">North America</span>
          <strong class="small">45%</strong>
        </div>
        <div class="d-flex align-items-center mb-2">
          <div class="bg-success rounded me-2" style="width:12px;height:12px"></div>
          <span class="flex-grow-1 small">Europe</span>
          <strong class="small">30%</strong>
        </div>
        <div class="d-flex align-items-center">
          <div class="bg-warning rounded me-2" style="width:12px;height:12px"></div>
          <span class="flex-grow-1 small">Asia Pacific</span>
          <strong class="small">25%</strong>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Export Options Panel

```html
<div class="card">
  <div class="card-header bg-white">
    <h5 class="mb-0">Export Report</h5>
  </div>
  <div class="card-body">
    <div class="row g-3">
      <div class="col-md-4">
        <div class="border rounded p-3 text-center">
          <i class="bi bi-file-earmark-excel fs-2 text-success mb-2"></i>
          <h6>CSV / Excel</h6>
          <p class="small text-muted">Export raw data for spreadsheet analysis</p>
          <button class="btn btn-outline-success btn-sm">Download CSV</button>
        </div>
      </div>
      <div class="col-md-4">
        <div class="border rounded p-3 text-center">
          <i class="bi bi-file-earmark-pdf fs-2 text-danger mb-2"></i>
          <h6>PDF Report</h6>
          <p class="small text-muted">Formatted report with charts and tables</p>
          <button class="btn btn-outline-danger btn-sm">Download PDF</button>
        </div>
      </div>
      <div class="col-md-4">
        <div class="border rounded p-3 text-center">
          <i class="bi bi-printer fs-2 text-primary mb-2"></i>
          <h6>Print</h6>
          <p class="small text-muted">Print-optimized version of this report</p>
          <button class="btn btn-outline-primary btn-sm" onclick="window.print()">Print Report</button>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Provide date range inputs with quick-select buttons (Today, Week, Month, Quarter)
2. Show summary KPI cards above the charts for at-a-glance metrics
3. Include trend indicators on each KPI card
4. Offer export options: CSV, PDF, and Print
5. Use chart card containers with dropdown menus for export actions
6. Filter by department, region, and other dimensions relevant to the business
7. Show a legend alongside pie/donut charts
8. Apply "Apply Filters" button so users control when data refreshes
9. Use consistent date formats across all reports
10. Provide loading states while report data is being fetched

## Common Pitfalls

1. **No date range default** - Users see an empty report. Default to a meaningful range like "This Month."
2. **Filters not applying** - Auto-apply filters can cause excessive API calls. Use an explicit "Apply" button.
3. **No export capability** - Users need to share reports. Always provide CSV/PDF export.
4. **Charts without legends** - Unlabeled chart sections are meaningless. Always include legends.
5. **Filter bar not responsive** - Filters overflow on mobile. Stack them vertically on small screens.
6. **Missing loading indicator** - Report generation can take time. Show a spinner or skeleton.

## Accessibility Considerations

- Label all filter inputs with associated `<label>` elements
- Use `aria-label="Date range from January 1 to March 31"` on the date range group
- Provide `aria-label` on export buttons (e.g., "Export as CSV")
- Use `role="img"` and `aria-label` on chart placeholder areas
- Announce filter results with `aria-live="polite"`

## Responsive Behavior

On **mobile**, filter controls stack vertically in a single column. Summary cards display in a 2-column grid. Charts take full width. Export options stack vertically. On **tablet**, filters can use a 2-column layout. On **desktop**, the full filter bar displays in a single row with summary cards in a 4-column grid.
