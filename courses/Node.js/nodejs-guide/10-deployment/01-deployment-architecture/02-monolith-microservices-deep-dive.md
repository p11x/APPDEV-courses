# Monolith to Microservices Deployment Deep Dive

## What You'll Learn

- Monolith vertical and horizontal scaling with PM2
- Zero-downtime deployment strategies
- Nginx reverse proxy with load balancing and sticky sessions
- Modular monolith architecture for migration readiness
- Microservices decomposition patterns
- API gateway implementation (Kong, custom Express gateway)
- Service registry and discovery (Consul, etcd)
- Inter-service communication (REST, gRPC, message queues)
- Strangler fig migration pattern
- Database per service with saga orchestration/choreography
- Docker Compose for microservices

## Architecture Overview

```
Monolith Scaling Path:
──────────────────────────────────────────────────────────────
  Single Process → PM2 Cluster → Nginx LB → Modular Monolith
       ↓               ↓            ↓              ↓
  Dev/Proto      Production     Enterprise    Migration Ready
──────────────────────────────────────────────────────────────

Microservices Architecture:
──────────────────────────────────────────────────────────────────────
                    ┌─────────────┐
  Clients ────────▶│ API Gateway │
                    └──────┬──────┘
              ┌────────────┼────────────┐
              ▼            ▼            ▼
        ┌──────────┐ ┌──────────┐ ┌──────────┐
        │  User    │ │  Order   │ │ Payment  │
        │ Service  │ │ Service  │ │ Service  │
        └────┬─────┘ └────┬─────┘ └────┬─────┘
             │            │            │
        ┌────▼─────┐ ┌────▼─────┐ ┌────▼─────┐
        │ User DB  │ │ Order DB │ │ Pay DB   │
        └──────────┘ └──────────┘ └──────────┘
              │            │            │
              └────────────┼────────────┘
                           ▼
                    ┌──────────────┐
                    │ Message Bus  │
                    │ (RabbitMQ/   │
                    │  Kafka/NATS) │
                    └──────────────┘
──────────────────────────────────────────────────────────────────────
```

---

## Part 1: Monolith Deployment Strategies

### 1.1 Vertical Scaling with PM2 Cluster Mode

PM2 cluster mode enables a Node.js application to utilize all available CPU cores by spawning multiple worker processes that share the same port.

```javascript
// ecosystem.config.js — PM2 cluster configuration
module.exports = {
    apps: [
        {
            name: 'my-monolith',
            script: './dist/server.js',
            instances: 'max',              // Use all available CPUs
            exec_mode: 'cluster',          // Enable cluster mode
            watch: false,
            max_memory_restart: '1G',

            // Environment-specific configs
            env_development: {
                NODE_ENV: 'development',
                PORT: 3000,
                LOG_LEVEL: 'debug',
            },
            env_staging: {
                NODE_ENV: 'staging',
                PORT: 3000,
                LOG_LEVEL: 'info',
            },
            env_production: {
                NODE_ENV: 'production',
                PORT: 3000,
                LOG_LEVEL: 'warn',
            },

            // Logging
            log_date_format: 'YYYY-MM-DD HH:mm:ss Z',
            error_file: '/var/log/pm2/my-app-error.log',
            out_file: '/var/log/pm2/my-app-out.log',
            merge_logs: true,

            // Advanced clustering
            instance_var: 'INSTANCE_ID',
            kill_timeout: 5000,
            listen_timeout: 8000,
            shutdown_with_message: true,

            // Graceful restart settings
            exp_backoff_restart_delay: 100,
            max_restarts: 10,
            min_uptime: '10s',

            // Auto-restart on file changes (dev only)
            // watch: ['src', 'config'],
            // watch_delay: 1000,
            // ignore_watch: ['node_modules', 'logs'],
        },
    ],

    // Deployment configuration
    deploy: {
        production: {
            user: 'deploy',
            host: ['app1.example.com', 'app2.example.com'],
            ref: 'origin/main',
            repo: 'git@github.com:org/my-app.git',
            path: '/var/www/my-app',
            'pre-deploy': 'git fetch --all',
            'post-deploy': 'npm install && npm run build && pm2 reload ecosystem.config.js --env production',
            'pre-setup': 'apt install git -y',
        },
    },
};
```

```bash
# PM2 cluster management commands
pm2 start ecosystem.config.js --env production
pm2 reload ecosystem.config.js --env production   # Zero-downtime reload
pm2 scale my-monolith 8                           # Scale to 8 instances
pm2 monit                                         # Real-time monitoring
pm2 save                                          # Save process list
pm2 startup                                       # Generate startup script
```

### 1.2 Zero-Downtime Deployment with PM2

PM2 supports graceful reload where old workers accept new connections only after existing requests complete.

```javascript
// src/server.js — Graceful shutdown support for zero-downtime
import express from 'express';
import { createServer } from 'http';
import process from 'process';

const app = express();
const server = createServer(app);

let isShuttingDown = false;
let activeConnections = 0;

// Track active connections
server.on('connection', (connection) => {
    activeConnections++;
    connection.on('close', () => activeConnections--);
});

// Health endpoint — returns 503 during shutdown
app.get('/health', (req, res) => {
    if (isShuttingDown) {
        return res.status(503).json({ status: 'shutting_down' });
    }
    res.json({
        status: 'healthy',
        pid: process.pid,
        uptime: process.uptime(),
        activeConnections,
    });
});

// Readiness probe — Kubernetes/ALB integration
app.get('/ready', (req, res) => {
    if (isShuttingDown) {
        return res.status(503).json({ ready: false });
    }
    res.json({ ready: true });
});

// Application routes
app.get('/api/data', async (req, res) => {
    // Your application logic
    res.json({ data: 'response' });
});

// PM2 graceful shutdown signal handler
process.on('message', (msg) => {
    if (msg === 'shutdown') {
        console.log(`Worker ${process.pid} received shutdown signal`);
        gracefulShutdown();
    }
});

// Also handle SIGTERM for non-PM2 environments
process.on('SIGTERM', gracefulShutdown);

async function gracefulShutdown() {
    isShuttingDown = true;
    console.log(`Worker ${process.pid}: Draining connections (${activeConnections} active)`);

    // Stop accepting new connections
    server.close(() => {
        console.log(`Worker ${process.pid}: All connections closed, exiting`);
        process.exit(0);
    });

    // Force shutdown after 30 seconds
    setTimeout(() => {
        console.error(`Worker ${process.pid}: Forced shutdown after timeout`);
        process.exit(1);
    }, 30000);
}

const PORT = process.env.PORT || 3000;
server.listen(PORT, () => {
    console.log(`Worker ${process.pid} listening on port ${PORT}`);
});
```

