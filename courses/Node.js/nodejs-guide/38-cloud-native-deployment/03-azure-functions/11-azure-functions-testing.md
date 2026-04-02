# Azure Functions Testing

## What You'll Learn

- How to test Azure Functions locally
- How to implement unit tests
- How to test with mocked triggers
- How to set up CI/CD testing

---

## Layer 1: Testing Strategies

### Unit Testing

```typescript
import { describe, it, expect, jest } from '@jest/globals';
import { Context } from '@azure/functions';

describe('HTTP Trigger', () => {
  it('returns greeting with name', async () => {
    const context = {
      log: jest.fn(),
      bindingData: {},
      req: {
        query: { name: 'John' },
        body: null
      },
      res: {}
    } as unknown as Context;

    await httpTrigger(context, context.req);
    
    expect(context.res.status).toBe(200);
    expect(context.res.body).toContain('John');
  });
});
```

---

## Next Steps

Continue to [GCP Cloud Functions Setup](../03-gcp-cloud-functions/01-gcp-cloud-functions-setup.md)