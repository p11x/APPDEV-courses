# Performance Tuning for Microservices

## Overview

Performance tuning involves optimizing various aspects of microservices to achieve better throughput, lower latency, and more efficient resource utilization. This guide covers practical tuning techniques for different layers of the microservices stack, including application, database, network, and infrastructure levels.

Effective performance tuning requires understanding the system's bottleneck and applying targeted optimizations. Premature optimization without proper measurement can lead to wasted effort and added complexity.

## Tuning Areas

### 1. Application-Level Tuning

```python
# Example: Async request handling
import asyncio
import aiohttp

class AsyncServiceClient:
    """Optimized async HTTP client"""
    
    def __init__(self, max_connections: int = 100):
        self.connector = aiohttp.TCPConnector(
            limit=max_connections,
            limit_per_host=20,
            keepalive_timeout=30
        )
        self.session = aiohttp.ClientSession(connector=self.connector)
    
    async def fetch_all(self, urls: List[str]) -> List[dict]:
        """Fetch multiple URLs concurrently"""
        async with aiohttp.ClientSession() as session:
            tasks = [self._fetch(session, url) for url in urls]
            return await asyncio.gather(*tasks, return_exceptions=True)
```

### 2. Database Tuning

```sql
-- Example: Optimized query with proper indexing
-- Before: Full table scan
SELECT * FROM orders WHERE user_id = 123 
AND created_at > '2024-01-01' ORDER BY created_at DESC;

-- After: Using covering index
CREATE INDEX idx_orders_user_date 
ON orders(user_id, created_at) 
INCLUDE (status, total);

-- Optimized query
SELECT order_id, status, total, created_at 
FROM orders 
WHERE user_id = 123 
AND created_at > '2024-01-01' 
ORDER BY created_at DESC;
```

### 3. JVM Tuning (for Java services)

```bash
# JVM memory and GC tuning
JAVA_OPTS="-Xms2g -Xmx2g \
  -XX:+UseG1GC \
  -XX:MaxGCPauseMillis=200 \
  -XX:+HeapDumpOnOutOfMemoryError \
  -XX:HeapDumpPath=/logs/heapdump.hprof \
  -XX:+PrintGCDetails \
  -Xloggc:/logs/gc.log"
```

### 4. Network Tuning

```yaml
# Kubernetes: Optimize network settings
apiVersion: v1
kind: ConfigMap
metadata:
  name: sysctl-config
data:
  net.core.somaxconn: "4096"
  net.ipv4.tcp_max_syn_backlog: "8192"
  net.ipv4.ip_local_port_range: "1024 65535"
```

## Implementation Example

