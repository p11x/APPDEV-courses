<!-- FILE: 01_getting_started/01_introduction/03_how_http_works.md -->

## Overview

**HTTP** (Hypertext Transfer Protocol) is the foundation of all communication on the web. When your Flask application serves a page to a user, it is participating in an HTTP exchange. Understanding HTTP — specifically **requests**, **responses**, **methods**, and **status codes** — is essential for building effective web applications. This knowledge helps you debug issues, design better APIs, and understand how data flows between browsers and servers.

## Prerequisites

- Basic understanding of what a web application does
- Familiarity with the concept of URLs and web browsers
- No prior HTTP knowledge required (this file teaches it from scratch)

## Core Concepts

HTTP is a **request-response protocol** — the browser (client) sends a request to the server, and the server sends back a response. Every HTTP exchange involves several components:

### The Request

When you type `https://example.com/page` in your browser, it sends an HTTP request that includes:

1. **HTTP Method** — Specifies the type of action (GET, POST, PUT, DELETE, etc.)
2. **URL (Uniform Resource Locator)** — The address of the resource being requested
3. **Headers** — Metadata about the request (browser type, accepted content types, cookies)
4. **Body** — Optional data sent with the request (form data, JSON)

### The Response

The server processes the request and sends back an HTTP response containing:

1. **Status Code** — A three-digit number indicating success or failure
2. **Headers** — Metadata about the response (content type, caching instructions, cookies)
3. **Body** — The actual content (HTML, JSON, images, etc.)

### Common HTTP Methods

| Method | Purpose | Common Use Cases |
|--------|---------|------------------|
| **GET** | Retrieve data | Loading a page, fetching API data |
| **POST** | Submit data | Form submissions, creating resources |
| **PUT** | Replace data | Updating an entire record |
| **PATCH** | Partially update data | Modifying specific fields |
| **DELETE** | Remove data | Deleting a record |

### Common Status Codes

| Code | Meaning | Example |
|------|---------|---------|
| **200** | Success | Page loaded successfully |
| **201** | Created | New resource was created |
| **301/302** | Redirect | Page moved permanently/temporarily |
| **400** | Bad Request | Invalid form data |
| **401/403** | Unauthorized/Forbidden | Login required / access denied |
| **404** | Not Found | URL doesn't exist |
| **500** | Server Error | Application crashed |

> **💡 Tip:** Think of HTTP like a postal service — the request is like a letter with a return address (headers), the method is the type of service (express, certified, etc.), and the response is the letter you get back.

## Code Walkthrough

Flask handles HTTP requests and responses automatically, but you can inspect and manipulate them. Here is a Flask application that demonstrates different HTTP methods and returns different status codes:

```python
# http_demo.py — Demonstrating HTTP concepts in Flask
from flask import Flask, request, jsonify, abort

app = Flask(__name__)

# Sample data store (in production, use a database)
items = {"1": {"name": "Apple", "price": 1.50}}

# GET request — Retrieve data
@app.route("/items", methods=["GET"])
def get_items():
    """Handle GET request: return all items."""
    # Flask automatically sets status code 200 and Content-Type to JSON
    return jsonify(items), 200  # (200 = OK)

# POST request — Create data
@app.route("/items", methods=["POST"])
def create_item():
    """Handle POST request: create a new item."""
    data = request.get_json()  # Get JSON data from request body
    
    if not data or "name" not in data:
        # Return 400 Bad Request if validation fails
        return jsonify({"error": "Missing 'name' field"}), 400
    
    item_id = str(len(items) + 1)  # Generate simple ID
    items[item_id] = {"name": data["name"], "price": data.get("price", 0.00)}
    
    # Return 201 Created with the new resource
    return jsonify({"id": item_id, "message": "Item created"}), 201

# GET single item — Demonstrate 404 handling
@app.route("/items/<item_id>", methods=["GET"])
def get_item(item_id):
    """Handle GET request for a specific item."""
    if item_id not in items:
        # Trigger a 404 Not Found error
        abort(404)
    
    return jsonify({"id": item_id, **items[item_id]}), 200

# DELETE request — Remove data
@app.route("/items/<item_id>", methods=["DELETE"])
def delete_item(item_id):
    """Handle DELETE request: remove an item."""
    if item_id not in items:
        abort(404)
    
    del items[item_id]
    
    # Return 204 No Content (successful but no response body)
    return "", 204

if __name__ == "__main__":
    app.run(debug=True)
```

### Line-by-Line Breakdown

- `from flask import Flask, request, jsonify, abort` — Imports Flask components: `request` gives access to incoming request data, `jsonify` converts Python dicts to JSON responses, and `abort` triggers HTTP error responses.
- `methods=["GET"]` — Specifies which HTTP methods this route accepts. Without this, only GET is allowed by default.
- `request.get_json()` — Parses the request body as JSON and returns a Python dictionary.
- `return jsonify(items), 200` — `jsonify()` creates a JSON response; the second argument sets the HTTP status code explicitly.
- `abort(404)` — Immediately stops execution and returns a 404 Not Found response to the client.
- `return "", 204` — Returns an empty body with 204 No Content status (common for DELETE operations).

## Common Mistakes

❌ **Returning a dictionary directly without jsonify**
```python
# WRONG — Flask won't automatically convert dict to JSON in all cases
@app.route("/api/data")
def get_data():
    return {"key": "value"}  # May cause unexpected behavior
```

✅ **Correct — Always use jsonify for JSON responses**
```python
# CORRECT — jsonify ensures proper JSON response with correct headers
@app.route("/api/data")
def get_data():
    return jsonify({"key": "value"})
```

❌ **Ignoring HTTP method differences**
```python
# WRONG — A route that accepts both GET and POST but handles them the same way
# This mixes retrieval and submission logic, which is confusing and insecure
@app.route("/submit")
def submit():
    data = request.form["data"]
    # Process data...
```

✅ **Correct — Separate routes for different methods or handle them explicitly**
```python
# CORRECT — Clear separation of concerns by HTTP method
@app.route("/submit", methods=["POST"])
def submit_post():
    # Handle form submission
    data = request.form["data"]
    return jsonify({"status": "created"})

@app.route("/submit", methods=["GET"])
def submit_form():
    # Show the form
    return render_template("form.html")
```

## Quick Reference

| Concept | Description |
|---------|-------------|
| HTTP Method | Action type: GET (read), POST (create), PUT (replace), PATCH (update), DELETE (remove) |
| Status Code 200 | Request succeeded |
| Status Code 201 | Resource created successfully |
| Status Code 400 | Client sent invalid data |
| Status Code 401/403 | Authentication/authorization failed |
| Status Code 404 | Resource not found |
| Status Code 500 | Server error |
| `request` object | Flask object containing all request data (headers, body, cookies) |
| `jsonify()` | Flask function to create JSON responses |

## Next Steps

Now that you understand how HTTP works, continue to [01_installing_python.md](../02_environment_setup/01_installing_python.md) to set up your development environment by installing Python.