```javascript
// deploy.sh — Zero-downtime deployment script
const { execSync } = require('child_process');

const DEPLOY_STEPS = [
    { name: 'Pull latest code', cmd: 'git pull origin main' },
    { name: 'Install dependencies', cmd: 'npm ci --production=false' },
    { name: 'Run database migrations', cmd: 'npm run migrate' },
    { name: 'Build application', cmd: 'npm run build' },
    { name: 'Run smoke tests', cmd: 'npm run test:smoke' },
    { name: 'Reload PM2 processes', cmd: 'pm2 reload ecosystem.config.js --env production' },
    { name: 'Verify health', cmd: 'npm run health-check' },
];

for (const step of DEPLOY_STEPS) {
    console.log(`\n>>> ${step.name}...`);
    try {
        execSync(step.cmd, { stdio: 'inherit' });
    } catch (err) {
        console.error(`FAILED: ${step.name}`);
        process.exit(1);
    }
}
console.log('\nDeployment complete!');
```

### 1.3 Nginx Reverse Proxy with Load Balancing

```nginx
# /etc/nginx/conf.d/my-app.conf
# ──────────────────────────────────────────────────────────────
# Upstream: defines backend Node.js instances
# ──────────────────────────────────────────────────────────────
upstream node_backend {
    # Load balancing method (choose one):
    # round-robin (default) — requests distributed evenly
    # least_conn            — sends to least busy server
    # ip_hash               — sticky sessions by client IP
    # hash $request_id      — consistent hashing

    least_conn;

    server 127.0.0.1:3000 weight=3;  # Primary (3x traffic)
    server 127.0.0.1:3001 weight=2;  # Secondary (2x traffic)
    server 127.0.0.1:3002 weight=1;  # Tertiary (1x traffic)
    server 127.0.0.1:3003 backup;    # Only used when others fail

    keepalive 32;                      # Persistent upstream connections
}

# ──────────────────────────────────────────────────────────────
# Upstream with sticky sessions (requires nginx-sticky-module)
# ──────────────────────────────────────────────────────────────
upstream node_backend_sticky {
    sticky cookie srv_id expires=1h domain=.example.com path=/;

    server 127.0.0.1:3000;
    server 127.0.0.1:3001;
    server 127.0.0.1:3002;
    server 127.0.0.1:3003;
}

# ──────────────────────────────────────────────────────────────
# Rate limiting zones
# ──────────────────────────────────────────────────────────────
limit_req_zone $binary_remote_addr zone=api_limit:10m rate=100r/s;
limit_req_zone $binary_remote_addr zone=auth_limit:10m rate=10r/s;
limit_conn_zone $binary_remote_addr zone=conn_limit:10m;

# ──────────────────────────────────────────────────────────────
# Main server block
# ──────────────────────────────────────────────────────────────
server {
    listen 80;
    server_name example.com;
    return 301 https://$host$request_uri;
}

server {
    listen 443 ssl http2;
    server_name example.com;

    # SSL Configuration
    ssl_certificate     /etc/letsencrypt/live/example.com/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/example.com/privkey.pem;
    ssl_protocols       TLSv1.2 TLSv1.3;
    ssl_ciphers         HIGH:!aNULL:!MD5;
    ssl_session_cache   shared:SSL:10m;
    ssl_session_timeout 10m;

    # Security headers
    add_header X-Frame-Options       "SAMEORIGIN" always;
    add_header X-Content-Type-Options "nosniff" always;
    add_header X-XSS-Protection      "1; mode=block" always;
    add_header Strict-Transport-Security "max-age=31536000; includeSubDomains" always;

    # Connection limits
    limit_conn conn_limit 50;

    # API routes with rate limiting
    location /api/ {
        limit_req zone=api_limit burst=50 nodelay;

        proxy_pass http://node_backend;
        proxy_http_version 1.1;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_set_header X-Request-ID $request_id;

        # Timeouts
        proxy_connect_timeout 10s;
        proxy_send_timeout    60s;
        proxy_read_timeout    60s;

        # Buffering
        proxy_buffering on;
        proxy_buffer_size 4k;
        proxy_buffers 8 4k;
    }

    # Auth routes with stricter rate limiting
    location /api/auth/ {
        limit_req zone=auth_limit burst=5 nodelay;

        proxy_pass http://node_backend;
        proxy_http_version 1.1;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }

    # WebSocket support
    location /ws/ {
        proxy_pass http://node_backend;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection "upgrade";
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_read_timeout 86400s;  # 24h for long-lived WS
    }

    # Static files with aggressive caching
    location /static/ {
        alias /var/www/my-app/public/;
        expires 1y;
        add_header Cache-Control "public, immutable";
        access_log off;
    }

    # Health check endpoint (no logging)
    location /health {
        proxy_pass http://node_backend/health;
        access_log off;
    }

    # Default location
    location / {
        proxy_pass http://node_backend;
        proxy_http_version 1.1;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }

    # Error pages
    error_page 502 503 504 /50x.html;
    location = /50x.html {
        root /usr/share/nginx/html;
    }
}
```

### 1.4 Modular Monolith Architecture

Structuring a monolith with clear module boundaries enables eventual migration to microservices without premature decomposition.

```javascript
// src/modules/index.js — Module registry
// Each module is self-contained with its own routes, services, and data access
import { userModule } from './users/index.js';
import { orderModule } from './orders/index.js';
import { paymentModule } from './payments/index.js';
import { notificationModule } from './notifications/index.js';

export const modules = [userModule, orderModule, paymentModule, notificationModule];

export function registerModules(app, container) {
    for (const mod of modules) {
        mod.register(app, container);
        console.log(`Module registered: ${mod.name}`);
    }
}
```

