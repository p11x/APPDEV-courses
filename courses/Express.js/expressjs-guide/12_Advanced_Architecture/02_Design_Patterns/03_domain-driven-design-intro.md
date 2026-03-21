# Domain-Driven Design Intro

## 📌 What You'll Learn

- Core DDD concepts: entities, value objects, aggregates, bounded contexts
- How to map DDD concepts to Express routes and architecture
- Organizing code by domain rather than technical layer
- When to apply DDD and when to keep it simple

## 🧠 Concept Explained (Plain English)

**Domain-Driven Design (DDD)** is an approach to software development that focuses on understanding and modeling the business domain (the "real world" problem you're solving) before thinking about technical implementation.

Think about building an e-commerce store:
- **Naive approach**: Tables for users, products, orders, payments (technical focus)
- **DDD approach**: Understand the business — customers, catalog, shopping cart, checkout, fulfillment (domain focus)

**Key DDD concepts:**

- **Entity**: Something with identity that persists over time (Order #123, User john@email.com)
- **Value Object**: Descriptive without identity (Address, Money, Color)
- **Aggregate**: Cluster of related entities treated as one unit (Order contains OrderItems)
- **Bounded Context**: Boundary where a particular domain model applies (Checkout context vs. Catalog context)
- **Domain Service**: Operations that don't belong to a single entity

**DDD in Express:**
- Routes organized by domain (not by HTTP methods)
- Domain logic in services/entities, not in route handlers
- Clear boundaries between bounded contexts

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';

const app = express();


// ============================================
// Domain Layer
// ============================================

/*
 * In a real DDD project, these would be in separate files/folders:
 * - src/domain/entities/
 * - src/domain/valueObjects/
 * - src/domain/aggregates/
 * - src/domain/services/
 * - src/domain/repositories/
 */

// ============================================
// Value Objects (immutable, no identity)
// ============================================

class Money {
  constructor(amount, currency = 'USD') {
    this.amount = amount;
    this.currency = currency;
  }
  
  add(other) {
    if (other.currency !== this.currency) {
      throw new Error('Cannot add different currencies');
    }
    return new Money(this.amount + other.amount, this.currency);
  }
  
  format() {
    return `${this.currency} ${this.amount.toFixed(2)}`;
  }
}

class Address {
  constructor(street, city, state, zip) {
    this.street = street;
    this.city = city;
    this.state = state;
    this.zip = zip;
  }
  
  equals(other) {
    return this.street === other.street &&
           this.city === other.city &&
           this.state === other.state &&
           this.zip === other.zip;
  }
}

// ============================================
// Entities (have identity)
// ============================================

class Product {
  constructor(id, name, price, stock) {
    this.id = id;
    this.name = name;
    this.price = price; // Money value object
    this.stock = stock;
  }
  
  isAvailable() {
    return this.stock > 0;
  }
  
  reduceStock(quantity) {
    if (this.stock < quantity) {
      throw new Error('Insufficient stock');
    }
    this.stock -= quantity;
  }
}

// ============================================
// Aggregates (cluster of entities)
// ============================================

class OrderAggregate {
  constructor(id, customerId) {
    this.id = id;
    this.customerId = customerId;
    this.items = [];
    this.status = 'pending';
    this.createdAt = new Date();
  }
  
  addItem(product, quantity) {
    if (!product.isAvailable()) {
      throw new Error(`Product ${product.name} is not available`);
    }
    
    const existing = this.items.find(i => i.productId === product.id);
    
    if (existing) {
      existing.quantity += quantity;
    } else {
      this.items.push({
        productId: product.id,
        name: product.name,
        unitPrice: product.price.amount,
        quantity
      });
    }
    
    product.reduceStock(quantity);
  }
  
  getTotal() {
    return this.items.reduce(
      (sum, item) => sum + (item.unitPrice * item.quantity),
      0
    );
  }
  
  submit() {
    if (this.items.length === 0) {
      throw new Error('Cannot submit empty order');
    }
    this.status = 'submitted';
    this.submittedAt = new Date();
  }
  
  complete() {
    this.status = 'completed';
    this.completedAt = new Date();
  }
}


// ============================================
// Domain Services (business operations)
// ============================================

class OrderService {
  constructor(productRepository, orderRepository) {
    this.products = productRepository;
    this.orders = orderRepository;
  }
  
  async createOrder(customerId, items) {
    // Create aggregate
    const order = new OrderAggregate(
      'order-' + Date.now(),
      customerId
    );
    
    // Add items
    for (const item of items) {
      const product = await this.products.findById(item.productId);
      order.addItem(product, item.quantity);
    }
    
    // Submit
    order.submit();
    
    // Save
    await this.orders.save(order);
    
    return order;
  }
  
  async completeOrder(orderId) {
    const order = await this.orders.findById(orderId);
    
    if (!order) {
      throw new Error('Order not found');
    }
    
    order.complete();
    await this.orders.save(order);
    
    return order;
  }
}


// ============================================
// Repositories (data access)
// ============================================

class ProductRepository {
  constructor() {
    this.products = new Map([
      ['prod-1', new Product('prod-1', 'Laptop', new Money(999.99), 10)],
      ['prod-2', new Product('prod-2', 'Mouse', new Money(29.99), 100)],
      ['prod-3', new Product('prod-3', 'Keyboard', new Money(79.99), 50)]
    ]);
  }
  
  async findById(id) {
    return this.products.get(id);
  }
  
  async findAll() {
    return Array.from(this.products.values());
  }
  
  async save(product) {
    this.products.set(product.id, product);
  }
}

class OrderRepository {
  constructor() {
    this.orders = new Map();
  }
  
  async findById(id) {
    return this.orders.get(id);
  }
  
  async save(order) {
    this.orders.set(order.id, order);
  }
  
  async findByCustomer(customerId) {
    return Array.from(this.orders.values())
      .filter(o => o.customerId === customerId);
  }
}


// ============================================
// Initialize Services
// ============================================

const productRepo = new ProductRepository();
const orderRepo = new OrderRepository();
const orderService = new OrderService(productRepo, orderRepo);


// ============================================
// Express Routes (mapped to bounded contexts)
// ============================================

app.use(express.json());

// Catalog Bounded Context
const catalogRouter = express.Router();

catalogRouter.get('/products', async (req, res) => {
  const products = await productRepo.findAll();
  res.json({
    products: products.map(p => ({
      id: p.id,
      name: p.name,
      price: p.price.format(),
      available: p.isAvailable()
    }))
  });
});

catalogRouter.get('/products/:id', async (req, res) => {
  const product = await productRepo.findById(req.params.id);
  
  if (!product) {
    return res.status(404).json({ error: 'Product not found' });
  }
  
  res.json({
    id: product.id,
    name: product.name,
    price: product.price.format(),
    stock: product.stock
  });
});


// Order Bounded Context  
const orderRouter = express.Router();

orderRouter.post('/orders', async (req, res) => {
  const { customerId, items } = req.body || {};
  
  if (!customerId || !items?.length) {
    return res.status(400).json({ error: 'Customer ID and items required' });
  }
  
  try {
    const order = await orderService.createOrder(customerId, items);
    res.status(201).json({
      id: order.id,
      status: order.status,
      items: order.items,
      total: order.getTotal()
    });
  } catch (error) {
    res.status(400).json({ error: error.message });
  }
});

orderRouter.get('/orders/:id', async (req, res) => {
  const order = await orderRepo.findById(req.params.id);
  
  if (!order) {
    return res.status(404).json({ error: 'Order not found' });
  }
  
  res.json({
    id: order.id,
    customerId: order.customerId,
    status: order.status,
    items: order.items,
    total: order.getTotal(),
    createdAt: order.createdAt
  });
});

orderRouter.post('/orders/:id/complete', async (req, res) => {
  try {
    const order = await orderService.completeOrder(req.params.id);
    res.json({ id: order.id, status: order.status });
  } catch (error) {
    res.status(400).json({ error: error.message });
  }
});


// Mount routers
app.use('/api', catalogRouter);
app.use('/api', orderRouter);


app.get('/health', (req, res) => {
  res.json({ status: 'ok' });
});


const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 19-35 | `Money` value object | Immutable currency amount |
| 38-49 | `Address` value object | Immutable address without ID |
| 54-70 | `Product` entity | Has ID, persists over time |
| 77-117 | `OrderAggregate` | Cluster of order items as single unit |
| 80 | `items` array | Related entities |
| 85-95 | `addItem()` method | Business rule for adding items |
| 124-150 | `OrderService` | Domain service for order operations |
| 158-176 | `ProductRepository` | Data access for products |
| 179-198 | `OrderRepository` | Data access for orders |
| 206-217 | Catalog router | Routes for product bounded context |
| 222-237 | Order router | Routes for order bounded context |

## ⚠️ Common Mistakes

### 1. Over-engineering with DDD

**What it is**: Using DDD for simple CRUD apps.

**Why it happens**: Wanting to apply "advanced" patterns everywhere.

**How to fix it**: Start simple, add DDD only when domain complexity justifies it.

### 2. Anemic domain models

**What it is**: Models with only data, no behavior (like old PHP/ASP).

**Why it happens**: Putting all logic in controllers or services.

**How to fix it**: Add behavior to entities and aggregates.

### 3. Ignoring bounded contexts

**What it is**: One big model for everything.

**Why it happens**: Not thinking about domain boundaries.

**How to fix it**: Identify separate contexts (catalog, orders, billing) early.

## ✅ Quick Recap

- DDD focuses on business domain before technical implementation
- Entities have identity, value objects are descriptive
- Aggregates are clusters treated as one unit
- Bounded contexts define where models apply
- Organize Express routes around domains, not HTTP methods

## 🔗 What's Next

Now learn about [Hexagonal Architecture](./04_hexagonal-architecture.md).
