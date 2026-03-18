# Django Project Structure

## What You'll Learn
- Best practices for organizing Django projects
- Understanding settings.py configuration
- Environment-specific settings (development vs production)
- Django apps structure

## Prerequisites
- Completed `01-django-introduction.md`

## The Problem with Default Structure

The default Django project structure works, but as your application grows, you need a more organized approach. Let's look at how to structure a production-ready Django project.

## Recommended Project Structure

For a medium-to-large Django project, use this structure:

```
myproject/
├── manage.py
├── requirements.txt
├── .env                    # Environment variables (don't commit!)
├── .gitignore
├── docker-compose.yml      # Docker configuration
├── myproject/            # Project configuration
│   ├── __init__.py
│   ├── settings/
│   │   ├── __init__.py
│   │   ├── base.py       # Common settings
│   │   ├── development.py
│   │   └── production.py
│   ├── urls.py
│   ├── wsgi.py
│   └── asgi.py
├── apps/                 # Your Django apps
│   ├── __init__.py
│   ├── blog/
│   ├── accounts/
│   └── products/
├── templates/            # Shared templates
├── static/              # Static files (CSS, JS, images)
├── media/               # User-uploaded files
└── tests/               # Test configurations
```

## Environment-Specific Settings

Instead of one large `settings.py`, use separate files:

### base.py (Common Settings)

```python
# myproject/settings/base.py
from pathlib import Path
from typing import Any

# Build paths inside the project like this: BASE_DIR / 'subdir'.
BASE_DIR: Path = Path(__file__).resolve().parent.parent.parent

# SECURITY WARNING: keep the secret key used in production secret!
SECRET_KEY: str = 'django-insecure-change-this-in-production'

# Application definition
INSTALLED_APPS: list[str] = [
    'django.contrib.admin',
    'django.contrib.auth',
    'django.contrib.contenttypes',
    'django.contrib.sessions',
    'django.contrib.messages',
    'django.contrib.staticfiles',
    # Third party
    'rest_framework',
    'corsheaders',
    # Local apps
    'apps.blog',
    'apps.accounts',
]

MIDDLEWARE: list[str] = [
    'django.middleware.security.SecurityMiddleware',
    'corsheaders.middleware.CorsMiddleware',
    'django.contrib.sessions.middleware.SessionMiddleware',
    'django.middleware.common.CommonMiddleware',
    'django.middleware.csrf.CsrfViewMiddleware',
    'django.contrib.auth.middleware.AuthenticationMiddleware',
    'django.contrib.messages.middleware.MessageMiddleware',
    'django.middleware.clickjacking.XFrameOptionsMiddleware',
]

ROOT_URLCONF: str = 'myproject.urls'

TEMPLATES: list[dict[str, Any]] = [
    {
        'BACKEND': 'django.template.backends.django.DjangoTemplates',
        'DIRS': [BASE_DIR / 'templates'],
        'APP_DIRS': True,
        'OPTIONS': {
            'context_processors': [
                'django.template.context_processors.debug',
                'django.template.context_processors.request',
                'django.contrib.auth.context_processors.auth',
                'django.contrib.messages.context_processors.messages',
            ],
        },
    },
]

WSGI_APPLICATION: str = 'myproject.wsgi.application'

# Database configuration
DATABASES: dict[str, dict[str, str]] = {
    'default': {
        'ENGINE': 'django.db.backends.sqlite3',
        'NAME': BASE_DIR / 'db.sqlite3',
    }
}

# Password validation
AUTH_PASSWORD_VALIDATORS: list[dict[str, str]] = [
    {'NAME': 'django.contrib.auth.password_validation.UserAttributeSimilarityValidator'},
    {'NAME': 'django.contrib.auth.password_validation.MinimumLengthValidator'},
    {'NAME': 'django.contrib.auth.password_validation.CommonPasswordValidator'},
    {'NAME': 'django.contrib.auth.password_validation.NumericPasswordValidator'},
]

# Internationalization
LANGUAGE_CODE: str = 'en-us'
TIME_ZONE: str = 'UTC'
USE_I18N: bool = True
USE_TZ: bool = True

# Static files
STATIC_URL: str = '/static/'
STATIC_ROOT: Path = BASE_DIR / 'staticfiles'
STATICFILES_DIRS: list[Path] = [BASE_DIR / 'static']

# Media files
MEDIA_URL: str = '/media/'
MEDIA_ROOT: Path = BASE_DIR / 'media'

# Default primary key field type
DEFAULT_AUTO_FIELD: str = 'django.db.models.BigAutoField'
```

### development.py

```python
# myproject/settings/development.py
from .base import *

DEBUG: bool = True

ALLOWED_HOSTS: list[str] = ['*']

# Use SQLite for development
DATABASES: dict[str, dict[str, str]] = {
    'default': {
        'ENGINE': 'django.db.backends.sqlite3',
        'NAME': BASE_DIR / 'db.sqlite3',
    }
}

# Email backend for development (prints to console)
EMAIL_BACKEND: str = 'django.core.mail.backends.console.EmailBackend'

# CORS settings for development
CORS_ALLOW_ALL_ORIGINS: bool = True
```

