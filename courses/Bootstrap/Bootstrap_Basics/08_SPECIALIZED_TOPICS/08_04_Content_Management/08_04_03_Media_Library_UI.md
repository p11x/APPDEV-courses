---
title: "Media Library UI"
module: "Content Management"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["04_01_Card_Component", "04_07_Modal", "04_03_Offcanvas"]
---

## Overview

A media library provides a centralized interface for uploading, organizing, and selecting media files. Bootstrap 5 grid for thumbnails, modals for previews, offcanvas for upload panels, and dropdowns for actions create a complete media management experience.

## Basic Implementation

### Media Grid with Upload

```html
<div class="d-flex justify-content-between align-items-center mb-4">
  <h4>Media Library</h4>
  <div class="d-flex gap-2">
    <div class="btn-group btn-group-sm" role="group">
      <input type="radio" class="btn-check" name="view" id="gridView" checked>
      <label class="btn btn-outline-secondary" for="gridView"><i class="bi bi-grid-3x3"></i></label>
      <input type="radio" class="btn-check" name="view" id="listView">
      <label class="btn btn-outline-secondary" for="listView"><i class="bi bi-list"></i></label>
    </div>
    <button class="btn btn-primary btn-sm" data-bs-toggle="offcanvas" data-bs-target="#uploadPanel">
      <i class="bi bi-cloud-upload me-1"></i>Upload
    </button>
  </div>
</div>

<!-- Upload Drop Zone -->
<div class="border-2 border-dashed rounded p-4 text-center mb-4 bg-light" id="dropZone">
  <i class="bi bi-cloud-arrow-up display-4 text-muted"></i>
  <p class="mb-1">Drag and drop files here</p>
  <p class="text-muted small mb-2">or</p>
  <button class="btn btn-outline-primary btn-sm">Browse Files</button>
  <p class="text-muted small mt-2 mb-0">Max 10MB per file. Supported: JPG, PNG, GIF, SVG, PDF, MP4</p>
</div>

<!-- Media Grid -->
<div class="row row-cols-2 row-cols-sm-3 row-cols-md-4 row-cols-lg-6 g-3">
  <div class="col">
    <div class="card h-100" role="button" data-bs-toggle="modal" data-bs-target="#imagePreview">
      <div class="position-relative">
        <img src="media/photo-1.jpg" class="card-img-top" alt="Product photo" style="height:120px;object-fit:cover" loading="lazy">
        <div class="position-absolute top-0 end-0 m-1">
          <span class="badge bg-dark small">JPG</span>
        </div>
      </div>
      <div class="card-body p-2">
        <p class="card-text small text-truncate mb-0">product-photo-01.jpg</p>
        <small class="text-muted">245 KB</small>
      </div>
    </div>
  </div>
  <div class="col">
    <div class="card h-100" role="button">
      <div class="position-relative">
        <img src="media/photo-2.jpg" class="card-img-top" alt="Team photo" style="height:120px;object-fit:cover" loading="lazy">
        <div class="position-absolute top-0 end-0 m-1">
          <span class="badge bg-dark small">PNG</span>
        </div>
      </div>
      <div class="card-body p-2">
        <p class="card-text small text-truncate mb-0">team-photo.png</p>
        <small class="text-muted">1.2 MB</small>
      </div>
    </div>
  </div>
  <div class="col">
    <div class="card h-100 bg-light d-flex align-items-center justify-content-center" role="button" style="height:160px">
      <div class="text-center text-muted">
        <i class="bi bi-file-pdf fs-2 d-block mb-1 text-danger"></i>
        <p class="small text-truncate mb-0">annual-report.pdf</p>
        <small>3.4 MB</small>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Image Preview Modal

```html
<div class="modal fade" id="imagePreview" tabindex="-1">
  <div class="modal-dialog modal-lg">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">product-photo-01.jpg</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>
      <div class="modal-body p-0">
        <img src="media/photo-1.jpg" alt="Product photo" class="img-fluid w-100">
      </div>
      <div class="modal-footer d-flex justify-content-between">
        <div class="text-muted small">
          <span>JPG &bull; 245 KB &bull; 1920x1080 &bull; Uploaded Mar 15, 2024</span>
        </div>
        <div class="d-flex gap-2">
          <button class="btn btn-outline-secondary btn-sm"><i class="bi bi-link-45deg me-1"></i>Copy URL</button>
          <button class="btn btn-outline-secondary btn-sm"><i class="bi bi-download me-1"></i>Download</button>
          <button class="btn btn-outline-danger btn-sm"><i class="bi bi-trash me-1"></i>Delete</button>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Upload Progress Panel

