# HTML Document Validation

## Topic Title
Validating HTML Documents

## Concept Explanation

### What is HTML Validation?

HTML validation is the process of checking that your HTML code follows proper syntax and standards. The World Wide Web Consortium (W3C) provides a free validator.

### Why Validate?

1. **Cross-browser compatibility** - Valid code renders consistently
2. **Accessibility** - Helps screen readers work correctly
3. **SEO** - Search engines prefer valid code
4. **Maintainability** - Easier to find and fix errors
5. **Professional standards** - Industry best practice

### Validation Tools

1. **W3C HTML Validator** - https://validator.w3.org/
2. **HTML5 Validator** - Online validation service
3. **Browser Developer Tools** - Built-in validation
4. **VS Code Extensions** - Real-time validation

## Code Examples

### Example 1: Common Validation Errors

```html
<!-- Error: Missing alt attribute on image -->
<img src="photo.jpg">

<!-- Fixed -->
<img src="photo.jpg" alt="Description">

<!-- Error: Unclosed tags -->
<p>Paragraph

<!-- Fixed -->
<p>Paragraph</p>

<!-- Error: Improper nesting -->
<p>This is <strong>bold</p></strong>

<!-- Fixed -->
<p>This is <strong>bold</strong></p>

<!-- Error: Using deprecated tags -->
<center>Centered text</center>
<font color="red">Red text</font>

<!-- Fixed - use CSS instead -->
<div style="text-align: center;">Centered text</div>
<p style="color: red;">Red text</p>
```

### Example 2: Validation Checklist

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Valid Document</title>
</head>
<body>
    <!-- All elements properly closed -->
    <h1>Title</h1>
    <p>Paragraph with <strong>bold</strong> text.</p>
    
    <!-- Attributes properly quoted -->
    <img src="image.jpg" alt="Description" width="100">
    
    <!-- Required attributes present -->
    <a href="page.html">Link</a>
    
    <!-- Proper heading hierarchy -->
    <h1>Main</h1>
    <h2>Section</h2>
    <h3>Subsection</h3>
</body>
</html>
```

## How to Validate

### Using W3C Validator

1. Go to https://validator.w3.org/
2. Enter your URL or paste HTML code
3. Click "Check"
4. Review and fix any errors

### Using VS Code

1. Install "HTML CSS Support" extension
2. Errors appear in "Problems" panel
3. Hover over errors for details

## Best Practices

1. **Validate regularly** - Check code during development
2. **Fix errors immediately** - Don't let them accumulate
3. **Use HTML5 DOCTYPE** - Ensures standards mode
4. **Check accessibility** - Valid HTML helps accessibility

## Common Validation Errors

| Error | Fix |
|-------|-----|
| Missing alt text | Add alt attribute to images |
| Unclosed tags | Close all tags properly |
| Duplicate IDs | Use unique IDs |
| Improper nesting | Nest elements correctly |
| Deprecated tags | Use modern alternatives |
