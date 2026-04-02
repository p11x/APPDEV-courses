# Compliance, Automation, and Security Hardening

## What You'll Learn

- Security headers and Helmet.js configuration
- TLS/SSL certificate management and automation
- WAF and DDoS protection strategies
- API security hardening (rate limiting, JWT, OAuth2)
- Compliance automation with OPA and Sentinel
- SOC2, PCI-DSS, and GDPR compliance for Node.js
- Security baseline scripts and automated reporting

## Security Headers with Helmet.js

```javascript
// src/middleware/security-headers.js
import helmet from 'helmet';

export function configureSecurityHeaders(app) {
    app.use(helmet());

    // Content Security Policy
    app.use(helmet.contentSecurityPolicy({
        directives: {
            defaultSrc: ["'self'"],
            scriptSrc: ["'self'", "'nonce-{NONCE}'"],
            styleSrc: ["'self'", "'unsafe-inline'", 'https://fonts.googleapis.com'],
            imgSrc: ["'self'", 'data:', 'https://cdn.example.com'],
            connectSrc: ["'self'", 'https://api.example.com'],
            fontSrc: ["'self'", 'https://fonts.gstatic.com'],
            objectSrc: ["'none'"],
            mediaSrc: ["'none'"],
            frameSrc: ["'none'"],
            baseUri: ["'self'"],
            formAction: ["'self'"],
            frameAncestors: ["'none'"],
            upgradeInsecureRequests: [],
        },
        reportOnly: process.env.NODE_ENV !== 'production',
    }));

    // HTTP Strict Transport Security
    app.use(helmet.hsts({
        maxAge: 63072000, // 2 years
        includeSubDomains: true,
        preload: true,
    }));

    // X-Frame-Options
    app.use(helmet.frameguard({ action: 'deny' }));

    // X-Content-Type-Options
    app.use(helmet.noSniff());

    // Referrer Policy
    app.use(helmet.referrerPolicy({ policy: 'strict-origin-when-cross-origin' }));

    // Permissions Policy
    app.use(helmet.permissionsPolicy({
        features: {
            camera: ["'none'"],
            microphone: ["'none'"],
            geolocation: ["'none'"],
            payment: ["'self'"],
        },
    }));

    // Remove X-Powered-By
    app.disable('x-powered-by');
}
```

## CORS Configuration

```javascript
// src/middleware/cors-config.js
import cors from 'cors';

const allowedOrigins = [
    process.env.FRONTEND_URL,
    process.env.ADMIN_URL,
].filter(Boolean);

export const corsOptions = {
    origin(origin, callback) {
        if (!origin || allowedOrigins.includes(origin)) {
            callback(null, true);
        } else {
            callback(new Error('Not allowed by CORS'));
        }
    },
    methods: ['GET', 'POST', 'PUT', 'PATCH', 'DELETE'],
    allowedHeaders: ['Content-Type', 'Authorization', 'X-Request-ID'],
    exposedHeaders: ['X-Request-ID', 'X-RateLimit-Remaining'],
    credentials: true,
    maxAge: 600, // 10 minutes preflight cache
};

export const corsMiddleware = cors(corsOptions);
```

## TLS/SSL Configuration

```javascript
// src/server.js
import https from 'https';
import fs from 'fs';
import path from 'path';

function createSecureServer(app) {
    if (process.env.NODE_ENV === 'production') {
        const options = {
            key: fs.readFileSync(path.resolve(process.env.TLS_KEY_PATH)),
            cert: fs.readFileSync(path.resolve(process.env.TLS_CERT_PATH)),
            ca: process.env.TLS_CA_PATH
                ? fs.readFileSync(path.resolve(process.env.TLS_CA_PATH))
                : undefined,
            minVersion: 'TLSv1.2',
            ciphers: [
                'ECDHE-ECDSA-AES256-GCM-SHA384',
                'ECDHE-RSA-AES256-GCM-SHA384',
                'ECDHE-ECDSA-CHACHA20-POLY1305',
                'ECDHE-RSA-CHACHA20-POLY1305',
                'ECDHE-ECDSA-AES128-GCM-SHA256',
                'ECDHE-RSA-AES128-GCM-SHA256',
            ].join(':'),
            honorCipherOrder: true,
        };

        return https.createServer(options, app);
    }

    // Development fallback
    return app;
}
```

