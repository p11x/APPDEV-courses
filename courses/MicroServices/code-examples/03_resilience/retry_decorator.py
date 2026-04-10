"""
Retry Decorator with Backoff

This module provides retry logic with various backoff strategies:
- Fixed backoff
- Exponential backoff
- Jitter (randomization)
- Decorator and context manager versions

Usage:
    @retry(max_attempts=3, backoff=exponential_backoff(min=1, max=10))
    async def unreliable_operation():
        # May fail
        pass
"""

import asyncio
import logging
import random
import time
from typing import Any, Callable, Optional, Tuple, Type
from dataclasses import dataclass
from functools import wraps


logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)


# Backoff strategies
def fixed_backoff(delay: float = 1.0) -> Callable[[int], float]:
    """
    Fixed backoff - same delay between retries.
    
    Args:
        delay: Fixed delay in seconds
        
    Returns:
        Function that returns delay
    """
    def backoff(attempt: int) -> float:
        return delay
    return backoff


def exponential_backoff(
    multiplier: float = 1.0,
    min_delay: float = 1.0,
    max_delay: float = 60.0,
) -> Callable[[int], float]:
    """
    Exponential backoff - delay doubles each attempt.
    
    Args:
        multiplier: Multiplier for exponential calculation
        min_delay: Minimum delay in seconds
        max_delay: Maximum delay in seconds
        
    Returns:
        Function that returns delay
    """
    def backoff(attempt: int) -> float:
        delay = min_delay * (multiplier ** attempt)
        return min(delay, max_delay)
    return backoff


def linear_backoff(
    increment: float = 1.0,
    min_delay: float = 1.0,
    max_delay: float = 60.0,
) -> Callable[[int], float]:
    """
    Linear backoff - delay increases linearly.
    
    Args:
        increment: Increment in seconds per attempt
        min_delay: Minimum delay in seconds
        max_delay: Maximum delay in seconds
        
    Returns:
        Function that returns delay
    """
    def backoff(attempt: int) -> float:
        delay = min_delay + (increment * attempt)
        return min(delay, max_delay)
    return backoff


def add_jitter(
    backoff_fn: Callable[[int], float],
    jitter: float = 0.1,
) -> Callable[[int], float]:
    """
    Add randomization (jitter) to a backoff function.
    
    Args:
        backoff_fn: Base backoff function
        jitter: Jitter factor (0-1)
        
    Returns:
        Backoff function with jitter
    """
    def backoff(attempt: int) -> float:
        base_delay = backoff_fn(attempt)
        jitter_amount = base_delay * jitter * random.uniform(-1, 1)
        return max(0, base_delay + jitter_amount)
    return backoff


@dataclass
class RetryConfig:
    """Configuration for retry behavior."""
    max_attempts: int = 3
    backoff_fn: Callable[[int], float] = field(
        default_factory=lambda: exponential_backoff()
    )
    exceptions: Tuple[Type[Exception], ...] = (Exception,)
    should_retry: Optional[Callable[[Exception], bool]] = None
    on_retry: Optional[Callable[[int, Exception], None]] = None


def retry(
    max_attempts: int = 3,
    backoff: Optional[Callable[[int], float]] = None,
    exceptions: Tuple[Type[Exception], ...] = (Exception,),
    should_retry: Optional[Callable[[Exception], bool]] = None,
    on_retry: Optional[Callable[[int, Exception], None]] = None,
):
    """
    Decorator to retry failed async/sync operations.
    
    Args:
        max_attempts: Maximum number of attempts
        backoff: Backoff function (default: exponential)
        exceptions: Exceptions to catch and retry
        should_retry: Custom function to decide if retry is needed
        on_retry: Callback function called on each retry
        
    Usage:
        @retry(max_attempts=3, backoff=exponential_backoff())
        async def unreliable_call():
            # May fail
            pass
            
        @retry(max_attempts=5, should_retry=lambda e: e.status_code >= 500)
        async def api_call():
            # Retry on server errors
            pass
    """
    if backoff is None:
        backoff = exponential_backoff()
    
    config = RetryConfig(
        max_attempts=max_attempts,
        backoff_fn=backoff,
        exceptions=exceptions,
        should_retry=should_retry,
        on_retry=on_retry,
    )
    
    def decorator(func: Callable) -> Callable:
        @wraps(func)
        async def async_wrapper(*args, **kwargs):
            return await _retry_async(func, config, *args, **kwargs)
        
        @wraps(func)
        def sync_wrapper(*args, **kwargs):
            return _retry_sync(func, config, *args, **kwargs)
        
        # Return appropriate wrapper based on function type
        if asyncio.iscoroutinefunction(func):
            return async_wrapper
        return sync_wrapper
    
    return decorator


async def _retry_async(
    func: Callable,
    config: RetryConfig,
    *args,
    **kwargs,
) -> Any:
    """Async retry implementation."""
    last_exception: Optional[Exception] = None
    
    for attempt in range(1, config.max_attempts + 1):
        try:
            return await func(*args, **kwargs)
        
        except config.exceptions as e:
            last_exception = e
            
            # Check custom retry condition
            if config.should_retry and not config.should_retry(e):
                logger.info(f"Not retrying: {e}")
                raise
            
            # Check if this was the last attempt
            if attempt == config.max_attempts:
                logger.error(
                    f"All {config.max_attempts} attempts failed. "
                    f"Last error: {e}"
                )
                raise
            
            # Calculate delay
            delay = config.backoff_fn(attempt)
            
            # Call retry callback
            if config.on_retry:
                config.on_retry(attempt, e)
            
            logger.warning(
                f"Attempt {attempt}/{config.max_attempts} failed: {e}. "
                f"Retrying in {delay:.2f}s..."
            )
            
            await asyncio.sleep(delay)
    
    # Should not reach here, but just in case
    if last_exception:
        raise last_exception


