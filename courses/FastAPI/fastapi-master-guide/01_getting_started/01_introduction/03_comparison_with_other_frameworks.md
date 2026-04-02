# Comparison with Other Frameworks

## Overview

Understanding how FastAPI compares to other Python web frameworks helps you make informed decisions for your projects. This guide compares FastAPI with popular alternatives across key dimensions.

## Framework Comparison Matrix

| Feature | FastAPI | Flask | Django | Express (Node) |
|---------|---------|-------|--------|----------------|
| Performance | Very High | Moderate | Moderate | High |
| Learning Curve | Low-Medium | Low | Medium-High | Low |
| Async Support | Native | Limited | Limited | Native |
| Auto Documentation | Built-in | Manual | Manual | Manual |
| Data Validation | Built-in (Pydantic) | Manual | DRF Serializer | Manual |
| Type Hints | Full Support | Optional | Optional | N/A (TypeScript) |
| ORM | Optional | Optional | Built-in (Django ORM) | Optional |
| Admin Panel | No | No | Built-in | No |

## FastAPI vs Flask

### Flask Overview
Flask is a lightweight WSGI micro-framework known for its simplicity and flexibility.

### Code Comparison

#### Flask Example
```python
# Flask implementation of a simple API with validation
from flask import Flask, request, jsonify
from werkzeug.exceptions import BadRequest

app = Flask(__name__)

# Manual validation function - Flask doesn't provide built-in validation
def validate_item(data):
    """Custom validation that FastAPI handles automatically"""
    errors = {}
    if not data.get('name'):
        errors['name'] = 'Name is required'
    if not data.get('price'):
        errors['price'] = 'Price is required'
    elif not isinstance(data.get('price'), (int, float)):
        errors['price'] = 'Price must be a number'
    elif data.get('price') <= 0:
        errors['price'] = 'Price must be positive'
    return errors

@app.route('/items', methods=['POST'])
def create_item():
    """
    Flask endpoint requiring manual validation.
    Note: No automatic documentation, no type checking.
    """
    data = request.get_json()

    # Manual validation
    errors = validate_item(data)
    if errors:
        return jsonify({'errors': errors}), 400

    # Manual type casting
    item = {
        'name': data['name'],
        'price': float(data['price']),
        'description': data.get('description', '')
    }

    return jsonify(item), 201

@app.route('/items/<int:item_id>', methods=['GET'])
def get_item(item_id):
    """Flask route with path parameter"""
    # item_id is automatically converted to int by Flask
    return jsonify({'id': item_id, 'name': 'Sample Item'})

# No built-in documentation
# Must use extensions like flask-restx or flasgger for docs
```

#### FastAPI Equivalent
```python
# FastAPI implementation - same functionality, much cleaner
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, Field
from typing import Optional

app = FastAPI()

# Automatic validation with Pydantic
class Item(BaseModel):
    name: str = Field(..., min_length=1)
    price: float = Field(..., gt=0)
    description: Optional[str] = None

@app.post("/items/", response_model=Item, status_code=201)
async def create_item(item: Item):
    """
    FastAPI endpoint with automatic validation.
    - Auto-generates OpenAPI documentation
    - Validates request body automatically
    - Provides clear error messages
    - Editor autocompletion for item fields
    """
    # No manual validation needed - Pydantic handles it
    # If validation fails, FastAPI returns 422 with detailed errors
    return item

@app.get("/items/{item_id}")
async def get_item(item_id: int):
    """
    FastAPI route - type hint provides:
    - Automatic conversion
    - Validation (must be integer)
    - Documentation
    """
    return {"id": item_id, "name": "Sample Item"}
```

### When to Choose Flask vs FastAPI

**Choose Flask when:**
- Building simple web applications with templates
- Need maximum ecosystem flexibility
- Working with legacy Flask codebases
- Prefer minimal structure

**Choose FastAPI when:**
- Building APIs (especially REST APIs)
- Need automatic documentation
- Want type safety and validation
- Require async/await support
- Building microservices

```python
# Example: When Flask might be preferable - Template-based app
from flask import Flask, render_template

app = Flask(__name__)

@app.route('/')
def home():
    """Flask excels at serving HTML templates"""
    return render_template('home.html', title='Welcome')

# FastAPI can serve templates too, but Flask has more mature tooling
```

## FastAPI vs Django

### Django Overview
Django is a full-featured "batteries-included" web framework with an ORM, admin panel, and extensive built-in features.

### Code Comparison

