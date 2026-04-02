---
title: "User Management UI"
module: "SaaS Applications"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["04_04_Table", "04_07_Modal", "04_09_Badges"]
---

## Overview

User management interfaces let administrators invite, view, edit, and remove team members. Bootstrap 5 provides tables for user lists, badges for roles, modals for invite flows, and form controls for permission management. This pattern is essential for SaaS applications with multi-user teams.

## Basic Implementation

### User Table

```html
<div class="card">
  <div class="card-header bg-white d-flex flex-wrap justify-content-between align-items-center gap-2">
    <h5 class="mb-0">Team Members (8)</h5>
    <div class="d-flex gap-2">
      <input type="search" class="form-control form-control-sm" placeholder="Search users..." style="width:200px">
      <button class="btn btn-primary btn-sm" data-bs-toggle="modal" data-bs-target="#inviteModal">
        <i class="bi bi-person-plus me-1"></i>Invite
      </button>
    </div>
  </div>
  <div class="table-responsive">
    <table class="table table-hover align-middle mb-0">
      <thead class="table-light">
        <tr>
          <th>User</th>
          <th>Role</th>
          <th>Status</th>
          <th>Last Active</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td>
            <div class="d-flex align-items-center">
              <div class="bg-primary text-white rounded-circle d-flex align-items-center justify-content-center me-3" style="width:40px;height:40px">JD</div>
              <div>
                <div class="fw-semibold">John Doe</div>
                <small class="text-muted">john@example.com</small>
              </div>
            </div>
          </td>
          <td><span class="badge bg-danger">Owner</span></td>
          <td><span class="badge bg-success">Active</span></td>
          <td class="text-muted">Just now</td>
          <td>
            <div class="dropdown">
              <button class="btn btn-sm btn-outline-secondary" data-bs-toggle="dropdown">
                <i class="bi bi-three-dots-vertical"></i>
              </button>
              <ul class="dropdown-menu dropdown-menu-end">
                <li><a class="dropdown-item" href="#">Edit</a></li>
                <li><a class="dropdown-item" href="#">View Activity</a></li>
                <li><hr class="dropdown-divider"></li>
                <li><a class="dropdown-item text-danger" href="#">Remove</a></li>
              </ul>
            </div>
          </td>
        </tr>
        <tr>
          <td>
            <div class="d-flex align-items-center">
              <div class="bg-success text-white rounded-circle d-flex align-items-center justify-content-center me-3" style="width:40px;height:40px">AS</div>
              <div>
                <div class="fw-semibold">Alice Smith</div>
                <small class="text-muted">alice@example.com</small>
              </div>
            </div>
          </td>
          <td><span class="badge bg-primary">Admin</span></td>
          <td><span class="badge bg-success">Active</span></td>
          <td class="text-muted">2 hours ago</td>
          <td>
            <div class="dropdown">
              <button class="btn btn-sm btn-outline-secondary" data-bs-toggle="dropdown">
                <i class="bi bi-three-dots-vertical"></i>
              </button>
              <ul class="dropdown-menu dropdown-menu-end">
                <li><a class="dropdown-item" href="#">Edit</a></li>
                <li><a class="dropdown-item" href="#">Change Role</a></li>
                <li><hr class="dropdown-divider"></li>
                <li><a class="dropdown-item text-danger" href="#">Remove</a></li>
              </ul>
            </div>
          </td>
        </tr>
        <tr>
          <td>
            <div class="d-flex align-items-center">
              <div class="bg-warning text-white rounded-circle d-flex align-items-center justify-content-center me-3" style="width:40px;height:40px">BJ</div>
              <div>
                <div class="fw-semibold">Bob Johnson</div>
                <small class="text-muted">bob@example.com</small>
              </div>
            </div>
          </td>
          <td><span class="badge bg-secondary">Member</span></td>
          <td><span class="badge bg-warning text-dark">Invited</span></td>
          <td class="text-muted">-</td>
          <td>
            <div class="dropdown">
              <button class="btn btn-sm btn-outline-secondary" data-bs-toggle="dropdown">
                <i class="bi bi-three-dots-vertical"></i>
              </button>
              <ul class="dropdown-menu dropdown-menu-end">
                <li><a class="dropdown-item" href="#">Resend Invite</a></li>
                <li><hr class="dropdown-divider"></li>
                <li><a class="dropdown-item text-danger" href="#">Revoke</a></li>
              </ul>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</div>
```

## Advanced Variations

### Invite User Modal

