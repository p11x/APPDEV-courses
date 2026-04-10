# Microservices Pattern Questions

## Overview

This section covers common questions about microservices design patterns. These questions test your understanding of when and how to apply specific architectural patterns.

## Pattern Questions

### Q1: When would you use the Circuit Breaker pattern?

**Answer**: Use Circuit Breaker when calling remote services that may fail or become slow. It prevents cascading failures by:
- Tracking failures
- Opening circuit after threshold
- Failing fast when circuit is open
- Allowing recovery after timeout

### Q2: Explain the difference between choreography and orchestration in sagas

**Answer**:
- **Choreography**: Services communicate through events without central control. Each service reacts to events and publishes new ones.
- **Orchestration**: A central coordinator tells services what to do. The coordinator manages the transaction flow.

### Q3: When would you choose database per service vs shared database?

**Answer**: Choose database per service when:
- You need true service independence
- Teams need full control of their data
- You want loose coupling

Choose shared database when:
- Data consistency is critical
- You have strong cohesion between domains
- Migration constraints exist

## Output

```
Pattern Questions: 25
Topics Covered:
- Communication Patterns (8)
- Data Patterns (7)
- Resilience Patterns (6)
- Operational Patterns (4)

Difficulty Distribution:
- Basic: 8
- Intermediate: 12
- Advanced: 5
```
