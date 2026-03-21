# CQRS Pattern

## 📌 What You'll Learn

- What CQRS (Command Query Responsibility Segregation) means
- How to separate read and write models in Express
- Why this pattern improves performance and scalability
- Implementing CQRS with separate endpoints and data sources

## 🧠 Concept Explained (Plain English)

Most applications do the same operations for reading and writing data — they use the same models, same database queries, same endpoints. But reads and writes have very different characteristics:

- **Reads**: Can be frequent, need to be fast, benefit from caching
- **Writes**: Need validation, business logic, data integrity

**CQRS** means separating these two sides:
- **Commands**: Operations that change state (create, update, delete)
- **Queries**: Operations that read data without modifying

Instead of one model doing everything, you have:
- **Command Model**: Optimized for writes, enforces business rules
- **Query Model**: Optimized for reads, can be denormalized, cached

**Why bother?**
- Reads can be scaled independently from writes
- Different databases for different purposes (e.g., PostgreSQL for writes, Elasticsearch for reads)
- Optimized read models for specific views
- Better performance for read-heavy applications

**Real-world analogy**: A restaurant has cooks (command side) preparing food in the kitchen, and servers (query side) bringing food to tables. They don't do each other's jobs!

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';

const app = express();


// ============================================
// Models (Conceptual - in production, these would be DB interactions)
// ============================================

// Command side: strict, validated, business logic
class CommandModel {
  async createUser(userData) {
    // Validation
    if (!userData.email || !userData.email.includes('@')) {
      throw new Error('Invalid email');
    }
    
    if (!userData.name || userData.name.length < 2) {
      throw new Error('Name must be at least 2 characters');
    }
    
    // Create user in primary database
    const user = {
      id: 'user-' + Date.now(),
      ...userData,
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString(),
      version: 1
    };
    
    console.log('📝 User created (command):', user.id);
    return user;
  }
  
  async updateUser(userId, updates) {
    // Validation and business logic
    const user = { id: userId, ...updates, version: 1 };
    
    console.log('📝 User updated (command):', userId);
    return user;
  }
  
  async deleteUser(userId) {
    console.log('📝 User deleted (command):', userId);
    return { deleted: true, id: userId };
  }
}

// Query side: optimized for reads, can use caches, denormalized views
class QueryModel {
  // Cache for read optimization
  userCache = new Map();
  
  // Different query methods for different views
  
  async getUserById(userId) {
    // Check cache first
    if (this.userCache.has(userId)) {
      console.log('📖 User from cache:', userId);
      return this.userCache.get(userId);
    }
    
    // Fetch from database
    const user = {
      id: userId,
      email: 'user@example.com',
      name: 'John Doe',
      createdAt: new Date().toISOString(),
      // Could include computed fields
      accountAge: 365
    };
    
    this.userCache.set(userId, user);
    console.log('📖 User from DB (query):', userId);
    return user;
  }
  
  async getUserWithOrders(userId) {
    // Denormalized view - includes orders
    return {
      id: userId,
      name: 'John Doe',
      email: 'user@example.com',
      orders: [
        { id: 'order-1', total: 99.99, items: 2 },
        { id: 'order-2', total: 149.99, items: 3 }
      ],
      orderCount: 2,
      totalSpent: 249.98
    };
  }
  
  async getUserSummary() {
    // Lightweight summary for lists
    return [
      { id: 'user-1', name: 'John Doe', orderCount: 5 },
      { id: 'user-2', name: 'Jane Smith', orderCount: 3 },
      { id: 'user-3', name: 'Bob Wilson', orderCount: 8 }
    ];
  }
  
  async searchUsers(query) {
    // Could use Elasticsearch
    return [
      { id: 'user-1', name: 'John Doe', match: 'email contains ' + query },
      { id: 'user-2', name: 'Johnny Cash', match: 'name contains ' + query }
    ];
  }
  
  clearCache(userId) {
    this.userCache.delete(userId);
  }
}

const commands = new CommandModel();
const queries = new QueryModel();


// ============================================
// Middleware
// ============================================

app.use(express.json());


// ============================================
// Command Endpoints (Write) - POST, PUT, DELETE
// ============================================

// Create user - returns 201 Created
app.post('/api/users', async (req, res) => {
  try {
    const user = await commands.createUser(req.body);
    
    // Update read cache
    queries.clearCache(user.id);
    
    res.status(201).json(user);
  } catch (error) {
    res.status(400).json({ error: error.message });
  }
});

// Update user - returns 200 OK
app.put('/api/users/:id', async (req, res) => {
  try {
    const user = await commands.updateUser(req.params.id, req.body);
    
    // Invalidate cache
    queries.clearCache(user.id);
    
    res.json(user);
  } catch (error) {
    res.status(400).json({ error: error.message });
  }
});

// Delete user - returns 204 No Content
app.delete('/api/users/:id', async (req, res) => {
  try {
    await commands.deleteUser(req.params.id);
    
    // Clear cache
    queries.clearCache(req.params.id);
    
    res.status(204).send();
  } catch (error) {
    res.status(400).json({ error: error.message });
  }
});