```python
#!/usr/bin/env python3
"""
Performance Tuning Manager
Applies and tracks performance optimizations
"""

from dataclasses import dataclass, field
from typing import Dict, List, Optional
from datetime import datetime
from enum import Enum
import json


class TuningCategory(Enum):
    APPLICATION = "application"
    DATABASE = "database"
    NETWORK = "network"
    INFRASTRUCTURE = "infrastructure"
    CACHING = "caching"


class TuningStatus(Enum):
    RECOMMENDED = "recommended"
    APPLIED = "applied"
    VERIFIED = "verified"
    ROLLED_BACK = "rolled_back"


@dataclass
class TuningRecommendation:
    """A performance tuning recommendation"""
    id: str
    category: TuningCategory
    title: str
    description: str
    impact: str  # "high", "medium", "low"
    effort: str  # "high", "medium", "low"
    expected_improvement: str
    status: TuningStatus = TuningStatus.RECOMMENDED
    applied_at: Optional[datetime] = None
    verified_at: Optional[datetime] = None


@dataclass
class TuningResult:
    """Result of applying a tuning"""
    recommendation_id: str
    before_metrics: Dict
    after_metrics: Dict
    improvement_percent: float


class PerformanceTuningManager:
    """Manages performance tuning recommendations"""
    
    def __init__(self):
        self.recommendations: Dict[str, TuningRecommendation] = {}
        self.results: List[TuningResult] = []
    
    def add_recommendation(
        self,
        category: TuningCategory,
        title: str,
        description: str,
        impact: str,
        effort: str,
        expected_improvement: str
    ) -> TuningRecommendation:
        """Add a tuning recommendation"""
        
        rec = TuningRecommendation(
            id=f"tune_{len(self.recommendations) + 1:03d}",
            category=category,
            title=title,
            description=description,
            impact=impact,
            effort=effort,
            expected_improvement=expected_improvement
        )
        
        self.recommendations[rec.id] = rec
        return rec
    
    def apply_tuning(self, recommendation_id: str) -> bool:
        """Mark a tuning as applied"""
        
        rec = self.recommendations.get(recommendation_id)
        if not rec:
            return False
        
        rec.status = TuningStatus.APPLIED
        rec.applied_at = datetime.now()
        
        return True
    
    def verify_tuning(
        self,
        recommendation_id: str,
        before_metrics: Dict,
        after_metrics: Dict
    ) -> TuningResult:
        """Verify tuning effectiveness"""
        
        rec = self.recommendations.get(recommendation_id)
        if not rec:
            raise ValueError("Recommendation not found")
        
        # Calculate improvement
        if "latency" in before_metrics and "latency" in after_metrics:
            improvement = (
                (before_metrics["latency"] - after_metrics["latency"])
                / before_metrics["latency"] * 100
            )
        else:
            improvement = 0
        
        result = TuningResult(
            recommendation_id=recommendation_id,
            before_metrics=before_metrics,
            after_metrics=after_metrics,
            improvement_percent=improvement
        )
        
        rec.status = TuningStatus.VERIFIED
        rec.verified_at = datetime.now()
        
        self.results.append(result)
        
        return result
    
    def get_tuning_roadmap(self) -> Dict:
        """Generate tuning roadmap sorted by impact/effort"""
        
        # Sort by impact (high first) then effort (low first)
        sorted_recs = sorted(
            self.recommendations.values(),
            key=lambda r: (
                {"high": 0, "medium": 1, "low": 2}[r.impact],
                {"low": 0, "medium": 1, "high": 2}[r.effort]
            )
        )
        
        roadmap = {
            "total_recommendations": len(sorted_recs),
            "by_status": {
                "recommended": 0,
                "applied": 0,
                "verified": 0
            },
            "by_category": {},
            "priority_list": []
        }
        
        for rec in sorted_recs:
            roadmap["by_status"][rec.status.value] += 1
            
            cat = rec.category.value
            if cat not in roadmap["by_category"]:
                roadmap["by_category"][cat] = 0
            roadmap["by_category"][cat] += 1
            
            if rec.status == TuningStatus.RECOMMENDED:
                roadmap["priority_list"].append({
                    "id": rec.id,
                    "title": rec.title,
                    "category": cat,
                    "impact": rec.impact,
                    "effort": rec.effort,
                    "expected": rec.expected_improvement
                })
        
        return roadmap


# Example usage
if __name__ == "__main__":
    manager = PerformanceTuningManager()
    
    # Add recommendations
    manager.add_recommendation(
        TuningCategory.CACHING,
        "Add Redis caching for user profiles",
        "Cache frequently accessed user data",
        "high", "low", "40% latency reduction"
    )
    
    manager.add_recommendation(
        TuningCategory.DATABASE,
        "Add database indexes",
        "Create indexes for common queries",
        "high", "medium", "60% query speed improvement"
    )
    
    manager.add_recommendation(
        TuningCategory.APPLICATION,
        "Enable async processing",
        "Process background jobs asynchronously",
        "medium", "medium", "30% throughput increase"
    )
    
    # Get roadmap
    roadmap = manager.get_tuning_roadmap()
    print("Performance Tuning Roadmap:")
    print(json.dumps(roadmap, indent=2))
```

## Best Practices

1. **Measure First**: Always measure before and after tuning to verify improvement.

2. **Tune Incrementally**: Apply changes one at a time to understand their impact.

3. **Focus on Bottlenecks**: Identify the system's bottleneck before optimizing.

4. **Automate Where Possible**: Use infrastructure as code for reproducible tuning.

5. **Monitor Continuously**: Track metrics after tuning to detect regressions.

---

## Output Statement

```
Performance Tuning Roadmap
===========================
Total Recommendations: 12
Applied: 5
Verified: 3
Pending: 4

By Category:
- Caching: 3
- Database: 4
- Network: 2
- Application: 3

Priority (High Impact, Low Effort):
1. Add Redis caching - 40% latency reduction
2. Database indexing - 60% query speed
3. Connection pool tuning - 25% throughput
4. Async processing - 30% throughput

Expected Overall Improvement:
- Latency: -45%
- Throughput: +35%
- Resource Usage: -20%
```
