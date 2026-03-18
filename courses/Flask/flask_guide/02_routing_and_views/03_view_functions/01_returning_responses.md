<!-- FILE: 02_routing_and_views/03_view_functions/01_returning_responses.md -->

## Overview

Every Flask view function must return a **response** — the data sent back to the client. While returning a simple string works for basic cases, Flask provides the `Response` object for fine-grained control over status codes, headers, and content types. This file teaches you how Flask converts different return types into responses and how to create custom responses.

## Prerequisites

- Understanding of Flask routing and view functions
- Basic knowledge of HTTP status codes and headers

## Core Concepts

### How Flask Converts Return Values

Flask is flexible about what view functions can return. It automatically converts:

| Return Type | Converted To |
|-------------|--------------|
| String | Response with string as body, 200 status, text/html content type |
| Dict (in API) | Response with jsonify(), 200 status, application/json |
| Tuple (body, status) | Response with custom status code |
| Tuple (body, status, headers) | Response with custom status and headers |
| Response object | Used directly |
| make_response() result | Modified Response object |

### Response Object

For full control, use the `Response` class:
```python
from flask import Response

response = Response("Hello World", status=200, headers={"X-Custom": "value"})
return response
```

### JSON Responses

For APIs, return JSON using `jsonify()`:
```python
from flask import jsonify

return jsonify({"key": "value"})
```

`jsonify()` automatically:
- Converts Python dicts/lists to JSON
- Sets Content-Type to application/json
- Handles dates, UUIDs, and other types

## Code Walkthrough

### Returning Different Response Types

```python
# responses.py — Demonstrating various response types
from flask import Flask, Response, jsonify, make_response, render_template_string

app = Flask(__name__)

# 1. Return a string (most common)
@app.route("/string")
def return_string():
    """Returns plain text string."""
    return "Hello, World!"  # Flask adds: status=200, Content-Type=text/html

# 2. Return a string with custom status
@app.route("/not-found")
def not_found():
    """Returns 404 status."""
    return "Page not found", 404  # Tuple: (body, status)

# 3. Return a string with headers
@app.route("/custom-header")
def custom_header():
    """Returns response with custom header."""
    return "Check the headers!", 200, {"X-Custom-Header": "my-value"}

# 4. Return JSON (for APIs)
@app.route("/json")
def return_json():
    """Returns JSON response."""
    return jsonify({
        "message": "Success",
        "data": {"name": "Alice", "age": 30}
    })

# 5. Return JSON with custom status
@app.route("/json-error")
def return_json_error():
    """Returns JSON error response."""
    return jsonify({"error": "Invalid input"}), 400

# 6. Create custom Response object
@app.route("/response-object")
def response_object():
    """Returns a Response object with full control."""
    response = Response(
        "Custom response body",
        status=200,
        mimetype="text/plain",
        headers={"X-Custom": "value", "X-Another": "value2"}
    )
    return response

# 7. Using make_response() to modify response
@app.route("/make-response")
def make_response_demo():
    """
    Use make_response() to get a Response object that you can modify.
    This is useful for setting cookies, headers, etc.
    """
    # render_template_string returns HTML; make_response converts it to a Response
    response = make_response(render_template_string("<h1>Hello {{ name }}</h1>", name="Bob"))
    
    # Now we can modify the response
    response.headers["X-Custom"] = "value"
    response.set_cookie("theme", "dark")  # Set a cookie
    
    return response

# 8. Return a tuple with all options
@app.route("/full-control")
def full_control():
    """Returns with body, status, and headers as tuple."""
    return (
        '{"status": "created"}',  # Body
        201,                       # Status code
        {"Content-Type": "application/json", "X-Request-ID": "12345"}  # Headers
    )

# 9. Streaming response (for large files)
@app.route("/stream")
def stream_response():
    """Returns a streaming response for large data."""
    def generate():
        for i in range(10):
            yield f"Line {i}\n"
    
    return Response(generate(), mimetype="text/plain")

# 10. File download response
@app.route("/download")
def download():
    """Returns a file download response."""
    content = "This is file content"
    response = Response(
        content,
        mimetype="text/plain",
        headers={"Content-Disposition": "attachment; filename=example.txt"}
    )
    return response

if __name__ == "__main__":
    app.run(debug=True)
```

### Line-by-Line Breakdown

- `return "Hello, World!"` — Flask automatically wraps strings in a Response with default headers.
- `return "Page not found", 404` — Tuple of (body, status) sets custom status code.
- `jsonify({...})` — Converts Python dict to JSON response with proper headers.
- `Response(...)` — Creates a Response object with full control over all aspects.
- `make_response(...)` — Takes existing return value and converts to Response object for modification.
- `response.set_cookie("theme", "dark")` — Adds a cookie to the response.
- `Response(generate(), ...)` — Streaming response that generates content incrementally.

### Testing Responses

```bash
# String response
curl -i http://127.0.0.1:5000/string
# HTTP/1.1 200 OK
# Content-Type: text/html; charset=utf-8
# Hello, World!

# JSON response
curl -i http://127.0.0.1:5000/json
# HTTP/1.1 200 OK
# Content-Type: application/json
# {"data": {"age": 30, "name": "Alice"}, "message": "Success"}

# Custom headers
curl -i http://127.0.0.1:5000/custom-header
# HTTP/1.1 200 OK
# X-Custom-Header: my-value
```

## Common Mistakes

❌ **Returning a list directly**
```python
# WRONG — Lists aren't automatically JSON-serialized in all contexts
@app.route("/api/items")
def get_items():
    return ["item1", "item2"]  # May cause unexpected behavior
```

✅ **Correct — Use jsonify for JSON**
```python
# CORRECT — jsonify ensures proper JSON response
@app.route("/api/items")
def get_items():
    return jsonify(["item1", "item2"])
```

❌ **Forgetting to set Content-Type for JSON**
```python
# WRONG — Missing Content-Type means browser may not parse as JSON
@app.route("/api/data")
def get_data():
    return '{"key": "value"}'
```

✅ **Correct — Use jsonify**
```python
# CORRECT — jsonify sets Content-Type automatically
@app.route("/api/data")
def get_data():
    return jsonify({"key": "value"})
```

## Quick Reference

| Return Type | Example | Notes |
|-------------|---------|-------|
| String | `return "Hello"` | Default 200, text/html |
| Tuple | `return "Error", 404` | Custom status |
| Tuple | `return "OK", 200, {"X-Header": "value"}` | Full control |
| jsonify | `return jsonify({"key": "value"})` | JSON response |
| Response | `return Response("body", status=200)` | Full control object |

## Next Steps

Now you understand responses. Continue to [02_redirects_and_errors.md](02_redirects_and_errors.md) to learn about redirects and error handling.