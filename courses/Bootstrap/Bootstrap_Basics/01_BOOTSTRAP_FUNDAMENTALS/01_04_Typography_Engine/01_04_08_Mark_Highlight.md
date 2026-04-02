---
title: "Mark Highlight"
topic: "Typography Engine"
subtopic: "Mark Highlight"
difficulty: 1
duration: "15 minutes"
prerequisites: ["Paragraph Styles", "Text Alignment Wrapping"]
learning_objectives:
  - Use the mark element for inline text highlighting
  - Apply custom highlight colors with utility classes
  - Understand semantic use cases for highlighted text
---

## Overview

The HTML `<mark>` element highlights text with a yellow background by default, drawing attention to search results, key terms, or relevant passages. Bootstrap styles `<mark>` with a yellow background (`#fff3cd`) and dark text. You can customize the highlight color using Bootstrap's background color utilities or custom CSS to match your design system.

## Basic Implementation

Default mark element styling:

```html
<p>
  This paragraph contains <mark>highlighted text</mark> using the default
  Bootstrap mark styling with a yellow background.
</p>
```

Mark within a paragraph for search result highlighting:

```html
<div class="card">
  <div class="card-body">
    <p class="card-text">
      Search results for "<mark>Bootstrap</mark>": The <mark>Bootstrap</mark>
      framework provides responsive utilities for building modern web
      applications. <mark>Bootstrap</mark> 5 includes a complete grid system.
    </p>
  </div>
</div>
```

Mark with other inline text elements:

```html
<p>
  Use <mark><strong>bold highlight</strong></mark> for emphasis, or
  <mark><em>italic highlight</em></mark> for subtle attention.
  Even <mark><a href="#" class="text-decoration-none">linked highlights</a></mark>
  work well.
</p>
```

## Advanced Variations

Custom highlight colors using Bootstrap background utilities:

```html
<p>
  Default: <mark>yellow highlight</mark><br>
  Primary: <mark class="bg-primary text-white">blue highlight</mark><br>
  Success: <mark class="bg-success text-white">green highlight</mark><br>
  Danger: <mark class="bg-danger text-white">red highlight</mark><br>
  Info: <mark class="bg-info text-white">cyan highlight</mark>
</p>
```

Using mark in a definition list for terminology:

```html
<dl class="row">
  <dt class="col-sm-3"><mark>Grid System</mark></dt>
  <dd class="col-sm-9">A layout structure using rows and columns.</dd>
  <dt class="col-sm-3"><mark>Breakpoint</mark></dt>
  <dd class="col-sm-9">A screen width threshold for responsive design.</dd>
  <dt class="col-sm-3"><mark>Container</mark></dt>
  <dd class="col-sm-9">A wrapper element that centers and pads content.</dd>
</dl>
```

Mark with opacity for subtle highlighting:

```html
<p>
  Subtle highlight: <mark class="bg-warning bg-opacity-25">low opacity</mark>
  Medium highlight: <mark class="bg-warning bg-opacity-50">medium opacity</mark>
  Full highlight: <mark class="bg-warning">full opacity</mark>
</p>
```

## Best Practices

1. Use `<mark>` for text that matches a search query or needs attention within a passage.
2. Keep highlights short — highlight individual words or short phrases, not entire paragraphs.
3. Use `bg-*` utility classes to customize highlight colors to match your brand palette.
4. Apply `text-white` with dark background highlights to maintain contrast ratio.
5. Combine `<mark>` with `<strong>` or `<em>` for layered emphasis.
6. Use `<mark>` sparingly in body text — over-highlighting reduces its effectiveness.
7. Prefer `<mark>` over custom `<span>` with background — `<mark>` has semantic meaning.
8. Test highlight colors against WCAG contrast requirements (4.5:1 for normal text).
9. Use `bg-opacity-*` for subtle highlighting that doesn't overpower the text.
10. Apply highlights consistently — if you highlight search terms, highlight all occurrences.

## Common Pitfalls

- **Over-highlighting**: Marking too much text makes nothing stand out, defeating the purpose.
- **Poor contrast**: Yellow highlight on white text is invisible. Use dark text on light highlights or `text-white` on dark highlights.
- **Using `<mark>` for decoration**: `<mark>` has semantic meaning for highlighted/relevant text — use `<span>` for purely decorative backgrounds.
- **Nesting block elements**: `<mark>` is an inline element — nesting `<div>` or `<p>` inside it is invalid HTML.
- **Ignoring dark mode**: Default yellow `<mark>` may look harsh in dark themes — override with CSS variables.
- **Custom styling conflicts**: Setting `background: none` on `<mark>` removes the semantic visual indicator.

## Accessibility Considerations

- `<mark>` conveys "highlighted/relevant" semantics to screen readers — use it when text is genuinely marked for attention.
- Ensure highlighted text maintains a 4.5:1 contrast ratio with its background.
- Don't rely solely on color to convey meaning — the `<mark>` element provides semantic meaning beyond visual styling.
- Use `aria-label` or surrounding context to explain why text is highlighted for screen reader users.
- Avoid highlighting critical information that all users must notice — use headings or alerts instead.
- Test highlighted text with screen readers to verify it's announced appropriately.

## Responsive Behavior

Mark elements are inline and adapt naturally to any container width. No responsive classes are needed — highlighted text wraps within its parent paragraph at all viewport sizes.

```html
<div class="container">
  <div class="row">
    <div class="col-12 col-md-6">
      <p>
        This column contains <mark>highlighted terms</mark> that wrap
        naturally on narrow screens without any special responsive handling.
      </p>
    </div>
    <div class="col-12 col-md-6">
      <p>
        The <mark class="bg-primary text-white">styled highlight</mark>
        also responds to column width changes automatically.
      </p>
    </div>
  </div>
</div>
```

For responsive font size changes that affect highlighted text, combine `<mark>` with Bootstrap's responsive font size classes or CSS `clamp()`.
