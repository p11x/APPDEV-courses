# Serving Static Files

## What You'll Learn
- How to serve CSS, JavaScript, and images
- Flask static files
- FastAPI static files

## Prerequisites
- Completed Flask or FastAPI introduction

## Flask Static Files

Create a `static` folder:

```
project/
├── app.py
├── static/
│   ├── css/
│   │   └── style.css
│   ├── js/
│   │   └── main.js
│   └── images/
│       └── logo.png
└── templates/
    └── index.html
```

### Using in Templates

```html
<link rel="stylesheet" href="{{ url_for('static', filename='css/style.css') }}">
<script src="{{ url_for('static', filename='js/main.js') }}"></script>
<img src="{{ url_for('static', filename='images/logo.png') }}">
```

## FastAPI Static Files

```python
from fastapi import FastAPI
from fastapi.staticfiles import StaticFiles

app = FastAPI()

app.mount("/static", StaticFiles(directory="static"), name="static")
```

## Summary
- Flask: Place files in `static/` folder
- FastAPI: Use `app.mount()`
- Use `url_for()` to generate URLs

## Next Steps
→ Continue to `02-jinja2-templates-advanced.md`
