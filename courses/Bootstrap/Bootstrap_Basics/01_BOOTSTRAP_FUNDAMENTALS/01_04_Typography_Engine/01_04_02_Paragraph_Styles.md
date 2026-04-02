---
title: "Paragraph Styles in Bootstrap 5"
subtitle: "Lead paragraphs, inline text elements, and text truncation techniques"
category: "Bootstrap Basics"
subcategory: "Typography Engine"
difficulty: 1
duration: "30 minutes"
prerequisites:
  - "01_04_01_Heading_Typography"
learning_objectives:
  - "Apply the lead paragraph class for emphasized introductory text"
  - "Use inline text elements: small, mark, del, ins, u, strong, em"
  - "Implement text truncation techniques for overflow control"
  - "Combine inline elements for semantic text formatting"
keywords:
  - "bootstrap paragraphs"
  - "lead paragraph"
  - "inline text elements"
  - "text truncation"
  - "mark del ins"
  - "text-truncate"
---

# Paragraph Styles in Bootstrap 5

## Overview

Paragraphs form the backbone of readable web content. Bootstrap 5 provides a rich set of paragraph styles and inline text elements that allow developers to create well-structured, semantically meaningful, and visually appealing body text. These tools go far beyond basic text rendering, offering precise control over emphasis, annotations, deletions, insertions, highlights, and overflow behavior.

The **lead paragraph** is Bootstrap's primary paragraph enhancement. Applied via the `.lead` class, it creates a visually distinct introductory paragraph with a larger font size (`1.25rem`) and lighter weight (`300`). Lead paragraphs serve as opening statements, summaries, or introductory text that sets context for the content that follows. They are commonly used immediately after headings to provide a brief overview before diving into detailed body text.

Bootstrap's **inline text elements** correspond to semantic HTML elements that carry specific meaning. The `<strong>` element indicates strong importance, `<em>` indicates emphasis, `<mark>` highlights text for reference, `<del>` marks deleted text, `<ins>` marks inserted text, `<u>` indicates underlined text (typically for proper names or misspelled words), and `<small>` represents side comments or fine print. Each element has both semantic meaning (which screen readers interpret) and visual styling (which Bootstrap applies).

**Text truncation** is essential for controlling overflow in constrained layouts. Bootstrap's `.text-truncate` utility applies `text-overflow: ellipsis` to single-line text, replacing overflow content with an ellipsis character. For multi-line scenarios, developers can use CSS line clamping. The `.text-break` utility forces long words to wrap, preventing horizontal overflow from unbroken strings like URLs or generated tokens.

Understanding the distinction between semantic and presentational use of these elements is critical. Using `<strong>` for bold text that isn't important, or `<em>` for italic text that isn't emphasized, misleads assistive technologies and search engines. Bootstrap also provides purely presentational utility classes (`fw-bold`, `fst-italic`, `text-decoration-underline`) for cases where styling is visual rather than semantic.

This module covers all paragraph-related typography tools in Bootstrap 5, from basic lead paragraphs through complex combinations of inline elements with truncation strategies.

## Basic Implementation

### Lead Paragraph

The `.lead` class is the most straightforward paragraph enhancement in Bootstrap:

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
  <title>Paragraph Styles</title>
</head>
<body>
  <div class="container py-5">
    <h1>Introduction to Web Typography</h1>

    <p class="lead">
      Typography is the art and technique of arranging type to make written
      language legible, readable, and appealing when displayed. Good typography
      enhances user experience and content comprehension.
    </p>

    <p>
      The choice of typeface, size, line height, and spacing all contribute
      to how users perceive and interact with text on a web page. Bootstrap 5
      provides a comprehensive set of typography utilities that make it
      straightforward to implement professional typographic standards without
      writing custom CSS.
    </p>

    <p>
      Understanding the principles behind these utilities — including font
      scaling, vertical rhythm, and responsive behavior — enables developers
      to create interfaces that are both visually compelling and highly readable
      across all devices and screen sizes.
    </p>
  </div>
