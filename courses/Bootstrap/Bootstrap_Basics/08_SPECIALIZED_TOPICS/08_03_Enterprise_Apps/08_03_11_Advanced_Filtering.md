---
title: "Advanced Filtering"
description: "Build multi-criteria filter interfaces with filter chips, saved filters, and collapsible filter sidebars using Bootstrap 5."
difficulty: 2
estimated_time: "35 minutes"
prerequisites:
  - "Bootstrap 5 Forms"
  - "Bootstrap 5 Offcanvas"
  - "Bootstrap 5 Badges"
  - "Bootstrap 5 Collapse"
---

## Overview

Advanced filtering components enable users to narrow large datasets using multiple criteria simultaneously. Bootstrap 5 provides offcanvas for filter sidebars, collapse for collapsible filter groups, badges for active filter chips, and form controls for filter inputs. These components combine to create powerful filtering experiences for enterprise data tables.

The pattern supports saving filter presets, displaying active filters as removable chips, applying multiple values per filter, and clearing all filters at once. This is essential for CRM, ERP, and analytics dashboards with complex data.

## Basic Implementation

### Filter Sidebar Trigger

```html
<button class="btn btn-outline-secondary" data-bs-toggle="offcanvas" data-bs-target="#filterSidebar">
  <i class="bi bi-funnel me-1"></i> Filters
  <span class="badge bg-primary ms-1">3</span>
</button>
```

### Offcanvas Filter Sidebar

```html
<div class="offcanvas offcanvas-start" tabindex="-1" id="filterSidebar" aria-labelledby="filterSidebarLabel">
  <div class="offcanvas-header">
    <h5 class="offcanvas-title" id="filterSidebarLabel">Filters</h5>
    <button type="button" class="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
  </div>
  <div class="offcanvas-body">
    <div class="accordion accordion-flush" id="filterAccordion">
      <div class="accordion-item">
        <h2 class="accordion-header">
          <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#filterStatus">
            Status
          </button>
        </h2>
        <div id="filterStatus" class="accordion-collapse collapse show" data-bs-parent="#filterAccordion">
          <div class="accordion-body">
            <div class="form-check mb-2">
              <input class="form-check-input" type="checkbox" id="statusActive" checked>
              <label class="form-check-label" for="statusActive">Active <span class="text-muted">(142)</span></label>
            </div>
            <div class="form-check mb-2">
              <input class="form-check-input" type="checkbox" id="statusPending">
              <label class="form-check-label" for="statusPending">Pending <span class="text-muted">(38)</span></label>
            </div>
            <div class="form-check mb-2">
              <input class="form-check-input" type="checkbox" id="statusArchived">
              <label class="form-check-label" for="statusArchived">Archived <span class="text-muted">(5)</span></label>
            </div>
          </div>
        </div>
      </div>
      <div class="accordion-item">
        <h2 class="accordion-header">
          <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#filterDate">
            Date Range
          </button>
        </h2>
        <div id="filterDate" class="accordion-collapse collapse" data-bs-parent="#filterAccordion">
          <div class="accordion-body">
            <div class="mb-2">
              <label class="form-label small">From</label>
              <input type="date" class="form-control form-control-sm">
            </div>
            <div class="mb-2">
              <label class="form-label small">To</label>
              <input type="date" class="form-control form-control-sm">
            </div>
          </div>
        </div>
      </div>
      <div class="accordion-item">
        <h2 class="accordion-header">
          <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#filterCategory">
            Category
          </button>
        </h2>
        <div id="filterCategory" class="accordion-collapse collapse" data-bs-parent="#filterAccordion">
          <div class="accordion-body">
            <div class="form-check mb-2">
              <input class="form-check-input" type="checkbox" id="catElectronics">
              <label class="form-check-label" for="catElectronics">Electronics</label>
            </div>
            <div class="form-check mb-2">
              <input class="form-check-input" type="checkbox" id="catClothing">
              <label class="form-check-label" for="catClothing">Clothing</label>
            </div>
            <div class="form-check mb-2">
              <input class="form-check-input" type="checkbox" id="catBooks">
              <label class="form-check-label" for="catBooks">Books</label>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="d-flex gap-2 mt-3">
      <button class="btn btn-primary flex-grow-1">Apply Filters</button>
      <button class="btn btn-outline-secondary">Clear All</button>
    </div>
  </div>
</div>
```

### Active Filter Chips

```html
<div class="d-flex flex-wrap gap-2 mb-3">
  <span class="badge bg-primary d-flex align-items-center gap-1">
    Status: Active
    <button class="btn-close btn-close-white" style="font-size: 0.55rem;" aria-label="Remove Status: Active filter"></button>
  </span>
  <span class="badge bg-primary d-flex align-items-center gap-1">
    Category: Electronics
    <button class="btn-close btn-close-white" style="font-size: 0.55rem;" aria-label="Remove Category: Electronics filter"></button>
  </span>
  <span class="badge bg-primary d-flex align-items-center gap-1">
    Date: Mar 1 - Mar 31
    <button class="btn-close btn-close-white" style="font-size: 0.55rem;" aria-label="Remove Date filter"></button>
  </span>
  <button class="btn btn-link btn-sm p-0 text-decoration-none align-self-center">Clear all</button>
</div>
```

## Advanced Variations

### Saved Filters Dropdown

