# Multi-Tenant Architecture

## 📌 What You'll Learn

- What multi-tenancy is and its benefits
- Different approaches: shared vs isolated databases
- Identifying tenants from requests
- Architecting Express for multi-tenant workloads

## 🧠 Concept Explained (Plain English)

**Multi-tenancy** means serving multiple customers (tenants) from a single application instance. This is how SaaS works — you have one app, but each customer sees only their own data.

**Benefits:**
- Cost savings (one infrastructure)
- Easier maintenance (one deployment)
- Scalability (shared resources)

**Challenges:**
- Data isolation (each tenant's data must be separate)
- Performance isolation (noisy neighbors)
- Customization per tenant

**Three main approaches:**

1. **Shared database, shared schema**
   - All tenants in same tables
   - Tenant ID column on every table
   - Most common, easiest

2. **Shared database, separate schemas**
   - Each tenant has own schema
   - PostgreSQL schemas, MySQL databases

3. **Separate databases**
   - Each tenant has own database
   - Maximum isolation
   - Highest cost

**Tenant identification:**
- Subdomain: `acme.example.com`
- Header: `X-Tenant-ID`
- Path: `example.com/acme/...`
- JWT token

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';

const app = express();


// ============================================
// Tenant Context
// ============================================

class TenantContext {
  constructor(tenantId) {
    this.tenantId = tenantId;
  }
  
  // Methods to scope queries
  getTablePrefix() {
    return `tenant_${this.tenantId}_`;
  }
  
  getSchema() {
    return this.tenantId;
  }
}


// ============================================
// Mock Database with Multi-Tenant Support
// ============================================

class MultiTenantDB {
  constructor() {
    // Map of tenantId -> their data store
    this.stores = new Map();
  }
  
  // Get or create store for tenant
  getStore(tenantId) {
    if (!this.stores.has(tenantId)) {
      this.stores.set(tenantId, {
        users: [],
        products: [],
        orders: []
      });
    }
    return this.stores.get(tenantId);
  }
  
  // Scoped queries
  findUsers(tenantId) {
    return this.getStore(tenantId).users;
  }
  
  findUser(tenantId, userId) {
    return this.getStore(tenantId).users.find(u => u.id === userId);
  }
  
  createUser(tenantId, userData) {
    const user = { id: 'user-' + Date.now(), ...userData };
    this.getStore(tenantId).users.push(user);
    return user;
  }
  
  findProducts(tenantId) {
    return this.getStore(tenantId).products;
  }
  
  findOrders(tenantId) {
    return this.getStore(tenantId).orders;
  }
  
  createOrder(tenantId, orderData) {
    const order = { id: 'order-' + Date.now(), ...orderData };
    this.getStore(tenantId).orders.push(order);
    return order;
  }
}

const db = new MultiTenantDB();


// Pre-populate some data
['tenant-a', 'tenant-b'].forEach(tenant => {
  db.createUser(tenant, { name: 'John Doe', email: 'john@example.com' });
  db.createUser(tenant, { name: 'Jane Smith', email: 'jane@example.com' });
  db.getStore(tenant).products = [
    { id: 'prod-1', name: 'Widget', price: 9.99 },
    { id: 'prod-2', name: 'Gadget', price: 19.99 }
  ];
});


// ============================================
// Tenant Identification Strategies
// ============================================

// Strategy 1: Subdomain
function getTenantFromSubdomain(req) {
  const host = req.headers.host || '';
  const subdomain = host.split('.')[0];
  
  // Ignore 'localhost' or 'www'
  if (subdomain === 'localhost' || subdomain === 'www') {
    return null;
  }
  
  return subdomain;
}

// Strategy 2: Custom header
function getTenantFromHeader(req) {
  return req.headers['x-tenant-id'];
}

// Strategy 3: JWT token
function getTenantFromJWT(req) {
  const auth = req.headers.authorization;
  if (!auth) return null;
  
  // In real code, decode JWT and extract tenant
  // const payload = jwt.decode(token.replace('Bearer ', ''));
  // return payload.tenantId;
  
  // Demo: check for mock tenant in token
  const token = auth.replace('Bearer ', '');
  if (token.startsWith('tenant-')) {
    return token.replace('tenant-', '');
  }
  
  return null;
}


// ============================================
// Tenant Resolution Middleware
// ============================================

app.use(async (req, res, next) => {
  // Try different strategies
  let tenantId = 
    getTenantFromSubdomain(req) ||
    getTenantFromHeader(req) ||
    getTenantFromJWT(req) ||
    req.query.tenantId; // Query param fallback
  
  if (!tenantId) {
    // Default tenant for demo
    tenantId = 'tenant-a';
  }
  
  // Attach tenant context to request
  req.tenant = new TenantContext(tenantId);
  req.tenantId = tenantId;
  
  // Also attach database with tenant methods
  req.db = {
    findUsers: () => db.findUsers(tenantId),
    findUser: (id) => db.findUser(tenantId, id),
    createUser: (data) => db.createUser(tenantId, data),
    findProducts: () => db.findProducts(tenantId),
    findOrders: () => db.findOrders(tenantId),
    createOrder: (data) => db.createOrder(tenantId, data)
  };
  
  console.log(`🏢 Tenant: ${tenantId}`);
  
  next();
});


// ============================================
// Routes (automatically scoped to tenant)
// ============================================

app.get('/health', (req, res) => {
  res.json({ 
    status: 'ok',
    tenant: req.tenantId
  });
});

app.get('/api/users', (req, res) => {
  const users = req.db.findUsers();
  res.json({ users, tenant: req.tenantId });
});

app.post('/api/users', (req, res) => {
  const { name, email } = req.body || {};
  
  if (!name || !email) {
    return res.status(400).json({ error: 'Name and email required' });
  }
  
  const user = req.db.createUser({ name, email });
  res.status(201).json({ user, tenant: req.tenantId });
});

app.get('/api/products', (req, res) => {
  const products = req.db.findProducts();
  res.json({ products, tenant: req.tenantId });
});

app.get('/api/orders', (req, res) => {
  const orders = req.db.findOrders();
  res.json({ orders, tenant: req.tenantId });
});

app.post('/api/orders', (req, res) => {
  const { items } = req.body || {};
  
  if (!items || !items.length) {
    return res.status(400).json({ error: 'Items required' });
  }
  
  const order = req.db.createOrder({ items, createdAt: new Date() });
  res.status(201).json({ order, tenant: req.tenantId });
});


// Tenant-specific customization
app.get('/api/config', (req, res) => {
  // Different config per tenant
  const configs = {
    'tenant-a': { theme: 'blue', features: ['beta'] },
    'tenant-b': { theme: 'green', features: [] }
  };
  
  res.json({
    config: configs[req.tenantId] || configs['tenant-a'],
    tenant: req.tenantId
  });
});


const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
  console.log('\n📝 Test multi-tenancy:');
  console.log(`  curl http://localhost:${PORT}/api/users`);
  console.log(`  curl -H "X-tenant-id: tenant-b" http://localhost:${PORT}/api/users`);
  console.log(`  curl -H "Authorization: Bearer tenant-c" http://localhost:${PORT}/api/users`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 10-19 | `TenantContext` class | Encapsulates tenant-specific operations |
| 27-66 | `MultiTenantDB` class | Database that scopes data by tenant |
| 73-81 | Subdomain extraction | Gets tenant from subdomain |
| 84-89 | Header extraction | Gets tenant from header |
| 92-107 | JWT extraction | Gets tenant from token |
| 112-134 | Tenant middleware | Resolves tenant and scopes request |
| 119-128 | Request-scoped db | Methods that automatically use tenant |
| 140-180 | Tenant routes | Automatically scoped to tenant |

## ⚠️ Common Mistakes

### 1. Tenant data leaking between customers

**What it is**: User sees another tenant's data.

**Why it happens**: Forgetting to scope queries by tenant ID.

**How to fix it**: Always include tenant ID in queries, use middleware.

### 2. Hardcoding tenant IDs

**What it is**: Code has specific tenant logic.

**Why it happens**: Not building generic tenant handling.

**How to fix it**: Use tenant context, avoid special cases.

### 3. No tenant validation

**What it is**: Invalid tenant IDs accepted.

**Why it happens**: Not validating tenant exists.

**How to fix it**: Validate tenant ID against allowlist or DB.

## ✅ Quick Recap

- Multi-tenancy serves multiple customers from one app
- Three approaches: shared schema, separate schemas, separate databases
- Identify tenants via subdomain, header, or JWT
- Always scope data operations by tenant
- Middleware sets up tenant context for each request

## 🔗 What's Next

Now learn about [Tenant Middleware](./02_tenant-middleware.md) for extracting and using tenant information.
