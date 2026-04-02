---
title: Floating Labels
category: Form Ecosystem
difficulty: 2
time: 30 minutes
tags:
  - bootstrap
  - forms
  - floating-labels
  - form-floating
  - placeholder
---

## Overview

Floating labels are a modern form design pattern where the label text starts inside the input field and animates above it when the user focuses or enters a value. Bootstrap implements this pattern with the `.form-floating` class, which wraps the form control and its label. The label automatically floats above the input when the field has a value, is focused, or has the `:autofill` pseudo-class applied by the browser.

The `.form-floating` container requires specific element ordering: the form control (`<input>`, `<select>`, or `<textarea>`) must come first, followed by the `<label>` element. This reverse ordering is necessary for CSS adjacent sibling selectors to detect the input's state and position the label accordingly. The input must also have a `placeholder` attribute (even if empty) because Bootstrap uses the `:placeholder-shown` pseudo-class to determine whether the label should float.

Floating labels work with text inputs, email inputs, password inputs, number inputs, textareas, and selects. They do not work with input groups, checkboxes, radios, or range inputs. The pattern reduces vertical space by combining the label and input into a single visual unit, making it popular in sign-up forms, login screens, and compact data entry interfaces.

Bootstrap 5.3 enhanced floating labels with CSS custom properties for color customization and better support for validation states. When an input with a floating label is validated (`.is-valid` or `.is-invalid`), the label color changes to match the validation state, and feedback messages appear below the input.

## Basic Implementation

The simplest floating label requires a `.form-floating` wrapper with an input and a label. The input must have a `placeholder` attribute for the label to animate correctly.

```html
<div class="form-floating mb-3">
  <input type="text" class="form-control" id="floatingName" placeholder="Name">
  <label for="floatingName">Full Name</label>
</div>
```

An email input with a floating label works identically. The `placeholder` value can be any text; it is never visible to the user because the label covers it.

```html
<div class="form-floating mb-3">
  <input type="email" class="form-control" id="floatingEmail" placeholder="Email">
  <label for="floatingEmail">Email address</label>
</div>
```

A password input with a floating label follows the same structure.

```html
<div class="form-floating mb-3">
  <input type="password" class="form-control" id="floatingPassword" placeholder="Password">
  <label for="floatingPassword">Password</label>
</div>
```

Pre-filled inputs display with the label already floating above the value.

```html
<div class="form-floating mb-3">
  <input type="text" class="form-control" id="floatingPreFilled" placeholder="Username" value="john_doe">
  <label for="floatingPreFilled">Username</label>
</div>
```

A readonly floating label input keeps the label floating and prevents editing.

```html
<div class="form-floating mb-3">
  <input type="text" class="form-control" id="floatingReadonly" placeholder="ID" value="USR-2026-0042" readonly>
  <label for="floatingReadonly">User ID</label>
</div>
```

## Advanced Variations

Textarea elements with floating labels require the `placeholder` attribute and a defined height. The label floats when the textarea is focused or contains text.

```html
<div class="form-floating mb-3">
  <textarea class="form-control" id="floatingTextarea" placeholder="Comments" style="height: 120px;"></textarea>
  <label for="floatingTextarea">Comments</label>
</div>
```

Select elements with floating labels require an empty `<option selected>` as the first option. The label floats when an option other than the empty placeholder is selected.

```html
<div class="form-floating mb-3">
  <select class="form-select" id="floatingSelect" aria-label="Floating label select">
    <option selected></option>
    <option value="1">React</option>
    <option value="2">Vue</option>
    <option value="3">Angular</option>
  </select>
  <label for="floatingSelect">Preferred Framework</label>
</div>
```

Combining floating labels with validation requires placing the validation feedback div after the label inside the `.form-floating` container. The validation classes are applied to the form control as usual.

```html
<div class="form-floating mb-3">
  <input
    type="email"
    class="form-control is-invalid"
    id="floatingValidEmail"
    placeholder="Email"
    value="invalid-email"
  >
  <label for="floatingValidEmail">Email</label>
  <div class="invalid-feedback">Please enter a valid email address.</div>
</div>

<div class="form-floating mb-3">
  <input
    type="text"
    class="form-control is-valid"
    id="floatingValidName"
    placeholder="Name"
    value="Jane Smith"
  >
  <label for="floatingValidName">Full Name</label>
  <div class="valid-feedback">Looks great!</div>
</div>
```