</body>
</html>
```

The `.lead` paragraph visually separates itself from regular paragraphs through its larger size and lighter weight, creating an immediate visual hierarchy that guides the reader's eye.

### Inline Text Elements

Each inline text element serves a specific semantic purpose:

```html
<div class="container py-4">
  <p><strong>Bold text using strong</strong> — indicates important information.</p>
  <p><em>Italic text using em</em> — indicates emphasis or stress.</p>
  <p><small>Small text</small> — for side comments and fine print.</p>
  <p><mark>Highlighted text using mark</mark> — marks text for reference.</p>
  <p><del>Deleted text</del> — shows removed content.</p>
  <p><ins>Inserted text</ins> — shows added content.</p>
  <p><u>Underlined text</u> — for proper names or annotations.</p>
</div>
```

### Combining Inline Elements

Inline elements can be nested and combined for complex formatting:

```html
<div class="container py-4">
  <p>
    The original price was <del class="text-body-secondary">$99.99</del>
    and is now <strong class="text-success">$49.99</strong>.
    <mark>This offer expires today!</mark>
  </p>

  <p>
    <small>
      <em>Terms and conditions apply.</em> Discount valid for
      <strong>new customers only</strong>.
    </small>
  </p>
</div>
```

### Text Truncation

The `.text-truncate` class applies single-line ellipsis truncation:

```html
<div class="container py-4" style="max-width: 400px;">
  <p class="text-truncate">
    This is a long paragraph that will be truncated with an ellipsis
    because it exceeds the available width of its container element.
    The truncated text is replaced with "..." at the end.
  </p>
</div>
```

### Text Break

The `.text-break` utility prevents long words from causing horizontal overflow:

```html
<div class="container py-4" style="max-width: 300px;">
  <p class="text-break">
    ThisURLIsVeryLongAndWillBreak: https://example.com/api/v2/resources/extended-path/with-many-segments/document-id-12345
  </p>
</div>
```

## Advanced Variations

### Lead Paragraph with Contextual Styling

Lead paragraphs can be enhanced with color, weight, and spacing utilities for different contexts:

```html
<div class="container py-4">
  <!-- Hero lead paragraph -->
  <h1 class="display-4 fw-bold">Build Better Products</h1>
  <p class="lead text-body-secondary mb-5">
    A comprehensive framework for creating modern, responsive web
    applications with less effort and more consistency.
  </p>

  <!-- Article lead with border -->
  <h2>Understanding CSS Grid</h2>
  <p class="lead border-start border-primary border-4 ps-3 mb-4">
    CSS Grid Layout is a two-dimensional layout system that revolutionizes
    how we design web page layouts, offering precise control over rows
    and columns simultaneously.
  </p>

  <!-- Alert-style lead -->
  <div class="alert alert-warning">
    <p class="lead mb-0 fw-semibold">
      <mark>Important:</mark> System maintenance scheduled for Saturday,
      March 15, from 2:00 AM to 6:00 AM UTC.
    </p>
  </div>
</div>
```

### Revision Tracking with del and ins

The `<del>` and `<ins>` elements are powerful for displaying document revisions, changelogs, and collaborative editing:

```html
<div class="container py-4">
  <h3>Document Revisions</h3>
  <div class="border rounded p-3 mb-3">
    <p>
      The project deadline has been moved from
      <del datetime="2025-03-01">March 1, 2025</del>
      to
      <ins datetime="2025-03-15">March 15, 2025</ins>
      to accommodate additional testing requirements.
    </p>
  </div>

  <h3>Changelog</h3>
  <ul class="list-group">
    <li class="list-group-item">
      <strong>v2.1.0</strong> —
      <del>Removed legacy API endpoint</del>
      <ins>Restored legacy API with deprecation notice</ins>
    </li>
    <li class="list-group-item">
      <strong>v2.0.5</strong> —
      Fixed <mark>critical</mark> authentication bypass vulnerability
    </li>
  </ul>
</div>
```

### Highlighted Search Results

The `<mark>` element is semantically designed for search result highlighting and reference marking:

```html
<div class="container py-4">
  <div class="mb-3">
    <input type="text" class="form-control" placeholder="Search..." value="bootstrap">
  </div>

  <div class="card mb-2">
    <div class="card-body">
      <h5 class="card-title">Getting Started with <mark>Bootstrap</mark></h5>
      <p class="card-text">
        <mark>Bootstrap</mark> is a powerful front-end framework for building
        responsive websites. Learn how to set up <mark>Bootstrap</mark> in your
        project and start building.
      </p>
    </div>
  </div>

  <small class="text-body-secondary">
    Showing 3 matches for "<mark>bootstrap</mark>"
  </small>
