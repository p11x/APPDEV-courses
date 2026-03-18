# Django Middleware

## What You'll Learn
- Middleware fundamentals and request/response flow
- Built-in Django middleware
- Creating custom middleware
- Middleware order and configuration
- Practical examples

## Prerequisites
- Completed `06-django-caching.md` — Caching strategies
- Understanding of HTTP requests and responses

## What Is Middleware?

Middleware is a series of hooks that process requests before they reach views, and responses before they leave views:

```
Request Flow:
─────────────────────────────────────────────────────
Client
    │
    ▼
┌─────────────────────────────────────────────────┐
│ Middleware 1 (e.g., Authentication)            │
│   - Processes request                           │
│   - May modify request                         │
│   - May return early (short-circuit)            │
└─────────────────────────────────────────────────┘
    │
    ▼
┌─────────────────────────────────────────────────┐
│ Middleware 2 (e.g., Session)                    │
└─────────────────────────────────────────────────┘
    │
    ▼
┌─────────────────────────────────────────────────┐
│ Middleware 3                                    │
└─────────────────────────────────────────────────┘
    │
    ▼
View (your code)

Response Flow (reverse order):
View → Middleware 3 → Middleware 2 → Middleware 1 → Client
```

## Built-in Django Middleware

```python
# settings.py

MIDDLEWARE = [
    # Security middleware (top - processes first)
    'django.middleware.security.SecurityMiddleware',
    
    # Session middleware
    'django.contrib.sessions.middleware.SessionMiddleware',
    
    # Common middleware (URLs, CSRF, etc)
    'django.middleware.common.CommonMiddleware',
    
    # Authentication
    'django.contrib.auth.middleware.AuthenticationMiddleware',
    
    # Message framework
    'django.contrib.messages.middleware.MessageMiddleware',
    
    # CSRF protection
    'django.middleware.csrf.CsrfViewMiddleware',
]
```

🔍 **Line-by-Line Breakdown:**
1. `SecurityMiddleware` — Handles security headers, HTTPS redirect
2. `SessionMiddleware` — Enables session management
3. `CommonMiddleware` — Base middleware features
4. `AuthenticationMiddleware` — Attaches user to request
5. `MessageMiddleware` — Flash messages
6. `CsrfViewMiddleware` — CSRF token validation

## Creating Custom Middleware

### Basic Function-Based Middleware

```python
# middleware.py

def RequestLoggingMiddleware(get_response):
    """Log all requests."""
    
    def middleware(request):
        # Code before view (request phase)
        print(f"📝 {request.method} {request.path}")
        
        # Call next middleware or view
        response = get_response(request)
        
        # Code after view (response phase)
        print(f"📤 Response: {response.status_code}")
        
        return response
    
    return middleware
```

### Class-Based Middleware

```python
# middleware.py
from django.utils.deprecation import MiddlewareMixin

class RequestTimingMiddleware(MiddlewareMixin):
    """Track request processing time."""
    
    def process_request(self, request):
        """Called before view."""
        import time
        request.start_time = time.time()
        return None
    
    def process_response(self, request, response):
        """Called after view."""
        if hasattr(request, "start_time"):
            import time
            duration = time.time() - request.start_time
            response["X-Request-Duration"] = str(duration)
        return response
```

### Modern Middleware Style (Django 3.2+)

```python
# middleware.py
import time

class RequestTimingMiddleware:
    """Modern middleware using __call__."""
    
    def __init__(self, get_response):
        self.get_response = get_response
    
    def __call__(self, request):
        # Before
        start_time = time.time()
        
        # Call next middleware/view
        response = self.get_response(request)
        
        # After
        duration = time.time() - start_time
        response["X-Request-Duration"] = f"{duration:.3f}s"
        
        return response
```

## Practical Middleware Examples

### 1. Authentication Middleware

```python
# middleware.py
class UserAgentMiddleware:
    """Add user agent info to request."""
    
    def __init__(self, get_response):
        self.get_response = get_response
    
    def __call__(self, request):
        # Extract user agent
        user_agent = request.META.get("HTTP_USER_AGENT", "")
        
        # Add parsed info to request
        request.is_mobile = "Mobile" in user_agent
        request.is_bot = "Bot" in user_agent or "Spider" in user_agent
        
        response = self.get_response(request)
        return response

# Usage in view
def my_view(request):
    if request.is_mobile:
        return render(request, "mobile/page.html")
    return render(request, "desktop/page.html")
```

### 2. Rate Limiting Middleware

```python
# middleware.py
from django.http import JsonResponse
from django.core.cache import cache
import time

class RateLimitMiddleware:
    """Simple rate limiting by IP."""
    
    def __init__(self, get_response):
        self.get_response = get_response
        self.requests_per_minute = 60
    
    def __call__(self, request):
        # Get client IP
        ip = self.get_client_ip(request)
        
        # Check rate limit
        cache_key = f"rate_limit:{ip}"
        requests = cache.get(cache_key, [])
        
        # Clean old requests (older than 1 minute)
        now = time.time()
        requests = [r for r in requests if now - r < 60]
        
        if len(requests) >= self.requests_per_minute:
            return JsonResponse(
                {"error": "Rate limit exceeded"},
                status=429
            )
        
        # Add current request
        requests.append(now)
        cache.set(cache_key, requests, timeout=60)
        
        response = self.get_response(request)
        return response
    
    def get_client_ip(self, request):
        x_forwarded_for = request.META.get("HTTP_X_FORWARDED_FOR")
        if x_forwarded_for:
            return x_forwarded_for.split(",")[0].strip()
        return request.META.get("REMOTE_ADDR")
```

