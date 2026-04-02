---
title: "Checkboxes and Radios"
description: "Learn Bootstrap 5 checkbox and radio button implementation including inline layouts, reverse ordering, switch toggles, and indeterminate states."
category: "Bootstrap Basics"
subcategory: "Form Ecosystem"
section: "02_COMPONENT_SYSTEM"
lesson: "02_02_Form_Ecosystem"
topic: "Checkboxes and Radios"
difficulty: 1
prerequisites:
  - "HTML Form Controls"
  - "Bootstrap 5 Form Basics"
  - "CSS Flexbox Concepts"
learning_objectives:
  - "Implement form-check with form-check-input and form-check-label"
  - "Create inline checkbox and radio groups"
  - "Build reverse-ordered checkboxes and radios"
  - "Construct switch toggle controls"
  - "Manage indeterminate checkbox state programmatically"
key_terms:
  - "form-check"
  - "form-check-input"
  - "form-check-label"
  - "form-switch"
  - "indeterminate"
  - "inline checks"
  - "reverse layout"
version: "5.3.0"
last_updated: "2026-04-01"
---

# Checkboxes and Radios

## Overview

Checkboxes and radios are fundamental selection controls that allow users to choose one or more options from a set of predefined choices. Checkboxes permit multiple selections — each option can be independently toggled on or off. Radio buttons enforce single selection within a group — choosing one radio automatically deselects any previously selected radio in the same group.

Bootstrap 5 structures checkboxes and radios using the `form-check` container class, which wraps both the input and its label. The input element receives the `form-check-input` class for consistent sizing, spacing, and visual treatment. The label element receives the `form-check-label` class for appropriate typography and alignment. This three-class pattern (`form-check` > `form-check-input` + `form-check-label`) ensures that every checkbox and radio in the application shares identical visual and spacing behavior.

The semantic difference between checkboxes and radios is enforced by HTML attributes, not by Bootstrap classes. Checkboxes use `<input type="checkbox">` and can operate independently or as part of a group. Radios use `<input type="radio">` and must share the same `name` attribute to function as a mutually exclusive group. Bootstrap's styling respects these native behaviors while applying consistent visual design.

Inline layout is achieved by adding the `form-check-inline` class to each `form-check` container. This transforms the block-level stacking into a horizontal row using flexbox. The reverse layout variant uses the `form-check-reverse` class to swap the position of the input and label, placing the label on the left and the input on the right. This is useful in right-to-left (RTL) interfaces or design systems that prefer label-first layouts.

Bootstrap's switch toggle is a stylistic variant of the checkbox that renders as an iOS-style sliding toggle. It uses the `form-switch` class added to the `form-check` container. Functionally, the switch behaves identically to a checkbox — it submits the same form data and triggers the same change events. The visual difference is purely cosmetic and intended to represent binary on/off states more intuitively.

The indeterminate state is a programmatic-only property of checkboxes. It cannot be set via HTML attributes — only through JavaScript (`checkboxElement.indeterminate = true`). The indeterminate state visually represents a partial selection, commonly used in "select all" patterns where some but not all child items are checked. Bootstrap renders the indeterminate state as a horizontal dash inside the checkbox.

Each of these patterns integrates seamlessly with Bootstrap's validation system. Invalid checkboxes and radios receive the `is-invalid` class and display feedback via `invalid-feedback`. Valid states use `is-valid` and `valid-feedback`. The validation styling preserves the custom appearance of Bootstrap's checkbox and radio elements.

Accessibility for checkboxes and radios requires that every input has a visible label connected via the `for`/`id` relationship. Group-level context should be provided by a `<fieldset>` with a `<legend>`. Screen readers announce the group label, the individual option label, the checked/unchecked state, and the total number of options. Keyboard users navigate between options using Tab and select using Space.

This lesson covers standard checkboxes, radio groups, inline layouts, reverse ordering, switch toggles, indeterminate state management, validation integration, and accessibility best practices.

---

## Basic Implementation

### Standard Checkbox

A single checkbox with a label, wrapped in the `form-check` container.

```html
<div class="form-check">
  <input
    class="form-check-input"
    type="checkbox"
    value="terms"
    id="termsCheck"
  />
  <label class="form-check-label" for="termsCheck">
    I agree to the terms and conditions
  </label>
</div>
```

The `form-check` container manages the flex layout and spacing between the input and label. The `form-check-input` class positions and sizes the checkbox. The `form-check-label` class ensures proper alignment. The `for`/`id` association connects the label to the input for accessibility.

