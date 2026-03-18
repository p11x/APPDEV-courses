# E-Commerce Backend

## What You'll Learn

- Building a complete e-commerce backend
- Product management
- Shopping cart functionality
- Order processing
- Payment integration

## Prerequisites

- Completed the FastAPI and databases sections
- Understanding of authentication

## Introduction

This project covers building a complete e-commerce backend with FastAPI, covering products, cart, orders, and payments.

## Project Structure

```
e-commerce-backend/
├── app/
│   ├── __init__.py
│   ├── main.py
│   ├── config.py
│   ├── models/
│   │   ├── __init__.py
│   │   ├── product.py
│   │   ├── cart.py
│   │   ├── order.py
│   │   └── user.py
│   ├── routers/
│   │   ├── __init__.py
│   │   ├── products.py
│   │   ├── cart.py
│   │   ├── orders.py
│   │   └── auth.py
│   └── services/
│       ├── __init__.py
│       └── payment.py
└── requirements.txt
```

## Core Models

```python
from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime
from enum import Enum
import uuid


# Enums
class ProductCategory(str, Enum):
    ELECTRONICS = "electronics"
    CLOTHING = "clothing"
    BOOKS = "books"
    HOME = "home"
    SPORTS = "sports"


class OrderStatus(str, Enum):
    PENDING = "pending"
    PAID = "paid"
    PROCESSING = "processing"
    SHIPPED = "shipped"
    DELIVERED = "delivered"
    CANCELLED = "cancelled"


# Product Models
class Product(BaseModel):
    id: str = Field(default_factory=lambda: str(uuid.uuid4()))
    name: str
    description: str
    price: float = Field(..., gt=0)
    category: ProductCategory
    stock: int = Field(default=0, ge=0)
    image_url: Optional[str] = None
    created_at: datetime = Field(default_factory=datetime.now)
    updated_at: datetime = Field(default_factory=datetime.now)


class ProductCreate(BaseModel):
    name: str
    description: str
    price: float = Field(..., gt=0)
    category: ProductCategory
    stock: int = Field(default=0, ge=0)
    image_url: Optional[str] = None


# Cart Models
class CartItem(BaseModel):
    product_id: str
    quantity: int = Field(..., gt=0)


class Cart(BaseModel):
    user_id: str
    items: List[CartItem] = Field(default_factory=list)
    updated_at: datetime = Field(default_factory=datetime.now)
    
    def total(self, products: dict) -> float:
        """Calculate cart total."""
        total = 0.0
        for item in self.items:
            if item.product_id in products:
                total += products[item.product_id].price * item.quantity
        return total


# Order Models
class OrderItem(BaseModel):
    product_id: str
    quantity: int
    price: float  # Price at time of purchase


class Order(BaseModel):
    id: str = Field(default_factory=lambda: str(uuid.uuid4()))
    user_id: str
    items: List[OrderItem]
    total: float
    status: OrderStatus = OrderStatus.PENDING
    shipping_address: dict
    payment_id: Optional[str] = None
    created_at: datetime = Field(default_factory=datetime.now)
    updated_at: datetime = Field(default_factory=datetime.now)


# User Models
class User(BaseModel):
    id: str = Field(default_factory=lambda: str(uuid.uuid4()))
    email: str
    name: str
    hashed_password: str
    addresses: List[dict] = Field(default_factory=list)
    created_at: datetime = Field(default_factory=datetime.now)
```

## Main Application

```python
# app/main.py
from fastapi import FastAPI
from app.routers import products, cart, orders, auth


app = FastAPI(
    title="E-Commerce API",
    description="Complete e-commerce backend API",
    version="1.0.0",
)

# Include routers
app.include_router(auth.router, prefix="/api/auth", tags=["auth"])
app.include_router(products.router, prefix="/api/products", tags=["products"])
app.include_router(cart.router, prefix="/api/cart", tags=["cart"])
app.include_router(orders.router, prefix="/api/orders", tags=["orders"])


@app.get("/")
async def root():
    return {"message": "E-Commerce API", "version": "1.0.0"}


@app.get("/health")
async def health_check():
    return {"status": "healthy"}
```

## Products Router

