# Django Signals

## What You'll Learn
- Django signal system
- Pre and post save signals
- Custom signals

## Prerequisites
- Completed `09-class-based-views.md`

## What Are Signals?

Signals allow certain senders to notify a set of receivers when certain actions have occurred. They're useful for decoupled applications.

## Built-in Signals

### Model Signals

```python
# blog/signals.py
from django.db.models.signals import post_save, pre_save, post_delete
from django.dispatch import receiver
from .models import Post

@receiver(post_save, sender=Post)
def post_saved(sender, instance, created, **kwargs):
    if created:
        print(f"New post created: {instance.title}")
    else:
        print(f"Post updated: {instance.title}")

@receiver(pre_save, sender=Post)
def pre_save_post(sender, instance, **kwargs):
    if not instance.slug:
        instance.slug = instance.title.lower().replace(' ', '-')

@receiver(post_delete, sender=Post)
def post_deleted(sender, instance, **kwargs):
    print(f"Post deleted: {instance.title}")
```

## Register Signals

```python
# apps.py
from django.apps import AppConfig

class BlogConfig(AppConfig):
    default_auto_field = 'django.db.models.BigAutoField'
    name = 'blog'

    def ready(self):
        import blog.signals
```

## Summary
- Signals notify receivers when actions occur
- Use @receiver decorator
- Common signals: post_save, pre_save, post_delete
- Register in AppConfig.ready()
