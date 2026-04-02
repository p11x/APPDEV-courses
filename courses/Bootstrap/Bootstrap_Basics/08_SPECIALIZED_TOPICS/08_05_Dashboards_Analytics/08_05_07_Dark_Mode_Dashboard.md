---
title: "Dark Mode Dashboard"
module: "Dashboards & Analytics"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["02_02_Utilities", "04_01_Card_Component", "CSS_Custom_Properties"]
---

## Overview

Dark mode dashboards reduce eye strain and save battery on OLED screens. Bootstrap 5's dark mode support via `data-bs-theme="dark"`, combined with adapted chart colors and CSS custom properties, enables building polished dark-themed dashboards that maintain readability and visual hierarchy.

## Basic Implementation

### Dark Mode Toggle

```html
<div class="d-flex justify-content-end mb-4">
  <div class="form-check form-switch">
    <input class="form-check-input" type="checkbox" id="darkModeToggle" onchange="document.documentElement.setAttribute('data-bs-theme', this.checked ? 'dark' : 'light')">
    <label class="form-check-label" for="darkModeToggle">
      <i class="bi bi-moon-stars me-1"></i>Dark Mode
    </label>
  </div>
</div>
```

### Dark Theme Dashboard Cards

```html
<html data-bs-theme="dark">
<body class="bg-dark">
<div class="container-fluid py-4">
  <div class="row g-4 mb-4">
    <div class="col-sm-6 col-xl-3">
      <div class="card bg-dark border-secondary h-100">
        <div class="card-body">
          <div class="d-flex justify-content-between align-items-start">
            <div>
              <p class="text-secondary small mb-1">Total Revenue</p>
              <h3 class="mb-0 text-white">$48,250</h3>
            </div>
            <div class="bg-success bg-opacity-10 rounded p-2">
              <i class="bi bi-graph-up-arrow text-success fs-5"></i>
            </div>
          </div>
          <div class="mt-3">
            <span class="badge bg-success bg-opacity-10 text-success">
              <i class="bi bi-arrow-up"></i> 12.5%
            </span>
          </div>
        </div>
      </div>
    </div>
    <div class="col-sm-6 col-xl-3">
      <div class="card bg-dark border-secondary h-100">
        <div class="card-body">
          <div class="d-flex justify-content-between align-items-start">
            <div>
              <p class="text-secondary small mb-1">Active Users</p>
              <h3 class="mb-0 text-white">2,847</h3>
            </div>
            <div class="bg-primary bg-opacity-10 rounded p-2">
              <i class="bi bi-people text-primary fs-5"></i>
            </div>
          </div>
          <div class="mt-3">
            <span class="badge bg-success bg-opacity-10 text-success">
              <i class="bi bi-arrow-up"></i> 8.2%
            </span>
          </div>
        </div>
      </div>
    </div>
  </div>

  <!-- Dark Chart Area -->
  <div class="card bg-dark border-secondary mb-4">
    <div class="card-header bg-dark border-secondary d-flex justify-content-between align-items-center">
      <h5 class="mb-0 text-white">Revenue Overview</h5>
      <div class="btn-group btn-group-sm">
        <input type="radio" class="btn-check" name="period" id="d7" checked>
        <label class="btn btn-outline-secondary" for="d7">7D</label>
        <input type="radio" class="btn-check" name="period" id="d30">
        <label class="btn btn-outline-secondary" for="d30">30D</label>
      </div>
    </div>
    <div class="card-body">
      <div class="rounded d-flex align-items-center justify-content-center" style="height:300px;background:#1a1d21">
        <p class="text-secondary mb-0">Chart with dark-adapted colors</p>
      </div>
    </div>
  </div>

  <!-- Dark Table -->
  <div class="card bg-dark border-secondary">
    <div class="card-header bg-dark border-secondary"><h5 class="mb-0 text-white">Recent Transactions</h5></div>
    <div class="table-responsive">
      <table class="table table-dark table-hover mb-0">
        <thead>
          <tr>
            <th>Customer</th>
            <th>Amount</th>
            <th>Status</th>
            <th>Date</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>John Doe</td>
            <td>$299.00</td>
            <td><span class="badge bg-success">Completed</span></td>
            <td>Mar 15, 2024</td>
          </tr>
          <tr>
            <td>Alice Smith</td>
            <td>$149.00</td>
            <td><span class="badge bg-warning text-dark">Pending</span></td>
            <td>Mar 15, 2024</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</div>
</body>
</html>
```

