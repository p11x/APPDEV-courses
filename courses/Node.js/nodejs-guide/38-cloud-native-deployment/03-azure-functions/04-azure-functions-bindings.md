# Azure Functions Bindings

## What You'll Learn

- How to use input and output bindings
- How to configure binding expressions
- How to implement custom bindings
- How to chain bindings for complex workflows

---

## Layer 1: Binding Types

### Input Bindings

```typescript
import { AzureFunction, Context } from '@azure/functions';

const httpTrigger: AzureFunction = async function (
  context: Context,
  req: HttpRequest,
  tableData: any[]
): Promise<void> {
  context.log('Table rows:', tableData.length);
  // Use tableData as input
};
```

### Output Bindings

```typescript
const httpTrigger: AzureFunction = async function (
  context: Context,
  req: HttpRequest
): Promise<void> {
  context.bindings.outputBlob = 'Hello, World!';
  context.bindings.outputQueue = JSON.stringify({ message: 'Sent' });
  context.res = { body: 'Success' };
};
```

---

## Layer 2: Common Bindings

| Binding | Direction | Description |
|---------|-----------|-------------|
| Blob | In/Out | Azure Storage blobs |
| Table | In/Out | Azure Table storage |
| Queue | Out | Storage queue |
| ServiceBus | Out | Service Bus |
| SendGrid | Out | Email |
| Twilio | Out | SMS |

---

## Next Steps

Continue to [Azure Functions Deployment](./05-azure-functions-deployment.md)