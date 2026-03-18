# Hashing and Signing

## What You'll Learn

- Message signing
- HMAC
- Digital signatures

## Prerequisites

- Completed `06-secure-api-design.md`

## HMAC (Hash-based Message Authentication Code)

```python
import hmac
import hashlib

def create_hmac(message: str, secret: str) -> str:
    """Create HMAC signature."""
    return hmac.new(
        secret.encode(),
        message.encode(),
        hashlib.sha256
    ).hexdigest()

def verify_hmac(message: str, signature: str, secret: str) -> bool:
    """Verify HMAC signature."""
    expected = create_hmac(message, secret)
    return hmac.compare_digest(expected, signature)

# Usage
message = "Hello, World!"
secret = "my_secret_key"

signature = create_hmac(message, secret)
print(f"Signature: {signature}")

is_valid = verify_hmac(message, signature, secret)
print(f"Valid: {is_valid}")
```

## Using hashlib

```python
import hashlib
import secrets

# SHA-256
data = "Hello, World!"
sha256_hash = hashlib.sha256(data.encode()).hexdigest()

# SHA-512 (more secure)
sha512_hash = hashlib.sha512(data.encode()).hexdigest()

# PBKDF2 (for password hashing)
salt = secrets.token_bytes(32)
key = hashlib.pbkdf2_hmac(
    'sha256',
    b'password',
    salt,
    100000
).hex()

# scrypt (memory-hard)
key = hashlib.scrypt(
    b'password',
    salt=salt,
    n=16384,
    r=8,
    p=1
).hex()
```

## Digital Signatures

```python
from cryptography.hazmat.primitives.asymmetric import rsa
from cryptography.hazmat.primitives import hashes, serialization
from cryptography.hazmat.backends import default_backend

# Generate key pair
private_key = rsa.generate_private_key(
    public_exponent=65537,
    key_size=2048,
    backend=default_backend()
)
public_key = private_key.public_key()

# Sign message
message = b"Hello, World!"
signature = private_key.sign(
    message,
    hashes.SHA256()
)

# Verify signature
try:
    public_key.verify(
        signature,
        message,
        hashes.SHA256()
    )
    print("Signature valid!")
except Exception:
    print("Signature invalid!")
```

## Using Ed25519 (Recommended)

```python
from cryptography.hazmat.primitives.asymmetric import ed25519

# Generate key pair
private_key = ed25519.Ed25519PrivateKey.generate()
public_key = private_key.public_key()

# Sign message
message = b"Hello, World!"
signature = private_key.sign(message)

# Verify
public_key.verify(signature, message)
```

## Signing API Requests

```python
import hmac
import hashlib
import time

class APIAuth:
    def __init__(self, api_secret: str):
        self.api_secret = api_secret
    
    def sign_request(self, method: str, path: str, body: str) -> dict:
        """Sign an API request."""
        timestamp = str(int(time.time()))
        message = f"{method}{path}{body}{timestamp}"
        
        signature = hmac.new(
            self.api_secret.encode(),
            message.encode(),
            hashlib.sha256
        ).hexdigest()
        
        return {
            "X-Timestamp": timestamp,
            "X-Signature": signature
        }
    
    def verify_request(self, method: str, path: str, body: str, 
                       timestamp: str, signature: str) -> bool:
        """Verify an API request."""
        message = f"{method}{path}{body}{timestamp}"
        expected = hmac.new(
            self.api_secret.encode(),
            message.encode(),
            hashlib.sha256
        ).hexdigest()
        
        return hmac.compare_digest(expected, signature)
```

## Summary

- HMAC for message authentication
- Digital signatures for non-repudiation
- Use Ed25519 for new applications

## Next Steps

Continue to `08-secure-random.md`.
