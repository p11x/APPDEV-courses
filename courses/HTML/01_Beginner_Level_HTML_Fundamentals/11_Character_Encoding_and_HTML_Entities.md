# Character Encoding and HTML Entities

## Topic Title
Character Encoding and HTML Entities

## Concept Explanation

### What is Character Encoding?

Character encoding defines how characters are stored and displayed in digital text. The most common encoding is UTF-8, which supports virtually all characters in all languages.

### Why Encoding Matters

1. **Internationalization** - Support for all languages
2. **Special Characters** - Display symbols and emojis
3. **Browser Compatibility** - Consistent rendering
4. **Security** - Prevent encoding-based attacks

### HTML Entities

HTML entities are special codes used to display characters that have special meaning in HTML or cannot be easily typed on a keyboard.

**Entity Syntax:**
- Named entities: `&entity_name;`
- Numeric entities: `&#entity_number;`

### Common HTML Entities

| Character | Named Entity | Numeric Entity |
|-----------|--------------|----------------|
| < | `<` | `&#60;` |
| > | `>` | `&#62;` |
| & | `&` | `&#38;` |
| " | `"` | `&#34;` |
| ' | `'` | `'` |
| Space | `&nbsp;` | `&#160;` |
| © | `&copy;` | `&#169;` |
| ® | `&reg;` | `&#174;` |
| ™ | `&trade;` | `&#8482;` |

## Code Examples

### Example 1: Setting Character Encoding

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Character Encoding Example</title>
</head>
<body>
    <p>This page uses UTF-8 encoding.</p>
    <p>It can display: 日本語 中文 한국어 العربية</p>
    <p>And emojis: 😀 🚀 💻 🌐</p>
</body>
</html>
```

### Example 2: Using HTML Entities

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>HTML Entities</title>
</head>
<body>
    <h1>HTML Entities Demo</h1>
    
    <!-- Special characters -->
    <p>5 < 10</p>
    <p>10 > 5</p>
    <p>Coffee & Tea</p>
    
    <!-- Quotation marks -->
    <p>He said "Hello World"</p>
    
    <!-- Copyright -->
    <p>&copy; 2024 Company Name</p>
    
    <!-- Non-breaking space -->
    <p>Price: $100&nbsp;USD</p>
    
    <!-- Registered trademark -->
    <p>Product&#8482; is amazing</p>
</body>
</html>
```

### Example 3: Math and Currency Symbols

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Math and Currency</title>
</head>
<body>
    <h1>Mathematical Symbols</h1>
    <p>Addition: 5 + 3 = 8</p>
    <p>Multiplication: 4 &times; 5 = 20</p>
    <p>Division: 20 &divide; 4 = 5</p>
    <p>Plus/Minus: 5 &plusmn; 3</p>
    <p>Not Equal: 5 &ne; 6</p>
    <p>Greater or Equal: 5 &ge; 3</p>
    <p>Less or Equal: 3 &le; 5</p>
    
    <h1>Currency Symbols</h1>
    <p>Dollar: $100</p>
    <p>Euro: &euro;100</p>
    <p>Pound: &pound;100</p>
    <p>Yen: &yen;100</p>
    <p>Cent: &cent;50</p>
</body>
</html>
```

### Example 4: Arrow and Special Symbols

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Arrows and Symbols</title>
</head>
<body>
    <h1>Arrow Symbols</h1>
    <p>Left: &larr;</p>
    <p>Right: &rarr;</p>
    <p>Up: &uarr;</p>
    <p>Down: &darr;</p>
    <p>Double: &harr;</p>
    
    <h1>Special Symbols</h1>
    <p>Checkmark: &check;</p>
    <p>Cross: &times;</p>
    <p>Star: &star;</p>
    <p>Bullet: &bull;</p>
    <p>Ellipsis: ... or &hellip;</p>
</body>
</html>
```

## Best Practices

1. **Always use UTF-8** - `<meta charset="UTF-8">`
2. **Use entities for special chars** - < > & in content
3. **Encode URLs properly** - Use percent-encoding
4. **Validate your code** - Check for encoding issues

## Common Mistakes

### Wrong: Not Declaring Encoding
```html
<!-- Wrong -->
<head><title>Page</title></head>

<!-- Correct -->
<head>
    <meta charset="UTF-8">
    <title>Page</title>
</head>
```

### Wrong: Using Literal Special Characters
```html
<!-- Wrong -->
<p>5 < 10</p>

<!-- Correct -->
<p>5 < 10</p>
```

## Exercises

### Exercise 1: Create a Symbol Page
Create a page displaying all the entities from this lesson.

### Exercise 2: Fix Encoding Issues
Find and fix encoding problems in a sample page.
