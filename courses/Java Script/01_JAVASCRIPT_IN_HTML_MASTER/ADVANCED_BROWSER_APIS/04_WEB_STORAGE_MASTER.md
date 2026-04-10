# 💾 Web Storage Advanced Complete Guide

## Modern Storage Solutions in JavaScript

---

## Table of Contents

1. [Local Storage](#local-storage)
2. [Session Storage](#session-storage)
3. [IndexedDB](#indexeddb)
4. [Cache API](#cache-api)
5. [Storage Manager](#storage-manager)

---

## Local Storage

### Basic Usage

```javascript
// Set item
localStorage.setItem('name', 'John');

// Get item
const name = localStorage.getItem('name');

// Remove item
localStorage.removeItem('name');

// Clear all
localStorage.clear();
```

### JSON Storage

```javascript
// Store object
const user = { name: 'John', age: 30 };
localStorage.setItem('user', JSON.stringify(user));

// Retrieve object
const userData = JSON.parse(localStorage.getItem('user'));
```

---

## Session Storage

### Usage

```javascript
// Set
sessionStorage.setItem('key', 'value');

// Get
const value = sessionStorage.getItem('key');
```

---

## IndexedDB

### Opening Database

```javascript
const request = indexedDB.open('MyDatabase', 1);

request.onerror = () => console.error('Error');

request.onupgradeneeded = (e) => {
  const db = e.target.result;
  const store = db.createObjectStore('users', { keyPath: 'id' });
};

request.onsuccess = (e) => {
  const db = e.target.result;
  console.log('Database opened');
};
```

### CRUD Operations

```javascript
// Add
function addUser(user) {
  const transaction = db.transaction(['users'], 'readwrite');
  const store = transaction.objectStore('users');
  store.add(user);
}

// Get
function getUser(id) {
  const transaction = db.transaction(['users'], 'readonly');
  const store = transaction.objectStore('users');
  return store.get(id);
}

// Delete
function deleteUser(id) {
  const transaction = db.transaction(['users'], 'readwrite');
  const store = transaction.objectStore('users');
  store.delete(id);
}
```

---

## Cache API

### Caching Requests

```javascript
async function cacheRequest(url) {
  const cache = await caches.open('my-cache');
  await cache.add(url);
}

async function getCached(url) {
  const cache = await caches.open('my-cache');
  return cache.match(url);
}
```

---

## Storage Manager

### Check Quota

```javascript
async function checkQuota() {
  if (navigator.storage && navigator.storage.estimate) {
    const { usage, quota } = await navigator.storage.estimate();
    console.log(`Using ${usage} of ${quota} bytes`);
  }
}
```

### Request Persistent Storage

```javascript
async function requestPersistent() {
  if (navigator.storage && navigator.storage.persist) {
    const isPersisted = await navigator.storage.persist();
    console.log('Persisted:', isPersisted);
  }
}
```

---

## Summary

### Key Takeaways

1. **LocalStorage**: Simple key-value
2. **SessionStorage**: Session-scoped
3. **IndexedDB**: Complex storage
4. **Cache API**: Request caching
5. **Storage Manager**: Quota management

### Next Steps

- Continue with: [05_PWA_MASTER.md](05_PWA_MASTER.md)
- Implement offline-first
- Study storage encryption

---

## Cross-References

- **Previous**: [03_WEB_AUDIO_API_MASTER.md](03_WEB_AUDIO_API_MASTER.md)
- **Next**: [05_PWA_MASTER.md](05_PWA_MASTER.md)

---

*Last updated: 2024*