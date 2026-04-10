"""
Circuit Breaker Wrapper

This module provides a circuit breaker pattern implementation for:
- Failing fast when a service is unavailable
- Preventing cascade failures
- Automatic recovery when service recovers
- Monitoring and logging

The circuit breaker has three states:
- CLOSED: Normal operation, requests pass through
- OPEN: Service is failing, requests fail immediately
- HALF_OPEN: Testing if service has recovered

Usage:
    @circuit_breaker(fail_threshold=5, recovery_timeout=30)
    async def call_external_service():
        # Your service call
        pass
"""

import asyncio
import logging
import time
from typing import Any, Callable, Dict, Optional, TypeVar, Generic
from dataclasses import dataclass, field
from enum import Enum
from functools import wraps

logger = logging.getLogger(__name__)

T = TypeVar('T')


class CircuitState(Enum):
    """Circuit breaker states."""
    CLOSED = "closed"      # Normal operation
    OPEN = "open"          # Failing, reject requests
    HALF_OPEN = "half_open"  # Testing recovery


@dataclass
class CircuitBreakerConfig:
    """Configuration for circuit breaker."""
    fail_threshold: int = 5          # Failures before opening circuit
    success_threshold: int = 3       # Successes in half-open before closing
    recovery_timeout: float = 30.0   # Seconds before trying half-open
    half_open_max_calls: int = 3     # Max concurrent calls in half-open
    excluded_exceptions: tuple = ()  # Exceptions that don't count as failures


class CircuitBreakerError(Exception):
    """Exception raised when circuit is open."""
    
    def __init__(self, message: str, state: CircuitState):
        self.message = message
        self.state = state
        super().__init__(self.message)


class CircuitBreaker(Generic[T]):
    """
    Circuit breaker implementation for protecting services from cascade failures.
    
    The circuit breaker monitors failures and opens the circuit when a threshold
    is reached. This prevents further requests to a failing service and allows
    it time to recover.
    
    Attributes:
        name: Name identifier for this circuit breaker
        config: Configuration parameters
    """
    
    def __init__(self, name: str, config: Optional[CircuitBreakerConfig] = None):
        """
        Initialize circuit breaker.
        
        Args:
            name: Identifier for this circuit
            config: Configuration parameters
        """
        self.name = name
        self.config = config or CircuitBreakerConfig()
        
        self._state = CircuitState.CLOSED
        self._failure_count = 0
        self._success_count = 0
        self._last_failure_time: Optional[float] = None
        self._half_open_calls = 0
        
        # Statistics
        self._total_calls = 0
        self._total_failures = 0
        self._total_successes = 0
    
    @property
    def state(self) -> CircuitState:
        """Get current circuit state, checking for recovery timeout."""
        if self._state == CircuitState.OPEN:
            # Check if recovery timeout has passed
            if self._last_failure_time:
                elapsed = time.time() - self._last_failure_time
                if elapsed >= self.config.recovery_timeout:
                    logger.info(f"Circuit {self.name}: Opening half-open after {elapsed:.1f}s")
                    self._state = CircuitState.HALF_OPEN
                    self._half_open_calls = 0
                    self._success_count = 0
        
        return self._state
    
    @property
    def stats(self) -> Dict[str, Any]:
        """Get circuit breaker statistics."""
        return {
            "name": self.name,
            "state": self.state.value,
            "failure_count": self._failure_count,
            "success_count": self._success_count,
            "total_calls": self._total_calls,
            "total_failures": self._total_failures,
            "total_successes": self._total_successes,
        }
    
    def _can_execute(self) -> bool:
        """Check if a request can be executed."""
        if self.state == CircuitState.CLOSED:
            return True
        
        if self.state == CircuitState.HALF_OPEN:
            return self._half_open_calls < self.config.half_open_max_calls
        
        return False
    
    def _record_success(self):
        """Record a successful call."""
        self._total_calls += 1
        self._total_successes += 1
        
        if self._state == CircuitState.HALF_OPEN:
            self._success_count += 1
            self._half_open_calls = max(0, self._half_open_calls - 1)
            
            if self._success_count >= self.config.success_threshold:
                logger.info(f"Circuit {self.name}: Closing after {self._success_count} successes")
                self._state = CircuitState.CLOSED
                self._failure_count = 0
                self._success_count = 0
        
        elif self._state == CircuitState.CLOSED:
            # Reset failure count on success
            self._failure_count = 0
    
    def _record_failure(self, exception: Exception):
        """Record a failed call."""
        self._total_calls += 1
        self._total_failures += 1
        
        # Check if exception should be excluded
        if isinstance(exception, self.config.excluded_exceptions):
            logger.debug(f"Circuit {self.name}: Exception {type(exception).__name__} excluded")
            return
        
        self._failure_count += 1
        self._last_failure_time = time.time()
        
        if self._state == CircuitState.HALF_OPEN:
            # Any failure in half-open reopens the circuit
            logger.warning(f"Circuit {self.name}: Reopening after failure in half-open")
            self._state = CircuitState.OPEN
            self._half_open_calls = 0
            self._success_count = 0
        
        elif self._state == CircuitState.CLOSED:
            if self._failure_count >= self.config.fail_threshold:
                logger.warning(
                    f"Circuit {self.name}: Opening after {self._failure_count} failures"
                )
                self._state = CircuitState.OPEN
    
    async def execute(self, func: Callable[..., Awaitable[T]], *args, **kwargs) -> T:
        """
        Execute a function with circuit breaker protection.
        
        Args:
            func: Async function to execute
            *args: Positional arguments
            **kwargs: Keyword arguments
            
        Returns:
            Result from the function
            
        Raises:
            CircuitBreakerError: If circuit is open
        """
        if not self._can_execute():
            raise CircuitBreakerError(
                f"Circuit {self.name} is {self.state.value}",
                self.state
            )
        
        if self._state == CircuitState.HALF_OPEN:
            self._half_open_calls += 1
        
        try:
            result = await func(*args, **kwargs)
            self._record_success()
            return result
            
        except Exception as e:
            self._record_failure(e)
            raise
    
    def reset(self):
        """Manually reset the circuit breaker."""
        logger.info(f"Circuit {self.name}: Manual reset")
        self._state = CircuitState.CLOSED
        self._failure_count = 0
        self._success_count = 0
        self._last_failure_time = None
        self._half_open_calls = 0


