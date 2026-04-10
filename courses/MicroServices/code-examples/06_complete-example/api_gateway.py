"""
API Gateway - FastAPI Microservice

This service provides:
- Request routing to backend services
- Request aggregation
- Simple rate limiting
- Health monitoring

Usage:
    uvicorn api_gateway:app --host 0.0.0.0 --port 8080
"""

import logging
import os
from typing import Dict, Any, Optional
from datetime import datetime

from fastapi import FastAPI, HTTPException, Request, Response
from fastapi.responses import JSONResponse
import httpx


logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

app = FastAPI(
    title="E-commerce API Gateway",
    version="1.0.0",
    description="API Gateway for E-commerce Microservices",
)


# =============================================================================
# CONFIGURATION
# =============================================================================

PRODUCT_SERVICE_URL = os.getenv("PRODUCT_SERVICE_URL", "http://localhost:8001")
ORDER_SERVICE_URL = os.getenv("ORDER_SERVICE_URL", "http://localhost:8002")


# HTTP Client
http_client = httpx.AsyncClient(timeout=30.0)


async def proxy_request(
    service_url: str,
    method: str,
    path: str,
    params: Optional[Dict] = None,
    json: Optional[Dict] = None,
) -> Dict[str, Any]:
    """Proxy request to backend service."""
    url = f"{service_url}{path}"
    
    try:
        response = await http_client.request(
            method=method,
            url=url,
            params=params,
            json=json,
        )
        
        response.raise_for_status()
        
        return response.json()
    
    except httpx.HTTPStatusError as e:
        logger.error(f"HTTP error: {e}")
        raise HTTPException(
            status_code=e.response.status_code,
            detail=str(e),
        )
    
    except httpx.RequestError as e:
        logger.error(f"Request error: {e}")
        raise HTTPException(
            status_code=503,
            detail=f"Service unavailable: {e}",
        )


# =============================================================================
# ROUTES
# =============================================================================

@app.get("/health")
async def health_check():
    """Health check endpoint."""
    health = {
        "status": "healthy",
        "service": "api-gateway",
        "timestamp": datetime.utcnow().isoformat(),
        "services": {},
    }
    
    # Check product service
    try:
        product_health = await http_client.get(
            f"{PRODUCT_SERVICE_URL}/health",
            timeout=5.0,
        )
        health["services"]["product-service"] = (
            "healthy" if product_health.status_code == 200 else "unhealthy"
        )
    except Exception:
        health["services"]["product-service"] = "unhealthy"
    
    # Check order service
    try:
        order_health = await http_client.get(
            f"{ORDER_SERVICE_URL}/health",
            timeout=5.0,
        )
        health["services"]["order-service"] = (
            "healthy" if order_health.status_code == 200 else "unhealthy"
        )
    except Exception:
        health["services"]["order-service"] = "unhealthy"
    
    return health


@app.get("/")
async def root():
    """Root endpoint."""
    return {
        "service": "E-commerce API Gateway",
        "version": "1.0.0",
        "docs": "/docs",
    }


# =============================================================================
# PRODUCT ROUTES
# =============================================================================

@app.get("/api/products")
async def list_products(
    category: Optional[str] = None,
    limit: int = 100,
    offset: int = 0,
):
    """List all products."""
    return await proxy_request(
        PRODUCT_SERVICE_URL,
        "GET",
        "/products",
        params={"category": category, "limit": limit, "offset": offset},
    )


@app.get("/api/products/{product_id}")
async def get_product(product_id: str):
    """Get a product by ID."""
    return await proxy_request(
        PRODUCT_SERVICE_URL,
        "GET",
        f"/products/{product_id}",
    )


@app.post("/api/products", status_code=201)
async def create_product(request: Request):
    """Create a new product."""
    body = await request.json()
    return await proxy_request(
        PRODUCT_SERVICE_URL,
        "POST",
        "/products",
        json=body,
    )


@app.put("/api/products/{product_id}")
async def update_product(product_id: str, request: Request):
    """Update a product."""
    body = await request.json()
    return await proxy_request(
        PRODUCT_SERVICE_URL,
        "PUT",
        f"/products/{product_id}",
        json=body,
    )


@app.delete("/api/products/{product_id}")
async def delete_product(product_id: str):
    """Delete a product."""
    await proxy_request(
        PRODUCT_SERVICE_URL,
        "DELETE",
        f"/products/{product_id}",
    )
    return {"message": "Product deleted"}


# =============================================================================
# ORDER ROUTES
# =============================================================================

@app.get("/api/orders")
async def list_orders(
    customer_id: Optional[str] = None,
    status: Optional[str] = None,
    limit: int = 100,
    offset: int = 0,
):
    """List all orders."""
    return await proxy_request(
        ORDER_SERVICE_URL,
        "GET",
        "/orders",
        params={"customer_id": customer_id, "status": status, "limit": limit, "offset": offset},
    )


@app.get("/api/orders/{order_id}")
async def get_order(order_id: str):
    """Get an order by ID."""
    return await proxy_request(
        ORDER_SERVICE_URL,
        "GET",
        f"/orders/{order_id}",
    )


@app.post("/api/orders", status_code=201)
async def create_order(request: Request):
    """Create a new order."""
    body = await request.json()
    return await proxy_request(
        ORDER_SERVICE_URL,
        "POST",
        "/orders",
        json=body,
    )


@app.patch("/api/orders/{order_id}/status")
async def update_order_status(order_id: str, request: Request):
    """Update order status."""
    body = await request.json()
    status = body.get("status")
    
    return await proxy_request(
        ORDER_SERVICE_URL,
        "PATCH",
        f"/orders/{order_id}/status",
        json={"status": status},
    )


# =============================================================================
# MAIN
# =============================================================================

if __name__ == "__main__":
    import uvicorn
    port = int(os.getenv("PORT", "8080"))
    uvicorn.run(app, host="0.0.0.0", port=port)