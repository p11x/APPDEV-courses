---
title: "Text Alignment and Wrapping"
subtitle: "Controlling horizontal alignment, responsive text positioning, and overflow wrapping in Bootstrap 5"
category: "Bootstrap Basics"
subcategory: "Typography Engine"
difficulty: 1
duration: "20 minutes"
prerequisites: ["01_04_01_Heading_Typography", "01_04_02_Paragraph_Styles"]
learning_objectives:
  - "Apply text-start, text-center, and text-end alignment utilities"
  - "Use responsive text alignment classes for breakpoint-specific positioning"
  - "Control text wrapping with text-wrap, text-nowrap, and text-break utilities"
  - "Combine alignment and wrapping utilities for robust layout control"
  - "Understand how text alignment interacts with flexbox and grid systems"
keywords:
  - "text alignment bootstrap"
  - "text-start text-center text-end"
  - "responsive text alignment"
  - "text-wrap text-nowrap"
  - "text-break"
  - "bootstrap typography utilities"
---

# Text Alignment and Wrapping

## Overview

Text alignment and wrapping are foundational controls for typographic layout. They determine where text sits horizontally within its container and how text behaves when it exceeds the available width. Bootstrap 5 provides a complete set of utility classes for both concerns, including responsive variants that allow alignment and wrapping rules to change at specific breakpoints.

The three primary text alignment utilities — `text-start`, `text-center`, and `text-end` — map directly to the CSS `text-align` property values `left` (or `right` in RTL), `center`, and `right` (or `left` in RTL). These utilities are direction-aware: in left-to-right layouts, `text-start` aligns to the left and `text-end` aligns to the right. In right-to-left layouts, the behavior reverses automatically when using Bootstrap's RTL support.

Responsive text alignment extends this system by appending breakpoint infixes to the class names. The pattern `text-{breakpoint}-{alignment}` allows you to specify alignment at a particular breakpoint and above. For example, `text-md-center` centers text on medium screens and larger, while `text-start` (without a breakpoint) applies to all screen sizes below the medium breakpoint.

Wrapping utilities address text overflow. The `text-wrap` class (the default behavior) allows text to wrap to the next line when it reaches the container edge. The `text-nowrap` class prevents wrapping, causing text to extend beyond its container unless constrained by overflow handling. The `text-break` class forces long words to break at arbitrary points, preventing overflow from unbroken strings like URLs or generated codes.

Understanding the interaction between text alignment and Bootstrap's grid and flexbox systems is important. Text alignment affects inline content within a block element. It does not affect block-level layout. For centering block elements themselves, you need flex utilities like `d-flex justify-content-center` or margin utilities like `mx-auto`.

## Basic Implementation

### Text Alignment Utilities

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
  <title>Text Alignment and Wrapping</title>
</head>
<body>
  <div class="container py-5">
    <p class="text-start">Left-aligned text (text-start). This is the default alignment for LTR content.</p>
    <p class="text-center">Center-aligned text (text-center). Useful for headings, captions, and hero sections.</p>
    <p class="text-end">Right-aligned text (text-end). Common for numerical data, timestamps, and footer elements.</p>
  </div>
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

### Responsive Text Alignment

Responsive alignment classes follow the pattern `text-{breakpoint}-{alignment}`. The alignment applies at the specified breakpoint and all larger breakpoints:

```html
<div class="container py-4">
  <p class="text-start text-md-center text-lg-end">
    Starts left on small screens, centers on medium screens (768px+),
    and aligns right on large screens (992px+).
  </p>

  <p class="text-center text-sm-start">
    Centers on extra-small screens, left-aligns on small screens (576px+) and above.
  </p>

  <p class="text-end text-xl-center">
    Right-aligned by default, centers on extra-large screens (1200px+) and above.
  </p>
</div>
```

### Text Wrapping

```html
<div class="container py-4">
  <div class="row">
    <div class="col-4">
      <div class="border p-3 mb-3">
        <p class="text-wrap mb-0">
          Text wrapping is the default behavior. Long words and normal text wrap
          at the edge of the container naturally.
        </p>
      </div>
    </div>

    <div class="col-4">
      <div class="border p-3 mb-3 overflow-auto">
        <p class="text-nowrap mb-0">
          Text nowrap prevents wrapping. This text will extend beyond its container.
        </p>
      </div>
    </div>

    <div class="col-4">
      <div class="border p-3 mb-3">
        <p class="text-break mb-0">
          Text break forces long words to break: Pneumonoultramicroscopicsilicovolcanoconiosis
        </p>
      </div>
    </div>
  </div>
</div>
```

### Alignment on Table Cells

Text alignment utilities work on table cells for data presentation:

