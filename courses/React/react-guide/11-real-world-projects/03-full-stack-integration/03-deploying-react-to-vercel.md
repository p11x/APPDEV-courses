# Deploying React to Vercel

## Overview

Vercel is a cloud platform for static sites and serverless functions. It provides zero-config deployment for React applications with automatic SSL, preview deployments, and custom domains.

## Prerequisites

- Vercel account
- Git repository

## Core Concepts

### Vercel Configuration

```json
// File: vercel.json

{
  "rewrites": [
    { "source": "/(.*)", "destination": "/index.html" }
  ]
}
```

### Environment Variables

In Vercel Dashboard:
1. Go to Settings → Environment Variables
2. Add your variables:
   - VITE_API_URL=https://your-api.vercel.app
   - VITE_APP_URL=https://your-app.vercel.app

### Deployment Steps

1. **Connect Git Repository**
   - Import your project from GitHub/GitLab/Bitbucket
   - Vercel auto-detects the framework

2. **Configure Build Settings**
   - Build Command: `npm run build` or `vite build`
   - Output Directory: `dist`
   - Install Command: `npm install`

3. **Deploy**
   - Click "Deploy"
   - Vercel builds and deploys automatically

### Preview Deployments

Every PR gets a unique preview URL for testing before merging to main.

## Key Takeaways

- Connect Git for automatic deployments
- Configure environment variables in dashboard
- Use preview deployments for PRs
- Custom domains are free

## What's Next

This completes the React Web Application Development Guide. You now have a comprehensive understanding of React from fundamentals to deployment!