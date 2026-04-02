---
title: "App Shell Pattern with Bootstrap"
topic: "Mobile First PWA"
difficulty: 3
duration: "45 minutes"
prerequisites: ["Service Workers", "PWA fundamentals", "Bootstrap layout system"]
tags: ["app-shell", "skeleton", "caching", "pwa", "bootstrap"]
---

## Overview

The App Shell pattern separates the application's static UI framework from its dynamic content. The "shell" — navigation, layout containers, header, footer — is cached by the service worker and loads instantly, while content populates the shell via API calls. This provides a native-app-like perceived performance: the UI appears immediately, and content fills in progressively.

Bootstrap 5's grid system, navbar, offcanvas, and card components form the structural shell. The service worker caches the shell's HTML, CSS, and JavaScript. Skeleton screens (placeholder UI with animated shimmer effects) fill content areas while data loads, maintaining layout stability and providing visual feedback.

## Basic Implementation

### App Shell HTML Structure

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <meta name="theme-color" content="#6366f1">
  <link rel="manifest" href="/manifest.json">
  <link href="/css/shell.css" rel="stylesheet">
  <title>App Shell</title>
</head>
<body>
  <!-- Shell: cached by service worker, renders immediately -->
  <nav class="navbar navbar-expand-md navbar-dark bg-dark sticky-top">
    <div class="container-fluid">
      <a class="navbar-brand" href="/">MyApp</a>
      <button class="navbar-toggler" data-bs-toggle="offcanvas"
              data-bs-target="#navDrawer">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse d-none d-md-flex justify-content-end">
        <ul class="navbar-nav">
          <li class="nav-item"><a class="nav-link active" href="/">Home</a></li>
          <li class="nav-item"><a class="nav-link" href="/dashboard">Dashboard</a></li>
          <li class="nav-item"><a class="nav-link" href="/settings">Settings</a></li>
        </ul>
      </div>
    </div>
  </nav>

  <div class="offcanvas offcanvas-start" tabindex="-1" id="navDrawer">
    <div class="offcanvas-header">
      <h5 class="offcanvas-title">Menu</h5>
      <button class="btn-close" data-bs-dismiss="offcanvas"></button>
    </div>
    <div class="offcanvas-body">
      <a href="/" class="d-block py-2">Home</a>
      <a href="/dashboard" class="d-block py-2">Dashboard</a>
      <a href="/settings" class="d-block py-2">Settings</a>
    </div>
  </div>

  <!-- Content area: populated dynamically -->
  <main class="container py-4" id="app-content">
    <!-- Skeleton screens shown while loading -->
  </main>

  <footer class="bg-light py-3 mt-auto border-top">
    <div class="container text-center text-muted small">
      &copy; 2024 MyApp
    </div>
  </footer>

  <script src="/js/shell.js"></script>
</body>
</html>
```

### Skeleton Screen Component

```js
// components/skeleton.js
export function cardSkeleton() {
  return `
    <div class="card mb-3" aria-hidden="true">
      <div class="card-body">
        <div class="placeholder-glow">
          <span class="placeholder col-6 mb-2"></span>
          <span class="placeholder col-12 mb-1"></span>
          <span class="placeholder col-10 mb-1"></span>
          <span class="placeholder col-8"></span>
        </div>
      </div>
    </div>
  `;
}

export function listSkeleton(count = 5) {
  return Array.from({ length: count }, () => `
    <div class="d-flex align-items-center mb-3 placeholder-glow">
      <span class="placeholder rounded-circle me-3"
            style="width: 48px; height: 48px;"></span>
      <div class="flex-grow-1">
        <span class="placeholder col-6 d-block mb-1"></span>
        <span class="placeholder col-4 d-block"></span>
      </div>
    </div>
  `).join('');
}

export function tableSkeleton(rows = 5, cols = 4) {
  const header = `<tr>${Array.from({ length: cols }, () => '<th><span class="placeholder col-8"></span></th>').join('')}</tr>`;
  const body = Array.from({ length: rows }, () =>
    `<tr>${Array.from({ length: cols }, () => '<td><span class="placeholder col-10"></span></td>').join('')}</tr>`
  ).join('');

  return `
    <div class="table-responsive">
      <table class="table">
        <thead>${header}</thead>
        <tbody>${body}</tbody>
      </table>
    </div>
  `;
}
```

### Content Loading with Skeleton

```js
// shell.js
import { cardSkeleton, listSkeleton } from './components/skeleton.js';

const content = document.getElementById('app-content');

