# Mixins and MRO in Django

## What You'll Learn
- Mixin pattern in Django class-based views
- Method Resolution Order (MRO) explained
- Building custom mixins
- Common Django mixins and their purposes
- Debugging mixin conflicts

## Prerequisites
- Completed `01-class-based-views.md` — CBV basics
- Understanding of Python inheritance
- Basic understanding of Django views

## What Are Mixins?

A **mixin** is a class that provides methods to other classes through inheritance, but is not meant for standalone use. Think of it as a "plugin" that adds functionality:

```python
# Mixin class - provides logging functionality
class LoggingMixin:
    """Add logging to any view."""
    
    def dispatch(self, request, *args, **kwargs):
        print(f"📝 {request.method} {request.path}")
        return super().dispatch(request, *args, **kwargs)

# View class - uses the mixin
class ArticleListView(LoggingMixin, ListView):
    """List articles with logging."""
    model = Article
```

🔍 **Line-by-Line Breakdown:**
1. `LoggingMixin` — Provides `dispatch` method but not meant to be used alone
2. `class ArticleListView(LoggingMixin, ListView)` — Inherits from both
3. `super().dispatch(...)` — Calls the next class in MRO

## Method Resolution Order (MRO)

MRO determines which method gets called when multiple classes have the same method name:

```python
class A:
    def greet(self):
        return "Hello from A"

class B:
    def greet(self):
        return "Hello from B"

class C(A, B):
    pass

# What does C().greet() return?
print(C().greet())  # "Hello from A" — A comes first!
```

Django's Multiple Inheritance pattern:
```python
# Typical CBV with mixins
class ArticleCreateView(LoginRequiredMixin, CreateView):
    #          ↑                    ↑
    #      Mixin            Base View
    pass
```

MRO for above: ArticleCreateView → LoginRequiredMixin → CreateView → Base → object

### Viewing MRO

```python
# Print method resolution order
print(ArticleCreateView.__mro__)

# Or in Django shell
>>> from blog.views import ArticleCreateView
>>> ArticleCreateView.__mro__
```

Output:
```
(<class 'blog.views.ArticleCreateView'>,
 <class 'django.contrib.auth.mixins.LoginRequiredMixin'>,
 <class 'django.views.generic.CreateView'>,
 <class 'django.views.generic.edit.ModelFormMixin'>,
 <class 'django.views.generic.edit.ProcessFormView'>,
 <class 'django.views.generic.base.TemplateResponseMixin'>,
 <class 'django.views.generic.base.View'>,
 <class 'object'>)
```

## Building Custom Mixins

### 1. Context Data Mixin

```python
from django.views.generic import ListView

class CommonContextMixin:
    """Add common data to every view context."""
    
    def get_context_data(self, **kwargs):
        """Add extra context."""
        context = super().get_context_data(**kwargs)
        context["site_name"] = "My Blog"
        context["current_year"] = 2024
        return context

class ArticleListView(CommonContextMixin, ListView):
    model = Article
    # Template now has {{ site_name }} and {{ current_year }}
```

### 2. Queryset Filtering Mixin

```python
class PublishedMixin:
    """Filter queryset to only show published items."""
    
    def get_queryset(self):
        return super().get_queryset().filter(published=True)

class ArticleListView(PublishedMixin, ListView):
    model = Article

class DraftListView(PublishedMixin, ListView):
    """Also filters by published - but can override!"""
    model = Article
    
    def get_queryset(self):
        return Article.objects.filter(published=False)
```

### 3. Owner Mixin (RBAC)

```python
class OwnerMixin:
    """Filter objects to only show user's own objects."""
    
    def get_queryset(self):
        """Only return objects owned by current user."""
        return super().get_queryset().filter(owner=self.request.user)

class MyArticlesView(OwnerMixin, ListView):
    """User's own articles."""
    model = Article
    template_name = "blog/my_articles.html"

class EditArticleView(OwnerMixin, UpdateView):
    """User can only edit their own articles."""
    model = Article
    fields = ["title", "content"]
```

### 4. AJAX Mixin

```python
import json
from django.http import JsonResponse

class AjaxMixin:
    """Handle AJAX requests differently."""
    
    def render_to_response(self, context, **response_kwargs):
        """Return JSON for AJAX, HTML for normal."""
        if self.request.headers.get("X-Requested-With") == "XMLHttpRequest":
            # Convert context to JSON-friendly format
            return JsonResponse(self.get_ajax_data(context))
        
        return super().render_to_response(context, **response_kwargs)
    
    def get_ajax_data(self, context):
        """Convert context to AJAX response format."""
        # Override in subclass
        return context

class ArticleListView(AjaxMixin, ListView):
    model = Article
    
    def get_ajax_data(self, context):
        return {
            "articles": list(context["object_list"].values("id", "title"))
        }
```

