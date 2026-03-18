# Building an Android Backend

## What You'll Learn

- Android-specific API considerations
- Push notification setup with FCM
- Android app links verification
- Play Store App Signing

## Prerequisites

- Completed `01-restful-api-for-mobile.md`

## Introduction

Android apps have specific requirements including Firebase Cloud Messaging (FCM) for push notifications, App Links for deep linking, and specific authentication considerations.

## Firebase Cloud Messaging (FCM)

FCM is the recommended way to send push notifications to Android devices:

```bash
pip install firebase-admin
```

Set up FCM in your backend:

```python
import os
import firebase_admin
from firebase_admin import credentials
from firebase_admin import messaging
from dataclasses import dataclass
from typing import Optional


@dataclass
class FCMConfig:
    """Firebase Cloud Messaging configuration."""
    credentials_path: str


class FCMClient:
    """Client for sending push notifications via FCM."""
    
    def __init__(self, config: FCMConfig) -> None:
        # Initialize Firebase Admin SDK
        cred = credentials.Certificate(config.credentials_path)
        
        if not firebase_admin._apps:
            firebase_admin.initialize_app(cred)
        
        self.credentials = config.credentials_path
    
    def send_notification(
        self,
        token: str,
        title: str,
        body: str,
        data: Optional[dict] = None,
    ) -> str:
        """Send a push notification to a specific device."""
        
        # Build the message
        message = messaging.Message(
            notification=messaging.Notification(
                title=title,
                body=body,
            ),
            data=data or {},
            token=token,
        )
        
        # Send the message
        response = messaging.send(message)
        
        return response
    
    def send_to_topic(
        self,
        topic: str,
        title: str,
        body: str,
        data: Optional[dict] = None,
    ) -> str:
        """Send a notification to all devices subscribed to a topic."""
        
        message = messaging.Message(
            notification=messaging.Notification(
                title=title,
                body=body,
            ),
            data=data or {},
            topic=topic,
        )
        
        response = messaging.send(message)
        return response
    
    def subscribe_to_topic(
        self,
        tokens: list[str],
        topic: str,
    ) -> dict:
        """Subscribe devices to a topic."""
        
        response = messaging.subscribe_to_topic(tokens, topic)
        
        return {
            "success": response.success_count,
            "failure": response.failure_count,
            "errors": [
                {"index": e.index, "error": e.reason}
                for e in response.errors
            ],
        }
    
    def unsubscribe_from_topic(
        self,
        tokens: list[str],
        topic: str,
    ) -> dict:
        """Unsubscribe devices from a topic."""
        
        response = messaging.unsubscribe_from_topic(tokens, topic)
        
        return {
            "success": response.success_count,
            "failure": response.failure_count,
        }


# Example usage
def main() -> None:
    config = FCMConfig(
        credentials_path=os.environ["FCM_CREDENTIALS_PATH"],
    )
    
    client = FCMClient(config)
    
    # Send to a specific device
    result = client.send_notification(
        token="device_token_here",
        title="New Message",
        body="You have a new message from John",
        data={"type": "message", "sender_id": "123"},
    )
    print(f"Notification sent: {result}")
    
    # Subscribe to a topic
    client.subscribe_to_topic(
        tokens=["token1", "token2"],
        topic="updates",
    )


if __name__ == "__main__":
    main()
```

🔍 **Line-by-Line Breakdown:**

1. `firebase_admin` — Google's official Firebase Admin SDK for Python.
2. `credentials.Certificate()` — Loads the Firebase service account credentials from a JSON file.
3. `firebase_admin.initialize_app()` — Initializes the Firebase app with credentials.
4. `messaging.Message()` — Creates an FCM message with notification and data payload.
5. `notification` — The visual notification shown to users (title, body).
6. `data` — Custom key-value pairs passed to the app (used for handling in background).
7. `token` — The device token uniquely identifying an Android device.
8. `topic` — A way to send messages to multiple devices that subscribed to a topic.

## Advanced Notification Options

```python
def send_advanced_notification(
    self,
    token: str,
    title: str,
    body: str,
    image_url: Optional[str] = None,
    click_action: Optional[str] = None,
) -> str:
    """Send a notification with advanced options."""
    
    # Build Android-specific notification
    android_config = messaging.AndroidConfig(
        priority=messaging.AndroidPriority.HIGH,
        ttl=3600,  # Time to live in seconds
        collapse_key="updates",  # Collapse multiple notifications
        notification=messaging.AndroidNotification(
            title=title,
            body=body,
            icon="notification_icon",
            color="#FF5722",
            sound="default",
            click_action=click_action,
            image_url=image_url,
            tag="updates",  # Used to replace existing notification
        ),
    )
    
    message = messaging.Message(
        notification=messaging.Notification(title=title, body=body),
        android=android_config,
        token=token,
    )
    
    return messaging.send(message)
```

