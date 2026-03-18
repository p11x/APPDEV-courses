<!-- FILE: 02_routing_and_views/01_basic_routing/02_dynamic_routes.md -->

## Overview

**Dynamic routes** allow your Flask application to handle URLs with variable parts — like `/user/Alice`, `/post/42`, or `/products/python-book`. Instead of creating separate routes for every possible user or post, you create one route pattern with a variable that captures the changing part. This is essential for building real-world applications like blogs, e-commerce sites, and user dashboards.

## Prerequisites

- Understanding of Flask's `@app.route()` decorator (from the previous file)
- Basic Python function knowledge
- Familiarity with URL structure

## Core Concepts

### URL Variables

A URL variable is a placeholder in a route that captures a portion of the URL. It is denoted by `<variable_name>` inside the route string. Flask captures whatever is in that position and passes it to your view function as an argument.

For example, the route `/user/<username>` matches URLs like:
- `/user/Alice`
- `/user/Bob`
- `/user/any_thing_here`

And the captured value gets passed to your view function.

### Variable Converters

By default, URL variables capture everything until the next `/`. But you can restrict what they capture using **converters**:

| Converter | Syntax | Description | Example Match |
|-----------|--------|-------------|---------------|
| String (default) | `<var>` | Matches any characters except `/` | `/user/alice` |
| Integer | `<int:var>` | Matches digits only | `/post/42` |
| Float | `<float:var>` | Matches numbers with decimals | `/price/19.99` |
| Path | `<path:var>` | Matches including `/` | `/files/a/b/c` |
| UUID | `<uuid:var>` | Matches UUID format | `/item/550e8400-e29b-41d4-a716-446655440000` |
| Any | `<any(opt1,opt2):var>` | Matches specific options | `/page/<any(admin,user):name>` |

### Why Use Converters?

Converters provide two benefits:
1. **Validation** — Only matching URLs trigger the route; others return 404
2. **Type Safety** — The captured value is automatically converted to the right type

## Code Walkthrough

### Dynamic Route Examples

```python
# dynamic_routes.py — Examples of dynamic URL routing
from flask import Flask, jsonify

app = Flask(__name__)

# 1. Basic string variable - captures any text
@app.route("/user/<username>")
def user_profile(username):
    """Display a user's profile page.
    
    Args:
        username: The username captured from the URL
    """
    return f"<h1>User Profile</h1><p>Welcome, {username}!</p>"

# 2. Integer variable - only matches numbers
@app.route("/post/<int:post_id>")
def show_post(post_id):
    """Display a specific blog post.
    
    Args:
        post_id: The post ID (must be an integer)
    """
    # post_id is already an integer, not a string!
    return f"<h1>Blog Post #{post_id}</h1><p>Content of post {post_id}</p>"

# 3. Float variable - matches decimal numbers
@app.route("/price/<float:amount>")
def show_price(amount):
    """Display a price.
    
    Args:
        amount: The price as a float
    """
    # amount is a float, so we can do math
    return f"<h1>Price</h1><p>${amount:.2f}</p>"

# 4. Multiple variables in one route
@app.route("/blog/<int:year>/<int:month>/<slug>")
def blog_post(year, month, slug):
    """Blog post with date and slug.
    
    Args:
        year: Publication year (4 digits)
        month: Publication month (1-12)
        slug: URL-friendly post identifier
    """
    return f"""
    <h1>{slug.replace('-', ' ').title()}</h1>
    <p>Published: {month}/{year}</p>
    """

# 5. Path variable - captures including slashes
@app.route("/files/<path:filepath>")
def serve_file(filepath):
    """Serve a file path.
    
    Args:
        filepath: Full file path including subdirectories
    """
    return f"<h1>File Request</h1><p>Path: {filepath}</p>"

# 6. Optional parameters using defaults
@app.route("/greet")
@app.route("/greet/<name>")
def greet(name="World"):
    """Greet a user - name is optional.
    
    Args:
        name: Name to greet (defaults to 'World')
    """
    return f"<h1>Hello, {name}!</h1>"

# 7. Using the 'any' converter for limited options
@app.route("/<any(admin,user,moderator):role>/dashboard")
def role_dashboard(role):
    """Dashboard for different user roles.
    
    Args:
        role: Must be one of admin, user, or moderator
    """
    dashboards = {
        "admin": "Admin Dashboard - Full access",
        "user": "User Dashboard - Limited access",
        "moderator": "Moderator Dashboard - Content moderation"
    }
    return f"<h1>{dashboards[role]}</h1>"

# 8. Combining with query parameters
@app.route("/search")
def search():
    """Search with query parameters.
    URL: /search?q=flask&page=1
    """
    from flask import request
    query = request.args.get("q", "")  # Get 'q' parameter, default to ""
    page = request.args.get("page", "1")  # Get 'page' parameter
    return f"<h1>Search Results</h1><p>Query: {query}, Page: {page}</p>"

if __name__ == "__main__":
    app.run(debug=True)
```