```html
<div class="offcanvas offcanvas-end" tabindex="-1" id="uploadPanel">
  <div class="offcanvas-header">
    <h5 class="offcanvas-title">Upload Files</h5>
    <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
  </div>
  <div class="offcanvas-body">
    <div class="border-2 border-dashed rounded p-4 text-center mb-4">
      <i class="bi bi-cloud-upload fs-1 text-muted"></i>
      <p class="mb-0">Drop files here</p>
    </div>
    <!-- Upload Progress -->
    <div class="mb-3">
      <div class="d-flex align-items-center mb-2">
        <div class="flex-grow-1">
          <div class="small fw-semibold">product-photo.jpg</div>
          <div class="progress mt-1" style="height:4px">
            <div class="progress-bar bg-success" style="width:100%"></div>
          </div>
        </div>
        <i class="bi bi-check-circle text-success ms-2"></i>
      </div>
    </div>
    <div class="mb-3">
      <div class="d-flex align-items-center mb-2">
        <div class="flex-grow-1">
          <div class="small fw-semibold">banner-image.png</div>
          <div class="progress mt-1" style="height:4px">
            <div class="progress-bar progress-bar-striped progress-bar-animated" style="width:65%"></div>
          </div>
          <small class="text-muted">65% - 1.2 MB / 1.8 MB</small>
        </div>
        <button class="btn btn-sm btn-link text-danger ms-2 p-0"><i class="bi bi-x-circle"></i></button>
      </div>
    </div>
  </div>
</div>
```

### Folder Navigation

```html
<div class="mb-4">
  <nav aria-label="Media folders">
    <ol class="breadcrumb mb-2">
      <li class="breadcrumb-item"><a href="#">All Media</a></li>
      <li class="breadcrumb-item"><a href="#">Images</a></li>
      <li class="breadcrumb-item active">Products</li>
    </ol>
  </nav>
  <div class="d-flex gap-2 flex-wrap">
    <span class="badge bg-light text-dark border py-2 px-3">
      <i class="bi bi-folder me-1"></i>Images (248)
    </span>
    <span class="badge bg-light text-dark border py-2 px-3">
      <i class="bi bi-folder me-1"></i>Documents (34)
    </span>
    <span class="badge bg-light text-dark border py-2 px-3">
      <i class="bi bi-folder me-1"></i>Videos (12)
    </span>
    <span class="badge bg-light text-dark border py-2 px-3">
      <i class="bi bi-folder-plus me-1"></i>New Folder
    </span>
  </div>
</div>
```

## Best Practices

1. Provide both grid and list view toggles
2. Use a drag-and-drop upload zone with a fallback browse button
3. Show file type badges (JPG, PNG, PDF) on thumbnails
4. Display file size and upload date in preview modals
5. Include upload progress with cancel option
6. Provide "Copy URL" and "Download" actions in the preview
7. Use breadcrumbs for folder navigation
8. Lazy-load thumbnail images for performance
9. Support multi-file upload with individual progress bars
10. Show file metadata (dimensions, size, date) in the preview modal

## Common Pitfalls

1. **No upload progress** - Users don't know if uploads are working. Always show progress.
2. **Missing file type validation** - Accepting any file type can cause issues. Validate on upload.
3. **No preview for non-image files** - PDFs and documents need file-type icons.
4. **Thumbnails too small** - Users need to identify files visually. Use at least 120px height.
5. **No folder organization** - Large media libraries need folder structure for organization.
6. **Missing alt text field** - Accessibility requires alt text on images. Provide an edit option.

## Accessibility Considerations

- Use `alt` text on all thumbnail images
- Provide `aria-label` on file cards (e.g., "product-photo.jpg, 245 KB, JPG image")
- Mark the upload drop zone with `aria-label="Upload files by dragging here or browsing"`
- Use `role="grid"` on the media grid
- Announce upload completion with `aria-live="polite"`

## Responsive Behavior

On **mobile**, the grid shows 2 columns. Upload panel becomes full-width. On **tablet**, 3-4 columns. On **desktop**, 6 columns for compact thumbnails. The preview modal scales to `modal-lg` on larger screens. Folder navigation wraps on small screens.
