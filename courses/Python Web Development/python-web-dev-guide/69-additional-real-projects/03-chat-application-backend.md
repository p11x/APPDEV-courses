# Chat Application Backend

## What You'll Learn

- Building a real-time chat backend
- WebSocket implementation
- Direct messages and group chats
- Message history and typing indicators

## Prerequisites

- Completed `02-social-media-backend.md`

## Introduction

This project covers building a real-time chat application with WebSockets for instant messaging.

## Models

```python
from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime
import uuid


class User(BaseModel):
    id: str = Field(default_factory=lambda: str(uuid.uuid4()))
    username: str
    online: bool = False


class Message(BaseModel):
    id: str = Field(default_factory=lambda: str(uuid.uuid4()))
    sender_id: str
    receiver_id: Optional[str] = None  # For direct messages
    group_id: Optional[str] = None  # For group messages
    content: str
    read: bool = False
    created_at: datetime = Field(default_factory=datetime.now)


class Group(BaseModel):
    id: str = Field(default_factory=lambda: str(uuid.uuid4()))
    name: str
    members: List[str] = []
    created_by: str
    created_at: datetime = Field(default_factory=datetime.now)
```

## WebSocket Chat Server

```python
from fastapi import FastAPI, WebSocket, WebSocketDisconnect, HTTPException
from typing import Dict, Set
import json


app = FastAPI(title="Chat API")


# Connection managers
class ConnectionManager:
    """Manage WebSocket connections."""
    
    def __init__(self) -> None:
        # user_id -> WebSocket
        self.active_connections: Dict[str, WebSocket] = {}
        # group_id -> set of user_ids
        self.group_members: Dict[str, Set[str]] = {}
    
    async def connect(self, user_id: str, websocket: WebSocket) -> None:
        """Connect a user."""
        await websocket.accept()
        self.active_connections[user_id] = websocket
    
    def disconnect(self, user_id: str) -> None:
        """Disconnect a user."""
        if user_id in self.active_connections:
            del self.active_connections[user_id]
        
        # Remove from all groups
        for group_id in self.group_members:
            self.group_members[group_id].discard(user_id)
    
    async def send_personal_message(
        self,
        message: dict,
        user_id: str,
    ) -> None:
        """Send message to a specific user."""
        if user_id in self.active_connections:
            await self.active_connections[user_id].send_json(message)
    
    async def broadcast_to_group(
        self,
        message: dict,
        group_id: str,
    ) -> None:
        """Broadcast message to all members of a group."""
        if group_id in self.group_members:
            for member_id in self.group_members[group_id]:
                await self.send_personal_message(message, member_id)
    
    async def broadcast_online_users(self, message: dict) -> None:
        """Broadcast to all online users."""
        for connection in self.active_connections.values():
            await connection.send_json(message)
    
    def join_group(self, user_id: str, group_id: str) -> None:
        """Add user to a group."""
        if group_id not in self.group_members:
            self.group_members[group_id] = set()
        self.group_members[group_id].add(user_id)
    
    def leave_group(self, user_id: str, group_id: str) -> None:
        """Remove user from a group."""
        if group_id in self.group_members:
            self.group_members[group_id].discard(user_id)


manager = ConnectionManager()

# In-memory storage
users_db: dict[str, User] = {}
messages_db: list[Message] = []
groups_db: dict[str, Group] = {}


# ============ HTTP Endpoints ============

@app.post("/api/users", response_model=User)
async def create_user(username: str) -> User:
    """Create a new user."""
    user = User(username=username)
    users_db[user.id] = user
    return user


@app.get("/api/users", response_model=List[User])
async def list_users() -> List[User]:
    """List all users."""
    return list(users_db.values())


@app.get("/api/users/{user_id}/online")
async def get_online_status(user_id: str) -> dict:
    """Get user's online status."""
    return {
        "user_id": user_id,
        "online": user_id in manager.active_connections,
    }


# ============ Groups ============

@app.post("/api/groups", response_model=Group)
async def create_group(name: str, creator_id: str) -> Group:
    """Create a new group."""
    group = Group(name=name, created_by=creator_id, members=[creator_id])
    groups_db[group.id] = group
    manager.join_group(creator_id, group.id)
    return group


@app.post("/api/groups/{group_id}/join")
async def join_group(group_id: str, user_id: str) -> dict:
    """Add user to a group."""
    if group_id not in groups_db:
        raise HTTPException(status_code=404, detail="Group not found")
    
    if user_id not in users_db:
        raise HTTPException(status_code=404, detail="User not found")
    
    groups_db[group_id].members.append(user_id)
    manager.join_group(user_id, group_id)
    
    # Notify group
    await manager.broadcast_to_group(
        {
            "type": "user_joined",
            "user_id": user_id,
            "username": users_db[user_id].username,
        },
        group_id,
    )
    
    return {"success": True}


@app.get("/api/groups/{group_id}/members")
async def get_group_members(group_id: str) -> List[User]:
    """Get group members."""
    if group_id not in groups_db:
        raise HTTPException(status_code=404, detail="Group not found")
    
    return [
        users_db[uid]
        for uid in groups_db[group_id].members
        if uid in users_db
    ]


# ============ Messages ============

@app.get("/api/messages/dm/{user_id}", response_model=List[Message])
async def get_direct_messages(
    user_id: str,
    other_user_id: str,
    limit: int = 50,
) -> List[Message]:
    """Get direct messages between two users."""
    
    messages = [
        m for m in messages_db
        if (
            (m.sender_id == user_id and m.receiver_id == other_user_id) or
            (m.sender_id == other_user_id and m.receiver_id == user_id)
        )
    ]
    
    messages.sort(key=lambda m: m.created_at)
    return messages[-limit:]


@app.get("/api/messages/group/{group_id}", response_model=List[Message])
async def get_group_messages(
    group_id: str,
    limit: int = 50,
) -> List[Message]:
    """Get messages in a group."""
    
    messages = [m for m in messages_db if m.group_id == group_id]
    messages.sort(key=lambda m: m.created_at)
    return messages[-limit:]


# ============ WebSocket Endpoint ============

@app.websocket("/ws/{user_id}")
async def websocket_endpoint(websocket: WebSocket, user_id: str):
    """WebSocket endpoint for real-time chat."""
    
    if user_id not in users_db:
        await websocket.close(code=4004)
        return
    
    await manager.connect(user_id, websocket)
    
    try:
        # Send online users list
        await websocket.send_json({
            "type": "online_users",
            "users": list(manager.active_connections.keys()),
        })
        
        while True:
            data = await websocket.receive_text()
            message_data = json.loads(data)
            
            await handle_message(user_id, message_data)
            
    except WebSocketDisconnect:
        manager.disconnect(user_id)
        
        # Notify others
        await manager.broadcast_online_users({
            "type": "user_offline",
            "user_id": user_id,
        })


async def handle_message(sender_id: str, data: dict) -> None:
    """Handle incoming WebSocket messages."""
    
    message_type = data.get("type")
    
    match message_type:
        case "direct_message":
            # Send direct message
            receiver_id = data.get("receiver_id")
            content = data.get("content")
            
            message = Message(
                sender_id=sender_id,
                receiver_id=receiver_id,
                content=content,
            )
            messages_db.append(message)
            
            # Send to receiver
            await manager.send_personal_message({
                "type": "new_message",
                "message": message.model_dump(),
            }, receiver_id)
            
            # Send back to sender (confirmation)
            await manager.send_personal_message({
                "type": "message_sent",
                "message": message.model_dump(),
            }, sender_id)
        
        case "group_message":
            # Send to group
            group_id = data.get("group_id")
            content = data.get("content")
            
            if group_id not in groups_db:
                return
            
            message = Message(
                sender_id=sender_id,
                group_id=group_id,
                content=content,
            )
            messages_db.append(message)
            
            # Broadcast to group
            await manager.broadcast_to_group({
                "type": "new_message",
                "message": message.model_dump(),
            }, group_id)
        
        case "typing":
            # Typing indicator
            receiver_id = data.get("receiver_id")
            is_typing = data.get("is_typing", True)
            
            await manager.send_personal_message({
                "type": "typing",
                "user_id": sender_id,
                "is_typing": is_typing,
            }, receiver_id)
        
        case _:
            pass


from typing import List
```

## Summary

This chat backend includes:
- WebSocket real-time messaging
- Direct messages
- Group chats
- Online/offline status
- Typing indicators
- Message history

In production, add:
- Authentication
- Database persistence
- Message encryption
- File/image sharing
- Read receipts

## Next Steps

This concludes the Additional Real Projects section. The Python Web Development Guide is now complete with comprehensive coverage of all major topics.
