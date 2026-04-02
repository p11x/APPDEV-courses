---
title: "Content Versioning UI"
module: "Content Management"
difficulty: 3
estimated_time: "30 min"
prerequisites: ["04_04_Table", "04_09_Badges", "04_07_Modal"]
---

## Overview

Content versioning tracks changes over time, allowing users to view history, compare versions, and restore previous states. Bootstrap 5 tables, badges, modals, and grid components build version history lists, diff views, and restore workflows that are essential for enterprise content governance.

## Basic Implementation

### Version History List

```html
<div class="card">
  <div class="card-header bg-white d-flex justify-content-between align-items-center">
    <h5 class="mb-0">Version History</h5>
    <button class="btn btn-outline-primary btn-sm">
      <i class="bi bi-arrow-counterclockwise me-1"></i>Compare Versions
    </button>
  </div>
  <div class="list-group list-group-flush">
    <div class="list-group-item d-flex align-items-center bg-light">
      <div class="me-3">
        <div class="bg-success rounded-circle d-flex align-items-center justify-content-center text-white" style="width:36px;height:36px">
          <i class="bi bi-check"></i>
        </div>
      </div>
      <div class="flex-grow-1">
        <div class="d-flex justify-content-between align-items-center">
          <strong>Version 5 (Current)</strong>
          <span class="badge bg-success">Published</span>
        </div>
        <small class="text-muted">Mar 15, 2024 at 10:30 AM by John Doe</small>
        <div class="small text-muted mt-1">Updated pricing section and added new testimonials.</div>
      </div>
      <div class="d-flex gap-2 ms-3">
        <button class="btn btn-sm btn-outline-secondary" title="View"><i class="bi bi-eye"></i></button>
      </div>
    </div>
    <div class="list-group-item d-flex align-items-center">
      <div class="me-3">
        <div class="bg-light border rounded-circle d-flex align-items-center justify-content-center" style="width:36px;height:36px">
          <span class="small text-muted">4</span>
        </div>
      </div>
      <div class="flex-grow-1">
        <div class="d-flex justify-content-between align-items-center">
          <strong>Version 4</strong>
          <span class="badge bg-secondary">Draft</span>
        </div>
        <small class="text-muted">Mar 14, 2024 at 3:15 PM by Alice Smith</small>
        <div class="small text-muted mt-1">Fixed typos in hero section.</div>
      </div>
      <div class="d-flex gap-2 ms-3">
        <button class="btn btn-sm btn-outline-secondary" title="View"><i class="bi bi-eye"></i></button>
        <button class="btn btn-sm btn-outline-primary" title="Restore" data-bs-toggle="modal" data-bs-target="#restoreModal">
          <i class="bi bi-arrow-counterclockwise"></i>
        </button>
      </div>
    </div>
    <div class="list-group-item d-flex align-items-center">
      <div class="me-3">
        <div class="bg-light border rounded-circle d-flex align-items-center justify-content-center" style="width:36px;height:36px">
          <span class="small text-muted">3</span>
        </div>
      </div>
      <div class="flex-grow-1">
        <div class="d-flex justify-content-between align-items-center">
          <strong>Version 3</strong>
          <span class="badge bg-secondary">Draft</span>
        </div>
        <small class="text-muted">Mar 13, 2024 at 11:00 AM by John Doe</small>
        <div class="small text-muted mt-1">Added feature comparison table.</div>
      </div>
      <div class="d-flex gap-2 ms-3">
        <button class="btn btn-sm btn-outline-secondary" title="View"><i class="bi bi-eye"></i></button>
        <button class="btn btn-sm btn-outline-primary" title="Restore" data-bs-toggle="modal" data-bs-target="#restoreModal">
          <i class="bi bi-arrow-counterclockwise"></i>
        </button>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Diff View

```html
<div class="card">
  <div class="card-header bg-white">
    <div class="d-flex justify-content-between align-items-center">
      <h5 class="mb-0">Comparing Version 4 vs Version 5</h5>
      <button class="btn btn-primary btn-sm">Restore Version 4</button>
    </div>
  </div>
  <div class="card-body p-0">
    <div class="table-responsive">
      <table class="table table-sm mb-0 font-monospace small">
        <tbody>
          <tr>
            <td class="text-muted text-end pe-3" style="width:50px">1</td>
            <td class="bg-danger-subtle text-decoration-line-through">Our product costs $99/month.</td>
          </tr>
          <tr>
            <td class="text-muted text-end pe-3" style="width:50px">1</td>
            <td class="bg-success-subtle">Our product starts at $79/month.</td>
          </tr>
          <tr>
            <td class="text-muted text-end pe-3" style="width:50px">2</td>
            <td>Get started with a free trial.</td>
          </tr>
          <tr>
            <td class="text-muted text-end pe-3" style="width:50px">3</td>
            <td class="bg-success-subtle">"This product changed our workflow completely!" - Jane, TechCorp</td>
          </tr>
          <tr>
            <td class="text-muted text-end pe-3" style="width:50px">4</td>
            <td>Join thousands of satisfied customers.</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
  <div class="card-footer bg-white small">
    <span class="me-3"><span class="badge bg-danger-subtle text-danger me-1"></span>Removed (1 line)</span>
    <span><span class="badge bg-success-subtle text-success me-1"></span>Added (2 lines)</span>
  </div>
