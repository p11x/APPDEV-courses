<!-- FILE: 18_rate_limiting_and_security/05_https_and_tls/02_ssl_certificates_with_lets_encrypt.md -->

## Overview

Set up free SSL/TLS certificates using Let's Encrypt for Flask applications.

## Prerequisites

- Domain name pointing to your server
- Shell access to your server

## Core Concepts

Let's Encrypt is a free, automated Certificate Authority. Certificates are valid for 90 days and can be auto-renewed.

## Getting Started

### Option 1: Certbot (Recommended)

Certbot is the official Let's Encrypt client.

```bash
# Install certbot (Ubuntu/Debian)
sudo apt-get update
sudo apt-get install certbot python3-certbot-nginx

# Install certbot (CentOS/RHEL)
sudo yum install certbot python3-certbot-nginx
```

### Option 2: Standalone Certificate

```bash
# Get certificate without nginx plugin
sudo certbot certonly --standalone -d yourdomain.com -d www.yourdomain.com

# Certificate saved to:
# /etc/letsencrypt/live/yourdomain.com/fullchain.pem
# /etc/letsencrypt/live/yourdomain.com/privkey.pem
```

## Manual Certificate Generation

```bash
# Install certbot
pip install certbot

# Generate certificate
sudo certbot certonly --webroot -w /var/www/html -d yourdomain.com -d www.yourdomain.com

# Certificate location:
# /etc/letsencrypt/live/yourdomain.com/fullchain.pem
# /etc/letsencrypt/live/yourdomain.com/privkey.pem
```

## Using Certificates with Flask

### Development with Flask-SSLify (for testing)

```bash
pip install flask-sslify
```

```python
# app.py
from flask import Flask
from flask_sslify import SSLify

app = Flask(__name__)
sslify = SSLify(app)

# Note: This requires a real SSL certificate in production
# For testing, use self-signed certificates
```

### Production with Gunicorn

```bash
# Run with SSL
gunicorn -w 4 --keyfile /etc/letsencrypt/live/yourdomain.com/privkey.pem \
              --certfile /etc/letsencrypt/live/yourdomain.com/fullchain.pem \
              app:app
```

### Production with Nginx (Recommended)

```nginx
# /etc/nginx/sites-available/default

server {
    listen 80;
    server_name yourdomain.com www.yourdomain.com;
    
    # Redirect HTTP to HTTPS
    location / {
        return 301 https://$server_name$request_uri;
    }
}

server {
    listen 443 ssl http2;
    server_name yourdomain.com www.yourdomain.com;
    
    ssl_certificate /etc/letsencrypt/live/yourdomain.com/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/yourdomain.com/privkey.pem;
    
    # SSL configuration
    ssl_protocols TLSv1.2 TLSv1.3;
    ssl_ciphers HIGH:!aNULL:!MD5;
    ssl_prefer_server_ciphers on;
    
    location / {
        proxy_pass http://localhost:8000;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

## Auto-Renewal

```bash
# Test renewal
sudo certbot renew --dry-run

# Set up cron job for auto-renewal
sudo crontab -e

# Add this line (runs twice daily)
0 0,12 * * * certbot renew --quiet --deploy-hook "systemctl reload nginx"
```

> **⚠️ Warning:** Always back up your certificates. If Let's Encrypt has issues, you'll need them to restore service.

## Common Mistakes

- ❌ Not renewing certificates before expiry
- ✅ Set up auto-renewal immediately

- ❌ Using self-signed certificates in production
- ✅ Use Let's Encrypt or paid certificates

- ❌ Not redirecting HTTP to HTTPS
- ✅ Always redirect HTTP to HTTPS

## Quick Reference

| Command | Purpose |
|---------|---------|
| `certbot certonly` | Get certificate |
| `certbot renew` | Renew certificate |
| `certbot delete` | Remove certificate |

## Next Steps

Continue to [03_forcing_https_in_flask.md](./03_forcing_https_in_flask.md) to learn how to enforce HTTPS in Flask.