#### Django + Django REST Framework
```python
# Django REST Framework implementation
# Requires: models.py, serializers.py, views.py, urls.py

# models.py
from django.db import models

class Item(models.Model):
    """Django ORM model - database table definition"""
    name = models.CharField(max_length=100)
    price = models.DecimalField(max_digits=10, decimal_places=2)
    description = models.TextField(blank=True, null=True)
    created_at = models.DateTimeField(auto_now_add=True)

    class Meta:
        db_table = 'items'

# serializers.py
from rest_framework import serializers
from .models import Item

class ItemSerializer(serializers.ModelSerializer):
    """Django REST Framework serializer for validation"""
    class Meta:
        model = Item
        fields = ['id', 'name', 'price', 'description', 'created_at']
        read_only_fields = ['id', 'created_at']

# views.py
from rest_framework import viewsets, status
from rest_framework.response import Response
from .models import Item
from .serializers import ItemSerializer

class ItemViewSet(viewsets.ModelViewSet):
    """
    Django REST Framework viewset.
    Provides CRUD operations with less code.
    """
    queryset = Item.objects.all()
    serializer_class = ItemSerializer

    def create(self, request, *args, **kwargs):
        serializer = self.get_serializer(data=request.data)
        serializer.is_valid(raise_exception=True)
        self.perform_create(serializer)
        return Response(serializer.data, status=status.HTTP_201_CREATED)

# urls.py
from django.urls import path, include
from rest_framework.routers import DefaultRouter
from .views import ItemViewSet

router = DefaultRouter()
router.register(r'items', ItemViewSet)

urlpatterns = [
    path('api/', include(router.urls)),
]
```

#### FastAPI Equivalent
```python
# FastAPI implementation - single file for equivalent functionality
from fastapi import FastAPI, HTTPException, Depends
from pydantic import BaseModel, Field
from typing import Optional
from datetime import datetime
from sqlalchemy import create_engine, Column, Integer, String, Float, DateTime
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker, Session

app = FastAPI()

# Database setup (using SQLAlchemy instead of Django ORM)
SQLALCHEMY_DATABASE_URL = "sqlite:///./test.db"
engine = create_engine(SQLALCHEMY_DATABASE_URL)
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)
Base = declarative_base()

# SQLAlchemy model
class ItemDB(Base):
    __tablename__ = "items"
    id = Column(Integer, primary_key=True, index=True)
    name = Column(String(100))
    price = Column(Float)
    description = Column(String, nullable=True)
    created_at = Column(DateTime, default=datetime.utcnow)

Base.metadata.create_all(bind=engine)

# Pydantic schemas
class ItemCreate(BaseModel):
    name: str = Field(..., min_length=1, max_length=100)
    price: float = Field(..., gt=0)
    description: Optional[str] = None

class ItemResponse(ItemCreate):
    id: int
    created_at: datetime

    class Config:
        from_attributes = True

# Dependency
def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

# Routes
@app.post("/items/", response_model=ItemResponse, status_code=201)
async def create_item(item: ItemCreate, db: Session = Depends(get_db)):
    """
    Create item with FastAPI.
    - Less boilerplate than Django REST Framework
    - Same functionality in fewer files
    - Built-in documentation
    """
    db_item = ItemDB(**item.model_dump())
    db.add(db_item)
    db.commit()
    db.refresh(db_item)
    return db_item

@app.get("/items/{item_id}", response_model=ItemResponse)
async def read_item(item_id: int, db: Session = Depends(get_db)):
    """Get item by ID with automatic validation"""
    item = db.query(ItemDB).filter(ItemDB.id == item_id).first()
    if not item:
        raise HTTPException(status_code=404, detail="Item not found")
    return item
```

### When to Choose Django vs FastAPI

**Choose Django when:**
- Building full-stack web applications
- Need an admin panel out of the box
- Require built-in authentication, ORM, and templating
- Working on content-heavy websites
- Team has Django experience

**Choose FastAPI when:**
- Building API-first applications
- Need high performance
- Require async support
- Want modern Python features (type hints)
- Building microservices

## FastAPI vs Express.js (Node.js)

### Performance Comparison

```python
# FastAPI - High performance async Python
from fastapi import FastAPI
import asyncio

app = FastAPI()

@app.get("/benchmark")
async def benchmark():
    """
    FastAPI uses uvloop for performance.
    Performance is comparable to Node.js/Express.
    Benchmarks show similar throughput for I/O operations.
    """
    # Non-blocking I/O
    await asyncio.sleep(0.001)
    return {"framework": "fastapi", "language": "python"}
```

