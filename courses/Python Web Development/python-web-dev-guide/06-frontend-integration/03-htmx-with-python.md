# HTMX with Python

## What You'll Learn
- What HTMX is
- Using HTMX with Flask/FastAPI
- Dynamic page updates without JavaScript

## Prerequisites
- Completed Flask or FastAPI basics

## What Is HTMX?

**HTMX** is a library that lets you use AJAX directly in HTML. It enables dynamic pages without writing JavaScript.

```html
<script src="https://unpkg.com/htmx.org"></script>

<button hx-get="/api/data" hx-trigger="click">
    Load Data
</button>
```

## HTMX with Flask

```python
from flask import Flask, render_template, jsonify

app = Flask(__name__)

@app.route("/")
def index():
    return render_template("index.html")

@app.route("/clicked")
def clicked():
    return "<span>You clicked!</span>"

# index.html
"""
<button hx-get="/clicked" hx-target="#result">
    Click Me
</button>
<div id="result"></div>
"""
```

## Summary
- HTMX adds AJAX to HTML
- Use hx-get, hx-post, hx-target attributes
- Server returns HTML fragments, not full pages
