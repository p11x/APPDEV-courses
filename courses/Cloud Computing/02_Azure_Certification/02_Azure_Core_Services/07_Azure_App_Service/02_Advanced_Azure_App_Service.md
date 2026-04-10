---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure App Service
Purpose: Advanced Azure App Service configuration and enterprise features
Difficulty: advanced
Prerequisites: 01_Basic_Azure_App_Service.md
RelatedFiles: 01_Basic_Azure_App_Service.md, 03_Practical_Azure_App_Service.md
UseCase: Enterprise web apps
CertificationExam: AZ-204 Azure Developer
LastUpdated: 2025
---

# Azure App Service - Advanced

This module covers enterprise-grade features and advanced configuration options for Azure App Service. These capabilities are essential for production workloads requiring high availability, custom security configurations, and automated DevOps pipelines.

## 🔴 Auto-Scaling Configuration

Auto-scaling enables automatic adjustment of compute resources based on demand metrics. This feature is available in Standard, Premium, and Isolated tiers.

### Understanding Auto-Scaling

Auto-scaling in Azure App Service operates at two levels:

1. **Scale Out**: Adding more instances when demand increases
2. **Scale In**: Removing instances when demand decreases

The platform supports metric-based scaling and schedule-based scaling rules.

### Configuring Auto-Scaling

```bash
# Enable autoscale on an App Service plan
az monitor autoscale create \
    --name myAutoscale \
    --resource-group myResourceGroup \
    --resource "/subscriptions/.../providers/Microsoft.Web/serverfarms/myAppServicePlan" \
    --minimal-count 1 \
    --maximal-count 10

# Add scaling rule based on CPU percentage
az monitor autoscale rule create \
    --name myAutoscale \
    --resource-group myResourceGroup \
    --condition "CPU percentage > 70 avg 5m" \
    --scale-out-operations 2

# Add scaling rule based on HTTP queue depth
az monitor autoscale rule create \
    --name myAutoscale \
    --resource-group myResourceGroup \
    --condition "HttpQueue > 100 avg 5m" \
    --scale-out-operations 1

# Add scale-in rule
az monitor autoscale rule create \
    --name myAutoscale \
    --resource-group myResourceGroup \
    --condition "CPU percentage < 30 avg 5m" \
    --scale-in-operations 1
```

### Auto-Scale Rules Best Practices

1. **Set Appropriate Thresholds**: CPU at 70-80% for scale-out, 20-30% for scale-in
2. **Use Multiple Metrics**: Combine CPU, memory, and HTTP queue for better decisions
3. **Define Cooldown Periods**: Wait 5-10 minutes between scaling actions
4. **Test Scaling Rules**: Simulate load to verify rule effectiveness
5. **Monitor Costs**: Track spending as instances scale

### Azure Portal Configuration

In the Azure Portal, auto-scale settings can be configured through:

1. Navigate to your App Service plan
2. Select "Scale out (App Service plan)"
3. Choose "Custom autoscale"
4. Add rules based on:
   - CPU percentage
   - Memory percentage
   - HTTP requests per second
   - Azure Queue messages
   - Azure Storage queue

### Scaling Profiles

```bash
# Create time-based scaling profile
az monitor autoscale profile create \
    --name myAutoscale \
    --resource-group myResourceGroup \
    --profile-name "business-hours" \
    --start "0 0 0 * * 1-5" \
    --end "0 0 0 * * 1-5" \
    --recurrence weekly mon,tue,wed,thu,fri

# Add scaling rules to profile
az monitor autoscale rule create \
    --name myAutoscale \
    --resource-group myResourceGroup \
    --condition "CPU > 50" \
    --scale-out-operations 1 \
    --profile-name "business-hours"
```

## 🔴 Custom Domains with TLS

Custom domains allow users to access your application using your own domain name instead of the default `*.azurewebsites.net` domain.

### Purchasing a Domain

```bash
# Check domain availability
az domain check-name \
    --name mycustomdomain.com \
    --resource-group myResourceGroup
```

### Adding Custom Domain

```bash
# Add domain to Web App
az webapp config hostname bind \
    --hostname www.mycustomdomain.com \
    --name myWebApp \
    --resource-group myResourceGroup
```

### TLS/SSL Certificates

#### Using App Service Managed Certificates

Azure provides free TLS/SSL certificates for custom domains:

```bash
# Bind App Service managed certificate
az webapp config ssl bind \
    --name myWebApp \
    --resource-group myResourceGroup \
    --hostname www.mycustomdomain.com \
    --certificate-thumbprint <managed-cert-thumbprint>
```

