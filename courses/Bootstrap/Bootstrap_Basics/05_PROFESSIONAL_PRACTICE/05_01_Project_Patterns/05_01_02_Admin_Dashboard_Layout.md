---
title: Admin Dashboard Layout
category: Professional Practice
difficulty: 3
time: 60 min
tags: bootstrap5, dashboard, sidebar, navbar, cards, tables, charts, responsive
---

## Overview

An admin dashboard consolidates data visualization, navigation, and management tools into a single interface. Bootstrap 5 provides the grid system, navbar, cards, and utility classes required to build a professional dashboard with a collapsible sidebar, top navigation, stat cards, data tables, and a chart area.

## Basic Implementation

### Top Navbar and Collapsible Sidebar

```html
<nav class="navbar navbar-expand-lg navbar-dark bg-dark fixed-top">
  <div class="container-fluid">
    <button class="btn btn-outline-light me-2" type="button" data-bs-toggle="offcanvas" data-bs-target="#sidebar">
      <i class="bi bi-list"></i>
    </button>
    <a class="navbar-brand" href="#">Admin Panel</a>
    <div class="d-flex align-items-center ms-auto">
      <span class="text-white me-3">admin@example.com</span>
      <img src="avatar.png" alt="User avatar" class="rounded-circle" width="32" height="32">
    </div>
  </div>
</nav>

<div class="offcanvas offcanvas-start bg-dark text-white" tabindex="-1" id="sidebar">
  <div class="offcanvas-header">
    <h5 class="offcanvas-title">Navigation</h5>
    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body">
    <ul class="nav flex-column">
      <li class="nav-item"><a class="nav-link active text-white" href="#">Dashboard</a></li>
      <li class="nav-item"><a class="nav-link text-white-50" href="#">Users</a></li>
      <li class="nav-item"><a class="nav-link text-white-50" href="#">Orders</a></li>
      <li class="nav-item"><a class="nav-link text-white-50" href="#">Settings</a></li>
    </ul>
  </div>
</div>
```

### Stat Cards Row

Use card components with utility classes to display KPIs.

```html
<main class="container-fluid pt-5 mt-4">
  <div class="row g-3 mb-4">
    <div class="col-sm-6 col-xl-3">
      <div class="card border-0 shadow-sm">
        <div class="card-body">
          <div class="d-flex justify-content-between">
            <div>
              <p class="text-muted mb-1">Total Users</p>
              <h3 class="fw-bold mb-0">12,345</h3>
            </div>
            <div class="bg-primary bg-opacity-10 rounded-circle d-flex align-items-center justify-content-center" style="width:48px;height:48px">
              <i class="bi bi-people text-primary"></i>
            </div>
          </div>
          <small class="text-success"><i class="bi bi-arrow-up"></i> 12% vs last month</small>
        </div>
      </div>
    </div>
    <div class="col-sm-6 col-xl-3">
      <div class="card border-0 shadow-sm">
        <div class="card-body">
          <div class="d-flex justify-content-between">
            <div>
              <p class="text-muted mb-1">Revenue</p>
              <h3 class="fw-bold mb-0">$48,200</h3>
            </div>
            <div class="bg-success bg-opacity-10 rounded-circle d-flex align-items-center justify-content-center" style="width:48px;height:48px">
              <i class="bi bi-currency-dollar text-success"></i>
            </div>
          </div>
          <small class="text-success"><i class="bi bi-arrow-up"></i> 8% vs last month</small>
        </div>
      </div>
    </div>
    <div class="col-sm-6 col-xl-3">
      <div class="card border-0 shadow-sm">
        <div class="card-body">
          <div class="d-flex justify-content-between">
            <div>
              <p class="text-muted mb-1">Orders</p>
              <h3 class="fw-bold mb-0">856</h3>
            </div>
            <div class="bg-warning bg-opacity-10 rounded-circle d-flex align-items-center justify-content-center" style="width:48px;height:48px">
              <i class="bi bi-cart text-warning"></i>
            </div>
          </div>
          <small class="text-danger"><i class="bi bi-arrow-down"></i> 3% vs last month</small>
        </div>
      </div>
    </div>
    <div class="col-sm-6 col-xl-3">
      <div class="card border-0 shadow-sm">
        <div class="card-body">
          <div class="d-flex justify-content-between">
            <div>
              <p class="text-muted mb-1">Conversion</p>
              <h3 class="fw-bold mb-0">3.2%</h3>
            </div>
            <div class="bg-info bg-opacity-10 rounded-circle d-flex align-items-center justify-content-center" style="width:48px;height:48px">
              <i class="bi bi-graph-up text-info"></i>
            </div>
          </div>
          <small class="text-success"><i class="bi bi-arrow-up"></i> 1.1% vs last month</small>
        </div>
      </div>
    </div>
  </div>
```

### Data Table

