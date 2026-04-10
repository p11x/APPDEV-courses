---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure App Service
Purpose: Understanding Azure App Service for web applications
Difficulty: beginner
Prerequisites: 01_Basic_Azure_Core.md
RelatedFiles: 02_Advanced_Azure_App_Service.md, 03_Practical_Azure_App_Service.md
UseCase: Web application hosting
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

# Azure App Service - Basic

Azure App Service is a fully managed platform-as-a-service (PaaS) offering from Microsoft Azure that enables developers to build and host web applications without managing underlying infrastructure. This service supports multiple programming languages and frameworks, making it an ideal choice for developers seeking a streamlined deployment experience.

## 🟢 What is Azure App Service?

Azure App Service is an HTTP-based service for hosting web applications, REST APIs, and mobile backends. It provides enterprise-grade security, compliance, and DevOps capabilities out of the box. The platform handles infrastructure management, operating system updates, and runtime patching, allowing developers to focus entirely on their application code.

The service integrates seamlessly with other Azure services and supports automatic scaling based on demand. Whether you're deploying a simple personal blog or a complex enterprise application, Azure App Service offers the flexibility and reliability needed for production workloads.

### Key Characteristics

- **Fully Managed Platform**: No need to manage servers, operating systems, or runtime environments
- **Multiple Language Support**: .NET, .NET Core, Java, Node.js, PHP, Python, Ruby, and Go
- **Automatic Scaling**: Scale out manually or automatically based on CPU usage or HTTP requests
- **Deployment Options**: GitHub, Azure DevOps, Bitbucket, or external Git repositories
- **Integrated Security**: HTTPS by default, Azure Active Directory integration, and compliance certifications

## 🟢 App Service Plans and Pricing Tiers

Azure App Service plans define the underlying compute resources and features available to your applications. Understanding the different tiers is crucial for cost optimization and performance planning.

### Pricing Tiers Overview

| Tier | Description | Use Case |
|------|-------------|----------|
| Free | Limited compute, shared infrastructure | Development, testing |
| Shared | Custom domains, TLS certificates | Small projects, prototypes |
| Basic | Dedicated VMs, manual scaling | Production workloads, small to medium apps |
| Standard | Auto-scaling, deployment slots | Production apps with scaling needs |
| Premium | Advanced performance, larger instance sizes | High-traffic applications |
| Isolated | Dedicated infrastructure, network isolation | Enterprise, compliance-heavy workloads |

### K Tier (Development/Testing)

The K tier is included in the Free and Shared pricing tiers. This tier provides basic functionality suitable for development and testing purposes:

- **Free Tier**: Host applications on shared Azure infrastructure with limited execution time (60 minutes per day). Ideal for learning and experimenting with Azure App Service features without incurring costs.

- **Shared Tier**: Provides custom domain support and basic TLS certificates. Applications run on shared compute resources but receive dedicated DNS names.

### P Tier (Production)

The P tier (Basic, Standard, Premium) represents production-ready compute options with dedicated resources:

- **Basic Tier (B1, B2, B3)**: Dedicated virtual machines with configurable CPU and memory. Supports manual scaling and deployment slots. Suitable for production workloads that don't requireauto-scaling.

- **Standard Tier (S1, S2, S3)**: Adds automatic scaling (up to 10 instances), traffic routing between deployment slots, and higher storage quotas. Recommended for most production web applications.

- **Premium Tier (P1v2, P2v2, P3v2, P1v3, P2v3, P3v3)**: Provides the best performance with larger VMs, more deployment slots, and advanced scaling capabilities. Ideal for high-traffic applications requiring maximum performance.

### Scaling Considerations

When selecting a tier, consider the following factors:

1. **Expected Traffic**: Peak and average concurrent users
2. **Scaling Requirements**: Manual versus automatic scaling
3. **Performance Needs**: CPU, memory, and storage requirements
4. **Budget Constraints**: Monthly cost tolerance
5. **Feature Requirements**: Deployment slots, custom domains, TLS versions

## 🟢 App Service Types

Azure App Service encompasses several specialized app types, each optimized for specific workload patterns.

### Web Apps

Web Apps are the most common App Service type, designed for hosting HTTP-based web applications and websites. They support all popular web frameworks and provide:

- **Automatic Web Server Configuration**: Internet Information Services (IIS) for Windows-based apps, Nginx for Linux-based apps
- **Web Framework Support**: ASP.NET, ASP.NET Core, Node.js, PHP, Python, Ruby on Rails, Java Servlet containers
- **Built-in Monitoring**: Application Insights integration for performance monitoring
- **SSL/TLS Support**: Free App Service-managed certificates or bring your own

