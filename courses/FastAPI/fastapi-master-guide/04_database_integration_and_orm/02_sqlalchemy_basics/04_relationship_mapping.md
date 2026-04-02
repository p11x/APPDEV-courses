# Relationship Mapping

## Overview

SQLAlchemy relationships define how database tables connect. This guide covers all relationship types for FastAPI applications.

## Relationship Types

### One-to-Many

```python
# Example 1: One-to-many relationship
from sqlalchemy import Column, Integer, String, ForeignKey
from sqlalchemy.orm import relationship, Mapped, mapped_column
from typing import List

class Base(DeclarativeBase):
    pass

class User(Base):
    __tablename__ = "users"

    id: Mapped[int] = mapped_column(primary_key=True)
    username: Mapped[str] = mapped_column(String(50), unique=True)

    # One user has many posts
    posts: Mapped[List["Post"]] = relationship(
        back_populates="author",
        cascade="all, delete-orphan",  # Delete posts when user deleted
        lazy="selectin"  # Eager load posts
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

### Many-to-Many

```python
# Example 2: Many-to-many relationship
from sqlalchemy import Table, Column, Integer, ForeignKey

# Association table
user_roles = Table(
    "user_roles",
    Base.metadata,
    Column("user_id", Integer, ForeignKey("users.id"), primary_key=True),
    Column("role_id", Integer, ForeignKey("roles.id"), primary_key=True)
)

class User(Base):
    __tablename__ = "users"

    id: Mapped[int] = mapped_column(primary_key=True)
    username: Mapped[str] = mapped_column(String(50))

    # Many-to-many with roles
    roles: Mapped[List["Role"]] = relationship(
        secondary=user_roles,
        back_populates="users",
        lazy="selectin"
    )

class Role(Base):
    __tablename__ = "roles"

    id: Mapped[int] = mapped_column(primary_key=True)
    name: Mapped[str] = mapped_column(String(50), unique=True)

    users: Mapped[List["User"]] = relationship(
        secondary=user_roles,
        back_populates="roles"
    )

# Usage
def assign_role(db: Session, user_id: int, role_id: int):
    user = db.query(User).filter(User.id == user_id).first()
    role = db.query(Role).filter(Role.id == role_id).first()
    user.roles.append(role)
    db.commit()
```

### One-to-One

```python
# Example 3: One-to-one relationship
class User(Base):
    __tablename__ = "users"

    id: Mapped[int] = mapped_column(primary_key=True)
    username: Mapped[str] = mapped_column(String(50))

    # One-to-one relationship
    profile: Mapped["Profile"] = relationship(
        back_populates="user",
        uselist=False,  # This makes it one-to-one
        cascade="all, delete-orphan"
    )

class Profile(Base):
    __tablename__ = "profiles"

    id: Mapped[int] = mapped_column(primary_key=True)
    user_id: Mapped[int] = mapped_column(ForeignKey("users.id"), unique=True)
    bio: Mapped[str] = mapped_column(default="")
    avatar_url: Mapped[str] = mapped_column(default="")

    user: Mapped["User"] = relationship(back_populates="profile")
```

## Loading Strategies

### Eager vs Lazy Loading

```python
# Example 4: Loading strategies
from sqlalchemy.orm import joinedload, selectinload, subqueryload

# Lazy loading (default) - loads on access
user = db.query(User).first()
posts = user.posts  # Additional query here

# Eager loading with joinedload - LEFT JOIN
users = db.query(User).options(
    joinedload(User.posts)
).all()

# Eager loading with selectinload - separate IN query
users = db.query(User).options(
    selectinload(User.posts)
).all()

# Multiple eager loads
users = db.query(User).options(
    selectinload(User.posts).joinedload(Post.comments),
    selectinload(User.roles)
).all()

# Async loading
from sqlalchemy import select
from sqlalchemy.orm import selectinload

async def get_users_with_posts(session: AsyncSession):
    stmt = select(User).options(selectinload(User.posts))
    result = await session.execute(stmt)
    return result.scalars().all()
```

## Cascade Options

### Cascade Behaviors

```python
# Example 5: Cascade options
class User(Base):
    __tablename__ = "users"

    # Cascade options:
    # "save-update" - Add related objects to session
    # "merge" - Merge related objects
    # "expunge" - Remove related from session
    # "delete" - Delete related objects
    # "delete-orphan" - Delete orphaned objects
    # "all" - All except delete-orphan
    # "all, delete-orphan" - Everything

    posts: Mapped[List["Post"]] = relationship(
        cascade="all, delete-orphan"
    )
```

## Best Practices

### Relationship Guidelines

```python
# Example 6: Best practices
"""
Relationship Best Practices:

1. Always define back_populates
   - Bidirectional relationships
   - Consistency between sides

2. Choose appropriate loading strategy
   - selectinload for collections
   - joinedload for many-to-one

3. Use cascade carefully
   - delete-orphan can be dangerous
   - Test cascade behavior

4. Avoid N+1 queries
   - Use eager loading
   - Monitor query count

5. Use association objects for M:N with data
   - Track relationship metadata
   - Add timestamps, etc.
"""

# Association object with additional data
class UserProject(Base):
    __tablename__ = "user_projects"

    user_id: Mapped[int] = mapped_column(ForeignKey("users.id"), primary_key=True)
    project_id: Mapped[int] = mapped_column(ForeignKey("projects.id"), primary_key=True)
    role: Mapped[str] = mapped_column(default="member")
    joined_at: Mapped[datetime] = mapped_column(default=datetime.utcnow)

    # Relationships
    user: Mapped["User"] = relationship(back_populates="project_associations")
    project: Mapped["Project"] = relationship(back_populates="user_associations")
```

## Summary

| Relationship | SQLAlchemy | Database |
|--------------|------------|----------|
| One-to-Many | `relationship()` + `ForeignKey` | FK on child |
| Many-to-Many | `secondary=table` | Association table |
| One-to-One | `uselist=False` | FK with unique |

## Next Steps

Continue learning about:
- [Eager Loading](./08_eager_loading_strategies.md) - Performance
- [Lazy Loading](./09_lazy_loading_strategies.md) - On-demand loading
