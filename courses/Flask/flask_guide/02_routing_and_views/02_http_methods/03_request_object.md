<!-- FILE: 02_routing_and_views/02_http_methods/03_request_object.md -->

## Overview

Flask's **request object** is your gateway to all client data sent with an HTTP request. It provides convenient access to form data, query parameters, JSON payloads, headers, cookies, and uploaded files. Understanding the request object is essential for building dynamic web applications that respond to user input.

## Prerequisites

- Understanding of HTTP methods (GET, POST, PUT, etc.)
- Basic Flask routing knowledge
- Familiarity with HTML forms

## Core Concepts

### What is the Request Object?

The `request` object is a Flask global object (actually a `LocalProxy`) that represents the current HTTP request. It is automatically populated by Flask with data from the incoming request. You import it from Flask:

```python
from flask import request
```

### Request Data Sources

| Source | Access Property | Description |
|--------|----------------|-------------|
| Query string | `request.args` | URL parameters after `?` |
| Form data | `request.form` | POST form fields |
| JSON body | `request.get_json()` | JSON payload |
| Path variables | `request.view_args` | URL variables from route |
| Headers | `request.headers` | HTTP headers |
| Cookies | `request.cookies` | Browser cookies |
| Files | `request.files` | Uploaded files |

All these work like dictionaries — use `.get()` to safely access values.

## Code Walkthrough

### Complete Request Object Demo

```python
# request_demo.py — Comprehensive request object usage
from flask import Flask, request, jsonify

app = Flask(__name__)

@app.route("/demo", methods=["GET", "POST", "PUT"])
def demo():
    """
    Demonstrates all request object capabilities.
    """
    response = {
        # 1. HTTP Method
        "method": request.method,
        
        # 2. URL Information
        "path": request.path,           # The path without query string
        "url": request.url,           # Full URL with query string
        "base_url": request.base_url, # URL without query string
        "host": request.host,         # Host including port
        "host_url": request.host_url, # Scheme + host
        
        # 3. Query String (?key=value)
        "args": dict(request.args),    # All query parameters
        "query": request.args.get("q", "default_value"),  # Single param
        
        # 4. Form Data (POST/PUT)
        "form": dict(request.form),
        
        # 5. JSON Data
        "json": request.get_json(silent=True),  # Returns None if not JSON
        
        # 6. Headers
        "user_agent": request.headers.get("User-Agent"),
        "accept": request.headers.get("Accept"),
        "content_type": request.headers.get("Content-Type"),
        
        # 7. Cookies
        "session_id": request.cookies.get("session_id"),
        
        # 8. Remote Information
        "remote_addr": request.remote_addr,
        
        # 9. Request ID (useful for tracing)
        "request_id": request.headers.get("X-Request-ID"),
    }
    
    return jsonify(response)

# Example: Query string extraction
@app.route("/search")
def search():
    """Extract query parameters."""
    query = request.args.get("q", "")       # Get 'q' parameter
    page = request.args.get("page", "1")    # Get 'page' parameter
    sort = request.args.get("sort", "date") # Get 'sort' parameter
    
    # request.args is a MultiDict - use .getlist() for multiple values
    # Example: /search?tag=python&tag=flask -> request.args.getlist("tag")
    tags = request.args.getlist("tag")
    
    return jsonify({
        "query": query,
        "page": page,
        "sort": sort,
        "tags": tags
    })

# Example: Form handling
@app.route("/submit", methods=["POST"])
def submit():
    """Handle form submission."""
    name = request.form.get("name", "").strip()
    email = request.form.get("email", "").strip()
    newsletter = request.form.get("newsletter") == "on"  # Checkboxes are "on" if checked
    
    # Validate
    errors = []
    if not name:
        errors.append("Name is required")
    if not email or "@" not in email:
        errors.append("Valid email is required")
    
    if errors:
        return jsonify({"errors": errors}), 400
    
    return jsonify({
        "status": "success",
        "data": {"name": name, "email": email, "newsletter": newsletter}
    }), 201

# Example: JSON API handling
@app.route("/api/data", methods=["POST"])
def api_data():
    """Handle JSON API requests."""
    # Get JSON and return None if not valid JSON (silent=True)
    data = request.get_json(silent=True)
    
    if data is None:
        return jsonify({"error": "Invalid JSON"}), 400
    
    # Access nested data
    name = data.get("name")
    options = data.get("options", {})  # Default to empty dict
    
    return jsonify({
        "received": data,
        "name": name,
        "options": options
    })

# Example: Working with headers
@app.route("/headers")
def show_headers():
    """Display all request headers."""
    # Convert headers to dict (they're a special Headers object)
    headers_dict = dict(request.headers)
    return jsonify(headers_dict)

# Example: Accessing uploaded files
@app.route("/upload", methods=["POST"])
def upload():
    """Handle file uploads."""
    # request.files contains uploaded files
    if "file" not in request.files:
        return jsonify({"error": "No file part"}), 400
    
    file = request.files["file"]
    
    if file.filename == "":
        return jsonify({"error": "No selected file"}), 400
    
    # Access file properties
    filename = file.filename
    content_type = file.content_type
    
    # Read file contents
    content = file.read()
    
    return jsonify({
        "filename": filename,
        "content_type": content_type,
        "size": len(content)
    })

if __name__ == "__main__":
    app.run(debug=True)
```