Example creating a Web App:

```bash
# Create a resource group
az group create --name myResourceGroup --location eastus

# Create an App Service plan
az appservice plan create --name myAppServicePlan --resource-group myResourceGroup --sku S1

# Create a Web App
az webapp create --name myWebApp --resource-group myResourceGroup --plan myAppServicePlan
```

### API Apps

API Apps provide specialized features for hosting RESTful APIs:

- **OpenAPI Support**: Built-in Swagger UI for API documentation
- **CORS Configuration**: Simplified Cross-Origin Resource Sharing setup
- **Authentication**: Easy integration with Azure Active Directory and other providers
- **API Management Integration**: Seamless connection to Azure API Management

API Apps include all Web Apps features plus:

- Swagger/OpenAPI endpoint generation
- Automatic schema validation
- API version management
- Response caching configuration

### Mobile Apps

Mobile Apps provide backend services for mobile applications:

- **Offline Sync**: Data synchronization when connectivity is available
- **Push Notifications**: Integration with Azure Notification Hubs
- **User Authentication**: Social and enterprise identity providers
- **Dynamic Schema**: Automatic table creation based on data models

These features enable rapid development of mobile backends without managing server infrastructure.

## 🟢 Deployment Slots

Deployment slots enable you to deploy your application to a staging environment and then swap it with the production slot with zero downtime. This functionality is available in Standard, Premium, and Isolated tiers.

### Benefits of Deployment Slots

1. **Zero-Downtime Deployment**: Validate changes in staging before production exposure
2. **Easy Rollback**: Swap back to previous version if issues arise
3. **Traffic Testing**: Route percentage of traffic to staging for A/B testing
4. **Production-Like Testing**: Staging environment matches production configuration

### Working with Deployment Slots

```bash
# Create a deployment slot (requires Standard tier or higher)
az webapp deployment slot create --name myWebApp --resource-group myResourceGroup --slot staging

# Deploy to staging slot
az webapp deployment slot publish --name myWebApp --resource-group myResourceGroup --slot staging

# Swap staging to production
az webapp deployment slot swap --name myWebApp --resource-group myResourceGroup --slot staging --target-slot production
```

### Slot Configuration

Each slot maintains its own:

- Application settings
- Connection strings
- Environment variables
- Handler mappings

Settings can be "sticky" (remain with the slot) or "auto-swap" (apply to target slot after swap).

## 🟢 Basic Azure CLI Commands

The Azure CLI provides comprehensive commands for managing App Service resources. Below are essential commands for everyday operations.

### Creating Resources

```bash
# Create a resource group
az group create --name myResourceGroup --location eastus

# Create an App Service plan
az appservice plan create \
    --name myAppServicePlan \
    --resource-group myResourceGroup \
    --sku S1 \
    --is-linux

# Create a Web App
az webapp create \
    --name myWebApp \
    --resource-group myResourceGroup \
    --plan myAppServicePlan \
    --runtime "NODE:18-lts"

# Create with custom startup command
az webapp create \
    --name myCustomWebApp \
    --resource-group myResourceGroup \
    --plan myAppServicePlan \
    --runtime "NODE:18-lts" \
    --startup-file "npm start"
```

### Viewing and Managing Apps

```bash
# List all web apps in resource group
az webapp list --resource-group myResourceGroup

# Show web app details
az webapp show --name myWebApp --resource-group myResourceGroup

# View app configuration
az webapp config show --name myWebApp --resource-group myResourceGroup

# View application settings
az webapp config/appsettings list --name myWebApp --resource-group myResourceGroup
```

### Configuration Management

```bash
# Update app settings
az webapp config appsettings set \
    --name myWebApp \
    --resource-group myResourceGroup \
    --settings WEBSITE_NODE_DEFAULT_VERSION="~18"

# Set connection string
az webapp config connection-string set \
    --name myWebApp \
    --resource-group myResourceGroup \
    --connection-string-type SQLAzure \
    --name "DefaultConnection" \
    --value "Server=tcp:myserver.database.windows.net,1433;Database=mydb;User ID=myuser;Password=mypassword;Trusted_Connection=False;Encrypt=True;"

# Configure general settings
az webapp config set \
    --name myWebApp \
    --resource-group myResourceGroup \
    --min-tls-version "1.2" \
    --http20-enabled true
```

### Deployment Commands