```javascript
// Express.js - High performance async JavaScript
const express = require('express');
const app = express();

app.get('/benchmark', async (req, res) => {
    // Non-blocking I/O
    await new Promise(resolve => setTimeout(resolve, 1));
    res.json({ framework: 'express', language: 'javascript' });
});
```

### Key Differences

| Aspect | FastAPI | Express.js |
|--------|---------|------------|
| Language | Python | JavaScript |
| Type System | Type hints (Python) | TypeScript (optional) |
| Validation | Pydantic (built-in) | Joi/Yup (external) |
| Documentation | Auto-generated | Swagger-jsdoc (manual) |
| Learning Curve | Python knowledge | JavaScript knowledge |

## FastAPI vs Other Python Async Frameworks

### FastAPI vs Sanic

```python
# Sanic - Another async Python framework
from sanic import Sanic
from sanic.response import json

app = Sanic("MyApp")

@app.route("/items/<item_id:int>")
async def get_item(request, item_id):
    """Sanic route - no automatic validation or documentation"""
    return json({"id": item_id})

# FastAPI provides more features with similar performance
# - Automatic validation
# - Auto-generated docs
# - Type hints support
```

### FastAPI vs Starlette

```python
# Starlette - The foundation of FastAPI
from starlette.applications import Starlette
from starlette.responses import JSONResponse
from starlette.routing import Route

async def homepage(request):
    """Starlette endpoint - FastAPI adds validation layer on top"""
    return JSONResponse({"hello": "world"})

app = Starlette(routes=[
    Route('/', homepage),
])

# FastAPI = Starlette + Pydantic + Automatic Documentation
# If you need just the web layer without validation, use Starlette
```

## Decision Matrix

### Use FastAPI When:
1. **Building REST/GraphQL APIs** - Primary use case
2. **Need Auto Documentation** - Saves significant time
3. **Type Safety is Important** - Reduces runtime errors
4. **Async Operations** - High-concurrency requirements
5. **Modern Python** - Want to use latest Python features

### Use Django When:
1. **Full-Stack Applications** - Need templates, ORM, admin
2. **Rapid Prototyping** - Built-in everything
3. **Content Management** - Admin panel for content
4. **Team Experience** - Team knows Django

### Use Flask When:
1. **Simple Applications** - Minimal requirements
2. **Maximum Flexibility** - Choose your own components
3. **Legacy Integration** - Existing Flask codebase
4. **Learning Web Development** - Simpler mental model

## Migration Considerations

### Migrating from Flask to FastAPI

```python
# Flask code
from flask import Flask, jsonify

app = Flask(__name__)

@app.route('/items/<int:item_id>')
def get_item(item_id):
    return jsonify({'id': item_id})

# FastAPI equivalent - minimal changes needed
from fastapi import FastAPI

app = FastAPI()

@app.get('/items/{item_id}')
async def get_item(item_id: int):
    return {'id': item_id}

# Key changes:
# 1. @app.route becomes @app.get, @app.post, etc.
# 2. Type hints added for parameters
# 3. jsonify() not needed - auto-converts dicts
# 4. Added async keyword (optional but recommended)
```

### Migrating from Django REST Framework

```python
# Django REST Framework view
from rest_framework.views import APIView
from rest_framework.response import Response

class ItemView(APIView):
    def get(self, request, item_id):
        item = Item.objects.get(id=item_id)
        serializer = ItemSerializer(item)
        return Response(serializer.data)

# FastAPI equivalent
from fastapi import FastAPI, Depends

@app.get("/items/{item_id}")
async def get_item(item_id: int, db: Session = Depends(get_db)):
    item = db.query(Item).filter(Item.id == item_id).first()
    return item

# Key changes:
# 1. Class-based views become function-based
# 2. Serializers become Pydantic models
# 3. Request object not passed explicitly
# 4. Dependency injection for database
```

## Summary

| Framework | Best For | Avoid When |
|-----------|----------|------------|
| FastAPI | APIs, microservices, async apps | Full-stack with templates |
| Django | Full-stack, admin panels, CMS | Simple APIs, high-performance needs |
| Flask | Simple apps, learning, flexibility | Complex APIs, type safety needed |
| Express | JavaScript teams, real-time apps | Python expertise available |

## Next Steps

Now that you understand FastAPI's position in the ecosystem:
- [Prerequisites](../02_setup_and_installation/01_prerequisites.md) - What you need before starting
- [Installation Guide](../02_setup_and_installation/02_installation_guide.md) - Set up your environment
