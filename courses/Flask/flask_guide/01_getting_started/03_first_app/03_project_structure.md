<!-- FILE: 01_getting_started/03_first_app/03_project_structure.md -->

## Overview

As your Flask application grows beyond a single file, organizing your code becomes critical. A well-structured project makes code easier to find, test, and maintain. This file introduces a basic project structure that scales — starting simple but following conventions that work for larger applications. You will learn where to put Python code, HTML templates, static files (CSS, JavaScript), and configuration.

## Prerequisites

- A working Flask application (from previous files)
- Understanding of basic file organization concepts
- Familiarity with what templates and static files are (conceptually)

## Core Concepts

### Why Project Structure Matters

In a tiny application, everything in one file works fine. But as you add:
- Multiple pages (routes)
- Database models
- Forms
- Authentication
- API endpoints
- Tests

A single `app.py` becomes unmanageable. A good structure:
- Separates different concerns (routes, models, configuration)
- Makes it easy to find where to make changes
- Enables testing
- Scales to hundreds of routes without chaos

### Basic Directory Structure

For a small-to-medium Flask application, this structure works well:

```
my_flask_project/
├── app.py              # Main entry point, creates and runs the app
├── config.py           # Configuration settings
├── requirements.txt   # Python dependencies
├── venv/              # Virtual environment (not committed to git)
├── templates/         # HTML templates
│   └── base.html
├── static/            # CSS, JavaScript, images
│   └── style.css
└── routes/            # Route handlers (optional for larger apps)
    └── main.py
```

### Key Directories Explained

| Directory/File | Purpose |
|----------------|---------|
| `app.py` or `run.py` | Application entry point; creates and runs the Flask app |
| `config.py` | Stores configuration values (secret keys, database URLs) |
| `requirements.txt` | Lists all Python dependencies |
| `templates/` | HTML files that Flask renders |
| `static/` | CSS, JavaScript, images — served as-is |
| `routes/` or `blueprints/` | Organized route handlers (for larger apps) |

> **💡 Tip:** Flask automatically looks for `templates/` and `static/` folders in the same directory as your main application file or in your project root.

## Code Walkthrough

### Creating the Project Structure

Let us build a small project with multiple routes, templates, and static files:

```bash
# Create the directory structure
mkdir -p my_flask_project/templates
mkdir -p my_flask_project/static
mkdir -p my_flask_project/routes
```

### 1. The Configuration File

Create `config.py` to store settings:

```python
# config.py — Application configuration
import os  # Import os to access environment variables

# Base configuration
class Config:
    """Base configuration class with common settings."""
    SECRET_KEY = os.environ.get("SECRET_KEY") or "dev-secret-key-change-in-prod"
    # The secret key signs session cookies and CSRF tokens
    # In production, always set SECRET_KEY via environment variable

# Development configuration
class DevelopmentConfig(Config):
    """Development-specific configuration."""
    DEBUG = True  # Enable debug mode
    SQLALCHEMY_DATABASE_URI = "sqlite:///dev.db"  # Local development database

# Production configuration
class ProductionConfig(Config):
    """Production-specific configuration."""
    DEBUG = False  # Disable debug mode
    # In production, set database URL via environment variable
    SQLALCHEMY_DATABASE_URI = os.environ.get("DATABASE_URL") or "postgresql://user:pass@localhost/db"

# Configuration dictionary for easy switching
config = {
    "development": DevelopmentConfig,
    "production": ProductionConfig,
    "default": DevelopmentConfig
}
```

### 2. The Application Factory (Introduction)

Instead of creating the app at the top level, we use a **factory function** that creates the app. This is best practice for larger apps:

```python
# app.py — Application factory pattern
from flask import Flask  # Import Flask
from config import config  # Import configuration dictionary


def create_app(config_name):
    """
    Application factory function.
    Creates and configures the Flask application instance.
    
    Args:
        config_name: String name of the configuration to use ('development', 'production')
    
    Returns:
        Configured Flask application instance
    """
    app = Flask(__name__)  # Create the Flask app instance
    
    # Load the configuration based on the config_name
    app.config.from_object(config[config_name])
    
    # Define routes directly here for this simple example
    @app.route("/")
    def index():
        """Home page route."""
        return "<h1>Welcome to My Flask App!</h1><p><a href='/hello'>Go to Hello page</a></p>"
    
    @app.route("/hello")
    def hello():
        """Simple hello page."""
        return "<h1>Hello, World!</h1>"
    
    @app.route("/greet/<name>")
    def greet(name):
        """Greet a user by name."""
        return f"<h1>Hello, {name}!</h1>"
    
    return app  # Return the configured app


# Only run when executed directly
if __name__ == "__main__":
    # Use 'development' config by default
    # Change to 'production' for deployment
    app = create_app("development")
    app.run(debug=True)
```

