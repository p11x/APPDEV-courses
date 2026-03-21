# Source Maps

## What You'll Learn
- Understand source maps and why they matter
- Configure source maps in Next.js
- Upload source maps to Sentry

## Prerequisites
- Understanding of Sentry setup

## Do I Need This Right Now?
Source maps make error stack traces readable. Without them, errors show as cryptic minified code. This is essential for debugging production errors.

## Concept Explained Simply

Source maps are like having the original blueprint when something goes wrong. When you build your app for production, the code gets minified (compressed) — like tearing up the blueprint and keeping only the finished building. When something breaks, source maps let you see the original code, not the compressed mess.

## How Source Maps Work

```
Original Code          Minified Code           Source Map
─────────────────      ──────────────         ───────────
function add(a, b) → function r(t,u)        "mappings": "AAAA..."
return t + u                                      
```

With source maps, Sentry shows:
```
Error: Cannot read property 'name' of undefined
  at addUser (src/utils/user.ts:15:5)
  at Page (src/app/page.tsx:8:3)
```

Without source maps, it would show:
```
Error: Cannot read property 'name' of undefined
  at r (bundle.js:15:2834)
```

## Configuring Source Maps

### Option 1: Automatic (Sentry Wizard)

The wizard already configures this:

```javascript
// next.config.js
const nextConfig = {
  sentry: {
    // Automatically uploads source maps
    widenClientFileUpload: true,
    hideSourceMaps: true,
  },
};
```

### Option 2: Manual Configuration

```javascript
// next.config.js
const withSentryConfig = require('@sentry/nextjs/withSentryConfig');

const nextConfig = {
  // Your existing config
  reactStrictMode: true,
};

const sentryConfig = {
  // Upload source maps to Sentry automatically
  silent: true, // Don't fail build on Sentry errors
  org: 'your-org',
  project: 'your-project',
  widenClientFileUpload: true,
  hideSourceMaps: true,
  disableLogger: true,
};

module.exports = withSentryConfig(nextConfig, sentryConfig);
```

### Option 3: Vercel Integration

If deploying to Vercel, it works automatically:

1. Install Sentry in Vercel
2. Add your DSN
3. Source maps upload automatically

## Environment Variables

```bash
# In your deployment platform
SENTRY_ORG=your-org
SENTRY_PROJECT=your-project
SENTRY_AUTH_TOKEN=your-auth-token
```

## Viewing Source Maps in Sentry

1. Go to your project in Sentry
2. Click on an error
3. Look for "Original" vs "Minified" toggle

## Disabling Source Maps

For extra security in production:

```javascript
// next.config.js
const nextConfig = {
  sentry: {
    // Hide source maps from browser DevTools
    hideSourceMaps: true,
    // Still upload to Sentry
    widenClientFileUpload: true,
  },
};
```

But still readable in Sentry because you uploaded them there!

## Common Mistakes

### Mistake #1: Not Uploading Source Maps
```javascript
// Wrong: Source maps not configured
const nextConfig = {};
// Errors will show as minified!
```

```javascript
// Correct: Configure source map upload
const nextConfig = {
  sentry: {
    hideSourceMaps: true, // Hide from browser, upload to Sentry
  },
};
```

### Mistake #2: Hiding Source Maps Without Uploading
```javascript
// Wrong: Can't see original code anywhere!
const nextConfig = {
  sentry: {
    hideSourceMaps: true,
    // Missing: Upload to Sentry first!
  },
};
```

```javascript
// Correct: Upload to Sentry before hiding
const nextConfig = {
  sentry: {
    widenClientFileUpload: true, // Upload to Sentry
    hideSourceMaps: true,        // Hide from browser
  },
};
```

### Mistake #3: Wrong Organization Name
```javascript
// Wrong: Wrong org/project
const sentryConfig = {
  org: 'my-org',       // Should match Sentry URL
  project: 'my-project', // Should match exactly
};
```

## Summary
- Source maps map minified code back to original
- Configure in next.config.js
- Use `widenClientFileUpload: true` for broader browser support
- Use `hideSourceMaps: true` to hide from browser DevTools
- Sentry still has the source maps for debugging
- Vercel deployments work automatically

## Next Steps
- [server-side-errors.md](../02-capturing-errors/server-side-errors.md) — Capturing server errors
