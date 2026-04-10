# 🚀 React Deployment Complete Guide

## Deploying React Applications to Production

---

## Table of Contents

1. [Build for Production](#build-for-production)
2. [Static Hosting](#static-hosting)
3. [Environment Variables](#environment-variables)
4. [Build Optimization](#build-optimization)
5. [Continuous Deployment](#continuous-deployment)
6. [Platform Deployment](#platform-deployment)
7. [CDN Configuration](#cdn-configuration)
8. [Performance Monitoring](#performance-monitoring)
9. [Security Headers](#security-headers)
10. [Real-World Examples](#real-world-examples)

---

## Build for Production

### Creating Production Build

```bash
# Using Create React App
npm run build

# Using Vite
npm run build
```

### Build Output

```
build/
├── index.html
├── static/
│   ├── css/
│   │   ├── main.abc123.css
│   │   └── main.abc123.css.map
│   └── js/
│       ├── main.abc123.js
│       ├── main.abc123.js.map
│       ├── runtime.abc123.js
│       └── runtime.abc123.js.map
├── assets/
│   ├── logo.abc123.svg
│   └── hero.abc123.jpg
└── manifest.json
```

### Build Configuration

```javascript
// vite.config.js
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  build: {
    outDir: 'dist',
    sourcemap: true,
    minify: 'terser',
    rollupOptions: {
      output: {
        manualChunks: {
          vendor: ['react', 'react-dom'],
          utils: ['lodash', 'moment']
        }
      }
    }
  }
});
```

---

## Static Hosting

### GitHub Pages

```bash
# Install gh-pages
npm install --save-dev gh-pages
```

```json
// package.json
{
  "scripts": {
    "predeploy": "npm run build",
    "deploy": "gh-pages -d build"
  }
}
```

```javascript
// vite.config.js
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  base: '/repository-name/'
});
```

```bash
# Deploy
npm run deploy
```

### Netlify

```bash
# Install Netlify CLI
npm install --save-dev netlify-cli
```

```toml
# netlify.toml
[build]
  command = "npm run build"
  publish = "build"

[[redirects]]
  from = "/*"
  to = "/index.html"
  status = 200
```

```bash
# Deploy
netlify deploy --prod
```

### Vercel

```bash
# Install Vercel CLI
npm install --save-dev vercel
```

```javascript
// vercel.json
{
  "buildCommand": "npm run build",
  "outputDirectory": "build",
  "framework": "create-react-app"
}
```

```bash
# Deploy
vercel --prod
```

---

## Environment Variables

### Using Environment Variables

```javascript
// .env
REACT_APP_API_URL=https://api.example.com
REACT_APP_GOOGLE_ANALYTICS=UA-XXXXX-X

// .env.development
REACT_APP_API_URL=http://localhost:3000

// .env.production
REACT_APP_API_URL=https://api.example.com
```

### Accessing Variables

```javascript
// Using runtime configuration
function App() {
  console.log(REACT_APP_API_URL);
  console.log(process.env.REACT_APP_API_URL);
  
  return <div>{process.env.REACT_APP_GOOGLE_ANALYTICS}</div>;
}
```

### Vercel Environment Variables

```bash
# Set via CLI
vercel env add production API_URL
vercel env add production GOOGLE_ANALYTICS

# Or via dashboard
# Settings → Environment Variables
```

### Netlify Environment Variables

```bash
# Set via CLI
netlify env:set API_URL https://api.example.com --production

# Or via dashboard
# Site settings → Environment variables
```

---

## Build Optimization

### Code Splitting

```javascript
// Lazy load routes
import { lazy, Suspense } from 'react';

const Dashboard = lazy(() => import('./pages/Dashboard'));
const Settings = lazy(() => import('./pages/Settings'));
const Analytics = lazy(() => import('./pages/Analytics'));

function App() {
  return (
    <Suspense fallback={<Loading />}>
      <Routes>
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/settings" element={<Settings />} />
        <Route path="/analytics" element={<Analytics />} />
      </Routes>
    </Suspense>
  );
}
```

### Tree Shaking

```javascript
// Only import what's needed
// ✅ Good
import { pick, omit } from 'lodash';

// ❌ Bad
import _ from 'lodash';
```

### Compression

```javascript
// vite.config.js
import { defineConfig } from 'vite';

export default defineConfig({
  build: {
    target: 'esnext',
    minify: 'esbuild',
    rollupOptions: {
      output: {
        manualChunks(id) {
          if (id.includes('node_modules')) {
            return 'vendor';
          }
        }
      }
    }
  }
});
```

---

## Continuous Deployment

### GitHub Actions

```yaml
# .github/workflows/deploy.yml
name: Deploy

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup Node.js
        uses: actions/setup-node@v3
        with:
          node-version: '18'
      
      - name: Install dependencies
        run: npm ci
      
      - name: Run tests
        run: npm test -- --passWithNoTests
      
      - name: Build
        run: npm run build
      
      - name: Deploy to Netlify
        uses: nwtgck/actions-netlify@v2.0
        with:
          publish-dir: './build'
          production-branch: 'main'
          github-token: ${{ secrets.GITHUB_TOKEN }}
          deploy-message: 'Deploy from GitHub Actions'
          enable-pull-request-comment: false
          enable-commit-comment: true
        env:
          NETLIFY_AUTH_TOKEN: ${{ secrets.NETLIFY_AUTH_TOKEN }}
          NETLIFY_SITE_ID: ${{ secrets.NETLIFY_SITE_ID }}
```

### CI/CD Pipeline

```yaml
# .github/workflows/ci.yml
name: CI

on:
  pull_request:
    branches: [main]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup Node.js
        uses: actions/setup-node@v3
        with:
          node-version: '18'
          cache: 'npm'
      
      - name: Install dependencies
        run: npm ci
      
      - name: Lint
        run: npm run lint
      
      - name: Test
        run: npm test -- --coverage
      
      - name: Build
        run: npm run build
```

---

## Platform Deployment

### AWS S3 + CloudFront

```bash
# Install AWS CLI
aws configure
```

```javascript
// s3-deploy.js
import S3 from 'aws-sdk/clients/s3';
import fs from 'fs';
import path from 'path';

const s3 = new S3({ region: 'us-east-1' });
const DIST_DIR = './build';

async function deploy() {
  const files = fs.readdirSync(DIST_DIR);
  
  for (const file of files) {
    const filePath = path.join(DIST_DIR, file);
    const fileContent = fs.readFileSync(filePath);
    
    await s3.putObject({
      Bucket: process.env.S3_BUCKET,
      Key: file,
      Body: fileContent,
      ContentType: getContentType(file)
    }).promise();
  }
  
  console.log('Deployed successfully!');
}

function getContentType(filename) {
  const ext = filename.split('.').pop();
  const types = {
    html: 'text/html',
    js: 'application/javascript',
    css: 'text/css',
    json: 'application/json'
  };
  return types[ext] || 'text/plain';
}

deploy();
```

### Firebase Hosting

```bash
# Install Firebase CLI
npm install -g firebase-tools
firebase init
```

```json
// firebase.json
{
  "hosting": {
    "public": "build",
    "ignore": ["firebase.json", "**/.*", "**/node_modules/**"],
    "rewrites": [
      {
        "source": "**",
        "destination": "/index.html"
      }
    ],
    "headers": [
      {
        "source": "**",
        "headers": [
          {
            "key": "Cache-Control",
            "value": "max-age=31536000"
          }
        ]
      }
    ]
  }
}
```

```bash
# Deploy
firebase deploy --only hosting
```

---

## CDN Configuration

### CloudFront Setup

```javascript
// cloudfront.js
import AWS from 'aws-sdk';

const cloudfront = new AWS.CloudFront({ apiVersion: '2020-05-31' });

async function invalidateCache(distributionId) {
  await cloudfront.createInvalidation({
    DistributionId: distributionId,
    InvalidationBatch: {
      CallerReference: `invalidation-${Date.now()}`,
      Paths: {
        Quantity: 1,
        Items: ['/*']
      }
    }
  }).promise();
  
  console.log('Cache invalidated!');
}
```

### Cache Headers

```javascript
// nginx.conf
location / {
  try_files $uri /index.html;
  
  # Cache static assets
  location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg|woff|woff2)$ {
    expires 1y;
    add_header Cache-Control "public, immutable";
  }
  
  # Don't cache HTML
  location ~* \.html$ {
    expires -1;
    add_header Cache-Control "no-store, no-cache, must-revalidate";
  }
}
```

---

## Performance Monitoring

### Google Analytics 4

```javascript
// App.jsx
import { useEffect } from 'react';

function App() {
  useEffect(() => {
    // Initialize GA4
    window.dataLayer = window.dataLayer || [];
    function gtag(){dataLayer.push(arguments);}
    gtag('js', new Date());
    gtag('config', 'G-XXXXXXX');
  }, []);
  
  return <Router>{routes}</Router>;
}
```

### Sentry Error Tracking

```bash
npm install @sentry/react @sentry/webpack-plugin
```

```javascript
// sentry.config.js
import * as Sentry from '@sentry/react';

Sentry.init({
  dsn: process.env.REACT_APP_SENTRY_DSN,
  environment: process.env.NODE_ENV,
  integrations: [
    new Sentry.BrowserTracing()
  ],
  tracesSampleRate: 0.1
});
```

```javascript
// index.js
import * as Sentry from '@sentry/react';

function App() {
  return (
    <Sentry.ErrorBoundary>
      <AppRoutes />
    </Sentry.ErrorBoundary>
  );
}
```

---

## Security Headers

### Header Configuration

```javascript
// helmet middleware for Express
import helmet from 'helmet';

app.use(helmet({
  contentSecurityPolicy: {
    directives: {
      defaultSrc: ["'self'"],
      scriptSrc: ["'self'", "'unsafe-inline'"],
      styleSrc: ["'self'", "'unsafe-inline'"],
      imgSrc: ["'self'", "data:", "https:"]
    }
  },
  crossOriginEmbedderPolicy: false
}));
```

### Netlify Headers

```toml
# netlify.toml
[[headers]]
  for = "/*"
  [headers.values]
    X-Frame-Options = "DENY"
    X-XSS-Protection = "1; mode=block"
    X-Content-Type-Options = "nosniff"
    Referrer-Policy = "strict-origin-when-cross-origin"
    Content-Security-Policy = "default-src 'self';"
```

### Vercel Headers

```javascript
// vercel.json
{
  "headers": [
    {
      "source": "/(.*)",
      "headers": [
        {
          "key": "X-Frame-Options",
          "value": "DENY"
        },
        {
          "key": "X-XSS-Protection",
          "value": "1; mode=block"
        }
      ]
    }
  ]
}
```

---

## Real-World Examples

### Complete Netlify Setup

```bash
# package.json
{
  "scripts": {
    "dev": "vite",
    "build": "vite build",
    "preview": "vite preview",
    "deploy": "netlify deploy --prod"
  }
}
```

```toml
# netlify.toml
[build]
  command = "npm run build"
  publish = "dist"

[build.environment]
  NODE_VERSION = "18"

[[redirects]]
  from = "/*"
  to = "/index.html"
  status = 200

[[headers]]
  for = "/*"
  [headers.values]
    X-Frame-Options = "DENY"
    X-XSS-Protection = "1; mode=block"
    Cache-Control = "public, max-age=31536000"
```

### Complete Vercel Setup

```javascript
// vercel.json
{
  "buildCommand": "npm run build",
  "outputDirectory": "dist",
  "framework": "create-react-app",
  "installCommand": "npm install",
  "routes": [
    {
      "src": "/(.*)",
      "dest": "/index.html"
    }
  ],
  "headers": [
    {
      "source": "/static/(.*)",
      "headers": [
        {
          "key": "Cache-Control",
          "value": "public, max-age=31536000, immutable"
        }
      ]
    }
  ]
}
```

---

## Summary

### Key Takeaways

1. **Build**: Always run production build
2. **Environment**: Use environment variables for secrets
3. **CDN**: Use CDN for static assets
4. **Caching**: Configure proper cache headers
5. **CI/CD**: Automate deployment
6. **Monitoring**: Set up error tracking

### Next Steps

- Continue with Vue Module: [01_VUE_FUNDAMENTALS.md](../VUE_MASTER/01_VUE_FUNDAMENTALS.md)
- Explore server-side rendering with Next.js
- Implement progressive enhancement

---

## Cross-References

- **Previous**: [08_REACT_TESTING_STRATEGIES.md](08_REACT_TESTING_STRATEGIES.md)
- **Next**: [01_VUE_FUNDAMENTALS.md](../VUE_MASTER/01_VUE_FUNDAMENTALS.md)

---

*Last updated: 2024*