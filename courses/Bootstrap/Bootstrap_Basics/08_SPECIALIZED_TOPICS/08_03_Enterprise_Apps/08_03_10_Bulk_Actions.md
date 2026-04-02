---
title: "Bulk Actions"
description: "Build bulk selection interfaces with checkbox tables, action toolbars, selection counts, and batch operations using Bootstrap 5."
difficulty: 2
estimated_time: "30 minutes"
prerequisites:
  - "Bootstrap 5 Tables"
  - "Bootstrap 5 Forms"
  - "Bootstrap 5 Button Group"
  - "Bootstrap 5 Modals"
---

## Overview

Bulk actions enable users to select multiple items from a list and perform operations on them simultaneously, such as delete, export, change status, or assign. Bootstrap 5's table, form check, button group, and modal components create a complete bulk action interface.

The pattern includes a "select all" checkbox in the header, individual row checkboxes, a floating or sticky action toolbar that appears when items are selected, a selection count indicator, and confirmation dialogs for destructive actions.

## Basic Implementation

### Table with Checkbox Selection

```html
<div class="table-responsive">
  <table class="table table-hover align-middle">
    <thead class="table-light">
      <tr>
        <th style="width: 40px;">
          <input class="form-check-input" type="checkbox" id="selectAll" aria-label="Select all rows">
        </th>
        <th>Name</th>
        <th>Status</th>
        <th>Date</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td><input class="form-check-input row-check" type="checkbox" aria-label="Select row"></td>
        <td>Project Alpha</td>
        <td><span class="badge bg-success">Active</span></td>
        <td class="text-muted small">Mar 15, 2026</td>
      </tr>
      <tr>
        <td><input class="form-check-input row-check" type="checkbox" aria-label="Select row"></td>
        <td>Project Beta</td>
        <td><span class="badge bg-warning text-dark">Pending</span></td>
        <td class="text-muted small">Mar 20, 2026</td>
      </tr>
      <tr>
        <td><input class="form-check-input row-check" type="checkbox" aria-label="Select row"></td>
        <td>Project Gamma</td>
        <td><span class="badge bg-secondary">Draft</span></td>
        <td class="text-muted small">Apr 1, 2026</td>
      </tr>
    </tbody>
  </table>
</div>
```

### Sticky Action Toolbar

```html
<div class="alert alert-primary d-flex justify-content-between align-items-center py-2 mb-3" role="alert" id="bulkToolbar">
  <div>
    <strong id="selectedCount">3</strong> items selected
  </div>
  <div class="d-flex gap-2">
    <button class="btn btn-sm btn-outline-primary"><i class="bi bi-pencil me-1"></i>Edit</button>
    <button class="btn btn-sm btn-outline-primary"><i class="bi bi-tag me-1"></i>Change Status</button>
    <button class="btn btn-sm btn-outline-primary"><i class="bi bi-download me-1"></i>Export</button>
    <button class="btn btn-sm btn-outline-danger" data-bs-toggle="modal" data-bs-target="#bulkDeleteModal">
      <i class="bi bi-trash me-1"></i>Delete
    </button>
    <button class="btn btn-sm btn-link text-muted" id="clearSelection">Clear</button>
  </div>
</div>
```

### Bulk Delete Confirmation Modal

```html
<div class="modal fade" id="bulkDeleteModal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title text-danger"><i class="bi bi-exclamation-triangle me-1"></i> Confirm Bulk Delete</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <p>Are you sure you want to delete <strong>3 items</strong>?</p>
        <div class="alert alert-danger mb-0">
          <strong>This action cannot be undone.</strong> All selected items and their associated data will be permanently removed.
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button type="button" class="btn btn-danger">Delete 3 Items</button>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Bulk Status Change Dropdown

```html
<div class="btn-group">
  <button class="btn btn-outline-primary btn-sm dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
    <i class="bi bi-tag me-1"></i>Change Status
  </button>
  <ul class="dropdown-menu">
    <li><button class="dropdown-item"><span class="badge bg-success me-1"></span>Active</button></li>
    <li><button class="dropdown-item"><span class="badge bg-warning text-dark me-1"></span>Pending</button></li>
    <li><button class="dropdown-item"><span class="badge bg-secondary me-1"></span>Archived</button></li>
    <li><hr class="dropdown-divider"></li>
    <li><button class="dropdown-item text-danger"><span class="badge bg-danger me-1"></span>Mark as Deleted</button></li>
  </ul>