async function loadPage() {
  // Show skeleton
  content.innerHTML = `
    <div class="row">
      <div class="col-md-8">
        ${cardSkeleton()}
        ${cardSkeleton()}
      </div>
      <div class="col-md-4">
        <h5 class="placeholder-glow"><span class="placeholder col-6"></span></h5>
        ${listSkeleton(4)}
      </div>
    </div>
  `;

  try {
    const response = await fetch('/api/dashboard');
    const data = await response.json();

    content.innerHTML = `
      <div class="row">
        <div class="col-md-8">
          ${data.posts.map(post => `
            <div class="card mb-3">
              <div class="card-body">
                <h5 class="card-title">${post.title}</h5>
                <p class="card-text">${post.excerpt}</p>
              </div>
            </div>
          `).join('')}
        </div>
        <div class="col-md-4">
          <h5>Trending</h5>
          ${data.trending.map(item => `
            <div class="d-flex align-items-center mb-3">
              <div class="rounded-circle bg-secondary me-3"
                   style="width:48px;height:48px;"></div>
              <div>${item.name}</div>
            </div>
          `).join('')}
        </div>
      </div>
    `;
  } catch (err) {
    content.innerHTML = `
      <div class="alert alert-danger">
        <h5>Unable to load content</h5>
        <p>Please check your connection and try again.</p>
        <button class="btn btn-outline-danger" onclick="loadPage()">Retry</button>
      </div>
    `;
  }
}

loadPage();
```

## Advanced Variations

### Service Worker for Shell Caching

```js
// sw.js
const SHELL_CACHE = 'app-shell-v1';
const SHELL_ASSETS = [
  '/',
  '/css/shell.css',
  '/js/shell.js',
  '/manifest.json',
  '/offline.html',
];

self.addEventListener('install', (e) => {
  e.waitUntil(
    caches.open(SHELL_CACHE).then(cache => cache.addAll(SHELL_ASSETS))
  );
  self.skipWaiting();
});

self.addEventListener('activate', (e) => {
  e.waitUntil(
    caches.keys().then(keys =>
      Promise.all(keys.filter(k => k !== SHELL_CACHE).map(k => caches.delete(k)))
    )
  );
  self.clients.claim();
});

self.addEventListener('fetch', (e) => {
  const { request } = e;

  // Shell assets: cache-first
  if (SHELL_ASSETS.some(asset => request.url.endsWith(asset))) {
    e.respondWith(
      caches.match(request).then(cached => cached || fetch(request))
    );
    return;
  }

  // API: network-first with offline fallback
  if (request.url.includes('/api/')) {
    e.respondWith(
      fetch(request)
        .then(response => {
          const clone = response.clone();
          caches.open('api-cache').then(cache => cache.put(request, clone));
          return response;
        })
        .catch(() => caches.match(request))
    );
    return;
  }

  // Everything else: network-first
  e.respondWith(
    fetch(request).catch(() => caches.match(request))
  );
});
```

### Shimmer Animation CSS

```css
/* css/shell.css — custom shimmer effect */
@keyframes shimmer {
  0% { background-position: -200% 0; }
  100% { background-position: 200% 0; }
}

.skeleton-shimmer {
  background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
  background-size: 200% 100%;
  animation: shimmer 1.5s infinite;
  border-radius: 0.375rem;
}
```

## Best Practices

1. **Cache the shell HTML, CSS, and JS** in the service worker's install event for instant loading.
2. **Use Bootstrap's `placeholder` component** for skeleton screens — it's built-in and accessible.
3. **Use `placeholder-glow`** or `placeholder-wave`** for animated loading states.
4. **Maintain the same DOM structure** in skeleton and loaded states to prevent layout shift.
5. **Cache API responses** with network-first strategy for offline shell population.
6. **Use `aria-hidden="true"`** on skeleton elements to hide them from screen readers.
7. **Preload critical shell resources** with `<link rel="preload">` for faster first paint.
8. **Version shell cache** to enable updates without full cache invalidation.
9. **Show skeleton for the exact number of items** expected to prevent content reflow.
10. **Include an offline fallback page** in the shell cache for when navigation fails.

## Common Pitfalls

1. **Not caching the shell** causes full page load on every visit, negating PWA benefits.
2. **Skeleton structure differs from loaded content** causes visible layout shift.
3. **Missing `aria-hidden`** on skeletons causes confusing screen reader announcements.
4. **Cache-first for API data** serves stale content — use network-first with cache fallback.
5. **Forgetting `self.skipWaiting()`** delays service worker activation.

## Accessibility Considerations

Skeleton screens must have `aria-hidden="true"` and `role="presentation"` to avoid screen reader confusion. The app shell's navigation must be immediately accessible with proper ARIA landmarks (`<nav>`, `<main>`, `<footer>`). Content that replaces skeletons should announce itself with `aria-live="polite"`. Focus management must remain stable during skeleton-to-content transitions — do not move focus unexpectedly.

## Responsive Behavior

The app shell uses Bootstrap's responsive grid for content areas: `col-md-8` for main content, `col-md-4` for sidebars. The navbar switches between collapsed hamburger (mobile) and expanded (desktop) via `navbar-expand-md`. Skeleton placeholders use Bootstrap's responsive grid classes to match the loaded content's responsive layout. Offcanvas navigation is only visible on mobile. The shell maintains consistent padding and margins across breakpoints via Bootstrap's `container` class.