</div>
```

### Compare Versions Modal

```html
<div class="modal fade" id="restoreModal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Restore Version 4</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>
      <div class="modal-body">
        <div class="alert alert-warning">
          <i class="bi bi-exclamation-triangle me-2"></i>
          Restoring this version will create a new version (Version 6) with the content from Version 4. The current version will be preserved in the history.
        </div>
        <dl class="row mb-0">
          <dt class="col-sm-4">Version</dt>
          <dd class="col-sm-8">4</dd>
          <dt class="col-sm-4">Date</dt>
          <dd class="col-sm-8">Mar 14, 2024 at 3:15 PM</dd>
          <dt class="col-sm-4">Author</dt>
          <dd class="col-sm-8">Alice Smith</dd>
          <dt class="col-sm-4">Changes</dt>
          <dd class="col-sm-8">Fixed typos in hero section</dd>
        </dl>
      </div>
      <div class="modal-footer">
        <button class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button class="btn btn-primary">Restore This Version</button>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Show version numbers, dates, authors, and change descriptions
2. Highlight the current version with a distinct badge and styling
3. Provide a "View" button on every version to see its content
4. Include a "Restore" button with a confirmation modal
5. Use a diff view (red/green highlighting) for comparing versions
6. Never delete old versions when restoring - create a new version instead
7. Show a summary of changes (added/removed lines count) in the diff footer
8. Use monospace font for diff views for alignment
9. Allow comparing any two versions, not just adjacent ones
10. Preserve author attribution on every version

## Common Pitfalls

1. **Restoring overwrites history** - Always create a new version when restoring, preserving the full timeline.
2. **No diff view** - Comparing versions by reading full content is impractical. Provide side-by-side or inline diffs.
3. **Missing change descriptions** - Users need to understand what changed without reading the diff.
4. **No author attribution** - Track who made each change for accountability.
5. **Version list too long** - Use pagination or grouping by date for long histories.

## Accessibility Considerations

- Use `aria-label="Version 5, current published version"` on version items
- Provide `aria-label="Compare version 4 with version 5"` on compare buttons
- Mark removed lines with `aria-label="Removed line"` and added lines with `aria-label="Added line"`
- Use `role="log"` on the version history list
- Announce restore success with `aria-live="polite"`

## Responsive Behavior

On **mobile**, version history items stack with full-width action buttons. The diff view scrolls horizontally. On **tablet and desktop**, version items display inline with compact action buttons. The diff view has comfortable spacing with line numbers visible.