```python
# app/routers/products.py
from fastapi import FastAPI, HTTPException, Depends, Query
from typing import Optional, List
from app.models.product import Product, ProductCreate, ProductCategory


# In-memory storage (use database in production)
products_db: dict[str, Product] = {}


# Add some sample products
sample_products = [
    Product(
        name="Laptop",
        description="High-performance laptop",
        price=999.99,
        category=ProductCategory.ELECTRONICS,
        stock=10,
    ),
    Product(
        name="T-Shirt",
        description="Comfortable cotton t-shirt",
        price=29.99,
        category=ProductCategory.CLOTHING,
        stock=100,
    ),
    Product(
        name="Python Cookbook",
        description="Programming cookbook",
        price=49.99,
        category=ProductCategory.BOOKS,
        stock=50,
    ),
]

for p in sample_products:
    products_db[p.id] = p


router = FastAPI()


@router.get("/", response_model=List[Product])
async def list_products(
    category: Optional[ProductCategory] = None,
    min_price: Optional[float] = None,
    max_price: Optional[float] = None,
    search: Optional[str] = None,
    limit: int = Query(default=20, le=100),
    offset: int = Query(default=0, ge=0),
) -> List[Product]:
    """List all products with optional filters."""
    
    products = list(products_db.values())
    
    # Apply filters
    if category:
        products = [p for p in products if p.category == category]
    
    if min_price is not None:
        products = [p for p in products if p.price >= min_price]
    
    if max_price is not None:
        products = [p for p in products if p.price <= max_price]
    
    if search:
        search_lower = search.lower()
        products = [
            p for p in products
            if search_lower in p.name.lower() or search_lower in p.description.lower()
        ]
    
    # Apply pagination
    products = products[offset:offset + limit]
    
    return products


@router.get("/{product_id}", response_model=Product)
async def get_product(product_id: str) -> Product:
    """Get a single product by ID."""
    
    if product_id not in products_db:
        raise HTTPException(status_code=404, detail="Product not found")
    
    return products_db[product_id]


@router.post("/", response_model=Product)
async def create_product(product: ProductCreate) -> Product:
    """Create a new product."""
    
    new_product = Product(**product.model_dump())
    products_db[new_product.id] = new_product
    
    return new_product


@router.put("/{product_id}", response_model=Product)
async def update_product(
    product_id: str,
    product: ProductCreate,
) -> Product:
    """Update a product."""
    
    if product_id not in products_db:
        raise HTTPException(status_code=404, detail="Product not found")
    
    updated_product = Product(
        id=product_id,
        **product.model_dump(),
    )
    
    products_db[product_id] = updated_product
    
    return updated_product


@router.delete("/{product_id}")
async def delete_product(product_id: str) -> dict:
    """Delete a product."""
    
    if product_id not in products_db:
        raise HTTPException(status_code=404, detail="Product not found")
    
    del products_db[product_id]
    
    return {"success": True}
```

## Cart Router

```python
# app/routers/cart.py
from fastapi import FastAPI, HTTPException, Depends
from typing import List
from app.models.product import Product
from app.models.cart import Cart, CartItem


# In-memory cart storage
carts_db: dict[str, Cart] = {}


router = FastAPI()


def get_cart(user_id: str) -> Cart:
    """Get or create cart for user."""
    if user_id not in carts_db:
        carts_db[user_id] = Cart(user_id=user_id)
    return carts_db[user_id]


@router.get("/", response_model=dict)
async def get_user_cart(user_id: str) -> dict:
    """Get user's cart with product details."""
    
    cart = get_cart(user_id)
    
    # Get product details
    from app.routers.products import products_db
    
    cart_items = []
    for item in cart.items:
        if item.product_id in products_db:
            product = products_db[item.product_id]
            cart_items.append({
                "product": product,
                "quantity": item.quantity,
                "subtotal": product.price * item.quantity,
            })
    
    return {
        "items": cart_items,
        "total": cart.total(products_db),
        "item_count": len(cart.items),
    }


@router.post("/add")
async def add_to_cart(
    user_id: str,
    product_id: str,
    quantity: int = 1,
) -> dict:
    """Add item to cart."""
    
    from app.routers.products import products_db
    
    # Verify product exists
    if product_id not in products_db:
        raise HTTPException(status_code=404, detail="Product not found")
    
    product = products_db[product_id]
    
    # Check stock
    cart = get_cart(user_id)
    current_quantity = sum(
        item.quantity for item in cart.items
        if item.product_id == product_id
    )
    
    if current_quantity + quantity > product.stock:
        raise HTTPException(
            status_code=400,
            detail="Not enough stock available"
        )
    
    # Add to cart
    existing_item = None
    for item in cart.items:
        if item.product_id == product_id:
            existing_item = item
            break
    
    if existing_item:
        existing_item.quantity += quantity
    else:
        cart.items.append(CartItem(product_id=product_id, quantity=quantity))
    
    cart.updated_at = datetime.now()
    
    return {"success": True, "cart_total": cart.total(products_db)}


@router.post("/remove")
async def remove_from_cart(
    user_id: str,
    product_id: str,
) -> dict:
    """Remove item from cart."""
    
    cart = get_cart(user_id)
    
    cart.items = [item for item in cart.items if item.product_id != product_id]
    cart.updated_at = datetime.now()
    
    from app.routers.products import products_db
    
    return {"success": True, "cart_total": cart.total(products_db)}


@router.post("/clear")
async def clear_cart(user_id: str) -> dict:
    """Clear all items from cart."""
    
    carts_db[user_id] = Cart(user_id=user_id)
    
    return {"success": True}


from datetime import datetime
```

