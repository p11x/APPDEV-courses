# Strangler Fig Architecture

## What You'll Learn

- Modernizing legacy systems
- Incremental migration
- Feature flags and canary releases
- Running old and new systems side by side

## Prerequisites

- Understanding of microservices
- Basic knowledge of deployment strategies

## Introduction

The Strangler Fig pattern gets its name from a tree that starts as a seed in the canopy of another tree, grows roots down to the ground, and gradually strangles and replaces the host tree. In software, this means gradually replacing a legacy system with a modern one, one piece at a time.

Think of renovating a house while someone still lives in it. You don't tear down the whole house at once—you replace one room, then another, while the family continues living there. This is much less risky than a big-bang rewrite.

## The Pattern

```
Before Strangler:
┌─────────────────────────────────────────┐
│           Legacy Monolith               │
│  ┌─────────┐ ┌─────────┐ ┌──────────┐  │
│  │  Users  │ │ Orders  │ │ Products │  │
│  └────┬────┘ └────┬────┘ └────┬─────┘  │
│       └───────────┴───────────┘         │
│                   │                     │
│              Database                   │
└─────────────────────────────────────────┘

During Strangler:
┌─────────────────────────────────────────┐
│              API Gateway                │
│         (Route traffic intelligently)   │
└────────────┬─────────────────┬─────────┘
             │                 │
             ▼                 ▼
┌──────────────────┐  ┌──────────────────┐
│  Legacy System   │  │  New Microservice │
│  ┌─────────┐    │  │  ┌─────────┐      │
│  │ Products│    │  │  │ Orders  │      │
│  └────┬────┘    │  │  └────┬────┘      │
│       └─────────┘  │       └───────────┘
│         Database   │         Database
└──────────────────┘  └──────────────────┘

After Strangler:
┌─────────────────────────────────────────┐
│              API Gateway                │
└────────────┬─────────────────┬─────────┘
             │                 │
             ▼                 ▼
┌──────────────────┐  ┌──────────────────┐
│  Microservice A  │  │  Microservice B  │
│  ┌─────────┐    │  │  ┌─────────┐     │
│  │  Users  │    │  │  │ Orders  │     │
│  └─────────┘    │  │  └─────────┘     │
└──────────────────┘  └──────────────────┘
```

## Implementation Strategies

### 1. Facade Pattern

```python
from fastapi import FastAPI, HTTPException
from typing import Optional
import httpx

app = FastAPI(title="API Gateway")

# Configuration for services
LEGACY_API_URL = "http://legacy-system:8000"
NEW_ORDERS_SERVICE_URL = "http://orders-service:8001"
NEW_PRODUCTS_SERVICE_URL = "http://products-service:8002"

# Feature flags
FEATURE_FLAGS = {
    "use_new_orders": True,
    "use_new_products": False,
    "use_new_users": True
}

class ProxyRouter:
    """Routes requests to legacy or new services based on feature flags."""
    
    async def route_request(
        self,
        path: str,
        method: str,
        feature_flag: str,
        legacy_path: str,
        **kwargs
    ):
        if FEATURE_FLAGS.get(feature_flag, False):
            # Route to new service
            new_url = f"{self._get_service_url(path)}{legacy_path}"
            return await self._forward_request(new_url, method, **kwargs)
        else:
            # Route to legacy service
            legacy_url = f"{LEGACY_API_URL}{legacy_path}"
            return await self._forward_request(legacy_url, method, **kwargs)
    
    def _get_service_url(self, path: str) -> str:
        if "/orders" in path:
            return NEW_ORDERS_SERVICE_URL
        elif "/products" in path:
            return NEW_PRODUCTS_SERVICE_URL
        return LEGACY_API_URL
    
    async def _forward_request(
        self,
        url: str,
        method: str,
        **kwargs
    ) -> dict:
        async with httpx.AsyncClient() as client:
            response = await client.request(method, url, **kwargs)
            return response.json()

router = ProxyRouter()

# Example routes
@app.api_route("/orders/{path:path}", methods=["GET", "POST", "PUT", "DELETE"])
async def proxy_orders(path: str):
    return await router.route_request(
        path=f"/orders/{path}",
        method="GET",  # Would extract from request
        feature_flag="use_new_orders",
        legacy_path=f"/api/orders/{path}"
    )

@app.api_route("/products/{path:path}", methods=["GET", "POST", "PUT", "DELETE"])
async def proxy_products(path: str):
    return await router.route_request(
        path=f"/products/{path}",
        method="GET",
        feature_flag="use_new_products",
        legacy_path=f"/api/products/{path}"
    )

@app.api_route("/users/{path:path}", methods=["GET", "POST", "PUT", "DELETE"])
async def proxy_users(path: str):
    return await router.route_request(
        path=f"/users/{path}",
        method="GET",
        feature_flag="use_new_users",
        legacy_path=f"/api/users/{path}"
    )
```

### 2. Change Data Capture

