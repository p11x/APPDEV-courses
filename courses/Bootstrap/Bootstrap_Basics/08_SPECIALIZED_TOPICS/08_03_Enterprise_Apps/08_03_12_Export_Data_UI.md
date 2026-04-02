---
title: "Export Data UI"
description: "Build data export interfaces with format selection, column pickers, date ranges, and progress indicators using Bootstrap 5."
difficulty: 2
estimated_time: "30 minutes"
prerequisites:
  - "Bootstrap 5 Modals"
  - "Bootstrap 5 Forms"
  - "Bootstrap 5 Progress"
  - "Bootstrap 5 Checks & Radios"
---

## Overview

Export data components allow users to download application data in various formats (CSV, Excel, PDF, JSON) with customizable options including column selection, date range filtering, and row limits. Bootstrap 5's modal, form, and progress components build a complete export workflow.

The export UI should present format options clearly, let users choose which columns to include, offer date range filtering for large datasets, show progress during export generation, and provide a download link upon completion.

## Basic Implementation

### Export Button and Modal Trigger

```html
<button class="btn btn-outline-primary" data-bs-toggle="modal" data-bs-target="#exportModal">
  <i class="bi bi-download me-1"></i> Export Data
</button>
```

### Export Format Selection Modal

```html
<div class="modal fade" id="exportModal" tabindex="-1" aria-labelledby="exportModalLabel">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exportModalLabel">Export Data</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <div class="mb-3">
          <label class="form-label">Export Format</label>
          <div class="row g-2">
            <div class="col-6">
              <input type="radio" class="btn-check" name="exportFormat" id="formatCsv" checked>
              <label class="btn btn-outline-primary w-100" for="formatCsv">
                <i class="bi bi-filetype-csv d-block fs-3 mb-1"></i> CSV
              </label>
            </div>
            <div class="col-6">
              <input type="radio" class="btn-check" name="exportFormat" id="formatXlsx">
              <label class="btn btn-outline-primary w-100" for="formatXlsx">
                <i class="bi bi-file-earmark-excel d-block fs-3 mb-1"></i> Excel
              </label>
            </div>
            <div class="col-6">
              <input type="radio" class="btn-check" name="exportFormat" id="formatPdf">
              <label class="btn btn-outline-primary w-100" for="formatPdf">
                <i class="bi bi-filetype-pdf d-block fs-3 mb-1"></i> PDF
              </label>
            </div>
            <div class="col-6">
              <input type="radio" class="btn-check" name="exportFormat" id="formatJson">
              <label class="btn btn-outline-primary w-100" for="formatJson">
                <i class="bi bi-filetype-json d-block fs-3 mb-1"></i> JSON
              </label>
            </div>
          </div>
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button type="button" class="btn btn-primary">Export</button>
      </div>
    </div>
  </div>
</div>
```

### Column Picker Checklist

```html
<div class="mb-3">
  <div class="d-flex justify-content-between align-items-center mb-2">
    <label class="form-label mb-0">Columns to Export</label>
    <button class="btn btn-link btn-sm p-0" id="toggleAllColumns">Select All</button>
  </div>
  <div class="border rounded p-2" style="max-height: 200px; overflow-y: auto;">
    <div class="form-check">
      <input class="form-check-input col-check" type="checkbox" id="colName" checked>
      <label class="form-check-label" for="colName">Name</label>
    </div>
    <div class="form-check">
      <input class="form-check-input col-check" type="checkbox" id="colEmail" checked>
      <label class="form-check-label" for="colEmail">Email</label>
    </div>
    <div class="form-check">
      <input class="form-check-input col-check" type="checkbox" id="colStatus" checked>
      <label class="form-check-label" for="colStatus">Status</label>
    </div>
    <div class="form-check">
      <input class="form-check-input col-check" type="checkbox" id="colDate">
      <label class="form-check-label" for="colDate">Created Date</label>
    </div>
    <div class="form-check">
      <input class="form-check-input col-check" type="checkbox" id="colAmount">
      <label class="form-check-label" for="colAmount">Amount</label>
    </div>
    <div class="form-check">
      <input class="form-check-input col-check" type="checkbox" id="colNotes">
      <label class="form-check-label" for="colNotes">Notes</label>
    </div>
  </div>
</div>
```

## Advanced Variations

### Date Range and Row Limit

