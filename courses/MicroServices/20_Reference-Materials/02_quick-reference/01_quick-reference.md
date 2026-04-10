# Quick Reference Cards

## Overview

Quick reference cards provide concise, at-a-glance information for common microservices tasks and patterns. These are ideal for daily reference during development.

## API Design Quick Reference

### HTTP Methods
- `GET` - Retrieve resource
- `POST` - Create new resource
- `PUT` - Update (full) resource
- `PATCH` - Partial update
- `DELETE` - Remove resource

### Status Codes
```
2xx Success
  200 OK
  201 Created
  204 No Content

4xx Client Error
  400 Bad Request
  401 Unauthorized
  404 Not Found
  429 Too Many Requests

5xx Server Error
  500 Internal Error
  502 Bad Gateway
  503 Unavailable
```

## Kubernetes Quick Reference

```bash
# Deploy
kubectl apply -f deployment.yaml

# Scale
kubectl scale deployment my-app --replicas=3

# Check pods
kubectl get pods -l app=my-app

# Logs
kubectl logs -f pod/my-app-abc123

# Port forward
kubectl port-forward svc/my-app 8080:80

# Rollback
kubectl rollout undo deployment/my-app
```

## Docker Quick Reference

```bash
# Build
docker build -t my-service:1.0 .

# Run
docker run -d -p 8080:5000 my-service:1.0

# List
docker ps

# Logs
docker logs container_id

# Compose
docker-compose up -d
```

## Output

```
Quick Reference Cards: 8
- API Design
- Kubernetes
- Docker
- Networking
- Security
- Monitoring
- Testing
- Deployment
```
