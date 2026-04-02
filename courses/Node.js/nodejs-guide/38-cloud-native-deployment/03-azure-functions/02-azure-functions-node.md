# Azure Functions with Node.js

## What You'll Learn

- How to create Azure Functions with Node.js
- How to implement various trigger types
- How to configure input/output bindings
- How to handle context and logging

---

## Layer 1: Function Types

### HTTP Trigger

```typescript
import { AzureFunction, Context, HttpRequest, HttpResponse } from '@azure/functions';

const httpTrigger: AzureFunction = async function (
  context: Context,
  req: HttpRequest
): Promise<HttpResponse> {
  context.log('HTTP trigger processed', req.method, req.url);
  
  const name = req.query.name || (req.body && req.body.name) || 'World';
  
  context.res = {
    status: 200,
    body: {
      message: `Hello, ${name}!`,
      timestamp: new Date().toISOString()
    }
  };
};

export default httpTrigger;
```

### Timer Trigger

```typescript
import { AzureFunction, Context } from '@azure/functions';

const timerTrigger: AzureFunction = async function (
  context: Context,
  myTimer: any
): Promise<void> {
  const timestamp = new Date().toISOString();
  context.log('Timer triggered at', timestamp);
  
  if (myTimer.isPastDue) {
    context.log('Timer is running late!');
  }
  
  await performScheduledTask();
};

async function performScheduledTask() {
  context.log('Executing scheduled task...');
}

export default timerTrigger;
```

### Queue Trigger

```typescript
import { AzureFunction, Context } from '@azure/functions';

const queueTrigger: AzureFunction = async function (
  context: Context,
  myQueueItem: string
): Promise<void> {
  context.log('Queue trigger processed', myQueueItem);
  
  const item = JSON.parse(myQueueItem);
  await processQueueItem(item);
};

async function processQueueItem(item: QueueItem) {
  context.log('Processing item', item.id);
}

export default queueTrigger;
```

---

## Layer 2: Configuration

### function.json

```json
{
  "bindings": [
    {
      "name": "req",
      "type": "httpTrigger",
      "direction": "in",
      "authLevel": "function",
      "methods": ["get", "post"],
      "route": "users/{id?}"
    },
    {
      "name": "$return",
      "type": "http",
      "direction": "out"
    }
  ],
  "scriptFile": "../dist/users/index.js"
}
```

---

## Next Steps

Continue to [Azure Functions Triggers](./03-azure-functions-triggers.md)