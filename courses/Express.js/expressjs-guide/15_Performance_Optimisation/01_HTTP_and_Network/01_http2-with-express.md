# HTTP/2 with Express

## 📌 What You'll Learn

- What HTTP/2 provides
- Using spdy/http2 module
- Multiplexing benefits

## 💻 Code Example

```js
import http2 from 'http2';
import fs from 'fs';
import express from 'express';

const app = express();

const server = http2.createSecureServer({
  key: fs.readFileSync('server-key.pem'),
  cert: fs.readFileSync('server-cert.pem')
}, app);

// HTTP/2 push for resources
app.get('/', (req, res) => {
  res.push('/styles.css', (push) => {
    push.writeHead(200, { 'Content-Type': 'text/css' });
    push.end('body { font: 14px sans-serif; }');
  });
  
  res.send('<html><head><link rel="stylesheet" href="/styles.css"></head><body>Hello HTTP/2</body></html>');
});

server.listen(3000);
```