```javascript
// src/modules/users/index.js — Self-contained user module
// ──────────────────────────────────────────────────────────────
// Boundary: This module owns user-related logic and data.
// Other modules interact ONLY through the exported public API.
// ──────────────────────────────────────────────────────────────
import { Router } from 'express';
import { UserService } from './user.service.js';
import { UserController } from './user.controller.js';
import { UserRepository } from './user.repository.js';
import { validateBody } from '../../shared/middleware/validation.js';
import { createUserSchema, updateUserSchema } from './user.schema.js';

export const userModule = {
    name: 'users',
    version: '1.0.0',

    register(app, container) {
        // Register dependencies in the DI container
        const repo = new UserRepository(container.get('db'));
        const service = new UserService(repo);
        const controller = new UserController(service);

        container.register('userService', service);

        // Mount routes
        const router = Router();
        router.get('/', controller.list.bind(controller));
        router.get('/:id', controller.getById.bind(controller));
        router.post('/', validateBody(createUserSchema), controller.create.bind(controller));
        router.put('/:id', validateBody(updateUserSchema), controller.update.bind(controller));
        router.delete('/:id', controller.delete.bind(controller));

        app.use('/api/users', router);
    },

    // Public API for inter-module communication
    // Other modules should ONLY use this, never reach into internals
    getPublicAPI(container) {
        const service = container.get('userService');
        return {
            findById: service.findById.bind(service),
            findByIds: service.findByIds.bind(service),
            validateUser: service.validateExists.bind(service),
        };
    },
};
```

```javascript
// src/modules/orders/index.js — Order module with inter-module dependency
import { Router } from 'express';

export const orderModule = {
    name: 'orders',
    version: '1.0.0',

    register(app, container) {
        const userAPI = container.getModuleAPI('users'); // Public API only
        const service = new OrderService(container.get('db'), userAPI);
        const controller = new OrderController(service);

        const router = Router();
        router.get('/', controller.list.bind(controller));
        router.post('/', controller.create.bind(controller));

        app.use('/api/orders', router);
    },

    getPublicAPI(container) {
        const service = container.get('orderService');
        return {
            findById: service.findById.bind(service),
            findByUser: service.findByUser.bind(service),
        };
    },
};

// src/modules/orders/order.service.js
export class OrderService {
    constructor(db, userAPI) {
        this.db = db;
        this.userAPI = userAPI; // Uses public API, not direct DB access
    }

    async create(userId, items) {
        // Uses user module's public API — this boundary enables extraction
        const user = await this.userAPI.findById(userId);
        if (!user) throw new Error('User not found');

        const order = await this.db.orders.create({
            userId,
            items,
            total: items.reduce((sum, i) => sum + i.price * i.quantity, 0),
            status: 'pending',
            createdAt: new Date(),
        });

        return order;
    }
}
```

---

## Part 2: Microservices Deployment Architectures

### 2.1 Service Decomposition Patterns

```
Decomposition Strategies:
──────────────────────────────────────────────────────────────
1. By Business Domain (DDD Bounded Contexts)
   ┌──────────┐ ┌──────────┐ ┌──────────┐
   │  Users   │ │  Orders  │ │ Inventory│
   │ Context  │ │ Context  │ │ Context  │
   └──────────┘ └──────────┘ └──────────┘

2. By Subdomain
   ┌──────────┐ ┌──────────┐ ┌──────────┐
   │  Auth    │ │  Catalog │ │ Shipping │
   │ Subdomain│ │ Subdomain│ │ Subdomain│
   └──────────┘ └──────────┘ └──────────┘

3. By Data Ownership
   ┌──────────┐ ┌──────────┐ ┌──────────┐
   │ Service A│ │ Service B│ │ Service C│
   │ owns DB_A│ │ owns DB_B│ │ owns DB_C│
   └──────────┘ └──────────┘ └──────────┘
──────────────────────────────────────────────────────────────
```

### 2.2 API Gateway — Custom Express Gateway

```javascript
// api-gateway/src/server.js — Custom API Gateway
import express from 'express';
import { createProxyMiddleware } from 'http-proxy-middleware';
import rateLimit from 'express-rate-limit';
import jwt from 'jsonwebtoken';
import axios from 'axios';

const app = express();

// ─── Service Registry (in production, use Consul or etcd) ───
const serviceRegistry = {
    'user-service': {
        instances: [
            { url: 'http://user-service-1:3001', healthy: true },
            { url: 'http://user-service-2:3001', healthy: true },
        ],
        currentIndex: 0,
    },
    'order-service': {
        instances: [
            { url: 'http://order-service-1:3002', healthy: true },
            { url: 'http://order-service-2:3002', healthy: true },
        ],
        currentIndex: 0,
    },
    'product-service': {
        instances: [
            { url: 'http://product-service-1:3003', healthy: true },
        ],
        currentIndex: 0,
    },
};

// ─── Round-robin load balancer ───────────────────────────────
function getServiceUrl(serviceName) {
    const service = serviceRegistry[serviceName];
    if (!service) throw new Error(`Service ${serviceName} not registered`);

    const healthyInstances = service.instances.filter(i => i.healthy);
    if (healthyInstances.length === 0) throw new Error(`No healthy instances for ${serviceName}`);

    const instance = healthyInstances[service.currentIndex % healthyInstances.length];
    service.currentIndex = (service.currentIndex + 1) % healthyInstances.length;
    return instance.url;
}

// ─── Health check loop ──────────────────────────────────────
setInterval(async () => {
    for (const [name, service] of Object.entries(serviceRegistry)) {
        for (const instance of service.instances) {
            try {
                await axios.get(`${instance.url}/health`, { timeout: 3000 });
                instance.healthy = true;
            } catch {
                instance.healthy = false;
                console.warn(`Unhealthy: ${name} @ ${instance.url}`);
            }
        }
    }
}, 10000);

// ─── Auth middleware ─────────────────────────────────────────
function authenticate(req, res, next) {
    const token = req.headers.authorization?.replace('Bearer ', '');
    if (!token) return res.status(401).json({ error: 'No token provided' });

    try {
        req.user = jwt.verify(token, process.env.JWT_SECRET);
        next();
    } catch {
        res.status(401).json({ error: 'Invalid token' });
    }
}

// ─── Rate limiting ──────────────────────────────────────────
const limiter = rateLimit({
    windowMs: 60 * 1000,
    max: 100,
    standardHeaders: true,
    legacyHeaders: false,
    keyGenerator: (req) => req.user?.id || req.ip,
});
app.use(limiter);

// ─── Request ID middleware ───────────────────────────────────
app.use((req, res, next) => {
    req.headers['x-request-id'] = req.headers['x-request-id']
        || crypto.randomUUID();
    req.headers['x-correlation-id'] = req.headers['x-correlation-id']
        || req.headers['x-request-id'];
    next();
});

// ─── Route definitions ──────────────────────────────────────
// Public routes
app.use('/api/auth', createProxyMiddleware({
    router: () => getServiceUrl('user-service'),
    target: '',
    changeOrigin: true,
    pathRewrite: { '^/api/auth': '/auth' },
}));

// Protected routes
app.use('/api/users', authenticate, createProxyMiddleware({
    router: () => getServiceUrl('user-service'),
    target: '',
    changeOrigin: true,
    pathRewrite: { '^/api/users': '/users' },
}));

app.use('/api/orders', authenticate, createProxyMiddleware({
    router: () => getServiceUrl('order-service'),
    target: '',
    changeOrigin: true,
    pathRewrite: { '^/api/orders': '/orders' },
}));

app.use('/api/products', createProxyMiddleware({
    router: () => getServiceUrl('product-service'),
    target: '',
    changeOrigin: true,
    pathRewrite: { '^/api/products': '/products' },
}));

// ─── Gateway health ─────────────────────────────────────────
app.get('/health', (req, res) => {
    const services = Object.entries(serviceRegistry).map(([name, svc]) => ({
        name,
        healthy: svc.instances.some(i => i.healthy),
        instances: svc.instances.length,
    }));
    res.json({ status: 'ok', services });
});

// ─── API versioning via headers ─────────────────────────────
app.use('/api/v2/users', authenticate, (req, res, next) => {
    req.headers['x-api-version'] = '2';
    createProxyMiddleware({
        router: () => getServiceUrl('user-service'),
        target: '',
        changeOrigin: true,
        pathRewrite: { '^/api/v2/users': '/v2/users' },
    })(req, res, next);
});

app.listen(8080, () => console.log('API Gateway on :8080'));
```

