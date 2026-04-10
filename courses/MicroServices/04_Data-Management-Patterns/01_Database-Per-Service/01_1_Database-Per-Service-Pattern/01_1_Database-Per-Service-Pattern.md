# Database Per Service Pattern

## Overview

The Database Per Service pattern is a fundamental architectural principle in microservices where each service owns and manages its own data store. This pattern is a core component of the microservices architecture, enabling services to be independently developed, deployed, and scaled. Instead of sharing a single database across multiple services, each service gets its own dedicated database that it controls entirely.

This pattern emerged from the need to eliminate tight coupling between services. In monolithic architectures, all components share a single database, which creates dependencies that make it difficult to change one part of the application without affecting others. The Database Per Service pattern solves this by establishing clear ownership boundaries - each service is responsible for its data, and other services can only access that data through well-defined APIs.

The pattern enforces the principle of loose coupling while maintaining high cohesion within each service. Each service can choose the database technology that best fits its data access patterns, scaling requirements, and performance needs. This freedom to choose different database technologies is often called polyglot persistence, which we'll explore in more detail in another section.

Data ownership becomes crystal clear with this pattern. The service that owns the data is the only one that can write to its database. Other services must consume data through the owning service's APIs, typically over a network. This eliminates the scenario where multiple services directly modify the same tables, which often leads to integration nightmares and tight coupling.

From a deployment perspective, each service and its database can be deployed independently. You can update the inventory service and its database without affecting the order service or customer service. This independence significantly reduces the risk associated with changes and enables teams to work autonomously on different services.

The pattern also enables true horizontal scaling. When a particular service experiences high load, you can scale only that service along with its database. You don't need to scale the entire application, which would be wastefully expensive if only one component is under stress.

### Key Characteristics

The Database Per Service pattern has several defining characteristics that distinguish it from other approaches. First, each service has exclusive write access to its own data store. No other service can directly write to this database - they must go through the service's API. Second, the schema is private to each service. Other services don't know the internal structure of the database; they only interact through documented APIs.

Another characteristic is data encapsulation. The service hides the implementation details of how it stores and retrieves data. This means you can change the underlying database schema or even switch to a completely different database technology without affecting consumers, as long as the API contract remains the same.

Finally, transactions are scoped to a single service. When you need to maintain consistency across multiple services, you can't rely on traditional database transactions. Instead, you need to use eventual consistency patterns, which we'll discuss later in this section.

## Flow Diagram

```mermaid
graph TB
    subgraph "Client Applications"
        A[Web App]
        B[Mobile App]
        C[External API]
    end
    
    subgraph "Order Service"
        D[Order Service API]
        E[Order Database]
    end
    
    subgraph "Inventory Service"
        F[Inventory Service API]
        G[Inventory Database]
    end
    
    subgraph "Customer Service"
        H[Customer Service API]
        I[Customer Database]
    end
    
    A --> D
    A --> F
    A --> H
    B --> D
    B --> F
    B --> H
    C --> D
    C --> F
    C --> H
    
    D --> E
    F --> G
    H --> I
    
    D -.->|Async Event| F
    D -.->|Async Event| H
```

The diagram illustrates how each service maintains its own dedicated database. Client applications interact with services through their APIs, and services communicate with each other through asynchronous events when needed. Notice that there are no direct connections between service databases - all data access goes through service APIs.

## Standard Example

Let's implement a simple e-commerce system with multiple services, each having its own database. We'll use Node.js with Express and PostgreSQL for this example.

### Order Service

