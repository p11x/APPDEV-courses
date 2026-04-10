# Canary Deployment for Migration

## Overview

Canary deployment is a strategy that gradually rolls out new microservices to a subset of users or traffic, allowing teams to validate the new service in production with minimal risk. The name comes from the mining practice of using canaries to detect dangerous conditions - in the same way, a small "canary" group of users tests the new service before full deployment.

This approach is particularly valuable during microservices migration because it enables teams to detect issues with the new architecture before affecting all users. Unlike blue-green deployment which requires two full environments, canary deployment can be done within a single environment by controlling traffic distribution.

## How Canary Deployment Works

The canary deployment process involves:

1. **Deploy New Version**: Deploy the new microservice version alongside the existing one
2. **Route Small Traffic**: Direct a small percentage of traffic to the new version
3. **Monitor Metrics**: Observe error rates, latency, and other key metrics
4. **Gradually Increase**: If metrics are healthy, gradually increase traffic
5. **Full Migration**: Once fully validated, migrate 100% to the new service
6. **Cleanup**: Remove the old version after successful migration

## Traffic Routing Strategies

### 1. Percentage-Based Routing

Route a specific percentage of traffic to the canary version:

```python
def route_request(user_id: str, canary_percentage: int) -> str:
    """Route based on user ID hash"""
    hash_value = hash(user_id) % 100
    if hash_value < canary_percentage:
        return "canary"
    return "stable"
```

### 2. User-Based Routing

Route specific users to the canary version:

```python
def route_request(user_id: str, target_users: List[str]) -> str:
    """Route specific users to canary"""
    if user_id in target_users:
        return "canary"
    return "stable"
```

### 3. Region-Based Routing

Route traffic from specific regions:

```python
def route_request(region: str, canary_regions: List[str]) -> str:
    """Route specific regions to canary"""
    if region in canary_regions:
        return "canary"
    return "stable"
```

## Implementation Example

