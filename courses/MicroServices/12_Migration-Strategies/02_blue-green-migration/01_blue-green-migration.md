# Blue-Green Migration Strategy

## Overview

Blue-green migration is a deployment strategy that runs two identical production environments simultaneously. One environment (blue) serves the current production traffic while the new environment (green) is prepared with the updated microservices. Once the green environment is validated, traffic is switched to make it the active environment.

This approach provides instant rollback capability - if issues are detected after the switch, traffic can be immediately redirected back to the blue environment. Blue-green deployment is particularly valuable during microservices migration because it allows teams to validate the complete system behavior before exposing users to the new architecture.

## How It Works

The blue-green strategy involves maintaining two identical infrastructure environments:

1. **Blue Environment**: The current production environment running the monolith or existing microservices
2. **Green Environment**: The new environment being prepared with migrated services

The migration process follows these steps:

1. Provision a green environment identical to blue
2. Deploy migrated microservices to the green environment
3. Run both environments in parallel
4. Validate green environment functionality
5. Switch traffic from blue to green
6. Keep blue environment on standby for rollback
7. Decommission blue after validation period

## Implementation Example

```python
#!/usr/bin/env python3
"""
Blue-Green Migration Manager
Manages blue-green deployment for microservices migration
"""

from dataclasses import dataclass, field
from typing import Dict, List, Optional
from datetime import datetime
from enum import Enum
import json


class EnvironmentType(Enum):
    BLUE = "blue"
    GREEN = "green"


class TrafficStatus(Enum):
    IDLE = "idle"
    MIGRATING = "migrating"
    ACTIVE = "active"
    ROLLBACK = "rollback"


@dataclass
class Environment:
    """Represents a blue or green environment"""
    env_id: str
    env_type: EnvironmentType
    services: Dict[str, str] = field(default_factory=dict)  # service -> version
    health_status: Dict[str, bool] = field(default_factory=dict)
    created_at: datetime = field(default_factory=datetime.now)
    active: bool = False


@dataclass
class MigrationState:
    """Tracks the current migration state"""
    blue_env: Optional[Environment] = None
    green_env: Optional[Environment] = None
    traffic_status: TrafficStatus = TrafficStatus.IDLE
    traffic_percentage: int = 0
    last_switch: Optional[datetime] = None


class BlueGreenMigrationManager:
    """Manages blue-green migration for microservices"""
    
    def __init__(self):
        self.state = MigrationState()
        self.migration_history: List[Dict] = []
    
    def setup_environment(
        self,
        env_type: EnvironmentType,
        services: Dict[str, str]
    ) -> Environment:
        """Set up a new environment"""
        
        env = Environment(
            env_id=f"{env_type.value}_{datetime.now().strftime('%Y%m%d_%H%M%S')}",
            env_type=env_type,
            services=services
        )
        
        if env_type == EnvironmentType.BLUE:
            self.state.blue_env = env
        else:
            self.state.green_env = env
        
        # Initialize health status
        for service in services:
            env.health_status[service] = False
        
        return env
    
    def validate_environment(self, env_type: EnvironmentType) -> Dict:
        """Validate environment health"""
        
        env = self._get_environment(env_type)
        if not env:
            return {"error": f"Environment {env_type.value} not found"}
        
        # Simulate health checks
        all_healthy = True
        for service in env.services:
            # In production, would actually check service health
            env.health_status[service] = True
            if not env.health_status[service]:
                all_healthy = False
        
        return {
            "env_id": env.env_id,
            "healthy": all_healthy,
            "services": env.health_status
        }
    
    def switch_traffic(
        self,
        percentage: int,
        direction: str  # "blue_to_green" or "green_to_blue"
    ) -> bool:
        """Switch traffic between environments"""
        
        if direction == "blue_to_green":
            if self.state.green_env:
                self.state.green_env.active = True
                if self.state.blue_env:
                    self.state.blue_env.active = False
                self.state.traffic_percentage = percentage
                self.state.traffic_status = TrafficStatus.ACTIVE
                self.state.last_switch = datetime.now()
        
        elif direction == "green_to_blue":
            if self.state.blue_env:
                self.state.blue_env.active = True
                if self.state.green_env:
                    self.state.green_env.active = False
                self.state.traffic_percentage = 100 - percentage
                self.state.traffic_status = TrafficStatus.ROLLBACK
                self.state.last_switch = datetime.now()
        
        self._record_migration_event("traffic_switch", {
            "direction": direction,
            "percentage": percentage
        })
        
        return True
    
    def rollback(self) -> bool:
        """Rollback to previous environment"""
        
        if self.state.green_env and self.state.green_env.active:
            return self.switch_traffic(100, "green_to_blue")
        
        return False
    
    def _get_environment(self, env_type: EnvironmentType) -> Optional[Environment]:
        """Get environment by type"""
        
        if env_type == EnvironmentType.BLUE:
            return self.state.blue_env
        else:
            return self.state.green_env
    
    def _record_migration_event(self, event_type: str, details: Dict):
        """Record migration event"""
        
        self.migration_history.append({
            "event": event_type,
            "details": details,
            "timestamp": datetime.now().isoformat()
        })
    
    def get_migration_status(self) -> Dict:
        """Get current migration status"""
        
        return {
            "blue_env": {
                "id": self.state.blue_env.env_id if self.state.blue_env else None,
                "active": self.state.blue_env.active if self.state.blue_env else False,
                "services": self.state.blue_env.services if self.state.blue_env else {}
            },
            "green_env": {
                "id": self.state.green_env.env_id if self.state.green_env else None,
                "active": self.state.green_env.active if self.state.green_env else False,
                "services": self.state.green_env.services if self.state.green_env else {}
            },
            "traffic_status": self.state.traffic_status.value,
            "traffic_percentage": self.state.traffic_percentage,
            "last_switch": self.state.last_switch.isoformat() if self.state.last_switch else None
        }


# Example usage
if __name__ == "__main__":
    manager = BlueGreenMigrationManager()
    
    # Set up blue environment (current production)
    blue_services = {
        "user-service": "v1.0",
        "order-service": "v1.0",
        "catalog-service": "v1.0"
    }
    manager.setup_environment(EnvironmentType.BLUE, blue_services)
    
    # Set up green environment (new microservices)
    green_services = {
        "user-service": "v2.0",
        "order-service": "v2.0",
        "catalog-service": "v2.0"
    }
    manager.setup_environment(EnvironmentType.GREEN, green_services)
    
    # Validate green environment
    validation = manager.validate_environment(EnvironmentType.GREEN)
    print(f"Green environment validation: {validation}")
    
    # Switch traffic (canary - 10%)
    manager.switch_traffic(10, "blue_to_green")
    
    # Get status
    status = manager.get_migration_status()
    print(f"\nMigration Status:")
    print(json.dumps(status, indent=2))
    
    # If issues, rollback
    # manager.rollback()
```

## Best Practices

1. **Maintain Identical Environments**: Both blue and green should have identical infrastructure configurations to ensure consistent behavior.

2. **Database Considerations**: Use database replication or careful schema management to handle data between environments.

3. **Gradual Traffic Shifting**: Start with a small percentage of traffic to validate the new environment.

4. **Extended Validation Period**: Keep both environments running for a period after switching to enable quick rollback.

5. **Automated Health Checks**: Implement automated health monitoring to detect issues early.

6. **Session Management**: Ensure session state is properly handled during traffic switches.

---

## Output Statement

```
Blue-Green Migration Status
============================
Environment: Production
Migration Phase: Active

Blue Environment:
- ID: blue_20240115_100000
- Status: Standby
- Services: user-service v1.0, order-service v1.0, catalog-service v1.0

Green Environment:
- ID: green_20240115_140000
- Status: Active (10% traffic)
- Services: user-service v2.0, order-service v2.0, catalog-service v2.0

Traffic: 10% → green, 90% → blue

Last Switch: 2024-01-15 14:30:00

Actions Available:
- Increase traffic to green
- Full switch to green
- Rollback to blue
```
