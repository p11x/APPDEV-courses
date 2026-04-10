# 📱 Progressive Web Apps Complete Guide

## Building Installable Web Applications

---

## Table of Contents

1. [PWA Introduction](#pwa-introduction)
2. [Service Workers](#service-workers)
3. [Web App Manifest](#web-app-manifest)
4. [Offline Support](#offline-support)
5. [Push Notifications](#push-notifications)
6. [Background Sync](#background-sync)

---

## PWA Introduction

### What is a PWA?

Progressive Web Apps are web applications that provide an app-like experience and work offline.

```
┌─────────────────────────────────────────────────────────────┐
│                    PWA FEATURES                           │
├─────────────────────────────────────────────────────────────┤
│  ✓ Installable                                    │
│  ✓ Work offline                                 │
│  ✓ Push notifications                          │
│  ✓ Background sync                            │
│  ✓ Add to home screen                         │
└─────────────────────────────────────────────────────────────┘
```

---

## Service Workers

### Registration

```javascript
if ('serviceWorker' in navigator) {
  navigator.serviceWorker.register('/sw.js')
    .then(registration => {
      console.log('SW registered:', registration);
    })
    .catch(error => {
      console.error('SW registration failed:', error);
    });
}
```

### Service Worker Lifecycle

```javascript
// sw.js
self.addEventListener('install', (event) => {
  console.log('Service Worker installing');
  event.waitUntil(
    caches.open('static-v1').then(cache => {
      return cache.addAll(['/', '/index.html', '/styles.css']);
    })
  );
});

self.addEventListener('activate', (event) => {
  console.log('Service Worker activated');
});

self.addEventListener('fetch', (event) => {
  event.respondWith(
    caches.match(event.request).then(response => {
      return response || fetch(event.request);
    })
  );
});
```

---

## Web App Manifest

### manifest.json

```json
{
  "name": "My PWA",
  "short_name": "MyApp",
  "start_url": "/",
  "display": "standalone",
  "background_color": "#ffffff",
  "theme_color": "#000000",
  "icons": [
    {
      "src": "/icon-192.png",
      "sizes": "192x192",
      "type": "image/png"
    },
    {
      "src": "/icon-512.png",
      "sizes": "512x512",
      "type": "image/png"
    }
  ]
}
```

```html
<link rel="manifest" href="/manifest.json">
```

### Installing PWA

```javascript
let deferredPrompt;

window.addEventListener('beforeinstallprompt', (e) => {
  e.preventDefault();
  deferredPrompt = e;
});

async function installPWA() {
  if (deferredPrompt) {
    deferredPrompt.prompt();
    const { outcome } = await deferredPrompt.userChoice;
    console.log('Install outcome:', outcome);
  }
}
```

---

## Offline Support

### Cache Strategies

```javascript
// Cache First
self.addEventListener('fetch', (event) => {
  event.respondWith(
    caches.match(event.request).then(response => {
      return response || fetch(event.request);
    })
  );
});

// Network First
self.addEventListener('fetch', (event) => {
  event.respondWith(
    fetch(event.request).catch(() => {
      return caches.match(event.request);
    })
  );
});

// Stale While Revalidate
self.addEventListener('fetch', (event) => {
  event.respondWith(
    caches.open('dynamic-v1').then(cache => {
      const fetchPromise = fetch(event.request).then(response => {
        cache.put(event.request, response.clone());
        return response;
      });
      return cache.match(event.request).then(cached => {
        return cached || fetchPromise;
      });
    })
  );
});
```

---

## Push Notifications

### Subscribing

```javascript
async function subscribe() {
  const registration = await navigator.serviceWorker.ready;
  const subscription = await registration.pushManager.subscribe({
    userVisibleOnly: true,
    applicationServerKey: urlBase64ToUint8Array(vapidPublicKey)
  });
  console.log('Push subscription:', subscription);
}
```

### Receiving Push

```javascript
self.addEventListener('push', (event) => {
  const data = event.data.json();
  
  const options = {
    body: data.body,
    icon: '/icon.png',
    badge: '/badge.png'
  };
  
  event.waitUntil(
    self.registration.showNotification(data.title, options)
  );
});
```

---

## Background Sync

### Register Sync

```javascript
async function registerSync() {
  const registration = await navigator.serviceWorker.ready;
  await registration.sync.register('sync-data');
}
```

### Handle Sync

```javascript
self.addEventListener('sync', (event) => {
  if (event.tag === 'sync-data') {
    event.waitUntil(syncData());
  }
});

async function syncData() {
  // Sync data
}
```

---

## Summary

### Key Takeaways

1. **Service Worker**: Background processing
2. **Manifest**: Installable
3. **Caching**: Offline support
4. **Push**: Notifications
5. **Background Sync**: Deferred actions

### Next Steps

- Continue with: [06_SERVICE_WORKERS_GUIDE.md](06_SERVICE_WORKERS_GUIDE.md)
- Study PWA tooling
- Implement full PWA

---

## Cross-References

- **Previous**: [04_WEB_STORAGE_MASTER.md](04_WEB_STORAGE_MASTER.md)
- **Next**: [06_SERVICE_WORKERS_GUIDE.md](06_SERVICE_WORKERS_GUIDE.md)

---

*Last updated: 2024*