Floating labels with the `.was-validated` form class work automatically. Each control shows its validation state after form submission.

```html
<form class="was-validated" novalidate>
  <div class="form-floating mb-3">
    <input type="text" class="form-control" id="floatingReq" placeholder="Name" required>
    <label for="floatingReq">Full Name</label>
    <div class="invalid-feedback">Name is required.</div>
  </div>

  <div class="form-floating mb-3">
    <input type="email" class="form-control" id="floatingReqEmail" placeholder="Email" required>
    <label for="floatingReqEmail">Email</label>
    <div class="invalid-feedback">Please provide a valid email.</div>
  </div>

  <button type="submit" class="btn btn-primary">Submit</button>
</form>
```

Custom colors on floating labels can be applied using Bootstrap 5.3 CSS custom properties or direct CSS targeting.

```html
<style>
  .custom-floating .form-control:focus ~ label,
  .custom-floating .form-control:not(:placeholder-shown) ~ label {
    color: #6f42c1;
  }
</style>

<div class="form-floating custom-floating mb-3">
  <input type="text" class="form-control" id="floatingCustom" placeholder="Custom">
  <label for="floatingCustom">Custom Colored Label</label>
</div>
```

## Best Practices

- **Always include a `placeholder` attribute** on the input, even if it is an empty string (`placeholder=""`). The `:placeholder-shown` CSS pseudo-class is required for Bootstrap to detect whether the input has content.
- **Place the input before the label** inside the `.form-floating` wrapper. The CSS adjacent sibling selector (`input ~ label`) depends on this order. Reversing them breaks the floating animation.
- **Use an empty `<option selected>` as the first option** in floating label selects. This acts as the placeholder equivalent for selects and allows the label to collapse when the user selects a real option.
- **Do not use floating labels with input groups.** The `.input-group` and `.form-floating` classes use conflicting flex and positioning styles that cause layout breakage.
- **Set a fixed height on floating label textareas.** Without an explicit `style="height: ..."` or a height utility class, the textarea may collapse to a single line, making it difficult to interact with.
- **Include validation feedback after the `<label>`** inside the `.form-floating` div. The feedback must be a sibling of both the input and the label for Bootstrap's CSS to position it correctly.
- **Use `autocomplete` attributes** on floating label inputs for login and registration forms. Browser autofill triggers the `:autofill` pseudo-class, which floats the label even without user interaction.
- **Avoid using `placeholder` text as the visible label.** The label element is the accessible name for the input. The `placeholder` attribute is a technical requirement for the CSS, not a user-facing label.
- **Test floating labels with pre-filled values.** If a browser autofills or a form is pre-populated, the label must start in the floated position. Verify this by testing with `value` attributes and browser autofill.
- **Keep labels concise.** Long labels may overlap the input value on small screens. Aim for one to three words (e.g., "Email address", "Password", "Full Name").
- **Use `.form-floating` on the correct wrapper element.** It must be a `<div>` that directly contains the input and label. Nesting an intermediate container breaks the CSS selectors.
- **Provide adequate bottom margin** on the `.form-floating` container. Use `mb-3` or similar utilities to prevent the floating label from overlapping adjacent form controls.

## Common Pitfalls

- **Forgetting the `placeholder` attribute.** Without it, the `:placeholder-shown` pseudo-class never applies, and the label remains inside the input at all times, covering the user's typed text.
- **Placing the label before the input.** Bootstrap's CSS uses the `~` general sibling combinator, which only matches elements that come later in the DOM. Putting the label first means the CSS rules never activate.
- **Using floating labels with `.input-group`.** These two components are incompatible. The input group's flex layout conflicts with the floating label's absolute positioning. Choose one pattern or the other.
- **Not setting an empty first `<option>` on floating selects.** Without the empty option, the select always has a value, and the label never shows its collapsed (floated) state for the placeholder.
- **Placing validation feedback outside the `.form-floating` div.** The feedback div must be inside the `.form-floating` container to maintain proper spacing and visibility rules.
- **Using `type="date"` or `type="time"` with floating labels.** These input types always have a value (formatted by the browser), which causes the label to float permanently. This is usually acceptable, but it differs from the behavior of text inputs where the label starts inside the field.
- **Applying floating labels to checkboxes or radio buttons.** Floating labels only work with inputs that have a text-based or select-based interaction model. Checkboxes and radios are not supported.
- **Missing `for` and `id` association** between the label and input. Without this, clicking the label does not focus the input, and screen readers cannot connect the label to the control.
- **Overriding the label's `position` property.** Bootstrap uses `position: absolute` on the label to overlay it on the input. Changing this breaks the floating behavior.

