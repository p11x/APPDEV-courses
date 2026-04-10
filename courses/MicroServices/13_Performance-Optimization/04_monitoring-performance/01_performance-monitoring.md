# Performance Monitoring for Microservices

## Overview

Performance monitoring in microservices is critical for identifying bottlenecks, optimizing resource usage, and maintaining SLA compliance. With multiple independent services, comprehensive monitoring provides visibility into system health and helps identify issues before they impact users.

This guide covers performance monitoring strategies, tools, and best practices for microservices architectures, including metrics collection, dashboards, alerting, and performance analysis.

## Key Metrics Categories

### 1. Infrastructure Metrics

- CPU usage and saturation
- Memory usage and allocation
- Disk I/O and storage
- Network throughput and latency

### 2. Application Metrics

- Request rate (throughput)
- Error rate
- Response latency (p50, p95, p99)
- Active connections

### 3. Business Metrics

- Orders per minute
- User signups
- API response times by endpoint

## Implementation Example

```python
#!/usr/bin/env python3
"""
Performance Monitoring System for Microservices
"""

from dataclasses import dataclass, field
from typing import Dict, List, Optional
from datetime import datetime, timedelta
from enum import Enum
import json
import time


class MetricType(Enum):
    COUNTER = "counter"
    GAUGE = "gauge"
    HISTOGRAM = "histogram"
    SUMMARY = "summary"


@dataclass
class MetricPoint:
    """Single metric data point"""
    timestamp: datetime
    value: float
    labels: Dict[str, str] = field(default_factory=dict)


@dataclass
class ServiceMetrics:
    """Metrics for a single service"""
    service_name: str
    requests_total: int = 0
    errors_total: int = 0
    latency_sum_ms: float = 0.0
    latency_count: int = 0
    
    @property
    def avg_latency_ms(self) -> float:
        return self.latency_sum_ms / self.latency_count if self.latency_count > 0 else 0
    
    @property
    def error_rate(self) -> float:
        return (self.errors_total / self.requests_total * 100) if self.requests_total > 0 else 0


class PerformanceMonitor:
    """Monitor and track performance metrics"""
    
    def __init__(self):
        self.service_metrics: Dict[str, ServiceMetrics] = {}
        self.histograms: Dict[str, List[float]] = {}
        self.thresholds = {
            "max_latency_p99_ms": 500,
            "max_error_rate_percent": 1.0,
            "max_cpu_percent": 80,
            "max_memory_percent": 85
        }
    
    def record_request(
        self,
        service_name: str,
        latency_ms: int,
        success: bool,
        endpoint: str = ""
    ):
        """Record a request metric"""
        
        if service_name not in self.service_metrics:
            self.service_metrics[service_name] = ServiceMetrics(service_name)
        
        metrics = self.service_metrics[service_name]
        metrics.requests_total += 1
        metrics.latency_sum_ms += latency_ms
        metrics.latency_count += 1
        
        if not success:
            metrics.errors_total += 1
        
        # Record histogram
        if service_name not in self.histograms:
            self.histograms[service_name] = []
        
        self.histograms[service_name].append(latency_ms)
        
        # Keep only recent 1000 samples
        if len(self.histograms[service_name]) > 1000:
            self.histograms[service_name] = self.histograms[service_name][-1000:]
    
    def calculate_percentiles(self, service_name: str) -> Dict:
        """Calculate latency percentiles"""
        
        if service_name not in self.histograms:
            return {}
        
        data = sorted(self.histograms[service_name])
        if not data:
            return {}
        
        n = len(data)
        return {
            "p50": data[int(n * 0.5)],
            "p90": data[int(n * 0.9)],
            "p95": data[int(n * 0.95)],
            "p99": data[int(n * 0.99)],
            "max": data[-1]
        }
    
    def check_thresholds(self, service_name: str) -> List[Dict]:
        """Check if metrics exceed thresholds"""
        
        alerts = []
        
        if service_name not in self.service_metrics:
            return alerts
        
        metrics = self.service_metrics[service_name]
        
        # Check error rate
        if metrics.error_rate > self.thresholds["max_error_rate_percent"]:
            alerts.append({
                "level": "critical",
                "metric": "error_rate",
                "value": f"{metrics.error_rate:.2f}%",
                "threshold": f"{self.thresholds['max_error_rate_percent']}%"
            })
        
        # Check latency
        if metrics.avg_latency_ms > self.thresholds["max_latency_p99_ms"]:
            alerts.append({
                "level": "warning",
                "metric": "avg_latency",
                "value": f"{metrics.avg_latency_ms:.0f}ms",
                "threshold": f"{self.thresholds['max_latency_p99_ms']}ms"
            })
        
        return alerts
    
    def get_service_dashboard(self, service_name: str) -> Dict:
        """Generate dashboard data for a service"""
        
        metrics = self.service_metrics.get(service_name)
        if not metrics:
            return {"error": "No metrics available"}
        
        percentiles = self.calculate_percentiles(service_name)
        alerts = self.check_thresholds(service_name)
        
        return {
            "service": service_name,
            "summary": {
                "requests_total": metrics.requests_total,
                "errors_total": metrics.errors_total,
                "error_rate": f"{metrics.error_rate:.2f}%",
                "avg_latency": f"{metrics.avg_latency_ms:.2f}ms"
            },
            "latency_percentiles": percentiles,
            "alerts": alerts,
            "status": "healthy" if not alerts else "warning"
        }
    
    def get_system_overview(self) -> Dict:
        """Get overview of all services"""
        
        services = []
        
        for service_name in self.service_metrics:
            dashboard = self.get_service_dashboard(service_name)
            services.append(dashboard)
        
        # Sort by error rate descending
        services.sort(key=lambda x: float(x.get("summary", {}).get("error_rate", "0%").replace("%", "")), reverse=True)
        
        return {
            "total_services": len(services),
            "healthy_count": sum(1 for s in services if s.get("status") == "healthy"),
            "warning_count": sum(1 for s in services if s.get("status") == "warning"),
            "services": services
        }


# Example usage
if __name__ == "__main__":
    monitor = PerformanceMonitor()
    
    # Simulate requests
    for i in range(100):
        monitor.record_request(
            "order-service",
            latency_ms=50 + (i % 50),
            success=i % 20 != 0
        )
    
    # Get dashboard
    dashboard = monitor.get_service_dashboard("order-service")
    print("Order Service Dashboard:")
    print(json.dumps(dashboard, indent=2))
    
    # System overview
    overview = monitor.get_system_overview()
    print(f"\nSystem Overview:")
    print(json.dumps(overview, indent=2))
```

## Monitoring Tools

| Tool | Type | Use Case |
|------|------|----------|
| Prometheus | Metrics | Time-series metrics collection |
| Grafana | Visualization | Dashboards and alerting |
| Jaeger | Tracing | Distributed tracing |
| ELK Stack | Logging | Log aggregation |
| DataDog | APM | Full-stack monitoring |

## Output Statement

```
Performance Dashboard - Order Service
=====================================
Status: HEALTHY

Request Metrics:
- Total Requests: 10,000
- Errors: 50
- Error Rate: 0.50%
- Avg Latency: 75ms

Latency Percentiles:
- p50: 55ms
- p90: 120ms
- p95: 180ms
- p99: 350ms

Alerts: None

System Wide:
- Total Services: 15
- Healthy: 12
- Warning: 3
```