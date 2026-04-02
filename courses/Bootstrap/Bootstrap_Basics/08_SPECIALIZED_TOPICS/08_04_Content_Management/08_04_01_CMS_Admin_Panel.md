---
title: "CMS Admin Panel"
module: "Content Management"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["04_01_Card_Component", "04_04_Table", "04_06_Nav_And_Tabs"]
---

## Overview

A CMS admin panel provides the interface for creating, editing, managing, and publishing content. Bootstrap 5 tables for content listings, badges for status indicators, tabs for content sections, modals for confirmations, and form controls for editors combine into a complete content management experience.

## Basic Implementation

### Content List Table

```html
<div class="card">
  <div class="card-header bg-white d-flex flex-wrap justify-content-between align-items-center gap-2">
    <h5 class="mb-0">All Content</h5>
    <div class="d-flex gap-2">
      <ul class="nav nav-pills card-header-pills">
        <li class="nav-item"><a class="nav-link active small" href="#">All (42)</a></li>
        <li class="nav-item"><a class="nav-link small" href="#">Published (28)</a></li>
        <li class="nav-item"><a class="nav-link small" href="#">Drafts (10)</a></li>
        <li class="nav-item"><a class="nav-link small" href="#">Trash (4)</a></li>
      </ul>
      <a href="#" class="btn btn-primary btn-sm"><i class="bi bi-plus me-1"></i>New Content</a>
    </div>
  </div>
  <div class="table-responsive">
    <table class="table table-hover align-middle mb-0">
      <thead class="table-light">
        <tr>
          <th style="width:40px"><input class="form-check-input" type="checkbox"></th>
          <th>Title</th>
          <th>Author</th>
          <th>Status</th>
          <th>Type</th>
          <th>Last Modified</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td><input class="form-check-input" type="checkbox"></td>
          <td>
            <div class="fw-semibold">Getting Started with Bootstrap 5</div>
            <small class="text-muted">/blog/getting-started-bootstrap-5</small>
          </td>
          <td>
            <div class="d-flex align-items-center">
              <div class="bg-primary text-white rounded-circle d-flex align-items-center justify-content-center me-2" style="width:28px;height:28px;font-size:0.7em">JD</div>
              <span class="small">John Doe</span>
            </div>
          </td>
          <td><span class="badge bg-success">Published</span></td>
          <td><span class="badge bg-light text-dark">Blog Post</span></td>
          <td class="text-muted small">Mar 15, 2024</td>
          <td>
            <div class="dropdown">
              <button class="btn btn-sm btn-outline-secondary" data-bs-toggle="dropdown"><i class="bi bi-three-dots-vertical"></i></button>
              <ul class="dropdown-menu dropdown-menu-end">
                <li><a class="dropdown-item" href="#"><i class="bi bi-pencil me-2"></i>Edit</a></li>
                <li><a class="dropdown-item" href="#"><i class="bi bi-eye me-2"></i>View</a></li>
                <li><a class="dropdown-item" href="#"><i class="bi bi-copy me-2"></i>Duplicate</a></li>
                <li><hr class="dropdown-divider"></li>
                <li><a class="dropdown-item text-danger" href="#"><i class="bi bi-trash me-2"></i>Move to Trash</a></li>
              </ul>
            </div>
          </td>
        </tr>
        <tr>
          <td><input class="form-check-input" type="checkbox"></td>
          <td>
            <div class="fw-semibold">Product Launch Announcement</div>
            <small class="text-muted">/announcements/product-launch</small>
          </td>
          <td>
            <div class="d-flex align-items-center">
              <div class="bg-success text-white rounded-circle d-flex align-items-center justify-content-center me-2" style="width:28px;height:28px;font-size:0.7em">AS</div>
              <span class="small">Alice Smith</span>
            </div>
          </td>
          <td><span class="badge bg-warning text-dark">Draft</span></td>
          <td><span class="badge bg-light text-dark">Page</span></td>
          <td class="text-muted small">Mar 14, 2024</td>
          <td>
            <div class="dropdown">
              <button class="btn btn-sm btn-outline-secondary" data-bs-toggle="dropdown"><i class="bi bi-three-dots-vertical"></i></button>
              <ul class="dropdown-menu dropdown-menu-end">
                <li><a class="dropdown-item" href="#"><i class="bi bi-pencil me-2"></i>Edit</a></li>
                <li><a class="dropdown-item" href="#"><i class="bi bi-send me-2"></i>Publish</a></li>
              </ul>
            </div>
          </td>
        </tr>
        <tr>
          <td><input class="form-check-input" type="checkbox"></td>
          <td>
            <div class="fw-semibold">Q1 2024 Newsletter</div>
            <small class="text-muted">/newsletters/q1-2024</small>
          </td>
          <td>
            <div class="d-flex align-items-center">
              <div class="bg-warning text-white rounded-circle d-flex align-items-center justify-content-center me-2" style="width:28px;height:28px;font-size:0.7em">BJ</div>
              <span class="small">Bob Johnson</span>
            </div>
          </td>
          <td><span class="badge bg-info">Scheduled</span></td>
          <td><span class="badge bg-light text-dark">Newsletter</span></td>
          <td class="text-muted small">Mar 13, 2024</td>
          <td>
            <div class="dropdown">
              <button class="btn btn-sm btn-outline-secondary" data-bs-toggle="dropdown"><i class="bi bi-three-dots-vertical"></i></button>
              <ul class="dropdown-menu dropdown-menu-end">
                <li><a class="dropdown-item" href="#"><i class="bi bi-pencil me-2"></i>Edit</a></li>
                <li><a class="dropdown-item" href="#"><i class="bi bi-calendar me-2"></i>Reschedule</a></li>
              </ul>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</div>
```