### 2.3 Service Discovery with Consul

```javascript
// shared/service-discovery/consul-client.js
import Consul from 'consul';

export class ServiceDiscovery {
    constructor(serviceName, servicePort) {
        this.consul = new Consul({
            host: process.env.CONSUL_HOST || 'consul',
            port: process.env.CONSUL_PORT || 8500,
        });
        this.serviceName = serviceName;
        this.servicePort = servicePort;
        this.serviceId = `${serviceName}-${process.pid}`;
    }

    async register() {
        await this.consul.agent.service.register({
            id: this.serviceId,
            name: this.serviceName,
            address: process.env.SERVICE_HOST || require('os').hostname(),
            port: parseInt(this.servicePort),
            tags: [`v${process.env.SERVICE_VERSION || '1'}`, process.env.NODE_ENV],
            check: {
                http: `http://${process.env.SERVICE_HOST || 'localhost'}:${this.servicePort}/health`,
                interval: '10s',
                timeout: '5s',
                deregistercriticalserviceafter: '30s',
            },
        });
        console.log(`Registered ${this.serviceName} as ${this.serviceId}`);
    }

    async deregister() {
        await this.consul.agent.service.deregister(this.serviceId);
        console.log(`Deregistered ${this.serviceId}`);
    }

    async discover(serviceName) {
        const result = await this.consul.health.service({
            service: serviceName,
            passing: true, // Only healthy instances
        });

        return result.map(entry => ({
            address: entry.Service.Address || entry.Node.Address,
            port: entry.Service.Port,
            id: entry.Service.ID,
            tags: entry.Service.Tags,
        }));
    }

    async getServiceUrl(serviceName) {
        const instances = await this.discover(serviceName);
        if (instances.length === 0) {
            throw new Error(`No healthy instances for ${serviceName}`);
        }
        // Simple round-robin
        const instance = instances[Math.floor(Math.random() * instances.length)];
        return `http://${instance.address}:${instance.port}`;
    }

    // Watch for service changes
    watch(serviceName, onChange) {
        const watcher = this.consul.watch({
            method: this.consul.health.service,
            options: { service: serviceName, passing: true },
        });

        watcher.on('change', (data) => {
            const instances = data.map(entry => ({
                address: entry.Service.Address || entry.Node.Address,
                port: entry.Service.Port,
            }));
            onChange(instances);
        });

        watcher.on('error', (err) => {
            console.error(`Consul watch error for ${serviceName}:`, err);
        });

        return watcher;
    }
}

// Usage in a service:
// const discovery = new ServiceDiscovery('order-service', 3002);
// await discovery.register();
// const userServiceUrl = await discovery.getServiceUrl('user-service');
```

### 2.4 Inter-Service Communication

#### REST (Synchronous)

```javascript
// shared/http-client.js — Resilient HTTP client for inter-service calls
import axios from 'axios';
import axiosRetry from 'axios-retry';

export function createServiceClient(baseURL, options = {}) {
    const client = axios.create({
        baseURL,
        timeout: options.timeout || 5000,
        headers: {
            'Content-Type': 'application/json',
            'X-Service-Name': process.env.SERVICE_NAME,
        },
    });

    // Retry with exponential backoff
    axiosRetry(client, {
        retries: options.retries || 3,
        retryDelay: axiosRetry.exponentialDelay,
        retryCondition: (error) =>
            axiosRetry.isNetworkOrIdempotentRequestError(error)
            || error.response?.status >= 500,
    });

    // Circuit breaker state
    let failures = 0;
    let circuitOpen = false;
    let lastFailure = 0;
    const threshold = options.circuitBreakerThreshold || 5;
    const resetTimeout = options.circuitBreakerReset || 30000;

    client.interceptors.request.use((config) => {
        if (circuitOpen) {
            if (Date.now() - lastFailure > resetTimeout) {
                circuitOpen = false;
                failures = 0;
            } else {
                throw new Error(`Circuit breaker open for ${baseURL}`);
            }
        }
        config.metadata = { startTime: Date.now() };
        return config;
    });

    client.interceptors.response.use(
        (response) => {
            failures = 0;
            const duration = Date.now() - response.config.metadata.startTime;
            console.log(`[HTTP] ${response.config.method?.toUpperCase()} ${response.config.url} ${response.status} ${duration}ms`);
            return response;
        },
        (error) => {
            failures++;
            if (failures >= threshold) {
                circuitOpen = true;
                lastFailure = Date.now();
                console.error(`Circuit breaker OPEN for ${baseURL} after ${failures} failures`);
            }
            return Promise.reject(error);
        }
    );

    return client;
}

