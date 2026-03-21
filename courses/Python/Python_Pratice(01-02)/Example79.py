# Example79.py
# Topic: Exception Handling — ExceptionGroups Validation Example

# Using ExceptionGroups for form validation

# === Traditional approach (works in all Python versions) ===

class ValidationError(Exception):
    """Exception for validation failures."""
    pass


def validate_field(value, field_name, validators):
    """Validate a single field with multiple rules."""
    errors = []
    
    for rule in validators:
        rule_type = rule["type"]
        
        if rule_type == "required":
            if not value and value != 0:
                errors.append(f"{field_name} is required")
        
        elif rule_type == "min_length":
            if len(str(value)) < rule["value"]:
                errors.append(f"{field_name} must be at least {rule['value']} chars")
        
        elif rule_type == "max_length":
            if len(str(value)) > rule["value"]:
                errors.append(f"{field_name} must be at most {rule['value']} chars")
    
    if errors:
        raise ValidationError("; ".join(errors))


def validate_form(data):
    """Validate entire form, collecting all errors."""
    errors = []
    
    # Define validators
    validators = {
        "username": [
            {"type": "required"},
            {"type": "min_length", "value": 3},
            {"type": "max_length", "value": 20},
        ],
        "email": [
            {"type": "required"},
        ],
        "age": [
            {"type": "required"},
        ],
    }
    
    # Validate each field
    for field, field_validators in validators.items():
        try:
            validate_field(data.get(field), field, field_validators)
        except ValidationError as e:
            errors.append((field, str(e)))
    
    if errors:
        return {"status": "invalid", "errors": errors}
    
    return {"status": "valid"}


# === Test with invalid data ===
print("=== Invalid Form Data ===")
invalid_data = {
    "username": "ab",  # Too short
    "email": "",       # Required but empty
    "age": None,       # Required but None
}

result = validate_form(invalid_data)
print("Status: " + result["status"])

if result["status"] == "invalid":
    print("Errors found:")
    for field, error in result["errors"]:
        print(f"  - {field}: {error}")

# === Test with valid data ===
print("\n=== Valid Form Data ===")
valid_data = {
    "username": "alice123",
    "email": "alice@example.com",
    "age": 25,
}

result = validate_form(valid_data)
print("Status: " + result["status"])

# === Python 3.11+ ExceptionGroup approach ===
# In Python 3.11+, you could use:
# raise ExceptionGroup("Validation failed", [ValidationError(...) for ...])

print("\nNote: ExceptionGroup requires Python 3.11+ for the raise syntax")
