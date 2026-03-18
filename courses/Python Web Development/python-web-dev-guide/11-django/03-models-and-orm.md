# Django Models and ORM

## What You'll Learn
- Creating Django models
- Understanding Django ORM
- Database fields and types
- Relationships between models
- Querysets and queries

## Prerequisites
- Completed `02-django-project-structure.md`

## What Is Django ORM?

The **ORM (Object-Relational Mapping)** in Django lets you interact with databases using Python objects instead of writing raw SQL. Think of it as a translator that converts your Python code into database operations.

Instead of writing:
```sql
SELECT * FROM blog_post WHERE published = true;
```

You write:
```python
Post.objects.filter(published=True)
```

## Creating Your First Model

Models go in your app's `models.py` file:

```python
# blog/models.py
from django.db import models
from django.conf import settings
from django.utils import timezone

class Post(models.Model):
    """Blog post model."""
    title = models.CharField(max_length=200)  # VARCHAR
    slug = models.SlugField(max_length=200, unique=True)
    content = models.TextField()  # TEXT
    author = models.ForeignKey(
        settings.AUTH_USER_MODEL,
        on_delete=models.CASCADE,
        related_name='blog_posts'
    )
    created_at = models.DateTimeField(default=timezone.now)
    updated_at = models.DateTimeField(auto_now=True)
    published = models.BooleanField(default=False)
    published_at = models.DateTimeField(null=True, blank=True)

    class Meta:
        ordering = ['-published_at', '-created_at']
        indexes = [
            models.Index(fields=['-published_at']),
            models.Index(fields=['slug']),
        ]

    def __str__(self) -> str:
        return self.title
```

🔍 **Line-by-Line Breakdown:**

1. `from django.db import models` — Django's models module provides the base `Model` class and all field types.

2. `class Post(models.Model):` — Every Django model inherits from `models.Model`. This single inheritance gives you the entire ORM capability.

3. `title = models.CharField(max_length=200)` — A field for short text. The `max_length` is required for CharField and determines the VARCHAR size in the database.

4. `slug = models.SlugField(unique=True)` — A slug is a short label for URLs (like "my-first-post"). `unique=True` ensures no two posts have the same slug.

5. `content = models.TextField()` — For longer text. Unlike CharField, TextField has no max_length and stores as TEXT in the database.

6. `author = models.ForeignKey(...)` — A foreign key creates a many-to-one relationship. Each Post has one Author, but an Author can have many Posts.

7. `settings.AUTH_USER_MODEL` — Reference to the User model. This is the proper way to reference the User model in Django, as it allows for custom user models.

8. `on_delete=models.CASCADE` — When the author is deleted, all their posts are also deleted. Other options: `SET_NULL`, `PROTECT`, `DO_NOTHING`.

9. `related_name='blog_posts'` — Allows accessing all posts by an author via `user.blog_posts.all()`.

10. `created_at = models.DateTimeField(default=timezone.now)` — Automatically set to current time when created. `auto_now_add=True` is similar but can't be changed.

11. `updated_at = models.DateTimeField(auto_now=True)` — Automatically updated to current time every time the model is saved.

12. `class Meta:` — Inner class for model-level configuration.

13. `ordering = ['-published_at', '-created_at']` — Default ordering for queries. The minus sign means descending order.

14. `def __str__(self):` — Returns a human-readable representation. Shown in the admin panel and shell.

## Common Field Types

| Field | Database Type | Use For |
|-------|--------------|---------|
| CharField | VARCHAR | Short text (names, titles) |
| TextField | TEXT | Long text (articles, content) |
| IntegerField | INT | Whole numbers |
| BooleanField | BOOLEAN | True/False |
| DateField | DATE | Dates only |
| DateTimeField | DATETIME | Dates and times |
| EmailField | VARCHAR | Email addresses (with validation) |
| URLField | VARCHAR | URLs |
| SlugField | VARCHAR | URL-friendly strings |
| FileField | VARCHAR | File paths |
| ImageField | VARCHAR | Image paths |
| DecimalField | DECIMAL | Precise decimal numbers (money) |
| ForeignKey | INT (indexed) | Many-to-one relationships |
| ManyToManyField | M2M table | Many-to-many relationships |
| OneToOneField | INT (unique) | One-to-one relationships |

## Field Options

Common options available on most fields:

```python
class Product(models.Model):
    name = models.CharField(max_length=100)
    description = models.TextField(blank=True)  # Allow empty in forms
    price = models.DecimalField(max_digits=10, decimal_places=2, null=True)
    in_stock = models.BooleanField(default=True)
    category = models.CharField(max_length=50, choices=[
        ('electronics', 'Electronics'),
        ('clothing', 'Clothing'),
        ('food', 'Food'),
    ])
    barcode = models.CharField(max_length=50, unique=True, db_index=True)
```