// Usage:
// const userService = createServiceClient('http://user-service:3001');
// const { data: user } = await userService.get(`/users/${userId}`);
```

#### gRPC (Synchronous, High Performance)

```protobuf
// proto/order.proto
syntax = "proto3";

package order;

service OrderService {
    rpc CreateOrder(CreateOrderRequest) returns (Order);
    rpc GetOrder(GetOrderRequest) returns (Order);
    rpc ListUserOrders(ListUserOrdersRequest) returns (OrderList);
}

message CreateOrderRequest {
    string user_id = 1;
    repeated OrderItem items = 2;
}

message OrderItem {
    string product_id = 1;
    int32 quantity = 2;
    double price = 3;
}

message Order {
    string id = 1;
    string user_id = 2;
    repeated OrderItem items = 3;
    double total = 4;
    string status = 5;
    string created_at = 6;
}

message GetOrderRequest {
    string id = 1;
}

message ListUserOrdersRequest {
    string user_id = 1;
    int32 page = 2;
    int32 limit = 3;
}

message OrderList {
    repeated Order orders = 1;
    int32 total = 2;
}
```

```javascript
// order-service/src/grpc-server.js
import grpc from '@grpc/grpc-js';
import protoLoader from '@grpc/proto-loader';
import path from 'path';

const PROTO_PATH = path.resolve('proto/order.proto');
const packageDef = protoLoader.loadSync(PROTO_PATH, {
    keepCase: false,
    longs: String,
    enums: String,
    defaults: true,
    oneofs: true,
});

const proto = grpc.loadPackageDefinition(packageDef).order;

const server = new grpc.Server();

server.addService(proto.OrderService.service, {
    async createOrder(call, callback) {
        try {
            const { user_id, items } = call.request;
            const total = items.reduce((sum, i) => sum + i.price * i.quantity, 0);
            const order = await db.orders.create({
                userId: user_id,
                items,
                total,
                status: 'pending',
            });
            callback(null, {
                id: order.id,
                userId: order.userId,
                items: order.items,
                total: order.total,
                status: order.status,
                createdAt: order.createdAt.toISOString(),
            });
        } catch (err) {
            callback({ code: grpc.status.INTERNAL, message: err.message });
        }
    },

    async getOrder(call, callback) {
        const order = await db.orders.findById(call.request.id);
        if (!order) {
            return callback({ code: grpc.status.NOT_FOUND, message: 'Order not found' });
        }
        callback(null, {
            id: order.id,
            userId: order.userId,
            items: order.items,
            total: order.total,
            status: order.status,
            createdAt: order.createdAt.toISOString(),
        });
    },
});

server.bindAsync(
    '0.0.0.0:50051',
    grpc.ServerCredentials.createInsecure(),
    (err, port) => {
        if (err) throw err;
        console.log(`gRPC server on port ${port}`);
        server.start();
    }
);
```

#### Message Queue (Asynchronous)

```javascript
// shared/message-bus/rabbitmq.js
import amqp from 'amqplib';

export class MessageBus {
    constructor(url) {
        this.url = url || process.env.RABBITMQ_URL || 'amqp://localhost';
        this.connection = null;
        this.channel = null;
        this.handlers = new Map();
    }

    async connect() {
        this.connection = await amqp.connect(this.url);
        this.channel = await this.connection.createChannel();

        this.connection.on('error', (err) => {
            console.error('RabbitMQ connection error:', err);
            setTimeout(() => this.connect(), 5000);
        });

        console.log('Connected to RabbitMQ');
    }

    async publish(exchange, routingKey, message, options = {}) {
        if (!this.channel) throw new Error('Not connected');

        await this.channel.assertExchange(exchange, 'topic', { durable: true });
        this.channel.publish(
            exchange,
            routingKey,
            Buffer.from(JSON.stringify({
                id: crypto.randomUUID(),
                timestamp: new Date().toISOString(),
                data: message,
            })),
            { persistent: true, ...options }
        );
    }

    async subscribe(exchange, queue, routingKeys, handler) {
        if (!this.channel) throw new Error('Not connected');

        await this.channel.assertExchange(exchange, 'topic', { durable: true });
        await this.channel.assertQueue(queue, {
            durable: true,
            arguments: { 'x-dead-letter-exchange': `${exchange}.dlx` },
        });

        for (const key of routingKeys) {
            await this.channel.bindQueue(queue, exchange, key);
        }

        this.channel.prefetch(10);

        this.channel.consume(queue, async (msg) => {
            if (!msg) return;
            try {
                const content = JSON.parse(msg.content.toString());
                await handler(content);
                this.channel.ack(msg);
            } catch (err) {
                console.error(`Handler error for ${routingKeys}:`, err);
                this.channel.nack(msg, false, false); // Send to DLX
            }
        });

        console.log(`Subscribed: ${queue} → [${routingKeys.join(', ')}]`);
    }

    async close() {
        await this.channel?.close();
        await this.connection?.close();
    }
}

// Usage in Order Service — publishes events after order creation:
// const bus = new MessageBus();
// await bus.connect();
// await bus.publish('orders', 'order.created', { orderId: order.id, userId: order.userId });

// Usage in Notification Service — subscribes to events:
// await bus.subscribe('orders', 'notification-queue', ['order.created', 'order.shipped'], async (event) => {
//     await sendEmail(event.data.userId, `Order ${event.data.orderId} updated`);
// });
```

---

## Part 3: Strangler Fig Migration Pattern

The strangler fig pattern incrementally replaces monolith functionality with microservices, routing traffic through a facade until the monolith is fully decomposed.

```
Strangler Fig Progression:
──────────────────────────────────────────────────────────────────
Phase 1:               Phase 2:               Phase 3:
┌──────────┐           ┌──────────┐           ┌──────────┐
│ Gateway  │           │ Gateway  │           │ Gateway  │
└────┬─────┘           └────┬─────┘           └────┬─────┘
     │                      │                      │
     ▼                      ▼                      ▼