### Checkbox Group

Multiple checkboxes stacked vertically, each representing an independent selection.

```html
<fieldset>
  <legend>Select your interests</legend>
  <div class="form-check">
    <input class="form-check-input" type="checkbox" value="sports" id="interestSports" />
    <label class="form-check-label" for="interestSports">Sports</label>
  </div>
  <div class="form-check">
    <input class="form-check-input" type="checkbox" value="music" id="interestMusic" />
    <label class="form-check-label" for="interestMusic">Music</label>
  </div>
  <div class="form-check">
    <input class="form-check-input" type="checkbox" value="travel" id="interestTravel" />
    <label class="form-check-label" for="interestTravel">Travel</label>
  </div>
</fieldset>
```

The `<fieldset>` and `<legend>` elements provide a group-level label announced by screen readers when focus enters the group. Each checkbox retains its own unique `id` and `value`.

### Radio Button Group

Radio buttons enforce single selection within a group by sharing the same `name` attribute.

```html
<fieldset>
  <legend>Choose your preferred contact method</legend>
  <div class="form-check">
    <input
      class="form-check-input"
      type="radio"
      name="contactMethod"
      id="contactEmail"
      value="email"
      checked
    />
    <label class="form-check-label" for="contactEmail">Email</label>
  </div>
  <div class="form-check">
    <input
      class="form-check-input"
      type="radio"
      name="contactMethod"
      id="contactPhone"
      value="phone"
    />
    <label class="form-check-label" for="contactPhone">Phone</label>
  </div>
  <div class="form-check">
    <input
      class="form-check-input"
      type="radio"
      name="contactMethod"
      id="contactSMS"
      value="sms"
    />
    <label class="form-check-label" for="contactSMS">SMS</label>
  </div>
</fieldset>
```

All three radio inputs share `name="contactMethod"`, which creates the mutual exclusivity constraint. The `checked` attribute on the first radio sets it as the default selection.

### Default Checked State

Both checkboxes and radios accept the `checked` attribute to pre-select an option.

```html
<div class="form-check">
  <input
    class="form-check-input"
    type="checkbox"
    id="newsletterCheck"
    checked
  />
  <label class="form-check-label" for="newsletterCheck">
    Subscribe to our newsletter
  </label>
</div>
```

---

## Advanced Variations

### Inline Checkboxes

The `form-check-inline` class transforms block-level checkboxes into a horizontal row.

```html
<fieldset>
  <legend>Select notification channels</legend>
  <div class="form-check form-check-inline">
    <input class="form-check-input" type="checkbox" id="notifEmail" value="email" />
    <label class="form-check-label" for="notifEmail">Email</label>
  </div>
  <div class="form-check form-check-inline">
    <input class="form-check-input" type="checkbox" id="notifPush" value="push" />
    <label class="form-check-label" for="notifPush">Push</label>
  </div>
  <div class="form-check form-check-inline">
    <input class="form-check-input" type="checkbox" id="notifSMS" value="sms" />
    <label class="form-check-label" for="notifSMS">SMS</label>
  </div>
</fieldset>
```

Each `form-check` div gains `form-check-inline`, which applies `display: inline-flex` and horizontal margin. The options appear on a single line, wrapping to the next line if the container is too narrow.

### Inline Radios

The same `form-check-inline` class works for radio buttons.

```html
<fieldset>
  <legend>Select your plan</legend>
  <div class="form-check form-check-inline">
    <input
      class="form-check-input"
      type="radio"
      name="planType"
      id="planFree"
      value="free"
    />
    <label class="form-check-label" for="planFree">Free</label>
  </div>
  <div class="form-check form-check-inline">
    <input
      class="form-check-input"
      type="radio"
      name="planType"
      id="planPro"
      value="pro"
    />
    <label class="form-check-label" for="planPro">Pro</label>
  </div>
  <div class="form-check form-check-inline">
    <input
      class="form-check-input"
      type="radio"
      name="planType"
      id="planEnterprise"
      value="enterprise"
    />
    <label class="form-check-label" for="planEnterprise">Enterprise</label>
  </div>
</fieldset>
```

### Reverse Layout

The `form-check-reverse` class swaps the position of the input and label, placing the label on the left.

```html
<div class="form-check form-check-reverse">
  <input class="form-check-input" type="checkbox" id="acceptReverse" />
  <label class="form-check-label" for="acceptReverse">
    Accept privacy policy
  </label>
</div>
```

