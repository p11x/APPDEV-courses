# Projected Volumes

## Overview

Projected volumes mount several existing volume sources into the same directory. They allow you to combine ConfigMaps, Secrets, and Downward API information into a single volume that can be mounted into a pod.

## Example

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: my-pod
spec:
  containers:
  - name: my-container
    image: myapp
    volumeMounts:
    - name: projected-config
      mountPath: /projected
  volumes:
  - name: projected-config
    projected:
      sources:
      - configMap:
          name: my-config
      - secret:
          name: my-secret
```

## What's Next

Continue to [Liveness and Readiness Probes](../operations/observability/03-liveness-and-readiness-probes.md)
