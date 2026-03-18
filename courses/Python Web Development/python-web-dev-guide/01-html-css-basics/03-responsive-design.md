# Responsive Design

## What You'll Learn
- What responsive design means and why it matters
- Using the viewport meta tag
- Media queries for different screen sizes
- Fluid layouts with relative units
- Mobile-first design approach
- Common responsive patterns

## Prerequisites
- Completed HTML Fundamentals
- Completed CSS Fundamentals

## What Is Responsive Design?

**Responsive design** is an approach where websites adapt to different screen sizes. A responsive site looks good on:
- Desktop computers (1920px wide)
- Laptops (1366px wide)
- Tablets (768px wide)
- Mobile phones (375px wide)

Without responsive design, users on mobile would see tiny, unreadable text or have to scroll horizontally.

Think of responsive design like water — it adapts to fill whatever container it's in.

## The Viewport Meta Tag

The first step to responsive design is adding the viewport meta tag in your HTML:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>My Responsive Site</title>
</head>
```

🔍 **Line-by-Line Breakdown:**

1. `<meta name="viewport" content="width=device-width, initial-scale=1.0">` — This tells the browser to:
   - `width=device-width` — Match the page width to the device's screen width
   - `initial-scale=1.0` — Start at 100% zoom (not zoomed out)

Without this tag, mobile browsers would render the page as if on a desktop and then shrink it down, making text unreadable.

## Relative Units vs Absolute Units

### Absolute Units (Avoid for Responsive)

```css
/* These are fixed sizes - they don't adapt */
width: 960px;      /* Problem: Won't fit on small screens */
font-size: 18px;   /* May be too small on mobile */
```

### Relative Units (Use These)

```css
/* These adapt to context */
width: 90%;        /* Percentage - adapts to parent container */
width: 50vw;       /* Viewport width - 50% of screen width */
font-size: 1.5rem; /* Relative to root font size */
font-size: 1.2em;  /* Relative to parent font size */
```

🔍 **Unit Comparison:**

| Unit | What it means | Best for |
|------|---------------|----------|
| `px` | Pixels (fixed) | Borders, precise spacing |
| `%` | Percentage of parent | Widths, some layouts |
| `vw` | Percentage of viewport | Full-width hero sections |
| `vh` | Percentage of viewport | Full-screen sections |
| `rem` | Root font size multiplier | Font sizes (accessible) |
| `em` | Parent font size multiplier | Spacing relative to text |

### Practical Example

```css
/* Base font size */
html {
    font-size: 16px;
}

h1 {
    font-size: 2rem;    /* 32px (2 × 16) */
}

p {
    font-size: 1rem;     /* 16px */
}

small {
    font-size: 0.875rem; /* 14px (0.875 × 16) */
}
```

## Media Queries

**Media queries** apply different styles based on conditions like screen width.

### Basic Syntax

```css
/* Default styles (apply everywhere) */
body {
    background-color: white;
    color: black;
}

/* Apply when screen is 768px or wider (tablet/desktop) */
@media (min-width: 768px) {
    body {
        background-color: lightblue;
    }
}

/* Apply when screen is 1024px or wider (desktop) */
@media (min-width: 1024px) {
    body {
        background-color: lightgreen;
    }
}
```

### Min-Width vs Max-Width

```css
/* Mobile-first: Base styles are for mobile, add more for larger screens */
@media (min-width: 768px) {
    /* Tablet and up */
}

@media (min-width: 1024px) {
    /* Desktop and up */
}

