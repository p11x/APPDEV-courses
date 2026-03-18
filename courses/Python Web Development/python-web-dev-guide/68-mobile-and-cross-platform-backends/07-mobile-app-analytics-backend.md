# Mobile App Analytics Backend

## What You'll Learn

- Tracking mobile app events
- Analytics API implementation
- User engagement metrics
- Crash reporting

## Prerequisites

- Completed `06-cross-platform-authentication.md`

## Introduction

Analytics help you understand how users interact with your mobile app. This guide covers implementing analytics tracking for mobile applications.

## Analytics Event Tracking

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import Optional, Any
from datetime import datetime
from dataclasses import dataclass, field
import uuid


app = FastAPI()


# ============ Event Models ============

@dataclass
class AnalyticsEvent:
    """An analytics event."""
    id: str = field(default_factory=lambda: str(uuid.uuid4()))
    event_name: str = ""
    user_id: Optional[str] = None
    device_id: Optional[str] = None
    platform: Optional[str] = None  # ios, android, web
    app_version: Optional[str] = None
    session_id: Optional[str] = None
    timestamp: datetime = field(default_factory=datetime.now)
    properties: dict = field(default_factory=dict)


class EventRequest(BaseModel):
    """Request to track an event."""
    event_name: str
    user_id: Optional[str] = None
    device_id: Optional[str] = None
    platform: Optional[str] = None
    app_version: Optional[str] = None
    session_id: Optional[str] = None
    properties: Optional[dict] = None


class BatchEventRequest(BaseModel):
    """Request to track multiple events."""
    events: list[EventRequest]


# ============ Storage ============
# In production, use a database or analytics service
events_db: list[AnalyticsEvent] = []


# ============ Endpoints ============

@app.post("/api/analytics/track")
async def track_event(event: EventRequest) -> dict:
    """Track a single analytics event."""
    
    analytics_event = AnalyticsEvent(
        event_name=event.event_name,
        user_id=event.user_id,
        device_id=event.device_id,
        platform=event.platform,
        app_version=event.app_version,
        session_id=event.session_id,
        properties=event.properties or {},
    )
    
    events_db.append(analytics_event)
    
    return {"success": True, "event_id": analytics_event.id}


@app.post("/api/analytics/track/batch")
async def track_events_batch(request: BatchEventRequest) -> dict:
    """Track multiple events in a single request."""
    
    event_ids = []
    
    for event_data in request.events:
        analytics_event = AnalyticsEvent(
            event_name=event_data.event_name,
            user_id=event_data.user_id,
            device_id=event_data.device_id,
            platform=event_data.platform,
            app_version=event_data.app_version,
            session_id=event_data.session_id,
            properties=event_data.properties or {},
        )
        
        events_db.append(analytics_event)
        event_ids.append(analytics_event.id)
    
    return {"success": True, "event_count": len(event_ids)}


@app.get("/api/analytics/events")
async def get_events(
    user_id: Optional[str] = None,
    event_name: Optional[str] = None,
    limit: int = 100,
) -> list[dict]:
    """Query analytics events."""
    
    results = events_db
    
    if user_id:
        results = [e for e in results if e.user_id == user_id]
    
    if event_name:
        results = [e for e in results if e.event_name == event_name]
    
    # Sort by timestamp descending
    results.sort(key=lambda e: e.timestamp, reverse=True)
    
    # Limit results
    results = results[:limit]
    
    return [
        {
            "id": e.id,
            "event_name": e.event_name,
            "user_id": e.user_id,
            "device_id": e.device_id,
            "platform": e.platform,
            "timestamp": e.timestamp.isoformat(),
            "properties": e.properties,
        }
        for e in results
    ]


# ============ Analytics Queries ============