## Common Django Mixins

### LoginRequiredMixin

```python
from django.contrib.auth.mixins import LoginRequiredMixin

class SecretView(LoginRequiredMixin, ListView):
    login_url = "/login/"
    redirect_field_name = "next"
    model = Secret
```

### PermissionRequiredMixin

```python
from django.contrib.auth.mixins import PermissionRequiredMixin

class AdminView(PermissionRequiredMixin, View):
    permission_required = "auth.change_user"
    login_url = "/login/"
    
    def get(self, request):
        return render(request, "admin.html")
```

### UserPassesTestMixin

```python
from django.views.generic import View
from django.utils.decorators import method_decorator
from django.contrib.auth.decorators import user_passes_test

def is_premium_user(user):
    return user.is_authenticated and hasattr(user, "subscription")

@method_decorator(user_passes_test(is_premium_user), name="dispatch")
class PremiumContentView(TemplateView):
    template_name = "premium/content.html"
```

## Mixin Conflict Patterns

### The "Super" Problem

```python
# WRONG: Both mixins override get_context_data
class MixinA:
    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        context["a"] = "A"
        return context

class MixinB:
    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        context["b"] = "B"
        return context

# Only B's data appears!
class MyView(MixinA, MixinB, ListView):
    pass
```

**Solution:** Use cooperative inheritance:

```python
# CORRECT: Both call super()
class MixinA:
    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)  # Cooperative!
        context["a"] = "A"
        return context

class MixinB:
    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)  # Cooperative!
        context["b"] = "B"
        return context

# Both A and B appear!
class MyView(MixinA, MixinB, ListView):
    pass
```

## Advanced: Method Decorator Mixin

```python
from django.utils.decorators import method_decorator

def require_ajax(view_func):
    """Decorator requiring AJAX request."""
    def wrapper(request, *args, **kwargs):
        if request.headers.get("X-Requested-With") != "XMLHttpRequest":
            return JsonResponse({"error": "AJAX required"}, status=400)
        return view_func(request, *args, **kwargs)
    return wrapper

class RequireAjaxMixin:
    """Mixin requiring AJAX requests."""
    
    @method_decorator(require_ajax)
    def dispatch(self, request, *args, **kwargs):
        return super().dispatch(request, *args, **kwargs)

class ApiView(RequireAjaxMixin, View):
    def get(self, request):
        return JsonResponse({"data": "success"})
```

## Production Considerations

- **MRO matters**: Always put mixins before base classes in inheritance
- **Cooperative inheritance**: Always call `super()` in mixin methods
- **Testing**: Test mixin behavior in isolation
- **Documentation**: Document what your mixin adds to context

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Wrong MRO order

**Wrong:**
```python
class MyView(ListView, LoginRequiredMixin):
    # ListView comes first - won't work!
    pass
```

**Why it fails:** LoginRequiredMixin never gets called.

**Fix:**
```python
class MyView(LoginRequiredMixin, ListView):
    # Mixin must come BEFORE base view
    pass
```

### ❌ Mistake 2: Not calling super()

**Wrong:**
```python
class BrokenMixin:
    def get_queryset(self):
        return Article.objects.all()  # No super()!
```

**Why it fails:** Breaks other mixins that depend on chaining.

**Fix:**
```python
class WorkingMixin:
    def get_queryset(self):
        return super().get_queryset().filter(published=True)
```

### ❌ Mistake 3: Using non-cooperative mixin

**Wrong:**
```python
class OldMixin:
    def get_context_data(self):
        # Forgot **kwargs!
        return {"old": "data"}
```

**Why it fails:** Breaks other mixins' context.

**Fix:**
```python
class GoodMixin:
    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        context["new"] = "data"
        return context
```

## Summary

- Mixins add reusable functionality through multiple inheritance
- MRO determines method execution order — leftmost class wins
- Always call `super()` in mixin methods for cooperative inheritance
- Django provides built-in mixins: LoginRequiredMixin, PermissionRequiredMixin
- Put mixins before base classes in inheritance chain

## Next Steps

→ Continue to `03-django-forms-and-modelforms.md` to learn advanced form handling in Django.