/* Alternative: Desktop-first - base is desktop, override for smaller */
@media (max-width: 767px) {
    /* Tablet and below */
}
```

**Recommendation:** Use **mobile-first** (min-width) approach. It's easier to add features for larger screens than to remove features for smaller ones.

## Responsive Layout Patterns

### 1. Single Column (Mobile)

```css
.container {
    width: 100%;
    padding: 1rem;
}
```

### 2. Multi-Column (Desktop)

```css
@media (min-width: 768px) {
    .container {
        display: grid;
        grid-template-columns: 1fr 2fr;  /* Sidebar + Main */
        gap: 2rem;
        max-width: 1200px;
        margin: 0 auto;
        padding: 2rem;
    }
}
```

### Flexbox Wrapping

```css
.cards {
    display: flex;
    flex-wrap: wrap;
    gap: 1rem;
}

.card {
    flex: 1 1 300px;  /* grow, shrink, basis */
}
```

🔍 **flex: 1 1 300px breakdown:**

1. `1` — Can grow to fill available space
2. `1` — Can shrink if space is limited
3. `300px` — Start at 300px wide

With `flex-wrap: wrap`, cards will stack on mobile and arrange in rows on desktop.

### Auto-Fit Grid

```css
.grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
    gap: 1.5rem;
}
```

This creates a responsive grid that automatically adjusts columns based on available space. The `minmax(300px, 1fr)` means "at least 300px wide, but can grow to fill space."

## Responsive Images

### Max-Width Solution

```css
img {
    max-width: 100%;  /* Never exceed container width */
    height: auto;     /* Maintain aspect ratio */
}
```

### The Picture Element

```html
<picture>
    <!-- Image for small screens -->
    <source srcset="image-small.jpg" media="(max-width: 600px)">
    
    <!-- Image for medium screens -->
    <source srcset="image-medium.jpg" media="(max-width: 1200px)">
    
    <!-- Default image -->
    <img src="image-large.jpg" alt="Description">
</picture>
```

This loads different sized images based on the device, saving bandwidth on mobile.

## Complete Responsive Example

**index.html:**

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Responsive Portfolio</title>
    <link rel="stylesheet" href="styles.css">
</head>
<body>
    <header class="header">
        <div class="header-content">
            <h1>John Doe</h1>
            <nav class="nav">
                <a href="#home">Home</a>
                <a href="#projects">Projects</a>
                <a href="#contact">Contact</a>
            </nav>
        </div>
    </header>
    
    <main>
        <section class="hero" id="home">
            <h2>Python Web Developer</h2>
            <p>I build modern, responsive web applications</p>
        </section>
        
        <section class="projects" id="projects">
            <h2>My Projects</h2>
            <div class="project-grid">
                <article class="project-card">
                    <h3>Todo App</h3>
                    <p>Task management with Flask</p>
                </article>
                <article class="project-card">
                    <h3>Blog API</h3>
                    <p>REST API with FastAPI</p>
                </article>
                <article class="project-card">
                    <h3>Portfolio</h3>
                    <p>Responsive design showcase</p>
                </article>
            </div>
        </section>
        
        <section class="contact" id="contact">
            <h2>Contact Me</h2>
            <form class="contact-form">
                <input type="text" placeholder="Your Name" required>
                <input type="email" placeholder="Your Email" required>
                <textarea placeholder="Your Message" rows="5" required></textarea>
                <button type="submit">Send</button>
            </form>
        </section>
    </main>
    
    <footer class="footer">
        <p>&copy; 2024 John Doe</p>
    </footer>
</body>
</html>
```

**styles.css:**

