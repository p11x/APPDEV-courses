---
title: "Markdown Preview"
description: "Build side-by-side markdown editors with live preview, formatting toolbars, and tab switching using Bootstrap 5."
difficulty: 2
estimated_time: "30 minutes"
prerequisites:
  - "Bootstrap 5 Navs & Tabs"
  - "Bootstrap 5 Forms"
  - "Bootstrap 5 Button Group"
---

## Overview

Markdown preview components provide a writing environment with a text editor on one side and a rendered preview on the other. Bootstrap 5's tabs, button groups, and form controls build the editor interface, while a markdown parsing library handles rendering.

The component supports split-pane editing, tabbed switching between edit and preview modes, a formatting toolbar with common markdown shortcuts, and live preview updates as the user types. This pattern is used in CMS platforms, documentation tools, and comment systems.

## Basic Implementation

### Tab-Based Editor/Preview

```html
<ul class="nav nav-tabs" id="editorTabs" role="tablist">
  <li class="nav-item" role="presentation">
    <button class="nav-link active" id="write-tab" data-bs-toggle="tab" data-bs-target="#writePane" type="button" role="tab">
      <i class="bi bi-pencil me-1"></i>Write
    </button>
  </li>
  <li class="nav-item" role="presentation">
    <button class="nav-link" id="preview-tab" data-bs-toggle="tab" data-bs-target="#previewPane" type="button" role="tab">
      <i class="bi bi-eye me-1"></i>Preview
    </button>
  </li>
</ul>
<div class="tab-content border border-top-0 rounded-bottom" id="editorTabContent">
  <div class="tab-pane fade show active p-0" id="writePane" role="tabpanel">
    <textarea class="form-control border-0 rounded-0 rounded-bottom" rows="12" placeholder="Write your markdown here..." style="resize: vertical;" id="markdownInput"></textarea>
  </div>
  <div class="tab-pane fade p-3" id="previewPane" role="tabpanel">
    <div id="previewContent" class="preview-output">
      <p class="text-muted">Preview will appear here...</p>
    </div>
  </div>
</div>
```

### Formatting Toolbar

```html
<div class="btn-toolbar mb-2" role="toolbar" aria-label="Formatting toolbar">
  <div class="btn-group btn-group-sm me-2" role="group">
    <button type="button" class="btn btn-outline-secondary" title="Bold" data-format="bold">
      <i class="bi bi-type-bold"></i>
    </button>
    <button type="button" class="btn btn-outline-secondary" title="Italic" data-format="italic">
      <i class="bi bi-type-italic"></i>
    </button>
    <button type="button" class="btn btn-outline-secondary" title="Strikethrough" data-format="strike">
      <i class="bi bi-type-strikethrough"></i>
    </button>
  </div>
  <div class="btn-group btn-group-sm me-2" role="group">
    <button type="button" class="btn btn-outline-secondary" title="Heading" data-format="heading">
      <i class="bi bi-type-h2"></i>
    </button>
    <button type="button" class="btn btn-outline-secondary" title="Quote" data-format="quote">
      <i class="bi bi-quote"></i>
    </button>
    <button type="button" class="btn btn-outline-secondary" title="Code" data-format="code">
      <i class="bi bi-code-slash"></i>
    </button>
  </div>
  <div class="btn-group btn-group-sm me-2" role="group">
    <button type="button" class="btn btn-outline-secondary" title="Unordered List" data-format="ul">
      <i class="bi bi-list-ul"></i>
    </button>
    <button type="button" class="btn btn-outline-secondary" title="Ordered List" data-format="ol">
      <i class="bi bi-list-ol"></i>
    </button>
  </div>
  <div class="btn-group btn-group-sm" role="group">
    <button type="button" class="btn btn-outline-secondary" title="Link" data-format="link">
      <i class="bi bi-link-45deg"></i>
    </button>
    <button type="button" class="btn btn-outline-secondary" title="Image" data-format="image">
      <i class="bi bi-image"></i>
    </button>
  </div>
</div>
```

### Side-by-Side Split View

```html
<div class="row g-0 border rounded overflow-hidden">
  <div class="col-md-6 border-end">
    <div class="bg-light px-3 py-1 border-bottom d-flex justify-content-between align-items-center">
      <small class="text-muted fw-bold"><i class="bi bi-pencil me-1"></i>Markdown</small>
      <small class="text-muted" id="charCount">0 characters</small>
    </div>
    <textarea class="form-control border-0 rounded-0" rows="16" placeholder="Write markdown..." id="splitEditor" style="resize: none;"></textarea>
  </div>
  <div class="col-md-6">
    <div class="bg-light px-3 py-1 border-bottom">
      <small class="text-muted fw-bold"><i class="bi bi-eye me-1"></i>Preview</small>
    </div>
    <div class="p-3" id="splitPreview" style="min-height: 400px; overflow-y: auto;">
      <p class="text-muted">Live preview...</p>
    </div>
  </div>
</div>
```

