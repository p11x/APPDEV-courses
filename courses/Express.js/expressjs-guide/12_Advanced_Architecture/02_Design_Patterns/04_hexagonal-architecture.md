# Hexagonal Architecture

## 📌 What You'll Learn

- What hexagonal (ports and adapters) architecture is
- How Express can be just an adapter for your core business logic
- Keeping domain logic framework-independent
- Implementing dependency injection for testability

## 🧠 Concept Explained (Plain English)

**Hexagonal Architecture** (also called Ports and Adapters) is about keeping your core business logic independent of external frameworks like Express, databases, or external services.

Think of it like a smartphone:
- Your app's logic is the phone itself
- The charging port, headphone jack, USB-C are "ports"
- The charger, headphones, cables are "adapters"

You can swap the charger or headphones without changing the phone.

**Layers:**
1. **Domain/Core**: Pure business logic, no dependencies
2. **Ports**: Interfaces that define how to interact with the outside
3. **Adapters**: Implementations that connect to external systems

**In Express:**
- Express is an adapter (driving adapter for incoming HTTP)
- Database is an adapter (driven adapter for data storage)
- Your domain logic should work without Express

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';

const app = express();


// ============================================
// DOMAIN LAYER (Core - no dependencies)
// ============================================

/*
 * This code knows nothing about Express, databases, or HTTP.
 * It's pure business logic.
 */

class Order {
  constructor(id, customerId) {
    this.id = id;
    this.customerId = customerId;
    this.items = [];
    this.status = 'created';
  }
  
  addItem(product, quantity) {
    if (product.stock < quantity) {
      throw new Error('Insufficient stock');
    }
    
    this.items.push({
      productId: product.id,
      productName: product.name,
      quantity,
      price: product.price
    });
  }
  
  getTotal() {
    return this.items.reduce((sum, item) => sum + item.price * item.quantity, 0);
  }
  
  canShip() {
    return this.status === 'confirmed' && this.items.length > 0;
  }
}

class OrderService {
  constructor(orderRepository, productRepository, notificationPort) {
    // Dependencies injected via constructor (dependency inversion)
    this.orders = orderRepository;
    this.products = productRepository;
    this.notifications = notificationPort;
  }
  
  async createOrder(customerId, items) {
    const order = new Order('order-' + Date.now(), customerId);
    
    for (const item of items) {
      const product = await this.products.findById(item.productId);
      
      if (!product) {
        throw new Error(`Product ${item.productId} not found`);
      }
      
      order.addItem(product, item.quantity);
    }
    
    await this.orders.save(order);
    
    await this.notifications.send({
      type: 'ORDER_CREATED',
      orderId: order.id,
      customerId
    });
    
    return order;
  }
  
  async confirmOrder(orderId) {
    const order = await this.orders.findById(orderId);
    
    if (!order) {
      throw new Error('Order not found');
    }
    
    order.status = 'confirmed';
    await this.orders.save(order);
    
    return order;
  }
  
  async getOrder(orderId) {
    return this.orders.findById(orderId);
  }
  
  async getCustomerOrders(customerId) {
    return this.orders.findByCustomer(customerId);
  }
}


// ============================================
// PORTS (Interfaces)
// ============================================

/*
 * These define what adapters must implement.
 * In TypeScript, these would be interfaces.
 */

// Port for order storage
class OrderRepositoryPort {
  async save(order) { throw new Error('Not implemented'); }
  async findById(id) { throw new Error('Not implemented'); }
  async findByCustomer(customerId) { throw new Error('Not implemented'); }
}

// Port for product storage  
class ProductRepositoryPort {
  async findById(id) { throw new Error('Not implemented'); }
  async findAll() { throw new Error('Not implemented'); }
}

// Port for notifications
class NotificationPort {
  async send(message) { throw new Error('Not implemented'); }
}


// ============================================
// ADAPTERS (Implementations)
// ============================================

// In-memory adapter (for demo/dev)
class InMemoryOrderAdapter extends OrderRepositoryPort {
  constructor() {
    super();
    this.orders = new Map();
  }
  
  async save(order) {
    this.orders.set(order.id, order);
    return order;
  }
  
  async findById(id) {
    return this.orders.get(id) || null;
  }
  
  async findByCustomer(customerId) {
    return Array.from(this.orders.values())
      .filter(o => o.customerId === customerId);
  }
}

class InMemoryProductAdapter extends ProductRepositoryPort {
  constructor() {
    super();
    this.products = new Map([
      ['prod-1', { id: 'prod-1', name: 'Widget', price: 10, stock: 100 }],
      ['prod-2', { id: 'prod-2', name: 'Gadget', price: 20, stock: 50 }]
    ]);
  }
  
