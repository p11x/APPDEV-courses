<!-- FILE: 03_templates/01_jinja2_basics/01_rendering_templates.md -->

## Overview

**Templates** are HTML files with special syntax that allows dynamic content. Flask uses **Jinja2** as its template engine — a powerful, secure, and Pythonic templating language. Instead of building entire HTML pages in Python strings, you create template files that Flask renders with data from your view functions. This separates your presentation logic from business logic.

## Prerequisites

- Understanding of Flask routes and view functions
- Basic HTML knowledge
- Familiarity with Python data structures (dicts, lists)

## Core Concepts

### What is Jinja2?

Jinja2 is Flask's default template engine (bundled with Flask). It lets you:
- Insert dynamic variables into HTML
- Use control structures (if/else, loops)
- Reuse HTML across multiple pages (template inheritance)
- Create reusable components (macros)

### How Flask Finds Templates

Flask looks for templates in a `templates/` folder relative to your application instance:
```
my_app/
├── app.py
└── templates/
    └── index.html
```

### Rendering Templates

Use `render_template()` to render a template with context data:

```python
from flask import render_template

@app.route("/")
def index():
    return render_template("index.html", name="Alice", age=30)
```

In `index.html`, you access these as `{{ name }}` and `{{ age }}`.

## Code Walkthrough

### Basic Template Rendering

```python
# app.py — Flask app with templates
from flask import Flask, render_template

app = Flask(__name__)

# 1. Basic template with variables
@app.route("/")
def index():
    """Pass variables to template."""
    return render_template(
        "index.html",
        title="Welcome",
        name="Alice",
        items=["Apple", "Banana", "Cherry"]
    )

# 2. Template with object data
@app.route("/user/<username>")
def user_profile(username):
    """Pass object to template."""
    user = {
        "username": username,
        "email": f"{username}@example.com",
        "role": "admin" if username == "admin" else "user",
        "posts": [
            {"id": 1, "title": "First Post", "likes": 10},
            {"id": 2, "title": "Second Post", "likes": 25}
        ]
    }
    return render_template("user.html", user=user)

# 3. Template with empty data
@app.route("/empty")
def empty_page():
    """Render template with no extra data."""
    return render_template("empty.html")

if __name__ == "__main__":
    app.run(debug=True)
```

### Template Files

```html
<!-- templates/index.html — Basic template with variables -->
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>{{ title }}</title>
</head>
<body>
    <h1>Hello, {{ name }}!</h1>
    
    <h2>Your Items:</h2>
    <ul>
        {% for item in items %}
            <li>{{ item }}</li>
        {% endfor %}
    </ul>
</body>
</html>
```

```html
<!-- templates/user.html — Template with object data -->
<!DOCTYPE html>
<html>
<head>
    <title>{{ user.username }}'s Profile</title>
</head>
<body>
    <h1>{{ user.username }}</h1>
    <p>Email: {{ user.email }}</p>
    <p>Role: {{ user.role }}</p>
    
    <h2>Posts:</h2>
    <ul>
    {% for post in user.posts %}
        <li>
            <strong>{{ post.title }}</strong> 
            ({{ post.likes }} likes)
        </li>
    {% endfor %}
    </ul>
</body>
</html>
```

```html
<!-- templates/empty.html — Minimal template -->
<!DOCTYPE html>
<html>
<head>
    <title>Empty Page</title>
</head>
<body>
    <h1>This page has no dynamic content</h1>
</body>
</html>
```

### Line-by-Line Breakdown

- `render_template("index.html", name="Alice", items=[...])` — Renders template with context variables.
- `{{ title }}` — **Variable interpolation** — replaced with the value of `title`.
- `{% for item in items %}` — **Control structure** — iterates over a list.
- `{% endfor %}` — Ends the for loop.
- `{{ user.username }}` — Accesses nested object property.

### Using render_template_string

For simple examples without creating files:

```python
# Using render_template_string instead of render_template
from flask import render_template_string

@app.route("/")
def index():
    template = """
    <!DOCTYPE html>
    <html>
    <body>
        <h1>Hello, {{ name }}!</h1>
        <ul>
        {% for item in items %}
            <li>{{ item }}</li>
        {% endfor %}
        </ul>
    </body>
    </html>
    """
    return render_template_string(template, name="Alice", items=["A", "B", "C"])
```

## Common Mistakes

❌ **Putting templates in the wrong folder**
```bash
# WRONG — Flask won't find this
my_app/
├── app.py
└── html/
    └── index.html
```

✅ **Correct — Use templates folder**
```bash
# CORRECT — Flask looks in templates/ by default
my_app/
├── app.py
└── templates/
    └── index.html
```

❌ **Forgetting to pass required variables**
```python
# WRONG — Template expects 'name' but none provided
return render_template("index.html")
# Template has {{ name }} but name is not defined
```

✅ **Correct — Pass all required variables**
```python
# CORRECT — Provide all variables the template needs
return render_template("index.html", name="Alice")
```

❌ **Using wrong variable syntax**
```html
<!-- WRONG — Square brackets are Python, not Jinja -->
<body>
    {{ items[0] }}  <!-- Won't work as expected -->
</body>
```

✅ **Correct — Use Jinja2 syntax**
```html
<!-- CORRECT — Use dot notation or proper indexing -->
<body>
    {{ items[0] }}  <!-- This actually works, but prefer: -->
    {{ items.0 }}   <!-- This also works -->
</body>
```

## Quick Reference

| Syntax | Description | Example |
|--------|-------------|---------|
| `{{ variable }}` | Output variable | `{{ name }}` |
| `{% for %}` | For loop | `{% for item in items %}` |
| `{% if %}` | If statement | `{% if user %}` |
| `{% endif %}` | End block | Closes if/for |
| `{% endfor %}` | End for loop | Closes for |

## Next Steps

Now that you can render templates, continue to [02_variables_and_filters.md](02_variables_and_filters.md) to learn about template variables, filters, and how to transform data within templates.