## Accessibility Considerations

Floating labels are accessible when the `<label>` element is properly associated with the input using `for` and `id` attributes. Screen readers announce the label text as the input's accessible name regardless of the label's visual position. The floating animation is purely visual and does not affect the accessibility tree.

However, there are specific concerns to address. The `placeholder` attribute, while required for the CSS to function, should be set to an empty string or a value that does not duplicate the label text. Screen readers may announce the placeholder text in addition to the label, creating redundant audio output.

```html
<div class="form-floating mb-3">
  <input type="text" class="form-control" id="accName" placeholder="">
  <label for="accName">Full Name</label>
</div>
```

When validation feedback is present, link it to the input with `aria-describedby` so that screen readers announce the error or success message when the user focuses the input.

```html
<div class="form-floating mb-3">
  <input
    type="email"
    class="form-control is-invalid"
    id="accEmail"
    placeholder=""
    aria-describedby="accEmailFeedback"
  >
  <label for="accEmail">Email Address</label>
  <div id="accEmailFeedback" class="invalid-feedback">
    Please enter a valid email address.
  </div>
</div>
```

The floating label animation relies on CSS transitions, which are not announced by screen readers. Users who rely on assistive technology will interact with the form through the input's accessible name and state, not through the visual label movement. This is the correct behavior and does not require additional ARIA attributes.

For selects with floating labels, ensure that the empty first option has an `aria-label` or descriptive text so screen readers do not announce a completely empty option as the selected value.

```html
<div class="form-floating mb-3">
  <select class="form-select" id="accSelect" aria-label="Choose your country">
    <option selected value="">-- Select a country --</option>
    <option value="us">United States</option>
    <option value="uk">United Kingdom</option>
    <option value="ca">Canada</option>
  </select>
  <label for="accSelect">Country</label>
</div>
```

## Responsive Behavior

Floating labels are inherently responsive. The `.form-floating` container stretches to fill its parent, and the label animates above the input at all screen sizes. No additional responsive classes are needed for basic functionality.

On narrow screens, the floating label remains above the input when the field has content. The main responsive consideration is label length. Labels that fit comfortably on desktop may truncate or overflow on mobile. Keep labels to one or two words to ensure they remain readable on all viewports.

When placing floating label inputs inside Bootstrap's grid columns, they adapt to the column width automatically. A `col-md-6` column with a floating label input will be half-width on medium screens and full-width on small screens.

```html
<div class="row">
  <div class="col-md-6 mb-3">
    <div class="form-floating">
      <input type="text" class="form-control" id="respFirst" placeholder="">
      <label for="respFirst">First Name</label>
    </div>
  </div>

  <div class="col-md-6 mb-3">
    <div class="form-floating">
      <input type="text" class="form-control" id="respLast" placeholder="">
      <label for="respLast">Last Name</label>
    </div>
  </div>
</div>
```

For forms that switch between a compact floating label layout on desktop and a traditional stacked label layout on mobile, you can toggle the `.form-floating` class with JavaScript based on viewport width. However, this is rarely necessary because floating labels work well at all sizes.

Textareas with floating labels may need different heights at different breakpoints. Use CSS media queries or Bootstrap's responsive display utilities to adjust the textarea height.

```html
<div class="form-floating mb-3">
  <textarea
    class="form-control"
    id="respTextarea"
    placeholder=""
    style="height: 150px;"
  ></textarea>
  <label for="respTextarea">Your Message</label>
</div>
```

On high-DPI displays and devices with dynamic zoom (such as browsers on tablets), verify that the floating label remains properly positioned above the input. The `transform` and `font-size` transitions used by Bootstrap are relative and should scale correctly, but custom CSS overrides may interfere with this behavior.
