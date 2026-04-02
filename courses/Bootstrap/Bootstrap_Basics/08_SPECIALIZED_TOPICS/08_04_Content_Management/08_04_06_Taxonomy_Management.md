---
title: "Taxonomy Management"
module: "Content Management"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["04_05_Forms", "04_01_Card_Component", "05_01_Accordion"]
---

## Overview

Taxonomy management provides interfaces for organizing content with tags, categories, and hierarchical structures. Bootstrap 5 form controls for tag inputs, accordion for category trees, badges for tag displays, and list groups for ordered items create flexible taxonomy management tools.

## Basic Implementation

### Tag Input Component

```html
<div class="card">
  <div class="card-header bg-white"><h5 class="mb-0">Article Tags</h5></div>
  <div class="card-body">
    <div class="mb-3">
      <label for="tagInput" class="form-label">Add Tags</label>
      <div class="border rounded p-2 d-flex flex-wrap gap-2 align-items-center">
        <span class="badge bg-primary d-flex align-items-center gap-1 py-2 px-3">
          Bootstrap <button class="btn-close btn-close-white" style="font-size:0.6em" aria-label="Remove Bootstrap tag"></button>
        </span>
        <span class="badge bg-primary d-flex align-items-center gap-1 py-2 px-3">
          CSS <button class="btn-close btn-close-white" style="font-size:0.6em" aria-label="Remove CSS tag"></button>
        </span>
        <span class="badge bg-primary d-flex align-items-center gap-1 py-2 px-3">
          Responsive <button class="btn-close btn-close-white" style="font-size:0.6em" aria-label="Remove Responsive tag"></button>
        </span>
        <input type="text" class="border-0 flex-grow-1" id="tagInput" placeholder="Type and press Enter to add tags..." style="outline:none;min-width:150px">
      </div>
      <div class="form-text">Press Enter or comma to add a tag. Max 10 tags.</div>
    </div>
    <div>
      <label class="form-label">Suggested Tags</label>
      <div class="d-flex flex-wrap gap-1">
        <button class="badge bg-light text-dark border py-2 px-3" onclick="addTag('JavaScript')">+ JavaScript</button>
        <button class="badge bg-light text-dark border py-2 px-3" onclick="addTag('Accessibility')">+ Accessibility</button>
        <button class="badge bg-light text-dark border py-2 px-3" onclick="addTag('Tutorial')">+ Tutorial</button>
        <button class="badge bg-light text-dark border py-2 px-3" onclick="addTag('Frontend')">+ Frontend</button>
      </div>
    </div>
  </div>
</div>
```

### Category Tree with Accordion

