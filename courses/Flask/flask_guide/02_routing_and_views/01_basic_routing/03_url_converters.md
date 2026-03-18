<!-- FILE: 02_routing_and_views/01_basic_routing/03_url_converters.md -->

## Overview

**URL converters** are Flask's built-in mechanism for validating and converting URL variables. You already saw some in action — like `<int:post_id>` converting a URL segment to an integer. This file dives deeper into how converters work, explores all built-in converters, and shows how to create **custom converters** for complex URL patterns like slugs, hashes, or custom formats.

## Prerequisites

- Understanding of dynamic routes (from the previous file)
- Basic Python class and regex knowledge
- Familiarity with Flask routing

## Core Concepts

### How Converters Work

When Flask receives a request, it:
1. Parses the URL path
2. Iterates through registered routes
3. For each route, tries to match the URL against the pattern
4. If a converter is present, it runs the converter's regex and type conversion
5. If conversion succeeds, the value is passed to the view function
6. If conversion fails, the route doesn't match (Flask tries the next route)

### Built-in Converter Classes

Flask uses Werkzeug's routing system, which provides these converters:

| Converter | Class | Regex Pattern | Output Type |
|-----------|-------|---------------|-------------|
| String | `UnicodeConverter` | `[^/]+` | `str` |
| Integer | `IntegerConverter` | `-?\d+` | `int` |
| Float | `FloatConverter` | `-?\d+\.?\d*` | `float` |
| UUID | `UUIDConverter` | `[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}` | `uuid.UUID` |
| Path | `PathConverter` | `.+` (including `/`) | `str` |

### Custom Converters

For advanced routing needs, you can create custom converters. This is useful for:
- URL slugs (alphanumeric with hyphens)
- Custom date formats
- Base64 encoded IDs
- Limited vocabulary patterns

## Code Walkthrough

### Using All Built-in Converters

```python
# converters_demo.py — Demonstrating all built-in converters
from flask import Flask, jsonify
import uuid  # For UUID examples

app = Flask(__name__)

# 1. String converter (default) - matches any characters except /
@app.route("/user/<username>")
def get_user(username):
    """Get user by username (string)."""
    return jsonify({"user": username, "type": type(username).__name__})

# 2. Integer converter - only matches digits
@app.route("/post/<int:post_id>")
def get_post(post_id):
    """Get post by ID (integer)."""
    return jsonify({"post_id": post_id, "type": type(post_id).__name__})

# 3. Float converter - matches decimal numbers
@app.route("/price/<float:price>")
def get_price(price):
    """Get price (float)."""
    return jsonify({"price": price, "type": type(price).__name__})

# 4. UUID converter - matches UUID format
@app.route("/item/<uuid:item_id>")
def get_item(item_id):
    """Get item by UUID."""
    # item_id is already a uuid.UUID object!
    return jsonify({"item_id": str(item_id), "type": type(item_id).__name__})

# 5. Path converter - matches including /
@app.route("/files/<path:filepath>")
def get_file(filepath):
    """Get file path (including subdirectories)."""
    return jsonify({"filepath": filepath, "type": type(filepath).__name__})

# 6. Any converter - matches specific options
@app.route("/<any(articles,posts):section>/<int:id>")
def get_content(section, id):
    """Get content from specific section."""
    return jsonify({"section": section, "id": id, "type": type(id).__name__})

if __name__ == "__main__":
    app.run(debug=True)
```

### Creating Custom Converters

For more complex URL patterns, create a custom converter:

