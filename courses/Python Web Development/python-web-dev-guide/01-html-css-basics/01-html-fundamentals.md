# HTML Fundamentals

## What You'll Learn
- What HTML is and how it works
- The basic structure of an HTML document
- Common HTML elements (headings, paragraphs, links, images)
- How to create forms for user input
- Semantic HTML elements

## Prerequisites
- Completed the introduction section
- A text editor (VS Code recommended)

## What Is HTML?

**HTML** stands for HyperText Markup Language. It's the standard language for creating web pages. Think of HTML as the skeleton of a webpage — it provides the structure and content.

When you visit a website, your browser (Chrome, Firefox, Safari) reads the HTML and displays it as the visual page you see.

## A Simple HTML Document

Every HTML document follows a basic structure:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>My First Webpage</title>
</head>
<body>
    <h1>Hello, World!</h1>
    <p>Welcome to my first webpage.</p>
</body>
</html>
```

🔍 **Line-by-Line Breakdown:**

1. `<!DOCTYPE html>` — Tells the browser this is an HTML5 document. This must be the very first line.
2. `<html lang="en">` — The root element that wraps all content. `lang="en"` declares the language as English.
3. `<head>` — Contains metadata (information about the page, not visible content)
4. `<meta charset="UTF-8">` — Sets character encoding to UTF-8 (supports all languages and special characters)
5. `<meta name="viewport" content="width=device-width, initial-scale=1.0">` — Makes the page responsive on mobile devices
6. `<title>My First Webpage</title>` — The page title shown in browser tabs
7. `<body>` — Contains all visible content
8. `<h1>` — Heading element (largest heading)
9. `<p>` — Paragraph element for text

## Headings

HTML provides six levels of headings, `<h1>` through `<h6>`:

```html
<h1>Main Heading</h1>
<h2>Section Heading</h2>
<h3>Subsection Heading</h3>
<h4>Sub-subsection Heading</h4>
<h5>Minor Heading</h5>
<h6>Smallest Heading</h6>
```

Use headings hierarchically — `<h1>` for the main title, `<h2>` for major sections, and so on. Search engines use headings to understand your page structure.

## Paragraphs and Text Formatting

```html
<p>This is a paragraph of text. It can contain multiple sentences.</p>

<p>You can make text <strong>bold</strong> or <em>italic</em>.</p>

<p>You can also use:</p>
<ul>
    <li><strong>bold</strong> with <strong></li>
    <li><em>italic</em> with <em></li>
    <li><u>underlined</u> with <u></li>
    <li><mark>highlighted</mark> with <mark></li>
</ul>
```

🔍 **Line-by-Line Breakdown:**

1. `<p>` — Paragraph element for blocks of text
2. `<strong>` — Makes text bold and indicates importance (screen readers emphasize it)
3. `<em>` — Makes text italic and indicates emphasis
4. `<ul>` — Unordered list (bullets)
5. `<li>` — List item
6. `<mark>` — Highlights text (like a marker pen)

## Links (Anchor Tags)

Links are what make the web interconnected. They allow users to navigate between pages:

```html
<!-- Link to another website -->
<a href="https://www.google.com">Visit Google</a>

<!-- Link to a local HTML file -->
<a href="about.html">About Me</a>

<!-- Link that opens in a new tab -->
<a href="https://www.python.org" target="_blank">Python Website (opens new tab)</a>

<!-- Link to a specific section on the same page -->
<a href="#section-id">Jump to Section</a>
<h2 id="section-id">This is the section</h2>
```

🔍 **Line-by-Line Breakdown:**

1. `<a href="URL">` — Anchor tag with `href` (hypertext reference) specifying the destination
2. `target="_blank"` — Opens the link in a new browser tab
3. `id="section-id"` — Creates an identifier for linking to a specific section

## Images

Images make web pages engaging. The `<img>` tag is self-closing:

```html
<!-- Basic image -->
<img src="photo.jpg" alt="A description of the photo">

<!-- Image with dimensions -->
<img src="photo.jpg" alt="A sunset over mountains" width="600" height="400">

<!-- Image from the internet -->
<img src="https://example.com/image.png" alt="Example image">
```

🔍 **Line-by-Line Breakdown:**

1. `<img>` — Self-closing image element (no closing tag needed)
2. `src="photo.jpg"` — The source URL or file path to the image
3. `alt="description"` — Alternative text shown if image fails to load; crucial for accessibility
4. `width` and `height` — Specify dimensions in pixels

## Lists

### Unordered List (Bullets)

```html
<ul>
    <li>Apples</li>
    <li>Bananas</li>
    <li>Oranges</li>
</ul>
```

### Ordered List (Numbers)

```html
<ol>
    <li>First step</li>
    <li>Second step</li>
    <li>Third step</li>
