---
title: "Team Invitations"
description: "Build team invitation modals, pending invitation lists, role selectors, and email invite forms using Bootstrap 5."
difficulty: 2
estimated_time: "30 minutes"
prerequisites:
  - "Bootstrap 5 Modals"
  - "Bootstrap 5 Forms"
  - "Bootstrap 5 Tables"
  - "Bootstrap 5 Badges"
---

## Overview

Team invitation components enable SaaS administrators to invite collaborators via email, assign roles, manage pending invitations, and track invite status. Bootstrap 5's modal, form, table, and badge components build a complete invitation workflow from sending to acceptance.

The UI should support single and bulk invitations, role assignment at invite time, pending invite management with resend/revoke actions, and clear status indicators for sent, accepted, and expired invitations.

## Basic Implementation

### Invite Team Member Modal Trigger

```html
<button class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#inviteModal">
  <i class="bi bi-person-plus me-1"></i> Invite Team Member
</button>
```

### Invite Modal with Email and Role

```html
<div class="modal fade" id="inviteModal" tabindex="-1" aria-labelledby="inviteModalLabel">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="inviteModalLabel">Invite Team Member</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <div class="mb-3">
          <label for="inviteEmail" class="form-label">Email Address</label>
          <input type="email" class="form-control" id="inviteEmail" placeholder="colleague@company.com" required>
          <div class="form-text">Separate multiple emails with commas.</div>
        </div>
        <div class="mb-3">
          <label for="inviteRole" class="form-label">Role</label>
          <select class="form-select" id="inviteRole">
            <option value="viewer">Viewer - Can view content only</option>
            <option value="editor">Editor - Can view and edit content</option>
            <option value="admin" selected>Admin - Full access</option>
          </select>
        </div>
        <div class="mb-3">
          <label for="inviteMessage" class="form-label">Personal Message (optional)</label>
          <textarea class="form-control" id="inviteMessage" rows="3" placeholder="Add a personal note to the invitation email..."></textarea>
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button type="button" class="btn btn-primary">Send Invitation</button>
      </div>
    </div>
  </div>
</div>
```

### Pending Invitations Table

```html
<div class="table-responsive">
  <table class="table table-hover align-middle">
    <thead class="table-light">
      <tr>
        <th>Email</th>
        <th>Role</th>
        <th>Status</th>
        <th>Sent</th>
        <th>Actions</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td>dev@acme.com</td>
        <td><span class="badge bg-info">Editor</span></td>
        <td><span class="badge bg-warning text-dark">Pending</span></td>
        <td class="text-muted small">2 days ago</td>
        <td>
          <button class="btn btn-sm btn-outline-secondary me-1" title="Resend">
            <i class="bi bi-arrow-clockwise"></i>
          </button>
          <button class="btn btn-sm btn-outline-danger" title="Revoke">
            <i class="bi bi-x-lg"></i>
          </button>
        </td>
      </tr>
      <tr>
        <td>manager@acme.com</td>
        <td><span class="badge bg-primary">Admin</span></td>
        <td><span class="badge bg-success">Accepted</span></td>
        <td class="text-muted small">5 days ago</td>
        <td>
          <button class="btn btn-sm btn-outline-secondary disabled" title="Already accepted">
            <i class="bi bi-check-lg text-success"></i>
          </button>
        </td>
      </tr>
      <tr>
        <td>old@acme.com</td>
        <td><span class="badge bg-secondary">Viewer</span></td>
        <td><span class="badge bg-danger">Expired</span></td>
        <td class="text-muted small">30 days ago</td>
        <td>
          <button class="btn btn-sm btn-outline-primary me-1" title="Resend">
            <i class="bi bi-arrow-clockwise"></i>
          </button>
          <button class="btn btn-sm btn-outline-danger" title="Remove">
            <i class="bi bi-trash"></i>
          </button>
        </td>
      </tr>
    </tbody>
  </table>
</div>
```

## Advanced Variations

### Bulk Invite with Role Assignment

```html
<div class="modal fade" id="bulkInviteModal" tabindex="-1">
  <div class="modal-dialog modal-lg">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Bulk Invite Team Members</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <div class="mb-3">
          <label for="bulkEmails" class="form-label">Email Addresses</label>
          <textarea class="form-control font-monospace" id="bulkEmails" rows="4" placeholder="dev1@acme.com&#10;dev2@acme.com&#10;designer@acme.com"></textarea>
          <div class="form-text">One email per line or comma-separated.</div>
        </div>
        <div class="mb-3">
          <label class="form-label">Assign Role to All</label>
          <select class="form-select">
            <option value="viewer">Viewer</option>
            <option value="editor" selected>Editor</option>
            <option value="admin">Admin</option>
          </select>
        </div>
        <div class="alert alert-info small mb-0">
          <i class="bi bi-info-circle me-1"></i>
          <strong>5 invitations</strong> will be sent. Duplicate and existing member emails will be skipped.
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button type="button" class="btn btn-primary">Send 5 Invitations</button>
      </div>
    </div>
  </div>
</div>
```

