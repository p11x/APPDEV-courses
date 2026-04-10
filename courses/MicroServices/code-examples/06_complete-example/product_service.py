"""
Product Service - FastAPI Microservice

This service provides product management functionality:
- CRUD operations for products
- Redis caching
- Health checks

Usage:
    uvicorn product_service:app --host 0.0.0.0 --port 8001
"""

import logging
import os
from typing import List, Optional
from dataclasses import dataclass
from datetime import datetime

from fastapi import FastAPI, HTTPException, Depends
from pydantic import BaseModel
import redis.asyncio as redis


logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

app = FastAPI(title="Product Service", version="1.0.0")


# =============================================================================
# MODELS
# =============================================================================

class ProductCreate(BaseModel):
    """Request model for creating a product."""
    name: str
    description: str
    price: float
    stock: int
    category: str


class ProductUpdate(BaseModel):
    """Request model for updating a product."""
    name: Optional[str] = None
    description: Optional[str] = None
    price: Optional[float] = None
    stock: Optional[int] = None
    category: Optional[str] = None


class ProductResponse(BaseModel):
    """Response model for product."""
    id: str
    name: str
    description: str
    price: float
    stock: int
    category: str
    created_at: datetime
    updated_at: datetime


# =============================================================================
# DATABASE (In-memory for demo)
# =============================================================================

class ProductDatabase:
    """In-memory product database."""
    
    def __init__(self):
        self._products = {}
        self._id_counter = 0
        
        # Seed some sample data
        self._seed_data()
    
    def _seed_data(self):
        """Add sample products."""
        products = [
            ("Laptop", "High-performance laptop", 999.99, 10, "Electronics"),
            ("Phone", "Smartphone with great camera", 699.99, 25, "Electronics"),
            ("Headphones", "Wireless noise-cancelling", 299.99, 50, "Audio"),
            ("Book", "Python programming guide", 49.99, 100, "Books"),
            ("Chair", "Ergonomic office chair", 199.99, 15, "Furniture"),
        ]
        
        for name, desc, price, stock, category in products:
            self.create_product(name, desc, price, stock, category)
    
    def _generate_id(self) -> str:
        self._id_counter += 1
        return f"prod-{self._id_counter:04d}"
    
    def create_product(
        self,
        name: str,
        description: str,
        price: float,
        stock: int,
        category: str,
    ) -> ProductResponse:
        """Create a new product."""
        product_id = self._generate_id()
        now = datetime.utcnow()
        
        product = ProductResponse(
            id=product_id,
            name=name,
            description=description,
            price=price,
            stock=stock,
            category=category,
            created_at=now,
            updated_at=now,
        )
        
        self._products[product_id] = product
        return product
    
    def get_product(self, product_id: str) -> Optional[ProductResponse]:
        """Get a product by ID."""
        return self._products.get(product_id)
    
    def list_products(
        self,
        category: Optional[str] = None,
        limit: int = 100,
        offset: int = 0,
    ) -> List[ProductResponse]:
        """List products with optional filtering."""
        products = list(self._products.values())
        
        if category:
            products = [p for p in products if p.category == category]
        
        return products[offset:offset + limit]
    
    def update_product(
        self,
        product_id: str,
        update_data: ProductUpdate,
    ) -> Optional[ProductResponse]:
        """Update a product."""
        product = self._products.get(product_id)
        if not product:
            return None
        
        update_dict = update_data.dict(exclude_unset=True)
        
        for key, value in update_dict.items():
            setattr(product, key, value)
        
        product.updated_at = datetime.utcnow()
        
        return product
    
    def delete_product(self, product_id: str) -> bool:
        """Delete a product."""
        if product_id in self._products:
            del self._products[product_id]
            return True
        return False


# Initialize database
product_db = ProductDatabase()


# =============================================================================
# REDIS CACHE
# =============================================================================

redis_client: Optional[redis.Redis] = None


async def get_redis() -> redis.Redis:
    """Get Redis client."""
    global redis_client
    if redis_client is None:
        redis_url = os.getenv("REDIS_URL", "redis://localhost:6379")
        redis_client = redis.from_url(redis_url)
    return redis_client


async def cache_get(key: str) -> Optional[str]:
    """Get value from cache."""
    try:
        client = await get_redis()
        return await client.get(key)
    except Exception as e:
        logger.warning(f"Cache get error: {e}")
        return None


async def cache_set(key: str, value: str, ttl: int = 300):
    """Set value in cache."""
    try:
        client = await get_redis()
        await client.setex(key, ttl, value)
    except Exception as e:
        logger.warning(f"Cache set error: {e}")


async def cache_delete(key: str):
    """Delete value from cache."""
    try:
        client = await get_redis()
        await client.delete(key)
    except Exception as e:
        logger.warning(f"Cache delete error: {e}")


# =============================================================================
# ROUTES
# =============================================================================

@app.get("/health")
async def health_check():
    """Health check endpoint."""
    return {"status": "healthy", "service": "product-service"}


@app.get("/")
async def root():
    """Root endpoint."""
    return {"service": "Product Service", "version": "1.0.0"}


@app.post("/products", response_model=ProductResponse, status_code=201)
async def create_product(product: ProductCreate):
    """Create a new product."""
    return product_db.create_product(
        name=product.name,
        description=product.description,
        price=product.price,
        stock=product.stock,
        category=product.category,
    )


@app.get("/products", response_model=List[ProductResponse])
async def list_products(
    category: Optional[str] = None,
    limit: int = 100,
    offset: int = 0,
):
    """List all products."""
    return product_db.list_products(category=category, limit=limit, offset=offset)


@app.get("/products/{product_id}", response_model=ProductResponse)
async def get_product(product_id: str):
    """Get a product by ID."""
    # Try cache first
    cache_key = f"product:{product_id}"
    cached = await cache_get(cache_key)
    
    if cached:
        logger.info(f"Cache hit for {product_id}")
        return ProductResponse.parse_raw(cached)
    
    # Get from database
    product = product_db.get_product(product_id)
    
    if not product:
        raise HTTPException(status_code=404, detail="Product not found")
    
    # Cache the result
    await cache_set(cache_key, product.json())
    
    return product


@app.put("/products/{product_id}", response_model=ProductResponse)
async def update_product(product_id: str, product: ProductUpdate):
    """Update a product."""
    updated = product_db.update_product(product_id, product)
    
    if not updated:
        raise HTTPException(status_code=404, detail="Product not found")
    
    # Invalidate cache
    await cache_delete(f"product:{product_id}")
    
    return updated


@app.delete("/products/{product_id}", status_code=204)
async def delete_product(product_id: str):
    """Delete a product."""
    deleted = product_db.delete_product(product_id)
    
    if not deleted:
        raise HTTPException(status_code=404, detail="Product not found")
    
    # Invalidate cache
    await cache_delete(f"product:{product_id}")


# =============================================================================
# MAIN
# =============================================================================

if __name__ == "__main__":
    import uvicorn
    port = int(os.getenv("PORT", "8001"))
    uvicorn.run(app, host="0.0.0.0", port=port)