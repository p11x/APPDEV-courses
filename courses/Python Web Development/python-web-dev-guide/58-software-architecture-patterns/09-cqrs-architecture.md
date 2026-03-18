# CQRS Architecture

## What You'll Learn

- Command Query Responsibility Segregation
- Separating read and write models
- Event sourcing with CQRS
- Scaling reads and writes independently

## Prerequisites

- Understanding of event-driven architecture
- Knowledge of databases

## Introduction

CQRS stands for Command Query Responsibility Segregation. The core idea is simple: separate the code that modifies data (commands) from the code that reads data (queries).

Think of a busy restaurant. There's a person taking orders (write side) and a person delivering food (read side). They have different responsibilities and optimized for different tasks. The kitchen prepares food based on orders, and the server delivers it. In a simple app, you'd have one person doing both, but in complex systems, separating these concerns brings big benefits.

## Traditional vs CQRS

```
Traditional Approach:
┌─────────────────────────────────────┐
│           Application               │
│    ┌──────────┐    ┌───────────┐    │
│    │  Command │    │   Query   │    │
│    │  Handler │    │  Handler  │    │
│    └────┬─────┘    └─────┬─────┘    │
│         └────────┬────────┘         │
│                  ▼                  │
│         ┌───────────────┐           │
│         │  Single Model │           │
│         │  (Same DB)    │           │
│         └───────────────┘           │
└─────────────────────────────────────┘

CQRS Approach:
┌─────────────────────────────────────────────────┐
│                  Application                    │
│    ┌─────────────┐         ┌─────────────┐     │
│    │   Command   │         │    Query    │     │
│    │   Handler   │         │   Handler   │     │
│    └──────┬──────┘         └──────┬──────┘     │
│           │                        │            │
│           ▼                        ▼            │
│  ┌─────────────┐        ┌────────────────┐     │
│  │ Write Model │        │  Read Model    │     │
│  │ (Primary DB)│        │ (Read Replica)│     │
│  └─────────────┘        └────────────────┘     │
│           │                        │            │
│           └───────────┬────────────┘            │
│                       ▼                         │
│              ┌────────────────┐                 │
│              │ Event Bus/     │                 │
│              │ Materialized  │                 │
│              │ Views         │                 │
│              └────────────────┘                 │
└─────────────────────────────────────────────────┘
```

## Implementing CQRS in Python

```python
from dataclasses import dataclass, field
from datetime import datetime
from typing import Optional, Callable
from abc import ABC, abstractmethod
import asyncio
import json

# ============== Domain Models ==============
@dataclass
class Product:
    id: int
    name: str
    price: float
    stock: int
    version: int = 1

@dataclass
class Order:
    id: int
    customer_id: int
    items: list[dict]
    total: float
    status: str
    created_at: datetime

# ============== Commands ==============
@dataclass
class CreateProductCommand:
    name: str
    price: float
    stock: int

@dataclass
class UpdateStockCommand:
    product_id: int
    quantity_change: int  # positive for add, negative for remove

@dataclass
class CreateOrderCommand:
    customer_id: int
    items: list[dict]  # [{product_id, quantity, price}]

# ============== Queries ==============
@dataclass
class GetProductQuery:
    product_id: int

@dataclass
class ListProductsQuery:
    limit: int = 50
    offset: int = 0

@dataclass
class GetOrderQuery:
    order_id: int

# ============== Command Handlers ==============
class CommandHandler(ABC):
    @abstractmethod
    async def handle(self, command) -> dict:
        pass

class ProductCommandHandler(CommandHandler):
    def __init__(self, event_publisher: Callable):
        self._products: dict[int, Product] = {}
        self._next_id = 1
        self._event_publisher = event_publisher
    
    async def handle(self, command) -> dict:
        match command:
            case CreateProductCommand():
                return await self._create_product(command)
            case UpdateStockCommand():
                return await self._update_stock(command)
    
    async def _create_product(self, command: CreateProductCommand) -> dict:
        product = Product(
            id=self._next_id,
            name=command.name,
            price=command.price,
            stock=command.stock
        )
        self._products[product.id] = product
        self._next_id += 1
        
        # Publish event for read models
        await self._event_publisher({
            "type": "ProductCreated",
            "data": {
                "id": product.id,
                "name": product.name,
                "price": product.price,
                "stock": product.stock
            }
        })
        
        return {"id": product.id, "name": product.name}
    
    async def _update_stock(self, command: UpdateStockCommand) -> dict:
        product = self._products.get(command.product_id)
        if not product:
            raise ValueError(f"Product {command.product_id} not found")
        
        new_stock = product.stock + command.quantity_change
        if new_stock < 0:
            raise ValueError("Insufficient stock")
        
        product.stock = new_stock
        
        await self._event_publisher({
            "type": "StockUpdated",
            "data": {
                "product_id": product.id,
                "old_stock": product.stock - command.quantity_change,
                "new_stock": new_stock
            }
        })
        
        return {"product_id": product.id, "new_stock": new_stock}

class OrderCommandHandler(CommandHandler):
    def __init__(self, event_publisher: Callable):
        self._orders: dict[int, Order] = {}
        self._next_id = 1
        self._event_publisher = event_publisher
    
    async def handle(self, command) -> dict:
        if isinstance(command, CreateOrderCommand):
            return await self._create_order(command)
        raise ValueError(f"Unknown command: {command}")

# ============== Query Handlers ==============
class QueryHandler(ABC):
    @abstractmethod
    async def handle(self, query) -> dict | list[dict]:
        pass

class ProductQueryHandler(QueryHandler):
    def __init__(self, read_model: dict):
        self._read_model = read_model
    
    async def handle(self, query) -> dict | list[dict]:
        match query:
            case GetProductQuery():
                return await self._get_product(query.product_id)
            case ListProductsQuery():
                return await self._list_products(query.limit, query.offset)
    
    async def _get_product(self, product_id: int) -> dict:
        return self._read_model.get("products", {}).get(str(product_id), {})
    
    async def _list_products(self, limit: int, offset: int) -> list[dict]:
        products = self._read_model.get("products", {})
        product_list = list(products.values())
        return product_list[offset:offset + limit]

class OrderQueryHandler(QueryHandler):
    def __init__(self, read_model: dict):
        self._read_model = read_model
    
    async def handle(self, query) -> dict:
        if isinstance(query, GetOrderQuery):
            return self._read_model.get("orders", {}).get(str(query.order_id), {})
        raise ValueError(f"Unknown query: {query}")
```