### Role Selector with Descriptions

```html
<div class="mb-3">
  <label class="form-label">Select Role</label>
  <div class="list-group">
    <label class="list-group-item d-flex gap-3">
      <input class="form-check-input flex-shrink-0" type="radio" name="roleDesc" value="viewer">
      <div>
        <strong class="d-block">Viewer</strong>
        <small class="text-muted">Can view dashboards and reports. Cannot make changes.</small>
      </div>
    </label>
    <label class="list-group-item d-flex gap-3">
      <input class="form-check-input flex-shrink-0" type="radio" name="roleDesc" value="editor" checked>
      <div>
        <strong class="d-block">Editor</strong>
        <small class="text-muted">Can create and edit content. Cannot manage team or billing.</small>
      </div>
    </label>
    <label class="list-group-item d-flex gap-3">
      <input class="form-check-input flex-shrink-0" type="radio" name="roleDesc" value="admin">
      <div>
        <strong class="d-block">Admin</strong>
        <small class="text-muted">Full access including team management, billing, and settings.</small>
      </div>
    </label>
  </div>
</div>
```

### Team Member Count Card

```html
<div class="card bg-light mb-3">
  <div class="card-body py-2 d-flex justify-content-between align-items-center">
    <div>
      <strong>Team Members</strong>
      <span class="text-muted">(<span id="memberCount">8</span>/10 seats used)</span>
    </div>
    <div class="progress" style="width: 120px; height: 8px;">
      <div class="progress-bar bg-warning" style="width: 80%;"></div>
    </div>
  </div>
</div>
```

### Invitation Link Sharing

```html
<div class="card">
  <div class="card-body">
    <h6 class="card-title">Share Invitation Link</h6>
    <p class="text-muted small">Anyone with this link can join your team.</p>
    <div class="input-group">
      <input type="text" class="form-control form-control-sm" value="https://app.example.com/join/abc123" readonly id="inviteLink">
      <button class="btn btn-outline-secondary btn-sm" type="button" id="copyLink">
        <i class="bi bi-clipboard"></i> Copy
      </button>
    </div>
    <div class="form-check form-switch mt-2">
      <input class="form-check-input" type="checkbox" id="linkEnabled" checked>
      <label class="form-check-label small" for="linkEnabled">Link active</label>
    </div>
  </div>
</div>
```

## Best Practices

1. Use `type="email"` for proper mobile keyboard and browser validation
2. Provide clear role descriptions so admins understand permission levels
3. Show seat usage to prevent exceeding plan limits
4. Support bulk invitations to speed up team onboarding
5. Allow resending expired invitations without recreating them
6. Use color-coded status badges (Pending=yellow, Accepted=green, Expired=red)
7. Include an optional personal message field for a human touch
8. Display the invite expiration period (e.g., "Expires in 7 days")
9. Revoke invitations immediately when access should be removed
10. Use `font-monospace` for invitation links for clarity
11. Show a count of pending invitations in the page header
12. Send confirmation to the inviter when an invitation is accepted
13. Validate that email addresses are not already team members before sending

## Common Pitfalls

1. **No email validation**: Accepting malformed email addresses wastes API calls and creates confusion.
2. **Missing duplicate check**: Sending invitations to existing members frustrates both parties.
3. **No seat limit warning**: Allowing invitations beyond plan limits leads to failed signups.
4. **Expired invitations without notification**: Users who accept expired links receive confusing errors. Show a clear "expired" state.
5. **No bulk invite**: Manually inviting 20 team members one at a time is tedious and error-prone.
6. **Missing role description**: Users do not understand what "Editor" means without a brief explanation.
7. **No revocation for pending invites**: Pending invitations that cannot be cancelled are a security risk.

## Accessibility Considerations

- Use `type="email"` for screen reader and keyboard optimization
- Associate all form labels with their inputs using `for`/`id`
- Use `aria-label` on action buttons (resend, revoke) in the invitation table
- Provide `role="alert"` on invitation success/error messages
- Use proper heading hierarchy in modals
- Announce bulk invitation results using `aria-live="polite"`
- Ensure role selector radios have visible labels with descriptions

## Responsive Behavior

On mobile, the invitation table should use `table-responsive`. The invite modal should use `modal-fullscreen-sm-down`. Role selector list items should stack properly. Bulk invite textarea should remain full-width. The team seat count card should remain compact. Action buttons in the table row should maintain touch target sizes of at least 44x44px.
