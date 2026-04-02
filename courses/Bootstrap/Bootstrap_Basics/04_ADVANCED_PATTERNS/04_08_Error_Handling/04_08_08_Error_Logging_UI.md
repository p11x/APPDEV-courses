---
title: "Error Logging UI"
description: "Displaying error logs with filtering, searching, and expandable detail views in Bootstrap"
difficulty: 2
tags: ["error-handling", "logging", "admin", "ui", "bootstrap"]
prerequisites: ["04_08_07_Toast_Notifications_Errors"]
---

## Overview

Production applications need an error log interface for developers and administrators to diagnose issues. Bootstrap's table, badge, accordion, and form components provide the building blocks for a filterable, searchable error log display. Each log entry shows severity, timestamp, message, and expandable details including stack traces, request context, and user information.

A well-designed error logging UI reduces mean time to resolution by making errors easy to find, filter, and investigate without requiring SSH access to server logs.

## Basic Implementation

```html
<!-- Basic error log table -->
<div class="container py-4">
  <h2 class="mb-4">Error Log</h2>

  <!-- Filters -->
  <div class="row g-3 mb-4">
    <div class="col-md-4">
      <input type="search" class="form-control" placeholder="Search errors..." id="searchInput">
    </div>
    <div class="col-md-3">
      <select class="form-select" id="severityFilter">
        <option value="">All Severities</option>
        <option value="critical">Critical</option>
        <option value="error">Error</option>
        <option value="warning">Warning</option>
      </select>
    </div>
    <div class="col-md-3">
      <input type="date" class="form-control" id="dateFilter">
    </div>
    <div class="col-md-2">
      <button class="btn btn-primary w-100" id="applyFilters">Filter</button>
    </div>
  </div>

  <!-- Log entries -->
  <div class="table-responsive">
    <table class="table table-hover">
      <thead class="table-dark">
        <tr>
          <th>Time</th>
          <th>Severity</th>
          <th>Message</th>
          <th>Source</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody id="logTableBody">
        <tr>
          <td class="text-nowrap">2024-01-15 14:32:01</td>
          <td><span class="badge bg-danger">Critical</span></td>
          <td>Database connection timeout after 30s</td>
          <td><code>api/users</code></td>
          <td>
            <button class="btn btn-sm btn-outline-primary" data-bs-toggle="collapse"
                    data-bs-target="#log-detail-1">Details</button>
          </td>
        </tr>
        <tr class="collapse" id="log-detail-1">
          <td colspan="5">
            <div class="card card-body bg-light">
              <h6>Stack Trace</h6>
              <pre class="bg-dark text-light p-3 rounded small"><code>Error: Connection timeout
    at Pool.connect (/app/db/pool.js:45:19)
    at UserService.findById (/app/services/user.js:12:22)
    at GET /api/users/:id (/app/routes/users.js:8:5)</code></pre>
              <div class="row mt-3">
                <div class="col-md-6">
                  <strong>Request ID:</strong> <code>req_abc123</code><br>
                  <strong>User ID:</strong> <code>usr_456</code>
                </div>
                <div class="col-md-6">
                  <strong>Environment:</strong> production<br>
                  <strong>Server:</strong> web-03
                </div>
              </div>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</div>
```

```js
// Error log filtering and rendering
const errorLogs = [
  {
    id: 1,
    timestamp: '2024-01-15T14:32:01Z',
    severity: 'critical',
    message: 'Database connection timeout after 30s',
    source: 'api/users',
    stack: 'Error: Connection timeout\n    at Pool.connect...',
    requestId: 'req_abc123'
  },
  {
    id: 2,
    timestamp: '2024-01-15T14:28:45Z',
    severity: 'error',
    message: 'Invalid JWT token on /api/profile',
    source: 'middleware/auth',
    stack: 'JsonWebTokenError: invalid signature...',
    requestId: 'req_def456'
  }
];

function renderLogs(logs) {
  const tbody = document.getElementById('logTableBody');
  tbody.innerHTML = '';

  logs.forEach(log => {
    const severityClass = {
      critical: 'bg-danger',
      error: 'bg-warning text-dark',
      warning: 'bg-info'
    }[log.severity] || 'bg-secondary';

    tbody.innerHTML += `
      <tr>
        <td class="text-nowrap">${new Date(log.timestamp).toLocaleString()}</td>
        <td><span class="badge ${severityClass}">${log.severity}</span></td>
        <td>${log.message}</td>
        <td><code>${log.source}</code></td>
        <td>
          <button class="btn btn-sm btn-outline-primary" data-bs-toggle="collapse"
                  data-bs-target="#log-${log.id}">Details</button>
        </td>
      </tr>
      <tr class="collapse" id="log-${log.id}">
        <td colspan="5">
          <div class="card card-body bg-light">
            <pre class="bg-dark text-light p-3 rounded small"><code>${log.stack}</code></pre>
            <small class="text-muted mt-2">Request ID: ${log.requestId}</small>
          </div>
        </td>
      </tr>`;
  });
}

document.getElementById('applyFilters').addEventListener('click', () => {
  const search = document.getElementById('searchInput').value.toLowerCase();
  const severity = document.getElementById('severityFilter').value;

  const filtered = errorLogs.filter(log => {
    const matchesSearch = !search || log.message.toLowerCase().includes(search);
    const matchesSeverity = !severity || log.severity === severity;
    return matchesSearch && matchesSeverity;
  });

  renderLogs(filtered);
});
```

