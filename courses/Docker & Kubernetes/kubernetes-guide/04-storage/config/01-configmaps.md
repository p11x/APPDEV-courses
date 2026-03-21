# ConfigMaps

## Overview

ConfigMaps store non-sensitive configuration data that can be consumed by pods. They allow you to separate configuration from application code, making applications more portable and easier to configure across environments.

## Example

```yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: my-config
data:
  DATABASE_HOST: "db-service"
  DATABASE_PORT: "5432"
  config.json: |
    {
      "logLevel": "info",
      "timeout": 30
    }
```

## Using ConfigMaps in Pods

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: my-pod
spec:
  containers:
  - name: my-container
    image: myapp
    envFrom:
    - configMapRef:
        name: my-config
```

## Common Mistakes

- **Storing secrets**: Use Secrets for sensitive data.
- **Large configs**: Limit ConfigMap size to 1MB.

## What's Next

Continue to [Secrets](./02-secrets.md)
