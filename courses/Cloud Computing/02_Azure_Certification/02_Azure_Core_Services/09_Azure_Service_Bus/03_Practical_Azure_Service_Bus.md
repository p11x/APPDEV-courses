---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Service Bus
Purpose: Practical Azure Service Bus implementation
Difficulty: intermediate
Prerequisites: 01_Basic_Azure_Service_Bus.md, 02_Advanced_Azure_Service_Bus.md
RelatedFiles: 01_Basic_Azure_Service_Bus.md, 02_Advanced_Azure_Service_Bus.md
UseCase: Building messaging solutions
CertificationExam: AZ-104 Azure Administrator
LastUpdated: 2025
---

# Practical Azure Service Bus Implementation

## Introduction to Practical Scenarios

This practical guide covers implementing Azure Service Bus in real-world scenarios. Unlike conceptual explanations, practical implementation requires understanding tools, code patterns, and operational procedures. This content prepares administrators and developers for the AZ-104 Azure Administrator exam and production Service Bus deployments.

Practical scenarios include creating namespaces, queues, and topics; sending and receiving messages; configuring subscriptions; and operational procedures. Each scenario includes step-by-step procedures and code examples. Understanding these scenarios enables effective Service Bus management in production environments.

Prerequisites include Azure subscription access, Azure CLI or PowerShell with Service Bus extension, and appropriate permissions. Resource Manager permissions create namespaces. Service Bus Data Owner or equivalent roles manage entities. Understanding permissions ensures appropriate access for tasks.

## Creating a Service Bus Namespace

The namespace is the fundamental container for Service Bus resources. Creating a namespace requires selecting an appropriate tier, region, and naming convention. The following steps create an operational namespace using Azure CLI.

First, ensure the Service Bus extension is installed. Azure CLI versions before 2.37 may require extension installation. Run "az extension add --name servicebus" to add the extension. Verify installation with "az extension list" showing servicebus.

Create the namespace using az servicebus namespace create. The command requires resource group, namespace name, location, and SKU. The namespace name must be globally unique; select a unique name for your environment. SKU selection depends on feature requirements.

```bash
# Create resource group if not exists
az group create --location eastus --name servicebus-rg

# Create Service Bus namespace
az servicebus namespace create \
    --resource-group servicebus-rg \
    --name myapp-servicebus \
    --location eastus \
    --sku Standard
```

The namespace creation completes asynchronously. Use "az servicebus namespace show" to verify creation and status. The output includes provisioningState, location, and SKU information. The namespace becomes available after provisioning completes.

Configure namespace properties after creation. Set namespace properties including zone redundancy for premium tier, or tags for organization. Tags enable resource organization across environments or applications. Apply consistent tagging for operational visibility.

Obtain connection strings for application access. Authorization rules control access permissions. The default RootManageSharedAccessKey provides full access but use application-specific rules in production. Create rules with minimum necessary permissions.

```bash
# Get connection string
az servicebus namespace authorization-rule keys list \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus \
    --name RootManageSharedAccessKey
```

Store connection strings securely. Environment variables or Azure Key Vault provide secure storage. Never commit connection strings to source control. Access control should follow security best practices.

## Creating Queues

Queues provide point-to-point messaging between producers and consumers. Create queues using Azure CLI or the Azure Portal. The following creates a queue with configured properties for common scenarios.

Queue creation requires specifying queue name and namespace. Additional parameters configure queue behavior. Properties include maximum message size, default TTL, lock duration, and dead-letter options. Configure based on application requirements.

```bash
# Create queue with standard configuration
az servicebus queue create \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus \
    --name order-queue \
    --enable-dead-lettering-on-message-expiration true \
    --default-message-time-to-live P7D \
    --lock-duration PT1M \
    --max-delivery-count 10
```

Queue properties control behavior. EnableDeadLetterOnMessageExpiration moves expired messages to the dead-letter queue rather than deleting them. DefaultMessageTimeToLive sets how long messages persist when not explicitly set. LockDuration controls peek-lock duration during processing. MaxDeliveryCount determines automatic dead-letter attempts.

Verify queue creation with az servicebus queue show. The output includes createdAt, sizeInBytes, and messageCount. MessageCount shows approximate queued messages. SizeInBytes shows total storage used by messages.

Queue configuration update modifies queue properties. Update queue settings with az servicebus queue update. Configurable properties include default TTL, lock duration, and dead-letter options. Queue property updates affect new messages but do not change existing messages.

```bash
# Update queue properties
az servicebus queue update \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus \
    --name order-queue \
    --default-message-time-to-live P14D
```

## Creating Topics and Subscriptions

