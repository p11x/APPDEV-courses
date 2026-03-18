<!-- FILE: 02_routing_and_views/03_view_functions/03_url_for.md -->

## Overview

The `url_for()` function is Flask's URL generator. Instead of hardcoding URLs in your templates and redirect calls, you use `url_for()` to dynamically generate URLs based on route names and parameters. This makes your application more maintainable — when you change a URL, you only update it in one place (the route definition), not everywhere it is referenced.

## Prerequisites

- Understanding of Flask routes
- Basic knowledge of templates
- Familiarity with redirects

## Core Concepts

### Why Use url_for()?

Hardcoded URLs create maintenance problems:
- Change a URL pattern → update every link manually
- Easy to miss some references
- Error-prone when URLs are complex

`url_for()` solves this by:
- Centralizing URL definitions in routes
- Automatically generating correct URLs
- Handling URL encoding automatically
- Working with dynamic route parameters

### How url_for() Works

You pass the **endpoint** (function name of the view) and any required **arguments**:

```python
url_for("profile", username="alice")  # Generates: /profile/alice
```

Flask matches the endpoint to its route and builds the URL with your parameters.

## Code Walkthrough

### url_for() Examples

```python
# url_for_demo.py — Comprehensive url_for() usage
from flask import Flask, url_for, redirect

app = Flask(__name__)

# 1. Basic route with url_for
@app.route("/")
def index():
    """Home page."""
    return """
    <h1>Home</h1>
    <nav>
        <a href="{{ url_for('about') }}">About</a>
        <a href="{{ url_for('contact') }}">Contact</a>
        <a href="{{ url_for('profile', username='alice') }}">Alice's Profile</a>
    </nav>
    """

@app.route("/about")
def about():
    """About page."""
    return "<h1>About</h1>"

@app.route("/contact")
def contact():
    """Contact page."""
    return "<h1>Contact</h1>"

# 2. Dynamic route parameters
@app.route("/profile/<username>")
def profile(username):
    """User profile page."""
    return f"<h1>Profile: {username}</h1>"

# 3. url_for with query parameters
@app.route("/search")
def search():
    """Search results page."""
    query = request.args.get("query", "")
    return f"<h1>Search Results</h1><p>Query: {query}</p>"

# Import request for the search route above
from flask import request

# 4. Using url_for in redirects
@app.route("/old-url")
def old_url():
    """Redirect old URL to new location using url_for."""
    return redirect(url_for("new_url"))

@app.route("/new-url")
def new_url():
    """New URL."""
    return "<h1>New URL</h1>"

# 5. url_for with anchor tags (fragments)
@app.route("/page")
def page():
    """Page with anchor links."""
    return """
    <h1>Page</h1>
    <a href="{{ url_for('page', _anchor='section2') }}">Go to Section 2</a>
    <h2 id="section2">Section 2</h2>
    """

# 6. url_for for static files
@app.route("/uses-static")
def uses_static():
    """Demonstrate url_for for static files."""
    return """
    <h1>Static Files Demo</h1>
    <img src="{{ url_for('static', filename='images/logo.png') }}" alt="Logo">
    <link rel="stylesheet" href="{{ url_for('static', filename='css/style.css') }}">
    <script src="{{ url_for('static', filename='js/app.js') }}"></script>
    """

# 7. url_for with external=True for full URLs
@app.route("/external-demo")
def external_demo():
    """Generate full URL with scheme and domain."""
    # Generate absolute URL (useful for emails, APIs)
    home_url = url_for("index", _external=True)  # http://localhost:5000/
    profile_url = url_for("profile", username="bob", _external=True)
    return f"""
    <h1>External URLs</h1>
    <p>Home: {home_url}</p>
    <p>Profile: {profile_url}</p>
    """

# 8. Using url_for in Python code (not templates)
@app.route("/generate-link")
def generate_link():
    """url_for in view function code."""
    # Generate URL for profile page
    profile_url = url_for("profile", username="testuser")
    search_url = url_for("search", query="flask", page=1)
    return f"""
    <h1>Generated URLs</h1>
    <p>Profile: {profile_url}</p>
    <p>Search: {search_url}</p>
    """

# 9. Building URLs with url_for and template inheritance
# In templates, url_for is available automatically
@app.route("/template-demo")
def template_demo():
    """Demonstrate url_for in template inheritance."""
    from flask import render_template_string
    
    template = """
    <!DOCTYPE html>
    <html>
    <head><title>{{ title }}</title></head>
    <body>
        <nav>
            <a href="{{ url_for('index') }}">Home</a>
            <a href="{{ url_for('about') }}">About</a>
            <a href="{{ url_for('contact') }}">Contact</a>
        </nav>
        <main>
            {% block content %}{% endblock %}
        </main>
    </body>
    </html>
    """
    return render_template_string(template, title="Demo")

if __name__ == "__main__":
    app.run(debug=True)
```

### Line-by-Line Breakdown

- `url_for("about")` — Generates URL for the "about" route (function name).
- `url_for("profile", username="alice")` — Passes dynamic parameter; generates `/profile/alice`.
- `url_for("search", query="flask")` — Adds query string parameter; generates `/search?query=flask`.
- `url_for("page", _anchor="section2")` — Adds anchor/fragment; generates `/page#section2`.
- `url_for("static", filename="css/style.css")` — Generates URL for static file.
- `url_for("index", _external=True)` — Generates full URL with domain: `http://localhost:5000/`.
- `redirect(url_for("new_url"))` — Uses url_for in redirect (very common pattern).

### Testing

```bash
# Basic URL generation
curl http://127.0.0.1:5000/generate-link
# <p>Profile: /profile/testuser</p>
# <p>Search: /search?query=flask&page=1</p>

# External URL
curl http://127.0.0.1:5000/external-demo
# <p>Home: http://localhost:5000/</p>
# <p>Profile: http://localhost:5000/profile/bob</p>
```

## Common Mistakes

❌ **Hardcoding URLs in templates**
```html
<!-- WRONG — Hardcoded URL breaks if route changes -->
<a href="/about">About</a>
```

✅ **Correct — Use url_for**
```html
<!-- CORRECT — URL updates automatically if route changes -->
<a href="{{ url_for('about') }}">About</a>
```

❌ **Using wrong endpoint name**
```python
# WRONG — Endpoint must match function name exactly
url_for("about_page")  # No route named "about_page"
```

✅ **Correct — Use function name as endpoint**
```python
# CORRECT — Use the actual function name
url_for("about")  # Matches @app.route("/about") def about():
```

❌ **Forgetting required parameters**
```python
# WRONG — profile route requires username parameter
url_for("profile")  # Raises BuildError
```

✅ **Correct — Provide all required parameters**
```python
# CORRECT — Provide the required parameter
url_for("profile", username="alice")  # Works: /profile/alice
```

## Quick Reference

| Usage | Code | Result |
|-------|------|--------|
| Basic | `url_for("about")` | `/about` |
| With parameter | `url_for("profile", username="bob")` | `/profile/bob` |
| Query string | `url_for("search", q="flask")` | `/search?q=flask` |
| Anchor | `url_for("page", _anchor="section")` | `/page#section` |
| Static file | `url_for("static", filename="css/style.css")` | `/static/css/style.css` |
| External | `url_for("index", _external=True)` | `http://localhost:5000/` |

## Next Steps

You have completed the routing and views chapter. This completes the fundamentals of handling requests and responses. Continue to [01_rendering_templates.md](../../03_templates/01_jinja2_basics/01_rendering_templates.md) to learn about templates — how to create dynamic HTML pages using Jinja2.