## Materialized Views

```python
# Read model updated via events
class ReadModelUpdater:
    """Updates materialized views based on events."""
    
    def __init__(self):
        self._read_model: dict = {
            "products": {},
            "orders": {},
            "product_list": []
        }
    
    async def handle_event(self, event: dict) -> None:
        event_type = event.get("type")
        data = event.get("data", {})
        
        match event_type:
            case "ProductCreated":
                await self._handle_product_created(data)
            case "StockUpdated":
                await self._handle_stock_updated(data)
            case "OrderCreated":
                await self._handle_order_created(data)
    
    async def _handle_product_created(self, data: dict) -> None:
        product_id = str(data["id"])
        self._read_model["products"][product_id] = data
        self._read_model["product_list"].append(data)
    
    async def _handle_stock_updated(self, data: dict) -> None:
        product_id = str(data["product_id"])
        if product_id in self._read_model["products"]:
            self._read_model["products"][product_id]["stock"] = data["new_stock"]
    
    async def _handle_order_created(self, data: dict) -> None:
        order_id = str(data["id"])
        self._read_model["orders"][order_id] = data
    
    def get_read_model(self) -> dict:
        return self._read_model

# ============== Event Bus ==============
class SimpleEventBus:
    def __init__(self, read_model_updater: ReadModelUpdater):
        self._updater = read_model_updater
    
    async def publish(self, event: dict) -> None:
        await self._updater.handle_event(event)
```

## FastAPI Application with CQRS

```python
from fastapi import FastAPI, HTTPException, Depends
from pydantic import BaseModel, Field
from typing import List

app = FastAPI()

# Setup components
read_model = ReadModelUpdater()
event_bus = SimpleEventBus(read_model)

product_commands = ProductCommandHandler(event_bus.publish)
product_queries = ProductQueryHandler(read_model.get_read_model())

# Request/Response models
class CreateProductRequest(BaseModel):
    name: str
    price: float = Field(gt=0)
    stock: int = Field(ge=0)

class ProductResponse(BaseModel):
    id: int
    name: str
    price: float
    stock: int

class UpdateStockRequest(BaseModel):
    quantity_change: int

# Command endpoints
@app.post("/products", response_model=dict)
async def create_product(request: CreateProductRequest) -> dict:
    command = CreateProductCommand(
        name=request.name,
        price=request.price,
        stock=request.stock
    )
    return await product_commands.handle(command)

@app.patch("/products/{product_id}/stock")
async def update_stock(product_id: int, request: UpdateStockRequest) -> dict:
    command = UpdateStockCommand(
        product_id=product_id,
        quantity_change=request.quantity_change
    )
    try:
        return await product_commands.handle(command)
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))

# Query endpoints
@app.get("/products/{product_id}", response_model=ProductResponse)
async def get_product(product_id: int) -> dict:
    query = GetProductQuery(product_id=product_id)
    result = await product_queries.handle(query)
    if not result:
        raise HTTPException(status_code=404, detail="Product not found")
    return result

@app.get("/products", response_model=List[ProductResponse])
async def list_products(limit: int = 50, offset: int = 0) -> list[dict]:
    query = ListProductsQuery(limit=limit, offset=offset)
    return await product_queries.handle(query)
```

## Benefits of CQRS

1. **Independent Scaling** - Read and write workloads can scale independently
2. **Optimized Read Models** - Read models can be denormalized for specific queries
3. **Performance** - Different models for different read patterns
4. **Flexibility** - Write side can use relational DB, read side can use document store
5. **Event Sourcing** - Complete audit trail of all changes

## Challenges

1. **Complexity** - More components and architectural complexity
2. **Consistency** - Eventual consistency between read and write models
3. **Learning Curve** - Team needs to understand the pattern
4. **Testing** - More components to test in isolation

## Summary

- CQRS separates read and write operations into different models
- Commands handle writes, queries handle reads
- Event bus propagates changes to materialized views
- Read models can be optimized for specific query patterns
- This pattern enables independent scaling and better performance

## Next Steps

Continue to `10-strangler-fig-architecture.md` to learn about modernizing legacy applications.
