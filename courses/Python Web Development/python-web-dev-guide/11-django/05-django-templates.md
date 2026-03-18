# Django Templates

## What You'll Learn
- Django template language basics
- Template inheritance
- Template tags and filters
- Context processors

## Prerequisites
- Completed `04-views-and-urls.md`

## Django Templates Overview

Django templates are HTML files with special syntax for dynamic content. They follow a similar pattern to Jinja2 (which you learned with Flask).

## Template Basics

```html
<!-- templates/blog/post_list.html -->
<!DOCTYPE html>
<html>
<head>
    <title>{{ page_title }}</title>
</head>
<body>
    <h1>{{ post.title }}</h1>
    <p>By {{ post.author.username }}</p>
    <p>{{ post.content|linebreaks }}</p>
    <p>Published: {{ post.published_at|date:"F j, Y" }}</p>
</body>
</html>
```

## Variables

```html
{{ variable }}
{{ object.field }}
{{ dictionary.key }}
{{ list.0 }}
```

## Tags

```html
{% for post in posts %}
    <h2>{{ post.title }}</h2>
{% empty %}
    <p>No posts found.</p>
{% endfor %}

{% if user.is_authenticated %}
    <p>Welcome, {{ user.username }}!</p>
{% else %}
    <a href="{% url 'login' %}">Login</a>
{% endif %}

{% csrf_token %}
{% url 'post_detail' post.slug %}
{% static 'css/style.css' %}
```

## Filters

```html
{{ name|upper }}
{{ text|truncatewords:50 }}
{{ post.content|linebreaks }}
{{ date|date:"F j, Y" }}
{{ price|currency }}
{{ name|default:"Anonymous" }}
```

## Template Inheritance

### Base Template

```html
<!-- templates/base.html -->
<!DOCTYPE html>
<html>
<head>
    <title>{% block title %}My Blog{% endblock %}</title>
    {% block extra_css %}{% endblock %}
</head>
<body>
    <header>
        <nav>
            <a href="{% url 'post_list' %}">Home</a>
        </nav>
    </header>
    
    <main>
        {% block content %}{% endblock %}
    </main>
    
    <footer>
        {% block footer %}{% endblock %}
    </footer>
</body>
</html>
```

### Child Template

```html
<!-- templates/blog/post_detail.html -->
{% extends 'base.html' %}

{% block title %}{{ post.title }}{% endblock %}

{% block content %}
    <article>
        <h1>{{ post.title }}</h1>
        {{ post.content|linebreaks }}
    </article>
{% endblock %}
```

## Including Templates

```html
{% include 'partials/nav.html' %}
{% include 'partials/post_card.html' with post=post %}
```

## Custom Template Tags

Create a `templatetags` directory:

```python
# blog/templatetags/__init__.py
from django import template
register = template.Library()

@register.simple_tag
def current_year():
    return timezone.now().year

@register.filter
def pluralize(value, singular, plural='s'):
    return singular if value == 1 else plural
```

Usage:
```html
{% load blog_tags %}
<p>{% current_year %}</p>
<p>{{ count|pluralize:"post,posts" }}</p>
```

## Summary
- Django templates use `{{ }}` for variables and `{% %}` for tags
- Use template inheritance to share common structure
- Filters modify variables
- Custom tags and filters provide reusability
