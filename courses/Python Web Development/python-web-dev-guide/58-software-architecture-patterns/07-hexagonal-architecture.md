# Hexagonal Architecture

## What You'll Learn

- Ports and adapters pattern
- Dependency inversion
- Building applications that are easy to test and change

## Prerequisites

- Understanding of clean architecture

## Introduction

Hexagonal architecture (also called Ports and Adapters) is a pattern that organizes your application around the business logic, with external concerns connected through "ports" and "adapters."

Think of your application as a power socket (the port) that can accept any device (adapter) that follows the interface. Your business logic doesn't care whether it's receiving data from a REST API, GraphQL, CLI, or test harness—as long as they conform to the port interface.

## The Hexagon

```
                    ┌──────────────────┐
                    │   Adapters       │
                    │  (Primary/Driven) │
                    │                  │
    ┌──────────────▶│  - REST API      │◀───────────┐
    │               │  - GraphQL       │            │
    │               │  - CLI           │            │
    │               └────────┬─────────┘            │
    │                        │                      │
    │                        ▼                      │
    │               ┌─────────────────┐            │
    │               │     Ports       │            │
    │               │  (Input/Output) │            │
    │               └────────┬────────┘            │
    │                        │                      │
    │                        ▼                      │
    │               ┌─────────────────┐            │
    │               │   Application   │            │
    │               │   Core/Domain   │            │
    │               └────────┬────────┘            │
    │                        │                      │
    │                        ▼                      │
    │               ┌─────────────────┐            │
    │               │     Ports       │            │
    │               │  (Output/Driven)│            │
    │               └────────┬────────┘            │
    │                        │                      │
    │                        ▼                      │
    │               ┌──────────────────┐          │
    └───────────────│    Adapters      │◀──────────┘
                    │ (Secondary/Driven)│
                    │                   │
                    │  - Database       │
                    │  - Message Queue  │
                    │  - External API  │
                    └───────────────────┘
```

## Core Domain

```python
from dataclasses import dataclass
from datetime import datetime
from typing import Optional
from abc import ABC, abstractmethod

# ============== Domain Entities ==============
@dataclass
class Product:
    id: int
    name: str
    description: str
    price: float
    stock: int
    
    def can_sell(self, quantity: int) -> bool:
        return self.stock >= quantity
    
    def reduce_stock(self, quantity: int) -> None:
        if not self.can_sell(quantity):
            raise ValueError(f"Insufficient stock for product {self.name}")
        self.stock -= quantity

@dataclass
class CartItem:
    product: Product
    quantity: int
    
    @property
    def subtotal(self) -> float:
        return self.product.price * self.quantity

@dataclass
class Cart:
    items: list[CartItem] = None
    
    def __post_init__(self):
        if self.items is None:
            self.items = []
    
    def add_item(self, product: Product, quantity: int) -> None:
        # Check if product already in cart
        for item in self.items:
            if item.product.id == product.id:
                item.quantity += quantity
                return
        
        self.items.append(CartItem(product=product, quantity=quantity))
    
    @property
    def total(self) -> float:
        return sum(item.subtotal for item in self.items)
```

## Ports (Interfaces)

```python
from abc import ABC, abstractmethod
from typing import Optional

# ============== Input Ports (Use Cases) ==============
class CartService(ABC):
    """Primary port for cart operations."""
    
    @abstractmethod
    async def get_cart(self, cart_id: str) -> Optional["Cart"]:
        pass
    
    @abstractmethod
    async def add_to_cart(self, cart_id: str, product_id: int, quantity: int) -> Cart:
        pass
    
    @abstractmethod
    async def checkout(self, cart_id: str) -> dict:
        pass

class ProductService(ABC):
    """Primary port for product operations."""
    
    @abstractmethod
    async def get_product(self, product_id: int) -> Optional[Product]:
        pass
    
    @abstractmethod
    async def list_products(self) -> list[Product]:
        pass

# ============== Output Ports (Repositories) ==============
class ProductRepository(ABC):
    """Secondary port for product persistence."""
    
    @abstractmethod
    async def find_by_id(self, product_id: int) -> Optional[Product]:
        pass
    
    @abstractmethod
    async def find_all(self) -> list[Product]:
        pass
    
    @abstractmethod
    async def save(self, product: Product) -> Product:
        pass

class CartRepository(ABC):
    """Secondary port for cart persistence."""
    
    @abstractmethod
    async def find_by_id(self, cart_id: str) -> Optional[Cart]:
        pass
    
    @abstractmethod
    async def save(self, cart: Cart) -> Cart:
        pass

class OrderRepository(ABC):
    """Secondary port for order persistence."""
    
    @abstractmethod
    async def save(self, order: dict) -> dict:
        pass
```

🔍 **Line-by-Line Breakdown:**

1. `from abc import ABC, abstractmethod` — Import for creating abstract interfaces.
2. `class CartService(ABC):` — Input port (primary port) defining the cart use cases.
3. `@abstractmethod` — Decorator marking methods that adapters must implement.
4. `class ProductRepository(ABC):` — Output port (secondary port) for data access.
5. The ports define interfaces that the core domain uses to interact with the outside world.

