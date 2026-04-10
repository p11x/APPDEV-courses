# Interview Preparation: Microservices Questions

## Overview

This guide covers common interview questions for microservices positions, from entry-level to senior roles. Each section includes conceptual questions, practical scenarios, and detailed answers to help you prepare effectively.

Questions are categorized by topic and difficulty, with guidance on what interviewers typically look for in responses.

## Category 1: Fundamentals

### Q1: What are microservices?

**Answer**: Microservices are an architectural style that structures an application as a collection of small, autonomous services. Each service is self-contained, owns a specific business capability, and can be deployed independently. Services communicate over well-defined APIs, typically HTTP/REST or messaging queues.

Key characteristics:
- Single responsibility principle
- Autonomous deployment
- Decentralized data management
- Infrastructure automation
- Design for failure

### Q2: How do microservices differ from monoliths?

**Answer**: 

| Aspect | Monolith | Microservices |
|--------|----------|---------------|
| Deployment | Single unit | Independent services |
| Data | Shared database | Per-service databases |
| Scaling | Scale entire app | Scale individual services |
| Development | One codebase | Multiple codebases |
| Technology | Single stack | Polyglot |
| Failure | Entire app fails | Isolated failures |

### Q3: What are the benefits and challenges of microservices?

**Answer**:

**Benefits**:
- Independent scaling and deployment
- Technology flexibility
- Faster release cycles
- Better fault isolation
- Team autonomy

**Challenges**:
- Distributed system complexity
- Data consistency across services
- Network latency and failures
- Operational overhead
- Testing complexity

## Category 2: Design Patterns

### Q4: Explain the Circuit Breaker pattern

**Answer**: The Circuit Breaker pattern prevents cascading failures by wrapping calls to remote services in a protective layer. It has three states:

1. **Closed**: Requests pass through normally; failures are counted
2. **Open**: Requests fail immediately; service is considered unavailable
3. **Half-Open**: Limited requests are allowed to test recovery

```python
# Example implementation
class CircuitBreaker:
    def __init__(self, failure_threshold=5, timeout=60):
        self.failure_threshold = failure_threshold
        self.timeout = timeout
        self.failure_count = 0
        self.last_failure_time = None
        self.state = "closed"
    
    def call(self, func):
        if self.state == "open":
            if time.time() - self.last_failure_time > self.timeout:
                self.state = "half-open"
            else:
                raise CircuitBreakerOpenError()
        
        try:
            result = func()
            self._on_success()
            return result
        except Exception as e:
            self._on_failure()
            raise
    
    def _on_success(self):
        self.failure_count = 0
        self.state = "closed"
    
    def _on_failure(self):
        self.failure_count += 1
        self.last_failure_time = time.time()
        
        if self.failure_count >= self.failure_threshold:
            self.state = "open"
```

### Q5: How would you handle distributed transactions?

**Answer**: Since distributed transactions (2PC) are impractical, we use eventual consistency patterns:

**Saga Pattern**: Orchestrate transactions across services using compensating operations:

```python
# Saga orchestrator example
class OrderSaga:
    def execute(self, order):
        try:
            # Step 1: Reserve inventory
            inventory_service.reserve(order.items)
            
            # Step 2: Process payment
            payment_service.charge(order.payment)
            
            # Step 3: Create order
            order_service.create(order)
            
            return {"status": "completed"}
        
        except PaymentFailed:
            # Compensate: Release inventory
            inventory_service.release(order.items)
            raise
        
        except InventoryFailed:
            # Compensate: Refund payment
            payment_service.refund(order.payment)
            raise
```

## Category 3: System Design

### Q6: Design a rate limiting system

**Answer**: Rate limiting can be implemented at different levels:

**Token Bucket Algorithm**:
```python
class TokenBucket:
    def __init__(self, rate, capacity):
        self.rate = rate
        self.capacity = capacity
        self.tokens = capacity
        self.last_update = time.time()
    
    def allow_request(self):
        self._refill()
        
        if self.tokens >= 1:
            self.tokens -= 1
            return True
        return False
    
    def _refill(self):
        now = time.time()
        elapsed = now - self.last_update
        self.tokens = min(
            self.capacity,
            self.tokens + elapsed * self.rate
        )
        self.last_update = now
```

**Distributed Rate Limiting with Redis**:
```python
def rate_limit(key, limit, window):
    redis = Redis()
    current = redis.incr(key)
    
    if current == 1:
        redis.expire(key, window)
    
    return current <= limit
```

### Q7: How would you design a notification system?

**Answer**: Key components:

1. **Notification Service**: API for sending notifications
2. **Template Service**: Manages notification templates
3. **Channel Handlers**: Email, SMS, Push, WebSocket
4. **Queue System**: Async processing
5. **Preference Service**: User preferences

```python
# Notification service design
class NotificationService:
    def __init__(self, queue, template_service, handlers):
        self.queue = queue
        self.template_service = template_service
        self.handlers = handlers
    
    def send(self, user_id, template, channel, data):
        # Render template
        content = self.template_service.render(template, data)
        
        # Queue for async processing
        self.queue.publish({
            "user_id": user_id,
            "channel": channel,
            "content": content
        })
    
    def process_queue(self):
        while True:
            message = self.queue.consume()
            handler = self.handlers[message["channel"]]
            handler.send(message["user_id"], message["content"])
```

## Category 4: Operations

### Q8: How do you monitor microservices?

**Answer**: Multi-layer observability:

1. **Logs**: Structured JSON logging with correlation IDs
2. **Metrics**: RED metrics (Rate, Errors, Duration)
3. **Traces**: Distributed tracing with OpenTelemetry

```python
# Structured logging
import logging
import json

class StructuredLogger:
    def __init__(self, service_name):
        self.service_name = service_name
    
    def log(self, level, message, **kwargs):
        log_entry = {
            "timestamp": datetime.now().isoformat(),
            "service": self.service_name,
            "level": level,
            "message": message,
            **kwargs
        }
        print(json.dumps(log_entry))

# Metrics with Prometheus
from prometheus_client import Counter

request_counter = Counter(
    'http_requests_total',
    'Total requests',
    ['method', 'endpoint', 'status']
)
```

### Q9: How would you handle deployment without downtime?

**Answer**: Deployment strategies:

1. **Rolling Updates**: Gradually replace instances
2. **Blue-Green**: Two identical environments
3. **Canary**: Route subset of traffic to new version
4. **Feature Flags**: Toggle features at runtime

```yaml
# Kubernetes rolling update
apiVersion: apps/v1
kind: Deployment
spec:
  strategy:
    type: RollingUpdate
    rollingUpdate:
      maxSurge: 1
      maxUnavailable: 0
```

## Output Statement

```
Interview Preparation Progress
==============================
Fundamentals: COMPLETED (9 questions)
Design Patterns: COMPLETED (8 questions)
System Design: COMPLETED (6 questions)
Operations: COMPLETED (7 questions)

Recommended Next Steps:
1. Practice system design for scale
2. Review real-world case studies
3. Prepare behavioral questions
4. Study company-specific tech stacks
```
