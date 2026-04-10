# 🛠️ JavaScript Development Setup Guide

## 📋 Prerequisites Checklist

Before we begin, make sure you have the following installed:

| Tool | Status | Installation |
|------|--------|--------------|
| Web Browser (Chrome/Edge/Firefox) | ☐ | [Download Chrome](https://www.google.com/chrome/) |
| Code Editor (VS Code) | ☐ | [Download VS Code](https://code.visualstudio.com/) |
| Node.js (LTS version) | ☐ | [Download Node.js](https://nodejs.org/) |
| Git | ☐ | [Download Git](https://git-scm.com/) |

---

## 🖥️ Step 1: Install Visual Studio Code

### Why VS Code?
- ✅ Free and open source
- ✅ Built-in JavaScript support
- ✅ Large extension ecosystem
- ✅ Integrated terminal
- ✅ Git integration

### Installation Steps

1. **Download VS Code** from [code.visualstudio.com](https://code.visualstudio.com/)

2. **Run the installer** and follow the prompts

3. **Recommended Settings** (File → Preferences → Settings):

```json
{
    "editor.fontSize": 14,
    "editor.tabSize": 2,
    "editor.formatOnSave": true,
    "files.autoSave": "afterDelay",
    "emmet.includeLanguages": {
        "javascript": "javascriptreact",
        "typescript": "typescriptreact"
    }
}
```

---

## 📦 Step 2: Essential VS Code Extensions

### Install These Extensions:

1. **ESLint** - Code linting
   - Publisher: Microsoft
   - Install: Search "ESLint" in Extensions

2. **Prettier** - Code formatting
   - Publisher: Prettier
   - Install: Search "Prettier" in Extensions

3. **Live Server** - Local development server
   - Publisher: Ritwick Dey
   - Install: Search "Live Server" in Extensions

4. **JavaScript (ES6) code snippets** - Code snippets
   - Publisher: charalampos karypidis
   - Install: Search "ES6" in Extensions

5. **Bracket Pair Colorizer** - Matching brackets
   - Publisher: CoenraadS
   - Install: Search "Bracket Pair" in Extensions

### Extension Settings (settings.json):

```json
{
    "editor.formatOnSave": true,
    "prettier.singleQuote": true,
    "prettier.tabWidth": 2,
    "eslint.autoFixOnSave": true,
    "eslint.validate": [
        "javascript",
        "javascriptreact",
        "typescript",
        "typescriptreact"
    ]
}
```

---

## 🟢 Step 3: Install Node.js

### What is Node.js?
Node.js allows you to run JavaScript outside the browser. It's essential for:
- Running development servers
- Using package managers (npm/yarn)
- Running build tools
- Server-side JavaScript

### Installation Steps:

1. **Download Node.js LTS** from [nodejs.org](https://nodejs.org/)

2. **Run the installer** with default settings

3. **Verify installation** - Open terminal/command prompt:
```bash
node --version
# Should output: v18.x.x or higher

npm --version
# Should output: 9.x.x or higher
```

### Configure npm (optional):
```bash
npm config set init-author-name "Your Name"
npm config set init-license "MIT"
```

---

## 🔧 Step 4: Browser Developer Tools

### Chrome DevTools Setup:

1. **Open DevTools:**
   - Press `F12` or `Ctrl+Shift+I` (Windows/Linux)
   - Press `Cmd+Option+I` (Mac)

2. **Important Panels:**
   - **Console** - View logs and errors
   - **Elements** - Inspect HTML/CSS
   - **Network** - Monitor network requests
   - **Sources** - Debug JavaScript
   - **Application** - View storage/localStorage

### DevTools Keyboard Shortcuts:

| Action | Windows/Linux | Mac |
|--------|---------------|-----|
| Open DevTools | F12 | F12 |
| Console | Ctrl+Shift+J | Cmd+Option+J |
| Elements | Ctrl+Shift+C | Cmd+Shift+C |
| Search in Files | Ctrl+Shift+F | Cmd+Shift+F |

---

## 📂 Step 5: Create Your First Project

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

### Create Project (Terminal):

```bash
# Create folder
mkdir my-javascript-project
cd my-javascript-project

# Create subfolders
mkdir css js

# Create files (Windows)
echo. > index.html
echo. > css\style.css
echo. > js\app.js
echo. > README.md
```

### index.html:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>My JavaScript Project</title>
    <link rel="stylesheet" href="css/style.css">
</head>
<body>
    <h1>Hello, JavaScript!</h1>
    <div id="app"></div>
    
    <script src="js/app.js"></script>
</body>
</html>
```

### js/app.js:

```javascript
// Your first JavaScript code!
console.log("Hello, World!");

// Select element
const app = document.getElementById('app');

// Modify content
app.textContent = "Welcome to JavaScript!";

// Add event listener
document.querySelector('h1').addEventListener('click', () => {
    alert('You clicked the heading!');
});
```

### css/style.css:

```css
body {
    font-family: Arial, sans-serif;
    padding: 20px;
    background-color: #f5f5f5;
}

h1 {
    color: #333;
}

#app {
    padding: 20px;
    background: white;
    border-radius: 8px;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}
```

---

## 🔍 Step 6: Running Your Project

### Option 1: Live Server (Recommended)

1. Open project in VS Code
2. Right-click `index.html` → "Open with Live Server"
3. Browser will open automatically
4. Any changes to HTML/CSS/JS will auto-refresh

### Option 2: Direct Browser

1. Double-click `index.html` to open in browser
2. For JS changes, press `Ctrl+R` to refresh

### Option 3: Node.js

```bash
# Run JavaScript in Node
node js/app.js

# Or use Node REPL
node
> console.log("Hello!")
```

---

## 🧪 Step 7: Verify Your Setup

### Test 1: Console Test

```javascript
// In browser console
console.log("Setup working!");

// Result: Should see "Setup working!" in console
```

### Test 2: DOM Test

```javascript
// In browser console
document.body.innerHTML = "<h1>JavaScript is working!</h1>";
```

### Test 3: Node.js Test

```bash
# In terminal
node -e "console.log('Node.js working!')"
```

---

## 📚 Recommended Learning Resources

### Official Documentation:
- [MDN JavaScript Guide](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide)
- [JavaScript.info](https://javascript.info/)
- [ECMAScript Specification](https://tc39.es/ecma262/)

### Video Tutorials:
- JavaScript Crash Course (Traversy Media)
- Modern JavaScript (Net Ninja)

### Interactive Platforms:
- [freeCodeCamp](https://www.freecodecamp.org/)
- [Codecademy JavaScript](https://www.codecademy.com/learn/learn-javascript)
- [CodePen](https://codepen.io/)

---

## 🔧 Troubleshooting Common Issues

### Issue: "node is not recognized"
**Solution:** Restart terminal or add Node.js to PATH

### Issue: Live Server not working
**Solution:** Check if port 5500 is available, try different port

### Issue: ESLint not working
**Solution:** Run `npm init -y` in project folder to create package.json

### Issue: JavaScript not running
**Solution:** Check browser console for errors, ensure script src is correct

---

## 🎯 Next Steps

Now that your environment is set up:

1. ☐ Start with Module 01: Core Concepts
2. ☐ Complete all exercises in each module
3. ☐ Build the projects as you learn
4. ☐ Join a coding community
5. ☐ Practice daily!

---

**Setup Complete!** You're ready to start your JavaScript journey! 🎉