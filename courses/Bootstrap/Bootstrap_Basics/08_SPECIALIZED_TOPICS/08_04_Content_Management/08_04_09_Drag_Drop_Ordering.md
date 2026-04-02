---
title: "Drag & Drop Ordering"
description: "Build sortable lists with drag handles, reorder feedback, and save order functionality using Bootstrap 5 and the HTML5 Drag and Drop API."
difficulty: 3
estimated_time: "45 minutes"
prerequisites:
  - "Bootstrap 5 List Group"
  - "Bootstrap 5 Buttons"
  - "HTML5 Drag and Drop API"
---

## Overview

Drag and drop ordering allows users to reorder content items like navigation menus, playlists, or priority lists by dragging elements to new positions. Bootstrap 5's list group component provides the visual container, while the HTML5 Drag and Drop API handles the interaction logic.

The pattern includes visible drag handles, visual feedback during dragging (opacity changes, drop zones), position indicators, and a save mechanism to persist the new order. This is essential for CMS navigation builders, content priority management, and any ordered list configuration.

## Basic Implementation

### Sortable List with Drag Handles

```html
<div class="list-group" id="sortableList">
  <div class="list-group-item d-flex align-items-center" draggable="true">
    <span class="drag-handle me-3 text-muted" style="cursor: grab;"><i class="bi bi-grip-vertical fs-5"></i></span>
    <div class="flex-grow-1">
      <strong>Home</strong>
      <div class="text-muted small">/home</div>
    </div>
    <span class="badge bg-light text-muted">Position 1</span>
  </div>
  <div class="list-group-item d-flex align-items-center" draggable="true">
    <span class="drag-handle me-3 text-muted" style="cursor: grab;"><i class="bi bi-grip-vertical fs-5"></i></span>
    <div class="flex-grow-1">
      <strong>About</strong>
      <div class="text-muted small">/about</div>
    </div>
    <span class="badge bg-light text-muted">Position 2</span>
  </div>
  <div class="list-group-item d-flex align-items-center" draggable="true">
    <span class="drag-handle me-3 text-muted" style="cursor: grab;"><i class="bi bi-grip-vertical fs-5"></i></span>
    <div class="flex-grow-1">
      <strong>Products</strong>
      <div class="text-muted small">/products</div>
    </div>
    <span class="badge bg-light text-muted">Position 3</span>
  </div>
  <div class="list-group-item d-flex align-items-center" draggable="true">
    <span class="drag-handle me-3 text-muted" style="cursor: grab;"><i class="bi bi-grip-vertical fs-5"></i></span>
    <div class="flex-grow-1">
      <strong>Contact</strong>
      <div class="text-muted small">/contact</div>
    </div>
    <span class="badge bg-light text-muted">Position 4</span>
  </div>
</div>
<div class="mt-3 d-flex justify-content-between align-items-center">
  <small class="text-muted"><i class="bi bi-info-circle me-1"></i>Drag items to reorder</small>
  <button class="btn btn-primary btn-sm" id="saveOrder"><i class="bi bi-check-lg me-1"></i>Save Order</button>
</div>
```

### Drag States CSS

```html
<style>
  .list-group-item.dragging {
    opacity: 0.5;
    background-color: var(--bs-primary-bg-subtle);
  }
  .list-group-item.drag-over {
    border-top: 3px solid var(--bs-primary);
  }
  .drag-handle:active {
    cursor: grabbing;
  }
</style>
```

### Simple Visual Feedback

```html
<div class="list-group-item d-flex align-items-center border-primary bg-primary bg-opacity-10" draggable="true">
  <span class="drag-handle me-3 text-muted"><i class="bi bi-grip-vertical fs-5"></i></span>
  <div class="flex-grow-1">
    <strong>Being Dragged</strong>
    <div class="text-muted small">Visual feedback during reorder</div>
  </div>
  <i class="bi bi-arrows-move text-primary"></i>
</div>
```

## Advanced Variations

### Sortable with Up/Down Buttons (Accessible Alternative)

```html
<div class="list-group" id="accessibleSortable">
  <div class="list-group-item d-flex align-items-center">
    <div class="btn-group-vertical btn-group-sm me-3">
      <button class="btn btn-outline-secondary py-0" disabled title="Move up"><i class="bi bi-chevron-up"></i></button>
      <button class="btn btn-outline-secondary py-0" title="Move down"><i class="bi bi-chevron-down"></i></button>
    </div>
    <div class="flex-grow-1">
      <strong>Navigation Item 1</strong>
    </div>
    <span class="badge bg-light text-muted">#1</span>
  </div>
  <div class="list-group-item d-flex align-items-center">
    <div class="btn-group-vertical btn-group-sm me-3">
      <button class="btn btn-outline-secondary py-0" title="Move up"><i class="bi bi-chevron-up"></i></button>
      <button class="btn btn-outline-secondary py-0" title="Move down"><i class="bi bi-chevron-down"></i></button>
    </div>
    <div class="flex-grow-1">
      <strong>Navigation Item 2</strong>
    </div>
    <span class="badge bg-light text-muted">#2</span>
  </div>
  <div class="list-group-item d-flex align-items-center">
    <div class="btn-group-vertical btn-group-sm me-3">
      <button class="btn btn-outline-secondary py-0" title="Move up"><i class="bi bi-chevron-up"></i></button>
      <button class="btn btn-outline-secondary py-0 disabled" disabled title="Move down"><i class="bi bi-chevron-down"></i></button>
    </div>
    <div class="flex-grow-1">
      <strong>Navigation Item 3</strong>
    </div>
    <span class="badge bg-light text-muted">#3</span>
  </div>
</div>
```