```html
<div class="card">
  <div class="card-header bg-white d-flex justify-content-between align-items-center">
    <h5 class="mb-0">Categories</h5>
    <button class="btn btn-primary btn-sm"><i class="bi bi-plus me-1"></i>Add Category</button>
  </div>
  <div class="card-body p-0">
    <div class="accordion accordion-flush" id="categoryTree">
      <div class="accordion-item">
        <h2 class="accordion-header">
          <button class="accordion-button" data-bs-toggle="collapse" data-bs-target="#cat1">
            <i class="bi bi-folder2-open me-2 text-primary"></i>
            Technology
            <span class="badge bg-secondary ms-2">12</span>
          </button>
        </h2>
        <div id="cat1" class="accordion-collapse collapse show" data-bs-parent="#categoryTree">
          <div class="accordion-body ps-4 py-2">
            <div class="list-group list-group-flush">
              <div class="list-group-item d-flex justify-content-between align-items-center px-0">
                <span><i class="bi bi-folder me-2 text-muted"></i>Web Development</span>
                <div>
                  <span class="badge bg-secondary me-2">8</span>
                  <button class="btn btn-sm btn-outline-secondary"><i class="bi bi-pencil"></i></button>
                </div>
              </div>
              <div class="list-group-item d-flex justify-content-between align-items-center px-0">
                <span><i class="bi bi-folder me-2 text-muted"></i>Mobile Development</span>
                <div>
                  <span class="badge bg-secondary me-2">4</span>
                  <button class="btn btn-sm btn-outline-secondary"><i class="bi bi-pencil"></i></button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="accordion-item">
        <h2 class="accordion-header">
          <button class="accordion-button collapsed" data-bs-toggle="collapse" data-bs-target="#cat2">
            <i class="bi bi-folder2 me-2 text-muted"></i>
            Design
            <span class="badge bg-secondary ms-2">7</span>
          </button>
        </h2>
        <div id="cat2" class="accordion-collapse collapse" data-bs-parent="#categoryTree">
          <div class="accordion-body ps-4 py-2">
            <div class="list-group list-group-flush">
              <div class="list-group-item d-flex justify-content-between align-items-center px-0">
                <span><i class="bi bi-folder me-2 text-muted"></i>UI Design</span>
                <div>
                  <span class="badge bg-secondary me-2">5</span>
                  <button class="btn btn-sm btn-outline-secondary"><i class="bi bi-pencil"></i></button>
                </div>
              </div>
              <div class="list-group-item d-flex justify-content-between align-items-center px-0">
                <span><i class="bi bi-folder me-2 text-muted"></i>UX Research</span>
                <div>
                  <span class="badge bg-secondary me-2">2</span>
                  <button class="btn btn-sm btn-outline-secondary"><i class="bi bi-pencil"></i></button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Bulk Edit Tags

```html
<div class="card">
  <div class="card-header bg-white"><h5 class="mb-0">Bulk Edit Tags</h5></div>
  <div class="card-body">
    <div class="alert alert-info">
      <i class="bi bi-info-circle me-2"></i>
      <strong>3 articles</strong> selected for bulk editing.
    </div>
    <div class="mb-3">
      <label class="form-label">Add Tags to Selected</label>
      <div class="input-group">
        <input type="text" class="form-control" placeholder="Enter tag name">
        <button class="btn btn-primary">Add</button>
      </div>
    </div>
    <div class="mb-3">
      <label class="form-label">Remove Tags from Selected</label>
      <div class="d-flex flex-wrap gap-2">
        <span class="badge bg-danger d-flex align-items-center gap-1 py-2 px-3">
          Bootstrap <button class="btn-close btn-close-white" style="font-size:0.6em"></button>
        </span>
        <span class="badge bg-danger d-flex align-items-center gap-1 py-2 px-3">
          Tutorial <button class="btn-close btn-close-white" style="font-size:0.6em"></button>
        </span>
      </div>
    </div>
    <div class="mb-3">
      <label class="form-label">Change Category</label>
      <select class="form-select">
        <option>Keep current categories</option>
        <option>Technology / Web Development</option>
        <option>Design / UI Design</option>
      </select>
    </div>
    <button class="btn btn-primary">Apply Changes</button>
  </div>
</div>
```

## Best Practices

1. Use badge pills with remove buttons for active tags
2. Provide suggested tags below the input for quick adding
3. Use accordion for hierarchical category trees
4. Show item counts next to each category
5. Support comma and Enter key for tag input
6. Provide bulk edit for managing tags across multiple items
7. Use folder icons for categories and tags for flat labels
8. Limit maximum tags per item with a visible counter
9. Support tag search/autocomplete for large tag sets
10. Use consistent badge colors: primary for tags, secondary for counts

## Common Pitfalls

1. **No tag suggestions** - Users create duplicate tags. Provide autocomplete and suggestions.
2. **Categories not hierarchical** - Flat lists don't work for complex taxonomies. Use nested structures.
3. **No bulk editing** - Managing tags one item at a time is tedious.
4. **Tag input not accessible** - Ensure the input is keyboard-navigable and screen-reader friendly.
5. **No validation** - Prevent duplicate tags and enforce character limits.

## Accessibility Considerations

- Use `aria-label="Remove Bootstrap tag"` on tag close buttons
- Mark the tag input region with `role="group" aria-label="Tags"`
- Use `aria-expanded` on accordion category toggles
- Provide `aria-label` on category folders describing content count
- Announce tag additions/removals with `aria-live="polite"`

## Responsive Behavior

On **mobile**, tag inputs take full width. Category trees remain accordion-based. Bulk edit cards stack vertically. On **tablet and desktop**, tag inputs and category management display side by side in a two-column layout.