```javascript
// order-service/src/index.js
const express = require('express');
const { Pool } = require('pg');

const app = express();
const pool = new Pool({
    connectionString: process.env.DATABASE_URL || 'postgresql://localhost:5432/orders'
});

app.use(express.json());

// Create a new order
app.post('/api/orders', async (req, res) => {
    const { customerId, items, totalAmount } = req.body;
    
    const client = await pool.connect();
    try {
        await client.query('BEGIN');
        
        const orderResult = await client.query(
            `INSERT INTO orders (customer_id, total_amount, status, created_at)
             VALUES ($1, $2, $3, NOW())
             RETURNING id, created_at`,
            [customerId, totalAmount, 'PENDING']
        );
        
        const orderId = orderResult.rows[0].id;
        
        for (const item of items) {
            await client.query(
                `INSERT INTO order_items (order_id, product_id, quantity, unit_price)
                 VALUES ($1, $2, $3, $4)`,
                [orderId, item.productId, item.quantity, item.unitPrice]
            );
        }
        
        await client.query('COMMIT');
        
        res.status(201).json({
            id: orderId,
            status: 'PENDING',
            createdAt: orderResult.rows[0].created_at
        });
    } catch (error) {
        await client.query('ROLLBACK');
        console.error('Error creating order:', error);
        res.status(500).json({ error: 'Failed to create order' });
    } finally {
        client.release();
    }
});

// Get order by ID
app.get('/api/orders/:id', async (req, res) => {
    const { id } = req.params;
    
    try {
        const orderResult = await pool.query(
            `SELECT id, customer_id, total_amount, status, created_at
             FROM orders WHERE id = $1`,
            [id]
        );
        
        if (orderResult.rows.length === 0) {
            return res.status(404).json({ error: 'Order not found' });
        }
        
        const itemsResult = await pool.query(
            `SELECT product_id, quantity, unit_price
             FROM order_items WHERE order_id = $1`,
            [id]
        );
        
        res.json({
            ...orderResult.rows[0],
            items: itemsResult.rows
        });
    } catch (error) {
        console.error('Error fetching order:', error);
        res.status(500).json({ error: 'Failed to fetch order' });
    }
});

// Get all orders for a customer
app.get('/api/orders/customer/:customerId', async (req, res) => {
    const { customerId } = req.params;
    
    try {
        const result = await pool.query(
            `SELECT id, total_amount, status, created_at
             FROM orders WHERE customer_id = $1
             ORDER BY created_at DESC`,
            [customerId]
        );
        
        res.json(result.rows);
    } catch (error) {
        console.error('Error fetching orders:', error);
        res.status(500).json({ error: 'Failed to fetch orders' });
    }
});

// Update order status
app.patch('/api/orders/:id/status', async (req, res) => {
    const { id } = req.params;
    const { status } = req.body;
    
    const validStatuses = ['PENDING', 'CONFIRMED', 'SHIPPED', 'DELIVERED', 'CANCELLED'];
    
    if (!validStatuses.includes(status)) {
        return res.status(400).json({ error: 'Invalid status value' });
    }
    
    try {
        const result = await pool.query(
            `UPDATE orders SET status = $1, updated_at = NOW()
             WHERE id = $2 RETURNING id, status`,
            [status, id]
        );
        
        if (result.rows.length === 0) {
            return res.status(404).json({ error: 'Order not found' });
        }
        
        // Publish event for other services
        await publishEvent('order.status.updated', {
            orderId: id,
            newStatus: status,
            timestamp: new Date().toISOString()
        });
        
        res.json(result.rows[0]);
    } catch (error) {
        console.error('Error updating order status:', error);
        res.status(500).json({ error: 'Failed to update order status' });
    }
});

// Database schema initialization
async function initializeDatabase() {
    const createTables = `
        CREATE TABLE IF NOT EXISTS orders (
            id SERIAL PRIMARY KEY,
            customer_id INTEGER NOT NULL,
            total_amount DECIMAL(10, 2) NOT NULL,
            status VARCHAR(50) NOT NULL DEFAULT 'PENDING',
            created_at TIMESTAMP DEFAULT NOW(),
            updated_at TIMESTAMP DEFAULT NOW()
        );
        
        CREATE TABLE IF NOT EXISTS order_items (
            id SERIAL PRIMARY KEY,
            order_id INTEGER REFERENCES orders(id),
            product_id INTEGER NOT NULL,
            quantity INTEGER NOT NULL,
            unit_price DECIMAL(10, 2) NOT NULL
        );
        
        CREATE INDEX IF NOT EXISTS idx_orders_customer_id ON orders(customer_id);
        CREATE INDEX IF NOT EXISTS idx_order_items_order_id ON order_items(order_id);
    `;
    
    await pool.query(createTables);
    console.log('Order service database initialized');
}

// Simple event publisher (would use RabbitMQ/Kafka in production)
async function publishEvent(eventType, payload) {
    console.log(`Publishing event: ${eventType}`, payload);
    // In production, this would publish to a message broker
}

const PORT = process.env.PORT || 3001;
app.listen(PORT, async () => {
    await initializeDatabase();
    console.log(`Order service running on port ${PORT}`);
});

module.exports = app;
```

