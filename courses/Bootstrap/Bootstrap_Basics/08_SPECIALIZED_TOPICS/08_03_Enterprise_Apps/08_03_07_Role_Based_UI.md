---
title: "Role-Based UI"
module: "Enterprise Apps"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["04_09_Badges", "04_01_Card_Component", "04_05_Forms"]
---

## Overview

Role-based UI patterns control what users see and can do based on their permissions. Bootstrap 5 badges for role indicators, conditional visibility classes, cards for admin panels, and alert components for access denied states enable building interfaces that respect organizational permission hierarchies.

## Basic Implementation

### Role Badges

```html
<span class="badge bg-danger fs-6">Super Admin</span>
<span class="badge bg-primary">Admin</span>
<span class="badge bg-secondary">Member</span>
<span class="badge bg-info">Viewer</span>
<span class="badge bg-warning text-dark">Billing</span>
```

### Permission-Based Visibility

```html
<!-- Visible only to admins -->
<div class="admin-only">
  <div class="alert alert-warning">
    <i class="bi bi-shield-exclamation me-2"></i>
    <strong>Admin Panel:</strong> Manage users, roles, and billing settings.
  </div>
  <div class="row g-3">
    <div class="col-md-4">
      <div class="card border-danger">
        <div class="card-body text-center">
          <i class="bi bi-people fs-2 text-danger mb-2"></i>
          <h6>User Management</h6>
          <a href="#" class="btn btn-outline-danger btn-sm">Manage</a>
        </div>
      </div>
    </div>
    <div class="col-md-4">
      <div class="card border-primary">
        <div class="card-body text-center">
          <i class="bi bi-key fs-2 text-primary mb-2"></i>
          <h6>Roles & Permissions</h6>
          <a href="#" class="btn btn-outline-primary btn-sm">Configure</a>
        </div>
      </div>
    </div>
    <div class="col-md-4">
      <div class="card border-success">
        <div class="card-body text-center">
          <i class="bi bi-credit-card fs-2 text-success mb-2"></i>
          <h6>Billing</h6>
          <a href="#" class="btn btn-outline-success btn-sm">View</a>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Access Denied State

```html
<div class="min-vh-50 d-flex align-items-center justify-content-center">
  <div class="text-center py-5">
    <div class="bg-danger bg-opacity-10 rounded-circle d-inline-flex align-items-center justify-content-center mb-4" style="width:100px;height:100px">
      <i class="bi bi-shield-lock text-danger display-4"></i>
    </div>
    <h3>Access Denied</h3>
    <p class="text-muted mb-4">You don't have permission to access this page. Contact your administrator if you believe this is an error.</p>
    <div class="d-flex justify-content-center gap-3">
      <a href="dashboard.html" class="btn btn-primary">Go to Dashboard</a>
      <button class="btn btn-outline-secondary">Request Access</button>
    </div>
  </div>
