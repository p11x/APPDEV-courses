---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Functions
Purpose: Practical hands-on labs for Azure Functions development
Difficulty: intermediate
Prerequisites: 01_Basic_Azure_Functions.md, 02_Advanced_Azure_Functions.md
RelatedFiles: 01_Basic_Azure_Functions.md, 02_Advanced_Azure_Functions.md
UseCase: Building serverless applications
CertificationExam: AZ-204 Azure Developer
LastUpdated: 2025
---

## 💡 WHY

Hands-on practice is essential for mastering Azure Functions. These practical labs demonstrate real-world patterns for HTTP triggers, timer triggers, queue triggers, and durable function workflows.

## 📖 WHAT

### Lab Overview

This module covers practical implementation of:
- HTTP trigger functions with routing
- Timer trigger functions for scheduled tasks
- Queue trigger functions for async processing
- Durable function workflows for orchestration
- Deployment and testing strategies

### HTTP Trigger Function

HTTP triggers expose REST endpoints. They support routing, authentication, and response formatting.

```python
import azure.functions as func
import logging
import json
import uuid

app = func.FunctionApp()

@app.function_name(name="HttpTrigger1")
@app.route(route="products/{id?}", methods=["GET", "POST"])
def test_function(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')
    
    method = req.method
    
    if method == "GET":
        product_id = req.route_params.get("id")
        
        if product_id:
            # Return single product
            return func.HttpResponse(
                json.dumps({"id": product_id, "name": "Sample Product"}),
                mimetype="application/json",
                status_code=200
            )
        else:
            # Return all products
            products = [
                {"id": "1", "name": "Product A"},
                {"id": "2", "name": "Product B"}
            ]
            return func.HttpResponse(
                json.dumps(products),
                mimetype="application/json",
                status_code=200
            )
    
    elif method == "POST":
        try:
            req_body = req.get_json()
        except ValueError:
            req_body = None
        
        if req_body:
            new_id = str(uuid.uuid4())
            return func.HttpResponse(
                json.dumps({"id": new_id, "status": "created"}),
                mimetype="application/json",
                status_code=201
            )
        else:
            return func.HttpResponse(
                "Invalid request body",
                status_code=400
            )
    
    return func.HttpResponse("Method not allowed", status_code=405)
```

### Timer Trigger Function

Timer triggers execute on a schedule using cron expressions.

```python
import azure.functions as func
import logging
from datetime import datetime

app = func.FunctionApp()

@app.function_name(name="TimerTrigger")
@app.schedule(schedule="0 */5 * * * *", run_on_startup=True)
def timer_function(req: func.TimerRequest) -> None:
    utc_timestamp = datetime.utcnow().isoformat()
    
    if req.past_due:
        logging.info("Timer is past due")
    
    logging.info(f"Timer triggered at {utc_timestamp}")
    
    # Perform scheduled task
    result = process_scheduled_task()
    logging.info(f"Task completed: {result}")

def process_scheduled_task():
    # Example: cleanup old records
    return {"processed": 10, "cleaned": 5}
```

### Queue Trigger Function

Queue triggers process messages asynchronously from Azure Queue Storage.

```python
import azure.functions as func
import logging
import json
import os

app = func.FunctionApp()

@app.function_name(name="QueueTrigger")
@app.queue_trigger(
    arg_name="msg", 
    queue_name="orders-queue", 
    connection="AzureWebJobsStorage"
)
def queue_function(msg: func.QueueMessage) -> None:
    logging.info(f"Python queue trigger function processed: {msg.get_body().decode()}")
    
    try:
        # Parse message body
        message_data = json.loads(msg.get_body().decode())
        
        order_id = message_data.get("orderId")
        customer_id = message_data.get("customerId")
        items = message_data.get("items", [])
        
        logging.info(f"Processing order {order_id} for customer {customer_id}")
        
        # Process order
        result = process_order(order_id, items)
        
        logging.info(f"Order {order_id} processed: {result}")
        
    except Exception as e:
        logging.error(f"Error processing message: {str(e)}")
        raise

def process_order(order_id, items):
    # Simulate order processing
    total = sum(item.get("price", 0) * item.get("quantity", 1) for item in items)
    return {
        "orderId": order_id,
        "status": "completed",
        "total": total
    }
```

### Durable Function Workflow

Durable functions orchestrate long-running workflows with state management.