### Customer Service

```javascript
// customer-service/src/index.js
const express = require('express');
const { Pool } = require('pg');

const app = express();
const pool = new Pool({
    connectionString: process.env.DATABASE_URL || 'postgresql://localhost:5433/customers'
});

app.use(express.json());

// Create a new customer
app.post('/api/customers', async (req, res) => {
    const { email, name, address, phone } = req.body;
    
    try {
        const result = await pool.query(
            `INSERT INTO customers (email, name, address, phone, created_at)
             VALUES ($1, $2, $3, $4, NOW())
             RETURNING id, email, name, created_at`,
            [email, name, address, phone]
        );
        
        res.status(201).json(result.rows[0]);
    } catch (error) {
        if (error.code === '23505') {
            return res.status(409).json({ error: 'Email already exists' });
        }
        console.error('Error creating customer:', error);
        res.status(500).json({ error: 'Failed to create customer' });
    }
});

// Get customer by ID
app.get('/api/customers/:id', async (req, res) => {
    const { id } = req.params;
    
    try {
        const result = await pool.query(
            `SELECT id, email, name, address, phone, created_at
             FROM customers WHERE id = $1`,
            [id]
        );
        
        if (result.rows.length === 0) {
            return res.status(404).json({ error: 'Customer not found' });
        }
        
        res.json(result.rows[0]);
    } catch (error) {
        console.error('Error fetching customer:', error);
        res.status(500).json({ error: 'Failed to fetch customer' });
    }
});

// Update customer
app.put('/api/customers/:id', async (req, res) => {
    const { id } = req.params;
    const { email, name, address, phone } = req.body;
    
    try {
        const result = await pool.query(
            `UPDATE customers 
             SET email = COALESCE($1, email),
                 name = COALESCE($2, name),
                 address = COALESCE($3, address),
                 phone = COALESCE($4, phone),
                 updated_at = NOW()
             WHERE id = $5
             RETURNING id, email, name, address, phone`,
            [email, name, address, phone, id]
        );
        
        if (result.rows.length === 0) {
            return res.status(404).json({ error: 'Customer not found' });
        }
        
        res.json(result.rows[0]);
    } catch (error) {
        if (error.code === '23505') {
            return res.status(409).json({ error: 'Email already exists' });
        }
        console.error('Error updating customer:', error);
        res.status(500).json({ error: 'Failed to update customer' });
    }
});

// Get customer with orders (calls Order Service)
app.get('/api/customers/:id/orders', async (req, res) => {
    const { id } = req.params;
    
    try {
        // First get customer
        const customerResult = await pool.query(
            `SELECT id, email, name FROM customers WHERE id = $1`,
            [id]
        );
        
        if (customerResult.rows.length === 0) {
            return res.status(404).json({ error: 'Customer not found' });
        }
        
        // Call order service to get orders
        const orderServiceUrl = process.env.ORDER_SERVICE_URL || 'http://localhost:3001';
        const ordersResponse = await fetch(`${orderServiceUrl}/api/orders/customer/${id}`);
        const orders = await ordersResponse.json();
        
        res.json({
            customer: customerResult.rows[0],
            orders: orders
        });
    } catch (error) {
        console.error('Error fetching customer orders:', error);
        res.status(500).json({ error: 'Failed to fetch customer orders' });
    }
});

// Database schema initialization
async function initializeDatabase() {
    const createTables = `
        CREATE TABLE IF NOT EXISTS customers (
            id SERIAL PRIMARY KEY,
            email VARCHAR(255) UNIQUE NOT NULL,
            name VARCHAR(255) NOT NULL,
            address TEXT,
            phone VARCHAR(50),
            created_at TIMESTAMP DEFAULT NOW(),
            updated_at TIMESTAMP DEFAULT NOW()
        );
        
        CREATE INDEX IF NOT EXISTS idx_customers_email ON customers(email);
    `;
    
    await pool.query(createTables);
    console.log('Customer service database initialized');
}

const PORT = process.env.PORT || 3002;
app.listen(PORT, async () => {
    await initializeDatabase();
    console.log(`Customer service running on port ${PORT}`);
});

module.exports = app;
```

