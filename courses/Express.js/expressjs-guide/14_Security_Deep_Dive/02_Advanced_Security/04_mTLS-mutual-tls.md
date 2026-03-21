# mTLS (Mutual TLS)

## 📌 What You'll Learn

- What mTLS is
- Configuring Node https with client cert verification

## 💻 Code Example

```js
import https from 'https';
import fs from 'fs';

const server = https.createServer({
  key: fs.readFileSync('server-key.pem'),
  cert: fs.readFileSync('server-cert.pem'),
  requestCert: true,
  ca: fs.readFileSync('ca-cert.pem'),
  rejectUnauthorized: true
}, (req, res) => {
  // Client certificate available in req.socket.getPeerCertificate()
  res.json({ 
    clientCert: req.socket.getPeerCertificate()?.subject,
    authenticated: true 
  });
});

server.listen(3443);
```