### 3. Response Caching Middleware

```python
# middleware.py
from django.core.cache import cache

class CacheControlMiddleware:
    """Add cache headers to responses."""
    
    def __init__(self, get_response):
        self.get_response = get_response
    
    def __call__(self, request):
        response = self.get_response(request)
        
        # Add cache headers for safe methods
        if request.method in ("GET", "HEAD"):
            # Cache for 5 minutes in browser, 10 minutes in CDN
            response["Cache-Control"] = "public, max-age=300, s-maxage=600"
        
        return response
```

### 4. API Version Middleware

```python
# middleware.py
class APIVersionMiddleware:
    """Extract API version from request."""
    
    def __init__(self, get_response):
        self.get_response = get_response
    
    def __call__(self, request):
        # Try header first
        api_version = request.META.get("HTTP_API_VERSION", "v1")
        
        # Then query param
        api_version = request.GET.get("version", api_version)
        
        request.api_version = api_version
        
        response = self.get_response(request)
        response["API-Version"] = api_version
        
        return response

# Usage
def api_view(request):
    version = request.api_version  # "v1" or "v2"
    if version == "v2":
        return v2_response()
    return v1_response()
```

## Middleware Settings

```python
# settings.py

# Add custom middleware
MIDDLEWARE = [
    'django.middleware.security.SecurityMiddleware',
    'django.contrib.sessions.middleware.SessionMiddleware',
    'django.middleware.common.CommonMiddleware',
    'django.middleware.csrf.CsrfViewMiddleware',
    'django.contrib.auth.middleware.AuthenticationMiddleware',
    
    # Your custom middleware
    'myapp.middleware.RequestLoggingMiddleware',
    'myapp.middleware.RateLimitMiddleware',
]
```

## Middleware with Exceptions

```python
# middleware.py
class ErrorLoggingMiddleware:
    """Log exceptions from views."""
    
    def __init__(self, get_response):
        self.get_response = get_response
    
    def __call__(self, request):
        return self.get_response(request)
    
    def process_exception(self, request, exception):
        """Handle exceptions from views."""
        import logging
        logger = logging.getLogger(__name__)
        
        logger.error(
            f"Exception in {request.path}: {exception}",
            exc_info=True
        )
        
        # Can return None to let other handlers process
        # Or return HttpResponse to short-circuit
        return None
```

## Process Methods Reference

| Method | When Called |
|--------|-------------|
| `process_request` | Before routing (request phase) |
| `process_view` | After routing, before view |
| `process_template_response` | After view returns, before rendering |
| `process_response` | After view, before sending response |
| `process_exception` | When view raises exception |

```python
class CompleteMiddleware:
    def process_request(self, request):
        """Called before URL routing."""
        # Return None to continue, or HttpResponse to stop
        return None
    
    def process_view(self, request, view_func, view_args, view_kwargs):
        """Called after routing, before view."""
        # Return None to continue, or Response to short-circuit
        return None
    
    def process_template_response(self, request, response):
        """Called after view returns TemplateResponse."""
        # Modify response before rendering
        return response
    
    def process_response(self, request, response):
        """Called after view, before sending."""
        # Modify response
        return response
    
    def process_exception(self, request, exception):
        """Called when view raises exception."""
        # Return None to continue, or Response to handle
        return None
```

## Production Considerations

- **Order matters**: Top middleware processes first on request, last on response
- **Performance**: Middleware runs on every request; keep it lightweight
- **Short-circuiting**: Returning early skips remaining middleware
- **Exceptions**: Use `process_exception` to handle view errors

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Wrong middleware order

**Wrong:**
```python
MIDDLEWARE = [
    'myapp.middleware.AuthMiddleware',  # Needs session first!
    'django.contrib.sessions.middleware.SessionMiddleware',
]
```

**Why it fails:** Authentication middleware needs session to be loaded.

**Fix:**
```python
MIDDLEWARE = [
    'django.contrib.sessions.middleware.SessionMiddleware',  # First!
    'myapp.middleware.AuthMiddleware',
]
```

### ❌ Mistake 2: Heavy computation in middleware

**Wrong:**
```python
class HeavyMiddleware:
    def __call__(self, request):
        # Query database!
        user = User.objects.get(id=request.user.id)
        # Expensive operation!
```

**Why it fails:** Runs on every request, impacts performance.

**Fix:**
```python
class LightMiddleware:
    def __call__(self, request):
        # Light operations only
        # Defer heavy work to view
        return self.get_response(request)
```

### ❌ Mistake 3: Forgetting to return response

**Wrong:**
```python
def process_request(self, request):
    # Forgot to return!
    if not request.user.is_authenticated:
        # Should return HttpResponse!
        pass
```

**Why it fails:** Middleware returns None, but you wanted to block!

**Fix:**
```python
def process_request(self, request):
    if not request.user.is_authenticated:
        return HttpResponse("Unauthorized", status=401)
    return None
```

## Summary

- Middleware processes requests before views and responses after views
- Built-in middleware handles authentication, sessions, CSRF, security
- Create custom middleware by defining `__call__` method
- Middleware order in settings determines execution order
- Keep middleware lightweight for performance

## Next Steps

This completes the Advanced Django Patterns folder. Continue to `35-auth-and-authorization-advanced/01-oauth2-deep-dive.md` to learn about OAuth2 implementation.