### Testing with curl

```bash
# GET with query string
curl "http://127.0.0.1:5000/search?q=flask&page=2&tag=python&tag=web"
# {"page": "2", "query": "flask", "sort": "date", "tags": ["python", "web"]}

# POST form data
curl -X POST -d "name=Alice&email=alice@example.com&newsletter=on" http://127.0.0.1:5000/submit
# {"data": {"email": "alice@example.com", "name": "Alice", "newsletter": true}, "status": "success"}

# POST JSON data
curl -X POST -H "Content-Type: application/json" -d '{"name":"Bob","options":{"theme":"dark"}}' http://127.0.0.1:5000/api/data
# {"name": "Bob", "options": {"theme": "dark"}, "received": {...}}

# Show all headers
curl -H "X-Custom-Header: value" -H "Authorization: Bearer token" http://127.0.0.1:5000/headers
```

## Common Mistakes

❌ **Not checking if data exists**
```python
# WRONG — Will crash if 'name' is not in the form
name = request.form["name"]  # KeyError if missing
```

✅ **Correct — Use .get() with defaults**
```python
# CORRECT — Returns default value if missing
name = request.form.get("name", "Anonymous")
```

❌ **Using wrong accessor for data type**
```python
# WRONG — Query string not in request.form
@app.route("/search")
def search():
    query = request.form.get("q")  # Always None for GET requests
```

✅ **Correct — Use correct accessor**
```python
# CORRECT — Query string is in request.args
@app.route("/search")
def search():
    query = request.args.get("q")  # Works for GET requests
```

❌ **Not handling missing JSON gracefully**
```python
# WRONG — Will return 400 error if JSON is invalid
data = request.get_json()  # Raises exception if not JSON
```

✅ **Correct — Use silent=True**
```python
# CORRECT — Returns None instead of raising exception
data = request.get_json(silent=True)
if data is None:
    return jsonify({"error": "Invalid JSON"}), 400
```

## Quick Reference

| Property | Description | Example |
|----------|-------------|---------|
| `request.method` | HTTP method | "GET", "POST" |
| `request.args` | Query parameters | `?q=search` |
| `request.form` | Form data | Form POST |
| `request.get_json()` | JSON body | API POST |
| `request.headers` | HTTP headers | User-Agent, etc. |
| `request.cookies` | Cookies | session_id |
| `request.files` | Uploaded files | File uploads |
| `request.remote_addr` | Client IP | "127.0.0.1" |

## Next Steps

Now you understand the request object thoroughly. Continue to [01_returning_responses.md](../../02_routing_and_views/03_view_functions/01_returning_responses.md) to learn how to return different types of responses from your view functions.