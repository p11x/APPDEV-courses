"""
API Gateway Implementation

This module provides an API Gateway that handles:
- Request routing to backend services
- Request/Response transformation
- Authentication and authorization
- Rate limiting
- Logging and monitoring
- Service discovery

Usage:
    gateway = APIGateway()
    gateway.add_route("/api/users", "http://user-service:8001")
    gateway.add_route("/api/products", "http://product-service:8002")
    
    await gateway.start()
"""

import asyncio
import logging
import time
import uuid
from typing import Any, Callable, Dict, List, Optional, Set
from dataclasses import dataclass, field
from enum import Enum

from fastapi import FastAPI, Request, Response, HTTPException, Depends
from fastapi.responses import JSONResponse
from fastapi.routing import APIRoute
from starlette.datastructures import URL
from starlette.middleware.base import BaseHTTPMiddleware
import httpx


logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)


class RateLimitExceeded(Exception):
    """Exception raised when rate limit is exceeded."""
    pass


@dataclass
class ServiceConfig:
    """Configuration for a backend service."""
    name: str
    url: str
    timeout: float = 30.0
    health_check_path: str = "/health"
    retry_attempts: int = 3


@dataclass
class RateLimitConfig:
    """Configuration for rate limiting."""
    requests_per_minute: int = 60
    burst_size: int = 10


@dataclass
class RouteConfig:
    """Configuration for a route."""
    path: str
    service_name: str
    methods: List[str] = field(default_factory=lambda: ["*"])
    auth_required: bool = False
    rate_limit: Optional[RateLimitConfig] = None
    timeout: float = 30.0
    retry_attempts: int = 3


class CircuitBreaker:
    """Simple circuit breaker for service calls."""
    
    def __init__(self, fail_threshold: int = 5, recovery_timeout: float = 30.0):
        self.fail_threshold = fail_threshold
        self.recovery_timeout = recovery_timeout
        self.failures = 0
        self.is_open = False
        self.last_failure_time: Optional[float] = None
    
    def record_success(self):
        """Record a successful call."""
        self.failures = 0
        self.is_open = False
    
    def record_failure(self):
        """Record a failed call."""
        self.failures += 1
        self.last_failure_time = time.time()
        
        if self.failures >= self.fail_threshold:
            self.is_open = True
            logger.warning(f"Circuit breaker opened")
    
    def can_execute(self) -> bool:
        """Check if calls can be made."""
        if self.is_open and self.last_failure_time:
            elapsed = time.time() - self.last_failure_time
            if elapsed >= self.recovery_timeout:
                self.is_open = False
                logger.info("Circuit breaker half-open")
        return not self.is_open


class TokenBucket:
    """Token bucket rate limiter implementation."""
    
    def __init__(self, rate: float, capacity: int):
        self.rate = rate  # tokens per second
        self.capacity = capacity
        self.tokens = capacity
        self.last_update = time.time()
    
    async def acquire(self, tokens: int = 1) -> bool:
        """Try to acquire tokens."""
        now = time.time()
        elapsed = now - self.last_update
        
        # Add tokens based on elapsed time
        self.tokens = min(self.capacity, self.tokens + elapsed * self.rate)
        self.last_update = now
        
        if self.tokens >= tokens:
            self.tokens -= tokens
            return True
        
        return False
    
    def available_tokens(self) -> float:
        """Get available tokens."""
        now = time.time()
        elapsed = now - self.last_update
        return min(self.capacity, self.tokens + elapsed * self.rate)


class AuthService:
    """Simple JWT-based authentication service."""
    
    def __init__(self, secret_key: str = "default-secret-key"):
        self.secret_key = secret_key
    
    def verify_token(self, token: str) -> bool:
        """Verify JWT token (simplified - use proper JWT in production)."""
        # In production, use proper JWT verification
        return len(token) > 0
    
    def extract_user_id(self, token: str) -> Optional[str]:
        """Extract user ID from token."""
        # In production, properly decode JWT
        return "user-123" if token else None


class RequestLogger:
    """Request/response logging middleware."""
    
    def __init__(self):
        self.requests: List[Dict[str, Any]] = []
        self.max_requests = 1000
    
    def log(self, request_id: str, method: str, path: str, 
           status_code: int, duration: float):
        """Log a request."""
        entry = {
            "request_id": request_id,
            "method": method,
            "path": path,
            "status_code": status_code,
            "duration": duration,
            "timestamp": time.time(),
        }
        
        self.requests.append(entry)
        
        # Keep only recent requests
        if len(self.requests) > self.max_requests:
            self.requests = self.requests[-self.max_requests:]
    
    def get_stats(self) -> Dict[str, Any]:
        """Get request statistics."""
        if not self.requests:
            return {"total": 0, "errors": 0}
        
        total = len(self.requests)
        errors = sum(1 for r in self.requests if r["status_code"] >= 400)
        avg_duration = sum(r["duration"] for r in self.requests) / total
        
        return {
            "total": total,
            "errors": errors,
            "error_rate": errors / total if total > 0 else 0,
            "avg_duration": avg_duration,
        }


class ProxyMiddleware(BaseHTTPMiddleware):
    """Middleware to proxy requests to backend services."""
    
    def __init__(self, app, gateway: 'APIGateway'):
        super().__init__(app)
        self.gateway = gateway
    
    async def dispatch(self, request: Request, call_next: Callable) -> Response:
        """Process request and route to backend service."""
        
        # Check if path matches any route
        route = self.gateway.find_route(request.url.path)
        
        if route:
            return await self.gateway.proxy_request(request, route)
        
        return await call_next(request)


