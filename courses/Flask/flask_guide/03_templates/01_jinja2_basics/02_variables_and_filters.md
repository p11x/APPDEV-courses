<!-- FILE: 03_templates/01_jinja2_basics/02_variables_and_filters.md -->

## Overview

Jinja2 provides powerful features for working with template variables, including **filters** that transform data, **tests** that check conditions, and **global objects** like `loop` and `config`. Understanding these features lets you perform common operations directly in templates without writing Python code.

## Prerequisites

- Understanding of basic Jinja2 template syntax
- Familiarity with Python data types

## Core Concepts

### Variable Access

Jinja2 provides flexible attribute access:

```html
{{ user.name }}           <!-- Dict key or object attribute -->
{{ user['name'] }}        <!-- Dict-style access -->
{{ loop.index }}          <!-- Loop variable -->
{{ request.args.q }}      <!-- Chained access -->
```

### Filters

**Filters** modify variables using the pipe (`|`) operator:

```html
{{ name|upper }}          <!-- Convert to uppercase -->
{{ text|truncate(50) }}   <!-- Truncate to 50 characters -->
{{ items|join(", ") }}    <!-- Join list with separator -->
```

Common built-in filters:
- `upper` / `lower` — Case conversion
- `title` — Title case
- `length` — Get length
- `first` / `last` — Get first/last element
- `sort` — Sort a list
- `default(value)` — Default if variable is undefined
- `safe` — Mark HTML as safe (not escaped)
- `datetimeformat` — Format dates

### Tests

**Tests** check conditions using `is`:

```html
{% if user is defined %}
    Hello {{ user.name }}
{% endif %}

{% if value is even %}
    Value is even
{% endif %}
```

### Loop Variables

Inside `{% for %}` loops, Jinja2 provides special variables:

- `loop.index` — Current iteration (1-based)
- `loop.index0` — Current iteration (0-based)
- `loop.first` — True if first iteration
- `loop.last` — True if last iteration
- `loop.length` — Total number of items

## Code Walkthrough

### Variables and Filters Demo

```python
# app.py — Template with variables and filters
from flask import Flask, render_template_string
from datetime import datetime

app = Flask(__name__)

@app.route("/")
def demo():
    """Demonstrate variables and filters."""
    
    # Sample data
    product = {
        "name": "laptop",
        "price": 999.99,
        "description": "A powerful laptop for developers",
        "tags": ["electronics", "computer", "sale"],
        "created_at": datetime(2024, 1, 15, 10, 30),
        "in_stock": True,
        "quantity": 0
    }
    
    users = [
        {"name": "Alice", "age": 25, "active": True},
        {"name": "Bob", "age": 17, "active": True},
        {"name": "Charlie", "age": 35, "active": False},
    ]
    
    template = """
<!DOCTYPE html>
<html>
<head><title>Jinja2 Variables & Filters</title></head>
<body>
    <h1>Variables & Filters Demo</h1>
    
    <!-- Basic Variables -->
    <h2>1. Basic Variables</h2>
    <p>Product: {{ product.name }}</p>
    <p>Price: {{ product.price }}</p>
    <p>Description: {{ product.description }}</p>
    
    <!-- String Filters -->
    <h2>2. String Filters</h2>
    <p>Upper: {{ product.name|upper }}</p>
    <p>Lower: {{ product.name|lower }}</p>
    <p>Title: {{ product.name|title }}</p>
    <p>Truncate: {{ product.description|truncate(20) }}</p>
    
    <!-- Number Filters -->
    <h2>3. Number Filters</h2>
    <p>Price: {{ product.price }}</p>
    <p>Rounded: {{ product.price|round }}</p>
    
    <!-- List Filters -->
    <h2>4. List Filters</h2>
    <p>Tags: {{ product.tags|join(", ") }}</p>
    <p>First tag: {{ product.tags|first }}</p>
    <p>Last tag: {{ product.tags|last }}</p>
    <p>Tag count: {{ product.tags|length }}</p>
    <p>Sorted: {{ product.tags|sort|join(", ") }}</p>
    
    <!-- Default Filter -->
    <h2>5. Default Values</h2>
    <p>Missing: {{ missing_var|default("N/A") }}</p>
    <p>Existing: {{ product.name|default("N/A") }}</p>
    
    <!-- Datetime Filters -->
    <h2>6. Date Formatting</h2>
    <p>Created: {{ product.created_at }}</p>
    <p>Formatted: {{ product.created_at.strftime("%B %d, %Y") }}</p>
    
    <!-- Conditional (Ternary) -->
    <h2>7. Conditional Expressions</h2>
    <p>Status: {{ "In Stock" if product.in_stock else "Out of Stock" }}</p>
    <p>Quantity: {{ product.quantity if product.quantity > 0 else "Sold Out" }}</p>
    
    <!-- Loop Variables -->
    <h2>8. Loop Variables</h2>
    <ul>
    {% for user in users %}
        <li>
            {{ loop.index }}. {{ user.name }} 
            (age: {{ user.age }})
            {% if loop.first %} - First!{% endif %}
            {% if loop.last %} - Last!{% endif %}
        </li>
    {% endfor %}
    </ul>
    
    <!-- Tests -->
    <h2>9. Tests (is)</h2>
    <p>"Alice" is defined: {{ "Alice" is defined }}</p>
    <p>product.name exists: {{ product.name is defined }}</p>
    <p>product.quantity is truthy: {{ product.quantity is truthy }}</p>
    <p>product.quantity is even: {{ product.quantity is even }}
    
    <!-- Length Test -->
    <h2>10. List Length with Test</h2>
    {% if product.tags|length > 2 %}
        <p>Product has many tags!</p>
    {% endif %}
    
    <!-- Chaining Filters -->
    <h2>11. Chaining Filters</h2>
    <p>{{ product.name|upper|reverse }}</p>
</body>
</html>
    """
    return render_template_string(template, product=product, users=users)

if __name__ == "__main__":
    app.run(debug=True)
```

