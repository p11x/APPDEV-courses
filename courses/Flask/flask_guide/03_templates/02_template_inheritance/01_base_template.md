<!-- FILE: 03_templates/02_template_inheritance/01_base_template.md -->

## Overview

**Template inheritance** is one of Jinja2's most powerful features. It lets you create a base template with the common structure (header, footer, navigation) and have child templates extend it, filling in only the unique content. This eliminates code duplication and makes maintaining consistent layouts across many pages easy.

## Prerequisites

- Understanding of Jinja2 templates
- Basic HTML knowledge
- Familiarity with Flask routes and render_template

## Core Concepts

### Base Template

The base template defines the common structure:
- HTML skeleton (doctype, head, body)
- Navigation
- Header and footer
- **Blocks** — placeholders that child templates override

### Block Syntax

```html
{% block title %}Default Title{% endblock %}
```

Child templates override blocks with their own content:

```html
{% extends "base.html" %}

{% block title %}My Custom Title{% endblock %}
```

### Why Use Inheritance?

Without inheritance, every page repeats header/footer:
```html
<!-- index.html -->
<header>...</header>
<main>Content</main>
<footer>...</footer>

<!-- about.html -->
<header>...</header>
<main>About page</main>
<footer>...</footer>
```

With inheritance:
```html
<!-- base.html -->
<header>...</header>
<main>{% block content %}{% endblock %}</main>
<footer>...</footer>

<!-- index.html -->
{% extends "base.html" %}
{% block content %}Content{% endblock %}
```

## Code Walkthrough

### Base Template

```html
<!-- templates/base.html — The master template -->
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>{% block title %}My Website{% endblock %}</title>
    
    <!-- CSS -->
    <link rel="stylesheet" href="{{ url_for('static', filename='css/style.css') }}">
    
    <!-- Extra head content from child templates -->
    {% block extra_head %}{% endblock %}
</head>
<body>
    <!-- Navigation -->
    <nav>
        <div class="logo">My Website</div>
        <ul class="nav-links">
            <li><a href="{{ url_for('index') }}">Home</a></li>
            <li><a href="{{ url_for('about') }}">About</a></li>
            <li><a href="{{ url_for('contact') }}">Contact</a></li>
        </ul>
    </nav>
    
    <!-- Flash messages -->
    {% with messages = get_flashed_messages() %}
        {% if messages %}
            <div class="flash-messages">
                {% for message in messages %}
                    <div class="alert">{{ message }}</div>
                {% endfor %}
            </div>
        {% endif %}
    {% endwith %}
    
    <!-- Main content area -->
    <main>
        {% block content %}
        <!-- Child templates replace this -->
        <p>Default content if none provided</p>
        {% endblock %}
    </main>
    
    <!-- Footer -->
    <footer>
        <p>&copy; 2024 My Website. All rights reserved.</p>
    </footer>
    
    <!-- JavaScript -->
    <script src="{{ url_for('static', filename='js/main.js') }}"></script>
    {% block extra_js %}{% endblock %}
</body>
</html>
```

### Child Templates

```html
<!-- templates/index.html — Home page extends base -->
{% extends "base.html" %}

{% block title %}Home - My Website{% endblock %}

{% block content %}
<div class="hero">
    <h1>Welcome to My Website!</h1>
    <p>This is the home page with a hero section.</p>
</div>

<div class="features">
    <div class="feature">
        <h3>Feature 1</h3>
        <p>Description of feature 1.</p>
    </div>
    <div class="feature">
        <h3>Feature 2</h3>
        <p>Description of feature 2.</p>
    </div>
    <div class="feature">
        <h3>Feature 3</h3>
        <p>Description of feature 3.</p>
    </div>
</div>
{% endblock %}
```

```html
<!-- templates/about.html — About page extends base -->
{% extends "base.html" %}

{% block title %}About Us - My Website{% endblock %}

{% block content %}
<div class="about">
    <h1>About Us</h1>
    <p>We are a company that does things.</p>
    
    <h2>Our Mission</h2>
    <p>To provide excellent services.</p>
    
    <h2>Our Team</h2>
    <ul>
        <li>Alice - CEO</li>
        <li>Bob - CTO</li>
        <li>Carol - Designer</li>
    </ul>
</div>
{% endblock %}
```

```html
<!-- templates/contact.html — Contact page with extra CSS -->
{% extends "base.html" %}

{% block title %}Contact - My Website{% endblock %}

{% block extra_head %}
<!-- Page-specific styles -->
<style>
    .contact-form { max-width: 500px; margin: 0 auto; }
    .contact-form input, .contact-form textarea { width: 100%; }
</style>
{% endblock %}

{% block content %}
<div class="contact">
    <h1>Contact Us</h1>
    <p>Fill out the form below to reach us.</p>
    
    <form class="contact-form" method="post">
        <div class="form-group">
            <label for="name">Name:</label>
            <input type="text" name="name" id="name" required>
        </div>
        <div class="form-group">
            <label for="email">Email:</label>
            <input type="email" name="email" id="email" required>
        </div>
        <div class="form-group">
            <label for="message">Message:</label>
            <textarea name="message" id="message" rows="5" required></textarea>
        </div>
        <button type="submit">Send Message</button>
    </form>
</div>
{% endblock %}
```

### Flask Application

```python
# app.py — Using template inheritance
from flask import Flask, flash

app = Flask(__name__)
app.secret_key = "secret"

@app.route("/")
def index():
    """Home page."""
    return render_template("index.html")

@app.route("/about")
def about():
    """About page."""
    return render_template("about.html")

@app.route("/contact")
def contact():
    """Contact page."""
    return render_template("contact.html")

@app.route("/flash")
def flash_demo():
    """Demonstrate flash messages with inheritance."""
    flash("This is a flash message!")
    return render_template("index.html")

# Need render_template for this to work
from flask import render_template

if __name__ == "__main__":
    app.run(debug=True)
```

### Line-by-Line Breakdown

- `{% extends "base.html" %}` — Tells Jinja2 this template extends base.html.
- `{% block title %}...{% endblock %}` — Defines a replaceable section; child templates override this.
- `{% block content %}...{% endblock %}` — The main content block that child templates fill in.
- `{% block extra_head %}{% endblock %}` — Optional block for page-specific head content.
- `url_for('static', filename='...')` — Generates URL for static files.

## Common Mistakes

❌ **Forgetting to extend base template**
```html
<!-- WRONG — This template stands alone, not inheriting -->
<h1>Welcome</h1>
```

✅ **Correct — Extend the base template**
```html
<!-- CORRECT — Extends base template -->
{% extends "base.html" %}
{% block content %}
<h1>Welcome</h1>
{% endblock %}
```

❌ **Not providing required blocks**
```html
<!-- WRONG — Base defines content block, must override -->
{% extends "base.html" %}
<!-- Missing {% block content %} -->
```

✅ **Correct — Override required blocks**
```html
<!-- CORRECT -->
{% extends "base.html" %}
{% block content %}
<h1>Welcome</h1>
{% endblock %}
```

## Quick Reference

| Tag | Description |
|-----|-------------|
| `{% extends "base.html" %}` | Child template extends base |
| `{% block name %}...{% endblock %}` | Define/replace a section |
| `{{ super() }}` | Include parent block content |
| `{% include "file.html" %}` | Include another template |

## Next Steps

Now you understand base templates. Continue to [02_blocks_and_extends.md](02_blocks_and_extends.md) to learn more about advanced block features like calling `super()` and multiple inheritance levels.