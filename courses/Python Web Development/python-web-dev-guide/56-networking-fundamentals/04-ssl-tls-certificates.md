# SSL/TLS Certificates

## What You'll Learn

- SSL/TLS basics
- Certificate types
- Let's Encrypt
- Certificate management

## Prerequisites

- Completed `03-dns-and-domains.md`

## SSL/TLS

SSL (Secure Sockets Layer) and TLS (Transport Layer Security) provide encryption for web traffic.

## Certificate Types

- DV (Domain Validation) — Basic verification
- OV (Organization Validation) — Organization verified
- EV (Extended Validation) — Highest trust

## Let's Encrypt

Free, automated certificates:

```bash
# Install Certbot
sudo apt install certbot python3-certbot-nginx

# Get certificate
sudo certbot --nginx -d example.com
```

## Summary

- TLS encrypts web traffic
- Let's Encrypt provides free certificates

## Next Steps

Continue to other networking topics.
