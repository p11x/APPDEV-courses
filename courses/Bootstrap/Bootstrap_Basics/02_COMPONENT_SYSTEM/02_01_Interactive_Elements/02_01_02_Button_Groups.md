---
tags: [bootstrap, button-groups, components, interactive]
category: Interactive Elements
difficulty: 1
time: 25 minutes
---

# Button Groups

## Overview

Button groups combine multiple buttons into a single connected component, visually indicating that the buttons are related and often represent a set of mutually exclusive or complementary actions. Bootstrap's `.btn-group` class merges adjacent buttons by removing border-radius on internal edges and overlapping borders to create a seamless segmented control appearance.

Beyond basic horizontal grouping, Bootstrap supports vertical groups with `.btn-group-vertical`, toolbar containers with `.btn-toolbar` for complex multi-group layouts, and size modifiers that cascade to all child buttons. Button groups also serve as the foundation for checkbox and radio toggle patterns, where grouped buttons replace traditional form inputs with a more visually engaging control.

The button group system is critical for navigation toolbars, segmented controls, filter bars, and any interface where discrete options need visual connection. ARIA attributes are essential in these patterns, particularly `role="group"` and `aria-label`, to ensure assistive technologies understand the relationship between grouped buttons.

## Basic Implementation

The simplest button group wraps buttons in a container with `.btn-group`. Bootstrap handles the border-radius adjustments and border overlapping automatically.

```html
<div class="btn-group" role="group" aria-label="Basic example">
  <button type="button" class="btn btn-primary">Left</button>
  <button type="button" class="btn btn-primary">Middle</button>
  <button type="button" class="btn btn-primary">Right</button>
</div>
```

The `role="group"` attribute is mandatory for accessibility. It tells screen readers that these buttons form a logical group. The `aria-label` provides a human-readable description of the group's purpose.

For vertical grouping, replace `.btn-group` with `.btn-group-vertical`:

```html
<div class="btn-group-vertical" role="group" aria-label="Vertical button group">
  <button type="button" class="btn btn-primary">Top</button>
  <button type="button" class="btn btn-primary">Middle</button>
  <button type="button" class="btn btn-primary">Bottom</button>
</div>
```

Nesting a button group inside a dropdown is a common pattern for splitting actions:

```html
<div class="btn-group" role="group" aria-label="Button group with nested dropdown">
  <button type="button" class="btn btn-primary">Save</button>
  <div class="btn-group" role="group">
    <button type="button" class="btn btn-primary dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
      Save Options
    </button>
    <ul class="dropdown-menu">
      <li><a class="dropdown-item" href="#">Save as Draft</a></li>
      <li><a class="dropdown-item" href="#">Save as Template</a></li>
      <li><a class="dropdown-item" href="#">Save and Close</a></li>
    </ul>
  </div>
</div>
```

This split-button pattern provides a primary action (Save) with a secondary dropdown for alternative variations.

## Advanced Variations

Button toolbars combine multiple `.btn-group` containers inside a `.btn-toolbar` for complex layouts like text editor formatting controls:

```html
<div class="btn-toolbar" role="toolbar" aria-label="Toolbar with button groups">
  <div class="btn-group me-2" role="group" aria-label="Text formatting">
    <button type="button" class="btn btn-outline-secondary">Bold</button>
    <button type="button" class="btn btn-outline-secondary">Italic</button>
    <button type="button" class="btn btn-outline-secondary">Underline</button>
  </div>
  <div class="btn-group me-2" role="group" aria-label="Alignment">
    <button type="button" class="btn btn-outline-secondary">Left</button>
    <button type="button" class="btn btn-outline-secondary">Center</button>
    <button type="button" class="btn btn-outline-secondary">Right</button>
  </div>
  <div class="btn-group" role="group" aria-label="Actions">
    <button type="button" class="btn btn-outline-danger">Delete</button>
  </div>
</div>
```

Size modifiers apply to the group container and cascade to all child buttons:

