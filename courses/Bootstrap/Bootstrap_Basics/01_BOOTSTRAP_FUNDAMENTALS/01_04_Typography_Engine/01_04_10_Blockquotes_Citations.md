---
title: "Blockquotes Citations"
topic: "Typography Engine"
subtopic: "Blockquotes Citations"
difficulty: 1
duration: "20 minutes"
prerequisites: ["Paragraph Styles", "Heading Typography"]
learning_objectives:
  - Use blockquote element with Bootstrap styling
  - Add footer and cite elements for source attribution
  - Style blockquotes with naming sources and visual variations
---

## Overview

The HTML `<blockquote>` element represents an extended quotation from another source. Bootstrap styles it with a left border and padding, creating a visually distinct quoted section. The `<footer>` element inside a blockquote provides source attribution, and `<cite>` marks up the title of the work being quoted. Together, these elements create properly attributed, semantic quotations.

## Basic Implementation

Basic blockquote with source attribution:

```html
<blockquote class="blockquote">
  <p>"The best way to predict the future is to invent it."</p>
  <footer class="blockquote-footer">Alan Kay</footer>
</blockquote>
```

Blockquote with cite element for the work title:

```html
<blockquote class="blockquote">
  <p>"Design is not just what it looks like and feels like. Design is how it works."</p>
  <footer class="blockquote-footer">
    Steve Jobs in <cite title="The New York Times">The New York Times</cite>
  </footer>
</blockquote>
```

Centered blockquote with text alignment:

```html
<blockquote class="blockquote text-center">
  <p>"Stay hungry, stay foolish."</p>
  <footer class="blockquote-footer">Steve Jobs, <cite>Stanford Commencement 2005</cite></footer>
</blockquote>
```

## Advanced Variations

Styled blockquote with custom border color:

```html
<blockquote class="blockquote border-start border-5 border-primary ps-3">
  <p class="mb-1">"Simplicity is the ultimate sophistication."</p>
  <footer class="blockquote-footer">Leonardo da Vinci</footer>
</blockquote>
```

Blockquote inside a card component for visual emphasis:

```html
<div class="card">
  <div class="card-body">
    <figure>
      <blockquote class="blockquote">
        <p>"A well-known quote, contained in a blockquote element."</p>
      </blockquote>
      <figcaption class="blockquote-footer">
        Someone famous in <cite title="Source Title">Source Title</cite>
      </figcaption>
    </figure>
  </div>
</div>
```

Right-aligned blockquote with different border color:

```html
<blockquote class="blockquote text-end border-end border-4 border-success pe-3">
  <p>"Code is like humor. When you have to explain it, it's bad."</p>
  <footer class="blockquote-footer">Cory House</footer>
</blockquote>
```

Multiple blockquotes in a testimonials section:

```html
<div class="row g-4">
  <div class="col-md-6">
    <blockquote class="blockquote bg-light p-4 rounded border-start border-4 border-primary">
      <p>"Excellent course material."</p>
      <footer class="blockquote-footer">Student A, <cite>2024 Cohort</cite></footer>
    </blockquote>
  </div>
  <div class="col-md-6">
    <blockquote class="blockquote bg-light p-4 rounded border-start border-4 border-success">
      <p>"Clear and well-structured."</p>
      <footer class="blockquote-footer">Student B, <cite>2024 Cohort</cite></footer>
    </blockquote>
  </div>
</div>
```

## Best Practices

1. Always use `<blockquote>` for extended quotations — don't style `<div>` elements to look like quotes.
2. Include `<footer class="blockquote-footer">` for source attribution on every blockquote.
3. Use `<cite>` to reference the title of the quoted work for proper academic attribution.
4. Apply `text-center` or `text-end` for blockquote alignment rather than custom CSS.
5. Use `border-start` with `border-{color}` and `border-{width}` for custom colored quote borders.
6. Wrap blockquotes in `<figure>` elements for semantic grouping when multiple quotes share a source.
7. Keep quoted text in `<p>` tags inside the blockquote for proper paragraph spacing.
8. Use blockquotes sparingly in body content — they draw significant visual attention.
9. Test blockquotes with long quoted text to ensure wrapping and spacing remain readable.
10. Apply responsive text alignment (`text-center text-md-start`) for adaptive quote layouts.

## Common Pitfalls

- **Using `<q>` instead of `<blockquote>`**: `<q>` is for short inline quotes; `<blockquote>` is for extended quotations.
- **Missing `<footer>` attribution**: Quotes without source attribution lack credibility and violate citation conventions.
- **Nesting blockquotes**: Placing `<blockquote>` inside `<blockquote>` creates confusing indentation and is rarely semantically correct.
- **Using blockquotes for indentation**: `<blockquote>` has semantic meaning for quotations — use CSS `margin` or `padding` for visual indentation.
- **Forgetting `<cite>` for titles**: Without `<cite>`, work titles in footers lack semantic markup.
- **Overriding border with `border-0`**: Removing the left border eliminates the primary visual indicator of a blockquote.
- **Inconsistent quote marks**: Including literal quote marks (`"`) inside `<p>` when CSS could provide decorative quotation marks.

## Accessibility Considerations

- Screen readers announce `<blockquote>` as a quotation, providing context for the enclosed text.
- Use `<footer>` inside blockquotes to associate attribution with the quoted content.
- Ensure `<cite>` contains the actual title of the work, not the author's name.
- Provide sufficient contrast between blockquote border color and the background.
- Use `lang` attribute on blockquotes quoting text in a different language.
- Don't use blockquotes for decorative purposes — they carry semantic meaning that screen readers rely on.

## Responsive Behavior

Blockquotes are block-level elements that adapt to their container width automatically. On narrow mobile screens, long quoted text wraps naturally within the available space.

```html
<div class="container">
  <div class="row">
    <div class="col-12 col-lg-8 offset-lg-2">
      <blockquote class="blockquote text-center border-start-0 border-top border-bottom py-4 my-4">
        <p class="fs-4">"In the middle of difficulty lies opportunity."</p>
        <footer class="blockquote-footer mt-2">Albert Einstein</footer>
      </blockquote>
    </div>
  </div>
</div>
```

Use responsive offset classes to center blockquotes on desktop while keeping them full-width on mobile. Apply responsive font size classes (`fs-4 fs-lg-3`) for quotations that should scale with viewport width.
