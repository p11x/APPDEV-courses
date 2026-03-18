# Flutter Backend Integration

## What You'll Learn

- Backend integration for Flutter apps
- HTTP and Dio client usage
- Push notifications with Firebase
- Deep linking

## Prerequisites

- Completed `04-react-native-backend-integration.md`

## Introduction

Flutter is Google's cross-platform UI toolkit. This guide covers backend integration for Flutter applications, using the same REST APIs and push notification services.

## API Design for Flutter

Flutter apps can use the same API endpoints as web and React Native apps:

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, EmailStr
from typing import Optional
from datetime import datetime
import uuid


app = FastAPI()


# ============ Data Models ============

class Post(BaseModel):
    id: str
    title: str
    content: str
    author_id: str
    created_at: datetime
    image_url: Optional[str] = None
    likes_count: int = 0


class PostCreate(BaseModel):
    title: str
    content: str
    image_url: Optional[str] = None


class Comment(BaseModel):
    id: str
    post_id: str
    author_id: str
    content: str
    created_at: datetime


# ============ In-Memory Storage ============
posts_db: dict[str, Post] = {}
comments_db: dict[str, Comment] = {}


# ============ Post Endpoints ============

@app.post("/api/posts", response_model=dict)
async def create_post(post: PostCreate, author_id: str) -> dict:
    """Create a new post."""
    post_id = str(uuid.uuid4())
    
    new_post = Post(
        id=post_id,
        title=post.title,
        content=post.content,
        author_id=author_id,
        image_url=post.image_url,
        created_at=datetime.now(),
    )
    
    posts_db[post_id] = new_post
    
    return {"post": new_post.model_dump()}


@app.get("/api/posts", response_model=dict)
async def get_posts(
    page: int = 1,
    limit: int = 20,
    author_id: Optional[str] = None,
) -> dict:
    """Get paginated list of posts."""
    posts = list(posts_db.values())
    
    # Filter by author if provided
    if author_id:
        posts = [p for p in posts if p.author_id == author_id]
    
    # Sort by created_at descending
    posts.sort(key=lambda p: p.created_at, reverse=True)
    
    # Paginate
    start = (page - 1) * limit
    end = start + limit
    paginated_posts = posts[start:end]
    
    return {
        "posts": [p.model_dump() for p in paginated_posts],
        "pagination": {
            "page": page,
            "limit": limit,
            "total": len(posts),
            "has_more": end < len(posts),
        },
    }


@app.get("/api/posts/{post_id}", response_model=Post)
async def get_post(post_id: str) -> Post:
    """Get a single post by ID."""
    if post_id not in posts_db:
        raise HTTPException(status_code=404, detail="Post not found")
    
    return posts_db[post_id]


@app.delete("/api/posts/{post_id}")
async def delete_post(post_id: str, author_id: str) -> dict:
    """Delete a post."""
    if post_id not in posts_db:
        raise HTTPException(status_code=404, detail="Post not found")
    
    post = posts_db[post_id]
    
    if post.author_id != author_id:
        raise HTTPException(status_code=403, detail="Not authorized")
    
    del posts_db[post_id]
    
    return {"success": True}


# ============ Like Endpoints ============

@app.post("/api/posts/{post_id}/like")
async def like_post(post_id: str) -> dict:
    """Like a post."""
    if post_id not in posts_db:
        raise HTTPException(status_code=404, detail="Post not found")
    
    posts_db[post_id].likes_count += 1
    
    return {"likes_count": posts_db[post_id].likes_count}


@app.delete("/api/posts/{post_id}/like")
async def unlike_post(post_id: str) -> dict:
    """Unlike a post."""
    if post_id not in posts_db:
        raise HTTPException(status_code=404, detail="Post not found")
    
    posts_db[post_id].likes_count = max(0, posts_db[post_id].likes_count - 1)
    
    return {"likes_count": posts_db[post_id].likes_count}


# ============ Comment Endpoints ============

@app.post("/api/posts/{post_id}/comments", response_model=Comment)
async def create_comment(
    post_id: str,
    author_id: str,
    content: str,
) -> Comment:
    """Create a comment on a post."""
    if post_id not in posts_db:
        raise HTTPException(status_code=404, detail="Post not found")
    
    comment = Comment(
        id=str(uuid.uuid4()),
        post_id=post_id,
        author_id=author_id,
        content=content,
        created_at=datetime.now(),
    )
    
    comments_db[comment.id] = comment
    
    return comment


@app.get("/api/posts/{post_id}/comments", response_model=list[Comment])
async def get_comments(post_id: str) -> list[Comment]:
    """Get all comments for a post."""
    return [
        c for c in comments_db.values()
        if c.post_id == post_id
    ]
