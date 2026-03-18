# Python for the Web

## What You'll Learn
- Why Python is excellent for web development
- An overview of Python web frameworks
- When to use Flask vs FastAPI vs Django
- The request-response cycle in Python web apps

## Prerequisites
- Basic understanding of web development (from previous chapter)
- Familiarity with Python syntax

## Why Python Excels at Web Development

Python was created in 1991 by Guido van Rossum with a philosophy emphasizing code readability. This philosophy makes Python particularly well-suited for web development:

### 1. Readable Code Means Maintainable Code

Web applications often involve complex logic. Python's clean syntax helps developers understand and maintain code over time. When you're building a web app at 2 AM debugging an issue, you'll appreciate code that reads like English.

### 2. Batteries-Included Standard Library

Python comes with everything you need built-in:

```python
# Need to handle dates? Python has datetime
from datetime import datetime, timedelta

# Need to work with JSON? It's built-in
import json

# Need to send HTTP requests? urllib is there
from urllib.request import urlopen
```

### 3. Rich Third-Party Ecosystem

Need something specific? There's probably a Python package for it:

```bash
pip install flask        # Web framework
pip install fastapi      # Modern API framework  
pip install sqlalchemy  # Database ORM
pip install pydantic    # Data validation
pip install httpx        # HTTP client
```

## Python Web Frameworks Overview

Python has several web frameworks, each designed for different use cases:

### Flask — The Lightweight Choice

**Flask** is a micro-framework. "Micro" means it provides only the essentials, and you add what you need.

```python
from flask import Flask

app = Flask(__name__)

@app.route("/")
def hello() -> str:
    return "Hello, World!"

# That's it! That's a web server.
```

**When to use Flask:**
- Learning web development
- Small to medium applications
- Prototypes and MVPs
- When you want full control over components

### FastAPI — The Modern Choice

**FastAPI** is a modern framework (released in 2018) designed for building APIs. It's known for being incredibly fast and having automatic documentation.

```python
from fastapi import FastAPI

app = FastAPI()

@app.get("/")
def hello() -> dict[str, str]:
    return {"message": "Hello, World!"}

# That's it! Run it and visit /docs for auto-generated API docs
```

**When to use FastAPI:**
- Building REST APIs
- Need high performance
- Want automatic request validation
- Building microservices
- Need async/await support

### Django — The Full-Featured Choice

**Django** is a "batteries-included" framework. It comes with everything: authentication, admin panel, database ORM, forms, and more.

```python
# Django requires more setup, but provides more out of the box
# models.py
from django.db import models

class Post(models.Model):
    title = models.CharField(max_length=200)
    content = models.TextField()
    created_at = models.DateTimeField(auto_now_add=True)
```

**When to use Django:**
- Large, complex applications
- Need built-in admin panel
- Enterprise applications
- Content management systems

## Comparing the Frameworks

| Feature | Flask | FastAPI | Django |
|---------|-------|---------|--------|
| Learning curve | Low | Low | Medium-High |
| Speed | Good | Excellent | Good |
| Built-in admin | No | No | Yes |
| Database ORM | Optional | Optional | Built-in |
| Async support | Via extensions | Native | Via channels |
| API documentation | Manual | Auto | Via DRF |
| Best for | Beginners, small apps | APIs, async apps | Large apps |

## The Request-Response Cycle

Every Python web framework follows the same basic pattern:

```
Client Request → Web Server → Your Python Code → Response → Client
```

Let's trace through a typical request:

```python
# This is Flask, but FastAPI follows the same pattern
from flask import Flask, request, jsonify

app = Flask(__name__)

# 1. User visits /greet/Alice
@app.route("/greet/<name>")
def greet(name: str) -> str:
    # 2. Flask calls your function with the URL parameter
    # 3. Your function processes it and returns a string
    return f"Hello, {name}!"

# POST request example
@app.route("/submit", methods=["POST"])
def submit() -> dict:
    # 1. User submits a form
    # 2. request.json contains the parsed JSON body
    data = request.json
    # 3. Process the data
    result = {"status": "success", "name": data.get("name")}
    # 4. Return JSON response
    return jsonify(result)
```

## How Python Web Servers Work

### WSGI (Web Server Gateway Interface)

**WSGI** is the standard interface between Python web servers and web applications. When you run a Flask or Django app, it communicates with the web server via WSGI.

```
[Nginx/Apache] → [WSGI] → [Flask/Django App]
```

### ASGI (Asynchronous Server Gateway Interface)

**ASGI** is the async successor to WSGI. FastAPI uses ASGI, which allows handling thousands of concurrent connections efficiently.

```
[Nginx/Apache] → [ASGI] → [FastAPI App]
```

## Installing Your First Web Framework

Let's install Flask to get started:

```bash
pip install flask   # Flask is a lightweight web framework
```

What just happened:
- `pip` is Python's package installer
- It downloaded Flask and its dependencies from PyPI (Python Package Index)
- Flask is now available to import in your Python code

## Your First Python Web App

Create a file called `app.py`:

```python
from flask import Flask, render_template_string

app = Flask(__name__)

@app.route("/")
def home() -> str:
    return "<h1>Hello, World!</h1><p>Welcome to my first web app!</p>"

@app.route("/about")
def about() -> str:
    return "<h1>About Me</h1><p>I'm learning Python web development!</p>"

if __name__ == "__main__":
    # This runs the development server
    app.run(debug=True)
```

Run it:

```bash
python app.py
```

Open your browser to `http://127.0.0.1:5000/`

🔍 **Line-by-Line Breakdown:**

1. `from flask import Flask, render_template_string` — Imports the Flask class and a helper for rendering templates. Flask is the web framework.
2. `app = Flask(__name__)` — Creates a Flask application instance. `__name__` tells Flask where to find templates and static files.
3. `@app.route("/")` — A **decorator** that maps the URL `/` to the function below. When someone visits the homepage, this function runs.
4. `def home() -> str:` — A view function that returns a string. The `-> str` is a **type hint** telling us it returns a string.
5. `return "<h1>Hello, World!</h1>..."` — This HTML is sent back to the browser. Flask handles converting Python strings to HTTP responses.
6. `if __name__ == "__main__":` — This block only runs when you execute the file directly (not when imported).
7. `app.run(debug=True)` — Starts the development server. `debug=True` enables auto-reload and detailed error pages.

## Summary
- Python is excellent for web development due to its readability and ecosystem
- **Flask** is lightweight and great for learning
- **FastAPI** is modern and excellent for APIs
- **Django** is full-featured and great for large apps
- All Python web frameworks follow the request-response cycle
- WSGI (Flask/Django) and ASGI (FastAPI) handle communication with web servers

## Next Steps
→ Continue to `03-setting-up-your-environment.md` to set up your development environment properly.