┌──────────┐           ┌──────────┐           ┌──────────┐
│ Monolith │◀────95%   │ Monolith │◀────40%   │ Monolith │◀────0%
│ (all)    │           │ (orders, │           │ (legacy) │
└──────────┘           │  reports)│           └──────────┘
                       └──────────┘
┌──────────┐           ┌──────────┐           ┌──────────┐
│ User Svc │◀────5%    │ User Svc │◀────30%   │ User Svc │◀────30%
└──────────┘           └──────────┘           └──────────┘
                       ┌──────────┐           ┌──────────┐
                       │ Order Svc│◀────30%   │ Order Svc│◀────30%
                       └──────────┘           └──────────┘
                                              ┌──────────┐
                                              │ Report Svc│◀────10%
                                              └──────────┘
──────────────────────────────────────────────────────────────────
```

```javascript
// strangler-fig-gateway/src/router.js
import express from 'express';
import { createProxyMiddleware } from 'http-proxy-middleware';

const app = express();

// ─── Feature flag configuration ─────────────────────────────
// In production, load from Consul, LaunchDarkly, or similar
const featureFlags = {
    users: {
        v2Enabled: true,
        v2Percentage: 30,       // 30% of traffic to new service
        monolithUrl: 'http://monolith:3000',
        serviceUrl: 'http://user-service:3001',
    },
    orders: {
        v2Enabled: true,
        v2Percentage: 10,
        monolithUrl: 'http://monolith:3000',
        serviceUrl: 'http://order-service:3002',
    },
};

// ─── Traffic router middleware ───────────────────────────────
function routeToService(featureName) {
    return (req, res, next) => {
        const config = featureFlags[featureName];

        if (!config?.v2Enabled) {
            req.targetUrl = config?.monolithUrl || 'http://monolith:3000';
            return next();
        }

        // Deterministic routing by user ID (sticky)
        // Falls back to random percentage for unauthenticated requests
        let shouldUseV2;
        if (req.user?.id) {
            const hash = req.user.id.split('').reduce((acc, c) => acc + c.charCodeAt(0), 0);
            shouldUseV2 = (hash % 100) < config.v2Percentage;
        } else {
            shouldUseV2 = Math.random() * 100 < config.v2Percentage;
        }

        req.targetUrl = shouldUseV2 ? config.serviceUrl : config.monolithUrl;
        req.targetLabel = shouldUseV2 ? 'microservice' : 'monolith';

        // Header for observability
        res.setHeader('X-Routed-To', req.targetLabel);
        res.setHeader('X-Feature', featureName);
        next();
    };
}

// ─── Proxy factory with logging ─────────────────────────────
function createProxy(pathRewrite) {
    return (req, res, next) => {
        createProxyMiddleware({
            target: req.targetUrl,
            changeOrigin: true,
            pathRewrite,
            onProxyReq: (proxyReq) => {
                proxyReq.setHeader('X-Request-Source', 'strangler-gateway');
                proxyReq.setHeader('X-Routed-To', req.targetLabel);
            },
            onError: (err, req, res) => {
                console.error(`Proxy error [${req.targetLabel}] ${req.url}:`, err.message);
                res.status(502).json({ error: 'Service unavailable' });
            },
        })(req, res, next);
    };
}

// ─── Route definitions ──────────────────────────────────────
app.use('/api/users',     routeToService('users'),  createProxy({ '^/api': '' }));
app.use('/api/orders',    routeToService('orders'), createProxy({ '^/api': '' }));

// Everything else goes to the monolith
app.use('/', createProxyMiddleware({
    target: 'http://monolith:3000',
    changeOrigin: true,
    onError: (err, req, res) => {
        res.status(502).json({ error: 'Monolith unavailable' });
    },
}));

app.listen(8080, () => console.log('Strangler fig gateway on :8080'));
```

---

## Part 4: Database per Service with Saga Pattern

### Database per Service

```
Database Ownership:
──────────────────────────────────────────────────────
  User Service ──────▶ PostgreSQL (users_db)
  Order Service ─────▶ PostgreSQL (orders_db)
  Payment Service ───▶ PostgreSQL (payments_db)
  Inventory Service ─▶ PostgreSQL (inventory_db)

  ⛔ NO direct cross-database queries
  ✅ Inter-service communication only
──────────────────────────────────────────────────────
```

### Saga Orchestration Pattern

```javascript
// saga-orchestrator/src/order-saga.js
// Central orchestrator coordinates the multi-service transaction
import { MessageBus } from '../../shared/message-bus/rabbitmq.js';

export class OrderSagaOrchestrator {
    constructor(db, messageBus) {
        this.db = db;
        this.bus = messageBus;
        this.steps = [
            { action: 'validate_inventory',  compensate: 'release_inventory' },
            { action: 'reserve_payment',     compensate: 'refund_payment' },
            { action: 'create_order',        compensate: 'cancel_order' },
            { action: 'confirm_inventory',   compensate: 'restore_inventory' },
            { action: 'charge_payment',      compensate: 'refund_payment' },
        ];
    }

