# Microservices Antipatterns

## What You'll Learn

- Common microservices mistakes
- How to identify antipatterns
- How to avoid them
- Real-world examples

## Antipatterns

### 1. Distributed Monolith

```
WRONG: Services that must all be deployed together
Service A → Service B → Service C (tightly coupled)
```

### 2. Shared Database

```
WRONG: Multiple services sharing one database
```

### 3. Chatty Services

```
WRONG: Too many inter-service calls for one request
Client → Service A → Service B → Service C → Service D
```

### 4. God Service

```
WRONG: One service that does everything
```

### 5. Wrong Service Boundaries

```
WRONG: Splitting by technical layer (data service, logic service)
CORRECT: Splitting by business domain (user service, order service)
```

## Next Steps

For refactoring, continue to [Refactoring to Microservices](./07-refactoring-to-microservices.md).
