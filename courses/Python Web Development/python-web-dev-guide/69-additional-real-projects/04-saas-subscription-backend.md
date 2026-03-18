# SaaS Subscription Backend

## What You'll Learn

- Building a SaaS subscription management system
- Plan tiers and pricing
- Usage tracking and limits
- Subscription lifecycle

## Prerequisites

- Completed `03-chat-application-backend.md`

## Introduction

This project covers building a SaaS backend with subscription management, usage tracking, and billing.

## Models

```python
from pydantic import BaseModel, Field
from typing import Optional, List, Dict
from datetime import datetime
from enum import Enum
import uuid


class PlanTier(str, Enum):
    FREE = "free"
    STARTER = "starter"
    PRO = "pro"
    ENTERPRISE = "enterprise"


class SubscriptionStatus(str, Enum):
    ACTIVE = "active"
    PAST_DUE = "past_due"
    CANCELLED = "cancelled"
    TRIALING = "trialing"


class Plan(BaseModel):
    id: str = Field(default_factory=lambda: str(uuid.uuid4()))
    name: str
    tier: PlanTier
    price: float = 0
    interval: str = "month"  # month, year
    features: Dict[str, int] = {}  # feature -> limit
    created_at: datetime = Field(default_factory=datetime.now)


class Subscription(BaseModel):
    id: str = Field(default_factory=lambda: str(uuid.uuid4()))
    user_id: str
    plan_id: str
    status: SubscriptionStatus = SubscriptionStatus.TRIALING
    current_period_start: datetime = Field(default_factory=datetime.now)
    current_period_end: datetime
    cancel_at_period_end: bool = False
    created_at: datetime = Field(default_factory=datetime.now)


class Usage(BaseModel):
    id: str = Field(default_factory=lambda: str(uuid.uuid4()))
    user_id: str
    feature: str
    count: int = 0
    reset_at: datetime
    created_at: datetime = Field(default_factory=datetime.now)
```

## Main Application

