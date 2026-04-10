---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure App Service
Purpose: Hands-on deployment and configuration of web applications
Difficulty: intermediate
Prerequisites: 01_Basic_Azure_App_Service.md, 02_Advanced_Azure_App_Service.md
RelatedFiles: 01_Basic_Azure_App_Service.md, 02_Advanced_Azure_App_Service.md
UseCase: Deploying web applications
CertificationExam: AZ-104 Azure Administrator
LastUpdated: 2025
---

# Azure App Service - Practical

This module provides hands-on exercises for deploying and managing web applications on Azure App Service. You'll work through real-world scenarios deploying .NET and Node.js applications with deployment slots, auto-scaling, and custom domain configuration.

## 🟡 Deploying a .NET Application

This exercise walks through deploying an ASP.NET Core application to Azure App Service.

### Prerequisites

- Azure subscription with contributor access
- .NET 8.0 SDK installed
- Azure CLI installed
- Git installed

### Step 1: Create the Application

```bash
# Create new ASP.NET Core Web API
dotnet new webapi -n AzureDemoApi -o AzureDemoApi

# Navigate to project directory
cd AzureDemoApi

# Test locally
dotnet run
# Verify at https://localhost:5001/weatherforecast
```

### Step 2: Prepare for Azure Deployment

Update `Program.cs` to handle Azure environment:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

var port = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://0.0.0.0:8080";
app.Urls.Add(port);

app.Run();
```

### Step 3: Create Azure Resources

```bash
# Variables
RESOURCE_GROUP="rg-azuredemo"
LOCATION="eastus"
APP_SERVICE_PLAN="asp-azuredemo"
WEB_APP_NAME="azuredemoapi"

# Create resource group
az group create \
    --name $RESOURCE_GROUP \
    --location $LOCATION

# Create App Service plan
az appservice plan create \
    --name $APP_SERVICE_PLAN \
    --resource-group $RESOURCE_GROUP \
    --sku S1 \
    --is-linux

# Create Web App
az webapp create \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --plan $APP_SERVICE_PPlan \
    --runtime "DOTNETCORE:8.0"
```

### Step 4: Configure Application Settings

```bash
# Set environment variables
az webapp config appsettings set \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --settings ASPNETCORE_ENVIRONMENT="Production"

# Enable HTTP 2.0
az webapp config set \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --http20-enabled true
```

### Step 5: Deploy Application

```bash
# Use zipdeploy for deployment
dotnet publish -c Release -o ./publish

az webapp deployment source config-local-git \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP

# Get deployment URL
DEPLOYMENT_URL=$(az webapp deployment list-publishing-credentials \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --query "[].publishUrl" -o tsv)

# Deploy using zipdeploy
az webapp deployment source sync \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP
```

Or using the Azure CLI direct deployment:

```bash
az webapp up --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --plan $APP_SERVICE_PLAN \
    --location $LOCATION
```

### Step 6: Verify Deployment

```bash
# Check app status
az webapp show --name $WEB_APP_NAME --resource-group $RESOURCE_GROUP

# Test the API
curl https://$WEB_APP_NAME.azurewebsites.net/weatherforecast

# View logs
az webapp log tail --name $WEB_APP_NAME --resource-group $RESOURCE_GROUP
```

## 🟡 Deploying a Node.js Application

This exercise covers deploying an Express.js application to Azure App Service.

### Step 1: Create the Application

```bash
# Initialize project
mkdir azure-node-demo
cd azure-node-demo
npm init -y

# Install Express
npm install express cors dotenv

# Create server.js
cat > server.js << 'EOF'
const express = require('express');
const cors = require('cors');
require('dotenv').config();

const app = express();
const PORT = process.env.PORT || 8080;

app.use(cors());
app.use(express.json());

// Health check endpoint
app.get('/health', (req, res) => {
    res.json({ status: 'healthy', timestamp: new Date().toISOString() });
});

// API endpoints
app.get('/api/data', (req, res) => {
    res.json({
        message: 'Azure App Service Node.js Demo',
        version: '1.0.0',
        data: [1, 2, 3, 4, 5]
    });
});

app.post('/api/data', (req, res) => {
    const { name, value } = req.body;
    res.json({ submitted: true, name, value });
});

app.listen(PORT, '0.0.0.0', () => {
    console.log(`Server running on port ${PORT}`);
});
EOF

# Create package.json with start script
cat > package.json << 'EOF'
{
  "name": "azure-node-demo",
  "version": "1.0.0",
  "main": "server.js",
  "scripts": {
    "start": "node server.js",
    "dev": "node server.js"
  },
  "dependencies": {
    "cors": "^2.8.5",
    "dotenv": "^16.0.0",
    "express": "^4.18.0"
  }
}
EOF

