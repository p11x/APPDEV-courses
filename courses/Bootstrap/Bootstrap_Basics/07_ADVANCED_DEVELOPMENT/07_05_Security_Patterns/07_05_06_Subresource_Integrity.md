---
title: "Subresource Integrity"
difficulty: 2
category: "Advanced Development"
subcategory: "Security Patterns"
prerequisites:
  - SRI Hash Generation
  - CDN Security
  - Script Loading Attributes
---

## Overview

Subresource Integrity (SRI) validates that files loaded from CDNs or external sources haven't been tampered with. The `integrity` attribute on `<script>` and `<link>` tags contains a cryptographic hash of the expected file content. The browser computes the hash of the downloaded file and compares it against the integrity value; if they don't match, the browser blocks the resource.

SRI is essential for CDN-loaded Bootstrap because CDN compromise could serve malicious code to all users. By pinning the integrity hash, the application ensures only the expected Bootstrap version loads, even if the CDN is compromised.

## Basic Implementation

```bash
# Generate SRI hash for a file
openssl dgst -sha384 -binary bootstrap.min.css | openssl base64 -A
# Output: sha384-hash-here

# Using the SRI Hash Generator
curl -s https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css | \
  openssl dgst -sha384 -binary | openssl base64 -A
```

```html
<!-- Bootstrap CDN with SRI -->
<link rel="stylesheet"
      href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"
      integrity="sha384-9ndCyUaIbzAi2FUVXJi0CjmCapSmO7gy/l5VBG2AeOa4U9Q0ksQ5kCEPlbQ"
      crossorigin="anonymous">

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"
        integrity="sha384-ENjdO4Dr2bkBIFxQpeoTz1HIcje39Wm4jDKdf19U8gI4ddQ3GYNS7NTKfAdVQSZe"
        crossorigin="anonymous"></script>
```

```js
// Build script to generate SRI hashes
// scripts/generate-sri.js
const crypto = require('crypto');
const https = require('https');

function fetchAndHash(url) {
  return new Promise((resolve, reject) => {
    https.get(url, (res) => {
      const chunks = [];
      res.on('data', chunk => chunks.push(chunk));
      res.on('end', () => {
        const content = Buffer.concat(chunks);
        const hash384 = crypto.createHash('sha384').update(content).digest('base64');
        const hash512 = crypto.createHash('sha512').update(content).digest('base64');
        resolve({
          sha384: `sha384-${hash384}`,
          sha512: `sha512-${hash512}`
        });
      });
    }).on('error', reject);
  });
}

async function generateSRITags() {
  const files = [
    { name: 'bootstrap-css', url: 'https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css' },
    { name: 'bootstrap-js', url: 'https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js' }
  ];

  for (const file of files) {
    const hashes = await fetchAndHash(file.url);
    console.log(`<!-- ${file.name} -->`);
    console.log(`integrity="${hashes.sha384}"`);
  }
}

generateSRITags();
```

## Best Practices

1. **Use SRI for all CDN resources** - Every external script and stylesheet needs integrity checking.
2. **Use sha384 minimum** - sha256 is too weak; sha384 is the standard; sha512 for extra security.
3. **Include crossorigin attribute** - Required for SRI to work with CORS-enabled CDNs.
4. **Regenerate hashes on version update** - Every Bootstrap version update needs new SRI hashes.
5. **Automate hash generation** - Build scripts should generate SRI hashes automatically.
6. **Pin exact versions** - Never use `@latest` with SRI; the hash won't match future versions.
7. **Include fallback CDN** - If primary CDN fails, provide a fallback with its own integrity hash.
8. **Test SRI enforcement** - Verify that tampered files are actually blocked by the browser.
9. **Document SRI in CSP** - CSP's `require-sri-for` directive enforces SRI for scripts and styles.
10. **Use in combination with CSP** - SRI + CSP provides defense-in-depth for external resources.

## Common Pitfalls

1. **Missing crossorigin** - SRI silently fails without the `crossorigin="anonymous"` attribute.
2. **Wrong hash algorithm** - Using sha256 when sha384 is expected causes validation failure.
3. **Stale hashes** - Updating file URLs without updating integrity hashes blocks resources.
4. **Encoding mismatch** - Hash must be base64-encoded; hex encoding doesn't work.
5. **Local development issues** - SRI hashes for local files change on every edit; disable for dev.

## Accessibility Considerations

SRI enforcement that blocks resources should degrade gracefully. Ensure the page remains accessible even if a non-critical CDN resource fails to load.

## Responsive Behavior

SRI is independent of responsive design. The same integrity hashes apply at all viewport sizes.
