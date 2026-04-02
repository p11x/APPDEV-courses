# Security Headers and Dependency Security

## What You'll Learn

- Implementing security headers with Helmet.js
- Dependency vulnerability scanning
- npm audit and Snyk integration
- Supply chain security best practices

## Helmet.js Security Headers

```bash
npm install helmet
```

```javascript
// security-headers.js — Comprehensive security headers

import express from 'express';
import helmet from 'helmet';

const app = express();

// Helmet sets 15+ security headers automatically
app.use(helmet());

// Custom configuration
app.use(helmet({
    contentSecurityPolicy: {
        directives: {
            defaultSrc: ["'self'"],
            scriptSrc: ["'self'", "'nonce-{random}'"],
            styleSrc: ["'self'", "'unsafe-inline'"],
            imgSrc: ["'self'", 'data:', 'https:'],
            connectSrc: ["'self'", 'https://api.example.com'],
            fontSrc: ["'self'", 'https://fonts.gstatic.com'],
            objectSrc: ["'none'"],
            upgradeInsecureRequests: [],
        },
    },
    crossOriginEmbedderPolicy: true,
    crossOriginOpenerPolicy: true,
    crossOriginResourcePolicy: { policy: 'same-origin' },
    hsts: { maxAge: 31536000, includeSubDomains: true, preload: true },
    referrerPolicy: { policy: 'strict-origin-when-cross-origin' },
}));

// Additional headers
app.use((req, res, next) => {
    res.setHeader('Permissions-Policy', 'camera=(), microphone=(), geolocation=()');
    res.setHeader('X-Content-Type-Options', 'nosniff');
    next();
});
```

## Dependency Security

### npm audit

```bash
# Check for vulnerabilities
npm audit

# Auto-fix
npm audit fix

# Only high/critical
npm audit --audit-level=high

# JSON report
npm audit --json > audit-report.json
```

### Snyk Integration

```bash
# Install Snyk CLI
npm install -g snyk

# Authenticate
snyk auth

# Test for vulnerabilities
snyk test

# Monitor project
snyk monitor

# Fix vulnerabilities
snyk fix
```

### GitHub Actions Security Scan

```yaml
name: Security Scan
on: [push, pull_request]
jobs:
    audit:
        runs-on: ubuntu-latest
        steps:
            - uses: actions/checkout@v4
            - uses: actions/setup-node@v4
              with: { node-version: 22 }
            - run: npm ci
            - run: npm audit --audit-level=high
            - name: Snyk scan
              uses: snyk/actions/node@master
              env:
                  SNYK_TOKEN: ${{ secrets.SNYK_TOKEN }}
```

## Best Practices Checklist

- [ ] Use Helmet.js for security headers
- [ ] Run npm audit in CI/CD
- [ ] Enable Dependabot or Renovate for updates
- [ ] Lock dependency versions in production
- [ ] Audit new dependencies before adding
- [ ] Use npm signatures for package verification

## Cross-References

- See [Auth and Rate Limiting](./02-auth-rate-limiting.md) for authentication
- See [OWASP Node.js](./03-owasp-nodejs.md) for vulnerability patterns
- See [Dependency Management](../16-package-management-hands-on/03-dependency-management.md) for updates

## Next Steps

Continue to [Authentication and Rate Limiting](./02-auth-rate-limiting.md) for auth patterns.
