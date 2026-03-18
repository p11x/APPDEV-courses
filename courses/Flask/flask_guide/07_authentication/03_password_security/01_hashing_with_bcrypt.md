<!-- FILE: 07_authentication/03_password_security/01_hashing_with_bcrypt.md -->

## Overview

**Never store plain-text passwords**. This file covers password hashing using Werkzeug's security utilities, which Flask-SQLAlchemy uses internally.

## Core Concepts

### Password Hashing

Hashing transforms passwords into irreversible strings:
- `generate_password_hash(password)` — Hash a password
- `check_password_hash(hash, password)` — Verify password

## Code Walkthrough

### Using Werkzeug Security

```python
# models.py — Password hashing
from werkzeug.security import generate_password_hash, check_password_hash

class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    password_hash = db.Column(db.String(200))
    
    def set_password(self, password):
        """Hash password before saving."""
        self.password_hash = generate_password_hash(password)
    
    def check_password(self, password):
        """Verify password against hash."""
        return check_password_hash(self.password_hash, password)
```

```python
# Using in registration
user = User(username="alice")
user.set_password("secret123")
db.session.add(user)
db.session.commit()

# Using in login
user = User.query.filter_by(username="alice").first()
if user and user.check_password("secret123"):
    # Login success
```

## Next Steps

Now you can secure passwords. Continue to [02_registration_flow.md](02_registration_flow.md) to create registration flow.