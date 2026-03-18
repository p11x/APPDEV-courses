# Django Caching

## What You'll Learn
- Caching fundamentals and strategies
- Django's cache framework
- Per-view caching
- Template fragment caching
- Cache invalidation patterns

## Prerequisites
- Completed `05-django-signals.md` — Signal handling
- Understanding of HTTP and web applications

## Why Caching?

Caching stores expensive computations so you don't have to repeat them:

```
Without Cache:
Request ──▶ Database Query ──▶ Process ──▶ Response
                  (slow)           (slow)

With Cache:
Request ──▶ Cache Hit! ──▶ Response
                  (fast)
```

## Django Cache Backends

### Configuration

```python
# settings.py

# ─────────────────────────────────────────────
# Local Memory Cache (Development)
# ─────────────────────────────────────────────
CACHES = {
    "default": {
        "BACKEND": "django.core.cache.backends.locmem.LocMemCache",
        "LOCATION": "unique-snowflake",
    }
}

# ─────────────────────────────────────────────
# Redis Cache (Production - Recommended)
# ─────────────────────────────────────────────
CACHES = {
    "default": {
        "BACKEND": "django.core.cache.backends.redis.RedisCache",
        "LOCATION": "redis://127.0.0.1:6379/1",
        "OPTIONS": {
            "CLIENT_CLASS": "django_redis.client.DefaultClient",
        }
    }
}

# ─────────────────────────────────────────────
# Memcached (Production)
# ─────────────────────────────────────────────
CACHES = {
    "default": {
        "BACKEND": "django.core.cache.backends.memcached.PyMemcacheCache",
        "LOCATION": "127.0.0.1:11211",
    }
}
```

🔍 **Line-by-Line Breakdown:**
1. `LocMemCache` — Local memory, fast but not shared between processes
2. `RedisCache` — Persistent, shared across processes, supports more features
3. `PyMemcachedCache` — Fast, memory-based, production-grade

## Basic Cache API

```python
from django.core.cache import cache

# Set a value with expiration (seconds)
cache.set("my_key", "my_value", timeout=300)  # 5 minutes

# Get a value
value = cache.get("my_key")

# Get with default
value = cache.get("my_key", "default_value")

# Set only if key doesn't exist
cache.add("my_key", "value")  # Returns True if set, False if exists

# Delete a key
cache.delete("my_key")

# Clear all cache
cache.clear()

# Get multiple keys
cache.get_many(["key1", "key2"])

# Set multiple keys
cache.set_many({"key1": "value1", "key2": "value2"})
```

## Per-View Caching

### cache_page Decorator

```python
from django.views.decorators.cache import cache_page
from django.urls import path

# Cache view for 15 minutes
@cache_page(60 * 15)
def my_view(request):
    ...

# URL configuration
path("articles/", cache_page(60 * 15)(ArticleListView.as_view()), name="article-list")
```

### Cache in Class-Based Views

```python
from django.views.decorators.cache import cache_page
from django.utils.decorators import method_decorator
from django.views.generic import ListView
from .models import Article

@method_decorator(cache_page(60 * 15), name="dispatch")
class ArticleListView(ListView):
    model = Article
    
    # This view is now cached for 15 minutes
    # Different URLs = different cache entries
```

### Cache Based on User

```python
from django.views.decorators.cache import cache_control
from django.contrib.auth.decorators import login_required

# Public cache: same for all users
@cache_page(60 * 15)
@cache_control(public=True)
def public_view(request):
    ...

# Private cache: different for each user
@cache_page(60 * 5)
@cache_control(private=True)
@login_required
def private_view(request):
    ...
```

## Template Fragment Caching

```python
# Template: base.html
{% load cache %}

<!DOCTYPE html>
<html>
<head><title>{% block title %}{% endblock %}</title></head>
<body>
    {% cache 300 "header" %}
    <header>
        <nav>...</nav>
    </header>
    {% endcache %}
    
    {% block content %}{% endblock %}
    
    {% cache 300 "footer" %}
    <footer>...</footer>
    {% endcache %}
</body>
</html>

# Template: article_list.html
{% extends "base.html" %}
{% load cache %}

{% block title %}Articles{% endblock %}

{% block content %}
    {# Cache for 15 minutes, key includes user #}
    {% cache 900 "article_list" request.user.id %}
        {% for article in articles %}
            <h2>{{ article.title }}</h2>
            <p>{{ article.content|truncatewords:50 }}</p>
        {% endfor %}
    {% endcache %}
{% endblock %}
```

## Low-Level Cache API

### Caching QuerySets

```python
from django.core.cache import cache
from .models import Article

def get_articles():
    """Get articles from cache or database."""
    cache_key = "articles:published"
    articles = cache.get(cache_key)
    
    if articles is None:
        # Cache miss - query database
        articles = list(Article.objects.filter(status="published"))
        # Store in cache for 10 minutes
        cache.set(cache_key, articles, timeout=600)
    
    return articles

def get_article(article_id):
    """Get single article with caching."""
    cache_key = f"article:{article_id}"
    article = cache.get(cache_key)
    
    if article is None:
        try:
            article = Article.objects.get(id=article_id)
            cache.set(cache_key, article, timeout=300)
        except Article.DoesNotExist:
            return None
    
    return article
```

