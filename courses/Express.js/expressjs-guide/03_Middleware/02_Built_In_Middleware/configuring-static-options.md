# Configuring Static Options

## 📌 What You'll Learn
- How to configure the express.static() middleware with options
- Understanding the available options
- Practical examples of using different options

## 🧠 Concept Explained (Plain English)

The `express.static()` middleware can be customized with an options object to control how it serves static files. These options allow you to fine-tune behavior such as caching, file extensions, and how hidden files are handled.

Think of it like setting up a vending machine. You can decide what products it sells (which files to serve), how much they cost (caching headers), and whether it accepts certain types of payment (file extensions). The options give you control over the behavior to match your application's needs.

## 💻 Code Example

```javascript
// ES Module - Configuring express.static() Options

import express from 'express';
import path from 'path';

const app = express();

// ========================================
// EXAMPLE 1: BASIC USAGE (NO OPTIONS)
// ========================================
// Serves files from the "public" directory with default settings
app.use(express.static('public'));

// ========================================
// EXAMPLE 2: SETTING CACHE CONTROL
// ========================================
// maxAge controls how long browsers should cache the files
// Value can be a number (milliseconds) or a string parsable by ms
app.use(express.static('public', {
    maxAge: '1d', // Cache for 1 day
    // Alternatively: maxAge: 86400000 // 24 * 60 * 60 * 1000
}));

// ========================================
// EXAMPLE 3: SERVING WITH A VIRTUAL PATH PREFIX
// ========================================
// Serve files from "public" but make them available under "/static"
app.use('/static', express.static('public', {
    maxAge: '1d'
}));

// ========================================
// EXAMPLE 4: HANDLING DOTFILES (FILES STARTING WITH .)
// ========================================
// By default, dotfiles are ignored. You can change this behavior.
app.use(express.static('public', {
    dotfiles: 'allow', // Allow serving dotfiles (e.g., .htaccess)
    // Options: 'allow', 'deny', 'ignore'
}));

// ========================================
// EXAMPLE 5: ADDING FALLBACK EXTENSIONS
// ========================================
// If a file is not found, try adding these extensions
app.use(express.static('public', {
    extensions: ['html', 'htm'] // If /about is requested, try /about.html and /about.htm
}));

// ========================================
// EXAMPLE 6: DISABLING ETAGS
// ========================================
// ETags are used for cache validation. You can disable them if needed.
app.use(express.static('public', {
    etag: false // Disable ETag generation
}));

// ========================================
// EXAMPLE 7: CUSTOM SETTINGS FOR INDEX FILES
// ========================================
// By default, serves index.html when a directory is requested.
// You can change the filename or disable it.
app.use(express.static('public', {
    index: 'default.html' // Serve default.html instead of index.html for directories
    // Set to false to disable serving index files
}));

// ========================================
// EXAMPLE 8: REDIRECTING TRAILING SLASHES
// ========================================
// By default, expresses redirects to add a trailing slash when a directory is requested.
// You can disable this behavior.
app.use(express.static('public', {
    redirect: false // Do not redirect to add trailing slash
}));

// ========================================
// A ROUTE TO TEST SOME OF THESE
// ========================================
app.get('/', (req, res) => {
    res.send(`
        <!DOCTYPE html>
        <html>
        <head>
            <link rel="stylesheet" href="/static/css/style.css">
            <title>Static Options Example</title>
        </head>
        <body>
            <h1>Static File Options Demo</h1>
            <p>Check the Network tab in DevTools to see headers.</p>
            <img src="/static/images/logo.png" alt="Logo">
        </body>
        </html>
    `);
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## Available Options

| Option | Type | Description | Default |
|--------|------|-------------|---------|
| **dotfiles** | String | How to treat files with a leading dot. Options: 'allow', 'deny', 'ignore' | 'ignore' |
| **etag** | Boolean | Enable or disable etag generation | true |
| **extensions** | Array | File extension fallbacks | [] |
| **index** | Boolean/String | Serve index.html files, or set a different filename | 'index.html' |
| **maxAge** | Number/String | Browser cache max-age in milliseconds | 0 |
| **redirect** | Boolean | Redirect to trailing "/" when the pathname is a directory | true |

## 🔍 Line-by-Line Breakdown (Example 2)

| Line | Code | What It Does |
|------|------|--------------|
| 8 | `app.use(express.static('public', {` | Start of static middleware with options |
| 9 | `maxAge: '1d',` | Sets cache control to 1 day |
| 10 | `});` | End of options object |

## ⚠️ Common Mistakes

**1. Using maxAge incorrectly**
The maxAge value can be a string (like '1d') or a number of milliseconds. If using a string, it must be parsable by the ms module (which Express uses internally).

**2. Forgetting that options apply to all static middleware**
If you have multiple `app.use(express.static())` calls, each can have its own options.

**3. Not understanding the order of static middleware**
Express serves files from the first static middleware that has the file. Order matters when you have multiple static directories.

**4. Confusing dotfiles behavior**
'dotfiles: 'ignore'' means files like .env or .gitignore won't be served. 'allow' means they will be served if requested.

## ✅ Quick Recap

- Use an options object as the second argument to `express.static()`
- Common options: maxAge (caching), dotfiles (hidden files), extensions (fallbacks)
- Options allow fine-tuning of static file serving behavior
- Each static middleware can have its own options

## 🔗 What's Next

Let's learn about third-party middleware, starting with morgan for logging.
