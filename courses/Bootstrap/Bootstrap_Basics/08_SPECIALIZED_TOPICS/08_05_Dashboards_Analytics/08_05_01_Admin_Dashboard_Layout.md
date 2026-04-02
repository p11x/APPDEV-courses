---
title: "Admin Dashboard Layout"
module: "Dashboards & Analytics"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["02_01_Grid_System", "04_01_Card_Component", "04_03_Offcanvas"]
---

## Overview

Admin dashboard layouts provide the structural foundation for data-driven applications. Bootstrap 5's grid system, navbar, offcanvas, and card components combine to create sidebars, top bars, widget grids, and responsive layouts that adapt from mobile to desktop.

## Basic Implementation

### Dashboard Shell with Sidebar

```html
<div class="d-flex" style="min-height:100vh">
  <!-- Sidebar -->
  <div class="d-none d-lg-flex flex-column bg-dark text-white p-3" style="width:250px">
    <a href="#" class="text-white text-decoration-none fs-5 fw-semibold mb-4 d-flex align-items-center">
      <i class="bi bi-grid-1x2-fill me-2"></i>Dashboard
    </a>
    <ul class="nav flex-column gap-1">
      <li><a href="#" class="nav-link text-white bg-primary bg-opacity-25 rounded"><i class="bi bi-speedometer2 me-2"></i>Overview</a></li>
      <li><a href="#" class="nav-link text-white-50 rounded"><i class="bi bi-bar-chart me-2"></i>Analytics</a></li>
      <li><a href="#" class="nav-link text-white-50 rounded"><i class="bi bi-people me-2"></i>Users</a></li>
      <li><a href="#" class="nav-link text-white-50 rounded"><i class="bi bi-folder me-2"></i>Projects</a></li>
      <li><a href="#" class="nav-link text-white-50 rounded"><i class="bi bi-gear me-2"></i>Settings</a></li>
    </ul>
  </div>

  <!-- Main Area -->
  <div class="flex-grow-1 d-flex flex-column">
    <!-- Top Bar -->
    <nav class="navbar navbar-light bg-white border-bottom px-4">
      <button class="btn btn-outline-secondary d-lg-none me-3" data-bs-toggle="offcanvas" data-bs-target="#sidebarMobile">
        <i class="bi bi-list"></i>
      </button>
      <h5 class="mb-0 d-none d-sm-block">Dashboard Overview</h5>
      <div class="d-flex align-items-center gap-3 ms-auto">
        <div class="input-group d-none d-md-flex" style="width:250px">
          <span class="input-group-text bg-light border-end-0"><i class="bi bi-search"></i></span>
          <input type="search" class="form-control bg-light border-start-0" placeholder="Search...">
        </div>
        <button class="btn btn-outline-secondary position-relative">
          <i class="bi bi-bell"></i>
          <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">3</span>
        </button>
        <div class="dropdown">
          <button class="btn btn-outline-secondary dropdown-toggle d-flex align-items-center gap-2" data-bs-toggle="dropdown">
            <div class="bg-primary text-white rounded-circle d-flex align-items-center justify-content-center" style="width:28px;height:28px;font-size:0.7em">JD</div>
            <span class="d-none d-md-inline">John</span>
          </button>
          <ul class="dropdown-menu dropdown-menu-end">
            <li><a class="dropdown-item" href="#">Profile</a></li>
            <li><a class="dropdown-item" href="#">Settings</a></li>
            <li><hr class="dropdown-divider"></li>
            <li><a class="dropdown-item" href="#">Sign Out</a></li>
          </ul>
        </div>
      </div>
    </nav>

    <!-- Content Area -->
    <div class="flex-grow-1 p-4 bg-light overflow-auto">
      <div class="row g-4">
        <div class="col-sm-6 col-xl-3">
          <div class="card h-100"><div class="card-body"><p class="text-muted small">Revenue</p><h3>$48,250</h3></div></div>
        </div>
        <div class="col-sm-6 col-xl-3">
          <div class="card h-100"><div class="card-body"><p class="text-muted small">Users</p><h3>2,847</h3></div></div>
        </div>
        <div class="col-sm-6 col-xl-3">
          <div class="card h-100"><div class="card-body"><p class="text-muted small">Orders</p><h3>1,429</h3></div></div>
        </div>
        <div class="col-sm-6 col-xl-3">
          <div class="card h-100"><div class="card-body"><p class="text-muted small">Growth</p><h3>23.5%</h3></div></div>
        </div>
        <div class="col-lg-8">
          <div class="card h-100">
            <div class="card-header bg-white"><h5 class="mb-0">Revenue Chart</h5></div>
            <div class="card-body"><div class="bg-light rounded d-flex align-items-center justify-content-center" style="height:300px"><p class="text-muted mb-0">Chart placeholder</p></div></div>
          </div>
        </div>
        <div class="col-lg-4">
          <div class="card h-100">
            <div class="card-header bg-white"><h5 class="mb-0">Recent Activity</h5></div>
            <div class="list-group list-group-flush">
              <div class="list-group-item"><small class="text-muted">2 min ago</small><br>New user signed up</div>
              <div class="list-group-item"><small class="text-muted">15 min ago</small><br>Order #1429 completed</div>
              <div class="list-group-item"><small class="text-muted">1 hr ago</small><br>Payment received $299</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Mobile Sidebar Offcanvas

```html
<div class="offcanvas offcanvas-start" tabindex="-1" id="sidebarMobile">
  <div class="offcanvas-header bg-dark text-white">
    <h5 class="offcanvas-title"><i class="bi bi-grid-1x2-fill me-2"></i>Dashboard</h5>
    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body p-0">
    <ul class="nav flex-column">
      <li><a href="#" class="nav-link active bg-primary bg-opacity-10 text-primary"><i class="bi bi-speedometer2 me-2"></i>Overview</a></li>
      <li><a href="#" class="nav-link"><i class="bi bi-bar-chart me-2"></i>Analytics</a></li>
      <li><a href="#" class="nav-link"><i class="bi bi-people me-2"></i>Users</a></li>
      <li><a href="#" class="nav-link"><i class="bi bi-gear me-2"></i>Settings</a></li>
    </ul>
  </div>