### Cache Versioning

```python
# Version 1
cache.set("key", "value", version=1)
cache.get("key", version=1)

# When you need to invalidate all cache
# Increment version in settings
CACHES = {
    "default": {
        "VERSION": 2,  # All keys now use version 2
    }
}

# Or programmatically
cache.incr_version("key")  # Increment specific key version
```

## Cache Invalidation

### Manual Invalidation

```python
from django.core.cache import cache

def update_article(article):
    """Update article and invalidate cache."""
    article.save()
    
    # Invalidate specific keys
    cache.delete(f"article:{article.id}")
    cache.delete("articles:published")  # List cache

def delete_article(article):
    """Delete article and invalidate cache."""
    article_id = article.id
    article.delete()
    
    # Invalidate
    cache.delete(f"article:{article_id}")
    cache.delete("articles:published")
```

### Automatic Invalidation with Signals

```python
# signals.py
from django.core.cache import cache
from django.db.models.signals import post_save, post_delete
from django.dispatch import receiver
from .models import Article

@receiver(post_save, sender=Article)
def invalidate_article_cache(sender, instance, created, **kwargs):
    """Clear cache when article changes."""
    cache.delete(f"article:{instance.id}")
    cache.delete("articles:published")

@receiver(post_delete, sender=Article)
def invalidate_article_delete(sender, instance, **kwargs):
    """Clear cache when article is deleted."""
    cache.delete(f"article:{instance.id}")
    cache.delete("articles:published")
```

## Caching Strategies

### Cache-Aside Pattern (Recommended)

```python
def get_user_profile(user_id):
    """Cache-aside: check cache first, then DB."""
    cache_key = f"profile:{user_id}"
    profile = cache.get(cache_key)
    
    if profile is None:
        # Cache miss - load from database
        try:
            profile = UserProfile.objects.get(user_id=user_id)
            cache.set(cache_key, profile, timeout=3600)
        except UserProfile.DoesNotExist:
            return None
    
    return profile
```

### Write-Through Pattern

```python
def save_profile(profile):
    """Write-through: update cache and DB together."""
    profile.save()  # Save to database
    
    # Update cache immediately
    cache.set(f"profile:{profile.user_id}", profile, timeout=3600)
```

## Production Considerations

- **Redis vs Memcached**: Redis is more feature-rich (persistence, clustering)
- **Cache keys**: Use descriptive keys with prefixes (e.g., "articles:123")
- **Expiration**: Set appropriate TTL based on data freshness needs
- **Monitoring**: Track cache hit/miss ratio
- **Cold start**: Plan for cache warming on deployment

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Caching too aggressively

**Wrong:**
```python
@cache_page(60 * 60)  # 1 hour!
def user_dashboard(request):
    # Shows personalized data - wrong for other users!
    return render(request, "dashboard.html")
```

**Why it fails:** Different users see each other's data.

**Fix:**
```python
# Use vary_on_cookie or vary_on_headers
@cache_page(60 * 15)
@login_required  # Only logged-in users
def user_dashboard(request):
    # Per-user cache key
    return render(request, "dashboard.html")
```

### ❌ Mistake 2: Never invalidating cache

**Wrong:**
```python
# Update database but cache is stale forever
def update_article(request, pk):
    article = get_object_or_404(Article, pk=pk)
    article.title = request.POST["title"]
    article.save()
    return redirect("article-detail", pk=pk)
# Cache still has old title!
```

**Why it fails:** Users see old data.

**Fix:**
```python
def update_article(request, pk):
    article = get_object_or_404(Article, pk=pk)
    article.title = request.POST["title"]
    article.save()
    
    # Invalidate cache
    cache.delete(f"article:{pk}")
    cache.delete("articles:published")
    
    return redirect("article-detail", pk=pk)
```

### ❌ Mistake 3: Using cache for sessions

**Wrong:**
```python
# Storing user sessions in default cache
cache.set(f"session:{session_key}", user_data)
```

**Why it fails:** Default cache can be cleared, losing sessions.

**Fix:**
```python
# Use separate cache backend for sessions
CACHES = {
    "default": {...},
    "sessions": {
        "BACKEND": "django.core.cache.backends.redis.RedisCache",
        "LOCATION": "redis://127.0.0.1:6379/2",
    }
}

SESSION_CACHE_ALIAS = "sessions"
```

## Summary

- Django supports multiple cache backends (Local memory, Redis, Memcached)
- Use `cache_page()` for entire view caching
- Use template `{% cache %}` for fragment caching
- Always invalidate cache when data changes
- Use descriptive cache keys with prefixes

## Next Steps

→ Continue to `07-django-testing.md` to learn testing strategies for Django applications.
