<!-- FILE: 03_templates/03_static_files/03_organizing_assets.md -->

## Overview

As your Flask application grows, organizing static assets becomes critical. A well-structured `static/` folder makes it easy to find files, manage dependencies, and scale. This file covers best practices for organizing CSS, JavaScript, images, fonts, and other assets in professional Flask projects.

## Prerequisites

- Understanding of static file serving
- Basic knowledge of CSS and JavaScript

## Core Concepts

### Recommended Folder Structure

For larger projects:

```
static/
├── css/
│   ├── main.css          # Main styles
│   ├── components/       # Component styles
│   │   ├── buttons.css
│   │   ├── cards.css
│   │   └── forms.css
│   └── pages/            # Page-specific styles
│       ├── home.css
│       └── dashboard.css
├── js/
│   ├── main.js           # Main JavaScript
│   ├── components/       # Component scripts
│   │   ├── modal.js
│   │   └── dropdown.js
│   └── pages/            # Page-specific scripts
│       ├── home.js
│       └── dashboard.js
├── images/
│   ├── logo.png
│   ├── icons/
│   │   ├── edit.svg
│   │   └── delete.svg
│   └── photos/
├── fonts/
│   ├── Roboto/
│   │   ├── Roboto-Regular.woff2
│   │   └── Roboto-Bold.woff2
│   └── OpenSans/
└── vendor/               # Third-party libraries
    ├── bootstrap/
    │   └── css/
    │       └── bootstrap.min.css
    └── jquery/
        └── jquery.min.js
```

### Using a Build Tool

For production, consider using a build tool like:
- **Webpack** — Bundles and optimizes JS/CSS
- **Vite** — Modern, fast bundler
- **Parcel** — Zero-config bundler
- **Gulp** — Task runner

These tools can:
- Combine multiple files into one
- Minify (compress) CSS/JS
- Add vendor prefixes
- Handle imports and dependencies

## Code Walkthrough

### Flask with Organized Structure

```python
# app.py
from flask import Flask, render_template

app = Flask(__name__)

@app.route("/")
def index():
    return render_template("index.html")

@app.route("/dashboard")
def dashboard():
    return render_template("dashboard.html")
```

### Template with Organized Assets

```html
<!-- templates/index.html -->
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>Home</title>
    
    <!-- Vendor CSS -->
    <link rel="stylesheet" href="{{ url_for('static', filename='vendor/bootstrap/css/bootstrap.min.css') }}">
    
    <!-- Main CSS -->
    <link rel="stylesheet" href="{{ url_for('static', filename='css/main.css') }}">
    
    <!-- Page-specific CSS -->
    <link rel="stylesheet" href="{{ url_for('static', filename='css/pages/home.css') }}">
</head>
<body>
    <nav class="navbar">
        <img src="{{ url_for('static', filename='images/logo.png') }}" alt="Logo">
    </nav>
    
    <main>
        <h1>Welcome</h1>
    </main>
    
    <!-- Vendor JS -->
    <script src="{{ url_for('static', filename='vendor/jquery/jquery.min.js') }}"></script>
    <script src="{{ url_for('static', filename='vendor/bootstrap/js/bootstrap.min.js') }}"></script>
    
    <!-- Main JS -->
    <script src="{{ url_for('static', filename='js/main.js') }}"></script>
    
    <!-- Page-specific JS -->
    <script src="{{ url_for('static', filename='js/pages/home.js') }}"></script>
</body>
</html>
```

### Using Flask-Assets (Extension)

Flask-Assets helps manage assets:

```python
# app.py — Using Flask-Assets
from flask import Flask
from flask_assets import Bundle, Environment

app = Flask(__name__)

# Initialize Flask-Assets
assets = Environment(app)

# Define bundles
css = Bundle(
    'css/main.css',
    'css/components/buttons.css',
    'css/components/cards.css',
    output='gen/css/all.css'  # Combined output
)

js = Bundle(
    'js/main.js',
    'js/components/modal.js',
    output='gen/js/all.js'
)

# Register bundles
assets.register('css_all', css)
assets.register('js_all', js)
```

```html
<!-- Template using bundles -->
<link rel="stylesheet" href="{{ assets.url('css_all') }}">
<script src="{{ assets.url('js_all') }}"></script>
```

### Using Webpack Encore

For modern JavaScript development:

```python
# app.py — Using Webpack Encore
from flask import Flask, render_template
from flask_encapsule import Encapsule

app = Flask(__name__)
Encore(app, build_path='build')

# In template:
# {{ encore_entry_script_tags('main') }}
# {{ encore_entry_link_tags('main') }}
```

## Common Mistakes

❌ **All files in one folder**
```bash
# WRONG — Hard to manage
static/
├── style.css
├── app.js
├── logo.png
└── font.woff
```

✅ **Correct — Organized folders**
```bash
# CORRECT
static/
├── css/
├── js/
├── images/
└── fonts/
```

❌ **Not using CDN for vendor libraries**
```html
<!-- WRONG — Serving large vendor files from your server -->
<script src="{{ url_for('static', filename='vendor/jquery/jquery.min.js') }}">
```

✅ **Correct — Use CDN for common libraries**
```html
<!-- CORRECT — Use CDN for popular libraries -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js">
```

## Quick Reference

| Folder | Purpose |
|--------|---------|
| `css/` | Stylesheets |
| `js/` | JavaScript files |
| `images/` | Images and icons |
| `fonts/` | Web fonts |
| `vendor/` | Third-party libraries |

## Next Steps

You have completed the templates chapter. Continue to [01_form_basics.md](../../04_forms_and_validation/01_html_forms/01_form_basics.md) to learn about handling HTML forms in Flask.