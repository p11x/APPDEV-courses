# Zero-Downtime Deployments

## 📌 What You'll Learn

- Understanding blue-green and rolling deployment strategies
- How PM2 handles clustering and reload
- Configuring readiness probes for deployment success
- Best practices for database migrations during deploys

## 🧠 Concept Explained (Plain English)

Zero-downtime deployments mean updating your application without users noticing any interruption. There are several strategies to achieve this:

**Rolling Updates** (used by Kubernetes default):
- Gradually replace old instances with new ones
- At any point, both old and new versions are running
- Users connect to whichever version is available
- If new version has problems, only a few users are affected

**Blue-Green Deployments**:
- Keep two identical environments (blue = current, green = new)
- Switch load balancer from blue to green when ready
- If problems, instantly switch back to blue
- Requires double the infrastructure temporarily

**Canary Releases**:
- Route small percentage of traffic to new version
- Gradually increase if new version works well
- Like rolling out a canary into a coal mine to test for danger

**Key requirements for zero-downtime:**
1. **Graceful shutdown** (covered earlier) — let requests finish before stopping
2. **Readiness probes** — don't receive traffic until ready
3. **Stateless design** — any instance can handle any request
4. **Backward compatibility** — new version works with old data

**PM2** is a production process manager for Node.js that handles clustering and zero-downtime restarts with its `reload` command.

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';

const app = express();


// ============================================
// Application Configuration
// ============================================

const config = {
  version: process.env.APP_VERSION || '1.0.0',
  name: 'my-express-app',
  port: process.env.PORT || 3000
};

console.log(`Starting ${config.name} v${config.version}`);


// ============================================
// Graceful Shutdown Setup (Critical for Zero-Downtime)
// ============================================

let isShuttingDown = false;

app.use((req, res, next) => {
  // Reject new requests during shutdown
  if (isShuttingDown) {
    res.setHeader('Connection', 'close');
    return res.status(503).json({ error: 'Shutting down' });
  }
  next();
});


// Health Check Endpoints (Critical for K8s Rolling Updates)
app.get('/health/live', (req, res) => {
  res.status(200).json({ status: 'alive', version: config.version });
});

app.get('/health/ready', (req, res) => {
  // Check if ready to receive traffic
  // For example: connected to database, warmed up cache
  const ready = !isShuttingDown;
  
  if (!ready) {
    return res.status(503).json({ status: 'not_ready' });
  }
  
  res.status(200).json({ status: 'ready', version: config.version });
});


// ============================================
// Application Routes
// ============================================

// Version endpoint to verify which version is running
app.get('/version', (req, res) => {
  res.json({ 
    version: config.version,
    name: config.name,
    timestamp: new Date().toISOString(),
    pid: process.pid
  });
});


app.get('/api/users', (req, res) => {
  res.json([
    { id: 1, name: 'John Doe' },
    { id: 2, name: 'Jane Smith' }
  ]);
});

app.post('/api/users', (req, res) => {
  const { name } = req.body || {};
  res.status(201).json({ id: 3, name: name || 'New User' });
});


// Simulate slow startup (e.g., connecting to databases, warming cache)
let startupComplete = false;

async function warmup() {
  console.log('Warming up application...');
  
  // Simulate database connection
  await new Promise(resolve => setTimeout(resolve, 1000));
  
  // Simulate cache warming
  await new Promise(resolve => setTimeout(resolve, 500));
  
  startupComplete = true;
  console.log('Warmup complete, ready to serve traffic');
}

// Run warmup before accepting traffic
warmup().then(() => {
  console.log(`Server ready on port ${config.port}`);
});


// ============================================
// Signal Handlers (for graceful shutdown)
// ============================================

process.on('SIGTERM', async () => {
  console.log('Received SIGTERM, shutting down gracefully...');
  isShuttingDown = true;
  
  // Wait for in-flight requests (handled by load balancer)
  // In Kubernetes, this is the grace period
  
  // Close connections, flush logs, etc.
  await new Promise(resolve => setTimeout(resolve, 1000));
  
  console.log('Shutdown complete');
  process.exit(0);
});