## Orders Router

```python
# app/routers/orders.py
from fastapi import FastAPI, HTTPException, Depends
from typing import List
from app.models.order import Order, OrderItem, OrderStatus
from datetime import datetime


# In-memory order storage
orders_db: dict[str, Order] = {}


router = FastAPI()


@router.post("/", response_model=Order)
async def create_order(
    user_id: str,
    shipping_address: dict,
    payment_id: str,
) -> Order:
    """Create an order from cart."""
    
    from app.routers.cart import carts_db
    from app.routers.products import products_db
    
    cart = carts_db.get(user_id)
    
    if not cart or not cart.items:
        raise HTTPException(status_code=400, detail="Cart is empty")
    
    # Verify stock and create order items
    order_items = []
    total = 0.0
    
    for item in cart.items:
        if item.product_id not in products_db:
            raise HTTPException(
                status_code=400,
                detail=f"Product {item.product_id} not found"
            )
        
        product = products_db[item.product_id]
        
        if product.stock < item.quantity:
            raise HTTPException(
                status_code=400,
                detail=f"Not enough stock for {product.name}"
            )
        
        order_items.append(OrderItem(
            product_id=item.product_id,
            quantity=item.quantity,
            price=product.price,
        ))
        
        total += product.price * item.quantity
        
        # Decrease stock
        product.stock -= item.quantity
    
    # Create order
    order = Order(
        user_id=user_id,
        items=order_items,
        total=total,
        status=OrderStatus.PAID,
        shipping_address=shipping_address,
        payment_id=payment_id,
    )
    
    orders_db[order.id] = order
    
    # Clear cart
    carts_db[user_id].items = []
    
    return order


@router.get("/", response_model=List[Order])
async def list_orders(user_id: str) -> List[Order]:
    """Get user's orders."""
    
    return [
        order for order in orders_db.values()
        if order.user_id == user_id
    ]


@router.get("/{order_id}", response_model=Order)
async def get_order(order_id: str) -> Order:
    """Get order by ID."""
    
    if order_id not in orders_db:
        raise HTTPException(status_code=404, detail="Order not found")
    
    return orders_db[order_id]


@router.post("/{order_id}/cancel")
async def cancel_order(order_id: str, user_id: str) -> dict:
    """Cancel an order."""
    
    if order_id not in orders_db:
        raise HTTPException(status_code=404, detail="Order not found")
    
    order = orders_db[order_id]
    
    if order.user_id != user_id:
        raise HTTPException(status_code=403, detail="Not authorized")
    
    if order.status not in [OrderStatus.PENDING, OrderStatus.PAID]:
        raise HTTPException(
            status_code=400,
            detail="Cannot cancel order in current status"
        )
    
    # Restore stock
    from app.routers.products import products_db
    
    for item in order.items:
        if item.product_id in products_db:
            products_db[item.product_id].stock += item.quantity
    
    order.status = OrderStatus.CANCELLED
    order.updated_at = datetime.now()
    
    return {"success": True}
```

## Summary

This e-commerce backend includes:

- Product management with categories, search, and filtering
- Shopping cart functionality
- Order processing with stock management
- Basic order lifecycle (create, cancel)

In production, you would add:
- User authentication with JWT
- Payment processing with Stripe
- Database persistence (PostgreSQL)
- Email notifications
- Admin dashboard

## Next Steps

Continue to `02-social-media-backend.md` to build a social media backend.
