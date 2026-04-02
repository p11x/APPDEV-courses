# Testing Pyramid

## What You'll Learn

- What the testing pyramid is
- How to balance test types
- How to choose test coverage targets
- How to organize tests

## The Pyramid

```
        /\
       /E2E\        ← Few, slow, expensive
      /──────\
     /Integra-\     ← Medium number
    /  tion    \
   /────────────\
  /    Unit      \  ← Many, fast, cheap
 /________________\
```

| Type | Count | Speed | Purpose |
|------|-------|-------|---------|
| Unit | 70% | <10ms | Individual functions |
| Integration | 20% | <500ms | Component interactions |
| E2E | 10% | <30s | Full user flows |

## Coverage Targets

| Metric | Target |
|--------|--------|
| Line coverage | 80%+ |
| Branch coverage | 70%+ |
| Critical paths | 100% |

## Next Steps

For unit testing, continue to [Unit Testing](./02-unit-testing.md).
