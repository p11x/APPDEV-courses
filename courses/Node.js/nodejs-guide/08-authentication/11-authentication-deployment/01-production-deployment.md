# Authentication Deployment and Operations

## What You'll Learn

- Production deployment best practices
- Environment configuration management
- Health checks and status endpoints
- Blue-green deployment for auth services
- Disaster recovery and backup

## Environment Configuration

```javascript
// config/auth.config.js
export default {
    jwt: {
        accessSecret: process.env.JWT_ACCESS_SECRET,
        refreshSecret: process.env.JWT_REFRESH_SECRET,
        accessExpiry: process.env.JWT_ACCESS_EXPIRY || '15m',
        refreshExpiry: process.env.JWT_REFRESH_EXPIRY || '7d',
        algorithm: process.env.JWT_ALGORITHM || 'HS256',
        issuer: process.env.JWT_ISSUER || 'myapp',
    },
    bcrypt: {
        saltRounds: parseInt(process.env.BCRYPT_ROUNDS || '12'),
    },
    session: {
        secret: process.env.SESSION_SECRET,
        maxAge: parseInt(process.env.SESSION_MAX_AGE || '1800000'), // 30 min
        secure: process.env.NODE_ENV === 'production',
    },
    rateLimit: {
        loginWindow: parseInt(process.env.RATE_LIMIT_WINDOW || '900000'), // 15 min
        loginMax: parseInt(process.env.RATE_LIMIT_MAX || '5'),
        apiWindow: parseInt(process.env.API_RATE_WINDOW || '60000'), // 1 min
        apiMax: parseInt(process.env.API_RATE_MAX || '100'),
    },
    cors: {
        origin: process.env.CORS_ORIGIN || 'https://example.com',
    },
    redis: {
        url: process.env.REDIS_URL,
    },
};

// .env.example
// JWT_ACCESS_SECRET=generate-64-char-random-string
// JWT_REFRESH_SECRET=generate-another-64-char-random-string
// JWT_ACCESS_EXPIRY=15m
// JWT_REFRESH_EXPIRY=7d
// SESSION_SECRET=generate-64-char-random-string
// BCRYPT_ROUNDS=12
// REDIS_URL=redis://localhost:6379
// CORS_ORIGIN=https://yourdomain.com
// NODE_ENV=production
```

## Health Checks

```javascript
import { Router } from 'express';

const healthRouter = Router();

// Basic health check
healthRouter.get('/health', (req, res) => {
    res.json({ status: 'ok', timestamp: new Date().toISOString() });
});

// Detailed health check
healthRouter.get('/health/detailed', async (req, res) => {
    const checks = {};

    // Database check
    try {
        await pool.query('SELECT 1');
        checks.database = { status: 'ok' };
    } catch (err) {
        checks.database = { status: 'error', message: err.message };
    }

    // Redis check
    try {
        await redis.ping();
        checks.redis = { status: 'ok' };
    } catch (err) {
        checks.redis = { status: 'error', message: err.message };
    }

    // JWT signing check
    try {
        const testToken = jwt.sign({ test: true }, process.env.JWT_ACCESS_SECRET, { expiresIn: '1s' });
        jwt.verify(testToken, process.env.JWT_ACCESS_SECRET);
        checks.jwt = { status: 'ok' };
    } catch (err) {
        checks.jwt = { status: 'error', message: err.message };
    }

    const healthy = Object.values(checks).every(c => c.status === 'ok');

    res.status(healthy ? 200 : 503).json({
        status: healthy ? 'healthy' : 'degraded',
        checks,
        timestamp: new Date().toISOString(),
    });
});

// Readiness probe (Kubernetes)
healthRouter.get('/ready', async (req, res) => {
    try {
        await pool.query('SELECT 1');
        await redis.ping();
        res.json({ ready: true });
    } catch {
        res.status(503).json({ ready: false });
    }
});

// Liveness probe (Kubernetes)
healthRouter.get('/alive', (req, res) => {
    res.json({ alive: true });
});
```

## Blue-Green Deployment

```javascript
// auth-service-deployment.yaml (Kubernetes)
const deploymentConfig = `
apiVersion: apps/v1
kind: Deployment
metadata:
  name: auth-service
spec:
  replicas: 3
  strategy:
    type: RollingUpdate
    rollingUpdate:
      maxSurge: 1
      maxUnavailable: 0
  selector:
    matchLabels:
      app: auth-service
  template:
    metadata:
      labels:
        app: auth-service
        version: "\${VERSION}"
    spec:
      containers:
      - name: auth
        image: myapp/auth-service:\${VERSION}
        ports:
        - containerPort: 3000
        env:
        - name: JWT_ACCESS_SECRET
          valueFrom:
            secretKeyRef:
              name: auth-secrets
              key: jwt-access-secret
        - name: REDIS_URL
          valueFrom:
            secretKeyRef:
              name: auth-secrets
              key: redis-url
        livenessProbe:
          httpGet:
            path: /health/alive
            port: 3000
          initialDelaySeconds: 10
          periodSeconds: 30
        readinessProbe:
          httpGet:
            path: /health/ready
            port: 3000
          initialDelaySeconds: 5
          periodSeconds: 10
        resources:
          requests:
            memory: "128Mi"
            cpu: "100m"
          limits:
            memory: "256Mi"
            cpu: "500m"
`;
```

## Secret Management

```javascript
import { SecretsManagerClient, GetSecretValueCommand } from '@aws-sdk/client-secrets-manager';

class SecretManager {
    constructor() {
        this.client = new SecretsManagerClient({ region: process.env.AWS_REGION });
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
        setTimeout(() => this.cache.delete(name), 300000); // 5 min cache

        return secret;
    }

    async initializeAuthSecrets() {
        const secrets = await this.getSecret('prod/auth-service');

        process.env.JWT_ACCESS_SECRET = secrets.jwtAccessSecret;
        process.env.JWT_REFRESH_SECRET = secrets.jwtRefreshSecret;
        process.env.SESSION_SECRET = secrets.sessionSecret;
        process.env.REDIS_URL = secrets.redisUrl;
    }
}
```

## Best Practices Checklist

- [ ] Use environment variables for all secrets
- [ ] Never commit secrets to version control
- [ ] Implement health checks (liveness + readiness)
- [ ] Use rolling deployments with zero downtime
- [ ] Set resource limits for auth containers
- [ ] Use a secrets manager (AWS, Vault, etc.)
- [ ] Implement graceful shutdown
- [ ] Monitor auth service availability

## Cross-References

- See [Monitoring](../10-authentication-monitoring/01-monitoring-metrics.md) for observability
- See [Security](../06-authentication-security/01-security-headers.md) for hardening
- See [Performance](../08-authentication-performance/01-performance-optimization.md) for optimization

## Next Steps

Continue to [Future Trends](../12-authentication-future-trends/01-webauthn-decentralized.md).
