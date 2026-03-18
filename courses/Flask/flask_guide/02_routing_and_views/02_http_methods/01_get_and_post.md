<!-- FILE: 02_routing_and_views/02_http_methods/01_get_and_post.md -->

## Overview

HTTP defines different **methods** (also called verbs) that specify the type of action a client wants to perform. **GET** and **POST** are the most common methods in web applications. GET retrieves data (like loading a page), while POST submits data to the server (like a form submission). Understanding when to use each method is fundamental to building proper RESTful web applications.

## Prerequisites

- Understanding of HTTP basics (from earlier in the guide)
- Flask routing knowledge
- Basic HTML form understanding

## Core Concepts

### GET Requests

GET requests are used to **retrieve** data from the server. Characteristics:
- Parameters are visible in the URL (query string)
- Can be cached by browsers
- Should not modify server state
- Length limited by URL length (about 2000 characters)
- Examples: viewing a page, searching, clicking a link

### POST Requests

POST requests are used to **submit** data to the server. Characteristics:
- Data is sent in the request body (not visible in URL)
- Not cached by browsers
- Can modify server state (create, update, delete)
- No practical length limit
- Examples: form submissions, API data creation, file uploads

### The methods Parameter

By default, `@app.route()` only handles GET requests. To handle POST (or other methods), add `methods=["GET", "POST"]` to the decorator.

## Code Walkthrough

### Handling Both GET and POST in One Route

```python
# http_methods.py — Handling GET and POST requests
from flask import Flask, request, render_template_string, redirect, url_for, flash

app = Flask(__name__)
app.secret_key = "secret-key-for-flash-messages"  # Required for flash()

# Simple in-memory data store
users = {"alice": "password123", "bob": "secret456"}

# HTML template for the login form
LOGIN_TEMPLATE = """
<!DOCTYPE html>
<html>
<head><title>Login</title></head>
<body>
    <h1>Login</h1>
    {% with messages = get_flashed_messages() %}
        {% if messages %}
            <ul style="color: red;">
            {% for message in messages %}
                <li>{{ message }}</li>
            {% endfor %}
            </ul>
        {% endif %}
    {% endwith %}
    <form method="post" action="{{ url_for('login') }}">
        <label>Username: <input type="text" name="username"></label><br>
        <label>Password: <input type="password" name="password"></label><br>
        <button type="submit">Login</button>
    </form>
</body>
</html>
"""

@app.route("/login", methods=["GET", "POST"])
def login():
    """
    Handle login - both GET (show form) and POST (process form).
    
    GET: Display the login form
    POST: Validate credentials and log in the user
    """
    if request.method == "POST":
        # This is a POST request - process the form data
        username = request.form.get("username", "").strip()
        password = request.form.get("password", "")
        
        # Validate credentials
        if username in users and users[username] == password:
            return f"<h1>Welcome, {username}!</h1><p>Login successful!</p>"
        else:
            # Invalid credentials - show error and re-display form
            flash("Invalid username or password")
            return redirect(url_for("login"))
    else:
        # This is a GET request - show the login form
        return render_template_string(LOGIN_TEMPLATE)

# Example: Pure GET route (default)
@app.route("/search")
def search():
    """Search page - only accepts GET requests (the default)."""
    query = request.args.get("q", "")
    if query:
        return f"<h1>Search Results</h1><p>You searched for: {query}</p>"
    return "<h1>Search</h1><p>Use ?q=your+query in the URL</p>"

# Example: Pure POST route
@app.route("/api/data", methods=["POST"])
def receive_data():
    """
    API endpoint - only accepts POST requests.
    Typically returns JSON instead of HTML.
    """
    # Get JSON data from request body
    data = request.get_json()
    return {
        "status": "success",
        "received": data,
        "message": "Data received successfully"
    }

if __name__ == "__main__":
    app.run(debug=True)
```

### Line-by-Line Breakdown

- `methods=["GET", "POST"]` — Tells Flask to handle both HTTP methods on this route.
- `if request.method == "POST":` — Checks which HTTP method was used.
- `request.form.get("username", "")` — Gets form data from POST body; similar to `request.args` for GET query strings.
- `request.get_json()` — Parses JSON data from the request body (for API endpoints).
- `flash("Invalid username")` — Stores a message to display on the next page load.
- `redirect(url_for("login"))` — Redirects the user to a different route.
- `url_for("login")` — Generates the URL for the "login" route (avoids hardcoding URLs).

### Testing with curl

```bash
# Test GET request to login page
curl http://127.0.0.1:5000/login
# Returns the login form HTML

# Test GET request with query string
curl "http://127.0.0.1:5000/search?q=flask"
# Returns: <h1>Search Results</h1><p>You searched for: flask</p>

# Test POST request (form data)
curl -X POST -d "username=alice&password=password123" http://127.0.0.1:5000/login
# Returns: <h1>Welcome, alice!</h1><p>Login successful!</p>

# Test POST request with invalid credentials
curl -X POST -d "username=alice&password=wrong" http://127.0.0.1:5000/login
# Returns redirect to login page with flash message

# Test POST request with JSON data
curl -X POST -H "Content-Type: application/json" -d '{"name":"Alice","age":30}' http://127.0.0.1:5000/api/data
# Returns: {"message": "Data received successfully", "received": {"age": 30, "name": "Alice"}, "status": "success"}
```

## Common Mistakes

❌ **Not specifying methods parameter**
```python
# WRONG — Only handles GET, POST requests will return 405 Method Not Allowed
@app.route("/submit")
def submit():
    return "Submitted"
```

✅ **Correct — Specify all methods you handle**
```python
# CORRECT — Handles both GET and POST
@app.route("/submit", methods=["GET", "POST"])
def submit():
    return "Submitted"
```

❌ **Confusing request.form and request.args**
```python
# WRONG — Using request.form for GET query parameters
@app.route("/search")
def search():
    # This won't work for ?q=term
    query = request.form.get("q")  # Returns None for GET requests
```

✅ **Correct — Use the right accessor**
```python
# CORRECT — Use request.args for GET query strings
@app.route("/search")
def search():
    query = request.args.get("q", "")  # Works for GET requests
    return f"Searched for: {query}"
```

## Quick Reference

| Request Property | Description |
|------------------|-------------|
| `request.method` | The HTTP method (GET, POST, PUT, etc.) |
| `request.args` | Query string parameters (GET) |
| `request.form` | Form data (POST) |
| `request.get_json()` | JSON body (POST/PUT/PATCH) |
| `request.headers` | HTTP headers |
| `request.cookies` | Cookies sent by client |

## Next Steps

Now that you understand GET and POST, continue to [02_put_patch_delete.md](02_put_patch_delete.md) to learn about the remaining HTTP methods: PUT, PATCH, and DELETE.