```python
from fastapi import FastAPI, HTTPException
from typing import List


app = FastAPI(title="SaaS Subscription API")


# ============ Plans ============

# Default plans
plans_db: dict[str, Plan] = {
    "free": Plan(
        id="free",
        name="Free",
        tier=PlanTier.FREE,
        price=0,
        features={"api_calls": 1000, "storage_mb": 100, "users": 1},
    ),
    "starter": Plan(
        id="starter",
        name="Starter",
        tier=PlanTier.STARTER,
        price=29,
        features={"api_calls": 10000, "storage_mb": 1000, "users": 5},
    ),
    "pro": Plan(
        id="pro",
        name="Professional",
        tier=PlanTier.PRO,
        price=99,
        features={"api_calls": 100000, "storage_mb": 10000, "users": 25},
    ),
    "enterprise": Plan(
        id="enterprise",
        name="Enterprise",
        tier=PlanTier.ENTERPRISE,
        price=299,
        features={"api_calls": -1, "storage_mb": -1, "users": -1},  # Unlimited
    ),
}


subscriptions_db: dict[str, Subscription] = {}
usage_db: dict[str, Usage] = {}


# ============ Plans API ============

@app.get("/api/plans", response_model=List[Plan])
async def list_plans() -> List[Plan]:
    """List all available plans."""
    return list(plans_db.values())


@app.get("/api/plans/{plan_id}", response_model=Plan)
async def get_plan(plan_id: str) -> Plan:
    """Get plan details."""
    if plan_id not in plans_db:
        raise HTTPException(status_code=404, detail="Plan not found")
    return plans_db[plan_id]


# ============ Subscription API ============

@app.post("/api/subscriptions")
async def create_subscription(
    user_id: str,
    plan_id: str,
    trial_days: int = 14,
) -> Subscription:
    """Create a new subscription."""
    
    if plan_id not in plans_db:
        raise HTTPException(status_code=404, detail="Plan not found")
    
    # Calculate period end
    from datetime import timedelta
    period_end = datetime.now() + timedelta(days=trial_days)
    
    subscription = Subscription(
        user_id=user_id,
        plan_id=plan_id,
        status=SubscriptionStatus.TRIALING,
        current_period_end=period_end,
    )
    
    subscriptions_db[subscription.id] = subscription
    
    # Initialize usage
    plan = plans_db[plan_id]
    for feature, limit in plan.features.items():
        usage_id = f"{user_id}:{feature}"
        usage_db[usage_id] = Usage(
            user_id=user_id,
            feature=feature,
            count=0,
            reset_at=period_end,
        )
    
    return subscription


@app.get("/api/subscriptions/{user_id}")
async def get_subscription(user_id: str) -> Subscription:
    """Get user's subscription."""
    
    for sub in subscriptions_db.values():
        if sub.user_id == user_id:
            return sub
    
    raise HTTPException(status_code=404, detail="No subscription found")


@app.post("/api/subscriptions/{subscription_id}/cancel")
async def cancel_subscription(subscription_id: str) -> dict:
    """Cancel subscription at period end."""
    
    if subscription_id not in subscriptions_db:
        raise HTTPException(status_code=404, detail="Subscription not found")
    
    sub = subscriptions_db[subscription_id]
    sub.cancel_at_period_end = True
    sub.status = SubscriptionStatus.CANCELLED
    
    return {"success": True}


# ============ Usage API ============

@app.post("/api/usage/{user_id}/track")
async def track_usage(
    user_id: str,
    feature: str,
    count: int = 1,
) -> dict:
    """Track feature usage."""
    
    # Get user's subscription
    user_sub = None
    for sub in subscriptions_db.values():
        if sub.user_id == user_id and sub.status == SubscriptionStatus.ACTIVE:
            user_sub = sub
            break
    
    if not user_sub:
        return {"allowed": False, "reason": "No active subscription"}
    
    plan = plans_db[user_sub.plan_id]
    limit = plan.features.get(feature, 0)
    
    # Check if unlimited
    if limit == -1:
        return {"allowed": True, "remaining": -1}
    
    # Get current usage
    usage_id = f"{user_id}:{feature}"
    if usage_id not in usage_db:
        usage_db[usage_id] = Usage(
            user_id=user_id,
            feature=feature,
            count=0,
            reset_at=user_sub.current_period_end,
        )
    
    usage = usage_db[usage_id]
    
    # Check limit
    remaining = limit - usage.count
    if remaining < count:
        return {
            "allowed": False,
            "remaining": remaining,
            "limit": limit,
        }
    
    # Increment usage
    usage.count += count
    
    return {
        "allowed": True,
        "remaining": remaining - count,
        "limit": limit,
    }


@app.get("/api/usage/{user_id}")
async def get_usage(user_id: str) -> dict:
    """Get user's usage statistics."""
    
    user_usage = {
        key: usage.count
        for key, usage in usage_db.items()
        if usage.user_id == user_id
    }
    
    # Get subscription and limits
    user_sub = None
    for sub in subscriptions_db.values():
        if sub.user_id == user_id:
            user_sub = sub
            break
    
    if not user_sub:
        return {"usage": user_usage, "subscription": None}
    
    plan = plans_db[user_sub.plan_id]
    
    return {
        "usage": user_usage,
        "limits": plan.features,
        "subscription": {
            "plan": plan.name,
            "status": user_sub.status,
        },
    }


@app.post("/api/usage/{user_id}/reset")
async def reset_usage(user_id: str) -> dict:
    """Reset usage for new billing period."""
    
    # Get user's subscription
    user_sub = None
    for sub in subscriptions_db.values():
        if sub.user_id == user_id:
            user_sub = sub
            break
    
    if not user_sub:
        raise HTTPException(status_code=404, detail="No subscription found")
    
    # Reset all usage for this user
    for usage in usage_db.values():
        if usage.user_id == user_id:
            usage.count = 0
            usage.reset_at = user_sub.current_period_end
    
    return {"success": True}
```

## Summary

This SaaS backend includes:
- Plan management with tiers
- Subscription creation and lifecycle
- Feature usage tracking
- Usage limits enforcement
- Usage reporting

## Next Steps

This concludes the guide. You now have a comprehensive understanding of Python web development from basics to advanced topics including Flask, FastAPI, databases, authentication, testing, deployment, API integrations, mobile backends, and complete project implementations.