### Custom Filters

You can create custom filters in Flask:

```python
# Custom filter example
from flask import Flask

app = Flask(__name__)

# Register a custom filter
@app.template_filter("reverse_string")
def reverse_string(s):
    """Reverse a string."""
    return s[::-1]

@app.template_filter("shout")
def shout(s):
    """Convert to uppercase with exclamation."""
    return s.upper() + "!"

# Use in template: {{ text|reverse_string }} or {{ text|shout }}
```

### Line-by-Line Breakdown

- `{{ product.name|upper }}` — Applies `upper` filter to `product.name`.
- `{{ product.tags|join(", ") }}` — Joins list with comma and space.
- `{{ missing_var|default("N/A") }}` — Shows "N/A" if variable is undefined.
- `{{ "In Stock" if product.in_stock else "Out of Stock" }}` — Ternary expression.
- `{{ loop.index }}` — Current iteration number (1-based).
- `{% if product.tags|length > 2 %}` — Using filter in test.
- `{{ product.name|upper|reverse }}` — Chaining filters: upper then reverse.

## Common Mistakes

❌ **Forgetting to use default for potentially missing values**
```html
<!-- WRONG — Will show empty if variable is missing -->
<p>{{ user.bio }}</p>
```

✅ **Correct — Use default filter**
```html
<!-- CORRECT — Shows "No bio" if missing -->
<p>{{ user.bio|default("No bio") }}</p>
```

❌ **Using Python methods directly**
```html
<!-- WRONG — Won't work in templates -->
<p>{{ product.price.round(2) }}</p>
```

✅ **Correct — Use Jinja2 filters**
```html
<!-- CORRECT — Use filters -->
<p>{{ product.price|round(2) }}</p>
```

## Quick Reference

| Filter | Description | Example |
|--------|-------------|---------|
| `upper` | Uppercase | `{{ "hi"|upper }}` → "HI" |
| `lower` | Lowercase | `{{ "HI"|lower }}` → "hi" |
| `title` | Title case | `{{ "hello world"|title }}` → "Hello World" |
| `length` | Length | `{{ list|length }}` |
| `join` | Join list | `{{ list|join(", ") }}` |
| `default` | Default value | `{{ x|default(0) }}` |
| `safe` | Mark HTML safe | `{{ html|safe }}` |

## Next Steps

Now you understand variables and filters. Continue to [03_control_structures.md](03_control_structures.md) to learn about Jinja2's control structures: loops, conditionals, and more.