---
title: "Pattern Library"
difficulty: 2
category: "Advanced Development"
subcategory: "Design Systems"
prerequisites:
  - Bootstrap 5 Layout Components
  - Common UI Patterns
  - Responsive Design Patterns
---

## Overview

A pattern library documents common UI patterns and page templates built with Bootstrap components. Unlike a component catalog that documents individual components, a pattern library shows how components compose into real-world layouts: login pages, dashboard shells, content grids, checkout flows, and settings pages. Each pattern includes the complete HTML structure, responsive behavior notes, and customization guidance.

Patterns bridge the gap between design and implementation by providing ready-to-use page templates that teams can copy and adapt. They encode best practices for layout, spacing, and component composition that individual components alone don't convey.

## Basic Implementation

```html
<!-- Pattern: Dashboard Layout -->
<!DOCTYPE html>
<html lang="en">
<body>
  <div class="d-flex" id="dashboard">
    <!-- Sidebar -->
    <nav class="d-flex flex-column flex-shrink-0 p-3 bg-body-tertiary"
         style="width: 280px; min-height: 100vh;"
         aria-label="Dashboard navigation">
      <a href="/" class="d-flex align-items-center mb-3 text-decoration-none">
        <span class="fs-5 fw-semibold">Dashboard</span>
      </a>
      <hr>
      <ul class="nav nav-pills flex-column mb-auto">
        <li class="nav-item">
          <a href="#" class="nav-link active" aria-current="page">Overview</a>
        </li>
        <li class="nav-item">
          <a href="#" class="nav-link">Reports</a>
        </li>
        <li class="nav-item">
          <a href="#" class="nav-link">Settings</a>
        </li>
      </ul>
    </nav>

    <!-- Main Content -->
    <div class="flex-grow-1">
      <header class="bg-body-tertiary p-3 border-bottom">
        <div class="d-flex justify-content-between align-items-center">
          <h1 class="h4 mb-0">Overview</h1>
          <div class="d-flex align-items-center gap-2">
            <button class="btn btn-outline-secondary btn-sm">Notifications</button>
            <div class="dropdown">
              <button class="btn btn-outline-secondary btn-sm dropdown-toggle"
                      data-bs-toggle="dropdown">Account</button>
              <ul class="dropdown-menu dropdown-menu-end">
                <li><a class="dropdown-item" href="#">Profile</a></li>
                <li><a class="dropdown-item" href="#">Settings</a></li>
              </ul>
            </div>
          </div>
        </div>
      </header>

      <main class="p-4">
        <!-- KPI Cards -->
        <div class="row g-3 mb-4">
          <div class="col-12 col-sm-6 col-xl-3">
            <div class="card">
              <div class="card-body">
                <h6 class="card-subtitle text-muted">Total Users</h6>
                <p class="card-title h3 mb-0">12,345</p>
              </div>
            </div>
          </div>
          <div class="col-12 col-sm-6 col-xl-3">
            <div class="card">
              <div class="card-body">
                <h6 class="card-subtitle text-muted">Revenue</h6>
                <p class="card-title h3 mb-0">$45,678</p>
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  </div>
</body>
</html>
```

```html
<!-- Pattern: Login Page -->
<div class="container">
  <div class="row justify-content-center min-vh-100 align-items-center">
    <div class="col-12 col-sm-8 col-md-6 col-lg-4">
      <div class="card shadow">
        <div class="card-body p-4">
          <h2 class="text-center mb-4">Sign In</h2>
          <form>
            <div class="mb-3">
              <label for="email" class="form-label">Email address</label>
              <input type="email" class="form-control" id="email" required
                     autocomplete="email">
            </div>
            <div class="mb-3">
              <label for="password" class="form-label">Password</label>
              <input type="password" class="form-control" id="password" required
                     autocomplete="current-password">
            </div>
            <div class="mb-3 form-check">
              <input type="checkbox" class="form-check-input" id="remember">
              <label class="form-check-label" for="remember">Remember me</label>
            </div>
            <button type="submit" class="btn btn-primary w-100">Sign In</button>
          </form>
          <hr>
          <p class="text-center text-muted mb-0">
            <a href="#">Forgot password?</a>
          </p>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. **Provide complete HTML** - Patterns should be copy-paste ready, not partial snippets.
2. **Include responsive behavior** - Document how the pattern adapts at each breakpoint.
3. **Use real content** - Lorem ipsum is acceptable but realistic content is better for validation.
4. **Name patterns descriptively** - "Dashboard with sidebar" not "Layout Pattern 3".
5. **Show variations** - Each pattern should have 2-3 variations (with/without sidebar, condensed/expanded).
6. **Mark customizable areas** - Clearly indicate which parts teams should modify.
7. **Keep patterns modular** - Patterns should compose from documented components.
8. **Version patterns with the system** - Update patterns when underlying components change.
9. **Include accessibility notes** - Document keyboard navigation and screen reader behavior.
10. **Test responsive behavior** - Verify patterns work at all Bootstrap breakpoints.

## Common Pitfalls

1. **Outdated patterns** - Patterns referencing deprecated components or APIs.
2. **Too rigid** - Patterns that are hard to adapt to different content types.
3. **Missing responsive notes** - Desktop-only patterns that break on mobile.
4. **No real-world validation** - Patterns tested only with placeholder content.
5. **Inaccessible patterns** - Missing skip links, landmark regions, or keyboard support.

## Accessibility Considerations

Every page template must include skip navigation links, landmark regions, and proper heading hierarchy.

## Responsive Behavior

Dashboard patterns should collapse the sidebar to an offcanvas on mobile, and card grids should stack vertically on small screens.
