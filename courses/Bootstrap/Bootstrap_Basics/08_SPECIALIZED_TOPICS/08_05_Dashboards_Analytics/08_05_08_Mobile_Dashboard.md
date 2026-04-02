---
title: "Mobile Dashboard"
module: "Dashboards & Analytics"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["02_01_Grid_System", "04_01_Card_Component", "04_03_Offcanvas"]
---

## Overview

Mobile dashboards must present complex data on small screens without sacrificing usability. Bootstrap 5 responsive utilities, collapsible sidebars, swipe-friendly patterns, and mobile-optimized card layouts ensure dashboards remain functional and readable on smartphones and tablets.

## Basic Implementation

### Collapsible Mobile Sidebar

```html
<!-- Mobile Header -->
<nav class="navbar navbar-light bg-white border-bottom sticky-top d-lg-none">
  <div class="container-fluid">
    <button class="btn btn-outline-secondary" data-bs-toggle="offcanvas" data-bs-target="#mobileSidebar">
      <i class="bi bi-list fs-5"></i>
    </button>
    <span class="navbar-brand mb-0">Dashboard</span>
    <button class="btn btn-outline-secondary position-relative">
      <i class="bi bi-bell"></i>
      <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">3</span>
    </button>
  </div>
</nav>

<!-- Mobile Sidebar Offcanvas -->
<div class="offcanvas offcanvas-start" tabindex="-1" id="mobileSidebar">
  <div class="offcanvas-header border-bottom">
    <h5 class="offcanvas-title">Menu</h5>
    <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body p-0">
    <div class="p-3 border-bottom">
      <div class="d-flex align-items-center">
        <div class="bg-primary text-white rounded-circle d-flex align-items-center justify-content-center me-3" style="width:40px;height:40px">JD</div>
        <div>
          <div class="fw-semibold">John Doe</div>
          <small class="text-muted">Admin</small>
        </div>
      </div>
    </div>
    <ul class="nav flex-column">
      <li><a href="#" class="nav-link active bg-primary bg-opacity-10 text-primary py-3"><i class="bi bi-speedometer2 me-3"></i>Overview</a></li>
      <li><a href="#" class="nav-link py-3"><i class="bi bi-bar-chart me-3"></i>Analytics</a></li>
      <li><a href="#" class="nav-link py-3"><i class="bi bi-people me-3"></i>Users</a></li>
      <li><a href="#" class="nav-link py-3"><i class="bi bi-gear me-3"></i>Settings</a></li>
    </ul>
  </div>
</div>
```

### Mobile-Optimized KPI Cards

```html
<div class="container-fluid py-3">
  <!-- Horizontal Scroll Cards -->
  <div class="d-flex gap-3 overflow-auto pb-3 mb-3" style="scroll-snap-type:x mandatory">
    <div class="card flex-shrink-0" style="width:160px;scroll-snap-align:start">
      <div class="card-body p-3">
        <p class="text-muted small mb-1">Revenue</p>
        <h5 class="mb-1">$48,250</h5>
        <span class="badge bg-success bg-opacity-10 text-success small">
          <i class="bi bi-arrow-up"></i> 12%
        </span>
      </div>
    </div>
    <div class="card flex-shrink-0" style="width:160px;scroll-snap-align:start">
      <div class="card-body p-3">
        <p class="text-muted small mb-1">Users</p>
        <h5 class="mb-1">2,847</h5>
        <span class="badge bg-success bg-opacity-10 text-success small">
          <i class="bi bi-arrow-up"></i> 8%
        </span>
      </div>
    </div>
    <div class="card flex-shrink-0" style="width:160px;scroll-snap-align:start">
      <div class="card-body p-3">
        <p class="text-muted small mb-1">Orders</p>
        <h5 class="mb-1">1,429</h5>
        <span class="badge bg-danger bg-opacity-10 text-danger small">
          <i class="bi bi-arrow-down"></i> 3%
        </span>
      </div>
    </div>
    <div class="card flex-shrink-0" style="width:160px;scroll-snap-align:start">
      <div class="card-body p-3">
        <p class="text-muted small mb-1">Growth</p>
        <h5 class="mb-1">23.5%</h5>
        <span class="badge bg-success bg-opacity-10 text-success small">
          <i class="bi bi-arrow-up"></i> 5%
        </span>
      </div>
    </div>
  </div>

  <!-- Mobile Chart -->
  <div class="card mb-3">
    <div class="card-header bg-white d-flex justify-content-between align-items-center">
      <h6 class="mb-0">Revenue</h6>
      <select class="form-select form-select-sm" style="width:auto">
        <option>7D</option>
        <option selected>30D</option>
        <option>90D</option>
      </select>
    </div>
    <div class="card-body">
      <div style="height:200px" class="bg-light rounded d-flex align-items-center justify-content-center">
        <p class="text-muted mb-0">Chart area</p>
      </div>
    </div>
  </div>

  <!-- Mobile List Cards -->
  <div class="card">
    <div class="card-header bg-white"><h6 class="mb-0">Recent Activity</h6></div>
    <div class="list-group list-group-flush">
      <div class="list-group-item d-flex align-items-center">
        <div class="bg-success bg-opacity-10 rounded-circle p-2 me-3 flex-shrink-0">
          <i class="bi bi-person-plus text-success"></i>
        </div>
        <div class="flex-grow-1">
          <p class="mb-0 small"><strong>New user</strong> signed up</p>
          <small class="text-muted">2 min ago</small>
        </div>
      </div>
      <div class="list-group-item d-flex align-items-center">
        <div class="bg-primary bg-opacity-10 rounded-circle p-2 me-3 flex-shrink-0">
          <i class="bi bi-credit-card text-primary"></i>
        </div>
        <div class="flex-grow-1">
          <p class="mb-0 small"><strong>Payment</strong> received</p>
          <small class="text-muted">15 min ago</small>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Swipeable Tab Navigation

```html
<ul class="nav nav-pills flex-nowrap overflow-auto pb-2 mb-3" style="scroll-snap-type:x mandatory">
  <li class="nav-item flex-shrink-0" style="scroll-snap-align:start">
    <a class="nav-link active px-3" href="#">Overview</a>
  </li>
  <li class="nav-item flex-shrink-0" style="scroll-snap-align:start">
    <a class="nav-link px-3" href="#">Revenue</a>
  </li>
  <li class="nav-item flex-shrink-0" style="scroll-snap-align:start">
    <a class="nav-link px-3" href="#">Users</a>
  </li>
  <li class="nav-item flex-shrink-0" style="scroll-snap-align:start">
    <a class="nav-link px-3" href="#">Orders</a>
  </li>
