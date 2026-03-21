# The Secrets Module

## What You'll Learn

- Generating cryptographically secure random numbers
- Creating secure tokens
- Constant-time comparison
- Password generation

## Prerequisites

- Read [08_algorithm_challenges.md](../../04_Data_Structures/03_Algorithms_With_Python/08_algorithm_challenges.md) first

## Generating Random Tokens

```python
# secrets_demo.py

import secrets

# Generate random bytes
token_bytes = secrets.token_bytes(32)
print(f"Token (bytes): {token_bytes}")

# Generate random hex string
token_hex = secrets.token_hex(32)
print(f"Token (hex): {token_hex}")

# Generate random URL-safe string
token_url = secrets.token_urlsafe(32)
print(f"Token (URL-safe): {token_url}")
```

## Secure Comparison

```python
# secure_comparison.py

import secrets

# Constant-time comparison - prevents timing attacks
def verify_token(expected: str, provided: str) -> bool:
    return secrets.compare_digest(expected, provided)


# Usage
real_token = secrets.token_hex(16)
fake_token = "different_token"

print(verify_token(real_token, real_token))  # True
print(verify_token(real_token, fake_token))  # False
```

## Annotated Full Example

```python
# secrets_module_demo.py
"""Complete demonstration of secrets module."""

import secrets
import string


def generate_password(length: int = 16) -> str:
    """Generate a secure random password."""
    alphabet = string.ascii_letters + string.digits
    return "".join(secrets.choice(alphabet) for _ in range(length))


def generate_verification_code() -> str:
    """Generate a numeric verification code."""
    return "".join(secrets.choice(string.digits) for _ in range(6))


def main() -> None:
    # Tokens
    print(f"Token: {secrets.token_urlsafe(32)}")
    
    # Password
    password = generate_password()
    print(f"Password: {password}")
    
    # Verification code
    code = generate_verification_code()
    print(f"Verification code: {code}")
    
    # Constant-time comparison
    a = secrets.token_hex(16)
    b = secrets.token_hex(16)
    print(f"Same (different): {secrets.compare_digest(a, b)}")
    print(f"Same (equal): {secrets.compare_digest(a, a)}")


if __name__ == "__main__":
    main()
```

## Summary

- Generating cryptographically secure random numbers
- Creating secure tokens
- Constant-time comparison

## Next Steps

Continue to **[02_env_vars_and_dotenv.md](./02_env_vars_and_dotenv.md)**
