---
title: "Caching Strategies"
module: "Performance Optimization"
difficulty: 2
duration: "25 minutes"
prerequisites: ["HTTP caching", "Service workers"]
tags: ["caching", "cdn", "service-worker", "performance"]
---

# Caching Strategies

## Overview

Effective caching strategies for Bootstrap assets ensure returning visitors experience near-instant page loads. Cache busting with content hashes, CDN configuration, service worker caching, and proper ETag/Cache-Control headers work together to maximize cache efficiency while ensuring users always receive the latest versions.

## Basic Implementation

Add content hashes to CSS filenames for cache busting:

```js
// webpack.config.js
module.exports = {
  output: {
    filename: 'js/[name].[contenthash:8].js',
    clean: true
  },
  plugins: [
    new MiniCssExtractPlugin({
      filename: 'css/[name].[contenthash:8].css'
    })
  ]
};
```

Reference hashed assets in HTML with build tool injection:

```html
<!-- Vite/webpack injects hashed filenames -->
<link rel="stylesheet" href="/css/custom.a1b2c3d4.css">
<script src="/js/app.e5f6g7h8.js" defer></script>
```

Configure Cache-Control headers:

```nginx
# nginx.conf
# Hashed assets: cache for 1 year
location ~* \.(css|js|woff2|jpg|webp)$ {
    expires 1y;
    add_header Cache-Control "public, immutable";
}

# HTML: cache for 0, always revalidate
location ~* \.html$ {
    expires -1;
    add_header Cache-Control "no-cache, must-revalidate";
}
```

## Advanced Variations

Implement service worker caching for Bootstrap assets:

```js
// sw.js - Service Worker
const CACHE_NAME = 'bootstrap-cache-v1';
const BOOTSTRAP_ASSETS = [
  '/css/custom.min.css',
  '/js/bootstrap.bundle.min.js',
  '/fonts/inter-variable.woff2'
];

self.addEventListener('install', (event) => {
  event.waitUntil(
    caches.open(CACHE_NAME).then((cache) => {
      return cache.addAll(BOOTSTRAP_ASSETS);
    })
  );
});

self.addEventListener('fetch', (event) => {
  event.respondWith(
    caches.match(event.request).then((cached) => {
      const fetchPromise = fetch(event.request).then((response) => {
        if (response.ok) {
          const clone = response.clone();
          caches.open(CACHE_NAME).then((cache) => {
            cache.put(event.request, clone);
          });
        }
        return response;
      });
      return cached || fetchPromise;
    })
  );
});

self.addEventListener('activate', (event) => {
  event.waitUntil(
    caches.keys().then((keys) => {
      return Promise.all(
        keys.filter(key => key !== CACHE_NAME)
            .map(key => caches.delete(key))
      );
    })
  );
});
```

Register the service worker:

```js
// main.js
if ('serviceWorker' in navigator) {
  window.addEventListener('load', () => {
    navigator.serviceWorker.register('/sw.js')
      .then(reg => console.log('SW registered:', reg.scope))
      .catch(err => console.warn('SW registration failed:', err));
  });
}
```

Configure CDN caching with Cloudflare/CloudFront:

```js
// Cloudflare Worker for edge caching
addEventListener('fetch', event => {
  event.respondWith(handleRequest(event.request));
});

async function handleRequest(request) {
  const cache = caches.default;
  const cached = await cache.match(request);

  if (cached) {
    return cached;
  }

  const response = await fetch(request);

  if (response.ok) {
    const headers = new Headers(response.headers);
    headers.set('Cache-Control', 'public, max-age=31536000, immutable');
    const cachedResponse = new Response(response.body, {
      status: response.status,
      headers
    });
    event.waitUntil(cache.put(request, cachedResponse));
  }

  return response;
}
```

Implement ETag-based validation:

```js
// Express.js ETag configuration
const express = require('express');
const app = express();

app.use(express.static('dist', {
  etag: true,
  lastModified: true,
  maxAge: '1y',
  setHeaders: (res, path) => {
    if (path.endsWith('.html')) {
      res.setHeader('Cache-Control', 'no-cache');
    } else {
      res.setHeader('Cache-Control', 'public, max-age=31536000, immutable');
    }
  }
}));
```

## Best Practices

1. Use content hashes in filenames for long-term caching
2. Set HTML to `no-cache` to always check for updates
3. Cache static assets (CSS, JS, fonts) for 1 year with `immutable`
4. Implement service worker for offline Bootstrap asset availability
5. Use CDN for globally distributed asset caching
6. Configure proper Vary headers for compressed assets
7. Test caching behavior with DevTools Network tab
8. Purge CDN cache on deployments
9. Use cache partitioning for security (modern browsers)
10. Monitor cache hit rates for optimization opportunities
11. Implement stale-while-revalidate for non-critical assets
12. Version cache names in service worker for updates

## Common Pitfalls

1. Not hashing filenames, causing stale cache issues
2. Caching HTML with long max-age (users miss updates)
3. Missing `immutable` flag on hashed assets
4. Not updating service worker cache version on deploy
5. Over-caching API responses
6. Ignoring CDN cache invalidation on deployments
7. Missing `Vary: Accept-Encoding` for gzipped assets
8. Not testing caching across different browsers

## Accessibility Considerations

- Ensure cached versions include all accessibility features
- Test service worker offline mode with screen readers
- Verify focus indicators work from cached CSS
- Maintain keyboard navigation in cached assets

## Responsive Behavior

- Verify cached responsive CSS works at all breakpoints
- Test service worker caching of responsive images
- Ensure CDN serves correct assets for mobile/desktop
- Validate cache behavior across viewport changes