This is useful for RTL interfaces or design systems that prefer label-first layouts. The visual order changes without affecting the DOM order for accessibility.

### Switch Toggle

The `form-switch` class renders a checkbox as a sliding toggle switch.

```html
<div class="form-check form-switch">
  <input
    class="form-check-input"
    type="checkbox"
    role="switch"
    id="darkModeToggle"
  />
  <label class="form-check-label" for="darkModeToggle">Enable dark mode</label>
</div>
```

The `role="switch"` attribute is recommended for screen readers to communicate the binary on/off nature of the control. The visual appearance changes from a square checkbox to a pill-shaped toggle with a sliding circle indicator.

### Disabled Checkboxes and Radios

The `disabled` attribute on the input element grays out the control and prevents interaction.

```html
<div class="form-check">
  <input
    class="form-check-input"
    type="checkbox"
    id="disabledCheck"
    disabled
  />
  <label class="form-check-label" for="disabledCheck">
    This option is unavailable
  </label>
</div>
```

The label automatically receives reduced opacity through Bootstrap's CSS to visually indicate the disabled state.

### Indeterminate Checkbox State

The indeterminate state is set exclusively via JavaScript. It represents a partial selection — typically used in "select all" parent checkboxes when some but not all child checkboxes are checked.

```html
<div class="form-check">
  <input
    class="form-check-input"
    type="checkbox"
    id="selectAll"
  />
  <label class="form-check-label" for="selectAll">Select All</label>
</div>

<script>
  const selectAllCheckbox = document.getElementById('selectAll');
  selectAllCheckbox.indeterminate = true;
</script>
```

The `indeterminate` property is a JavaScript-only property — there is no HTML attribute equivalent. When set to `true`, Bootstrap renders a horizontal dash (—) inside the checkbox. The checkbox is neither checked nor unchecked; it reflects the combined state of its child selections.

To implement a functional "select all" pattern:

```html
<fieldset>
  <legend>Select items</legend>
  <div class="form-check">
    <input class="form-check-input" type="checkbox" id="selectAll" />
    <label class="form-check-label fw-bold" for="selectAll">Select All</label>
  </div>
  <hr />
  <div class="form-check">
    <input class="form-check-input item-check" type="checkbox" id="item1" />
    <label class="form-check-label" for="item1">Item 1</label>
  </div>
  <div class="form-check">
    <input class="form-check-input item-check" type="checkbox" id="item2" />
    <label class="form-check-label" for="item2">Item 2</label>
  </div>
  <div class="form-check">
    <input class="form-check-input item-check" type="checkbox" id="item3" />
    <label class="form-check-label" for="item3">Item 3</label>
  </div>
</fieldset>

<script>
  const selectAll = document.getElementById('selectAll');
  const itemChecks = document.querySelectorAll('.item-check');

  function updateSelectAll() {
    const checked = document.querySelectorAll('.item-check:checked').length;
    const total = itemChecks.length;
    selectAll.checked = checked === total;
    selectAll.indeterminate = checked > 0 && checked < total;
  }

  selectAll.addEventListener('change', () => {
    itemChecks.forEach(cb => cb.checked = selectAll.checked);
    updateSelectAll();
  });

  itemChecks.forEach(cb => {
    cb.addEventListener('change', updateSelectAll);
  });
</script>
```

When all items are checked, the "Select All" checkbox shows checked. When none are checked, it shows unchecked. When some but not all are checked, it shows indeterminate (dash).

---

## Best Practices

1. **Always use `<fieldset>` and `<legend>` for checkbox and radio groups.** The `<fieldset>` groups related controls semantically, and the `<legend>` provides a group-level label announced by screen readers when users enter the group. This is the primary accessibility mechanism for multi-option selection controls.

2. **Connect every input to its label with matching `for`/`id` attributes.** This is the fundamental accessibility requirement. Without this connection, screen readers cannot announce what each option represents. The `for` value must exactly match the `id` value.

3. **Use `name` attribute on radio groups to enforce mutual exclusivity.** Radio buttons only deselect each other when they share the same `name` attribute. Without this, each radio operates independently like a checkbox, violating user expectations.

4. **Apply `form-check-inline` consistently within a group.** Do not mix inline and block-level checks within the same group — it creates visual inconsistency. Decide on a layout strategy (stacked or inline) and apply it uniformly.

5. **Set a default selection on radio groups when one option is pre-selected by business logic.** Users should always see one radio checked if a default choice exists. Use the `checked` attribute on the appropriate radio input.