## Cert-Manager and Let's Encrypt Automation

```yaml
# k8s/cert-manager/cluster-issuer.yaml
apiVersion: cert-manager.io/v1
kind: ClusterIssuer
metadata:
  name: letsencrypt-prod
spec:
  acme:
    server: https://acme-v02.api.letsencrypt.org/directory
    email: security@example.com
    privateKeySecretRef:
      name: letsencrypt-prod-key
    solvers:
      - http01:
          ingress:
            class: nginx
      - dns01:
          cloudflare:
            email: admin@example.com
            apiTokenSecretRef:
              name: cloudflare-api-token
              key: api-token
---
# k8s/cert-manager/certificate.yaml
apiVersion: cert-manager.io/v1
kind: Certificate
metadata:
  name: app-tls
  namespace: production
spec:
  secretName: app-tls-secret
  issuerRef:
    name: letsencrypt-prod
    kind: ClusterIssuer
  dnsNames:
    - app.example.com
    - api.example.com
  renewBefore: 360h  # 15 days before expiry
```

## WAF Configuration

```javascript
// src/middleware/waf.js
class WebApplicationFirewall {
    constructor() {
        this.rules = [];
        this.blockedIPs = new Set();
        this.rateLimitBuckets = new Map();
    }

    addRule(name, pattern, action = 'block') {
        this.rules.push({ name, pattern: new RegExp(pattern, 'i'), action });
    }

    initDefaultRules() {
        // SQL injection patterns
        this.addRule('SQLi-1', "(\\b(union|select|insert|update|delete|drop|alter)\\b.*\\b(from|into|table)\\b)");
        this.addRule('SQLi-2', "('.*or.*'.*=.*')");

        // XSS patterns
        this.addRule('XSS-1', '<script[^>]*>');
        this.addRule('XSS-2', 'javascript:');
        this.addRule('XSS-3', 'on(error|load|click|mouseover)\\s*=');

        // Path traversal
        this.addRule('PathTraversal', '(\\.\\.)(/|\\\\|%2f|%5c)');

        // Command injection
        this.addRule('CmdInjection', '(;|\\||&&|\\$\\(|`)(ls|cat|wget|curl|bash|sh)');
    }

    middleware() {
        return (req, res, next) => {
            // IP blocklist
            if (this.blockedIPs.has(req.ip)) {
                return res.status(403).json({ error: 'Forbidden' });
            }

            // Inspect request body, query, headers
            const targets = [
                JSON.stringify(req.body || {}),
                JSON.stringify(req.query || {}),
                JSON.stringify(req.params || {}),
                req.get('user-agent') || '',
                req.get('referer') || '',
            ];

            for (const target of targets) {
                for (const rule of this.rules) {
                    if (rule.pattern.test(target)) {
                        console.warn(`WAF: Blocked request matching rule "${rule.name}" from ${req.ip}`);
                        return res.status(403).json({ error: 'Request blocked by WAF' });
                    }
                }
            }

            next();
        };
    }
}

const waf = new WebApplicationFirewall();
waf.initDefaultRules();
export default waf;
```

### Cloudflare WAF Rules (Terraform)

```hcl
# terraform/cloudflare-waf.tf
resource "cloudflare_ruleset" "waf_custom" {
  zone_id = var.cloudflare_zone_id
  name    = "Custom WAF Rules"
  kind    = "zone"
  phase   = "http_request_firewall_custom"

  rules {
    action     = "block"
    expression  = "(http.request.uri.query contains \"../\")"
    description = "Block path traversal"
    enabled    = true
  }

  rules {
    action     = "challenge"
    expression = "(cf.bot_management.score lt 30)"
    description = "Challenge suspicious bots"
    enabled    = true
  }

  rules {
    action     = "managed_challenge"
    expression = "(not cf.client.bot) and (cf.threat_score gt 14)"
    description = "Challenge high threat score requests"
    enabled    = true
  }
}
```

## DDoS Protection Strategies

```javascript
// src/middleware/ddos-protection.js
import rateLimit from 'express-rate-limit';
import slowDown from 'express-slow-down';

