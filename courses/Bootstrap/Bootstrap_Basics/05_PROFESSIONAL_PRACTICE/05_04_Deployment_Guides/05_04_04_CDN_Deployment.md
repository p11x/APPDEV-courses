---
title: "CDN Deployment for Bootstrap 5"
section: "05_04_Deployment_Guides"
file: "05_04_04_CDN_Deployment.md"
difficulty: 2
tags: ["cdn", "cloudflare", "cloudfront", "caching", "purge"]
duration: "12 minutes"
prerequisites:
  - "Production build completed"
  - "Domain and hosting configured"
learning_objectives:
  - "Deploy assets to a CDN for global distribution"
  - "Configure cache headers and purge strategies"
  - "Implement versioned URLs for cache busting"
---

# CDN Deployment for Bootstrap 5

## Overview

A CDN (Content Delivery Network) distributes your static assets across geographically distributed edge servers, reducing latency for users worldwide. For Bootstrap sites, deploying CSS, JS, fonts, and images to a CDN offloads traffic from your origin server and dramatically improves Time to First Byte (TTFB).

CDN deployment involves three core concerns: **uploading assets**, **configuring cache policies**, and **purging stale content**. Providers like Cloudflare, AWS CloudFront, and Fastly each offer different APIs and pricing models, but the fundamental principles are identical.

---

## Basic Implementation

### AWS CloudFront + S3 Deployment

```bash
# 1. Sync build output to S3
aws s3 sync dist/ s3://my-bootstrap-site/assets/ \
  --cache-control "public, max-age=31536000, immutable" \
  --exclude "*.html"

# 2. Upload HTML with no-cache
aws s3 sync dist/ s3://my-bootstrap-site/ \
  --cache-control "no-cache" \
  --exclude "*" --include "*.html"
```

### Cloudflare Page Rules (Cache Configuration)

```
# Cache all static assets aggressively
URL: mysite.com/assets/*
Settings:
  - Cache Level: Cache Everything
  - Edge Cache TTL: 1 month
  - Browser Cache TTL: 1 year
```

### Versioned Asset URLs

```html
<!-- Version directory approach -->
<link rel="stylesheet" href="https://cdn.mysite.com/v2.1.0/css/main.a1b2c3.css">

<!-- Query string approach (less reliable) -->
<script src="https://cdn.mysite.com/js/app.js?v=2.1.0"></script>
```

---

## Advanced Variations

### CloudFront Cache Behavior Configuration

```json
// cloudfront-cache-policy.json
{
  "CachePolicyConfig": {
    "Name": "BootstrapStaticAssets",
    "DefaultTTL": 31536000,
    "MaxTTL": 31536000,
    "MinTTL": 86400,
    "ParametersInCacheKeyAndForwardedToOrigin": {
      "HeadersConfig": { "HeaderBehavior": "none" },
      "CookiesConfig": { "CookieBehavior": "none" },
      "QueryStringsConfig": { "QueryStringBehavior": "none" },
      "EnableAcceptEncodingBrotli": true,
      "EnableAcceptEncodingGzip": true
    }
  }
}
```

### Cloudflare Workers for Smart Caching

```js
// cloudflare-worker.js
addEventListener('fetch', event => {
  event.respondWith(handleRequest(event.request));
});

async function handleRequest(request) {
  const cache = caches.default;
  let response = await cache.match(request);

  if (!response) {
    response = await fetch(request);
    const headers = new Headers(response.headers);
    headers.set('Cache-Control', 'public, max-age=31536000, immutable');
    response = new Response(response.body, { ...response, headers });
    event.waitUntil(cache.put(request, response.clone()));
  }

  return response;
}
```

---

## Best Practices

1. **Use content-hash filenames** (`main.a1b2c3.css`) with `max-age=31536000, immutable` — never expires since the filename changes with content
2. **Set `no-cache` on HTML files** — browsers must revalidate but can use cached versions
3. **Enable Brotli and Gzip** at the CDN edge — reduces transfer sizes by 60-80%
4. **Use versioned directory paths** (`/v2.1.0/`) instead of query strings — some CDNs and proxies ignore query string cache keys
5. **Configure `Vary: Accept-Encoding`** to serve correct compressed variant
6. **Set `s-maxage` for shared CDN caching** separate from `max-age` for browser caching
7. **Deploy to the CDN origin with immutable headers** — content-hash files never change
8. **Use CDN-specific purge APIs** for emergency cache invalidation
9. **Monitor CDN cache hit ratios** — target >95% hit rate for static assets
10. **Enable HTTP/2 or HTTP/3** at the CDN edge for multiplexed asset delivery
11. **Set `Cross-Origin-Resource-Policy`** headers if loading fonts from a different domain
12. **Use `stale-while-revalidate`** for HTML pages to serve stale content while fetching fresh versions

---

## Common Pitfalls

1. **Not versioning asset filenames** — `max-age=31536000` on `/css/main.css` means users never get updates without manual purge
2. **Purging the entire CDN cache on every deploy** — causes origin stampede, degrading performance for all users
3. **Missing `immutable` directive** — browsers may revalidate content-hash files unnecessarily
4. **Caching HTML with long TTLs** — users see outdated content; HTML should use `no-cache` or short TTLs
5. **Ignoring CORS headers for fonts** — browsers block cross-origin font loading without `Access-Control-Allow-Origin`
6. **Not testing CDN delivery from multiple regions** — edge server configuration varies; a cache miss in one region may not reflect in another

---

## Accessibility Considerations

CDN configuration does not directly impact accessibility, but cache misconfigurations can cause outdated accessibility fixes to persist. When deploying ARIA attribute corrections or screen reader improvements, ensure HTML files bypass the CDN cache (`no-cache`) so assistive technologies receive the latest markup immediately.

Font delivery via CDN must include proper `crossorigin` attributes to prevent blocking, which would render text invisible and break accessibility for all users.

---

## Responsive Behavior

CDN edge servers serve the same responsive CSS regardless of device or viewport. Ensure your CDN is configured to respect the `Accept-Encoding` header so that compressed CSS (containing all responsive media queries) is delivered efficiently. Do not serve different CSS files per device at the CDN level — Bootstrap handles responsive behavior client-side.