```python
import azure.functions as func
import azure.durable_functions as df
import logging
import json

app = func.FunctionApp()

@app.function_name(name="OrchestratorStart")
@app.route_route("orchestrators/{orchestratorName}")
def orchestrator_start(req: func.HttpRequest) -> func.HttpResponse:
    client = df.DurableOrchestrationClient(req)
    
    instance_id = req.route_params.get("orchestratorName", "default")
    body = req.get_json()
    
    logging.info(f"Starting orchestration: {instance_id}")
    
    # Start the orchestrator
    instance_id = client.start_new(
        orchestrator_function_name="OrderProcessingOrchestrator",
        instance_id=instance_id,
        client_input=body
    )
    
    return client.create_check_status_response(req, instance_id)

@app.function_name(name="OrderProcessingOrchestrator")
def order_orchestrator(context: df.DurableOrchestrationContext) -> dict:
    logging.info("Starting order processing workflow")
    
    input_data = context.get_input()
    order_id = input_data.get("orderId", "unknown")
    
    # Step 1: Validate order
    validation_result = yield context.call_activity("ValidateOrder", input_data)
    
    if not validation_result.get("valid"):
        return {"status": "failed", "reason": "validation_failed"}
    
    # Step 2: Process payment (parallel for multiple items)
    payment_tasks = []
    for item in input_data.get("items", []):
        payment_tasks.append(
            context.call_activity("ProcessPayment", item)
        )
    
    payment_results = yield context.task_all(payment_tasks)
    
    # Step 3: Update inventory
    inventory_result = yield context.call_activity("UpdateInventory", {
        "orderId": order_id,
        "items": input_data.get("items", [])
    })
    
    # Step 4: Send notification
    notification_result = yield context.call_activity(
        "SendNotification",
        {"orderId": order_id, "status": "completed"}
    )
    
    return {
        "status": "completed",
        "orderId": order_id,
        "payments": payment_results,
        "inventory": inventory_result,
        "notification": notification_result
    }

@app.function_name(name="ValidateOrder")
def validate_order(activity_input: dict) -> dict:
    required_fields = ["orderId", "customerId", "items"]
    
    for field in required_fields:
        if field not in activity_input:
            return {"valid": False, "reason": f"Missing field: {field}"}
    
    if not activity_input.get("items"):
        return {"valid": False, "reason": "No items in order"}
    
    return {"valid": True}

@app.function_name(name="ProcessPayment")
def process_payment(item: dict) -> dict:
    return {
        "itemId": item.get("id"),
        "status": "paid",
        "amount": item.get("price", 0) * item.get("quantity", 1)
    }

@app.function_name(name="UpdateInventory")
def update_inventory(data: dict) -> dict:
    return {
        "orderId": data.get("orderId"),
        "status": "updated"
    }

@app.function_name(name="SendNotification")
def send_notification(data: dict) -> dict:
    return {
        "orderId": data.get("orderId"),
        "status": "notified"
    }
```

## 🔧 HOW

### Deploy Function App

```bash
# Prerequisites
az login
az account set --subscription MySubscription

# Create resource group
az group create --location eastus --name myrg

# Create storage account
az storage account create \
    --name mystorage123 \
    --resource-group myrg \
    --sku Standard_LRS

# Create function app
az functionapp create \
    --name myfuncapp \
    --resource-group myrg \
    --storage-account mystorage123 \
    --consumption-plan-location eastus \
    --runtime python \
    --runtime-version 3.11

# Configure deployment
az functionapp deployment source config-local-git \
    --name myfuncapp \
    --resource-group myrg

# Get deployment credentials
az functionapp deployment list-publishing-credentials \
    --name myfuncapp \
    --resource-group myrg

# Add remote
git remote add azure <deployment-url>

# Deploy
git push azure main:master
```

### Configure Bindings

```bash
# Storage connection
az functionapp config appsettings set \
    --name myfuncapp \
    --resource-group myrg \
    --settings "AzureWebJobsStorage=$(az storage account show-connection-string \
        --name mystorage123 --query connectionString)"

# Queue names
az functionapp config appsettings set \
    --name myfuncapp \
    --rg myrg \
    --settings "QueueName=orders-queue"
```

### Test Functions

```bash
# Test HTTP trigger
curl -X GET "https://myfuncapp.azurewebsites.net/api/products"

# Test with parameters
curl -X GET "https://myfuncapp.azurewebsites.net/api/products/123"

# Test POST
curl -X POST "https://myfuncapp.azurewebsites.net/api/products" \
    -H "Content-Type: application/json" \
    -d '{"name": "New Product", "price": 99.99}'

# Test queue trigger (add message to queue)
az storage message put \
    --queue-name orders-queue \
    --content '{"orderId": "123", "customerId": "cust1", "items": [{"id": "p1", "price": 10, "quantity": 2}]}' \
    --connection-string $(az storage account show-connection-string --name mystorage123 --query connectionString)

# Monitor logs
az functionapp logs tail --name myfuncapp --resource-group myrg
```

### Local Development

```bash
# Install Azure Functions Core Tools
npm install -g azure-functions-core-tools@4

# Create local.settings.json
cat > local.settings.json << 'EOF'
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "",
        "FUNCTIONS_WORKER_RUNTIME": "python"
    }
}
EOF

# Run locally
func start

# Test locally
curl -X GET http://localhost:7071/api/products
```

### CI/CD Deployment

```yaml
# azure-pipelines.yml
trigger:
  branches:
    include:
      - main

pr:
  branches:
    include:
      - main

pool:
  vmImage: ubuntu-latest

steps:
  - task: UsePythonVersion@0
    inputs:
      versionSpec: '3.11'
    
  - script: |
      pip install -r requirements.txt
    displayName: 'Install dependencies'
    
  - task: AzureFunctionApp@1
    inputs:
      azureSubscription: 'AzureServiceConnection'
      appType: 'functionApp'
      appName: 'myfuncapp'
      package: '$(System.DefaultWorkingDirectory)/'
```

## ✅ EXAM TIPS

- HTTP triggers support route parameters for dynamic endpoints
- Timer triggers use cron expressions: second minute hour day month day-of-week
- Queue triggers automatically deserialize JSON messages
- Durable functions return check-status response for polling
- Use local.settings.json for local development secrets
- Test with Azure Portal or Postman before deployment
- Monitor function execution with Application Insights