</div>
```

## Best Practices

1. Use a fixed-width sidebar (250px) on desktop with dark background
2. Switch to offcanvas on mobile to maximize content space
3. Include search, notifications, and user menu in the top bar
4. Use `min-height: 100vh` on the dashboard container
5. Apply `bg-light` to the content area for visual separation
6. Use `flex-grow-1` to fill remaining vertical space
7. Group related widgets in card components
8. Provide a visible page title in the top bar
9. Use consistent spacing with `p-4` for content area padding
10. Highlight the active navigation item

## Common Pitfalls

1. **Sidebar not responsive** - A fixed sidebar breaks on mobile. Use offcanvas.
2. **Content area not scrollable** - With a fixed sidebar, the content area must scroll independently.
3. **No mobile menu button** - Mobile users can't access the sidebar without a trigger button.
4. **Inconsistent spacing** - Different padding on cards creates visual clutter. Standardize with `g-4`.
5. **Top bar too tall** - Keep the navbar compact to maximize content area.
6. **No loading state** - Dashboard data may take time to load. Include skeleton placeholders.

## Accessibility Considerations

- Use `<nav aria-label="Dashboard sidebar">` for the sidebar
- Mark the active nav item with `aria-current="page"`
- Use `aria-label="User menu"` on the profile dropdown
- Label the offcanvas with `aria-label="Navigation menu"`
- Ensure all interactive elements are keyboard-accessible

## Responsive Behavior

On **mobile** (<992px), the sidebar becomes an offcanvas panel. The top bar shows a hamburger button. KPI cards stack in 2 columns. On **tablet** (992px+), the sidebar appears fixed. Cards can display in a 2x2 grid. On **desktop** (1200px+), KPI cards display in a 4-column row. Charts and activity panels sit side by side.