Topics provide publish-subscribe patterns. Creating topics requires Standard tier or higher. Topics organize messages by type, with subscriptions as individual logical endpoints. The following creates a topic and subscription for event distribution.

```bash
# Create topic
az servicebus topic create \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus \
    --events

# Create subscription
az servicebus subscription create \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus \
    --topic-name events \
    --subscription-name analytics
```

Subscription creation requires topic name and subscription name. Each subscription under a topic operates independently. Messages published to the topic go to each subscription. Subscriptions store messages independently.

Configure subscription filters for conditional delivery. Default filter delivers all messages. SQL filters add conditions for selective delivery. Correlation filters match specific properties. Filters enable routing without message body access.

```bash
# Create subscription with SQL filter
az servicebus subscription create \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus \
    --topic-name events \
    --subscription-name critical \
    --filter-sql-expression "priority >= 'critical'"
```

Multiple subscriptions under one topic enable scenario variety. One subscription processes all messages, another processes priority messages. This pattern distributes messages without modifying producer code.

Filter comparison demonstrates common patterns. SQL filter expressions evaluate message properties:
```sql
-- Priority based filtering
priority = 'critical'

-- Property range filtering  
amount > 1000

-- Combined conditions
status = 'new' AND region = 'eastus'
```

Subscription properties control behavior including dead-letter, TTL, and lock duration. Configure similarly to queues. Each subscription maintains independent message cursor, processing status, and dead-letter queue.

## Sending Messages

Application code sends messages using SDK libraries. Azure Service Bus SDK supports .NET, Java, JavaScript, and Python. The following examples demonstrate sending messages in C# and Python. Choose the SDK appropriate to your application.

Send operations require connection string and queue or topic name. Construct the client, create a message, and send. Application implementation includes error handling for reliable send operations.

C# send example uses Microsoft.Azure.ServiceBus library:

```csharp
using Microsoft.Azure.ServiceBus;

var connectionString = Environment.GetEnvironmentVariable("SERVICE_BUS_CONNECTION_STRING");
var queueName = "order-queue";

var client = new QueueClient(connectionString, queueName);

var message = new Message
{
    Body = Encoding.UTF8.GetBytes("Order data: 1, Widget, 10"),
    ContentType = "application/json",
    MessageId = Guid.NewGuid().ToString(),
    Properties = 
    {
        ["orderId"] = "1",
        ["product"] = "Widget",
        ["quantity"] = "10"
    }
};

await client.SendAsync(message);
await client.CloseAsync();
```

Python send example using azure-servicebus:

```python
from azure.servicebus import ServiceBusClient, Message
import os

connection_string = os.environ["SERVICE_BUS_CONNECTION_STRING"]
queue_name = "order-queue"

client = ServiceBusClient.from_connection_string(connection_string)
sender = client.get_queue_sender(queue_name)

message = Message(
    b"Order data: 1, Widget, 10",
    content_type="application/json",
    message_id="order-1",
    properties={
        "orderId": "1",
        "product": "Widget", 
        "quantity": 10
    }
)

sender.send_messages(message)
sender.close()
```

Message properties enable application-specific metadata. Properties support string, int, long, boolean, and GUID types. Properties appear in monitoring and enable filtering. Set properties useful for tracking and routing.

Batch sending improves throughput for high-volume scenarios. Multiple messages combined into fewer operations reduce network overhead. SDK batching automatically groups messages:

```csharp
var messages = new List<Message>();
for (int i = 0; i < batchSize; i++)
{
    messages.Add(CreateMessage(i));
}

await client.SendAsync(messages);
```

Topic publishing uses similar code with topic name and publisher. Messages publish to topics similarly to queues. Each subscription receives a copy. Configure publisher properties for same behavior.

Scheduled messages become available at specified times. Set ScheduledEnqueueTimeUtc for future delivery. Messages hold until scheduled time, when receiving applications can process them. Use scheduled messages for delayed processing or rate limiting.

## Receiving Messages

Receiving messages completes the messaging cycle. Receivers can use peek-lock for reliable processing or receive-and-delete for fire-and-forget patterns. Peek-lock enables retry on processing failure. Receive-and-delete provides lower latency when failure is acceptable.

C# receive example demonstrates peek-lock processing:

```csharp
using Microsoft.Azure.ServiceBus;

var connectionString = Environment.GetEnvironmentVariable("SERVICE_BUS_CONNECTION_STRING");
var queueName = "order-queue";

var client = new QueueClient(connectionString, queueName);

var options = new MessageHandlerOptions(ExceptionReceivedHandler)
{
    MaxConcurrentCalls = 1,
    AutoComplete = false
};

client.RegisterMessageHandler(async (message, token) =>
{
    var body = Encoding.UTF8.GetString(message.Body);
    Console.WriteLine($"Processing: {body}");
    
    // Process message
    await ProcessOrderAsync(message);
    
    // Complete message
    await client.CompleteAsync(message.SystemProperties.LockToken);
}, options);

static async Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
{
    Console.WriteLine($"Exception: {args.Exception}");
}
```

