---
title: "Dashboard Widgets"
description: "Build draggable widget grids, configurable dashboard cards, and collapsible widget layouts using Bootstrap 5."
difficulty: 3
estimated_time: "45 minutes"
prerequisites:
  - "Bootstrap 5 Cards"
  - "Bootstrap 5 Grid"
  - "Bootstrap 5 Collapse"
  - "Bootstrap 5 Dropdowns"
---

## Overview

Dashboard widgets are modular, configurable components that users can arrange, resize, and customize to create personalized dashboard layouts. Bootstrap 5's card, grid, collapse, and dropdown components build the widget container and controls, while JavaScript libraries like GridStack.js or Sortable.js handle drag-and-drop positioning.

Each widget includes a header with title and action menu, a collapsible body, configuration options, and drag handle. The widget system supports adding, removing, and rearranging widgets with persistent layout storage.

## Basic Implementation

### Widget Card Structure

```html
<div class="card" data-widget="revenue">
  <div class="card-header d-flex justify-content-between align-items-center py-2">
    <div class="d-flex align-items-center">
      <i class="bi bi-grip-vertical text-muted me-2" style="cursor: grab;"></i>
      <strong class="small">Revenue Overview</strong>
    </div>
    <div class="dropdown">
      <button class="btn btn-sm btn-link text-muted p-0" data-bs-toggle="dropdown" aria-label="Widget options">
        <i class="bi bi-three-dots-vertical"></i>
      </button>
      <ul class="dropdown-menu dropdown-menu-end">
        <li><button class="dropdown-item" data-action="refresh"><i class="bi bi-arrow-clockwise me-1"></i>Refresh</button></li>
        <li><button class="dropdown-item" data-action="configure"><i class="bi bi-gear me-1"></i>Configure</button></li>
        <li><hr class="dropdown-divider"></li>
        <li><button class="dropdown-item text-danger" data-action="remove"><i class="bi bi-trash me-1"></i>Remove</button></li>
      </ul>
    </div>
  </div>
  <div class="card-body">
    <div class="display-6 fw-bold">$48,250</div>
    <div class="text-success small"><i class="bi bi-arrow-up"></i> 12.5% vs last month</div>
  </div>
</div>
```

### Collapsible Widget

```html
<div class="card" data-widget="activity">
  <div class="card-header d-flex justify-content-between align-items-center py-2">
    <div class="d-flex align-items-center">
      <i class="bi bi-grip-vertical text-muted me-2" style="cursor: grab;"></i>
      <strong class="small">Recent Activity</strong>
    </div>
    <button class="btn btn-sm btn-link text-muted p-0" data-bs-toggle="collapse" data-bs-target="#activityBody" aria-expanded="true">
      <i class="bi bi-chevron-up"></i>
    </button>
  </div>
  <div class="collapse show" id="activityBody">
    <div class="card-body">
      <ul class="list-group list-group-flush">
        <li class="list-group-item px-0 d-flex justify-content-between">
          <span>New user signup</span>
          <small class="text-muted">2m ago</small>
        </li>
        <li class="list-group-item px-0 d-flex justify-content-between">
          <span>Order #1247 completed</span>
          <small class="text-muted">15m ago</small>
        </li>
        <li class="list-group-item px-0 d-flex justify-content-between">
          <span>Payment received</span>
          <small class="text-muted">1h ago</small>
        </li>
      </ul>
    </div>
  </div>
</div>
```

### Widget Grid Layout

```html
<div class="row g-3">
  <div class="col-md-4">
    <div class="card">
      <div class="card-header py-2 d-flex justify-content-between align-items-center">
        <strong class="small">Total Users</strong>
        <i class="bi bi-three-dots-vertical text-muted"></i>
      </div>
      <div class="card-body text-center">
        <div class="display-6 fw-bold">12,847</div>
        <span class="badge bg-success">+8.3%</span>
      </div>
    </div>
  </div>
  <div class="col-md-4">
    <div class="card">
      <div class="card-header py-2 d-flex justify-content-between align-items-center">
        <strong class="small">Revenue</strong>
        <i class="bi bi-three-dots-vertical text-muted"></i>
      </div>
      <div class="card-body text-center">
        <div class="display-6 fw-bold">$48.2K</div>
        <span class="badge bg-success">+12.5%</span>
      </div>
    </div>
  </div>
  <div class="col-md-4">
    <div class="card">
      <div class="card-header py-2 d-flex justify-content-between align-items-center">
        <strong class="small">Conversion</strong>
        <i class="bi bi-three-dots-vertical text-muted"></i>
      </div>
      <div class="card-body text-center">
        <div class="display-6 fw-bold">3.8%</div>
        <span class="badge bg-danger">-0.4%</span>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Add Widget Modal

```html
<div class="modal fade" id="addWidgetModal" tabindex="-1">
  <div class="modal-dialog modal-lg">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Add Widget</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <div class="row g-3">
          <div class="col-md-4">
            <div class="card h-100 border-dashed text-center p-3" style="cursor: pointer; border-style: dashed;">
              <i class="bi bi-graph-up fs-2 text-primary mb-2"></i>
              <strong class="small">Revenue Chart</strong>
              <p class="text-muted small mb-0">Line chart showing revenue trends</p>
            </div>
          </div>
          <div class="col-md-4">
            <div class="card h-100 border-dashed text-center p-3" style="cursor: pointer; border-style: dashed;">
              <i class="bi bi-people fs-2 text-success mb-2"></i>
              <strong class="small">User Stats</strong>
              <p class="text-muted small mb-0">Active users and signup metrics</p>
            </div>
          </div>
          <div class="col-md-4">
            <div class="card h-100 border-dashed text-center p-3" style="cursor: pointer; border-style: dashed;">
              <i class="bi bi-list-check fs-2 text-warning mb-2"></i>
              <strong class="small">Task List</strong>
              <p class="text-muted small mb-0">Pending tasks and deadlines</p>
            </div>
          </div>
          <div class="col-md-4">
            <div class="card h-100 border-dashed text-center p-3" style="cursor: pointer; border-style: dashed;">
              <i class="bi bi-bell fs-2 text-danger mb-2"></i>
              <strong class="small">Alerts</strong>
              <p class="text-muted small mb-0">System alerts and notifications</p>
            </div>
          </div>
          <div class="col-md-4">
            <div class="card h-100 border-dashed text-center p-3" style="cursor: pointer; border-style: dashed;">
              <i class="bi bi-calendar-event fs-2 text-info mb-2"></i>
              <strong class="small">Calendar</strong>
              <p class="text-muted small mb-0">Upcoming events and deadlines</p>
            </div>
          </div>
          <div class="col-md-4">
            <div class="card h-100 border-dashed text-center p-3" style="cursor: pointer; border-style: dashed;">
              <i class="bi bi-table fs-2 text-secondary mb-2"></i>
              <strong class="small">Data Table</strong>
              <p class="text-muted small mb-0">Sortable data table view</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Widget Configuration Panel

