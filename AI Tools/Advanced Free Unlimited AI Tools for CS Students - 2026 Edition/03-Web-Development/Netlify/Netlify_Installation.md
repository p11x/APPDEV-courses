# Netlify Installation Guide

## Method 1: CLI Installation

```bash
# Install globally via npm
npm install netlify-cli -g

# Verify installation
netlify --version
```

## Method 2: GitHub Integration

1. **Create Account**
   - Visit: https://app.netlify.com
   - Click "Sign up"
   - Choose GitHub authentication

2. **Connect Repository**
   - Click "Add new site" → Import existing project
   - Select GitHub repository
   - Configure:
     - Build command: (auto-detect)
     - Publish directory: (auto-detect)

3. **Deploy**
   - Click "Deploy site"

## Method 3: Drag and Drop

1. Build your project
2. Visit: https://app.netlify.com/drop
3. Drag your `dist` or `build` folder
4. Done!

## netlify.toml Configuration

```toml
[build]
  command = "npm run build"
  publish = "dist"

[[redirects]]
  from = "/*"
  to = "/index.html"
  status = 200
```

---

*Back to [Web Development README](../README.md)*
*Back to [Main README../../README.md)*