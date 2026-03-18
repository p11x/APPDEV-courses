# Django Introduction

## What You'll Learn
- What Django is and why it's called "batteries-included"
- The difference between Django and Flask
- Installing Django
- Creating your first Django project
- Understanding Django's architecture (MTV pattern)

## Prerequisites
- Completed Flask fundamentals (folders 00-10)
- Understanding of Python web development concepts

## What Is Django?

**Django** (/ˈdʒæŋɡoʊ/) is a high-level Python web framework that encourages rapid development and clean, pragmatic design. It follows the principle of "batteries included" — meaning it comes with almost everything you need to build a web application out of the box.

Think of Django like a pre-furnished apartment:
- You get a bedroom (URL routing), kitchen (ORM), bathroom (forms), and living room (admin panel) all included
- You can move in and start living (building your app) immediately
- You don't need to shop for furniture (install and configure individual packages)

This is different from Flask, which is more like an empty plot of land — you have complete freedom to build whatever you want, but you have to source every material yourself.

## When to Use Django

Use Django when you need:
- A full-featured admin panel built-in
- User authentication system ready to use
- Database migrations out of the box
- Security features (SQL injection prevention, CSRF protection) included
- A large ecosystem of packages
- Convention over configuration (sensible defaults)

## Installing Django

Django can be installed via pip:

```bash
pip install django   # Django web framework
```

What just happened:
- `pip` downloaded Django from PyPI
- Django and its dependencies were installed
- You can now use the `django` command-line tool

## Creating Your First Django Project

The `django-admin` command creates a new Django project structure:

```bash
django-admin startproject myproject
cd myproject
```

This creates the following structure:

```
myproject/
├── manage.py          # Django's command-line utility
└── myproject/        # The actual Python package
    ├── __init__.py
    ├── settings.py   # Configuration
    ├── urls.py       # URL routing
    ├── asgi.py       # ASGI configuration
    └── wsgi.py      # WSGI configuration
```

Let me explain what each of these files does, because they're the foundation of every Django project.

## Understanding the Project Structure

### manage.py

This is Django's command-line utility. You use it to:

```bash
python manage.py runserver    # Start development server
python manage.py migrate      # Apply database migrations
python manage.py createsuperuser  # Create admin user
python manage.py test        # Run tests
```

Think of `manage.py` as the keys to your Django project — it's the main tool you use to control everything.

### settings.py

This is where all your configuration lives. Key settings include:

```python
# settings.py
INSTALLED_APPS = [
    'django.contrib.admin',      # Admin panel
    'django.contrib.auth',      # Authentication
    'django.contrib.contenttypes',
    'django.contrib.sessions',
    'django.contrib.messages',
    'django.contrib.staticfiles',
]

DATABASES = {
    'default': {
        'ENGINE': 'django.db.backends.sqlite3',
        'NAME': BASE_DIR / 'db.sqlite3',
    }
}

SECRET_KEY = 'your-secret-key-change-this-in-production'

DEBUG = True  # Set to False in production

ALLOWED_HOSTS = []  # Domain names that can serve this app
```

Let me break down these important settings:

1. `INSTALLED_APPS` — Django comes with several built-in apps. The admin panel, authentication system, and sessions are all included here.

2. `DATABASES` — Django supports multiple databases (PostgreSQL, MySQL, SQLite). By default, it uses SQLite which is perfect for development.

3. `SECRET_KEY` — A unique key used for cryptographic signing. In production, this must be kept secret and never committed to version control.

4. `DEBUG` — When `True`, Django shows detailed error pages. Always set to `False` in production.

### urls.py

This file maps URLs to views. It's the "table of contents" for your website:

```python
# urls.py
from django.contrib import admin
from django.urls import path

urlpatterns = [
    path('admin/', admin.site.urls),  # Built-in admin panel
]
```

Every URL pattern in Django follows this format:
- The first argument is the URL pattern (e.g., `'admin/'` or `''` for homepage)
- The second argument is the view function or class that handles that URL
- Optionally, you can give each URL a `name=` parameter for reverse lookup

## Running the Development Server

Start the development server to see your empty Django project:

```bash
python manage.py runserver
```

You'll see output like:

```
Watching for file changes with stat()
Django version 4.2, using settings 'myproject.settings'
Starting development server at http://127.0.0.1:8000/
```

Open your browser to `http://127.0.0.1:8000` — you should see a "The install worked successfully!" Congratulations page!

## Django's MTV Architecture

Django follows the **MTV (Model-Template-View)** pattern:

- **Model** — Data layer. Defines your database tables as Python classes.
- **Template** — Presentation layer. HTML files that define how pages look.
- **View** — Logic layer. Python functions that process requests and return responses.

This is conceptually similar to MVC (Model-View-Controller) but with different naming:
- Django's "View" is like MVC's "Controller" (handles logic)
- Django's "Template" is like MVC's "View" (handles presentation)

## Creating Your First App

Django projects are organized into "apps" — modular components that do specific things. Create your first app:

```bash
python manage.py startapp blog
```

This creates a `blog/` directory with:

```
blog/
├── __init__.py
├── admin.py       # Configure admin panel
├── apps.py       # App configuration
├── migrations/   # Database migration files
├── models.py     # Database models
├── tests.py      # Unit tests
└── views.py      # View functions
```

## Registering Your App

Before Django will recognize your app, you need to add it to `INSTALLED_APPS` in `settings.py`:

```python
# settings.py
INSTALLED_APPS = [
    'django.contrib.admin',
    'django.contrib.auth',
    'django.contrib.contenttypes',
    'django.contrib.sessions',
    'django.contrib.messages',
    'django.contrib.staticfiles',
    'blog',  # Add your app here
]
```

Now Django knows about your app and will include it in the project.

## Django vs Flask: When to Use Each

| Feature | Django | Flask |
|---------|--------|-------|
| Learning curve | Medium | Low |
| Project structure | Fixed | Flexible |
| Admin panel | Built-in | Not included |
| Database ORM | Built-in | Via extensions |
| Forms | Built-in | Via extensions |
| Authentication | Built-in | Via extensions |
| Size/complexity | Large apps | Small-medium apps |
| Speed | Slightly slower | Slightly faster |
| Flexibility | Less flexible | Very flexible |

For beginners: Start with Flask to understand the fundamentals, then move to Django when you need to build something more complex quickly.

## Summary
- Django is a "batteries-included" framework with admin, auth, ORM built-in
- Django follows MTV (Model-Template-View) architecture
- Key files: `manage.py`, `settings.py`, `urls.py`, `views.py`, `models.py`
- Django projects are organized into reusable "apps"
- Use Django for larger projects that need many features out of the box

## Next Steps
→ Continue to `02-django-project-structure.md` to understand Django's project structure better and learn about best practices for organizing larger Django projects.
