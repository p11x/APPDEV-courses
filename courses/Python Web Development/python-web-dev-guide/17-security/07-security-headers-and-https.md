# Security Headers and HTTPS

## What You'll Learn
- Complete security headers setup
- HTTPS configuration
- SSL/TLS best practices
- Redirect HTTP to HTTPS

## Prerequisites
- Completed API security best practices

## Complete Security Headers

```python
from fastapi import FastAPI, Response
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI()

@app.middleware("http")
async def security_headers(response: Response):
    """Add comprehensive security headers"""
    
    # Prevent content type sniffing
    response.headers["X-Content-Type-Options"] = "nosniff"
    
    # Prevent clickjacking
    response.headers["X-Frame-Options"] = "DENY"
    
    # XSS protection
    response.headers["X-XSS-Protection"] = "1; mode=block"
    
    # Force HTTPS
    response.headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload"
    
    # Content Security Policy
    response.headers["Content-Security-Policy"] = (
        "default-src 'self'; "
        "script-src 'self' 'unsafe-inline'; "
        "style-src 'self' 'unsafe-inline'; "
        "img-src 'self' data: https:; "
        "connect-src 'self' https://api.example.com; "
        "frame-ancestors 'none';"
    )
    
    # Referrer policy
    response.headers["Referrer-Policy"] = "strict-origin-when-cross-origin"
    
    # Permissions policy
    response.headers["Permissions-Policy"] = (
        "geolocation=(), "
        "microphone=(), "
        "camera=()"
    )
    
    return response
```

## HTTPS with Uvicorn

```python
import uvicorn

# Run with HTTPS
uvicorn.run(
    "main:app",
    host="0.0.0.0",
    port=443,
    ssl_keyfile="/etc/letsencrypt/live/example.com/privkey.pem",
    ssl_certfile="/etc/letsencrypt/live/example.com/fullchain.pem",
    # TLS 1.3 only
    ssl_version="TLSv1.3",
)

# HTTP to HTTPS redirect
from fastapi import FastAPI, Response
import redirects

@app.get("/{full_path:path}")
async def redirect_to_https(full_path: str):
    return Response(
        status_code=301,
        headers={"Location": f"https://example.com/{full_path}"}
    )
```

## SSL Certificate with Let's Encrypt

```bash
# Install Certbot
sudo apt install certbot python3-certbot-nginx

# Get certificate
sudo certbot certonly --webroot -w /var/www/html -d example.com

# Auto-renewal
sudo certbot renew --dry-run
```

## TLS Configuration

```python
# production.py
import ssl

ssl_context = ssl.SSLContext(ssl.PROTOCOL_TLS_SERVER)

# Load certificate
ssl_context.load_cert_chain(
    certfile="/path/to/cert.pem",
    keyfile="/path/to/key.pem"
)

# Minimum TLS version
ssl_context.minimum_version = ssl.TLSVersion.TLSv1_3

# Enable OCSP stapling
ssl_context.ssl_context_set_ocsp_staple(True)

# Run server
uvicorn.run("main:app", ssl=ssl_context)
```

## Summary
- Implement all security headers
- Use HTTPS exclusively in production
- Use TLS 1.3 minimum
- Redirect HTTP to HTTPS
- Use Let's Encrypt for free certificates

## Next Steps
→ Move to `18-api-design/`
