---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Functions
Purpose: Advanced Azure Functions including durable functions, bindings, and performance optimization
Difficulty: advanced
Prerequisites: 01_Basic_Azure_Functions.md
RelatedFiles: 01_Basic_Azure_Functions.md, 03_Practical_Azure_Functions.md
UseCase: Enterprise serverless architectures
CertificationExam: AZ-204 Azure Developer
LastUpdated: 2025
---

## 💡 WHY

Advanced Azure Functions concepts are essential for building enterprise-grade serverless applications. Durable functions provide stateful orchestration capabilities, while advanced bindings enable complex integration scenarios without writing boilerplate code.

## 📖 WHAT

### Durable Functions Overview

Durable Functions extend Azure Functions with stateful workflows. They handle long-running processes, support chaining, fan-out/fan-in patterns, and provide built-in checkpointing for reliability.

#### Orchestration Patterns

- **Chaining**: Sequential activity execution where output feeds into next input
- **Fan-out/Fan-in**: Parallel execution with result aggregation
- **Async HTTP**: Polling-based pattern for external operations
- **Monitor**: Recurring workflows with cleanup

#### Activity Functions

Activity functions are the unit of work in orchestrations. Each activity is atomic and can retry independently.

```python
import azure.durable_functions as df

def orchestrator_function(context: df.DurableOrchestrationContext):
    outputs = []
    
    # Chaining pattern
    result1 = yield context.call_activity("Activity1", input_data)
    outputs.append(result1)
    
    result2 = yield context.call_activity("Activity2", result1)
    outputs.append(result2)
    
    return outputs
```

### Input/Output Bindings

#### Blob Bindings

```python
import azure.functions as func

def process_blob_trigger(myblob: func.InputStream):
    # Read blob content
    content = myblob.read()
    
    return f"Processed {len(content)} bytes"
```

#### Queue Bindings

```python
def queue_processor(msg: func.QueueMessage):
    # Process message
    message_text = msg.get_body().decode('utf-8')
    
    return f"Processed: {message_text}"
```

#### Cosmos DB Bindings

```python
import azure.functions as func

def cosmos_query(req: func.HttpRequest, documents: func.DocumentList):
    # Documents parameter is automatically populated
    results = [doc.as_dict() for doc in documents]
    
    return func.HttpResponse(json.dumps(results), mimetype="application/json")
```

### Custom Handlers

Custom handlers enable running Azure Functions on platforms not natively supported. You define a JSON-based protocol over HTTP.

#### Handler Configuration

```json
{
    "version": "2.0",
    "extensions": [
        { "name": "CustomHandler", "type": "extensionBundle" }
    ],
    "customHandler": {
        "description": {
            "defaultExecutablePath": "handler.exe",
            "workingDirectory": "",
            "arguments": []
        },
        "enableForwardProxy": true
    }
}
```

#### Request Format

```json
{
    "invocationId": "unique-id",
    "functionName": "HttpTrigger1",
    "executionContext": {
        "requestId": "request-id",
        "correlationId": "correlation-id",
        "locale": "en-US"
    },
    "inputData": [
        {
            "type": "triggerhttp",
            "data": {
                "method": "GET",
                "url": "/api/HttpTrigger1?name=test"
            }
        }
    ]
}
```

### Performance Optimization

#### Memory Management

```python
import azure.functions as func

def optimized_function(req: func.HttpRequest):
    # Use streaming for large responses
    def stream_with_chunks():
        for chunk in large_dataset():
            yield chunk
    
    return func.HttpResponse(
        stream_with_chunks(),
        mimetype="application/octet-stream"
    )
```

#### Caching

```python
from functools import lru_cache

@lru_cache(maxsize=1)
def get_cached_client():
    return create_expensive_client()

def cached_function(req: req):
    client = get_cached_client()
    return func.HttpResponse(client.query())
```

## 🔧 HOW

### Example 1: Durable Function Orchestration

```bash
# Create durable function app
az functionapp create \
    --name durablefuncapp \
    --resource-group myrg \
    --storage-account mystorage \
    --consumption-plan-location eastus \
    --runtime python

# Configure application settings
az functionapp config appsettings set \
    --name durablefuncapp \
    --resource-group myrg \
    --settings \
        "AzureWebJobsStorage=$(az storage account show-connection-string \
            --name mystorage --query connectionString)" \
        "FUNCTIONS_EXTENSION_VERSION=~4"
```

### Example 2: Durable Event Hub Processing

```python
import azure.durable_functions as df
import azure.functions as func

def orchestrator(context: df.DurableOrchestrationContext):
    events = context.get_input()
    
    # Fan-out: process all events in parallel
    tasks = [
        context.call_activity("ProcessEvent", event) 
        for event in events
    ]
    results = yield context.task_all(tasks)
    
    # Aggregate results
    total = sum(results)
    yield context.call_activity("StoreTotal", total)
    
    return {"processed": len(events), "total": total}

def activity_process_event(event_data):
    # Process individual event
    return process_event(event_data)
```

### Example 3: Advanced Bindings Configuration

