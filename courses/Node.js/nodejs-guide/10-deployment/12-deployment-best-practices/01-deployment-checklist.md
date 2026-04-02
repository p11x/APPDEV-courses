# Deployment Best Practices and Operations

## What You'll Learn

- Deployment checklists and procedures
- Blue-green and canary deployments
- Deployment troubleshooting
- Capacity planning
- Cost optimization

## Deployment Checklist

```markdown
# Production Deployment Checklist

## Pre-Deployment
- [ ] All tests passing (unit, integration, e2e)
- [ ] Security scan clean (no critical/high vulnerabilities)
- [ ] Code review approved
- [ ] Database migrations tested
- [ ] Environment variables configured
- [ ] Secrets updated in secrets manager
- [ ] Rollback plan documented
- [ ] Monitoring alerts configured

## Deployment
- [ ] Deploy to staging first
- [ ] Run smoke tests on staging
- [ ] Deploy to production (blue-green/canary)
- [ ] Verify health endpoints
- [ ] Check error rates in monitoring
- [ ] Verify key user flows

## Post-Deployment
- [ ] Monitor for 30 minutes
- [ ] Check application logs for errors
- [ ] Verify database connections
- [ ] Confirm cache warming
- [ ] Update deployment tracking
- [ ] Notify stakeholders
```

## Blue-Green Deployment

```bash
#!/bin/bash
# scripts/blue-green-deploy.sh

set -e

VERSION=$1
ENVIRONMENT=${2:-production}

echo "Deploying $VERSION to $ENVIRONMENT (blue-green)"

# 1. Deploy to green (inactive)
kubectl set image deployment/my-app-green app=my-app:$VERSION -n $ENVIRONMENT
kubectl rollout status deployment/my-app-green -n $ENVIRONMENT --timeout=300s

# 2. Run smoke tests against green
GREEN_URL=$(kubectl get service my-app-green -n $ENVIRONMENT -o jsonpath='{.status.loadBalancer.ingress[0].hostname}')
./scripts/smoke-test.sh "https://$GREEN_URL"

# 3. Switch traffic to green
kubectl patch service my-app -n $ENVIRONMENT -p '{"spec":{"selector":{"version":"green"}}}'

# 4. Monitor for issues (5 minute observation)
echo "Monitoring for 5 minutes..."
sleep 300

ERROR_RATE=$(curl -s "https://$GREEN_URL/metrics" | grep 'http_requests_total.*status="5"' | awk '{print $2}')
if (( $(echo "$ERROR_RATE > 10" | bc -l) )); then
    echo "Error rate too high, rolling back!"
    kubectl patch service my-app -n $ENVIRONMENT -p '{"spec":{"selector":{"version":"blue"}}}'
    exit 1
fi

echo "Deployment successful!"
```

## Canary Deployment

```bash
#!/bin/bash
# scripts/canary-deploy.sh

set -e

VERSION=$1
CANARY_WEIGHT=${2:-10}

echo "Deploying canary $VERSION at ${CANARY_WEIGHT}% traffic"

# 1. Deploy canary version
kubectl apply -f - <<EOF
apiVersion: apps/v1
kind: Deployment
metadata:
  name: my-app-canary
spec:
  replicas: 1
  selector:
    matchLabels:
      app: my-app
      track: canary
  template:
    metadata:
      labels:
        app: my-app
        track: canary
        version: $VERSION
    spec:
      containers:
        - name: app
          image: my-app:$VERSION
EOF

# 2. Update Istio VirtualService for traffic splitting
kubectl apply -f - <<EOF
apiVersion: networking.istio.io/v1beta1
kind: VirtualService
metadata:
  name: my-app
spec:
  hosts:
    - my-app.example.com
  http:
    - route:
        - destination:
            host: my-app
            subset: stable
          weight: $((100 - CANARY_WEIGHT))
        - destination:
            host: my-app
            subset: canary
          weight: $CANARY_WEIGHT
EOF

# 3. Monitor canary
echo "Monitoring canary for 10 minutes..."
for i in $(seq 1 20); do
    CANARY_ERRORS=$(kubectl exec -it prometheus -- wget -qO- "http://localhost:9090/api/v1/query?query=sum(rate(http_requests_total{track=\"canary\",status=~\"5..\"}[1m]))" | jq '.data.result[0].value[1]')

    if (( $(echo "$CANARY_ERRORS > 0.01" | bc -l) )); then
        echo "Canary error rate too high, rolling back!"
        kubectl delete deployment my-app-canary
        exit 1
    fi

    sleep 30
done

echo "Canary healthy, promoting to stable"
```

