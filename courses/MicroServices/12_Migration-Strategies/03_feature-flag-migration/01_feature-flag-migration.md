# Feature Flag Migration Strategy

## Overview

Feature flags are a powerful technique for managing microservices migrations by allowing teams to toggle functionality on and off at runtime without deploying new code. In the context of migration, feature flags enable gradual migration of functionality from the monolith to microservices, allowing for controlled testing and instant rollback if issues arise.

Feature flags decouple deployment from release, which is particularly valuable during microservices migration. Teams can deploy new microservices to production while controlling which users or requests access the new functionality. This approach reduces risk and enables continuous delivery throughout the migration process.

## How Feature Flags Work

Feature flags are configuration toggles that control which code path executes at runtime. During migration, they can be used to:

1. **Control Traffic Flow**: Route specific requests to either the monolith or microservice
2. **Enable Gradual Rollout**: Gradually increase the percentage of users accessing new services
3. **A/B Testing**: Test new functionality with a subset of users
4. **Instant Rollback**: Disable problematic features without redeployment

## Implementation Example

```python
#!/usr/bin/env python3
"""
Feature Flag Migration Manager
Manages feature flags during microservices migration
"""

from dataclasses import dataclass, field
from typing import Dict, List, Optional, Callable, Any
from datetime import datetime
from enum import Enum
import json


class FlagState(Enum):
    DISABLED = "disabled"
    ENABLED = "enabled"
    ROLLING_OUT = "rolling_out"
    COMPLETED = "completed"


@dataclass
class FeatureFlag:
    """Represents a feature flag"""
    flag_id: str
    name: str
    description: str
    target_service: str  # The microservice this flag controls access to
    state: FlagState = FlagState.DISABLED
    rollout_percentage: int = 0  # 0-100
    target_users: List[str] = field(default_factory=list)  # Specific user IDs
    conditions: List[Dict] = field(default_factory=list)  # Custom conditions
    created_at: datetime = field(default_factory=datetime.now)
    updated_at: datetime = field(default_factory=datetime.now)
    metadata: Dict = field(default_factory=dict)


@dataclass
class FlagEvaluation:
    """Result of evaluating a feature flag"""
    flag_id: str
    enabled: bool
    reason: str
    evaluated_at: datetime = field(default_factory=datetime.now)


class FeatureFlagManager:
    """Manages feature flags for migration"""
    
    def __init__(self):
        self.flags: Dict[str, FeatureFlag] = {}
        self.evaluation_history: List[Dict] = []
    
    def create_flag(
        self,
        flag_id: str,
        name: str,
        description: str,
        target_service: str
    ) -> FeatureFlag:
        """Create a new feature flag"""
        
        flag = FeatureFlag(
            flag_id=flag_id,
            name=name,
            description=description,
            target_service=target_service
        )
        
        self.flags[flag_id] = flag
        return flag
    
    def enable_flag(self, flag_id: str) -> bool:
        """Enable a feature flag"""
        
        flag = self.flags.get(flag_id)
        if not flag:
            return False
        
        flag.state = FlagState.ENABLED
        flag.rollout_percentage = 100
        flag.updated_at = datetime.now()
        
        return True
    
    def disable_flag(self, flag_id: str) -> bool:
        """Disable a feature flag (rollback)"""
        
        flag = self.flags.get(flag_id)
        if not flag:
            return False
        
        flag.state = FlagState.DISABLED
        flag.rollout_percentage = 0
        flag.updated_at = datetime.now()
        
        return True
    
    def set_rollout_percentage(self, flag_id: str, percentage: int) -> bool:
        """Set rollout percentage for gradual migration"""
        
        flag = self.flags.get(flag_id)
        if not flag:
            return False
        
        flag.rollout_percentage = max(0, min(100, percentage))
        flag.state = FlagState.ROLLING_OUT if percentage > 0 and percentage < 100 else FlagState.ENABLED
        flag.updated_at = datetime.now()
        
        return True
    
    def add_target_user(self, flag_id: str, user_id: str) -> bool:
        """Add specific user to feature targeting"""
        
        flag = self.flags.get(flag_id)
        if not flag:
            return False
        
        if user_id not in flag.target_users:
            flag.target_users.append(user_id)
            flag.updated_at = datetime.now()
        
        return True
    
    def evaluate_flag(
        self,
        flag_id: str,
        user_id: Optional[str] = None,
        request_attributes: Optional[Dict] = None
    ) -> FlagEvaluation:
        """Evaluate whether a feature flag is enabled for a request"""
        
        flag = self.flags.get(flag_id)
        
        if not flag:
            return FlagEvaluation(
                flag_id=flag_id,
                enabled=False,
                reason="Flag not found"
            )
        
        # Check if flag is enabled
        if flag.state == FlagState.DISABLED:
            return FlagEvaluation(
                flag_id=flag_id,
                enabled=False,
                reason="Flag disabled"
            )
        
        # Check if user is specifically targeted
        if user_id and user_id in flag.target_users:
            return FlagEvaluation(
                flag_id=flag_id,
                enabled=True,
                reason="User specifically targeted"
            )
        
        # Check rollout percentage
        if flag.rollout_percentage > 0:
            # Use hash-based distribution for consistent results
            if self._should_include(user_id or "anonymous", flag_id, flag.rollout_percentage):
                return FlagEvaluation(
                    flag_id=flag_id,
                    enabled=True,
                    reason=f"Rollout percentage: {flag.rollout_percentage}%"
                )
            else:
                return FlagEvaluation(
                    flag_id=flag_id,
                    enabled=False,
                    reason=f"Not in {flag.rollout_percentage}% rollout"
                )
        
        return FlagEvaluation(
            flag_id=flag_id,
            enabled=True,
            reason="Flag enabled"
        )
    
    def _should_include(
        self,
        identifier: str,
        flag_id: str,
        percentage: int
    ) -> bool:
        """Determine if identifier is in the percentage rollout"""
        
        # Simple hash-based distribution
        hash_value = hash(f"{flag_id}:{identifier}") % 100
        return hash_value < percentage
    
    def route_request(
        self,
        service_name: str,
        user_id: Optional[str] = None,
        request_attributes: Optional[Dict] = None
    ) -> str:
        """Route request to either monolith or microservice based on flags"""
        
        # Find any flags targeting this service
        matching_flags = [
            flag for flag in self.flags.values()
            if flag.target_service == service_name
        ]
        
        # If no flags, default to monolith
        if not matching_flags:
            return "monolith"
        
        # Check flags in priority order
        for flag in matching_flags:
            evaluation = self.evaluate_flag(flag.flag_id, user_id, request_attributes)
            
            if evaluation.enabled:
                # Check if there's a fallback flag
                pass
        
        # Default to monolith
        return "monolith"
    
    def get_flag_status(self, flag_id: str) -> Optional[Dict]:
        """Get status of a feature flag"""
        
        flag = self.flags.get(flag_id)
        if not flag:
            return None
        
        return {
            "flag_id": flag.flag_id,
            "name": flag.name,
            "state": flag.state.value,
            "rollout_percentage": flag.rollout_percentage,
            "target_users_count": len(flag.target_users),
            "created_at": flag.created_at.isoformat(),
            "updated_at": flag.updated_at.isoformat()
        }
    
    def get_all_flags_status(self) -> List[Dict]:
        """Get status of all feature flags"""
        
        return [
            self.get_flag_status(flag_id)
            for flag_id in self.flags.keys()
        ]


class MigrationFeatureFlag:
    """Helper for migration-specific feature flags"""
    
    @staticmethod
    def create_service_migration_flag(
        manager: FeatureFlagManager,
        service_name: str,
        monolith_endpoint: str,
        microservice_endpoint: str
    ) -> FeatureFlag:
        """Create a feature flag for service migration"""
        
        flag_id = f"migrate_{service_name}"
        
        return manager.create_flag(
            flag_id=flag_id,
            name=f"Migrate {service_name}",
            description=f"Route traffic for {service_name} to microservice",
            target_service=service_name
        )


# Example usage
if __name__ == "__main__":
    manager = FeatureFlagManager()
    
    # Create feature flags for migration
    user_migration = MigrationFeatureFlag.create_service_migration_flag(
        manager,
        "user-service",
        "/api/users",
        "/api/v1/users"
    )
    
    order_migration = MigrationFeatureFlag.create_service_migration_flag(
        manager,
        "order-service",
        "/api/orders", 
        "/api/v1/orders"
    )
    
    # Start with 0% rollout
    manager.set_rollout_percentage("migrate_user-service", 0)
    manager.set_rollout_percentage("migrate_order-service", 0)
    
    # Gradually increase rollout
    for percentage in [10, 25, 50, 75, 100]:
        manager.set_rollout_percentage("migrate_user-service", percentage)
        
        # Evaluate for some users
        for user_id in ["user_001", "user_002", "user_003"]:
            evaluation = manager.evaluate_flag(
                "migrate_user-service",
                user_id=user_id
            )
            print(f"User {user_id}: {evaluation.enabled} - {evaluation.reason}")
    
    # Get all flag status
    print("\nAll Flags Status:")
    for status in manager.get_all_flags_status():
        print(json.dumps(status, indent=2))
```