</div>
```

### Bulk Assign Modal

```html
<div class="modal fade" id="bulkAssignModal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Assign Selected Items</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <p>Assign <strong>3 selected items</strong> to a team member:</p>
        <div class="mb-3">
          <label for="assignee" class="form-label">Assign to</label>
          <select class="form-select" id="assignee">
            <option selected disabled>Choose team member...</option>
            <option>Jane Cooper</option>
            <option>Mike Chen</option>
            <option>Sarah Park</option>
          </select>
        </div>
        <div class="form-check">
          <input class="form-check-input" type="checkbox" id="notifyAssignee" checked>
          <label class="form-check-label" for="notifyAssignee">Send notification to assignee</label>
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button type="button" class="btn btn-primary">Assign Items</button>
      </div>
    </div>
  </div>
</div>
```

### Row Highlight on Selection

```html
<style>
  tr.selected {
    background-color: var(--bs-primary-bg-subtle) !important;
  }
</style>

<div class="table-responsive">
  <table class="table table-hover align-middle">
    <thead class="table-light">
      <tr>
        <th style="width: 40px;"><input class="form-check-input" type="checkbox" id="selectAll2"></th>
        <th>Item</th>
        <th>Owner</th>
        <th>Status</th>
      </tr>
    </thead>
    <tbody>
      <tr class="selected">
        <td><input class="form-check-input row-check" type="checkbox" checked></td>
        <td>Invoice #1042</td>
        <td>Jane Cooper</td>
        <td><span class="badge bg-success">Paid</span></td>
      </tr>
      <tr class="selected">
        <td><input class="form-check-input row-check" type="checkbox" checked></td>
        <td>Invoice #1043</td>
        <td>Mike Chen</td>
        <td><span class="badge bg-warning text-dark">Pending</span></td>
      </tr>
      <tr>
        <td><input class="form-check-input row-check" type="checkbox"></td>
        <td>Invoice #1044</td>
        <td>Sarah Park</td>
        <td><span class="badge bg-danger">Overdue</span></td>
      </tr>
    </tbody>
  </table>
</div>
```

### Selection Range with Shift-Click

```html
<p class="text-muted small">
  <i class="bi bi-info-circle me-1"></i>
  Hold <kbd>Shift</kbd> and click to select a range of items. Hold <kbd>Ctrl</kbd> (or <kbd>Cmd</kbd>) to toggle individual items.
</p>
```

## Best Practices

1. Show the selection count prominently in the action toolbar
2. Provide a "Clear selection" or "Deselect all" action to reset the state
3. Use a confirmation modal for destructive bulk actions like delete
4. Highlight selected rows with a subtle background color
5. Include a "Select all" checkbox in the table header
6. Support Shift+click for range selection on desktop
7. Disable bulk actions that require different permissions than individual actions
8. Show loading state during bulk operations with a progress indicator
9. Display a summary of items to be affected before confirming destructive actions
10. Use consistent checkbox styling with `form-check-input`
11. Make the action toolbar sticky so it remains accessible while scrolling
12. Provide keyboard shortcuts (Ctrl+A for select all) for power users
13. Use `aria-label` on all checkboxes for screen reader accessibility

## Common Pitfalls

1. **No confirmation for bulk delete**: Deleting multiple items without confirmation leads to data loss.
2. **Missing selection count**: Users need to know exactly how many items are selected before acting.
3. **No visual feedback on selected rows**: Without row highlighting, users cannot easily see which items are selected.
4. **Action toolbar disappears on scroll**: If the toolbar is not sticky, users lose access to actions on long lists.
5. **Select all does not account for pagination**: "Select all" should clarify if it selects all pages or just the current page.
6. **No loading state**: Bulk operations on many items take time. Show progress to prevent users from thinking the UI is frozen.
7. **Missing keyboard support**: Enterprise power users expect Ctrl+A, Shift+click, and Escape to clear selection.

## Accessibility Considerations

- Use `aria-label` on all checkboxes describing what they select
- Implement `aria-live="polite"` to announce selection count changes
- Use `role="alert"` on the action toolbar when it appears
- Ensure the "Select all" checkbox communicates its state with `aria-checked`
- Make all action buttons keyboard accessible with visible focus indicators
- Provide `aria-describedby` linking checkboxes to their row content
- Announce bulk operation results using `aria-live` regions

## Responsive Behavior

On mobile, the action toolbar should stack vertically with action buttons on multiple rows. The table should use `table-responsive` for horizontal scrolling. Checkboxes should maintain minimum 44x44px touch targets. Consider hiding less important columns on small screens with `d-none d-md-table-cell`. The bulk action dropdown should remain functional at all breakpoints.
