# Secure Password Storage

## What You'll Learn

- Why use bcrypt
- Hashing passwords properly
- Using passlib

## Prerequisites

- Completed `01-cryptography-basics.md`

## Why Bcrypt

Bcrypt is designed for password hashing with:
- Built-in salt
- Configurable work factor
- Slow by design (resistant to brute force)

## Installing Bcrypt

```bash
pip install bcrypt
```

## Basic Usage

```python
import bcrypt

# Hashing password
password = "securePassword123"
salt = bcrypt.gensalt()
hashed = bcrypt.hashpw(password.encode(), salt)
print(f"Hashed: {hashed}")

# Verifying password
password_to_check = "securePassword123"
result = bcrypt.checkpw(password_to_check.encode(), hashed)
print(f"Match: {result}")  # True
```

## Using in Application

```python
import bcrypt
from dataclasses import dataclass

@dataclass
class PasswordService:
    work_factor: int = 12
    
    def hash_password(self, password: str) -> str:
        """Hash a password."""
        salt = bcrypt.gensalt(rounds=self.work_factor)
        return bcrypt.hashpw(password.encode(), salt).decode()
    
    def verify_password(self, password: str, hashed: str) -> bool:
        """Verify password against hash."""
        return bcrypt.checkpw(password.encode(), hashed.encode())

# Usage
service = PasswordService()
hashed = service.hash_password("mySecurePassword")
print(service.verify_password("mySecurePassword", hashed))  # True
print(service.verify_password("wrongPassword", hashed))  # False
```

## Using Passlib

```python
from passlib.context import CryptContext

# Configure password hashing
pwd_context = CryptContext(
    schemes=["bcrypt"],
    deprecated="auto"
)

# Hash password
def hash_password(password: str) -> str:
    return pwd_context.hash(password)

# Verify password
def verify_password(plain_password: str, hashed_password: str) -> bool:
    return pwd_context.verify(plain_password, hashed_password)

# Usage
hashed = hash_password("securePassword")
print(verify_password("securePassword", hashed))  # True
```

## Integration with Flask

```python
from flask import Flask, request, jsonify
from passlib.context import CryptContext

app = Flask(__name__)
pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")

# In-memory storage (use database in production)
users_db = {}

@app.route("/register", methods=["POST"])
def register():
    data = request.json
    email = data.get("email")
    password = data.get("password")
    
    if email in users_db:
        return jsonify({"error": "User exists"}), 400
    
    # Hash password before storing
    users_db[email] = {
        "email": email,
        "password": pwd_context.hash(password)
    }
    
    return jsonify({"message": "User created"}), 201

@app.route("/login", methods=["POST"])
def login():
    data = request.json
    email = data.get("email")
    password = data.get("password")
    
    user = users_db.get(email)
    if not user:
        return jsonify({"error": "Invalid credentials"}), 401
    
    if not pwd_context.verify(password, user["password"]):
        return jsonify({"error": "Invalid credentials"}), 401
    
    return jsonify({"message": "Login successful"})
```

## Best Practices

- Never store plain text passwords
- Use unique salt per password (bcrypt does this)
- Use appropriate work factor (12+)
- Use HTTPS in production

## Summary

- Use bcrypt for password hashing
- Never store plain text passwords
- Use passlib for flexibility

## Next Steps

Continue to `03-tls-ssl-https.md`.