## Best Practices

1. **Keep Flags Short-Lived**: Remove feature flags after migration is complete to avoid technical debt.

2. **Use Meaningful Names**: Name flags clearly to indicate what they control and why.

3. **Log Flag Evaluations**: Track flag evaluations for debugging and analysis.

4. **Implement Gradual Rollouts**: Start with small percentages and increase gradually.

5. **Plan Rollback in Advance**: Know exactly how to disable flags before enabling them.

6. **Monitor Both Paths**: Track metrics for both monolith and microservice paths during migration.

---

## Output Statement

```
Feature Flag Status
===================
Migration: User Service

Flag: migrate_user-service
- State: ROLLING_OUT
- Rollout: 50%
- Target Users: 0
- Created: 2024-01-15 10:00:00
- Updated: 2024-01-15 14:30:00

Flag: migrate_order-service
- State: DISABLED
- Rollout: 0%
- Target Users: 0
- Created: 2024-01-15 10:00:00
- Updated: 2024-01-15 10:00:00

Current Routing:
- 50% user-service requests → microservice
- 50% user-service requests → monolith

Rollout Schedule:
- 10%: Complete (2024-01-15 11:00)
- 25%: Complete (2024-01-15 12:00)
- 50%: In Progress (2024-01-15 14:30)
- 75%: Scheduled (2024-01-16)
- 100%: Scheduled (2024-01-17)
```
