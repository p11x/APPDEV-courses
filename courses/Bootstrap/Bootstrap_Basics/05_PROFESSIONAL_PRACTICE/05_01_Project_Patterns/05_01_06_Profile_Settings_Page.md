---
title: Profile Settings Page
category: Professional Practice
difficulty: 2
time: 45 min
tags: bootstrap5, profile, settings, tabs, forms, avatar, responsive
---

## Overview

A profile settings page allows users to manage personal information, preferences, and account options. Bootstrap 5's nav tabs, card, form controls, and grid system combine to create a tabbed, responsive settings interface with an avatar upload, profile card, and grouped settings sections.

## Basic Implementation

### Profile Header Card

```html
<div class="container py-5">
  <div class="card border-0 shadow-sm mb-4">
    <div class="card-body d-flex flex-column flex-md-row align-items-center gap-4 p-4">
      <div class="position-relative">
        <img src="avatar.jpg" alt="User avatar" class="rounded-circle" width="96" height="96" style="object-fit:cover">
        <label class="position-absolute bottom-0 end-0 btn btn-sm btn-primary rounded-circle" style="width:32px;height:32px" title="Change avatar">
          <i class="bi bi-camera"></i>
          <input type="file" class="d-none" accept="image/*">
        </label>
      </div>
      <div class="text-center text-md-start">
        <h4 class="mb-1">Sarah Johnson</h4>
        <p class="text-muted mb-2">sarah.johnson@example.com</p>
        <span class="badge bg-success">Pro Member</span>
      </div>
      <div class="ms-md-auto">
        <button class="btn btn-outline-primary"><i class="bi bi-pencil me-1"></i>Edit Profile</button>
      </div>
    </div>
  </div>
```

### Tabbed Settings Navigation

