# Connection Pooling for Microservices

## Overview

Connection pooling is essential for efficient database and service-to-service communication in microservices architectures. Each connection setup involves significant overhead (network latency, authentication, protocol handshake), making it inefficient to create new connections for every request. Connection pools maintain a set of pre-established connections that can be reused, dramatically improving performance.

In microservices, connection pools are needed for:
- Database connections
- HTTP client connections to other services
- Message queue connections
- Cache client connections

Proper connection pool configuration balances resource usage (keeping connections available) with system limits (not overwhelming the target service or database).

## Connection Pool Concepts

### 1. Pool Size Configuration

The optimal pool size depends on:
- Number of concurrent requests
- Database or service capacity
- Response time requirements
- Available memory

```python
# Example: Database connection pool configuration
from dataclasses import dataclass

@dataclass
class PoolConfig:
    min_connections: int = 5      # Minimum idle connections
    max_connections: int = 20      # Maximum total connections
    max_idle_time_seconds: int = 300  # Idle connection timeout
    connection_timeout_seconds: int = 30  # Connection acquisition timeout
    validation_interval_seconds: int = 60  # Health check interval
```

### 2. Connection Lifecycle

```python
class ConnectionLifecycle:
    def __init__(self):
        self.acquired = 0
        self.released = 0
        self.creation_errors = 0
    
    def acquire(self, timeout: int) -> Connection:
        """Acquire connection from pool"""
        start_time = time.time()
        
        while time.time() - start_time < timeout:
            conn = self._try_acquire()
            if conn:
                self.acquired += 1
                return conn
        
        raise ConnectionTimeoutError("Failed to acquire connection")
    
    def release(self, connection):
        """Return connection to pool"""
        if connection.is_valid():
            self._return_to_pool(connection)
        else:
            self._destroy_connection(connection)
        
        self.released += 1
```

## Implementation Example

