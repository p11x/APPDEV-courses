# 🎯 Async Context Management

## 📋 Overview

Async context management allows tracking the context (like request IDs, user data) across asynchronous operations. This is crucial for debugging, logging, and maintaining state in async-heavy applications.

---

## 🔍 Why Context Matters

### Problem: Lost Context

```javascript
// Without context - hard to trace
async function fetchUserData(userId) {
    const user = await getUser(userId);      // Which request?
    const posts = await getPosts(user.id);   // What user?
    const friends = await getFriends(user.id);
    return { user, posts, friends };
}
```

### Solution: Async Context

```javascript
// With async context - trackable
const asyncHooks = require('async_hooks');

// Create context
const context = asyncHooks.createContext();

// Store request ID
asyncHooks.executeAsyncContext(context, () => {
    context.set('requestId', 'req-123');
    return fetchUserData(1);
});
```

---

## 🔄 Context in Modern Frameworks

### Express.js Style

```javascript
// Add request ID to each request
app.use((req, res, next) => {
    req.id = crypto.randomUUID();
    next();
});

// Use in async handlers
app.get('/api/users', async (req, res) => {
    console.log('Request:', req.id);
    // All async calls can access req.id
});
```

### Node.js Diagnostics

```javascript
// Use AsyncLocalStorage (Node.js 12+)
import { AsyncLocalStorage } from 'async_hooks';

const requestContext = new AsyncLocalStorage();

app.use((req, res, next) => {
    requestContext.run({ requestId: req.id }, () => next());
});

// Access anywhere in async chain
function getUserData(id) {
    const { requestId } = requestContext.getStore();
    console.log('Fetching user for request:', requestId);
}
```

---

## 🔗 Related Topics

- [02_Promises_Complete_Guide.md](./02_Promises_Complete_Guide.md)
- [03_Async_Await_Master_Class.md](./03_Async_Await_Master_Class.md)
- [10_Event_Loop_Deep_Dive.md](./10_Event_Loop_Deep_Dive.md)

---

**Complete Async JavaScript Module!** 

Now you're ready to continue with DOM manipulation or start building projects!