</div>
```

### Custom Mark Styling

Bootstrap's default `<mark>` styling (yellow background) can be customized for different highlight contexts:

```html
<style>
  mark.relevance-high { background-color: #ffc107; padding: 0.1em 0.2em; }
  mark.relevance-medium { background-color: #fff3cd; padding: 0.1em 0.2em; }
  mark.relevance-low { background-color: #f8f9fa; padding: 0.1em 0.2em; }
</style>

<div class="container py-4">
  <p>
    The analysis found
    <mark class="relevance-high">3 critical issues</mark>,
    <mark class="relevance-medium">7 warnings</mark>, and
    <mark class="relevance-low">12 informational notices</mark>.
  </p>
</div>
```

### Text Truncation with Tooltips

Combine truncation with Bootstrap tooltips to provide the full text on hover:

```html
<div class="container py-4" style="max-width: 300px;">
  <p
    class="text-truncate"
    data-bs-toggle="tooltip"
    data-bs-placement="top"
    title="This is the complete text that was truncated. Hover to see the full content."
  >
    This is the complete text that was truncated. Hover to see the full content.
  </p>
</div>

<script>
  // Initialize tooltips
  document.addEventListener('DOMContentLoaded', function() {
    var tooltipTriggerList = [].slice.call(
      document.querySelectorAll('[data-bs-toggle="tooltip"]')
    );
    tooltipTriggerList.map(function(el) {
      return new bootstrap.Tooltip(el);
    });
  });
</script>
```

### Multi-line Text Clamping

For truncating text across multiple lines (not natively supported by Bootstrap, but achievable with CSS):

```html
<style>
  .text-clamp-2 {
    display: -webkit-box;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
    overflow: hidden;
  }
  .text-clamp-3 {
    display: -webkit-box;
    -webkit-line-clamp: 3;
    -webkit-box-orient: vertical;
    overflow: hidden;
  }
</style>

<div class="container py-4">
  <div class="card" style="max-width: 350px;">
    <div class="card-body">
      <h5 class="card-title">Article Preview</h5>
      <p class="card-text text-clamp-3">
        This is a long article summary that will be clamped to exactly three
        lines. Any content beyond the third line will be hidden and replaced
        with an ellipsis character. This technique is invaluable for card
        layouts, feed items, and preview components where variable-length
        content must conform to a fixed visual footprint.
      </p>
      <a href="#" class="card-link">Read more</a>
    </div>
  </div>
</div>
```

## Best Practices

1. **Use `.lead` for introductory paragraphs only.** The lead class is designed for opening statements that set context. Using it on multiple paragraphs within the same section dilutes its visual emphasis and creates an inconsistent reading experience.

2. **Use semantic inline elements for their intended purpose.** Reserve `<strong>` for important information, `<em>` for emphasized text, `<mark>` for highlighted references, `<del>` for deleted content, and `<ins>` for inserted content. Incorrect semantic usage misleads assistive technologies.

3. **Use presentational utilities when semantic meaning doesn't apply.** If text should be bold for visual reasons but isn't important, use `.fw-bold` instead of `<strong>`. If text should be italic for stylistic reasons but isn't emphasized, use `.fst-italic` instead of `<em>`.

4. **Apply `.text-truncate` only to single-line containers with defined widths.** Truncation requires a constrained width to calculate overflow. Apply it to elements within grid columns, fixed-width containers, or elements with explicit `max-width`.

5. **Use `.text-break` for user-generated content and URLs.** Long unbroken strings from dynamic content can break layouts. Apply `.text-break` to containers that may receive such content.

6. **Nest `<small>` within `<p>` or heading elements for secondary text.** The `<small>` element reduces text size and is semantically appropriate for footnotes, disclaimers, and supplementary information.

7. **Combine `<del>` and `<ins>` with `datetime` attributes for revision tracking.** The `datetime` attribute provides machine-readable timestamps that can be used by browsers and assistive technologies.

8. **Use `<mark>` for search results and reference highlights, not general emphasis.** The `<mark>` element specifically indicates text marked for reference. For general highlighting, use background color utilities instead.

9. **Test truncation behavior across all breakpoints.** A container that truncates text on desktop may not truncate on mobile (or vice versa) due to different widths. Verify truncation at each breakpoint.

10. **Provide accessible alternatives for truncated content.** Truncated text hides information from sighted users. Use tooltips, "Read more" links, or expandable sections to make the full content accessible.

11. **Maintain adequate line height for body text.** Bootstrap's default line height of 1.5 for paragraphs provides good readability. Avoid reducing line height below 1.4 for body text, as it impairs reading speed and comprehension.

12. **Use consistent paragraph spacing.** Bootstrap applies `margin-top: 0` and `margin-bottom: 1rem` to paragraphs by default. Maintain this spacing consistency or override it systematically using spacing utilities.

## Common Pitfalls

### Using `<strong>` for Pure Visual Boldness

Using `<strong>` when text is not semantically important misleads screen readers, which may announce the text with added emphasis:

```html
<!-- WRONG: Using strong for visual styling -->
<p>The <strong>blue widget</strong> is on sale.</p>

<!-- CORRECT: Use fw-bold for visual bold, strong for importance -->
<p>The <span class="fw-bold">blue widget</span> is on sale.</p>
<p><strong>Warning:</strong> This action cannot be undone.</p>
```

### Truncation Without Defined Width

Applying `.text-truncate` to an element without width constraints has no visible effect because the element expands to fit its content:

```html
<!-- WRONG: No width constraint -->
<p class="text-truncate">
  This text will not truncate because the container has no width limit.
</p>

<!-- CORRECT: Constrained width -->
<div style="max-width: 300px;">
  <p class="text-truncate">
    This text will truncate because the container has a max-width.
  </p>
</div>
```

### Overusing `<mark>` for General Highlighting

The `<mark>` element carries specific semantic meaning (text marked for reference). Using it as a general-purpose highlight tool misrepresents its purpose:

```html
<!-- WRONG: Mark for general styling -->
<p>Our <mark>amazing</mark> new product is <mark>revolutionary</mark>.</p>

<!-- CORRECT: Mark for search results or reference -->
<p>Search results for "bootstrap":</p>
<p><mark>Bootstrap</mark> is a CSS framework. <mark>Bootstrap</mark> 5 includes utilities.</p>

<!-- For general highlighting, use background utilities -->
<p>Our <span class="bg-warning bg-opacity-25 px-1">amazing</span> new product.</p>
```

### Using `<u>` for Links or General Underlining

In web contexts, underlined text is strongly associated with hyperlinks. Using `<u>` for non-link text confuses users:

```html
<!-- WRONG: Underlined text looks like a link -->
<p>This is <u>important information</u> that users might click on.</p>

<!-- CORRECT: Use u only for proper names or misspellings per HTML5 spec -->
<p>The word <u>cefalexin</u> was misspelled in the original document.</p>
<p>Use <strong class="text-danger">important information</strong> instead.</p>
```

### Confusing `<del>` + `<ins>` Usage

When showing edits, both elements should clearly communicate what changed and what replaced it:

```html
<!-- WRONG: Unclear revision -->
<p>Meeting on <del>Monday</del> <ins>Tuesday</ins>.</p>

<!-- CORRECT: Clear context with datetime -->
<p>
  The weekly team meeting has been rescheduled from
  <del datetime="2025-03-10">Monday, March 10</del>
  to
  <ins datetime="2025-03-12">Wednesday, March 12</ins>
  due to a scheduling conflict.
</p>
```

### Multiple Lead Paragraphs

Using `.lead` on multiple paragraphs in the same section creates visual confusion:

```html
<!-- WRONG: Multiple leads compete for attention -->
<p class="lead">First important point about the product.</p>
<p class="lead">Second important point about the product.</p>
<p class="lead">Third important point about the product.</p>

<!-- CORRECT: One lead for introduction, regular paragraphs for details -->
<p class="lead">Our product offers three key advantages for modern teams.</p>
<p>First, it streamlines collaboration across distributed teams.</p>
<p>Second, it reduces overhead through intelligent automation.</p>
<p>Third, it provides real-time analytics for data-driven decisions.</p>
```

## Accessibility Considerations

Semantic inline elements carry built-in accessibility benefits that screen readers interpret. `<strong>` may be announced with increased vocal emphasis, `<em>` with stress emphasis, `<del>` as "deleted" or with a different tone, and `<ins>` as "inserted." Using these elements correctly ensures that the semantic structure of your content is communicated to non-sighted users.

When using `<mark>` for highlighted text, ensure the highlighted color provides sufficient contrast against its background. Bootstrap's default yellow highlight (`#ffc107` background) meets WCAG AA requirements for normal-sized text against white. Custom highlight colors should always be tested.

Text truncation creates a potential accessibility issue: the truncated content is invisible to sighted users. Screen readers will read the full text of a truncated element because the truncation is purely visual. However, sighted users relying on screen magnification may not be able to access the full content. Provide accessible alternatives such as tooltips, expand/collapse controls, or links to full content.

```html
<!-- Accessible truncation pattern -->
<div style="max-width: 300px;">
  <p class="text-truncate" aria-label="Full article title: Understanding Bootstrap 5 Typography System in Detail">
    Understanding Bootstrap 5 Typography System in Detail
  </p>
  <a href="/article" class="small">Read full article</a>
</div>
```

The `<small>` element should not contain critical information. While screen readers will announce `<small>` content, its reduced visual prominence means sighted users may overlook it. Place warnings, errors, and critical notices in appropriately prominent elements.

For `<ins>` and `<del>` elements used in document revision contexts, ensure that the revision is understandable without relying solely on visual strikethrough and underline cues. Screen readers may not consistently convey these visual changes, so context from surrounding text is important.

```html
<!-- Accessible revision with context -->
<p>
  <strong>Updated:</strong> The deadline changed from
  <del aria-label="previous date">March 1</del> to
  <ins aria-label="new date">March 15</ins>.
</p>
```

## Responsive Behavior

Paragraph text responds to viewport changes through Bootstrap's relative sizing (rem units) and the grid system. Standard paragraphs maintain readability across breakpoints due to Bootstrap's default body font size of 1rem (16px).

Lead paragraphs, being larger, may require responsive adjustments. On small screens, a lead paragraph at `1.25rem` within a narrow container can produce very short lines that look awkward. Consider using responsive font size utilities:

```html
<p class="lead fs-6 fs-md-5">
  This lead paragraph adjusts its size based on the viewport width.
</p>
```

Text truncation behavior changes with container width. A paragraph truncated at 400px on desktop may not truncate on a 320px mobile screen if the grid column changes from `col-4` to `col-12`. Always test truncation at each breakpoint.

For multi-line clamping, the number of visible lines may need to change based on viewport:

```html
<style>
  .clamp-responsive {
    display: -webkit-box;
    -webkit-box-orient: vertical;
    overflow: hidden;
    -webkit-line-clamp: 5;
  }
  @media (min-width: 768px) {
    .clamp-responsive {
      -webkit-line-clamp: 3;
    }
  }
</style>

<p class="clamp-responsive">
  Long content that shows more lines on mobile and fewer on desktop...
</p>
```

Text alignment utilities (`text-start`, `text-center`, `text-end`) can be applied responsively to paragraphs:

```html
<p class="text-center text-md-start">
  Centered on mobile, left-aligned on tablet and larger screens.
</p>
```

For content-heavy pages, consider constraining paragraph width using `max-width` or Bootstrap's container sizes to maintain optimal line length (45-75 characters per line) across all viewports:

```html
<div class="container">
  <div class="row justify-content-center">
    <div class="col-12 col-lg-8 col-xl-7">
      <p class="lead">Optimally constrained paragraph text...</p>
      <p>Body text with comfortable line length for reading...</p>
    </div>
  </div>
</div>
```

This approach ensures paragraphs remain readable regardless of screen size, avoiding the strain of reading extremely long lines on wide monitors or cramped text on narrow mobile screens.
