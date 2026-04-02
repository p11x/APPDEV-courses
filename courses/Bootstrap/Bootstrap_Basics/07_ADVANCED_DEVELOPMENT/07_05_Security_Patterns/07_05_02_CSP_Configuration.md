---
title: "CSP Configuration"
difficulty: 3
category: "Advanced Development"
subcategory: "Security Patterns"
prerequisites:
  - Content-Security-Policy Headers
  - Nonce-Based Script Loading
  - Bootstrap 5 Compatibility
---

## Overview

Content Security Policy (CSP) headers prevent XSS attacks by controlling which resources the browser is allowed to load and execute. A strict CSP disables inline scripts and styles, blocks eval(), and restricts resource origins. Bootstrap 5 is mostly CSP-compatible but requires nonce-based loading for inline scripts and careful configuration for dynamically generated content.

The strictest CSP uses nonce-based script and style loading: each inline `<script>` and `<style>` tag receives a unique, per-request nonce that the CSP header validates. This prevents injected scripts from executing because attackers cannot predict the nonce value.

Bootstrap's JavaScript is CSP-compatible out of the box when loaded from an external file. The challenge is inline scripts that Bootstrap documentation often shows (event handlers, initialization code). These must be refactored into external scripts or loaded with nonces.

## Basic Implementation

```nginx
# Nginx CSP header
add_header Content-Security-Policy "
  default-src 'self';
  script-src 'self' 'nonce-$request_id';
  style-src 'self' 'nonce-$request_id' https://fonts.googleapis.com;
  font-src 'self' https://fonts.gstatic.com;
  img-src 'self' data: https:;
  connect-src 'self' https://api.example.com;
  frame-ancestors 'none';
  base-uri 'self';
  form-action 'self';
" always;
```

```js
// Express.js CSP middleware
const crypto = require('crypto');

function cspMiddleware(req, res, next) {
  const nonce = crypto.randomBytes(16).toString('base64');
  res.locals.nonce = nonce;

  res.setHeader('Content-Security-Policy', [
    "default-src 'self'",
    `script-src 'self' 'nonce-${nonce}'`,
    `style-src 'self' 'nonce-${nonce}'`,
    "img-src 'self' data: https:",
    "font-src 'self'",
    "connect-src 'self'",
    "frame-ancestors 'none'",
    "base-uri 'self'",
    "form-action 'self'"
  ].join('; '));

  next();
}
```

```html
<!-- HTML with nonce-based script loading -->
<head>
  <style nonce="RANDOM_NONCE">
    /* Critical inline styles */
    .hero { padding: 3rem 0; }
  </style>
</head>
<body>
  <!-- Bootstrap loaded from external file (CSP-safe) -->
  <script src="/js/bootstrap.bundle.min.js" defer></script>

  <!-- Inline script with nonce -->
  <script nonce="RANDOM_NONCE">
    document.addEventListener('DOMContentLoaded', () => {
      const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
      tooltipTriggerList.forEach(el => new bootstrap.Tooltip(el));
    });
  </script>
</body>
```

## Advanced Variations

```js
// CSP with hash-based fallback
// Build script generates hashes for inline scripts
const crypto = require('crypto');
const fs = require('fs');

function generateCSPHashes(htmlFile) {
  const html = fs.readFileSync(htmlFile, 'utf8');
  const scriptRegex = /<script(?![^>]*src)[^>]*>([\s\S]*?)<\/script>/gi;
  const styleRegex = /<style[^>]*>([\s\S]*?)<\/style>/gi;
  const hashes = [];

  let match;
  while ((match = scriptRegex.exec(html)) !== null) {
    const content = match[1].trim();
    const hash = crypto.createHash('sha256').update(content).digest('base64');
    hashes.push(`'sha256-${hash}'`);
  }

  while ((match = styleRegex.exec(html)) !== null) {
    const content = match[1].trim();
    const hash = crypto.createHash('sha256').update(content).digest('base64');
    hashes.push(`'sha256-${hash}'`);
  }

  return hashes;
}

const hashes = generateCSPHashes('dist/index.html');
const scriptPolicy = `'self' ${hashes.join(' ')}`;
console.log(`script-src ${scriptPolicy}`);
```

```js
// CSP violation reporting
// Report-only mode first, then enforce
app.use((req, res, next) => {
  // Report-only (doesn't block, just reports)
  res.setHeader('Content-Security-Policy-Report-Only', [
    "default-src 'self'",
    `script-src 'self'`,
    "report-uri /api/csp-report"
  ].join('; '));

  next();
});

// CSP violation endpoint
app.post('/api/csp-report', (req, res) => {
  const report = req.body['csp-report'];
  console.error('CSP Violation:', {
    blocked: report['blocked-uri'],
    violated: report['violated-directive'],
    source: report['source-file'],
    line: report['line-number']
  });
  res.status(204).end();
});
```

## Best Practices

1. **Start with report-only** - Deploy CSP in report-only mode first to catch violations without breaking the site.
2. **Use nonces over hashes** - Nonces are more flexible; hashes require recomputation for every change.
3. **Avoid unsafe-inline** - This directive defeats the purpose of CSP for scripts.
4. **Avoid unsafe-eval** - Blocks eval(), new Function(), and setTimeout with strings.
5. **Generate nonces per request** - Each page load gets a unique nonce to prevent replay attacks.
6. **Externalize all scripts** - Move inline scripts to external files loaded with `defer`.
7. **Include report-uri** - Monitor CSP violations to catch attacks and misconfigurations.
8. **Test with CSP evaluator** - Use Google's CSP Evaluator to check policy strength.
9. **Version your policy** - Track CSP changes alongside code deployments.
10. **Separate concerns** - Use different policies for scripts, styles, and images.

## Common Pitfalls

1. **unsafe-inline for scripts** - Allows injected scripts to execute; defeats CSP purpose.
2. **Static nonces** - Using the same nonce across requests allows attackers to predict it.
3. **Missing report-uri** - Violations go unnoticed until users report broken functionality.
4. **Too strict initially** - Deploying strict CSP without testing breaks the application.
5. **CDN scripts without domain allowlist** - Bootstrap CDN scripts blocked by restrictive `script-src`.

## Accessibility Considerations

CSP should not block assistive technology scripts or accessibility-related CSS. Allow necessary third-party accessibility widgets in the policy.

## Responsive Behavior

CSP is independent of responsive design. The same policy applies at all viewport sizes.
