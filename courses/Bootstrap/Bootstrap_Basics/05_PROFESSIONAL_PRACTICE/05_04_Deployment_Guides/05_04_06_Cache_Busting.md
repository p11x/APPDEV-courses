---
title: "Cache Busting for Bootstrap 5"
section: "05_04_Deployment_Guides"
file: "05_04_06_Cache_Busting.md"
difficulty: 2
tags: ["caching", "hash", "etag", "service-worker", "versioning"]
duration: "12 minutes"
prerequisites:
  - "Production build pipeline configured"
  - "Understanding of HTTP caching headers"
learning_objectives:
  - "Implement content-hash filename strategies"
  - "Configure ETag and cache validation"
  - "Set up service worker cache strategies"
---

# Cache Busting for Bootstrap 5

## Overview

Cache busting ensures browsers fetch updated assets after deployment rather than serving stale cached versions. Without cache busting, aggressive caching (which is optimal for performance) means users see outdated styles and broken layouts after you deploy changes.

There are three primary strategies: **content-hash filenames** (most reliable), **query string versioning** (simple but less reliable), and **HTTP validation headers** (ETag/If-None-Match). For Progressive Web Apps, **service worker cache strategies** add a fourth layer of control. Content-hash filenames combined with immutable cache headers is the industry standard.

---

## Basic Implementation

### Content-Hash Filenames (Recommended)

```js
// vite.config.js
export default {
  build: {
    rollupOptions: {
      output: {
        entryFileNames: 'assets/[name].[hash].js',
        assetFileNames: 'assets/[name].[hash].[ext]',
      },
    },
  },
};
```

This produces:
```
dist/assets/main.a1b2c3d4.js
dist/assets/style.e5f6g7h8.css
```

HTML references update automatically:
```html
<!-- Build tool injects hashed filenames -->
<script src="/assets/main.a1b2c3d4.js"></script>
```

### Cache Headers for Hashed Files

```nginx
# nginx — hashed files never change, cache forever
location ~* \.[0-9a-f]{8}\.(js|css|woff2|png|jpg)$ {
  expires 1y;
  add_header Cache-Control "public, immutable";
}

# HTML — always revalidate
location ~* \.html$ {
  expires -1;
  add_header Cache-Control "no-cache";
}
```

---

## Advanced Variations

### Query String Versioning

```html
<!-- Simple but less reliable -->
<link rel="stylesheet" href="/css/bootstrap.min.css?v=2.1.0">
<script src="/js/app.js?v=2.1.0"></script>
```

Many proxies and CDNs ignore query strings for caching, making this approach unreliable.

### ETag / If-None-Match Validation

```nginx
# nginx.conf
etag on;
if_modified_since exact;

# Server returns 304 Not Modified when content unchanged
# Client sends: If-None-Match: "abc123"
# Server responds: 304 (no body, uses cached version)
```

### Service Worker Cache Strategies

```js
// sw.js — Workbox example
import { registerRoute } from 'workbox-routing';
import { CacheFirst, StaleWhileRevalidate } from 'workbox-strategies';
import { ExpirationPlugin } from 'workbox-expiration';
import { precacheAndRoute } from 'workbox-precaching';

// Precache build assets
precacheAndRoute(self.__WB_MANIFEST);

// Cache-first for hashed assets
registerRoute(
  ({url}) => url.pathname.startsWith('/assets/'),
  new CacheFirst({
    cacheName: 'static-assets',
    plugins: [
      new ExpirationPlugin({ maxEntries: 100, maxAgeSeconds: 60 * 60 * 24 * 365 }),
    ],
  })
);

// Stale-while-revalidate for HTML
registerRoute(
  ({request}) => request.destination === 'document',
  new StaleWhileRevalidate({ cacheName: 'html-cache' })
);
```

---

## Best Practices

1. **Use content-hash filenames** — the most reliable cache busting method; URL changes when content changes
2. **Set `immutable` on hashed assets** — tells browsers to never revalidate, saving round trips
3. **Always use `no-cache` on HTML** — ensures users get the latest asset references
4. **Separate vendor and app bundles** — Bootstrap changes rarely, so its hash stays stable across deploys
5. **Use `chunkhash` not `hash`** for per-entry hashing — `hash` changes all files when any file changes
6. **Implement `stale-while-revalidate`** for HTML — serves cached page while fetching update in background
7. **Version your service worker** — update the cache name on each deploy to purge old caches
8. **Use `workbox-webpack-plugin` or `vite-plugin-pwa`** — abstracts service worker complexity
9. **Test cache behavior with DevTools** — "Disable cache" checkbox must be OFF to verify real-world caching
10. **Set `Vary: Accept-Encoding`** — ensures Gzip and Brotli variants are cached separately
11. **Deploy HTML last** — upload hashed assets before HTML so references resolve immediately
12. **Monitor cache hit rates** — target >90% for static assets, <5% for HTML

---

## Common Pitfalls

1. **Caching HTML aggressively** — users see old asset references, load outdated CSS/JS even with hashed filenames
2. **Using query strings on CDNs that ignore them** — Cloudflare and others may strip query strings from cache keys
3. **Not updating service worker cache names** — old caches persist indefinitely, serving stale content
4. **Hashing only JS but not CSS** — CSS updates are invisible to browsers that cache the unhashed filename
5. **Setting `immutable` on non-hashed files** — these files can never be updated without manual user cache clear
6. **Deploying HTML and assets simultaneously with race conditions** — HTML references a hash that hasn't been uploaded yet, causing 404s

---

## Accessibility Considerations

Cache busting ensures accessibility fixes deploy immediately. When you fix an ARIA attribute or add a skip-navigation link, `no-cache` HTML guarantees users receive the update on next visit. Service workers should use `StaleWhileRevalidate` for HTML to balance performance with accessibility update delivery.

Screen reader users often navigate without clearing cache. Proper cache busting prevents them from experiencing outdated landmark structures or broken focus management that has been fixed in newer versions.

---

## Responsive Behavior

Cache busting applies equally to all responsive CSS variants within a single hashed file. Bootstrap's media queries are bundled into one CSS file with a single hash. When any responsive style changes, the entire file gets a new hash, ensuring all breakpoint behaviors update consistently.
