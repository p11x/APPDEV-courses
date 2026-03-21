# Plugin Extension System

## 📌 What You'll Learn

- Building an Express app that accepts plugins
- Defining hook points for extensibility
- Dynamic middleware loading
- Creating a plugin API for third-party developers

## 🧠 Concept Explained (Plain English)

A **plugin system** lets other developers extend your Express application without modifying the core code. Think of how VS Code has extensions, or how WordPress has plugins.

**Key concepts:**
- **Host application**: The main Express app that loads plugins
- **Plugin**: A module that extends functionality
- **Hook points**: Places in the host where plugins can add behavior
- **Plugin API**: The interface plugins use to interact with the host

**Benefits:**
- Third-party extensions without modifying core
- Modular functionality
- Easy to enable/disable features
- Community ecosystem building

**Common hook points:**
- `onRequest`: Before route handling
- `preRoute`: After routing, before handler
- `postHandler`: After response sent

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';
import { readdirSync, existsSync } from 'fs';
import { join, dirname } from 'path';
import { fileURLToPath } from 'url';

const __dirname = dirname(fileURLToPath(import.meta.url));
const app = express();


// ============================================
// Plugin System Core
// ============================================

class PluginManager {
  constructor(app) {
    this.app = app;
    this.plugins = new Map();
    this.hooks = {
      onRequest: [],
      preRouting: [],
      preHandler: [],
      postHandler: [],
      onError: []
    };
  }
  
  // Register a plugin
  register(name, plugin) {
    if (this.plugins.has(name)) {
      throw new Error(`Plugin ${name} already registered`);
    }
    
    console.log(`📦 Loading plugin: ${name}`);
    
    // Initialize plugin with app context
    if (plugin.init) {
      plugin.init(this);
    }
    
    // Register hooks
    if (plugin.hooks) {
      for (const [hookName, handler] of Object.entries(plugin.hooks)) {
        this.registerHook(name, hookName, handler);
      }
    }
    
    // Register routes
    if (plugin.routes) {
      this.registerRoutes(name, plugin.routes);
    }
    
    // Register middleware
    if (plugin.middleware) {
      this.registerMiddleware(name, plugin.middleware);
    }
    
    this.plugins.set(name, plugin);
    console.log(`✅ Plugin loaded: ${name}`);
  }
  
  // Register a hook handler
  registerHook(pluginName, hookName, handler) {
    if (!this.hooks[hookName]) {
      console.warn(`Unknown hook: ${hookName}`);
      return;
    }
    
    this.hooks[hookName].push({ plugin: pluginName, handler });
  }
  
  // Register plugin routes
  registerRoutes(pluginName, routes) {
    const router = express.Router();
    
    for (const [method, path, handler] of routes) {
      router[method](path, handler);
    }
    
    this.app.use(`/plugins/${pluginName}`, router);
  }
  
  // Register plugin middleware
  registerMiddleware(pluginName, middleware) {
    this.app.use(middleware);
  }
  
  // Execute all handlers for a hook
  async executeHook(hookName, ...args) {
    const handlers = this.hooks[hookName] || [];
    
    for (const { handler } of handlers) {
      try {
        if (handler.length > args.length) {
          await handler(...args);
        } else {
          handler(...args);
        }
      } catch (error) {
        console.error(`Error in ${hookName} hook:`, error);
      }
    }
  }
  
  // Get list of registered plugins
  getPlugins() {
    return Array.from(this.plugins.keys());
  }
  
  // Unregister a plugin
  unregister(name) {
    const plugin = this.plugins.get(name);
    if (!plugin) {
      throw new Error(`Plugin ${name} not found`);
    }
    
    if (plugin.destroy) {
      plugin.destroy();
    }
    
    this.plugins.delete(name);
    console.log(`🗑️ Plugin unloaded: ${name}`);
  }
}

const pluginManager = new PluginManager(app);


// ============================================
// Plugin Hook Functions
// ============================================

// Helper to create hook handlers
function createRequestLogger() {
  return async (req, res, next) => {
    console.log(`📝 ${req.method} ${req.path}`);
    next();
  };
}

function createRateLimiterPlugin(maxRequests = 100) {
  const requests = new Map();
  
  return {
    hooks: {
      onRequest: (req, res, next) => {
        const key = req.ip;
        const count = (requests.get(key) || 0) + 1;
        requests.set(key, count);
        
        if (count > maxRequests) {
          return res.status(429).json({ error: 'Too many requests' });
        }
        
        next();
      }
    }
  };
}

function createAnalyticsPlugin() {
  return {
    hooks: {
      postHandler: (req, res) => {
        console.log(`📊 ${req.method} ${req.path} -> ${res.statusCode}`);
      }
    }
  };
}


// ============================================
// Example Plugin: Authentication
// ============================================

