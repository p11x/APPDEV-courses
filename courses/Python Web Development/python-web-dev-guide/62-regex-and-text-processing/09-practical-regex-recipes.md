# Practical Regex Recipes

## What You'll Learn

- Common regex patterns
- Real-world examples
- Debugging regex

## Prerequisites

- Completed `08-string-manipulation-techniques.md`

## Common Patterns

```python
import re

# Username: alphanumeric and underscore, 3-20 chars
username = r"^[a-zA-Z0-9_]{3,20}$"

# Strong password: at least 8 chars, uppercase, lowercase, digit
password = r"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"

# Hex color code
hex_color = r"^#?([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$"

# Credit card number (simplified)
credit_card = r"^\d{4}[\s-]?\d{4}[\s-]?\d{4}[\s-]?\d{4}$"

# UUID
uuid = r"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$"

# HTML tag
html_tag = r"<([a-z]+)([^<]+)*(?:>(.*)<\/\1>|\s+\/>)"
```

## Real-World Examples

```python
# Extract all URLs
text = "Visit https://example.com or http://test.org"
urls = re.findall(r"https?://[^\s]+", text)

# Extract all numbers with units
text = "Price: $50, Weight: 100kg"
prices = re.findall(r"\$\d+", text)      # ['$50']
weights = re.findall(r"\d+kg", text)      # ['100kg']

# Find all email addresses
text = "Contact us at info@example.com or support@test.org"
emails = re.findall(r"[\w.-]+@[\w.-]+\.\w+", text)

# Extract dates from text
text = "Events on 2024-01-15 and 2024-02-20"
dates = re.findall(r"\d{4}-\d{2}-\d{2}", text)

# Replace multiple spaces with single space
text = "Too   many    spaces"
cleaned = re.sub(r"\s+", " ", text)
```

## Data Extraction

```python
# Extract data from log lines
log = "2024-01-15 10:30:45 ERROR Connection timeout"
pattern = r"(\d{4}-\d{2}-\d{2})\s+(\d{2}:\d{2}:\d{2})\s+(\w+)\s+(.+)"
match = re.match(pattern, log)
if match:
    date, time, level, message = match.groups()
    print(f"{date} {level}: {message}")

# Extract structured info
text = "Order #12345 for $99.99 shipped"
pattern = r"Order #(\d+) for \$(\d+\.\d+) (.+)"
match = re.search(pattern, text)
if match:
    order_id, amount, status = match.groups()
```

## Password Validation

```python
import re

def validate_password_strength(password: str) -> dict:
    """Validate password strength."""
    errors = []
    
    if len(password) < 8:
        errors.append("At least 8 characters")
    if not re.search(r"[A-Z]", password):
        errors.append("At least one uppercase letter")
    if not re.search(r"[a-z]", password):
        errors.append("At least one lowercase letter")
    if not re.search(r"\d", password):
        errors.append("At least one digit")
    if not re.search(r"[!@#$%^&*(),.?\":{}|<>]", password):
        errors.append("At least one special character")
    
    return {
        "valid": len(errors) == 0,
        "errors": errors
    }
```

## Slug Generation

```python
def create_slug(text: str) -> str:
    """Create URL-friendly slug from text."""
    # Convert to lowercase
    slug = text.lower()
    
    # Replace spaces with hyphens
    slug = re.sub(r"\s+", "-", slug)
    
    # Remove non-alphanumeric (except hyphen)
    slug = re.sub(r"[^a-z0-9-]", "", slug)
    
    # Remove duplicate hyphens
    slug = re.sub(r"-+", "-", slug)
    
    # Remove leading/trailing hyphens
    slug = slug.strip("-")
    
    return slug

# Usage
create_slug("Hello World!")  # "hello-world"
create_slug("Python  &  JavaScript")  # "python-javascript"
```

## Debugging Regex

```python
import re

# Use verbose mode for complex regex
pattern = re.compile(r"""
    ^                   # Start of string
    (?P<protocol>https?) # Protocol
    ://                 # Separator
    (?P<domain>[\w.-]+)  # Domain
    (?P<path>/[\w./-]*)?  # Optional path
    $                   # End of string
""", re.VERBOSE)

match = pattern.match("https://example.com/path")
if match:
    print(match.groupdict())
```

## Summary

- Regex is powerful for text processing
- Use verbose mode for complex patterns
- Test regex patterns thoroughly

## Next Steps

Continue to `10-performance-considerations.md`.
