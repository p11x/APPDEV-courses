# ClusterIP

## Overview

ClusterIP is the default Kubernetes Service type. It exposes the Service on a cluster-internal IP, making it reachable only from within the cluster. This is perfect for internal communication between application components.

## Example

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
  type: ClusterIP
```

## How It Works

- Gets internal cluster IP
- Load balances across pods
- Pods can access via service name
- Not accessible from outside

## What's Next

Continue to [NodePort and LoadBalancer](./02-nodeport-and-loadbalancer.md)
