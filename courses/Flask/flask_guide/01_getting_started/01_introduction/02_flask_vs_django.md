<!-- FILE: 01_getting_started/01_introduction/02_flask_vs_django.md -->

## Overview

**Django** is another popular Python web framework, often compared to Flask. While both can build the same types of web applications, they take fundamentally different approaches. Flask is **minimal and flexible** — it gives you the bare essentials and lets you decide how to structure your project. Django is **batteries-included** — it provides built-in solutions for databases, authentication, admin panels, and more, following strict conventions. Understanding the differences helps you choose the right tool for your project and career path.

## Prerequisites

- Basic understanding of what Flask is (from the previous file)
- Familiarity with the concept of web frameworks in general

## Core Concepts

The key difference between Flask and Django lies in their **philosophy** and **scope**:

**Flask — The Micro-Framework**
- Provides only the core functionality: routing, request handling, and basic template rendering
- Leaves decisions about databases, authentication, and project structure entirely to you
- Ideal for small-to-medium applications, APIs, microservices, and learning web development
- Gives you freedom to mix and match any third-party libraries you prefer
- Has a flatter learning curve because there is less to learn upfront

**Django — The Full-Stack Framework**
- Includes everything needed for a complete web application: ORM, authentication, admin panel, forms, caching
- Enforces a specific project structure and naming conventions
- Better suited for large, database-driven applications like e-commerce sites, content management systems, and social platforms
- Has a steeper learning curve but provides more built-in features
- Comes with a powerful **Django Admin** interface that automatically generates a CRUD (Create, Read, Update, Delete) interface for your database models

> **💡 Tip:** Many developers start with Flask to learn the fundamentals, then move to Django for larger projects. Some teams even use both — Flask for microservices and APIs, Django for the main application.

## Code Walkthrough

Both frameworks can accomplish the same task, but the code looks different. Here is a simple "list of items" example in each:

```python
# Flask approach — Minimal and explicit
# You choose the database library (SQLAlchemy is most common) and structure

from flask import Flask, render_template
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///items.db'
db = SQLAlchemy(app)

class Item(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(100))

@app.route('/')
def index():
    items = Item.query.all()  # You write the query explicitly
    return render_template('index.html', items=items)
```

```python
# Django approach — Convention over configuration
# The ORM and project structure are built-in

# models.py
from django.db import models

class Item(models.Model):
    name = models.CharField(max_length=100)
    
    # Django automatically creates a admin panel and CRUD interface

# views.py
from django.shortcuts import render
from .models import Item

def index(request):
    items = Item.objects.all()  # Django ORM handles the query
    return render(request, 'index.html', {'items': items})
```

### Line-by-Line Breakdown (Flask Example)

- `from flask_sqlalchemy import SQLAlchemy` — Imports SQLAlchemy extension for Flask, which provides ORM functionality.
- `app.config['SQLALCHEMY_DATABASE_URI']` — Configures the database connection string; `sqlite:///items.db` creates a local SQLite file.
- `db = SQLAlchemy(app)` — Initializes the database with the Flask app instance.
- `class Item(db.Model):` — Defines a database model that maps to a table; each instance is a row.
- `Item.query.all()` — Executes a SELECT * query to fetch all items from the database.

### Line-by-Line Breakdown (Django Example)

- `from django.db import models` — Imports Django's built-in model system.
- `class Item(models.Model):` — Defines a model that Django automatically connects to a database table.
- `Item.objects.all()` — Django's ORM automatically generates the SQL query; `objects` is Django's default manager for database operations.
- `render(request, 'index.html', {...})` — Django's render function takes the request object directly, unlike Flask's which takes template name and context separately.

## Common Mistakes

❌ **Choosing Django when you need flexibility**
```python
# WRONG — Using Django's rigid structure when you just need a simple API
# Django forces many conventions that may slow down a small project
```

✅ **Correct — Use Flask for simple projects**
```python
# CORRECT — Flask lets you build a simple API in minutes without Django's overhead
from flask import Flask, jsonify
app = Flask(__name__)

@app.route('/api/items')
def get_items():
    return jsonify(['item1', 'item2'])
```

❌ **Choosing Flask for a large, complex application**
```python
# WRONG — Building a large e-commerce site with Flask means reinventing features Django provides out of the box
# You would need to manually implement authentication, admin panel, etc.
```

✅ **Correct — Use Django for large, database-driven applications**
```python
# CORRECT — Django's built-in auth, admin, and ORM save months of development time for large projects
```

## Quick Reference

| Feature | Flask | Django |
|---------|-------|--------|
| Project size | Small to medium | Medium to large |
| Database ORM | Optional (Flask-SQLAlchemy) | Built-in |
| Admin panel | Optional | Built-in |
| Authentication | Optional (Flask-Login) | Built-in |
| Learning curve | Gentle | Steeper |
| Flexibility | High | Low (conventions enforced) |
| Best for | APIs, microservices, learning | Full-stack web apps, CMS, e-commerce |

## Next Steps

Now that you understand the differences between Flask and Django, continue to [03_how_http_works.md](03_how_http_works.md) to learn how HTTP requests and responses work — essential knowledge for any web developer.