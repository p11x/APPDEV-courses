# Django REST Framework

## What You'll Learn
- Building APIs with Django REST Framework
- Serializers
- Viewsets and routers

## Prerequisites
- Completed `07-django-admin.md`

## Installing Django REST Framework

```bash
pip install djangorestframework
```

## Setup

Add to INSTALLED_APPS:

```python
INSTALLED_APPS = [
    ...
    'rest_framework',
]
```

## Serializers

```python
# blog/serializers.py
from rest_framework import serializers
from .models import Post

class PostSerializer(serializers.ModelSerializer):
    class Meta:
        model = Post
        fields = ['id', 'title', 'slug', 'content', 'author', 'published', 'created_at']
        read_only_fields = ['id', 'created_at']
```

## API Views

```python
# blog/views.py
from rest_framework import viewsets
from rest_framework.permissions import IsAuthenticated
from .models import Post
from .serializers import PostSerializer

class PostViewSet(viewsets.ModelViewSet):
    queryset = Post.objects.all()
    serializer_class = PostSerializer
    permission_classes = [IsAuthenticated]
```

## URLs

```python
# blog/urls.py
from rest_framework.routers import DefaultRouter
from .views import PostViewSet

router = DefaultRouter()
router.register(r'posts', PostViewSet)

urlpatterns = router.urls
```

## Summary
- DRF provides REST API functionality
- Serializers convert models to JSON
- ViewSets provide CRUD operations
- Routers auto-generate URLs
