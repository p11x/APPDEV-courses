<!-- FILE: 18_rate_limiting_and_security/01_security_fundamentals/01_owasp_top_10_for_flask.md -->

## Overview

Understand the OWASP Top 10 security risks and how they apply to Flask applications.

## Prerequisites

- Basic Flask knowledge
- Understanding of web security concepts

## Core Concepts

The OWASP Top 10 is a standard awareness document for developers about web application security. It lists the most critical security risks. In Flask, we need to address each of these.

## The OWASP Top 10 (2021)

| Rank | Risk | Flask Mitigation |
|------|------|------------------|
| A01 | Broken Access Control | `@login_required`, role checks |
| A02 | Cryptographic Failures | HTTPS, secure cookies |
| A03 | Injection | Parameterized queries, ORM |
| A04 | Insecure Design | Security architecture |
| A05 | Security Misconfiguration | Debug=False, secure headers |
| A06 | Vulnerable Components | Keep packages updated |
| A07 | Auth Failures | Strong password hashing |
| A08 | Data Integrity Failures | CSRF tokens |
| A09 | Logging Failures | Proper error logging |
| A10 | SSRF | Validate external URLs |

## A01: Broken Access Control

**Attack scenario:** User accesses admin endpoints without authorization.

**Flask protection:**
```python
@app.route('/admin')
@login_required
def admin():
    if not current_user.is_admin:
        abort(403)
    return render_template('admin.html')
```

## A02: Cryptographic Failures

**Attack scenario:** Data intercepted in transit.

**Flask protection:**
```python
# Always use HTTPS in production
app.config['SESSION_COOKIE_SECURE'] = True
app.config['SESSION_COOKIE_HTTPONLY'] = True
app.config['SESSION_COOKIE_SAMESITE'] = 'Lax'
```

## A03: Injection

**Attack scenario:** SQL injection via user input.

**Flask protection (always use ORM):**
```python
# ❌ NEVER do this
query = f"SELECT * FROM users WHERE name = '{username}'"

# ✅ ALWAYS use parameterized queries via ORM
user = User.query.filter_by(name=username).first()
```

## A05: Security Misconfiguration

**Attack scenario:** Debug mode enabled in production exposes sensitive data.

**Flask protection:**
```python
# In production
app.config['DEBUG'] = False
app.config['TESTING'] = False
```

> **🔒 Security Note:** Never deploy with DEBUG=True. It allows remote code execution.

## Quick Reference

| OWASP Risk | Flask Solution |
|------------|----------------|
| Access Control | Decorators + role checks |
| Cryptography | HTTPS + secure cookies |
| Injection | ORM / parameterized queries |
| Security Config | Debug=False, minimal headers |
| Auth Failures | bcrypt, Flask-Login |

## Next Steps

Continue to [02_input_sanitization.md](./02_input_sanitization.md) to learn about sanitizing user input.
