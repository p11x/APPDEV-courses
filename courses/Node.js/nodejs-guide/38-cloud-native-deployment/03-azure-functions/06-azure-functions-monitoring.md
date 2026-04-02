# Azure Functions Monitoring

## What You'll Learn

- How to monitor Azure Functions
- How to configure Application Insights
- How to set up alerts and diagnostics
- How to analyze function performance

---

## Layer 1: Monitoring Stack

### Application Insights Integration

```json
// host.json
{
  "version": "2.0",
  "extensionBundle": {
    "id": "Microsoft.Azure.Functions.ExtensionBundle",
    "version": "[4.*, 5.0.0)"
  },
  "logging": {
    "logLevel": {
      "default": "Information",
      "Function": "Debug",
      "Host.Results": "Information"
    },
    "applicationInsights": {
      "samplingSettings": {
        "isEnabled": true,
        "maxTelemetryItemsPerSecond": 20
      }
    }
  }
}
```

---

## Next Steps

Continue to [Azure Functions vs AWS](./07-azure-functions-vs-aws.md)