# Global circuit breaker registry
_circuit_breakers: Dict[str, CircuitBreaker] = {}


def get_circuit_breaker(name: str, config: Optional[CircuitBreakerConfig] = None) -> CircuitBreaker:
    """
    Get or create a circuit breaker by name.
    
    Args:
        name: Circuit breaker identifier
        config: Optional configuration
        
    Returns:
        Circuit breaker instance
    """
    if name not in _circuit_breakers:
        _circuit_breakers[name] = CircuitBreaker(name, config)
    return _circuit_breakers[name]


def circuit_breaker(
    fail_threshold: int = 5,
    recovery_timeout: float = 30.0,
    success_threshold: int = 3,
    name: Optional[str] = None,
    excluded_exceptions: tuple = (),
):
    """
    Decorator to add circuit breaker protection to an async function.
    
    Args:
        fail_threshold: Number of failures before opening circuit
        recovery_timeout: Seconds to wait before trying half-open
        success_threshold: Successes needed in half-open to close
        name: Optional name (defaults to function name)
        excluded_exceptions: Exceptions that don't count as failures
        
    Returns:
        Decorated function
        
    Example:
        @circuit_breaker(name="payment-service", fail_threshold=3)
        async def process_payment(payment_data):
            # Call payment service
            pass
    """
    config = CircuitBreakerConfig(
        fail_threshold=fail_threshold,
        recovery_timeout=recovery_timeout,
        success_threshold=success_threshold,
        excluded_exceptions=excluded_exceptions,
    )
    
    def decorator(func: Callable) -> Callable:
        cb_name = name or func.__name__
        
        @wraps(func)
        async def wrapper(*args, **kwargs):
            cb = get_circuit_breaker(cb_name, config)
            return await cb.execute(func, *args, **kwargs)
        
        # Add circuit breaker access to wrapper
        wrapper.circuit_breaker = lambda: get_circuit_breaker(cb_name)
        
        return wrapper
    
    return decorator


# Example usage
async def unreliable_service_call() -> Dict[str, str]:
    """Simulate an unreliable external service call."""
    import random
    
    # 70% chance of failure
    if random.random() < 0.7:
        raise ConnectionError("Service unavailable")
    
    return {"status": "success", "data": "Some data"}


@circuit_breaker(
    name="unreliable-service",
    fail_threshold=3,
    recovery_timeout=10,
    success_threshold=2,
)
async def call_service_with_circuit_breaker() -> Dict[str, str]:
    """Example function protected by circuit breaker."""
    return await unreliable_service_call()


async def main():
    """Demonstrate circuit breaker usage."""
    
    # Make multiple calls to see circuit breaker in action
    for i in range(20):
        try:
            result = await call_service_with_circuit_breaker()
            logger.info(f"Call {i+1}: Success - {result}")
        except CircuitBreakerError as e:
            logger.warning(f"Call {i+1}: Circuit {e.state.value} - {e.message}")
        except ConnectionError as e:
            logger.error(f"Call {i+1}: Service error - {e}")
        
        await asyncio.sleep(0.5)
    
    # Print final statistics
    cb = get_circuit_breaker("unreliable-service")
    logger.info(f"Final stats: {cb.stats}")


if __name__ == "__main__":
    asyncio.run(main())
