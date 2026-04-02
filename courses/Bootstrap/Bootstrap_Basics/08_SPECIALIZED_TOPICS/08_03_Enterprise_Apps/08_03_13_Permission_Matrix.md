---
title: "Permission Matrix"
description: "Build role-permission matrices with toggle grids, bulk permission assignment, and visual permission overviews using Bootstrap 5."
difficulty: 3
estimated_time: "45 minutes"
prerequisites:
  - "Bootstrap 5 Tables"
  - "Bootstrap 5 Forms"
  - "Bootstrap 5 Modals"
  - "Bootstrap 5 Utilities"
---

## Overview

Permission matrices provide a visual grid of roles versus permissions, allowing administrators to configure access control at a glance. Bootstrap 5's table, form switch, and utility components build an interactive grid where each cell represents whether a role has a specific permission.

The matrix displays roles as columns and permissions as rows, with toggle switches at each intersection. This approach makes it easy to see the full access landscape, identify overprivileged roles, and make bulk permission changes.

## Basic Implementation

### Basic Permission Matrix Table

```html
<div class="table-responsive">
  <table class="table table-bordered text-center align-middle">
    <thead class="table-light">
      <tr>
        <th class="text-start">Permission</th>
        <th>Viewer</th>
        <th>Editor</th>
        <th>Admin</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td class="text-start">View Dashboard</td>
        <td><div class="form-check d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" checked disabled></div></td>
        <td><div class="form-check d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" checked disabled></div></td>
        <td><div class="form-check d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" checked disabled></div></td>
      </tr>
      <tr>
        <td class="text-start">Create Reports</td>
        <td><div class="form-check d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" disabled></div></td>
        <td><div class="form-check d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" checked></div></td>
        <td><div class="form-check d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" checked></div></td>
      </tr>
      <tr>
        <td class="text-start">Manage Users</td>
        <td><div class="form-check d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" disabled></div></td>
        <td><div class="form-check d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" disabled></div></td>
        <td><div class="form-check d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" checked></div></td>
      </tr>
      <tr>
        <td class="text-start">Delete Content</td>
        <td><div class="form-check d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" disabled></div></td>
        <td><div class="form-check d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" checked></div></td>
        <td><div class="form-check d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" checked></div></td>
      </tr>
    </tbody>
  </table>
</div>
```

### Interactive Permission Toggle

```html
<div class="table-responsive">
  <table class="table table-bordered text-center align-middle">
    <thead class="table-light">
      <tr>
        <th class="text-start" style="min-width: 200px;">Permission</th>
        <th>Viewer</th>
        <th>Editor</th>
        <th>Admin</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td class="text-start">
          <strong>View Dashboard</strong>
          <div class="text-muted small">Access to read-only dashboard views</div>
        </td>
        <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch" checked aria-label="Viewer: View Dashboard"></div></td>
        <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch" checked aria-label="Editor: View Dashboard"></div></td>
        <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch" checked aria-label="Admin: View Dashboard"></div></td>
      </tr>
      <tr>
        <td class="text-start">
          <strong>Edit Content</strong>
          <div class="text-muted small">Create and modify content items</div>
        </td>
        <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch" aria-label="Viewer: Edit Content"></div></td>
        <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch" checked aria-label="Editor: Edit Content"></div></td>
        <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch" checked aria-label="Admin: Edit Content"></div></td>
      </tr>
    </tbody>
  </table>
</div>
```

## Advanced Variations

### Permission Categories with Collapse

```html
<div class="accordion" id="permAccordion">
  <div class="accordion-item">
    <h2 class="accordion-header">
      <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#permContent">
        <i class="bi bi-file-earmark me-2"></i> Content Permissions
        <span class="badge bg-success ms-2">4 granted</span>
      </button>
    </h2>
    <div id="permContent" class="accordion-collapse collapse show" data-bs-parent="#permAccordion">
      <div class="accordion-body p-0">
        <div class="table-responsive">
          <table class="table table-bordered text-center align-middle mb-0">
            <thead class="table-light">
              <tr><th class="text-start">Permission</th><th>Viewer</th><th>Editor</th><th>Admin</th></tr>
            </thead>
            <tbody>
              <tr>
                <td class="text-start">View Content</td>
                <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch" checked></div></td>
                <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch" checked></div></td>
                <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch" checked></div></td>
              </tr>
              <tr>
                <td class="text-start">Create Content</td>
                <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch"></div></td>
                <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch" checked></div></td>
                <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch" checked></div></td>
              </tr>
              <tr>
                <td class="text-start">Delete Content</td>
                <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch"></div></td>
                <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch"></div></td>
                <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch" checked></div></td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
  <div class="accordion-item">
    <h2 class="accordion-header">
      <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#permAdmin">
        <i class="bi bi-gear me-2"></i> Administration Permissions
        <span class="badge bg-danger ms-2">2 granted</span>
      </button>
    </h2>
    <div id="permAdmin" class="accordion-collapse collapse" data-bs-parent="#permAccordion">
      <div class="accordion-body p-0">
        <div class="table-responsive">
          <table class="table table-bordered text-center align-middle mb-0">
            <thead class="table-light">
              <tr><th class="text-start">Permission</th><th>Viewer</th><th>Editor</th><th>Admin</th></tr>
            </thead>
            <tbody>
              <tr>
                <td class="text-start">Manage Users</td>
                <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch"></div></td>
                <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch"></div></td>
                <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch" checked></div></td>
              </tr>
              <tr>
                <td class="text-start">System Settings</td>
                <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch"></div></td>
                <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch"></div></td>
                <td><div class="form-check form-switch d-flex justify-content-center mb-0"><input class="form-check-input" type="checkbox" role="switch" checked></div></td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Bulk Column Toggle

```html
<div class="d-flex gap-2 mb-3">
  <button class="btn btn-sm btn-outline-success">
    <i class="bi bi-check-all me-1"></i>Grant All to Viewer
  </button>
  <button class="btn btn-sm btn-outline-danger">
    <i class="bi bi-x-lg me-1"></i>Revoke All from Viewer
  </button>