// ============================================
// Query Endpoints (Read) - GET
// ============================================

// Get single user - basic view
app.get('/api/users/:id', async (req, res) => {
  try {
    const user = await queries.getUserById(req.params.id);
    res.json(user);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

// Get user with orders - denormalized read view
app.get('/api/users/:id/orders', async (req, res) => {
  try {
    const userWithOrders = await queries.getUserWithOrders(req.params.id);
    res.json(userWithOrders);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

// Get user summary list
app.get('/api/users', async (req, res) => {
  try {
    const users = await queries.getUserSummary();
    res.json({ users, count: users.length });
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

// Search users - different query endpoint
app.get('/api/users/search', async (req, res) => {
  const { q } = req.query;
  
  if (!q) {
    return res.status(400).json({ error: 'Search query required' });
  }
  
  try {
    const results = await queries.searchUsers(q);
    res.json({ results, count: results.length });
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});


// ============================================
// Orders (another example)
// ============================================

class OrderCommands {
  async createOrder(orderData) {
    // Complex validation
    if (!orderData.items || orderData.items.length === 0) {
      throw new Error('Order must have items');
    }
    
    const order = {
      id: 'order-' + Date.now(),
      ...orderData,
      status: 'pending',
      createdAt: new Date().toISOString()
    };
    
    console.log('📝 Order created:', order.id);
    return order;
  }
  
  async updateOrderStatus(orderId, status) {
    console.log('📝 Order status updated:', orderId, status);
    return { id: orderId, status };
  }
}

class OrderQueries {
  async getOrder(orderId) {
    return {
      id: orderId,
      items: [{ name: 'Widget', quantity: 2, price: 10 }],
      total: 20,
      status: 'pending'
    };
  }
  
  async getOrdersByUser(userId) {
    return [
      { id: 'order-1', total: 50, status: 'shipped' },
      { id: 'order-2', total: 25, status: 'delivered' }
    ];
  }
  
  async getOrderStats() {
    return {
      totalOrders: 150,
      pending: 10,
      shipped: 5,
      delivered: 135,
      revenue: 25000
    };
  }
}

const orderCommands = new OrderCommands();
const orderQueries = new OrderQueries();

// Command: Create order
app.post('/api/orders', async (req, res) => {
  try {
    const order = await orderCommands.createOrder(req.body);
    res.status(201).json(order);
  } catch (error) {
    res.status(400).json({ error: error.message });
  }
});

// Query: Get order
app.get('/api/orders/:id', async (req, res) => {
  const order = await orderQueries.getOrder(req.params.id);
  res.json(order);
});

// Query: Get user's orders
app.get('/api/users/:id/orders', async (req, res) => {
  const orders = await orderQueries.getOrdersByUser(req.params.id);
  res.json({ orders });
});

// Query: Order statistics
app.get('/api/stats/orders', async (req, res) => {
  const stats = await orderQueries.getOrderStats();
  res.json(stats);
});


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
| 14-40 | `CommandModel` class | Handles writes with validation |
| 45-100 | `QueryModel` class | Handles reads with caching |
| 62-70 | `getUserById` | Checks cache first, then DB |
| 73-85 | `getUserWithOrders` | Denormalized read view |
| 90-98 | `getUserSummary` | Lightweight list view |
| 125-145 | POST `/api/users` | Command to create |
| 148-160 | PUT `/api/users/:id` | Command to update |
| 163-172 | DELETE `/api/users/:id` | Command to delete |
| 177-186 | GET `/api/users/:id` | Query for single user |
| 189-197 | GET with orders | Denormalized view |
| 200-208 | GET list | Summary view |
| 211-223 | GET search | Full-text search |
| 239-275 | Order commands/queries | Separate read/write for orders |
| 260 | `createOrder` | Command with validation |
| 278 | GET single | Query endpoint |
| 281 | GET user's orders | Query endpoint |
| 284 | GET stats | Aggregated read |

## ⚠️ Common Mistakes

### 1. Overengineering for simple apps

**What it is**: Implementing CQRS for small apps that don't need it.

**Why it happens**: Wanting to use "advanced" patterns everywhere.

**How to fix it**: Start simple, add CQRS only when you have read/write performance needs.

### 2. Not keeping read and write models in sync

**What it is**: Reads return stale data after writes.

**Why it happens**: Not invalidating caches or not updating read models.

**How to fix it**: Use cache invalidation, event-driven updates, or eventual consistency.

### 3. Same endpoints for commands and queries

**What it is**: Using POST for everything or PUT for reads.

**Why it happens**: Not understanding REST conventions.

**How to fix it**: Use GET for queries, POST/PUT/DELETE for commands.

## ✅ Quick Recap

- CQRS separates read and write operations
- Commands handle writes with validation (POST, PUT, DELETE)
- Queries handle reads (GET) with optimized data views
- Different endpoints, potentially different data sources
- Can use caching for reads
- Improves performance for read-heavy applications

## 🔗 What's Next

Now learn about the [Repository Pattern Deep Dive](./02_repository-pattern-deep-dive.md) for abstracting data access.
