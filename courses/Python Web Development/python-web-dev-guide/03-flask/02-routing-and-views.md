# Routing and Views

## What You'll Learn
- Advanced routing patterns
- URL converters and validation
- Building view functions
- Redirects and errors
- URL building with url_for
- Blueprints for modular applications

## Prerequisites
- Completed Flask Introduction
- Understanding of HTTP methods

## Advanced Routing

### URL Variables with Converters

Flask can automatically convert URL segments to specific types:

```python
from flask import Flask

app = Flask(__name__)

# String (default)
@app.route("/user/<username>")
def user_profile(username: str) -> str:
    return f"<h1>User: {username}</h1>"

# Integer
@app.route("/post/<int:post_id>")
def show_post(post_id: int) -> str:
    return f"<h1>Post #{post_id}</h1>"

# Float
@app.route("/price/<float:price>")
def show_price(price: float) -> str:
    return f"<h1>Price: ${price:.2f}</h1>"

# Path (includes slashes)
@app.route("/path/<path:filepath>")
def show_path(filepath: str) -> str:
    return f"<h1>File: {filepath}</h1>"

# UUID (universally unique identifier)
@app.route("/item/<uuid:item_id>")
def show_item(item_id: str) -> str:
    return f"<h1>Item: {item_id}</h1>"
```

🔍 **URL Converters:**

| Converter | Syntax | Example | Value Type |
|-----------|--------|---------|------------|
| String | `<name>` | `/user/alice` | `str` |
| Integer | `<int:n>` | `/post/42` | `int` |
| Float | `<float:n>` | `/price/19.99` | `float` |
| Path | `<path:n>` | `/path/a/b/c` | `str` (includes `/`) |
| UUID | `<uuid:n>` | `/item/abc-123` | `str` (UUID format) |

### Multiple Rules Per Function

A single view function can handle multiple URL patterns:

```python
@app.route("/hello")
@app.route("/hello/<name>")
def hello(name: str | None = None) -> str:
    if name is None:
        name = "World"
    return f"<h1>Hello, {name}!</h1>"
```

### HTTP Methods

Specify which HTTP methods a route accepts:

```python
from flask import request, jsonify

@app.route("/api/data", methods=["GET"])
def get_data() -> dict:
    return {"data": [1, 2, 3]}

@app.route("/api/data", methods=["POST"])
def create_data() -> dict:
    data: dict = request.get_json()
    return {"created": data}, 201

# Shorthand methods
@app.get("/api/items")
def get_items() -> dict:
    return {"items": ["a", "b", "c"]}

@app.post("/api/items")
def create_item() -> dict:
    return {"created": True}, 201
```

## Building URLs with url_for

`url_for()` generates URLs from route functions — more maintainable than hardcoding URLs:

```python
from flask import Flask, url_for

app = Flask(__name__)

@app.route("/")
def home() -> str:
    # Generate URLs for other routes
    home_url: str = url_for("home")
    about_url: str = url_for("about")
    user_url: str = url_for("user_profile", username="alice")
    
    return f"""
    <h1>Home</h1>
    <ul>
        <li>Home: {home_url}</li>
        <li>About: {about_url}</li>
        <li>User: {user_url}</li>
    </ul>
    """

@app.route("/about")
def about() -> str:
    return "<h1>About</h1>"

@app.route("/user/<username>")
def user_profile(username: str) -> str:
    return f"<h1>User: {username}</h1>"
```

🔍 **url_for Benefits:**

1. **DRY principle** — Change URL without updating every link
2. **Automatic escaping** — Handles special characters
3. **External URLs** — Can generate URLs for other apps
4. **Blueprint support** — Works with modular applications

## Redirects

Redirect users to different URLs:

```python
from flask import redirect, url_for, abort

@app.route("/old-page")
def old_page() -> Response:
    # Permanent redirect (301) - for SEO
    return redirect(url_for("new_page"), code=301)

@app.route("/new-page")
def new_page() -> str:
    return "<h1>New Page</h1>"

@app.route("/temp-redirect")
def temp_redirect() -> Response:
    # Temporary redirect (302)
    return redirect(url_for("home"))

@app.route("/login")
def login() -> str:
    # Redirect with parameters
    next_page: str = request.args.get("next", "/")
    return redirect(url_for("home") + "?redirected=true")

@app.route("/error-example")
def error_example() -> Response:
    # Raise HTTP errors
    abort(404)  # Not Found
    # Or:
    abort(500)  # Internal Server Error
```

## Custom Error Pages

Create custom pages for HTTP errors:

```python
from flask import render_template_string

@app.errorhandler(404)
def page_not_found(e) -> tuple:
    return "<h1>404 - Page Not Found</h1><p>The page doesn't exist.</p>", 404

@app.errorhandler(500)
def internal_error(e) -> tuple:
    return "<h1>500 - Server Error</h1><p>Something went wrong.</p>", 500

# Using templates (explained in next section)
@app.errorhandler(404)
def page_not_found(e) -> tuple:
    return render_template("404.html"), 404
```

## Blueprints: Modular Applications

**Blueprints** organize Flask apps into modular components. Think of them like plugins:

### Creating a Blueprint

```python
# blog.py
from flask import Blueprint, render_template

blog = Blueprint("blog", __name__, url_prefix="/blog")

@blog.route("/")
def index() -> str:
    return "<h1>Blog Index</h1>"

@blog.route("/<int:post_id>")
def post(post_id: int) -> str:
    return f"<h1>Blog Post #{post_id}</h1>"

@blog.route("/create")
def create() -> str:
    return "<h1>Create New Post</h1>"
```

### Registering a Blueprint

```python
# app.py
from flask import Flask
from blog import blog
from admin import admin

app = Flask(__name__)

# Register blueprints
app.register_blueprint(blog)
app.register_blueprint(admin)

@app.route("/")
def home() -> str:
    return "<h1>Home Page</h1>"

if __name__ == "__main__":
    app.run(debug=True)
```

🔍 **Blueprint Structure:**

1. `Blueprint("name", __name__, url_prefix="/path")` — Creates a blueprint with optional URL prefix
2. `@blog.route()` — Defines routes within the blueprint
3. `app.register_blueprint(blog)` — Adds the blueprint to the Flask app

### Multiple Blueprints Example

```python
# admin.py
from flask import Blueprint

admin = Blueprint("admin", __name__, url_prefix="/admin")

@admin.route("/")
def dashboard() -> str:
    return "<h1>Admin Dashboard</h1>"

@admin.route("/users")
def users() -> str:
    return "<h1>User Management</h1>"
```

Now URLs are:
- `/` — home
- `/blog/` — blog index
- `/blog/42` — blog post #42
- `/admin/` — admin dashboard

## View Function Best Practices

### Return Type Hints

```python
from flask import Response

@app.route("/")
def home() -> str:
    return "<h1>Hello</h1>"

@app.route("/json")
def json_view() -> dict:
    return {"message": "Hello"}

@app.route("/custom")
def custom_view() -> Response:
    response = Response("<h1>Custom</h1>")
    response.headers["X-Custom"] = "value"
    return response
```

### Organize with Blueprints

```python
# Project structure:
# app/
#   __init__.py
#   main.py
#   routes/
#     __init__.py
#     blog.py
#     admin.py
#   templates/
#   static/

# app/routes/blog.py
from flask import Blueprint

bp = Blueprint("blog", __name__)

@bp.route("/")
def index():
    return "Blog index"

# app/routes/__init__.py
from .blog import bp as blog_bp

def register_routes(app):
    app.register_blueprint(blog_bp)
```

## Summary
- Use **URL converters** (`<int:id>`, `<float:price>`) to validate and convert input
- Use `url_for()` to generate URLs — more maintainable than hardcoding
- Use `redirect()` to send users to different URLs
- Use `@app.errorhandler()` for custom error pages
- Use **Blueprints** to organize larger applications into modules
- Blueprints can have their own **URL prefixes**

## Next Steps
→ Continue to `03-templates-with-jinja2.md` to learn how to use Jinja2 templates for dynamic HTML.
