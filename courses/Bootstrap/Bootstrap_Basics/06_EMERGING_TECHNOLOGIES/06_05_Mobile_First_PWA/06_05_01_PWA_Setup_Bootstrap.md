---
title: "PWA Setup with Bootstrap"
topic: "Mobile First PWA"
difficulty: 2
duration: "35 minutes"
prerequisites: ["Service Worker basics", "Web App Manifest", "Bootstrap 5"]
tags: ["pwa", "service-worker", "manifest", "install-prompt", "bootstrap"]
---

## Overview

A Progressive Web App (PWA) combines web reach with app-like capabilities — offline access, home screen installation, and push notifications. Bootstrap 5 provides the UI layer, while the PWA infrastructure (service worker, web app manifest, install prompt) handles the native app behavior. The `manifest.json` file defines the app's name, icons, theme colors, and display mode, while the service worker caches Bootstrap's CSS, JS, and assets for offline functionality.

Setting up a PWA with Bootstrap requires: a valid `manifest.json` linked in the HTML, a service worker registered in JavaScript, HTTPS hosting, and optionally a custom install prompt built with Bootstrap's modal or toast components. Bootstrap's responsive grid and mobile-first utilities ensure the installed app looks native on mobile devices.

## Basic Implementation

### Web App Manifest

```json
// manifest.json
{
  "name": "Bootstrap PWA",
  "short_name": "BSPWA",
  "description": "A Progressive Web App built with Bootstrap 5",
  "start_url": "/",
  "display": "standalone",
  "background_color": "#ffffff",
  "theme_color": "#6366f1",
  "orientation": "portrait-primary",
  "icons": [
    {
      "src": "/icons/icon-192x192.png",
      "sizes": "192x192",
      "type": "image/png",
      "purpose": "any"
    },
    {
      "src": "/icons/icon-512x512.png",
      "sizes": "512x512",
      "type": "image/png",
      "purpose": "any"
    },
    {
      "src": "/icons/icon-maskable-512.png",
      "sizes": "512x512",
      "type": "image/png",
      "purpose": "maskable"
    }
  ]
}
```

### Service Worker

```js
// sw.js
const CACHE_NAME = 'bootstrap-pwa-v1';
const STATIC_ASSETS = [
  '/',
  '/index.html',
  '/css/app.css',
  '/js/app.js',
  '/manifest.json',
  'https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css',
  'https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js',
];

// Install: cache static assets
self.addEventListener('install', (event) => {
  event.waitUntil(
    caches.open(CACHE_NAME).then((cache) => cache.addAll(STATIC_ASSETS))
  );
  self.skipWaiting();
});

// Activate: clean old caches
self.addEventListener('activate', (event) => {
  event.waitUntil(
    caches.keys().then((keys) =>
      Promise.all(keys.filter(k => k !== CACHE_NAME).map(k => caches.delete(k)))
    )
  );
  self.clients.claim();
});

// Fetch: cache-first strategy for static, network-first for API
self.addEventListener('fetch', (event) => {
  const { request } = event;
  const url = new URL(request.url);

  if (url.pathname.startsWith('/api/')) {
    event.respondWith(
      fetch(request).catch(() => caches.match(request))
    );
  } else {
    event.respondWith(
      caches.match(request).then((cached) => cached || fetch(request))
    );
  }
});
```

### Registration and Install Prompt

```html
<!-- index.html -->
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <meta name="theme-color" content="#6366f1">
  <link rel="manifest" href="/manifest.json">
  <link rel="apple-touch-icon" href="/icons/icon-192x192.png">
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"
        rel="stylesheet">
  <title>Bootstrap PWA</title>
</head>
<body>
  <div class="container py-4">
    <h1 class="text-primary">Bootstrap PWA</h1>
    <p class="lead">Install this app for offline access.</p>
    <button id="install-btn" class="btn btn-primary d-none">Install App</button>
  </div>

  <div class="toast-container position-fixed bottom-0 end-0 p-3">
    <div id="install-toast" class="toast" role="alert" aria-live="assertive">
      <div class="toast-header">
        <strong class="me-auto">Install App</strong>
        <button type="button" class="btn-close" data-bs-dismiss="toast"></button>
      </div>
      <div class="toast-body">
        <p>Add to your home screen for the best experience.</p>
        <button id="toast-install-btn" class="btn btn-primary btn-sm">Install</button>
        <button class="btn btn-secondary btn-sm" data-bs-dismiss="toast">Not Now</button>
      </div>
    </div>
  </div>

  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
  <script>
    // Register service worker
    if ('serviceWorker' in navigator) {
      navigator.serviceWorker.register('/sw.js')
        .then(reg => console.log('SW registered:', reg.scope))
        .catch(err => console.error('SW registration failed:', err));
    }

    // Install prompt
    let deferredPrompt;
    const installBtn = document.getElementById('install-btn');
    const toastEl = document.getElementById('install-toast');
    const toastInstallBtn = document.getElementById('toast-install-btn');
    const toast = new bootstrap.Toast(toastEl, { autohide: false });

    window.addEventListener('beforeinstallprompt', (e) => {
      e.preventDefault();
      deferredPrompt = e;
      installBtn.classList.remove('d-none');
      toast.show();
    });

    async function promptInstall() {
      if (!deferredPrompt) return;
      deferredPrompt.prompt();
      const { outcome } = await deferredPrompt.userChoice;
      console.log('Install outcome:', outcome);
      deferredPrompt = null;
      installBtn.classList.add('d-none');
      toast.hide();
    }

    installBtn.addEventListener('click', promptInstall);
    toastInstallBtn.addEventListener('click', promptInstall);
  </script>
</body>
</html>
```

