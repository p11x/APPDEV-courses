<!-- FILE: 02_routing_and_views/03_view_functions/02_redirects_and_errors.md -->

## Overview

**Redirects** send the user's browser to a different URL, while **error handlers** manage how your application responds when things go wrong (404 page not found, 500 server error, etc.). These are essential for building user-friendly applications — guiding users to the right place and handling errors gracefully.

## Prerequisites

- Understanding of Flask view functions and responses
- Basic knowledge of HTTP status codes
- Familiarity with the request object

## Core Concepts

### Redirects

A **redirect** tells the browser to make a new request to a different URL. Common uses:
- After form submission, redirect to prevent duplicate submissions
- Route old URLs to new ones
- Require HTTPS by redirecting HTTP requests
- Login-required redirects

The redirect response has status code 302 (temporary) or 301 (permanent).

### Error Handlers

Flask allows you to define custom handlers for HTTP error codes:
- 400 — Bad Request
- 401 — Unauthorized
- 403 — Forbidden
- 404 — Not Found
- 500 — Internal Server Error

Error handlers let you return custom HTML, JSON, or any response for these status codes.

## Code Walkthrough

### Redirect Examples

```python
# redirects.py — Demonstrating redirects
from flask import Flask, redirect, url_for, request, flash

app = Flask(__name__)
app.secret_key = "secret"  # Required for flash messages

# Simple in-memory data
users = {"alice": "data1", "bob": "data2"}

# 1. Basic redirect to another route
@app.route("/")
def index():
    """Home page redirects to dashboard."""
    return redirect(url_for("dashboard"))

@app.route("/dashboard")
def dashboard():
    """Dashboard page."""
    return "<h1>Dashboard</h1><p>Welcome!</p>"

# 2. Redirect with flash message
@app.route("/login", methods=["GET", "POST"])
def login():
    """Login page with redirect after POST."""
    if request.method == "POST":
        username = request.form.get("username")
        password = request.form.get("password")
        
        if username == "admin" and password == "secret":
            # Success - redirect to dashboard with success message
            flash("Login successful!", "success")
            return redirect(url_for("dashboard"))
        else:
            # Failed - redirect back to login with error message
            flash("Invalid credentials", "error")
            return redirect(url_for("login"))
    
    return """
    <h1>Login</h1>
    {% with messages = get_flashed_messages() %}
        {% if messages %}
            {% for category, message in messages %}
                <p class="{{ category }}">{{ message }}</p>
            {% endfor %}
        {% endif %}
    {% endwith %}
    <form method="post">
        <input type="text" name="username" placeholder="Username"><br>
        <input type="password" name="password" placeholder="Password"><br>
        <button type="submit">Login</button>
    </form>
    """

# 3. Redirect with query parameters
@app.route("/search")
def search():
    """Search redirects to results page."""
    query = request.args.get("q", "")
    # Redirect to results with query as parameter
    return redirect(url_for("results", query=query))

@app.route("/results")
def results():
    """Display search results."""
    query = request.args.get("query", "")
    return f"<h1>Results</h1><p>You searched for: {query}</p>"

# 4. Permanent redirect (301) - for SEO when URLs change
@app.route("/old-page")
def old_page():
    """Old URL redirects permanently to new location."""
    return redirect(url_for("new_page"), code=301)

@app.route("/new-page")
def new_page():
    """New URL."""
    return "<h1>New Page</h1>"

# 5. External redirect
@app.route("/external")
def external():
    """Redirect to external website."""
    return redirect("https://example.com")

if __name__ == "__main__":
    app.run(debug=True)
```

### Error Handler Examples