```html
<div class="btn-group btn-group-lg" role="group" aria-label="Large button group">
  <button type="button" class="btn btn-outline-primary">One</button>
  <button type="button" class="btn btn-outline-primary">Two</button>
  <button type="button" class="btn btn-outline-primary">Three</button>
</div>

<div class="btn-group" role="group" aria-label="Default button group">
  <button type="button" class="btn btn-outline-primary">One</button>
  <button type="button" class="btn btn-outline-primary">Two</button>
  <button type="button" class="btn btn-outline-primary">Three</button>
</div>

<div class="btn-group btn-group-sm" role="group" aria-label="Small button group">
  <button type="button" class="btn btn-outline-primary">One</button>
  <button type="button" class="btn btn-outline-primary">Two</button>
  <button type="button" class="btn btn-outline-primary">Three</button>
</div>
```

Checkbox button groups use visually hidden checkboxes with `.btn-check` paired with styled labels:

```html
<input type="checkbox" class="btn-check" id="btncheck1" autocomplete="off">
<label class="btn btn-outline-primary" for="btncheck1">Checkbox 1</label>

<input type="checkbox" class="btn-check" id="btncheck2" autocomplete="off">
<label class="btn btn-outline-primary" for="btncheck2">Checkbox 2</label>

<input type="checkbox" class="btn-check" id="btncheck3" autocomplete="off">
<label class="btn btn-outline-primary" for="btncheck3">Checkbox 3</label>
```

Radio button groups enforce single selection within the group by sharing the same `name` attribute:

```html
<div class="btn-group" role="group" aria-label="Radio toggle button group">
  <input type="radio" class="btn-check" name="btnradio" id="btnradio1" autocomplete="off" checked>
  <label class="btn btn-outline-primary" for="btnradio1">Radio 1</label>

  <input type="radio" class="btn-check" name="btnradio" id="btnradio2" autocomplete="off">
  <label class="btn btn-outline-primary" for="btnradio2">Radio 2</label>

  <input type="radio" class="btn-check" name="btnradio" id="btnradio3" autocomplete="off">
  <label class="btn btn-outline-primary" for="btnradio3">Radio 3</label>
</div>
```

## Best Practices

1. **Always include `role="group"` on the button group container.** Without it, assistive technologies cannot identify the relationship between buttons. Screen readers will announce each button in isolation rather than as part of a grouped control.

2. **Provide `aria-label` on every button group.** The label describes the group's purpose, such as "Text formatting options" or "Filter by status." This context is essential for screen reader users navigating by landmarks.

3. **Use `btn-toolbar` with `role="toolbar"` for multi-group layouts.** Toolbars create a higher-level grouping that helps assistive technologies present multiple related groups as a single navigable unit.

4. **Maintain consistent button variants within a group.** Mixing outline and filled buttons in the same group creates visual confusion. Pick one style and apply it uniformly to all buttons within the group.

5. **Apply size modifiers to the group container, not individual buttons.** Using `btn-group-lg` ensures all child buttons scale proportionally. Applying `btn-lg` to individual buttons within a group can break the connected visual appearance.

6. **Use checkbox groups for multi-select and radio groups for single-select.** The underlying HTML semantics enforce this behavior. Checkbox groups allow any combination of selections; radio groups enforce mutual exclusivity.

7. **Include `autocomplete="off"` on checkbox and radio inputs.** This prevents browsers from persisting checked states across page reloads, which can conflict with server-side state or JavaScript initialization.

8. **Use `me-2` (margin-end) spacing between toolbar groups.** Bootstrap provides spacing utilities for consistent gaps. Avoid inline styles or custom margins that may conflict with the utility system.

9. **Test button groups with keyboard-only navigation.** Tab should move focus between groups, and arrow keys should move focus within radio groups. Verify that the visual focus indicator is always visible.