## Advanced Variations

### Chart Color Adaptation

```html
<style>
  :root {
    --chart-line-1: #0d6efd;
    --chart-line-2: #198754;
    --chart-line-3: #ffc107;
    --chart-line-4: #dc3545;
  }
  [data-bs-theme="dark"] {
    --chart-line-1: #4dabf7;
    --chart-line-2: #51cf66;
    --chart-line-3: #ffd43b;
    --chart-line-4: #ff6b6b;
  }
</style>

<!-- Chart legends adapt to theme -->
<div class="d-flex gap-3 mb-3">
  <span class="d-flex align-items-center gap-1">
    <span class="d-inline-block rounded" style="width:12px;height:12px;background:var(--chart-line-1)"></span>
    <small>Revenue</small>
  </span>
  <span class="d-flex align-items-center gap-1">
    <span class="d-inline-block rounded" style="width:12px;height:12px;background:var(--chart-line-2)"></span>
    <small>Expenses</small>
  </span>
</div>
```

### Theme-Aware Sidebar

```html
<div class="d-flex bg-dark" data-bs-theme="dark" style="min-height:100vh">
  <div class="bg-body-secondary p-3" style="width:250px">
    <a href="#" class="text-body text-decoration-none fs-5 fw-semibold mb-4 d-block">
      <i class="bi bi-grid-1x2-fill me-2"></i>Dashboard
    </a>
    <ul class="nav flex-column gap-1">
      <li><a href="#" class="nav-link text-body bg-primary bg-opacity-10 rounded"><i class="bi bi-speedometer2 me-2"></i>Overview</a></li>
      <li><a href="#" class="nav-link text-body-secondary rounded"><i class="bi bi-bar-chart me-2"></i>Analytics</a></li>
    </ul>
  </div>
</div>
```

## Best Practices

1. Use `data-bs-theme="dark"` on the root element for Bootstrap's built-in dark mode
2. Use `table-dark` class for dark-themed tables
3. Adapt chart colors to brighter variants for dark backgrounds
4. Use `bg-dark` and `border-secondary` on cards for dark theme
5. Use CSS custom properties to define theme-aware colors
6. Provide a toggle switch for user preference
7. Respect `prefers-color-scheme: dark` OS setting
8. Use `text-secondary` for muted text in dark mode
9. Ensure chart grid lines are visible on dark backgrounds
10. Test contrast ratios meet 4.5:1 minimum

## Common Pitfalls

1. **Hardcoded colors** - Colors that look good in light mode may be invisible in dark mode. Use CSS variables.
2. **Charts not adapted** - Default chart colors are designed for light backgrounds. Brighten them for dark mode.
3. **Missing toggle** - Users should be able to choose their preferred theme.
4. **Not respecting OS preference** - Use `prefers-color-scheme` media query as the default.
5. **Borders invisible** - Default borders may disappear on dark backgrounds. Use `border-secondary`.
6. **Contrast too low** - Dark mode often has worse contrast. Test with WCAG tools.

## Accessibility Considerations

- Maintain 4.5:1 contrast ratio for text in dark mode
- Use `aria-label="Toggle dark mode"` on the theme switch
- Save the user's preference to localStorage
- Ensure focus indicators are visible in both themes
- Test with screen readers in both modes

## Responsive Behavior

Dark mode is a visual theme, not a layout change. All responsive breakpoints work identically in both light and dark modes. The toggle switch should be easily accessible at all screen sizes, typically in the top bar or settings area.
