# Django REST Framework

## What You'll Learn
- REST API fundamentals
- Serializers and validation
- ViewSets and Routers
- Authentication and permissions
- Pagination and filtering

## Prerequisites
- Completed `07-django-middleware.md` — Django middleware patterns
- Understanding of Django models and views

## What Is Django REST Framework?

Django REST Framework (DRF) builds REST APIs on top of Django:

```
Traditional Django:     Request → View → Template → HTML
DRF:                    Request → Serializer → JSON
```

## Installation

```bash
pip install djangorestframework

# settings.py
INSTALLED_APPS = [
    ...
    'rest_framework',
]

REST_FRAMEWORK = {
    'DEFAULT_PAGINATION_CLASS': 'rest_framework.pagination.PageNumberPagination',
    'PAGE_SIZE': 20,
}
```

## Serializers

```python
# serializers.py
from rest_framework import serializers
from .models import Article, Author

class AuthorSerializer(serializers.ModelSerializer):
    """Serializer for Author model."""
    
    class Meta:
        model = Author
        fields = ["id", "name", "email", "bio"]

class ArticleSerializer(serializers.ModelSerializer):
    """Serializer for Article model."""
    
    # Nested serializer - shows author details
    author = AuthorSerializer(read_only=True)
    
    # Write-only for creating
    author_id = serializers.PrimaryKeyRelatedField(
        queryset=Author.objects.all(),
        source="author",
        write_only=True
    )
    
    class Meta:
        model = Article
        fields = ["id", "title", "slug", "content", "author", "author_id", 
                  "status", "created_at", "updated_at"]
        read_only_fields = ["id", "created_at", "updated_at"]
```

🔍 **Line-by-Line Breakdown:**
1. `serializers.ModelSerializer` — Auto-generates serializer from Django model
2. `fields` — Which model fields to include
3. `read_only` — Fields that can't be modified by clients
4. `PrimaryKeyRelatedField` — Foreign key as ID instead of nested object

### Custom Validation

```python
class ArticleSerializer(serializers.ModelSerializer):
    
    class Meta:
        model = Article
        fields = ["title", "slug", "content", "status"]
    
    def validate_title(self, value):
        """Field-level validation."""
        if len(value) < 5:
            raise serializers.ValidationError("Title too short")
        return value
    
    def validate(self, data):
        """Object-level validation."""
        if data.get("status") == "published" and not data.get("content"):
            raise serializers.ValidationError(
                "Cannot publish article without content"
            )
        return data
```

## API Views

### Function-Based Views

```python
# views.py
from rest_framework.decorators import api_view
from rest_framework.response import Response
from rest_framework import status
from .models import Article
from .serializers import ArticleSerializer

@api_view(["GET", "POST"])
def article_list(request):
    """List all articles or create new article."""
    
    if request.method == "GET":
        articles = Article.objects.all()
        serializer = ArticleSerializer(articles, many=True)
        return Response(serializer.data)
    
    elif request.method == "POST":
        serializer = ArticleSerializer(data=request.data)
        if serializer.is_valid():
            serializer.save()
            return Response(serializer.data, status=status.HTTP_201_CREATED)
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

@api_view(["GET", "PUT", "DELETE"])
def article_detail(request, pk):
    """Get, update, or delete article."""
    
    try:
        article = Article.objects.get(pk=pk)
    except Article.DoesNotExist:
        return Response(status=status.HTTP_404_NOT_FOUND)
    
    if request.method == "GET":
        serializer = ArticleSerializer(article)
        return Response(serializer.data)
    
    elif request.method == "PUT":
        serializer = ArticleSerializer(article, data=request.data)
        if serializer.is_valid():
            serializer.save()
            return Response(serializer.data)
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)
    
    elif request.method == "DELETE":
        article.delete()
        return Response(status=status.HTTP_204_NO_CONTENT)
```

### Class-Based Views (APIView)

```python
# views.py
from rest_framework.views import APIView
from rest_framework.response import Response
from rest_framework import status
from .models import Article
from .serializers import ArticleSerializer

class ArticleList(APIView):
    """List all articles or create new one."""
    
    def get(self, request):
        articles = Article.objects.all()
        serializer = ArticleSerializer(articles, many=True)
        return Response(serializer.data)
    
    def post(self, request):
        serializer = ArticleSerializer(data=request.data)
        if serializer.is_valid():
            serializer.save()
            return Response(serializer.data, status=status.HTTP_201_CREATED)
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

class ArticleDetail(APIView):
    """Get, update, or delete article."""
    
    def get_object(self, pk):
        try:
            return Article.objects.get(pk=pk)
        except Article.DoesNotExist:
            return None
    
    def get(self, request, pk):
        article = self.get_object(pk)
        if not article:
            return Response(status=status.HTTP_404_NOT_FOUND)
        serializer = ArticleSerializer(article)
        return Response(serializer.data)
    
    def put(self, request, pk):
        article = self.get_object(pk)
        if not article:
            return Response(status=status.HTTP_404_NOT_FOUND)
        serializer = ArticleSerializer(article, data=request.data)
        if serializer.is_valid():
            serializer.save()
            return Response(serializer.data)
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)
    
    def delete(self, request, pk):
        article = self.get_object(pk)
        if not article:
            return Response(status=status.HTTP_404_NOT_FOUND)
        article.delete()
        return Response(status=status.HTTP_204_NO_CONTENT)
```

## ViewSets and Routers

### ViewSet