// Tiered rate limiting
export const globalRateLimit = rateLimit({
    windowMs: 15 * 60 * 1000,
    max: 1000,
    standardHeaders: true,
    legacyHeaders: false,
    handler: (req, res) => {
        res.status(429).json({
            error: 'Too many requests',
            retryAfter: Math.ceil(req.rateLimit.resetTime / 1000),
        });
    },
});

export const apiRateLimit = rateLimit({
    windowMs: 1 * 60 * 1000,
    max: 100,
    keyGenerator: (req) => req.user?.id || req.ip,
});

export const authRateLimit = rateLimit({
    windowMs: 15 * 60 * 1000,
    max: 10,
    skipSuccessfulRequests: true,
    keyGenerator: (req) => req.ip,
});

// Speed limiter (progressive slowdown)
export const speedLimiter = slowDown({
    windowMs: 15 * 60 * 1000,
    delayAfter: 50,
    delayMs: (hits) => hits * 100,
});
```

## API Security: JWT Best Practices

```javascript
// src/auth/jwt-service.js
import jwt from 'jsonwebtoken';
import crypto from 'crypto';

class JWTService {
    constructor() {
        this.accessSecret = process.env.JWT_ACCESS_SECRET;
        this.refreshSecret = process.env.JWT_REFRESH_SECRET;
        this.accessExpiry = '15m';
        this.refreshExpiry = '7d';
        this.issuer = process.env.APP_NAME;
    }

    generateTokenPair(userId, roles = []) {
        const jti = crypto.randomUUID();

        const accessToken = jwt.sign(
            { sub: userId, roles, type: 'access' },
            this.accessSecret,
            {
                expiresIn: this.accessExpiry,
                issuer: this.issuer,
                audience: 'api',
                jwtid: jti,
                algorithm: 'RS256',
            }
        );

        const refreshToken = jwt.sign(
            { sub: userId, type: 'refresh', jti },
            this.refreshSecret,
            {
                expiresIn: this.refreshExpiry,
                issuer: this.issuer,
                jwtid: crypto.randomUUID(),
                algorithm: 'RS256',
            }
        );

        return { accessToken, refreshToken, expiresIn: 900 };
    }

    verifyAccessToken(token) {
        return jwt.verify(token, this.accessSecret, {
            issuer: this.issuer,
            audience: 'api',
            algorithms: ['RS256'],
        });
    }

    verifyRefreshToken(token) {
        return jwt.verify(token, this.refreshSecret, {
            issuer: this.issuer,
            algorithms: ['RS256'],
        });
    }
}

export default new JWTService();
```

## OAuth2 Implementation

```javascript
// src/auth/oauth2.js
import { Issuer, generators } from 'openid-client';

class OAuth2Provider {
    constructor(config) {
        this.config = config;
    }

    async initialize() {
        const issuer = await Issuer.discover(this.config.discoveryUrl);
        this.client = new issuer.Client({
            client_id: this.config.clientId,
            client_secret: this.config.clientSecret,
            redirect_uris: [this.config.redirectUri],
            response_types: ['code'],
        });
    }

    getAuthorizationUrl(state) {
        const codeVerifier = generators.codeVerifier();
        const codeChallenge = generators.codeChallenge(codeVerifier);

        const url = this.client.authorizationUrl({
            scope: 'openid profile email',
            code_challenge: codeChallenge,
            code_challenge_method: 'S256',
            state,
        });

        return { url, codeVerifier };
    }

    async handleCallback(params, codeVerifier) {
        const tokenSet = await this.client.callback(
            this.config.redirectUri,
            params,
            { code_verifier: codeVerifier }
        );

        const userInfo = await this.client.userinfo(tokenSet.access_token);

        return {
            accessToken: tokenSet.access_token,
            refreshToken: tokenSet.refresh_token,
            idToken: tokenSet.id_token,
            userInfo,
            expiresAt: tokenSet.expires_at,
        };
    }
}