```python
#!/usr/bin/env python3
"""
Canary Deployment Manager for Microservices Migration
Manages gradual traffic shifting during migration
"""

from dataclasses import dataclass, field
from typing import Dict, List, Optional, Callable
from datetime import datetime
from enum import Enum
import json
import time


class ServiceVersion(Enum):
    STABLE = "stable"
    CANARY = "canary"


@dataclass
class ServiceDeployment:
    """Represents a deployed service version"""
    version: str
    version_type: ServiceVersion
    replicas: int
    health_status: str = "unknown"
    deployed_at: datetime = field(default_factory=datetime.now)


@dataclass
class CanaryMetrics:
    """Metrics for canary evaluation"""
    request_count: int = 0
    error_count: int = 0
    avg_latency_ms: float = 0.0
    p99_latency_ms: float = 0.0
    success_rate: float = 100.0


@dataclass
class CanaryConfig:
    """Configuration for canary deployment"""
    service_name: str
    stable_version: str
    canary_version: str
    initial_traffic_percentage: int = 5
    increment_percentage: int = 5
    increment_interval_minutes: int = 10
    error_threshold: float = 1.0  # Max error rate %
    latency_threshold_ms: int = 500  # Max P99 latency


class CanaryDeploymentManager:
    """Manages canary deployments during migration"""
    
    def __init__(self):
        self.deployments: Dict[str, Dict[str, ServiceDeployment]] = {}
        self.canary_configs: Dict[str, CanaryConfig] = {}
        self.metrics: Dict[str, Dict[str, CanaryMetrics]] = {}
    
    def configure_canary(
        self,
        service_name: str,
        stable_version: str,
        canary_version: str,
        initial_traffic: int = 5
    ) -> CanaryConfig:
        """Configure canary deployment for a service"""
        
        config = CanaryConfig(
            service_name=service_name,
            stable_version=stable_version,
            canary_version=canary_version,
            initial_traffic_percentage=initial_traffic
        )
        
        self.canary_configs[service_name] = config
        
        # Initialize deployments
        self.deployments[service_name] = {
            ServiceVersion.STABLE.value: ServiceDeployment(
                version=stable_version,
                version_type=ServiceVersion.STABLE,
                replicas=3
            ),
            ServiceVersion.CANARY.value: ServiceDeployment(
                version=canary_version,
                version_type=ServiceVersion.CANARY,
                replicas=1
            )
        }
        
        # Initialize metrics
        self.metrics[service_name] = {
            ServiceVersion.STABLE.value: CanaryMetrics(),
            ServiceVersion.CANARY.value: CanaryMetrics()
        }
        
        return config
    
    def route_request(
        self,
        service_name: str,
        user_id: str
    ) -> str:
        """Route a request to stable or canary"""
        
        config = self.canary_configs.get(service_name)
        if not config:
            return ServiceVersion.STABLE.value
        
        # Get current traffic percentage
        traffic_percentage = self._get_current_traffic(service_name)
        
        # Use consistent hashing for user-based routing
        hash_value = hash(user_id) % 100
        
        if hash_value < traffic_percentage:
            return ServiceVersion.CANARY.value
        else:
            return ServiceVersion.STABLE.value
    
    def _get_current_traffic(self, service_name: str) -> int:
        """Get current canary traffic percentage"""
        # This would be stored in a config service or database
        return 10  # Default to 10%
    
    def record_metrics(
        self,
        service_name: str,
        version: str,
        latency_ms: int,
        is_error: bool
    ):
        """Record metrics for a service version"""
        
        if service_name not in self.metrics:
            self.metrics[service_name] = {}
        
        if version not in self.metrics[service_name]:
            self.metrics[service_name][version] = CanaryMetrics()
        
        metrics = self.metrics[service_name][version]
        metrics.request_count += 1
        
        if is_error:
            metrics.error_count += 1
        
        # Update latency metrics (simplified)
        metrics.avg_latency_ms = (
            (metrics.avg_latency_ms * (metrics.request_count - 1) + latency_ms) 
            / metrics.request_count
        )
        
        if latency_ms > metrics.p99_latency_ms:
            metrics.p99_latency_ms = latency_ms
        
        # Update success rate
        metrics.success_rate = (
            (metrics.request_count - metrics.error_count) 
            / metrics.request_count * 100
        )
    
    def evaluate_canary(self, service_name: str) -> Dict:
        """Evaluate if canary is healthy and should progress"""
        
        config = self.canary_configs.get(service_name)
        if not config:
            return {"error": "No canary configured"}
        
        stable_metrics = self.metrics[service_name].get(ServiceVersion.STABLE.value)
        canary_metrics = self.metrics[service_name].get(ServiceVersion.CANARY.value)
        
        if not stable_metrics or not canary_metrics:
            return {"status": "insufficient_data"}
        
        # Check error rate
        error_rate = (canary_metrics.error_count / canary_metrics.request_count * 100) if canary_metrics.request_count > 0 else 0
        
        # Check latency
        latency_ok = canary_metrics.p99_latency_ms <= config.latency_threshold_ms
        
        # Compare with stable
        stable_latency = stable_metrics.avg_latency_ms
        canary_latency = canary_metrics.avg_latency_ms
        latency_regression = canary_latency > stable_latency * 1.5  # 50% regression
        
        # Decision
        if error_rate > config.error_threshold:
            return {
                "status": "rollback",
                "reason": f"Error rate {error_rate:.2f}% exceeds threshold {config.error_threshold}%",
                "action": "Reduce canary traffic"
            }
        
        if not latency_ok:
            return {
                "status": "warning",
                "reason": f"P99 latency {canary_metrics.p99_latency_ms}ms exceeds threshold",
                "action": "Monitor closely"
            }
        
        if latency_regression:
            return {
                "status": "warning",
                "reason": f"Latency regression detected: {canary_latency:.0f}ms vs {stable_latency:.0f}ms",
                "action": "Investigate before progressing"
            }
        
        return {
            "status": "healthy",
            "reason": "Canary metrics within thresholds",
            "action": "Safe to increase traffic"
        }
    
    def increase_traffic(self, service_name: str) -> int:
        """Increase canary traffic percentage"""
        
        config = self.canary_configs.get(service_name)
        if not config:
            return 0
        
        current = self._get_current_traffic(service_name)
        new_percentage = min(100, current + config.increment_percentage)
        
        return new_percentage
    
    def promote_canary(self, service_name: str) -> bool:
        """Promote canary to stable (full migration)"""
        
        config = self.canary_configs.get(service_name)
        if not config:
            return False
        
        # Update deployment status
        if service_name in self.deployments:
            stable = self.deployments[service_name].get(ServiceVersion.STABLE.value)
            canary = self.deployments[service_name].get(ServiceVersion.CANARY.value)
            
            if canary:
                stable.version = canary.version
                stable.replicas += canary.replicas
        
        return True
    
    def get_canary_status(self, service_name: str) -> Dict:
        """Get detailed canary deployment status"""
        
        config = self.canary_configs.get(service_name)
        if not config:
            return {"error": "No canary configured"}
        
        stable_metrics = self.metrics.get(service_name, {}).get(ServiceVersion.STABLE.value)
        canary_metrics = self.metrics.get(service_name, {}).get(ServiceVersion.CANARY.value)
        
        current_traffic = self._get_current_traffic(service_name)
        
        return {
            "service": service_name,
            "stable_version": config.stable_version,
            "canary_version": config.canary_version,
            "current_canary_traffic": f"{current_traffic}%",
            "stable_metrics": {
                "requests": stable_metrics.request_count if stable_metrics else 0,
                "errors": stable_metrics.error_count if stable_metrics else 0,
                "success_rate": f"{stable_metrics.success_rate:.2f}%" if stable_metrics else "N/A",
                "avg_latency": f"{stable_metrics.avg_latency_ms:.0f}ms" if stable_metrics else "N/A"
            },
            "canary_metrics": {
                "requests": canary_metrics.request_count if canary_metrics else 0,
                "errors": canary_metrics.error_count if canary_metrics else 0,
                "success_rate": f"{canary_metrics.success_rate:.2f}%" if canary_metrics else "N/A",
                "avg_latency": f"{canary_metrics.avg_latency_ms:.0f}ms" if canary_metrics else "N/A"
            }
        }


# Example usage
if __name__ == "__main__":
    manager = CanaryDeploymentManager()
    
    # Configure canary deployment
    config = manager.configure_canary(
        service_name="order-service",
        stable_version="v1.0",
        canary_version="v2.0",
        initial_traffic=10
    )
    
    # Simulate some requests
    for i in range(100):
        user_id = f"user_{i:03d}"
        
        # Route request
        target = manager.route_request("order-service", user_id)
        
        # Simulate metrics (some errors, some latency)
        is_error = i % 50 == 0  # 2% error rate
        latency = 50 + (i % 100)  # 50-150ms
        
        manager.record_metrics("order-service", target, latency, is_error)
    
    # Evaluate canary
    evaluation = manager.evaluate_canary("order-service")
    print(f"Canary Evaluation: {evaluation}")
    
    # Get status
    status = manager.get_canary_status("order-service")
    print(f"\nCanary Status:")
    print(json.dumps(status, indent=2))
```

## Best Practices

1. **Start Small**: Begin with 1-5% traffic to limit blast radius of issues.

2. **Monitor Key Metrics**: Track error rates, latency, and business metrics.

3. **Use Consistent Hashing**: Ensure users see consistent behavior during migration.

4. **Define Clear Thresholds**: Establish clear error rate and latency thresholds for rollback.

5. **Automated Progression**: Automatically increase traffic when metrics are healthy.

6. **Extended Observation**: Keep canary running for sufficient time to catch issues.

---

## Output Statement

```
Canary Deployment Status
=========================
Service: order-service

Versions:
- Stable: v1.0 (90% traffic)
- Canary: v2.0 (10% traffic)

Metrics Comparison:
                    Stable        Canary
Requests:            9,000        1,000
Errors:              18           3
Success Rate:        99.80%       99.70%
Avg Latency:         45ms         52ms
P99 Latency:         120ms        180ms

Evaluation: HEALTHY
Recommendation: Increase canary to 15%

Actions:
- [Increase Traffic] - Progress to 15%
- [Hold] - Continue monitoring
- [Rollback] - Revert to v1.0
- [Promote] - Make v2.0 the stable version
```
