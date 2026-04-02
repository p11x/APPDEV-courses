# Azure Functions Setup

## What You'll Learn

- How to set up Azure Functions development environment
- How to install Azure Functions Core Tools
- How to configure VS Code for Azure development
- How to create your first Azure Functions project

---

## Layer 1: Academic Foundation

### Serverless on Azure

Azure Functions is Microsoft's serverless compute service that enables event-driven code execution without managing infrastructure.

**Key Concepts:**
- **Triggers**: Events that initiate function execution
- **Bindings**: Declarative connections to data sources
- **Durable Functions**: Stateful workflow orchestration
- **Premium Plan**: Dedicated infrastructure with VNet

---

## Layer 2: Code Evolution

### Environment Setup

```bash
# Install Azure Functions Core Tools
# For Windows (chocolatey)
choco install azure-functions-core-tools

# For macOS
brew tap azure/functions
brew install azure-functions-core-tools@4

# For Linux
curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > packages.microsoft.gpg
sudo install -o root -g root -m 644 packages.microsoft.gpg /usr/share/keyrings/
echo "deb [arch=amd64 signed-by=/usr/share/keyrings/packages.microsoft.gpg] https://packages.microsoft.com/repos/azure-functions bullseye main" | sudo tee /etc/apt/sources.list.d/azure-functions.list
sudo apt-get update
sudo apt-get install azure-functions-core-tools-4
```

### VS Code Setup

```json
// .vscode/launch.json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": "Attach to Azure Functions",
      "type": "node",
      "request": "attach",
      "port": 9229,
      "preLaunchTask": "func: host start"
    }
  ]
}
```

### Project Initialization

```bash
# Create new function app
func init --template "HTTP trigger" --name my-function

# Start local development
func start

# Azure Functions project structure
my-function/
├── host.json
├── local.settings.json
├── package.json
└── src/
    └── functions/
        └── myFunction.ts
```

---

## Layer 3: Configuration

### host.json

```json
{
  "version": "2.0",
  "extensionBundle": {
    "id": "Microsoft.Azure.Functions.ExtensionBundle",
    "version": "[4.*, 5.0.0)"
  },
  "logging": {
    "logLevel": {
      "default": "Information",
      "Host.Results": "Information",
      "Function": "Debug"
    }
  },
  "extensions": {
    "http": {
      "routePrefix": "api"
    }
  }
}
```

---

## Next Steps

Continue to [Azure Functions Node.js](./02-azure-functions-node.md) for implementation.