export default OAuth2Provider;
```

## Open Policy Agent (OPA) Integration

```rego
# policies/deployment.rego
package deployment

default allow = false

# Deny images without digest pinning
deny[msg] {
    input.kind == "Deployment"
    container := input.spec.template.spec.containers[_]
    not contains(container.image, "@sha256:")
    not contains(container.image, ":v")
    msg := sprintf("Container image '%s' must be pinned by digest or tag", [container.image])
}

# Deny containers running as root
deny[msg] {
    input.kind == "Deployment"
    container := input.spec.template.spec.containers[_]
    not container.securityContext.runAsNonRoot
    msg := sprintf("Container '%s' must run as non-root", [container.name])
}

# Require resource limits
deny[msg] {
    input.kind == "Deployment"
    container := input.spec.template.spec.containers[_]
    not container.resources.limits.memory
    msg := sprintf("Container '%s' must have memory limits", [container.name])
}

deny[msg] {
    input.kind == "Deployment"
    container := input.spec.template.spec.containers[_]
    not container.resources.limits.cpu
    msg := sprintf("Container '%s' must have CPU limits", [container.name])
}

# Deny privileged containers
deny[msg] {
    input.kind == "Deployment"
    container := input.spec.template.spec.containers[_]
    container.securityContext.privileged == true
    msg := sprintf("Container '%s' must not be privileged", [container.name])
}

# Require readiness and liveness probes
deny[msg] {
    input.kind == "Deployment"
    container := input.spec.template.spec.containers[_]
    not container.readinessProbe
    msg := sprintf("Container '%s' must have a readiness probe", [container.name])
}
```

```yaml
# .github/workflows/opa-gatekeeper.yml
name: OPA Policy Check
on: [pull_request]

jobs:
  opa-check:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Install OPA
        run: |
          curl -L -o opa https://openpolicyagent.org/downloads/latest/opa_linux_amd64
          chmod +x opa && sudo mv opa /usr/local/bin/

      - name: Validate policies
        run: opa test policies/ -v

      - name: Check manifests against policies
        run: |
          for file in k8s/**/*.yaml; do
            opa eval -d policies/deployment.rego \
              -i "$file" \
              "data.deployment.deny" \
              --format pretty
          done
```

## SOC2 Compliance Checklist

```javascript
// src/compliance/soc2-checklist.js
class SOC2ComplianceChecker {
    constructor() {
        this.categories = {
            CC6: 'Logical and Physical Access Controls',
            CC7: 'System Operations',
            CC8: 'Change Management',
            CC9: 'Risk Mitigation',
            A1:  'Availability',
            C1:  'Confidentiality',
        };
    }

    async runFullAudit() {
        return {
            timestamp: new Date().toISOString(),
            environment: process.env.NODE_ENV,
            checks: {
                accessControl: await this.checkAccessControls(),
                encryption: await this.checkEncryption(),
                logging: await this.checkAuditLogging(),
                changeManagement: await this.checkChangeManagement(),
                availability: await this.checkAvailability(),
                incidentResponse: await this.checkIncidentResponse(),
                vulnerabilityManagement: await this.checkVulnerabilityManagement(),
            },
        };
    }

    async checkAccessControls() {
        return {
            category: 'CC6.1',
            items: [
                {
                    name: 'Multi-factor authentication enabled',
                    passed: !!process.env.MFA_ENABLED,
                },
                {
                    name: 'Role-based access control implemented',
                    passed: !!process.env.RBAC_ENABLED,
                },
                {
                    name: 'Session timeout configured',
                    passed: parseInt(process.env.SESSION_TIMEOUT || '0') <= 1800,
                },
                {
                    name: 'Failed login lockout enabled',
                    passed: !!process.env.ACCOUNT_LOCKOUT_ENABLED,
                },
                {
                    name: 'Password complexity enforced',
                    passed: parseInt(process.env.MIN_PASSWORD_LENGTH || '0') >= 12,
                },
            ],
        };
    }

