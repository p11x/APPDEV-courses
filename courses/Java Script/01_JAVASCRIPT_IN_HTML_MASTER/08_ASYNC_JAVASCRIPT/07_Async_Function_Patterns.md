# Async Function Patterns

## 🎯 Overview

Asynchronous programming in JavaScript has evolved significantly over the years. While `async/await` has become the standard approach, understanding various async function patterns helps you write cleaner, more maintainable code. This guide covers essential patterns that every JavaScript developer should know.

---

## 🚀 Async IIFE (Immediately Invoked Function Expressions)

Async IIFE allows you to execute async code immediately at the top level or within other functions without waiting for the entire module to load.

```javascript
// Basic async IIFE
(async () => {
  const data = await fetch('https://api.example.com/data');
  const json = await data.json();
  console.log('Data fetched:', json);
})();

// Async IIFE with error handling
(async () => {
  try {
    const response = await fetch('https://api.example.com/user');
    const user = await response.json();
    console.log('User:', user);
  } catch (error) {
    console.error('Failed to fetch user:', error);
  }
})();

// Async IIFE for initialization
const app = (async () => {
  const config = await loadConfig();
  const db = await connectDatabase(config.database);
  return { config, db };
})();
```

### When to Use Async IIFE

| Use Case | Example |
|----------|---------|
| Module initialization | Loading config, connecting to databases |
| One-time async setup | Setting up event listeners with async data |
| Blocking async operations | Ensuring async work completes before continuing |

---

## 📦 Async Class Methods

Classes can contain async methods for performing asynchronous operations. This is particularly useful for API clients, services, and data repositories.

```javascript
class UserService {
  constructor(baseUrl) {
    this.baseUrl = baseUrl;
  }

  // Async method to fetch users
  async getUsers() {
    try {
      const response = await fetch(`${this.baseUrl}/users`);
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      return await response.json();
    } catch (error) {
      console.error('Error fetching users:', error);
      throw error;
    }
  }

  // Async method with parameters
  async getUserById(id) {
    const response = await fetch(`${this.baseUrl}/users/${id}`);
    if (!response.ok) {
      throw new Error(`User not found: ${id}`);
    }
    return await response.json();
  }

  // Async method that calls other async methods
  async getUserWithPosts(userId) {
    const [user, posts] = await Promise.all([
      this.getUserById(userId),
      this.getPosts(userId)
    ]);
    return { user, posts };
  }

  async getPosts(userId) {
    const response = await fetch(`${this.baseUrl}/users/${userId}/posts`);
    return await response.json();
  }
}

// Usage
const userService = new UserService('https://api.example.com');
const users = await userService.getUsers();
```

### Static Async Methods

```javascript
class ApiClient {
  static async createClient(apiKey) {
    const client = new ApiClient(apiKey);
    await client.initialize();
    return client;
  }

  constructor(apiKey) {
    this.apiKey = apiKey;
    this.initialized = false;
  }

  async initialize() {
    // Perform async initialization
    const token = await this.authenticate();
    this.token = token;
    this.initialized = true;
  }

  async authenticate() {
    // Async auth logic
    return 'auth-token';
  }
}

// Usage without needing to instantiate first
const client = await ApiClient.createClient('my-api-key');
```

---

## 🔑 Async Object Methods

Objects can have async methods for encapsulating async behavior in a more lightweight way than classes.

```javascript
const dataProcessor = {
  cache: new Map(),

  async fetchData(url) {
    // Check cache first
    if (this.cache.has(url)) {
      console.log('Returning cached data');
      return this.cache.get(url);
    }

    // Fetch and cache
    const response = await fetch(url);
    const data = await response.json();
    this.cache.set(url, data);
    return data;
  },

  async processWithRetry(url, maxRetries = 3) {
    for (let attempt = 1; attempt <= maxRetries; attempt++) {
      try {
        return await this.fetchData(url);
      } catch (error) {
        if (attempt === maxRetries) throw error;
        console.log(`Retry ${attempt}/${maxRetries}`);
        await new Promise(r => setTimeout(r, 1000 * attempt));
      }
    }
  },

  async batchProcess(urls) {
    return Promise.all(urls.map(url => this.fetchData(url)));
  }
};
```

