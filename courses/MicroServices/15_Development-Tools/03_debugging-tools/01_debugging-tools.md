# Debugging Tools for Microservices

## Overview

Debugging microservices requires specialized tools due to the distributed nature of the system. This guide covers essential debugging tools and techniques for identifying and resolving issues.

## Debugging Tools

### 1. Logging Tools

- **ELK Stack** (Elasticsearch, Logstash, Kibana)
- **Splunk**
- **Loki** ( Grafana's logging)

### 2. Tracing Tools

- **Jaeger**
- **Zipkin**
- **OpenTelemetry**

### 3. CLI Tools

```bash
# kubectl debug examples
kubectl debug -it pod-name --image=debugger

# Port forwarding for debugging
kubectl port-forward svc/my-service 8080:80

# View logs
kubectl logs -f deployment/my-service
```

## Remote Debugging

```python
# Enable remote debugging
import debugpy

debugpy.listen(("0.0.0.0", 5678))
debugpy.wait_for_client()  # Blocks until debugger attaches
```

## Output

```
Debugging Session:
Service: order-service
Pod: order-service-7d9f8b6c9-abc12
Issues: 3 errors in last hour

Recent Logs:
ERROR: Connection timeout to database
WARN: Retry attempt 2/3
INFO: Processing batch 1534
```
