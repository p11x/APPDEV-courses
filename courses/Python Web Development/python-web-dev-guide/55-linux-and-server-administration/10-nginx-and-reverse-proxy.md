# Nginx and Reverse Proxy

## What You'll Learn

- Nginx basics
- Reverse proxy configuration
- Load balancing
- SSL/TLS with Certbot

## Prerequisites

- Completed `09-securing-linux-server.md`

## Installing Nginx

```bash
sudo apt update
sudo apt install nginx
```

## Basic Configuration

### Nginx Configuration Structure

```
/etc/nginx/
├── nginx.conf          # Main config
├── sites-available/     # Site configs
│   └── default
└── sites-enabled/      # Enabled sites (symlinks)
```

### Basic Server Block

Create `/etc/nginx/sites-available/myapp`:

```nginx
server {
    listen 80;
    server_name example.com;

    location / {
        proxy_pass http://127.0.0.1:8000;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

Enable:

```bash
sudo ln -s /etc/nginx/sites-available/myapp /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl reload nginx
```

## SSL with Certbot

```bash
# Install
sudo apt install certbot python3-certbot-nginx

# Get certificate
sudo certbot --nginx -d example.com -d www.example.com

# Auto-renewal
sudo certbot renew --dry-run
```

## Summary

- Nginx handles HTTP, forwards to your app via proxy_pass
- Use Certbot for free SSL certificates
- Proxy headers pass client info to your app

## This Completes Folder 55

This folder covered Linux and server administration basics. Continue to folder 56 (Networking Fundamentals) for more networking depth.
