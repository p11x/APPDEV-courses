# ⚡ Promise.all, allSettled, Race

## 📋 Overview

JavaScript provides several Promise combinators for handling multiple promises simultaneously. Understanding when to use each is essential for building robust async applications.

---

## 🎯 Promise.all()

Waits for all promises to resolve. Fails fast if any promise rejects.

```javascript
// Basic Promise.all
const urls = [
    'https://api.example.com/users',
    'https://api.example.com/posts',
    'https://api.example.com/comments'
];

Promise.all(urls.map(url => fetch(url).then(r => r.json())))
    .then(([users, posts, comments]) => {
        console.log('All loaded:', { users, posts, comments });
    })
    .catch(error => {
        console.error('One failed, all failed:', error);
    });
```

### Real-World: Dashboard Data

```javascript
async function loadDashboard() {
    const [user, notifications, messages] = await Promise.all([
        fetchUser(),
        fetchNotifications(),
        fetchMessages()
    ]);
    
    return { user, notifications, messages };
}
```

---

## 🎯 Promise.allSettled()

Waits for all promises to settle (resolve or reject). Never fails.

```javascript
// All settles even if some fail
const results = await Promise.allSettled([
    fetch('/api/users').then(r => r.json()),
    fetch('/api/invalid').then(r => r.json()), // Will fail
    fetch('/api/settings').then(r => r.json())
]);

results.forEach((result, index) => {
    if (result.status === 'fulfilled') {
        console.log(`Task ${index}: Success`, result.value);
    } else {
        console.log(`Task ${index}: Failed`, result.reason);
    }
});
```

---

## 🎯 Promise.race()

Returns the first promise to settle (resolve or reject).

```javascript
// Timeout pattern
function withTimeout(promise, ms) {
    return Promise.race([
        promise,
        new Promise((_, reject) => 
            setTimeout(() => reject(new Error('Timeout')), ms)
        )
    ]);
}

// Usage
const data = await withTimeout(fetch('/api/data'), 5000);
```

---

## 🎯 Promise.any()

Returns the first promise to resolve. Ignores rejections.

```javascript
// Try multiple sources, use first success
const data = await Promise.any([
    fetchPrimaryAPI(),
    fetchBackupAPI(),
    fetchFallbackAPI()
]);
```

---

## 🔗 Related Topics

- [02_Promises_Complete_Guide.md](./02_Promises_Complete_Guide.md)
- [03_Async_Await_Master_Class.md](./03_Async_Await_Master_Class.md)

---

**Next: Learn about [Async Function Patterns](./07_Async_Function_Patterns.md)**