# Test locally
npm start
```

### Step 2: Create Azure Resources

```bash
RESOURCE_GROUP="rg-nodedemo"
LOCATION="eastus"
APP_SERVICE_PLAN="asp-nodedemo"
WEB_APP_NAME="azuredemonode"

az group create --name $RESOURCE_GROUP --location $LOCATION

az appservice plan create \
    --name $APP_SERVICE_PLAN \
    --resource-group $RESOURCE_GROUP \
    --sku S1 \
    --is-linux

az webapp create \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --plan $APP_SERVICE_PLAN \
    --runtime "NODE:18-lts"
```

### Step 3: Deploy Using Git

```bash
# Configure local Git deployment
az webapp deployment source config-local-git \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP

# Get deployment credentials
az webapp deployment list-publishing-credentials \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP
```

### Step 4: Deploy Using Azure CLI

```bash
# Deploy application
az webapp up --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --plan $APP_SERVICE_PLAN \
    --location $LOCATION
```

### Step 5: Verify

```bash
# Test endpoints
curl https://$WEB_APP_NAME.azurewebsites.net/health
curl https://$WEB_APP_NAME.azurewebsites.net/api/data
```

## 🟡 Configuring Deployment Slots

This exercise demonstrates using deployment slots for zero-downtime deployments.

### Prerequisites

- Standard tier or higher App Service plan

### Step 1: Create Staging Slot

```bash
RESOURCE_GROUP="rg-demo"
WEB_APP_NAME="mywebapp"
SLOT_NAME="staging"

# Create deployment slot
az webapp deployment slot create \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --slot $SLOT_NAME
```

### Step 2: Configure Slot Settings

```bash
# Set staging-specific settings
az webapp config appsettings set \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --slot $SLOT_NAME \
    --settings ENVIRONMENT="Staging"
```

### Step 3: Deploy to Staging

```bash
# Deploy to staging slot
az webapp deployment slot publish \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --slot $SLOT_NAME

# Verify staging
curl https://$WEB_APP_NAME-staging.azurewebsites.net/health
```

### Step 4: Test and Swap

```bash
# Run smoke tests on staging
# If all tests pass, swap to production

# Swap staging to production
az webapp deployment slot swap \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --slot $SLOT_NAME \
    --target-slot production
```

### Step 5: Rollback (if needed)

```bash
# Swap back to previous version
az webapp deployment slot swap \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --slot production \
    --target-slot $SLOT_NAME
```

### Traffic Routing

```bash
# Route percentage of traffic to staging
az webapp update \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --slot-config-names-names production \
    --duration 01:00:00 \
    --percentage 20
```

## 🟡 Configuring Auto-Scaling

This exercise sets up auto-scaling for production workloads.

### Prerequisites

- Standard tier App Service plan

### Step 1: Enable Autoscale

```bash
RESOURCE_GROUP="rg-demo"
APP_SERVICE_PLAN="myasp"

# Create autoscale setting
az monitor autoscale create \
    --name $APP_SERVICE_PLAN-autoscale \
    --resource-group $RESOURCE_GROUP \
    --resource "/subscriptions/$(az account show --query id -o tsv)/providers/Microsoft.Web/serverfarms/$APP_SERVICE_PLAN" \
    --最小-count 1 \
    --最大-count 10
```

### Step 2: Add Scale-Out Rule

```bash
# Add scale-out rule (CPU > 70%)
az monitor autoscale rule create \
    --name $APP_SERVICE_PLAN-autoscale \
    --resource-group $RESOURCE_GROUP \
    --condition "CPU percentage > 70 avg 5m" \
    --scale-out-operations 2 \
    --scale-type ChangeCount \
    --cooldown 5
```

### Step 3: Add Scale-In Rule

```bash
# Add scale-in rule (CPU < 30%)
az monitor autoscale rule create \
    --name $APP_SERVICE_PLAN-autoscale \
    --resource-group $RESOURCE_GROUP \
    --condition "CPU percentage < 30 avg 5m" \
    --scale-in-operations 1 \
    --scale-type ChangeCount \
    --cooldown 5
```

### Step 4: Configure HTTP-Based Scaling

```bash
# Add HTTP queue scaling rule
az monitor autoscale rule create \
    --name $APP_SERVICE_PLAN-autoscale \
    --resource-group $RESOURCE_GROUP \
    --condition "HttpQueue > 50 avg 3m" \
    --scale-out-operations 1 \
    --scale-type ChangeCount
```

### Step 5: Add Schedule-Based Rules

```bash
# Create business hours profile
az monitor autoscale profile create \
    --name $APP_SERVICE_PLAN-autoscale \
    --resource-group $RESOURCE_GROUP \
    --profile-name "business-hours" \
    --start "0 0 0 * * 1-5" \
    --end "0 0 0 * * 1-5" \
    --recurrence weekly mon,tue,wed,thu,fri