## Advanced Variations

### Markdown Cheatsheet Dropdown

```html
<div class="dropdown">
  <button class="btn btn-outline-secondary btn-sm dropdown-toggle" data-bs-toggle="dropdown">
    <i class="bi bi-question-circle me-1"></i>Formatting Help
  </button>
  <div class="dropdown-menu dropdown-menu-end p-3" style="width: 320px;">
    <h6 class="dropdown-header">Markdown Cheatsheet</h6>
    <table class="table table-sm small mb-0">
      <tbody>
        <tr><td><code># Heading</code></td><td>Heading 1</td></tr>
        <tr><td><code>## Heading</code></td><td>Heading 2</td></tr>
        <tr><td><code>**bold**</code></td><td>Bold text</td></tr>
        <tr><td><code>*italic*</code></td><td>Italic text</td></tr>
        <tr><td><code>[text](url)</code></td><td>Link</td></tr>
        <tr><td><code>![alt](url)</code></td><td>Image</td></tr>
        <tr><td><code>- item</code></td><td>Unordered list</td></tr>
        <tr><td><code>1. item</code></td><td>Ordered list</td></tr>
        <tr><td><code>`code`</code></td><td>Inline code</td></tr>
        <tr><td><code>```block```</code></td><td>Code block</td></tr>
      </tbody>
    </table>
  </div>
</div>
```

### Word Count and Reading Time

```html
<div class="d-flex justify-content-between text-muted small mt-2">
  <span id="wordCountDisplay">0 words</span>
  <span id="readTimeDisplay">~0 min read</span>
</div>
```

### Draft Auto-Save Indicator

```html
<div class="d-flex align-items-center gap-2 text-muted small">
  <span class="spinner-border spinner-border-sm d-none" id="saveSpinner" role="status"></span>
  <i class="bi bi-cloud-check text-success" id="saveIcon"></i>
  <span id="saveStatus">Draft saved</span>
</div>
```

## Best Practices

1. Use tabs for edit/preview switching on mobile; use side-by-side on desktop
2. Include a formatting toolbar with common markdown shortcuts
3. Update the preview in real-time as the user types with debouncing
4. Show character count and word count below the editor
5. Provide a markdown cheatsheet for users unfamiliar with the syntax
6. Use `font-family: monospace` on the editor textarea for code readability
7. Implement auto-save with a visual indicator showing save status
8. Support keyboard shortcuts (Ctrl+B for bold, Ctrl+I for italic)
9. Sanitize the rendered HTML preview to prevent XSS attacks
10. Use `resize: vertical` on the textarea to allow height adjustment
11. Store drafts in localStorage as a backup mechanism
12. Indicate unsaved changes with a visual marker
13. Support full-screen editing mode for distraction-free writing

## Common Pitfalls

1. **No XSS protection in preview**: Rendering markdown as raw HTML without sanitization allows script injection.
2. **Missing debouncing on live preview**: Re-rendering on every keystroke causes performance issues with long documents.
3. **No auto-save**: Users lose content if the browser crashes or they accidentally navigate away.
4. **Preview not matching final output**: If the preview uses different CSS than the published page, users get unexpected results.
5. **No keyboard shortcuts**: Power users expect Ctrl+B/I/S shortcuts. Missing them reduces productivity.
6. **Hardcoded font sizes in preview**: The preview should use the same typography as the published output.
7. **Missing loading state on preview**: For complex markdown with code highlighting, the preview needs a loading indicator.

## Accessibility Considerations

- Use `role="toolbar"` with `aria-label` on the formatting toolbar
- Associate `aria-controls` on toolbar buttons with the textarea
- Use `aria-label` on all formatting buttons describing the action
- Ensure tabs follow WAI-ARIA tab pattern with arrow key navigation
- Provide `aria-live="polite"` on word count updates
- Use `role="tabpanel"` on editor and preview panes
- Make the textarea accessible with proper `label` or `aria-label`

## Responsive Behavior

On mobile, switch from side-by-side to tabbed view using `col-12` instead of `col-md-6`. The formatting toolbar should use `btn-group-sm` and wrap if needed. The cheatsheet dropdown should use `dropdown-menu-end`. The textarea should remain full-width on all breakpoints. Preview content should be scrollable within its container on small screens.