```html
<div class="btn-group me-2">
  <button class="btn btn-outline-secondary btn-sm dropdown-toggle" data-bs-toggle="dropdown">
    <i class="bi bi-bookmark me-1"></i> Saved Filters
  </button>
  <ul class="dropdown-menu">
    <li><button class="dropdown-item d-flex justify-content-between">
      My Active Orders <span class="badge bg-light text-muted">3 filters</span>
    </button></li>
    <li><button class="dropdown-item d-flex justify-content-between">
      Pending Reviews <span class="badge bg-light text-muted">2 filters</span>
    </button></li>
    <li><hr class="dropdown-divider"></li>
    <li><button class="dropdown-item"><i class="bi bi-plus me-1"></i>Save current filters</button></li>
  </ul>
</div>
```

### Inline Filter Bar

```html
<div class="card bg-light mb-3">
  <div class="card-body py-2">
    <div class="row g-2 align-items-end">
      <div class="col-md-3">
        <label class="form-label small mb-1">Search</label>
        <input type="search" class="form-control form-control-sm" placeholder="Search...">
      </div>
      <div class="col-md-2">
        <label class="form-label small mb-1">Status</label>
        <select class="form-select form-select-sm">
          <option>All</option>
          <option>Active</option>
          <option>Pending</option>
          <option>Archived</option>
        </select>
      </div>
      <div class="col-md-2">
        <label class="form-label small mb-1">Category</label>
        <select class="form-select form-select-sm">
          <option>All Categories</option>
          <option>Electronics</option>
          <option>Clothing</option>
        </select>
      </div>
      <div class="col-md-2">
        <label class="form-label small mb-1">From</label>
        <input type="date" class="form-control form-control-sm">
      </div>
      <div class="col-md-2">
        <label class="form-label small mb-1">To</label>
        <input type="date" class="form-control form-control-sm">
      </div>
      <div class="col-md-1">
        <button class="btn btn-primary btn-sm w-100"><i class="bi bi-funnel"></i></button>
      </div>
    </div>
  </div>
</div>
```

### Multi-Select Filter with Tags

```html
<div class="mb-3">
  <label class="form-label">Tags</label>
  <select class="form-select" multiple aria-label="Filter by tags">
    <option selected>Urgent</option>
    <option selected>Q1-2026</option>
    <option>High Priority</option>
    <option>Client-Facing</option>
    <option>Internal</option>
  </select>
  <div class="form-text">Hold Ctrl/Cmd to select multiple tags.</div>
</div>
```

### Filter Result Count

```html
<div class="d-flex justify-content-between align-items-center mb-3">
  <div>
    <span class="text-muted">Showing</span> <strong>42</strong> <span class="text-muted">of</span> <strong>1,247</strong> <span class="text-muted">results</span>
  </div>
  <div>
    <label class="form-label small mb-0 me-1">Sort by:</label>
    <select class="form-select form-select-sm d-inline-block" style="width: auto;">
      <option>Newest First</option>
      <option>Oldest First</option>
      <option>Name A-Z</option>
      <option>Name Z-A</option>
    </select>
  </div>
</div>
```

## Best Practices

1. Show active filter count on the filter trigger button
2. Display filters as removable chips above the results
3. Support "Save current filters" for frequently used filter combinations
4. Use accordion groups to organize filter categories in the sidebar
5. Show result counts next to filter options (e.g., "Active (142)")
6. Provide a "Clear all" action to reset all filters at once
7. Use `offcanvas` for filter sidebars on mobile and desktop
8. Implement debounced search to avoid excessive API calls
9. Use `form-select-sm` and `form-control-sm` for compact filter inputs
10. Persist filter state in the URL for shareable links
11. Include a sort control alongside filter results
12. Show "No results" messaging when filters produce empty results
13. Support keyboard navigation through filter options

## Common Pitfalls

1. **No active filter visibility**: Users apply filters and forget about them, then wonder why results are limited. Always show active filter chips.
2. **Missing result count per option**: Without counts, users cannot estimate how many results a filter will return.
3. **No clear all functionality**: Removing 10 individual filter chips one by one is frustrating.
4. **Filters not reflected in URL**: Filtered views that cannot be bookmarked or shared reduce collaboration efficiency.
5. **No empty state handling**: When filters produce zero results, showing a blank table without guidance is poor UX.
6. **Overlapping filter groups**: Allowing contradictory filters (e.g., Status: Active AND Status: Archived) without validation creates confusion.
7. **Slow filter application**: Applying filters without a loading indicator causes users to click repeatedly.

## Accessibility Considerations

- Use `aria-label` on all filter controls describing their purpose
- Implement `aria-expanded` on accordion filter groups
- Use `aria-live="polite"` on the result count to announce filter changes
- Ensure offcanvas filter sidebar traps focus when open
- Provide `aria-label` on filter chip remove buttons
- Use `role="search"` on search filter inputs
- Announce when filters are applied or cleared using `aria-live` regions

## Responsive Behavior

On mobile, the inline filter bar should collapse into the offcanvas sidebar. Filter chips should wrap naturally using `d-flex flex-wrap`. The offcanvas should use `offcanvas-start` for left-side placement. The saved filters dropdown should remain functional at all widths. Multi-select inputs should become scrollable on small screens. Result count and sort controls should stack vertically on narrow screens.
