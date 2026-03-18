# Jinja2 Templates Advanced

## What You'll Learn
- Template inheritance patterns
- Custom filters
- Macros
- Template testing

## Prerequisites
- Completed Jinja2 templates basics

## Template Inheritance

```python
# base.html
<!DOCTYPE html>
<html>
<head>
    {% block head %}{% endblock %}
</head>
<body>
    {% block content %}{% endblock %}
</body>
</html>

# child.html
{% extends "base.html" %}

{% block head %}
<title>{{ title }}</title>
{% endblock %}

{% block content %}
<h1>{{ content_title }}</h1>
{% endblock %}
```

## Custom Filters

```python
from flask import Flask

app = Flask(__name__)

@app.template_filter('reverse')
def reverse_filter(s):
    return s[::-1]

# Usage: {{ name|reverse }}
```

## Summary
- Use inheritance for consistent layouts
- Use macros for reusable components
- Create custom filters for data transformation
