# 🔐 Cryptography Basics

## 🎯 What You'll Learn

- Symmetric encryption with Fernet
- Asymmetric encryption basics
- Hashing for integrity

---

## Installation

```bash
pip install cryptography
```

---

## Symmetric Encryption (Fernet)

```python
from cryptography.fernet import Fernet

# Generate key
key = Fernet.generate_key()
cipher = Fernet(key)

# Encrypt
encrypted = cipher.encrypt(b"secret message")

# Decrypt
decrypted = cipher.decrypt(encrypted)
print(decrypted)  # b'secret message'
```

---

## Asymmetric Encryption

```python
from cryptography.hazmat.primitives.asymmetric import rsa
from cryptography.hazmat.primitives import serialization
from cryptography.hazmat.backends import default_backend

# Generate key pair
private_key = rsa.generate_private_key(
    public_exponent=65537,
    key_size=2048,
)
public_key = private_key.public_key()

# Encrypt with public key
encrypted = public_key.encrypt(
    b"secret message",
    padding.PKCS1v15()
)

# Decrypt with private key
decrypted = private_key.decrypt(
    encrypted,
    padding.PKCS1v15()
)
```

---

## Hashing

```python
import hashlib

# SHA-256
data = b"message"
hash_value = hashlib.sha256(data).hexdigest()

# HMAC (keyed hash)
import hmac
hmac_value = hmac.new(b"key", b"message", hashlib.sha256).hexdigest()
```

---

## TLS/HTTPS

```python
import httpx

# Verify is on by default!
response = httpx.get("https://api.example.com")

# DON'T do this in production!
# response = httpx.get("https://api.example.com", verify=False)
```

---

## ✅ Summary

- Fernet: easiest symmetric encryption
- Use TLS for network communication
- Use hashlib for data integrity

## 🔗 Further Reading

- [cryptography Documentation](https://cryptography.io/)