## FastAPI Integration with FCM

Create endpoints to manage notifications:

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import Optional


app = FastAPI()

# Initialize FCM client
fcm_client = FCMClient(FCMConfig(credentials_path="firebase-credentials.json"))


class NotificationRequest(BaseModel):
    """Request to send a notification."""
    token: str
    title: str
    body: str
    data: Optional[dict] = None


class TopicNotificationRequest(BaseModel):
    """Request to send to a topic."""
    topic: str
    title: str
    body: str
    data: Optional[dict] = None


@app.post("/api/notifications/send")
async def send_notification(request: NotificationRequest) -> dict:
    """Send a notification to a specific device."""
    try:
        result = fcm_client.send_notification(
            token=request.token,
            title=request.title,
            body=request.body,
            data=request.data,
        )
        return {"success": True, "message_id": result}
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))


@app.post("/api/notifications/topic")
async def send_to_topic(request: TopicNotificationRequest) -> dict:
    """Send a notification to a topic."""
    try:
        result = fcm_client.send_to_topic(
            topic=request.topic,
            title=request.title,
            body=request.body,
            data=request.data,
        )
        return {"success": True, "message_id": result}
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))


@app.post("/api/notifications/subscribe")
async def subscribe_to_topic(topic: str, tokens: list[str]) -> dict:
    """Subscribe devices to a topic."""
    try:
        result = fcm_client.subscribe_to_topic(tokens, topic)
        return result
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))
```

## Android App Links

Android App Links are HTTP URLs that link to content in your app:

```python
from fastapi import FastAPI


app = FastAPI()


@app.get("/.well-known/assetlinks.json")
async def asset_links():
    """Serve Android App Links verification file."""
    # This file verifies ownership of your domains
    return [
        {
            "relation": ["delegate_permission/common.handle_all_urls"],
            "target": {
                "namespace": "android_app",
                "package_name": "com.yourcompany.yourapp",
                "sha256_cert_fingerprints": [
                    "YOUR:FINGERPRINT:HERE"
                ],
            },
        }
    ]


@app.get("/.well-known/apple-app-site-association")
async def apple_app_site_association():
    """Serve Apple App Site Association file (for iOS)."""
    return {
        "applinks": {
            "apps": [],
            "details": [
                {
                    "appID": "TEAMID.com.yourcompany.yourapp",
                    "paths": ["/path/*"],
                }
            ],
        }
    }
```

## Token Management

Handle FCM tokens properly:

```python
from datetime import datetime, timedelta


class TokenManager:
    """Manage device tokens."""
    
    def __init__(self) -> None:
        # In production, use a database
        self.tokens: dict[str, dict] = {}
    
    def store_token(
        self,
        user_id: str,
        token: str,
        platform: str = "android",
    ) -> None:
        """Store or update a device token."""
        self.tokens[token] = {
            "user_id": user_id,
            "platform": platform,
            "created_at": datetime.now(),
            "last_used": datetime.now(),
        }
    
    def get_user_tokens(self, user_id: str) -> list[str]:
        """Get all tokens for a user."""
        return [
            token
            for token, info in self.tokens.items()
            if info["user_id"] == user_id
        ]
    
    def cleanup_old_tokens(self, days: int = 90) -> int:
        """Remove tokens not used in specified days."""
        cutoff = datetime.now() - timedelta(days=days)
        
        old_tokens = [
            token
            for token, info in self.tokens.items()
            if info["last_used"] < cutoff
        ]
        
        for token in old_tokens:
            del self.tokens[token]
        
        return len(old_tokens)


# Use token manager
token_manager = TokenManager()


@app.post("/api/devices/register")
async def register_device(
    user_id: str,
    token: str,
    platform: str = "android",
) -> dict:
    """Register a device token for a user."""
    token_manager.store_token(user_id, token, platform)
    return {"success": True}
```

## Summary

- Firebase Cloud Messaging (FCM) is the standard for Android push notifications
- Use topics for broadcasting to multiple devices
- Configure Android-specific notification options (priority, sound, icon)
- Handle token refresh and cleanup properly
- Android App Links require serving verification files from your domain

## Next Steps

→ Continue to `03-building-an-ios-backend.md` to learn about iOS-specific backend considerations.
