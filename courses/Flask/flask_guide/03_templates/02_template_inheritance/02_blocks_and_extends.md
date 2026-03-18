<!-- FILE: 03_templates/02_template_inheritance/02_blocks_and_extends.md -->

## Overview

Building on basic template inheritance, this file covers advanced features: using `super()` to append to parent content, creating multi-level inheritance hierarchies, and organizing your templates for large applications. These techniques give you fine-grained control over how templates combine.

## Prerequisites

- Understanding of base templates and inheritance
- Familiarity with Jinja2 blocks

## Core Concepts

### The super() Function

When you override a block, you normally replace the parent's content entirely. Use `super()` to include the parent's content while adding more:

```html
{% block title %}
    {{ super() }} - Extra Title
{% endblock %}
```

This appends or prepends to the parent's block content.

### Multi-Level Inheritance

You can create multiple inheritance levels:
1. `base.html` — Core HTML structure
2. `layout.html` extends `base.html` — Common page layouts (sidebar, etc.)
3. `page.html` extends `layout.html` — Specific page types

### Block Scope

Variables set inside a block are not visible outside that block unless using `with` or specific scoping.

## Code Walkthrough

### super() Example

```html
<!-- templates/base.html -->
<!DOCTYPE html>
<html>
<head>
    <title>{% block title %}My Site{% endblock %}</title>
    {% block head %}{% endblock %}
</head>
<body>
    {% block body %}{% endblock %}
</body>
</html>
```

```html
<!-- templates/child.html — Using super() -->
{% extends "base.html" %}

{% block title %}
    {{ super() }} - Child Page
{% endblock %}

{% block head %}
    {{ super() }}
    <link rel="stylesheet" href="child.css">
{% endblock %}

{% block body %}
    <h1>Child Content</h1>
{% endblock %}
```

This results in:
- Title: "My Site - Child Page"
- Head includes base head + child.css

### Multi-Level Inheritance Example

```html
<!-- templates/base.html — Level 0 -->
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>{% block title %}{% endblock %}</title>
    {% block css %}{% endblock %}
</head>
<body>
    <header>{% block header %}{% endblock %}</header>
    <main>{% block content %}{% endblock %}</main>
    <footer>{% block footer %}{% endblock %}</footer>
    {% block js %}{% endblock %}
</body>
</html>
```

```html
<!-- templates/layout.html — Level 1 -->
{% extends "base.html" %}

{% block header %}
    <nav>
        <a href="/">Home</a>
        <a href="/about">About</a>
    </nav>
{% endblock %}

{% block footer %}
    <p>&copy; 2024 Company</p>
    {{ super() }}
{% endblock %}
```

```html
<!-- templates/blog.html — Level 2 -->
{% extends "layout.html" %}

{% block title %}Blog - My Site{% endblock %}

{% block css %}
    {{ super() }}
    <link rel="stylesheet" href="blog.css">
{% endblock %}

{% block content %}
    <article>
        {% block article_content %}
        {% endblock %}
    </article>
{% endblock %}
```

```html
<!-- templates/blog_post.html — Level 3 -->
{% extends "blog.html" %}

{% block article_content %}
    <h1>Blog Post Title</h1>
    <p>Post content here...</p>
{% endblock %}
```

### Calling super() with Arguments (Jinja2 2.2+)

```html
<!-- base.html -->
{% block content %}
    Default content
{% endblock %}
```

```html
<!-- child.html -->
{% block content %}
    {{ super() }}
    Additional content
{% endblock %}
```

### Dynamic Inheritance

You can dynamically choose which template to extend:

```python
# Dynamic extends based on user theme
template_name = "themes/" + user_theme + ".html"
return render_template(template_name, ...)
```

## Common Mistakes

❌ **Using super() incorrectly**
```html
<!-- WRONG — super() is a function, not a variable -->
{% block title %}
    {{ super }} - Page
{% endblock %}
```

✅ **Correct — Call super() as function**
```html
<!-- CORRECT -->
{% block title %}
    {{ super() }} - Page
{% endblock %}
```

❌ **Too many inheritance levels**
```python
# WRONG — Too complex, hard to debug
# base -> layout1 -> layout2 -> page1 -> page2
```

✅ **Correct — Limit to 2-3 levels**
```python
# CORRECT
# base -> layout -> page
```

## Quick Reference

| Feature | Syntax | Description |
|---------|--------|-------------|
| Call parent | `{{ super() }}` | Include parent block content |
| Extend | `{% extends "template" %}` | Inherit from template |
| Override | `{% block name %}...{% endblock %}` | Replace block |
| Scoped | `{% block name scoped %}` | Keep variables in scope |

## Next Steps

Now you understand advanced inheritance. Continue to [03_macros.md](03_macros.md) to learn about macros — reusable template components that work like functions.