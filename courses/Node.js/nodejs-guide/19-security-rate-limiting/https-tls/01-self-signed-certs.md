# Self-Signed Certificates

## What You'll Learn

- What TLS/HTTPS is and why it matters
- How to generate self-signed certificates with OpenSSL
- How to create an HTTPS server with Node.js
- How to handle the `cert` and `key` options in `https.createServer`
- When to use self-signed vs production certificates

## What Is TLS/HTTPS?

HTTP sends data in plaintext — anyone on the network can read it. **HTTPS** wraps HTTP in **TLS** (Transport Layer Security), which encrypts the connection:

```
HTTP:   Client ←[plaintext]→ Server     (anyone can read)
HTTPS:  Client ←[encrypted]→ Server     (only client and server can read)
```

TLS uses **certificates** to verify the server's identity. In production, these are issued by trusted Certificate Authorities (CAs). For development, you generate **self-signed certificates**.

## Generating Certificates

```bash
# Generate a private key (2048-bit RSA)
openssl genrsa -out server-key.pem 2048

# Generate a self-signed certificate (valid for 365 days)
openssl req -new -x509 -key server-key.pem -out server-cert.pem -days 365 \
  -subj "/CN=localhost"
```

The `-subj` flag sets the certificate fields without interactive prompts:
- `CN` = Common Name (should match your domain, e.g., `localhost`)

## HTTPS Server

```js
// https-server.js — HTTPS server with self-signed certificate

import { createServer } from 'node:https';
import { readFileSync } from 'node:fs';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));

// Read the certificate files
const options = {
  key: readFileSync(resolve(__dirname, 'server-key.pem')),   // Private key
  cert: readFileSync(resolve(__dirname, 'server-cert.pem')), // Certificate
};

// Create an HTTPS server — identical API to createServer from node:http
const server = createServer(options, (req, res) => {
  res.writeHead(200, { 'Content-Type': 'application/json' });
  res.end(JSON.stringify({
    message: 'Hello from HTTPS!',
    protocol: req.httpVersion,
    encrypted: req.socket.encrypted,  // true for HTTPS connections
  }));
});

server.listen(443, () => {
  console.log('HTTPS server on https://localhost:443');
  console.log('Note: Browsers will warn about the self-signed certificate');
});
```

## HTTP and HTTPS Side by Side

```js
// dual-server.js — Run both HTTP and HTTPS

import { createServer as createHttp } from 'node:http';
import { createServer as createHttps } from 'node:https';
import { readFileSync } from 'node:fs';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));

function handler(req, res) {
  res.writeHead(200, { 'Content-Type': 'text/plain' });
  res.end(`Hello! Protocol: ${req.socket.encrypted ? 'HTTPS' : 'HTTP'}`);
}

// HTTP server on port 80 — redirects to HTTPS
createHttp((req, res) => {
  res.writeHead(301, { Location: `https://localhost:443${req.url}` });
  res.end();
}).listen(80, () => {
  console.log('HTTP redirect on http://localhost:80');
});

// HTTPS server on port 443
createHttps({
  key: readFileSync(resolve(__dirname, 'server-key.pem')),
  cert: readFileSync(resolve(__dirname, 'server-cert.pem')),
}, handler).listen(443, () => {
  console.log('HTTPS server on https://localhost:443');
});
```

## Testing with curl

```bash
# With self-signed certs, curl rejects the connection
curl https://localhost:443
# curl: (60) SSL certificate problem: self signed certificate

# Use -k to skip certificate verification (development only!)
curl -k https://localhost:443
# {"message":"Hello from HTTPS!","protocol":"2","encrypted":true}
```

## How It Works

### Certificate Contents

A TLS certificate contains:
- **Public key** — used to encrypt data sent to the server
- **Identity** — domain name (CN=localhost)
- **Validity** — notBefore and notAfter dates
- **Signature** — proves the certificate was issued by a trusted authority

The private key (never shared) decrypts data encrypted with the public key.

### Why Self-Signed Is Not for Production

Browsers ship with a list of trusted CAs. Self-signed certificates are not on this list, so browsers show a security warning. For production, use Let's Encrypt (free) or a commercial CA.

## Common Mistakes

### Mistake 1: Committing Private Keys

```bash
# WRONG — private key in the repository
git add server-key.pem  # Anyone who clones the repo can impersonate your server

# CORRECT — add to .gitignore
echo "*.pem" >> .gitignore
```

### Mistake 2: Using HTTP in Production

```js
// WRONG — production API over HTTP
createHttp(handler).listen(3000);

// CORRECT — always use HTTPS in production
createHttps({ key, cert }, handler).listen(443);
// Or use a reverse proxy (nginx) that terminates TLS
```

### Mistake 3: Wrong CN (Common Name)

```bash
# WRONG — CN does not match the domain
openssl req -new -x509 -subj "/CN=myserver" ...
# Browser rejects: certificate is for "myserver" but you accessed "localhost"

# CORRECT — CN matches the domain you will access
openssl req -new -x509 -subj "/CN=localhost" ...
```

## Try It Yourself

### Exercise 1: Generate and Test

Generate a self-signed certificate. Create an HTTPS server. Access it with `curl -k` and verify the response.

### Exercise 2: HTTP Redirect

Run both HTTP (port 80) and HTTPS (port 443). Configure HTTP to redirect all requests to HTTPS.

### Exercise 3: Certificate Info

Use `openssl x509 -in server-cert.pem -text -noout` to inspect the certificate. Find the CN, validity dates, and public key algorithm.

## Next Steps

You can create HTTPS servers with self-signed certs. For production-ready certificates, continue to [Let's Encrypt](./02-lets-encrypt.md).