</ol>
```

## Forms

Forms allow users to submit data to a server. This is essential for:

- Contact forms
- Login/signup
- Search boxes
- Surveys

```html
<form action="/submit" method="POST">
    <label for="name">Name:</label>
    <input type="text" id="name" name="name" required>
    
    <label for="email">Email:</label>
    <input type="email" id="email" name="email" required>
    
    <label for="message">Message:</label>
    <textarea id="message" name="message" rows="4"></textarea>
    
    <button type="submit">Send Message</button>
</form>
```

🔍 **Line-by-Line Breakdown:**

1. `<form>` — Container for form elements
2. `action="/submit"` — The URL where form data will be sent
3. `method="POST"` — HTTP method for sending data (POST is more secure for sensitive data)
4. `<label for="name">` — Label associated with an input; `for` matches the input's `id`
5. `<input type="text">` — Text input field; other types: `email`, `password`, `checkbox`, `radio`
6. `id="name"` — Unique identifier for linking with `<label>`
7. `name="name"` — Key used when sending form data
8. `required` — Makes the field mandatory
9. `<textarea>` — Multi-line text input with `rows` specifying visible lines
10. `<button type="submit">` — Button that submits the form

### Common Input Types

```html
<input type="text">        <!-- Single-line text -->
<input type="email">       <!-- Email with validation -->
<input type="password">    <!-- Hidden characters -->
<input type="number">      <!-- Numbers only -->
<input type="date">        <!-- Date picker -->
<input type="checkbox">   <!-- On/off checkbox -->
<input type="radio">       <!-- Single choice from multiple options -->
<input type="file">        <!-- File upload -->
```

## Semantic HTML

**Semantic HTML** uses elements that describe their meaning. Instead of using `<div>` for everything, use elements that convey purpose:

```html
<!-- Non-semantic (bad) -->
<div class="header">Welcome</div>
<div class="nav">
    <div><a href="/">Home</a></div>
    <div><a href="/about">About</a></div>
</div>
<div class="main-content">
    <div class="article">
        <div class="article-title">My Post</div>
        <div class="article-content">Content here</div>
    </div>
</div>
<div class="footer">Copyright 2024</div>

<!-- Semantic (good) -->
<header>Welcome</header>
<nav>
    <a href="/">Home</a>
    <a href="/about">About</a>
</nav>
<main>
    <article>
        <h1>My Post</h1>
        <p>Content here</p>
    </article>
</main>
<footer>Copyright 2024</footer>
```

🔍 **Semantic Elements:**

1. `<header>` — Introductory content or navigation links
2. `<nav>` — Navigation links
3. `<main>` — The main content area
4. `<article>` — Self-contained content (like a blog post)
5. `<section>` — Thematic grouping of content
6. `<aside>` — Sidebar content
7. `<footer>` — Footer content

Semantic HTML improves:
- **Accessibility** — Screen readers understand page structure
- **SEO** — Search engines understand content importance
- **Maintainability** — Code is easier to understand

## HTML Entities

Some characters have special meaning in HTML. Use entities to display them:

```html
<p>Use & for ampersand</p>
<p>Use < for less than</p>
<p>Use > for greater than</p>
<p>Use " for quotes</p>
<p>Use &nbsp; for non-breaking space</p>
```

## Complete Example

Here's a more complete HTML page:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>My Portfolio</title>
</head>
<body>
    <header>
        <h1>John Doe</h1>
        <nav>
            <a href="#about">About</a>
            <a href="#projects">Projects</a>
            <a href="#contact">Contact</a>
        </nav>
    </header>
    
    <main>
        <section id="about">
            <h2>About Me</h2>
            <p>I'm a <strong>Python web developer</strong> passionate about building <em>modern</em> web applications.</p>
        </section>
        
        <section id="projects">
            <h2>My Projects</h2>
            <ul>
                <li>
                    <h3>Todo App</h3>
                    <p>A task management application built with Flask.</p>
                </li>
                <li>
                    <h3>Blog API</h3>
                    <p>A RESTful API built with FastAPI.</p>
                </li>
            </ul>
        </section>
        
        <section id="contact">
            <h2>Contact Me</h2>
            <form action="/contact" method="POST">
                <div>
                    <label for="name">Name:</label>
                    <input type="text" id="name" name="name" required>
                </div>
                <div>
                    <label for="email">Email:</label>
                    <input type="email" id="email" name="email" required>
                </div>
                <div>
                    <label for="message">Message:</label>
                    <textarea id="message" name="message" rows="5" required></textarea>
                </div>
                <button type="submit">Send</button>
            </form>
        </section>
    </main>
    
    <footer>
        <p>&copy; 2024 John Doe. Built with HTML.</p>
    </footer>
</body>
</html>
```

## Summary
- HTML provides the structure of web pages
- Every document needs `<!DOCTYPE html>`, `<html>`, `<head>`, and `<body>`
- Use headings (`<h1>`-`<h6>`) hierarchically
- `<a>` creates links, `<img>` displays images
- Forms with `<input>`, `<textarea>`, and `<button>` collect user data
- Use **semantic HTML** for accessibility and SEO

## Next Steps
→ Continue to `02-css-fundamentals.md` to learn how to style your HTML with CSS.
