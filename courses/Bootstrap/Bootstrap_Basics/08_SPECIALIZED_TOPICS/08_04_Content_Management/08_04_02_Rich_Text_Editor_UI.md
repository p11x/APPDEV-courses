---
title: "Rich Text Editor UI"
module: "Content Management"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["04_05_Forms", "04_06_Nav_And_Tabs", "04_07_Modal"]
---

## Overview

Rich text editors allow content creators to format text without writing HTML. Bootstrap 5 provides button groups, dropdowns, and form controls for building editor toolbars, formatting buttons, image upload areas, and preview modes that integrate with libraries like TinyMCE, Quill, or TipTap.

## Basic Implementation

### Editor Toolbar

```html
<div class="card">
  <div class="card-header bg-white p-2">
    <div class="d-flex flex-wrap gap-1 align-items-center">
      <!-- Text Formatting -->
      <div class="btn-group btn-group-sm me-2" role="group" aria-label="Text formatting">
        <button class="btn btn-outline-secondary" title="Bold" aria-label="Bold"><i class="bi bi-type-bold"></i></button>
        <button class="btn btn-outline-secondary" title="Italic" aria-label="Italic"><i class="bi bi-type-italic"></i></button>
        <button class="btn btn-outline-secondary" title="Underline" aria-label="Underline"><i class="bi bi-type-underline"></i></button>
        <button class="btn btn-outline-secondary" title="Strikethrough" aria-label="Strikethrough"><i class="bi bi-type-strikethrough"></i></button>
      </div>

      <!-- Heading Selector -->
      <select class="form-select form-select-sm me-2" style="width:auto" aria-label="Heading level">
        <option>Paragraph</option>
        <option>Heading 1</option>
        <option>Heading 2</option>
        <option>Heading 3</option>
        <option>Heading 4</option>
      </select>

      <!-- Lists -->
      <div class="btn-group btn-group-sm me-2" role="group" aria-label="Lists">
        <button class="btn btn-outline-secondary" title="Bullet List" aria-label="Bullet list"><i class="bi bi-list-ul"></i></button>
        <button class="btn btn-outline-secondary" title="Numbered List" aria-label="Numbered list"><i class="bi bi-list-ol"></i></button>
        <button class="btn btn-outline-secondary" title="Task List" aria-label="Task list"><i class="bi bi-list-check"></i></button>
      </div>

      <!-- Insert -->
      <div class="btn-group btn-group-sm me-2" role="group" aria-label="Insert">
        <button class="btn btn-outline-secondary" title="Link" aria-label="Insert link"><i class="bi bi-link-45deg"></i></button>
        <button class="btn btn-outline-secondary" title="Image" aria-label="Insert image"><i class="bi bi-image"></i></button>
        <button class="btn btn-outline-secondary" title="Table" aria-label="Insert table"><i class="bi bi-table"></i></button>
        <button class="btn btn-outline-secondary" title="Code Block" aria-label="Insert code block"><i class="bi bi-code-slash"></i></button>
      </div>

      <!-- Alignment -->
      <div class="btn-group btn-group-sm me-2" role="group" aria-label="Text alignment">
        <button class="btn btn-outline-secondary" title="Align Left" aria-label="Align left"><i class="bi bi-text-left"></i></button>
        <button class="btn btn-outline-secondary" title="Align Center" aria-label="Align center"><i class="bi bi-text-center"></i></button>
        <button class="btn btn-outline-secondary" title="Align Right" aria-label="Align right"><i class="bi bi-text-right"></i></button>
      </div>

      <!-- Undo/Redo -->
      <div class="btn-group btn-group-sm" role="group" aria-label="History">
        <button class="btn btn-outline-secondary" title="Undo" aria-label="Undo"><i class="bi bi-arrow-counterclockwise"></i></button>
        <button class="btn btn-outline-secondary" title="Redo" aria-label="Redo"><i class="bi bi-arrow-clockwise"></i></button>
      </div>
    </div>
  </div>
  <!-- Editor Content Area -->
  <div class="card-body p-4" contenteditable="true" style="min-height:400px" role="textbox" aria-multiline="true" aria-label="Content editor">
    <h2>Your Content Here</h2>
    <p>Start typing your content in this editable area. Use the toolbar above to format your text.</p>
    <p>You can add <strong>bold</strong>, <em>italic</em>, and <u>underlined</u> text.</p>
    <ul>
      <li>Bullet point one</li>
      <li>Bullet point two</li>
    </ul>
    <pre><code>// Code block example
const greeting = "Hello, World!";</code></pre>
  </div>
  <!-- Status Bar -->
  <div class="card-footer bg-white d-flex justify-content-between text-muted small">
    <span>Words: 245 | Characters: 1,432</span>
    <span>Last saved: 2 minutes ago</span>
  </div>
</div>
```

