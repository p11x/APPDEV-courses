<!-- FILE: 01_getting_started/03_first_app/01_hello_world.md -->

## Overview

Your first Flask application starts here. In this file, you will create a minimal "Hello, World!" Flask app, run the development server, and access it in your web browser. This is the traditional first program when learning any web framework, and it verifies that your environment is set up correctly before you dive into more complex features.

## Prerequisites

- Python 3.9+ installed
- A virtual environment created and activated
- Flask installed in your virtual environment (from the previous section)

## Core Concepts

### The Flask Application Object

The core of any Flask application is the `Flask` instance (usually named `app`). This object is your web application — it holds all the configuration, routes, and settings. When you create `Flask(__name__)`, Flask:

1. Sets up the application configuration
2. Registers the current directory as the root for finding templates and static files
3. Prepares the routing system
4. Connects to the WSGI server

### The Development Server

Flask includes a built-in development server — a small web server that runs on your computer. When you call `app.run()`, it starts listening for HTTP requests on localhost (127.0.0.1) at port 5000 by default. You can visit `http://127.0.0.1:5000` in your browser to see your application.

### Running vs. Importing

The `if __name__ == "__main__":` block is crucial. This condition is `True` only when you run the file directly (e.g., `python app.py`). If another file imports your app (like when running tests), this block does not run. This prevents the development server from starting during imports.

## Code Walkthrough

### Creating Your First Flask Application

Create a new file called `app.py` in your project folder:

```python
# app.py — Your first Flask application
from flask import Flask  # Import the Flask class from the flask package

# Create a Flask application instance
# __name__ tells Flask where to find templates and static files relative to this file
app = Flask(__name__)

# Define a route for the root URL ("/")
# When someone visits http://yourdomain.com/, this function runs
@app.route("/")
def index():
    """View function that handles requests to the home page."""
    return "Hello, World!"  # Return a plain text response

# Define another route at /greet/<name>
# The <name> part is a dynamic variable that gets passed to the function
@app.route("/greet/<name>")
def greet(name):
    """Greet the user by name."""
    return f"Hello, {name}!"  # f-string inserts the name into the response

# Only run the server when this file is executed directly
if __name__ == "__main__":
    # Start the Flask development server on http://127.0.0.1:5000
    # debug=True enables auto-reload and detailed error pages
    app.run(debug=True)
```

### Running the Application

In your terminal (with the virtual environment activated), run:

```bash
python app.py
```

You will see output like:
```
 * Debug mode: on
 * Running on http://127.0.0.1:5000
 * Press CTRL+C to quit
```

### Viewing in Your Browser

1. Open your web browser
2. Navigate to `http://127.0.0.1:5000` or `http://localhost:5000`
3. You should see "Hello, World!" displayed

Try the dynamic route:
- Go to `http://127.0.0.1:5000/greet/Alice`
- You should see "Hello, Alice!"

### Line-by-Line Breakdown

- `from flask import Flask` — Imports Flask so you can create an application instance.
- `app = Flask(__name__)` — Creates the Flask app; `__name__` helps Flask locate resources like templates.
- `@app.route("/")` — Decorator that maps the root URL to the following function.
- `def index():` — Defines the view function that runs when someone visits "/".
- `return "Hello, World!"` — Returns a string, which Flask converts to an HTTP response.
- `@app.route("/greet/<name>")` — `<name>` is a **URL variable** that captures part of the URL.
- `def greet(name):` — The captured URL segment is passed as the `name` parameter.
- `if __name__ == "__main__":` — Ensures the server only starts when running this file directly.
- `app.run(debug=True)` — Starts the development server with debug features enabled.

## Common Mistakes

❌ **Forgetting to activate the virtual environment**
```bash
# WRONG — Running with system Python, which may not have Flask installed
python app.py
# ModuleNotFoundError: No module named 'flask'
```

✅ **Correct — Activate the venv first**
```bash
# CORRECT — Flask is available in the activated virtual environment
source venv/bin/activate  # or venv\Scripts\activate on Windows
python app.py
```

❌ **Running with debug=True in production**
```bash
# WRONG — Debug mode exposes sensitive information
app.run(debug=True)  # NEVER use in production!
```

✅ **Correct — Set debug=False for production**
```bash
# CORRECT — Use False or remove the debug argument for production
app.run(debug=False)
# Or better: use a production WSGI server like Gunicorn (see Chapter 10)
```

❌ **Not stopping the server before running again**
```bash
# WRONG — Trying to start a second server on the same port
# Error: Port 5000 is already in use
```

✅ **Correct — Stop the current server first**
```bash
# Press CTRL+C to stop the running server
# Then run again
python app.py
```

## Quick Reference

| Command | Description |
|---------|-------------|
| `python app.py` | Run the Flask application |
| `http://127.0.0.1:5000` | Localhost URL (same as localhost:5000) |
| `CTRL+C` | Stop the development server |
| `@app.route("/")` | Decorator to define a route |
| `debug=True` | Enables auto-reload and error pages (development only) |

## Next Steps

Now that you have a working Flask app, continue to [02_debug_mode.md](02_debug_mode.md) to learn more about Flask's debug mode, how it helps during development, and important security considerations.