# Add rule for business hours
az monitor autoscale rule create \
    --name $APP_SERVICE_PLAN-autoscale \
    --resource-group $RESOURCE_GROUP \
    --condition "CPU > 50" \
    --scale-out-operations 2 \
    --profile-name "business-hours"
```

### Step 6: Test Auto-Scaling

```bash
# View autoscale settings
az monitor autoscale show \
    --name $APP_SERVICE_PLAN-autoscale \
    --resource-group $RESOURCE_GROUP

# List scale conditions
az monitor autoscale list-rules \
    --name $APP_SERVICE_PLAN-autoscale \
    --resource-group $RESOURCE_GROUP

# Generate load for testing
# Use Apache Bench or similar:
ab -n 10000 -c 100 https://mywebapp.azurewebsites.net/api/data
```

## 🟡 Configuring Custom Domain

This exercise configures a custom domain with TLS for your web application.

### Prerequisites

- Custom domain registered (purchased from registrar or internal)

### Step 1: Add Custom Domain

```bash
RESOURCE_GROUP="rg-demo"
WEB_APP_NAME="mywebapp"
CUSTOM_DOMAIN="www.myapp.com"

# Add custom domain
az webapp config hostname bind \
    --hostname $CUSTOM_DOMAIN \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP
```

### Step 2: Verify Domain Ownership

```bash
# Get TXT record for domain verification
az webapp config hostname get-app-setting \
    --hostname $CUSTOM_DOMAIN \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP
```

Add TXT record at DNS provider:

- Host: awverify._domainkey.myapp.com
- Value: <verification-value>

### Step 3: Create TLS/SSL Certificate

Option A: Use App Service Managed Certificate (free)

```bash
# Create managed certificate
az webapp config ssl create \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --hostname $CUSTOM_DOMAIN
```

Option B: Upload custom certificate

```bash
# Upload PFX certificate
az webapp config ssl upload \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --certificate-file "certificate.pfx" \
    --certificate-password "mypassword"

# Get certificate thumbprint
THUMBPRINT=$(az webapp config ssl list \
    --resource-group $RESOURCE_GROUP \
    --query "[?主机名=='$CUSTOM_DOMAIN'].thumbprint" -o tsv)
```

### Step 4: Bind Certificate to Domain

```bash
# Bind SSL certificate
az webapp config ssl bind \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --hostname $CUSTOM_DOMAIN \
    --certificate-thumbprint $THUMBPRINT
```

### Step 5: Enforce HTTPS

```bash
# Enable HTTPS-only
az webapp update \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --https-only true
```

### Step 6: Configure TLS Version

```bash
# Set minimum TLS version
az webapp config set \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --min-tls-version "1.2"
```

### Step 7: Test Configuration

```bash
# Verify HTTPS works
curl -I https://$CUSTOM_DOMAIN/health

# Check SSL certificate
curl -I -v https://$CUSTOM_DOMAIN
```

## 🟡 Complete Deployment Exercise

This comprehensive exercise combines all concepts into a production-ready deployment.

### Complete Script

```bash
#!/bin/bash
# Complete deployment script

# Variables
RESOURCE_GROUP="rg-production"
LOCATION="eastus"
APP_NAME="myproductionapp"
PLAN_NAME="asp-production"
SLOTS=("staging" "qa")

# Colors for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

log() { echo -e "${GREEN}[$(date +'%T')] $1${NC}"; }
warn() { echo -e "${YELLOW}[$(date +'%T')] $1${NC}"; }

# Step 1: Create Resource Group
log "Creating resource group..."
az group create \
    --name $RESOURCE_GROUP \
    --location $LOCATION 2>/dev/null || warn "Resource group already exists"

# Step 2: Create App Service Plan
log "Creating App Service plan..."
az appservice plan create \
    --name $PLAN_NAME \
    --resource-group $RESOURCE_GROUP \
    --sku S1 \
    --is-linux 2>/dev/null || warn "App Service plan already exists"

# Step 3: Create Web App
log "Creating Web App..."
az webapp create \
    --name $APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --plan $PLAN_NAME \
    --runtime "NODE:18-lts" 2>/dev/null || warn "Web App already exists"

# Step 4: Configure Settings
log "Configuring application settings..."
az webapp config set \
    --name $APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --http20-enabled true \
    --min-tls-version "1.2"

az webapp config appsettings set \
    --name $APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --settings NODE_ENV="production" \
    --WEBSITE_ENABLE_OUTPUT_CACHE=1

