# Characteristics of Microservices

## Overview

Microservices architecture is an approach to software development where an application is built as a collection of small, independent services. Each service is self-contained, loosely coupled, and focuses on a specific business capability. This architectural style has become a dominant paradigm for building modern, cloud-native applications due to its numerous advantages over monolithic architectures.

## Definition

Microservices are independently deployable services that are organized around business capabilities, owned by a small team, and communicate through well-defined APIs. Each microservice owns its data and logic, enabling autonomous development, deployment, and scaling.

### Key Characteristics Summary

| Characteristic | Description |
|---------------|-------------|
| Independent Deployability | Services can be deployed without coordinating with other services |
| Decentralized Data Management | Each service manages its own database/data store |
| Business Capability Focus | Services align with business domains rather than technical layers |
| API-based Communication | Services communicate via lightweight protocols (typically REST/gRPC) |
| Infrastructure Automation | CI/CD, containers, and orchestration are essential |
| Organizational Alignment | Teams are organized around business capabilities |
| Technology Heterogeneity | Different services can use different technologies |
| Resilience | Failures are isolated to individual services |
| Scalability | Services scale independently based on demand |
| Observable Systems | Centralized logging, monitoring, and tracing |

---

## 1. Independent Deployability

Independent deployability is the cornerstone of microservices architecture. Each service can be deployed, updated, scaled, and restarted without affecting other services in the system.

### Why It Matters

- **Faster Release Cycles**: Teams can deploy changes independently
- **Reduced Risk**: A faulty deployment only affects one service
- **Team Autonomy**: Different teams can work on different services simultaneously
- **Scalability**: Individual services can be scaled based on load

### Implementation Example

```yaml
# docker-compose.yml - Example orchestration for independent services
version: '3.8'

services:
  user-service:
    build: ./user-service
    ports:
      - "8001:8001"
    environment:
      - DB_HOST=user-db
      - SERVICE_NAME=user-service
    depends_on:
      - user-db
    deploy:
      replicas: 2
      resources:
        limits:
          cpus: '0.5'
          memory: 512M

  order-service:
    build: ./order-service
    ports:
      - "8002:8002"
    environment:
      - DB_HOST=order-db
      - SERVICE_NAME=order-service
    depends_on:
      - order-db
    deploy:
      replicas: 4
      resources:
        limits:
          cpus: '1.0'
          memory: 1G

  notification-service:
    build: ./notification-service
    ports:
      - "8003:8003"
    deploy:
      replicas: 1
      resources:
        limits:
          cpus: '0.25'
          memory: 256M
```

### CI/CD Pipeline Example

```yaml
# .github/workflows/deploy-user-service.yml
name: Deploy User Service

on:
  push:
    branches:
      - main
    paths:
      - 'user-service/**'

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Run Unit Tests
        working-directory: ./user-service
        run: npm test
        
      - name: Build Docker Image
        working-directory: ./user-service
        run: docker build -t user-service:${{ github.sha }} .
        
      - name: Deploy to Production
        run: |
          kubectl set image deployment/user-service \
          user-service=user-service:${{ github.sha }}
```

---

## 2. Decentralized Data Management

Each microservice owns and manages its own data. There is no single shared database; instead, each service maintains its private data store optimized for its specific needs.

### Patterns for Data Management

#### Database per Service

```python
# user-service/models.py - User service owns its database
from sqlalchemy import Column, Integer, String, DateTime
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.sql import func

Base = declarative_base()

class User(Base):
    __tablename__ = 'users'
    
    id = Column(Integer, primary_key=True)
    email = Column(String(255), unique=True, nullable=False)
    username = Column(String(100), unique=True, nullable=False)
    password_hash = Column(String(255), nullable=False)
    created_at = Column(DateTime, default=func.now())
    updated_at = Column(DateTime, default=func.now(), onupdate=func.now())

# user-service/database.py
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
import os

DATABASE_URL = os.getenv('DATABASE_URL', 'postgresql://user:pass@localhost:5432/users_db')

engine = create_engine(DATABASE_URL)
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()
```