```html
<div class="offcanvas offcanvas-end" tabindex="-1" id="widgetConfig">
  <div class="offcanvas-header">
    <h5 class="offcanvas-title">Widget Settings</h5>
    <button type="button" class="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
  </div>
  <div class="offcanvas-body">
    <div class="mb-3">
      <label class="form-label">Widget Title</label>
      <input type="text" class="form-control" value="Revenue Overview">
    </div>
    <div class="mb-3">
      <label class="form-label">Refresh Interval</label>
      <select class="form-select">
        <option>Manual</option>
        <option selected>Every 5 minutes</option>
        <option>Every 15 minutes</option>
        <option>Every hour</option>
      </select>
    </div>
    <div class="mb-3">
      <label class="form-label">Date Range</label>
      <select class="form-select">
        <option>Last 7 days</option>
        <option selected>Last 30 days</option>
        <option>Last 90 days</option>
        <option>Custom</option>
      </select>
    </div>
    <div class="form-check form-switch mb-3">
      <input class="form-check-input" type="checkbox" id="showLegend" checked>
      <label class="form-check-label" for="showLegend">Show legend</label>
    </div>
    <button class="btn btn-primary w-100">Save Settings</button>
  </div>
</div>
```

### Add Widget Floating Button

```html
<button class="btn btn-primary rounded-circle shadow-lg position-fixed" style="bottom: 24px; right: 24px; width: 56px; height: 56px;" data-bs-toggle="modal" data-bs-target="#addWidgetModal" aria-label="Add widget">
  <i class="bi bi-plus-lg fs-5"></i>
</button>
```

## Best Practices

1. Use card components as the standard widget container
2. Include a dropdown menu with refresh, configure, and remove actions
3. Provide a drag handle icon for draggable widgets
4. Make widgets collapsible to save dashboard space
5. Use consistent widget header styling across all widgets
6. Include an "Add Widget" modal with available widget types
7. Store widget layout preferences in localStorage or user settings
8. Show a loading skeleton during widget data fetch
9. Use `position-fixed` for the floating add widget button
10. Support widget-specific configuration through an offcanvas panel
11. Allow full-screen expansion for chart widgets
12. Use `data-widget` attributes to identify widget types for persistence
13. Implement auto-refresh with configurable intervals

## Common Pitfalls

1. **No layout persistence**: Widget arrangements that reset on page reload frustrate users. Save layout to localStorage or API.
2. **Missing loading states**: Widgets showing blank content while fetching data look broken. Use skeleton screens.
3. **No widget removal**: Users cannot customize their dashboard if they cannot remove unwanted widgets.
4. **Hardcoded widget set**: Not allowing users to add new widgets limits dashboard personalization.
5. **No collapse option**: Tall widgets that cannot be collapsed waste screen space and hide other widgets.
6. **Missing refresh capability**: Widgets with stale data and no manual refresh option reduce trust in the dashboard.
7. **No error handling**: Widget data fetch failures showing raw errors instead of friendly messages degrade the experience.

## Accessibility Considerations

- Use `aria-label` on widget action buttons (menu, collapse, drag handle)
- Implement `aria-expanded` on collapsible widgets
- Use `role="region"` with `aria-label` on each widget container
- Ensure drag handles are keyboard accessible with arrow key reordering
- Provide `aria-live="polite"` on widget content updates
- Use proper heading hierarchy within widget titles
- Announce widget addition/removal using `aria-live` regions

## Responsive Behavior

On mobile, widgets should stack vertically using `col-12`. The widget grid should use `col-md-4` for 3-column layout on medium screens. The add widget FAB should remain fixed at the bottom-right corner. Widget menus should use `dropdown-menu-end`. Collapsible widgets should default to collapsed state on mobile to save space. The widget configuration offcanvas should use full width on mobile.
