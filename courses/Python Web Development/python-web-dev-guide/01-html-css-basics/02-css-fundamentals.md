# CSS Fundamentals

## What You'll Learn
- What CSS is and how it works
- Selecting HTML elements to style
- The box model (margin, padding, border)
- Colors, fonts, and text styling
- Layout with Flexbox and Grid
- Common CSS properties and values

## Prerequisites
- Completed HTML Fundamentals
- Understanding of basic HTML elements

## What Is CSS?

**CSS** stands for Cascading Style Sheets. While HTML provides the structure (skeleton), CSS provides the visual presentation (skin, clothes, makeup).

Think of it this way:
- HTML = The frame of a house
- CSS = The paint, curtains, furniture, and decoration

## Three Ways to Add CSS

### 1. Inline Styles (Not Recommended)

Add styles directly in HTML elements:

```html
<p style="color: blue; font-size: 18px;">Blue text</p>
```

**Why not?** Mixes content with presentation, hard to maintain.

### 2. Internal Stylesheet (Okay for Small Pages)

Add CSS in a `<style>` tag in the `<head>`:

```html
<!DOCTYPE html>
<html>
<head>
    <style>
        p {
            color: blue;
            font-size: 18px;
        }
    </style>
</head>
<body>
    <p>Blue text</p>
</body>
</html>
```

### 3. External Stylesheet (Recommended)

Create a separate `.css` file and link it:

**index.html:**
```html
<!DOCTYPE html>
<html>
<head>
    <link rel="stylesheet" href="styles.css">
</head>
<body>
    <p>Blue text</p>
</body>
</html>
```

**styles.css:**
```css
p {
    color: blue;
    font-size: 18px;
}
```

## CSS Selectors

**Selectors** target HTML elements to apply styles.

### Element Selector

```css
p {
    color: blue;
}
```

Targets all `<p>` elements.

### Class Selector

```html
<p class="highlight">This is highlighted</p>
<p>This is normal</p>
```

```css
.highlight {
    background-color: yellow;
}
```

Classes can be reused on multiple elements.

### ID Selector

```html
<p id="special">This is special</p>
```

```css
#special {
    font-weight: bold;
}
```

IDs are unique — should only appear once per page.

### Universal Selector

```css
* {
    margin: 0;
    padding: 0;
}
```

Targets all elements.

### Combination Selectors

```css
/* Descendant: any p inside article */
article p {
    line-height: 1.6;
}

/* Child: only direct p children of article */
article > p {
    margin-bottom: 1em;
}

/* Multiple: h1 and h2 */
h1, h2 {
    color: #333;
}
```

## The Box Model

Every HTML element is a box. The box model describes how sizing works:

```
┌─────────────────────────────────────┐
│             MARGIN                  │  ← Space outside the border
│  ┌─────────────────────────────────┐│
│  │           BORDER                ││  ← Border around padding
│  │  ┌─────────────────────────────┐││
│  │  │         PADDING             │││  ← Space inside the border
│  │  │  ┌───────────────────────┐   │││
│  │  │  │     CONTENT          │   │││  ← The actual content
│  │  │  │   (text, image)      │   │││
│  │  │  └───────────────────────┘   │││
│  │  └─────────────────────────────┘││
│  └─────────────────────────────────┘│
└─────────────────────────────────────┘
```

```css
.box {
    /* Space outside the element */
    margin: 20px;
    
    /* Space inside the border */
    padding: 15px;
    
    /* Border around the element */
    border: 2px solid #333;
    
    /* Actual content size */
    width: 200px;
    height: 100px;
}
```

🔍 **Line-by-Line Breakdown:**

1. `margin` — Space between this element and others. Can use `margin-top`, `margin-right`, `margin-bottom`, `margin-left`, or shorthand.
2. `padding` — Space between the border and the content inside. Similar shorthand as margin.
3. `border` — Line around the padding. Format: `width style color`. Styles: `solid`, `dashed`, `dotted`.
4. `width` and `height` — Content dimensions. Does NOT include margin, padding, or border.

