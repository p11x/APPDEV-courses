<!-- FILE: 18_rate_limiting_and_security/05_https_and_tls/01_why_https_matters.md -->

## Overview

Understand why HTTPS is essential for Flask applications and web security.

## Prerequisites

- Basic understanding of HTTP
- Knowledge of web security concepts

## Core Concepts

HTTPS (HTTP Secure) encrypts all communication between the browser and server. Without it, attackers can intercept sensitive data.

## Why HTTPS Matters

### 1. Data Encryption

**Without HTTPS:**
```
# Attacker on same network can see:
# GET /login HTTP/1.1
# Host: example.com
# 
# username=john&password=secret123
```

**With HTTPS:**
```
# Encrypted - unreadable without key
# 8x7d9f8a7s9d8f7a9s8d7f9as8d7f9as8d7f9
```

### 2. Authentication

HTTPS verifies the server's identity using SSL certificates. Users can trust they're communicating with your real server, not an imposter.

### 3. Data Integrity

HTTPS ensures data isn't modified in transit. Without it, attackers can inject malware into downloaded files.

### 4. SEO Benefits

Google uses HTTPS as a ranking factor. Sites without HTTPS rank lower in search results.

### 5. Browser Warnings

Modern browsers show warnings for non-HTTPS sites:

- 🔴 "Not Secure" in address bar
- ⚠️ Warnings before entering passwords
- ❌ Some features disabled

## Attack Scenarios

### Man-in-the-Middle (MITM)

```
User <----> Attacker <----> Server
           Intercepts
           all traffic
```

### Session Hijacking

```
# Attacker steals session cookie
# Uses it to impersonate user
```

### Credential Theft

```
# Unencrypted login form
POST /login
username=admin&password=password123
```

## What HTTPS Provides

| Protection | Description |
|------------|-------------|
| Encryption | Data unreadable to attackers |
| Authentication | Verify server identity |
| Integrity | Data not modified in transit |
| SEO | Better search rankings |
| Trust | Users feel safe |

## Quick Reference

| Protocol | Secure? |
|----------|---------|
| HTTP | ❌ No |
| HTTPS | ✅ Yes (TLS/SSL) |
| WSS (WebSocket) | ✅ With TLS |

## Next Steps

Continue to [02_ssl_certificates_with_lets_encrypt.md](./02_ssl_certificates_with_lets_encrypt.md) to learn about SSL certificates.
