# Relationship Types

## Overview

SQLAlchemy supports various relationship types for modeling database associations.

## Relationship Patterns

### One-to-One

```python
# Example 1: One-to-one relationship
from sqlalchemy import Column, Integer, String, ForeignKey
from sqlalchemy.orm import relationship

class User(Base):
    __tablename__ = "users"

    id = Column(Integer, primary_key=True)
    username = Column(String(50), unique=True)

    # One-to-one relationship
    profile = relationship("Profile", back_populates="user", uselist=False)

class Profile(Base):
    __tablename__ = "profiles"

    id = Column(Integer, primary_key=True)
    user_id = Column(Integer, ForeignKey("users.id"), unique=True)
    bio = Column(String(500))

    user = relationship("User", back_populates="profile")
```

### One-to-Many

```python
# Example 2: One-to-many relationship
class User(Base):
    __tablename__ = "users"

    id = Column(Integer, primary_key=True)
    username = Column(String(50))

    posts = relationship("Post", back_populates="author")

class Post(Base):
    __tablename__ = "posts"

    id = Column(Integer, primary_key=True)
    title = Column(String(200))
    author_id = Column(Integer, ForeignKey("users.id"))

    author = relationship("User", back_populates="posts")
```

### Many-to-Many

```python
# Example 3: Many-to-many relationship
from sqlalchemy import Table

# Association table
post_tags = Table(
    "post_tags",
    Base.metadata,
    Column("post_id", Integer, ForeignKey("posts.id"), primary_key=True),
    Column("tag_id", Integer, ForeignKey("tags.id"), primary_key=True)
)

class Post(Base):
    __tablename__ = "posts"

    id = Column(Integer, primary_key=True)
    title = Column(String(200))

    tags = relationship("Tag", secondary=post_tags, back_populates="posts")

class Tag(Base):
    __tablename__ = "tags"

    id = Column(Integer, primary_key=True)
    name = Column(String(50), unique=True)

    posts = relationship("Post", secondary=post_tags, back_populates="tags")
```

## Summary

SQLAlchemy supports all common relationship patterns through the `relationship()` function.

## Next Steps

Continue learning about:
- [Eager Loading](./08_eager_loading_strategies.md)
- [Lazy Loading](./09_lazy_loading_strategies.md)
