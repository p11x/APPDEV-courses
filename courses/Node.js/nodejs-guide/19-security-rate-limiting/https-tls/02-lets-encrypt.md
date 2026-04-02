# Let's Encrypt

## What You'll Learn

- What Let's Encrypt is and how it provides free TLS certificates
- How to use certbot to obtain and renew certificates
- How to set up auto-renewal with a cron job
- How to use nginx as a reverse proxy with TLS termination
- How the ACME challenge verifies domain ownership

## What Is Let's Encrypt?

Let's Encrypt is a free, automated Certificate Authority. It issues TLS certificates that all browsers trust — no security warnings. Certificates are valid for 90 days and can be auto-renewed.

```
Self-signed: You create → Browsers warn → Only for development
Let's Encrypt: CA issues → Browsers trust → Production-ready
```

## Installing Certbot

```bash
# Ubuntu/Debian
sudo apt update
sudo apt install certbot

# macOS
brew install certbot
```

## Obtaining a Certificate

### Standalone Mode (no web server needed)

```bash
# Certbot starts a temporary web server on port 80
# Let's Encrypt connects to it to verify you own the domain
sudo certbot certonly --standalone -d yourdomain.com -d www.yourdomain.com
```

### Webroot Mode (if a web server is already running)

```bash
# Certbot places a file in your web root; Let's Encrypt fetches it
sudo certbot certonly --webroot -w /var/www/html -d yourdomain.com
```

### What Happens

1. Certbot generates a private key
2. Certbot creates a challenge file (a random token)
3. Let's Encrypt's servers fetch `http://yourdomain.com/.well-known/acme-challenge/<token>`
4. If the token matches, Let's Encrypt issues the certificate
5. Certificate files are saved to `/etc/letsencrypt/live/yourdomain.com/`

### Certificate Files

```
/etc/letsencrypt/live/yourdomain.com/
├── fullchain.pem   ← Certificate + intermediate chain (give this to your server)
├── privkey.pem     ← Private key (keep secret)
├── cert.pem        ← Certificate only
└── chain.pem       ← Intermediate certificates only
```

## Using with Node.js

```js
// letsencrypt-server.js — HTTPS server with Let's Encrypt certificate

import { createServer } from 'node:https';
import { readFileSync } from 'node:fs';

const domain = 'yourdomain.com';

const server = createServer({
  key: readFileSync(`/etc/letsencrypt/live/${domain}/privkey.pem`),
  cert: readFileSync(`/etc/letsencrypt/live/${domain}/fullchain.pem`),
}, (req, res) => {
  res.writeHead(200, { 'Content-Type': 'text/plain' });
  res.end('Hello from a trusted HTTPS server!');
});

server.listen(443, () => {
  console.log(`HTTPS server on https://${domain}`);
});
```

## Nginx Reverse Proxy (Recommended)

Instead of Node.js handling TLS directly, use **nginx** as a reverse proxy. Nginx terminates TLS and forwards plain HTTP to Node.js:

```
Client ←──HTTPS──→ Nginx (port 443) ←──HTTP──→ Node.js (port 3000)
```

### Nginx Configuration

```nginx
# /etc/nginx/sites-available/yourdomain.com

server {
    listen 80;
    server_name yourdomain.com www.yourdomain.com;

    # Let's Encrypt ACME challenge — certbot needs this to renew
    location /.well-known/acme-challenge/ {
        root /var/www/html;
    }

    # Redirect all HTTP to HTTPS
    location / {
        return 301 https://$host$request_uri;
    }
}

server {
    listen 443 ssl http2;
    server_name yourdomain.com www.yourdomain.com;

    # Let's Encrypt certificate paths
    ssl_certificate     /etc/letsencrypt/live/yourdomain.com/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/yourdomain.com/privkey.pem;

    # Modern TLS settings
    ssl_protocols TLSv1.2 TLSv1.3;
    ssl_ciphers ECDHE-ECDSA-AES128-GCM-SHA256:ECDHE-RSA-AES128-GCM-SHA256;
    ssl_prefer_server_ciphers off;

    # Proxy to Node.js
    location / {
        proxy_pass http://127.0.0.1:3000;  # Forward to Node.js
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

```bash
# Enable the site and reload nginx
sudo ln -s /etc/nginx/sites-available/yourdomain.com /etc/nginx/sites-enabled/
sudo nginx -t    # Test configuration
sudo systemctl reload nginx
```

## Auto-Renewal

```bash
# Test renewal (dry run)
sudo certbot renew --dry-run

# Add a cron job to renew twice daily (certbot only renews when < 30 days remain)
sudo crontab -e
# Add this line:
0 0,12 * * * certbot renew --quiet --deploy-hook "systemctl reload nginx"
```

The `--deploy-hook` runs only when a certificate is actually renewed — it reloads nginx to pick up the new certificate.

## How It Works

### The ACME Protocol

```
Your Server                  Let's Encrypt
    │                              │
    │── Request certificate ──────→│
    │←── Challenge token ──────────│
    │                              │
    │── Serve token at             │
    │   /.well-known/acme-challenge│
    │                              │
    │←── Verify & issue cert ──────│
    │                              │
    │── Certificate installed ────→│
```

### Why Nginx for TLS?

| Approach | Pros | Cons |
|----------|------|------|
| Node.js handles TLS | Simple setup | Node.js must run as root (port 443) |
| Nginx terminates TLS | Node.js runs unprivileged (port 3000), battle-tested TLS, serves static files | Extra service to manage |

## Common Mistakes

### Mistake 1: Forgetting the ACME Challenge Location

```nginx
# WRONG — certbot cannot renew because there is no /.well-known location
server {
    listen 80;
    return 301 https://$host$request_uri;  # Redirects everything — ACME fails
}

# CORRECT — allow ACME challenges on HTTP
location /.well-known/acme-challenge/ {
    root /var/www/html;
}
```

### Mistake 2: Not Reloading Nginx After Renewal

```bash
# WRONG — certificate renewed but nginx still uses the old one
0 0,12 * * * certbot renew --quiet

# CORRECT — reload nginx after renewal
0 0,12 * * * certbot renew --quiet --deploy-hook "systemctl reload nginx"
```

### Mistake 3: Certificates for IP Addresses

Let's Encrypt does not issue certificates for IP addresses — only for domain names. You need a domain name with DNS pointing to your server.

## Try It Yourself

### Exercise 1: Generate a Certificate (Local)

Use `certbot certonly --standalone` on a VPS or a domain you own. Inspect the certificate files.

### Exercise 2: Nginx Proxy

Configure nginx to proxy requests to a Node.js app on port 3000. Test with `curl -v https://yourdomain.com`.

### Exercise 3: Renewal Test

Run `sudo certbot renew --dry-run` to verify your renewal setup works.

## Next Steps

You understand HTTPS setup. For CLI tool development, continue to [Chapter 20: CLI Tools](../../20-cli-tools/cli-basics/01-commander-setup.md).
