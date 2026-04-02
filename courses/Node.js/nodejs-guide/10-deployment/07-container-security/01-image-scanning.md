# Container Security in Production

## What You'll Learn

- Container image security scanning
- Runtime security and hardening
- Network security for containers
- Vulnerability management
- Security monitoring and compliance

## Image Security Scanning

```yaml
# .github/workflows/security-scan.yml
name: Container Security

on:
  push:
    branches: [main]
  pull_request:

jobs:
  trivy-scan:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Build image
        run: docker build -t my-app:scan .

      - name: Run Trivy vulnerability scanner
        uses: aquasecurity/trivy-action@master
        with:
          image-ref: my-app:scan
          format: table
          exit-code: 1
          severity: CRITICAL,HIGH
          ignore-unfixed: true

      - name: Run Trivy (SARIF)
        uses: aquasecurity/trivy-action@master
        with:
          image-ref: my-app:scan
          format: sarif
          output: trivy-results.sarif

      - name: Upload to GitHub Security
        uses: github/codeql-action/upload-sarif@v2
        if: always()
        with:
          sarif_file: trivy-results.sarif

  snyk-scan:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: snyk/actions/docker@master
        with:
          args: --severity-threshold=high
        env:
          SNYK_TOKEN: ${{ secrets.SNYK_TOKEN }}
```

## Hardened Dockerfile

```dockerfile
# Dockerfile — Production hardened
# Build stage
FROM node:20-alpine AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

# Production stage — minimal and hardened
FROM node:20-alpine AS production

# Security: Don't run as root
RUN addgroup -g 1001 -S appgroup && \
    adduser -S appuser -u 1001 -G appgroup

WORKDIR /app

# Copy only production dependencies
COPY package*.json ./
RUN npm ci --only=production && npm cache clean --force

# Copy built application
COPY --from=builder --chown=appuser:appgroup /app/dist ./dist

# Security: Remove unnecessary packages
RUN apk --no-cache add dumb-init && \
    rm -rf /var/cache/apk/* /tmp/*

# Security: Set file permissions
RUN chown -R appuser:appgroup /app && \
    chmod -R 550 /app

USER appuser

# Security: Non-root port
EXPOSE 3000

# Health check
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
    CMD ["wget", "--no-verbose", "--tries=1", "--spider", "http://localhost:3000/health", "||", "exit", "1"]

# Use dumb-init to handle signals properly
ENTRYPOINT ["dumb-init", "--"]
CMD ["node", "dist/index.js"]
```

## Runtime Security

```yaml
# k8s/security-policies.yaml
# Pod Security Standards
apiVersion: v1
kind: Namespace
metadata:
  name: my-app
  labels:
    pod-security.kubernetes.io/enforce: restricted
    pod-security.kubernetes.io/audit: restricted
    pod-security.kubernetes.io/warn: restricted
---
# Network Policy
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: app-network-policy
  namespace: my-app
spec:
  podSelector:
    matchLabels:
      app: node-app
  policyTypes:
    - Ingress
    - Egress
  ingress:
    - from:
        - namespaceSelector:
            matchLabels:
              name: ingress-nginx
      ports:
        - port: 3000
  egress:
    - to:
        - podSelector:
            matchLabels:
              app: postgres
      ports:
        - port: 5432
    - to:
        - podSelector:
            matchLabels:
              app: redis
      ports:
        - port: 6379
    - to: # DNS
        - namespaceSelector: {}
      ports:
        - port: 53
          protocol: UDP
---
# Pod Security Context
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-app
  namespace: my-app
spec:
  template:
    spec:
      securityContext:
        runAsNonRoot: true
        runAsUser: 1001
        runAsGroup: 1001
        fsGroup: 1001
        seccompProfile:
          type: RuntimeDefault
      containers:
        - name: app
          securityContext:
            allowPrivilegeEscalation: false
            readOnlyRootFilesystem: true
            capabilities:
              drop:
                - ALL
          volumeMounts:
            - name: tmp
              mountPath: /tmp
      volumes:
        - name: tmp
          emptyDir:
            sizeLimit: 100Mi
```

## Security Scanning Script

```bash
#!/bin/bash
# scripts/security-scan.sh

echo "=== Security Scan ==="

# 1. Scan dependencies
echo "[1/4] Scanning npm dependencies..."
npm audit --audit-level=high
npm outdated

# 2. Scan Docker image
echo "[2/4] Scanning Docker image..."
docker build -t my-app:security-scan .
trivy image --severity HIGH,CRITICAL my-app:security-scan

# 3. Check for secrets
echo "[3/4] Checking for secrets..."
if command -v gitleaks &> /dev/null; then
    gitleaks detect --source . --report-format json
fi

# 4. Scan Kubernetes manifests
echo "[4/4] Scanning Kubernetes manifests..."
if command -v kubesec &> /dev/null; then
    for f in k8s/*.yaml; do
        kubesec scan "$f"
    done
fi

echo "=== Scan Complete ==="
```

## Best Practices Checklist

- [ ] Scan images in CI/CD pipeline
- [ ] Use minimal base images (Alpine, distroless)
- [ ] Run as non-root user
- [ ] Use read-only filesystem where possible
- [ ] Drop all Linux capabilities
- [ ] Implement network policies
- [ ] Use Pod Security Standards
- [ ] Scan for secrets in repositories
- [ ] Monitor runtime for anomalies

## Cross-References

- See [Docker](../docker/01-dockerfile.md) for container setup
- See [Kubernetes](../03-container-orchestration/01-kubernetes-patterns.md) for K8s security
- See [CI/CD](../05-ci-cd-pipelines/01-github-actions.md) for pipeline integration
- See [Deployment Security](../09-deployment-security/01-security-scanning.md) for overall security

## Next Steps

Continue to [Deployment Monitoring](../08-deployment-monitoring/01-apm-metrics.md).
