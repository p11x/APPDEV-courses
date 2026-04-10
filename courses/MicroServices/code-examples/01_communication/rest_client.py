"""
REST Client Implementation using Python/FastAPI

This module provides a robust REST client with:
- Connection pooling
- Automatic retry logic
- Timeout handling
- Response parsing
- Error handling

Usage:
    client = RESTClient(base_url="http://localhost:8000")
    response = await client.get("/users/1")
"""

import asyncio
from typing import Any, Dict, Optional
from urllib.parse import urljoin

import httpx
from tenacity import (
    retry,
    stop_after_attempt,
    wait_exponential,
    retry_if_exception_type,
)


class RESTClient:
    """
    A robust REST client with built-in retry logic, timeouts, and error handling.
    
    Attributes:
        base_url: Base URL for all requests
        timeout: Default timeout in seconds
        max_retries: Maximum number of retry attempts
    """
    
    def __init__(
        self,
        base_url: str,
        timeout: float = 30.0,
        max_retries: int = 3,
        headers: Optional[Dict[str, str]] = None,
    ):
        """
        Initialize the REST client.
        
        Args:
            base_url: Base URL for all API requests
            timeout: Default request timeout in seconds
            max_retries: Maximum number of retry attempts on failure
            headers: Default headers to include in all requests
        """
        self.base_url = base_url.rstrip("/")
        self.timeout = timeout
        self.max_retries = max_retries
        self.default_headers = headers or {}
        
        # Create httpx client with connection pooling
        self._client: Optional[httpx.AsyncClient] = None
    
    async def __aenter__(self):
        """Async context manager entry."""
        await self.connect()
        return self
    
    async def __aexit__(self, exc_type, exc_val, exc_tb):
        """Async context manager exit."""
        await self.close()
    
    async def connect(self):
        """Initialize the HTTP client with connection pooling."""
        self._client = httpx.AsyncClient(
            base_url=self.base_url,
            timeout=httpx.Timeout(self.timeout),
            headers=self.default_headers,
            limits=httpx.Limits(
                max_connections=100,
                max_keepalive_connections=20,
            ),
        )
    
    async def close(self):
        """Close the HTTP client and release resources."""
        if self._client:
            await self._client.aclose()
            self._client = None
    
    def _build_url(self, endpoint: str) -> str:
        """Build complete URL from endpoint."""
        return urljoin(self.base_url, endpoint)
    
    async def request(
        self,
        method: str,
        endpoint: str,
        params: Optional[Dict[str, Any]] = None,
        json: Optional[Dict[str, Any]] = None,
        data: Optional[Any] = None,
        headers: Optional[Dict[str, str]] = None,
    ) -> httpx.Response:
        """
        Make an HTTP request with automatic retry logic.
        
        Args:
            method: HTTP method (GET, POST, PUT, DELETE, etc.)
            endpoint: API endpoint path
            params: Query parameters
            json: JSON body
            data: Form data
            headers: Additional headers
            
        Returns:
            HTTP response object
            
        Raises:
            httpx.HTTPError: On request failure after retries
        """
        if not self._client:
            await self.connect()
        
        url = self._build_url(endpoint)
        
        @retry(
            stop=stop_after_attempt(self.max_retries),
            wait=wait_exponential(multiplier=1, min=1, max=10),
            retry=retry_if_exception_type((httpx.ConnectError, httpx.TimeoutException)),
            reraise=True,
        )
        async def _make_request():
            response = await self._client.request(
                method=method,
                url=url,
                params=params,
                json=json,
                data=data,
                headers=headers,
            )
            response.raise_for_status()
            return response
        
        return await _make_request()
    
    async def get(
        self,
        endpoint: str,
        params: Optional[Dict[str, Any]] = None,
        headers: Optional[Dict[str, str]] = None,
    ) -> Dict[str, Any]:
        """
        Make a GET request.
        
        Args:
            endpoint: API endpoint
            params: Query parameters
            headers: Request headers
            
        Returns:
            Parsed JSON response
        """
        response = await self.request("GET", endpoint, params=params, headers=headers)
        return response.json()
    
    async def post(
        self,
        endpoint: str,
        json: Optional[Dict[str, Any]] = None,
        data: Optional[Any] = None,
        headers: Optional[Dict[str, str]] = None,
    ) -> Dict[str, Any]:
        """
        Make a POST request.
        
        Args:
            endpoint: API endpoint
            json: JSON body
            data: Form data
            headers: Request headers
            
        Returns:
            Parsed JSON response
        """
        response = await self.request("POST", endpoint, json=json, data=data, headers=headers)
        return response.json()
    
    async def put(
        self,
        endpoint: str,
        json: Optional[Dict[str, Any]] = None,
        headers: Optional[Dict[str, str]] = None,
    ) -> Dict[str, Any]:
        """
        Make a PUT request.
        
        Args:
            endpoint: API endpoint
            json: JSON body
            headers: Request headers
            
        Returns:
            Parsed JSON response
        """
        response = await self.request("PUT", endpoint, json=json, headers=headers)
        return response.json()
    
    async def patch(
        self,
        endpoint: str,
        json: Optional[Dict[str, Any]] = None,
        headers: Optional[Dict[str, str]] = None,
    ) -> Dict[str, Any]:
        """
        Make a PATCH request.
        
        Args:
            endpoint: API endpoint
            json: JSON body
            headers: Request headers
            
        Returns:
            Parsed JSON response
        """
        response = await self.request("PATCH", endpoint, json=json, headers=headers)
        return response.json()
    
    async def delete(
        self,
        endpoint: str,
        headers: Optional[Dict[str, str]] = None,
    ) -> Dict[str, Any]:
        """
        Make a DELETE request.
        
        Args:
            endpoint: API endpoint
            headers: Request headers
            
        Returns:
            Parsed JSON response
        """
        response = await self.request("DELETE", endpoint, headers=headers)
        return response.json()


# Example usage
async def main():
    """Demonstrate REST client usage."""
    async with RESTClient(base_url="https://api.example.com") as client:
        # GET request
        # users = await client.get("/users", params={"page": 1})
        
        # POST request
        # new_user = await client.post("/users", json={"name": "John", "email": "john@example.com"})
        
        print("REST Client initialized and ready to use")
        
        # Simulate some work
        await asyncio.sleep(0.1)


if __name__ == "__main__":
    asyncio.run(main())
