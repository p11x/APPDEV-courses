# One-to-Many Relationships

## Overview

One-to-many relationships link one parent record to multiple child records, the most common relationship pattern.

## Implementation

### Basic One-to-Many

```python
# Example 1: One-to-many relationship
from sqlalchemy import Column, Integer, String, ForeignKey
from sqlalchemy.orm import relationship, Mapped, mapped_column
from typing import List

class User(Base):
    __tablename__ = "users"

    id: Mapped[int] = mapped_column(primary_key=True)
    username: Mapped[str] = mapped_column(String(50), unique=True)

    # One user has many posts
    posts: Mapped[List["Post"]] = relationship(
        back_populates="author",
        cascade="all, delete-orphan",
        lazy="selectin"
    )

class Post(Base):
    __tablename__ = "posts"

    id: Mapped[int] = mapped_column(primary_key=True)
    title: Mapped[str] = mapped_column(String(200))
    content: Mapped[str] = mapped_column()
    author_id: Mapped[int] = mapped_column(ForeignKey("users.id"))

    # Many posts belong to one user
    author: Mapped["User"] = relationship(back_populates="posts")
```

## Usage Examples

```python
# Example 2: Working with one-to-many
async def create_post_for_user(db: Session, user_id: int, post_data: dict):
    """Create post for existing user"""
    user = db.query(User).filter(User.id == user_id).first()
    if not user:
        raise HTTPException(404, "User not found")

    post = Post(**post_data, author_id=user_id)
    db.add(post)
    db.commit()
    return post

async def get_user_with_posts(db: Session, user_id: int):
    """Get user and all their posts"""
    return db.query(User).options(
        selectinload(User.posts)
    ).filter(User.id == user_id).first()
```

## Cascade Options

```python
# Example 3: Cascade behavior
class User(Base):
    posts = relationship(
        "Post",
        back_populates="author",
        cascade="all, delete-orphan",  # Delete posts when user deleted
        passive_deletes=True           # Let database handle deletes
    )
```

## Summary

One-to-many relationships are fundamental for modeling parent-child data.

## Next Steps

Continue learning about:
- [Many-to-Many](./04_many_to_many_relationships.md)
- [Eager Loading](./08_eager_loading_strategies.md)
