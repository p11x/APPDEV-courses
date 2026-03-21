# Rollbacks

## Overview

Kubernetes maintains deployment history, allowing you to roll back to previous versions when issues arise. This is crucial for production environments where quick recovery from problematic deployments is essential.

## Commands

```bash
# Undo last rollout
kubectl rollout undo deployment/myapp

# Rollback to specific revision
kubectl rollout undo deployment/myapp --to-revision=2

# View revision history
kubectl rollout history deployment/myapp

# Check status of last rollback
kubectl rollout status deployment/myapp
```

## Common Mistakes

- **Not testing rollbacks**: Always test rollback procedures.
- **No revision history**: Configure revision history limit.

## What's Next

Continue to [Services Overview](../networking/services/01-clusterip.md)
