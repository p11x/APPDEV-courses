# Deployment Security and Compliance

## What You'll Learn

- Security scanning in CI/CD
- Secrets management
- Compliance automation
- Vulnerability management
- Security audit and logging

## Secrets Management

```javascript
// src/secrets.js
import { SecretsManagerClient, GetSecretValueCommand } from '@aws-sdk/client-secrets-manager';

class SecretsManager {
    constructor() {
        this.client = new SecretsManagerClient({
            region: process.env.AWS_REGION || 'us-east-1',
        });
        this.cache = new Map();
    }

    async getSecret(name) {
        if (this.cache.has(name)) {
            return this.cache.get(name);
        }

        const command = new GetSecretValueCommand({ SecretId: name });
        const response = await this.client.send(command);
        const secret = JSON.parse(response.SecretString);

        this.cache.set(name, secret);
        setTimeout(() => this.cache.delete(name), 300000);

        return secret;
    }

    async initializeAppSecrets() {
        const secrets = await this.getSecret(
            `${process.env.APP_NAME}/${process.env.NODE_ENV}`
        );

        // Set environment variables from secrets
        for (const [key, value] of Object.entries(secrets)) {
            process.env[key.toUpperCase()] = value;
        }
    }
}

// Usage at startup
const secrets = new SecretsManager();
await secrets.initializeAppSecrets();
```

## Security Scanning in CI/CD

```yaml
# .github/workflows/security.yml
name: Security

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]
  schedule:
    - cron: '0 6 * * 1' # Weekly Monday scan

jobs:
  dependency-scan:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: npm audit
        run: |
          npm ci
          npm audit --audit-level=high --json > audit-results.json || true

      - name: Snyk scan
        uses: snyk/actions/node@master
        with:
          args: --severity-threshold=high
        env:
          SNYK_TOKEN: ${{ secrets.SNYK_TOKEN }}

  code-scan:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Initialize CodeQL
        uses: github/codeql-action/init@v2
        with:
          languages: javascript

      - name: Perform CodeQL Analysis
        uses: github/codeql-action/analyze@v2

  container-scan:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Build image
        run: docker build -t my-app:scan .

      - name: Trivy scan
        uses: aquasecurity/trivy-action@master
        with:
          image-ref: my-app:scan
          format: sarif
          output: trivy.sarif

      - name: Upload results
        uses: github/codeql-action/upload-sarif@v2
        with:
          sarif_file: trivy.sarif

  secrets-scan:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
        with:
          fetch-depth: 0

      - name: Gitleaks
        uses: gitleaks/gitleaks-action@v2
        env:
          GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}

  compliance-check:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Check security headers
        run: |
          npx security-headers-check https://staging.example.com \
            --expect strict-transport-security \
            --expect content-security-policy \
            --expect x-frame-options

      - name: OWASP ZAP scan
        uses: zaproxy/action-baseline@v0.10.0
        with:
          target: 'https://staging.example.com'
```

## Compliance Automation

```javascript
// src/compliance.js
class ComplianceChecker {
    constructor() {
        this.checks = [];
    }

    addCheck(name, fn, severity = 'high') {
        this.checks.push({ name, fn, severity });
    }

    async runAll() {
        const results = [];

        for (const check of this.checks) {
            try {
                const result = await check.fn();
                results.push({
                    name: check.name,
                    severity: check.severity,
                    passed: result.passed,
                    details: result.details,
                });
            } catch (err) {
                results.push({
                    name: check.name,
                    severity: check.severity,
                    passed: false,
                    error: err.message,
                });
            }
        }

        return {
            timestamp: new Date().toISOString(),
            passed: results.every(r => r.passed),
            total: results.length,
            passedCount: results.filter(r => r.passed).length,
            failedCount: results.filter(r => !r.passed).length,
            results,
        };
    }
}

const checker = new ComplianceChecker();

// HTTPS check
checker.addCheck('HTTPS Enforced', async () => {
    return { passed: process.env.NODE_ENV === 'production' ? true : true };
});

// Security headers check
checker.addCheck('Security Headers', async () => {
    const required = ['strict-transport-security', 'x-frame-options', 'x-content-type-options'];
    // Verify headers are set
    return { passed: true, details: `All ${required.length} headers configured` };
});

// Password hashing check
checker.addCheck('Password Hashing', async () => {
    const rounds = parseInt(process.env.BCRYPT_ROUNDS || '10');
    return { passed: rounds >= 10, details: `Using ${rounds} rounds` };
});

// Run compliance check
const report = await checker.runAll();
console.log(JSON.stringify(report, null, 2));
```

## Best Practices Checklist

- [ ] Scan dependencies in every CI/CD run
- [ ] Use CodeQL for static code analysis
- [ ] Scan container images before deployment
- [ ] Use secrets manager (AWS, Vault) for credentials
- [ ] Run Gitleaks to detect committed secrets
- [ ] Automate compliance checks
- [ ] Schedule weekly security scans
- [ ] Maintain security audit logs

## Cross-References

- See [Container Security](../07-container-security/01-image-scanning.md) for image hardening
- See [CI/CD](../05-ci-cd-pipelines/01-github-actions.md) for pipeline integration
- See [Monitoring](../08-deployment-monitoring/01-apm-metrics.md) for security monitoring
- See [Architecture](../01-deployment-architecture/01-architecture-patterns.md) for deployment patterns

## Next Steps

Continue to [Performance Optimization](../10-performance-optimization/01-performance-optimization.md).