```python
import asyncio
from dataclasses import dataclass
from datetime import datetime
from typing import Callable

@dataclass
class DatabaseEvent:
    table: str
    operation: str  # INSERT, UPDATE, DELETE
    old_data: dict | None
    new_data: dict | None
    timestamp: datetime

class ChangeDataCapture:
    """Captures changes from legacy database and syncs to new system."""
    
    def __init__(self, event_handler: Callable):
        self._event_handler = event_handler
        self._running = False
    
    async def start_capturing(self):
        """Start monitoring the legacy database for changes."""
        self._running = True
        while self._running:
            # Poll for changes (simplified - real impl would use CDC tools)
            events = await self._poll_for_changes()
            for event in events:
                await self._process_event(event)
            await asyncio.sleep(1)
    
    async def _poll_for_changes(self) -> list[DatabaseEvent]:
        # In production, use Debezium, AWS DMS, or similar
        # This is a simplified example
        return []
    
    async def _process_event(self, event: DatabaseEvent):
        # Handle different event types
        match event.table:
            case "orders":
                await self._handle_order_change(event)
            case "products":
                await self._handle_product_change(event)
            case "users":
                await self._handle_user_change(event)
    
    async def _handle_order_change(self, event: DatabaseEvent):
        match event.operation:
            case "INSERT" | "UPDATE":
                await self._event_handler("order_updated", event.new_data)
            case "DELETE":
                await self._event_handler("order_deleted", event.old_data)
    
    async def _handle_product_change(self, event: DatabaseEvent):
        match event.operation:
            case "INSERT" | "UPDATE":
                await self._event_handler("product_updated", event.new_data)
            case "DELETE":
                await self._event_handler("product_deleted", event.old_data)
    
    async def _handle_user_change(self, event: DatabaseEvent):
        match event.operation:
            case "INSERT" | "UPDATE":
                await self._event_handler("user_updated", event.new_data)
            case "DELETE":
                await self._event_handler("user_deleted", event.old_data)
    
    def stop(self):
        self._running = False
```

### 3. Feature Flags

```python
from dataclasses import dataclass
from typing import Optional
import random

@dataclass
class FeatureFlag:
    name: str
    enabled: bool
    rollout_percentage: float = 100.0  # 0-100

class FeatureFlagService:
    """Manages feature flags for gradual rollout."""
    
    def __init__(self):
        self._flags: dict[str, FeatureFlag] = {}
    
    def add_flag(self, name: str, enabled: bool = False, rollout: float = 100.0):
        self._flags[name] = FeatureFlag(name, enabled, rollout)
    
    def enable(self, name: str):
        if name in self._flags:
            self._flags[name].enabled = True
    
    def disable(self, name: str):
        if name in self._flags:
            self._flags[name].enabled = False
    
    def is_enabled(self, name: str, user_id: Optional[str] = None) -> bool:
        """Check if feature is enabled for a user."""
        flag = self._flags.get(name)
        if not flag:
            return False
        
        if not flag.enabled:
            return False
        
        # Rollout percentage check
        if flag.rollout_percentage < 100.0 and user_id:
            # Consistent hashing based on user_id
            user_hash = hash(user_id) % 100
            return user_hash < flag.rollout_percentage
        
        return True
    
    def get_enabled_features(self, user_id: Optional[str] = None) -> list[str]:
        return [
            name for name, flag in self._flags.items()
            if self.is_enabled(name, user_id)
        ]

# Usage
flags = FeatureFlagService()
flags.add_flag("new_checkout", enabled=True, rollout=10)
flags.add_flag("new_recommendations", enabled=True, rollout=50)

# Check in code
if flags.is_enabled("new_checkout", user_id="user123"):
    # Use new checkout flow
    pass
else:
    # Use legacy checkout
    pass
```

### 4. Decorator-Based Feature Flags

```python
from functools import wraps
from typing import Callable

def feature_flag(flag_name: str, default: bool = False):
    """Decorator to conditionally run code based on feature flag."""
    def decorator(func: Callable):
        @wraps(func)
        async def async_wrapper(*args, **kwargs):
            # Get flag service from context
            flag_service = kwargs.pop('_flag_service', None)
            
            if flag_service and flag_service.is_enabled(flag_name):
                return await func(*args, **kwargs)
            elif not flag_service and default:
                return await func(*args, **kwargs)
            
            # Return original behavior or raise
            raise NotImplementedError(f"Feature {flag_name} not enabled")
        
        @wraps(func)
        def sync_wrapper(*args, **kwargs):
            flag_service = kwargs.pop('_flag_service', None)
            
            if flag_service and flag_service.is_enabled(flag_name):
                return func(*args, **kwargs)
            elif not flag_service and default:
                return func(*args, **kwargs)
            
            raise NotImplementedError(f"Feature {flag_name} not enabled")
        
        # Return appropriate wrapper
        import asyncio
        if asyncio.iscoroutinefunction(func):
            return async_wrapper
        return sync_wrapper
    return decorator

# Usage
class OrderService:
    def __init__(self, flag_service: FeatureFlagService):
        self._flags = flag_service
    
    @feature_flag("new_pricing_engine")
    async def calculate_price(self, items: list[dict]) -> float:
        # New pricing logic
        return sum(item["price"] * 1.1 for item in items)
    
    @feature_flag("new_pricing_engine", default=True)
    def calculate_price_legacy(self, items: list[dict]) -> float:
        # Old pricing logic
        return sum(item["price"] for item in items)
```

## Migration Strategy Steps

1. **Identify Boundaries** - Find natural service boundaries in the monolith
2. **Set Up Gateway** - Create an API gateway to route traffic
3. **Extract First Service** - Choose the least risky service to extract first
4. **Implement Feature Flags** - Enable gradual rollout
5. **Run in Parallel** - Run old and new side by side
6. **Verify and Switch** - Confirm new service works, switch traffic completely
7. **Repeat** - Continue with next service
8. **Decommission** - Remove legacy code once all services are migrated

## Summary

- Strangler Fig pattern enables incremental migration from legacy systems
- Feature flags allow gradual rollout with easy rollback
- API Gateway routes traffic between old and new systems
- Change Data Capture keeps systems in sync during migration
- This approach reduces risk compared to big-bang rewrites

## Next Steps

Continue to the next folder in your learning journey, or revisit earlier topics to deepen your understanding.
