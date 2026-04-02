---
title: "Text Truncation"
topic: "Typography Engine"
subtopic: "Text Truncation"
difficulty: 1
duration: "15 minutes"
prerequisites: ["Text Alignment Wrapping", "Paragraph Styles"]
learning_objectives:
  - Apply text-truncate for single-line ellipsis truncation
  - Implement multi-line truncation with webkit line-clamp
  - Use custom truncation patterns for various content types
---

## Overview

Bootstrap's `text-truncate` class truncates overflowing text with an ellipsis (`...`) on a single line. For multi-line truncation, the `-webkit-line-clamp` CSS property (supported in all modern browsers) limits text to a specified number of lines before adding an ellipsis. These utilities are essential for card titles, table cells, navigation items, and any container where space is limited.

## Basic Implementation

Single-line truncation with `text-truncate`:

```html
<div class="text-truncate bg-light p-2" style="max-width: 200px;">
  This is a very long text that will be truncated with an ellipsis when it exceeds the container width.
</div>
```

Truncation inside a flex container:

```html
<div class="d-flex align-items-center bg-light p-3">
  <div class="flex-shrink-0 me-3">
    <div class="bg-primary rounded" style="width: 40px; height: 40px;"></div>
  </div>
  <div class="text-truncate">
    <strong class="text-truncate d-block">Very Long Username That Overflows</strong>
    <span class="text-muted text-truncate d-block">user-with-a-very-long-email@example.com</span>
  </div>
</div>
```

Truncation in a card title:

```html
<div class="card" style="width: 250px;">
  <div class="card-body">
    <h5 class="card-title text-truncate">
      This Is an Extremely Long Card Title That Will Be Truncated
    </h5>
    <p class="card-text">Card description text here.</p>
  </div>
</div>
```

## Advanced Variations

Multi-line truncation using `-webkit-line-clamp`:

```html
<style>
  .text-truncate-2 {
    display: -webkit-box;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
    overflow: hidden;
  }
  .text-truncate-3 {
    display: -webkit-box;
    -webkit-line-clamp: 3;
    -webkit-box-orient: vertical;
    overflow: hidden;
  }
</style>
<div class="card" style="width: 300px;">
  <div class="card-body">
    <p class="text-truncate-2 mb-0">
      This paragraph will be truncated after exactly two lines. Any text
      beyond the second line will be hidden with an ellipsis indicator,
      making it perfect for card descriptions and preview text.
    </p>
  </div>
</div>
```

Truncation in a table cell:

```html
<table class="table table-bordered">
  <thead>
    <tr>
      <th style="width: 150px;">Name</th>
      <th>Description</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td class="text-truncate">Item Alpha</td>
      <td class="text-truncate">
        This is a very long description that exceeds the table cell width
        and will be truncated with an ellipsis to keep the table readable.
      </td>
    </tr>
    <tr>
      <td class="text-truncate">Item Beta</td>
      <td class="text-truncate">
        Another lengthy description that demonstrates truncation behavior
        in Bootstrap table cells.
      </td>
    </tr>
  </tbody>
</table>
```

Custom truncation with "Read more" link:

```html
<div class="card" style="max-width: 350px;">
  <div class="card-body">
    <p class="text-truncate-3 mb-2">
      Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do
      eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim
      ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut
      aliquip ex ea commodo consequat.
    </p>
    <a href="#" class="small">Read more</a>
  </div>
</div>
```

## Best Practices

1. Use `text-truncate` for single-line overflow in tight UI spaces like table cells and card titles.
2. Apply `d-block` or `display: block` along with `text-truncate` on inline elements to enable truncation.
3. Set explicit `width` or `max-width` on the container — `text-truncate` requires a constrained width.
4. Use `-webkit-line-clamp` for multi-line truncation in card descriptions and preview text.
5. Combine `text-truncate` with flex containers to prevent text from pushing sibling elements.
6. Provide a tooltip or expand mechanism for truncated content so users can access the full text.
7. Use `overflow-hidden` as a fallback for browsers that don't support `-webkit-line-clamp`.
8. Apply truncation consistently across similar UI elements (all card titles, all table cells).
9. Test truncation at various container widths to ensure the ellipsis appears correctly.
10. Consider providing a hover state or click-to-expand for truncated content.

## Common Pitfalls

- **Missing container width**: `text-truncate` without a width constraint has no effect — the text has no overflow to truncate.
- **Forgetting `d-block`**: `text-truncate` on inline elements like `<span>` doesn't work without `display: block`.
- **Using truncation for paragraphs**: Long-form content should wrap naturally — truncation is for UI labels and titles.
- **Browser support assumptions**: `-webkit-line-clamp` works in all modern browsers but requires the webkit prefix.
- **Truncated accessible names**: Screen readers read the full text even if visually truncated — this is correct behavior but may surprise users.
- **No way to access full text**: Truncating critical content without an expand option frustrates users.
- **Responsive width issues**: Fixed `max-width` truncation may not adapt to responsive layouts.

## Accessibility Considerations

- Screen readers announce the full text even when visually truncated — this is correct and expected.
- Provide a way to access the full text (tooltip, expand button, or link to detail page).
- Use `aria-label` with the full text on interactive elements if the visible text is truncated.
- Ensure truncated text still conveys enough meaning for users who cannot expand it.
- Don't truncate error messages or validation feedback — users must see complete error information.
- Test with screen magnification — truncation may hide content that magnification users need.

## Responsive Behavior

Combine truncation with responsive containers for adaptive text display:

```html
<div class="container">
  <div class="row">
    <div class="col-12 col-md-6 col-lg-4">
      <div class="card">
        <div class="card-body">
          <h5 class="card-title text-truncate">Responsive Card Title</h5>
          <p class="card-text text-truncate-2">
            Card description that truncates to two lines. On smaller screens,
            the narrower container causes earlier truncation while on wider
            screens more text is visible.
          </p>
        </div>
      </div>
    </div>
    <div class="col-12 col-md-6 col-lg-4">
      <div class="card">
        <div class="card-body">
          <h5 class="card-title text-truncate">Another Card Title</h5>
          <p class="card-text text-truncate-2">
            Second card with the same truncation pattern applied consistently
            across the grid for visual uniformity.
          </p>
        </div>
      </div>
    </div>
  </div>
</div>
```

As the column width changes at each breakpoint, `text-truncate` and `-webkit-line-clamp` adapt automatically — more text fits on wider screens, and more is truncated on narrow screens.
