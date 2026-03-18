# Common Vulnerabilities

## What You'll Learn

- OWASP Top 10
- Common attacks
- Prevention methods

## Prerequisites

- Completed `08-secure-random.md`

## OWASP Top 10

The most critical web application security risks:

1. Broken Access Control
2. Cryptographic Failures
3. Injection
4. Insecure Design
5. Security Misconfiguration
6. Vulnerable Components
7. Authentication Failures
8. Data Integrity Failures
9. Logging Failures
10. SSRF

## SQL Injection

```python
# VULNERABLE
query = f"SELECT * FROM users WHERE id = {user_id}"

# SECURE - parameterized
query = "SELECT * FROM users WHERE id = ?"
cursor.execute(query, (user_id,))

# SECURE - ORM
user = session.query(User).filter(User.id == user_id).first()
```

## XSS (Cross-Site Scripting)

```python
# VULNERABLE - directly rendering user input
html = f"<h1>{user_input}</h1>"

# SECURE - escaping
import html
html = f"<h1>{html.escape(user_input)}</h1>"

# SECURE - template engine (Jinja2 auto-escapes)
from jinja2 import Template
template = Template("<h1>{{ user_input }}</h1>")
html = template.render(user_input=user_input)
```

## CSRF (Cross-Site Request Forgery)

```python
# Using Flask-WTF for CSRF protection
from flask_wtf import FlaskForm
from wtforms import StringField, SubmitField

class MyForm(FlaskForm):
    name = StringField('Name')
    submit = SubmitField('Submit')

# In template
<form method="POST">
    {{ form.hidden_tag() }}
    {{ form.name() }}
    {{ form.submit() }}
</form>
```

## Path Traversal

```python
import os

# VULNERABLE
filename = request.args.get("file")
content = open(f"/data/{filename}").read()

# SECURE
filename = request.args.get("file")
# Validate path
safe_path = os.path.join("/data", filename)
if not os.path.commonpath([safe_path]).startswith("/data"):
    raise ValueError("Invalid path")
content = open(safe_path).read()
```

## Command Injection

```python
import subprocess

# VULNERABLE
cmd = f"ls {user_input}"
os.system(cmd)

# SECURE
cmd = ["ls", user_input]  # List, not string
subprocess.run(cmd, shell=False)
```

## Summary

- Understand common vulnerabilities
- Always validate and sanitize input
- Use parameterized queries

## Next Steps

Continue to `10-cryptography-best-practices.md`.
