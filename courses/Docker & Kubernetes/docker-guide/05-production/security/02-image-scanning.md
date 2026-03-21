# Image Scanning

## Overview

Image scanning analyzes Docker images for known vulnerabilities, misconfigurations, and security issues. Regular scanning is essential for production deployments to ensure your images don't contain exploitable vulnerabilities. Docker Scout and other tools provide automated vulnerability detection.

## Prerequisites

- Docker Desktop or Docker Scout CLI
- Understanding of container security

## Core Concepts

### Vulnerability Types

- **Critical**: Immediate security risk
- **High**: Should be fixed soon
- **Medium**: Lower priority
- **Low**: Minor issues

### Scanning Tools

- Docker Scout (built into Docker Desktop)
- Trivy
- Clair
- Snyk

## Step-by-Step Examples

### Using Docker Scout

```bash
# Enable Docker Scout
docker scout enable

# Quick vulnerability summary
docker scout cves myimage:latest

# Full report with fix recommendations
docker scout recommend myimage:latest
```

### Using Trivy

```bash
# Install Trivy
docker run --rm aquasec/trivy image myimage:latest

# Scan with JSON output
docker run --rm aquasec/trivy image --format json myimage:latest

# Scan with policy violations
docker run --rm aquasec/trivy image \
  --security-checks vuln,config myimage:latest
```

### CI/CD Integration

```yaml
# GitHub Actions example
- name: Build and push
  uses: docker/build-push-action@v5
  with:
    push: true
    tags: myapp:latest

- name: Run Trivy
  uses: aquasecurity/trivy-action@master
  with:
    image-ref: 'myapp:latest'
    format: 'sarif'
```

## Common Mistakes

- **Not scanning regularly**: Scan images regularly, not just before deploy.
- **Ignoring low severity**: Lower severity issues can still be exploited.

## Quick Reference

| Tool | Type |
|------|------|
| Docker Scout | Built-in |
| Trivy | Open source |
| Snyk | Commercial |

## What's Next

Continue to [Secrets Management](./03-secrets-management.md)
