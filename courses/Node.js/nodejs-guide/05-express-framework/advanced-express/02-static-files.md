# Serving Static Files

## What You'll Learn

- Serving static files with express.static
- Creating a public folder
- Custom static file options

## express.static()

Serve static files (HTML, CSS, JS, images):

```javascript
// Serve from 'public' folder
app.use(express.static('public'));

// Serve multiple folders
app.use(express.static('public'));
app.use(express.static('uploads'));
```

## Creating Static Files

### public/index.html

```html
<!DOCTYPE html>
<html>
<head>
  <title>My App</title>
</head>
<body>
  <h1>Welcome!</h1>
  <script src="/script.js"></script>
</body>
</html>
```

### server.js

```javascript
import express from 'express';
const app = express();

app.use(express.static('public'));

app.listen(3000);
```

## Virtual Path Prefix

```javascript
// Serve at /static prefix
app.use('/static', express.static('public'));
```

## Try It Yourself

### Exercise 1: Static Server
Create a static file server with HTML, CSS, and JS files.

### Exercise 2: Multiple Folders
Serve static files from multiple directories.
