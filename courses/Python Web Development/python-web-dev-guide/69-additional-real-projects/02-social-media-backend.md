# Social Media Backend

## What You'll Learn

- Building a social media backend
- User profiles and relationships
- Posts, likes, and comments
- Feed generation

## Prerequisites

- Completed `01-e-commerce-backend.md`

## Introduction

This project covers building a social media backend with features like posts, likes, comments, and feed generation.

## Models

```python
from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime
from enum import Enum
import uuid


class User(BaseModel):
    id: str = Field(default_factory=lambda: str(uuid.uuid4()))
    username: str
    display_name: str
    bio: str = ""
    avatar_url: Optional[str] = None
    followers_count: int = 0
    following_count: int = 0
    posts_count: int = 0
    created_at: datetime = Field(default_factory=datetime.now)


class Post(BaseModel):
    id: str = Field(default_factory=lambda: str(uuid.uuid4()))
    user_id: str
    content: str
    image_url: Optional[str] = None
    likes_count: int = 0
    comments_count: int = 0
    created_at: datetime = Field(default_factory=datetime.now)


class Like(BaseModel):
    id: str = Field(default_factory=lambda: str(uuid.uuid4()))
    user_id: str
    post_id: str
    created_at: datetime = Field(default_factory=datetime.now)


class Comment(BaseModel):
    id: str = Field(default_factory=lambda: str(uuid.uuid4()))
    post_id: str
    user_id: str
    content: str
    created_at: datetime = Field(default_factory=datetime.now)


class Follow(BaseModel):
    follower_id: str
    following_id: str
    created_at: datetime = Field(default_factory=datetime.now)
```

## Main Application

