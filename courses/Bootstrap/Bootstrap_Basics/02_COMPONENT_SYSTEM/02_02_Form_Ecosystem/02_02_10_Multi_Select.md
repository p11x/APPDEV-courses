---
title: Multi-Select
category: Component System
difficulty: 2
time: 25 min
tags: bootstrap5, multi-select, select, forms, dropdown, tags
---

## Overview

Bootstrap supports native multi-select via `<select multiple>` with the `form-select` class. The native control lets users select multiple options using Ctrl/Cmd+click. For richer interactions, you can build tag-based multi-select patterns using dropdowns, badges, and JavaScript. These patterns improve usability on mobile devices and provide visual feedback with removable tag chips.

## Basic Implementation

The native multi-select uses `form-select` with the `multiple` attribute.

```html
<!-- Native multi-select -->
<div class="mb-3">
  <label for="multiSelect" class="form-label">Select multiple options</label>
  <select class="form-select" id="multiSelect" multiple aria-label="Multiple select example">
    <option selected>Open this select menu</option>
    <option value="1">One</option>
    <option value="2">Two</option>
    <option value="3">Three</option>
    <option value="4">Four</option>
    <option value="5">Five</option>
  </select>
  <div class="form-text">Hold Ctrl (Cmd on Mac) to select multiple options.</div>
</div>

<!-- Multi-select with size attribute -->
<div class="mb-3">
  <label for="multiSelectSize" class="form-label">Visible options</label>
  <select class="form-select" id="multiSelectSize" multiple size="5" aria-label="Size 5 multi-select">
    <option value="react">React</option>
    <option value="vue">Vue</option>
    <option value="angular">Angular</option>
    <option value="svelte">Svelte</option>
    <option value="solid">Solid</option>
    <option value="preact">Preact</option>
  </select>
</div>
```

## Advanced Variations

```html
<!-- Tag-based multi-select pattern -->
<style>
  .tag-select-container {
    border: 1px solid #ced4da;
    border-radius: 0.375rem;
    padding: 0.375rem 0.75rem;
    display: flex;
    flex-wrap: wrap;
    gap: 0.25rem;
    align-items: center;
    min-height: 38px;
    cursor: text;
  }
  .tag-select-container:focus-within {
    border-color: #86b7fe;
    box-shadow: 0 0 0 0.25rem rgba(13, 110, 253, 0.25);
  }
  .tag-chip {
    display: inline-flex;
    align-items: center;
    gap: 0.25rem;
    padding: 0.125rem 0.5rem;
    background-color: #0d6efd;
    color: #fff;
    border-radius: 0.25rem;
    font-size: 0.875rem;
  }
  .tag-chip .btn-close {
    filter: brightness(0) invert(1);
    font-size: 0.6rem;
  }
</style>

<div class="mb-3">
  <label class="form-label">Skills</label>
  <div class="tag-select-container" id="tagSelect">
    <span class="tag-chip">JavaScript <button type="button" class="btn-close btn-close-white" aria-label="Remove"></button></span>
    <span class="tag-chip">Python <button type="button" class="btn-close btn-close-white" aria-label="Remove"></button></span>
    <input type="text" class="border-0 flex-grow-1" style="outline: none; min-width: 80px;" placeholder="Add a skill..." aria-label="Add skill">
  </div>
</div>
```

```html
<!-- Multi-select with checkboxes in dropdown -->
<div class="dropdown mb-3">
  <label class="form-label d-block">Select frameworks</label>
  <button class="btn btn-outline-secondary dropdown-toggle w-100 text-start" type="button"
          data-bs-toggle="dropdown" aria-expanded="false">
    Select options
  </button>
  <div class="dropdown-menu w-100 p-2">
    <div class="form-check">
      <input class="form-check-input" type="checkbox" value="bootstrap" id="chk1">
      <label class="form-check-label" for="chk1">Bootstrap</label>
    </div>
    <div class="form-check">
      <input class="form-check-input" type="checkbox" value="tailwind" id="chk2">
      <label class="form-check-label" for="chk2">Tailwind CSS</label>
    </div>
    <div class="form-check">
      <input class="form-check-input" type="checkbox" value="bulma" id="chk3">
      <label class="form-check-label" for="chk3">Bulma</label>
    </div>
    <div class="form-check">
      <input class="form-check-input" type="checkbox" value="foundation" id="chk4">
      <label class="form-check-label" for="chk4">Foundation</label>
    </div>
    <hr class="dropdown-divider">
    <button class="btn btn-sm btn-primary w-100" type="button">Apply</button>
  </div>
</div>
```

## Best Practices

1. Use native `<select multiple>` for simple multi-select needs.
2. Set `size` attribute to control visible rows in native multi-select.
3. Provide clear instructions for Ctrl/Cmd+click behavior on native selects.
4. Use tag chips with close buttons for custom multi-select UX.
5. Ensure the tag container is focusable and keyboard-accessible.
6. Limit displayed tags with an "and N more" overflow pattern for long selections.
7. Use dropdown checkboxes as an alternative to native multi-select.
8. Sync custom multi-select UI with a hidden `<select multiple>` for form submission.
9. Validate that at least one option is selected when required.
10. Provide visual feedback when the maximum selection limit is reached.

## Common Pitfalls

1. **No keyboard support in custom implementations.** Tag-based selects must support Tab, Enter, and Backspace.
2. **Native multi-select on mobile.** Mobile browsers render `<select multiple>` poorly; use checkbox lists instead.
3. **Overflowing tags.** Long tag lists push content down; use scroll containers or collapsed summaries.
4. **Missing hidden input.** Custom multi-selects without a hidden `<select>` do not submit values with forms.
5. **Inaccessible dropdown checkboxes.** Dropdowns must manage focus and ARIA attributes properly.
6. **No clear-all action.** Users need a way to deselect all options at once.

## Accessibility Considerations

Native multi-select is accessible by default. For custom implementations, use `role="listbox"` with `aria-multiselectable="true"` on the option container. Each tag chip needs `aria-label` describing the selected value and a button with `aria-label="Remove [value]"`. Ensure focus management returns to the input after removing a tag. Use `aria-live="polite"` to announce selection changes. Provide a visible label and `aria-describedby` for instructions.

## Responsive Behavior

Native multi-select stretches to container width by default. On mobile, use `size` to limit visible rows and avoid full-height selects. Tag-based multi-selects should wrap tags with `flex-wrap`. Dropdown checkbox multi-selects should be full-width on mobile with `w-100`. Ensure tag chips remain readable on small screens by using appropriate font sizes and padding.
