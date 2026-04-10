# 🔗 Promise Chaining Techniques

## 📋 Overview

Promise chaining allows you to execute multiple asynchronous operations in sequence, where each operation starts after the previous one completes. Understanding chaining patterns is essential for writing clean async code.

---

## 🔄 Basic Chaining

### Sequential Execution

```javascript
// Chain promises one after another
fetch('/api/user')
    .then(response => response.json())
    .then(user => fetch(`/api/posts/${user.id}`))
    .then(response => response.json())
    .then(posts => console.log('Posts:', posts.length))
    .catch(error => console.error('Error:', error));
```

### Reusing Functions

```javascript
// Break into reusable functions
function getUser() {
    return fetch('/api/user').then(r => r.json());
}

function getUserPosts(userId) {
    return fetch(`/api/posts/${userId}`).then(r => r.json());
}

function getUserFriends(userId) {
    return fetch(`/api/friends/${userId}`).then(r => r.json());
}

// Chain them
getUser()
    .then(user => getUserPosts(user.id))
    .then(posts => getUserFriends(posts[0].userId))
    .then(friends => console.log('First friend:', friends[0].name))
    .catch(console.error);
```

---

## 🎯 Advanced Patterns

### Parallel vs Sequential

```javascript
// Sequential - each waits for previous
async function sequential(userId) {
    const user = await fetchUser(userId);        // 1s
    const posts = await fetchPosts(user.id);    // 1s
    const friends = await fetchFriends(user.id); // 1s
    return { user, posts, friends }; // Total: 3s
}

// Parallel - all start together
async function parallel(userId) {
    const [user, posts, friends] = await Promise.all([
        fetchUser(userId),
        fetchPosts(userId),
        fetchFriends(userId)
    ]);
    return { user, posts, friends }; // Total: 1s!
}
```

### Mixed Pattern

```javascript
async function getDashboardData(userId) {
    // First get user (must be first)
    const user = await fetchUser(userId);
    
    // Then get posts and settings in parallel
    const [posts, settings] = await Promise.all([
        fetchPosts(user.id),
        fetchSettings()
    ]);
    
    // Finally get friends (depends on user)
    const friends = await fetchFriends(user.id);
    
    return { user, posts, settings, friends };
}
```

---

## 🎯 Error Recovery Patterns

### Retry Pattern

```javascript
function withRetry(promiseFn, maxRetries = 3, delayMs = 1000) {
    return new Promise((resolve, reject) => {
        let attempts = 0;
        
        function attempt() {
            promiseFn()
                .then(resolve)
                .catch(error => {
                    attempts++;
                    if (attempts < maxRetries) {
                        console.log(`Retry ${attempts}/${maxRetries}`);
                        setTimeout(attempt, delayMs * attempts);
                    } else {
                        reject(error);
                    }
                });
        }
        
        attempt();
    });
}

// Usage
const data = await withRetry(() => fetch('/api/data').then(r => r.json()));
```

### Fallback Pattern

```javascript
async function fetchWithFallback(primary, fallback) {
    try {
        return await primary();
    } catch (error) {
        console.warn('Primary failed, trying fallback:', error.message);
        return await fallback();
    }
}

// Usage
const data = await fetchWithFallback(
    () => fetch('/api/premium-data').then(r => r.json()),
    () => fetch('/api/free-data').then(r => r.json())
);
```

---

## 🧪 Practice Exercise

### Promise Queue

```javascript
class PromiseQueue {
    constructor(concurrency = 1) {
        this.concurrency = concurrency;
        this.running = 0;
        this.queue = [];
    }
    
    add(promiseFn) {
        return new Promise((resolve, reject) => {
            this.queue.push({ promiseFn, resolve, reject });
            this.process();
        });
    }
    
    process() {
        while (this.running < this.concurrency && this.queue.length > 0) {
            const { promiseFn, resolve, reject } = this.queue.shift();
            this.running++;
            
            promiseFn()
                .then(resolve)
                .catch(reject)
                .finally(() => {
                    this.running--;
                    this.process();
                });
        }
    }
}

// Usage
const queue = new PromiseQueue(2);

queue.add(() => fetch('/api/1').then(r => r.json()))
    .then(data => console.log('Task 1:', data));

queue.add(() => fetch('/api/2').then(r => r.json()))
    .then(data => console.log('Task 2:', data));

queue.add(() => fetch('/api/3').then(r => r.json()))
    .then(data => console.log('Task 3:', data));
```

---

## 🔗 Related Topics

- [02_Promises_Complete_Guide.md](./02_Promises_Complete_Guide.md)
- [03_Async_Await_Master_Class.md](./03_Async_Await_Master_Class.md)

---

**Next: Learn about [Error Handling in Async](./05_Error_Handling_in_Async.md)**