## Adapters (Implementations)

```python
import asyncio
from typing import Optional

# ============== Secondary Adapters (Database, etc.) ==============
class InMemoryProductRepository(ProductRepository):
    def __init__(self):
        self._products: dict[int, Product] = {}
    
    async def find_by_id(self, product_id: int) -> Optional[Product]:
        return self._products.get(product_id)
    
    async def find_all(self) -> list[Product]:
        return list(self._products.values())
    
    async def save(self, product: Product) -> Product:
        self._products[product.id] = product
        return product
    
    def add_product(self, product: Product) -> Product:
        self._products[product.id] = product
        return product

class InMemoryCartRepository(CartRepository):
    def __init__(self):
        self._carts: dict[str, Cart] = {}
    
    async def find_by_id(self, cart_id: str) -> Optional[Cart]:
        return self._carts.get(cart_id)
    
    async def save(self, cart: Cart) -> Cart:
        self._carts[cart.id] = cart  # type: ignore
        return cart

class InMemoryOrderRepository(OrderRepository):
    def __init__(self):
        self._orders: dict[int, dict] = {}
        self._next_id = 1
    
    async def save(self, order: dict) -> dict:
        order["id"] = self._next_id
        self._next_id += 1
        self._orders[order["id"]] = order
        return order

# ============== Primary Adapter (REST API) ==============
from fastapi import FastAPI, HTTPException, Depends
from pydantic import BaseModel

app = FastAPI()

class AddToCartRequest(BaseModel):
    product_id: int
    quantity: int

class CartItemResponse(BaseModel):
    product_id: int
    product_name: str
    quantity: int
    subtotal: float

class CartResponse(BaseModel):
    cart_id: str
    items: list[CartItemResponse]
    total: float

# Simple in-memory adapters
product_repo = InMemoryProductRepository()
cart_repo = InMemoryCartRepository()
order_repo = InMemoryOrderRepository()

# Add sample products
sample_products = [
    Product(1, "Laptop", "High-performance laptop", 999.99, 10),
    Product(2, "Mouse", "Wireless mouse", 29.99, 50),
    Product(3, "Keyboard", "Mechanical keyboard", 149.99, 25),
]
for product in sample_products:
    product_repo.add_product(product)

@app.get("/products")
async def list_products() -> list[dict]:
    products = await product_repo.find_all()
    return [
        {"id": p.id, "name": p.name, "price": p.price, "stock": p.stock}
        for p in products
    ]

@app.post("/carts/{cart_id}/items")
async def add_to_cart(
    cart_id: str,
    request: AddToCartRequest
) -> CartResponse:
    # Get or create cart
    cart = await cart_repo.find_by_id(cart_id)
    if not cart:
        cart = Cart(id=cart_id, items=[])  # type: ignore
    
    # Get product
    product = await product_repo.find_by_id(request.product_id)
    if not product:
        raise HTTPException(status_code=404, detail="Product not found")
    
    # Add to cart
    cart.add_item(product, request.quantity)
    await cart_repo.save(cart)
    
    return CartResponse(
        cart_id=cart_id,
        items=[
            CartItemResponse(
                product_id=item.product.id,
                product_name=item.product.name,
                quantity=item.quantity,
                subtotal=item.subtotal
            )
            for item in cart.items
        ],
        total=cart.total
    )

@app.post("/carts/{cart_id}/checkout")
async def checkout(cart_id: str) -> dict:
    cart = await cart_repo.find_by_id(cart_id)
    if not cart or not cart.items:
        raise HTTPException(status_code=400, detail="Cart is empty")
    
    # Create order
    order = {
        "cart_id": cart_id,
        "items": [
            {"product_id": item.product.id, "quantity": item.quantity}
            for item in cart.items
        ],
        "total": cart.total,
        "created_at": datetime.now().isoformat()
    }
    
    saved_order = await order_repo.save(order)
    
    # Clear cart
    cart.items = []
    await cart_repo.save(cart)
    
    return {"order_id": saved_order["id"], "total": saved_order["total"]}
```

## Dependency Injection in Practice

```python
from typing import TypeVar, Generic

T = TypeVar('T')

class Container:
    """Simple dependency injection container."""
    
    def __init__(self):
        self._services: dict[type, object] = {}
    
    def register(self, interface: type[T], implementation: T) -> None:
        self._services[interface] = implementation
    
    def resolve(self, interface: type[T]) -> T:
        if interface not in self._services:
            raise ValueError(f"No implementation registered for {interface}")
        return self._services[interface]

# Usage
container = Container()
container.register(ProductRepository, InMemoryProductRepository())
container.register(CartRepository, InMemoryCartRepository())
container.register(OrderRepository, InMemoryOrderRepository())

# Get dependencies
product_repo = container.resolve(ProductRepository)
```

## Summary

- Hexagonal architecture isolates the core domain from external concerns
- Ports define interfaces; adapters implement them
- Primary ports are for incoming operations (use cases)
- Secondary ports are for outgoing operations (data access)
- This pattern enables easy testing and swapping of implementations

## Next Steps

Continue to `08-serverless-architecture.md` to learn about building applications without managing servers.
