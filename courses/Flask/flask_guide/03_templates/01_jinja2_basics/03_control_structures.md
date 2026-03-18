<!-- FILE: 03_templates/01_jinja2_basics/03_control_structures.md -->

## Overview

Jinja2's **control structures** let you add logic to your templates: loops for iterating over lists, conditionals for showing different content based on values, and special features like `include` and `set` for reusing and organizing code. These structures make templates dynamic and flexible.

## Prerequisites

- Understanding of basic Jinja2 syntax
- Familiarity with Python loops and conditionals
- Basic template rendering knowledge

## Core Concepts

### For Loops

Iterate over sequences (lists, dictionaries, tuples):

```html
{% for item in items %}
    {{ item }}
{% endfor %}
```

### If Statements

Conditional content:

```html
{% if user.is_admin %}
    <p>Admin controls</p>
{% elif user.is_member %}
    <p>Member content</p>
{% else %}
    <p>Public content</p>
{% endif %}
```

### Set and With

Assign variables in templates:

```html
{% set name = "Alice" %}
{% with total = 0 %}
    {% for item in items %}
        {% set total = total + item.price %}
    {% endfor %}
    Total: {{ total }}
{% endwith %}
```

### Include

Include other template files:

```html
{% include "header.html" %}
{% include "footer.html" %}
```

## Code Walkthrough

### Control Structures Demo

```python
# app.py — Template control structures
from flask import Flask, render_template_string

app = Flask(__name__)

@app.route("/")
def demo():
    """Demonstrate Jinja2 control structures."""
    
    products = [
        {"name": "Laptop", "price": 999, "category": "electronics", "in_stock": True},
        {"name": "Book", "price": 19, "category": "books", "in_stock": True},
        {"name": "Phone", "price": 599, "category": "electronics", "in_stock": False},
        {"name": "Pen", "price": 2, "category": "supplies", "in_stock": True},
    ]
    
    user = {"name": "Alice", "is_admin": False, "is_member": True}
    empty_list = []
    
    template = """
<!DOCTYPE html>
<html>
<head><title>Control Structures</title></head>
<body>
    <h1>Jinja2 Control Structures</h1>
    
    <!-- 1. For Loop -->
    <h2>1. For Loop</h2>
    <ul>
    {% for product in products %}
        <li>{{ product.name }} - ${{ product.price }}</li>
    {% endfor %}
    </ul>
    
    <!-- 2. For Loop with Loop Variables -->
    <h2>2. Loop Variables</h2>
    <ul>
    {% for product in products %}
        <li>
            {{ loop.index }}. {{ product.name }}
            {% if loop.first %} (first){% endif %}
            {% if loop.last %} (last){% endif %}
        </li>
    {% endfor %}
    </ul>
    
    <!-- 3. For Loop with Conditional -->
    <h2>3. For with If</h2>
    <ul>
    {% for product in products %}
        {% if product.in_stock %}
            <li>{{ product.name }} - In Stock</li>
        {% endif %}
    {% endfor %}
    </ul>
    
    <!-- 4. If-Elif-Else -->
    <h2>4. If-Elif-Else</h2>
    {% if user.is_admin %}
        <p>Welcome, Admin! You have full access.</p>
    {% elif user.is_member %}
        <p>Welcome, Member! You have limited access.</p>
    {% else %}
        <p>Welcome, Guest! Please sign up.</p>
    {% endif %}
    
    <!-- 5. Testing for Empty Lists -->
    <h2>5. Empty List Check</h2>
    {% if products %}
        <p>There are {{ products|length }} products.</p>
    {% else %}
        <p>No products available.</p>
    {% endif %}
    
    {% if empty_list %}
        <p>List has items</p>
    {% else %}
        <p>List is empty</p>
    {% endif %}
    
    <!-- 6. Set Variable -->
    <h2>6. Set Variable</h2>
    {% set greeting = "Hello" %}
    <p>{{ greeting }}, {{ user.name }}!</p>
    
    <!-- 7. With Block (scoped variables) -->
    <h2>7. With Block</h2>
    {% with total = 0 %}
        {% for product in products %}
            {% set total = total + product.price %}
        {% endfor %}
        <p>Total: ${{ total }}</p>
    {% endwith %}
    <!-- 'total' is not available here -->
    
    <!-- 8. Filtering in For Loop -->
    <h2>8. Filtered Loop</h2>
    <ul>
    {% for product in products|selectattr("in_stock") %}
        <li>{{ product.name }}</li>
    {% endfor %}
    </ul>
    
    <!-- 9. Loop Over Dictionary -->
    <h2>9. Dictionary Iteration</h2>
    {% for key, value in user.items() %}
        <p>{{ key }}: {{ value }}</p>
    {% endfor %}
    
    <!-- 10. Break and Continue (Jinja2 2.10+) -->
    <h2>10. Break Example</h2>
    <ul>
    {% for product in products %}
        {% if loop.index > 2 %}
            {% break %}
        {% endif %}
        <li>{{ product.name }}</li>
    {% endfor %}
    </ul>
    
</body>
</html>
    """
    return render_template_string(template, products=products, user=user, empty_list=empty_list)

if __name__ == "__main__":
    app.run(debug=True)
```