```html
<div class="container py-4">
  <table class="table">
    <thead>
      <tr>
        <th class="text-start">Product</th>
        <th class="text-center">Quantity</th>
        <th class="text-end">Price</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td class="text-start">Widget A</td>
        <td class="text-center">150</td>
        <td class="text-end">$1,250.00</td>
      </tr>
      <tr>
        <td class="text-start">Widget B</td>
        <td class="text-center">75</td>
        <td class="text-end">$890.50</td>
      </tr>
    </tbody>
  </table>
</div>
```

## Advanced Variations

### Alignment Within Grid Columns

Combining text alignment with Bootstrap's grid system for responsive layouts:

```html
<div class="container py-4">
  <div class="row">
    <div class="col-12 col-md-6 text-md-start text-center">
      <h4 class="fw-bold">Feature One</h4>
      <p>Centered on mobile, left-aligned on tablet and desktop.</p>
    </div>
    <div class="col-12 col-md-6 text-md-end text-center">
      <h4 class="fw-bold">Feature Two</h4>
      <p>Centered on mobile, right-aligned on tablet and desktop.</p>
    </div>
  </div>
</div>
```

### Alignment in Flex Containers

Text alignment applies to inline content. For centering block elements, combine with flexbox utilities:

```html
<div class="container py-4">
  <!-- Text alignment centers inline text -->
  <div class="border p-4 text-center">
    This text is centered via text-align.
  </div>

  <!-- Flexbox centers the block element itself -->
  <div class="d-flex justify-content-center border p-4 mt-3">
    <div class="bg-primary text-white p-3 rounded">
      This block is centered via flexbox.
    </div>
  </div>

  <!-- Combining both: centered block with centered text -->
  <div class="d-flex justify-content-center border p-4 mt-3">
    <div class="bg-dark text-white p-3 rounded text-center">
      Centered block with centered text inside.
    </div>
  </div>
</div>
```

### Text Alignment in Cards

```html
<div class="row">
  <div class="col-md-4">
    <div class="card text-center">
      <div class="card-body">
        <h5 class="card-title">Centered Card</h5>
        <p class="card-text">All content centered.</p>
        <a href="#" class="btn btn-primary">Action</a>
      </div>
    </div>
  </div>
  <div class="col-md-4">
    <div class="card text-start">
      <div class="card-body">
        <h5 class="card-title">Left-Aligned Card</h5>
        <p class="card-text">Standard left alignment.</p>
        <a href="#" class="btn btn-primary">Action</a>
      </div>
    </div>
  </div>
  <div class="col-md-4">
    <div class="card text-end">
      <div class="card-body">
        <h5 class="card-title">Right-Aligned Card</h5>
        <p class="card-text">Right-aligned content.</p>
        <a href="#" class="btn btn-primary">Action</a>
      </div>
    </div>
  </div>
</div>
```

### Wrapping in Navigation and Badges

Preventing text wrapping in navigation links and badges ensures consistent layout:

```html
<nav class="nav border-bottom mb-4">
  <a class="nav-link text-nowrap px-3" href="#">Dashboard Overview</a>
  <a class="nav-link text-nowrap px-3" href="#">User Management</a>
  <a class="nav-link text-nowrap px-3" href="#">System Configuration</a>
</nav>

<div class="d-flex gap-2 flex-wrap">
  <span class="badge bg-primary text-nowrap">JavaScript</span>
  <span class="badge bg-success text-nowrap">TypeScript</span>
  <span class="badge bg-danger text-nowrap">React Framework</span>
  <span class="badge bg-warning text-dark text-nowrap">Node.js Runtime</span>
</div>
```

### Combining Break and Alignment

```html
<div class="container py-4">
  <div class="col-6 mx-auto">
    <div class="border p-3 text-center text-break">
      https://api.example.com/v2/resources/users/12345/profile/settings/preferences
    </div>
  </div>
</div>
```

## Best Practices

1. **Use `text-start` as the default for body content in LTR languages.** Left-aligned text creates a consistent reading edge that helps users track line beginnings. Avoid centering long paragraphs.

2. **Use `text-center` for short content: headings, captions, call-to-action text, and hero sections.** Centered body text is harder to read because the ragged left edge makes it difficult to find the start of each line.

3. **Use `text-end` for numerical data, timestamps, and metadata.** Right-aligning numbers makes them easier to compare vertically, as the decimal point and significant digits line up.

4. **Apply responsive alignment with a mobile-first approach.** Set the base alignment without a breakpoint prefix, then override at larger breakpoints. For example, `text-center text-md-start` centers on mobile and left-aligns on tablets and desktops.

5. **Use `text-nowrap` for navigation links, badges, and labels that should not break across lines.** This prevents awkward line wraps in UI elements that are meant to remain compact.

