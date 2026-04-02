# Azure Functions Deployment

## What You'll Learn

- How to deploy Azure Functions to production
- How to configure deployment slots
- How to implement CI/CD for functions
- How to manage function app settings

---

## Layer 1: Deployment Methods

### Azure CLI Deployment

```bash
# Create function app
az functionapp create \
  --resource-group myResourceGroup \
  --consumption-plan-location eastus \
  --name myFunctionApp \
  --storage-account mystorageaccount

# Configure deployment
az functionapp deployment source config-local-git \
  --name myFunctionApp \
  --resource-group myResourceGroup

# Get deployment credentials
az functionapp deployment list-publishing-credentials
```

### ZIP Deployment

```bash
# Build and package
npm run build
zip -r function.zip dist/

# Deploy
az functionapp deployment source delete -g MyGroup -n MyApp
az functionapp deployment source config-zip \
  --resource-group MyGroup \
  --name MyApp \
  --src function.zip
```

---

## Layer 2: Deployment Slots

```bash
# Create staging slot
az functionapp deployment slot create \
  --resource-group MyGroup \
  --name MyApp \
  --slot staging

# Swap slots
az functionapp deployment slot swap \
  --resource-group MyGroup \
  --name MyApp \
  --slot staging
```

---

## Next Steps

Continue to [Azure Functions Monitoring](./06-azure-functions-monitoring.md)