process.on('SIGINT', async () => {
  console.log('Received SIGINT, shutting down...');
  isShuttingDown = true;
  await new Promise(resolve => setTimeout(resolve, 500));
  process.exit(0);
});


// Start server
const server = app.listen(config.port, () => {
  console.log(`${config.name} v${config.version} running on port ${config.port}`);
});


export default app;
```

## 🔍 Deployment Configuration Examples

### PM2 Configuration (ecosystem.config.js)

```js
// ecosystem.config.js
module.exports = {
  apps: [{
    name: 'my-express-app',
    script: 'src/index.js',
    
    // Number of instances (use 0 for CPU cores)
    instances: 'max',
    
    // Cluster mode
    exec_mode: 'cluster',
    
    // Wait for port to be free before starting new instance
    wait_ready: true,
    
    // Send ready signal after this many ms
    listen_timeout: 5000,
    
    // Kill old instance after this ms (force restart)
    kill_timeout: 5000,
    
    // Environment
    env: {
      NODE_ENV: 'development',
      PORT: 3000
    },
    env_production: {
      NODE_ENV: 'production',
      PORT: 3000
    }
  }]
};
```

### Kubernetes Deployment Example

```yaml
# deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: my-express-app
spec:
  replicas: 3
  strategy:
    type: RollingUpdate
    rollingUpdate:
      maxSurge: 1        # Allow 1 extra pod during rollout
      maxUnavailable: 0  # Don't have any unavailable pods
  selector:
    matchLabels:
      app: my-express-app
  template:
    metadata:
      labels:
        app: my-express-app
    spec:
      containers:
      - name: my-express-app
        image: my-registry/my-express-app:v1.2.3
        ports:
        - containerPort: 3000
        
        # Readiness probe - don't send traffic until ready
        readinessProbe:
          httpGet:
            path: /health/ready
            port: 3000
          initialDelaySeconds: 5
          periodSeconds: 5
          successThreshold: 1
          failureThreshold: 3
        
        # Liveness probe - restart if hung
        livenessProbe:
          httpGet:
            path: /health/live
            port: 3000
          initialDelaySeconds: 10
          periodSeconds: 10
          failureThreshold: 3
        
        # Grace period for shutdown
        lifecycle:
          preStop:
            exec:
              command: ["sleep", "10"]
        
        resources:
          limits:
            memory: "256Mi"
            cpu: "500m"
          requests:
            memory: "128Mi"
            cpu: "250m"
```

## ⚠️ Common Mistakes

### 1. Not waiting for readiness

**What it is**: New instances receive traffic before they're ready, causing errors.

**Why it happens**: Skipping readiness probes or setting timeouts too short.

**How to fix it**: Implement `/health/ready` endpoint that checks dependencies. Set appropriate probe intervals.

### 2. Breaking changes without version negotiation

**What it is**: API changes that old clients can't handle.

**Why it happens**: Not maintaining backward compatibility.

**How to fix it**: Use API versioning, support old versions for a transition period.

### 3. Database migrations during deploy

**What it is**: Schema changes that break running instances.

**Why it happens**: Running migrations before or during deployment.

**How to fix it**: Use the expand-contract pattern — add new fields/tables first, deploy, then remove old ones.

## ✅ Quick Recap

- Zero-downtime deploys require graceful shutdown and readiness checks
- Rolling updates gradually replace instances (default Kubernetes strategy)
- Blue-green uses two environments and swaps traffic at once
- Canary releases route small % of traffic to test new versions
- PM2 with `cluster` mode and `reload` command provides zero-downtime restarts
- Readiness probes prevent traffic to unready instances

## 🔗 What's Next

This completes the Production Readiness section. The guide now continues to [Advanced Architecture](./../12_Advanced_Architecture/01_Event_Driven/eventemitter-in-express.md) where you'll learn about event-driven patterns in Express.
