# Section 3: HTML & CSS Fundamentals

## Introduction to Web Development Basics

In this section, you'll learn the building blocks of every webpage. Think of HTML as the skeleton of a house and CSS as the paint and decorations.

### What You'll Learn

- HTML structure and elements
- CSS styling and layout
- Creating a product card component
- Responsive design basics

---

## 3.1 Understanding HTML

### What is HTML?

HTML (HyperText Markup Language) is the standard language for creating web pages. It provides the structure - the skeleton of your webpage.

### The Analogy

If your webpage were a human body:
- **HTML** = The bones and skeleton
- **CSS** = The skin, hair, and clothes
- **JavaScript** = The muscles that move the body

### Basic HTML Document Structure

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>My First Webpage</title>
</head>
<body>
    <!-- This is a comment - it won't be displayed -->
    <h1>Hello, World!</h1>
    <p>This is my first webpage.</p>
</body>
</html>
```

### Breaking Down the Structure

| Tag | Purpose |
|-----|---------|
| `<!DOCTYPE html>` | Tells browser this is HTML5 |
| `<html>` | Root element of the page |
| `<head>` | Contains metadata |
| `<meta>` | Information about the page |
| `<title>` | Browser tab title |
| `<body>` | Visible content |

---

## 3.2 Common HTML Elements

### Headings

HTML provides six levels of headings:

```html
<h1>Main Heading - Largest</h1>
<h2>Secondary Heading</h2>
<h3>Tertiary Heading</h3>
<h4>Smaller Heading</h4>
<h5>Even Smaller</h5>
<h6>Smallest Heading</h6>
```

### Paragraphs and Text

```html
<p>This is a paragraph of text.</p>
<strong>This text is bold/important</strong>
<em>This text is italicized</em>
<br> <!-- Line break -->
<hr> <!-- Horizontal rule -->
```

### Lists

**Unordered List (bullet points):**
```html
<ul>
    <li>First item</li>
    <li>Second item</li>
    <li>Third item</li>
</ul>
```

**Ordered List (numbered):**
```html
<ol>
    <li>First step</li>
    <li>Second step</li>
    <li>Third step</li>
</ol>
```

### Links

```html
<a href="https://google.com">Click here to visit Google</a>
<a href="about.html">Go to About page</a>
<a href="#section1">Jump to Section 1</a>
```

### Images

```html
<img src="image.jpg" alt="Description of image" width="300">
```

### Tables

```html
<table border="1">
    <tr>
        <th>Header 1</th>
        <th>Header 2</th>
    </tr>
    <tr>
        <td>Data 1</td>
        <td>Data 2</td>
    </tr>
</table>
```

---

## 3.3 Introduction to CSS

### What is CSS?

CSS (Cascading Style Sheets) controls how HTML elements look. It handles colors, fonts, spacing, layout, and more.

### Three Ways to Add CSS

**1. Inline Styles** (not recommended):
```html
<p style="color: blue; font-size: 20px;">Blue text</p>
```

**2. Internal CSS** (in the head):
```html
<head>
    <style>
        p { color: blue; font-size: 20px; }
    </style>
</head>
```

**3. External CSS** (separate file) - BEST PRACTICE:
```html
<head>
    <link rel="stylesheet" href="styles.css">
