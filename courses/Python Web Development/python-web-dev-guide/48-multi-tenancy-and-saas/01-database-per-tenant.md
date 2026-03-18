# Multi-Tenancy Architecture

## What You'll Learn
- Multi-tenancy patterns
- Database per tenant
- Schema per tenant
- Shared database approaches

## Prerequisites
- Database knowledge

## Patterns Overview

### 1. Database per Tenant

Each tenant has their own database:

```
Tenant A DB          Tenant B DB          Tenant C DB
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│   Users     │     │   Users     │     │   Users     │
│   Orders    │     │   Orders    │     │   Orders    │
│   Products  │     │   Products  │     │   Products  │
└─────────────┘     └─────────────┘     └─────────────┘
```

### 2. Schema per Tenant

All tenants in one database, separate schemas:

```
┌─────────────────────────────────────────┐
│           Main Database                  │
├─────────────────────────────────────────┤
│ tenant_a schema: Users, Orders          │
│ tenant_b schema: Users, Orders          │
│ tenant_c schema: Users, Orders          │
└─────────────────────────────────────────┘
```

### 3. Shared Database with Tenant ID

All tenants in one table, identified by tenant_id:

```sql
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    tenant_id INT NOT NULL,
    name VARCHAR(100),
    email VARCHAR(100)
);

CREATE INDEX idx_users_tenant ON users(tenant_id);
```

## Implementation

```python
from fastapi import FastAPI, Request
from contextvars import ContextVar

# Store tenant ID in context
tenant_context: ContextVar[int | None] = ContextVar("tenant_id", default=None)

def get_tenant_id() -> int:
    """Get current tenant ID."""
    tid = tenant_context.get()
    if tid is None:
        raise ValueError("No tenant context")
    return tid

@app.middleware("http")
async def set_tenant_context(request: Request, call_next):
    """Extract tenant from subdomain or header."""
    # Try subdomain: tenant.example.com
    host = request.headers.get("host", "")
    subdomain = host.split(".")[0] if "localhost" not in host else None
    
    # Or from header
    tenant_id = request.headers.get("X-Tenant-ID", subdomain)
    
    token = tenant_context.set(int(tenant_id) if tenant_id else None)
    
    try:
        return await call_next(request)
    finally:
        tenant_context.reset(token)

@app.get("/users")
async def get_users():
    tenant_id = get_tenant_id()
    # Query with tenant_id filter
    users = await db.users.filter(tenant_id=tenant_id).all()
    return users
```

## Summary

- Multi-tenancy enables serving multiple customers
- Database per tenant: highest isolation
- Schema per tenant: good isolation, shared resources
- Shared database with tenant_id: most efficient, requires careful queries