#### Event Sourcing for Data Consistency

```python
# Event store implementation for maintaining data consistency across services
from dataclasses import dataclass
from datetime import datetime
from typing import Optional, Dict, Any
import json

@dataclass
class DomainEvent:
    event_id: str
    event_type: str
    aggregate_id: str
    payload: Dict[str, Any]
    timestamp: datetime
    version: int

class EventStore:
    def __init__(self):
        self.events: list[DomainEvent] = []
    
    def append_event(self, event: DomainEvent):
        self.events.append(event)
    
    def get_events_for_aggregate(self, aggregate_id: str) -> list[DomainEvent]:
        return [e for e in self.events if e.aggregate_id == aggregate_id]

# Example: Order service publishes events for other services to consume
class OrderService:
    def __init__(self, event_store: EventStore):
        self.event_store = event_store
    
    def create_order(self, user_id: int, items: list[dict]) -> DomainEvent:
        order_id = f"order-{user_id}-{len(items)}"
        
        event = DomainEvent(
            event_id=f"evt-{order_id}",
            event_type="OrderCreated",
            aggregate_id=order_id,
            payload={
                "user_id": user_id,
                "items": items,
                "status": "PENDING"
            },
            timestamp=datetime.utcnow(),
            version=1
        )
        
        self.event_store.append_event(event)
        return event
```

### Saga Pattern for Distributed Transactions

```python
# saga_pattern.py - Managing distributed transactions
from abc import ABC, abstractmethod
from typing import Callable, list
from dataclasses import dataclass

@dataclass
class SagaStep:
    name: str
    forward: Callable
    backward: Callable

class Saga:
    def __init__(self, steps: list[SagaStep]):
        self.steps = steps
        self.executed_steps: list[str] = []
    
    def execute(self) -> bool:
        try:
            for step in self.steps:
                step.forward()
                self.executed_steps.append(step.name)
            return True
        except Exception as e:
            self.compensate()
            return False
    
    def compensate(self):
        for step_name in reversed(self.executed_steps):
            for step in self.steps:
                if step.name == step_name:
                    step.backward()

# Example: Order creation saga
class OrderSaga:
    @staticmethod
    def create_order_saga(user_id: int, items: list[dict]):
        steps = [
            SagaStep(
                name="reserve_inventory",
                forward=lambda: InventoryService.reserve_items(items),
                backward=lambda: InventoryService.release_items(items)
            ),
            SagaStep(
                name="charge_payment",
                forward=lambda: PaymentService.charge(user_id, items),
                backward=lambda: PaymentService.refund(user_id)
            ),
            SagaStep(
                name="create_order",
                forward=lambda: OrderService.create(user_id, items),
                backward=lambda: OrderService.cancel(user_id)
            ),
            SagaStep(
                name="send_notification",
                forward=lambda: NotificationService.send_order_confirmation(user_id),
                backward=lambda: None  # No compensation needed
            )
        ]
        
        saga = Saga(steps)
        return saga.execute()
```

---

## 3. Business Capability Focus

Services are organized around business capabilities rather than technical layers. This aligns technical implementation with business needs, enabling faster iteration and better domain modeling.

### Designing Services Around Business Capabilities

```
┌─────────────────────────────────────────────────────────────────┐
│                    Business Capabilities                         │
├──────────────────┬──────────────────┬───────────────────────────┤
│  User Management │  Order Management│  Inventory Management    │
│  - Registration  │  - Create Order  │  - Track Stock            │
│  - Authentication│  - View Orders   │  - Reserve Items          │
│  - Profile Mgmt  │  - Order History │  - Update Stock           │
└──────────────────┴──────────────────┴───────────────────────────┘
         │                   │                    │
         ▼                   ▼                    ▼
    ┌─────────┐       ┌─────────┐          ┌─────────┐
    │  User   │       │  Order  │          │Inventory│
    │ Service │       │ Service │          │ Service │
    └─────────┘       └─────────┘          └─────────┘
```

