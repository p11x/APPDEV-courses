---
title: Vertical Alignment Utilities
category: Bootstrap Fundamentals
difficulty: 1
time: 15 min
tags: bootstrap5, vertical-align, alignment, inline, table, utilities
---

## Overview

Bootstrap 5 vertical alignment utilities control the vertical positioning of inline, inline-block, and table-cell elements relative to their parent or surrounding content. These classes map directly to the CSS `vertical-align` property and are primarily used for aligning text with inline images, table cell content, and inline-block elements. The available options include `align-baseline`, `align-top`, `align-middle`, `align-bottom`, `align-text-top`, and `align-text-bottom`.

## Basic Implementation

Vertical alignment utilities apply to inline, inline-block, and table-cell elements.

```html
<!-- Inline element alignment with text -->
<span class="align-baseline">baseline</span>
<span class="align-top">top</span>
<span class="align-middle">middle</span>
<span class="align-bottom">bottom</span>
<span class="align-text-top">text-top</span>
<span class="align-text-bottom">text-bottom</span>
```

Aligning icons or images with adjacent text is a common use case.

```html
<!-- Aligning icons with text -->
<p>
  <i class="bi bi-star align-middle"></i> Middle aligned icon
</p>
<p>
  <i class="bi bi-star align-top"></i> Top aligned icon
</p>
<p>
  <i class="bi bi-star align-bottom"></i> Bottom aligned icon
</p>

<!-- Inline image alignment -->
<p>
  Text with an
  <img src="icon.png" class="align-middle" alt="icon" style="width: 24px;">
  inline image
</p>
```

Table cell alignment is essential for data presentation.

```html
<!-- Table cell vertical alignment -->
<table class="table">
  <thead>
    <tr>
      <th>Product</th>
      <th>Description</th>
      <th>Price</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td class="align-middle">Widget</td>
      <td class="align-top">A small component with multiple features and benefits for users</td>
      <td class="align-bottom">$9.99</td>
    </tr>
    <tr>
      <td class="align-middle">Gadget</td>
      <td class="align-middle">Standard device</td>
      <td class="align-middle">$19.99</td>
    </tr>
  </tbody>
</table>
```

## Advanced Variations

Combining vertical alignment with flex containers requires understanding when these utilities are effective versus using flex alignment.

```html
<!-- Vertical alignment in inline-block layouts -->
<div style="height: 100px; background: #f8f9fa;">
  <span class="d-inline-block align-middle" style="height: 100%;">
  </span>
  <span class="d-inline-block align-middle">
    Vertically centered content next to a full-height inline-block
  </span>
</div>

<!-- Badge alignment with buttons -->
<button class="btn btn-primary">
  Notifications
  <span class="badge bg-danger align-middle">5</span>
</button>
```

For complex layouts, combining vertical alignment with display utilities provides precise control.

```html
<!-- Aligning form elements -->
<div class="d-inline-block align-middle">
  <label class="form-label mb-0 align-middle">Search:</label>
  <input type="text" class="form-control d-inline-block align-middle" style="width: auto;">
  <button class="btn btn-outline-primary align-middle">Go</button>
</div>
```

## Best Practices

1. **Use `align-middle` for inline icons** - Align icons with adjacent text using `align-middle` for consistent vertical centering.
2. **Apply to table cells for data alignment** - Use `align-middle`, `align-top`, or `align-bottom` on `<td>` elements for consistent table presentation.
3. **Combine with `d-inline-block`** - Vertical alignment requires the element to be inline-level. Use `d-inline-block` on block elements.
4. **Prefer flex for complex centering** - For centering block-level content, use flexbox (`d-flex align-items-center`) instead of vertical-align.
5. **Use `align-baseline` as the default** - Baseline alignment works well for text and inline elements in most scenarios.
6. **Align form inputs with labels** - Use `align-middle` on labels and inputs to keep them horizontally aligned.
7. **Consider line-height effects** - Vertical alignment is relative to the line-height of the parent. Adjust line-height if alignment appears off.
8. **Use `align-text-top` for superscript effects** - Position elements at the top of the text line without changing font size.
9. **Test across browsers** - Vertical alignment rendering can vary slightly between browsers. Verify on target platforms.
10. **Apply consistently across similar elements** - Maintain the same alignment approach for all icons, badges, or inline elements in a section.

## Common Pitfalls

1. **Using vertical-align on block elements** - Vertical alignment has no effect on block-level elements. Flexbox or grid alignment should be used instead.
2. **Expecting vertical-align to center in a div** - `vertical-align: middle` does not center an element within a parent div. It aligns inline elements within a line.
3. **Applying to flex children** - Vertical-align is ignored on flex items. Use `align-self` for individual flex child alignment.
4. **Browser inconsistencies** - Some browsers render vertical-align slightly differently, especially for `text-top` and `text-bottom`.
5. **Not accounting for font metrics** - Different fonts have different baseline and cap height measurements, which affects alignment results.

## Accessibility Considerations

Vertical alignment is a visual presentation concern and does not affect screen reader interpretation. However, ensure that visual alignment does not create misleading reading order. When aligning icons with text, verify that the icon's position conveys the correct meaning. Misaligned form labels can confuse users relying on screen magnification. Maintain logical visual relationships between labels and their associated controls regardless of alignment choices.

## Responsive Behavior

Bootstrap 5 does not include responsive variants for vertical alignment utilities. The alignment class applies at all screen sizes. For responsive alignment behavior, use custom CSS with media queries or switch to flexbox alignment at specific breakpoints. In responsive tables, vertical alignment becomes particularly important as cell content wraps differently on narrow screens. Consider using `align-top` for table cells on mobile to prevent uneven row heights caused by varying content lengths.
