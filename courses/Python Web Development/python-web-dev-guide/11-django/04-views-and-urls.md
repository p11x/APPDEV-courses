# Views and URLs in Django

## What You'll Learn
- Creating function-based views
- URL routing and patterns
- Using reverse URL lookup
- HttpRequest and HttpResponse objects
- Handling different HTTP methods

## Prerequisites
- Completed `03-models-and-orm.md`

## What Are Views?

Views are the heart of your Django application. They are Python functions (or classes) that receive a web request and return a web response. Think of views as the "controller" in the MVC pattern — they contain the logic that processes user requests and determines what to send back.

## Your First View

Create views in your app's `views.py`:

```python
# blog/views.py
from django.http import HttpResponse
from django.shortcuts import render, get_object_or_404
from django.http import JsonResponse
from .models import Post

def post_list(request):
    """Display a list of all published posts."""
    posts = Post.objects.filter(published=True)
    return render(request, 'blog/post_list.html', {'posts': posts})

def post_detail(request, slug):
    """Display a single post."""
    post = get_object_or_404(Post, slug=slug, published=True)
    return render(request, 'blog/post_detail.html', {'post': post})

def about(request):
    """Simple about page."""
    return HttpResponse("This is the about page.")

def json_endpoint(request):
    """Return JSON response."""
    return JsonResponse({'message': 'Hello, World!'})
```

🔍 **Line-by-Line Breakdown:**

1. `from django.http import HttpResponse` — The most basic response type. It takes a string and returns it as the HTTP response body.

2. `from django.shortcuts import render` — The `render` function combines a template with context data and returns an HTML response.

3. `from django.shortcuts import get_object_or_404` — A convenience function that either gets the object or returns a 404 error if not found. This saves you from manually checking if the object exists.

4. `def post_list(request):` — Every view receives at least one argument: the `request` object, which contains all information about the request.

5. `posts = Post.objects.filter(published=True)` — Query the database for published posts.

6. `return render(request, 'blog/post_list.html', {'posts': posts})` — Render the template with context data. The dictionary becomes available as variables in the template.

7. `def post_detail(request, slug):` — URL parameters are passed as additional arguments.

8. `get_object_or_404(Post, slug=slug, published=True)` — Get post by slug, return 404 if not found.

9. `return JsonResponse({'message': 'Hello, World!'})` — Return JSON for APIs.

## Understanding HttpRequest

The `request` object contains all information about the incoming HTTP request:

```python
def analyze_request(request):
    # HTTP method (GET, POST, etc.)
    method = request.method
    
    # URL path
    path = request.path
    
    # Query string (everything after ?)
    query = request.GET.get('q')
    
    # POST data
    name = request.POST.get('name')
    
    # Headers
    user_agent = request.headers.get('User-Agent')
    
    # Cookies
    session_id = request.COOKIES.get('sessionid')
    
    # User (if authenticated)
    if request.user.is_authenticated:
        username = request.user.username
    
    return HttpResponse(f"Method: {method}, Path: {path}")
```

### Common HTTP Methods

| Method | Purpose | Safe/Idempotent |
|--------|---------|-----------------|
| GET | Retrieve data | Yes |
| POST | Create data | No |
| PUT | Replace data | Yes |
| PATCH | Update data | No |
| DELETE | Delete data | Yes |

### Handling Different Methods

```python
def my_view(request):
    if request.method == 'GET':
        # Handle GET request
        return render(request, 'form.html')
    elif request.method == 'POST':
        # Handle POST request
        name = request.POST.get('name')
        return HttpResponse(f"Hello, {name}!")
    else:
        return HttpResponseNotAllowed(['GET', 'POST'])
```

## URL Routing

URL patterns go in your app's `urls.py`:

```python
# blog/urls.py
from django.urls import path
from . import views

urlpatterns = [
    path('', views.post_list, name='post_list'),
    path('post/<slug:slug>/', views.post_detail, name='post_detail'),
    path('about/', views.about, name='about'),
    path('api/data/', views.json_endpoint, name='api_data'),
]
```

### URL Pattern Syntax

