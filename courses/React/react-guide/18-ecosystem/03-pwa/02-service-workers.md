# Service Workers

## Overview
Service workers are scripts that run in the background, acting as a network proxy between web apps and the network.

## Prerequisites
- JavaScript async/await
- PWA basics

## Core Concepts

### Service Worker Lifecycle

1. **Register** - Browser finds and downloads sw.js
2. **Install** - Assets cached
3. **Activate** - Clean up old caches
4. **Fetch** - Intercept network requests

### Complete Service Worker

```javascript
// [File: public/sw.js]
const CACHE_NAME = 'app-cache-v2';
const STATIC_ASSETS = [
  '/',
  '/index.html',
  '/offline.html'
];

// Install - cache static assets
self.addEventListener('install', (event) => {
  event.waitUntil(
    caches.open(CACHE_NAME)
      .then((cache) => cache.addAll(STATIC_ASSETS))
      .then(() => self.skipWaiting())
  );
});

// Activate - clean old caches
self.addEventListener('activate', (event) => {
  event.waitUntil(
    caches.keys()
      .then((keys) => Promise.all(
        keys
          .filter((key) => key !== CACHE_NAME)
          .map((key) => caches.delete(key))
      ))
      .then(() => self.clients.claim())
  );
});

// Fetch - network first, fallback to cache
self.addEventListener('fetch', (event) => {
  // Skip cross-origin requests
  if (!event.request.url.startsWith(self.location.origin)) {
    return;
  }

  event.respondWith(
    fetch(event.request)
      .then((response) => {
        // Clone response for caching
        const responseClone = response.clone();
        caches.open(CACHE_NAME)
          .then((cache) => cache.put(event.request, responseClone));
        return response;
      })
      .catch(() => {
        // Fallback to cache
        return caches.match(event.request)
          .then((cached) => cached || caches.match('/offline.html'));
      })
  );
});
```

### Registering Service Worker

```typescript
// [File: src/serviceWorkerRegistration.ts]
export function register() {
  if ('serviceWorker' in navigator) {
    window.addEventListener('load', () => {
      navigator.serviceWorker.register('/sw.js')
        .then((registration) => {
          console.log('SW registered:', registration);
        })
        .catch((error) => {
          console.log('SW registration failed:', error);
        });
    });
  }
}

export function unregister() {
  if ('serviceWorker' in navigator) {
    navigator.serviceWorker.ready
      .then((registration) => {
        registration.unregister();
      });
  }
}
```

## Key Takeaways
- Service workers enable offline functionality
- Cache strategies: cache-first, network-first, stale-while-revalidate
- Lifecycle has install, activate, fetch events

## What's Next
Continue to [Offline Strategies](03-offline-strategies.md) for advanced caching strategies.