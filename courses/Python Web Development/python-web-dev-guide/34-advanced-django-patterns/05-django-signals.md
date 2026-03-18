# Django Signals

## What You'll Learn
- Signal fundamentals and use cases
- Built-in Django signals
- Creating custom signals
- Connecting receivers
- Practical examples with code

## Prerequisites
- Completed `04-django-orm-queries.md` — ORM queries
- Understanding of Django models

## What Are Signals?

Signals are Django's way to let you get notified when certain events happen. Think of them as a **mailbox** — when something happens (like a letter arrives), the signal notifies all interested parties (receivers).

```
Model.save() called
        │
        ▼
┌───────────────────┐
│   Signal Fired    │  ──▶  post_save
└───────────────────┘
        │
        ▼
┌───────────────────────────────────────────┐
│              Receivers                    │
│   - Send welcome email                    │
│   - Update search index                   │
│   - Create audit log                      │
│   - Notify admins                         │
└───────────────────────────────────────────┘
```

## Built-in Django Signals

### Model Signals

```python
# django.db.models.signals

# Called before/after model save()
pre_save.connect(receiver, sender=Model)
post_save.connect(receiver, sender=Model)

# Called before/after model delete()
pre_delete.connect(receiver, sender=Model)
post_delete.connect(receiver, sender=Model)

# Called when model is loaded from DB
m2m_changed.connect(receiver, sender=Model)
```

### Request/Response Signals

```python
# django.core.signals

# Request finished (always)
request_started.connect(receiver)
request_finished.connect(receiver)

# Exception occurred
got_request_exception.connect(receiver)
```

### Example: Post-Save Signal

```python
# signals.py
from django.db.models.signals import post_save
from django.dispatch import receiver
from django.contrib.auth import get_user_model
from django.core.mail import send_mail

User = get_user_model()

@receiver(post_save, sender=User)
def user_created_handler(sender, instance, created, **kwargs):
    """Send welcome email when user is created."""
    
    if created:
        print(f"🎉 New user created: {instance.email}")
        
        # Send welcome email
        send_mail(
            subject="Welcome to Our Site!",
            message=f"Hi {instance.username}, welcome!",
            from_email="noreply@example.com",
            recipient_list=[instance.email]
        )
```

🔍 **Line-by-Line Breakdown:**
1. `@receiver(post_save, sender=User)` — Decorator that registers function as receiver
2. `sender=User` — Only listen for User model events
3. `instance` — The model instance that was saved
4. `created` — Boolean: True for new objects, False for updates

### Register Signals in Apps

```python
# apps.py
from django.apps import AppConfig

class BlogConfig(AppConfig):
    name = "blog"
    
    def ready(self):
        # Import signals to register them
        import blog.signals  # noqa
```

```python
# __init__.py (in app directory)
default_app_config = "blog.apps.BlogConfig"
```

## Pre-Save Signal

```python
from django.db.models.signals import pre_save
from django.dispatch import receiver

@receiver(pre_save, sender=Article)
def slugify_title(sender, instance, **kwargs):
    """Auto-generate slug from title before saving."""
    if not instance.slug:
        from django.utils.text import slugify
        instance.slug = slugify(instance.title)
        
        # Make unique if needed
        original_slug = instance.slug
        counter = 1
        while Article.objects.filter(slug=instance.slug).exists():
            instance.slug = f"{original_slug}-{counter}"
            counter += 1
```

## Post-Delete Signal

```python
from django.db.models.signals import post_delete
from django.dispatch import receiver
from django.conf import settings
import os

@receiver(post_delete, sender=Article)
def delete_related_files(sender, instance, **kwargs):
    """Delete article's image when article is deleted."""
    if instance.image:
        # Get file path
        file_path = instance.image.path
        
        if os.path.exists(file_path):
            os.remove(file_path)
            print(f"🗑️ Deleted file: {file_path}")
```

## Custom Signals

### Creating Custom Signals

```python
# signals.py
from django.dispatch import Signal

# Custom signal with data
order_completed = Signal()

# Custom signal with arguments
order_status_changed = Signal()
```

### Emitting Custom Signals

```python
# tasks.py (or views.py)
from .signals import order_completed, order_status_changed

def process_order(order_id):
    """Process order and emit signals."""
    order = Order.objects.get(id=order_id)
    
    # Do processing...
    order.status = "completed"
    order.save()
    
    # Emit signal with no arguments
    order_completed.send(sender=Order, order=order)
    
    # Emit signal with arguments
    order_status_changed.send(
        sender=Order,
        order=order,
        old_status="pending",
        new_status="completed"
    )
```

### Connecting to Custom Signals

```python
# receivers.py
from django.dispatch import receiver
from .signals import order_completed, order_status_changed

@receiver(order_completed)
def send_order_confirmation(sender, order, **kwargs):
    """Send confirmation when order completes."""
    send_mail(
        subject=f"Order #{order.id} Completed",
        message="Your order has been processed!",
        recipient_list=[order.customer.email]
    )

@receiver(order_status_changed)
def notify_status_change(sender, order, old_status, new_status, **kwargs):
    """Notify customer of status change."""
    print(f"Order {order.id} changed: {old_status} → {new_status}")
```

