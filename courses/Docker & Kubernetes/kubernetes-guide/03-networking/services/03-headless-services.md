# Headless Services

## Overview

A headless Service doesn't have a cluster IP. Instead, it returns the IPs of the pods directly via DNS. This is useful for stateful applications that need direct pod access.

## Example

```yaml
apiVersion: v1
kind: Service
metadata:
  name: my-headless-service
spec:
  selector:
    app: myapp
  ports:
  - port: 80
    targetPort: 8080
  clusterIP: None
```

## When to Use

- StatefulSets
- Database clusters
- When clients need pod IPs

## What's Next

Continue to [ConfigMaps and Secrets](../storage/config/01-configmaps.md)
