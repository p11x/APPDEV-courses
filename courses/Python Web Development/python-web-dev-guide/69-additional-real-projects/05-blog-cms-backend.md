# Blog CMS Backend

## What You'll Learn

- Building a content management system
- Article management with rich content
- Categories and tags
- Draft and publish workflow

## Prerequisites

- Completed `04-saas-subscription-backend.md`

## Introduction

This project covers building a blog/content management system backend with articles, categories, and publishing workflow.

## Models

```python
from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime
import uuid


class Category(BaseModel):
    id: str = Field(default_factory=lambda: str(uuid.uuid4()))
    name: str
    slug: str
    description: str = ""
    created_at: datetime = Field(default_factory=datetime.now)


class Tag(BaseModel):
    id: str = Field(default_factory=lambda: str(uuid.uuid4()))
    name: str
    slug: str


class Article(BaseModel):
    id: str = Field(default_factory=lambda: str(uuid.uuid4()))
    title: str
    slug: str
    content: str
    excerpt: str = ""
    featured_image: Optional[str] = None
    author_id: str
    category_id: Optional[str] = None
    tags: List[str] = []
    status: str = "draft"  # draft, published, archived
    published_at: Optional[datetime] = None
    views: int = 0
    created_at: datetime = Field(default_factory=datetime.now)
    updated_at: datetime = Field(default_factory=datetime.now)


class Author(BaseModel):
    id: str = Field(default_factory=lambda: str(uuid.uuid4()))
    name: str
    bio: str = ""
    avatar: Optional[str] = None
    created_at: datetime = Field(default_factory=datetime.now)
```

## Main Application

```python
from fastapi import FastAPI, HTTPException
from typing import List


app = FastAPI(title="Blog CMS API")


# Storage
authors_db: dict[str, Author] = {}
categories_db: dict[str, Category] = {}
tags_db: dict[str, Tag] = {}
articles_db: dict[str, Article] = {}


# ============ Authors ============

@app.post("/api/authors", response_model=Author)
async def create_author(name: str, bio: str = "") -> Author:
    """Create a new author."""
    author = Author(name=name, bio=bio)
    authors_db[author.id] = author
    return author


@app.get("/api/authors", response_model=List[Author])
async def list_authors() -> List[Author]:
    return list(authors_db.values())


@app.get("/api/authors/{author_id}", response_model=Author)
async def get_author(author_id: str) -> Author:
    if author_id not in authors_db:
        raise HTTPException(status_code=404, detail="Author not found")
    return authors_db[author_id]


# ============ Categories ============

@app.post("/api/categories", response_model=Category)
async def create_category(name: str, description: str = "") -> Category:
    """Create a new category."""
    slug = name.lower().replace(" ", "-")
    
    category = Category(
        name=name,
        slug=slug,
        description=description,
    )
    categories_db[category.id] = category
    return category


@app.get("/api/categories", response_model=List[Category])
async def list_categories() -> List[Category]:
    return list(categories_db.values())


@app.get("/api/categories/{category_id}/articles")
async def get_category_articles(category_id: str) -> List[Article]:
    """Get articles in a category."""
    return [
        a for a in articles_db.values()
        if a.category_id == category_id
    ]


# ============ Tags ============

@app.post("/api/tags", response_model=Tag)
async def create_tag(name: str) -> Tag:
    """Create a new tag."""
    slug = name.lower().replace(" ", "-")
    tag = Tag(name=name, slug=slug)
    tags_db[tag.id] = tag
    return tag


@app.get("/api/tags", response_model=List[Tag])
async def list_tags() -> List[Tag]:
    return list(tags_db.values())


# ============ Articles ============

@app.post("/api/articles", response_model=Article)
async def create_article(
    title: str,
    content: str,
    author_id: str,
    category_id: Optional[str] = None,
    tags: List[str] = [],
    excerpt: str = "",
    featured_image: Optional[str] = None,
) -> Article:
    """Create a new article."""
    
    if author_id not in authors_db:
        raise HTTPException(status_code=404, detail="Author not found")
    
    slug = title.lower().replace(" ", "-")
    
    article = Article(
        title=title,
        slug=slug,
        content=content,
        author_id=author_id,
        category_id=category_id,
        tags=tags,
        excerpt=excerpt,
        featured_image=featured_image,
    )
    
    articles_db[article.id] = article
    return article


@app.get("/api/articles", response_model=List[Article])
async def list_articles(
    status: str = "published",
    limit: int = 20,
    offset: int = 0,
) -> List[Article]:
    """List articles with filters."""
    
    articles = [
        a for a in articles_db.values()
        if a.status == status
    ]
    
    articles.sort(key=lambda a: a.published_at or a.created_at, reverse=True)
    
    return articles[offset:offset + limit]


@app.get("/api/articles/{article_id}", response_model=Article)
async def get_article(article_id: str) -> Article:
    """Get article by ID."""
    if article_id not in articles_db:
        raise HTTPException(status_code=404, detail="Article not found")
    
    article = articles_db[article_id]
    article.views += 1
    
    return article


@app.get("/api/articles/slug/{slug}", response_model=Article)
async def get_article_by_slug(slug: str) -> Article:
    """Get article by slug."""
    for article in articles_db.values():
        if article.slug == slug:
            article.views += 1
            return article
    
    raise HTTPException(status_code=404, detail="Article not found")


@app.put("/api/articles/{article_id}", response_model=Article)
async def update_article(
    article_id: str,
    title: Optional[str] = None,
    content: Optional[str] = None,
    status: Optional[str] = None,
) -> Article:
    """Update an article."""
    
    if article_id not in articles_db:
        raise HTTPException(status_code=404, detail="Article not found")
    
    article = articles_db[article_id]
    
    if title:
        article.title = title
        article.slug = title.lower().replace(" ", "-")
    
    if content:
        article.content = content
    
    if status:
        article.status = status
        if status == "published" and not article.published_at:
            article.published_at = datetime.now()
    
    article.updated_at = datetime.now()
    
    return article


@app.delete("/api/articles/{article_id}")
async def delete_article(article_id: str) -> dict:
    """Delete an article."""
    if article_id not in articles_db:
        raise HTTPException(status_code=404, detail="Article not found")
    
    del articles_db[article_id]
    return {"success": True}


# ============ Search ============

@app.get("/api/search")
async def search_articles(q: str, limit: int = 10) -> List[Article]:
    """Search articles."""
    
    q_lower = q.lower()
    results = [
        a for a in articles_db.values()
        if a.status == "published" and (
            q_lower in a.title.lower() or
            q_lower in a.content.lower() or
            q_lower in a.excerpt.lower()
        )
    ]
    
    return results[:limit]


from datetime import datetime
```

## Summary

This blog CMS backend includes:
- Author management
- Categories and tags
- Article CRUD operations
- Publishing workflow (draft, published, archived)
- Views tracking
- Search functionality

This concludes the Python Web Development Guide with comprehensive coverage of all major topics from basics to advanced real-world projects.