```

## Firebase for Flutter

Flutter typically uses Firebase for many features:

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import Optional


app = FastAPI()


# Firebase Cloud Messaging (same as Android)
class FCMNotificationRequest(BaseModel):
    """Request to send FCM notification."""
    token: str
    title: str
    body: str
    data: Optional[dict] = None


@app.post("/api/fcm/send")
async def send_fcm_notification(request: FCMNotificationRequest) -> dict:
    """Send notification via Firebase Cloud Messaging."""
    # Same implementation as Android FCM
    import firebase_admin
    from firebase_admin import messaging
    
    message = messaging.Message(
        notification=messaging.Notification(
            title=request.title,
            body=request.body,
        ),
        data=request.data or {},
        token=request.token,
    )
    
    response = messaging.send(message)
    
    return {"success": True, "message_id": response}
```

## Deep Linking

Handle deep links from Flutter:

```python
from fastapi import FastAPI, Request
from fastapi.responses import HTMLResponse


app = FastAPI()


# App Links (Android) - same as before
@app.get("/.well-known/assetlinks.json")
async def asset_links():
    """Android App Links verification."""
    return [
        {
            "relation": ["delegate_permission/common.handle_all_urls"],
            "target": {
                "namespace": "android_app",
                "package_name": "com.yourcompany.flutterapp",
                "sha256_cert_fingerprints": ["FINGERPRINT"],
            },
        }
    ]


# iOS Universal Links
@app.get("/.well-known/apple-app-site-association")
async def apple_app_site_association():
    """iOS Universal Links verification."""
    return {
        "applinks": {
            "apps": [],
            "details": [
                {
                    "appID": "TEAMID.com.yourcompany.flutterapp",
                    "paths": ["/*"],
                }
            ],
        }
    }


# Deep link handling
DEEP_LINK_DATA = {
    "posts": {
        "view": "PostDetailScreen",
    },
    "profile": {
        "view": "ProfileScreen",
    },
    "settings": {
        "view": "SettingsScreen",
    },
}


@app.get("/link/{path:path}")
async def deep_link(path: str, request: Request) -> dict:
    """Handle deep links."""
    # Parse the path
    parts = path.split("/")
    
    if not parts:
        return {"error": "Invalid path"}
    
    route = parts[0]
    
    # Build response based on route
    if route in DEEP_LINK_DATA:
        return {
            "deep_link": True,
            "view": DEEP_LINK_DATA[route]["view"],
            "params": parts[1:] if len(parts) > 1 else [],
        }
    
    # Check for post links
    if route == "post":
        if len(parts) > 1:
            return {
                "deep_link": True,
                "view": "PostDetailScreen",
                "post_id": parts[1],
            }
    
    return {"deep_link": True, "view": "HomeScreen"}
```

## Offline-First Considerations

Design for offline-first Flutter apps:

```python
from fastapi import FastAPI
from pydantic import BaseModel
from typing import Optional
from datetime import datetime


app = FastAPI()


class SyncRequest(BaseModel):
    """Request to sync local data with server."""
    user_id: str
    last_sync: datetime
    local_changes: list[dict]


class SyncResponse(BaseModel):
    """Response with server changes."""
    changes: list[dict]
    server_time: datetime
    deleted_ids: list[str]


@app.post("/api/sync", response_model=SyncResponse)
async def sync_data(request: SyncRequest) -> SyncResponse:
    """Sync data between Flutter app and server."""
    
    # In production, query database for changes since last_sync
    server_changes = []
    deleted_ids = []
    
    return SyncResponse(
        changes=server_changes,
        server_time=datetime.now(),
        deleted_ids=deleted_ids,
    )
```

## Image Handling

Optimize images for mobile:

```python
from fastapi import FastAPI, HTTPException
from fastapi.responses import FileResponse
from PIL import Image
import io


app = FastAPI()


# Image optimization endpoint
@app.get("/api/images/{image_id}")
async def get_optimized_image(
    image_id: str,
    width: Optional[int] = None,
    height: Optional[int] = None,
    quality: int = 80,
):
    """Get optimized image for mobile."""
    # In production, fetch from storage and resize
    # This is a simplified example
    
    # Create placeholder image
    img = Image.new("RGB", (400, 400), color="blue")
    
    # Resize if dimensions provided
    if width or height:
        img = img.resize((width or 100, height or 100))
    
    # Save to buffer
    buffer = io.BytesIO()
    img.save(buffer, format="JPEG", quality=quality)
    buffer.seek(0)
    
    return FileResponse(
        buffer,
        media_type="image/jpeg",
        headers={
            "Cache-Control": "public, max-age=86400",
        },
    )
```

## Summary

- Flutter apps use standard REST APIs for backend communication
- Firebase provides FCM for push notifications
- Use deep links for navigation from web to app
- Design for offline-first with sync endpoints
- Optimize images for mobile bandwidth

## Next Steps

→ Continue to `06-cross-platform-authentication.md` to learn about authentication across platforms.
