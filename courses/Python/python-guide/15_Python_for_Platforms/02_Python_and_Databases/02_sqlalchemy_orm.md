# 🏗️ SQLAlchemy 2.0: The Professional ORM

## 🎯 What You'll Learn

- SQLAlchemy 2.0 basics with type annotations
- Creating models with Mapped[]
- Querying with select()
- Relationships

---

## Installation

```bash
pip install sqlalchemy
```

---

## Basic Setup

```python
from sqlalchemy import create_engine
from sqlalchemy.orm import DeclarativeBase, Session, Mapped, mapped_column

# Base class for models
class Base(DeclarativeBase):
    pass

# Engine
engine = create_engine("sqlite:///myapp.db", echo=True)

# Create tables
Base.metadata.create_all(engine)
```

---

## Defining Models

```python
from sqlalchemy import String, Integer
from sqlalchemy.orm import Mapped, mapped_column

class User(Base):
    __tablename__ = "users"
    
    id: Mapped[int] = mapped_column(primary_key=True)
    name: Mapped[str] = mapped_column(String(100))
    email: Mapped[str] = mapped_column(String(200), unique=True)
    
    def __repr__(self):
        return f"<User {self.name}>"
```

---

## CRUD with Session

```python
# Create
with Session(engine) as session:
    user = User(name="Alice", email="alice@example.com")
    session.add(user)
    session.commit()
    print(f"Created: {user.id}")

# Read
with Session(engine) as session:
    # Get one
    user = session.get(User, 1)
    
    # Query
    from sqlalchemy import select
    stmt = select(User).where(User.name == "Alice")
    user = session.scalar(stmt)
    
    # List
    stmt = select(User)
    users = session.scalars(stmt).all()

# Update
with Session(engine) as session:
    user = session.get(User, 1)
    user.name = "Alice Smith"
    session.commit()

# Delete
with Session(engine) as session:
    user = session.get(User, 1)
    session.delete(user)
    session.commit()
```

---

## Relationships

```python
class Post(Base):
    __tablename__ = "posts"
    
    id: Mapped[int] = mapped_column(primary_key=True)
    title: Mapped[str]
    user_id: Mapped[int] = mapped_column ForeignKey("users.id")
    
    # Relationship
    user: Mapped["User"] = relationship(back_populates="posts")

class User(Base):
    __tablename__ = "users"
    
    id: Mapped[int] = mapped_column(primary_key=True)
    name: Mapped[str]
    
    posts: Mapped[list["Post"]] = relationship(back_populates="user")

# Usage
with Session(engine) as session:
    user = session.get(User, 1)
    for post in user.posts:  # Lazy-loaded!
        print(post.title)
```

---

## SQLAlchemy 2.0 vs 1.x

```python
# Old style (1.x)
session.query(User).filter_by(name="Alice").first()

# New style (2.0)
from sqlalchemy import select
stmt = select(User).where(User.name == "Alice")
user = session.scalar(stmt)
```

---

## ✅ Summary

- Use Mapped[] and mapped_column() for type-safe columns
- Use select() instead of query() in 2.0
- Session manages transactions automatically
- Relationships use back_populates for bidirectional links

## 🔗 Further Reading

- [SQLAlchemy 2.0 Documentation](https://docs.sqlalchemy.org/en/20/)
