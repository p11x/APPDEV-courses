---
title: "Security Headers"
difficulty: 2
category: "Advanced Development"
subcategory: "Security Patterns"
prerequisites:
  - HTTP Security Headers
  - Nginx/Apache Configuration
  - Express.js Middleware
---

## Overview

HTTP security headers protect Bootstrap applications from common web attacks by instructing the browser how to behave regarding content types, framing, referrer information, and other security-sensitive concerns. The key headers are X-Content-Type-Options (prevents MIME sniffing), X-Frame-Options (prevents clickjacking), Referrer-Policy (controls referrer leakage), and Strict-Transport-Security (enforces HTTPS).

These headers work alongside CSP and CORS to create a comprehensive security posture. They require no code changes to Bootstrap components - they are server-level configurations that apply to all responses.

## Basic Implementation

```nginx
# Nginx security headers
server {
  # Prevent MIME type sniffing
  add_header X-Content-Type-Options "nosniff" always;

  # Prevent framing (clickjacking protection)
  add_header X-Frame-Options "DENY" always;

  # Control referrer information
  add_header Referrer-Policy "strict-origin-when-cross-origin" always;

  # Enforce HTTPS (2 years, include subdomains)
  add_header Strict-Transport-Security "max-age=63072000; includeSubDomains; preload" always;

  # Permissions policy (feature restrictions)
  add_header Permissions-Policy "camera=(), microphone=(), geolocation=()" always;

  # Remove server identification
  server_tokens off;
}
```

```js
// Express.js security headers middleware
// Using helmet.js for comprehensive headers
const helmet = require('helmet');

app.use(helmet({
  contentSecurityPolicy: false, // Configure separately (see CSP section)
  crossOriginEmbedderPolicy: true,
  crossOriginOpenerPolicy: { policy: 'same-origin' },
  crossOriginResourcePolicy: { policy: 'same-origin' },
  dnsPrefetchControl: { allow: true },
  frameguard: { action: 'deny' },
  hidePoweredBy: true,
  hsts: { maxAge: 63072000, includeSubDomains: true, preload: true },
  ieNoOpen: true,
  noSniff: true,
  referrerPolicy: { policy: 'strict-origin-when-cross-origin' },
  xssFilter: true
}));
```

```js
// Manual header configuration
app.use((req, res, next) => {
  // Prevent MIME sniffing
  res.setHeader('X-Content-Type-Options', 'nosniff');

  // Prevent framing
  res.setHeader('X-Frame-Options', 'DENY');

  // Control referrer
  res.setHeader('Referrer-Policy', 'strict-origin-when-cross-origin');

  // XSS protection (legacy browsers)
  res.setHeader('X-XSS-Protection', '1; mode=block');

  // Enforce HTTPS
  res.setHeader('Strict-Transport-Security', 'max-age=63072000; includeSubDomains');

  // Feature policy
  res.setHeader('Permissions-Policy', 'camera=(), microphone=(), geolocation=(), payment=()');

  // Remove X-Powered-By
  res.removeHeader('X-Powered-By');

  next();
});
```

## Advanced Variations

```yaml
# Cloudflare security header configuration
# Using Cloudflare Transform Rules
rules:
  - expression: true
    headers:
      set:
        X-Content-Type-Options: nosniff
        X-Frame-Options: DENY
        Referrer-Policy: strict-origin-when-cross-origin
        Permissions-Policy: "camera=(), microphone=()"
```

```js
// Security header validation script
// scripts/check-headers.js
async function checkSecurityHeaders(url) {
  const response = await fetch(url);
  const headers = response.headers;

  const expected = {
    'x-content-type-options': 'nosniff',
    'x-frame-options': ['DENY', 'SAMEORIGIN'],
    'referrer-policy': 'strict-origin-when-cross-origin',
    'strict-transport-security': (v) => v && v.includes('max-age'),
    'permissions-policy': (v) => v && v.length > 0
  };

  const results = [];
  Object.entries(expected).forEach(([header, check]) => {
    const value = headers.get(header);
    const passed = typeof check === 'function'
      ? check(value)
      : Array.isArray(check)
        ? check.includes(value)
        : value === check;

    results.push({ header, value: value || 'MISSING', passed });
  });

  results.forEach(r => {
    console.log(`${r.passed ? 'PASS' : 'FAIL'}: ${r.header}: ${r.value}`);
  });

  return results;
}
```

## Best Practices

1. **Use Helmet.js** - Comprehensive security header configuration with sensible defaults.
2. **Set X-Content-Type-Options: nosniff** - Prevents browsers from MIME-sniffing responses.
3. **Set X-Frame-Options: DENY** - Prevents clickjacking unless you specifically need framing.
4. **Use strict Referrer-Policy** - `strict-origin-when-cross-origin` balances security and analytics.
5. **Enable HSTS** - Forces HTTPS with long max-age; submit to preload list after testing.
6. **Configure Permissions-Policy** - Disable camera, microphone, geolocation if unused.
7. **Remove server identification** - `server_tokens off` in Nginx; `helmet.hidePoweredBy()` in Express.
8. **Test with securityheaders.com** - Grade your header configuration online.
9. **Include in CI** - Automated header checks prevent regression.
10. **Document exceptions** - If you need to allow framing for specific pages, document why.

## Common Pitfalls

1. **Missing always keyword** - Nginx doesn't add headers on error pages without `always`.
2. **X-Frame-Options too restrictive** - Using `DENY` breaks legitimate iframe embeds.
3. **HSTS without testing** - HSTS with preload is hard to undo; test thoroughly first.
4. **Conflicting policies** - CSP frame-ancestors and X-Frame-Options should be consistent.
5. **Missing on API responses** - Security headers should apply to API endpoints too.

## Accessibility Considerations

Security headers don't directly impact accessibility. However, strict CSP and X-Frame-Options may prevent embedding accessible widgets from third parties.

## Responsive Behavior

Security headers are independent of viewport size. The same headers apply at all screen sizes.
