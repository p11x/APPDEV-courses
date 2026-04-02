# Azure Functions Triggers

## What You'll Learn

- Different trigger types and their use cases
- How to configure trigger bindings
- How to handle trigger-specific data
- How to implement trigger-based workflows

---

## Layer 1: Trigger Types

| Trigger | Use Case | Description |
|---------|----------|-------------|
| HTTP | API requests | RESTful endpoints |
| Timer | Scheduled tasks | CRON-based execution |
| Queue | Async processing | Service Bus/Storage queues |
| Blob | File processing | Storage blob events |
| Event Hub | Streaming | Real-time event processing |
| Cosmos DB | Database changes | Document changes |

---

## Layer 2: Implementation

### Event Hub Trigger

```typescript
import { AzureFunction, Context } from '@azure/functions';

const eventHubTrigger: AzureFunction = async function (
  context: Context,
  events: any[]
): Promise<void> {
  for (const event of events) {
    context.log('Event processed:', event.id, event.timestamp);
    
    await processEvent(event);
  }
};

async function processEvent(event: Event) {
  // Process the event
}

export default eventHubTrigger;
```

### Cosmos DB Trigger

```typescript
import { AzureFunction, Context } from '@azure/functions';

const cosmosDbTrigger: AzureFunction = async function (
  context: Context,
  documents: any[]
): Promise<void> {
  for (const doc of documents) {
    if (doc) {
      context.log('Document processed:', doc.id, doc._etag);
      await handleDocumentChange(doc);
    }
  }
};

export default cosmosDbTrigger;
```

---

## Next Steps

Continue to [Azure Functions Bindings](./04-azure-functions-bindings.md)