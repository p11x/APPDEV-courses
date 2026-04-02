---
title: "Enterprise Layout"
module: "Enterprise Apps"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["02_01_Grid_System", "04_03_Offcanvas", "04_06_Nav_And_Tabs"]
---

## Overview

Enterprise applications require structured layouts with sidebar navigation, header bars, breadcrumbs, content areas, and footers. Bootstrap 5 provides offcanvas for mobile sidebars, navbar for headers, breadcrumbs for navigation hierarchy, and grid utilities for content areas that form a consistent application shell.

## Basic Implementation

### Sidebar Navigation

```html
<div class="d-flex">
  <!-- Sidebar -->
  <div class="d-none d-lg-flex flex-column flex-shrink-0 p-3 bg-dark text-white" style="width:260px;min-height:100vh">
    <a href="#" class="d-flex align-items-center mb-3 text-white text-decoration-none">
      <i class="bi bi-building me-2 fs-4"></i>
      <span class="fs-5 fw-semibold">Acme Corp</span>
    </a>
    <hr>
    <ul class="nav nav-pills flex-column mb-auto">
      <li class="nav-item">
        <a href="#" class="nav-link active text-white" aria-current="page">
          <i class="bi bi-speedometer2 me-2"></i>Dashboard
        </a>
      </li>
      <li>
        <a href="#" class="nav-link text-white-50">
          <i class="bi bi-people me-2"></i>Users
        </a>
      </li>
      <li>
        <a href="#" class="nav-link text-white-50">
          <i class="bi bi-folder me-2"></i>Projects
        </a>
      </li>
      <li>
        <a href="#" class="nav-link text-white-50">
          <i class="bi bi-bar-chart me-2"></i>Reports
        </a>
      </li>
      <li>
        <a href="#" class="nav-link text-white-50">
          <i class="bi bi-gear me-2"></i>Settings
        </a>
      </li>
    </ul>
    <hr>
    <div class="dropdown">
      <a href="#" class="d-flex align-items-center text-white text-decoration-none dropdown-toggle" data-bs-toggle="dropdown">
        <div class="bg-primary rounded-circle d-flex align-items-center justify-content-center me-2" style="width:32px;height:32px">JD</div>
        <span>John Doe</span>
      </a>
      <ul class="dropdown-menu dropdown-menu-dark">
        <li><a class="dropdown-item" href="#">Profile</a></li>
        <li><a class="dropdown-item" href="#">Settings</a></li>
        <li><hr class="dropdown-divider"></li>
        <li><a class="dropdown-item" href="#">Sign Out</a></li>
      </ul>
    </div>
  </div>

  <!-- Main Content -->
  <div class="flex-grow-1">
    <!-- Top Header Bar -->
    <nav class="navbar navbar-expand-lg navbar-light bg-white border-bottom px-4">
      <button class="btn btn-outline-secondary d-lg-none me-3" data-bs-toggle="offcanvas" data-bs-target="#sidebarOffcanvas">
        <i class="bi bi-list"></i>
      </button>
      <form class="d-none d-md-flex me-auto">
        <div class="input-group" style="width:300px">
          <span class="input-group-text bg-light border-end-0"><i class="bi bi-search"></i></span>
          <input type="search" class="form-control bg-light border-start-0" placeholder="Search...">
        </div>
      </form>
      <div class="d-flex align-items-center gap-3">
        <button class="btn btn-outline-secondary position-relative">
          <i class="bi bi-bell"></i>
          <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">3</span>
        </button>
        <button class="btn btn-outline-secondary">
          <i class="bi bi-question-circle"></i>
        </button>
      </div>
    </nav>

    <!-- Breadcrumb -->
    <div class="px-4 py-3 bg-light border-bottom">
      <nav aria-label="breadcrumb">
        <ol class="breadcrumb mb-0">
          <li class="breadcrumb-item"><a href="#">Home</a></li>
          <li class="breadcrumb-item"><a href="#">Projects</a></li>
          <li class="breadcrumb-item active">Project Alpha</li>
        </ol>
      </nav>
    </div>

    <!-- Page Content -->
    <div class="p-4">
      <h1 class="h3 mb-4">Dashboard</h1>
      <p>Page content goes here...</p>
    </div>

    <!-- Footer -->
    <footer class="border-top p-4 mt-auto bg-light">
      <div class="d-flex flex-wrap justify-content-between align-items-center">
        <p class="col-md-4 mb-0 text-muted">&copy; 2024 Acme Corp</p>
        <ul class="nav col-md-4 justify-content-end">
          <li class="nav-item"><a href="#" class="nav-link px-2 text-muted">Privacy</a></li>
          <li class="nav-item"><a href="#" class="nav-link px-2 text-muted">Terms</a></li>
          <li class="nav-item"><a href="#" class="nav-link px-2 text-muted">Support</a></li>
        </ul>
      </div>
    </footer>
  </div>
</div>
```

