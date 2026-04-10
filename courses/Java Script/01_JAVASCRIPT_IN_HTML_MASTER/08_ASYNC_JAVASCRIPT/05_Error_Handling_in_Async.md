# ⚠️ Error Handling in Async

## 📋 Overview

Proper error handling in asynchronous code is crucial for building robust applications. This guide covers patterns and best practices for handling errors in Promises and async/await code.

---

## 🔍 Understanding Async Errors

### Promise Rejection

```javascript
// Promise can be rejected
const promise = new Promise((resolve, reject) => {
    // Reject with error
    reject(new Error('Something went wrong'));
});

// Or throw in executor
const promise2 = new Promise((resolve, reject) => {
    throw new Error('Exception in executor');
});
```

### Error Propagation

```javascript
// Errors propagate through the chain
fetch('/api/data')
    .then(data => {
        // If this throws, goes to next .catch()
        throw new Error('Processing failed');
    })
    .catch(error => {
        // Catches errors from previous .then()
        console.error(error.message);
    });
```

---

## 🎯 Error Handling Patterns

### try...catch with async/await

```javascript
async function fetchData() {
    try {
        const response = await fetch('/api/data');
        
        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`);
        }
        
        const data = await response.json();
        return data;
        
    } catch (error) {
        // Network errors, parse errors, thrown errors
        console.error('Fetch failed:', error.message);
        throw error;
    }
}
```

### Multiple Error Types

```javascript
async function handleErrors() {
    try {
        const user = await fetchUser();
        const posts = await fetchPosts();
        const settings = await fetchSettings();
        
        return { user, posts, settings };
        
    } catch (error) {
        if (error.name === 'TypeError') {
            // Network/parsing error
            return { error: 'Network error', fallback: true };
        }
        
        if (error.status === 404) {
            // Not found
            return { error: 'Resource not found' };
        }
        
        throw error; // Re-throw unknown errors
    }
}
```

---

## 🎯 Global Error Handling

### Unhandled Rejections

```javascript
// Handle unhandled promise rejections
window.addEventListener('unhandledrejection', (event) => {
    console.error('Unhandled rejection:', event.reason);
    // Report to error tracking service
    reportError(event.reason);
});

// Prevent default (in Node.js)
process.on('unhandledRejection', (reason, promise) => {
    console.error('Unhandled Rejection at:', promise, 'reason:', reason);
});
```

---

## 🎯 Practice Exercise

### Error Boundary Wrapper

```javascript
async function withErrorBoundary(asyncFn, errorHandler) {
    try {
        return await asyncFn();
    } catch (error) {
        return errorHandler(error);
    }
}

// Usage
const user = await withErrorBoundary(
    () => fetchUser(123),
    error => ({ name: 'Guest', error: error.message })
);
```

---

## 🔗 Related Topics

- [02_Promises_Complete_Guide.md](./02_Promises_Complete_Guide.md)
- [03_Async_Await_Master_Class.md](./03_Async_Await_Master_Class.md)

---

**Next: Learn about [Promise.all, allSettled, race](./06_Promise_All_AllSettled_Race.md)**