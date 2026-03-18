<!-- FILE: 01_getting_started/01_introduction/01_what_is_flask.md -->

## Overview

**Flask** is a lightweight, beginner-friendly web framework for Python that lets you build web applications quickly with minimal boilerplate code. It provides the essential tools for handling HTTP requests, routing URLs to Python functions, rendering templates, and managing sessions — without imposing a rigid structure on your project. Flask is classified as a "micro-framework," meaning it keeps things simple and extensible, allowing you to add only the features you need as your application grows.

## Prerequisites

- Basic Python programming knowledge (variables, functions, loops)
- Understanding of what a web application does (serves pages to users)
- A computer with Python 3.12 or later installed

## Core Concepts

At its core, Flask is a **WSGI** (Web Server Gateway Interface) application framework. WSGI is a specification that defines how web servers communicate with Python web applications. When a user visits a URL in their browser, the following happens:

1. The browser sends an **HTTP request** to your web server
2. The server (via Flask) receives the request and matches the URL to a **route**
3. A **view function** (a Python function you write) runs and processes the request
4. Flask builds an **HTTP response** (usually HTML, JSON, or a redirect)
5. The browser receives the response and displays it to the user

Flask gives you a simple way to define routes using the `@app.route()` decorator, which maps URLs to your Python functions. It also handles the request/response cycle automatically, so you can focus on writing the logic that makes your application useful.

> **💡 Tip:** Think of Flask as a **dispatcher** — it receives incoming requests and sends them to the right handler function, then sends the response back to the browser.

## Code Walkthrough

Here is a minimal Flask application that displays "Hello, World!" when users visit the home page:

```python
# app.py — A minimal Flask application
from flask import Flask  # Import Flask class from the flask package

app = Flask(__name__)  # Create a Flask application instance; __name__ tells Flask where to find templates/static

@app.route("/")  # Decorator that maps the root URL "/" to the function below
def home():
    """View function that runs when someone visits the home page."""
    return "Hello, World!"  # Return a plain text response

if __name__ == "__main__":  # Only run this when the file is executed directly (not imported)
    app.run(debug=True)     # Start the development server with debug mode enabled
```

### Line-by-Line Breakdown

- `from flask import Flask` — Imports the Flask class from the flask package, which is the foundation of any Flask application.
- `app = Flask(__name__)` — Creates a new Flask application instance. The `__name__` argument tells Flask where to look for templates and static files relative to this file.
- `@app.route("/")` — A **decorator** that registers the following function as a handler for the root URL (`/`). When someone visits `http://yourdomain.com/`, Flask calls this function.
- `def home():` — Defines a **view function** named `home`. This function runs whenever a user visits the mapped URL.
- `return "Hello, World!"` — The view function returns a string, which Flask automatically converts into an HTTP response with status code 200 (OK).
- `if __name__ == "__main__":` — Ensures the development server only starts when this file is run directly (not when imported as a module).
- `app.run(debug=True)` — Starts Flask's built-in development server. The `debug=True` option enables auto-reloading and detailed error pages during development.

## Common Mistakes

❌ **Forgetting the `@app.route()` decorator**
```python
# WRONG — No route defined, so this function can never be reached via a URL
def home():
    return "Hello!"
```

✅ **Correct — Route decorator maps URL to function**
```python
# CORRECT — The route makes this function accessible at the specified URL
@app.route("/")
def home():
    return "Hello!"
```

❌ **Running Flask in production with `debug=True`**
```python
# WRONG — Debug mode exposes sensitive information and should never be used in production
app.run(debug=True)
```

✅ **Correct — Use environment-based configuration for production**
```python
# CORRECT — Set debug=False for production, or better yet, use a production WSGI server (see Chapter 10)
if __name__ == "__main__":
    app.run(debug=False)
```

## Quick Reference

| Concept | Description |
|---------|-------------|
| `Flask(__name__)` | Creates the application instance |
| `@app.route("/")` | Maps a URL to a view function |
| View function | Python function that handles a request and returns a response |
| `app.run()` | Starts the development server |
| `debug=True` | Enables auto-reload and detailed error pages (development only) |

## Next Steps

Now that you understand what Flask is and have seen a minimal application, continue to [02_flask_vs_django.md](02_flask_vs_django.md) to learn how Flask compares to Django and when to choose each framework.