### Editor Interface Preview

```html
<div class="card">
  <div class="card-header bg-white d-flex justify-content-between align-items-center">
    <div class="d-flex gap-2">
      <button class="btn btn-sm btn-outline-secondary active">Edit</button>
      <button class="btn btn-sm btn-outline-secondary">Preview</button>
    </div>
    <div class="d-flex gap-2">
      <button class="btn btn-sm btn-outline-secondary">Save Draft</button>
      <button class="btn btn-sm btn-primary">Publish</button>
    </div>
  </div>
  <div class="card-body">
    <div class="mb-3">
      <input type="text" class="form-control form-control-lg border-0 px-0" placeholder="Enter title..." value="Getting Started with Bootstrap 5">
    </div>
    <div class="border rounded p-4 bg-light" style="min-height:300px">
      <p class="text-muted">Rich text editor placeholder - integrate with TinyMCE, Quill, or similar.</p>
    </div>
  </div>
</div>
```

## Advanced Variations

### Publishing Workflow Sidebar

```html
<div class="card">
  <div class="card-header bg-white"><h6 class="mb-0">Publish</h6></div>
  <div class="card-body">
    <div class="mb-3">
      <label class="form-label small fw-semibold">Status</label>
      <select class="form-select form-select-sm">
        <option>Draft</option>
        <option selected>Published</option>
        <option>Scheduled</option>
        <option>Pending Review</option>
      </select>
    </div>
    <div class="mb-3">
      <label class="form-label small fw-semibold">Publish Date</label>
      <input type="datetime-local" class="form-control form-control-sm" value="2024-03-15T10:00">
    </div>
    <div class="mb-3">
      <label class="form-label small fw-semibold">Visibility</label>
      <select class="form-select form-select-sm">
        <option>Public</option>
        <option>Password Protected</option>
        <option>Private</option>
      </select>
    </div>
    <div class="d-flex gap-2">
      <button class="btn btn-outline-secondary btn-sm flex-grow-1">Save Draft</button>
      <button class="btn btn-primary btn-sm flex-grow-1">Update</button>
    </div>
  </div>
</div>
```

## Best Practices

1. Show content status with badges: Published (green), Draft (yellow), Scheduled (blue), Trash (red)
2. Provide filter tabs for content states (All, Published, Drafts, Trash)
3. Include slug/URL below the title for SEO context
4. Use dropdown menus for row actions to keep the table clean
5. Provide Edit/Preview toggle in the editor header
6. Include a publishing workflow sidebar with status, date, and visibility
7. Show last modified timestamp and author on each row
8. Provide "Save Draft" separate from "Publish" to prevent accidental publishing
9. Include checkbox column for bulk actions
10. Use breadcrumb navigation for editor context

## Common Pitfalls

1. **No draft auto-save** - Users lose work if they navigate away. Auto-save periodically.
2. **Accidental publishing** - Separate "Save Draft" and "Publish" buttons with clear labels.
3. **No content preview** - Users need to see how content looks before publishing.
4. **Missing trash/restore** - Deleted content should go to trash, not be permanently deleted.
5. **No bulk actions** - Managing many items without multi-select is tedious.
6. **Editor doesn't show status** - Users don't know if they're editing a draft or published content.

## Accessibility Considerations

- Use `aria-label="Content actions for 'Title'"` on dropdown buttons
- Mark status badges with `aria-label="Status: Published"`
- Use `aria-sort` on sortable table columns
- Label all editor toolbar buttons
- Announce save/publish success with `aria-live="polite"`

## Responsive Behavior

On **mobile**, the content table converts to a card layout showing title, status, and actions. Filter tabs become a horizontal scrollable row. The editor goes full-width with the publishing sidebar collapsing into an accordion. On **tablet and desktop**, the full table and side-by-side editor layout display.
