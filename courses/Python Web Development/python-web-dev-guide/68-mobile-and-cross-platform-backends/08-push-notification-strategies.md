# Push Notification Strategies

## What You'll Learn

- Effective push notification strategies
- Segmentation and targeting
- Notification scheduling
- Personalization techniques

## Prerequisites

- Completed `07-mobile-app-analytics-backend.md`

## Introduction

Push notifications are a powerful engagement tool. This guide covers strategies for effective push notifications across mobile platforms.

## Notification Management System

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import Optional, List
from datetime import datetime
from dataclasses import dataclass
import uuid


app = FastAPI()


# ============ Models ============

@dataclass
class NotificationTemplate:
    """Notification template."""
    id: str
    title: str
    body: str
    data: dict
    platform: str  # ios, android, all


@dataclass
class NotificationCampaign:
    """Notification campaign."""
    id: str
    name: str
    template_id: str
    target_segment: str
    scheduled_at: Optional[datetime]
    status: str  # draft, scheduled, sent, cancelled
    created_at: datetime
    sent_count: int = 0
    open_count: int = 0


class CreateCampaignRequest(BaseModel):
    """Request to create a campaign."""
    name: str
    title: str
    body: str
    target_segment: str  # all, active, inactive, custom
    custom_users: Optional[List[str]] = None
    scheduled_at: Optional[datetime] = None


# ============ Storage ============
campaigns_db: dict[str, NotificationCampaign] = {}
templates_db: dict[str, NotificationTemplate] = {}


# ============ Campaigns ============

@app.post("/api/campaigns")
async def create_campaign(request: CreateCampaignRequest) -> dict:
    """Create a new notification campaign."""
    
    campaign_id = str(uuid.uuid4())
    
    campaign = NotificationCampaign(
        id=campaign_id,
        name=request.name,
        template_id=str(uuid.uuid4()),
        target_segment=request.target_segment,
        scheduled_at=request.scheduled_at,
        status="scheduled" if request.scheduled_at else "draft",
        created_at=datetime.now(),
    )
    
    campaigns_db[campaign_id] = campaign
    
    return {"success": True, "campaign_id": campaign_id}


@app.get("/api/campaigns", response_model=List[dict])
async def get_campaigns() -> List[dict]:
    """Get all campaigns."""
    return [
        {
            "id": c.id,
            "name": c.name,
            "target_segment": c.target_segment,
            "status": c.status,
            "scheduled_at": c.scheduled_at.isoformat() if c.scheduled_at else None,
            "sent_count": c.sent_count,
            "open_count": c.open_count,
        }
        for c in campaigns_db.values()
    ]


@app.get("/api/campaigns/{campaign_id}")
async def get_campaign(campaign_id: str) -> dict:
    """Get campaign details."""
    
    if campaign_id not in campaigns_db:
        raise HTTPException(status_code=404, detail="Campaign not found")
    
    campaign = campaigns_db[campaign_id]
    
    return {
        "id": campaign.id,
        "name": campaign.name,
        "target_segment": campaign.target_segment,
        "status": campaign.status,
        "sent_count": campaign.sent_count,
        "open_count": campaign.open_count,
        "ctr": campaign.open_count / campaign.sent_count if campaign.sent_count > 0 else 0,
    }


@app.post("/api/campaigns/{campaign_id}/send")
async def send_campaign(campaign_id: str) -> dict:
    """Send a campaign immediately."""
    
    if campaign_id not in campaigns_db:
        raise HTTPException(status_code=404, detail="Campaign not found")
    
    campaign = campaigns_db[campaign_id]
    
    # In production, send to actual devices
    # For demo, just update counts
    campaign.status = "sent"
    campaign.sent_count = 100  # Would be actual count
    campaign.open_count = 25   # Would track actual opens
    
    return {"success": True, "sent_count": campaign.sent_count}


# ============ Segmentation ============

SEGMENTS = {
    "all": "All users",
    "active": "Users active in last 7 days",
    "inactive": "Users inactive for 30+ days",
    "new": "Users who signed up in last 7 days",
}


@app.get("/api/segments")
async def get_segments() -> dict:
    """Get available user segments."""
    return SEGMENTS


@app.get("/api/segments/{segment}/count")
async def get_segment_count(segment: str) -> dict:
    """Get user count for a segment."""
    
    # In production, query database
    # Demo values
    counts = {
        "all": 10000,
        "active": 7500,
        "inactive": 2500,
        "new": 500,
    }
    
    if segment not in counts:
        raise HTTPException(status_code=404, detail="Segment not found")
    
    return {"segment": segment, "count": counts[segment]}
```

## Personalization

```python
from pydantic import BaseModel


class PersonalizedNotification(BaseModel):
    """Personalized notification request."""
    user_id: str
    template: str  # Template with placeholders
    data: dict  # User data for personalization


def personalize_message(template: str, user_data: dict) -> str:
    """Replace placeholders with user data."""
    
    message = template
    
    for key, value in user_data.items():
        placeholder = f"{{{{{key}}}}}"
        message = message.replace(placeholder, str(value))
    
    return message


# Example templates
NOTIFICATION_TEMPLATES = {
    "welcome": {
        "title": "Welcome, {{name}}!",
        "body": "Thanks for joining {{app_name}}! Start exploring now.",
    },
    "order_shipped": {
        "title": "Your order is on the way!",
        "body": "Hi {{name}}, your order #{{order_id}} has shipped!",
    },
    "abandoned_cart": {
        "title": "Don't forget your items!",
        "body": "{{name}}, you left {{item_count}} items in your cart.",
    },
    "daily_digest": {
        "title": "Your daily summary",
        "body": "You have {{notification_count}} new notifications today!",
    },
}


@app.post("/api/notifications/personalized")
async def send_personalized_notification(
    user_id: str,
    template_name: str,
    user_data: dict,
) -> dict:
    """Send a personalized notification."""
    
    if template_name not in NOTIFICATION_TEMPLATES:
        raise HTTPException(status_code=400, detail="Template not found")
    
    template = NOTIFICATION_TEMPLATES[template_name]
    
    # Personalize
    title = personalize_message(template["title"], user_data)
    body = personalize_message(template["body"], user_data)
    
    # In production, send notification
    return {
        "success": True,
        "personalized_title": title,
        "personalized_body": body,
    }
```

## Summary

- Create notification campaigns with targeting
- Use segments for efficient delivery
- Personalize messages with user data
- Track campaign performance

## Next Steps

→ Continue to `09-mobile-backend-best-practices.md` to learn about mobile backend best practices.