### Factory Pattern with Async Methods

```javascript
const createApiClient = async (config) => {
  const token = await authenticate(config.apiKey);
  
  return {
    baseUrl: config.baseUrl,
    token,
    
    async get(endpoint) {
      const response = await fetch(`${this.baseUrl}${endpoint}`, {
        headers: { 'Authorization': `Bearer ${this.token}` }
      });
      return response.json();
    },
    
    async post(endpoint, data) {
      const response = await fetch(`${this.baseUrl}${endpoint}`, {
        method: 'POST',
        headers: { 
          'Authorization': `Bearer ${this.token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
      });
      return response.json();
    }
  };
};

// Usage
const api = await createApiClient({ 
  apiKey: 'secret', 
  baseUrl: 'https://api.example.com' 
});
const data = await api.get('/users');
```

---

## 🌊 Async Generators

Async generators combine generators with async iteration, allowing you to produce values asynchronously over time.

```javascript
// Basic async generator
async function* fetchPages(urls) {
  for (const url of urls) {
    const response = await fetch(url);
    const data = await response.json();
    yield data; // Pause and emit one value at a time
  }
}

// Usage
async function main() {
  const urls = [
    'https://api.example.com/page1',
    'https://api.example.com/page2',
    'https://api.example.com/page3'
  ];

  for await (const page of fetchPages(urls)) {
    console.log('Page:', page);
  }
}

// Async generator with error handling
async function* watchMessages(userId) {
  let lastMessageId = 0;
  
  while (true) {
    try {
      const messages = await fetch(
        `/api/messages?user=${userId}&after=${lastMessageId}`
      ).then(r => r.json());
      
      for (const message of messages) {
        yield message;
        lastMessageId = message.id;
      }
      
      await new Promise(r => setTimeout(r, 5000)); // Poll every 5 seconds
    } catch (error) {
      console.error('Connection error, retrying...');
      await new Promise(r => setTimeout(r, 10000)); // Back off on error
    }
  }
}

// Async generator for processing streams
async function* processLargeFile(file) {
  const chunkSize = 1024 * 1024; // 1MB chunks
  
  for await (const chunk of file.stream()) {
    const processed = await processChunk(chunk);
    yield processed;
  }
}
```

### Combining Async Generators with Promise Methods

```javascript
async function* asyncMap(iterable, asyncFn) {
  for await (const item of iterable) {
    yield await asyncFn(item);
  }
}

async function* asyncFilter(iterable, asyncPredicate) {
  for await (const item of iterable) {
    if (await asyncPredicate(item)) {
      yield item;
    }
  }
}

// Usage
const processedNumbers = asyncMap(
  [1, 2, 3, 4, 5],
  async (n) => n * 2
);

for await (const num of processedNumbers) {
  console.log(num); // 2, 4, 6, 8, 10
}
```

---

## ⚡ Top-Level Await (ES2022)

Top-level await allows you to use `await` at the module top level without wrapping in an async function. This is only available in ES modules.

```javascript
// utils/api.js - Module initialization with top-level await
const config = await fetch('/api/config').then(r => r.json());
const apiClient = createClient(config);

export { apiClient };

// main.js - Using imported async data
import { apiClient } from './utils/api.js';

// No need to wrap in async function
const users = await apiClient.getUsers();

// Module-level parallel async operations
const [cachedData, session] = await Promise.all([
  loadCachedData(),
  initializeSession()
]);

export { cachedData, session };
```

### Common Top-Level Await Patterns

```javascript
// Dynamic module loading
const dynamicModule = await import('./dynamic-module.js');

// Environment-based configuration
const isProduction = process.env.NODE_ENV === 'production';
const config = isProduction
  ? await import('./config.prod.js')
  : await import('./config.dev.js');

// Lazy initialization
let cache = null;