### Service Bounded Context Example

```python
# user-service/domain/user_bounded_context.py
from dataclasses import dataclass
from datetime import datetime
from typing import Optional

@dataclass
class User:
    user_id: str
    email: str
    username: str
    first_name: str
    last_name: str
    phone: Optional[str]
    created_at: datetime
    is_active: bool

class UserService:
    def register_user(self, email: str, username: str, 
                      password: str, first_name: str, 
                      last_name: str) -> User:
        # Validate unique email
        if self.user_repository.find_by_email(email):
            raise ValueError("Email already registered")
        
        # Create user entity
        user = User(
            user_id=self.generate_user_id(),
            email=email,
            username=username,
            first_name=first_name,
            last_name=last_name,
            phone=None,
            created_at=datetime.utcnow(),
            is_active=True
        )
        
        # Hash password
        user.password_hash = self.hash_password(password)
        
        # Persist
        self.user_repository.save(user)
        
        # Publish event
        self.event_publisher.publish(UserRegisteredEvent(user.user_id))
        
        return user
```

---

## 4. API-Based Communication

Services communicate through well-defined APIs, typically REST or gRPC. This ensures loose coupling and allows services to evolve independently.

### REST API Example

```python
# user-service/api/routes.py
from fastapi import FastAPI, HTTPException, Depends
from pydantic import BaseModel, EmailStr
from typing import Optional

app = FastAPI()

class UserCreate(BaseModel):
    email: EmailStr
    username: str
    password: str
    first_name: str
    last_name: str

class UserResponse(BaseModel):
    user_id: str
    email: str
    username: str
    first_name: str
    last_name: str

class UserUpdate(BaseModel):
    first_name: Optional[str] = None
    last_name: Optional[str] = None
    phone: Optional[str] = None

@app.post("/api/v1/users", response_model=UserResponse, status_code=201)
async def create_user(user: UserCreate):
    try:
        new_user = user_service.register_user(
            email=user.email,
            username=user.username,
            password=user.password,
            first_name=user.first_name,
            last_name=user.last_name
        )
        return UserResponse(
            user_id=new_user.user_id,
            email=new_user.email,
            username=new_user.username,
            first_name=new_user.first_name,
            last_name=new_user.last_name
        )
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))

@app.get("/api/v1/users/{user_id}", response_model=UserResponse)
async def get_user(user_id: str):
    user = user_service.get_user(user_id)
    if not user:
        raise HTTPException(status_code=404, detail="User not found")
    return UserResponse(
        user_id=user.user_id,
        email=user.email,
        username=user.username,
        first_name=user.first_name,
        last_name=user.last_name
    )

@app.patch("/api/v1/users/{user_id}", response_model=UserResponse)
async def update_user(user_id: str, updates: UserUpdate):
    user = user_service.update_user(user_id, updates.dict(exclude_none=True))
    if not user:
        raise HTTPException(status_code=404, detail="User not found")
    return UserResponse(
        user_id=user.user_id,
        email=user.email,
        username=user.username,
        first_name=user.first_name,
        last_name=user.last_name
    )
```

### gRPC Service Definition

