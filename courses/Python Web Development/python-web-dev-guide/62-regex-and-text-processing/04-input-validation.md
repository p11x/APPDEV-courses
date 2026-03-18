# Input Validation

## What You'll Learn

- Validating user input
- Using regex for validation
- Building validators

## Prerequisites

- Completed `03-text-processing.md`

## Email Validation

```python
import re
from dataclasses import dataclass

@dataclass
class ValidationResult:
    valid: bool
    error: str | None = None

def validate_email(email: str) -> ValidationResult:
    """Validate email address."""
    pattern = r"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
    
    if not email:
        return ValidationResult(False, "Email is required")
    
    if not re.match(pattern, email):
        return ValidationResult(False, "Invalid email format")
    
    return ValidationResult(True)

# Usage
result = validate_email("test@example.com")
if not result.valid:
    print(result.error)
```

## Password Validation

```python
def validate_password(password: str) -> ValidationResult:
    """Validate password strength."""
    if len(password) < 8:
        return ValidationResult(False, "Password must be at least 8 characters")
    
    if not re.search(r"[A-Z]", password):
        return ValidationResult(False, "Password must contain uppercase letter")
    
    if not re.search(r"[a-z]", password):
        return ValidationResult(False, "Password must contain lowercase letter")
    
    if not re.search(r"\d", password):
        return ValidationResult(False, "Password must contain a digit")
    
    if not re.search(r"[!@#$%^&*(),.?\":{}|<>]", password):
        return ValidationResult(False, "Password must contain special character")
    
    return ValidationResult(True)
```

## Phone Number Validation

```python
def validate_phone(phone: str) -> ValidationResult:
    """Validate US phone number."""
    # Remove common formatting
    cleaned = re.sub(r"[\s\-\(\)]", "", phone)
    
    # Check for valid digits
    if not re.match(r"^\+?1?\d{10}$", cleaned):
        return ValidationResult(False, "Invalid phone number format")
    
    return ValidationResult(True)

def format_phone(phone: str) -> str:
    """Format phone number for display."""
    cleaned = re.sub(r"\D", "", phone)
    
    if len(cleaned) == 10:
        return f"({cleaned[:3]}) {cleaned[3:6]}-{cleaned[6:]}"
    elif len(cleaned) == 11 and cleaned[0] == "1":
        return f"+1 ({cleaned[1:4]}) {cleaned[4:7]}-{cleaned[7:]}"
    
    return phone
```

## URL Validation

```python
def validate_url(url: str) -> ValidationResult:
    """Validate URL."""
    pattern = (
        r"^https?://"  # http:// or https://
        r"(?:(?:[A-Z0-9](?:[A-Z0-9-]{0,61}[A-Z0-9])?\.)+[A-Z]{2,6}\.?|"  # domain
        r"localhost|"  # localhost
        r"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})"  # IP
        r"(?::\d+)?"  # optional port
        r"(?:/?|[/?]\S+)$"
    )
    
    if not re.match(pattern, url, re.IGNORECASE):
        return ValidationResult(False, "Invalid URL format")
    
    return ValidationResult(True)
```

## Form Validation

```python
from dataclasses import dataclass

@dataclass
class FormData:
    username: str
    email: str
    password: str
    confirm_password: str

@dataclass
class FormErrors:
    username: str | None = None
    email: str | None = None
    password: str | None = None
    confirm_password: str | None = None

def validate_registration(data: FormData) -> FormErrors:
    """Validate registration form."""
    errors = FormErrors()
    
    # Username
    if not data.username:
        errors.username = "Username is required"
    elif len(data.username) < 3:
        errors.username = "Username must be at least 3 characters"
    elif not re.match(r"^[a-zA-Z0-9_]+$", data.username):
        errors.username = "Username can only contain letters, numbers, and underscores"
    
    # Email
    email_result = validate_email(data.email)
    if not email_result.valid:
        errors.email = email_result.error
    
    # Password
    password_result = validate_password(data.password)
    if not password_result.valid:
        errors.password = password_result.error
    
    # Confirm password
    if data.password != data.confirm_password:
        errors.confirm_password = "Passwords do not match"
    
    return errors
```

## Summary

- Validate all user input
- Use regex for pattern matching
- Return clear error messages

## Next Steps

Continue to `05-working-with-json.md`.