    async checkEncryption() {
        return {
            category: 'CC6.7',
            items: [
                {
                    name: 'TLS 1.2+ enforced',
                    passed: process.env.TLS_MIN_VERSION === 'TLSv1.2',
                },
                {
                    name: 'Data at rest encrypted',
                    passed: !!process.env.ENCRYPTION_AT_REST,
                },
                {
                    name: 'Secrets in managed vault',
                    passed: ['aws-secrets', 'vault', 'gcp-secret-manager']
                        .includes(process.env.SECRETS_PROVIDER),
                },
                {
                    name: 'Database connections encrypted',
                    passed: process.env.DB_SSL === 'true',
                },
            ],
        };
    }

    async checkAuditLogging() {
        return {
            category: 'CC7.2',
            items: [
                {
                    name: 'Authentication events logged',
                    passed: !!process.env.LOG_AUTH_EVENTS,
                },
                {
                    name: 'Data access events logged',
                    passed: !!process.env.LOG_DATA_ACCESS,
                },
                {
                    name: 'Admin actions logged',
                    passed: !!process.env.LOG_ADMIN_ACTIONS,
                },
                {
                    name: 'Log tampering protection',
                    passed: !!process.env.LOG_INTEGRITY_CHECK,
                },
                {
                    name: 'Log retention >= 1 year',
                    passed: parseInt(process.env.LOG_RETENTION_DAYS || '0') >= 365,
                },
            ],
        };
    }

    async checkChangeManagement() {
        return {
            category: 'CC8.1',
            items: [
                {
                    name: 'Code review required before merge',
                    passed: !!process.env.REQUIRE_PR_REVIEW,
                },
                {
                    name: 'CI/CD pipeline with security gates',
                    passed: !!process.env.SECURITY_GATES_ENABLED,
                },
                {
                    name: 'Rollback mechanism in place',
                    passed: !!process.env.CANARY_DEPLOYMENT,
                },
            ],
        };
    }

    async checkAvailability() {
        return {
            category: 'A1.1',
            items: [
                {
                    name: 'Health checks configured',
                    passed: !!process.env.HEALTH_CHECK_ENDPOINT,
                },
                {
                    name: 'Auto-scaling enabled',
                    passed: !!process.env.AUTO_SCALING,
                },
                {
                    name: 'Backup and recovery tested',
                    passed: !!process.env.BACKUP_TESTED,
                },
                {
                    name: 'Multi-AZ deployment',
                    passed: !!process.env.MULTI_AZ,
                },
            ],
        };
    }

    async checkIncidentResponse() {
        return {
            category: 'CC7.3',
            items: [
                {
                    name: 'Incident response plan documented',
                    passed: !!process.env.IRP_DOCUMENTED,
                },
                {
                    name: 'Security alerting configured',
                    passed: !!process.env.SECURITY_ALERTS,
                },
                {
                    name: 'On-call rotation established',
                    passed: !!process.env.ON_CALL_ROTATION,
                },
            ],
        };
    }

    async checkVulnerabilityManagement() {
        return {
            category: 'CC7.1',
            items: [
                {
                    name: 'Dependency scanning in CI/CD',
                    passed: !!process.env.DEPENDENCY_SCAN_ENABLED,
                },
                {
                    name: 'Container image scanning',
                    passed: !!process.env.IMAGE_SCAN_ENABLED,
                },
                {
                    name: 'SAST/DAST in pipeline',
                    passed: !!process.env.SAST_DAST_ENABLED,
                },
                {
                    name: 'Critical vulns patched within 48h',
                    passed: !!process.env.VULN_SLA_48H,
                },
            ],
        };
    }
}

