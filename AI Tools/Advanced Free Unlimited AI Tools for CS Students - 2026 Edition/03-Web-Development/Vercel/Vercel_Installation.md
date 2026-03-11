# Vercel Installation Guide

## Method 1: CLI Installation

```bash
# Install globally via npm
npm i -g vercel

# Verify installation
vercel --version
```

## Method 2: GitHub Integration (Recommended)

1. **Create Account**
   - Visit: https://vercel.com
   - Click "Sign Up"
   - Choose GitHub authentication

2. **Connect Repository**
   - Click "Add New..." → Project
   - Select GitHub repository
   - Configure settings:
     - Framework Preset: Auto-detect
     - Build Command: (auto-detect)
     - Output Directory: (auto-detect)

3. **Deploy**
   - Click "Deploy"
   - Wait for build completion

## Method 3: Desktop App

1. Download from: https://vercel.com/download
2. Install on Windows/macOS/Linux
3. Sign in with GitHub

## Project Configuration

### vercel.json (Optional)

```json
{
  "rewrites": [
    { "source": "/(.*)", "destination": "/index.html" }
  ],
  "headers": [
    {
      "source": "/(.*)",
      "headers": [
        { "key": "X-Frame-Options", "value": "DENY" }
      ]
    }
  ]
}
```

---

*Back to [Web Development README](../README.md)*
*Back to [Main README../../README.md)*