#### Uploading Custom Certificate

```bash
# Upload SSL certificate
az webapp config ssl upload \
    --name myWebApp \
    --resource-group myResourceGroup \
    --certificate-file "certificate.pfx" \
    --certificate-password "mypassword"

# Bind certificate to hostname
az webapp config ssl bind \
    --name myWebApp \
    --resource-group myResourceGroup \
    --hostname www.mycustomdomain.com \
    --certificate-thumbprint <cert-thumbprint>
```

### TLS Configuration

```bash
# Enforce minimum TLS version
az webapp config set \
    --name myWebApp \
    --resource-group myResourceGroup \
    --min-tls-version "1.2"

# Enable HTTP 2.0
az webapp config set \
    --name myWebApp \
    --resource-group myResourceGroup \
    --http20-enabled true
```

### Certificate Management Best Practices

1. **Use TLS 1.2+**: Disable older versions for security
2. **Renew Certificates**: Set renewal reminders 30 days before expiration
3. **Use Key Vault**: Store certificates in Azure Key Vault for management
4. **Implement HSTS**: Enable HTTP Strict Transport Security

```bash
# Store certificate in Key Vault
az keyvault certificate import \
    --vault-name myKeyVault \
    --name myCertificate \
    --file "certificate.pfx" \
    --password "mypassword"

# Configure Web App to use Key Vault certificate
az webapp config ssl import \
    --name myWebApp \
    --resource-group myResourceGroup \
    --key-vault myKeyVault \
    --key-vault-certificate-name myCertificate
```

## 🔴 CI/CD Integration

Continuous Integration and Continuous Deployment pipelines automate the build, test, and deployment process.

### GitHub Actions Integration

```yaml
# .github/workflows/azure-deploy.yml
name: Deploy to Azure App Service

on:
  push:
    branches: [ main ]

jobs:
  build:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Set up .NET Core
      uses: actions/setup-dotnet@v2
      with:
        dotnet-version: '8.0.x'
    
    - name: Build
      run: dotnet build --configuration Release
    
    - name: Test
      run: dotnet test --configuration Release --no-build
    
    - name: Publish
      run: dotnet publish --configuration Release --output ./publish
    
    - name: Azure WebApp
      uses: azure/webapps-deploy@v3
      with:
        app-name: myWebApp
        publish-profile: ${{ secrets.AZURE_PUBLISH_PROFILE }}
        package: ./publish
```

### Azure DevOps Pipeline

```yaml
# azure-pipelines.yml
trigger:
- main

pool:
  vmImage: 'ubuntu-latest'

variables:
  webAppName: 'myWebApp'
  buildConfiguration: 'Release'

steps:
- task: UsePythonVersion@0
  inputs:
    versionSpec: '3.11'
  
- script: |
    python -m pip install --upgrade pip
    pip install -r requirements.txt
  displayName: 'Install dependencies'

- task: Python@0
  inputs:
    command: 'test'
    workingDirectory: '$(System.DefaultWorkingDirectory)'
    args: '--coverage xml'

- task: Python@0
  inputs:
    command: 'build'
    workingDirectory: '$(System.DefaultWorkingDirectory)'

- task: AzureWebApp@1
  inputs:
    azureSubscription: 'myAzureSubscription'
    appType: 'webAppLinux'
    appName: '$(webAppName)'
    package: '$(System.DefaultWorkingDirectory)/**/dist/**/*.zip'
```

### Deployment Center Configuration

```bash
# Configure deployment center using Azure CLI
az webapp deployment source config-local-git \
    --name myWebApp \
    --resource-group myResourceGroup

# Get deployment credentials
az webapp deployment list-publishing-credentials \
    --name myWebApp \
    --resource-group myResourceGroup
```

### Container Registry Integration

```bash
# Configure Azure Container Registry
az webapp config container set \
    --name myWebApp \
    --resource-group myResourceGroup \
    --container-image-name myregistry.azurecr.io/myapp:latest \
    --container-registry-url https://myregistry.azurecr.io \
    --container-registry-username myusername \
    --container-registry-password mypassword
```

### Webhooks for Continuous Deployment

```bash
# Register webhook with container registry
az acr webhook create \
    --name myWebhook \
    --resource-group myResourceGroup \
    --registry myregistry \
    --uri https://mywebapp.azurewebsites.net/api/containers \
    --actions push
```

## 🔴 Azure App Service vs AWS Elastic Beanstalk vs GCP App Engine

Understanding how Azure App Service compares with competitor offerings helps in making informed architectural decisions.

### Comparison Table