```css
/* ===== Base Styles (Mobile First) ===== */
* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
}

html {
    font-size: 16px;
}

body {
    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
    line-height: 1.6;
    color: #333;
}

/* Header */
.header {
    background: #2c3e50;
    color: white;
    padding: 1rem;
    position: sticky;
    top: 0;
}

.header-content {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 1rem;
}

.nav {
    display: flex;
    gap: 1rem;
}

.nav a {
    color: white;
    text-decoration: none;
    font-size: 0.9rem;
}

.nav a:hover {
    text-decoration: underline;
}

/* Hero */
.hero {
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    color: white;
    text-align: center;
    padding: 3rem 1rem;
}

.hero h2 {
    font-size: 1.75rem;
    margin-bottom: 0.5rem;
}

/* Projects */
.projects {
    padding: 2rem 1rem;
    max-width: 1200px;
    margin: 0 auto;
}

.projects h2 {
    text-align: center;
    margin-bottom: 1.5rem;
}

.project-grid {
    display: grid;
    grid-template-columns: 1fr;
    gap: 1rem;
}

.project-card {
    background: #f9f9f9;
    border: 1px solid #ddd;
    border-radius: 8px;
    padding: 1.5rem;
}

/* Contact */
.contact {
    background: #f5f5f5;
    padding: 2rem 1rem;
    text-align: center;
}

.contact h2 {
    margin-bottom: 1rem;
}

.contact-form {
    display: flex;
    flex-direction: column;
    gap: 1rem;
    max-width: 500px;
    margin: 0 auto;
}

.contact-form input,
.contact-form textarea {
    padding: 0.75rem;
    border: 1px solid #ddd;
    border-radius: 4px;
    font-size: 1rem;
    width: 100%;
}

.contact-form button {
    padding: 0.75rem 1.5rem;
    background: #007bff;
    color: white;
    border: none;
    border-radius: 4px;
    font-size: 1rem;
    cursor: pointer;
}

.contact-form button:hover {
    background: #0056b3;
}

/* Footer */
.footer {
    background: #2c3e50;
    color: white;
    text-align: center;
    padding: 1rem;
    font-size: 0.875rem;
}

/* ===== Tablet (768px and up) ===== */
@media (min-width: 768px) {
    .header-content {
        flex-direction: row;
        justify-content: space-between;
        max-width: 1200px;
        margin: 0 auto;
    }
    
    .hero {
        padding: 5rem 2rem;
    }
    
    .hero h2 {
        font-size: 2.5rem;
    }
    
    .project-grid {
        grid-template-columns: repeat(2, 1fr);
    }
}

/* ===== Desktop (1024px and up) ===== */
@media (min-width: 1024px) {
    .project-grid {
        grid-template-columns: repeat(3, 1fr);
    }
    
    .projects, .contact {
        padding: 3rem 2rem;
    }
}
```

🔍 **Responsive Techniques Used:**

1. `max-width: 100%` on form inputs — Prevents inputs from overflowing on mobile
2. `flex-direction: column` on mobile, changes to `row` on tablet — Stack navigation on mobile, spread on desktop
3. `grid-template-columns: 1fr` on mobile, `repeat(2, 1fr)` on tablet, `repeat(3, 1fr)` on desktop — Progressive grid
4. `sticky` positioning — Header stays visible while scrolling
5. `viewport` meta tag — Essential for mobile rendering

## Testing Responsive Designs

### Browser DevTools

1. Open your page in Chrome
2. Press `F12` to open DevTools
3. Click the "Toggle device toolbar" icon (or press `Ctrl+Shift+M`)
4. Select different device sizes or drag to resize

### Online Tools

- [Google Mobile-Friendly Test](https://search.google.com/test/mobile-friendly)
- [Responsinator](https://www.responsinator.com/)

## Common Breakpoints

These are typical breakpoints, but you can adjust based on your design:

| Breakpoint | Width | Devices |
|------------|-------|---------|
| sm | 640px | Large phones |
| md | 768px | Tablets |
| lg | 1024px | Laptops |
| xl | 1280px | Desktops |

## Summary
- **Responsive design** adapts websites to different screen sizes
- Add `<meta name="viewport">` for proper mobile rendering
- Use **relative units** (`%`, `rem`, `vw`) instead of fixed pixels
- **Media queries** apply different styles at specific widths
- Use **mobile-first** approach (base styles for mobile, enhance for larger screens)
- Test with browser DevTools or online testing tools

## Next Steps
→ Continue to `../02-python-fundamentals/01-python-syntax-refresher.md` to refresh your Python skills.
