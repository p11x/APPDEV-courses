# Refactoring to Microservices

## What You'll Learn

- How to identify microservice boundaries
- How to extract services from a monolith
- How to handle shared databases
- How to plan the migration

## Strangler Fig Pattern

```
Phase 1: Monolith handles everything
Phase 2: Extract one feature to microservice
Phase 3: Route traffic to both monolith and new service
Phase 4: Extract more features
Phase 5: Monolith handles nothing — decommission
```

## Identifying Boundaries

```
Domain-Driven Design (DDD) bounded contexts:

User Management → user-service
Order Processing → order-service
Payment → payment-service
Notifications → notification-service
Product Catalog → catalog-service
```

## Extraction Steps

1. Identify bounded context
2. Create new service with its own database
3. Implement API matching current monolith interface
4. Route traffic to new service
5. Remove code from monolith
6. Repeat

## Shared Database Anti-Pattern

```
WRONG:
  Service A → shared_db ← Service B

CORRECT:
  Service A → db_a
  Service B → db_b
  Service A ← events → Service B
```

## Next Steps

This concludes Chapter 37. Return to the [guide index](../../index.html).