```bash
# Deploy from local Git repository
az webapp deployment source config-local-git \
    --name myWebApp \
    --resource-group myResourceGroup

# Get deployment credentials
az webapp deployment list-publishing-credentials \
    --name myWebApp \
    --resource-group myResourceGroup

# Deploy using ZIP file
az webapp deployment source sync \
    --name myWebApp \
    --resource-group myResourceGroup

# Deploy from GitHub (requires authorization)
az webapp deployment source configure \
    --name myWebApp \
    --resource-group myResourceGroup \
    --repo-url "https://github.com/myorg/myapp" \
    --branch main \
    --git-token $GITHUB_TOKEN
```

### Logging and Diagnostics

```bash
# View application logs
az webapp log tail --name myWebApp --resource-group myResourceGroup

# Download logs
az webapp log download --name myWebApp --resource-group myResourceGroup

# Configure log streaming
az webapp deployment container config \
    --name myWebApp \
    --resource-group myResourceGroup \
    --enable-cd true
```

### Scaling Operations

```bash
# Scale out to specified instance count
az appservice plan update \
    --name myAppServicePlan \
    --resource-group myResourceGroup \
    --sku S1 \
    --number-of-workers 2

# Scale up to different tier
az appservice plan update \
    --name myAppServicePlan \
    --resource-group myResourceGroup \
    --sku S2
```

## 🟢 Authentication and Authorization

Azure App Service provides built-in authentication that simplifies implementing sign-in for your applications without writing authentication code.

### EasyAuth (Built-in Authentication)

The App Service Authentication (EasyAuth) feature:

- Authenticates users with Azure Active Directory, Facebook, Google, Microsoft Account, or Twitter
- Validates and refreshes tokens automatically
- Manages session cookies
- Works with custom identity providers via OpenID Connect

```bash
# Enable-built-in authentication
az webapp auth update \
    --name myWebApp \
    --resource-group myResourceGroup \
    --enabled true \
    --action LoginWithAzureActiveDirectory \
    --aad-client-id my-client-id \
    --aad-client-secret my-client-secret \
    --aad-tenant-id my-tenant-id
```

### Configured Identities

For accessing Azure resources:

```bash
# Assign system-managed identity
az webapp identity assign \
    --name myWebApp \
    --resource-group myResourceGroup

# Assign user-managed identity
az user MSI assign \
    --name myWebApp \
    --resource-group myResourceGroup \
    --identities "/subscriptions/.../user-mi-name"
```

## 🟢 Networking Features

Azure App Service provides various networking options for securing and controlling network traffic.

### VNet Integration

- **Regional VNet Integration**: Connect to Azure virtual networks in the same region
- **Gateway-Required VNet Integration**: Connect to remote networks via VPN gateway

```bash
# Enable VNet integration
az webapp vnet-integration add \
    --name myWebApp \
    --resource-group myResourceGroup \
    --vnet myVnetName \
    --subnet mySubnetName
```

### Access Restrictions

```bash
# Add IP restriction
az webapp config access-restriction add \
    --name myWebApp \
    --resource-group myResourceGroup \
    --rule-name "AllowCorporate" \
    --ip-address "203.0.113.0/255.255.255.0" \
    --action Allow
```

## 🟢 Monitoring and Diagnostics

Application Insights provides comprehensive monitoring for App Service applications.

### Integration

```bash
# Add Application Insights
az monitor app-insights component create \
    --app myAppInsights \
    --location eastus \
    --resource-group myResourceGroup

# Configure App Service to use Application Insights
az webapp config appsettings set \
    --name myWebApp \
    --resource-group myResourceGroup \
    --settings APPINSIGHTS_INSTRUMENTATIONKEY=my-instrumentation-key
```

### Log Streaming

Real-time log streaming for troubleshooting:

```bash
# Stream logs locally
az webapp log tail --name myWebApp --resource-group myResourceGroup

# Filter by log level
az webapp log tail --name myWebApp --resource-group myResourceGroup --provider application
```

## 🟢 Summary

Azure App Service provides a robust, scalablePlatform-as-a-Service for hosting web applications, APIs, and mobile backends. Key takeaways include:

1. **Flexible Pricing**: From free development tiers to enterprise-grade isolated environments
2. **Multiple App Types**: Web Apps, API Apps, and Mobile Apps each optimized for specific use cases
3. **Zero-Downtime Deployments**: Deployment slots enable safe staging and production deployments
4. **Integrated Security**: Built-in authentication, TLS, and compliance features
5. **Developer Productivity**: Comprehensive CLI, GitHub integration, and application insights

For beginners, start with the Free tier to explore features, then upgrade to Basic or Standard when moving to production. The Standard tier provides the best balance of features and cost for most production workloads.

In the next module, we'll explore advanced topics including auto-scaling configuration, custom domains with TLS certificates, and CI/CD pipeline integration.