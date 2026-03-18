# Building an iOS Backend

## What You'll Learn

- Apple Push Notification service (APNs) setup
- iOS-specific notification considerations
- Universal Links
- Device token management

## Prerequisites

- Completed `02-building-an-android-backend.md`

## Introduction

iOS apps require Apple Push Notification service (APNs) for push notifications. This guide covers the backend setup for sending notifications to iOS devices.

## Apple Push Notification Service (APNs)

Set up APNs for your backend:

```bash
pip install hyper
```

```python
import os
import jwt
import time
from datetime import datetime, timedelta
from dataclasses import dataclass
from typing import Optional


@dataclass
class APNsConfig:
    """Apple Push Notification service configuration."""
    key_id: str
    team_id: str
    bundle_id: str
    key_path: str  # Path to .p8 file


class APNsClient:
    """Client for sending push notifications via APNs."""
    
    def __init__(self, config: APNsConfig) -> None:
        self.config = config
        self.base_url = "https://api.push.apple.com"  # Production
        # self.base_url = "api.sandbox.push.apple.com"  # Sandbox
    
    def _generate_token(self) -> str:
        """Generate a JWT token for authentication."""
        
        with open(self.config.key_path, "r") as f:
            private_key = f.read()
        
        now = int(time.time())
        
        token = jwt.encode(
            {
                "iss": self.config.team_id,
                "iat": now,
            },
            private_key,
            algorithm="ES256",
            headers={
                "kid": self.config.key_id,
                "alg": "ES256",
            },
        )
        
        return token
    
    def send_notification(
        self,
        token: str,
        title: str,
        body: str,
        data: Optional[dict] = None,
        badge: Optional[int] = None,
        sound: str = "default",
        category: Optional[str] = None,
    ) -> dict:
        """Send a push notification to an iOS device."""
        
        jwt_token = self._generate_token()
        
        # Build the notification payload
        payload = {
            "aps": {
                "alert": {
                    "title": title,
                    "body": body,
                },
                "sound": sound,
            },
        }
        
        # Add badge
        if badge is not None:
            payload["aps"]["badge"] = badge
        
        # Add custom data
        if data:
            payload.update(data)
        
        # Import for HTTP/2 request
        import hyper.tls
        import hyper
        
        conn = hyper.HTTP20Connection(
            f"{self.config.bundle_id}.push.apple.com:443",
            ssl_context=hyper.tls.init_context(),
        )
        
        headers = {
            "apns-priority": "10",
            "apns-topic": self.config.bundle_id,
        }
        
        if category:
            headers["apns-category"] = category
        
        conn.request(
            "POST",
            f"/3/device/{token}",
            body=str(payload),
            headers=headers,
        )
        
        response = conn.get_response()
        
        return {
            "status": response.status,
            "body": response.read().decode(),
        }


# Example usage
def main() -> None:
    config = APNsConfig(
        key_id=os.environ["APNS_KEY_ID"],
        team_id=os.environ["APNS_TEAM_ID"],
        bundle_id=os.environ["APNS_BUNDLE_ID"],
        key_path=os.environ["APNS_KEY_PATH"],
    )
    
    client = APNsClient(config)
    
    # Note: Token format for APNs is different from FCM
    result = client.send_notification(
        token="device_token_here",
        title="New Message",
        body="You have a new message",
        badge=1,
    )
    print(f"Notification sent: {result}")


if __name__ == "__main__":
    main()
```

🔍 **Line-by-Line Breakdown:**

1. `jwt` — PyJWT library for creating JSON Web Tokens for APNs authentication.
2. `APNsConfig` — Configuration containing Apple Developer credentials (key ID, team ID, bundle ID, path to .p8 file).
3. `_generate_token()` — Creates a JWT using the .p8 private key with ES256 algorithm.
4. `jwt.encode()` — Creates the token with claims (issuer, issued at time) and headers (key ID, algorithm).
5. `payload["aps"]` — The Apple Push Notification service payload structure.
6. `apns-priority` — Header indicating delivery priority (10 = immediate, 5 = power-saving).
7. `apns-topic` — The topic (usually bundle ID) for the notification.

## Alternative: Using a Library

```bash
pip install apns2
```

```python
from apns2.client import APNsClient
from apns2.payload import Payload
from apns2.enums import Priority


class SimplifiedAPNsClient:
    """Simplified APNs client using apns2 library."""
    
    def __init__(self, bundle_id: str, token_key_path: str) -> None:
        self.bundle_id = bundle_id
        self.client = APNsClient(
            token_key_path,
            bundle_id=bundle_id,
            production=True,
        )
    
    def send_notification(
        self,
        token: str,
        title: str,
        body: str,
        badge: Optional[int] = None,
    ) -> bool:
        """Send notification to a device."""
        
        payload = Payload(
            alert=title,
            body=body,
            badge=badge,
            sound="default",
        )
        
        try:
            self.client.send(payload, token)
            return True
        except Exception as e:
            print(f"Failed to send: {e}")
            return False
```

## Rich Notifications

iOS supports rich notifications with images and actions:

