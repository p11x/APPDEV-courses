---
title: "Static Site Deployment for Bootstrap 5"
section: "05_04_Deployment_Guides"
file: "05_04_05_Static_Site_Deployment.md"
difficulty: 1
tags: ["netlify", "vercel", "github-pages", "static-site", "deployment"]
duration: "10 minutes"
prerequisites:
  - "Production build configured"
  - "Git repository initialized"
learning_objectives:
  - "Deploy Bootstrap sites to Netlify, Vercel, and GitHub Pages"
  - "Configure build commands and environment variables"
  - "Set up custom domains with SSL"
---

# Static Site Deployment for Bootstrap 5

## Overview

Static site hosting platforms eliminate server management by serving pre-built HTML, CSS, and JS files directly from a CDN. For Bootstrap sites without a backend, these platforms provide zero-config deployments with built-in SSL, global CDN distribution, and Git-based continuous deployment.

**Netlify**, **Vercel**, and **GitHub Pages** are the three dominant platforms. Netlify excels at form handling and redirects, Vercel at performance and edge functions, and GitHub Pages at simplicity and cost (free for public repos). All three support Bootstrap projects identically — the build output is plain static files.

---

## Basic Implementation

### Netlify Deployment

```toml
# netlify.toml
[build]
  command = "npm run build"
  publish = "dist"

[[redirects]]
  from = "/*"
  to = "/index.html"
  status = 200
```

```bash
# Deploy via CLI
npm install -g netlify-cli
netlify init
netlify deploy --prod
```

### Vercel Deployment

```json
// vercel.json
{
  "buildCommand": "npm run build",
  "outputDirectory": "dist",
  "rewrites": [
    { "source": "/(.*)", "destination": "/index.html" }
  ]
}
```

```bash
# Deploy via CLI
npm install -g vercel
vercel --prod
```

### GitHub Pages Deployment

```yaml
# .github/workflows/deploy.yml
name: Deploy to GitHub Pages
on:
  push:
    branches: [main]
jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with: { node-version: 20 }
      - run: npm ci && npm run build
      - uses: peaceiris/actions-gh-pages@v3
        with:
          github_token: ${{ secrets.GITHUB_TOKEN }}
          publish_dir: ./dist
```

---

## Advanced Variations

### Environment Variables

```bash
# Netlify
netlify env:set API_URL "https://api.production.com"

# Vercel
vercel env add API_URL production

# GitHub Pages (via repository settings)
# Settings > Secrets > Actions > New repository secret
```

### Custom Domain with SSL (Netlify)

```toml
# netlify.toml
[[redirects]]
  from = "https://www.mysite.com/*"
  to = "https://mysite.com/:splat"
  status = 301

# DNS: Add CNAME record pointing to your-site.netlify.app
# SSL: Auto-provisioned via Let's Encrypt
```

### Preview Deployments (Branch Previews)

```toml
# netlify.toml
[context.deploy-preview]
  command = "npm run build:staging"
  environment = { API_URL = "https://staging-api.mysite.com" }
```

---

## Best Practices

1. **Use `npm ci` instead of `npm install`** in CI — faster, deterministic, respects lock file exactly
2. **Set `publish` directory correctly** — mismatched paths cause 404s on all assets
3. **Configure SPA redirects** — Bootstrap SPAs need `/* -> /index.html` to handle client-side routing
4. **Use environment variables for API URLs** — never hardcode production endpoints in source
5. **Enable branch preview deployments** — test changes before merging to main
6. **Set Node.js version explicitly** in build config — prevents "works on my machine" issues
7. **Add `package-lock.json` to version control** — ensures reproducible builds on all platforms
8. **Configure build caching** — Netlify and Vercel cache `node_modules` by default; verify it's enabled
9. **Use deploy notifications** — Slack/Discord webhooks for deployment status alerts
10. **Set `robots.txt` to block preview deployments** — prevent search engines from indexing staging URLs
11. **Monitor build minutes** — free tiers have limits; optimize build times with caching
12. **Use `_redirects` file for simple rules** — faster than parsing `netlify.toml` for basic redirects

---

## Common Pitfalls

1. **Wrong output directory** — building to `build/` but configuring `publish: "dist"` results in blank page
2. **Missing SPA redirect** — direct navigation to `/about` returns 404 instead of serving `index.html`
3. **Hardcoded localhost API URLs** — production site calls `http://localhost:3000/api` which always fails
4. **Not setting Node version** — platform defaults to an old Node version, build fails on unsupported syntax
5. **Committing `node_modules`** — bloats repo, slows deploys; use `.gitignore` to exclude it
6. **Forgetting to set environment variables** — build succeeds but runtime features fail silently
7. **Using absolute paths in assets** — `/css/main.css` breaks when deployed to a subdirectory on GitHub Pages

---

## Accessibility Considerations

Static site platforms serve files verbatim — they do not modify HTML. Ensure your build output includes proper `lang` attributes, ARIA landmarks, and semantic heading hierarchy. Test deployed sites with Lighthouse accessibility audits, not just local dev server results, since CDN delivery timing can affect screen reader behavior.

Configure `404.html` pages with accessible markup and navigation links so users encountering errors can recover using keyboard navigation.

---

## Responsive Behavior

Static site hosts deliver the same responsive CSS to all devices. Bootstrap's client-side media query handling works identically on all platforms. Verify that compressed CSS delivery does not strip media query syntax by testing the deployed site at multiple viewport widths using browser DevTools device emulation.