| Feature | Azure App Service | AWS Elastic Beanstalk | GCP App Engine |
|---------|-------------------|----------------------|----------------|
| **Pricing Model** | Pay-per-use, tiered | Pay-per-use | Pay-per-use with free tier |
| **Supported Languages** | .NET, Java, Node.js, Python, Ruby, Go, PHP | Java, .NET, Node.js, Python, Ruby, Go, PHP | Python, Java, Go, Node.js, Ruby, PHP, .NET |
| **Free Tier** | Yes (limited) | Yes (single instance) | Yes (F1/B1 instances) |
| **Auto-Scaling** | Yes (metric-based) | Yes (metric-based) | Yes (automatic) |
| **Custom Domains** | Yes | Yes | Yes |
| **TLS Certificates** | Free managed + custom | Free managed + custom | Free managed only |
| **Deployment Slots** | Yes (Standard+) | Yes (with additional config) | Yes (versions) |
| **Container Support** | Windows/Linux containers | Docker | Docker |
| **VPC Integration** | Yes | Yes | Yes |
| **Sticky Sessions** | Yes | Yes | Yes |
| **Max Instances** | 20 (Standard), 100 (Premium) | Unlimited | Unlimited |
| **HTTP/2 Support** | Yes | Yes | Yes |
| **WebSockets** | Yes | Yes | Yes |
| **Network Security** | VNet integration, Private Link | VPC, Security Groups | VPC, Firewall rules |
| **Monitoring** | Application Insights | CloudWatch | Cloud Logging |
| **Deployment Options** | Git, GitHub, DevOps, FTP, Docker | Git, CLI, S3, Docker | Git, Cloud Build, Docker |
| **Serverless Option** | Yes (Azure Functions) | Yes (Lambda) | Yes (Cloud Functions) |
| **Managed SSL** | App Service certificates | ACM | Cloud SSL |

### Detailed Feature Analysis

#### Scaling Capabilities

- **Azure App Service**: Scale out up to 20 instances (Standard), 100 instances (Premium). Manual and automatic scaling supported. Scale rules based on CPU, memory, HTTP queue, and custom metrics.

- **AWS Elastic Beanstalk**: Scales to custom instance limits. Supports scaling triggers based on CPU, network in/out, and custom CloudWatch metrics.

- **GCP App Engine**: Automatic scaling based on target CPU utilization, throughput, or custom metrics. Supports both automatic and manual scaling modes.

#### Deployment Slots

- **Azure**: Up to 5 slots (Standard), 20 slots (Premium). Zero-downtime swapping with traffic routing.

- **AWS**: Requires additional configuration with Elastic Beanstalk CLI or environment swaps. No native slot concept.

- **GCP**: Versions allow traffic splitting but requires additional setup for zero-downtime deployments.

#### Pricing

- **Azure**: Base + compute costs. Free tier for learning. Standard starts at ~$70/month.

- **AWS**: Based on EC2 instance types. Free tier available. Development environment ~$50/month.

- **GCP**: F1/B1 instances free tier. F2/B2 ~$30/month. More generous free tier.

### Migration Considerations

When migrating between platforms:

1. **Code Changes**: Minimal for standard frameworks; significant for platform-specific APIs
2. **Configuration**: Each platform has unique environment variables and configuration formats
3. **Services**: Native cloud services (databases, caches) may differ
4. **Networking**: VPN and firewall rules require platform-specific configuration
5. **Monitoring**: Different logging and metrics tools

## 🔴 Advanced Networking

### Private Endpoint

Secure your App Service with a private endpoint accessible only from your virtual network:

```bash
# Create private endpoint
az network private-endpoint create \
    --name myPrivateEndpoint \
    --resource-group myResourceGroup \
    --vnet-name myVnet \
    --subnet mySubnet \
    --connection-name myConnection \
    --private-link-resource-id $(az webapp show -n myWebApp -g myResourceGroup --query id)
```

### Azure Front Door Integration

```bash
# Configure Azure Front Door with App Service
az afd endpoint create \
    --name myAFDEndpoint \
    --resource-group myResourceGroup \
    --enabled-state Enabled

az afd origin-group create \
    --name myOriginGroup \
    --resource-group myResourceGroup \
    --endpoint-name myAFDEndpoint \
    --health-path /healthcheck

az afd origin create \
    --name myAppServiceOrigin \
    --resource-group myResourceGroup \
    --origin-group-name myOriginGroup \
    --origin-host-name mywebapp.azurewebsites.net
```

### Application Gateway