### Margin Collapsing

Vertical margins between elements collapse to the larger value:

```
Element A: margin-bottom: 20px
Element B: margin-top: 10px
Actual space between: 20px (not 30px)
```

## Colors

### Named Colors

```css
p { color: red; }
h1 { color: royalblue; }
```

### Hexadecimal

```css
p { color: #ff0000; }      /* Red */
h1 { color: #007bff; }    /* Blue */
```

### RGB and RGBA

```css
p { color: rgb(255, 0, 0); }           /* Red */
h1 { color: rgba(0, 123, 255, 0.5); }  /* Semi-transparent blue */
```

### HSL

```css
p { color: hsl(0, 100%, 50%); }  /* Red - hue, saturation, lightness */
```

## Typography

```css
body {
    /* Font family - try multiple for fallback */
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    
    /* Font size */
    font-size: 16px;
    
    /* Line height for readability */
    line-height: 1.6;
    
    /* Text alignment */
    text-align: left;
}

h1 {
    font-size: 2.5rem;
    font-weight: bold;
    text-transform: uppercase;
}
```

🔍 **Properties:**

1. `font-family` — The typeface. List multiple separated by commas as fallbacks if the user doesn't have the font.
2. `font-size` — Text size. Can use `px`, `rem`, `em`, or `%`.
3. `line-height` — Space between lines. Unitless values (like 1.6) multiply the font size.
4. `text-align` — Horizontal alignment: `left`, `right`, `center`, `justify`.
5. `font-weight` — Boldness: `normal`, `bold`, or numeric (100-900).
6. `text-transform` — `uppercase`, `lowercase`, `capitalize`.

## Flexbox Layout

**Flexbox** is a one-dimensional layout method for arranging items in rows or columns.

```html
<div class="container">
    <div class="item">Item 1</div>
    <div class="item">Item 2</div>
    <div class="item">Item 3</div>
</div>
```

```css
.container {
    display: flex;
    
    /* Main axis (row) alignment */
    justify-content: space-between;
    
    /* Cross axis (column) alignment */
    align-items: center;
    
    /* Gap between items */
    gap: 20px;
}

.item {
    padding: 20px;
    background-color: #f0f0f0;
}
```

🔍 **Flexbox Properties:**

1. `display: flex` — Turns the container into a flex container
2. `justify-content` — Aligns items along the main axis (horizontal for rows):
   - `flex-start` — Left
   - `flex-end` — Right
   - `center` — Center
   - `space-between` — Equal space between
   - `space-around` — Equal space around
3. `align-items` — Aligns items along the cross axis (vertical for rows):
   - `flex-start`, `flex-end`, `center`, `stretch`
4. `gap` — Space between flex items

### Flex Direction

```css
.container {
    display: flex;
    flex-direction: column;  /* Stack vertically instead of horizontally */
}
```

### Flex Items Can Grow

```css
.item {
    flex: 1;  /* All items share equal space */
}

.item:first-child {
    flex: 2;  /* First item takes double space */
}
```

## CSS Grid Layout

**CSS Grid** is a two-dimensional layout system for rows and columns together.

```html
<div class="grid-container">
    <div class="header">Header</div>
    <div class="sidebar">Sidebar</div>
    <div class="main">Main Content</div>
    <div class="footer">Footer</div>
</div>
```

```css
.grid-container {
    display: grid;
    
    /* Define columns: 200px sidebar, rest is auto */
    grid-template-columns: 200px 1fr;
    
    /* Define rows */
    grid-template-rows: auto 1fr auto;
    
    /* Gap between items */
    gap: 20px;
}

.header, .footer {
    grid-column: 1 / -1;  /* Span full width */
}
```

🔍 **Grid Properties:**