6. **Apply `text-break` to containers displaying user-generated content, URLs, or long codes.** This prevents overflow from unbroken strings that would otherwise force horizontal scrolling.

7. **Do not use `text-nowrap` on long paragraphs or content blocks.** Preventing wrap on multi-line content causes horizontal overflow and poor reading experiences. Reserve `text-nowrap` for short, single-line elements.

8. **Combine `text-break` with `overflow-hidden` if you want the broken text to stay within a fixed-height container.** Without `overflow-hidden`, the broken text will expand the container vertically.

9. **Use `text-center` on a parent element to center all child inline content.** Avoid applying `text-center` to every child element. Apply it once to the parent.

10. **Test alignment across all breakpoints.** A layout that looks balanced at one breakpoint may feel misaligned at another. Verify that responsive alignment classes produce the intended result at each viewport width.

11. **Avoid mixing `text-align` with `justify-content` for the same content.** These serve different purposes: `text-align` controls inline content within a block, while `justify-content` controls block-level items within a flex container. Using both on the same element creates confusion.

12. **Use `text-wrap` to explicitly reset `text-nowrap` at specific breakpoints when inheriting nowrap from a parent.** This provides precise control over wrapping behavior at each viewport size.

## Common Pitfalls

### Centering Body Text

Centering paragraphs of body text is one of the most common typographic mistakes. Centered text creates an uneven left margin, making it difficult for users to find the beginning of each line:

```html
<!-- WRONG: Centered body text is hard to read -->
<div class="text-center">
  <p>Climate change is accelerating at an unprecedented rate. Scientists have
  documented rising sea levels, increasing global temperatures, and more
  frequent extreme weather events across every continent.</p>
  <p>Policy responses must be swift and comprehensive. Governments need to
  coordinate international efforts to reduce carbon emissions while investing
  in renewable energy infrastructure.</p>
</div>

<!-- RIGHT: Center the heading, left-align body -->
<div class="text-center">
  <h2>Climate Report 2025</h2>
</div>
<p>Climate change is accelerating at an unprecedented rate. Scientists have
documented rising sea levels, increasing global temperatures, and more
frequent extreme weather events across every continent.</p>
```

### Using Text Alignment for Block Element Centering

`text-align: center` centers inline content within a block, not the block itself. To center a block element, you need margin or flex utilities:

```html
<!-- WRONG: text-center does not center the div -->
<div class="text-center">
  <div class="card" style="width: 300px;">
    <div class="card-body">This card is NOT centered.</div>
  </div>
</div>

<!-- RIGHT: mx-auto centers the block element -->
<div class="d-flex justify-content-center">
  <div class="card" style="width: 300px;">
    <div class="card-body">This card IS centered.</div>
  </div>
</div>

<!-- OR: mx-auto with d-block -->
<div class="text-center">
  <div class="card mx-auto" style="width: 300px;">
    <div class="card-body">This card IS centered via mx-auto.</div>
  </div>
</div>
```

### Applying `text-nowrap` to Multi-Line Content

The `text-nowrap` utility prevents text from wrapping, which is appropriate for labels and navigation links but causes horizontal overflow on multi-line content:

```html
<!-- WRONG: nowrap on a paragraph forces horizontal scroll -->
<p class="text-nowrap">
  This long paragraph will never wrap to the next line, forcing the user
  to scroll horizontally to read the entire content, which is a poor UX.
</p>

<!-- RIGHT: nowrap only on short elements -->
<span class="text-nowrap badge bg-primary">Short Label</span>
```

### Not Providing Overflow Handling with `text-nowrap`

When `text-nowrap` is applied, the text can extend beyond its container. Without overflow handling, this can break the page layout:

```html
<!-- WRONG: nowrap without overflow control -->
<div class="col-3">
  <p class="text-nowrap">This text overflows and may overlap adjacent content.</p>
</div>

<!-- RIGHT: nowrap with overflow-auto for scrollable overflow -->
<div class="col-3 overflow-auto">
  <p class="text-nowrap">This text overflows within a scrollable container.</p>
</div>

<!-- OR: nowrap with text-truncate for ellipsis -->
<div class="col-3">
  <p class="text-nowrap text-truncate">This text is truncated with an ellipsis.</p>
</div>
```

### Responsive Alignment Order

The mobile-first approach means that classes without breakpoint prefixes apply to all sizes, and breakpoint-specific classes only take effect at that breakpoint and above. Placing responsive classes in the wrong order can produce unexpected results:

