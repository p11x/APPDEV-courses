---
title: "AWS, GCP, and Azure Deployment for Bootstrap 5"
section: "05_04_Deployment_Guides"
file: "05_04_08_AWS_GCP_Azure_Deployment.md"
difficulty: 3
tags: ["aws", "s3", "cloudfront", "firebase", "azure", "cloud"]
duration: "15 minutes"
prerequisites:
  - "Cloud provider account (AWS, GCP, or Azure)"
  - "Production build completed"
  - "CLI tools installed for chosen provider"
learning_objectives:
  - "Deploy Bootstrap sites to AWS S3 + CloudFront"
  - "Configure Firebase Hosting and Azure Static Web Apps"
  - "Set up CDN caching policies per platform"
---

# AWS, GCP, and Azure Deployment for Bootstrap 5

## Overview

Major cloud providers offer static hosting with integrated CDN, SSL, and custom domain support. **AWS S3 + CloudFront**, **Firebase Hosting (GCP)**, and **Azure Static Web Apps** are the primary options for deploying Bootstrap sites at enterprise scale. Each provides global edge distribution, automated SSL certificate management, and CLI-based deployment workflows.

The choice depends on your existing infrastructure: AWS for teams already in the AWS ecosystem, Firebase for Google Cloud projects and rapid prototyping, and Azure for enterprises with Microsoft 365 or Active Directory integration.

---

## Basic Implementation

### AWS S3 + CloudFront

```bash
# 1. Create S3 bucket
aws s3 mb s3://my-bootstrap-site

# 2. Enable static website hosting
aws s3 website s3://my-bootstrap-site \
  --index-document index.html \
  --error-document 404.html

# 3. Upload build output
aws s3 sync dist/ s3://my-bootstrap-site \
  --delete \
  --cache-control "public, max-age=31536000, immutable" \
  --exclude "*.html"

# 4. Upload HTML with no-cache
aws s3 sync dist/ s3://my-bootstrap-site \
  --cache-control "no-cache" \
  --exclude "*" --include "*.html"
```

### Firebase Hosting

```json
// firebase.json
{
  "hosting": {
    "public": "dist",
    "ignore": ["firebase.json", "**/.*", "**/node_modules/**"],
    "rewrites": [{ "source": "**", "destination": "/index.html" }],
    "headers": [
      {
        "source": "**/*.@(js|css|woff2|png|jpg)",
        "headers": [{ "key": "Cache-Control", "value": "public, max-age=31536000, immutable" }]
      }
    ]
  }
}
```

```bash
npm install -g firebase-tools
firebase login
firebase init hosting
firebase deploy --only hosting
```

### Azure Static Web Apps

```yaml
# .github/workflows/azure-deploy.yml
name: Deploy to Azure
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
      - uses: Azure/static-web-apps-deploy@v1
        with:
          azure_static_web_apps_api_token: ${{ secrets.AZURE_TOKEN }}
          repo_token: ${{ secrets.GITHUB_TOKEN }}
          action: upload
          app_location: dist
```

---

## Advanced Variations

### CloudFront Cache Policy (Terraform)

```hcl
resource "aws_cloudfront_distribution" "site" {
  origin {
    domain_name = aws_s3_bucket.site.bucket_regional_domain_name
    origin_id   = "S3-my-bootstrap-site"
  }

  default_cache_behavior {
    allowed_methods  = ["GET", "HEAD"]
    cached_methods   = ["GET", "HEAD"]
    target_origin_id = "S3-my-bootstrap-site"
    viewer_protocol_policy = "redirect-to-https"
    compress               = true
    min_ttl                = 0
    default_ttl            = 86400
    max_ttl                = 31536000

    forwarded_values {
      query_string = false
      cookies { forward = "none" }
    }
  }

  default_root_object = "index.html"
  enabled             = true
  is_ipv6_enabled     = true
}
```

### Azure Static Web App Configuration

```json
// staticwebapp.config.json
{
  "navigationFallback": {
    "rewrite": "/index.html",
    "exclude": ["/assets/*.{js,css,woff2,png,jpg}"]
  },
  "responseOverrides": {
    "404": {
      "rewrite": "/404.html",
      "statusCode": 404
    }
  },
  "routes": [
    {
      "route": "/assets/*",
      "headers": { "cache-control": "public, max-age=31536000, immutable" }
    }
  ]
}
```

---

## Best Practices

1. **Separate asset and HTML caching** — `immutable` for hashed assets, `no-cache` for HTML on all platforms
2. **Enable Brotli/Gzip compression** at the CDN edge — reduces transfer sizes by 60-80%
3. **Use infrastructure-as-code** (Terraform, CloudFormation) for reproducible cloud configurations
4. **Set up custom domains with managed SSL** — all three platforms auto-provision certificates
5. **Configure SPA fallback routing** — `index.html` rewrite for client-side routing support
6. **Deploy with `--delete` flag** (S3) to remove stale assets from previous builds
7. **Use separate buckets/sites for staging and production** — isolate environments
8. **Set up CloudWatch/Firebase Analytics monitoring** for traffic and error tracking
9. **Enable HTTP/2 and HTTP/3** at the CDN layer for multiplexed asset delivery
10. **Use `Cache-Control: immutable`** on content-hashed assets to eliminate revalidation requests
11. **Configure Origin Access Identity (OAI)** for S3 — keep bucket private, serve only through CloudFront
12. **Set up automated deployments via CI/CD** — never deploy manually from local machines in production

---

## Common Pitfalls

1. **Public S3 bucket without CloudFront** — exposes bucket listing, incurs higher costs, no SSL by default
2. **Missing SPA rewrites** — direct navigation to `/about` returns 403 (S3) or 404 (Firebase)
3. **Caching HTML with long TTLs** — users see outdated content after deployments
4. **Not invalidating CloudFront cache** — deploy new assets but old cached versions persist at edge locations
5. **Using S3 website endpoint with CloudFront** — use the REST API bucket endpoint instead for proper HTTPS
6. **Forgetting `firebase.json` rewrite rules** — SPA routing breaks on all non-root paths
7. **Deploying without `--delete` on S3** — old hashed files accumulate, increasing storage costs

---

## Accessibility Considerations

Cloud provider configuration does not affect Bootstrap's accessibility features directly. However, ensure custom error pages (404, 500) include accessible markup with navigation links and `aria-live` regions for error messages. Firebase and Azure allow custom error page configuration — use it.

SSL enforcement is an accessibility concern: some assistive technology software refuses to load resources from HTTP origins. Always redirect HTTP to HTTPS at the CDN level.

---

## Responsive Behavior

All three cloud platforms serve identical responsive CSS to every device. CDN edge servers do not perform device detection or content transformation. Bootstrap's client-side media queries handle all responsive behavior. Verify that CDN compression does not corrupt CSS media query syntax by testing the deployed site at Bootstrap's five breakpoint widths (576, 768, 992, 1200, 1400px).

For Firebase Hosting, configure `headers` for font files to include `Access-Control-Allow-Origin: *` to prevent CORS issues with responsive icon fonts loaded from subdomains.