export async function getCache() {
  if (!cache) {
    cache = await initializeCache();
  }
  return cache;
}
```

### Comparison: Top-Level Await vs Async IIFE

| Aspect | Top-Level Await | Async IIFE |
|--------|-----------------|-------------|
| Scope | Module level | Anywhere |
| ES Modules | Required | Not required |
| Blocking | Blocks entire module | Blocks only that scope |
| Browser Support | Modern browsers | All browsers |
| Use Case | Module initialization | Local async operations |

---

## 💼 Real-World Application Patterns

### Pattern 1: Async Module Registry

```javascript
const modules = new Map();

// Lazy load and cache modules
async function getModule(name) {
  if (modules.has(name)) {
    return modules.get(name);
  }
  
  const module = await import(`./modules/${name}.js`);
  modules.set(name, module);
  return module;
}

// Preload critical modules
export const core = await getModule('core');
```

### Pattern 2: Async Event Queue

```javascript
class AsyncEventEmitter {
  constructor() {
    this.events = new Map();
  }

  async on(event, handler) {
    if (!this.events.has(event)) {
      this.events.set(event, []);
    }
    this.events.get(event).push(handler);
  }

  async emit(event, data) {
    const handlers = this.events.get(event) || [];
    await Promise.all(handlers.map(handler => handler(data)));
  }
}

// Usage
const emitter = new AsyncEventEmitter();
emitter.on('user:created', async (user) => {
  await sendWelcomeEmail(user);
  await logAnalytics('user_created', user);
});
```

### Pattern 3: Async Data Pipeline

```javascript
async function* createPipeline(source, ...processors) {
  for await (const data of source) {
    let result = data;
    for (const processor of processors) {
      result = await processor(result);
    }
    yield result;
  }
}

// Usage
const rawData = fetchRawData();
const validated = createPipeline(rawData, validate, transform, enrich);
const final = createPipeline(validated, sanitize, cache);

for await (const item of final) {
  console.log('Processed:', item);
}
```

### Pattern 4: Retry with Backoff

```javascript
async function withRetry(fn, options = {}) {
  const {
    maxAttempts = 3,
    initialDelay = 1000,
    backoffMultiplier = 2,
    shouldRetry = () => true
  } = options;

  let lastError;
  
  for (let attempt = 1; attempt <= maxAttempts; attempt++) {
    try {
      return await fn();
    } catch (error) {
      lastError = error;
      
      if (attempt === maxAttempts || !shouldRetry(error)) {
        throw error;
      }
      
      const delay = initialDelay * Math.pow(backoffMultiplier, attempt - 1);
      await new Promise(r => setTimeout(r, delay));
    }
  }
  
  throw lastError;
}

// Usage
const data = await withRetry(
  () => fetch('https://api.example.com/data').then(r => r.json()),
  { maxAttempts: 5, initialDelay: 500 }
);
```

---

## 📊 Summary Table

| Pattern | Best For | Key Benefit |
|---------|----------|-------------|
| Async IIFE | Immediate async execution | No async wrapper needed |
| Async Class Methods | OOP code organization | Reusable, testable |
| Async Object Methods | Lightweight services | Simple, flexible |
| Async Generators | Stream processing | Memory efficient |
| Top-Level Await | Module initialization | Clean module setup |

---

## 🔗 Related Topics

- **[Promises Complete Guide](../02_Promises_Complete_Guide.md)** - Understanding the foundation of async operations
- **[Async/Await Master Class](../03_Async_Await_Master_Class.md)** - Deep dive into async/await syntax
- **[Error Handling in Async](../05_Error_Handling_in_Async.md)** - Managing errors in async code
- **[Promise.all, AllSettled, Race](../06_Promise_All_AllSettled_Race.md)** - Concurrent async operations

---

## ✅ Best Practices

1. **Always handle errors** - Use try/catch or .catch() for every async operation
2. **Avoid blocking top-level await** - Be mindful of module load times
3. **Use Promise.all for parallel operations** - Don't await sequentially when possible
4. **Choose the right pattern** - Use classes for complex state, objects for simple utilities
5. **Consider cancellation** - Use AbortController for long-running async operations
6. **Test async code thoroughly** - Use libraries like Jest's async testing utilities