@app.get("/api/analytics/summary")
async def get_analytics_summary(
    start_date: Optional[str] = None,
    end_date: Optional[str] = None,
) -> dict:
    """Get analytics summary."""
    
    # Filter events by date range
    events = events_db
    
    if start_date:
        start = datetime.fromisoformat(start_date)
        events = [e for e in events if e.timestamp >= start]
    
    if end_date:
        end = datetime.fromisoformat(end_date)
        events = [e for e in events if e.timestamp <= end]
    
    # Calculate metrics
    total_events = len(events)
    unique_users = len(set(e.user_id for e in events if e.user_id))
    unique_devices = len(set(e.device_id for e in events if e.device_id))
    
    # Event counts by type
    event_counts: dict[str, int] = {}
    for event in events:
        event_counts[event.event_name] = event_counts.get(event.event_name, 0) + 1
    
    # Platform distribution
    platform_counts: dict[str, int] = {}
    for event in events:
        if event.platform:
            platform_counts[event.platform] = platform_counts.get(event.platform, 0) + 1
    
    return {
        "total_events": total_events,
        "unique_users": unique_users,
        "unique_devices": unique_devices,
        "event_counts": event_counts,
        "platform_distribution": platform_counts,
    }


@app.get("/api/analytics/user/{user_id}")
async def get_user_analytics(user_id: str) -> dict:
    """Get analytics for a specific user."""
    
    user_events = [e for e in events_db if e.user_id == user_id]
    
    if not user_events:
        raise HTTPException(status_code=404, detail="No events found for user")
    
    # Calculate user metrics
    first_seen = min(e.timestamp for e in user_events)
    last_seen = max(e.timestamp for e in user_events)
    total_events = len(user_events)
    
    # Events by type
    event_types = {}
    for event in user_events:
        event_types[event.event_name] = event_types.get(event.event_name, 0) + 1
    
    return {
        "user_id": user_id,
        "first_seen": first_seen.isoformat(),
        "last_seen": last_seen.isoformat(),
        "total_events": total_events,
        "event_types": event_types,
    }
```

## Screen Tracking

```python
from pydantic import BaseModel


class ScreenViewEvent(BaseModel):
    """Screen view event."""
    screen_name: str
    user_id: Optional[str] = None
    device_id: Optional[str] = None
    platform: Optional[str] = None
    duration_seconds: Optional[int] = None
    properties: Optional[dict] = None


@app.post("/api/analytics/screen")
async def track_screen_view(event: ScreenViewEvent) -> dict:
    """Track screen view."""
    
    analytics_event = AnalyticsEvent(
        event_name="screen_view",
        user_id=event.user_id,
        device_id=event.device_id,
        platform=event.platform,
        properties={
            "screen_name": event.screen_name,
            "duration_seconds": event.duration_seconds,
            **(event.properties or {}),
        },
    )
    
    events_db.append(analytics_event)
    
    return {"success": True}


# Screen view analytics
@app.get("/api/analytics/screens")
async def get_screen_analytics() -> dict:
    """Get analytics for screens."""
    
    screen_events = [
        e for e in events_db
        if e.event_name == "screen_view"
    ]
    
    screen_stats: dict[str, dict] = {}
    
    for event in screen_events:
        screen_name = event.properties.get("screen_name", "unknown")
        
        if screen_name not in screen_stats:
            screen_stats[screen_name] = {
                "views": 0,
                "unique_users": set(),
            }
        
        screen_stats[screen_name]["views"] += 1
        if event.user_id:
            screen_stats[screen_name]["unique_users"].add(event.user_id)
    
    # Convert sets to counts
    for screen_name in screen_stats:
        screen_stats[screen_name]["unique_users"] = len(
            screen_stats[screen_name]["unique_users"]
        )
    
    return screen_stats
```

## Summary

- Track events with consistent naming conventions
- Use batch endpoints to reduce network requests
- Implement user and device tracking
- Query analytics to understand user behavior

## Next Steps

→ Continue to `08-push-notification-strategies.md` to learn about push notification best practices.
