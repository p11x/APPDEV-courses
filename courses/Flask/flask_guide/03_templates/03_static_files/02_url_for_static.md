<!-- FILE: 03_templates/03_static_files/02_url_for_static.md -->

## Overview

The `url_for('static', ...)` function generates URLs for static files. This file explores advanced usage: cache busting with version hashes, serving files from custom locations, and best practices for production static file handling.

## Prerequisites

- Understanding of static file serving
- Basic Flask template knowledge

## Core Concepts

### Static URL Generation

`url_for('static', filename='path')` generates URLs like `/static/css/style.css`.

### Cache Busting

Browsers cache static files aggressively. When you update CSS/JS, users might see old versions. **Cache busting** adds a version identifier to force refresh:

```
/static/css/style.css?v=1.2.3
/static/js/app.js?hash=abc123
```

### Custom Static Handling

You can customize static file serving behavior in Flask.

## Code Walkthrough

### Basic url_for_static

```python
# app.py
from flask import Flask, url_for

app = Flask(__name__)

# In templates, use:
# {{ url_for('static', filename='css/style.css') }}
# Output: /static/css/style.css
```

### Cache Busting with Filename

```python
# app.py — Cache busting with version
import os

app = Flask(__name__)

# Simple version-based cache busting
@app.context_processor
def inject_version():
    """Make version available in all templates."""
    return dict(version="1.0.0")

# In template:
# {{ url_for('static', filename='css/style.css') }}?v={{ version }}
# Output: /static/css/style.css?v=1.0.0
```

### Automatic Cache Busting

```python
# app.py — Auto cache busting with file modification time
from flask import Flask
import os

app = Flask(__name__)

@app.context_processor
def static_utils():
    """Provide cache-busting URL generator."""
    def cache_bust(filename):
        """Add file modification time to URL for cache busting."""
        static_path = os.path.join(app.root_path, 'static', filename)
        if os.path.exists(static_path):
            mtime = int(os.path.getmtime(static_path))
            return f"{url_for('static', filename=filename)}?v={mtime}"
        return url_for('static', filename=filename)
    
    return dict(cache_bust=cache_bust)

# In template:
# <link rel="stylesheet" href="{{ cache_bust('css/style.css') }}">
# Output: /static/css/style.css?v=1700000000
```

### Custom Static Folder

```python
# app.py — Custom static folder
app = Flask(__name__,
    static_folder='assets',      # Default: 'static'
    static_url_path='/assets')  # Default: '/static'

# Files are now at:
# /assets/css/style.css
# Instead of:
# /static/css/style.css
```

### Multiple Static Folders

```python
# app.py — Multiple static folders (advanced)
from flask import Flask
from werkzeug.middleware.static import StaticFiles

app = Flask(__name__)

# Add extra static folder
extra_static = StaticFiles(app, '/extra_static')
# Note: This requires more setup for full functionality
```

### Using url_for in Python Code

```python
# Generating static URLs in Python code
with app.app_context():
    url = url_for('static', filename='css/style.css')
    # Returns: /static/css/style.css
```

### CDN Configuration

For production, serve static from CDN:

```python
# app.py — CDN configuration
import os

app = Flask(__name__)

# CDN base URL (use environment variable in production)
CDN_URL = os.environ.get('CDN_URL', '')

@app.context_processor
def cdn_processor():
    """Make CDN URL available in templates."""
    def cdn_url(filename):
        """Generate CDN URL for static file."""
        if CDN_URL:
            return f"{CDN_URL}/{filename}"
        return url_for('static', filename=filename)
    return dict(cdn_url=cdn_url)

# In template:
# <link rel="stylesheet" href="{{ cdn_url('css/style.css') }}">
# Dev: /static/css/style.css
# Prod: https://cdn.example.com/css/style.css
```

## Common Mistakes

❌ **Hardcoding CDN URLs**
```html
<!-- WRONG — Breaks in development -->
<link href="https://cdn.example.com/css/style.css">
```

✅ **Correct — Use environment-aware URL generator**
```html
<!-- CORRECT — Works in both dev and prod -->
<link rel="stylesheet" href="{{ cdn_url('css/style.css') }}">
```

❌ **Forgetting to invalidate cache**
```python
# WRONG — User sees old files
url_for('static', filename='css/style.css')
# Always returns same URL, browser caches it
```

✅ **Correct — Add version/hash**
```python
# CORRECT — Different URL when file changes
url_for('static', filename='css/style.css') + '?v=' + version
```

## Quick Reference

| Task | Code |
|------|------|
| Basic static URL | `url_for('static', filename='css/style.css')` |
| With version | `url_for('static', filename='css/style.css') + '?v=' + version` |
| In context | `cache_bust('css/style.css')` |

## Next Steps

Now you understand static file URLs. Continue to [03_organizing_assets.md](03_organizing_assets.md) to learn best practices for organizing CSS, JavaScript, and other assets in larger projects.