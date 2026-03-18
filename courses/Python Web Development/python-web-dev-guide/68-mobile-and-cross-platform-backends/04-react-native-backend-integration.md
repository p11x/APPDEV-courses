# React Native Backend Integration

## What You'll Learn

- How to integrate with React Native apps
- Handling Expo and React Native CLI
- Push notification setup for React Native
- Authentication considerations

## Prerequisites

- Completed `03-building-an-ios-backend.md`

## Introduction

React Native allows you to build mobile apps using JavaScript and React. This guide covers backend integration for React Native applications, whether using Expo or the CLI.

## API Client Setup

Create a robust API client for React Native:

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, EmailStr
from typing import Optional
from datetime import datetime


app = FastAPI()


# Models
class User(BaseModel):
    id: int
    name: str
    email: EmailStr
    avatar_url: Optional[str] = None
    created_at: datetime


class UserCreate(BaseModel):
    name: str
    email: EmailStr
    password: str


class UserUpdate(BaseModel):
    name: Optional[str] = None
    avatar_url: Optional[str] = None


# In-memory storage (use database in production)
users_db: dict[int, User] = {}
next_id = 1


# Authentication endpoints
@app.post("/api/auth/register", response_model=dict)
async def register(user_data: UserCreate) -> dict:
    """Register a new user."""
    global next_id
    
    if any(u.email == user_data.email for u in users_db.values()):
        raise HTTPException(status_code=400, detail="Email already registered")
    
    user = User(
        id=next_id,
        name=user_data.name,
        email=user_data.email,
        created_at=datetime.now(),
    )
    
    users_db[next_id] = user
    next_id += 1
    
    # In production, generate and return JWT token
    return {
        "user": user.model_dump(),
        "token": "jwt_token_here",
    }


@app.post("/api/auth/login", response_model=dict)
async def login(email: str, password: str) -> dict:
    """Login user and return token."""
    user = None
    for u in users_db.values():
        if u.email == email:
            user = u
            break
    
    if not user:
        raise HTTPException(status_code=401, detail="Invalid credentials")
    
    return {
        "user": user.model_dump(),
        "token": "jwt_token_here",
    }


@app.get("/api/users/me", response_model=User)
async def get_current_user() -> User:
    """Get current authenticated user."""
    # In production, get user from JWT token
    return User(
        id=1,
        name="Demo User",
        email="demo@example.com",
        created_at=datetime.now(),
    )


@app.put("/api/users/me", response_model=User)
async def update_current_user(update: UserUpdate) -> User:
    """Update current user profile."""
    # In production, get user from JWT and update in database
    user = User(
        id=1,
        name=update.name or "Updated Name",
        email="demo@example.com",
        avatar_url=update.avatar_url,
        created_at=datetime.now(),
    )
    return user
```

## Push Notifications for React Native

React Native supports both FCM (Android) and APNs (iOS). Use Expo's push notification service:

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import Optional


app = FastAPI()

# For Expo, use Expo's push service
EXPO_PUSH_URL = "https://exp.host/--/api/v2/push/send"


class ExpoNotification(BaseModel):
    """Notification for Expo Push service."""
    to: str  # Expo push token
    title: str
    body: str
    data: Optional[dict] = None
    sound: Optional[str] = "default"
    badge: Optional[int] = None


@app.post("/api/notifications/expo/send")
async def send_expo_notification(notification: ExpoNotification) -> dict:
    """Send push notification to Expo-powered app."""
    import requests
    
    payload = {
        "to": notification.to,
        "title": notification.title,
        "body": notification.body,
    }
    
    if notification.sound:
        payload["sound"] = notification.sound
    
    if notification.badge is not None:
        payload["badge"] = notification.badge
    
    if notification.data:
        payload["data"] = notification.data
    
    response = requests.post(EXPO_PUSH_URL, json=payload)
    
    if response.status_code != 200:
        raise HTTPException(
            status_code=500,
            detail=f"Expo API error: {response.text}"
        )
    
    return response.json()


@app.post("/api/notifications/expo/send-multiple")
async def send_multiple_expo_notifications(
    notifications: list[ExpoNotification],
) -> dict:
    """Send multiple notifications in a single request."""
    import requests
    
    payloads = [
        {
            "to": n.to,
            "title": n.title,
            "body": n.body,
            "data": n.data,
            "sound": n.sound,
            "badge": n.badge,
        }
        for n in notifications
    ]
    
    response = requests.post(EXPO_PUSH_URL, json=payloads)
    
    return response.json()
```

## File Upload for React Native

Handle image and file uploads:

```python
from fastapi import FastAPI, UploadFile, File, HTTPException
from fastapi.responses import JSONResponse
import aiofiles
import os
import uuid
from pathlib import Path


app = FastAPI()

UPLOAD_DIR = Path("uploads")
UPLOAD_DIR.mkdir(exist_ok=True)


@app.post("/api/upload/image")
async def upload_image(
    file: UploadFile = File(...),
    user_id: Optional[int] = None,
) -> dict:
    """Upload an image from React Native."""
    
    # Validate file type
    allowed_types = ["image/jpeg", "image/png", "image/gif"]
    if file.content_type not in allowed_types:
        raise HTTPException(
            status_code=400,
            detail=f"Invalid file type. Allowed: {allowed_types}"
        )
    
    # Generate unique filename
    ext = file.filename.split(".")[-1]
    filename = f"{uuid.uuid4()}.{ext}"
    
    if user_id:
        user_dir = UPLOAD_DIR / str(user_id)
        user_dir.mkdir(exist_ok=True)
        file_path = user_dir / filename
    else:
        file_path = UPLOAD_DIR / filename
    
    # Save file
    async with aiofiles.open(file_path, "wb") as f:
        content = await file.read()
        await f.write(content)
    
    return {
        "success": True,
        "filename": filename,
        "url": f"/uploads/{user_id or 'general'}/{filename}",
        "size": len(content),
    }


@app.get("/uploads/{path:path}")
async def get_uploaded_file(path: str):
    """Serve uploaded files."""
    from fastapi.responses import FileResponse
    
    file_path = UPLOAD_DIR / path
    
    if not file_path.exists():
        raise HTTPException(status_code=404, detail="File not found")
    
    return FileResponse(file_path)
```

## Real-time Features

Implement real-time updates for React Native:

```python
from fastapi import FastAPI, WebSocket, WebSocketDisconnect
from typing import List
import json


app = FastAPI()


class ConnectionManager:
    """Manage WebSocket connections."""
    
    def __init__(self) -> None:
        self.active_connections: dict[int, WebSocket] = {}
    
    async def connect(self, user_id: int, websocket: WebSocket) -> None:
        """Connect a user."""
        await websocket.accept()
        self.active_connections[user_id] = websocket
    
    def disconnect(self, user_id: int) -> None:
        """Disconnect a user."""
        if user_id in self.active_connections:
            del self.active_connections[user_id]
    
    async def send_personal_message(
        self,
        message: dict,
        user_id: int,
    ) -> None:
        """Send message to specific user."""
        if user_id in self.active_connections:
            await self.active_connections[user_id].send_json(message)
    
    async def broadcast(self, message: dict) -> None:
        """Broadcast message to all connected users."""
        for connection in self.active_connections.values():
            await connection.send_json(message)


manager = ConnectionManager()


@app.websocket("/ws/{user_id}")
async def websocket_endpoint(websocket: WebSocket, user_id: int):
    """WebSocket endpoint for real-time communication."""
    await manager.connect(user_id, websocket)
    
    try:
        while True:
            # Wait for messages from client
            data = await websocket.receive_text()
            message = json.loads(data)
            
            # Handle different message types
            match message.get("type"):
                case "ping":
                    await manager.send_personal_message(
                        {"type": "pong"},
                        user_id,
                    )
                case "message":
                    # Handle chat message
                    await manager.send_personal_message(
                        {"type": "ack", "id": message.get("id")},
                        user_id,
                    )
                case _:
                    pass
                    
    except WebSocketDisconnect:
        manager.disconnect(user_id)


# Send notification to connected user
@app.post("/api/notify/{user_id}")
async def notify_user(user_id: int, message: str) -> dict:
    """Send real-time notification to a user."""
    await manager.send_personal_message(
        {"type": "notification", "message": message},
        user_id,
    )
    return {"success": True}
```

## React Native Authentication

Common authentication patterns for React Native:

```python
from fastapi import FastAPI, HTTPException, Header
from pydantic import BaseModel
from typing import Optional


app = FastAPI()


# JWT-based authentication
@app.get("/api/protected")
async def protected_route(
    authorization: Optional[str] = Header(None),
) -> dict:
    """Protected endpoint requiring authentication."""
    
    if not authorization:
        raise HTTPException(status_code=401, detail="No token provided")
    
    if not authorization.startswith("Bearer "):
        raise HTTPException(status_code=401, detail="Invalid token format")
    
    token = authorization[7:]  # Remove "Bearer " prefix
    
    # In production, verify JWT and extract user
    # For demo, just check token exists
    if token == "valid_token":
        return {"user_id": 1, "message": "Authenticated"}
    
    raise HTTPException(status_code=401, detail="Invalid token")


# OAuth for React Native
@app.get("/api/auth/google")
async def google_auth(code: str) -> dict:
    """Exchange Google auth code for tokens."""
    # In production, exchange code for tokens with Google
    return {
        "access_token": "access_token_here",
        "refresh_token": "refresh_token_here",
        "id_token": "id_token_here",
    }


@app.post("/api/auth/refresh")
async def refresh_token(refresh_token: str) -> dict:
    """Refresh access token."""
    # In production, verify refresh token and issue new access token
    return {
        "access_token": "new_access_token",
    }
```

## Summary

- React Native can use the same REST API as web applications
- For push notifications, use Expo Push Service or platform-specific (FCM/APNs)
- Handle file uploads with proper validation
- WebSockets enable real-time features
- Use JWT-based authentication compatible with mobile storage

## Next Steps

→ Continue to `05-flutter-backend-integration.md` to learn about Flutter backend integration.