### Line-by-Line Breakdown

- `<username>` — A string variable that captures any characters except `/`.
- `def user_profile(username):` — The captured value becomes the function argument.
- `<int:post_id>` — Converts the URL segment to an integer; `/post/abc` returns 404.
- `def show_post(post_id):` — `post_id` is already a Python int, not a string.
- `<float:amount>` — Converts to a float; `/price/19.99` gives `19.99`.
- `<path:filepath>` — Matches path segments including `/`; `/files/a/b/c` gives `a/b/c`.
- `@app.route("/greet")` and `@app.route("/greet/<name>")` — Two routes sharing one function; `<name>` defaults to "World".
- `request.args.get("q", "")` — Gets query parameters from the URL (the part after `?`).

### Testing Dynamic Routes

Run the application and test:

```bash
# Test basic string variable
curl http://127.0.0.1:5000/user/Alice
# Output: <h1>User Profile</h1><p>Welcome, Alice!</p>

# Test integer variable (valid)
curl http://127.0.0.1:5000/post/42
# Output: <h1>Blog Post #42</h1><p>Content of post 42</p>

# Test integer variable (invalid - returns 404)
curl http://127.0.0.1:5000/post/abc
# Output: 404 Not Found

# Test default parameter
curl http://127.0.0.1:5000/greet
# Output: <h1>Hello, World!</h1>

# Test with query string
curl "http://127.0.0.1:5000/search?q=flask&page=2"
# Output: <h1>Search Results</h1><p>Query: flask, Page: 2</p>
```

## Common Mistakes

❌ **Mismatched variable names**
```python
# WRONG — Variable name in route doesn't match function parameter
@app.route("/user/<username>")
def user_profile(name):  # 'name' != 'username'
    return f"Hello, {name}"  # NameError: name 'name' is not defined
```

✅ **Correct — Match the variable names**
```python
# CORRECT — Variable name matches parameter name
@app.route("/user/<username>")
def user_profile(username):  # 'username' matches <username>
    return f"Hello, {username}"
```

❌ **Not converting types when needed**
```python
# WRONG — Trying to do math with a string
@app.route("/add/<a>/<b>")
def add(a, b):
    return str(a + b)  # TypeError: unsupported operand for +: 'str' and 'str'
```

✅ **Correct — Convert to the needed type**
```python
# CORRECT — Convert to int before doing math
@app.route("/add/<int:a>/<int:b>")
def add(a, b):
    return str(a + b)  # Works! Returns "7"
```

❌ **Using path variable incorrectly**
```python
# WRONG — Default string converter doesn't include slashes
@app.route("/path/<segment>")
def show_path(segment):
    return segment  # /path/a/b/c won't match
```

## Quick Reference

| Converter | Code | Notes |
|-----------|------|-------|
| String | `<name>` | Default, no conversion |
| Integer | `<int:id>` | Converts to int |
| Float | `<float:price>` | Converts to float |
| Path | `<path:filepath>` | Includes forward slashes |
| UUID | `<uuid:item_id>` | Validates UUID format |
| Any | `<any(a,b):choice>` | Matches specific options |

## Next Steps

Now that you understand dynamic routes, continue to [03_url_converters.md](03_url_converters.md) to learn more about Flask's built-in converters and how to create custom converters for advanced routing patterns.