---
title: "Audit Trail UI"
description: "Build activity timelines, audit logs, and user activity tracking interfaces with filtering using Bootstrap 5."
difficulty: 2
estimated_time: "30 minutes"
prerequisites:
  - "Bootstrap 5 Tables"
  - "Bootstrap 5 Badges"
  - "Bootstrap 5 Forms"
  - "Bootstrap 5 Pagination"
---

## Overview

Audit trails provide a chronological record of user actions and system events for compliance, security, and debugging. Bootstrap 5 enables building timeline-based activity views, filterable audit tables, and detailed action logs. These components are essential for enterprise SaaS applications that require accountability and regulatory compliance.

The audit trail displays who did what, when, and from where. It should support filtering by user, action type, date range, and resource, with clear visual indicators for different event categories.

## Basic Implementation

### Activity Timeline

```html
<div class="position-relative ps-4">
  <div class="position-absolute top-0 start-0 translate-middle-x">
    <span class="badge rounded-pill bg-primary"><i class="bi bi-person"></i></span>
  </div>
  <div class="card mb-3">
    <div class="card-body py-2">
      <div class="d-flex justify-content-between">
        <div>
          <strong>Jane Cooper</strong> updated project settings
          <div class="text-muted small">Changed billing plan from Pro to Enterprise</div>
        </div>
        <div class="text-muted small text-end">2 min ago<br><span class="badge bg-light text-dark">203.0.113.42</span></div>
      </div>
    </div>
  </div>
</div>
<div class="position-relative ps-4">
  <div class="position-absolute top-0 start-0 translate-middle-x">
    <span class="badge rounded-pill bg-success"><i class="bi bi-plus"></i></span>
  </div>
  <div class="card mb-3">
    <div class="card-body py-2">
      <div class="d-flex justify-content-between">
        <div>
          <strong>Admin Bot</strong> created new user account
          <div class="text-muted small">User: dev@acme.com, Role: Developer</div>
        </div>
        <div class="text-muted small text-end">15 min ago</div>
      </div>
    </div>
  </div>
</div>
<div class="position-relative ps-4">
  <div class="position-absolute top-0 start-0 translate-middle-x">
    <span class="badge rounded-pill bg-danger"><i class="bi bi-shield-exclamation"></i></span>
  </div>
  <div class="card mb-3">
    <div class="card-body py-2">
      <div class="d-flex justify-content-between">
        <div>
          <strong>Security System</strong> blocked suspicious login
          <div class="text-muted small">Failed login attempt from unrecognized device</div>
        </div>
        <div class="text-muted small text-end">1 hr ago<br><span class="badge bg-light text-dark">198.51.100.10</span></div>
      </div>
    </div>
  </div>
</div>
```

### Audit Log Table

```html
<div class="table-responsive">
  <table class="table table-sm table-hover">
    <thead class="table-light">
      <tr>
        <th>Timestamp</th>
        <th>User</th>
        <th>Action</th>
        <th>Resource</th>
        <th>IP Address</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td class="text-nowrap small">Apr 2, 2026 10:30 AM</td>
        <td>Jane Cooper</td>
        <td><span class="badge bg-primary">UPDATE</span></td>
        <td><code>settings/billing</code></td>
        <td class="font-monospace small">203.0.113.42</td>
      </tr>
      <tr>
        <td class="text-nowrap small">Apr 2, 2026 10:15 AM</td>
        <td>System</td>
        <td><span class="badge bg-success">CREATE</span></td>
        <td><code>users/dev@acme.com</code></td>
        <td class="font-monospace small">-</td>
      </tr>
      <tr>
        <td class="text-nowrap small">Apr 2, 2026 9:30 AM</td>
        <td>Alex Smith</td>
        <td><span class="badge bg-danger">DELETE</span></td>
        <td><code>projects/old-project</code></td>
        <td class="font-monospace small">192.168.1.100</td>
      </tr>
    </tbody>
  </table>
</div>
```

## Advanced Variations

### Filter Bar

```html
<div class="card bg-light mb-3">
  <div class="card-body py-2">
    <div class="row g-2 align-items-end">
      <div class="col-md-3">
        <label class="form-label small mb-1">Action Type</label>
        <select class="form-select form-select-sm">
          <option selected>All Actions</option>
          <option>CREATE</option>
          <option>UPDATE</option>
          <option>DELETE</option>
          <option>LOGIN</option>
        </select>
      </div>
      <div class="col-md-3">
        <label class="form-label small mb-1">User</label>
        <select class="form-select form-select-sm">
          <option selected>All Users</option>
          <option>Jane Cooper</option>
          <option>Alex Smith</option>
          <option>System</option>
        </select>
      </div>
      <div class="col-md-3">
        <label class="form-label small mb-1">Date Range</label>
        <input type="date" class="form-control form-control-sm">
      </div>
      <div class="col-md-3">
        <button class="btn btn-sm btn-primary w-100"><i class="bi bi-funnel me-1"></i>Apply Filters</button>
      </div>
    </div>
  </div>
</div>
```