```html
  <div class="row g-3">
    <div class="col-lg-8">
      <div class="card border-0 shadow-sm">
        <div class="card-header bg-white d-flex justify-content-between align-items-center">
          <h5 class="mb-0">Recent Orders</h5>
          <a href="#" class="btn btn-sm btn-outline-primary">View All</a>
        </div>
        <div class="table-responsive">
          <table class="table table-hover mb-0">
            <thead class="table-light">
              <tr>
                <th>Order ID</th>
                <th>Customer</th>
                <th>Amount</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>#10234</td>
                <td>Jane Cooper</td>
                <td>$250.00</td>
                <td><span class="badge bg-success">Completed</span></td>
              </tr>
              <tr>
                <td>#10235</td>
                <td>Robert Fox</td>
                <td>$1,200.00</td>
                <td><span class="badge bg-warning text-dark">Pending</span></td>
              </tr>
              <tr>
                <td>#10236</td>
                <td>Leslie Alexander</td>
                <td>$450.00</td>
                <td><span class="badge bg-danger">Cancelled</span></td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
    <div class="col-lg-4">
      <div class="card border-0 shadow-sm">
        <div class="card-header bg-white">
          <h5 class="mb-0">Sales Chart</h5>
        </div>
        <div class="card-body">
          <div class="ratio ratio-4x3 bg-light rounded d-flex align-items-center justify-content-center text-muted">
            Chart Placeholder
          </div>
        </div>
      </div>
    </div>
  </div>
</main>
```

## Advanced Variations

- **Persistent Sidebar on Desktop:** Use a fixed `col-auto` sidebar column instead of offcanvas for `≥992px` screens; collapse to offcanvas below that breakpoint.
- **Dark/Light Mode Toggle:** Apply `data-bs-theme="dark"` on `<html>` and store preference in `localStorage`.
- **Chart.js Integration:** Replace the placeholder with `<canvas id="salesChart">` and initialize with Chart.js for interactive line or bar charts.
- **Notification Bell Dropdown:** Add a dropdown with `data-bs-auto-close="outside"` for a notifications panel.
- **Search with Autocomplete:** Use a Bootstrap input group with a dropdown for live search results.

## Best Practices

1. Use `fixed-top` on the navbar and add `pt-5 mt-4` on `<main>` to offset fixed positioning.
2. Apply `table-responsive` wrapper around every data table to enable horizontal scrolling.
3. Keep stat card columns at `col-xl-3` so four cards fit one row on large screens.
4. Use `shadow-sm` with `border-0` on cards for a clean, modern appearance.
5. Leverage `offcanvas` for mobile sidebar navigation instead of a custom hamburger menu.
6. Use `g-3` gutters on dashboard rows for tight, consistent spacing.
7. Group related actions into card headers with `d-flex justify-content-between`.
8. Apply `table-hover` on data tables for row highlighting on interaction.
9. Use `badge` components with semantic colors (`bg-success`, `bg-warning`, `bg-danger`) for status indicators.
10. Keep the sidebar link count manageable; nest collapsible groups with `accordion` for deep menus.
11. Use `ratio` utility for chart containers to maintain aspect ratios.
12. Apply `bg-opacity-10` on stat card icon backgrounds for subtle color accents.

## Common Pitfalls

1. **No navbar offset:** Forgetting `pt-5` on `<main>` causes content to hide behind the fixed navbar.
2. **Overflowing tables:** Tables without `table-responsive` break layouts on small screens.
3. **Too many stat cards:** More than 4-6 KPIs overwhelms the dashboard; group lesser metrics below the fold.
4. **Hardcoded sidebar width:** Use `offcanvas` or flex utilities instead of fixed pixel widths.
5. **Missing `role="navigation"` on sidebar:** Screen readers need semantic landmarks.
6. **Unresponsive chart containers:** Charts without `ratio` or `overflow-hidden` can bleed outside cards.
7. **Inconsistent icon sizing:** Mixing `fs-3`, `fs-4`, and `fs-5` in the same context looks unpolished.

## Accessibility Considerations

- Add `role="navigation"` to the sidebar and `role="main"` to the content area.
- Use `aria-label="Main navigation"` on the sidebar `<nav>` element.
- Ensure table headers use `<th>` with `scope="col"` for screen reader association.
- Provide `aria-current="page"` on the active sidebar link.
- Use `aria-expanded` and `aria-controls` on the sidebar toggle button.

## Responsive Behavior

| Breakpoint | Sidebar | Stat Cards | Table + Chart |
|------------|---------|------------|---------------|
| `<576px` | Offcanvas | 1 column | Stacked |
| `≥576px` | Offcanvas | 2 columns | Stacked |
| `≥768px` | Offcanvas | 2 columns | Stacked |
| `≥992px` | Persistent column | 2 columns | 8/4 split |
| `≥1200px` | Persistent column | 4 columns | 8/4 split |

Use `col-xl-3` for stat cards and `col-lg-8` / `col-lg-4` for the table/chart split.