```protobuf
// user.proto
syntax = "proto3";

package user;

service UserService {
    rpc CreateUser (CreateUserRequest) returns (UserResponse);
    rpc GetUser (GetUserRequest) returns (UserResponse);
    rpc UpdateUser (UpdateUserRequest) returns (UserResponse);
    rpc DeleteUser (DeleteUserRequest) returns (Empty);
    rpc ListUsers (ListUsersRequest) returns (stream UserResponse);
}

message CreateUserRequest {
    string email = 1;
    string username = 2;
    string password = 3;
    string first_name = 4;
    string last_name = 5;
}

message UserResponse {
    string user_id = 1;
    string email = 2;
    string username = 3;
    string first_name = 4;
    string last_name = 5;
    int64 created_at = 6;
}

message GetUserRequest {
    string user_id = 1;
}

message UpdateUserRequest {
    string user_id = 1;
    string first_name = 2;
    string last_name = 3;
}

message DeleteUserRequest {
    string user_id = 1;
}

message ListUsersRequest {
    int32 page_size = 1;
    string page_token = 2;
}

message Empty {}
```

---

## 5. Infrastructure Automation

Microservices require robust infrastructure automation including CI/CD pipelines, containerization, and orchestration.

### Kubernetes Deployment

```yaml
# k8s/user-service-deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: user-service
  labels:
    app: user-service
spec:
  replicas: 3
  selector:
    matchLabels:
      app: user-service
  template:
    metadata:
      labels:
        app: user-service
        version: v1
    spec:
      containers:
      - name: user-service
        image: myregistry/user-service:v1.2.3
        ports:
        - containerPort: 8001
        env:
        - name: DATABASE_URL
          valueFrom:
            secretKeyRef:
              name: db-credentials
              key: url
        - name: JWT_SECRET
          valueFrom:
            secretKeyRef:
              name: jwt-secret
              key: secret
        resources:
          requests:
            memory: "256Mi"
            cpu: "250m"
          limits:
            memory: "512Mi"
            cpu: "500m"
        livenessProbe:
          httpGet:
            path: /health
            port: 8001
          initialDelaySeconds: 30
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: /ready
            port: 8001
          initialDelaySeconds: 5
          periodSeconds: 5
---
apiVersion: v1
kind: Service
metadata:
  name: user-service
spec:
  selector:
    app: user-service
  ports:
  - port: 80
    targetPort: 8001
  type: ClusterIP
```

### Helm Chart Values

```yaml
# helm/user-service/values.yaml
replicaCount: 3

image:
  repository: myregistry/user-service
  tag: v1.2.3
  pullPolicy: IfNotPresent

service:
  type: ClusterIP
  port: 80
  targetPort: 8001

ingress:
  enabled: true
  className: nginx
  annotations:
    cert-manager.io/cluster-issuer: letsencrypt-prod
  hosts:
    - host: api.example.com
      paths:
        - path: /api/v1/users
          pathType: Prefix

resources:
  limits:
    cpu: 500m
    memory: 512Mi
  requests:
    cpu: 250m
    memory: 256Mi

autoscaling:
  enabled: true
  minReplicas: 2
  maxReplicas: 10
  targetCPUUtilizationPercentage: 70

config:
  DATABASE_URL: "postgresql://postgres:password@postgres:5432/users"
  REDIS_URL: "redis://redis:6379"
  LOG_LEVEL: "info"
```

---

## 6. Organizational Alignment

Teams are organized around business capabilities, following Conway's Law. Each team owns their services end-to-end, from development to deployment.

### Team Structure

```
┌─────────────────────────────────────────────────────────────────┐
│                        Organization                              │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ┌──────────────────────┐   ┌──────────────────────┐           │
│  │   User Team          │   │   Order Team         │           │
│  │   (5-7 engineers)    │   │   (5-7 engineers)    │           │
│  │                      │   │                      │           │
│  │  - User Service      │   │  - Order Service     │           │
│  │  - Auth Service      │   │  - Payment Service   │           │
│  │  - Profile Service   │   │  - Cart Service      │           │
│  └──────────────────────┘   └──────────────────────┘           │
│                                                                  │
│  ┌──────────────────────┐   ┌──────────────────────┐           │
│  │   Inventory Team    │   │   Notification Team │           │
│  │   (5-7 engineers)    │   │   (3-5 engineers)   │           │
│  │                      │   │                      │           │
│  │  - Inventory Service│   │  - Email Service    │           │
│  │  - Warehouse Service│   │  - SMS Service      │           │
│  │  - Stock Service     │   │  - Push Service     │           │
│  └──────────────────────┘   └──────────────────────┘           │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### Team Responsibility Contract

```markdown
## User Team Responsibilities

