---
title: Edge Rendering with Bootstrap
category: [Future Bootstrap, Cutting Edge]
difficulty: 3
time: 30 min
tags: bootstrap5, edge-ssr, cdn-rendering, edge-workers, performance
---

## Overview

Edge rendering executes server-side HTML generation at CDN edge locations close to users, reducing latency. When combined with Bootstrap, edge workers can render complete component HTML, personalize content, and stream progressive UI — all within milliseconds of the user's geographic location.

## Basic Implementation

A Cloudflare Worker that renders Bootstrap HTML at the edge.

```js
// worker.js — Cloudflare Edge Worker
export default {
  async fetch(request) {
    const url = new URL(request.url);
    const country = request.cf?.country || 'US';
    const userAgent = request.headers.get('user-agent') || '';
    const isMobile = /mobile/i.test(userAgent);

    const html = `
      <!DOCTYPE html>
      <html lang="en" data-bs-theme="auto">
      <head>
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"
              rel="stylesheet">
        <title>Edge-Rendered Bootstrap</title>
      </head>
      <body>
        <div class="container py-5">
          <div class="alert alert-info">
            Rendered at edge node for <strong>${country}</strong>
            — Device: ${isMobile ? 'Mobile' : 'Desktop'}
          </div>
          <h1>Welcome</h1>
          <div class="row g-3">
            ${[1, 2, 3].map(i => `
              <div class="col-md-4">
                <div class="card">
                  <div class="card-body">
                    <h5 class="card-title">Feature ${i}</h5>
                    <p class="card-text">Server-rendered at the edge.</p>
                    <a href="#" class="btn btn-primary">Learn More</a>
                  </div>
                </div>
              </div>
            `).join('')}
          </div>
        </div>
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
      </body>
      </html>
    `;

    return new Response(html, {
      headers: { 'content-type': 'text/html; charset=utf-8' }
    });
  }
};
```

## Advanced Variations

Streaming edge SSR with progressive HTML delivery.

```js
export default {
  async fetch(request) {
    const { readable, writable } = new TransformStream();
    const writer = writable.getWriter();
    const encoder = new TextEncoder();

    const stream = async () => {
      // Head — immediate
      await writer.write(encoder.encode(`<!DOCTYPE html>
        <html><head>
          <meta charset="utf-8">
          <meta name="viewport" content="width=device-width, initial-scale=1">
          <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
        </head><body>
          <nav class="navbar navbar-dark bg-dark">
            <div class="container"><a class="navbar-brand" href="/">App</a></div>
          </nav>
          <main class="container py-4">
            <h1>Loading...</h1>
      `));

      // Data — fetched from origin/API
      const data = await fetch('https://api.example.com/products').then(r => r.json());

      // Content — streamed after data arrives
      await writer.write(encoder.encode(`
            <div class="row g-3">
              ${data.products.slice(0, 12).map(p => `
                <div class="col-sm-6 col-lg-4">
                  <div class="card h-100">
                    <div class="card-body">
                      <h5 class="card-title">${p.name}</h5>
                      <p class="card-text text-body-secondary">${p.description}</p>
                      <span class="badge text-bg-primary">$${p.price}</span>
                    </div>
                  </div>
                </div>
              `).join('')}
            </div>
      `));

      // Tail — scripts and closing
      await writer.write(encoder.encode(`
          </main>
          <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
        </body></html>
      `));

      await writer.close();
    };

    stream();
    return new Response(readable, {
      headers: { 'content-type': 'text/html; charset=utf-8' }
    });
  }
};
```

Edge-side A/B testing with Bootstrap component variants.

```js
export default {
  async fetch(request) {
    const cookie = request.headers.get('cookie') || '';
    const abGroup = cookie.includes('ab=B') ? 'B' : Math.random() > 0.5 ? 'B' : 'A';

    const variants = {
      A: {
        heroClass: 'bg-primary text-white',
        ctaClass: 'btn-light btn-lg',
        ctaText: 'Get Started'
      },
      B: {
        heroClass: 'bg-dark text-white',
        ctaClass: 'btn-outline-light btn-lg',
        ctaText: 'Start Free Trial'
      }
    };

    const v = variants[abGroup];

    const html = `
      <section class="${v.heroClass} py-5 text-center">
        <div class="container">
          <h1 class="display-4 fw-bold">Build Faster</h1>
          <p class="lead">Edge-rendered Bootstrap with A/B testing.</p>
          <a href="/signup" class="btn ${v.ctaClass}">${v.ctaText}</a>
        </div>
      </section>
    `;

    return new Response(html, {
      headers: {
        'content-type': 'text/html',
        'set-cookie': `ab=${abGroup}; Path=/; Max-Age=86400`
      }
    });
  }
};
```

## Best Practices

1. Render complete Bootstrap HTML at the edge to eliminate origin round-trips
2. Stream HTML progressively — head first, then content as data resolves
3. Use edge worker environment variables for feature flags and configuration
4. Cache edge-rendered HTML at the CDN layer for static pages
5. Personalize content using geo, device, and cookie data available in edge context
6. Load Bootstrap CSS from CDN — edge workers don't bundle static assets
7. Set appropriate cache headers (`cache-control`, `etag`) for edge-cached pages
8. Use `TransformStream` for streaming partial HTML responses
9. Fall back to origin server when edge computation exceeds budget
10. Monitor edge execution time to stay within provider limits (typically 10-50ms CPU)

## Common Pitfalls

1. **Cold start latency** — Edge worker first invocation may add 50-200ms
2. **CPU time limits** — Complex rendering may exceed edge worker compute quotas
3. **No file system** — Edge workers cannot read local files; use KV or R2 storage
4. **Limited API access** — Some Node.js APIs are unavailable in edge runtimes
5. **Bootstrap JS not at edge** — Interactive behavior still requires client-side Bootstrap JS
6. **Caching complexity** — Invalidation strategies for edge-cached pages are harder than origin
7. **Debug difficulty** — Edge logs are distributed across global nodes
8. **Streaming compatibility** — Some proxies/browsers buffer streamed responses

## Accessibility Considerations

Edge-rendered HTML includes all ARIA attributes immediately in the first byte. Streaming doesn't affect accessibility since screen readers process HTML as it arrives. Ensure the edge-rendered document includes complete `<head>` content (title, meta, lang) before streaming body content. Test with assistive technology to verify progressive rendering doesn't cause reading order issues.

## Responsive Behavior

Edge workers can detect device type from `user-agent` and serve mobile-optimized Bootstrap markup. Use edge-side device detection to choose between mobile and desktop component variants. Apply Bootstrap's responsive grid classes consistently regardless of rendering location. Cache separate mobile/desktop variants at the edge for optimal serving.