    async execute(orderRequest) {
        const sagaId = crypto.randomUUID();
        const completedSteps = [];

        await this.db.sagaLog.create({
            sagaId,
            status: 'started',
            data: orderRequest,
        });

        try {
            // Step 1: Validate inventory
            const inventory = await this.callService('inventory-service', 'POST', '/validate', {
                items: orderRequest.items,
                reservationId: sagaId,
            });
            completedSteps.push({ step: 'validate_inventory', data: inventory });

            // Step 2: Reserve payment
            const payment = await this.callService('payment-service', 'POST', '/reserve', {
                userId: orderRequest.userId,
                amount: orderRequest.total,
                reservationId: sagaId,
            });
            completedSteps.push({ step: 'reserve_payment', data: payment });

            // Step 3: Create order
            const order = await this.callService('order-service', 'POST', '/orders', {
                userId: orderRequest.userId,
                items: orderRequest.items,
                total: orderRequest.total,
                sagaId,
            });
            completedSteps.push({ step: 'create_order', data: order });

            // Step 4: Confirm inventory reservation
            await this.callService('inventory-service', 'POST', '/confirm', {
                reservationId: sagaId,
            });
            completedSteps.push({ step: 'confirm_inventory' });

            // Step 5: Charge payment
            await this.callService('payment-service', 'POST', '/charge', {
                reservationId: sagaId,
            });
            completedSteps.push({ step: 'charge_payment' });

            await this.db.sagaLog.update(sagaId, { status: 'completed' });
            return { success: true, orderId: order.id, sagaId };

        } catch (error) {
            console.error(`Saga ${sagaId} failed at step:`, error.message);
            await this.compensate(completedSteps, sagaId);
            await this.db.sagaLog.update(sagaId, { status: 'failed', error: error.message });
            return { success: false, sagaId, error: error.message };
        }
    }

    async compensate(completedSteps, sagaId) {
        // Execute compensating actions in reverse order
        for (const step of [...completedSteps].reverse()) {
            const stepDef = this.steps.find(s => s.action === step.step);
            if (!stepDef?.compensate) continue;

            try {
                console.log(`Compensating: ${stepDef.compensate} for saga ${sagaId}`);
                await this.callService(
                    this.getServiceForStep(step.step),
                    'POST',
                    `/${stepDef.compensate}`,
                    step.data
                );
            } catch (compErr) {
                console.error(`Compensation failed for ${stepDef.compensate}:`, compErr.message);
                // Log for manual intervention — critical in production
            }
        }
    }

    async callService(serviceName, method, path, data) {
        const response = await fetch(`http://${serviceName}:3000${path}`, {
            method,
            headers: { 'Content-Type': 'application/json', 'X-Saga-Id': data?.sagaId },
            body: JSON.stringify(data),
        });
        if (!response.ok) {
            throw new Error(`${serviceName}${path} failed: ${response.status}`);
        }
        return response.json();
    }

    getServiceForStep(step) {
        const map = {
            validate_inventory: 'inventory-service',
            reserve_payment: 'payment-service',
            create_order: 'order-service',
            confirm_inventory: 'inventory-service',
            charge_payment: 'payment-service',
        };
        return map[step];
    }
}
```

### Saga Choreography Pattern

```javascript
// Each service listens for events and reacts — no central coordinator
// order-service/src/events.js
import { MessageBus } from '../../shared/message-bus/rabbitmq.js';

const bus = new MessageBus();

export async function setupOrderEventHandlers() {
    await bus.connect();

    // Order service listens for inventory reservation confirmations
    await bus.subscribe('inventory', 'order-service-queue',
        ['inventory.reserved', 'inventory.rejected'],
        async (event) => {
            if (event.data.routingKey === 'inventory.reserved') {
                // Inventory confirmed — create the order
                const order = await db.orders.create({
                    sagaId: event.data.sagaId,
                    userId: event.data.userId,
                    items: event.data.items,
                    status: 'inventory_reserved',
                });

                // Emit next event in the saga
                await bus.publish('orders', 'order.created', {
                    sagaId: event.data.sagaId,
                    orderId: order.id,
                    userId: order.userId,
                    total: order.total,
                });
            }

            if (event.data.routingKey === 'inventory.rejected') {
                // Inventory unavailable — cancel the saga
                await db.orders.updateBySagaId(event.data.sagaId, { status: 'cancelled' });
                await bus.publish('orders', 'order.cancelled', {
                    sagaId: event.data.sagaId,
                    reason: 'inventory_unavailable',
                });
            }
        }
    );

    // Listen for payment results
    await bus.subscribe('payment', 'order-service-queue',
        ['payment.charged', 'payment.failed'],
        async (event) => {
            if (event.data.routingKey === 'payment.charged') {
                await db.orders.updateBySagaId(event.data.sagaId, { status: 'confirmed' });
                await bus.publish('orders', 'order.confirmed', {
                    sagaId: event.data.sagaId,
                    orderId: event.data.orderId,
                });
            }

            if (event.data.routingKey === 'payment.failed') {
                // Compensate: release inventory
                await db.orders.updateBySagaId(event.data.sagaId, { status: 'payment_failed' });
                await bus.publish('inventory', 'inventory.release', {
                    sagaId: event.data.sagaId,
                });
            }
        }
    );
}
```

---

## Part 5: Docker Compose for Microservices

```yaml
# docker-compose.microservices.yml
# Full working microservices stack with service discovery, message bus, and databases
version: '3.8'