</div>
```

## Advanced Variations

### Role Management Table

```html
<div class="card">
  <div class="card-header bg-white"><h5 class="mb-0">Role Permissions Matrix</h5></div>
  <div class="table-responsive">
    <table class="table align-middle mb-0">
      <thead class="table-light">
        <tr>
          <th>Permission</th>
          <th class="text-center">Viewer</th>
          <th class="text-center">Member</th>
          <th class="text-center">Admin</th>
          <th class="text-center">Owner</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td>View projects</td>
          <td class="text-center"><i class="bi bi-check-circle text-success"></i></td>
          <td class="text-center"><i class="bi bi-check-circle text-success"></i></td>
          <td class="text-center"><i class="bi bi-check-circle text-success"></i></td>
          <td class="text-center"><i class="bi bi-check-circle text-success"></i></td>
        </tr>
        <tr>
          <td>Edit projects</td>
          <td class="text-center"><i class="bi bi-x-circle text-muted"></i></td>
          <td class="text-center"><i class="bi bi-check-circle text-success"></i></td>
          <td class="text-center"><i class="bi bi-check-circle text-success"></i></td>
          <td class="text-center"><i class="bi bi-check-circle text-success"></i></td>
        </tr>
        <tr>
          <td>Manage users</td>
          <td class="text-center"><i class="bi bi-x-circle text-muted"></i></td>
          <td class="text-center"><i class="bi bi-x-circle text-muted"></i></td>
          <td class="text-center"><i class="bi bi-check-circle text-success"></i></td>
          <td class="text-center"><i class="bi bi-check-circle text-success"></i></td>
        </tr>
        <tr>
          <td>Billing access</td>
          <td class="text-center"><i class="bi bi-x-circle text-muted"></i></td>
          <td class="text-center"><i class="bi bi-x-circle text-muted"></i></td>
          <td class="text-center"><i class="bi bi-x-circle text-muted"></i></td>
          <td class="text-center"><i class="bi bi-check-circle text-success"></i></td>
        </tr>
        <tr>
          <td>Delete organization</td>
          <td class="text-center"><i class="bi bi-x-circle text-muted"></i></td>
          <td class="text-center"><i class="bi bi-x-circle text-muted"></i></td>
          <td class="text-center"><i class="bi bi-x-circle text-muted"></i></td>
          <td class="text-center"><i class="bi bi-check-circle text-success"></i></td>
        </tr>
      </tbody>
    </table>
  </div>
</div>
```

### Conditional Action Buttons

```html
<!-- Full access for admin -->
<div class="d-flex gap-2 mb-3">
  <button class="btn btn-primary"><i class="bi bi-plus me-1"></i>Create Project</button>
  <button class="btn btn-outline-secondary"><i class="bi bi-download me-1"></i>Export</button>
  <button class="btn btn-outline-danger admin-only"><i class="bi bi-trash me-1"></i>Delete All</button>
</div>

<!-- Read-only for viewer -->
<div class="d-flex gap-2 mb-3 viewer-readonly">
  <button class="btn btn-outline-secondary" disabled><i class="bi bi-lock me-1"></i>Create (Read-Only)</button>
  <button class="btn btn-outline-secondary"><i class="bi bi-download me-1"></i>Export</button>
</div>
```

## Best Practices

1. Use color-coded role badges: red (owner), primary (admin), secondary (member), info (viewer)
2. Hide or disable actions the current user's role doesn't permit
3. Show an "Access Denied" page for unauthorized routes
4. Provide a "Request Access" button for users who need elevated permissions
5. Display a permissions matrix so users understand role capabilities
6. Use `disabled` attribute on buttons for insufficient permissions
7. Show a lock icon or tooltip explaining why an action is disabled
8. Keep role names consistent across the application
9. Separate admin-only sections with visual distinction (border, background)
10. Use server-side permission checks in addition to UI hiding

## Common Pitfalls

1. **UI-only permission checks** - Hiding buttons isn't enough. Always validate permissions server-side.
2. **No explanation for disabled actions** - Users don't know why buttons are disabled. Use tooltips.
3. **Role names not descriptive** - "Level 3" is unclear. Use "Admin", "Editor", "Viewer".
4. **No access denied page** - Unauthenticated/ unauthorized users see broken pages.
5. **Admin panel visible to all** - Even if buttons are disabled, the admin section shouldn't be visible to non-admins.
6. **No role documentation** - Users need to understand what each role can do. Provide a permissions matrix.

## Accessibility Considerations

- Use `aria-disabled="true"` on disabled buttons with a reason in `aria-label`
- Mark role badges with `aria-label="Role: Admin"`
- Use `role="alert"` on access denied messages
- Provide descriptive labels on permission matrix table headers
- Announce permission changes with `aria-live="polite"`

## Responsive Behavior

Role badges are inline elements that work at all breakpoints. The permissions matrix table uses `table-responsive` on mobile. Admin panel cards stack on mobile (1 column) and display side by side on desktop (3 columns). Access denied pages center content with responsive padding.
