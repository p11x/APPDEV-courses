"""
Bulkhead Pattern Implementation

This module provides a bulkhead pattern for isolating resources:
- Thread pool bulkhead (semaphore-based)
- Process isolation (separate pools)
- Fallback handling
- Timeout support

Usage:
    bulkhead = Bulkhead(max_workers=10, max_queue=50)
    
    result = await bulkhead.execute(unreliable_function)
"""

import asyncio
import logging
import time
from typing import Any, Callable, Dict, Optional, TypeVar
from dataclasses import dataclass, field
from enum import Enum
from functools import wraps
import threading


logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)


T = TypeVar('T')


class BulkheadError(Exception):
    """Exception raised when bulkhead is exhausted."""
    pass


@dataclass
class BulkheadMetrics:
    """Metrics for bulkhead monitoring."""
    total_calls: int = 0
    successful_calls: int = 0
    rejected_calls: int = 0
    timed_out_calls: int = 0
    total_execution_time: float = 0.0
    
    @property
    def rejection_rate(self) -> float:
        if self.total_calls == 0:
            return 0.0
        return self.rejected_calls / self.total_calls
    
    @property
    def avg_execution_time(self) -> float:
        if self.total_calls == 0:
            return 0.0
        return self.total_execution_time / self.total_calls


class SemaphoreBulkhead:
    """
    Semaphore-based bulkhead for controlling concurrent access.
    
    The bulkhead limits the number of concurrent executions to protect
    downstream services from being overwhelmed.
    
    Attributes:
        max_concurrent: Maximum concurrent executions
        max_queue: Maximum queue size for waiting calls
        timeout: Timeout for waiting in queue
    """
    
    def __init__(
        self,
        max_concurrent: int = 10,
        max_queue: int = 50,
        timeout: float = 30.0,
    ):
        """
        Initialize bulkhead.
        
        Args:
            max_concurrent: Maximum concurrent executions
            max_queue: Maximum queue size
            timeout: Wait timeout in seconds
        """
        self.max_concurrent = max_concurrent
        self.max_queue = max_queue
        self.timeout = timeout
        
        # Semaphore for controlling concurrency
        self._semaphore = asyncio.Semaphore(max_concurrent)
        
        # Queue for waiting calls
        self._queue: asyncio.Queue = asyncio.Queue(maxsize=max_queue)
        
        # Metrics
        self.metrics = BulkheadMetrics()
        
        # Active calls tracking
        self._active_calls: int = 0
        self._lock = asyncio.Lock()
    
    async def execute(
        self,
        func: Callable[..., Any],
        *args,
        **kwargs,
    ) -> Any:
        """
        Execute a function with bulkhead protection.
        
        Args:
            func: Async function to execute
            *args: Positional arguments
            **kwargs: Keyword arguments
            
        Returns:
            Result from function
            
        Raises:
            BulkheadError: If rejected or timeout
        """
        self.metrics.total_calls += 1
        start_time = time.time()
        
        # Try to acquire semaphore
        try:
            async with asyncio.timeout(self.timeout):
                await self._semaphore.acquire()
                
                async with self._lock:
                    self._active_calls += 1
                
                try:
                    # Execute the function
                    if asyncio.iscoroutinefunction(func):
                        result = await func(*args, **kwargs)
                    else:
                        result = func(*args, **kwargs)
                    
                    self.metrics.successful_calls += 1
                    return result
                
                finally:
                    async with self._lock:
                        self._active_calls -= 1
                    self._semaphore.release()
        
        except asyncio.TimeoutError:
            self.metrics.rejected_calls += 1
            self.metrics.timed_out_calls += 1
            raise BulkheadError(
                f"Bulkhead timeout after {self.timeout}s "
                f"(max_concurrent={self.max_concurrent}, active={self._active_calls})"
            )
        
        except asyncio.QueueFull:
            self.metrics.rejected_calls += 1
            raise BulkheadError(
                f"Bulkhead queue full (max_queue={self.max_queue})"
            )
        
        finally:
            self.metrics.total_execution_time += time.time() - start_time
    
    async def get_status(self) -> Dict[str, Any]:
        """Get current bulkhead status."""
        return {
            "max_concurrent": self.max_concurrent,
            "active_calls": self._active_calls,
            "available_slots": self.max_concurrent - self._active_calls,
            "queue_size": self._queue.qsize(),
            "max_queue": self.max_queue,
            "metrics": {
                "total_calls": self.metrics.total_calls,
                "successful_calls": self.metrics.successful_calls,
                "rejected_calls": self.metrics.rejected_calls,
                "rejection_rate": self.metrics.rejection_rate,
            },
        }


class ThreadPoolBulkhead:
    """
    Thread pool-based bulkhead for sync operations.
    
    This bulkhead uses a thread pool to limit concurrent executions
    and provides isolation between different operations.
    """
    
    def __init__(
        self,
        max_workers: int = 10,
        max_queue: int = 50,
    ):
        """
        Initialize bulkhead with thread pool.
        
        Args:
            max_workers: Maximum number of worker threads
            max_queue: Maximum queue size
        """
        from concurrent.futures import ThreadPoolExecutor, TimeoutFuture
        
        self.max_workers = max_workers
        self.max_queue = max_queue
        
        self._executor = ThreadPoolExecutor(max_workers=max_workers)
        
        self.metrics = BulkheadMetrics()
        self._active_count = 0
        self._lock = threading.Lock()
    
    def execute(
        self,
        func: Callable[..., Any],
        *args,
        **kwargs,
    ) -> Any:
        """
        Execute a function with bulkhead protection.
        
        Args:
            func: Function to execute
            *args: Positional arguments
            **kwargs: Keyword arguments
            
        Returns:
            Result from function
        """
        self.metrics.total_calls += 1
        
        try:
            future = self._executor.submit(func, *args, **kwargs)
            result = future.result(timeout=30.0)
            self.metrics.successful_calls += 1
            return result
        
        except Exception as e:
            self.metrics.rejected_calls += 1
            raise BulkheadError(f"Bulkhead execution failed: {e}") from e
    
    def shutdown(self, wait: bool = True):
        """Shutdown the thread pool."""
        self._executor.shutdown(wait=wait)
    
    def get_status(self) -> Dict[str, Any]:
        """Get current bulkhead status."""
        return {
            "max_workers": self.max_workers,
            "active_count": self._active_count,
            "metrics": {
                "total_calls": self.metrics.total_calls,
                "successful_calls": self.metrics.successful_calls,
                "rejected_calls": self.metrics.rejected_calls,
            },
        }