</ul>
```

### Mobile Bottom Navigation

```html
<nav class="navbar navbar-light bg-white border-top fixed-bottom d-lg-none py-2">
  <div class="container-fluid d-flex justify-content-around px-0">
    <a href="#" class="nav-link text-primary text-center">
      <i class="bi bi-speedometer2 d-block fs-5"></i>
      <small>Home</small>
    </a>
    <a href="#" class="nav-link text-muted text-center">
      <i class="bi bi-bar-chart d-block fs-5"></i>
      <small>Analytics</small>
    </a>
    <a href="#" class="nav-link text-muted text-center">
      <i class="bi bi-bell d-block fs-5"></i>
      <small>Alerts</small>
    </a>
    <a href="#" class="nav-link text-muted text-center">
      <i class="bi bi-person d-block fs-5"></i>
      <small>Profile</small>
    </a>
  </div>
</nav>
```

## Best Practices

1. Use horizontally scrollable card containers with `scroll-snap`
2. Provide a bottom navigation bar for primary actions on mobile
3. Collapse the sidebar into an offcanvas panel on mobile
4. Reduce chart heights to 200px on mobile for better screen utilization
5. Use `container-fluid` with small padding for mobile dashboards
6. Make touch targets at least 44x44px for mobile usability
7. Use `d-lg-none` for mobile-only elements and `d-none d-lg-block` for desktop-only
8. Provide a sticky mobile header with menu and notification buttons
9. Use scroll-snap on horizontally scrolling containers
10. Simplify tables to card layouts on mobile

## Common Pitfalls

1. **Not mobile-first** - Designing desktop-first and then adapting creates poor mobile experiences.
2. **Touch targets too small** - Buttons and links must be at least 44x44px on mobile.
3. **No bottom navigation** - Mobile users expect bottom nav for primary actions.
4. **Charts too tall** - Charts that take the full screen height waste space. Use 200px max on mobile.
5. **Tables not adapted** - Full tables don't work on mobile. Convert to stacked cards.
6. **No swipe support** - Horizontally scrolling content should use scroll-snap for a native feel.

## Accessibility Considerations

- Use `aria-label="Mobile navigation menu"` on the hamburger button
- Mark the bottom navigation with `<nav aria-label="Mobile bottom navigation">`
- Ensure all touch targets meet the 44x44px minimum
- Provide `aria-current="page"` on the active bottom nav item
- Use `role="tablist"` on horizontally scrollable tab navigation

## Responsive Behavior

On **mobile** (<576px), the dashboard uses a single-column layout. KPI cards scroll horizontally. The sidebar becomes offcanvas. Charts are 200px tall. On **small tablets** (576-768px), KPI cards can display in a 2x2 grid. On **tablets** (768-992px), a 2-column grid works for most content. On **desktop** (992px+), the full sidebar is visible, KPI cards display in a 4-column row, and charts expand to 300-400px height. Bottom navigation hides on desktop (`d-lg-none`).
