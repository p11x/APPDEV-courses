---
title: Dashboard Export
category: [Dashboards, Analytics]
difficulty: 2
time: 25 min
tags: bootstrap5, export, pdf, csv, print, share, dashboards
---

## Overview

Dashboard export features let users download reports, print formatted views, and share dashboard links with preserved state. This guide covers implementing export-to-PDF/CSV buttons, print-optimized layouts, and shareable dashboard URLs using Bootstrap components.

## Basic Implementation

Adding export action buttons to a dashboard toolbar using Bootstrap button groups.

```html
<div class="d-flex flex-wrap justify-content-between align-items-center mb-4">
  <h3 class="mb-2 mb-md-0">Sales Dashboard</h3>
  <div class="btn-group" role="group" aria-label="Export actions">
    <button class="btn btn-outline-primary" id="exportPdf">
      <i class="bi bi-file-pdf me-1"></i> PDF
    </button>
    <button class="btn btn-outline-primary" id="exportCsv">
      <i class="bi bi-file-spreadsheet me-1"></i> CSV
    </button>
    <button class="btn btn-outline-primary" onclick="window.print()">
      <i class="bi bi-printer me-1"></i> Print
    </button>
    <button class="btn btn-outline-primary" id="shareLink">
      <i class="bi bi-share me-1"></i> Share
    </button>
  </div>
</div>
```

A share link modal that generates a URL with query parameters preserving dashboard state.

```html
<div class="modal fade" id="shareModal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Share Dashboard</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>
      <div class="modal-body">
        <label for="shareUrl" class="form-label">Dashboard Link</label>
        <div class="input-group">
          <input type="text" class="form-control" id="shareUrl" readonly>
          <button class="btn btn-primary" id="copyShareUrl">Copy</button>
        </div>
        <div class="form-check mt-3">
          <input class="form-check-input" type="checkbox" id="includeFilters" checked>
          <label class="form-check-label" for="includeFilters">
            Include current filters and date range
          </label>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

CSV export implementation that collects table data and triggers a download.

```html
<table class="table table-striped" id="salesTable">
  <thead class="table-dark">
    <tr>
      <th>Product</th>
      <th>Region</th>
      <th>Revenue</th>
      <th>Units</th>
    </tr>
  </thead>
  <tbody>
    <tr><td>Widget A</td><td>North</td><td>$12,400</td><td>312</td></tr>
    <tr><td>Widget B</td><td>South</td><td>$8,900</td><td>198</td></tr>
    <tr><td>Widget C</td><td>East</td><td>$15,200</td><td>405</td></tr>
  </tbody>
</table>

<script>
document.getElementById('exportCsv').addEventListener('click', () => {
  const table = document.getElementById('salesTable');
  const rows = Array.from(table.querySelectorAll('tr'));
  const csv = rows.map(row => {
    const cells = Array.from(row.querySelectorAll('th, td'));
    return cells.map(c => `"${c.textContent.trim()}"`).join(',');
  }).join('\n');

  const blob = new Blob([csv], { type: 'text/csv' });
  const url = URL.createObjectURL(blob);
  const a = document.createElement('a');
  a.href = url;
  a.download = `dashboard-export-${new Date().toISOString().slice(0,10)}.csv`;
  a.click();
  URL.revokeObjectURL(url);
});
</script>
```

Print-specific styles using a dedicated stylesheet or `@media print` block.

```html
<style>
  @media print {
    .navbar, .sidebar, .btn-group, .no-print {
      display: none !important;
    }
    .card {
      break-inside: avoid;
      border: 1px solid #ddd !important;
      box-shadow: none !important;
    }
    .col-md-6 {
      flex: 0 0 50%;
      max-width: 50%;
    }
    body {
      font-size: 12pt;
      color: #000;
    }
  }
</style>

<div class="container-fluid">
  <div class="row g-3">
    <div class="col-md-6">
      <div class="card">
        <div class="card-header">Revenue</div>
        <div class="card-body">
          <p class="display-6">$142,800</p>
          <p class="text-success">+12.4% vs last month</p>
        </div>
      </div>
    </div>
    <div class="col-md-6">
      <div class="card">
        <div class="card-header">Orders</div>
        <div class="card-body">
          <p class="display-6">3,241</p>
          <p class="text-danger">-2.1% vs last month</p>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Group export buttons using Bootstrap `btn-group` for visual cohesion
2. Use `window.print()` for native print functionality without extra libraries
3. Include `@media print` styles to hide navigation and non-essential UI
4. Set `break-inside: avoid` on cards to prevent awkward page splits when printing
5. Generate CSV with quoted fields to handle commas in cell values
6. Append timestamps to exported filenames for version tracking
7. Use `URL.createObjectURL` for client-side file generation without server round-trips
8. Preserve dashboard filters in share URLs using `URLSearchParams`
9. Provide copy-to-clipboard feedback with a temporary button label change
10. Use `loading="lazy"` patterns for heavy export operations with progress indicators
11. Disable export buttons during generation to prevent duplicate requests
12. Sanitize shared URL parameters to prevent injection attacks

## Common Pitfalls

1. **No print styles** — Navigation and sidebars clutter printed output
2. **CSV injection** — Unescaped cell values starting with `=` can execute formulas in spreadsheet apps
3. **Missing clipboard fallback** — `navigator.clipboard` is unavailable in non-HTTPS contexts
4. **Large CSV blocking UI** — Synchronous generation of large datasets freezes the browser
5. **Hardcoded share URLs** — Breaks when deployment domain or path changes
6. **Not encoding filter state** — Special characters in filters break shared URLs
7. **Forgetting `break-inside: avoid`** — Cards split across printed pages
8. **Missing filename dates** — Multiple exports overwrite each other

## Accessibility Considerations

Label all export buttons with descriptive text including the format (e.g., "Export as PDF"). Use `aria-label` on icon-only buttons. Ensure the share modal traps focus and is keyboard navigable. Announce export completion using `aria-live` regions. Provide screen-reader-only text for status updates like "CSV download started."

## Responsive Behavior

On small screens, stack export buttons vertically using `btn-group-vertical` or wrap with `flex-wrap`. Collapse multi-button toolbars into a dropdown menu on mobile using Bootstrap's dropdown component. Ensure print layouts remain readable at A4/Letter dimensions. Test share modal on narrow viewports to verify input field usability.