### Services Owned
- `user-service`: User registration, profile management
- `auth-service`: Authentication, authorization, JWT token management

### API Contracts
- REST API: `GET/POST/PATCH/DELETE /api/v1/users/*`
- gRPC: `UserService` RPCs
- Events: `UserCreated`, `UserUpdated`, `UserDeleted`

### SLOs (Service Level Objectives)
- Availability: 99.9%
- Latency: p50 < 50ms, p99 < 200ms
- Error rate: < 0.1%

### On-Call Rotation
- Primary: Team member 1
- Secondary: Team member 2
- Rotation: Weekly
```

---

## 7. Technology Heterogeneity

Different services can use different technology stacks based on their specific requirements.

### Technology Selection by Service

```yaml
# docker-compose.yml - Multiple technologies
version: '3.8'

services:
  # Java/Spring for user service - better for complex business logic
  user-service:
    build:
      context: ./user-service
      dockerfile: Dockerfile
    ports:
      - "8001:8080"
  
  # Python/FastAPI for ML recommendation service
  recommendation-service:
    build:
      context: ./recommendation-service
      dockerfile: Dockerfile
    ports:
      - "8002:8000"
  
  # Go for high-performance notification service
  notification-service:
    build:
      context: ./notification-service
      dockerfile: Dockerfile
    ports:
      - "8003:8080"
  
  # Node.js for real-time chat service
  chat-service:
    build:
      context: ./chat-service
      dockerfile: Dockerfile
    ports:
      - "8004:3000"
  
  # Different databases per service
  user-db:
    image: postgres:15
    environment:
      POSTGRES_DB: users
    volumes:
      - user-data:/var/lib/postgresql/data
  
  # MongoDB for flexible document storage
  recommendation-db:
    image: mongo:7
    volumes:
      - recommendation-data:/data/db
  
  # Redis for caching
  cache:
    image: redis:7-alpine
    volumes:
      - cache-data:/data
```

---

## 8. Resilience and Fault Isolation

Microservices must handle failures gracefully. Each service should be resilient to failures in dependent services.

### Circuit Breaker Pattern

```python
# resilience/circuit_breaker.py
from enum import Enum
from datetime import datetime, timedelta
from typing import Callable, TypeVar, Generic
import time
import logging

logger = logging.getLogger(__name__)

class CircuitState(Enum):
    CLOSED = "closed"
    OPEN = "open"
    HALF_OPEN = "half_open"

T = TypeVar('T')

class CircuitBreaker(Generic[T]):
    def __init__(
        self,
        failure_threshold: int = 5,
        recovery_timeout: int = 60,
        expected_exception: type = Exception
    ):
        self.failure_threshold = failure_threshold
        self.recovery_timeout = recovery_timeout
        self.expected_exception = expected_exception
        self.state = CircuitState.CLOSED
        self.failure_count = 0
        self.last_failure_time = None
        self.success_count = 0
    
    def call(self, func: Callable[[], T]) -> T:
        if self.state == CircuitState.OPEN:
            if self._should_attempt_reset():
                self.state = CircuitState.HALF_OPEN
            else:
                raise CircuitBreakerOpenError("Circuit breaker is OPEN")
        
        try:
            result = func()
            self._on_success()
            return result
        except self.expected_exception as e:
            self._on_failure()
            raise e
    
    def _on_success(self):
        self.failure_count = 0
        self.state = CircuitState.CLOSED
        self.success_count += 1
    
    def _on_failure(self):
        self.failure_count += 1
        self.last_failure_time = datetime.utcnow()
        
        if self.failure_count >= self.failure_threshold:
            self.state = CircuitState.OPEN
            logger.warning("Circuit breaker opened")
    
    def _should_attempt_reset(self) -> bool:
        if self.last_failure_time is None:
            return True
        return datetime.utcnow() - self.last_failure_time > timedelta(seconds=self.recovery_timeout)

