---
title: Input Groups
category: Form Ecosystem
difficulty: 2
time: 35 minutes
tags:
  - bootstrap
  - forms
  - input-group
  - prepend
  - append
  - addons
---

## Overview

Input groups extend form controls by attaching text, buttons, dropdowns, or other elements to either side of an input field. They provide visual context and reduce the need for placeholder text by showing persistent labels, units, icons, or actions directly alongside the control. The `.input-group` wrapper class creates a flex container that horizontally arranges the input and its addons without gaps.

Bootstrap input groups are composed of three main parts: the wrapper (`.input-group`), text addons (`.input-group-text`), and the form control itself. Addons can be placed before the input using `.input-group-prepend` (or directly as children in Bootstrap 5.1+) or after using `.input-group-append`. In Bootstrap 5.3+, the prepend/append wrapper classes are optional; `.input-group-text` elements placed directly inside `.input-group` are automatically positioned based on their order relative to the form control.

Input groups support buttons, dropdowns, checkboxes, and radio buttons as addons. They can be sized with `.input-group-sm` and `.input-group-lg` to match the form control size. Multiple addons can be chained on either side of the input, and they work with all standard form control types including text inputs, selects, and textareas.

The component is particularly useful for fields where the context is a unit of measurement ($, kg, %), a URL protocol (https://), or an action (search button, password toggle). By keeping this context visually attached to the input, the form remains compact and self-documenting.

## Basic Implementation

The simplest input group adds text to the left of a form control. The `.input-group-text` class styles the addon to visually connect with the input.

```html
<div class="mb-3">
  <label for="basicPrepend" class="form-label">Username</label>
  <div class="input-group">
    <span class="input-group-text">@</span>
    <input type="text" class="form-control" id="basicPrepend" placeholder="Username">
  </div>
</div>
```

An addon on the right side of the input is created by placing the `.input-group-text` element after the form control.

```html
<div class="mb-3">
  <label for="basicAppend" class="form-label">Email</label>
  <div class="input-group">
    <input type="text" class="form-control" id="basicAppend" placeholder="your-email">
    <span class="input-group-text">@example.com</span>
  </div>
</div>
```

Addons can appear on both sides simultaneously. This is common for currency fields that show a symbol on the left and a denomination on the right.

```html
<div class="mb-3">
  <label for="bothSides" class="form-label">Amount</label>
  <div class="input-group">
    <span class="input-group-text">$</span>
    <input type="number" class="form-control" id="bothSides" placeholder="0.00">
    <span class="input-group-text">USD</span>
  </div>
</div>
```

Multiple text addons can be stacked on the same side for multi-segment prefixes or suffixes.

```html
<div class="mb-3">
  <label for="multiAddon" class="form-label">Website</label>
  <div class="input-group">
    <span class="input-group-text">https://</span>
    <span class="input-group-text">www.</span>
    <input type="text" class="form-control" id="multiAddon" placeholder="example.com">
  </div>
</div>
```

A button as an addon provides inline actions such as copy, search, or toggle functionality.

```html
<div class="mb-3">
  <label for="buttonAddon" class="form-label">Search</label>
  <div class="input-group">
    <input type="text" class="form-control" id="buttonAddon" placeholder="Search...">
    <button class="btn btn-outline-primary" type="button">Search</button>
  </div>
</div>
```

## Advanced Variations

Dropdown menus can be used as input group addons. Place a `.dropdown` container inside the input group with a dropdown toggle button and menu.

```html
<div class="mb-3">
  <label for="dropdownAddon" class="form-label">Filter by Category</label>
  <div class="input-group">
    <button class="btn btn-outline-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown">
      Category
    </button>
    <ul class="dropdown-menu">
      <li><a class="dropdown-item" href="#">Books</a></li>
      <li><a class="dropdown-item" href="#">Electronics</a></li>
      <li><a class="dropdown-item" href="#">Clothing</a></li>
    </ul>
    <input type="text" class="form-control" id="dropdownAddon" placeholder="Enter search term">
  </div>
</div>
```

Segmented buttons combine a primary action button with a dropdown toggle. This pattern is common for forms that offer a default submit action alongside alternative options.

```html
<div class="mb-3">
  <label for="segmentedBtn" class="form-label">Action with Options</label>
  <div class="input-group">
    <input type="text" class="form-control" id="segmentedBtn" placeholder="Enter value">
    <button type="button" class="btn btn-primary">Submit</button>
    <button type="button" class="btn btn-primary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown">
      <span class="visually-hidden">Toggle Dropdown</span>
    </button>
    <ul class="dropdown-menu dropdown-menu-end">
      <li><a class="dropdown-item" href="#">Submit & Add Another</a></li>
      <li><a class="dropdown-item" href="#">Submit as Draft</a></li>
      <li><hr class="dropdown-divider"></li>
      <li><a class="dropdown-item" href="#">Cancel</a></li>
    </ul>
  </div>
</div>
```

Checkboxes and radio buttons can serve as input group addons for toggle-style controls alongside a text input.

```html
<div class="mb-3">
  <label for="checkAddon" class="form-label">Promo Code</label>
  <div class="input-group">
    <div class="input-group-text">
      <input class="form-check-input mt-0" type="checkbox" id="promoCheck">
    </div>
    <input type="text" class="form-control" id="checkAddon" placeholder="Enter promo code">
  </div>
</div>

<div class="mb-3">
  <label for="radioAddon" class="form-label">Amount (with payment type)</label>
  <div class="input-group">
    <div class="input-group-text">
      <input class="form-check-input mt-0" type="radio" name="payType" checked>
    </div>
    <span class="input-group-text">$</span>
    <input type="number" class="form-control" id="radioAddon" placeholder="0.00">
  </div>
</div>
```

Input groups with custom HTML content allow embedding badges, icons (from icon libraries), or small status indicators.

```html
<div class="mb-3">
  <label for="iconAddon" class="form-label">Email with Status</label>
  <div class="input-group">
    <span class="input-group-text">
      <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
        <path d="M0 4a2 2 0 0 1 2-2h12a2 2 0 0 1 2 2v8a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2V4Zm2-1a1 1 0 0 0-1 1v.217l7 4.2 7-4.2V4a1 1 0 0 0-1-1H2Zm13 2.383-4.708 2.825L15 11.105V5.383Zm-.034 6.876-5.64-3.471L8 9.583l-1.326-.795-5.64 3.47A1 1 0 0 0 2 13h12a1 1 0 0 0 .966-.741ZM1 11.105l4.708-2.897L1 5.383v5.722Z"/>
      </svg>
    </span>
    <input type="email" class="form-control" id="iconAddon" placeholder="you@example.com">
    <span class="input-group-text text-success">
      <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
        <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>
      </svg>
    </span>
  </div>
</div>
```

## Best Practices

- **Always use a `<label>` with every input group.** The input group does not replace the need for a form label. Screen readers require the label association to understand the input's purpose.
- **Place `.input-group-text` elements directly inside `.input-group`** in Bootstrap 5.1+. The `.input-group-prepend` and `.input-group-append` wrapper classes are no longer required and add unnecessary markup.
- **Use semantic HTML for addons.** Use `<span>` for text addons, `<button>` for action addons, and `<label>` or `<div>` for checkbox/radio addons. Choosing the correct element improves accessibility and behavior.
- **Match input group size to the form control size.** Apply `.input-group-sm` or `.input-group-lg` to the `.input-group` wrapper, not to individual elements inside it.
- **Avoid placing validation messages inside the input group.** Validation feedback (`.invalid-feedback`) should be a sibling of the `.input-group`, not nested inside it, to maintain proper CSS selector behavior.
- **Use `aria-label` or `aria-describedby`** when the addon text alone does not fully describe the input's purpose. For example, a "$" addon tells the user the field is monetary, but the label should still say "Price."
- **Do not nest multiple `.input-group` elements.** Input groups are not designed for nesting. If you need a complex layout, use Bootstrap's grid system to arrange separate input groups side by side.
- **Keep addon text brief.** Addons should be one or two characters (like "$", "kg", "@") or a short word. Long text in addons causes layout issues on small screens.
- **Use `type="button"` on button addons** to prevent accidental form submission. A button inside an input group should not trigger form submission unless explicitly intended.
- **Test with keyboard navigation.** Ensure that users can Tab through the input group and interact with buttons or dropdowns inside it using Enter and Space keys.
- **Use `input-group-text` for all non-input addons.** This class applies consistent styling (background, border, padding) that visually connects the addon to the adjacent form control.
- **Consider the visual order for screen readers.** If the DOM order differs from the visual order (e.g., using CSS `order`), screen readers will still read the DOM order. Keep the logical reading order consistent with the visual layout.

## Common Pitfalls

- **Forgetting the `<label>` element.** An input group without a label is inaccessible. Users with screen readers will hear the addon text and the input type but will not understand what the input is for.
- **Using `.input-group-prepend` and `.input-group-append` wrappers** when they are not needed. In Bootstrap 5.1+, these wrappers are optional. Adding them creates unnecessary DOM depth and can interfere with certain CSS selectors.
- **Placing validation feedback inside the `.input-group`.** The `.invalid-feedback` class uses the `:not(sibling)` selector pattern to show/hide. Nesting it inside the input group breaks this mechanism. Place the feedback div after the closing `</div>` of the input group.
- **Not sizing both the input and the addon.** If you apply `.form-control-sm` to the input but not `.input-group-sm` to the wrapper, the addon will remain at the default size, creating a visual mismatch.
- **Overloading the input group with too many addons.** While you can place multiple addons on each side, more than two on either side causes overflow on narrow screens. Keep addons to a minimum.
- **Using a dropdown inside an input group without initializing the Bootstrap JavaScript.** The dropdown toggle button requires Bootstrap's Dropdown plugin. Without it, clicking the toggle does nothing.
- **Expecting `flex-fill` to work inside an input group.** The input group already uses flexbox internally. Applying `flex-fill` to the input or addon can conflict with the group's layout. Use the `.form-control` class on the input to let it fill available space naturally.
- **Using input groups with floating labels.** Bootstrap does not support combining `.input-group` with `.form-floating`. These two patterns are mutually exclusive. Choose one based on your design requirements.
- **Not handling the `disabled` state on button addons.** A disabled button inside an input group should use the `disabled` attribute to prevent interaction and apply Bootstrap's disabled styling.

## Accessibility Considerations

Input groups are accessible when implemented with proper semantic HTML and ARIA attributes. The `.input-group` wrapper is a presentational element (it uses `display: flex`) and does not add any ARIA roles. The accessibility of the component depends entirely on the elements it contains.

The form control inside the input group must have a `<label>` associated with it via the `for` and `id` attributes. The addon text is supplementary context and should not be relied upon as the sole source of the input's accessible name. Screen readers will announce the label, the addon text content, and the input type in sequence.

For input groups containing buttons, ensure that the button has an accessible name. If the button contains only an icon, add `aria-label` to describe the action.

```html
<div class="input-group">
  <input type="text" class="form-control" id="searchInput" placeholder="Search...">
  <button class="btn btn-primary" type="button" aria-label="Submit search">
    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
      <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z"/>
    </svg>
  </button>
</div>
```

When using a checkbox or radio button as an addon, the `<label>` for the checkbox/radio should be hidden visually but present for screen readers, or the `aria-label` attribute should be used on the input element itself.

For dropdown addons within input groups, the dropdown menu items must be keyboard-navigable. Bootstrap's Dropdown plugin handles this automatically, but verify that arrow keys move through items and Escape closes the menu.

## Responsive Behavior

Input groups span the full width of their parent container by default. When placed inside Bootstrap's grid columns, they adapt to the column width. On small screens where columns stack to full width, the input group stretches accordingly.

For horizontal arrangements, use the grid system to place input groups side by side. Each input group should be inside its own column so that they wrap independently on smaller viewports.

```html
<div class="row">
  <div class="col-md-6 mb-3">
    <label for="respMin" class="form-label">Min Price</label>
    <div class="input-group">
      <span class="input-group-text">$</span>
      <input type="number" class="form-control" id="respMin" placeholder="0">
    </div>
  </div>

  <div class="col-md-6 mb-3">
    <label for="respMax" class="form-label">Max Price</label>
    <div class="input-group">
      <span class="input-group-text">$</span>
      <input type="number" class="form-control" id="respMax" placeholder="1000">
    </div>
  </div>
</div>
```

When input groups contain long addon text, they may overflow on narrow screens. Mitigate this by shortening addon text on mobile (e.g., showing "$" instead of "USD") or by using CSS to truncate the addon content.

```html
<div class="mb-3">
  <label for="truncAddon" class="form-label">Website URL</label>
  <div class="input-group">
    <span class="input-group-text text-truncate" style="max-width: 120px;">https://www.</span>
    <input type="text" class="form-control" id="truncAddon" placeholder="example.com">
  </div>
</div>
```

Input group sizes behave predictably across breakpoints. `.input-group-sm` and `.input-group-lg` set fixed sizes that do not change with viewport width. If you need the input group to scale, place it inside a responsive grid column and let the column handle the width adjustments.

For forms that switch between horizontal and vertical layouts on different screen sizes, input groups integrate naturally. Inside a horizontal form on desktop, the input group fits within the input column. On mobile, when the form stacks vertically, the input group stretches to full width without additional markup changes.
