# 🔑 Password Hashing

## 🎯 What You'll Learn

- Why NOT to use md5/sha256 for passwords
- Using bcrypt
- Using argon2

---

## Why Not md5/sha256?

```python
# ❌ DON'T use these for passwords!
import hashlib

# Fast = bad for passwords!
hashlib.md5(b"password")      # Fast to compute = fast to crack!
hashlib.sha256(b"password")   # Same problem
hashlib.sha512(b"password")   # Same problem
```

---

## bcrypt

```bash
pip install bcrypt
```

```python
import bcrypt

# Hashing
password = b"super secret password"
salt = bcrypt.gensalt()
hashed = bcrypt.hashpw(password, salt)

# Verifying
if bcrypt.checkpw(password, hashed):
    print("Password matches!")

# With work factor (rounds)
hashed = bcrypt.hashpw(password, bcrypt.gensalt(rounds=12))
```

---

## argon2-cffi

```bash
pip install argon2-cffi
```

```python
from argon2 import PasswordHasher

ph = PasswordHasher()

# Hash
hash = ph.hash("password")

# Verify
if ph.verify(hash, "password"):
    print("Matches!")

# Check if needs rehash (upgrade)
if ph.check_needs_rehash(hash):
    # Rehash with new parameters
    new_hash = ph.hash("password")
```

---

## Comparison

| Algorithm | Deliberately Slow | Recommended |
|-----------|-------------------|-------------|
| md5/sha256 | No | ❌ |
| bcrypt | Yes | ✅ |
| argon2 | Yes | ✅ (best) |

---

## ✅ Summary

- Never use md5/sha256 for passwords
- Use bcrypt or argon2 (deliberately slow)
- argon2 is the modern winner

## 🔗 Further Reading

- [bcrypt Documentation](https://pypi.org/project/bcrypt/)
- [argon2-cffi Documentation](https://pypi.org/project/argon2-cffi/)
