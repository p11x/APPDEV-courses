---
title: Date and Time Pickers
category: Component System
difficulty: 2
time: 25 min
tags: bootstrap5, date-picker, time-picker, datetime-local, forms
---

## Overview

Bootstrap provides styling for native HTML date and time inputs using `form-control` classes. The browser renders the native date picker UI for `<input type="date">`, `<input type="time">`, and `<input type="datetime-local">`. Bootstrap applies consistent border, padding, and focus styles. For richer experiences, you can combine Bootstrap form controls with third-party date picker libraries or build custom calendar patterns.

## Basic Implementation

Native date and time inputs styled with Bootstrap's `form-control` class.

```html
<!-- Date input -->
<div class="mb-3">
  <label for="dateInput" class="form-label">Date</label>
  <input type="date" class="form-control" id="dateInput">
</div>

<!-- Time input -->
<div class="mb-3">
  <label for="timeInput" class="form-label">Time</label>
  <input type="time" class="form-control" id="timeInput">
</div>

<!-- DateTime-local input -->
<div class="mb-3">
  <label for="datetimeInput" class="form-label">Date and Time</label>
  <input type="datetime-local" class="form-control" id="datetimeInput">
</div>

<!-- With default value -->
<div class="mb-3">
  <label for="dateDefault" class="form-label">Appointment Date</label>
  <input type="date" class="form-control" id="dateDefault" value="2026-04-02">
</div>
```

## Advanced Variations

```html
<!-- Date range with two inputs -->
<div class="row g-3">
  <div class="col-md-6">
    <label for="startDate" class="form-label">Start Date</label>
    <input type="date" class="form-control" id="startDate">
  </div>
  <div class="col-md-6">
    <label for="endDate" class="form-label">End Date</label>
    <input type="date" class="form-control" id="endDate">
  </div>
</div>

<!-- Date input with min and max constraints -->
<div class="mb-3">
  <label for="dateConstrained" class="form-label">Select a date (next 30 days)</label>
  <input type="date" class="form-control" id="dateConstrained"
         min="2026-04-02" max="2026-05-02">
  <div class="form-text">Dates limited to the next 30 days.</div>
</div>
```

```html
<!-- Date in input group with icon -->
<div class="mb-3">
  <label for="dateIcon" class="form-label">Birthday</label>
  <div class="input-group">
    <span class="input-group-text"><i class="bi bi-calendar"></i></span>
    <input type="date" class="form-control" id="dateIcon">
  </div>
</div>

<!-- DateTime in a card form -->
<div class="card p-3">
  <h6>Schedule Event</h6>
  <div class="mb-3">
    <label for="eventDate" class="form-label">Event Date</label>
    <input type="date" class="form-control" id="eventDate" required>
  </div>
  <div class="row g-3">
    <div class="col-md-6">
      <label for="startTime" class="form-label">Start Time</label>
      <input type="time" class="form-control" id="startTime" required>
    </div>
    <div class="col-md-6">
      <label for="endTime" class="form-label">End Time</label>
      <input type="time" class="form-control" id="endTime" required>
    </div>
  </div>
  <button class="btn btn-primary mt-3">Schedule</button>
</div>
```

## Best Practices

1. Always associate a `<label>` with date and time inputs.
2. Use `min` and `max` attributes to constrain selectable date ranges.
3. Set default `value` attributes to reasonable starting dates.
4. Use `required` attribute when date selection is mandatory.
5. Provide `form-text` hints explaining the expected date format or constraints.
6. Use `datetime-local` when both date and time are needed in a single input.
7. Validate date ranges (start before end) with JavaScript when using paired inputs.
8. Group related date and time inputs in a `row` for horizontal layout.
9. Use input groups with calendar icons for visual context.
10. Test date inputs across browsers since rendering varies significantly.

## Common Pitfalls

1. **No min/max constraints.** Users can select past dates when only future dates are valid.
2. **Missing labels.** Date inputs without labels are inaccessible and unclear.
3. **Browser rendering differences.** Chrome, Firefox, and Safari render date pickers differently; test thoroughly.
4. **No validation feedback.** Invalid date ranges (end before start) need JavaScript validation and user feedback.
5. **Using text inputs for dates.** This loses the native date picker and validation.
6. **Timezone issues.** `datetime-local` is timezone-naive; convert to UTC on the server.

## Accessibility Considerations

Date inputs are accessible by default as native form controls. Ensure each input has a visible `<label>` with a matching `for` attribute. Use `aria-describedby` to link constraints or help text. When date ranges are invalid, use Bootstrap's `invalid-feedback` class to show error messages. Provide `aria-live` announcements when validation fails dynamically. Avoid replacing native date inputs with custom widgets that lack keyboard support.

## Responsive Behavior

Date and time inputs fill their container width by default. Use Bootstrap's grid system to place multiple date inputs side by side on desktop (`col-md-6`) and stacked on mobile (`col-12`). Input groups with icons maintain alignment at all breakpoints. Ensure `form-text` hints remain visible and readable on small screens. Native date pickers adapt to mobile with full-screen picker UIs.
