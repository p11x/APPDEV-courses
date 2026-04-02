# Helmet

## What You'll Learn

- What HTTP security headers are and why they matter
- How to use helmet to set security headers automatically
- How to configure Content Security Policy (CSP)
- How to enable HSTS, X-Frame-Options, and other protections
- How to customize helmet for your application

## Why Security Headers?

Browsers read HTTP headers to decide security behavior. Without proper headers:
- Your site can be embedded in an attacker's iframe (clickjacking)
- Scripts from untrusted sources can execute (XSS)
- Connections can be downgraded from HTTPS to HTTP (MITM attacks)

**Helmet** is an Express middleware that sets these headers automatically.

## Project Setup

```bash
npm install express helmet
```

## Basic Helmet

```js
// server.js — Express server with helmet security headers

import express from 'express';
import helmet from 'helmet';

const app = express();

// helmet() applies a set of security headers with sensible defaults
app.use(helmet());

// Your routes
app.get('/', (req, res) => {
  res.json({ message: 'Secure server with helmet' });
});

app.get('/api/data', (req, res) => {
  res.json({ users: ['Alice', 'Bob'] });
});

app.listen(3000, () => {
  console.log('Secure server on http://localhost:3000');
});
```

### Headers Set by Default

| Header | Value | Purpose |
|--------|-------|---------|
| `Content-Security-Policy` | Strict policy | Controls which resources can load |
| `X-Frame-Options` | `SAMEORIGIN` | Prevents clickjacking via iframes |
| `X-Content-Type-Options` | `nosniff` | Prevents MIME type sniffing |
| `Strict-Transport-Security` | `max-age=15552000` | Forces HTTPS for 180 days |
| `X-DNS-Prefetch-Control` | `off` | Disables DNS prefetching |
| `X-Permitted-Cross-Domain-Policies` | `none` | Blocks Flash/PDF cross-domain |
| `Referrer-Policy` | `no-referrer` | Controls referrer information |

## Custom Configuration

```js
// custom-helmet.js — Customize helmet for a real application

import express from 'express';
import helmet from 'helmet';

const app = express();

app.use(
  helmet({
    // Content Security Policy — controls where resources can be loaded from
    contentSecurityPolicy: {
      directives: {
        // default-src: fallback for all resource types
        defaultSrc: ["'self'"],                // Only load from same origin

        // script-src: where JavaScript can be loaded from
        scriptSrc: ["'self'", "'nonce-abc123'"],  // Only inline with nonce

        // style-src: where CSS can be loaded from
        styleSrc: ["'self'", "'unsafe-inline'"],  // Allow inline styles

        // img-src: where images can be loaded from
        imgSrc: ["'self'", 'data:', 'https:'],    // Same origin, data URIs, any HTTPS

        // connect-src: where fetch/XHR/WebSocket can connect to
        connectSrc: ["'self'", 'https://api.example.com'],

        // font-src: where fonts can be loaded from
        fontSrc: ["'self'", 'https://fonts.gstatic.com'],

        // object-src: block Flash and other plugins
        objectSrc: ["'none'"],

        // frame-ancestors: who can embed this page in an iframe
        frameAncestors: ["'self'"],

        // upgrade-insecure-requests: auto-upgrade HTTP to HTTPS
        upgradeInsecureRequests: [],
      },
    },

    // HTTP Strict Transport Security
    hsts: {
      maxAge: 31536000,     // 1 year in seconds
      includeSubDomains: true,  // Apply to all subdomains
      preload: true,            // Allow inclusion in browser HSTS preload list
    },

    // X-Frame-Options
    frameguard: {
      action: 'deny',  // 'deny' = never allow framing, 'sameorigin' = only same domain
    },

    // Referrer-Policy
    referrerPolicy: {
      policy: 'strict-origin-when-cross-origin',
      // Full URL for same-origin, only origin for cross-origin, nothing for downgrades
    },
  })
);

// Serve a simple HTML page to test CSP
app.get('/', (req, res) => {
  // CSP prevents inline scripts unless 'unsafe-inline' or a nonce is used
  res.send(`
    <!DOCTYPE html>
    <html>
    <head><title>CSP Test</title></head>
    <body>
      <h1>Check the response headers</h1>
      <p>Open DevTools → Network → check the response headers for security headers</p>
    </body>
    </html>
  `);
});

app.listen(3000, () => {
  console.log('Server with custom helmet on http://localhost:3000');
});
```

## Selective Middleware

```js
// selective.js — Use only specific helmet middlewares

import express from 'express';
import helmet from 'helmet';

const app = express();

// Use individual helmet middlewares instead of the full set
app.use(helmet.noSniff());           // X-Content-Type-Options: nosniff
app.use(helmet.frameguard({ action: 'deny' }));  // X-Frame-Options: DENY
app.use(helmet.hsts({ maxAge: 31536000 }));       // Strict-Transport-Security

// Disable a specific header
app.use(helmet({ crossOriginEmbedderPolicy: false }));  // Needed for some CDNs

app.listen(3000);
```

## How It Works

### How CSP Prevents XSS

```js
// Without CSP — attacker injects a script tag into your page
// <script src="https://evil.com/steal-cookies.js"></script>
// Browser loads and executes it

// With CSP: scriptSrc: ["'self'"]
// Browser checks: is "https://evil.com" in the allowed list? No.
// Browser blocks the script from loading
```

### How X-Frame-Options Prevents Clickjacking

```html
<!-- Attacker's page -->
<iframe src="https://your-bank.com/transfer" style="opacity: 0">
  <!-- User thinks they are clicking a game, but actually clicking "Transfer" -->
</iframe>

<!-- With X-Frame-Options: DENY, the browser refuses to load the page in an iframe -->
```

## Common Mistakes

### Mistake 1: Using `'unsafe-inline'` in Production

```js
// WRONG — unsafe-inline defeats the purpose of CSP
scriptSrc: ["'self'", "'unsafe-inline'"],  // Any inline script runs
// An XSS attack that injects <script>alert('xss')</script> succeeds

// CORRECT — use nonces or hashes for inline scripts
scriptSrc: ["'self'", "'nonce-abc123'"],
// Then in your HTML: <script nonce="abc123">...</script>
```

### Mistake 2: Forgetting to Test CSP

```js
// WRONG — deploying strict CSP breaks your app
// Images, scripts, styles from CDNs are blocked
// Users see blank pages

// CORRECT — use report-only mode first
app.use(
  helmet({
    contentSecurityPolicy: {
      directives: { /* your policy */ },
      reportOnly: true,  // Log violations but do not block
    },
  })
);
// Monitor logs, fix violations, then switch to enforcement
```

### Mistake 3: Not Including Subdomains in HSTS

```js
// WRONG — only the main domain is protected
hsts: { maxAge: 31536000 };
// api.yourdomain.com can still be accessed over HTTP

// CORRECT — include all subdomains
hsts: { maxAge: 31536000, includeSubDomains: true };
```

## Try It Yourself

### Exercise 1: Inspect Headers

Start the server and open `http://localhost:3000` in the browser. Open DevTools → Network → check the response headers. List every security header you see.

### Exercise 2: CSP Violation

Create an HTML page that tries to load a script from `https://evil.com`. With CSP enabled, verify the browser blocks it. Check the console for the violation message.

### Exercise 3: Custom CSP

Configure CSP to allow scripts from your domain and one CDN (e.g., `https://cdn.jsdelivr.net`). Verify that scripts from other CDNs are blocked.

## Next Steps

You have security headers. For rate limiting API requests, continue to [Rate Limiting](./02-rate-limiting.md).
