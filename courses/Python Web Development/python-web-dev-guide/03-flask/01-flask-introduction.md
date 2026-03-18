# Flask Introduction

## What You'll Learn
- What Flask is and when to use it
- Installing Flask and creating your first app
- Understanding the Flask application structure
- Running the development server
- Debug mode and auto-reload

## Prerequisites
- Completed Python fundamentals
- Virtual environment set up
- Basic understanding of HTTP requests

## What Is Flask?

**Flask** is a lightweight Python web framework. It's classified as a "micro-framework" because it provides only the essential tools for web development, leaving you free to add what you need.

Think of Flask like a toolkit:
- It has the basics: routing, request handling, templates
- It doesn't force a specific database or authentication system on you
- You add extensions as needed

### When to Use Flask

- **Learning web development** — Simple, easy to understand
- **Small to medium applications** — Don't need Django's complexity
- **Microservices** — Lightweight API services
- **Prototypes and MVPs** — Fast to build and iterate
- **Custom requirements** — Full control over architecture

## Installing Flask

First, create and activate a virtual environment:

```bash
python -m venv venv
# Windows:
venv\Scripts\activate
# macOS/Linux:
source venv/bin/activate
```

Then install Flask:

```bash
pip install flask   # Install Flask web framework
```

What just happened:
- `pip` downloaded Flask from PyPI
- Flask and its dependencies (Werkzeug, Jinja2, click) were installed
- You can now import Flask in your Python code

## Your First Flask Application

Create a file called `app.py`:

```python
from flask import Flask

app = Flask(__name__)

@app.route("/")
def home() -> str:
    return "<h1>Hello, World!</h1><p>Welcome to my Flask app!</p>"

@app.route("/about")
def about() -> str:
    return "<h1>About</h1><p>This is my first Flask application.</p>"

if __name__ == "__main__":
    app.run(debug=True)
```

🔍 **Line-by-Line Breakdown:**

1. `from flask import Flask` — Imports the Flask class from the flask package
2. `app = Flask(__name__)` — Creates a Flask application instance. `__name__` tells Flask where to find templates and static files
3. `@app.route("/")` — A **decorator** that maps the URL "/" to the function below. This is called a **route**
4. `def home() -> str:` — A **view function** that returns a string. The `-> str` is a type hint
5. `return "<h1>Hello...</h1>"` — This HTML is sent back to the browser as the response
6. `@app.route("/about")` — Another route for the /about page
7. `if __name__ == "__main__":` — Ensures the server only runs when the file is executed directly
8. `app.run(debug=True)` — Starts the development server with debug mode enabled

## Running the Application

```bash
python app.py
```

You'll see output like:

```
 * Serving Flask app 'app'
 * Debug mode: on
 * WARNING: This is a development server. Do not use in production.
 * Running on http://127.0.0.1:5000
```

Open your browser to `http://127.0.0.1:5000` — you should see "Hello, World!"

## How Flask Handles Requests

Here's what happens when you visit `http://127.0.0.1:5000/`:

```
1. Browser sends HTTP request to http://127.0.0.1:5000/
2. Flask receives the request
3. Flask matches the URL "/" to the home() function
4. home() returns an HTML string
5. Flask wraps it in an HTTP response
6. Browser receives and displays the HTML
```

## Understanding Routes

A **route** is a URL pattern that maps to a function. Flask uses decorators to define routes:

```python
@app.route("/")           # Root URL
@app.route("/about")      # /about page
@app.route("/contact")    # /contact page
```

### Dynamic Routes

Routes can have variable parts:

```python
@app.route("/user/<username>")
def user_profile(username: str) -> str:
    return f"<h1>Profile: {username}</h1>"

@app.route("/post/<int:post_id>")
def show_post(post_id: int) -> str:
    return f"<h1>Post #{post_id}</h1>"

@app.route("/path/<path:subpath>")
def show_path(subpath: str) -> str:
    return f"<h1>Path: {subpath}</h1>"
```

🔍 **Route Converters:**

1. `<username>` — String (default)
2. `<int:post_id>` — Integer (converts "42" to 42)
3. `<float:value>` — Float
4. `<path:subpath>` — Path (includes slashes)

## HTTP Methods

By default, routes only respond to GET requests. You can specify other methods:

```python
from flask import request, jsonify

@app.route("/submit", methods=["POST"])
def submit_form() -> dict:
    # Get form data
    name: str = request.form.get("name", "")
    email: str = request.form.get("email", "")
    
    # Process data (save to database, etc.)
    result: dict = {
        "status": "success",
        "message": f"Thank you, {name}!",
        "email": email
    }
    
    return jsonify(result)

@app.route("/api/data", methods=["GET", "POST"])
def api_handler() -> dict:
    if request.method == "GET":
        return {"data": "Here's the data"}
    else:
        # POST request
        data: dict = request.get_json()
        return {"received": data}
```

🔍 **HTTP Methods:**

1. **GET** — Retrieve data (visiting a page)
2. **POST** — Send data to be processed (form submission)
3. **PUT** — Update existing data
4. **DELETE** — Remove data
5. **PATCH** — Partially update data

## Debug Mode

`debug=True` enables:
- Auto-reload when code changes
- Detailed error pages
- Interactive debugger in browser

⚠️ **Never use debug=True in production!**

## The Request Object

Flask provides a global `request` object with information about the current request:

```python
from flask import request

@app.route("/inspect")
def inspect_request() -> str:
    # Access different parts of the request
    path: str = request.path           # The URL path
    method: str = request.method       # GET, POST, etc.
    args: dict = request.args          # Query parameters (?key=value)
    headers: dict = dict(request.headers)  # HTTP headers
    cookies: dict = request.cookies    # Cookies
    
    return f"""
    <h1>Request Info</h1>
    <p>Path: {path}</p>
    <p>Method: {method}</p>
    <p>Args: {args}</p>
    """
```

## The Response Object

Flask automatically creates responses, but you can customize them:

```python
from flask import make_response, jsonify

@app.route("/custom-response")
def custom_response() -> Response:
    response: Response = make_response("<h1>Custom Response</h1>")
    response.status_code = 200
    response.headers["X-Custom-Header"] = "Hello"
    return response

@app.route("/json-response")
def json_response() -> dict:
    return jsonify({"message": "Hello", "status": "ok"})
    # Or simply: return {"message": "Hello", "status": "ok"}
```

## Complete Example: A Simple API

```python
from flask import Flask, jsonify, request

app = Flask(__name__)

# In-memory database
books: list[dict[str, str | int]] = [
    {"id": 1, "title": "Python Basics", "author": "Alice"},
    {"id": 2, "title": "Flask Web Dev", "author": "Bob"},
]

# Get all books
@app.route("/api/books", methods=["GET"])
def get_books() -> dict:
    return jsonify({"books": books})

# Get single book
@app.route("/api/books/<int:book_id>", methods=["GET"])
def get_book(book_id: int) -> dict | tuple:
    book: dict | None = next((b for b in books if b["id"] == book_id), None)
    if book is None:
        return jsonify({"error": "Book not found"}), 404
    return jsonify(book)

# Add a book
@app.route("/api/books", methods=["POST"])
def add_book() -> tuple:
    data: dict = request.get_json()
    new_book: dict[str, str | int] = {
        "id": len(books) + 1,
        "title": data.get("title", ""),
        "author": data.get("author", "")
    }
    books.append(new_book)
    return jsonify(new_book), 201

# Delete a book
@app.route("/api/books/<int:book_id>", methods=["DELETE"])
def delete_book(book_id: int) -> tuple:
    global books
    initial_count: int = len(books)
    books = [b for b in books if b["id"] != book_id]
    
    if len(books) == initial_count:
        return jsonify({"error": "Book not found"}), 404
    return jsonify({"message": "Deleted"}), 200

if __name__ == "__main__":
    app.run(debug=True)
```

Run this and test with:
- GET http://127.0.0.1:5000/api/books
- POST to http://127.0.0.1:5000/api/books with JSON `{"title": "New Book", "author": "Author"}`

## Summary
- **Flask** is a lightweight, flexible web framework
- Install with `pip install flask`
- Use **decorators** (`@app.route`) to define routes
- Routes can have **dynamic segments** (`<int:id>`)
- Use different **HTTP methods** for different actions
- `debug=True` enables auto-reload and helpful error messages

## Next Steps
→ Continue to `02-routing-and-views.md` to learn more about routing patterns and view functions.