### 3. Templates

Create HTML files in the `templates/` folder:

```html
<!-- templates/base.html — Base template with common layout -->
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>{% block title %}My Flask App{% endblock %}</title>
    <!-- Link to CSS file in static folder -->
    <link rel="stylesheet" href="{{ url_for('static', filename='style.css') }}">
</head>
<body>
    <header>
        <nav>
            <a href="{{ url_for('index') }}">Home</a>
            <a href="{{ url_for('hello') }}">Hello</a>
        </nav>
    </header>
    
    <main>
        {% block content %}{% endblock %}
    </main>
    
    <footer>
        <p>&copy; 2024 My Flask App</p>
    </footer>
</body>
</html>
```

```html
<!-- templates/index.html — Home page template -->
{% extends "base.html" %}

{% block title %}Home - My Flask App{% endblock %}

{% block content %}
<h1>Welcome!</h1>
<p>This is a Flask application with a proper project structure.</p>
<p>Learn more about templates in the next chapter.</p>
{% endblock %}
```

### 4. Static Files

Add CSS in the `static/` folder:

```css
/* static/style.css — Simple stylesheet */
body {
    font-family: Arial, sans-serif;
    margin: 0;
    padding: 0;
    background-color: #f5f5f5;
}

header {
    background-color: #333;
    color: white;
    padding: 1rem;
}

nav a {
    color: white;
    text-decoration: none;
    margin-right: 15px;
}

nav a:hover {
    text-decoration: underline;
}

main {
    padding: 2rem;
    max-width: 800px;
    margin: 0 auto;
}
```

### 5. Updating app.py to Use Templates

```python
# app.py — Updated to use templates
from flask import Flask, render_template
from config import config


def create_app(config_name):
    """Application factory function."""
    app = Flask(__name__)
    app.config.from_object(config[config_name])
    
    @app.route("/")
    def index():
        """Render the home page using a template."""
        return render_template("index.html")
    
    @app.route("/hello")
    def hello():
        """Simple hello page."""
        return "<h1>Hello, World!</h1>"  # Still using plain text for simplicity
    
    return app


if __name__ == "__main__":
    app = create_app("development")
    app.run(debug=True)
```

## Common Mistakes

❌ **Placing templates in the wrong location**
```bash
# WRONG — Templates won't be found
my_app/
├── app.py
└── html/
    └── index.html
```

✅ **Correct — Use the templates folder**
```bash
# CORRECT — Flask automatically finds templates in the templates/ folder
my_app/
├── app.py
└── templates/
    └── index.html
```

❌ **Hardcoding paths to static files**
```python
# WRONG — This breaks if the URL path changes
<link rel="stylesheet" href="/static/style.css">
```

✅ **Correct — Use url_for()**
```python
# CORRECT — url_for() generates the correct URL automatically
<link rel="stylesheet" href="{{ url_for('static', filename='style.css') }}">
```

❌ **Keeping everything in one file as the app grows**
```python
# WRONG — app.py becomes thousands of lines long
# Hard to find anything, impossible to test individual parts
```

✅ **Correct — Organize into modules/packages**
```python
# CORRECT — Separate concerns into different files/routes
# routes/main.py, routes/api.py, models.py, forms.py, etc.
```

## Quick Reference

| Concept | Description |
|---------|-------------|
| `templates/` | Folder where Flask looks for HTML files |
| `static/` | Folder for CSS, JavaScript, images |
| `url_for()` | Flask function that generates URLs |
| `render_template()` | Flask function that renders an HTML file |
| Application factory | Function that creates and configures the Flask app |
| `config.py` | File that stores configuration settings |

## Next Steps

Now you have a solid foundation with a proper project structure. Continue to [01_route_decorator.md](../../02_routing_and_views/01_basic_routing/01_route_decorator.md) in the next chapter to learn about routing in detail — how to map URLs to view functions, handle different URL patterns, and organize your routes.