```python
# error_handlers.py — Custom error pages
from flask import Flask, render_template_string, jsonify

app = Flask(__name__)

# Custom 404 error page
@app.errorhandler(404)
def page_not_found(e):
    """Handle 404 Not Found errors."""
    return "<h1>404 - Page Not Found</h1><p>The page you requested doesn't exist.</p>", 404

# Custom 500 error page
@app.errorhandler(500)
def internal_error(e):
    """Handle 500 Internal Server Error."""
    return "<h1>500 - Server Error</h1><p>Something went wrong. Please try again later.</p>", 500

# Custom 400 error page
@app.errorhandler(400)
def bad_request(e):
    """Handle 400 Bad Request."""
    return "<h1>400 - Bad Request</h1><p>Your request was invalid.</p>", 400

# Custom 403 error page
@app.errorhandler(403)
def forbidden(e):
    """Handle 403 Forbidden."""
    return "<h1>403 - Forbidden</h1><p>You don't have permission to access this resource.</p>", 403

# API-style JSON error responses
@app.errorhandler(404)
def api_not_found(e):
    """JSON 404 for API routes."""
    return jsonify({"error": "Resource not found", "status": 404}), 404

# Abort function - raise errors programmatically
from flask import abort

@app.route("/protected")
def protected():
    """Example using abort to raise errors."""
    # Simulate authentication check
    is_authenticated = False
    
    if not is_authenticated:
        # Raise 401 Unauthorized
        abort(401)
    
    return "<h1>Protected Content</h1>"

# Using abort with custom message
@app.route("/item/<int:item_id>")
def get_item(item_id):
    """Get item or 404 if not found."""
    items = {1: "Apple", 2: "Banana", 3: "Cherry"}
    
    if item_id not in items:
        # Raise 404 with custom description
        abort(404, description=f"Item {item_id} not found")
    
    return f"<h1>Item: {items[item_id]}</h1>"

# JSON API error handler for all errors
@app.errorhandler(Exception)
def handle_exception(e):
    """Catch-all handler for unhandled exceptions."""
    # Return JSON instead of HTML for API routes
    return jsonify({
        "error": str(e),
        "type": type(e).__name__
    }), 500

if __name__ == "__main__":
    app.run(debug=True)
```

### Line-by-Line Breakdown

- `redirect(url_for("dashboard"))` — Redirects to the "dashboard" route; url_for() generates the URL.
- `flash("message", "success")` — Stores a message to display on the next page load.
- `code=301` — Sets the HTTP redirect status code (301 = permanent, 302 = temporary).
- `@app.errorhandler(404)` — Decorator that registers a function to handle 404 errors.
- `abort(401)` — Programmatically raises an HTTP error (stops execution, triggers error handler).
- `abort(404, description="...")` — Abort with custom error message.

### Testing

```bash
# Test redirect
curl -i http://127.0.0.1:5000/
# HTTP/1.1 302 FOUND
# Location: /dashboard

# Test 404 handler
curl -i http://127.0.0.1:5000/nonexistent
# HTTP/1.1 404 NOT FOUND
# <h1>404 - Page Not Found</h1>...

# Test abort
curl -i http://127.0.0.1:5000/item/999
# HTTP/1.1 404 NOT FOUND
```

## Common Mistakes

❌ **Redirecting after POST without using flash**
```python
# WRONG — User sees no feedback after form submission
@app.route("/submit", methods=["POST"])
def submit():
    # Process form...
    return redirect(url_for("index"))
```

✅ **Correct — Use flash messages**
```python
# CORRECT — User sees feedback
@app.route("/submit", methods=["POST"])
def submit():
    flash("Data saved successfully!", "success")
    return redirect(url_for("index"))
```

❌ **Using redirect instead of abort for errors**
```python
# WRONG — Semantically incorrect
@app.route("/item/<int:id>")
def get_item(id):
    if id not in items:
        return redirect(url_for("index"))  # Wrong - this isn't a redirect situation
```

✅ **Correct — Use abort for errors**
```python
# CORRECT — 404 is an error condition
@app.route("/item/<int:id>")
def get_item(id):
    if id not in items:
        abort(404)
```

## Quick Reference

| Function | Purpose | Status Code |
|----------|---------|-------------|
| `redirect(url, code=302)` | Redirect to another URL | 302 (temp), 301 (perm) |
| `abort(code)` | Raise an HTTP error | Varies (404, 401, etc.) |
| `@app.errorhandler(code)` | Register error handler | Custom |
| `url_for("route")` | Generate URL for a route | - |
| `flash(message, category)` | Store message for next request | - |

## Next Steps

Now you understand redirects and errors. Continue to [03_url_for.md](03_url_for.md) to learn about the url_for() function and how to properly generate URLs in your Flask applications.