---
title: "Text Transformation"
subtitle: "Controlling letter casing with Bootstrap 5 text transformation utilities"
category: "Bootstrap Basics"
subcategory: "Typography Engine"
difficulty: 1
duration: "10 minutes"
prerequisites: ["01_04_01_Heading_Typography", "01_04_02_Paragraph_Styles"]
learning_objectives:
  - "Apply text-lowercase to convert text to all lowercase"
  - "Use text-uppercase to convert text to all uppercase"
  - "Use text-capitalize to capitalize the first letter of each word"
  - "Understand the CSS text-transform property behind these utilities"
  - "Combine text transformation with other typography utilities"
keywords:
  - "text transformation bootstrap"
  - "text-lowercase"
  - "text-uppercase"
  - "text-capitalize"
  - "text-transform CSS"
  - "bootstrap casing utilities"
---

# Text Transformation

## Overview

Text transformation utilities in Bootstrap 5 provide a straightforward way to control the casing of text without altering the underlying HTML content. These utilities map directly to the CSS `text-transform` property, offering three values: `lowercase`, `uppercase`, and `capitalize`. The Bootstrap classes are `text-lowercase`, `text-uppercase`, and `text-capitalize`.

The key advantage of using CSS-based text transformation is that it preserves the original content in the DOM while changing only its visual presentation. This means that screen readers, search engines, and copy-paste operations interact with the original casing, while users see the transformed version. This is fundamentally different from changing the casing in the HTML itself, which would alter the actual content.

`text-lowercase` converts all characters to lowercase. This is useful for normalizing content that may arrive with inconsistent casing, such as user-generated input or data from APIs. `text-uppercase` converts all characters to uppercase, which is commonly used for labels, badges, navigation links, and headings that need a strong, commanding presence. `text-capitalize` converts the first character of each word to uppercase, useful for titles, names, and section labels.

These utilities are particularly powerful when combined with other Bootstrap typography utilities like font-weight classes, letter-spacing (which can be applied via custom CSS), color utilities, and responsive font sizes. The transformation persists across all breakpoints unless overridden by custom CSS.

Understanding the implications of text transformation for accessibility is essential. Screen readers may read uppercase text letter-by-letter in some configurations, and `text-capitalize` does not handle exceptions like articles and prepositions the way proper title case does. These considerations should inform how and where you apply text transformations.

## Basic Implementation

### Text Lowercase

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
  <title>Text Transformation</title>
</head>
<body>
  <div class="container py-5">
    <h2 class="mb-4">Text Transformation Utilities</h2>

    <p class="text-lowercase">THIS TEXT WILL DISPLAY IN ALL LOWERCASE.</p>
    <p class="text-uppercase">this text will display in all uppercase.</p>
    <p class="text-capitalize">this text will have each word capitalized.</p>
  </div>
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

The HTML content retains its original casing. Only the visual rendering changes.

### Text Uppercase

```html
<div class="container py-4">
  <span class="badge bg-primary text-uppercase">New Release</span>
  <span class="badge bg-danger text-uppercase">Critical</span>
  <span class="badge bg-success text-uppercase">Completed</span>

  <h5 class="text-uppercase mt-4">Section Title in Uppercase</h5>
  <a href="#" class="text-uppercase text-decoration-none">Read More</a>
</div>
```

### Text Capitalize

```html
<div class="container py-4">
  <p class="text-capitalize">welcome to our product showcase</p>
  <p class="text-capitalize">john doe — senior developer</p>
  <p class="text-capitalize">the quick brown fox jumps over the lazy dog</p>
</div>
```

### Combining with Font Weight

```html
<div class="container py-4">
  <p class="text-uppercase fw-bold">Bold uppercase heading</p>
  <p class="text-capitalize fw-semibold">Semibold capitalized title</p>
  <p class="text-lowercase fw-light">Light lowercase subtitle</p>
</div>
```

## Advanced Variations

### Uppercase Labels with Spacing

```html
<div class="container py-4">
  <style>
    .label-upper {
      text-transform: uppercase;
      letter-spacing: 0.15em;
      font-size: 0.75rem;
      font-weight: 600;
    }
  </style>

  <div class="mb-3">
    <label class="label-upper text-muted d-block mb-1">Email Address</label>
    <input type="email" class="form-control" placeholder="you@example.com">
  </div>
  <div class="mb-3">
    <label class="label-upper text-muted d-block mb-1">Password</label>
    <input type="password" class="form-control" placeholder="Enter password">
  </div>
</div>
```

### Capitalize for Dynamic Names

```html
<div class="container py-4">
  <h4 class="mb-3">Team Members</h4>
  <div class="list-group">
    <div class="list-group-item">
      <span class="text-capitalize fw-semibold">alice johnson</span>
      <small class="text-muted ms-2">Lead Designer</small>
    </div>
    <div class="list-group-item">
      <span class="text-capitalize fw-semibold">bob martinez</span>
      <small class="text-muted ms-2">Backend Engineer</small>
    </div>
    <div class="list-group-item">
      <span class="text-capitalize fw-semibold">charlie nguyen</span>
      <small class="text-muted ms-2">Product Manager</small>
    </div>
  </div>
</div>
```