```bash
# Configure Application Gateway with App Service
az network application-gateway http-settings create \
    --name myAppGatewaySettings \
    --resource-group myResourceGroup \
    --gateway-name myAppGateway \
    --protocol Http \
    --port 80 \
    --timeout 30 \
    --affinity-cookie-name ARRAffinity
```

## 🔴 Performance Optimization

### Caching Configuration

```bash
# Enable output caching
az webapp config/appsettings set \
    --name myWebApp \
    --resource-group myResourceGroup \
    --settings WEBSITE_ENABLE_OUTPUT_CACHE=1

# Configure Redis cache
az webapp config/appsettings set \
    --name myWebApp \
    --resource-group myResourceGroup \
    --settings REDIS_CONNECTION_STRING="myredis.cache.windows.net:6380,password=mypassword,ssl=True"
```

### Compression

```bash
# Enable dynamic compression
az webapp config set \
    --name myWebApp \
    --resource-group myResourceGroup \
    --http20-enabled true
```

### Connection Strings

Optimize database connections:

```bash
# Set connection string with encryption
az webapp config connection-string set \
    --name myWebApp \
    --resource-group myResourceGroup \
    --connection-string-type SQLAzure \
    --name "DefaultConnection" \
    --value "Server=tcp:myserver.database.windows.net,1433;Database=mydb;Encrypt=True;TrustServerCertificate=False"
```

## 🔴 Disaster Recovery

### Backup and Restore

```bash
# Configure storage account for backups
az webapp config backup update \
    --name myWebApp \
    --resource-group myResourceGroup \
    --storage-account "mystorageaccount" \
    --backup-schedule "Daily 030000" \
    --retain-days 30

# Create manual backup
az webapp backup create \
    --name myWebApp \
    --resource-group myResourceGroup \
    --backup-name "mymanualbackup"

# Restore from backup
az webapp backup restore \
    --name myWebApp \
    --resource-group myResourceGroup \
    --backup-name "mymanualbackup" \
    --overwrite true
```

## 🔴 Diagnostics and Troubleshooting

### Advanced Logging

```bash
# Configure detailed error logging
az webapp config set \
    --name myWebApp \
    --resource-group myResourceGroup \
    --detailed-error-logging-configured true

# Enable failed request tracing
az webapp config set \
    --name myWebApp \
    --resource-group myResourceGroup \
    --failed-request-tracing-enabled true

# Stream diagnostics
az webapp log download \
    --name myWebApp \
    --resource-group myResourceGroup \
    --output-file ./diagnostics.zip
```

### Kudu Debug Console

The Kudu service console provides advanced debugging capabilities accessible at:

```
https://mywebapp.scm.azurewebsites.net
```

Features include:

- File system browser
- Process explorer
- Environment variables editor
- SSH terminal (Linux)
- Debug snapshot capture

## 🔴 Security Hardening

### IP Restrictions

```bash
# Add IP restriction rule
az webapp config access-restriction add \
    --name myWebApp \
    --resource-group myResourceGroup \
    --rule-name "AllowCorporate" \
    --ip-address "203.0.113.0/255.255.255.0" \
    --action Allow \
    --priority 100

# Disable public access
az webapp config access-restriction add \
    --name myWebApp \
    --resource-group myResourceGroup \
    --rule-name "DenyAll" \
    --ip-address "0.0.0.0/0" \
    --action Deny
```

### Managed Identity

```bash
# Enable system-managed identity
az webapp identity assign \
    --name myWebApp \
    --resource-group myResourceGroup

# Grant Key Vault access
az keyvault set-policy \
    --name myKeyVault \
    --object-id $(az webapp show -n myWebApp -g myResourceGroup --query identity.principalId) \
    --secret-permissions get list
```

### Security Headers

```bash
# Configure security headers via web.config
az webapp config set \
    --name myWebApp \
    --resource-group myResourceGroup \
    --x-ms-version-required 2015
```

## 🔴 Summary

Advanced Azure App Service features enable enterprise-grade deployments with:

1. **Auto-Scaling**: Automatic resource adjustment based on demand metrics
2. **Custom Domains**: Branded URLs with TLS encryption
3. **CI/CD Pipelines**: Automated build and deployment workflows
4. **Security Hardening**: IP restrictions, managed identities, and security headers
5. **Performance Optimization**: Caching, compression, and connection pooling

These capabilities prepare you for the AZ-204 Azure Developer Associate certification and production enterprise deployments. Combined with the basic concepts from the previous module, you now have comprehensive knowledge of Azure App Service.

In the practical module, we'll apply these concepts to deploy real applications with all the advanced features configured.