### Include Example

Create separate template files:

```html
<!-- templates/header.html -->
<header>
    <nav>
        <a href="/">Home</a>
        <a href="/about">About</a>
        <a href="/contact">Contact</a>
    </nav>
</header>
```

```html
<!-- templates/footer.html -->
<footer>
    <p>&copy; 2024 My Website</p>
</footer>
```

```python
# Using include in a template
template = """
{% include 'header.html' %}
<main>
    <h1>Welcome!</h1>
</main>
{% include 'footer.html' %}
"""
```

### Macro Example

Macros are reusable template functions:

```html
<!-- templates/macros.html -->
{% macro input_field(name, label, type='text') %}
    <div class="form-group">
        <label for="{{ name }}">{{ label }}</label>
        <input type="{{ type }}" name="{{ name }}" id="{{ name }}">
    </div>
{% endmacro %}

{% macro render_product(product) %}
    <div class="product">
        <h3>{{ product.name }}</h3>
        <p>${{ product.price }}</p>
    </div>
{% endmacro %}
```

```html
<!-- Using macros -->
{% from 'macros.html' import input_field, render_product %}

<form>
    {{ input_field('username', 'Username') }}
    {{ input_field('email', 'Email', 'email') }}
    {{ input_field('password', 'Password', 'password') }}
</form>

{% for product in products %}
    {{ render_product(product) }}
{% endfor %}
```

## Common Mistakes

❌ **Forgetting to close blocks**
```html
<!-- WRONG — Missing endif or endfor -->
{% if user %}
    <p>Hello</p>
{% endfor %}
```

✅ **Correct — Close all blocks**
```html
<!-- CORRECT -->
{% if user %}
    <p>Hello</p>
{% endif %}
```

❌ **Variable scope issues**
```html
<!-- WRONG — 'total' persists outside the loop unexpectedly -->
{% for product in products %}
    {% set total = total + product.price %}
{% endfor %}
<p>Total: {{ total }}</p>
```

✅ **Correct — Use with block for scoping**
```html
<!-- CORRECT — 'total' is scoped -->
{% with total = 0 %}
    {% for product in products %}
        {% set total = total + product.price %}
    {% endfor %}
    Total: {{ total }}
{% endwith %}
```

## Quick Reference

| Syntax | Description |
|--------|-------------|
| `{% for item in items %}` | Loop over items |
| `{% if condition %}` | Conditional |
| `{% elif %}` | Else if |
| `{% else %}` | Else case |
| `{% endif %}` | End if |
| `{% endfor %}` | End for |
| `{% set x = value %}` | Set variable |
| `{% with x = value %}` | Scoped variable |
| `{% include 'file.html' %}` | Include template |
| `{% macro %}{% endmacro %}` | Reusable template function |

## Next Steps

Now you understand Jinja2 control structures. Continue to [01_base_template.md](../02_template_inheritance/01_base_template.md) to learn about template inheritance — the key to building maintainable websites with reusable layouts.