| Pattern | Matches |
|---------|---------|
| `''` | Homepage |
| `'about/'` | /about/ |
| `'<slug:slug>/'` | /my-post/ |
| `'<int:id>/'` | /42/ |
| `'<uuid:id>/'` | /abc-123/ |
| `'<path:path>/'` | /files/document.pdf/ |

### Connecting App URLs to Project URLs

In your project's `urls.py`:

```python
# myproject/urls.py
from django.contrib import admin
from django.urls import path, include

urlpatterns = [
    path('admin/', admin.site.urls),
    path('blog/', include('blog.urls')),
    path('', include('core.urls')),
]
```

## Using Reverse URL Lookup

Instead of hardcoding URLs in templates, use names:

```python
# In views - redirect
from django.shortcuts import redirect
from django.urls import reverse

def my_view(request):
    return redirect(reverse('post_detail', args=['my-slug']))

# Better: use redirect with name directly
def my_view(request):
    return redirect('post_detail', slug='my-slug')
```

```html
<!-- In templates -->
<a href="{% url 'post_detail' post.slug %}">{{ post.title }}</a>
<a href="{% url 'about' %}">About</a>
```

## Shortcut Functions

Django provides several shortcuts to reduce boilerplate:

### render()

Renders a template with context:

```python
return render(request, 'template.html', {'key': value})
```

### redirect()

Redirects to another URL:

```python
# By URL name
return redirect('post_detail', slug='my-post')

# By absolute URL
return redirect('/blog/my-post/')

# By named URL with reverse
return redirect(reverse('post_detail', args=['my-post']))
```

### get_object_or_404()

Gets object or returns 404:

```python
from django.shortcuts import get_object_or_404

post = get_object_or_404(Post, slug=slug, published=True)
```

## Class-Based Views

For more complex views, use class-based views:

```python
# blog/views.py
from django.views.generic import ListView, DetailView
from .models import Post

class PostListView(ListView):
    model = Post
    template_name = 'blog/post_list.html'
    context_object_name = 'posts'
    queryset = Post.objects.filter(published=True)
    paginate_by = 10

class PostDetailView(DetailView):
    model = Post
    template_name = 'blog/post_detail.html'
    context_object_name = 'post'

# urls.py
from .views import PostListView, PostDetailView

urlpatterns = [
    path('', PostListView.as_view(), name='post_list'),
    path('post/<slug:slug>/', PostDetailView.as_view(), name='post_detail'),
]
```

CBVs (Class-Based Views) provide:
- Pre-built handling for common patterns
- Mixins for additional functionality
- Cleaner code for complex views

## Summary
- Views receive HttpRequest and return HttpResponse
- Use `render()` for templates, `JsonResponse()` for APIs
- URL patterns map URLs to view functions
- Use `reverse()` or `redirect()` for URL generation
- Class-based views provide reusable view patterns

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Hardcoding URLs in templates
**Wrong:**
```html
<a href="/blog/my-post/">Read more</a>
```
**Why it fails:** If you change your URL structure, all links break.
**Fix:** Use reverse URL lookup:
```html
<a href="{% url 'post_detail' post.slug %}">Read more</a>
```

### ❌ Mistake 2: Not handling missing objects
**Wrong:**
```python
def post_detail(request, slug):
    post = Post.objects.get(slug=slug)  # Can raise DoesNotExist
    return render(request, 'post.html', {'post': post})
```
**Why it fails:** If post doesn't exist, raises exception.
**Fix:** Use get_object_or_404:
```python
def post_detail(request, slug):
    post = get_object_or_404(Post, slug=slug)
    return render(request, 'post.html', {'post': post})
```

### ❌ Mistake 3: Not checking request method
**Wrong:**
```python
def create_post(request):
    # Always creates, regardless of GET or POST
    title = request.POST.get('title')
    Post.objects.create(title=title)
    return HttpResponse("Created!")
```
**Why it fails:** GET requests (someone visiting the URL directly) will also create posts.
**Fix:** Check the method:
```python
def create_post(request):
    if request.method == 'POST':
        title = request.POST.get('title')
        Post.objects.create(title=title)
        return HttpResponse("Created!")
    return render(request, 'create_form.html')
```

## Next Steps
→ Continue to `05-django-templates.md` to learn about Django's template system.