## Advanced Variations

### Image Upload in Editor

```html
<!-- Link/Image Insert Modal -->
<div class="modal fade" id="insertLinkModal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Insert Link</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>
      <div class="modal-body">
        <div class="mb-3">
          <label for="linkUrl" class="form-label">URL</label>
          <input type="url" class="form-control" id="linkUrl" placeholder="https://example.com">
        </div>
        <div class="mb-3">
          <label for="linkText" class="form-label">Display Text</label>
          <input type="text" class="form-control" id="linkText" placeholder="Link text">
        </div>
        <div class="form-check">
          <input class="form-check-input" type="checkbox" id="linkNewTab">
          <label class="form-check-label" for="linkNewTab">Open in new tab</label>
        </div>
      </div>
      <div class="modal-footer">
        <button class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button class="btn btn-primary">Insert Link</button>
      </div>
    </div>
  </div>
</div>

<!-- Image Upload Area -->
<div class="border-2 border-dashed rounded p-5 text-center bg-light">
  <i class="bi bi-cloud-upload display-4 text-muted mb-3"></i>
  <p class="mb-2">Drag and drop images here or</p>
  <button class="btn btn-outline-primary btn-sm">Browse Files</button>
  <p class="text-muted small mt-2">Supports: JPG, PNG, GIF, WebP (max 5MB)</p>
</div>
```

### Edit/Preview Toggle

```html
<ul class="nav nav-tabs card-header-tabs">
  <li class="nav-item">
    <button class="nav-link active" data-bs-toggle="tab" data-bs-target="#editPane">
      <i class="bi bi-pencil me-1"></i>Edit
    </button>
  </li>
  <li class="nav-item">
    <button class="nav-link" data-bs-toggle="tab" data-bs-target="#previewPane">
      <i class="bi bi-eye me-1"></i>Preview
    </button>
  </li>
  <li class="nav-item">
    <button class="nav-link" data-bs-toggle="tab" data-bs-target="#htmlPane">
      <i class="bi bi-code me-1"></i>HTML
    </button>
  </li>
</ul>
<div class="tab-content">
  <div class="tab-pane fade show active" id="editPane">
    <!-- Editor toolbar and content area -->
  </div>
  <div class="tab-pane fade" id="previewPane">
    <div class="p-4" id="previewContent">
      <!-- Rendered preview -->
    </div>
  </div>
  <div class="tab-pane fade" id="htmlPane">
    <textarea class="form-control font-monospace" rows="20" readonly>&lt;h2&gt;Your Content&lt;/h2&gt;</textarea>
  </div>
</div>
```

## Best Practices

1. Group toolbar buttons logically: formatting, lists, insert, alignment, history
2. Use `btn-group-sm` for compact toolbar buttons
3. Provide `title` and `aria-label` on all icon-only toolbar buttons
4. Show word and character count in the status bar
5. Include undo/redo for mistake recovery
6. Provide Edit, Preview, and HTML tabs for different views
7. Use modal dialogs for link/image insertion with validation
8. Support drag-and-drop for image uploads
9. Auto-save content periodically with a "Last saved" indicator
10. Use `contenteditable="true"` with proper ARIA roles for the editor area

## Common Pitfalls

1. **No keyboard shortcuts** - Power users expect Ctrl+B, Ctrl+I. Support common shortcuts.
2. **Toolbar not accessible** - Icon-only buttons need `aria-label` for screen readers.
3. **No auto-save** - Content is lost if the browser crashes. Save to draft automatically.
4. **Preview not accurate** - The preview should match the published output exactly.
5. **Image upload without validation** - Accept only valid image types and enforce size limits.
6. **No word count** - Content creators need to track length for SEO and guidelines.

## Accessibility Considerations

- Use `role="toolbar"` on the toolbar container with `aria-label="Formatting toolbar"`
- Provide `aria-label` on every icon-only button
- Use `role="textbox" aria-multiline="true"` on the contenteditable area
- Announce save status with `aria-live="polite"`
- Ensure toolbar buttons are keyboard-navigable with arrow keys

## Responsive Behavior

On **mobile**, the toolbar wraps to multiple rows with all buttons visible. The editor content area takes full width. On **tablet**, the toolbar fits on 1-2 rows. On **desktop**, the full toolbar displays on a single row with the status bar showing word count and save status.