### Nested Sortable Groups

```html
<div class="list-group">
  <div class="list-group-item d-flex align-items-center bg-light" draggable="true">
    <i class="bi bi-grip-vertical me-3 text-muted"></i>
    <i class="bi bi-folder me-2 text-warning"></i>
    <strong>Main Menu</strong>
    <span class="badge bg-secondary ms-auto">3 items</span>
  </div>
  <div class="list-group-item d-flex align-items-center ps-5" draggable="true">
    <i class="bi bi-grip-vertical me-3 text-muted"></i>
    <i class="bi bi-file-earmark me-2 text-primary"></i>
    <span>Sub Item A</span>
  </div>
  <div class="list-group-item d-flex align-items-center ps-5" draggable="true">
    <i class="bi bi-grip-vertical me-3 text-muted"></i>
    <i class="bi bi-file-earmark me-2 text-primary"></i>
    <span>Sub Item B</span>
  </div>
  <div class="list-group-item d-flex align-items-center ps-5" draggable="true">
    <i class="bi bi-grip-vertical me-3 text-muted"></i>
    <i class="bi bi-file-earmark me-2 text-primary"></i>
    <span>Sub Item C</span>
  </div>
</div>
```

### Order Saved Confirmation

```html
<div class="alert alert-success alert-dismissible fade show" role="alert">
  <i class="bi bi-check-circle me-1"></i>
  <strong>Order saved!</strong> The new arrangement has been saved successfully.
  <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
</div>
```

### Content Priority List with Thumbnails

```html
<div class="list-group">
  <div class="list-group-item d-flex align-items-center" draggable="true">
    <i class="bi bi-grip-vertical me-3 text-muted fs-5"></i>
    <img src="thumb1.jpg" class="rounded me-3" width="48" height="48" style="object-fit: cover;" alt="">
    <div class="flex-grow-1">
      <strong>Featured Article</strong>
      <div class="text-muted small">Published Apr 1, 2026</div>
    </div>
    <span class="badge bg-success">Visible</span>
  </div>
  <div class="list-group-item d-flex align-items-center" draggable="true">
    <i class="bi bi-grip-vertical me-3 text-muted fs-5"></i>
    <img src="thumb2.jpg" class="rounded me-3" width="48" height="48" style="object-fit: cover;" alt="">
    <div class="flex-grow-1">
      <strong>Product Launch</strong>
      <div class="text-muted small">Published Mar 28, 2026</div>
    </div>
    <span class="badge bg-success">Visible</span>
  </div>
</div>
```

## Best Practices

1. Use a visible grip icon (`bi-grip-vertical`) as the drag handle
2. Change opacity during drag to indicate the item being moved
3. Show a clear drop zone indicator (border or background change) at the target position
4. Provide up/down arrow buttons as an accessible keyboard alternative
5. Disable the up button on the first item and down button on the last item
6. Show a success notification when the order is saved
7. Use `draggable="true"` on list items and handle `dragstart`, `dragover`, `drop` events
8. Include position numbers or badges to show current ordering
9. Prevent text selection on drag handles with `user-select: none`
10. Use `cursor: grab` on handles and `cursor: grabbing` during active drag
11. Debounce the save operation to avoid excessive API calls
12. Support touch events for mobile drag and drop functionality
13. Allow canceling a drag with the Escape key

## Common Pitfalls

1. **No keyboard alternative**: Drag and drop is inaccessible to keyboard-only users. Always provide up/down buttons.
2. **Missing visual feedback**: Without opacity changes or drop zone indicators, users cannot tell where items will land.
3. **No save confirmation**: Users reorder items but have no indication the changes persisted.
4. **Touch device incompatibility**: The HTML5 Drag and Drop API does not work on touch devices without a polyfill.
5. **Entire row as drag handle**: Making the entire row draggable prevents text selection and button clicks within items.
6. **No undo capability**: Accidental reordering cannot be reversed without a cancel or undo mechanism.
7. **Losing scroll position**: After reordering, the list scrolls to the top, disorienting the user.

## Accessibility Considerations

- Provide up/down buttons alongside drag handles for keyboard navigation
- Use `aria-label` on grip handles describing the item and its position
- Implement `aria-live="polite"` to announce position changes
- Use `role="list"` on the sortable container
- Support arrow key navigation when an item is focused
- Announce the new position after an item is moved
- Ensure the save button is reachable via keyboard after reordering

## Responsive Behavior

On mobile, drag handles should have adequate touch target size (minimum 44x44px). Consider hiding position badges on very small screens to save space. The up/down button group should remain visible as the primary reorder method on touch devices. List item content should truncate with ellipsis if too long. Thumbnails should be smaller on mobile (32x32px).
