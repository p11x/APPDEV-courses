# Secrets

## Overview

Secrets store sensitive data like passwords, OAuth tokens, and SSH keys. They are similar to ConfigMaps but are Base64 encoded and can be encrypted at rest. Use Secrets to pass sensitive information to pods securely.

## Example

```yaml
apiVersion: v1
kind: Secret
metadata:
  name: my-secret
type: Opaque
stringData:
  username: admin
  password: secret123
```

## Using Secrets in Pods

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: my-pod
spec:
  containers:
  - name: my-container
    image: myapp
    env:
    - name: DB_PASSWORD
      valueFrom:
        secretKeyRef:
          name: my-secret
          key: password
```

## Best Practices

- Use external secret management in production
- Enable encryption at rest
- Rotate secrets regularly

## What's Next

Continue to [Liveness and Readiness Probes](../operations/observability/03-liveness-and-readiness-probes.md)