export default SOC2ComplianceChecker;
```

## PCI-DSS Considerations

```javascript
// src/compliance/pci-dss.js
export const pciDSSMiddleware = {
    // PCI-DSS Requirement 6.5: Secure coding practices
    inputValidation(req, res, next) {
        const sanitize = (obj) => {
            if (typeof obj !== 'object' || obj === null) return obj;
            for (const key of Object.keys(obj)) {
                if (typeof obj[key] === 'string') {
                    obj[key] = obj[key]
                        .replace(/[<>]/g, '')
                        .replace(/javascript:/gi, '')
                        .trim();
                } else if (typeof obj[key] === 'object') {
                    sanitize(obj[key]);
                }
            }
            return obj;
        };

        if (req.body) sanitize(req.body);
        if (req.query) sanitize(req.query);
        next();
    },

    // PCI-DSS Requirement 3.4: Protect stored cardholder data
    cardDataProtection(req, res, next) {
        const originalJson = res.json.bind(res);
        res.json = (data) => {
            const sanitized = JSON.parse(JSON.stringify(data, (key, value) => {
                if (/card|pan|cvv|cvc|expir/i.test(key)) return '[REDACTED]';
                return value;
            }));
            return originalJson(sanitized);
        };
        next();
    },

    // PCI-DSS Requirement 10: Track and monitor access
    accessLogging(req, res, next) {
        const start = Date.now();
        res.on('finish', () => {
            const log = {
                timestamp: new Date().toISOString(),
                method: req.method,
                path: req.path,
                statusCode: res.statusCode,
                duration: Date.now() - start,
                userId: req.user?.id || 'anonymous',
                ip: req.ip,
                userAgent: req.get('user-agent'),
            };
            console.log(JSON.stringify({ event: 'pci_access', ...log }));
        });
        next();
    },
};
```

## GDPR Data Protection in Deployment

```javascript
// src/compliance/gdpr.js
class GDPRDataProtection {
    constructor(db) {
        this.db = db;
    }

    // Article 17: Right to erasure
    async deleteUserData(userId) {
        const collections = ['users', 'sessions', 'audit_logs', 'preferences'];
        const results = {};

        for (const collection of collections) {
            results[collection] = await this.db.collection(collection)
                .deleteMany({ userId });
        }

        await this.db.collection('deletion_log').insertOne({
            userId,
            deletedAt: new Date(),
            collections,
            gdprArticle: '17',
        });

        return results;
    }

    // Article 20: Right to data portability
    async exportUserData(userId) {
        const userData = await this.db.collection('users')
            .findOne({ _id: userId });

        const sessions = await this.db.collection('sessions')
            .find({ userId }).toArray();

        return {
            exportDate: new Date().toISOString(),
            format: 'JSON',
            data: {
                profile: userData,
                sessions: sessions.map(s => ({
                    createdAt: s.createdAt,
                    lastActive: s.lastActive,
                })),
            },
        };
    }

    // Article 33: Breach notification (72h requirement)
    async handleDataBreach(incident) {
        const notification = {
            breachDetectedAt: new Date().toISOString(),
            notifyBy: new Date(Date.now() + 72 * 60 * 60 * 1000).toISOString(),
            affectedUsers: incident.affectedUserIds,
            dataCategories: incident.dataCategories,
            severity: incident.severity,
            status: 'investigating',
        };

        await this.db.collection('breach_notifications').insertOne(notification);

        // Trigger alerting pipeline
        if (incident.severity === 'high') {
            await this.notifyDataProtectionOfficer(notification);
            await this.notifyAffectedUsers(notification);
        }

        return notification;
    }

    async notifyDataProtectionOfficer(notification) {
        console.log('DPO notification:', JSON.stringify(notification));
    }

    async notifyAffectedUsers(notification) {
        console.log('User notification queued:', notification.affectedUsers.length, 'users');
    }
}

export default GDPRDataProtection;
```

## Security Baseline Configuration Script

```bash
#!/bin/bash
# scripts/security-baseline.sh

set -euo pipefail

echo "=== Node.js Security Baseline Check ==="

# Check Node.js version
NODE_VERSION=$(node -v)
echo "Node.js version: $NODE_VERSION"
if [[ "$NODE_VERSION" < "v18" ]]; then
    echo "WARNING: Node.js version should be >= 18 LTS"
fi

# Check for known vulnerabilities
echo "Running npm audit..."
npm audit --audit-level=high

# Check for outdated packages
echo "Checking for outdated packages..."
npm outdated --long || true

# Verify lockfile integrity
if [ ! -f "package-lock.json" ]; then
    echo "ERROR: package-lock.json not found"
    exit 1
fi