## Many-to-Many Signals

```python
from django.db.models.signals import m2m_changed

@receiver(m2m_changed, sender=Article.tags.through)
def tags_changed(sender, instance, action, **kwargs):
    """React to tags being added/removed."""
    
    if action == "post_add":
        print(f"Tags added to {instance.title}")
    elif action == "post_remove":
        print(f"Tags removed from {instance.title}")
    elif action == "post_clear":
        print(f"Tags cleared from {instance.title}")
```

## Signal Receiver Options

### sender — Filter by model

```python
# Only for Article model
@receiver(post_save, sender=Article)
def article_saved(**kwargs):
    pass

# Only for published articles
@receiver(post_save, sender=Article)
def published_article_saved(sender, instance, created, **kwargs):
    if instance.status == "published":
        # Index in search
        pass
```

### dispatch_uid — Prevent duplicate registration

```python
# Even if this file is imported twice, only runs once
@receiver(post_save, sender=User, dispatch_uid="my_unique_id")
def my_handler(sender, **kwargs):
    pass
```

### weak — Prevent memory leaks

```python
# Default is weak=True (receiver can be garbage collected)
# Set weak=False to keep receiver alive
@receiver(post_save, sender=User, weak=False)
def strong_handler(sender, **kwargs):
    pass
```

## Practical Examples

### Example 1: Audit Trail

```python
# models.py
class AuditLog(models.Model):
    action = models.CharField(max_length=50)
    model = models.CharField(max_length=100)
    object_id = models.IntegerField()
    user = models.ForeignKey("auth.User", on_delete=models.SET_NULL, null=True)
    changes = models.JSONField(default=dict)
    timestamp = models.DateTimeField(auto_now_add=True)

# signals.py
@receiver(post_save)
def audit_log_save(sender, instance, created, **kwargs):
    """Log all model saves."""
    if sender._meta.app_label == "blog":  # Only for blog app
        AuditLog.objects.create(
            action="create" if created else "update",
            model=sender._meta.model_name,
            object_id=instance.pk,
            user=getattr(instance, "_current_user", None)
        )
```

### Example 2: Search Index

```python
# signals.py
@receiver(post_save, sender=Article)
def update_search_index(sender, instance, created, **kwargs):
    """Update search index when article changes."""
    if instance.status == "published":
        # Add to search index (e.g., Elasticsearch)
        index_article(instance)
    else:
        remove_from_index(instance)

@receiver(post_delete, sender=Article)  
def remove_from_search_index(sender, instance, **kwargs):
    """Remove from search index on delete."""
    remove_from_index(instance)
```

### Example 3: Denormalized Counts

```python
# models.py
class Category(models.Model):
    name = models.CharField(max_length=100)
    article_count = models.IntegerField(default=0)  # Denormalized!

# signals.py
@receiver(post_save, sender=Article)
def update_category_count(sender, instance, created, **kwargs):
    """Update denormalized count."""
    if created:
        Category.objects.filter(pk=instance.category_id).update(
            article_count=models.F("article_count") + 1
        )

@receiver(post_delete, sender=Article)
def decrement_category_count(sender, instance, **kwargs):
    """Decrement count on delete."""
    Category.objects.filter(pk=instance.category_id).update(
        article_count=models.F("article_count") - 1
    )
```

## Production Considerations

- **Performance**: Signals run synchronously; heavy processing should use async tasks
- **Testing**: Mock signals in tests to avoid side effects
- **Order**: Signal order isn't guaranteed; don't depend on other signals
- **Debugging**: Use logging, not print()

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Forgetting to import signals

**Wrong:**
```python
# signals.py exists but not imported in apps.py
```

**Why it fails:** Signals never get registered.

**Fix:**
```python
# In your app's apps.py
def ready(self):
    import myapp.signals
```

### ❌ Mistake 2: Heavy operations in signals

**Wrong:**
```python
@receiver(post_save, sender=User)
def heavy_operation(sender, instance, created, **kwargs):
    # Send 100 emails, generate PDF, upload to S3
    # Blocks response!
```

**Why it fails:** Blocks HTTP response until complete.

**Fix:**
```python
@receiver(post_save, sender=User)
def schedule_heavy_operation(sender, instance, created, **kwargs):
    if created:
        # Use Celery/task queue
        process_user_task.delay(instance.pk)
```

### ❌ Mistake 3: Circular imports

**Wrong:**
```python
# models.py imports from signals.py
# signals.py imports from models.py
```

**Why it fails:** Python can't resolve imports.

**Fix:**
```python
# Use string references
@receiver(post_save, sender="blog.Article")
def handler(**kwargs):
    pass
```

## Summary

- Signals notify you when events happen in Django
- Built-in signals: `pre_save`, `post_save`, `pre_delete`, `post_delete`, `m2m_changed`
- Use `@receiver` decorator to connect handlers
- Register signals in `apps.py` `ready()` method
- For heavy tasks, use task queues instead of blocking signals

## Next Steps

→ Continue to `06-django-caching.md` to learn about caching strategies for Django applications.