- `null=True` — Allows NULL in database (for optional fields)
- `blank=True` — Allows empty in forms/validation
- `default=` — Default value
- `choices=` — Predefined options (like an enum)
- `unique=True` — No duplicate values
- `db_index=True` — Creates database index for faster queries

## Relationships

### ForeignKey (Many-to-One)

One author has many posts:

```python
class Author(models.Model):
    name = models.CharField(max_length=100)

class Post(models.Model):
    author = models.ForeignKey(Author, on_delete=models.CASCADE)
    title = models.CharField(max_length=200)

# Usage
author = Author.objects.first()
posts = author.post_set.all()  # All posts by this author

post = Post.objects.first()
author = post.author  # Get the author of a post
```

### ManyToManyField (Many-to-Many)

Posts can have multiple tags, tags can have multiple posts:

```python
class Tag(models.Model):
    name = models.CharField(max_length=50)

class Post(models.Model):
    tags = models.ManyToManyField(Tag, related_name='posts')

# Usage
post = Post.objects.first()
post.tags.add(tag1, tag2)
post.tags.remove(tag1)

tag = Tag.objects.first()
posts = tag.posts.all()
```

### OneToOneField (One-to-One)

A user has one profile:

```python
class Profile(models.Model):
    user = models.OneToOneField(User, on_delete=models.CASCADE)
    bio = models.TextField()
    avatar = models.ImageField(upload_to='avatars/')
```

## Making Migrations

After creating or modifying models, create migrations:

```bash
python manage.py makemigrations
```

This creates a migration file in your app's `migrations/` folder.

Apply migrations:

```bash
python manage.py migrate
```

## Querying Data

### Creating Records

```python
from blog.models import Post
from django.contrib.auth import get_user_model

User = get_user_model()
user = User.objects.first()

# Method 1: Create and save
post = Post(title="My Post", slug="my-post", content="Content", author=user)
post.save()

# Method 2: create (save in one step)
post = Post.objects.create(
    title="Another Post",
    slug="another-post",
    content="More content",
    author=user
)
```

### Reading Records

```python
# Get all posts
all_posts = Post.objects.all()

# Get first/last
first_post = Post.objects.first()
last_post = Post.objects.last()

# Get by primary key
post = Post.objects.get(pk=1)

# Filter
published_posts = Post.objects.filter(published=True)
recent_posts = Post.objects.filter(published_at__gte='2024-01-01')

# Exclude
draft_posts = Post.objects.exclude(published=True)

# Order
ordered_posts = Post.objects.order_by('-created_at')

# Limit
top_5 = Post.objects.all()[:5]

# Count
count = Post.objects.filter(published=True).count()

# Check existence
exists = Post.objects.filter(slug='my-post').exists()
```

### Update Records

```python
# Update single object
post = Post.objects.get(pk=1)
post.title = "New Title"
post.save()  # Saves all fields

# Update multiple objects
Post.objects.filter(published=False).update(published=True)
```

### Delete Records

```python
# Delete single object
post = Post.objects.get(pk=1)
post.delete()

# Delete multiple objects
Post.objects.filter(published=False).delete()
```

## Related Objects Lookup

Django provides convenient lookups across relationships:

```python
# Get all published posts by a specific author
posts = Post.objects.filter(author=user, published=True)

# Query across relationships
from django.db.models import F

# Get posts with author name containing "John"
posts = Post.objects.filter(author__name__icontains='john')

# Get posts with more than 5 comments
posts = Post.objects.filter(comments__gt=5)

# Aggregate functions
from django.db.models import Count, Avg, Max

# Count posts per author
authors = User.objects.annotate(post_count=Count('blog_posts'))
```

## Summary
- Django models define database tables as Python classes
- Models inherit from `models.Model`
- Field types map to database column types
- ForeignKey, ManyToManyField, OneToOneField define relationships
- Use `makemigrations` and `migrate` to sync models with database
- Querysets provide a chainable API for database operations

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Not setting on_delete on ForeignKey
**Wrong:**
```python
author = models.ForeignKey(User)  # Missing on_delete
```
**Why it fails:** Django will show a warning and in future versions this will be required.
**Fix:**
```python
author = models.ForeignKey(User, on_delete=models.CASCADE)
```

### ❌ Mistake 2: Using String instead of settings.AUTH_USER_MODEL
**Wrong:**
```python
author = models.ForeignKey('auth.User', ...)
```
**Why it fails:** This doesn't work with custom user models.
**Fix:**
```python
from django.conf import settings
author = models.ForeignKey(settings.AUTH_USER_MODEL, ...)
```

### ❌ Mistake 3: Modifying model without creating migrations
**Wrong:** Changing a model and expecting Django to automatically update the database.
**Why it fails:** Django doesn't automatically change the database schema.
**Fix:**
```bash
python manage.py makemigrations
python manage.py migrate
```

## Next Steps
→ Continue to `04-views-and-urls.md` to learn how to create views and URL patterns.