class BulkheadRegistry:
    """
    Registry for managing multiple bulkheads.
    
    Allows creating and reusing bulkheads for different services.
    """
    
    def __init__(self):
        self._bulkheads: Dict[str, SemaphoreBulkhead] = {}
    
    def get_or_create(
        self,
        name: str,
        max_concurrent: int = 10,
        max_queue: int = 50,
        timeout: float = 30.0,
    ) -> SemaphoreBulkhead:
        """Get or create a bulkhead by name."""
        if name not in self._bulkheads:
            self._bulkheads[name] = SemaphoreBulkhead(
                max_concurrent=max_concurrent,
                max_queue=max_queue,
                timeout=timeout,
            )
        return self._bulkheads[name]
    
    def get_all_status(self) -> Dict[str, Dict[str, Any]]:
        """Get status of all bulkheads."""
        return {
            name: bulkhead.get_status()
            for name, bulkhead in self._bulkheads.items()
        }


# Global registry
_bulkhead_registry = BulkheadRegistry()


def get_bulkhead(
    name: str,
    max_concurrent: int = 10,
    max_queue: int = 50,
    timeout: float = 30.0,
) -> SemaphoreBulkhead:
    """Get a bulkhead from the global registry."""
    return _bulkhead_registry.get_or_create(
        name, max_concurrent, max_queue, timeout
    )


def bulkhead(
    max_concurrent: int = 10,
    max_queue: int = 50,
    timeout: float = 30.0,
    name: Optional[str] = None,
):
    """
    Decorator to add bulkhead protection to async functions.
    
    Args:
        max_concurrent: Maximum concurrent executions
        max_queue: Maximum queue size
        timeout: Wait timeout in seconds
        name: Optional bulkhead name (defaults to function name)
        
    Usage:
        @bulkhead(max_concurrent=5, timeout=10)
        async def call_service():
            # May be limited
            pass
    """
    def decorator(func: Callable) -> Callable:
        bulkhead_name = name or f"bulkhead-{func.__name__}"
        bulkhead = get_bulkhead(
            bulkhead_name,
            max_concurrent=max_concurrent,
            max_queue=max_queue,
            timeout=timeout,
        )
        
        @wraps(func)
        async def wrapper(*args, **kwargs):
            return await bulkhead.execute(func, *args, **kwargs)
        
        # Expose bulkhead for inspection
        wrapper.bulkhead = bulkhead
        
        return wrapper
    
    return decorator


# Example usage
async def mock_service_call(delay: float = 0.1) -> str:
    """Simulate a service call."""
    await asyncio.sleep(delay)
    return f"Completed after {delay}s"


async def unreliable_service() -> str:
    """Simulate unreliable service."""
    import random
    if random.random() < 0.3:
        raise ConnectionError("Service unavailable")
    return "Success"


async def main():
    """Demonstrate bulkhead pattern."""
    
    # Example 1: Basic bulkhead
    print("Example 1: Basic bulkhead")
    
    bulkhead = SemaphoreBulkhead(max_concurrent=3, max_queue=5, timeout=5.0)
    
    # Try to execute more than max_concurrent
    tasks = []
    for i in range(10):
        task = bulkhead.execute(mock_service_call, delay=0.2)
        tasks.append(task)
    
    results = await asyncio.gather(*tasks, return_exceptions=True)
    
    success = sum(1 for r in results if not isinstance(r, Exception))
    rejected = sum(1 for r in results if isinstance(r, BulkheadError))
    
    print(f"Success: {success}, Rejected: {rejected}")
    print(f"Status: {await bulkhead.get_status()}")
    
    # Example 2: Decorator usage
    print("\nExample 2: Decorator usage")
    
    @bulkhead(max_concurrent=2, timeout=2.0, name="my-service")
    async def call_service():
        await asyncio.sleep(0.5)
        return "Done"
    
    # Make multiple calls
    tasks = [call_service() for _ in range(5)]
    results = await asyncio.gather(*tasks, return_exceptions=True)
    
    success = sum(1 for r in results if not isinstance(r, Exception))
    print(f"Success: {success}, Failed: {len(results) - success}")
    
    # Example 3: Bulkhead with fallback
    print("\nExample 3: Bulkhead with fallback")
    
    async def call_with_fallback():
        try:
            return await bulkhead.execute(unreliable_service)
        except BulkheadError:
            return "Fallback: Bulkhead full"
    
    for i in range(3):
        result = await call_with_fallback()
        print(f"Call {i+1}: {result}")
    
    # Example 4: Registry usage
    print("\nExample 4: Registry usage")
    
    payment_bulkhead = get_bulkhead("payment-service", max_concurrent=5)
    order_bulkhead = get_bulkhead("order-service", max_concurrent=10)
    
    print(f"Payment bulkhead: {payment_bulkhead.get_status()}")
    print(f"Order bulkhead: {order_bulkhead.get_status()}")


if __name__ == "__main__":
    asyncio.run(main())