The RegisterMessageHandler approach provides automatic completion if processing succeeds. Manual CompleteAsync acknowledges processing. Abandon releases for other consumers if processing fails.

Python receive example:

```python
from azure.servicebus import ServiceBusClient
import os

connection_string = os.environ["SERVICE_BUS_CONNECTION_STRING"]
queue_name = "order-queue"

client = ServiceBusClient.from_connection_string(connection_string)
receiver = client.get_queue_receiver(queue_name)

with receiver:
    for msg in receiver:
        print(f"Processing: {str(msg)}")
        process_message(msg)
        receiver.complete_message(msg)
```

Dead-letter handling processes failed messages:

```csharp
var deadLetterClient = new QueueClient(connectionString, "$DeadLetterQueue");

var options = new MessageHandlerOptions(ExceptionReceivedHandler)
{
    MaxConcurrentCalls = 1,
    AutoComplete = false
};

deadLetterClient.RegisterMessageHandler(async (message, token) =>
{
    var body = Encoding.UTF8.GetString(message.Body);
    Console.WriteLine($"Dead letter: {body}");
    
    // Investigate and potentially resubmit
    await InvestigateAndResubmitAsync(message);
    
    await deadLetterClient.CompleteAsync(message.SystemProperties.LockToken);
}, options);
```

Topic subscription processing uses similar code with subscription clients. Receive from subscriptions identically to queues. Each subscription receives independent copies.

## Configuring Subscriptions

Subscription configuration controls delivery behavior beyond defaults. Configuration includes filter types, dead-letter settings, and forward configuration. Proper subscription configuration enables complex routing scenarios.

Default subscription settings deliver all messages. Custom filters select messages based on properties. SQL filters evaluate message properties; correlation filters match specific IDs. Filter combination enables sophisticated routing.

```bash
# Create subscription with correlation filter
az servicebus subscription create \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus \
    --topic-name events \
    --subscription-name orders \
    --filter-correlation-id "orders"
```

Subscription dead-letter configuration handles failed processing:

```bash
# Enable dead-letter on subscription
az servicebus subscription update \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus \
    --topic-name events \
    --subscription-name orders \
    --enable-dead-lettering true \
    --max-delivery-count 10
```

Subscription forward configuration sends messages to queues:

```bash
# Forward to queue
az servicebus subscription update \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus \
    --topic-name events \
    --subscription-name archived \
    --forward-to order-queue
```

Rule management creates filters controlling which messages reach subscriptions. Rules create filters and action for message routing. Rules enable dynamic filtering based on message content.

```bash
# Create SQL filter rule  
az servicebus rule create \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus \
    --topic-name events \
    --subscription-name analytics \
    --name high-priority \
    --filter-sql-expression "priority = 'high'"
```

## Operational Procedures

Production operations include monitoring, troubleshooting, and maintenance. Standard procedures ensure reliable operations. Understanding procedures enables effective Service Bus administration.

Queue monitoring uses Azure Monitor for metrics. Key metrics include message count, incoming messages, and failed operations. Set alerts for thresholds indicating problems. Monitor metrics correlate with application behavior.

```bash
# Get queue metrics
az monitor metrics list-definitions \
    --resource /subscriptions/xxx/resourceGroups/servicebus-rg/providers/Microsoft.ServiceBus/namespaces/myapp-servicebus/queues/order-queue
```

Azure Monitor query returns metrics definitions. Apply metrics to specific queue for monitoring. Create alert rules based on metrics to notify on issues.

Message exploration verifies message content:

```bash
# Peek at messages without locking
az servicebus queue peek \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus \
    --queue-name order-queue \
    --message-count 10
```

Peek shows messages without removing them. Use peek during troubleshooting to inspect messages. Message count indicates queued volume.

Entity listing shows active resources:

```bash
# List queues
az servicebus queue list \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus

# List topics  
az servicebus topic list \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus
```

Operational queries show resource state. Use queries for inventory and status verification.

## Security Configuration

Service Bus security involves network, authentication, and authorization configuration. Proper security reduces unauthorized access risk. Configure based on security requirements.

Network security uses virtual networks and private endpoints. Private endpoints provide private connectivity through Azure Private Link. Virtual network integration restricts traffic to specific networks.

