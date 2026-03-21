# Tenant Middleware

## 📌 What You'll Learn

- Extracting tenant information from various sources
- Attaching tenant context to Express requests
- Scoping database queries by tenant
- Handling tenant-specific configurations

## 🧠 Concept Explained (Plain English)

Tenant middleware sits between the raw HTTP request and your route handlers, extracting tenant information and making it available throughout the request lifecycle. This is the foundation of multi-tenant Express applications.

**Key responsibilities:**
1. **Identify**: Extract tenant ID from request
2. **Validate**: Ensure tenant exists and is authorized
3. **Contextualize**: Make tenant available via `req.tenant`
4. **Scope**: Attach tenant-scoped services

The middleware should run early in the middleware chain, before any business logic that needs tenant context.

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';

const app = express();


// Mock tenant database
const tenants = new Map([
  ['acme', { id: 'acme', name: 'Acme Corp', plan: 'enterprise', features: ['sso', 'api'] }],
  ['globex', { id: 'globex', name: 'Globex Inc', plan: 'business', features: ['api'] }],
  ['starter', { id: 'starter', name: 'Starter Co', plan: 'free', features: [] }]
]);

// Mock user database per tenant
const tenantData = new Map();
['acme', 'globex', 'starter'].forEach(t => {
  tenantData.set(t, {
    users: [
      { id: '1', name: 'John' },
      { id: '2', name: 'Jane' }
    ],
    products: [
      { id: 'p1', name: 'Widget', price: 10 },
      { id: 'p2', name: 'Gadget', price: 20 }
    ]
  });
});


// ============================================
// Tenant Resolution
// ============================================

function resolveTenant(req) {
  // 1. Check subdomain (acme.example.com)
  const host = req.headers.host || '';
  const subdomain = host.split('.')[0];
  if (subdomain !== 'localhost' && subdomain !== 'www' && tenants.has(subdomain)) {
    return subdomain;
  }
  
  // 2. Check header
  const headerTenant = req.headers['x-tenant-id'];
  if (headerTenant && tenants.has(headerTenant)) {
    return headerTenant;
  }
  
  // 3. Check JWT
  const auth = req.headers.authorization;
  if (auth) {
    const token = auth.replace('Bearer ', '');
    const parts = token.split('.');
    if (parts.length === 3) {
      try {
        const payload = JSON.parse(Buffer.from(parts[1], 'base64').toString());
        if (payload.tenantId && tenants.has(payload.tenantId)) {
          return payload.tenantId;
        }
      } catch (e) {}
    }
  }
  
  // 4. Check query param (development)
  if (req.query.tenant) {
    return req.query.tenant;
  }
  
  return null;
}


// ============================================
// Main Tenant Middleware
// ============================================

app.use((req, res, next) => {
  const tenantId = resolveTenant(req);
  
  if (!tenantId) {
    // Allow health checks without tenant
    if (req.path === '/health') {
      return next();
    }
    return res.status(400).json({ error: 'Tenant not identified' });
  }
  
  const tenant = tenants.get(tenantId);
  
  // Attach tenant to request
  req.tenant = {
    id: tenant.id,
    name: tenant.name,
    plan: tenant.plan,
    features: tenant.features
  };
  
  // Scoped database
  req.tenantDb = tenantData.get(tenantId);
  
  // Tenant-specific config
  req.tenantConfig = {
    maxUsers: tenant.plan === 'free' ? 5 : -1,
    apiEnabled: tenant.features.includes('api'),
    ssoEnabled: tenant.features.includes('sso')
  };
  
  next();
});


// ============================================
// Routes
// ============================================

app.get('/health', (req, res) => {
  res.json({ status: 'ok' });
});

app.get('/api/tenant', (req, res) => {
  res.json(req.tenant);
});

app.get('/api/users', (req, res) => {
  res.json(req.tenantDb.users);
});

app.get('/api/products', (req, res) => {
  res.json(req.tenantDb.products);
});

// Feature-gated endpoint
app.get('/api/advanced', (req, res) => {
  if (!req.tenantConfig.apiEnabled) {
    return res.status(403).json({ error: 'API not included in your plan' });
  }
  res.json({ data: 'Advanced features!' });
});


const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 25-55 | Tenant database | Mock tenant configs |
| 60-85 | resolveTenant | Extracts tenant from multiple sources |
| 90-118 | Tenant middleware | Sets up request context |
| 121-140 | Routes | Uses tenant context |

## ✅ Quick Recap

- Extract tenant from subdomain, header, or JWT
- Validate tenant exists
- Attach tenant to `req.tenant`
- Scope database access to tenant
- Apply tenant-specific configuration

## 🔗 What's Next

Now learn about [Feature Flags](./03_feature-flags.md).