### Uppercase Navigation

```html
<nav class="navbar navbar-expand-lg navbar-dark bg-dark mb-4">
  <div class="container">
    <a class="navbar-brand text-uppercase fw-bold" href="#">Brand</a>
    <div class="navbar-nav">
      <a class="nav-link text-uppercase small" href="#">Home</a>
      <a class="nav-link text-uppercase small" href="#">Products</a>
      <a class="nav-link text-uppercase small" href="#">About</a>
      <a class="nav-link text-uppercase small" href="#">Contact</a>
    </div>
  </div>
</nav>
```

### Transformation with Color and Background

```html
<div class="container py-4">
  <span class="badge bg-warning text-dark text-uppercase fw-bold px-3 py-2">
    Limited Offer
  </span>

  <p class="text-uppercase text-primary fw-bold fs-5 mt-3">Important Announcement</p>
  <p class="text-capitalize bg-light p-3 rounded">scheduled maintenance window: saturday 2am - 4am</p>
</div>
```

### Responsive Text Transformation

Bootstrap does not include responsive text transformation utilities by default. Custom CSS is needed:

```css
@media (max-width: 576px) {
  .text-transform-responsive {
    text-transform: none;
  }
}
@media (min-width: 577px) {
  .text-transform-responsive {
    text-transform: uppercase;
  }
}
```

```html
<h2 class="text-transform-responsive fw-bold">Responsive Transformation</h2>
```

## Best Practices

1. **Use `text-uppercase` for labels, badges, and short UI text.** Uppercase text commands attention and works well for elements that need to stand out from body content. Avoid uppercase for paragraphs.

2. **Use `text-capitalize` for dynamically generated names and titles.** When displaying user names or API-driven content that may arrive in lowercase, `text-capitalize` normalizes the display without modifying the source data.

3. **Use `text-lowercase` for email addresses and URLs.** Normalizing these elements to lowercase improves visual consistency, as the original content may have mixed casing.

4. **Add letter-spacing when using `text-uppercase`.** Uppercase letters at their default spacing can feel cramped. Custom letter-spacing (e.g., `0.1em` to `0.2em`) improves readability.

5. **Do not use text transformation on long paragraphs.** Uppercase paragraphs are significantly harder to read because all characters are the same height, removing the visual cues provided by ascenders and descenders.

6. **Preserve original content in the HTML.** The power of CSS text transformation is that the DOM content remains unchanged. Do not also transform the HTML content, as this would duplicate the transformation and make the content harder to maintain.

7. **Test `text-capitalize` with articles and prepositions.** The capitalize value capitalizes every word, including "a", "an", "the", "of", and "in". This is not proper title case. For true title case, you need custom logic.

8. **Use `text-uppercase` with caution on acronyms and abbreviations.** If a word is already uppercase in the HTML (like "NASA" or "HTML"), `text-uppercase` is redundant. However, if mixed casing is present, the transform ensures consistency.

9. **Combine transformation with responsive font sizes.** A `text-uppercase` heading at `2rem` looks bold and authoritative. The same text at `0.875rem` can look cramped. Ensure font size complements the transformation.

10. **Consider using custom CSS for transformations that need to be responsive.** Bootstrap's transformation utilities are not responsive. If you need to apply or remove transformations at specific breakpoints, define custom classes.

11. **Avoid mixing transformations on nested elements.** Applying `text-uppercase` to a parent and `text-lowercase` to a child creates CSS specificity conflicts that can be confusing to debug.

12. **Use text transformation consistently across similar UI elements.** If navigation links use `text-uppercase`, all navigation links should use it. Inconsistent application creates visual discord.

## Common Pitfalls

### Uppercase Paragraphs

Applying `text-uppercase` to long paragraphs severely degrades readability. All-caps text eliminates the visual shape of words that comes from varying letter heights:

```html
<!-- WRONG: Uppercase paragraphs are unreadable -->
<p class="text-uppercase">
  This long paragraph in uppercase is extremely difficult to read because
  every character is the same height. The reader loses the visual shape
  of words and must concentrate harder to parse the content. This is a
  well-documented phenomenon in typographic research.
</p>

<!-- RIGHT: Uppercase for short labels, normal case for body -->
<span class="text-uppercase fw-semibold small text-muted">Category</span>
<p>This long paragraph uses normal casing for comfortable reading.</p>
```

### Expecting Title Case from `text-capitalize`

`text-capitalize` capitalizes the first letter of every word. This is not title case, which requires lowercase articles and prepositions:

```html
<!-- text-capitalize produces: "The Lord Of The Rings" -->
<!-- Proper title case: "The Lord of the Rings" -->
<p class="text-capitalize">the lord of the rings</p>

<!-- For proper title case, apply correct casing in the HTML -->
<p>The Lord of the Rings</p>
```

### Applying Transformations to Already-Correct Content

If content already has proper casing, applying a transformation can break it:

```html
<!-- WRONG: Transform overrides intentional casing -->
<p class="text-lowercase">NASA launches Artemis III Mission</p>
<!-- Renders as: "nasa launches artemis iii mission" -->
<!-- NASA is an acronym that should remain uppercase -->

<!-- RIGHT: Let intentionally-cased content stand as-is -->
<p>NASA launches Artemis III Mission</p>
```

### Using Transformations Instead of Proper Data Casing

Relying on CSS transformations to fix improperly cased data masks the underlying data quality issue:

```html
<!-- WRONG: Hiding bad data with CSS -->
<p class="text-capitalize">username: jOHn_DOE_123</p>
<!-- Produces: "Username: JOHn_DOE_123" — still has underscores and mixed case -->

<!-- RIGHT: Fix the data at the source -->
<p>John Doe</p>
```

### Nested Transformation Conflicts

Applying conflicting transformations to parent and child elements creates confusion:

```html
<!-- WRONG: Conflicting nested transformations -->
<div class="text-uppercase">
  THIS IS UPPERCASE
  <span class="text-lowercase">this is lowercase</span>
  THIS IS UPPERCASE AGAIN
</div>
<!-- While this technically works, it creates unpredictable reading patterns -->

<!-- RIGHT: Consistent transformation at one level -->
<div class="text-uppercase">
  ALL CONTENT IS UPPERCASE CONSISTENTLY
</div>
```

## Accessibility Considerations

Text transformation has specific accessibility implications that developers must understand. The CSS `text-transform` property does not change the underlying text content. Screen readers read the original content, not the transformed version. This means that `text-uppercase` applied to "Hello World" will be read as "Hello World", not as individual letters.

However, some older screen readers and specific configurations may read uppercase text differently. JAWS, for example, has been known to spell out uppercase words letter-by-letter in certain modes. To mitigate this, avoid using `text-uppercase` on critical information that users must understand quickly.

```html
<!-- ACCESSIBLE: Uppercase for decorative labels, not critical info -->
<span class="text-uppercase badge bg-info">Beta</span>

<!-- AVOID: Uppercase for critical instructions -->
<p class="text-uppercase"><strong>Warning: Do not close this window</strong></p>
```

The `text-capitalize` utility does not handle special cases like proper nouns, acronyms, or title case exceptions. If the visual presentation suggests proper capitalization but the underlying content has errors, users relying on the raw text (via copy-paste, screen readers, or assistive technologies) will encounter the incorrect casing.

For form labels that use `text-uppercase` with custom letter spacing, ensure the `aria-label` or associated `<label>` element provides the correct text:

```html
<label for="email" class="text-uppercase small" aria-label="Email Address">
  Email Address
</label>
<input type="email" id="email" class="form-control">
```

The letter-spacing added to uppercase text should not be so extreme that it disrupts the visual reading flow. Excessive letter-spacing (above `0.3em`) can cause uppercase text to appear as separate words, confusing users with cognitive disabilities.

## Responsive Behavior

Bootstrap's text transformation utilities apply globally and do not have responsive variants. The transformation set by `text-uppercase`, `text-lowercase`, or `text-capitalize` persists at all viewport sizes.

For responsive transformation behavior, define custom CSS using Bootstrap's breakpoint variables:

```css
/* Uppercase on desktop, normal case on mobile */
@media (max-width: 576px) {
  .text-upper-md-up {
    text-transform: none;
  }
}
@media (min-width: 577px) {
  .text-upper-md-up {
    text-transform: uppercase;
  }
}
```

```html
<h2 class="text-upper-md-up fw-bold">Section Title</h2>
```

### Responsive Uppercase Labels

A common pattern is to apply uppercase to form labels on desktop where there is more visual space, but use normal case on mobile where uppercase text can feel heavy:

```css
@media (min-width: 768px) {
  .label-responsive {
    text-transform: uppercase;
    letter-spacing: 0.1em;
    font-size: 0.75rem;
    font-weight: 600;
    color: #6c757d;
  }
}
@media (max-width: 767px) {
  .label-responsive {
    text-transform: none;
    font-size: 0.875rem;
    font-weight: 500;
    color: #212529;
  }
}
```

```html
<label class="label-responsive d-block mb-1" for="name">Full Name</label>
<input type="text" id="name" class="form-control">
```

### Transformation in Responsive Navigation

Navigation links often switch between uppercase on desktop and normal case on mobile:

```html
<style>
  @media (min-width: 992px) {
    .nav-link-upper {
      text-transform: uppercase;
      letter-spacing: 0.08em;
      font-size: 0.8125rem;
    }
  }
</style>

<nav class="navbar navbar-expand-lg navbar-light bg-light">
  <div class="container">
    <a class="navbar-brand" href="#">Brand</a>
    <div class="navbar-nav">
      <a class="nav-link nav-link-upper" href="#">Home</a>
      <a class="nav-link nav-link-upper" href="#">Services</a>
      <a class="nav-link nav-link-upper" href="#">Contact</a>
    </div>
  </div>
</nav>
```

This approach ensures that text transformation enhances the design where appropriate without degrading readability on smaller viewports.