function createAuthPlugin() {
  const publicPaths = ['/health', '/api/auth/login', '/api/auth/register'];
  
  return {
    hooks: {
      preHandler: (req, res, next) => {
        if (publicPaths.includes(req.path)) {
          return next();
        }
        
        const token = req.headers.authorization?.replace('Bearer ', '');
        
        if (!token) {
          return res.status(401).json({ error: 'Unauthorized' });
        }
        
        // Simple check - in production use JWT
        req.user = { id: 'user-1', token };
        next();
      }
    }
  };
}


// ============================================
// Example Plugin: Feature Flags
// ============================================

function createFeatureFlagPlugin() {
  const flags = {
    newCheckout: true,
    betaAPI: false
  };
  
  return {
    getFlag(name) {
      return flags[name] || false;
    },
    
    hooks: {
      preHandler: (req, res, next) => {
        req.featureFlags = flags;
        next();
      }
    }
  };
}


// ============================================
// Load Built-in Plugins
// ============================================

console.log('\n🔌 Loading plugins...\n');

pluginManager.register('logger', {
  hooks: {
    onRequest: createRequestLogger()
  }
});

pluginManager.register('rate-limiter', createRateLimiterPlugin(50));

pluginManager.register('analytics', createAnalyticsPlugin());

pluginManager.register('auth', createAuthPlugin());

pluginManager.register('feature-flags', createFeatureFlagPlugin());


// ============================================
// Dynamic Plugin Loading
// ============================================

function loadPluginsFromDirectory(dir) {
  if (!existsSync(dir)) {
    console.log(`📂 Plugin directory not found: ${dir}`);
    return;
  }
  
  const files = readdirSync(dir).filter(f => f.endsWith('.js'));
  
  for (const file of files) {
    try {
      const plugin = await import(join(dir, file));
      const name = file.replace('.js', '');
      pluginManager.register(name, plugin.default || plugin);
    } catch (error) {
      console.error(`Failed to load plugin ${file}:`, error.message);
    }
  }
}

// Example: loadPluginsFromDirectory('./plugins');


// ============================================
// Application Routes
// ============================================

app.use(express.json());

// Health check (public)
app.get('/health', (req, res) => {
  res.json({ 
    status: 'ok',
    plugins: pluginManager.getPlugins()
  });
});

// Auth routes (public)
app.post('/api/auth/login', (req, res) => {
  const { email } = req.body || {};
  res.json({ token: 'fake-jwt-token', email });
});

app.post('/api/auth/register', (req, res) => {
  res.status(201).json({ userId: 'user-new' });
});

// Protected routes
app.get('/api/users', (req, res) => {
  res.json([{ id: '1', name: 'John' }]);
});

app.get('/api/feature-flags', (req, res) => {
  res.json(req.featureFlags || {});
});

// Plugin management endpoints
app.get('/api/plugins', (req, res) => {
  res.json({ plugins: pluginManager.getPlugins() });
});

app.post('/api/plugins/:name/unload', (req, res) => {
  try {
    pluginManager.unregister(req.params.name);
    res.json({ message: 'Plugin unloaded' });
  } catch (error) {
    res.status(400).json({ error: error.message });
  }
});


const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`\n🚀 Server running on port ${PORT}`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 20-30 | `PluginManager` class | Core plugin system |
| 35-48 | `register()` method | Loads and initializes plugins |
| 52-62 | `registerHook()` | Registers hook handlers |
| 65-75 | `registerRoutes()` | Adds plugin routes under /plugins/:name |
| 78-84 | `executeHook()` | Runs all handlers for a hook |
| 117-150 | Rate limiter plugin | Example plugin with hooks |
| 153-165 | Analytics plugin | Post-handler hook example |
| 168-188 | Auth plugin | Pre-handler authentication |
| 191-207 | Feature flags plugin | Feature toggle system |
| 213-223 | Loading built-in plugins | Registers example plugins |
| 226-237 | Dynamic loading | Loads plugins from directory |
| 255-280 | API routes | Demo routes with plugins |

## ⚠️ Common Mistakes

### 1. Not handling plugin errors

**What it is**: One plugin crashing takes down entire app.

**Why it happens**: No error isolation.

**How to fix it**: Wrap plugin code in try/catch, consider process isolation.

### 2. Plugin version incompatibility

**What it is**: Plugins written for older versions break.

**Why it happens**: No version checking.

**How to fix it**: Add API versioning and compatibility checks.

### 3. Too many hooks slowing down

**What it is**: Many plugins all adding middleware.

**Why it happens**: Not optimizing hook execution.

**How to fix it**: Use lazy loading, optimize middleware order.

## ✅ Quick Recap

- Plugin systems allow extending Express without core modifications
- Hook points (onRequest, preHandler, postHandler) allow plugin injection
- Plugins register hooks, routes, and middleware
- Plugin manager orchestrates loading and lifecycle
- Consider error isolation for production systems

## 🔗 What's Next

Now moving to Multi-Tenancy section. Learn about [Multi-Tenant Architecture](./../03_Multi_Tenancy/01_multi-tenant-architecture.md).
