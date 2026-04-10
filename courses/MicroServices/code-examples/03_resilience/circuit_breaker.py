"""
Circuit Breaker Implementation

This module provides a production-ready circuit breaker:
- State machine: Closed, Open, Half-Open
- Configurable thresholds and timeouts
- Metrics collection
- Thread-safe operations

Usage:
    cb = CircuitBreaker(
        failure_threshold=5,
        recovery_timeout=30,
        expected_exception=Exception
    )
    
    result = await cb.call(maybe_unreliable_function)
"""

import asyncio
import logging
import time
from typing import Any, Callable, Dict, Optional, Type, TypeVar, Generic
from dataclasses import dataclass, field
from enum import Enum
from functools import wraps
import threading


logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)


T = TypeVar('T')


class CircuitState(Enum):
    """Circuit breaker states."""
    CLOSED = "closed"      # Normal operation
    OPEN = "open"          # Failing, reject calls
    HALF_OPEN = "half_open"  # Testing recovery


class CircuitBreakerOpen(Exception):
    """Raised when circuit breaker is open."""
    pass


class CircuitBreakerError(Exception):
    """Base exception for circuit breaker errors."""
    pass


@dataclass
class CircuitBreakerConfig:
    """Configuration for circuit breaker behavior."""
    failure_threshold: int = 5        # Failures to open circuit
    success_threshold: int = 3        # Successes to close circuit
    recovery_timeout: float = 30.0     # Seconds before half-open
    half_open_max_calls: int = 3      # Max calls in half-open
    excluded_exceptions: tuple = ()   # Exceptions that don't count


@dataclass
class CircuitBreakerMetrics:
    """Metrics for monitoring circuit breaker."""
    total_calls: int = 0
    successful_calls: int = 0
    failed_calls: int = 0
    rejected_calls: int = 0
    last_failure_time: Optional[float] = None
    last_success_time: Optional[float] = None
    state_changes: int = 0
    
    @property
    def failure_rate(self) -> float:
        """Calculate failure rate."""
        if self.total_calls == 0:
            return 0.0
        return self.failed_calls / self.total_calls


class CircuitBreaker:
    """
    Thread-safe circuit breaker implementation.
    
    The circuit breaker prevents cascading failures by failing fast
    when a service is unavailable. It has three states:
    
    - CLOSED: Normal operation, requests pass through
    - OPEN: Service is down, requests fail immediately
    - HALF_OPEN: Testing if service has recovered
    
    State Transitions:
    - CLOSED -> OPEN: After failure_threshold failures
    - OPEN -> HALF_OPEN: After recovery_timeout seconds
    - HALF_OPEN -> CLOSED: After success_threshold successes
    - HALF_OPEN -> OPEN: On any failure
    
    Usage:
        cb = CircuitBreaker(failure_threshold=5, recovery_timeout=30)
        
        try:
            result = await cb.call(unreliable_service)
        except CircuitBreakerOpen:
            print("Service unavailable")
    """
    
    def __init__(
        self,
        name: str = "default",
        config: Optional[CircuitBreakerConfig] = None,
    ):
        """
        Initialize circuit breaker.
        
        Args:
            name: Name identifier for this circuit
            config: Configuration parameters
        """
        self.name = name
        self.config = config or CircuitBreakerConfig()
        
        # State
        self._state = CircuitState.CLOSED
        self._lock = threading.RLock()
        
        # Counters
        self._failure_count = 0
        self._success_count = 0
        self._last_failure_time: Optional[float] = None
        
        # Half-open tracking
        self._half_open_calls = 0
        
        # Metrics
        self.metrics = CircuitBreakerMetrics()
    
    @property
    def state(self) -> CircuitState:
        """Get current state, checking for automatic transitions."""
        with self._lock:
            if self._state == CircuitState.OPEN:
                # Check if recovery timeout has passed
                if self._last_failure_time:
                    elapsed = time.time() - self._last_failure_time
                    if elapsed >= self.config.recovery_timeout:
                        self._transition_to(CircuitState.HALF_OPEN)
            
            return self._state
    
    def _transition_to(self, new_state: CircuitState):
        """Transition to a new state."""
        logger.info(
            f"Circuit {self.name}: {self._state.value} -> {new_state.value}"
        )
        self._state = new_state
        self.metrics.state_changes += 1
        
        if new_state == CircuitState.CLOSED:
            self._failure_count = 0
            self._success_count = 0
        
        elif new_state == CircuitState.HALF_OPEN:
            self._success_count = 0
            self._half_open_calls = 0
        
        elif new_state == CircuitState.OPEN:
            self._last_failure_time = time.time()
    
    def _can_execute(self) -> bool:
        """Check if a call can be executed."""
        if self.state == CircuitState.CLOSED:
            return True
        
        if self.state == CircuitState.HALF_OPEN:
            return self._half_open_calls < self.config.half_open_max_calls
        
        return False
    
    async def call(
        self,
        func: Callable[..., Any],
        *args,
        **kwargs,
    ) -> Any:
        """
        Execute a function with circuit breaker protection.
        
        Args:
            func: Async function to execute
            *args: Positional arguments
            **kwargs: Keyword arguments
            
        Returns:
            Result from function
            
        Raises:
            CircuitBreakerOpen: If circuit is open
        """
        with self._lock:
            if not self._can_execute():
                self.metrics.rejected_calls += 1
                raise CircuitBreakerOpen(
                    f"Circuit {self.name} is {self.state.value}"
                )
            
            if self._state == CircuitState.HALF_OPEN:
                self._half_open_calls += 1
        
        # Execute the call
        self.metrics.total_calls += 1
        
        try:
            if asyncio.iscoroutinefunction(func):
                result = await func(*args, **kwargs)
            else:
                result = func(*args, **kwargs)
            
            self._record_success()
            self.metrics.last_success_time = time.time()
            return result
        
        except Exception as e:
            self._record_failure(e)
            self.metrics.last_failure_time = time.time()
            raise
    
    def _record_success(self):
        """Record a successful call."""
        with self._lock:
            self.metrics.successful_calls += 1
            
            if self._state == CircuitState.HALF_OPEN:
                self._success_count += 1
                self._half_open_calls = max(0, self._half_open_calls - 1)
                
                if self._success_count >= self.config.success_threshold:
                    self._transition_to(CircuitState.CLOSED)
            
            elif self._state == CircuitState.CLOSED:
                self._failure_count = 0
    
    def _record_failure(self, exception: Exception):
        """Record a failed call."""
        with self._lock:
            self.metrics.failed_calls += 1
            
            # Check if exception should be excluded
            if isinstance(exception, self.config.excluded_exceptions):
                logger.debug(
                    f"Circuit {self.name}: Ignoring excluded exception"
                )
                return
            
            if self._state == CircuitState.HALF_OPEN:
                # Any failure in half-open reopens the circuit
                self._transition_to(CircuitState.OPEN)
            
            elif self._state == CircuitState.CLOSED:
                self._failure_count += 1
                
                if self._failure_count >= self.config.failure_threshold:
                    self._transition_to(CircuitState.OPEN)
    
    def reset(self):
        """Manually reset the circuit breaker."""
        with self._lock:
            self._transition_to(CircuitState.CLOSED)
            self.metrics = CircuitBreakerMetrics()
    
    def get_stats(self) -> Dict[str, Any]:
        """Get circuit breaker statistics."""
        return {
            "name": self.name,
            "state": self.state.value,
            "failure_count": self._failure_count,
            "success_count": self._success_count,
            "metrics": {
                "total_calls": self.metrics.total_calls,
                "successful_calls": self.metrics.successful_calls,
                "failed_calls": self.metrics.failed_calls,
                "rejected_calls": self.metrics.rejected_calls,
                "failure_rate": self.metrics.failure_rate,
                "state_changes": self.metrics.state_changes,
            },
        }


