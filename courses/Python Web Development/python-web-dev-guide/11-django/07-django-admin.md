# Django Admin

## What You'll Learn
- Django admin features
- Customizing admin interface
- Registering models

## Prerequisites
- Completed `06-django-forms.md`

## Django Admin Overview

Django provides a built-in admin interface that allows you to manage your data. It's automatically generated from your models.

## Registering Models

```python
# blog/admin.py
from django.contrib import admin
from .models import Post

@admin.register(Post)
class PostAdmin(admin.ModelAdmin):
    list_display = ['title', 'author', 'published', 'created_at']
    list_filter = ['published', 'created_at']
    search_fields = ['title', 'content']
    prepopulated_fields = {'slug': ('title',)}
    date_hierarchy = 'created_at'
```

## Summary
- Django admin provides CRUD interface automatically
- Customize with ModelAdmin class
- Use list_display, list_filter, search_fields
