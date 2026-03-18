<!-- FILE: 02_routing_and_views/01_basic_routing/01_route_decorator.md -->

## Overview

**Routing** is the process of mapping a URL (like `/about` or `/products/5`) to a Python function that handles it. In Flask, you define routes using the `@app.route()` decorator. This is the foundation of any Flask application — every page a user visits corresponds to a route you define. Understanding how the route decorator works gives you precise control over how users navigate your application.

## Prerequisites

- A working Flask application setup
- Basic understanding of Python functions and decorators
- Familiarity with URLs and web pages

## Core Concepts

### What is a Route?

A **route** is a URL pattern that Flask associates with a view function. When a user visits a URL:
1. Flask receives the HTTP request
2. Flask matches the URL path against all registered routes
3. Flask calls the matching view function
4. The view function returns a response
5. Flask sends the response back to the browser

### The Route Decorator

The `@app.route()` decorator is Flask's primary way to define routes. It takes the URL pattern as an argument and registers the following function as the handler for that pattern.

### URL Patterns

Routes can be static or dynamic:
- **Static routes** — Match exactly (e.g., `/about` matches only `/about`)
- **Dynamic routes** — Contain variables that capture parts of the URL (e.g., `/user/<username>` matches `/user/alice` and `/user/bob`)

### Default Behavior

By default, `@app.route()` only handles GET requests. If you need to handle POST, PUT, DELETE, or other methods, you must explicitly specify them.

## Code Walkthrough

### Basic Route Examples

```python
# routes.py — Demonstrating various route patterns
from flask import Flask, jsonify

app = Flask(__name__)

# 1. Simple static route — exactly matches the URL
@app.route("/")
def home():
    """Home page - serves when user visits the root URL."""
    return "Welcome to the Home Page!"

# 2. Another static route
@app.route("/about")
def about():
    """About page - serves when user visits /about."""
    return "This is the About Page."

# 3. Route with exact path (handling trailing slashes)
# By default, /contact and /contact/ are treated as different routes
@app.route("/contact")
def contact():
    """Contact page."""
    return "Contact us at: email@example.com"

# 4. Using the redirect_trailing_slashes behavior
# This makes /contact/ redirect to /contact (SEO-friendly)
app.url_map.strict_slashes = False  # Makes /contact AND /contact/ work

# 5. Multiple routes pointing to the same view function
@app.route("/")
@app.route("/home")
@app.route("/index")
def home():
    """Same view handles multiple URLs."""
    return "Welcome Home!"

# 6. Dynamic route - captures a variable
@app.route("/user/<username>")
def user_profile(username):
    """User profile page - username is captured from the URL."""
    return f"<h1>User Profile</h1><p>Hello, {username}!</p>"

# 7. Route with type conversion
@app.route("/post/<int:post_id>")
def show_post(post_id):
    """Show a specific post by ID - only matches integers."""
    return f"<h1>Post #{post_id}</h1><p>This is post number {post_id}."

# 8. Multiple URL variables
@app.route("/blog/<int:year>/<int:month>/<slug>")
def blog_post(year, month, slug):
    """Blog post URL like /blog/2024/03/my-first-post."""
    return f"<h1>{slug}</h1><p>Posted in {month}/{year}</p>"

if __name__ == "__main__":
    app.run(debug=True)
```

### Line-by-Line Breakdown

- `app = Flask(__name__)` — Creates the Flask application instance.
- `@app.route("/")` — Registers the following function to handle requests to "/".
- `def home():` — Defines the view function that runs when "/" is accessed.
- `return "Welcome to the Home Page!"` — Returns a plain text response.
- `@app.route("/user/<username>")` — The `<username>` part is a **URL variable** that captures any value.
- `def user_profile(username):` — The captured value is passed as an argument to the function.
- `@app.route("/post/<int:post_id>")` — `<int:post_id>` converts the URL segment to an integer; if non-numeric, this route won't match.
- `app.url_map.strict_slashes = False` — Configures Flask to treat URLs with and without trailing slashes as equivalent.

### Testing the Routes

Run the application and test these URLs:
- `http://127.0.0.1:5000/` → "Welcome to the Home Page!"
- `http://127.0.0.1:5000/about` → "This is the About Page."
- `http://127.0.0.1:5000/user/Alice` → "Hello, Alice!"
- `http://127.0.0.1:5000/post/42` → "Post #42"
- `http://127.0.0.1:5000/post/abc` → 404 Not Found (because "abc" is not an integer)

## Common Mistakes

❌ **Forgetting to return a response**
```python
# WRONG — View functions must return a response
@app.route("/")
def empty():
    pass  # This causes an error
```

✅ **Correct — Always return something**
```python
# CORRECT — Return a string, Response object, or redirect
@app.route("/")
def index():
    return "Hello!"  # Must return a response
```

❌ **Forgetting that routes are matched in order**
```python
# WRONG — /profile/new might be caught by /profile/<username> first
@app.route("/profile/<username>")
def profile(username):
    return f"Profile: {username}"

@app.route("/profile/new")
def new_profile():
    return "Create new profile"
# /profile/new would match /profile/<username> with username='new'
```

✅ **Correct — More specific routes first**
```python
# CORRECT — More specific routes should be defined first
@app.route("/profile/new")  # More specific first
def new_profile():
    return "Create new profile"

@app.route("/profile/<username>")  # More general second
def profile(username):
    return f"Profile: {username}"
```

❌ **Using the wrong URL variable converter**
```python
# WRONG — /product/abc would not match, would return 404
@app.route("/product/<int:product_id>")
def product(product_id):
    return f"Product {product_id}"
# Cannot handle non-integer URLs like /product/abc
```

## Quick Reference

| Pattern | Description | Example Match |
|---------|-------------|---------------|
| `/` | Static root route | `/` |
| `/about` | Static route | `/about` |
| `<var>` | String variable | `/user/alice` |
| `<int:var>` | Integer variable | `/post/42` |
| `<float:var>` | Float variable | `/price/19.99` |
| `<path:var>` | Path including slashes | `/files/path/to/file` |

## Next Steps

Now that you understand the route decorator, continue to [02_dynamic_routes.md](02_dynamic_routes.md) to learn more about dynamic URL parameters and how to capture, validate, and use them in your view functions.