  async findById(id) {
    return this.products.get(id) || null;
  }
  
  async findAll() {
    return Array.from(this.products.values());
  }
}

class ConsoleNotificationAdapter extends NotificationPort {
  async send(message) {
    console.log('📧 Notification:', message);
  }
}

// Database adapter (example - would use real DB in production)
/*
class PostgreSQLOrderAdapter extends OrderRepositoryPort {
  constructor(pool) {
    super();
    this.pool = pool;
  }
  
  async save(order) {
    // Save to PostgreSQL
  }
  
  async findById(id) {
    // Query from PostgreSQL
  }
  
  async findByCustomer(customerId) {
    // Query from PostgreSQL
  }
}
*/


// ============================================
// APPLICATION LAYER (Wiring)
// ============================================

// Create adapters
const orderAdapter = new InMemoryOrderAdapter();
const productAdapter = new InMemoryProductAdapter();
const notificationAdapter = new ConsoleNotificationAdapter();

// Create service with injected dependencies
const orderService = new OrderService(
  orderAdapter,
  productAdapter,
  notificationAdapter
);


// ============================================
// ADAPTERS: Express (Driving Adapter)
// ============================================

app.use(express.json());

// Order routes
app.post('/api/orders', async (req, res) => {
  try {
    const { customerId, items } = req.body;
    
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

app.get('/api/orders/:id', async (req, res) => {
  try {
    const order = await orderService.getOrder(req.params.id);
    
    if (!order) {
      return res.status(404).json({ error: 'Order not found' });
    }
    
    res.json({
      id: order.id,
      status: order.status,
      items: order.items,
      total: order.getTotal()
    });
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

app.post('/api/orders/:id/confirm', async (req, res) => {
  try {
    const order = await orderService.confirmOrder(req.params.id);
    res.json({ id: order.id, status: order.status });
  } catch (error) {
    res.status(400).json({ error: error.message });
  }
});

app.get('/api/customers/:id/orders', async (req, res) => {
  const orders = await orderService.getCustomerOrders(req.params.id);
  res.json({ orders });
});

// Product routes (read-only for this example)
app.get('/api/products', async (req, res) => {
  const products = await productAdapter.findAll();
  res.json({ products });
});

app.get('/api/products/:id', async (req, res) => {
  const product = await productAdapter.findById(req.params.id);
  
  if (!product) {
    return res.status(404).json({ error: 'Product not found' });
  }
  
  res.json(product);
});


app.get('/health', (req, res) => {
  res.json({ status: 'ok' });
});


const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
  console.log('\n📝 Architecture:');
  console.log('  Domain: Order, OrderService (no Express dependencies)');
  console.log('  Ports: OrderRepositoryPort, ProductRepositoryPort');
  console.log('  Adapters: InMemory adapters, Express adapter');
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 14-42 | `Order` class | Pure domain entity, no imports |
| 48-73 | `OrderService` | Domain service with injected dependencies |
| 85-103 | `OrderRepositoryPort` | Interface for order storage |
| 106-110 | `ProductRepositoryPort` | Interface for product storage |
| 113-117 | `NotificationPort` | Interface for notifications |
| 123-145 | `InMemoryOrderAdapter` | In-memory implementation of port |
| 163-173 | Wiring | Creates adapters and service |
| 185-213 | Express routes | HTTP adapter using domain service |

## ⚠️ Common Mistakes

### 1. Leaking Express into domain

**What it is**: Using `req`, `res`, `express.json()` in domain layer.

**Why it happens**: Not separating concerns early.

**How to fix it**: Keep domain pure, pass only data to/from it.

### 2. Not using dependency injection

**What it is**: Creating dependencies inside domain classes.

**Why it happens**: Easiest approach in development.

**How to fix it**: Pass dependencies via constructor (dependency injection).

### 3. Too many abstractions

**What it is**: Creating ports for everything, over-engineering.

**Why it happens**: Following rules without understanding purpose.

**How to fix it**: Only abstract what might change (databases, external services).

## ✅ Quick Recap

- Hexagonal architecture separates core domain from external concerns
- Domain layer has no dependencies on frameworks
- Ports define interfaces for external interactions
- Adapters implement ports (Express, DB, etc.)
- Express becomes just one adapter among many
- Dependencies flow inward toward domain

## 🔗 What's Next

Now learn about the [Plugin Extension System](./05_plugin-extension-system.md).