# Decorator version
def circuit_breaker(
    failure_threshold: int = 5,
    recovery_timeout: float = 30.0,
    success_threshold: int = 3,
    name: Optional[str] = None,
    excluded_exceptions: tuple = (),
):
    """
    Decorator to add circuit breaker protection to async functions.
    
    Args:
        failure_threshold: Failures before opening circuit
        recovery_timeout: Seconds before trying half-open
        success_threshold: Successes needed to close circuit
        name: Circuit breaker name
        excluded_exceptions: Exceptions that don't count as failures
        
    Usage:
        @circuit_breaker(failure_threshold=3, recovery_timeout=10)
        async def call_service():
            # May fail
            pass
    """
    config = CircuitBreakerConfig(
        failure_threshold=failure_threshold,
        recovery_timeout=recovery_timeout,
        success_threshold=success_threshold,
        excluded_exceptions=excluded_exceptions,
    )
    
    def decorator(func: Callable) -> Callable:
        cb_name = name or func.__name__
        breaker = CircuitBreaker(cb_name, config)
        
        @wraps(func)
        async def wrapper(*args, **kwargs):
            return await breaker.call(func, *args, **kwargs)
        
        # Expose circuit breaker for inspection
        wrapper.circuit_breaker = breaker
        
        return wrapper
    
    return decorator


# Example usage
async def unreliable_service(fail_rate: float = 0.7) -> str:
    """Simulate an unreliable service."""
    import random
    
    if random.random() < fail_rate:
        raise ConnectionError("Service unavailable")
    
    return "Success!"


async def main():
    """Demonstrate circuit breaker."""
    
    # Create circuit breaker
    breaker = CircuitBreaker(
        name="my-service",
        config=CircuitBreakerConfig(
            failure_threshold=3,
            recovery_timeout=5,
            success_threshold=2,
        ),
    )
    
    print("Circuit Breaker Demo")
    print("="*40)
    
    # Make calls
    for i in range(20):
        try:
            result = await breaker.call(unreliable_service)
            print(f"Call {i+1}: Success - {result}")
        except CircuitBreakerOpen as e:
            print(f"Call {i+1}: Rejected - {e}")
        except ConnectionError as e:
            print(f"Call {i+1}: Failed - {e}")
        
        await asyncio.sleep(0.5)
        
        # Print state periodically
        if i % 5 == 4:
            stats = breaker.get_stats()
            print(f"  State: {stats['state']}, Failures: {stats['failure_count']}")
            print(f"  Metrics: {stats['metrics']}")
    
    print("\nFinal stats:", breaker.get_stats())


if __name__ == "__main__":
    asyncio.run(main())