```bash
# Create virtual network
az network vnet create \
    --resource-group servicebus-rg \
    --name servicebus-vnet \
    --address-prefix 10.0.0.0/16

# Create subnet
az network vnet subnet create \
    --resource-group servicebus-rg \
    --vnet-name servicebus-vnet \
    --name private-endpoint \
    --address-prefix 10.0.0.0/24

# Get Service Bus ID
servicebus_id=$(az servicebus show \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus \
    --query id -o tsv)

# Create private endpoint
az network private-endpoint create \
    --resource-group servicebus-rg \
    --name servicebus-pe \
    --connection-name servicebus-connection \
    --target-resource-id $servicebus_id \
    --subnet /subscriptions/xxx/resourceGroups/servicebus-rg/providers/Microsoft.Network/virtualNetworks/servicebus-vnet/subnets/private-endpoint
```

Private endpoint configuration adds network security. Access requires authorization from within the virtual network.

Role-based access control manages authorization using Azure AD roles. Service Bus roles include Owner, Sender, and Receiver. Custom roles provide granular permissions.

```bash
# Get Service Bus resource ID
sb_id=$(az servicebus show \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus \
    --query id -o tsv)

# Assign Reader role
az role assignment create \
    --assignee user@domain.com \
    --role "Service Bus Reader" \
    --scope $sb_id
```

Authorization assignment provides access control. Role assignments follow least privilege principles appropriate for operational requirements.

## Resource Management

Resource management includes scaling, cleanup, and lifecycle operations. Proper resource management controls costs and maintains performance. Standard procedures maintain operational efficiency.

Scaling adjusts resource capacity. Premium tier scaling adds messaging units. Standard tier uses shared resources without scaling options. Understand tier differences before deployment.

```bash
# Update Premium tier messaging units
az servicebus namespace update \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus \
    --sku Premium \
    --capacity 2
```

Capacity update modifies premium tier throughput. Scale based on performance requirements. Monitor to understand scaling needs.

Resource cleanup removes unnecessary resources. Delete queues, topics, and finally namespaces. Cleanup reduces costs and simplifies resource management.

```bash
# Delete queue
az servicebus queue delete \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus \
    --name order-queue

# Delete namespace
az servicebus namespace delete \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus
```

Deletion operations are irretrievable. Confirm before deletion. Consider exporting data before cleanup.

## Disaster Recovery

Disaster recovery preparation ensures business continuity. Premium tier Geo-disaster recovery provides automatic replication. Application design considers recovery scenarios.

Geo-disaster recovery configuration pairs namespaces:

```bash
# Get primary namespace ID
primary_id=$(az servicebus show \
    --resource-group servicebus-rg \
    --namespace myapp-primary \
    --query id -o tsv)

# Get secondary namespace ID
secondary_id=$(az servicebus show \
    --resource-group servicebus-rg \
    --namespace myapp-secondary \
    --query id -o tsv)

# Create Geo-DR pairing
az servicebus georecovery-alias set-primary \
    --resource-group servicebus-rg \
    --alias myapp-dr \
    --namespace-name myapp-primary \
    --primary-namespace $primary_id \
    --secondary-namespace $secondary_id
```

Geo-DR configuration for Premium tier. Paired namespaces replicate across regions. Failover uses alias which redirects to secondary.

Application failover handles connection updates. Connection strings may change on failover. Applications must handle connection changes gracefully. Implement retry logic with exponential backoff.

## Troubleshooting Common Issues

Common issues include access failures, message accumulation, and throttling. Understanding issues enables rapid problem resolution. Document symptoms and error messages for investigation.

Access failures indicate authorization problems. Verify connection string validity and permissions. Check shared access key permissions match requirements. Ensure firewall rules permit access.

```bash
# Test connection
az servicebus namespace authorization-rule keys list \
    --resource-group servicebus-rg \
    --namespace myapp-servicebus \
    --name myapp-sender
```

Test connection to verify permissions. Correct connection provides working authorization.

Message queue accumulation suggests producer-consumer imbalance. Check producer and consumer operation rates. Increase consumer count or fix consumer errors. Monitor queue depth.

Throttling indicates resource limit approaches. Premium tier limits are configurable; Standard tier has shared limits. Review operation rates and optimize batching or scale.

## Summary

Practical Service Bus implementation covers operational procedures for production. Creating namespaces, queues, and topics provides infrastructure. Sending and receiving messages completes messaging scenarios. Configuring subscriptions enables complex routing.

Operational procedures maintain production operations. Monitoring, security, and troubleshooting ensure reliable operations. Disaster recovery preparation maintains business continuity. Skills apply to AZ-104 exam and production work.

Continuing with advanced features deepens implementation. Further study examines session and Premium tier features. Hands-on practice reinforces understanding. The practical approach develops operational skills for Service Bus implementation.