```html
<div class="row g-3 mb-3">
  <div class="col-md-6">
    <label class="form-label">Date Range</label>
    <select class="form-select">
      <option>All time</option>
      <option>Last 7 days</option>
      <option>Last 30 days</option>
      <option selected>Last 90 days</option>
      <option>Custom range</option>
    </select>
  </div>
  <div class="col-md-6">
    <label class="form-label">Row Limit</label>
    <select class="form-select">
      <option>All rows (1,247)</option>
      <option>First 100</option>
      <option>First 500</option>
      <option>First 1000</option>
      <option>Current view (42)</option>
    </select>
  </div>
</div>
<div class="row g-3 mb-3" id="customDateRange" style="display: none;">
  <div class="col-md-6">
    <label class="form-label">From</label>
    <input type="date" class="form-control">
  </div>
  <div class="col-md-6">
    <label class="form-label">To</label>
    <input type="date" class="form-control">
  </div>
</div>
```

### Export Progress Indicator

```html
<div class="card">
  <div class="card-body text-center">
    <div class="spinner-border text-primary mb-3" role="status">
      <span class="visually-hidden">Generating export...</span>
    </div>
    <h6>Generating Export...</h6>
    <p class="text-muted small mb-2">Processing 1,247 rows</p>
    <div class="progress mb-3" style="height: 8px;">
      <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" style="width: 65%;" aria-valuenow="65" aria-valuemin="0" aria-valuemax="100"></div>
    </div>
    <p class="text-muted small mb-0">Estimated time remaining: ~15 seconds</p>
  </div>
</div>
```

### Export Complete with Download

```html
<div class="alert alert-success d-flex align-items-center" role="alert">
  <i class="bi bi-check-circle-fill me-2 fs-4"></i>
  <div class="flex-grow-1">
    <strong>Export Complete!</strong>
    <div class="small">orders_export_2026-04-02.csv (2.4 MB, 1,247 rows)</div>
  </div>
  <a href="#" class="btn btn-success btn-sm"><i class="bi bi-download me-1"></i>Download</a>
</div>
```

### Export History

```html
<div class="card">
  <div class="card-header"><strong>Recent Exports</strong></div>
  <ul class="list-group list-group-flush">
    <li class="list-group-item d-flex justify-content-between align-items-center">
      <div>
        <strong>orders_export.csv</strong>
        <div class="text-muted small">1,247 rows - Apr 2, 2026</div>
      </div>
      <a href="#" class="btn btn-sm btn-outline-primary"><i class="bi bi-download"></i></a>
    </li>
    <li class="list-group-item d-flex justify-content-between align-items-center">
      <div>
        <strong>customers_export.xlsx</strong>
        <div class="text-muted small">856 rows - Mar 30, 2026</div>
      </div>
      <a href="#" class="btn btn-sm btn-outline-primary"><i class="bi bi-download"></i></a>
    </li>
  </ul>
</div>
```

## Best Practices

1. Offer multiple export formats (CSV, Excel, PDF, JSON) with visual icons
2. Include a column picker so users export only relevant data
3. Show estimated row count and file size before export
4. Provide date range presets (last 7 days, 30 days, custom)
5. Display progress bar during export generation
6. Offer a row limit option for large datasets
7. Show export history with re-download capability
8. Use `btn-check` with radio inputs for format selection
9. Support exporting filtered/visible data vs. all data
10. Include file size in the download confirmation
11. Use `progress-bar-animated` for active export progress
12. Auto-delete old exports after a configurable retention period
13. Validate that at least one column is selected before allowing export

## Common Pitfalls

1. **No column selection**: Exporting all columns including internal/system fields exposes unnecessary data.
2. **Missing progress indicator**: Large exports without feedback cause users to think the system is unresponsive.
3. **No row limit option**: Exporting millions of rows without a limit can crash the browser or generate huge files.
4. **Forgetting date range**: Without date filtering, exports include all historical data which is rarely needed.
5. **No export history**: Users who lose their downloaded file have no way to re-download without re-running the export.
6. **Hardcoded format list**: Not supporting PDF when users need printable reports, or JSON for developers, reduces utility.
7. **Missing file size estimate**: Users downloading on slow connections need to know the file size in advance.

## Accessibility Considerations

- Use `btn-check` pattern for format selection with proper `label` association
- Provide `aria-label` on column picker checkboxes
- Use `role="progressbar"` with `aria-valuenow` on the progress indicator
- Announce export completion using `aria-live="polite"` regions
- Ensure the modal is properly labeled with `aria-labelledby`
- Make the column picker scrollable area keyboard accessible
- Provide text descriptions alongside format icons

## Responsive Behavior

On mobile, format selection cards should use `col-6` for a 2x2 grid. The column picker should remain scrollable. The export modal should use `modal-fullscreen-sm-down`. Date range inputs should stack vertically. The export history list should use full-width layout. Download buttons should maintain adequate touch target sizes.
