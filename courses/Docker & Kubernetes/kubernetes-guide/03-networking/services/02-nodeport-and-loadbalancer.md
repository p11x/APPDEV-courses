# NodePort and LoadBalancer

## Overview

NodePort and LoadBalancer are Service types that expose services externally. NodePort opens a specific port on all nodes, while LoadBalancer provisions external load balancers in cloud environments.

## NodePort Example

```yaml
apiVersion: v1
kind: Service
metadata:
  name: my-service
spec:
  selector:
    app: myapp
  ports:
  - protocol: TCP
    port: 80
    targetPort: 8080
    nodePort: 30080
  type: NodePort
```

## LoadBalancer Example

```yaml
apiVersion: v1
kind: Service
metadata:
  name: my-service
spec:
  selector:
    app: myapp
  ports:
  - protocol: TCP
    port: 80
    targetPort: 8080
  type: LoadBalancer
```

## Comparison

| Type | Access | Use Case |
|------|--------|----------|
| ClusterIP | Internal only | Microservices |
| NodePort | Via node IP:port | Development |
| LoadBalancer | Via cloud LB | Production |

## What's Next

Continue to [ConfigMaps](../storage/config/01-configmaps.md)
