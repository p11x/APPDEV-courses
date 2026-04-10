---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Functions
Purpose: Understanding Azure Functions serverless compute
Difficulty: beginner
Prerequisites: 01_Basic_Azure_Core.md
RelatedFiles: 02_Advanced_Azure_Functions.md, 03_Practical_Azure_Functions.md
UseCase: Event-driven serverless applications
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

## 💡 WHY

Azure Functions provides serverless compute that runs code in response to events. Understanding serverless helps build event-driven architectures.

## 📖 WHAT

### Azure Functions Features

- **Triggers**: HTTP, Timer, Queue, Blob, Event Hub
- **Bindings**: Input/output connections
- **Consumption Plan**: Pay per execution
- **Premium Plan**: Dedicated resources
- **App Service Plan**: Always-on resources

### Consumption Plan Pricing

- Execution: $0.000016/GB-second
- Requests: $0.20/1M requests

## 🔧 HOW

### Example 1: Create Function App

```bash
# Create function app
az functionapp create \
    --name myfuncapp \
    --resource-group myrg \
    --storage-account mystorage \
    --consumption-plan-location eastus \
    --runtime python

# Deploy function
az functionapp deployment source config-local-git \
    --name myfuncapp \
    --resource-group myrg
```

## ✅ EXAM TIPS

- Serverless = pay only for execution time
- Triggers and bindings simplify integration
- Good for event-driven workloads