# 🔄 Asynchronous JavaScript Basics

## 📋 Overview

Asynchronous JavaScript allows your code to handle operations that take time (like fetching data, waiting for user input) without freezing the entire application.

---

## ⏱️ Callbacks

### Basic Callback

```javascript
// Function that takes a callback
function fetchData(callback) {
    setTimeout(() => {
        callback({ name: "John", age: 30 });
    }, 1000);
}

// Using the callback
fetchData(function(data) {
    console.log(data); // { name: "John", age: 30 }
});
```

### Callback Hell

```javascript
// Nested callbacks - hard to read
fetchData(function(data) {
    processData(data, function(processed) {
        saveData(processed, function(saved) {
            console.log(saved);
        });
    });
});
```

---

## 🌊 Promises

### Creating Promises

```javascript
// Create a promise
const myPromise = new Promise((resolve, reject) => {
    const success = true;
    
    if (success) {
        resolve("Operation successful!");
    } else {
        reject("Operation failed!");
    }
});

// Using the promise
myPromise
    .then(result => console.log(result))
    .catch(error => console.error(error));
```

### Promise Chain

```javascript
fetch('https://api.example.com/data')
    .then(response => response.json())
    .then(data => console.log(data))
    .then(() => console.log("All done!"))
    .catch(error => console.error(error));
```

---

## ⏳ async/await

### Basic Syntax

```javascript
// async function
async function fetchData() {
    try {
        const response = await fetch('https://api.example.com/data');
        const data = await response.json();
        return data;
    } catch (error) {
        console.error(error);
    }
}

// Call the function
fetchData().then(data => console.log(data));
```

### Parallel Execution

```javascript
// Sequential (slower)
const user = await fetchUser();
const posts = await fetchPosts();

// Parallel (faster)
const [userResponse, postsResponse] = await Promise.all([
    fetch('/api/user'),
    fetch('/api/posts')
]);
```

---

## 📊 Comparison

| Approach | Pros | Cons |
|----------|------|------|
| Callbacks | Simple | Callback hell |
| Promises | Chainable, error handling | Still needs .then() |
| async/await | Readable, synchronous look | Requires try/catch |

---

## 🔗 Related Topics

- [08_ASYNC_JAVASCRIPT/01_Callbacks_Deep_Dive.md](./08_ASYNC_JAVASCRIPT/01_Callbacks_Deep_Dive.md)
- [08_ASYNC_JAVASCRIPT/02_Promises_Complete.md](./08_ASYNC_JAVASCRIPT/02_Promises_Complete.md)

---

**Next: Learn about [Fetch API](./08_ASYNC_JAVASCRIPT/03_Fetch_API_Complete.md)**