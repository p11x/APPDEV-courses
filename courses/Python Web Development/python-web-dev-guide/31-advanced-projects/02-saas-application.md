# SaaS Application

## What You'll Learn
- Multi-tenant architecture
- Subscription billing
- Team management

## Prerequisites
- Completed full-stack e-commerce

## Multi-Tenancy

```python
# Tenant isolation
class TenantMixin:
    @declared_attr
    def tenant_id(cls):
        return Column(Integer, ForeignKey('tenants.id'))

class User(TenantMixin, Base):
    __tablename__ = "users"
    # Users are scoped to tenants
```

## Summary
- Design for multi-tenancy
- Implement subscription billing
- Build team features

## Next Steps
→ Continue to `03-real-time-chat.md`
