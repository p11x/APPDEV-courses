---
title: "Data Tables Enterprise"
module: "Enterprise Apps"
difficulty: 2
estimated_time: "30 min"
prerequisites: ["04_04_Table", "05_03_Pagination", "04_05_Forms"]
---

## Overview

Enterprise data tables display large datasets with sorting, filtering, bulk actions, and export capabilities. Bootstrap 5 provides responsive tables, form controls for filters, pagination, dropdowns for bulk actions, and badge components for status columns that together create powerful data management interfaces.

## Basic Implementation

### Sortable Table with Actions

```html
<div class="card">
  <div class="card-header bg-white d-flex flex-wrap justify-content-between align-items-center gap-2">
    <h5 class="mb-0">Employees</h5>
    <div class="d-flex gap-2 flex-wrap">
      <input type="search" class="form-control form-control-sm" placeholder="Search..." style="width:200px">
      <select class="form-select form-select-sm" style="width:150px">
        <option>All Departments</option>
        <option>Engineering</option>
        <option>Marketing</option>
        <option>Sales</option>
      </select>
      <button class="btn btn-primary btn-sm">
        <i class="bi bi-plus me-1"></i>Add Employee
      </button>
    </div>
  </div>
  <div class="table-responsive">
    <table class="table table-hover align-middle mb-0">
      <thead class="table-light">
        <tr>
          <th scope="col" style="width:40px">
            <input class="form-check-input" type="checkbox" id="selectAll">
          </th>
          <th scope="col" class="sortable" style="cursor:pointer">
            Name <i class="bi bi-arrow-down-up small text-muted"></i>
          </th>
          <th scope="col" class="sortable" style="cursor:pointer">
            Department <i class="bi bi-arrow-down-up small text-muted"></i>
          </th>
          <th scope="col" class="sortable" style="cursor:pointer">
            Status <i class="bi bi-arrow-down-up small text-muted"></i>
          </th>
          <th scope="col">Email</th>
          <th scope="col" class="sortable" style="cursor:pointer">
            Start Date <i class="bi bi-arrow-down-up small text-muted"></i>
          </th>
          <th scope="col" style="width:60px"></th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td><input class="form-check-input row-checkbox" type="checkbox"></td>
          <td>
            <div class="d-flex align-items-center">
              <div class="bg-primary text-white rounded-circle d-flex align-items-center justify-content-center me-2" style="width:36px;height:36px;font-size:0.8em">JD</div>
              <div>
                <div class="fw-semibold">John Doe</div>
                <small class="text-muted">Software Engineer</small>
              </div>
            </div>
          </td>
          <td>Engineering</td>
          <td><span class="badge bg-success">Active</span></td>
          <td><a href="mailto:john@acme.com">john@acme.com</a></td>
          <td>Jan 15, 2023</td>
          <td>
            <div class="dropdown">
              <button class="btn btn-sm btn-outline-secondary" data-bs-toggle="dropdown">
                <i class="bi bi-three-dots"></i>
              </button>
              <ul class="dropdown-menu dropdown-menu-end">
                <li><a class="dropdown-item" href="#">Edit</a></li>
                <li><a class="dropdown-item" href="#">View Details</a></li>
                <li><hr class="dropdown-divider"></li>
                <li><a class="dropdown-item text-danger" href="#">Deactivate</a></li>
              </ul>
            </div>
          </td>
        </tr>
        <tr>
          <td><input class="form-check-input row-checkbox" type="checkbox"></td>
          <td>
            <div class="d-flex align-items-center">
              <div class="bg-success text-white rounded-circle d-flex align-items-center justify-content-center me-2" style="width:36px;height:36px;font-size:0.8em">AS</div>
              <div>
                <div class="fw-semibold">Alice Smith</div>
                <small class="text-muted">Product Manager</small>
              </div>
            </div>
          </td>
          <td>Product</td>
          <td><span class="badge bg-success">Active</span></td>
          <td><a href="mailto:alice@acme.com">alice@acme.com</a></td>
          <td>Mar 22, 2022</td>
          <td>
            <div class="dropdown">
              <button class="btn btn-sm btn-outline-secondary" data-bs-toggle="dropdown">
                <i class="bi bi-three-dots"></i>
              </button>
              <ul class="dropdown-menu dropdown-menu-end">
                <li><a class="dropdown-item" href="#">Edit</a></li>
                <li><a class="dropdown-item" href="#">View Details</a></li>
              </ul>
            </div>
          </td>
        </tr>
        <tr>
          <td><input class="form-check-input row-checkbox" type="checkbox"></td>
          <td>
            <div class="d-flex align-items-center">
              <div class="bg-secondary text-white rounded-circle d-flex align-items-center justify-content-center me-2" style="width:36px;height:36px;font-size:0.8em">BJ</div>
              <div>
                <div class="fw-semibold">Bob Johnson</div>
                <small class="text-muted">Sales Rep</small>
              </div>
            </div>
          </td>
          <td>Sales</td>
          <td><span class="badge bg-warning text-dark">On Leave</span></td>
          <td><a href="mailto:bob@acme.com">bob@acme.com</a></td>
          <td>Jul 10, 2021</td>
          <td>
            <div class="dropdown">
              <button class="btn btn-sm btn-outline-secondary" data-bs-toggle="dropdown">
                <i class="bi bi-three-dots"></i>
              </button>
              <ul class="dropdown-menu dropdown-menu-end">
                <li><a class="dropdown-item" href="#">Edit</a></li>
                <li><a class="dropdown-item" href="#">View Details</a></li>
              </ul>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
  <div class="card-footer bg-white d-flex flex-wrap justify-content-between align-items-center">
    <div class="d-flex align-items-center gap-2 mb-2 mb-sm-0">
      <select class="form-select form-select-sm" style="width:auto">
        <option>10 per page</option>
        <option>25 per page</option>
        <option>50 per page</option>
      </select>
      <span class="text-muted small">Showing 1-10 of 248</span>
    </div>
    <nav aria-label="Table pagination">
      <ul class="pagination pagination-sm mb-0">
        <li class="page-item disabled"><a class="page-link" href="#">Previous</a></li>
        <li class="page-item active"><a class="page-link" href="#">1</a></li>
        <li class="page-item"><a class="page-link" href="#">2</a></li>
        <li class="page-item"><a class="page-link" href="#">3</a></li>
        <li class="page-item"><a class="page-link" href="#">Next</a></li>
      </ul>
    </nav>
  </div>
</div>
```