## Advanced Variations

### Mobile Sidebar Offcanvas

```html
<div class="offcanvas offcanvas-start" tabindex="-1" id="sidebarOffcanvas">
  <div class="offcanvas-header bg-dark text-white">
    <h5 class="offcanvas-title">Acme Corp</h5>
    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body p-0">
    <ul class="nav flex-column">
      <li class="nav-item">
        <a href="#" class="nav-link active bg-primary text-white">
          <i class="bi bi-speedometer2 me-2"></i>Dashboard
        </a>
      </li>
      <li class="nav-item">
        <a href="#" class="nav-link">
          <i class="bi bi-people me-2"></i>Users
        </a>
      </li>
      <li class="nav-item">
        <a href="#" class="nav-link">
          <i class="bi bi-folder me-2"></i>Projects
        </a>
      </li>
    </ul>
  </div>
</div>
```

### Collapsible Sidebar

```html
<div class="d-flex">
  <div id="sidebar" class="bg-dark text-white p-3" style="width:260px;transition:width 0.3s">
    <button class="btn btn-outline-light btn-sm mb-3" onclick="document.getElementById('sidebar').style.width='60px'">
      <i class="bi bi-chevron-left"></i>
    </button>
    <ul class="nav flex-column">
      <li class="nav-item">
        <a href="#" class="nav-link text-white px-2" title="Dashboard">
          <i class="bi bi-speedometer2"></i>
          <span class="sidebar-label ms-2">Dashboard</span>
        </a>
      </li>
    </ul>
  </div>
</div>
```

## Best Practices

1. Use a fixed sidebar (260px) on desktop for persistent navigation
2. Switch to offcanvas on mobile to save screen space
3. Include breadcrumbs on all sub-pages for navigation context
4. Provide a search bar in the header for global search
5. Show notification and help icons in the top header bar
6. Use a user dropdown in the sidebar footer for profile/settings
7. Highlight the active nav item with `active` class and `aria-current="page"`
8. Include a footer with legal links and copyright
9. Use consistent spacing with `p-4` for content areas
10. Make the sidebar collapsible to maximize content space

## Common Pitfalls

1. **Sidebar not responsive** - A fixed sidebar breaks on mobile. Always provide an offcanvas alternative.
2. **No breadcrumbs** - Users lose context in deep navigation hierarchies.
3. **Active nav not highlighted** - Users can't tell where they are. Use `active` class.
4. **Header too tall** - Enterprise headers with many items waste vertical space. Keep them compact.
5. **No search** - Enterprise users navigate by search. Include a global search bar.
6. **Footer not sticky** - On short pages, the footer floats. Use `min-height: 100vh` on the content area.

## Accessibility Considerations

- Use `<nav aria-label="Main sidebar">` for the sidebar navigation
- Mark the active nav item with `aria-current="page"`
- Use `aria-label="Search"` on the search input
- Provide `aria-label="User menu"` on the user dropdown
- Ensure sidebar links have sufficient contrast (4.5:1 minimum)
- Use `role="navigation"` on breadcrumb elements

## Responsive Behavior

On **mobile** (<992px), the sidebar becomes an offcanvas panel triggered by a hamburger button. The top bar hides the search bar and shows only the logo and menu button. On **tablet** (992px+), the sidebar appears fixed. The search bar is visible in the header. On **desktop** (1200px+), the full sidebar with labels and icons is visible. Content area fills the remaining space with appropriate padding.