## Troubleshooting Guide

```
Deployment Troubleshooting:
─────────────────────────────────────────────
Problem: Pod CrashLoopBackOff
Solution:
├── Check logs: kubectl logs pod-name --previous
├── Check events: kubectl describe pod pod-name
├── Check resource limits
└── Verify environment variables

Problem: High Error Rate After Deploy
Solution:
├── Check application logs
├── Verify database connectivity
├── Check if migrations ran
├── Rollback: kubectl rollout undo deployment/my-app

Problem: Slow Response Times
Solution:
├── Check CPU/memory usage
├── Verify cache is working (X-Cache header)
├── Check database query performance
├── Review connection pool utilization

Problem: Memory Leak
Solution:
├── Take heap snapshot: --inspect flag
├── Monitor heap growth over time
├── Check for unclosed connections
├── Review stream handling
```

## Capacity Planning

```
Capacity Planning Formula:
─────────────────────────────────────────────
Requests per second (RPS):
  Peak RPS = Average RPS × Peak Factor (typically 3-5x)

Server capacity:
  Required servers = Peak RPS / Single server RPS × Safety margin (1.5)

Database connections:
  Pool size = Server count × Per-server connections

Memory estimation:
  Total = Heap per instance × Instance count + Redis + DB buffer

Example (1000 avg RPS):
├── Peak RPS: 5000
├── Single server: 500 RPS
├── Required: 5000 / 500 × 1.5 = 15 instances
├── DB connections: 15 × 10 = 150 (configure pool accordingly)
└── Memory: 15 × 256MB + 2GB + 4GB = ~10GB total
```

## Cost Optimization

```
Cost Optimization Strategies:
─────────────────────────────────────────────
Compute:
├── Right-size instances based on actual usage
├── Use spot/preemptible for non-critical workloads
├── Auto-scale based on demand
├── Schedule dev/staging to shut down off-hours
└── Use ARM instances (Graviton) — 20% cheaper

Storage:
├── Use lifecycle policies for S3/GCS
├── Compress old backups
├── Delete unused snapshots
├── Use appropriate storage classes
└── Monitor storage growth

Network:
├── Use CDN for static assets
├── Compress API responses
├── Minimize cross-region traffic
├── Use VPC endpoints for AWS services
└── Cache DNS lookups

Database:
├── Use read replicas for read-heavy workloads
├── Implement connection pooling
├── Right-size database instances
├── Use reserved instances for production
└── Archive old data
```

## Best Practices Checklist

- [ ] Follow deployment checklist for every release
- [ ] Use blue-green or canary for zero-downtime deployments
- [ ] Implement automatic rollback on failure
- [ ] Monitor for 30 minutes after deployment
- [ ] Document troubleshooting procedures
- [ ] Plan capacity based on growth projections
- [ ] Review costs monthly
- [ ] Maintain runbooks for common issues

## Cross-References

- See [CI/CD](../05-ci-cd-pipelines/01-github-actions.md) for deployment automation
- See [Monitoring](../08-deployment-monitoring/01-apm-metrics.md) for observability
- See [Architecture](../01-deployment-architecture/01-architecture-patterns.md) for patterns
- See [Disaster Recovery](../11-disaster-recovery/01-backup-recovery.md) for DR