## Advanced Variations

### Bulk Actions Bar

```html
<div class="alert alert-primary d-flex align-items-center justify-content-between d-none" id="bulkActions">
  <span><strong>3</strong> items selected</span>
  <div class="d-flex gap-2">
    <button class="btn btn-sm btn-outline-primary">
      <i class="bi bi-download me-1"></i>Export Selected
    </button>
    <button class="btn btn-sm btn-outline-danger">
      <i class="bi bi-trash me-1"></i>Delete Selected
    </button>
  </div>
</div>
```

### Export Buttons

```html
<div class="btn-group btn-group-sm">
  <button class="btn btn-outline-secondary dropdown-toggle" data-bs-toggle="dropdown">
    <i class="bi bi-download me-1"></i>Export
  </button>
  <ul class="dropdown-menu dropdown-menu-end">
    <li><a class="dropdown-item" href="#"><i class="bi bi-file-earmark-excel me-2"></i>Export as CSV</a></li>
    <li><a class="dropdown-item" href="#"><i class="bi bi-file-earmark-pdf me-2"></i>Export as PDF</a></li>
    <li><a class="dropdown-item" href="#"><i class="bi bi-printer me-2"></i>Print</a></li>
  </ul>
</div>
```

## Best Practices

1. Use `table-hover` for row highlighting on hover
2. Provide column sort indicators with `bi-arrow-down-up` icons
3. Include checkbox column for bulk selection with a "Select All" header
4. Use `form-select-sm` for per-page pagination controls
5. Show a bulk actions bar when rows are selected
6. Offer CSV, PDF, and Print export options
7. Use `table-responsive` wrapper for horizontal scrolling on mobile
8. Include search and department filter in the header
9. Display status with color-coded badges
10. Use `align-middle` to vertically center cell content
11. Provide per-page options (10, 25, 50) for user control

## Common Pitfalls

1. **No responsive wrapper** - Tables overflow on mobile. Always use `table-responsive`.
2. **Sort state not visible** - Users can't tell which column is sorted. Use directional arrows.
3. **Bulk actions hidden** - Show the bulk action bar only when items are selected.
4. **No empty state** - When filters return no results, show a helpful message.
5. **Pagination too far from data** - Place pagination at the bottom of the table card.
6. **Missing accessibility headers** - Use `<th scope="col">` for all column headers.

## Accessibility Considerations

- Use `<th scope="col">` and `<th scope="row">` for proper table semantics
- Add `aria-sort="ascending"` or `aria-sort="descending"` on sorted columns
- Use `aria-label="Select all"` on the header checkbox
- Provide `aria-label="Actions for John Doe"` on row action dropdowns
- Use `role="table"` if the table is inside a non-semantic container
- Announce sort changes with `aria-live="polite"`

## Responsive Behavior

On **mobile**, the table wraps in `table-responsive` for horizontal scrolling. Alternatively, use a card-based layout where each row becomes a card with labeled fields. On **tablet**, the full table is visible with some column width adjustments. On **desktop**, all columns display comfortably with action dropdowns right-aligned.
