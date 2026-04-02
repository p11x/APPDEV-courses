---
title: "Audit Log UI"
module: "Enterprise Apps"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["04_04_Table", "04_05_Forms", "05_01_Accordion"]
---

## Overview

Audit logs track user actions and system events for compliance, security, and debugging. Bootstrap 5 tables, badges, forms, accordion, and list groups provide the components to build filterable, searchable, expandable audit log interfaces that meet enterprise compliance requirements.

## Basic Implementation

### Audit Log Table

```html
<div class="card">
  <div class="card-header bg-white">
    <div class="row g-3 align-items-end">
      <div class="col-md-3">
        <label class="form-label small">Search</label>
        <input type="search" class="form-control form-control-sm" placeholder="Search events...">
      </div>
      <div class="col-md-2">
        <label class="form-label small">User</label>
        <select class="form-select form-select-sm">
          <option>All Users</option>
          <option>John Doe</option>
          <option>Alice Smith</option>
        </select>
      </div>
      <div class="col-md-2">
        <label class="form-label small">Action</label>
        <select class="form-select form-select-sm">
          <option>All Actions</option>
          <option>Login</option>
          <option>Create</option>
          <option>Update</option>
          <option>Delete</option>
        </select>
      </div>
      <div class="col-md-3">
        <label class="form-label small">Date Range</label>
        <div class="input-group input-group-sm">
          <input type="date" class="form-control">
          <span class="input-group-text">to</span>
          <input type="date" class="form-control">
        </div>
      </div>
      <div class="col-md-2">
        <button class="btn btn-primary btn-sm w-100">Filter</button>
      </div>
    </div>
  </div>
  <div class="table-responsive">
    <table class="table table-hover align-middle mb-0">
      <thead class="table-light">
        <tr>
          <th>Timestamp</th>
          <th>User</th>
          <th>Action</th>
          <th>Resource</th>
          <th>IP Address</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td class="text-nowrap"><small>Mar 15, 2024 10:30:45 AM</small></td>
          <td>
            <div class="d-flex align-items-center">
              <div class="bg-primary text-white rounded-circle d-flex align-items-center justify-content-center me-2" style="width:28px;height:28px;font-size:0.7em">JD</div>
              <span class="small">John Doe</span>
            </div>
          </td>
          <td><span class="badge bg-success">Create</span></td>
          <td>Project: Alpha</td>
          <td class="font-monospace small">192.168.1.42</td>
          <td>
            <button class="btn btn-sm btn-outline-secondary" data-bs-toggle="collapse" data-bs-target="#detail1">
              <i class="bi bi-chevron-down"></i>
            </button>
          </td>
        </tr>
        <tr class="collapse" id="detail1">
          <td colspan="6" class="bg-light">
            <div class="p-3">
              <h6 class="small mb-2">Event Details</h6>
              <pre class="bg-white border rounded p-3 small mb-0"><code>{
  "event": "project.create",
  "user_id": "usr_123",
  "resource_id": "proj_456",
  "changes": {
    "name": "Alpha",
    "status": "active"
  },
  "user_agent": "Mozilla/5.0 Chrome/122"
}</code></pre>
            </div>
          </td>
        </tr>
        <tr>
          <td class="text-nowrap"><small>Mar 15, 2024 10:28:12 AM</small></td>
          <td>
            <div class="d-flex align-items-center">
              <div class="bg-success text-white rounded-circle d-flex align-items-center justify-content-center me-2" style="width:28px;height:28px;font-size:0.7em">AS</div>
              <span class="small">Alice Smith</span>
            </div>
          </td>
          <td><span class="badge bg-warning text-dark">Update</span></td>
          <td>User: bob@acme.com</td>
          <td class="font-monospace small">10.0.0.15</td>
          <td>
            <button class="btn btn-sm btn-outline-secondary" data-bs-toggle="collapse" data-bs-target="#detail2">
              <i class="bi bi-chevron-down"></i>
            </button>
          </td>
        </tr>
        <tr>
          <td class="text-nowrap"><small>Mar 15, 2024 10:25:00 AM</small></td>
          <td>
            <div class="d-flex align-items-center">
              <div class="bg-secondary text-white rounded-circle d-flex align-items-center justify-content-center me-2" style="width:28px;height:28px;font-size:0.7em">SY</div>
              <span class="small">System</span>
            </div>
          </td>
          <td><span class="badge bg-info">Login</span></td>
          <td>Session: ses_789</td>
          <td class="font-monospace small">172.16.0.5</td>
          <td>
            <button class="btn btn-sm btn-outline-secondary" data-bs-toggle="collapse" data-bs-target="#detail3">
              <i class="bi bi-chevron-down"></i>
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
  <div class="card-footer bg-white d-flex justify-content-between align-items-center">
    <span class="text-muted small">Showing 1-25 of 12,847 events</span>
    <nav>
      <ul class="pagination pagination-sm mb-0">
        <li class="page-item disabled"><a class="page-link" href="#">Previous</a></li>
        <li class="page-item active"><a class="page-link" href="#">1</a></li>
        <li class="page-item"><a class="page-link" href="#">2</a></li>
        <li class="page-item"><a class="page-link" href="#">Next</a></li>
      </ul>
    </nav>
  </div>
</div>
```