## Advanced Variations

```js
// Paginated error log with real-time updates
class ErrorLogDashboard {
  constructor(container) {
    this.container = container;
    this.page = 1;
    this.perPage = 20;
    this.filters = {};
    this.init();
  }

  init() {
    this.loadLogs();
    this.bindEvents();
    this.startPolling();
  }

  async loadLogs() {
    const params = new URLSearchParams({
      page: this.page,
      per_page: this.perPage,
      ...this.filters
    });

    const response = await fetch(`/api/errors?${params}`);
    const data = await response.json();

    this.renderTable(data.errors);
    this.renderPagination(data.total);
    this.updateStats(data.summary);
  }

  renderTable(errors) {
    const tbody = this.container.querySelector('tbody');
    tbody.innerHTML = errors.map(err => `
      <tr class="${err.severity === 'critical' ? 'table-danger' : ''}">
        <td>${new Date(err.timestamp).toLocaleString()}</td>
        <td><span class="badge ${this.getSeverityClass(err.severity)}">${err.severity}</span></td>
        <td class="text-truncate" style="max-width: 300px;" title="${err.message}">${err.message}</td>
        <td><code>${err.source}</code></td>
        <td>
          <button class="btn btn-sm btn-outline-primary" data-bs-toggle="modal"
                  data-bs-target="#errorDetailModal" data-error-id="${err.id}">
            View
          </button>
        </td>
      </tr>
    `).join('');
  }

  renderPagination(total) {
    const pages = Math.ceil(total / this.perPage);
    const pagination = this.container.querySelector('.pagination');

    let html = `<li class="page-item ${this.page === 1 ? 'disabled' : ''}">
      <a class="page-link" href="#" data-page="${this.page - 1}">Previous</a>
    </li>`;

    for (let i = 1; i <= pages; i++) {
      html += `<li class="page-item ${i === this.page ? 'active' : ''}">
        <a class="page-link" href="#" data-page="${i}">${i}</a>
      </li>`;
    }

    html += `<li class="page-item ${this.page === pages ? 'disabled' : ''}">
      <a class="page-link" href="#" data-page="${this.page + 1}">Next</a>
    </li>`;

    pagination.innerHTML = html;
  }

  startPolling() {
    setInterval(() => this.loadLogs(), 30000);
  }

  getSeverityClass(severity) {
    return { critical: 'bg-danger', error: 'bg-warning text-dark', warning: 'bg-info' }[severity];
  }
}
```

## Best Practices

1. Show severity as colored badges — red for critical, yellow for error, blue for warning
2. Use monospace `<code>` tags for source paths, request IDs, and technical identifiers
3. Make stack traces collapsible to avoid overwhelming the table layout
4. Implement search that filters across message, source, and request ID fields
5. Paginate results — loading 1000+ log entries freezes the browser
6. Auto-refresh the log display every 30-60 seconds for real-time monitoring
7. Use `table-danger` row highlighting for critical severity entries
8. Include request context (user ID, environment, server) in expanded details
9. Provide a date range filter to narrow results to specific incidents
10. Allow copying stack traces with a single click for sharing in bug reports

## Common Pitfalls

1. **No pagination** — Rendering thousands of log rows crashes the browser; always paginate
2. **Missing severity filter** — Scrolling through warnings to find critical errors wastes time
3. **Truncated messages without tooltips** — `text-truncation` hides important context; add `title` attributes
4. **No auto-refresh** — Stale logs miss recent errors; poll or use WebSockets for updates
5. **Stack trace in plain text** — Use `<pre><code>` for readable formatting and copy support
6. **Filtering resets on page change** — Preserve filter state across pagination navigation

## Accessibility Considerations

Log tables need proper `<thead>` and `<th scope="col">` markup for screen reader navigation. Expandable detail rows should use `aria-expanded` on the toggle button. Use `aria-label` on action buttons like "View details for error: [message excerpt]". Ensure sufficient color contrast on severity badges.

## Responsive Behavior

On mobile, the log table should scroll horizontally using `table-responsive` or switch to a card-based layout. Stack filter controls vertically on narrow viewports. Use `text-truncation` on long messages with tooltips to prevent horizontal overflow. Collapsible detail sections should be full-width on mobile.
