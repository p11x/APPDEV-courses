---
title: "Notification Center"
module: "SaaS Applications"
difficulty: 2
estimated_time: "20 min"
prerequisites: ["04_09_Badges", "05_02_Dropdowns", "04_01_Card_Component"]
---

## Overview

A notification center keeps users informed about important events, updates, and actions. Bootstrap 5 dropdowns, list groups, badges, and tabs provide the components to build notification dropdowns, read/unread states, notification preferences, and bulk actions like mark-all-read.

## Basic Implementation

### Notification Dropdown

```html
<div class="dropdown">
  <button class="btn btn-outline-secondary position-relative" data-bs-toggle="dropdown" aria-expanded="false" aria-label="Notifications">
    <i class="bi bi-bell"></i>
    <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
      5
      <span class="visually-hidden">unread notifications</span>
    </span>
  </button>
  <div class="dropdown-menu dropdown-menu-end p-0" style="width:360px;max-height:480px;overflow-y:auto">
    <div class="d-flex justify-content-between align-items-center p-3 border-bottom">
      <h6 class="mb-0">Notifications</h6>
      <a href="#" class="small text-primary">Mark all read</a>
    </div>
    <div class="list-group list-group-flush">
      <a href="#" class="list-group-item list-group-item-action border-0 bg-light">
        <div class="d-flex">
          <div class="bg-primary bg-opacity-10 rounded-circle p-2 me-3 flex-shrink-0">
            <i class="bi bi-person-plus text-primary"></i>
          </div>
          <div class="flex-grow-1">
            <p class="mb-0 small"><strong>Jane Smith</strong> joined your team</p>
            <small class="text-muted">2 minutes ago</small>
          </div>
          <div class="bg-primary rounded-circle flex-shrink-0" style="width:8px;height:8px;margin-top:8px"></div>
        </div>
      </a>
      <a href="#" class="list-group-item list-group-item-action border-0 bg-light">
        <div class="d-flex">
          <div class="bg-success bg-opacity-10 rounded-circle p-2 me-3 flex-shrink-0">
            <i class="bi bi-check-circle text-success"></i>
          </div>
          <div class="flex-grow-1">
            <p class="mb-0 small"><strong>Project Alpha</strong> deployment succeeded</p>
            <small class="text-muted">15 minutes ago</small>
          </div>
          <div class="bg-primary rounded-circle flex-shrink-0" style="width:8px;height:8px;margin-top:8px"></div>
        </div>
      </a>
      <a href="#" class="list-group-item list-group-item-action border-0">
        <div class="d-flex">
          <div class="bg-warning bg-opacity-10 rounded-circle p-2 me-3 flex-shrink-0">
            <i class="bi bi-exclamation-triangle text-warning"></i>
          </div>
          <div class="flex-grow-1">
            <p class="mb-0 small"><strong>API usage</strong> at 80% of limit</p>
            <small class="text-muted">1 hour ago</small>
          </div>
        </div>
      </a>
      <a href="#" class="list-group-item list-group-item-action border-0">
        <div class="d-flex">
          <div class="bg-info bg-opacity-10 rounded-circle p-2 me-3 flex-shrink-0">
            <i class="bi bi-chat-dots text-info"></i>
          </div>
          <div class="flex-grow-1">
            <p class="mb-0 small"><strong>Bob</strong> commented on Task #42</p>
            <small class="text-muted">3 hours ago</small>
          </div>
        </div>
      </a>
    </div>
    <div class="p-2 border-top text-center">
      <a href="notifications.html" class="small">View All Notifications</a>
    </div>
  </div>
</div>
```

### Full Notifications Page