class CircuitBreakerOpenError(Exception):
    pass

# Usage example
class PaymentServiceClient:
    def __init__(self):
        self.circuit_breaker = CircuitBreaker(
            failure_threshold=5,
            recovery_timeout=30,
            expected_exception=ConnectionError
        )
    
    def process_payment(self, user_id: str, amount: float) -> dict:
        return self.circuit_breaker.call(
            lambda: self._process_payment_request(user_id, amount)
        )
    
    def _process_payment_request(self, user_id: str, amount: float) -> dict:
        # External API call
        pass
```

### Retry Pattern with Exponential Backoff

```python
# resilience/retry.py
import time
import random
from functools import wraps
from typing import Callable, TypeVar, Any

T = TypeVar('T')

def retry_with_backoff(
    max_retries: int = 3,
    base_delay: float = 1.0,
    max_delay: float = 30.0,
    exponential_base: float = 2.0,
    jitter: bool = True
):
    def decorator(func: Callable[..., T]) -> Callable[..., T]:
        @wraps(func)
        def wrapper(*args: Any, **kwargs: Any) -> T:
            last_exception = None
            
            for attempt in range(max_retries + 1):
                try:
                    return func(*args, **kwargs)
                except Exception as e:
                    last_exception = e
                    
                    if attempt == max_retries:
                        break
                    
                    delay = min(base_delay * (exponential_base ** attempt), max_delay)
                    
                    if jitter:
                        delay = delay * (0.5 + random.random())
                    
                    time.sleep(delay)
            
            raise last_exception
        
        return wrapper
    return decorator

# Usage
@retry_with_backoff(max_retries=3, base_delay=1.0, max_delay=30.0)
def call_external_api(url: str) -> dict:
    # API call that might fail
    pass
```

### Bulkhead Pattern

```python
# resilience/bulkhead.py
import threading
from queue import Queue
from typing import Callable, TypeVar

T = TypeVar('T')

class Bulkhead:
    def __init__(self, max_concurrent: int, queue_size: int):
        self.semaphore = threading.Semaphore(max_concurrent)
        self.queue = Queue(maxsize=queue_size)
    
    def execute(self, func: Callable[[], T]) -> T:
        if self.queue.full():
            raise BulkheadFullError("Bulkhead queue is full")
        
        self.queue.put(threading.current_thread())
        self.semaphore.acquire()
        
        try:
            return func()
        finally:
            self.semaphore.release()
            self.queue.get()

class BulkheadFullError(Exception):
    pass
```

---

## 9. Scalability

Services can scale independently based on their specific load characteristics and resource requirements.

### Horizontal Scaling Architecture

```mermaid
graph TB
    subgraph "Load Balancer"
        LB[NGINX / HAProxy]
    end
    
    subgraph "User Service (3 replicas)"
        US1[User Service 1]
        US2[User Service 2]
        US3[User Service 3]
    end
    
    subgraph "Order Service (10 replicas)"
        OS1[Order Service 1]
        OS2[Order Service 2]
        OS10[Order Service 10]
    end
    
    subgraph "Database Layer"
        PG[(PostgreSQL Primary)]
        PG_R1[(PostgreSQL Replica 1)]
        PG_R2[(PostgreSQL Replica 2)]
    end
    
    LB --> US1
    LB --> US2
    LB --> US3
    
    US1 --> PG
    US2 --> PG
    US3 --> PG
    
    OS1 --> PG_R1
    OS2 --> PG_R2
    OS10 --> PG_R1
