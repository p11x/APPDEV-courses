# Secure Random

## What You'll Learn

- Using secrets module
- Generating secure tokens
- Cryptographic randomness

## Prerequisites

- Completed `07-hashing-and-signing.md`

## Using Secrets Module

Python's secrets module provides cryptographically strong random numbers:

```python
import secrets

# Generate random bytes
random_bytes = secrets.token_bytes(16)
print(f"Random bytes: {random_bytes.hex()}")

# Generate random URL-safe tokens
token = secrets.token_urlsafe(32)
print(f"Token: {token}")

# Generate random hex string
hex_string = secrets.token_hex(16)
print(f"Hex: {hex_string}")
```

## Generating Tokens

```python
import secrets
import string
import hashlib
import time

def generate_session_token() -> str:
    """Generate secure session token."""
    random_bytes = secrets.token_urlsafe(32)
    return random_bytes

def generate_verification_code(length: int = 6) -> str:
    """Generate numeric verification code."""
    return ''.join(secrets.choice(string.digits) for _ in range(length))

def generate_password_reset_token() -> dict:
    """Generate password reset token with expiry."""
    token = secrets.token_urlsafe(32)
    expiry = int(time.time()) + 3600  # 1 hour
    
    return {
        "token": token,
        "expiry": expiry
    }
```

## Secure Password Generation

```python
import secrets
import string

def generate_secure_password(length: int = 16) -> str:
    """Generate a secure random password."""
    alphabet = string.ascii_letters + string.digits + string.punctuation
    
    # Ensure at least one of each type
    password = [
        secrets.choice(string.ascii_lowercase),
        secrets.choice(string.ascii_uppercase),
        secrets.choice(string.digits),
        secrets.choice(string.punctuation)
    ]
    
    # Fill rest randomly
    for _ in range(length - 4):
        password.append(secrets.choice(alphabet))
    
    # Shuffle
    secrets.SystemRandom().shuffle(password)
    
    return ''.join(password)

# Usage
password = generate_secure_password()
print(f"Password: {password}")
```

## Comparison (Timing-Safe)

```python
import secrets

def constant_time_compare(a: str, b: str) -> bool:
    """Compare strings in constant time to prevent timing attacks."""
    return secrets.compare_digest(a.encode(), b.encode())

# Usage
stored_hash = "abc123"
input_hash = "abc123"

if constant_time_compare(stored_hash, input_hash):
    print("Match!")
else:
    print("No match!")
```

## Token Bucket Algorithm

```python
import time
import secrets

class RateLimiter:
    def __init__(self, capacity: int, refill_rate: float):
        self.capacity = capacity
        self.refill_rate = refill_rate
        self.tokens = capacity
        self.last_refill = time.time()
    
    def _refill(self) -> None:
        now = time.time()
        elapsed = now - self.last_refill
        self.tokens = min(
            self.capacity,
            self.tokens + elapsed * self.refill_rate
        )
        self.last_refill = now
    
    def allow_request(self, tokens: int = 1) -> bool:
        """Check if request is allowed."""
        self._refill()
        
        if self.tokens >= tokens:
            self.tokens -= tokens
            return True
        
        return False
```

## Summary

- Use secrets module for cryptographic randomness
- Generate tokens with token_urlsafe
- Use constant-time comparison

## Next Steps

Continue to `09-common-vulnerabilities.md`.