```html
<div class="container py-5">
  <div class="d-flex justify-content-between align-items-center mb-4">
    <h2>Notifications</h2>
    <div class="d-flex gap-2">
      <button class="btn btn-outline-secondary btn-sm">Mark All Read</button>
      <button class="btn btn-outline-danger btn-sm">Clear All</button>
    </div>
  </div>
  <ul class="nav nav-tabs mb-4">
    <li class="nav-item">
      <a class="nav-link active" href="#">All <span class="badge bg-secondary ms-1">12</span></a>
    </li>
    <li class="nav-item">
      <a class="nav-link" href="#">Unread <span class="badge bg-primary ms-1">5</span></a>
    </li>
    <li class="nav-item">
      <a class="nav-link" href="#">Mentions <span class="badge bg-secondary ms-1">3</span></a>
    </li>
  </ul>
  <div class="list-group">
    <div class="list-group-item list-group-item-action bg-light d-flex align-items-start">
      <div class="bg-primary bg-opacity-10 rounded-circle p-2 me-3 flex-shrink-0">
        <i class="bi bi-at text-primary"></i>
      </div>
      <div class="flex-grow-1">
        <div class="d-flex justify-content-between">
          <strong>Sarah mentioned you</strong>
          <small class="text-muted">5 min ago</small>
        </div>
        <p class="mb-0 small text-muted">"@John can you review the latest PR?"</p>
      </div>
      <div class="bg-primary rounded-circle flex-shrink-0 ms-2" style="width:8px;height:8px;margin-top:8px"></div>
    </div>
    <div class="list-group-item list-group-item-action d-flex align-items-start">
      <div class="bg-success bg-opacity-10 rounded-circle p-2 me-3 flex-shrink-0">
        <i class="bi bi-check-circle text-success"></i>
      </div>
      <div class="flex-grow-1">
        <div class="d-flex justify-content-between">
          <strong>Build succeeded</strong>
          <small class="text-muted">1 hour ago</small>
        </div>
        <p class="mb-0 small text-muted">Project Alpha - Build #142 completed successfully.</p>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Notification Preferences

```html
<div class="card">
  <div class="card-header bg-white"><h5 class="mb-0">Notification Settings</h5></div>
  <div class="card-body">
    <div class="table-responsive">
      <table class="table align-middle">
        <thead>
          <tr>
            <th>Notification Type</th>
            <th class="text-center">Email</th>
            <th class="text-center">Push</th>
            <th class="text-center">In-App</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>Team member joins</td>
            <td class="text-center"><input class="form-check-input" type="checkbox" checked></td>
            <td class="text-center"><input class="form-check-input" type="checkbox" checked></td>
            <td class="text-center"><input class="form-check-input" type="checkbox" checked></td>
          </tr>
          <tr>
            <td>Project updates</td>
            <td class="text-center"><input class="form-check-input" type="checkbox"></td>
            <td class="text-center"><input class="form-check-input" type="checkbox" checked></td>
            <td class="text-center"><input class="form-check-input" type="checkbox" checked></td>
          </tr>
          <tr>
            <td>Billing alerts</td>
            <td class="text-center"><input class="form-check-input" type="checkbox" checked></td>
            <td class="text-center"><input class="form-check-input" type="checkbox"></td>
            <td class="text-center"><input class="form-check-input" type="checkbox" checked></td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</div>
```

## Best Practices

1. Show an unread count badge on the bell icon that updates in real-time
2. Use visual distinction for unread vs read notifications (background color, bold text, dot indicator)
3. Group notifications by type with icons for quick scanning
4. Provide "Mark all read" as a top-level action
5. Limit dropdown to recent 5-10 notifications with a "View All" link
6. Use timestamps that are relative ("5 minutes ago") not absolute
7. Offer granular notification preferences (email, push, in-app per type)
8. Use tab filters (All, Unread, Mentions) on the full notifications page
9. Include a "Clear All" option with confirmation
10. Use `bi-bell-fill` when there are unread notifications

## Common Pitfalls

1. **No unread indicator** - Users can't distinguish new from old. Use bold text, background color, or a dot.
2. **Badge count not updating** - Stale badges confuse users. Update the count when notifications are read.
3. **No notification preferences** - Users get overwhelmed. Let them control what they receive.
4. **Dropdown too narrow** - Notifications get truncated. Use at least 360px width.
5. **No mark-all-read** - Reading each individually is tedious. Provide bulk actions.
6. **Missing timestamps** - Time context is essential. Always show when the notification occurred.

## Accessibility Considerations

- Use `aria-label="Notifications, 5 unread"` on the bell button
- Announce new notifications with `aria-live="polite"`
- Mark unread items with `aria-current="false"` or a descriptive class
- Use `role="status"` on the notification count area
- Ensure notification links have descriptive text, not just "View"
- Provide keyboard navigation through notification items

## Responsive Behavior

On **mobile**, the notification dropdown becomes a full-width overlay or offcanvas panel. Notification items expand to full screen width. On **tablet and desktop**, the dropdown appears as a 360-400px panel anchored to the top-right. The notification preferences table uses `table-responsive` on mobile.
