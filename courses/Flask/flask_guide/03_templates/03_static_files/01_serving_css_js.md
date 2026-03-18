<!-- FILE: 03_templates/03_static_files/01_serving_css_js.md -->

## Overview

**Static files** are files that don't change: CSS stylesheets, JavaScript files, images, and fonts. Flask automatically serves files from a `static/` folder. This file teaches you how to organize and serve static assets in your Flask application, making your web pages look and behave professionally.

## Prerequisites

- Basic Flask application setup
- Understanding of templates
- Familiarity with HTML

## Core Concepts

### Static Folder

Flask looks for static files in a `static/` folder:

```
my_app/
├── app.py
├── templates/
│   └── index.html
└── static/
    ├── css/
    │   └── style.css
    ├── js/
    │   └── main.js
    └── images/
        └── logo.png
```

### Serving Static Files

Use `url_for('static', filename='path')` in templates:

```html
<link rel="stylesheet" href="{{ url_for('static', filename='css/style.css') }}">
<script src="{{ url_for('static', filename='js/main.js') }}"></script>
```

## Code Walkthrough

### Project Structure

```
static_demo/
├── app.py
├── templates/
│   └── index.html
└── static/
    ├── css/
    │   └── style.css
    └── js/
        └── main.js
```

### Flask Application

```python
# app.py — Serving static files
from flask import Flask, render_template

app = Flask(__name__)

@app.route("/")
def index():
    """Render the home page with static files."""
    return render_template("index.html")

@app.route("/about")
def about():
    """About page."""
    return render_template("about.html")

if __name__ == "__main__":
    app.run(debug=True)
```

### HTML Template with Static Files

```html
<!-- templates/index.html -->
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Static Files Demo</title>
    
    <!-- CSS: Use url_for() to generate the URL -->
    <link rel="stylesheet" href="{{ url_for('static', filename='css/style.css') }}">
</head>
<body>
    <header>
        <!-- Image from static folder -->
        <img src="{{ url_for('static', filename='images/logo.png') }}" alt="Logo" class="logo">
        <nav>
            <a href="{{ url_for('index') }}">Home</a>
            <a href="{{ url_for('about') }}">About</a>
        </nav>
    </header>
    
    <main>
        <h1>Welcome!</h1>
        <p>This page demonstrates static file serving.</p>
        
        <div class="card">
            <h2>Card Title</h2>
            <p>Card content goes here.</p>
        </div>
        
        <button id="demo-btn" class="btn-primary">Click Me</button>
    </main>
    
    <footer>
        <!-- JavaScript at the end for faster page loading -->
        <script src="{{ url_for('static', filename='js/main.js') }}"></script>
    </footer>
</body>
</html>
```

### CSS File

```css
/* static/css/style.css */
body {
    font-family: Arial, sans-serif;
    margin: 0;
    padding: 0;
    background-color: #f5f5f5;
}

header {
    background-color: #333;
    color: white;
    padding: 1rem;
    display: flex;
    align-items: center;
    justify-content: space-between;
}

.logo {
    height: 50px;
    width: auto;
}

nav a {
    color: white;
    text-decoration: none;
    margin-left: 15px;
}

nav a:hover {
    text-decoration: underline;
}

main {
    padding: 2rem;
    max-width: 800px;
    margin: 0 auto;
}

.card {
    background: white;
    padding: 1.5rem;
    border-radius: 8px;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    margin: 1rem 0;
}

.btn-primary {
    background-color: #007bff;
    color: white;
    border: none;
    padding: 0.5rem 1rem;
    border-radius: 4px;
    cursor: pointer;
}

.btn-primary:hover {
    background-color: #0056b3;
}
```

### JavaScript File

```javascript
// static/js/main.js
// Document ready function
document.addEventListener('DOMContentLoaded', function() {
    // Get the button element
    const button = document.getElementById('demo-btn');
    
    // Add click event listener
    button.addEventListener('click', function() {
        // Change button text
        this.textContent = 'Clicked!';
        this.classList.add('clicked');
        
        // Show alert after a delay
        setTimeout(function() {
            alert('Button was clicked!');
        }, 100);
    });
});
```

### Custom Static Folder Location

If you need a different static folder:

```python
# Custom static folder
app = Flask(__name__,
    static_folder='assets',        # Use 'assets' instead of 'static'
    static_url_path='/assets')    # Access at /assets/...
```

## Common Mistakes

❌ **Hardcoding static URLs**
```html
<!-- WRONG — Don't hardcode paths -->
<link href="/static/css/style.css">
```

✅ **Correct — Use url_for**
```html
<!-- CORRECT — Let Flask generate the URL -->
<link href="{{ url_for('static', filename='css/style.css') }}">
```

❌ **Placing static files in wrong location**
```bash
# WRONG — Flask won't find them
my_app/
├── app.py
└── static_files/
    └── style.css
```

✅ **Correct — Use static folder**
```bash
# CORRECT
my_app/
├── app.py
└── static/
    └── style.css
```

## Quick Reference

| Task | Code |
|------|------|
| Link CSS | `<link rel="stylesheet" href="{{ url_for('static', filename='css/style.css') }}">` |
| Add JS | `<script src="{{ url_for('static', filename='js/app.js') }}"></script>` |
| Add image | `<img src="{{ url_for('static', filename='images/logo.png') }}">` |

## Next Steps

Now you can serve static files. Continue to [02_url_for_static.md](02_url_for_static.md) to learn more about the url_for function for static files and advanced options.