```

### Auto-Scaling Configuration

```yaml
# k8s/hpa-user-service.yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: user-service-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: user-service
  minReplicas: 2
  maxReplicas: 20
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 70
  - type: Resource
    resource:
      name: memory
      target:
        type: Utilization
        averageUtilization: 80
  behavior:
    scaleUp:
      stabilizationWindowSeconds: 30
      policies:
      - type: Percent
        value: 100
        periodSeconds: 15
      - type: Pods
        value: 4
        periodSeconds: 15
      selectPolicy: Max
    scaleDown:
      stabilizationWindowSeconds: 300
      policies:
      - type: Percent
        value: 10
        periodSeconds: 60
```

---

## 10. Observable Systems

Comprehensive logging, monitoring, and tracing are essential for debugging and understanding distributed systems.

### Structured Logging

```python
# observability/logging.py
import logging
import json
from datetime import datetime
from typing import Any, Dict
import traceback

class JSONFormatter(logging.Formatter):
    def format(self, record: logging.LogRecord) -> str:
        log_data: Dict[str, Any] = {
            "timestamp": datetime.utcnow().isoformat(),
            "level": record.levelname,
            "logger": record.name,
            "message": record.getMessage(),
            "service": getattr(record, "service", "unknown"),
            "environment": getattr(record, "environment", "production"),
        }
        
        if record.exc_info:
            log_data["exception"] = {
                "type": record.exc_info[0].__name__,
                "message": str(record.exc_info[1]),
                "traceback": traceback.format_exception(*record.exc_info)
            }
        
        if hasattr(record, "user_id"):
            log_data["user_id"] = record.user_id
        
        if hasattr(record, "request_id"):
            log_data["request_id"] = record.request_id
        
        if hasattr(record, "duration_ms"):
            log_data["duration_ms"] = record.duration_ms
        
        return json.dumps(log_data)

# Usage
logger = logging.getLogger("user-service")
logger.setLevel(logging.INFO)

handler = logging.StreamHandler()
handler.setFormatter(JSONFormatter())
logger.addHandler(handler)

# Log with context
logger.info(
    "User created",
    extra={
        "service": "user-service",
        "environment": "production",
        "user_id": "user-123",
        "request_id": "req-456"
    }
)
```

### Distributed Tracing

```python
# observability/tracing.py
from opentelemetry import trace
from opentelemetry.exporter.jaeger.thrift import JaegerExporter
from opentelemetry.sdk.trace import TracerProvider
from opentelemetry.sdk.trace.export import BatchSpanProcessor
from opentelemetry.instrumentation.flask import FlaskInstrumentor
from opentelemetry.propagate import inject, extract

# Initialize tracing
trace.set_tracer_provider(TracerProvider())

jaeger_exporter = JaegerExporter(
    agent_host_name="jaeger",
    agent_port=6831,
)

trace.get_tracer_provider().add_span_processor(
    BatchSpanProcessor(jaeger_exporter)
)

tracer = trace.get_tracer(__name__)

# Instrument Flask app
FlaskInstrumentor().instrument_app(app)

# Usage in service
@app.route("/api/v1/users")
def get_users():
    with tracer.start_as_current_span(
        "get_users",
        attributes={
            "http.method": "GET",
            "http.url": "/api/v1/users",
            "service.name": "user-service"
        }
    ) as span:
        span.set_attribute("user.count", len(users))
        
        # Call another service
        with tracer.start_as_current_span("call_order_service") as order_span:
            inject(order_span.context)  # Inject trace context
            orders = call_order_service(user_id)
            order_span.set_attribute("order.count", len(orders))
        
        return users
```

### Health Check Endpoints

```python
# observability/health.py
from fastapi import FastAPI
from pydantic import BaseModel
from typing import Dict, Any
import httpx

app = FastAPI()

class HealthResponse(BaseModel):
    status: str
    checks: Dict[str, Any]

