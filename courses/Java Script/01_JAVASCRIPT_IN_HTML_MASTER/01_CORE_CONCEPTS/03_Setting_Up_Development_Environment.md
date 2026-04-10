# 🛠️ Setting Up Development Environment

## 📋 Overview

This guide will walk you through setting up a professional JavaScript development environment. A proper setup will make your coding experience efficient and enjoyable.

---

## 🖥️ Choosing a Code Editor

### Recommended: Visual Studio Code

**Why VS Code?**
- ✅ Free and open source
- ✅ Industry standard for web development
- ✅ Excellent JavaScript IntelliSense
- ✅ Vast extension ecosystem
- ✅ Integrated terminal
- ✅ Git integration built-in

### Installation Steps:

1. **Download** from [code.visualstudio.com](https://code.visualstudio.com/)
2. **Run** the installer
3. **Launch** VS Code

---

## 📦 Essential VS Code Extensions

### Install from Extensions Panel (Ctrl+Shift+X):

| Extension | Purpose | Search Term |
|-----------|---------|-------------|
| ESLint | Code linting | "ESLint" |
| Prettier | Code formatting | "Prettier" |
| Live Server | Local server | "Live Server" |
| JavaScript Snippets | Code shortcuts | "JavaScript Snippets" |
| Bracket Pair Colorizer | Color matching brackets | "Bracket Pair" |

### Recommended Settings:

```json
// File → Preferences → Settings → Open JSON

{
    "editor.fontSize": 14,
    "editor.tabSize": 2,
    "editor.formatOnSave": true,
    "editor.minimap.enabled": false,
    "files.autoSave": "afterDelay",
    "prettier.singleQuote": true,
    "prettier.semi": true,
    "prettier.tabWidth": 2,
    "eslint.autoFixOnSave": true,
    "liveServer.settings.CustomBrowser": "chrome"
}
```

---

## 🌐 Browser Developer Tools

### Chrome DevTools (Recommended)

**Access:** Press `F12` or `Ctrl+Shift+I`

### Key Panels:

```
┌─────────────────────────────────────────────┐
│  DevTools Tabs                              │
├─────────────────────────────────────────────┤
│  [Elements] [Console] [Network] [Sources]  │
│  [Performance] [Application] [Security]     │
├─────────────────────────────────────────────┤
│                                             │
│  Content Area                              │
│                                             │
└─────────────────────────────────────────────┘
```

### Essential Shortcuts:

| Windows/Linux | Mac | Action |
|---------------|-----|--------|
| F12 | F12 | Open DevTools |
| Ctrl+Shift+J | Cmd+Option+J | Open Console |
| Ctrl+Shift+C | Cmd+Option+C | Inspect Element |
| Ctrl+Shift+P | Cmd+Option+P | Command Menu |

---

## 🟢 Installing Node.js

### What is Node.js?

Node.js allows you to run JavaScript outside the browser. Essential for:
- Development servers
- Package management (npm)
- Build tools
- Server-side JavaScript

### Installation:

1. **Download** from [nodejs.org](https://nodejs.org/) (LTS version)
2. **Install** with default settings
3. **Verify** by opening terminal:

```bash
# Check versions
node --version   # Should show v18+ or v20+
npm --version    # Should show 9.x+
```

---

## 📂 Creating Your First Project

### Project Structure:

```
my-javascript-project/
├── index.html
├── css/
│   └── style.css
├── js/
│   └── app.js
└── README.md
```

### Step-by-Step:

```bash
# 1. Create project folder
mkdir my-javascript-project
cd my-javascript-project

# 2. Create subfolders
mkdir css js

# 3. Create files
# Windows:
type nul > index.html
type nul > README.md

# Or manually create in VS Code
```

### index.html:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>My JS Project</title>
    <link rel="stylesheet" href="css/style.css">
</head>
<body>
    <h1>Welcome to JavaScript!</h1>
    <div id="app"></div>
    
    <script src="js/app.js"></script>
</body>
</html>
```

### css/style.css:

```css
body {
    font-family: Arial, sans-serif;
    padding: 20px;
    background: #f0f0f0;
}

h1 { color: #333; }

#app {
    padding: 20px;
    background: white;
    border-radius: 8px;
    margin-top: 20px;
}
```

### js/app.js:

```javascript
// Your first JavaScript code!
console.log("JavaScript is working!");

// Select element
const app = document.getElementById('app');

// Modify content
app.textContent = "Hello from JavaScript!";

// Add interaction
document.querySelector('h1').addEventListener('click', () => {
    alert("You clicked the heading!");
});
```

---

## 🚀 Running Your Project

### Method 1: Live Server (Recommended)

1. Right-click `index.html` in VS Code
2. Select "Open with Live Server"
3. Browser opens with auto-refresh

### Method 2: Direct Browser

1. Double-click `index.html`
2. Open in Chrome/Firefox
3. Press `Ctrl+R` to refresh after changes

### Method 3: Node.js

```bash
# Run JavaScript file
node js/app.js

# Interactive REPL
node
> console.log("Hello!")
```

---

## 🔧 Environment Verification Tests

### Test 1: Browser Console

```javascript
// Open browser console and type:
console.log("Setup working!");
// Should see output in console
```

### Test 2: DOM Manipulation

```javascript
// In console:
document.body.innerHTML = "<h1>JavaScript Works!</h1>";
```

### Test 3: Node.js

```bash
# In terminal:
node -e "console.log('Node.js working!')"
```

### Test 4: npm

```bash
# Check npm
npm --version
```

---

## 📚 Recommended Tools Summary

| Category | Tool | Purpose |
|----------|------|---------|
| Editor | VS Code | Code writing |
| Browser | Chrome | Testing/debugging |
| Runtime | Node.js | Server-side JS |
| Terminal | VS Code Terminal | Command line |
| Version Control | Git | Code management |

---

## 🎯 Next Steps

- [ ] Complete environment setup
- [ ] Create first project
- [ ] Test all tools work
- [ ] Start learning JavaScript!

---

**Environment Ready!** Start coding! 🚀