# Check environment configuration
echo "Checking environment security..."
REQUIRED_VARS=(
    "NODE_ENV"
    "JWT_ACCESS_SECRET"
    "DB_CONNECTION_STRING"
    "ENCRYPTION_KEY"
)

for var in "${REQUIRED_VARS[@]}"; do
    if [ -z "${!var:-}" ]; then
        echo "WARNING: $var is not set"
    fi
done

# Verify file permissions
echo "Checking file permissions..."
find . -name "*.env*" -exec chmod 600 {} \; 2>/dev/null || true
find . -name "*.key" -exec chmod 600 {} \; 2>/dev/null || true
find . -name "*.pem" -exec chmod 600 {} \; 2>/dev/null || true

# Check for secrets in source
echo "Scanning for hardcoded secrets..."
if command -v gitleaks &> /dev/null; then
    gitleaks detect --source . --report-path gitleaks-report.json
fi

echo "=== Security Baseline Check Complete ==="
```

## Automated Compliance Reporting

```javascript
// src/compliance/report-generator.js
import fs from 'fs/promises';
import SOC2ComplianceChecker from './soc2-checklist.js';

class ComplianceReportGenerator {
    constructor() {
        this.soc2Checker = new SOC2ComplianceChecker();
    }

    async generateReport() {
        const soc2Audit = await this.soc2Checker.runFullAudit();

        const report = {
            metadata: {
                generatedAt: new Date().toISOString(),
                reportVersion: '1.0',
                environment: process.env.NODE_ENV,
                application: process.env.APP_NAME,
            },
            summary: this.computeSummary(soc2Audit),
            soc2: soc2Audit,
            recommendations: this.generateRecommendations(soc2Audit),
        };

        const filename = `compliance-report-${new Date().toISOString().split('T')[0]}.json`;
        await fs.writeFile(filename, JSON.stringify(report, null, 2));

        return { report, filename };
    }

    computeSummary(audit) {
        let total = 0;
        let passed = 0;

        for (const category of Object.values(audit.checks)) {
            for (const item of category.items) {
                total++;
                if (item.passed) passed++;
            }
        }

        return {
            totalChecks: total,
            passed,
            failed: total - passed,
            complianceRate: `${((passed / total) * 100).toFixed(1)}%`,
            status: passed === total ? 'COMPLIANT' : 'NON_COMPLIANT',
        };
    }

    generateRecommendations(audit) {
        const recommendations = [];

        for (const [checkName, category] of Object.entries(audit.checks)) {
            for (const item of category.items) {
                if (!item.passed) {
                    recommendations.push({
                        category: checkName,
                        requirement: category.category,
                        finding: item.name,
                        severity: 'high',
                        remediation: `Implement: ${item.name}`,
                    });
                }
            }
        }

        return recommendations;
    }
}

export default ComplianceReportGenerator;
```

## Best Practices Checklist

- [ ] Helmet.js configured with strict CSP and HSTS
- [ ] CORS restricted to known origins with credentials handling
- [ ] TLS 1.2+ enforced with strong cipher suites
- [ ] Cert-manager automating Let's Encrypt certificate renewal
- [ ] WAF rules for SQLi, XSS, and path traversal
- [ ] Tiered rate limiting on all API endpoints
- [ ] JWT tokens using RS256 with short expiry
- [ ] OAuth2 with PKCE for authorization flows
- [ ] OPA policies validated in CI/CD pipeline
- [ ] SOC2 controls automated and auditable
- [ ] PCI-DSS cardholder data redacted in logs
- [ ] GDPR data deletion and export mechanisms tested
- [ ] Security baseline script runs in pre-deploy hook

## Cross-References

- See [Security Scanning](01-security-scanning.md) for CI/CD scanning integration
- See [Vulnerability & Incident Response](03-vulnerability-incident-response.md) for incident procedures
- See [Container Security](../07-container-security/01-image-scanning.md) for image hardening
- See [CI/CD Pipelines](../05-ci-cd-pipelines/01-github-actions.md) for pipeline integration
- See [Monitoring](../08-deployment-monitoring/01-apm-metrics.md) for security alerting

## Next Steps

Continue to [Vulnerability & Incident Response](03-vulnerability-incident-response.md).