6. **Use the `role="switch"` attribute on switch toggles.** This signals to screen readers that the control is a binary toggle, not a standard checkbox. Some screen readers announce "switch" instead of "checkbox," providing clearer semantics.

7. **Implement indeterminate state only through JavaScript.** There is no HTML attribute for indeterminate. Attempting to set it in HTML has no effect. Always set `checkbox.indeterminate = true` (or `false`) via JavaScript after the DOM is available.

8. **Provide visual feedback for disabled options.** The `disabled` attribute grays out the input and label automatically. In addition, consider adding a tooltip or helper text explaining why the option is unavailable, improving the user experience.

9. **Avoid using checkboxes for binary on/off settings where a switch is more intuitive.** The switch toggle provides a stronger visual metaphor for enable/disable and on/off states. Use standard checkboxes for multi-select lists and terms acceptance.

10. **Validate checkbox and radio groups at the group level, not individually.** When a radio group requires selection, the error message should appear once for the entire group, not beside each radio. Use `invalid-feedback` on the last item in the group or on a wrapper element.

11. **Maintain adequate spacing between inline options.** Bootstrap applies margin via `form-check-inline`. If spacing feels too tight, add a margin utility (e.g., `me-3`) to increase the gap without disrupting the flex layout.

12. **Test keyboard navigation for checkbox and radio groups.** Users should be able to Tab into a checkbox group and Space to toggle each item. For radio groups, arrow keys should move between options within the same `name` group. Verify this behavior across browsers.

---

## Common Pitfalls

1. **Missing `for`/`id` connection between label and input.** This is the most common accessibility failure in forms. Without the connection, clicking the label does not toggle the input, and screen readers cannot announce the label text for the input.

2. **Forgetting the `name` attribute on radio buttons.** Without a shared `name`, radio buttons do not deselect each other. Each operates independently, which defeats the purpose of using radios over checkboxes. Always verify that all radios in a group share the same `name`.

3. **Using `disabled` on the label instead of the input.** The `disabled` attribute must be placed on the `<input>` element. Placing it on the `<label>` has no functional effect — the input remains interactive. Bootstrap styles the label appearance based on the input's disabled state.

4. **Attempting to set indeterminate via HTML.** There is no `indeterminate` HTML attribute. Only `checked` can be set in markup. The indeterminate state is exclusively a JavaScript property (`element.indeterminate = true`). Attempting `<input type="checkbox" indeterminate>` is invalid HTML.

5. **Mixing inline and non-inline checks within the same group.** Combining `form-check-inline` on some items and omitting it on others creates an inconsistent layout with items appearing at different positions and alignments.

6. **Omitting `<fieldset>` and `<legend>` for groups.** Without `<fieldset>`, screen readers announce each option individually without group context. Users cannot determine what the group of options is asking. The `<legend>` provides the essential group-level label.

7. **Not providing a default selection on required radio groups.** If a radio group is required, failing to pre-select an option forces users to make a choice. However, some design systems intentionally leave radios unselected to indicate that no default assumption is made. Be intentional about this decision and document the pattern.

8. **Using `checked` attribute as a JavaScript state setter.** The `checked` HTML attribute only sets the initial state on page load. To programmatically change the checked state after load, use `checkbox.checked = true` in JavaScript. The attribute and the property serve different purposes.

9. **Not handling indeterminate state transitions in "select all" patterns.** When a user manually checks individual items, the "select all" checkbox must transition between unchecked, indeterminate, and checked states. Failing to handle all three states results in misleading UI.

10. **Forgetting `role="switch"` on toggle switches.** Without `role="switch"`, screen readers announce the control as a checkbox. Adding the role ensures assistive technologies communicate the binary toggle nature, improving comprehension for users relying on audio feedback.

---

## Accessibility Considerations

Checkboxes and radios are natively accessible HTML elements, but Bootstrap's custom styling requires maintaining the correct semantic structure. The `form-check-input` class visually customizes the native input element rather than replacing it with a non-semantic `<div>`-based control. This preserves built-in keyboard navigation, focus management, and screen reader announcements.

Every checkbox and radio must have a visible label associated via `for`/`id`. For cases where a visible label is impractical (such as icon-only controls), use `aria-label` on the input element. However, visible labels are always preferred.

