<!-- FILE: 03_templates/02_template_inheritance/03_macros.md -->

## Overview

**Macros** are reusable template components that work like functions. They accept arguments, return rendered content, and help eliminate repetitive HTML code. Use macros for form fields, UI components, or any HTML snippet you need multiple times with slight variations.

## Prerequisites

- Understanding of Jinja2 templates
- Familiarity with template inheritance

## Core Concepts

### Macro Definition

Macros are defined with `{% macro %}` and called like functions:

```html
{% macro input(name, label, type='text') %}
    <label>{{ label }}</label>
    <input type="{{ type }}" name="{{ name }}">
{% endmacro %}
```

### Using Macros

```html
{{ input('email', 'Email', 'email') }}
{{ input('password', 'Password', 'password') }}
```

### Macro Files

Store macros in a separate file and import them:

```html
{% from 'macros.html' import input, button %}
```

## Code Walkthrough

### Form Macros Example

```html
<!-- templates/macros/form_macros.html -->
{% macro input_field(name, label, type='text', value='', required=false) %}
    <div class="form-group">
        <label for="{{ name }}">{{ label }}</label>
        <input 
            type="{{ type }}" 
            name="{{ name }}" 
            id="{{ name }}"
            value="{{ value }}"
            {% if required %}required{% endif %}
        >
    </div>
{% endmacro %}

{% macro textarea_field(name, label, rows=5, value='') %}
    <div class="form-group">
        <label for="{{ name }}">{{ label }}</label>
        <textarea name="{{ name }}" id="{{ name }}" rows="{{ rows }}">{{ value }}</textarea>
    </div>
{% endmacro %}

{% macro select_field(name, label, options, selected='') %}
    <div class="form-group">
        <label for="{{ name }}">{{ label }}</label>
        <select name="{{ name }}" id="{{ name }}">
            <option value="">-- Select --</option>
            {% for value, display in options %}
                <option value="{{ value }}" {% if selected == value %}selected{% endif %}>
                    {{ display }}
                </option>
            {% endfor %}
        </select>
    </div>
{% endmacro %}

{% macro checkbox_field(name, label, checked=false) %}
    <div class="form-group checkbox">
        <input type="checkbox" name="{{ name }}" id="{{ name }}" {% if checked %}checked{% endif %}>
        <label for="{{ name }}">{{ label }}</label>
    </div>
{% endmacro %}

{% macro submit_button(text='Submit') %}
    <div class="form-group">
        <button type="submit">{{ text }}</button>
    </div>
{% endmacro %}
```

### Using Macros in Templates

```html
<!-- templates/contact.html -->
{% extends "base.html" %}
{% from "macros/form_macros.html" import input_field, textarea_field, submit_button %}

{% block title %}Contact Us{% endblock %}

{% block content %}
<form method="post" class="contact-form">
    {{ input_field('name', 'Your Name', required=true) }}
    {{ input_field('email', 'Email Address', type='email', required=true) }}
    {{ input_field('phone', 'Phone Number', type='tel') }}
    {{ textarea_field('message', 'Message', rows=6, required=true) }}
    {{ submit_button('Send Message') }}
</form>
{% endblock %}
```

### UI Component Macros

```html
<!-- templates/macros/ui_macros.html -->
{% macro alert(message, type='info') %}
    <div class="alert alert-{{ type }}">
        {{ message }}
        <button type="button" class="close-alert">&times;</button>
    </div>
{% endmacro %}

{% macro card(title, content, footer='') %}
    <div class="card">
        <div class="card-header">
            <h3>{{ title }}</h3>
        </div>
        <div class="card-body">
            {{ content }}
        </div>
        {% if footer %}
        <div class="card-footer">
            {{ footer }}
        </div>
        {% endif %}
    </div>
{% endmacro %}

{% macro badge(text, color='primary') %}
    <span class="badge badge-{{ color }}">{{ text }}</span>
{% endmacro %}

{% macro avatar(name, size='medium') %}
    <div class="avatar avatar-{{ size }}">
        {{ name[0]|upper }}
    </div>
{% endmacro %}
```

### Using UI Macros

```html
<!-- templates/dashboard.html -->
{% extends "base.html" %}
{% from "macros/ui_macros.html" import alert, card, badge, avatar %}

{% block content %}
{{ alert('Welcome back!', 'success') }}

{{ card('User Profile', 
    '<p>Name: John Doe</p><p>Email: john@example.com</p>',
    footer='<a href="/profile/edit">Edit Profile</a>'
) }}

<div class="user-list">
    {% for user in users %}
        <div class="user-item">
            {{ avatar(user.name) }}
            <span>{{ user.name }}</span>
            {{ badge(user.role, 'primary' if user.role == 'admin' else 'secondary') }}
        </div>
    {% endfor %}
</div>
{% endblock %}
```

### Macro with Conditional Logic

```html
<!-- templates/macros/list_macros.html -->
{% macro render_list(items, ordered=false, empty_message='No items') %}
    {% if items %}
        {% if ordered %}
        <ol>
        {% else %}
        <ul>
        {% endif %}
            {% for item in items %}
            <li>{{ item }}</li>
            {% endfor %}
        {% if ordered %}
        </ol>
        {% else %}
        </ul>
        {% endif %}
    {% else %}
        <p class="empty">{{ empty_message }}</p>
    {% endif %}
{% endmacro %}

{% macro pagination(current_page, total_pages, url_pattern) %}
    <nav class="pagination">
        {% if current_page > 1 %}
            <a href="{{ url_pattern|format(current_page - 1) }}">Previous</a>
        {% endif %}
        
        {% for page in range(1, total_pages + 1) %}
            {% if page == current_page %}
                <span class="current">{{ page }}</span>
            {% else %}
                <a href="{{ url_pattern|format(page) }}">{{ page }}</a>
            {% endif %}
        {% endfor %}
        
        {% if current_page < total_pages %}
            <a href="{{ url_pattern|format(current_page + 1) }}">Next</a>
        {% endif %}
    </nav>
{% endmacro %}
```

## Common Mistakes

❌ **Not importing macros before using**
```html
<!-- WRONG — Using macro without import -->
{{ input('name', 'Name') }}
```

✅ **Correct — Import first**
```html
<!-- CORRECT -->
{% from "macros/form_macros.html" import input %}
{{ input('name', 'Name') }}
```

❌ **Wrong macro file path**
```html
<!-- WRONG — Path must be correct -->
{% from "forms.html" import input %}
```

✅ **Correct — Use correct path**
```html
<!-- CORRECT -->
{% from "macros/form_macros.html" import input %}
```

## Quick Reference

| Syntax | Description |
|--------|-------------|
| `{% macro name() %}{% endmacro %}` | Define macro |
| `{% from "file.html" import macro %}` | Import macro |
| `{{ macro(arg) }}` | Call macro |
| `{{ macro(arg, default=value) }}` | With default argument |

## Next Steps

Now you understand macros. Continue to [01_serving_css_js.md](../03_static_files/01_serving_css_js.md) to learn about serving static files like CSS and JavaScript in Flask.