```python
#!/usr/bin/env python3
"""
Connection Pool Manager for Microservices
"""

from dataclasses import dataclass, field
from typing import Dict, Optional, List
from enum import Enum
import threading
import time
import queue


class PoolType(Enum):
    DATABASE = "database"
    HTTP = "http"
    CACHE = "cache"
    MESSAGE_QUEUE = "message_queue"


class ConnectionStatus(Enum):
    AVAILABLE = "available"
    IN_USE = "in_use"
    INVALID = "invalid"


@dataclass
class PooledConnection:
    """Represents a pooled connection"""
    connection_id: str
    created_at: float
    last_used: float
    use_count: int = 0
    status: ConnectionStatus = ConnectionStatus.AVAILABLE
    metadata: Dict = field(default_factory=dict)


@dataclass
class PoolMetrics:
    """Connection pool metrics"""
    active_connections: int = 0
    idle_connections: int = 0
    waiting_requests: int = 0
    connection_timeouts: int = 0
    avg_acquire_time_ms: float = 0.0
    total_acquisitions: int = 0


class ConnectionPool:
    """Generic connection pool implementation"""
    
    def __init__(
        self,
        pool_type: PoolType,
        min_size: int = 5,
        max_size: int = 20,
        max_idle_time: int = 300,
        acquire_timeout: int = 30
    ):
        self.pool_type = pool_type
        self.min_size = min_size
        self.max_size = max_size
        self.max_idle_time = max_idle_time
        self.acquire_timeout = acquire_timeout
        
        self.connections: Dict[str, PooledConnection] = {}
        self.available = queue.Queue()
        self.lock = threading.Lock()
        
        self.metrics = PoolMetrics()
        self.acquire_times: List[float] = []
    
    def _create_connection(self) -> PooledConnection:
        """Create a new connection"""
        return PooledConnection(
            connection_id=f"conn_{len(self.connections)}",
            created_at=time.time(),
            last_used=time.time()
        )
    
    def acquire(self) -> PooledConnection:
        """Acquire a connection from the pool"""
        start_time = time.time()
        
        # Try to get from available pool
        try:
            conn = self.available.get_nowait()
            conn.status = ConnectionStatus.IN_USE
            self.metrics.active_connections += 1
            self.metrics.idle_connections -= 1
            return conn
        except queue.Empty:
            pass
        
        # Create new connection if pool not full
        with self.lock:
            if len(self.connections) < self.max_size:
                conn = self._create_connection()
                self.connections[conn.connection_id] = conn
                conn.status = ConnectionStatus.IN_USE
                self.metrics.active_connections += 1
                self.metrics.total_acquisitions += 1
                
                acquire_time = (time.time() - start_time) * 1000
                self.acquire_times.append(acquire_time)
                
                return conn
        
        # Wait for available connection
        try:
            conn = self.available.get(timeout=self.acquire_timeout)
            conn.status = ConnectionStatus.IN_USE
            self.metrics.active_connections += 1
            self.metrics.idle_connections -= 1
            return conn
        except queue.Empty:
            self.metrics.connection_timeouts += 1
            raise TimeoutError("Connection acquisition timeout")
    
    def release(self, conn: PooledConnection):
        """Return connection to the pool"""
        conn.last_used = time.time()
        conn.use_count += 1
        conn.status = ConnectionStatus.AVAILABLE
        
        # Check if connection should be closed
        if time.time() - conn.created_at > self.max_idle_time:
            self._close_connection(conn)
            return
        
        self.available.put(conn)
        self.metrics.active_connections -= 1
        self.metrics.idle_connections += 1
    
    def _close_connection(self, conn: PooledConnection):
        """Close and remove a connection"""
        with self.lock:
            if conn.connection_id in self.connections:
                del self.connections[conn.connection_id]
    
    def get_metrics(self) -> Dict:
        """Get pool metrics"""
        if self.acquire_times:
            self.metrics.avg_acquire_time_ms = sum(self.acquire_times) / len(self.acquire_times)
        
        return {
            "pool_type": self.pool_type.value,
            "total_connections": len(self.connections),
            "active": self.metrics.active_connections,
            "idle": self.metrics.idle_connections,
            "waiting": self.metrics.waiting_requests,
            "timeouts": self.metrics.connection_timeouts,
            "avg_acquire_time_ms": f"{self.metrics.avg_acquire_time_ms:.2f}",
            "total_acquisitions": self.metrics.total_acquisitions
        }
    
    def health_check(self):
        """Perform health check on connections"""
        current_time = time.time()
        
        with self.lock:
            for conn_id, conn in list(self.connections.items()):
                # Check idle timeout
                if (conn.status == ConnectionStatus.AVAILABLE and 
                    current_time - conn.last_used > self.max_idle_time):
                    self._close_connection(conn)
                
                # Check for stale connections
                if conn.use_count > 1000:
                    self._close_connection(conn)


class PoolManager:
    """Manages multiple connection pools"""
    
    def __init__(self):
        self.pools: Dict[PoolType, ConnectionPool] = {}
    
    def create_pool(
        self,
        name: str,
        pool_type: PoolType,
        min_size: int = 5,
        max_size: int = 20
    ) -> ConnectionPool:
        """Create a new connection pool"""
        pool = ConnectionPool(
            pool_type=pool_type,
            min_size=min_size,
            max_size=max_size
        )
        self.pools[name] = pool
        return pool
    
    def get_pool(self, name: str) -> Optional[ConnectionPool]:
        """Get a pool by name"""
        return self.pools.get(name)
    
    def get_all_metrics(self) -> Dict:
        """Get metrics from all pools"""
        return {
            name: pool.get_metrics()
            for name, pool in self.pools.items()
        }


# Example usage
if __name__ == "__main__":
    manager = PoolManager()
    
    # Create database pool
    db_pool = manager.create_pool(
        "orders_db",
        PoolType.DATABASE,
        min_size=5,
        max_size=20
    )
    
    # Create HTTP pool
    http_pool = manager.create_pool(
        "user_service_client",
        PoolType.HTTP,
        min_size=10,
        max_size=50
    )
    
    # Acquire and release connections
    conn = db_pool.acquire()
    print(f"Acquired connection: {conn.connection_id}")
    db_pool.release(conn)
    
    # Get metrics
    print(f"\nPool Metrics:")
    for name, metrics in manager.get_all_metrics().items():
        print(f"\n{name}:")
        for k, v in metrics.items():
            print(f"  {k}: {v}")
```

## Best Practices

1. **Configure Pool Size Based on Load**: Start with conservative values and adjust based on monitoring.

2. **Set Appropriate Timeouts**: Configure connection and acquire timeouts to prevent indefinite blocking.

3. **Implement Health Checks**: Regularly check connection validity and close stale connections.

4. **Monitor Pool Metrics**: Track acquisitions, releases, timeouts, and queue depths.

5. **Handle Failures Gracefully**: Implement circuit breaker patterns when connection failures occur.

---

## Output Statement

```
Connection Pool Report
=====================
Pool: orders_db (DATABASE)
- Total: 20
- Active: 8
- Idle: 12
- Timeouts: 0
- Avg Acquire Time: 2.3ms

Pool: user_service_client (HTTP)  
- Total: 50
- Active: 35
- Idle: 15
- Timeouts: 2
- Avg Acquire Time: 1.8ms

Recommendations:
1. Consider increasing database pool to 25
2. HTTP client pool operating at 70% capacity
3. Review timeout settings for high-traffic periods
```