### Inventory Service

```javascript
// inventory-service/src/index.js
const express = require('express');
const { Pool } = require('pg');

const app = express();
const pool = new Pool({
    connectionString: process.env.DATABASE_URL || 'postgresql://localhost:5434/inventory'
});

app.use(express.json());

// Add new product
app.post('/api/products', async (req, res) => {
    const { name, description, price, quantity } = req.body;
    
    try {
        const result = await pool.query(
            `INSERT INTO products (name, description, price, quantity, created_at)
             VALUES ($1, $2, $3, $4, NOW())
             RETURNING *`,
            [name, description, price, quantity]
        );
        
        res.status(201).json(result.rows[0]);
    } catch (error) {
        console.error('Error creating product:', error);
        res.status(500).json({ error: 'Failed to create product' });
    }
});

// Get product by ID
app.get('/api/products/:id', async (req, res) => {
    const { id } = req.params;
    
    try {
        const result = await pool.query(
            `SELECT * FROM products WHERE id = $1`,
            [id]
        );
        
        if (result.rows.length === 0) {
            return res.status(404).json({ error: 'Product not found' });
        }
        
        res.json(result.rows[0]);
    } catch (error) {
        console.error('Error fetching product:', error);
        res.status(500).json({ error: 'Failed to fetch product' });
    }
});

// Reserve inventory for an order
app.post('/api/inventory/reserve', async (req, res) => {
    const { items } = req.body; // Array of { productId, quantity }
    
    const client = await pool.connect();
    try {
        await client.query('BEGIN');
        
        const results = [];
        
        for (const item of items) {
            // Lock the row and check availability
            const result = await client.query(
                `UPDATE products 
                 SET quantity = quantity - $2,
                     reserved = reserved + $2,
                     updated_at = NOW()
                 WHERE id = $1 AND quantity >= $2
                 RETURNING id, name, quantity, reserved`,
                [item.productId, item.quantity]
            );
            
            if (result.rows.length === 0) {
                await client.query('ROLLBACK');
                return res.status(400).json({ 
                    error: 'Insufficient inventory',
                    productId: item.productId 
                });
            }
            
            results.push(result.rows[0]);
        }
        
        await client.query('COMMIT');
        
        res.json({
            success: true,
            reservedItems: results
        });
    } catch (error) {
        await client.query('ROLLBACK');
        console.error('Error reserving inventory:', error);
        res.status(500).json({ error: 'Failed to reserve inventory' });
    } finally {
        client.release();
    }
});

// Release inventory (when order is cancelled)
app.post('/api/inventory/release', async (req, res) => {
    const { items } = req.body;
    
    const client = await pool.connect();
    try {
        await client.query('BEGIN');
        
        for (const item of items) {
            await client.query(
                `UPDATE products 
                 SET quantity = quantity + $2,
                     reserved = reserved - $2,
                     updated_at = NOW()
                 WHERE id = $1`,
                [item.productId, item.quantity]
            );
        }
        
        await client.query('COMMIT');
        
        res.json({ success: true });
    } catch (error) {
        await client.query('ROLLBACK');
        console.error('Error releasing inventory:', error);
        res.status(500).json({ error: 'Failed to release inventory' });
    } finally {
        client.release();
    }
});

// Database schema initialization
async function initializeDatabase() {
    const createTables = `
        CREATE TABLE IF NOT EXISTS products (
            id SERIAL PRIMARY KEY,
            name VARCHAR(255) NOT NULL,
            description TEXT,
            price DECIMAL(10, 2) NOT NULL,
            quantity INTEGER NOT NULL DEFAULT 0,
            reserved INTEGER NOT NULL DEFAULT 0,
            created_at TIMESTAMP DEFAULT NOW(),
            updated_at TIMESTAMP DEFAULT NOW()
        );
        
        CREATE INDEX IF NOT EXISTS idx_products_name ON products(name);
    `;
    
    await pool.query(createTables);
    console.log('Inventory service database initialized');
}

const PORT = process.env.PORT || 3003;
app.listen(PORT, async () => {
    await initializeDatabase();
    console.log(`Inventory service running on port ${PORT}`);
});

module.exports = app;
```

