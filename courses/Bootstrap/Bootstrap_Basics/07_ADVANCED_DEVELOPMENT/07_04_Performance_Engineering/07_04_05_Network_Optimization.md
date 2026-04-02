---
title: "Network Optimization"
difficulty: 2
category: "Advanced Development"
subcategory: "Performance Engineering"
prerequisites:
  - HTTP/2 Protocol
  - Resource Hints
  - CDN Configuration
---

## Overview

Network optimization for Bootstrap assets focuses on reducing the number of requests, minimizing transfer sizes, and leveraging browser caching through HTTP/2 multiplexing, resource hints (preload, prefetch, preconnect), and CDN delivery. These optimizations ensure Bootstrap's CSS and JavaScript reach the browser as fast as possible.

Bootstrap's asset delivery benefits from CDN hosting (jsDelivr, unpkg, cdnjs) with global edge caching, HTTP/2 server push for critical CSS, and proper cache headers for long-term browser caching. Resource hints tell the browser to establish connections to CDN origins early, reducing DNS and TLS overhead.

## Basic Implementation

```html
<!-- Optimized resource loading -->
<head>
  <!-- Preconnect to CDN origins -->
  <link rel="preconnect" href="https://cdn.jsdelivr.net" crossorigin>
  <link rel="preconnect" href="https://fonts.googleapis.com">

  <!-- Preload critical CSS -->
  <link rel="preload"
        href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"
        as="style"
        onload="this.onload=null;this.rel='stylesheet'">
  <noscript>
    <link rel="stylesheet"
          href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
  </noscript>

  <!-- Preload critical JS (deferred) -->
  <link rel="preload"
        href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"
        as="script">

  <!-- Prefetch next-page assets -->
  <link rel="prefetch" href="/dist/css/pages/dashboard.css">
</head>

<body>
  <!-- Content here -->

  <!-- Load JS after render -->
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"
          defer></script>
</body>
```

```nginx
# Nginx configuration for HTTP/2 and caching
server {
  listen 443 ssl http2;

  # Gzip compression
  gzip on;
  gzip_types text/css application/javascript image/svg+xml;
  gzip_min_length 256;

  # Brotli compression (if module installed)
  brotli on;
  brotli_types text/css application/javascript;

  # Cache static assets
  location ~* \.(css|js|svg|woff2)$ {
    expires 1y;
    add_header Cache-Control "public, immutable";
    add_header Vary "Accept-Encoding";
  }

  # Serve from local dist
  location /dist/ {
    alias /var/www/app/dist/;
  }
}
```

```html
<!-- HTTP/2 Server Push (if supported) -->
<head>
  <link rel="preload" href="/dist/css/critical.css" as="style">
  <link rel="preload" href="/dist/js/main.js" as="script">
</head>
```

## Advanced Variations

```js
// Service Worker for offline Bootstrap asset caching
// sw.js
const CACHE_NAME = 'bootstrap-cache-v1';
const PRECACHE_URLS = [
  'https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css',
  'https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js'
];

self.addEventListener('install', (event) => {
  event.waitUntil(
    caches.open(CACHE_NAME).then(cache => cache.addAll(PRECACHE_URLS))
  );
});

self.addEventListener('fetch', (event) => {
  if (event.request.url.includes('bootstrap')) {
    event.respondWith(
      caches.match(event.request).then(cached => {
        const fetched = fetch(event.request).then(response => {
          const clone = response.clone();
          caches.open(CACHE_NAME).then(cache => cache.put(event.request, clone));
          return response;
        });
        return cached || fetched;
      })
    );
  }
});
```

## Best Practices

1. **Preconnect to CDNs** - Eliminate DNS/TLS round-trips for known origins.
2. **Preload critical assets** - CSS and JS needed for above-the-fold content.
3. **Defer non-critical JS** - Use `defer` or `async` for Bootstrap's JavaScript.
4. **Use HTTP/2** - Multiplexed connections reduce request overhead.
5. **Enable Brotli compression** - 15-25% smaller than gzip for text assets.
6. **Set immutable cache headers** - Hashed filenames enable long-term caching.
7. **Use a CDN** - Global edge caching reduces latency for international users.
8. **Minimize redirects** - Each redirect adds a full round-trip.
9. **Use resource hints** - `prefetch` for next-page assets, `preconnect` for known origins.
10. **Monitor with Lighthouse** - Regular audits catch performance regressions.

## Common Pitfalls

1. **Preloading too much** - Preloading everything defeats the purpose; only preload critical resources.
2. **Missing cache headers** - Assets re-download on every visit without proper caching.
3. **Render-blocking CSS** - Loading non-critical CSS synchronously delays first paint.
4. **No compression** - Uncompressed CSS and JS are 5-10x larger than necessary.
5. **Ignoring CDN cache invalidation** - Old assets served from CDN after updates.

## Accessibility Considerations

Network optimization should not delay the loading of accessibility-critical CSS (focus styles, screen reader classes). Preload accessibility-related stylesheets.

## Responsive Behavior

Mobile devices on slower connections benefit most from network optimizations. Preload mobile-critical assets and defer desktop-only resources.

```html
<!-- Conditional preloading based on viewport -->
<link rel="preload" href="hero-desktop.webp" as="image" media="(min-width: 768px)">
<link rel="preload" href="hero-mobile.webp" as="image" media="(max-width: 767px)">
```
