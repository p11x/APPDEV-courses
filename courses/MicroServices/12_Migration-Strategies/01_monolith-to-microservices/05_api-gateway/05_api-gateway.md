# API Gateway Configuration for Migration

## Overview

During the migration from monolith to microservices, an API gateway plays a crucial role in managing the transition. The API gateway serves as the single entry point for all client requests, routing them either to the monolith or to the newly created microservices based on the migration status. This allows for a gradual, controlled migration where traffic can be shifted incrementally from the monolith to microservices.

The API gateway during migration must handle several key responsibilities: routing requests to the appropriate backend (monolith or microservice), maintaining backward compatibility, enabling canary releases for testing new services, aggregating responses when services are partially migrated, and providing observability into migration progress.

A well-configured migration API gateway enables the "strangler fig" pattern by allowing new microservices to handle specific routes while the monolith continues to serve other requests. This pattern reduces risk by enabling immediate rollback if issues are detected in the new services.

## Key Responsibilities

### 1. Traffic Routing

The gateway must route requests based on configured rules that can be updated dynamically. Routes may be based on URL paths, HTTP headers, query parameters, or client characteristics. During migration, routes are gradually shifted from pointing to the monolith to pointing to microservices.

### 2. Version Management

As microservices are extracted, they may use different versions of APIs than the monolith. The gateway must handle version negotiation and translation to ensure backward compatibility for existing clients.

### 3. Circuit Breaking

During migration, services may be unstable. The gateway should implement circuit breaking to prevent cascading failures and enable graceful degradation.

## Implementation Example

```python
#!/usr/bin/env python3
"""
Migration API Gateway
Routes traffic between monolith and microservices during migration
"""

from dataclasses import dataclass, field
from typing import Dict, List, Optional, Callable
from enum import Enum
import time
import logging

logger = logging.getLogger(__name__)


class RouteTarget(Enum):
    MONOLITH = "monolith"
    MICROSERVICE = "microservice"


@dataclass
class Route:
    """Defines a routing rule"""
    path_pattern: str
    target: RouteTarget
    service_name: Optional[str] = None
    weight: int = 100  # 0-100 for percentage-based routing


@dataclass
class MigrationRoute:
    """Migration-specific route with traffic shifting"""
    route_id: str
    monolith_endpoint: str
    microservice_endpoint: str
    traffic_percentage: int = 0
    health_check_passed: bool = True


class MigrationAPIGateway:
    """API Gateway for managing migration traffic"""
    
    def __init__(self, monolith_base_url: str):
        self.monolith_base_url = monolith_base_url
        self.routes: List[Route] = []
        self.migration_routes: Dict[str, MigrationRoute] = {}
        self.circuit_breakers: Dict[str, Dict] = {}
    
    def add_route(
        self,
        path_pattern: str,
        target: RouteTarget,
        service_name: Optional[str] = None
    ):
        """Add a routing rule"""
        route = Route(
            path_pattern=path_pattern,
            target=target,
            service_name=service_name
        )
        self.routes.append(route)
        logger.info(f"Added route: {path_pattern} -> {target.value}")
    
    def setup_migration_route(
        self,
        route_id: str,
        path_pattern: str,
        monolith_endpoint: str,
        microservice_endpoint: str
    ):
        """Set up a route for gradual migration"""
        
        # Add initial route to monolith
        self.add_route(path_pattern, RouteTarget.MONOLITH)
        
        # Create migration route
        migration_route = MigrationRoute(
            route_id=route_id,
            monolith_endpoint=monolith_endpoint,
            microservice_endpoint=microservice_endpoint
        )
        
        self.migration_routes[route_id] = migration_route
        logger.info(f"Setup migration route: {route_id}")
    
    def shift_traffic(
        self,
        route_id: str,
        percentage: int
    ):
        """Shift percentage of traffic to microservice"""
        
        migration_route = self.migration_routes.get(route_id)
        if not migration_route:
            raise ValueError(f"Migration route {route_id} not found")
        
        old_percentage = migration_route.traffic_percentage
        migration_route.traffic_percentage = percentage
        
        # Update route weights
        if percentage > 0:
            # Find and update the route
            for route in self.routes:
                if route.route_id == route_id:
                    route.weight = percentage
        
        logger.info(
            f"Traffic shift for {route_id}: "
            f"{old_percentage}% -> {percentage}%"
        )
    
    def route_request(self, path: str, headers: Dict) -> str:
        """Determine the target URL for a request"""
        
        # Check migration routes first
        for route_id, migration_route in self.migration_routes.items():
            if self._matches_pattern(path, migration_route.monolith_endpoint):
                # Use weighted routing if traffic is partially shifted
                if 0 < migration_route.traffic_percentage < 100:
                    return self._weighted_route(
                        path,
                        migration_route,
                        headers
                    )
                elif migration_route.traffic_percentage == 100:
                    return migration_route.microservice_endpoint
        
        # Fall back to monolith
        return f"{self.monolith_base_url}{path}"
    
    def _matches_pattern(self, path: str, pattern: str) -> bool:
        """Simple pattern matching"""
        return path.startswith(pattern.split('?')[0])
    
    def _weighted_route(
        self,
        path: str,
        migration_route: MigrationRoute,
        headers: Dict
    ) -> str:
        """Route based on percentage weight"""
        
        import random
        threshold = migration_route.traffic_percentage
        
        if random.randint(1, 100) <= threshold:
            return migration_route.microservice_endpoint
        else:
            return f"{self.monolith_base_url}{path}"


# Example usage
if __name__ == "__main__":
    gateway = MigrationAPIGateway("http://monolith:8080")
    
    # Setup migration routes
    gateway.setup_migration_route(
        route_id="users",
        path_pattern="/api/users",
        monolith_endpoint="/api/users",
        microservice_endpoint="http://user-service:8081/api/v1"
    )
    
    gateway.setup_migration_route(
        route_id="orders",
        path_pattern="/api/orders", 
        monolith_endpoint="/api/orders",
        microservice_endpoint="http://order-service:8082/api/v1"
    )
    
    # Gradually shift traffic
    for percentage in [10, 25, 50, 75, 100]:
        gateway.shift_traffic("users", percentage)
        print(f"Shifted user traffic to {percentage}%")
    
    # Route some requests
    for i in range(5):
        target = gateway.route_request("/api/users/123", {})
        print(f"Request {i+1} -> {target}")