```python
def send_rich_notification(
    self,
    token: str,
    title: str,
    body: str,
    image_url: Optional[str] = None,
    actions: Optional[list[dict]] = None,
) -> dict:
    """Send a rich notification with attachments and actions."""
    
    # Build payload with notification extension
    payload = {
        "aps": {
            "alert": {
                "title": title,
                "body": body,
            },
            "mutable-content": 1,  # Allow modification by extension
            "category": "MESSAGE_CATEGORY",
        },
    }
    
    # Add attachment for rich notification
    if image_url:
        payload["aps"]["attachment-url"] = image_url
    
    # ... send using hyper
    return payload


# Define notification categories
NOTIFICATION_CATEGORIES = {
    "MESSAGE": {
        "actions": [
            {
                "id": "REPLY",
                "title": "Reply",
                "foreground": True,
            },
            {
                "id": "MARK_READ",
                "title": "Mark as Read",
                "foreground": False,
            },
        ],
        "options": {"authenticationRequired": True},
    },
    "ALERT": {
        "actions": [
            {
                "id": "VIEW",
                "title": "View",
                "foreground": True,
            },
            {
                "id": "DISMISS",
                "title": "Dismiss",
                "foreground": False,
            },
        ],
    },
}
```

## Universal Links

Universal Links allow your website URLs to open your iOS app:

```python
from fastapi import FastAPI


app = FastAPI()


@app.get("/.well-known/apple-app-site-association")
async def apple_app_site_association():
    """Serve Apple App Site Association file."""
    return {
        "applinks": {
            "apps": [],
            "details": [
                {
                    "appID": "TEAMID.com.yourcompany.yourapp",
                    "paths": [
                        "/posts/*",
                        "/products/*",
                        "/NOT /posts/draft/*",  # Exclude
                    ],
                    "appclips": {
                        "paths": ["/clip/*"],
                    },
                }
            ],
        }
    }


@app.get("/posts/{post_id}")
async def get_post(post_id: str):
    """Handle universal link to a post."""
    # Can serve both HTML for web and JSON for app
    return {
        "id": post_id,
        "title": "Sample Post",
        "content": "Content here",
    }
```

## Token Management

iOS device tokens can change, so proper management is important:

```python
from datetime import datetime


class iOSTokenManager:
    """Manage iOS device tokens."""
    
    def __init__(self) -> None:
        self.tokens: dict[str, dict] = {}
    
    def store_token(
        self,
        user_id: str,
        token: str,
    ) -> None:
        """Store or update device token."""
        self.tokens[token] = {
            "user_id": user_id,
            "created_at": datetime.now(),
            "last_used": datetime.now(),
        }
    
    def invalidate_token(self, token: str) -> bool:
        """Mark a token as invalid (e.g., on logout)."""
        if token in self.tokens:
            del self.tokens[token]
            return True
        return False
    
    def get_user_tokens(self, user_id: str) -> list[str]:
        """Get all tokens for a user."""
        return [
            token
            for token, info in self.tokens.items()
            if info["user_id"] == user_id
        ]


# FastAPI endpoint for token registration
from fastapi import HTTPException


@app.post("/api/devices/ios/register")
async def register_ios_device(
    user_id: str,
    token: str,
) -> dict:
    """Register iOS device token."""
    token_manager.store_token(user_id, token)
    return {"success": True}


@app.post("/api/devices/ios/unregister")
async def unregister_ios_device(token: str) -> dict:
    """Unregister iOS device token."""
    token_manager.invalidate_token(token)
    return {"success": True}
```

## Silent Notifications

iOS supports silent push notifications that wake your app:

```python
def send_silent_notification(
    self,
    token: str,
    data: dict,
    content_available: int = 1,
) -> dict:
    """Send a silent push notification to wake the app."""
    
    payload = {
        "aps": {
            "content-available": content_available,  # This makes it silent
        },
    }
    
    # Add custom data
    payload.update(data)
    
    # Send using hyper...
    return payload
```

## FastAPI Integration

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel


app = FastAPI()

# Initialize clients (would be configured from environment)
apns_client = SimplifiedAPNsClient(
    bundle_id=os.environ["APNS_BUNDLE_ID"],
    token_key_path=os.environ["APNS_KEY_PATH"],
)


class NotificationRequest(BaseModel):
    """Request to send notification."""
    token: str
    title: str
    body: str
    badge: Optional[int] = None


@app.post("/api/notifications/ios/send")
async def send_ios_notification(request: NotificationRequest) -> dict:
    """Send notification to iOS device."""
    try:
        success = apns_client.send_notification(
            token=request.token,
            title=request.title,
            body=request.body,
            badge=request.badge,
        )
        
        return {"success": success}
    except Exception as e:
        raise HTTPException(
            status_code=500,
            detail=f"Failed to send notification: {str(e)}"
        )
```

## Summary

- Apple Push Notification service (APNs) is required for iOS push notifications
- APNs uses token-based authentication with .p8 private keys
- Universal Links enable deep linking from web URLs to your app
- Device tokens can change — implement proper token management
- Silent notifications wake your app in the background
- Use rich notifications for interactive content

## Next Steps

→ Continue to `04-react-native-backend-integration.md` to learn about React Native backend integration.