```python
# custom_converters.py — Creating custom URL converters
from flask import Flask
from werkzeug.routing import BaseConverter  # Base class for converters

app = Flask(__name__)

# Custom converter for URL slugs (lowercase letters, numbers, and hyphens)
class SlugConverter(BaseConverter):
    """Custom converter for URL-friendly slugs.
    
    Matches patterns like 'my-blog-post', 'hello-world-123'
    """
    # This regex only matches lowercase letters, numbers, and hyphens
    regex = r'[a-z0-9]+(?:-[a-z0-9]+)*'
    
    # Optional: customize weight (higher = checked first)
    weight = 100

# Custom converter for base64 IDs
class Base64Converter(BaseConverter):
    """Custom converter for base64-encoded IDs.
    
    Matches patterns like 'YWJjZA==' (base64 for 'abcd')
    """
    regex = r'[A-Za-z0-9+/]+=*'

# Custom converter for limited choice with custom mapping
class ColorConverter(BaseConverter):
    """Custom converter for color codes."""
    # This uses a custom function to validate
    regex = r'(?:red|green|blue|yellow|purple)'

# Register custom converters with the app
app.url_map.converters['slug'] = SlugConverter
app.url_map.converters['base64'] = Base64Converter
app.url_map.converters['color'] = ColorConverter

# Using custom converters in routes
@app.route("/blog/<slug:post_slug>")
def blog_post(post_slug):
    """Blog post URL like /blog/my-first-post."""
    return f"<h1>Blog Post</h1><p>Slug: {post_slug}</p>"

@app.route("/data/<base64:encoded_id>")
def get_data(encoded_id):
    """Get data by base64-encoded ID."""
    return f"<h1>Data</h1><p>Encoded ID: {encoded_id}</p>"

@app.route("/theme/<color:bg_color>")
def theme(bg_color):
    """Set theme by color."""
    return f"<h1>Theme</h1><p>Background: {bg_color}</p>"

if __name__ == "__main__":
    app.run(debug=True)
```

### Line-by-Line Breakdown (Custom Converter)

- `from werkzeug.routing import BaseConverter` — Imports the base converter class.
- `class SlugConverter(BaseConverter):` — Creates a custom converter inheriting from BaseConverter.
- `regex = r'[a-z0-9]+(?:-[a-z0-9]+)*'` — Defines the regex pattern that validates URL segments.
- `app.url_map.converters['slug'] = SlugConverter` — Registers the converter so Flask recognizes `slug:` in routes.
- `def blog_post(post_slug):` — The converted value is automatically passed as the argument.

### Testing Custom Converters

```bash
# Test slug converter
curl http://127.0.0.1:5000/blog/my-first-post
# Output: <h1>Blog Post</h1><p>Slug: my-first-post</p>

# Test invalid slug (uppercase would fail)
curl http://127.0.0.1:5000/blog/My-First-Post
# Output: 404 Not Found (doesn't match lowercase regex)

# Test base64 converter
curl http://127.0.0.1:5000/data/YWJjZA==
# Output: <h1>Data</h1><p>Encoded ID: YWJjZA==</p>
```

## Common Mistakes

❌ **Not registering custom converters**
```python
# WRONG — Converter defined but not registered
class MyConverter(BaseConverter):
    regex = r'...'

@app.route("/item/<my:item_id>")  # Flask won't recognize 'my:'
def get_item(item_id):
    return str(item_id)
```

✅ **Correct — Register the converter**
```python
# CORRECT — Register before using
app.url_map.converters['my'] = MyConverter

@app.route("/item/<my:item_id>")  # Now works!
def get_item(item_id):
    return str(item_id)
```

❌ **Using overly permissive regex**
```python
# WRONG — Too broad, could match unintended URLs
class BadConverter(BaseConverter):
    regex = r'.*'  # Matches everything, loses validation benefit
```

✅ **Correct — Be specific with regex**
```python
# CORRECT — Specific pattern provides validation
class GoodConverter(BaseConverter):
    regex = r'[a-z]{8}'  # Exactly 8 lowercase letters
```

## Quick Reference

| Task | Code |
|------|------|
| Use integer | `<int:id>` |
| Use float | `<float:price>` |
| Use UUID | `<uuid:item_id>` |
| Use path | `<path:filepath>` |
| Use limited options | `<any(a,b):choice>` |
| Create custom converter | Define class inheriting from `BaseConverter` and register with `app.url_map.converters['name']` |

## Next Steps

Now you understand routing and URL converters. Continue to [01_get_and_post.md](../../02_routing_and_views/02_http_methods/01_get_and_post.md) to learn how to handle different HTTP methods like GET and POST in your Flask routes.