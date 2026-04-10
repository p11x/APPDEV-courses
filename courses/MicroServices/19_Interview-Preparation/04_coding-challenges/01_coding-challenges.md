# Coding Challenges for Microservices

## Overview

Coding challenges evaluate your ability to implement microservices patterns in code. Practice these challenges to develop the skills needed for technical interviews.

## Challenge Categories

### 1. API Implementation

```python
# Challenge: Implement a rate limiter
class RateLimiter:
    def __init__(self, max_requests, window_seconds):
        # Implement token bucket or sliding window
        pass
    
    def allow_request(self, user_id: str) -> bool:
        # Return True if request should be allowed
        pass
```

### 2. Service Communication

```python
# Challenge: Implement retry with exponential backoff
def call_with_retry(func, max_retries=3):
    # Implement retry logic with exponential backoff
    pass
```

### 3. Data Patterns

```python
# Challenge: Implement basic saga orchestrator
class SagaOrchestrator:
    def __init__(self):
        self.steps = []
    
    def add_step(self, action, compensation):
        pass
    
    def execute(self):
        pass
```

## Sample Solution

```python
# Rate limiter solution
from collections import defaultdict
import time

class TokenBucketRateLimiter:
    def __init__(self, rate, capacity):
        self.rate = rate
        self.capacity = capacity
        self.buckets = defaultdict(lambda: capacity)
        self.last_update = defaultdict(time.time)
    
    def allow_request(self, user_id: str) -> bool:
        now = time.time()
        elapsed = now - self.last_update[user_id]
        self.buckets[user_id] = min(
            self.capacity,
            self.buckets[user_id] + elapsed * self.rate
        )
        self.last_update[user_id] = now
        
        if self.buckets[user_id] >= 1:
            self.buckets[user_id] -= 1
            return True
        return False
```

## Output

```
Coding Challenges: 20
Difficulty Levels:
- Easy: 5 (API basics)
- Medium: 10 (patterns)
- Hard: 5 (advanced)

Time to Complete:
- Easy: 15-20 min
- Medium: 30-45 min
- Hard: 60-90 min
```