### Docker Compose for Local Development

```yaml
# docker-compose.yml
version: '3.8'

services:
  order-service:
    build: ./order-service
    ports:
      - "3001:3001"
    environment:
      - DATABASE_URL=postgresql://postgres:postgres@order-db:5432/orders
      - CUSTOMER_SERVICE_URL=http://customer-service:3002
      - INVENTORY_SERVICE_URL=http://inventory-service:3003
    depends_on:
      - order-db

  customer-service:
    build: ./customer-service
    ports:
      - "3002:3002"
    environment:
      - DATABASE_URL=postgresql://postgres:postgres@customer-db:5432/customers
      - ORDER_SERVICE_URL=http://order-service:3001
    depends_on:
      - customer-db

  inventory-service:
    build: ./inventory-service
    ports:
      - "3003:3003"
    environment:
      - DATABASE_URL=postgresql://postgres:postgres@inventory-db:5432/inventory
    depends_on:
      - inventory-db

  order-db:
    image: postgres:15
    environment:
      - POSTGRES_PASSWORD=postgres
      - POSTGRES_DB=orders
    volumes:
      - order-data:/var/lib/postgresql/data
    ports:
      - "5432:5432"

  customer-db:
    image: postgres:15
    environment:
      - POSTGRES_PASSWORD=postgres
      - POSTGRES_DB=customers
    volumes:
      - customer-data:/var/lib/postgresql/data
    ports:
      - "5433:5432"

  inventory-db:
    image: postgres:15
    environment:
      - POSTGRES_PASSWORD=postgres
      - POSTGRES_DB=inventory
    volumes:
      - inventory-data:/var/lib/postgresql/data
    ports:
      - "5434:5432"

volumes:
  order-data:
  customer-data:
  inventory-data:
```

## Real-World Examples

### Netflix

Netflix is one of the most prominent examples of microservices with database-per-service architecture. They run thousands of microservices, each with its own data store. Their content metadata service has its own Cassandra database for handling massive amounts of metadata about movies and TV shows. The playback history service uses a different database optimized for time-series data. Each team can choose the best database for their specific use case while maintaining loose coupling.

Netflix's approach allows different teams to work independently. The team responsible for recommendations can scale their database horizontally without affecting the team working on user authentication. They use Apache Cassandra extensively because it provides the high write throughput needed for their playback events while maintaining the availability required for their global audience.

### Amazon

Amazon's transition from a monolithic architecture to microservices involved implementing the database-per-service pattern. Their order management system consists of multiple services, each with its own database. The inventory service uses Oracle Database, while the recommendation service uses proprietary data stores optimized for machine learning workloads. This separation allows Amazon to scale individual services based on their specific performance characteristics.

Amazon also pioneered the concept of service ownership, where each team is fully responsible for their service's development, deployment, and data management. This ownership model ensures that teams make informed decisions about their database technology and can iterate quickly without coordinating with other teams.

### Uber

Uber's architecture consists of numerous microservices, each owning its data. The trip service maintains its own data store for handling the lifecycle of rides. The pricing service has its own database for fare calculations. The user service manages customer data in a separate database. This architecture allows Uber to handle millions of rides daily while maintaining system reliability and enabling rapid feature development.