1. `display: grid` — Turns the container into a grid container
2. `grid-template-columns` — Defines column sizes:
   - `200px` — Fixed width
   - `1fr` — Fraction of available space
   - `repeat(3, 1fr)` — Three equal columns
3. `grid-template-rows` — Defines row sizes
4. `gap` — Space between rows and columns
5. `grid-column: 1 / -1` — Item spans from line 1 to the end

## Common CSS Techniques

### Centering Elements

**Horizontal center:**
```css
.container {
    text-align: center;
}
```

**Center with Flexbox:**
```css
.container {
    display: flex;
    justify-content: center;
    align-items: center;
}
```

**Center with Grid:**
```css
.container {
    display: grid;
    place-items: center;
}
```

### Responsive Design with Media Queries

```css
/* Base styles (mobile first) */
.container {
    width: 100%;
    padding: 10px;
}

/* Tablet and up */
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
    }
}
```

### Hover Effects

```css
.button {
    background-color: #007bff;
    color: white;
    padding: 10px 20px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    transition: background-color 0.3s;
}

.button:hover {
    background-color: #0056b3;
}
```

🔍 **Hover Properties:**

1. `cursor: pointer` — Changes cursor to hand on hover
2. `transition` — Animates the change over time (0.3 seconds)
3. `:hover` — Pseudo-class that activates when mouse hovers over element

## Complete Example

```html
<!DOCTYPE html>
<html>
<head>
    <link rel="stylesheet" href="styles.css">
</head>
<body>
    <header class="site-header">
        <h1>My Portfolio</h1>
        <nav>
            <a href="#home">Home</a>
            <a href="#projects">Projects</a>
            <a href="#contact">Contact</a>
        </nav>
    </header>
    
    <main>
        <section class="hero">
            <h2>Welcome to My Site</h2>
            <p>I build modern web applications with Python</p>
        </section>
        
        <section class="projects">
            <article class="project-card">
                <h3>Todo App</h3>
                <p>A task management app built with Flask</p>
            </article>
            <article class="project-card">
                <h3>Blog API</h3>
                <p>A REST API built with FastAPI</p>
            </article>
        </section>
    </main>
    
    <footer class="site-footer">
        <p>&copy; 2024 My Portfolio</p>
    </footer>
</body>
</html>
```

**styles.css:**

```css
/* Reset */
* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
}

body {
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    line-height: 1.6;
    color: #333;
}

/* Header */
.site-header {
    background-color: #2c3e50;
    color: white;
    padding: 1rem 2rem;
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.site-header nav a {
    color: white;
    text-decoration: none;
    margin-left: 1rem;
}

.site-header nav a:hover {
    text-decoration: underline;
}

/* Hero Section */
.hero {
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    color: white;
    text-align: center;
    padding: 4rem 2rem;
}

.hero h2 {
    font-size: 2.5rem;
    margin-bottom: 1rem;
}

/* Projects */
.projects {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
    gap: 2rem;
    padding: 2rem;
    max-width: 1200px;
    margin: 0 auto;
}

.project-card {
    background: white;
    border: 1px solid #ddd;
    border-radius: 8px;
    padding: 1.5rem;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    transition: transform 0.2s;
}

.project-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 4px 8px rgba(0,0,0,0.15);
}

.project-card h3 {
    color: #2c3e50;
    margin-bottom: 0.5rem;
}

/* Footer */
.site-footer {
    background-color: #2c3e50;
    color: white;
    text-align: center;
    padding: 1rem;
    margin-top: 2rem;
}
```

## Summary
- CSS styles HTML elements for visual presentation
- Use **external stylesheets** for maintainability
- Use **selectors** (element, class, ID) to target elements
- The **box model** defines margin, border, padding, and content
- **Flexbox** is for one-dimensional layouts (row OR column)
- **CSS Grid** is for two-dimensional layouts (rows AND columns)
- Use **media queries** for responsive design

## Next Steps
→ Continue to `03-responsive-design.md` to learn how to make your websites work on all devices.