### production.py

```python
# myproject/settings/production.py
import os
from .base import *

DEBUG: bool = False

ALLOWED_HOSTS: list[str] = ['yourdomain.com', 'www.yourdomain.com']

# Use PostgreSQL in production
DATABASES: dict[str, dict[str, str]] = {
    'default': {
        'ENGINE': 'django.db.backends.postgresql',
        'NAME': os.environ.get('POSTGRES_DB', 'mydb'),
        'USER': os.environ.get('POSTGRES_USER', 'myuser'),
        'PASSWORD': os.environ.get('POSTGRES_PASSWORD', ''),
        'HOST': os.environ.get('POSTGRES_HOST', 'localhost'),
        'PORT': os.environ.get('POSTGRES_PORT', '5432'),
    }
}

# Secure settings
SECURE_BROWSER_XSS_FILTER: bool = True
SECURE_CONTENT_TYPE_NOSNIFF: bool = True
X_FRAME_OPTIONS: str = 'DENY'

# HTTPS settings
SESSION_COOKIE_SECURE: bool = True
CSRF_COOKIE_SECURE: bool = True
SECURE_SSL_REDIRECT: bool = True

# CORS settings for production
CORS_ALLOWED_ORIGINS: list[str] = ['https://yourdomain.com']
```

## Managing Settings with Environment Variables

Use environment variables for sensitive configuration:

```python
# myproject/settings/production.py
import os
from pathlib import Path

BASE_DIR: Path = Path(__file__).resolve().parent.parent.parent

# Load from .env file in development
if not os.environ.get('DJANGO_SETTINGS_MODULE'):
    from dotenv import load_dotenv
    load_dotenv(BASE_DIR / '.env')

SECRET_KEY: str = os.environ.get('SECRET_KEY', 'fallback-secret-key')
```

## Switching Between Environments

Set the `DJANGO_SETTINGS_MODULE` environment variable:

```bash
# Development
export DJANGO_SETTINGS_MODULE=myproject.settings.development

# Production
export DJANGO_SETTINGS_MODULE=myproject.settings.production
```

Or in manage.py:

```python
#!/usr/bin/env python
import os
import sys

if __name__ == '__main__':
    os.environ.setdefault('DJANGO_SETTINGS_MODULE', 'myproject.settings.development')
    try:
        from django.core.management import execute_from_command_line
    except ImportError as exc:
        raise ImportError(
            "Couldn't import Django. Are you sure it's installed?"
        ) from exc
    execute_from_command_line(sys.argv)
```

## Structuring Your Apps

Put all your Django apps in an `apps` directory:

```
apps/
├── __init__.py
├── blog/
│   ├── __init__.py
│   ├── models.py
│   ├── views.py
│   ├── urls.py
│   ├── admin.py
│   ├── apps.py
│   └── migrations/
├── accounts/
│   ├── __init__.py
│   ├── models.py
│   ├── views.py
│   └── urls.py
```

Update your apps.py to reference the correct path:

```python
# apps/blog/apps.py
from django.apps import AppConfig

class BlogConfig(AppConfig):
    default_auto_field = 'django.db.models.BigAutoField'
    name = 'apps.blog'
    verbose_name = 'Blog'
```

## Summary
- Use environment-specific settings files (base, development, production)
- Keep sensitive configuration in environment variables
- Organize apps in an `apps/` directory
- Use `DJANGO_SETTINGS_MODULE` to switch between configurations

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Keeping secrets in settings.py
**Wrong:**
```python
SECRET_KEY = 'my-secret-key-12345'
PASSWORD = 'mypassword'
```
**Why it fails:** If you commit this to version control, anyone can see your secrets.
**Fix:** Use environment variables:
```python
import os
SECRET_KEY = os.environ.get('SECRET_KEY')
```

### ❌ Mistake 2: Not separating development and production settings
**Wrong:** Using the same settings for both development and production.
**Why it fails:** Development settings like `DEBUG=True` expose sensitive information in production.
**Fix:** Use separate settings files and set `DJANGO_SETTINGS_MODULE`.

### ❌ Mistake 3: Hardcoding database credentials
**Wrong:**
```python
DATABASES = {
    'default': {
        'NAME': 'mydb',
        'USER': 'myuser',
        'PASSWORD': 'secret123',
    }
}
```
**Why it fails:** Credentials are exposed in version control.
**Fix:** Use environment variables:
```python
DATABASES = {
    'default': {
        'ENGINE': 'django.db.backends.postgresql',
        'NAME': os.environ.get('POSTGRES_DB'),
        'USER': os.environ.get('POSTGRES_USER'),
        'PASSWORD': os.environ.get('POSTGRES_PASSWORD'),
    }
}
```

## Next Steps
→ Continue to `03-models-and-orm.md` to learn how to create and manage Django models.