Uber's implementation emphasizes strong API contracts between services. Since databases are not shared, all inter-service communication happens through well-defined APIs. This approach has enabled Uber to migrate their infrastructure, update their mobile apps, and introduce new features without system-wide regressions.

### Spotify

Spotify uses database-per-service extensively for their music streaming platform. Their user data service uses PostgreSQL, while their music catalog service uses Cassandra to handle the massive amount of track and artist information. The playlist service maintains its own data store optimized for the specific access patterns of playlist operations.

Each service at Spotify is owned by a specific team that makes autonomous decisions about technology choices. This autonomy has enabled Spotify to innovate quickly and maintain high availability for their 500+ million users worldwide.

## Best Practices

### 1. Define Clear Data Ownership

Every piece of data in your system should have a clear owner. The owning service is the only one that can write to its database. Other services must consume data through APIs. Document these ownership boundaries clearly and enforce them through code reviews and architecture reviews. This clarity prevents the "shared database" anti-pattern where multiple services directly access the same tables.

When designing services, ask yourself: "Which service truly owns this data?" The answer should be obvious for most data types. Customer data belongs to the customer service, product data to inventory service, and so on. If you can't identify an owner, you might need to reconsider your service boundaries.

### 2. Use API-First Design

Design your service APIs before implementing the database schema. This approach ensures that your data store remains private and encapsulated. Other services should never need to know how you store your data - they should only interact through your APIs. This encapsulation gives you the freedom to change your database technology or schema without affecting consumers.

Document your APIs using OpenAPI or similar specifications. This documentation becomes the contract that other services depend on. Make sure this contract is stable and versioned properly.

### 3. Handle Distributed Transactions Carefully

When you need consistency across multiple services, you cannot use traditional database transactions. Instead, adopt eventual consistency patterns. Use asynchronous events to coordinate work across services. Implement compensation logic (sagas) for operations that span multiple services.

Always design for failure. What happens if a service is down when another service tries to update data? Implement retry mechanisms with idempotency to handle transient failures. Use the outbox pattern to ensure events are reliably published even when the service crashes.

### 4. Choose the Right Database Per Service

Don't default to the same database for all services. Analyze each service's data access patterns, scaling requirements, and performance needs. A service handling financial transactions might need ACID compliance and use PostgreSQL. A service handling user activity logs might benefit from a time-series database like InfluxDB. A service storing product catalog might work well with a document database like MongoDB.

This polyglot persistence approach allows you to optimize each service for its specific needs. However, don't choose different databases just for the sake of variety - each choice should be justified by the service's requirements.

### 5. Implement Proper Database Security

Each database should have its own credentials, and services should only have the minimum permissions they need. Never use shared database accounts across services. Implement network-level isolation so that databases can only be accessed by their owning services and not directly from external networks.

Enable encryption at rest and in transit for all databases. Implement proper backup strategies for each service's database. Test your backup and restore procedures regularly to ensure you can recover from failures.

### 6. Plan for Data Migration

When you extract a service from a monolith or split an existing service, you'll need to migrate data. Plan this migration carefully. Use the strangler fig pattern to gradually shift data ownership from the old system to the new service. Implement dual-write during the transition period, writing to both the old and new systems, then verify data consistency before removing the old writes.

Always maintain backward compatibility during migrations. Other services should be able to continue functioning while you migrate the data ownership from one service to another.

### 7. Monitor Database Metrics Per Service

Implement comprehensive monitoring for each service's database. Track query latency, connection pool usage, storage growth, and replication lag. Set up alerts for anomalies. This monitoring helps you identify performance issues before they become critical.

Create dashboards that show the health of each database independently. This visibility allows you to make informed decisions about scaling and capacity planning for each service.

### 8. Keep Service Databases Independent

Resist the temptation to create cross-service joins or shared reference tables. If you find that you frequently need to join data from two services, it might indicate that those services should be combined, or that you need to create a read model that aggregates data from multiple services.

Use asynchronous events to keep data in sync across services when needed. Don't try to maintain real-time consistency through synchronous database links.