services:
  # ─── Infrastructure ──────────────────────────────────────
  consul:
    image: hashicorp/consul:1.17
    ports:
      - "8500:8500"
    command: agent -server -bootstrap-expect=1 -ui -client=0.0.0.0
    networks:
      - microservices

  rabbitmq:
    image: rabbitmq:3.13-management-alpine
    ports:
      - "5672:5672"
      - "15672:15672"
    environment:
      RABBITMQ_DEFAULT_USER: admin
      RABBITMQ_DEFAULT_PASS: secret
    volumes:
      - rabbitmq-data:/var/lib/rabbitmq
    networks:
      - microservices

  # ─── Databases (one per service) ─────────────────────────
  user-db:
    image: postgres:16-alpine
    environment:
      POSTGRES_DB: users
      POSTGRES_USER: app
      POSTGRES_PASSWORD: secret
    volumes:
      - user-db-data:/var/lib/postgresql/data
    networks:
      - microservices

  order-db:
    image: postgres:16-alpine
    environment:
      POSTGRES_DB: orders
      POSTGRES_USER: app
      POSTGRES_PASSWORD: secret
    volumes:
      - order-db-data:/var/lib/postgresql/data
    networks:
      - microservices

  product-db:
    image: postgres:16-alpine
    environment:
      POSTGRES_DB: products
      POSTGRES_USER: app
      POSTGRES_PASSWORD: secret
    volumes:
      - product-db-data:/var/lib/postgresql/data
    networks:
      - microservices

  redis:
    image: redis:7-alpine
    volumes:
      - redis-data:/data
    networks:
      - microservices

  # ─── Application Services ────────────────────────────────
  api-gateway:
    build:
      context: .
      dockerfile: api-gateway/Dockerfile
    ports:
      - "8080:8080"
    environment:
      CONSUL_HOST: consul
      JWT_SECRET: ${JWT_SECRET:-change-me}
      NODE_ENV: production
    depends_on:
      - consul
      - user-service
      - order-service
      - product-service
    networks:
      - microservices
    deploy:
      replicas: 2
      resources:
        limits:
          memory: 256M
          cpus: '0.5'

  user-service:
    build:
      context: .
      dockerfile: services/user-service/Dockerfile
    environment:
      SERVICE_NAME: user-service
      PORT: 3001
      DATABASE_URL: postgresql://app:secret@user-db:5432/users
      REDIS_URL: redis://redis:6379/0
      CONSUL_HOST: consul
      RABBITMQ_URL: amqp://admin:secret@rabbitmq:5672
    depends_on:
      - user-db
      - redis
      - consul
      - rabbitmq
    networks:
      - microservices
    deploy:
      replicas: 2
      resources:
        limits:
          memory: 256M
          cpus: '0.5'

  order-service:
    build:
      context: .
      dockerfile: services/order-service/Dockerfile
    environment:
      SERVICE_NAME: order-service
      PORT: 3002
      DATABASE_URL: postgresql://app:secret@order-db:5432/orders
      CONSUL_HOST: consul
      RABBITMQ_URL: amqp://admin:secret@rabbitmq:5672
    depends_on:
      - order-db
      - consul
      - rabbitmq
    networks:
      - microservices
    deploy:
      replicas: 2
      resources:
        limits:
          memory: 256M
          cpus: '0.5'

  product-service:
    build:
      context: .
      dockerfile: services/product-service/Dockerfile
    environment:
      SERVICE_NAME: product-service
      PORT: 3003
      DATABASE_URL: postgresql://app:secret@product-db:5432/products
      REDIS_URL: redis://redis:6379/1
      CONSUL_HOST: consul
      RABBITMQ_URL: amqp://admin:secret@rabbitmq:5672
    depends_on:
      - product-db
      - redis
      - consul
      - rabbitmq
    networks:
      - microservices
    deploy:
      replicas: 2
      resources:
        limits:
          memory: 256M
          cpus: '0.5'

  # ─── Observability ───────────────────────────────────────
  prometheus:
    image: prom/prometheus:v2.51.0
    volumes:
      - ./monitoring/prometheus.yml:/etc/prometheus/prometheus.yml
    ports:
      - "9090:9090"
    networks:
      - microservices

  grafana:
    image: grafana/grafana:10.4.0
    ports:
      - "3001:3000"
    environment:
      GF_SECURITY_ADMIN_PASSWORD: admin
    volumes:
      - grafana-data:/var/lib/grafana
    networks:
      - microservices

volumes:
  user-db-data:
  order-db-data:
  product-db-data:
  redis-data:
  rabbitmq-data:
  grafana-data:

networks:
  microservices:
    driver: bridge
```

```dockerfile
# services/user-service/Dockerfile
FROM node:20-alpine AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY tsconfig.json ./
COPY src/ ./src/
RUN npm run build

FROM node:20-alpine
RUN apk add --no-cache tini
WORKDIR /app
COPY --from=builder /app/dist ./dist
COPY --from=builder /app/node_modules ./node_modules
COPY package.json ./
USER node
EXPOSE 3001
ENTRYPOINT ["/sbin/tini", "--"]
CMD ["node", "dist/server.js"]
```

---

## Troubleshooting

| Problem | Cause | Solution |
|---------|-------|----------|
| PM2 workers not distributing evenly | Default `round-robin` on Linux | Use `pm2 reload` or set `exec_mode: 'cluster'` explicitly |
| Nginx 502 errors during deploy | Backend not ready during reload | Use PM2 `listen_timeout` and Nginx `max_fails=2 fail_timeout=10s` |
| Saga stuck in partial state | Compensation failure | Implement dead letter queue + manual intervention dashboard |
| Service discovery returns stale entries | Health check interval too long | Reduce Consul check interval to `5s` with `deregistercriticalserviceafter: 30s` |
| gRPC connection refused | Service not ready on startup | Implement gRPC health checking protocol, retry with backoff |
| Message queue consumer lag | Slow handler or too few consumers | Scale consumers, increase prefetch, use worker pools |
| Cross-service data inconsistency | Eventual consistency lag | Implement idempotency keys, use read-your-own-writes pattern |
| Docker networking DNS failures | Container startup order | Add `depends_on` with health checks, use retry logic in service clients |

---

## Best Practices Checklist

- [ ] Start with a modular monolith; extract services only when scaling demands it
- [ ] Each microservice owns its database — no shared databases
- [ ] Implement circuit breakers for all inter-service communication
- [ ] Use correlation IDs across all services for distributed tracing
- [ ] Implement idempotent handlers for all message consumers
- [ ] Configure health check endpoints on every service (`/health`, `/ready`)
- [ ] Use the saga pattern for distributed transactions, not 2PC
- [ ] Deploy API gateway in front of all microservices
- [ ] Implement centralized logging (ELK/Loki) and distributed tracing (Jaeger/Zipkin)
- [ ] Use feature flags for gradual traffic migration (strangler fig)
- [ ] Automate everything: CI/CD, infrastructure provisioning, database migrations
- [ ] Plan for failure: assume every network call can fail

---

## Cross-References

- See [Architecture Patterns](./01-architecture-patterns.md) for overview of deployment patterns
- See [Docker](../docker/01-dockerfile.md) for container setup
- See [Docker Compose](../docker/02-docker-compose.md) for multi-container orchestration
- See [Kubernetes](../03-container-orchestration/01-kubernetes-patterns.md) for K8s deployment
- See [CI/CD](../05-ci-cd-pipelines/01-github-actions.md) for deployment automation
- See [Monitoring](../08-deployment-monitoring/01-apm-metrics.md) for observability setup
- See [Message Queues](../../17-message-queues/) for async communication patterns
- See [Caching & Redis](../../16-caching-redis/) for caching strategies

---

## Next Steps

Continue to [Hybrid, Edge & Multi-Cloud Deployment](./03-hybrid-edge-multi-cloud.md).
