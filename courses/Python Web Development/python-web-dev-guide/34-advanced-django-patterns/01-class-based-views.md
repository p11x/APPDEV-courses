# Class-Based Views in Django

## What You'll Learn
- Function-based vs class-based views
- Built-in generic CBVs (ListView, DetailView, etc.)
- CBV method dispatch pattern
- Customizing CBV behavior with mixins
- Building reusable view components

## Prerequisites
- Completed basic Django tutorial — Models, URLs, Templates
- Understanding of HTTP methods (GET, POST, PUT, DELETE)
- Basic Python OOP knowledge

## Function-Based vs Class-Based Views

**Function-Based View (FBV):**
```python
from django.http import JsonResponse
from .models import Article

def article_list(request):
    """Traditional function-based view."""
    if request.method == "GET":
        articles = Article.objects.all()
        return JsonResponse({"articles": [a.title for a in articles]})
    
    elif request.method == "POST":
        # Create new article
        ...
```

**Class-Based View (CBV):**
```python
from django.views import View
from django.http import JsonResponse
from .models import Article

class ArticleListView(View):
    """Class-based view - more organized."""
    
    def get(self, request):
        """Handle GET requests."""
        articles = Article.objects.all()
        return JsonResponse({"articles": [a.title for a in articles]})
    
    def post(self, request):
        """Handle POST requests."""
        # Create new article
        ...
```

🔍 **Line-by-Line Breakdown:**
1. `class ArticleListView(View)` — Inherits from Django's base View class
2. `def get(self, request)` — Methods named after HTTP verbs
3. CBV automatically dispatches based on HTTP method
4. More testable, reusable, and extensible than FBVs

## Django's Generic CBVs

Django provides pre-built class-based views for common patterns:

### ListView — Display a list of objects

```python
# views.py
from django.views.generic import ListView
from .models import Article

class ArticleListView(ListView):
    """Display list of articles."""
    model = Article  # Automatically queries: Article.objects.all()
    template_name = "blog/article_list.html"
    context_object_name = "articles"  # Template variable name
    paginate_by = 10  # Add pagination
    
    def get_queryset(self):
        """Customize queryset."""
        return Article.objects.filter(published=True).order_by("-created_at")
```

```html
<!-- template: blog/article_list.html -->
{% extends "base.html" %}

{% block content %}
<h1>Articles</h1>

{% for article in articles %}
    <article>
        <h2>{{ article.title }}</h2>
        <p>{{ article.summary }}</p>
    </article>
{% endfor %}

<!-- Pagination -->
{% if page_obj.has_previous %}
    <a href="?page=1">First</a>
    <a href="?page={{ page_obj.previous_page_number }}">Previous</a>
{% endif %}

<span>Page {{ page_obj.number }} of {{ page_obj.paginator.num_pages }}</span>

{% if page_obj.has_next %}
    <a href="?page={{ page_obj.next_page_number }}">Next</a>
    <a href="?page={{ page_obj.paginator.num_pages }}">Last</a>
{% endif %}
{% endblock %}
```

### DetailView — Display single object

```python
# views.py
from django.views.generic import DetailView
from .models import Article

class ArticleDetailView(DetailView):
    """Display single article."""
    model = Article
    template_name = "blog/article_detail.html"
    context_object_name = "article"
    
    def get_queryset(self):
        """Only show published articles."""
        return Article.objects.filter(published=True)
```

```html
<!-- template: blog/article_detail.html -->
{% extends "base.html" %}

{% block content %}
<article>
    <h1>{{ article.title }}</h1>
    <p class="meta">By {{ article.author }} on {{ article.created_at }}</p>
    <div class="content">
        {{ article.content|linebreaks }}
    </div>
</article>
{% endblock %}
```

### CreateView — Form for creating objects

```python
from django.views.generic import CreateView
from django.urls import reverse_lazy
from .models import Article
from .forms import ArticleForm

class ArticleCreateView(CreateView):
    """Create new article."""
    model = Article
    form_class = ArticleForm  # Use ModelForm
    template_name = "blog/article_form.html"
    success_url = reverse_lazy("article-list")  # Redirect after success
```

### UpdateView — Form for editing objects

```python
from django.views.generic import UpdateView
from .models import Article
from .forms import ArticleForm

class ArticleUpdateView(UpdateView):
    """Edit existing article."""
    model = Article
    form_class = ArticleForm
    template_name = "blog/article_form.html"
    
    def get_success_url(self):
        """Redirect to article detail after update."""
        return reverse_lazy("article-detail", kwargs={"pk": self.object.pk})
```

### DeleteView — Confirm and delete objects

```python
from django.views.generic import DeleteView
from django.urls import reverse_lazy
from .models import Article

class ArticleDeleteView(DeleteView):
    """Delete article."""
    model = Article
    template_name = "blog/article_confirm_delete.html"
    success_url = reverse_lazy("article-list")
```

## CBV URL Configuration

