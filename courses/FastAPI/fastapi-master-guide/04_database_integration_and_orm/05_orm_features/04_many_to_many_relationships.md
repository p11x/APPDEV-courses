# Many-to-Many Relationships

## Overview

Many-to-many relationships link records where each can relate to multiple records on the other side.

## Implementation

### Association Table

```python
# Example 1: Many-to-many with association table
from sqlalchemy import Table, Column, Integer, ForeignKey
from sqlalchemy.orm import relationship

# Association table
post_tags = Table(
    "post_tags",
    Base.metadata,
    Column("post_id", Integer, ForeignKey("posts.id", ondelete="CASCADE"), primary_key=True),
    Column("tag_id", Integer, ForeignKey("tags.id", ondelete="CASCADE"), primary_key=True)
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

### Association Object

```python
# Example 2: Association object with extra data
class PostTag(Base):
    __tablename__ = "post_tags"

    post_id = Column(Integer, ForeignKey("posts.id"), primary_key=True)
    tag_id = Column(Integer, ForeignKey("tags.id"), primary_key=True)
    tagged_at = Column(DateTime, default=datetime.utcnow)
    tagged_by = Column(Integer, ForeignKey("users.id"))

    post = relationship("Post", back_populates="tag_associations")
    tag = relationship("Tag", back_populates="post_associations")

class Post(Base):
    tag_associations = relationship("PostTag", back_populates="post")
```

## Usage

```python
# Example 3: Working with many-to-many
def add_tag_to_post(db: Session, post_id: int, tag_id: int):
    post = db.query(Post).get(post_id)
    tag = db.query(Tag).get(tag_id)
    post.tags.append(tag)
    db.commit()

def get_posts_by_tag(db: Session, tag_name: str):
    return db.query(Post).join(Post.tags).filter(Tag.name == tag_name).all()
```

## Summary

Many-to-many relationships require association tables or objects.

## Next Steps

Continue learning about:
- [Eager Loading](./08_eager_loading_strategies.md)
- [Hybrid Attributes](./10_hybrid_attributes.md)