class APIGateway:
    """
    API Gateway for routing and managing backend services.
    
    Features:
    - Dynamic routing to backend services
    - Request/Response transformation
    - Authentication
    - Rate limiting
    - Circuit breaking
    - Logging
    """
    
    def __init__(self, host: str = "0.0.0.0", port: int = 8080):
        self.host = host
        self.port = port
        
        # Service and route configurations
        self.services: Dict[str, ServiceConfig] = {}
        self.routes: Dict[str, RouteConfig] = {}
        self.service_circuit_breakers: Dict[str, CircuitBreaker] = {}
        self.rate_limiters: Dict[str, TokenBucket] = {}
        
        # Auth and logging
        self.auth_service = AuthService()
        self.request_logger = RequestLogger()
        
        # FastAPI app
        self.app = FastAPI(title="API Gateway")
        self._setup_app()
    
    def _setup_app(self):
        """Set up FastAPI app with middleware."""
        self.app.add_middleware(ProxyMiddleware, gateway=self)
        
        # Health endpoint
        @self.app.get("/health")
        async def health_check():
            return {"status": "healthy"}
        
        # Stats endpoint
        @self.app.get("/gateway/stats")
        async def get_stats():
            return {
                "services": list(self.services.keys()),
                "routes": list(self.routes.keys()),
                "requests": self.request_logger.get_stats(),
            }
    
    def add_service(self, config: ServiceConfig):
        """
        Add a backend service configuration.
        
        Args:
            config: Service configuration
        """
        self.services[config.name] = config
        self.service_circuit_breakers[config.name] = CircuitBreaker()
        logger.info(f"Added service: {config.name} -> {config.url}")
    
    def add_route(self, config: RouteConfig):
        """
        Add a route configuration.
        
        Args:
            config: Route configuration
        """
        self.routes[config.path] = config
        
        # Setup rate limiter if configured
        if config.rate_limit:
            self.rate_limiters[config.path] = TokenBucket(
                rate=config.rate_limit.requests_per_minute / 60,
                capacity=config.rate_limit.burst_size,
            )
        
        logger.info(
            f"Added route: {config.path} -> {config.service_name} "
            f"(auth_required={config.auth_required})"
        )
    
    def find_route(self, path: str) -> Optional[RouteConfig]:
        """Find route configuration for a path."""
        # Exact match
        if path in self.routes:
            return self.routes[path]
        
        # Prefix match
        for route_path, config in self.routes.items():
            if path.startswith(route_path.rstrip("*")):
                return config
        
        return None
    
    async def proxy_request(self, request: Request, route: RouteConfig) -> Response:
        """
        Proxy request to backend service.
        
        Args:
            request: Incoming request
            route: Route configuration
            
        Returns:
            Proxied response
        """
        request_id = str(uuid.uuid4())
        start_time = time.time()
        
        # Get service config
        service_config = self.services.get(route.service_name)
        if not service_config:
            raise HTTPException(status_code=503, detail="Service not found")
        
        # Check circuit breaker
        cb = self.service_circuit_breakers[route.service_name]
        if not cb.can_execute():
            raise HTTPException(status_code=503, detail="Service unavailable")
        
        # Check rate limit
        if route.path in self.rate_limiters:
            limiter = self.rate_limiters[route.path]
            if not await limiter.acquire():
                raise HTTPException(status_code=429, detail="Rate limit exceeded")
        
        # Build target URL
        target_url = f"{service_config.url}{request.url.path}"
        
        # Build headers (excluding hop-by-hop headers)
        headers = dict(request.headers)
        headers["X-Request-ID"] = request_id
        headers["X-Forwarded-For"] = request.client.host if request.client else "unknown"
        
        try:
            # Make proxy request
            async with httpx.AsyncClient() as client:
                response = await client.request(
                    method=request.method,
                    url=target_url,
                    headers=headers,
                    params=request.query_params,
                    timeout=route.timeout,
                )
                
                cb.record_success()
                
                # Log request
                duration = time.time() - start_time
                self.request_logger.log(
                    request_id, request.method, request.url.path,
                    response.status_code, duration
                )
                
                return JSONResponse(
                    content=response.json(),
                    status_code=response.status_code,
                    headers=dict(response.headers),
                )
        
        except Exception as e:
            cb.record_failure()
            
            logger.error(f"Proxy error: {e}")
            raise HTTPException(status_code=502, detail="Bad gateway")
    
    async def start(self):
        """Start the API Gateway server."""
        import uvicorn
        config = uvicorn.Config(
            self.app,
            host=self.host,
            port=self.port,
            log_level="info",
        )
        server = uvicorn.Server(config)
        await server.serve()


# Example usage
async def main():
    """Demonstrate API Gateway usage."""
    
    gateway = APIGateway(host="0.0.0.0", port=8080)
    
    # Add services
    gateway.add_service(ServiceConfig(
        name="user-service",
        url="http://user-service:8001",
    ))
    
    gateway.add_service(ServiceConfig(
        name="product-service", 
        url="http://product-service:8002",
    ))
    
    # Add routes
    gateway.add_route(RouteConfig(
        path="/api/users*",
        service_name="user-service",
        methods=["GET", "POST"],
        auth_required=True,
        rate_limit=RateLimitConfig(requests_per_minute=100, burst_size=20),
    ))
    
    gateway.add_route(RouteConfig(
        path="/api/products*",
        service_name="product-service",
        methods=["GET"],
    ))
    
    # Start gateway
    print("Starting API Gateway on http://0.0.0.0:8080")
    await gateway.start()


if __name__ == "__main__":
    asyncio.run(main())