```bash
# Create storage account for bindings
az storage account create \
    --name mystorage \
    --resource-group myrg \
    --sku Standard_LRS

# Create Cosmos DB
az cosmosdb create \
    --name mycosmosdb \
    --resource-group myrg

# Configure system identity
az functionapp identity assign \
    --name myfuncapp \
    --resource-group myrg

# Grant role assignments
az role assignment create \
    --assignee $(az functionapp identity show \
        --name myfuncapp --resource-group myrg \
        --query principalId) \
    --role "Storage Blob Data Reader" \
    --scope /subscriptions/$SUB/resourceGroups/myrg/providers/Microsoft.Storage/storageAccounts/mystorage
```

### Example 4: Custom Handler in Go

```go
package main

import (
    "encoding/json"
    "fmt"
    "io"
    "log"
    "net/http"
)

type InvocationRequest struct {
    InvocationID  string `json:"invocationId"`
    FunctionName  string `json:"functionName"`
    InputData     []InputData `json:"inputData"`
}

type InputData struct {
    Type string `json:"type"`
    Data InputHttp `json:"data"`
}

type InputHttp struct {
    Method string `json:"method"`
    URL    string `json:"url"`
}

func main() {
    http.HandleFunc("/api/", func(w http.ResponseWriter, r *http.Request) {
        var req InvocationRequest
        body, _ := io.ReadAll(r.Body)
        json.Unmarshal(body, &req)
        
        response := map[string]interface{}{
            "outputs": []map[string]interface{}{
                {
                    "type":     "http",
                    "statusCode": 200,
                    "data":     map[string]string{"message": "Hello from Go"},
                },
            },
        }
        
        json.NewEncoder(w).Encode(response)
    })
    
    log.Fatal(http.ListenAndServe(":8080", nil))
}
```

## 📊 COMPARISON TABLE

| Feature | Azure Functions | AWS Lambda | GCP Cloud Functions |
|---------|-----------------|------------|---------------------|
| **Language Support** | C#, Java, Node.js, PowerShell, Python | Node.js, Python, Java, C#, Go, Ruby | Node.js, Python, Go, Java, .NET |
| **Durable Functions** | Native (Orchestrations) | Step Functions (separate) | Workflows (separate) |
| **Cold Start** | ~1-3 seconds | ~1-5 seconds | ~1-3 seconds |
| **Max Execution Time** | 60 minutes (Premium) | 15 minutes | 60 seconds (1st gen), 9 min (2nd gen) |
| **Memory** | 128MB - 14GB | 128MB - 10GB | 128MB - 8GB |
| **Bindings** | Extensive built-in | Limited, use layers | Limited |
| **Custom Runtime** | Custom handlers | Custom runtime | Custom runtime |
| **VPC Support** | Premium/App Plan | VPC endpoint | VPC connector |
| **Free Tier** | 1M requests + 400K GB-s | 1M requests + 400K GB-s | 2M invocations |
| **Pricing (Req)** | $0.20/1M | $0.20/1M | $0.40/1M |
| **Pricing (GB-s)** | $0.000016/GB-s | $0.00001667/GB-s | $0.0000125/GB-s |
| **Regional** | 60+ regions | 25+ regions | 40+ regions |
| **Max Deployment Size** | 48MB (zip) | 50MB (direct), 250MB (layers) | 100MB (compressed) |
| **Concurrency** | 200 (consumption), unlimited (premium) | 1000 parallel | 1000 concurrent |
| **Authentication** | Easy Auth, Managed Identity | IAM, Cognito | IAM, Firebase |
| **Stateful** | Durable Functions | External (DynamoDB) | External (Firestore) |

## 📋 AZURE FUNCTIONS CLI REFERENCE

### Core Commands

```bash
# Function app management
az functionapp list                                 # List all function apps
az functionapp show --name myapp --resource-group rg
az functionapp delete --name myapp --rg myrg
az functionapp start --name myapp --rg myrg
az functionapp stop --name myapp --rg myrg

# Deployment
az functionapp deployment source config-local-git \
    --name myapp --resource-group myrg
az functionapp deployment source sync --name myapp --rg myrg

# Configuration
az functionapp config show --name myapp --rg myrg
az functionapp config appsettings set --name myapp --rg myrg \
    --settings "KEY=value"

# Slots (staging)
az functionapp deployment slot create --name myapp --rg myrg --slot staging
az functionapp deployment slot swap --name myapp --rg myrg \
    --target-slot production

# Scaling
az functionapp plan show --name myplan --rg myrg
az functionapp plan update --name myplan --rg myrg \
    --min-instances 2 --max-instances 10
```

### Durable Functions CLI

```bash
# Task Hub management
az durable taskhub show --name mytaskhub --resource-group myrg

# Purge history
az durable function delete --name myapp --resource-group myrg \
    --task-hub-name mytaskhub
```

## ✅ EXAM TIPS

- Durable Functions use an orchestrator function to coordinate activities
- Use retry policies for reliability in long-running workflows
- Custom handlers require JSON-based HTTP protocol
- Premium plan provides VNET integration and unlimited execution
- Use managed identities for secure resource access
- Configure concurrency settings based on workload