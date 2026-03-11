# Ways to Apply CSS

## Definition

There are three main ways to add CSS styles to your HTML webpage: inline CSS (directly on HTML elements), internal CSS (in a style tag within the HTML file), and external CSS (in a separate file linked to the HTML). Each method has different use cases, advantages, and best practices for when to use them in real-world development.

## Key Points

- Inline CSS uses the style attribute directly on HTML elements - good for quick testing but not recommended for production
- Internal CSS goes inside `<style>` tags in the `<head>` section - useful for single-page projects
- External CSS is stored in a separate .css file and linked to HTML - best for multi-page websites
- External CSS allows one style file to control many pages, making updates easy
- The `<link>` tag connects external CSS files to HTML
- You can use all three methods together, but external is preferred for maintainability
- Internal and inline CSS override external CSS when there's a conflict (more specific wins)

## Code Example

### Method 1: Inline CSS

```html
<!DOCTYPE html>
<html>
<head>
    <title>Inline CSS Example</title>
</head>
<body>
    <h1 style="color: purple; text-align: center;">Inline CSS Example</h1>
    <p style="color: darkgreen; font-size: 18px;">This paragraph uses inline CSS directly in the HTML element.</p>
    <button style="background-color: orange; color: white; padding: 10px 20px; border: none; border-radius: 5px;">Styled Button</button>
</body>
</html>
```

### Method 2: Internal CSS

```html
<!DOCTYPE html>
<html>
<head>
    <title>Internal CSS Example</title>
    <style>
        /* Internal CSS goes inside style tags */
        body {
            font-family: Arial, sans-serif;
            background-color: #f9f9f9;
        }
        
        h1 {
            color: navy;
            text-align: center;
        }
        
        p {
            color: #333;
            line-height: 1.6;
        }
        
        .highlight-box {
            background-color: lightblue;
            padding: 20px;
            border-left: 5px solid blue;
        }
    </style>
</head>
<body>
    <h1>Internal CSS Example</h1>
    <p>This paragraph is styled using internal CSS defined in the head section.</p>
    <div class="highlight-box">
        This box has a light blue background with blue border on the left.
    </div>
</body>
</html>
```

### Method 3: External CSS

```html
<!-- This is the HTML file (index.html) -->
<!DOCTYPE html>
<html>
<head>
    <title>External CSS Example</title>
    <!-- Link to external CSS file -->
    <link rel="stylesheet" href="styles.css">
</head>
<body>
    <h1>External CSS Example</h1>
    <p>This paragraph is styled using an external CSS file.</p>
    <div class="card">
        <h2>Card Title</h2>
        <p>This is card content styled from external CSS.</p>
    </div>
</body>
</html>
```

```css
/* This is the external CSS file (styles.css) */
body {
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    background-color: #e8f4f8;
    margin: 0;
    padding: 20px;
}

h1 {
    color: #2c3e50;
    text-align: center;
}

p {
    color: #555;
    font-size: 16px;
}

.card {
    background-color: white;
    border-radius: 8px;
    padding: 25px;
    box-shadow: 0 2px 5px rgba(0,0,0,0.1);
    max-width: 400px;
    margin: 20px auto;
}

.card h2 {
    color: #3498db;
    margin-top: 0;
}
```

## Explanation

### Inline CSS Explanation

- **style="color: purple; text-align: center;"** - The style attribute contains CSS directly on the element
- Each property is separated by a semicolon
- Quick to use but hard to maintain because styles are scattered throughout HTML

### Internal CSS Explanation

- **`<style>` tag** - Placed in the `<head>` section of HTML
- **body {...}** - Styles the entire page background and font
- **.highlight-box** - A class selector (note the dot) that styles elements with class="highlight-box"
- **border-left: 5px solid blue;** - Creates a thick blue line on the left side of the element

### External CSS Explanation

- **`<link rel="stylesheet" href="styles.css">`** - This line connects the CSS file to HTML
- **rel="stylesheet"** - Tells browser this is a stylesheet
- **href="styles.css"** - Points to the external CSS file location
- The external CSS file contains regular CSS syntax without any HTML tags

## Visual Result

### Inline CSS Result
- Purple centered heading
- Dark green paragraph text at 18px size
- Orange button with rounded corners

### Internal CSS Result
- Light gray background on the page
- Navy colored heading centered
- Dark text paragraphs with comfortable line spacing
- A highlighted box with light blue background and blue left border

### External CSS Result
- Light blue-tinted page background
- Professional looking card with white background
- Card has subtle shadow and rounded corners
- Smooth, modern appearance typical of real websites

All three methods produce visually styled content, but external CSS is the most maintainable for real websites because you can change one file and update all pages at once.