```html
<div class="modal fade" id="inviteModal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Invite Team Member</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>
      <div class="modal-body">
        <div class="mb-3">
          <label for="inviteEmail" class="form-label">Email Address</label>
          <input type="email" class="form-control" id="inviteEmail" placeholder="colleague@example.com">
        </div>
        <div class="mb-3">
          <label for="inviteRole" class="form-label">Role</label>
          <select class="form-select" id="inviteRole">
            <option value="member">Member - Can view and edit content</option>
            <option value="admin">Admin - Can manage team and settings</option>
            <option value="viewer">Viewer - Read-only access</option>
          </select>
        </div>
        <div class="mb-3">
          <label class="form-label">Personal Message (optional)</label>
          <textarea class="form-control" rows="3" placeholder="Add a welcome message..."></textarea>
        </div>
      </div>
      <div class="modal-footer">
        <button class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button class="btn btn-primary">Send Invitation</button>
      </div>
    </div>
  </div>
</div>
```

### Role Badges with Descriptions

```html
<div class="mb-4">
  <h6>Roles & Permissions</h6>
  <div class="row g-3">
    <div class="col-md-4">
      <div class="border rounded p-3">
        <span class="badge bg-danger mb-2">Owner</span>
        <p class="small text-muted mb-0">Full access including billing and account deletion.</p>
      </div>
    </div>
    <div class="col-md-4">
      <div class="border rounded p-3">
        <span class="badge bg-primary mb-2">Admin</span>
        <p class="small text-muted mb-0">Can manage team, projects, and settings.</p>
      </div>
    </div>
    <div class="col-md-4">
      <div class="border rounded p-3">
        <span class="badge bg-secondary mb-2">Member</span>
        <p class="small text-muted mb-0">Can view and edit content. Cannot manage team.</p>
      </div>
    </div>
  </div>
</div>
```

### Permission Toggles

```html
<div class="card">
  <div class="card-header bg-white"><h5 class="mb-0">Member Permissions</h5></div>
  <div class="card-body">
    <div class="form-check form-switch mb-3">
      <input class="form-check-input" type="checkbox" id="permProjects" checked>
      <label class="form-check-label" for="permProjects">Create and edit projects</label>
    </div>
    <div class="form-check form-switch mb-3">
      <input class="form-check-input" type="checkbox" id="permInvite" checked>
      <label class="form-check-label" for="permInvite">Invite new members</label>
    </div>
    <div class="form-check form-switch mb-3">
      <input class="form-check-input" type="checkbox" id="permApi">
      <label class="form-check-label" for="permApi">Access API keys</label>
    </div>
    <div class="form-check form-switch mb-3">
      <input class="form-check-input" type="checkbox" id="permBilling">
      <label class="form-check-label" for="permBilling">View billing information</label>
    </div>
  </div>
</div>
```

## Best Practices

1. Show user avatars (initials or photos) in the table for quick recognition
2. Use color-coded role badges: red for owner, primary for admin, secondary for member
3. Provide a search input to filter large user lists
4. Include status badges (Active, Invited, Suspended) for at-a-glance user states
5. Use dropdown menus for row actions to keep the table clean
6. Show "Last Active" timestamps to identify inactive users
7. Limit role options in invites to prevent accidental admin grants
8. Use `form-switch` toggles for granular permission management
9. Separate invited but not yet accepted users visually
10. Provide bulk actions (select all, bulk delete) for large teams

## Common Pitfalls

1. **No invite feedback** - Users don't know if the invite was sent. Show a success toast.
2. **Missing role descriptions** - Users don't understand permission levels. Add descriptions in the invite modal.
3. **No search/filter** - Large teams can't find members. Always provide search.
4. **Hard to revoke access** - Removal should be accessible but protected with confirmation.
5. **No pending invite state** - Invited users look the same as active users. Use a distinct "Invited" badge.
6. **Permissions not saved automatically** - Toggle changes should auto-save or show a save button.

## Accessibility Considerations

- Use `aria-label="Actions for John Doe"` on dropdown toggle buttons
- Provide `aria-sort` on sortable table columns
- Use `role="search"` on the user search input
- Label permission toggles with associated text labels
- Announce invite success/failure with `aria-live="polite"`

## Responsive Behavior

On **mobile**, the user table converts to a card-based layout with user details stacked vertically. The search input and invite button wrap to a second row. On **tablet and desktop**, the full table layout displays with all columns visible. Role description cards stack on mobile and sit side by side on larger screens.