```html
<div class="form-check">
  <input
    class="form-check-input"
    type="checkbox"
    id="agreeTerms"
    aria-describedby="termsHelp"
  />
  <label class="form-check-label" for="agreeTerms">
    I agree to the terms
  </label>
  <div id="termsHelp" class="form-text">
    You must accept the terms to create an account.
  </div>
</div>
```

The `aria-describedby` attribute links the helper text to the checkbox, ensuring screen readers announce it when the user focuses on the input.

For radio groups, the `<fieldset>` and `<legend>` elements are mandatory for accessibility. The `<legend>` is announced when the user enters the group, providing context for the individual options.

```html
<fieldset>
  <legend>Select your preferred delivery method</legend>
  <div class="form-check">
    <input class="form-check-input" type="radio" name="delivery" id="deliveryStd" value="standard" checked />
    <label class="form-check-label" for="deliveryStd">Standard (5-7 days)</label>
  </div>
  <div class="form-check">
    <input class="form-check-input" type="radio" name="delivery" id="deliveryExp" value="express" />
    <label class="form-check-label" for="deliveryExp">Express (2-3 days)</label>
  </div>
</fieldset>
```

For invalid checkboxes or radios, use `aria-invalid="true"` on the input and reference the error message with `aria-describedby`. This ensures screen readers announce the error when the user focuses on the invalid control.

```html
<div class="form-check">
  <input
    class="form-check-input is-invalid"
    type="checkbox"
    id="requiredCheck"
    aria-invalid="true"
    aria-describedby="requiredError"
  />
  <label class="form-check-label" for="requiredCheck">Required option</label>
  <div id="requiredError" class="invalid-feedback">
    You must check this box to proceed.
  </div>
</div>
```

Keyboard navigation for checkboxes supports Tab to focus and Space to toggle. Radio groups support Tab to enter the group, arrow keys to move between radios within the same `name` group, and Space to select. These behaviors are natively provided by the browser — Bootstrap's CSS does not alter them.

---

## Responsive Behavior

Checkboxes and radios are inline by default in their natural HTML rendering, but Bootstrap's `form-check` class makes them block-level elements that stack vertically. This stacking behavior is inherently responsive — on narrow screens, each option occupies a full row, maximizing touch target area.

The `form-check-inline` class creates horizontal layouts that wrap naturally when the container is too narrow. On mobile screens, inline checks may wrap to multiple lines, which is acceptable behavior. However, for groups with many options (5+), prefer stacked layout on all screen sizes to avoid excessive wrapping.

To create responsive layouts that switch between stacked and inline, use Bootstrap's display utilities with breakpoint suffixes:

```html
<fieldset>
  <legend>Select categories</legend>
  <div class="form-check d-block d-md-inline-block me-md-3 mb-2 mb-md-0">
    <input class="form-check-input" type="checkbox" id="catTech" />
    <label class="form-check-label" for="catTech">Technology</label>
  </div>
  <div class="form-check d-block d-md-inline-block me-md-3 mb-2 mb-md-0">
    <input class="form-check-input" type="checkbox" id="catDesign" />
    <label class="form-check-label" for="catDesign">Design</label>
  </div>
  <div class="form-check d-block d-md-inline-block me-md-3 mb-2 mb-md-0">
    <input class="form-check-input" type="checkbox" id="catBusiness" />
    <label class="form-check-label" for="catBusiness">Business</label>
  </div>
</fieldset>
```

On screens below the `md` breakpoint (768px), each checkbox displays as a block-level element stacked vertically. On `md` and wider screens, they display inline-block with horizontal spacing via `me-md-3`.

Switch toggles follow the same responsive behavior as standard checkboxes. They are full-width by default and scale proportionally with their container. No additional responsive classes are needed for switches.

For horizontal form layouts using the grid system, checkboxes and radios can be placed within grid columns alongside labels:

```html
<div class="row mb-3">
  <div class="col-sm-3">
    <label class="col-form-label">Notifications</label>
  </div>
  <div class="col-sm-9">
    <div class="form-check">
      <input class="form-check-input" type="checkbox" id="emailNotif" />
      <label class="form-check-label" for="emailNotif">Email notifications</label>
    </div>
    <div class="form-check">
      <input class="form-check-input" type="checkbox" id="smsNotif" />
      <label class="form-check-label" for="smsNotif">SMS notifications</label>
    </div>
  </div>
</div>
```

On small screens, the grid columns collapse and the checkboxes stack below the label. On `sm` and wider, the label occupies 3 columns and the checkboxes occupy 9, creating a clean horizontal layout.
