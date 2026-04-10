# 🔄 Service Workers Complete Guide

## Background Processing and Caching

---

## Table of Contents

1. [Service Worker Basics](#service-worker-basics)
2. [Lifecycle](#lifecycle)
3. [Caching Strategies](#caching-strategies)
4. [Advanced Caching](#advanced-caching)
5. [Workbox](#workbox)

---

## Service Worker Basics

### Registration

```javascript
if ('serviceWorker' in navigator) {
  navigator.serviceWorker.register('/sw.js')
    .then(reg => console.log('Registered'))
    .catch(err => console.error(err));
}
```

---

## Lifecycle

### Install

```javascript
self.addEventListener('install', (event) => {
  console.log('Installing...');
  event.waitUntil(
    caches.open('v1').then(cache => {
      return cache.addAll(['/', '/index.html']);
    })
  );
});
```

### Activate

```javascript
self.addEventListener('activate', (event) => {
  console.log('Activating...');
});
```

### Fetch

```javascript
self.addEventListener('fetch', (event) => {
  event.respondWith(
    fetch(event.request).catch(() => caches.match(event.request))
  );
});
```

---

## Caching Strategies

### Cache First

```javascript
self.addEventListener('fetch', (event) => {
  event.respondWith(
    caches.match(event.request).then(response => {
      return response || fetch(event.request);
    })
  );
});
```

### Network First

```javascript
self.addEventListener('fetch', (event) => {
  event.respondWith(
    fetch(event.request)
      .catch(() => caches.match(event.request))
      .then(response => response || new Response('Offline'))
  );
});
```

---

## Advanced Caching

### Versioned Cache

```javascript
const CACHE_VERSION = 'v1';
const CACHE_NAME = `static-${CACHE_VERSION}`;

self.addEventListener('install', (event) => {
  event.waitUntil(
    caches.open(CACHE_NAME).then(cache => cache.addAll([
      '/',
      '/index.html'
    ]))
  );
});

self.addEventListener('activate', (event) => {
  event.waitUntil(
    caches.keys().then(keys => {
      return Promise.all(
        keys.filter(key => key !== CACHE_NAME)
          .map(key => caches.delete(key))
      );
    })
  );
});
```

---

## Workbox

### Using Workbox

```bash
npm install workbox-routing workbox-strategies workbox-precaching
```

```javascript
import { registerRoute } from 'workbox-routing';
import { StaleWhileRevalidate } from 'workbox-strategies';

registerRoute(
  ({ request }) => request.destination === 'style',
  new StaleWhileRevalidate()
);
```

---

## Summary

### Key Takeaways

1. **Registration**: Enable SW
2. **Lifecycle**: Install, activate, fetch
3. **Strategies**: Cache and network
4. **Versioning**: Cache control
5. **Workbox**: Library

### Next Steps

- Continue with: [41_PROJECT_16_FINANCE_TRADING.md](../41_PROJECT_16_FINANCE_TRADING.md)
- Study more strategies
- Implement full caching

---

## Cross-References

- **Previous**: [05_PWA_MASTER.md](05_PWA_MASTER.md)
- **Next**: [41_PROJECT_16_FINANCE_TRADING.md](../41_PROJECT_16_FINANCE_TRADING.md)

---

*Last updated: 2024*