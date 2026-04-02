---
title: "Filter Dashboard"
module: "Dashboards & Analytics"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["04_05_Forms", "04_09_Badges", "05_02_Dropdowns"]
---

## Overview

Filter dashboards let users narrow data views using date ranges, dropdowns, and multi-select controls. Bootstrap 5 form controls, badges for filter chips, dropdowns, and button groups create intuitive filtering interfaces that update dashboard data dynamically.

## Basic Implementation

### Filter Bar

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
        <label class="form-label">Status</label>
        <select class="form-select">
          <option selected>All Statuses</option>
          <option>Active</option>
          <option>Inactive</option>
          <option>Pending</option>
        </select>
      </div>
      <div class="col-md-3">
        <label class="form-label">Quick Ranges</label>
        <div class="btn-group w-100" role="group">
          <input type="radio" class="btn-check" name="range" id="rToday">
          <label class="btn btn-outline-secondary btn-sm" for="rToday">Today</label>
          <input type="radio" class="btn-check" name="range" id="rWeek" checked>
          <label class="btn btn-outline-secondary btn-sm" for="rWeek">Week</label>
          <input type="radio" class="btn-check" name="range" id="rMonth">
          <label class="btn btn-outline-secondary btn-sm" for="rMonth">Month</label>
          <input type="radio" class="btn-check" name="range" id="rQuarter">
          <label class="btn btn-outline-secondary btn-sm" for="rQuarter">Quarter</label>
        </div>
      </div>
      <div class="col-md-2">
        <button class="btn btn-primary w-100">Apply Filters</button>
      </div>
    </div>
  </div>
</div>
```

### Active Filter Chips

```html
<div class="d-flex flex-wrap gap-2 mb-4 align-items-center">
  <span class="text-muted small">Active Filters:</span>
  <span class="badge bg-light text-dark border py-2 px-3 d-flex align-items-center gap-1">
    Department: Engineering
    <button class="btn-close" style="font-size:0.6em" aria-label="Remove department filter"></button>
  </span>
  <span class="badge bg-light text-dark border py-2 px-3 d-flex align-items-center gap-1">
    Date: Jan 1 - Mar 31
    <button class="btn-close" style="font-size:0.6em" aria-label="Remove date filter"></button>
  </span>
  <span class="badge bg-light text-dark border py-2 px-3 d-flex align-items-center gap-1">
    Status: Active
    <button class="btn-close" style="font-size:0.6em" aria-label="Remove status filter"></button>
  </span>
  <a href="#" class="small text-danger ms-2">Clear All</a>
</div>
```

## Advanced Variations

### Multi-Select Filter Dropdown

```html
<div class="dropdown">
  <button class="btn btn-outline-secondary dropdown-toggle" data-bs-toggle="dropdown">
    <i class="bi bi-funnel me-1"></i>Tags
    <span class="badge bg-primary ms-1">3</span>
  </button>
  <div class="dropdown-menu p-3" style="width:280px">
    <div class="mb-2">
      <input type="search" class="form-control form-control-sm" placeholder="Search tags...">
    </div>
    <div style="max-height:200px;overflow-y:auto">
      <div class="form-check mb-2">
        <input class="form-check-input" type="checkbox" id="tag1" checked>
        <label class="form-check-label" for="tag1">Bootstrap</label>
      </div>
      <div class="form-check mb-2">
        <input class="form-check-input" type="checkbox" id="tag2" checked>
        <label class="form-check-label" for="tag2">JavaScript</label>
      </div>
      <div class="form-check mb-2">
        <input class="form-check-input" type="checkbox" id="tag3" checked>
        <label class="form-check-label" for="tag3">CSS</label>
      </div>
      <div class="form-check mb-2">
        <input class="form-check-input" type="checkbox" id="tag4">
        <label class="form-check-label" for="tag4">React</label>
      </div>
    </div>
    <div class="border-top pt-2 mt-2 d-flex justify-content-between">
      <button class="btn btn-sm btn-link text-danger p-0">Clear</button>
      <button class="btn btn-sm btn-primary">Apply</button>
    </div>
  </div>
</div>
```

### Filter Summary Card

```html
<div class="card bg-light">
  <div class="card-body py-2">
    <div class="d-flex flex-wrap align-items-center gap-3">
      <div class="small">
        <span class="text-muted">Showing:</span>
        <strong>248 results</strong>
      </div>
      <div class="vr"></div>
      <div class="small">
        <span class="text-muted">From:</span>
        <strong>Engineering, Sales</strong>
      </div>
      <div class="vr"></div>
      <div class="small">
        <span class="text-muted">Period:</span>
        <strong>Last 30 days</strong>
      </div>
      <div class="vr"></div>
      <div class="small">
        <span class="text-muted">Status:</span>
        <strong>Active</strong>
      </div>
      <div class="ms-auto">
        <a href="#" class="small text-danger">Reset All</a>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Group filter controls in a card at the top of the dashboard
2. Use date range inputs paired with quick-select buttons
3. Show active filter chips below the filter bar
4. Provide individual "X" buttons on each filter chip to remove
5. Include a "Clear All" or "Reset" option
6. Show a filter summary: result count and active filters
7. Use `btn-group` with radio inputs for time range selection
8. Provide multi-select dropdowns for tag/category filtering
9. Apply filters with an explicit button (not auto-apply) to prevent excessive requests
10. Use consistent `form-select-sm` sizing for filter dropdowns

## Common Pitfalls

1. **No active filter indication** - Users forget which filters are applied. Always show chips.
2. **Auto-apply causing lag** - Every selection triggering an API call slows the UI. Use an Apply button.
3. **No result count after filtering** - Users need to see how many results match.
4. **Filters not resettable** - Provide a way to clear all filters at once.
5. **Date picker too complex** - Simple `<input type="date">` is often sufficient. Avoid heavy date picker libraries unless necessary.
6. **Mobile filter overflow** - Filters that don't wrap on mobile create horizontal scrolling.

## Accessibility Considerations

- Label all filter controls with associated `<label>` elements
- Use `aria-label="Remove department filter"` on chip close buttons
- Announce result count changes with `aria-live="polite"`
- Group filter controls with `role="group" aria-label="Dashboard filters"`
- Mark the active time range button with `aria-pressed="true"`

## Responsive Behavior

On **mobile**, filter controls stack vertically in a single column. Filter chips wrap to multiple lines. On **tablet**, filters use a 2-column layout. On **desktop**, all filter controls display in a single row with the Apply button right-aligned.