@app.get("/health")
async def health_check() -> HealthResponse:
    checks = {}
    overall_status = "healthy"
    
    # Database check
    try:
        await check_database()
        checks["database"] = {"status": "healthy"}
    except Exception as e:
        checks["database"] = {"status": "unhealthy", "error": str(e)}
        overall_status = "unhealthy"
    
    # External service checks
    try:
        await check_order_service()
        checks["order_service"] = {"status": "healthy"}
    except Exception as e:
        checks["order_service"] = {"status": "degraded", "error": str(e)}
        if overall_status == "healthy":
            overall_status = "degraded"
    
    return HealthResponse(
        status=overall_status,
        checks=checks
    )

@app.get("/ready")
async def readiness_check() -> Dict[str, str]:
    return {"status": "ready"}

async def check_database():
    async with httpx.AsyncClient() as client:
        response = await client.get(
            "http://user-db:5432/health",
            timeout=5.0
        )
        response.raise_for_status()

async def check_order_service():
    async with httpx.AsyncClient() as client:
        response = await client.get(
            "http://order-service:8002/health",
            timeout=5.0
        )
        response.raise_for_status()
```

---

## Flow Chart: Service Communication

```mermaid
graph TD
    Client[Client Application] -->|HTTP/gRPC| Gateway[API Gateway]
    
    subgraph "Microservices Architecture"
        Gateway -->|Route| UserService[User Service]
        Gateway -->|Route| OrderService[Order Service]
        Gateway -->|Route| InventoryService[Inventory Service]
        Gateway -->|Route| NotificationService[Notification Service]
        
        UserService -->|Async Events| EventBus[Event Bus]
        OrderService -->|Async Events| EventBus
        InventoryService -->|Async Events| EventBus
        
        EventBus -->|Process Event| NotificationService
        EventBus -->|Process Event| InventoryService
        
        UserService -->|Read/Write| UserDB[(User Database)]
        OrderService -->|Read/Write| OrderDB[(Order Database)]
        InventoryService -->|Read/Write| InventoryDB[(Inventory Database)]
        NotificationService -->|Read/Write| NotificationDB[(Notification Database)]
    end
    
    style Gateway fill:#f9f,style Gateway:fill:#f99
    style EventBus fill:#ff9,style EventBus:fill:#ff9
    style UserService fill:#9f9,style UserService:fill:#9f9
    style OrderService fill:#9f9,style OrderService:fill:#9f9
    style InventoryService fill:#9f9,style InventoryService:fill:#9f9
    style NotificationService fill:#9f9,style NotificationService:fill:#9f9
```

---

## Best Practices Summary

### 1. Service Design
- Keep services small and focused on single business capability
- Design APIs with backward compatibility in mind
- Use contract testing to ensure API compatibility

### 2. Data Management
- Each service owns its data - never share databases
- Use saga pattern for distributed transactions
- Implement event sourcing for eventual consistency

### 3. Communication
- Prefer asynchronous communication when possible
- Use message queues for decoupling
- Implement circuit breakers for fault tolerance

### 4. Deployment
- Containerize all services
- Use Kubernetes for orchestration
- Implement proper health checks and readiness probes

### 5. Monitoring
- Implement distributed tracing across all services
- Use structured JSON logging
- Create dashboards for key metrics

### 6. Security
- Implement API gateway for authentication
- Use mutual TLS between services
- Rotate secrets regularly

### 7. Testing
- Implement contract testing between services
- Use chaos engineering to test resilience
- Run integration tests in CI/CD pipeline

---

## Conclusion

Microservices architecture provides numerous benefits including independent deployability, scalability, and organizational alignment. However, it also introduces complexity that must be carefully managed. By following the characteristics outlined in this document and implementing the best practices, organizations can successfully build and operate microservices-based systems that are resilient, scalable, and maintainable.

The key to success lies in understanding that microservices is not just a technical architecture but also an organizational and cultural approach to software development. Teams must embrace automation, monitoring, and continuous improvement to fully realize the benefits of this architectural style.