### Active Filter Chips

```html
<div class="d-flex flex-wrap gap-2 mb-3">
  <span class="badge bg-light text-dark border d-flex align-items-center gap-1">
    Action: DELETE <button class="btn-close btn-close-sm" style="font-size: 0.6rem;" aria-label="Remove filter"></button>
  </span>
  <span class="badge bg-light text-dark border d-flex align-items-center gap-1">
    User: Alex Smith <button class="btn-close btn-close-sm" style="font-size: 0.6rem;" aria-label="Remove filter"></button>
  </span>
  <button class="btn btn-link btn-sm p-0 text-decoration-none">Clear all filters</button>
</div>
```

### Detailed Action Expansion

```html
<div class="accordion" id="auditAccordion">
  <div class="accordion-item">
    <h2 class="accordion-header">
      <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#audit1">
        <span class="badge bg-primary me-2">UPDATE</span>
        <span class="me-2">Jane Cooper updated billing settings</span>
        <span class="text-muted small">2 min ago</span>
      </button>
    </h2>
    <div id="audit1" class="accordion-collapse collapse" data-bs-parent="#auditAccordion">
      <div class="accordion-body">
        <h6>Changes</h6>
        <table class="table table-sm mb-3">
          <thead><tr><th>Field</th><th>Before</th><th>After</th></tr></thead>
          <tbody>
            <tr><td>Plan</td><td class="text-danger"><s>Pro</s></td><td class="text-success">Enterprise</td></tr>
            <tr><td>Max Users</td><td>10</td><td>Unlimited</td></tr>
          </tbody>
        </table>
        <div class="row small text-muted">
          <div class="col-md-4">IP: 203.0.113.42</div>
          <div class="col-md-4">User Agent: Chrome 120</div>
          <div class="col-md-4">Session: abc-123-def</div>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Display timestamps in the user's local timezone with relative time labels
2. Use color-coded badges for action types (CREATE=green, UPDATE=blue, DELETE=red)
3. Include IP address and user agent for security-relevant events
4. Provide filtering by multiple criteria (user, action, date, resource)
5. Show before/after diffs for update actions
6. Use a timeline layout for chronological activity feeds
7. Implement pagination for large audit logs (never load all records at once)
8. Include a search bar for quick lookup by resource or user name
9. Export audit data to CSV for compliance reporting
10. Use monospace font for technical identifiers (IPs, resource paths)
11. Retain audit logs for the required compliance period (typically 7 years)
12. Show filter chips with clear remove buttons for active filters
13. Distinguish human actions from system/automated actions with icons

## Common Pitfalls

1. **No pagination**: Loading thousands of audit entries crashes the browser. Always paginate server-side.
2. **Missing timezone handling**: Showing timestamps without timezone context creates confusion across global teams.
3. **No filtering**: Large audit logs without filters are unusable. Provide at minimum user and date filters.
4. **Truncated details**: Showing only "updated resource" without field-level changes makes auditing ineffective.
5. **No system event logging**: Ignoring automated actions (backups, cron jobs) creates gaps in the audit trail.
6. **Hardcoded relative times**: "2 min ago" without a full timestamp tooltip is insufficient for compliance.
7. **Missing IP tracking**: Security audits require source IP information for every logged action.

## Accessibility Considerations

- Use `time` element with `datetime` attribute for machine-readable timestamps
- Ensure filter controls have associated `label` elements
- Make the timeline navigable with keyboard using semantic HTML structure
- Use `aria-expanded` on accordion items for audit detail expansion
- Provide `aria-label` on filter chip close buttons
- Use `role="log"` on real-time activity feeds
- Ensure table sorting controls are keyboard accessible

## Responsive Behavior

On mobile, the filter bar should stack using `col-12` instead of `col-md-3`. The audit log table should use `table-responsive` for horizontal scrolling. Timeline cards should use full width. Filter chips should wrap naturally. The accordion detail panels should remain fully functional at all breakpoints. Consider collapsing less important columns (IP address, user agent) on small screens.