# Step 5: Create Deployment Slots
log "Creating deployment slots..."
for SLOT in "${SLOTS[@]}"; do
    az webapp deployment slot create \
        --name $APP_NAME \
        --resource-group $RESOURCE_GROUP \
        --slot $SLOT 2>/dev/null || warn "Slot $SLOT already exists"
done

# Step 6: Configure Auto-Scale
log "Configuring auto-scaling..."
az monitor autoscale create \
    --name "${PLAN_NAME}-autoscale" \
    --resource-group $RESOURCE_GROUP \
    --resource "/subscriptions/$(az account show --query id -o tsv)/providers/Microsoft.Web/serverfarms/$PLAN_NAME" \
    --最小-count 1 \
    --最大-count 10 2>/dev/null || warn "Auto-scaling already configured"

# Add scaling rules
az monitor autoscale rule create \
    --name "${PLAN_NAME}-autoscale" \
    --resource-group $RESOURCE_GROUP \
    --condition "CPU percentage > 70 avg 5m" \
    --scale-out-operations 2 \
    --cooldown 5

az monitor autoscale rule create \
    --name "${PLAN_NAME}-autoscale" \
    --resource-group $RESOURCE_GROUP \
    --condition "CPU percentage < 30 avg 5m" \
    --scale-in-operations 1 \
    --cooldown 5

# Step 7: Deploy Application
log "Deploying application..."
az webapp deployment source config-local-git \
    --name $APP_NAME \
    --resource-group $RESOURCE_GROUP

DEPLOYMENT_URL=$(az webapp deployment list-publishing-credentials \
    --name $APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --query "[].publishUrl" -o tsv)

# Deploy to production
echo "Deployment URL: https://$DEPLOYMENT_URL"
echo "Use Git to push your code to this URL"

log "Deployment complete!"
log "Web App URL: https://${APP_NAME}.azurewebsites.net"
```

## 🟡 Troubleshooting Common Issues

### Application Won't Start

```bash
# Check logs
az webapp log tail --name $APP_NAME --resource-group $RESOURCE_GROUP

# Check configuration
az webapp config show --name $APP_NAME --resource-group $RESOURCE_GROUP

# Check startup file
az webapp config show --name $APP_NAME --resource-group $RESOURCE_GROUP --query "startupFile"
```

### Deployment Failures

```bash
# Check deployment status
az webapp deployment list --name $APP_NAME --resource-group $RESOURCE_GROUP

# Sync deployment
az webapp deployment source sync --name $APP_NAME --resource-group $RESOURCE_GROUP
```

### SSL Certificate Issues

```bash
# List certificates
az webapp config ssl list --resource-group $RESOURCE_GROUP

# Check certificate bindings
az webapp config ssl list --name $APP_NAME --resource-group $RESOURCE_GROUP
```

### Scaling Issues

```bash
# Check current instance count
az webapp show --name $APP_NAME --resource-group $RESOURCE_GROUP --query "maximumNumberOfWorkers"

# Check autoscale history
az monitor autoscale show \
    --name "${PLAN_NAME}-autoscale" \
    --resource-group $RESOURCE_GROUP
```

## 🟡 Monitoring and Maintenance

### Configure Application Insights

```bash
# Create Application Insights
az monitor app-insights component create \
    --app $APP_NAME-insights \
    --location $LOCATION \
    --resource-group $RESOURCE_GROUP

# Get instrumentation key
INSTRUMENTATION_KEY=$(az monitor app-insights component show \
    --app $APP_NAME-insights \
    --resource-group $RESOURCE_GROUP \
    --query "instrumentationKey" -o tsv)

# Configure app to use Application Insights
az webapp config appsettings set \
    --name $APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --settings APPINSIGHTS_INSTRUMENTATIONKEY=$INSTRUMENTATION_KEY
```

### Set Up Backups

```bash
# Configure backup
az webapp config backup update \
    --name $APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --storage-account mystorageaccount \
    --backup-schedule "Daily 030000" \
    --retain-days 30
```

## 🟡 Summary

This practical module covered:

1. **.NET Application Deployment**: ASP.NET Core API deployment to Azure
2. **Node.js Application Deployment**: Express.js application deployment
3. **Deployment Slots**: Staging, testing, and production swapping
4. **Auto-Scaling Configuration**: Metric-based scaling rules
5. **Custom Domain Setup**: SSL/TLS configuration with custom domains

These hands-on exercises prepare you for real-world Azure App Service deployments and the AZ-104 Azure Administrator certification exam. Combined with the basic and advanced modules, you now have comprehensive practical knowledge of Azure App Service.

For continued learning, explore:

- Azure Functions for serverless architectures
- Azure Container Apps for containerized workloads
- Azure Static Web Apps for frontend applications
- Azure API Management for API governance