</head>
```

### CSS Syntax

```css
selector {
    property: value;
    another-property: another-value;
}
```

### Example:
```css
h1 {
    color: #333333;
    font-size: 32px;
    text-align: center;
}
```

---

## 3.4 CSS Selectors

### Element Selector
Selects all elements of a type:
```css
p {
    color: blue;
}
```

### Class Selector
Selects elements with a specific class (use .):
```css
.highlight {
    background-color: yellow;
}
```
Usage: `<p class="highlight">This is highlighted</p>`

### ID Selector
Selects a unique element (use #):
```css
#header {
    background-color: blue;
}
```
Usage: `<div id="header">Header content</div>`

### Combining Selectors

**Multiple elements:**
```css
h1, h2, h3 {
    font-family: Arial;
}
```

**Element with class:**
```css
p.important {
    font-weight: bold;
}
```

---

## 3.5 The Box Model

Every HTML element is a box with four parts:

```
┌─────────────────────────────────┐
│         MARGIN (outer)          │
│  ┌───────────────────────────┐  │
│  │        BORDER             │  │
│  │  ┌─────────────────────┐  │  │
│  │  │      PADDING        │  │  │
│  │  │  ┌───────────────┐  │  │  │
│  │  │  │    CONTENT    │  │  │  │
│  │  │  └───────────────┘  │  │  │
│  │  └─────────────────────┘  │  │
│  └───────────────────────────┘  │
└─────────────────────────────────┘
```

### Setting Box Model Properties

```css
.box {
    margin: 20px;      /* Space outside border */
    padding: 15px;     /* Space inside border */
    border: 2px solid black;
    width: 200px;
    height: 100px;
}
```

---

## 3.6 Flexbox Layout

Flexbox makes layout easy! It helps align and distribute space among items.

### Container Setup
```css
.container {
    display: flex;
    justify-content: center;  /* Horizontal alignment */
    align-items: center;     /* Vertical alignment */
    gap: 20px;               /* Space between items */
}
```

### Common Flexbox Properties

| Property | Values |
|----------|--------|
| display | flex |
| justify-content | center, flex-start, flex-end, space-between, space-around |
| align-items | center, flex-start, flex-end, stretch |
| flex-direction | row, column |
| flex-wrap | wrap, nowrap |

---

## 3.7 Creating a Product Card

Let's build a product card like what we'll use in our Angular app:

### HTML Structure
```html
<div class="product-card">
    <img src="product.jpg" alt="Product" class="product-image">
    <h3 class="product-name">Laptop Pro</h3>
    <p class="product-description">High-performance laptop</p>
    <p class="product-price">$1299.99</p>
    <button class="btn-add">Add to Cart</button>
</div>
```

### CSS Styling
```css
.product-card {
    width: 280px;
    border: 1px solid #ddd;
    border-radius: 8px;
    padding: 16px;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    transition: transform 0.3s;
}

.product-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 4px 8px rgba(0,0,0,0.15);
}

.product-image {
    width: 100%;
    height: 200px;
    object-fit: cover;
    border-radius: 4px;
}

.product-name {
    font-size: 18px;
    margin: 12px 0 8px;
    color: #333;
}

.product-price {
    font-size: 24px;
    font-weight: bold;
    color: #2ecc71;
    margin: 8px 0;
}

.btn-add {
    width: 100%;
    padding: 12px;
    background-color: #3498db;
    color: white;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    font-size: 16px;
}

.btn-add:hover {
    background-color: #2980b9;
}
```

---

## 3.8 Responsive Design

Make your site look good on all screen sizes!

### Using Media Queries

```css
/* Default styles (mobile first) */
.container {
    width: 100%;
    padding: 10px;
}

/* Tablet */
@media (min-width: 768px) {
    .container {
        width: 750px;
        margin: 0 auto;
    }
}

/* Desktop */
@media (min-width: 1024px) {
    .container {
        width: 960px;
        margin: 0 auto;
    }
}
```

### Mobile-First Approach
```css
/* Base styles for all screens */
.product-card {
    width: 100%;
}

/* Larger screens */
@media (min-width: 600px) {
    .product-card {
        width: 50%;
    }
}

@media (min-width: 900px) {
    .product-card {
        width: 33.33%;
    }
}
```

---

## 3.9 Best Practices

### HTML Best Practices
- Use semantic elements (`<header>`, `<nav>`, `<main>`, `<footer>`)
- Always add alt text to images
- Use proper heading hierarchy (h1 → h2 → h3)
- Close all tags properly
- Use lowercase for tags and attributes

### CSS Best Practices
- Use external CSS files
- Organize CSS logically
- Use comments to section code
- Use variables for colors
- Keep specificity low
- Use meaningful class names

---

## 3.10 Summary

### Key Takeaways

1. **HTML** provides structure (skeleton)
2. **CSS** provides styling (appearance)
3. **Elements** are the building blocks
4. **Selectors** target specific elements
5. **Box Model** controls spacing
6. **Flexbox** makes layout easy
7. **Responsive design** works on all devices

### What We Built

You now know how to create a styled product card - the foundation of our product management application!

### What's Next?

In the next section, we'll add interactivity with **JavaScript**, then **Bootstrap** for pre-built styling, and finally move to **TypeScript** and **Angular**.