```html
<!-- WRONG: Understanding of breakpoint cascading -->
<p class="text-md-center text-lg-end">
  At medium breakpoint, text is centered. At large, it is right-aligned.
  But at extra-large (1200px+), it REVERTS to center because text-md-center
  still applies... NO — text-lg-end overrides text-md-center for lg and above.
</p>

<!-- Clarification: Later breakpoints override earlier ones -->
<!-- text-md-center applies from 768px to 991px -->
<!-- text-lg-end applies from 992px and above -->
<p class="text-start text-md-center text-lg-end">
  XS: left | SM: left | MD: center | LG: right | XL: right | XXL: right
</p>
```

## Accessibility Considerations

Text alignment affects readability for users with cognitive disabilities and reading disorders. Left-aligned (in LTR) body text is recommended by accessibility guidelines because it creates a consistent left margin, making it easier for users to track line transitions. Centered and right-aligned body text should be avoided for long content.

For RTL (right-to-left) languages, the alignment recommendations reverse. Bootstrap's `text-start` and `text-end` classes automatically adjust for RTL layouts when the `dir="rtl"` attribute is set on the HTML element or a parent container.

```html
<html lang="ar" dir="rtl">
<body>
  <div class="container">
    <p class="text-start">Right-aligned in RTL (start of reading direction)</p>
    <p class="text-end">Left-aligned in RTL (end of reading direction)</p>
  </div>
</body>
</html>
```

The `text-break` utility is important for accessibility because it prevents content from being hidden by horizontal overflow. Users who zoom their browser to 200% or more (a WCAG requirement for reflow) may encounter horizontal scrolling if long unbroken strings are not broken. `text-break` ensures content remains within the viewport when zoomed.

The `text-nowrap` utility should be used cautiously in accessible interfaces. Screen magnification users may not be able to see content that extends beyond their magnified viewport. If `text-nowrap` is necessary, ensure the container has horizontal scrolling support or that the content is also available in an alternative non-truncated form.

```html
<!-- Accessible nowrap with scroll fallback -->
<div class="overflow-auto" tabindex="0" role="region" aria-label="Scrollable table">
  <table class="table text-nowrap">
    <!-- table content -->
  </table>
</div>
```

## Responsive Behavior

Text alignment is the most commonly responsive typography utility. Landing pages, dashboards, and content-heavy sites frequently need different alignment at different viewport widths.

### Common Responsive Alignment Patterns

```html
<!-- Pattern 1: Center on mobile, left-align on desktop -->
<h1 class="text-center text-md-start">Centered on mobile, left-aligned on desktop</h1>

<!-- Pattern 2: Center everything on mobile, distribute on desktop -->
<div class="row text-center text-lg-start">
  <div class="col-12 col-lg-4">Feature 1</div>
  <div class="col-12 col-lg-4">Feature 2</div>
  <div class="col-12 col-lg-4">Feature 3</div>
</div>

<!-- Pattern 3: Responsive table alignment -->
<td class="text-start text-md-end">Left on mobile, right on desktop</td>
```

### Responsive Wrapping

Wrapping behavior can also be responsive. On mobile, you might want to allow natural wrapping, while on desktop, prevent wrapping for certain elements:

```html
<nav class="nav flex-column flex-md-row">
  <a class="nav-link text-md-nowrap" href="#">Short Link</a>
  <a class="nav-link text-md-nowrap" href="#">A Much Longer Navigation Link</a>
  <a class="nav-link text-md-nowrap" href="#">Another Link</a>
</nav>
```

### Alignment with Container Width

Responsive alignment is most effective when combined with responsive container widths. A `text-center` heading on a full-width mobile container looks balanced, while the same heading in a narrow `col-4` container on desktop might feel cramped:

```html
<div class="row justify-content-center">
  <div class="col-12 col-md-10 col-lg-8">
    <h2 class="text-center text-lg-start">Responsive heading in a responsive container</h2>
    <p class="text-center text-lg-start">
      Both the container width and text alignment respond to the viewport,
      maintaining optimal reading conditions at every breakpoint.
    </p>
  </div>
</div>
```

### Wrapping in Responsive Tables

Tables are a common source of horizontal overflow on mobile. Combining responsive utilities with wrapping controls:

```html
<div class="table-responsive">
  <table class="table text-nowrap text-sm-wrap">
    <thead>
      <tr>
        <th>Column A</th>
        <th>Column B</th>
        <th>Column C</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td>Data</td>
        <td>Data</td>
        <td>Data</td>
      </tr>
    </tbody>
  </table>
</div>
```

Note that `text-sm-wrap` is not a built-in Bootstrap utility. To achieve this, define a custom class:

```css
@media (min-width: 576px) {
  .text-sm-wrap {
    white-space: normal;
  }
}
```

This pattern prevents wrapping on mobile (keeping table rows compact) and allows wrapping on larger screens where there is more horizontal space.
