# Offline Strategies

## Overview
Different caching strategies suit different types of content. Understanding when to use each strategy is crucial for building effective offline PWAs.

## Prerequisites
- Service workers
- Cache API

## Core Concepts

### Cache Strategies

#### 1. Cache First (Cache-First)

```javascript
// [File: public/sw-cache-first.js]
// Good for: Static assets, images
self.addEventListener('fetch', (event) => {
  event.respondWith(
    caches.match(event.request)
      .then((cached) => {
        // Return cached or fetch from network
        return cached || fetch(event.request);
      })
  );
});
```

#### 2. Network First (Network-First)

```javascript
// [File: public/sw-network-first.js]
// Good for: API calls, frequently updated content
self.addEventListener('fetch', (event) => {
  event.respondWith(
    fetch(event.request)
      .then((response) => {
        // Cache successful responses
        const clone = response.clone();
        caches.open('api-cache').then((cache) => cache.put(event.request, clone));
        return response;
      })
      .catch(() => {
        // Fallback to cache on network failure
        return caches.match(event.request);
      })
  );
});
```

#### 3. Stale-While-Revalidate

```javascript
// [File: public/sw-stale-while-revalidate.js]
// Good for: Balance between speed and freshness
self.addEventListener('fetch', (event) => {
  event.respondWith(
    caches.match(event.request)
      .then((cached) => {
        // Fetch and update cache in background
        const fetchPromise = fetch(event.request).then((response) => {
          caches.open('dynamic-cache').then((cache) => {
            cache.put(event.request, response.clone());
          });
          return response;
        });
        // Return cached immediately, update in background
        return cached || fetchPromise;
      })
  );
});
```

### Background Sync

```javascript
// [File: public/sw-sync.js]
// Queue actions when offline, sync when back online
self.addEventListener('sync', (event) => {
  if (event.tag === 'sync-orders') {
    event.waitUntil(syncOrders());
  }
});

async function syncOrders() {
  const db = await openDatabase();
  const orders = await getUnsyncedOrders(db);
  
  for (const order of orders) {
    await fetch('/api/orders', {
      method: 'POST',
      body: JSON.stringify(order)
    });
    await markOrderSynced(db, order.id);
  }
}
```

## Key Takeaways
- Choose strategy based on content type
- Stale-while-revalidate balances speed and freshness
- Background sync enables offline mutations

## What's Next
This completes the PWA section. Continue to [Dockerizing React](04-docker/01-dockerizing-react.md) for containerization.