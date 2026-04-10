# Development Server Configuration

## OVERVIEW

Development server configuration enables efficient local development. This guide covers local servers, hot module replacement, and debugging tools.

## IMPLEMENTATION DETAILS

### Vite Dev Server

```javascript
// vite.config.js
import { defineConfig } from 'vite';

export default defineConfig({
  server: {
    port: 3000,
    open: true,
    hmr: true
  }
});
```

### Custom Dev Server

```javascript
// server.js
import { createServer } from 'http';
import { readFile, stat } from 'fs/promises';
import { extname, join } from 'path';

const mimeTypes = {
  '.html': 'text/html',
  '.js': 'application/javascript',
  '.css': 'text/css',
  '.json': 'application/json'
};

const server = createServer(async (req, res) => {
  let filePath = join(process.cwd(), 'public', req.url === '/' ? 'index.html' : req.url);
  
  try {
    await stat(filePath);
  } catch {
    filePath = join(process.cwd(), 'public', 'index.html');
  }
  
  const ext = extname(filePath);
  const content = await readFile(filePath);
  
  res.writeHead(200, { 'Content-Type': mimeTypes[ext] || 'text/plain' });
  res.end(content);
});

server.listen(8080, () => console.log('Server running on port 8080'));
```

### Import Maps for Development

```html
<script type="importmap">
{
  "imports": {
    "lit": "http://localhost:3000/node_modules/lit/index.js",
    "lit/": "http://localhost:3000/node_modules/lit/"
  }
}
</script>
```

## NEXT STEPS

Proceed to **12_Tooling/12_5_Deployment-Strategies**.