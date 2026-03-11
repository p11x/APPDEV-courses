# Tailwind CSS Installation Guide

## Quick Start (CDN)

For learning and prototyping:

```html
<!DOCTYPE html>
<html>
<head>
  <script src="https://cdn.tailwindcss.com"></script>
</head>
<body>
  <h1 class="text-3xl font-bold">
    Hello World
  </h1>
</body>
</html>
```

## Installation with Node.js

### Step 1: Create Project

```bash
# Create project directory
mkdir my-project
cd my-project
npm init -y
```

### Step 2: Install Tailwind

```bash
# Install Tailwind CSS and dependencies
npm install -D tailwindcss postcss autoprefixer
```

### Step 3: Initialize Config

```bash
# Create tailwind.config.js
npx tailwindcss init -p
```

### Step 4: Configure

Edit `tailwind.config.js`:

```javascript
/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,js}"],
  theme: {
    extend: {},
  },
  plugins: [],
}
```

### Step 5: Add to CSS

Create `src/input.css`:

```css
@tailwind base;
@tailwind components;
@tailwind utilities;
```

### Step 6: Build

```bash
# Development (watch mode)
npx tailwindcss -i ./src/input.css -o ./dist/output.css --watch

# Production (minified)
npx tailwindcss -i ./src/input.css -o ./dist/output.css --minify
```

## With Vite

### Step 1: Create Vite Project

```bash
npm create vite@latest my-app -- --template vanilla
cd my-app
npm install
```

### Step 2: Install Tailwind

```bash
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p
```

### Step 3: Configure Vite

```javascript
// vite.config.js
export default {
  plugins: [
    require('tailwindcss'),
    require('autoprefixer'),
  ],
}
```

### Step 4: Add CSS

```css
/* src/style.css */
@tailwind base;
@tailwind components;
@tailwind utilities;
```

## With React (Vite)

### Step 1: Create Project

```bash
npm create vite@latest my-app -- --template react
cd my-app
npm install
```

### Step 2: Install Tailwind

```bash
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p
```

### Step 3: Configure

```javascript
// tailwind.config.js
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}
```

### Step 4: Add Directives

```css
/* src/index.css */
@tailwind base;
@tailwind components;
@tailwind utilities;
```

## VS Code Integration

### Install Extension

```bash
code --install-extension tailwindlabs.tailwind-css-intellisense
```

### Configuration

Add to settings.json:

```json
{
  "tailwindCSS.includeLanguages": {
    "html": "html",
    "javascript": "javascript"
  },
  "tailwindCSS.experimental.classRegex": [
    ["cva\\(([^)]*)\\)", "[\"'`]([^\"'`]*).*?[\"'`]"],
    ["cn\\(([^)]*)\\)", "[\"'`]([^\"'`]*).*?[\"'`]"]
  ]
}
```

## Troubleshooting

| Issue | Solution |
|-------|----------|
| Styles not loading | Check content path in config |
| Build errors | Ensure Node.js is up to date |
| Slow builds | Use JIT mode (default in v3+) |
| Purge issues | Check content array paths |

---

*Back to [Web Development README](../README.md)*
*Back to [Main README](../../README.md)*