</div>
```

### Permission Summary Cards

```html
<div class="row g-3 mb-4">
  <div class="col-md-4">
    <div class="card">
      <div class="card-body text-center">
        <h6 class="card-title">Viewer</h6>
        <div class="display-6 fw-bold text-info">5</div>
        <div class="text-muted small">permissions granted</div>
        <div class="progress mt-2" style="height: 6px;">
          <div class="progress-bar bg-info" style="width: 36%;"></div>
        </div>
      </div>
    </div>
  </div>
  <div class="col-md-4">
    <div class="card">
      <div class="card-body text-center">
        <h6 class="card-title">Editor</h6>
        <div class="display-6 fw-bold text-warning">9</div>
        <div class="text-muted small">permissions granted</div>
        <div class="progress mt-2" style="height: 6px;">
          <div class="progress-bar bg-warning" style="width: 64%;"></div>
        </div>
      </div>
    </div>
  </div>
  <div class="col-md-4">
    <div class="card">
      <div class="card-body text-center">
        <h6 class="card-title">Admin</h6>
        <div class="display-6 fw-bold text-success">14</div>
        <div class="text-muted small">permissions granted</div>
        <div class="progress mt-2" style="height: 6px;">
          <div class="progress-bar bg-success" style="width: 100%;"></div>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Use `form-switch` with `role="switch"` for accessible toggle controls
2. Group permissions by category (Content, Administration, Billing) with accordion sections
3. Provide permission descriptions in smaller text below the permission name
4. Show permission count per role using badges in category headers
5. Include bulk actions to grant/revoke all permissions for a role
6. Use summary cards showing permission distribution per role
7. Highlight cells that change with a brief visual flash on toggle
8. Save changes automatically or clearly indicate unsaved changes
9. Use `aria-label` on every toggle describing the role-permission pair
10. Prevent removing the last admin permission to avoid lockout
11. Show a confirmation dialog before bulk permission changes
12. Use color consistently: green for granted, gray for revoked
13. Include a "Clone Role" feature to duplicate permission sets

## Common Pitfalls

1. **No permission descriptions**: Listing "Manage_X" without context leaves administrators guessing about what each permission controls.
2. **Flat permission list**: Showing 50 permissions without categorization overwhelms users. Group by functional area.
3. **No bulk operations**: Changing permissions for 14 items one at a time is tedious and error-prone.
4. **Missing save confirmation**: Toggling permissions without clear save/discard flow leads to uncertainty about whether changes took effect.
5. **No lockout prevention**: Allowing removal of all admin permissions can lock out all administrators.
6. **Hardcoded permission set**: Permissions should be loaded from the backend to reflect the actual application capabilities.
7. **No visual grouping**: All permissions at the same level without category headers make the matrix hard to scan.

## Accessibility Considerations

- Use `role="switch"` on all toggle checkboxes for screen reader announcements
- Apply `aria-label` on each toggle describing both the role and permission name
- Use `aria-expanded` on accordion permission categories
- Provide text descriptions alongside toggle indicators
- Use proper `th` scope attributes for table header associations
- Announce permission changes using `aria-live="polite"` regions
- Ensure the matrix table is navigable with keyboard using proper tab order

## Responsive Behavior

On mobile, the permission matrix table should use `table-responsive` for horizontal scrolling. Consider collapsing role columns into stacked card layouts on very small screens. Permission category accordions remain functional at all widths. Summary cards should use `col-12` on mobile for full-width display. Toggle switches should maintain minimum 44x44px touch targets.
