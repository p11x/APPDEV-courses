# One-to-One Relationships

## Overview

One-to-one relationships link exactly one record to exactly one related record.

## Implementation

### Basic One-to-One

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
    avatar_url = Column(String(200))

    user = relationship("User", back_populates="profile")
```

### Usage

```python
# Example 2: Working with one-to-one
def create_user_with_profile(db: Session, username: str, bio: str):
    """Create user with profile"""
    user = User(username=username)
    profile = Profile(bio=bio, user=user)

    db.add(user)
    db.add(profile)
    db.commit()
    return user

def get_user_profile(db: Session, user_id: int):
    """Get user with profile"""
    return db.query(User).options(
        joinedload(User.profile)
    ).filter(User.id == user_id).first()
```

## Summary

One-to-one relationships are useful for optional or extended data.

## Next Steps

Continue learning about:
- [One-to-Many](./03_one_to_many_relationships.md)
- [Many-to-Many](./04_many_to_many_relationships.md)
