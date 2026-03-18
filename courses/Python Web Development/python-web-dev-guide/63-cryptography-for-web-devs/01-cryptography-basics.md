# Cryptography Basics

## What You'll Learn

- Understanding encryption
- Symmetric vs asymmetric encryption
- Hashing basics

## Prerequisites

- Basic Python knowledge

## What Is Cryptography

Cryptography is the practice of securing communication through encoding. In web development, it protects passwords, data in transit, and sensitive information.

Think of encryption like a locked box: only people with the key can open it and see what's inside.

## Symmetric Encryption

Same key for encryption and decryption:

```python
from cryptography.fernet import Fernet

# Generate key
key = Fernet.generate_key()
print(f"Key: {key}")  # Keep this secret!

# Create cipher
cipher = Fernet(key)

# Encrypt
message = b"Hello, World!"
encrypted = cipher.encrypt(message)
print(f"Encrypted: {encrypted}")

# Decrypt
decrypted = cipher.decrypt(encrypted)
print(f"Decrypted: {decrypted}")
```

## Asymmetric Encryption

Public key for encryption, private key for decryption:

```python
from cryptography.hazmat.primitives.asymmetric import rsa
from cryptography.hazmat.primitives import serialization
from cryptography.hazmat.backends import default_backend

# Generate key pair
private_key = rsa.generate_private_key(
    public_exponent=65537,
    key_size=2048,
    backend=default_backend()
)
public_key = private_key.public_key()

# Serialize keys
private_pem = private_key.private_bytes(
    encoding=serialization.Encoding.PEM,
    format=serialization.PrivateFormat.PKCS8,
    encryption_algorithm=serialization.NoEncryption()
)

public_pem = public_key.public_bytes(
    encoding=serialization.Encoding.PEM,
    format=serialization.PublicFormat.SubjectPublicKeyInfo
)
```

## Hashing

One-way function that produces a fixed-size output:

```python
import hashlib

# SHA-256 hash
data = "Hello, World!"
hash_value = hashlib.sha256(data.encode()).hexdigest()
print(f"Hash: {hash_value}")

# MD5 (not secure, but fast)
md5_hash = hashlib.md5(data.encode()).hexdigest()

# Using salt for passwords (use bcrypt in production)
def hash_with_salt(password: str, salt: bytes) -> str:
    """Hash password with salt."""
    return hashlib.pbkdf2_hmac(
        'sha256',
        password.encode(),
        salt,
        100000
    ).hex()
```

## Summary

- Symmetric encryption uses same key
- Asymmetric uses key pairs
- Hashing is one-way

## Next Steps

Continue to `02-password-hashing.md`.
