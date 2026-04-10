# ⚡ Promises Complete Guide

## 📋 Overview

Promises are fundamental to modern JavaScript asynchronous programming. They represent the eventual completion (or failure) of an asynchronous operation and provide a cleaner alternative to callback-based code.

---

## 🏗️ What are Promises?

### Promise States

A Promise can be in one of three states:

```
┌─────────────────────────────────────────────────┐
│                 PENDING                         │
│          (initial state, not fulfilled          │
│                   or rejected)                  │
└────────────────────┬────────────────────────────┘
                    │
        ┌───────────┴───────────┐
        ▼                       ▼
┌─────────────────┐    ┌─────────────────┐
│    RESOLVED     │    │    REJECTED    │
│ (fulfilled)     │    │    (failed)    │
└─────────────────┘    └─────────────────┘
```

### Creating a Promise

```javascript
const myPromise = new Promise((resolve, reject) => {
    console.log('Promise created');
    
    // Simulate async operation
    setTimeout(() => {
        const success = true;
        
        if (success) {
            resolve('Success! Data loaded.');
        } else {
            reject('Error! Something went wrong.');
        }
    }, 1000);
});

myPromise
    .then(result => console.log(result))
    .catch(error => console.error(error));
```

---

## 🌍 Real-World Promise Applications

### Scenario 1: API Data Fetching

```javascript
class WeatherService {
    constructor() {
        this.apiKey = 'demo-api-key';
        this.baseURL = 'https://api.openweathermap.org';
    }
    
    async fetchWeather(city) {
        return new Promise(async (resolve, reject) => {
            try {
                const response = await fetch(
                    `${this.baseURL}/data/2.5/weather?q=${city}&appid=${this.apiKey}`
                );
                
                if (!response.ok) {
                    throw new Error(`Weather not found: ${response.status}`);
                }
                
                const data = await response.json();
                resolve({
                    city: data.name,
                    temp: data.main.temp,
                    humidity: data.main.humidity,
                    description: data.weather[0].description
                });
            } catch (error) {
                reject(error);
            }
        });
    }
}

// Usage
const weather = new WeatherService();

weather.fetchWeather('London')
    .then(data => {
        console.log(`Temperature in ${data.city}: ${data.temp}°C`);
        console.log(`Conditions: ${data.description}`);
    })
    .catch(error => console.error('Error:', error.message));
```

### Scenario 2: Image Loading

```javascript
function loadImage(url) {
    return new Promise((resolve, reject) => {
        const img = new Image();
        
        img.onload = () => resolve(img);
        img.onerror = () => reject(new Error(`Failed to load: ${url}`));
        
        img.src = url;
    });
}

// Usage
loadImage('https://example.com/image.jpg')
    .then(img => {
        document.body.appendChild(img);
    })
    .catch(error => {
        console.error(error.message);
    });
```

---

## 🔧 Promise Methods

### Promise.all()

Wait for all promises to resolve:

```javascript
const urls = [
    'https://api.example.com/users',
    'https://api.example.com/posts',
    'https://api.example.com/comments'
];

const requests = urls.map(url => fetch(url).then(r => r.json()));

Promise.all(requests)
    .then(([users, posts, comments]) => {
        console.log('Users:', users.length);
        console.log('Posts:', posts.length);
        console.log('Comments:', comments.length);
    })
    .catch(error => console.error('One or more requests failed:', error));
```

### Promise.race()

Returns first promise to settle (resolve or reject):

```javascript
function timeout(ms) {
    return new Promise((_, reject) => 
        setTimeout(() => reject(new Error('Timeout!')), ms)
    );
}

Promise.race([
    fetch('https://slow-api.example.com/data'),
    timeout(5000)
])
    .then(response => response.json())
    .catch(error => console.error(error));
```

### Promise.allSettled()

Wait for all promises to settle (regardless of success/failure):

```javascript
const requests = [
    fetch('https://api1.example.com/data'),
    fetch('https://api2.example.com/data'),
    fetch('https://api3.example.com/data')
];

Promise.allSettled(requests)
    .then(results => {
        results.forEach((result, index) => {
            if (result.status === 'fulfilled') {
                console.log(`Request ${index}: Success -`, result.value);
            } else {
                console.log(`Request ${index}: Failed -`, result.reason);
            }
        });
    });
```

### Promise.any()

Returns first promise that resolves:

```javascript
Promise.any([
    fetch('https://primary-api.example.com/data').catch(() => null),
    fetch('https://backup-api.example.com/data').catch(() => null),
    fetch('https://fallback-api.example.com/data')
])
    .then(result => console.log('First successful response:', result))
    .catch(error => console.error('All promises failed'));
```

---

## 🔗 Chaining Promises

### Sequential Execution

```javascript
// Each .then() returns a promise
fetchUser(1)
    .then(user => fetchUserPosts(user.id))
    .then(posts => fetchPostComments(posts[0].id))
    .then(comments => console.log('Comments:', comments.length))
    .catch(error => console.error('Error:', error));

function fetchUser(id) {
    return fetch(`/api/users/${id}`).then(r => r.json());
}

function fetchUserPosts(userId) {
    return fetch(`/api/users/${userId}/posts`).then(r => r.json());
}

function fetchPostComments(postId) {
    return fetch(`/api/posts/${postId}/comments`).then(r => r.json());
}
```

### Parallel with Coordination

```javascript
async function fetchDashboardData() {
    // Fetch all data in parallel
    const [user, notifications, messages] = await Promise.all([
        fetch('/api/user').then(r => r.json()),
        fetch('/api/notifications').then(r => r.json()),
        fetch('/api/messages').then(r => r.json())
    ]);
    
    return { user, notifications, messages };
}
```

---

## 🎯 Error Handling

### try...catch with Promises

```javascript
async function fetchData() {
    try {
        const response = await fetch('https://api.example.com/data');
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Fetch error:', error);
        throw error; // Re-throw for caller to handle
    }
}
```

### Handling Multiple Errors

```javascript
Promise.all([
    fetchUser().catch(e => ({ error: 'user', message: e.message })),
    fetchPosts().catch(e => ({ error: 'posts', message: e.message })),
    fetchSettings().catch(e => ({ error: 'settings', message: e.message }))
])
    .then(results => {
        const errors = results.filter(r => r.error);
        if (errors.length > 0) {
            console.warn('Some requests failed:', errors);
        }
        return results;
    });
```

---

## 🎯 Practice Exercises

### Exercise 1: Promise-based File Uploader

```javascript
function uploadFile(file) {
    return new Promise((resolve, reject) => {
        if (!file) {
            reject(new Error('No file provided'));
            return;
        }
        
        // Simulate upload
        const progress = setInterval(() => {
            // Update progress (would be real upload progress)
        }, 100);
        
        setTimeout(() => {
            clearInterval(progress);
            resolve({
                name: file.name,
                size: file.size,
                url: `https://uploads.example.com/${file.name}`
            });
        }, 2000);
    });
}

// Usage
uploadFile({ name: 'image.jpg', size: 1024000 })
    .then(result => console.log('Uploaded:', result))
    .catch(error => console.error(error));
```

### Exercise 2: Retry Logic

```javascript
function fetchWithRetry(url, maxRetries = 3) {
    return new Promise((resolve, reject) => {
        let attempts = 0;
        
        function attempt() {
            fetch(url)
                .then(response => {
                    if (!response.ok) throw new Error('Request failed');
                    resolve(response.json());
                })
                .catch(error => {
                    attempts++;
                    if (attempts < maxRetries) {
                        console.log(`Retry ${attempts}/${maxRetries}...`);
                        setTimeout(attempt, 1000 * attempts);
                    } else {
                        reject(error);
                    }
                });
        }
        
        attempt();
    });
}
```

---

## 📊 Promise Methods Quick Reference

| Method | Description | Use Case |
|--------|-------------|----------|
| `new Promise()` | Create a promise | Wrap callback APIs |
| `.then()` | Handle resolved value | Process result |
| `.catch()` | Handle rejection | Error handling |
| `.finally()` | Always runs | Cleanup |
| `Promise.all()` | Wait for all | Parallel operations |
| `Promise.race()` | First to settle | Timeouts |
| `Promise.allSettled()` | All settled | Status reports |
| `Promise.any()` | First to resolve | Fallbacks |

---

## 🔗 Related Topics

- [01_Async_JavaScript_Basics.md](./01_Async_JavaScript_Basics.md)
- [03_Async_Await_Master_Class.md](./03_Async_Await_Master_Class.md)

---

**Next: Learn about [Async/Await Master Class](./03_Async_Await_Master_Class.md)**