```html
  <div class="card border-0 shadow-sm">
    <div class="card-header bg-white border-bottom-0 pt-3">
      <ul class="nav nav-tabs card-header-tabs" role="tablist">
        <li class="nav-item">
          <button class="nav-link active" data-bs-toggle="tab" data-bs-target="#general" type="button" role="tab">General</button>
        </li>
        <li class="nav-item">
          <button class="nav-link" data-bs-toggle="tab" data-bs-target="#security" type="button" role="tab">Security</button>
        </li>
        <li class="nav-item">
          <button class="nav-link" data-bs-toggle="tab" data-bs-target="#notifications" type="button" role="tab">Notifications</button>
        </li>
        <li class="nav-item">
          <button class="nav-link" data-bs-toggle="tab" data-bs-target="#billing" type="button" role="tab">Billing</button>
        </li>
      </ul>
    </div>
    <div class="card-body p-4">
      <div class="tab-content">
        <!-- General Tab -->
        <div class="tab-pane fade show active" id="general" role="tabpanel">
          <h5 class="mb-4">General Information</h5>
          <form>
            <div class="row g-3">
              <div class="col-md-6">
                <label for="displayName" class="form-label">Display Name</label>
                <input type="text" class="form-control" id="displayName" value="Sarah Johnson">
              </div>
              <div class="col-md-6">
                <label for="username" class="form-label">Username</label>
                <div class="input-group">
                  <span class="input-group-text">@</span>
                  <input type="text" class="form-control" id="username" value="sarahj">
                </div>
              </div>
              <div class="col-12">
                <label for="bio" class="form-label">Bio</label>
                <textarea class="form-control" id="bio" rows="3">Front-end developer passionate about UI design.</textarea>
              </div>
              <div class="col-md-6">
                <label for="timezone" class="form-label">Timezone</label>
                <select class="form-select" id="timezone">
                  <option>UTC-5 (Eastern Time)</option>
                  <option>UTC-8 (Pacific Time)</option>
                  <option selected>UTC+0 (GMT)</option>
                </select>
              </div>
              <div class="col-md-6">
                <label for="language" class="form-label">Language</label>
                <select class="form-select" id="language">
                  <option selected>English</option>
                  <option>Spanish</option>
                  <option>French</option>
                </select>
              </div>
            </div>
            <button type="submit" class="btn btn-primary mt-4">Save Changes</button>
          </form>
        </div>

        <!-- Security Tab -->
        <div class="tab-pane fade" id="security" role="tabpanel">
          <h5 class="mb-4">Security Settings</h5>
          <form>
            <div class="mb-3">
              <label for="currentPassword" class="form-label">Current Password</label>
              <input type="password" class="form-control" id="currentPassword">
            </div>
            <div class="row g-3 mb-3">
              <div class="col-md-6">
                <label for="newPassword" class="form-label">New Password</label>
                <input type="password" class="form-control" id="newPassword">
              </div>
              <div class="col-md-6">
                <label for="confirmNewPassword" class="form-label">Confirm New Password</label>
                <input type="password" class="form-control" id="confirmNewPassword">
              </div>
            </div>
            <div class="form-check form-switch mb-3">
              <input class="form-check-input" type="checkbox" id="twoFactor" checked>
              <label class="form-check-label" for="twoFactor">Enable Two-Factor Authentication</label>
            </div>
            <button type="submit" class="btn btn-primary">Update Security</button>
          </form>
        </div>

        <!-- Notifications Tab -->
        <div class="tab-pane fade" id="notifications" role="tabpanel">
          <h5 class="mb-4">Notification Preferences</h5>
          <div class="form-check form-switch mb-3">
            <input class="form-check-input" type="checkbox" id="emailNotif" checked>
            <label class="form-check-label" for="emailNotif">Email notifications</label>
          </div>
          <div class="form-check form-switch mb-3">
            <input class="form-check-input" type="checkbox" id="pushNotif">
            <label class="form-check-label" for="pushNotif">Push notifications</label>
          </div>
          <div class="form-check form-switch mb-3">
            <input class="form-check-input" type="checkbox" id="marketingNotif" checked>
            <label class="form-check-label" for="marketingNotif">Marketing emails</label>
          </div>
          <button class="btn btn-primary">Save Preferences</button>
        </div>

        <!-- Billing Tab -->
        <div class="tab-pane fade" id="billing" role="tabpanel">
          <h5 class="mb-4">Billing Information</h5>
          <div class="card bg-light border-0 mb-3">
            <div class="card-body d-flex justify-content-between align-items-center">
              <div>
                <p class="fw-semibold mb-0">Pro Plan</p>
                <small class="text-muted">$29/month &middot; Renews Apr 15, 2026</small>
              </div>
              <a href="#" class="btn btn-outline-primary btn-sm">Manage Plan</a>
            </div>
          </div>
          <h6>Payment Method</h6>
          <div class="d-flex align-items-center">
            <i class="bi bi-credit-card fs-3 me-3"></i>
            <div>
              <p class="mb-0">Visa ending in 4242</p>
              <small class="text-muted">Expires 12/2027</small>
            </div>
            <a href="#" class="btn btn-outline-secondary btn-sm ms-auto">Update</a>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

- **Drag-and-Drop Avatar Upload:** Use the HTML5 Drag and Drop API to accept dropped image files on the avatar area.
- **Activity Log Tab:** Add a timeline of recent account events using `list-group` with custom content.
- **API Keys Tab:** Display generated API keys with copy-to-clipboard buttons and visibility toggles.
- **Connected Accounts:** Show OAuth-linked accounts with disconnect buttons in a `list-group`.
- **Danger Zone Section:** Add a destructive actions area with delete account and data export buttons styled with `btn-outline-danger`.

## Best Practices

1. Use `nav-tabs card-header-tabs` inside the card header for seamless tab integration.
2. Apply `flex-column flex-md-row` on the profile header for responsive stacking.
3. Use `position-relative` on the avatar container for overlaying the camera button.
4. Hide the file input with `d-none` and wrap it in a `<label>` for accessible triggering.
5. Use `form-check form-switch` for toggle settings like notifications and 2FA.
6. Apply `row g-3` inside tab panes for consistent form field spacing.
7. Use `input-group` with `@` prefix for username fields.
8. Group related settings into separate tabs to avoid excessively long forms.
9. Apply `bg-light border-0` on billing summary cards for subtle contrast.
10. Use `object-fit: cover` on avatars to prevent distortion.
11. Keep save buttons within each tab so users don't need to scroll to submit.
12. Use `data-bs-target` with matching `id` values for tab-pane binding.

## Common Pitfalls

1. **Missing `role="tabpanel"` on panes:** Screen readers need this for proper tab association.
2. **No `for`/`id` on labels:** Labels without association are not clickable targets.
3. **Avatar overflow on small screens:** Use `flex-wrap` or `flex-column` to stack on mobile.
4. **Too many tabs:** More than 5-6 tabs overflows on mobile; use an accordion fallback instead.
5. **Missing `accept="image/*"` on avatar input:** Users can select non-image files without it.
6. **Unstyled active tab:** Active tabs need `active` class on both the `<button>` and its target pane.
7. **Hardcoded avatar dimensions:** Use `width`/`height` attributes with `style="object-fit:cover"` for consistency.

## Accessibility Considerations

- Use `role="tablist"`, `role="tab"`, and `role="tabpanel"` for proper ARIA tab semantics.
- Add `aria-selected="true"` on the active tab button and `aria-controls` pointing to the pane `id`.
- Ensure the avatar file input has `aria-label="Upload avatar"`.
- Use `aria-describedby` on password fields to reference password requirements.
- Add `role="alert"` on success/error notification banners.

## Responsive Behavior

| Breakpoint | Profile Header | Tabs | Form Layout |
|------------|---------------|------|-------------|
| `<576px` | Stacked center | Scrollable | 1 column |
| `≥576px` | Stacked center | Scrollable | 1 column |
| `≥768px` | Side-by-side | Full tabs | 2-col fields |
| `≥992px` | Side-by-side | Full tabs | 2-col fields |

Use `nav-tabs` with `flex-nowrap` and `overflow-auto` on small screens to prevent tab wrapping.