## Advanced Variations

### Timeline View

```html
<div class="position-relative ps-4">
  <div class="position-absolute top-0 bottom-0 start-2 border-start"></div>
  <div class="mb-4 position-relative">
    <div class="position-absolute bg-success rounded-circle border border-white" style="width:14px;height:14px;left:-32px;top:4px"></div>
    <div class="small text-muted mb-1">Mar 15, 2024 - 10:30 AM</div>
    <div class="card card-body p-3">
      <div class="d-flex justify-content-between">
        <strong>Project "Alpha" created</strong>
        <span class="badge bg-success">Create</span>
      </div>
      <small class="text-muted">by John Doe from 192.168.1.42</small>
    </div>
  </div>
  <div class="mb-4 position-relative">
    <div class="position-absolute bg-warning rounded-circle border border-white" style="width:14px;height:14px;left:-32px;top:4px"></div>
    <div class="small text-muted mb-1">Mar 15, 2024 - 10:28 AM</div>
    <div class="card card-body p-3">
      <div class="d-flex justify-content-between">
        <strong>User role updated</strong>
        <span class="badge bg-warning text-dark">Update</span>
      </div>
      <small class="text-muted">by Alice Smith - Changed role from Member to Admin</small>
    </div>
  </div>
  <div class="mb-4 position-relative">
    <div class="position-absolute bg-danger rounded-circle border border-white" style="width:14px;height:14px;left:-32px;top:4px"></div>
    <div class="small text-muted mb-1">Mar 15, 2024 - 10:20 AM</div>
    <div class="card card-body p-3">
      <div class="d-flex justify-content-between">
        <strong>API key revoked</strong>
        <span class="badge bg-danger">Delete</span>
      </div>
      <small class="text-muted">by System - Key sk_live_****abcd was automatically revoked</small>
    </div>
  </div>
</div>
```

## Best Practices

1. Provide filters by user, action type, date range, and keyword search
2. Use color-coded badges for action types: green (create), yellow (update), red (delete), blue (login)
3. Show timestamps in a consistent, readable format
4. Include expandable detail rows with raw event data (JSON)
5. Use a timeline view for chronological browsing
6. Allow export of filtered logs for compliance
7. Display IP addresses in monospace font
8. Use small avatars with initials for quick user identification
9. Show event count ("Showing 1-25 of 12,847")
10. Auto-refresh or provide a "Refresh" button for live monitoring

## Common Pitfalls

1. **No filtering** - Thousands of unfiltered events are useless. Always provide user/action/date filters.
2. **Missing detail expansion** - Raw event data is essential for debugging. Make it expandable.
3. **Timestamps not in user timezone** - Show times in the viewer's local timezone.
4. **No export capability** - Compliance requires downloadable logs. Provide CSV export.
5. **Action types not color-coded** - Visual distinction helps scanning. Use badges with colors.
6. **IP address not shown** - Security analysis requires IP tracking. Always include it.

## Accessibility Considerations

- Use `aria-expanded="false"` on expand buttons, toggling to `true` when opened
- Provide `aria-label="Expand event details"` on detail toggle buttons
- Use `role="log"` on the audit log container
- Label all filter controls with associated labels
- Announce filter result counts with `aria-live="polite"`

## Responsive Behavior

On **mobile**, the filter bar stacks vertically. The table uses `table-responsive` for horizontal scrolling, or converts to a card layout. Expandable details remain functional. On **tablet and desktop**, all filter controls display in a single row. The timeline view works well at all breakpoints with the vertical line on the left.