```python
from fastapi import FastAPI, HTTPException
from typing import List


app = FastAPI(title="Social Media API")

# In-memory storage
users_db: dict[str, User] = {}
posts_db: dict[str, Post] = {}
likes_db: dict[str, Like] = {}
comments_db: dict[str, Comment] = {}
follows_db: set = set()  # (follower_id, following_id)


# ============ Users ============

@app.post("/api/users", response_model=User)
async def create_user(username: str, display_name: str) -> User:
    """Create a new user."""
    
    # Check username taken
    if any(u.username == username for u in users_db.values()):
        raise HTTPException(status_code=400, detail="Username taken")
    
    user = User(username=username, display_name=display_name)
    users_db[user.id] = user
    
    return user


@app.get("/api/users/{user_id}", response_model=User)
async def get_user(user_id: str) -> User:
    """Get user profile."""
    
    if user_id not in users_db:
        raise HTTPException(status_code=404, detail="User not found")
    
    return users_db[user_id]


@app.get("/api/users/{user_id}/followers", response_model=List[User])
async def get_followers(user_id: str) -> List[User]:
    """Get user's followers."""
    
    follower_ids = [
        uid for fuid, uid in follows_db
        if fuid == user_id
    ]
    
    return [users_db[uid] for uid in follower_ids if uid in users_db]


@app.get("/api/users/{user_id}/following", response_model=List[User])
async def get_following(user_id: str) -> List[User]:
    """Get users that this user follows."""
    
    following_ids = [
        fuid for uid, fuid in follows_db
        if uid == user_id
    ]
    
    return [users_db[fuid] for fid in following_ids if fid in users_db]


# ============ Posts ============

@app.post("/api/posts", response_model=Post)
async def create_post(
    user_id: str,
    content: str,
    image_url: Optional[str] = None,
) -> Post:
    """Create a new post."""
    
    if user_id not in users_db:
        raise HTTPException(status_code=404, detail="User not found")
    
    post = Post(
        user_id=user_id,
        content=content,
        image_url=image_url,
    )
    
    posts_db[post.id] = post
    
    # Update post count
    users_db[user_id].posts_count += 1
    
    return post


@app.get("/api/posts/{post_id}", response_model=Post)
async def get_post(post_id: str) -> Post:
    """Get a post."""
    
    if post_id not in posts_db:
        raise HTTPException(status_code=404, detail="Post not found")
    
    return posts_db[post_id]


@app.get("/api/users/{user_id}/posts", response_model=List[Post])
async def get_user_posts(user_id: str) -> List[Post]:
    """Get user's posts."""
    
    user_posts = [
        post for post in posts_db.values()
        if post.user_id == user_id
    ]
    
    # Sort by newest first
    user_posts.sort(key=lambda p: p.created_at, reverse=True)
    
    return user_posts


# ============ Feed ============

@app.get("/api/feed", response_model=List[Post])
async def get_feed(
    user_id: str,
    limit: int = 20,
) -> List[Post]:
    """Get user's feed (posts from followed users)."""
    
    # Get IDs of users this user follows
    following_ids = [
        fuid for uid, fuid in follows_db
        if uid == user_id
    ]
    
    # Include own posts too
    following_ids.append(user_id)
    
    # Get posts from followed users
    feed_posts = [
        post for post in posts_db.values()
        if post.user_id in following_ids
    ]
    
    # Sort by newest
    feed_posts.sort(key=lambda p: p.created_at, reverse=True)
    
    return feed_posts[:limit]


# ============ Likes ============

@app.post("/api/posts/{post_id}/like")
async def like_post(post_id: str, user_id: str) -> dict:
    """Like a post."""
    
    if post_id not in posts_db:
        raise HTTPException(status_code=404, detail="Post not found")
    
    # Check if already liked
    already_liked = any(
        l.post_id == post_id and l.user_id == user_id
        for l in likes_db.values()
    )
    
    if already_liked:
        raise HTTPException(status_code=400, detail="Already liked")
    
    like = Like(user_id=user_id, post_id=post_id)
    likes_db[like.id] = like
    
    # Update like count
    posts_db[post_id].likes_count += 1
    
    return {"success": True}


@app.delete("/api/posts/{post_id}/like")
async def unlike_post(post_id: str, user_id: str) -> dict:
    """Unlike a post."""
    
    # Find the like
    like_id = None
    for lid, like in likes_db.items():
        if like.post_id == post_id and like.user_id == user_id:
            like_id = lid
            break
    
    if like_id:
        del likes_db[like_id]
        posts_db[post_id].likes_count -= 1
    
    return {"success": True}


# ============ Comments ============

@app.post("/api/posts/{post_id}/comments", response_model=Comment)
async def create_comment(
    post_id: str,
    user_id: str,
    content: str,
) -> Comment:
    """Add a comment to a post."""
    
    if post_id not in posts_db:
        raise HTTPException(status_code=404, detail="Post not found")
    
    comment = Comment(
        post_id=post_id,
        user_id=user_id,
        content=content,
    )
    
    comments_db[comment.id] = comment
    posts_db[post_id].comments_count += 1
    
    return comment


@app.get("/api/posts/{post_id}/comments", response_model=List[Comment])
async def get_comments(post_id: str) -> List[Comment]:
    """Get comments for a post."""
    
    post_comments = [
        c for c in comments_db.values()
        if c.post_id == post_id
    ]
    
    post_comments.sort(key=lambda c: c.created_at)
    
    return post_comments


# ============ Follow ============

@app.post("/api/users/{user_id}/follow")
async def follow_user(follower_id: str, user_id: str) -> dict:
    """Follow a user."""
    
    if user_id not in users_db or follower_id not in users_db:
        raise HTTPException(status_code=404, detail="User not found")
    
    if user_id == follower_id:
        raise HTTPException(status_code=400, detail="Cannot follow yourself")
    
    follows_db.add((follower_id, user_id))
    
    # Update counts
    users_db[follower_id].following_count += 1
    users_db[user_id].followers_count += 1
    
    return {"success": True}


@app.delete("/api/users/{user_id}/follow")
async def unfollow_user(follower_id: str, user_id: str) -> dict:
    """Unfollow a user."""
    
    if (follower_id, user_id) in follows_db:
        follows_db.remove((follower_id, user_id))
        
        # Update counts
        users_db[follower_id].following_count -= 1
        users_db[user_id].followers_count -= 1
    
    return {"success": True}
```

## Summary

This social media backend includes:
- User profiles
- Posts with images
- Likes and comments
- Follow system
- Feed generation

In production, add:
- Authentication
- Pagination
- Media uploads
- Real-time features with WebSockets

## Next Steps

Continue to `03-chat-application-backend.md` to build a real-time chat application.