```python
# urls.py
from django.urls import path
from . import views

urlpatterns = [
    path("articles/", views.ArticleListView.as_view(), name="article-list"),
    path("articles/<int:pk>/", views.ArticleDetailView.as_view(), name="article-detail"),
    path("articles/create/", views.ArticleCreateView.as_view(), name="article-create"),
    path("articles/<int:pk>/edit/", views.ArticleUpdateView.as_view(), name="article-edit"),
    path("articles/<int:pk>/delete/", views.ArticleDeleteView.as_view(), name="article-delete"),
]
```

## CBV Method Dispatch Flow

```
Request arrives
       │
       ▼
┌──────────────────────────────────────┐
│  dispatch(request, *args, **kwargs)  │
│  - Check HTTP method                 │
│  - Initialize view                   │
│       │                              │
│       ▼                              │
│  ┌─────────────────────────────┐     │
│  │ HTTP method (get, post, etc) │     │
│  └─────────────────────────────┘     │
│       │                              │
│       ▼                              │
│  Returns HttpResponse                │
└──────────────────────────────────────┘
```

### Customizing dispatch

```python
from django.views.generic import ListView
from django.http import JsonResponse
from .models import Article

class ArticleAPIView(ListView):
    """JSON API view for articles."""
    model = Article
    
    def dispatch(self, request, *args, **kwargs):
        """Handle different formats."""
        # Check for JSON request
        if request.headers.get("Accept") == "application/json":
            # Return JSON instead of rendering template
            return self.json_response()
        
        return super().dispatch(request, *args, **kwargs)
    
    def json_response(self):
        """Return JSON response."""
        data = list(self.get_queryset().values("id", "title", "created_at"))
        return JsonResponse({"articles": data})
    
    def get(self, request):
        """Override GET for custom handling."""
        return self.dispatch(request)
```

## Mixins for Reusability

Mixins add functionality to CBVs:

```python
from django.views.generic import ListView
from django.contrib.auth.mixins import LoginRequiredMixin

# Only logged-in users can access
class ArticleListView(LoginRequiredMixin, ListView):
    """List view requiring login."""
    model = Article
    login_url = "/accounts/login/"  # Redirect here if not logged in
```

### Common Django Mixins

```python
from django.contrib.auth.mixins import (
    LoginRequiredMixin,        # Require authentication
    PermissionRequiredMixin,   # Require specific permission
    UserPassesTestMixin,      # Custom test
)
from django.mixin import (
    ContextMixin,             # Add context data
    TemplateResponseMixin,    # Render template
)

# Example: Custom permission mixin
class OwnerRequiredMixin:
    """Mixin ensuring user owns the object."""
    
    def get_queryset(self):
        """Only show user's own objects."""
        return self.model.objects.filter(owner=self.request.user)

class MyItemListView(OwnerRequiredMixin, ListView):
    model = Item
```

## Building a Custom CBV from Scratch

```python
from django.views import View
from django.http import JsonResponse
from django.shortcuts import get_object_or_404
from .models import Article

class ArticleVoteView(View):
    """Vote on an article."""
    
    def get(self, request, pk):
        """Get current vote count."""
        article = get_object_or_404(Article, pk=pk)
        return JsonResponse({
            "votes": article.votes,
            "user_voted": article.votes.filter(id=request.user.id).exists()
        })
    
    def post(self, request, pk):
        """Vote for article."""
        article = get_object_or_404(Article, pk=pk)
        
        if request.user in article.votes.all():
            article.votes.remove(request.user)
            return JsonResponse({"votes": article.votes.count(), "voted": False})
        else:
            article.votes.add(request.user)
            return JsonResponse({"votes": article.votes.count(), "voted": True})
```

## Production Considerations

- **URL names**: Use `reverse_lazy` for CBV success URLs
- **LoginRequired**: Don't forget `LoginRequiredMixin` for protected views
- **Pagination**: Use Django's built-in pagination in ListView
- **Testing**: CBVs are easier to test than FBVs

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Forgetting .as_view()

**Wrong:**
```python
path("articles/", views.ArticleListView, name="article-list")
```

**Why it fails:** CBVs must be called with `.as_view()`.

**Fix:**
```python
path("articles/", views.ArticleListView.as_view(), name="article-list")
```

### ❌ Mistake 2: Wrong template context

**Wrong:**
```python
# In template, trying to access {{ article_list }}
# But didn't set context_object_name
```

**Why it fails:** Default context is `object_list`.

**Fix:**
```python
class ArticleListView(ListView):
    model = Article
    context_object_name = "articles"  # Now use {{ articles }}
```

### ❌ Mistake 3: Not handling form validation errors

**Wrong:**
```python
class ArticleCreateView(CreateView):
    # No form_class defined!
```

**Why it fails:** CreateView needs a form.

**Fix:**
```python
class ArticleCreateView(CreateView):
    model = Article
    fields = ["title", "content", "published"]  # Or form_class = ArticleForm
```

## Summary

- CBVs organize code by HTTP method rather than conditional logic
- Django provides generic views: ListView, DetailView, CreateView, UpdateView, DeleteView
- Use `as_view()` in URL configuration
- Mixins add reusable functionality
- CBVs are more testable and extensible than FBVs

## Next Steps

→ Continue to `02-mixins-and-mro.md` to understand mixin patterns and method resolution order in Django.
