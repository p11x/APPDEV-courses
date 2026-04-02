# Contract Testing Best Practices

## What You'll Learn

- When to use contract testing
- How to manage contracts
- How to integrate with CI/CD
- How to handle contract evolution

## When to Use Contract Testing

```
Is the API consumed by multiple clients?
├── Yes → Contract testing is valuable
│   ├── Internal microservices → Consumer-driven contracts (Pact)
│   └── Public API → OpenAPI spec + validation
└── No → Integration tests may suffice
```

## Contract Management

```yaml
# Use a Pact Broker for contract management
# Publish contracts from consumer tests
# Verify contracts in provider CI
# Break the build if contract is violated
```

## CI/CD Integration

```yaml
# .github/workflows/contracts.yml

jobs:
  consumer:
    steps:
      - run: npm run test:contract:consumer
      - run: npx pact-broker publish ./pacts --broker-url=$PACT_BROKER_URL

  provider:
    needs: consumer
    steps:
      - run: npm run test:contract:provider
```

## Best Practices

- Test one interaction per test
- Use provider states to set up test data
- Version contracts with your API
- Run consumer tests before provider tests
- Use can-i-deploy to verify compatibility before releasing

## Next Steps

This concludes Chapter 36. Return to the [guide index](../../index.html).
