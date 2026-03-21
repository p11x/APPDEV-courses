# PWA Fundamentals

## Overview
Progressive Web Apps (PWAs) are web applications that provide app-like experiences. They can be installed on devices and work offline.

## Prerequisites
- Web development basics
- Service Worker concepts

## Core Concepts

### What Makes an App PWA?

1. **HTTPS** - Required for service workers
2. **Web App Manifest** - Installable
3. **Service Worker** - Offline support
4. **Responsive Design** - Works on all devices

### Manifest File

```json
// [File: public/manifest.json]
{
  "name": "My PWA App",
  "short_name": "MyApp",
  "description": "A progressive web app",
  "start_url": "/",
  "display": "standalone",
  "background_color": "#ffffff",
  "theme_color": "#000000",
  "icons": [
    {
      "src": "/icons/icon-192.png",
      "sizes": "192x192",
      "type": "image/png"
    },
    {
      "src": "/icons/icon-512.png",
      "sizes": "512x512",
      "type": "image/png"
    }
  ]
}
```

### Vite PWA Plugin

```typescript
// [File: vite.config.ts]
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { VitePWA } from 'vite-plugin-pwa';

export default defineConfig({
  plugins: [
    react(),
    VitePWA({
      registerType: 'autoUpdate',
      includeAssets: ['favicon.ico', 'robots.txt'],
      manifest: {
        name: 'My PWA',
        short_name: 'PWA',
        theme_color: '#000000',
        icons: [
          {
            src: 'pwa-192x192.png',
            sizes: '192x192',
            type: 'image/png'
          }
        ]
      }
    })
  ]
});
```

### Basic Service Worker

```javascript
// [File: public/sw.js]
const CACHE_NAME = 'my-pwa-cache-v1';
const urlsToCache = [
  '/',
  '/index.html',
  '/static/js/main.js',
  '/static/css/main.css'
];

// Install event - cache assets
self.addEventListener('install', (event) => {
  event.waitUntil(
    caches.open(CACHE_NAME)
      .then((cache) => cache.addAll(urlsToCache))
  );
});

// Fetch event - serve from cache, fallback to network
self.addEventListener('fetch', (event) => {
  event.respondWith(
    caches.match(event.request)
      .then((response) => {
        return response || fetch(event.request);
      })
  );
});
```

## Key Takeaways
- PWAs are installable web apps
- Manifest defines app appearance
- Service workers enable offline use

## What's Next
Continue to [Service Workers](02-service-workers.md) for detailed service worker implementation.