## Advanced Variations

### Workbox Service Worker

```bash
npm install --save-dev workbox-cli
```

```js
// workbox-config.js
module.exports = {
  globDirectory: 'dist/',
  globPatterns: [
    '**/*.{html,js,css,woff2,png,svg,webp,json}'
  ],
  swDest: 'dist/sw.js',
  runtimeCaching: [
    {
      urlPattern: /^https:\/\/cdn\.jsdelivr\.net\/npm\/bootstrap/,
      handler: 'CacheFirst',
      options: {
        cacheName: 'bootstrap-cdn',
        expiration: { maxEntries: 20, maxAgeSeconds: 30 * 24 * 60 * 60 },
      },
    },
    {
      urlPattern: /^\/api\//,
      handler: 'NetworkFirst',
      options: {
        cacheName: 'api-cache',
        networkTimeoutSeconds: 3,
        expiration: { maxEntries: 50, maxAgeSeconds: 5 * 60 },
      },
    },
  ],
};
```

### Offline Page with Bootstrap

```js
// sw.js — serve offline page when network fails
const OFFLINE_URL = '/offline.html';

self.addEventListener('fetch', (event) => {
  if (event.request.mode === 'navigate') {
    event.respondWith(
      fetch(event.request).catch(() => caches.match(OFFLINE_URL))
    );
  }
});
```

```html
<!-- offline.html -->
<div class="container text-center py-5">
  <div class="display-1 text-muted mb-3">
    <i class="bi bi-wifi-off"></i>
  </div>
  <h1>You're Offline</h1>
  <p class="text-muted">Please check your internet connection.</p>
  <button class="btn btn-primary" onclick="location.reload()">Try Again</button>
</div>
```

## Best Practices

1. **Include both 192x192 and 512x512 icons** in the manifest for proper installation across devices.
2. **Add maskable icon** with `"purpose": "maskable"` for adaptive icon support on Android.
3. **Set `theme-color` meta tag** to match Bootstrap's primary color for native-feeling status bar.
4. **Use `display: "standalone"`** for app-like experience without browser chrome.
5. **Cache Bootstrap CDN assets** in the service worker for offline functionality.
6. **Use cache-first for static assets** and network-first for API calls to balance speed and freshness.
7. **Version your cache name** (`bootstrap-pwa-v1`) to invalidate stale caches on deployment.
8. **Call `self.skipWaiting()`** in the install event to activate updated service workers immediately.
9. **Use `e.preventDefault()`** on `beforeinstallprompt` to control when the install prompt appears.
10. **Register the service worker on page load**, not on user interaction, for immediate caching.

## Common Pitfalls

1. **Missing `manifest.json` link** in HTML `<head>` prevents the browser from detecting PWA capabilities.
2. **No HTTPS** — service workers and PWA installation require secure contexts.
3. **Forgetting `self.clients.claim()`** means the new service worker doesn't control existing tabs until reload.
4. **Not versioning cache names** serves stale assets after deployments.
5. **Caching API responses forever** without expiration leads to outdated data in the app.

## Accessibility Considerations

The install prompt toast uses `role="alert"` and `aria-live="assertive"` for screen reader announcement. The offline page should include accessible messaging with sufficient color contrast. Bootstrap's semantic HTML ensures the PWA's navigation and content are accessible in standalone mode. Service worker fetch responses must preserve ARIA attributes. Use `aria-label` on icon-only buttons in the PWA toolbar.

## Responsive Behavior

Bootstrap's mobile-first CSS ensures the PWA adapts to all screen sizes in standalone mode. Set `viewport-fit=cover` in the viewport meta tag for safe area insets on notched devices. Use Bootstrap's `pt-safe` custom utility (or `padding-top: env(safe-area-inset-top)`) for content below device notches. The `orientation: "portrait-primary"` manifest setting is appropriate for most mobile apps; omit it for responsive apps that support landscape.