10. **Avoid excessive nesting.** Nesting more than two levels of button groups creates confusing DOM structures that are difficult for assistive technologies to parse. Simplify the layout or use a different component pattern.

## Common Pitfalls

1. **Omitting `role="group"` from the container.** This is the most common accessibility error with button groups. Without the ARIA role, screen readers announce buttons as isolated controls with no logical relationship.

2. **Forgetting shared `name` on radio buttons.** Radio inputs require the same `name` attribute to enforce mutual exclusivity. Without it, users can select multiple radio options simultaneously, breaking the expected behavior.

3. **Using `btn-group-vertical` inside responsive layouts.** Vertical groups take full width and do not respond to horizontal flex wrapping. For responsive behavior, use horizontal `.btn-group` with flex utilities instead.

4. **Placing non-button elements inside `btn-group`.** Only `.btn` elements are styled for the connected appearance. Placing text nodes, images, or other elements inside the group breaks the visual connection and may introduce unwanted spacing.

5. **Not updating `aria-checked` on toggle buttons.** While Bootstrap handles visual state for `.btn-check` elements, custom JavaScript implementations must manually update ARIA attributes to maintain accessibility.

6. **Applying `data-bs-toggle="button"` to individual buttons inside a radio group.** Radio groups manage state through native HTML radio semantics. Adding the toggle attribute creates conflicting state management.

7. **Using fixed pixel widths on button groups in responsive containers.** Button groups should rely on content sizing or flex utilities. Fixed widths cause overflow or truncation on small viewports.

## Accessibility Considerations

Button groups require `role="group"` on the container to establish the relationship between child buttons. For toolbars, use `role="toolbar"` on the outer container with `role="group"` on each inner group.

Always provide accessible names via `aria-label` or `aria-labelledby`. When an existing heading or visible label describes the group, reference it with `aria-labelledby`:

```html
<h3 id="formatting-label">Text Formatting</h3>
<div class="btn-group" role="group" aria-labelledby="formatting-label">
  <button type="button" class="btn btn-outline-secondary">Bold</button>
  <button type="button" class="btn btn-outline-secondary">Italic</button>
</div>
```

For checkbox and radio button groups, the `<label>` element connected via `for` attribute provides the accessible name. Ensure every input has a corresponding label.

Within radio groups, arrow keys should move selection between options. Bootstrap's `.btn-check` pattern preserves native radio keyboard behavior, allowing arrow key navigation within the group while Tab moves focus in and out of the group.

## Responsive Behavior

Button groups are inline-flex by default and will wrap or overflow based on their container. For responsive behavior, combine `.btn-group` with Bootstrap's flex utilities.

Use `flex-wrap` to allow groups to wrap on small screens:

```html
<div class="btn-group flex-wrap" role="group" aria-label="Responsive button group">
  <button type="button" class="btn btn-outline-primary">One</button>
  <button type="button" class="btn btn-outline-primary">Two</button>
  <button type="button" class="btn btn-outline-primary">Three</button>
  <button type="button" class="btn btn-outline-primary">Four</button>
  <button type="button" class="btn btn-outline-primary">Five</button>
</div>
```

For toolbars, use `flex-column flex-sm-row` to stack groups vertically on small screens and horizontally on wider viewports:

```html
<div class="btn-toolbar flex-column flex-sm-row" role="toolbar" aria-label="Responsive toolbar">
  <div class="btn-group me-sm-2 mb-2 mb-sm-0" role="group" aria-label="Group 1">
    <button type="button" class="btn btn-outline-secondary">A</button>
    <button type="button" class="btn btn-outline-secondary">B</button>
  </div>
  <div class="btn-group" role="group" aria-label="Group 2">
    <button type="button" class="btn btn-outline-secondary">C</button>
    <button type="button" class="btn btn-outline-secondary">D</button>
  </div>
</div>
```

Vertical button groups do not support horizontal wrapping. If responsive behavior is required, use horizontal groups with responsive flex direction utilities instead of vertical groups.