```python
# views.py
from rest_framework import viewsets
from .models import Article
from .serializers import ArticleSerializer

class ArticleViewSet(viewsets.ModelViewSet):
    """ViewSet for Article: list, create, retrieve, update, partial_update, destroy."""
    
    queryset = Article.objects.all()
    serializer_class = ArticleSerializer
    
    def get_queryset(self):
        """Filter by status for GET requests."""
        if self.request.method == "GET":
            status = self.request.query_params.get("status")
            if status:
                return Article.objects.filter(status=status)
        return Article.objects.all()
    
    def perform_create(self, serializer):
        """Set author to current user."""
        serializer.save(author=self.request.user)
```

### Router

```python
# urls.py
from django.urls import path, include
from rest_framework.routers import DefaultRouter
from .views import ArticleViewSet

router = DefaultRouter()
router.register(r"articles", ArticleViewSet, basename="article")

urlpatterns = [
    path("", include(router.urls)),
]
```

Router generates these URLs:
- `GET /articles/` — list
- `POST /articles/` — create
- `GET /articles/{pk}/` — retrieve
- `PUT /articles/{pk}/` — update
- `DELETE /articles/{pk}/` — destroy

## Authentication

```python
# settings.py
REST_FRAMEWORK = {
    'DEFAULT_AUTHENTICATION_CLASSES': [
        'rest_framework.authentication.SessionAuthentication',
        'rest_framework.authentication.TokenAuthentication',  # Simple token
        'rest_framework.authentication.BasicAuthentication',
    ],
    'DEFAULT_PERMISSION_CLASSES': [
        'rest_framework.permissions.IsAuthenticated',
    ],
}
```

### Token Authentication

```bash
pip install django-rest-knox  # For better token handling
```

```python
# views.py
from rest_framework.permissions import IsAuthenticated

class ProtectedView(APIView):
    permission_classes = [IsAuthenticated]
    
    def get(self, request):
        return Response({
            "user": request.user.username,
            "message": "You are authenticated!"
        })
```

### Custom Permissions

```python
# permissions.py
from rest_framework import permissions

class IsAuthorOrReadOnly(permissions.BasePermission):
    """Allow read to all, write only to author."""
    
    def has_object_permission(self, request, view, obj):
        # Read allowed for everyone
        if request.method in permissions.SAFE_METHODS:
            return True
        
        # Write allowed only for author
        return obj.author == request.user

# Usage
class ArticleViewSet(viewsets.ModelViewSet):
    permission_classes = [IsAuthorOrReadOnly]
    ...
```

## Filtering

```python
# views.py
from rest_framework import filters

class ArticleViewSet(viewsets.ModelViewSet):
    queryset = Article.objects.all()
    serializer_class = ArticleSerializer
    
    # Add filtering
    filter_backends = [
        filters.SearchFilter,
        filters.OrderingFilter,
        filters.DjangoFilterBackend,
    ]
    
    # Search
    search_fields = ["title", "content", "author__name"]
    
    # Ordering
    ordering_fields = ["created_at", "title"]
    ordering = ["-created_at"]  # Default
    
    # Django Filter
    filterset_fields = ["status", "author"]
```

## Pagination

```python
# settings.py
REST_FRAMEWORK = {
    'DEFAULT_PAGINATION_CLASS': 'rest_framework.pagination.PageNumberPagination',
    'PAGE_SIZE': 10,
}

# Custom pagination
# pagination.py
from rest_framework.pagination import CursorPagination

class ArticlePagination(CursorPagination):
    page_size = 10
    ordering = "-created_at"

# views.py
class ArticleViewSet(viewsets.ModelViewSet):
    pagination_class = ArticlePagination
    ...
```

## Production Considerations

- **API versioning**: Include version in URL or header
- **Throttling**: Prevent abuse with rate limiting
- **CORS**: Install django-cors-headers for frontend access
- **Documentation**: Use DRF's built-in API explorer

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Not handling permissions

**Wrong:**
```python
class ArticleViewSet(viewsets.ModelViewSet):
    # No permission_classes!
    queryset = Article.objects.all()
```

**Why it fails:** Anyone can access/change data.

**Fix:**
```python
class ArticleViewSet(viewsets.ModelViewSet):
    permission_classes = [IsAuthenticated, IsAuthorOrReadOnly]
    queryset = Article.objects.all()
```

### ❌ Mistake 2: N+1 queries in serializers

**Wrong:**
```python
# Serializer accesses related objects
class ArticleSerializer(serializers.ModelSerializer):
    author_name = serializers.CharField(source="author.name")
    
# View
Article.objects.all()  # N+1 queries!
```

**Why it fails:** Each article triggers query for author.

**Fix:**
```python
# views.py
class ArticleViewSet(viewsets.ModelViewSet):
    def get_queryset(self):
        return Article.objects.select_related("author")
```

### ❌ Mistake 3: Not validating input

**Wrong:**
```python
def create(self, request):
    # No validation!
    Article.objects.create(**request.data)
```

**Why it fails:** Accepts any data, can corrupt database.

**Fix:**
```python
def create(self, request):
    serializer = ArticleSerializer(data=request.data)
    if serializer.is_valid():
        serializer.save()
    # Proper validation!
```

## Summary

- DRF serializers convert Django models to JSON and validate input
- ViewSets provide CRUD operations with minimal code
- Routers auto-generate URL patterns
- Use authentication and permissions to secure APIs
- Add filtering and pagination for better API UX

## Next Steps

This completes the Advanced Django Patterns folder. Continue to `35-auth-and-authorization-advanced/01-oauth2-deep-dive.md` to explore OAuth2 implementation in depth.