def _retry_sync(
    func: Callable,
    config: RetryConfig,
    *args,
    **kwargs,
) -> Any:
    """Sync retry implementation."""
    last_exception: Optional[Exception] = None
    
    for attempt in range(1, config.max_attempts + 1):
        try:
            return func(*args, **kwargs)
        
        except config.exceptions as e:
            last_exception = e
            
            # Check custom retry condition
            if config.should_retry and not config.should_retry(e):
                logger.info(f"Not retrying: {e}")
                raise
            
            # Check if this was the last attempt
            if attempt == config.max_attempts:
                logger.error(
                    f"All {config.max_attempts} attempts failed. "
                    f"Last error: {e}"
                )
                raise
            
            # Calculate delay
            delay = config.backoff_fn(attempt)
            
            # Call retry callback
            if config.on_retry:
                config.on_retry(attempt, e)
            
            logger.warning(
                f"Attempt {attempt}/{config.max_attempts} failed: {e}. "
                f"Retrying in {delay:.2f}s..."
            )
            
            time.sleep(delay)
    
    if last_exception:
        raise last_exception


class RetryContext:
    """
    Context manager for retry operations.
    
    Usage:
        async with RetryContext(max_attempts=3) as ctx:
            await unreliable_operation()
            
        if ctx.last_exception:
            print(f"Failed: {ctx.last_exception}")
    """
    
    def __init__(
        self,
        max_attempts: int = 3,
        backoff: Optional[Callable[[int], float]] = None,
        exceptions: Tuple[Type[Exception], ...] = (Exception,),
    ):
        self.config = RetryConfig(
            max_attempts=max_attempts,
            backoff_fn=backoff or exponential_backoff(),
            exceptions=exceptions,
        )
        self.last_exception: Optional[Exception] = None
        self.attempts: int = 0
        self.result: Any = None
    
    async def __aenter__(self):
        return self
    
    async def __aexit__(self, exc_type, exc_val, exc_tb):
        if exc_val:
            self.last_exception = exc_val
    
    async def execute(self, func: Callable, *args, **kwargs) -> Any:
        """Execute a function with retry logic."""
        self.attempts = 0
        self.last_exception = None
        
        for attempt in range(1, self.config.max_attempts + 1):
            self.attempts = attempt
            
            try:
                if asyncio.iscoroutinefunction(func):
                    self.result = await func(*args, **kwargs)
                else:
                    self.result = func(*args, **kwargs)
                
                return self.result
            
            except self.config.exceptions as e:
                self.last_exception = e
                
                if attempt == self.config.max_attempts:
                    raise
                
                delay = self.config.backoff_fn(attempt)
                logger.warning(f"Retry {attempt}/{self.config.max_attempts}, waiting {delay:.2f}s")
                await asyncio.sleep(delay)
        
        raise self.last_exception


# Example usage
async def unreliable_service(fail_count: int = 3) -> str:
    """Simulate an unreliable service that fails N times."""
    if fail_count > 0:
        raise ConnectionError(f"Failed (remaining: {fail_count})")
    return "Success!"


async def main():
    """Demonstrate retry decorator."""
    
    # Example 1: Basic retry
    print("Example 1: Basic retry")
    
    @retry(max_attempts=5, backoff=exponential_backoff())
    async def call_with_retry():
        return await unreliable_service(fail_count=3)
    
    try:
        result = await call_with_retry()
        print(f"Result: {result}")
    except Exception as e:
        print(f"Failed: {e}")
    
    # Example 2: Custom backoff with jitter
    print("\nExample 2: Custom backoff with jitter")
    
    custom_backoff = add_jitter(
        exponential_backoff(multiplier=2, min_delay=0.5, max_delay=10),
        jitter=0.3,
    )
    
    @retry(max_attempts=5, backoff=custom_backoff)
    async def call_with_jitter():
        return await unreliable_service(fail_count=2)
    
    result = await call_with_jitter()
    print(f"Result: {result}")
    
    # Example 3: Custom retry condition
    print("\nExample 3: Custom retry condition")
    
    class APIError(Exception):
        def __init__(self, status_code: int, message: str):
            self.status_code = status_code
            super().__init__(message)
    
    @retry(
        max_attempts=3,
        should_retry=lambda e: (
            isinstance(e, APIError) and e.status_code >= 500
        ),
    )
    async def call_with_condition():
        # Simulate 502 error
        raise APIError(502, "Bad Gateway")
    
    try:
        await call_with_condition()
    except APIError as e:
        print(f"Final error (not retried): {e}")
    
    # Example 4: With retry callback
    print("\nExample 4: With retry callback")
    
    @retry(
        max_attempts=5,
        on_retry=lambda attempt, error: print(f"  Retry {attempt}: {error}"),
    )
    async def call_with_callback():
        return await unreliable_service(fail_count=3)
    
    result = await call_with_callback()
    print(f"Result: {result}")
    
    # Example 5: Context manager
    print("\nExample 5: Context manager")
    
    async with RetryContext(max_attempts=3) as ctx:
        result = await ctx.execute(unreliable_service, 2)
        print(f"Result: {result}")
    
    print(f"Attempts: {ctx.attempts}, Last error: {ctx.last_exception}")


if __name__ == "__main__":
    asyncio.run(main())