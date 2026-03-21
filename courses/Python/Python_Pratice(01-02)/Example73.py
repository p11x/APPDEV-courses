# Example73.py
# Topic: Exception Handling — Custom Exception Classes

# Create your own exceptions for your application

# === Basic custom exception ===
class InvalidAgeError(Exception):
    """Raised when age is invalid."""
    pass


def set_age(age):
    if age < 0:
        raise InvalidAgeError("Age cannot be negative!")
    if age > 150:
        raise InvalidAgeError("Age is unrealistic!")
    print("Age set to: " + str(age))


try:
    set_age(-5)
except InvalidAgeError as e:
    print("Error: " + str(e))

set_age(25)  # Works

# === Custom exception with attributes ===
class ValidationError(Exception):
    """Custom exception for validation errors.
    
    Attributes:
        field: The field that failed validation
        value: The invalid value
    """
    
    def __init__(self, message, field, value):
        super().__init__(message)
        self.field = field
        self.value = value


def validate_username(username):
    if not username:
        raise ValidationError(
            "Username cannot be empty",
            field="username",
            value=username
        )
    if len(username) < 3:
        raise ValidationError(
            "Username too short",
            field="username",
            value=username
        )


try:
    validate_username("")
except ValidationError as e:
    print("Error in " + e.field + ": " + str(e))
    print("Invalid value: " + str(e.value))

# === Custom exception with __str__ override ===
class ValidationError2(Exception):
    """Custom exception for validation."""
    
    def __init__(self, message, field):
        super().__init__(message)
        self.field = field
    
    def __str__(self):
        return f"Validation error for '{self.field}': {super().__str__()}"


def validate_age(age):
    if age < 0:
        raise ValidationError2("Age cannot be negative", "age")
    if age > 150:
        raise ValidationError2("Age is unrealistic", "age")
    return age


try:
    validate_age(-5)
except ValidationError2 as e:
    print(e)

# === Practical: Validation library ===
class FieldError(Exception):
    """Error for form field validation."""
    
    def __init__(self, field, message):
        super().__init__(message)
        self.field = field


def validate_email(email):
    if "@" not in email:
        raise FieldError("email", "Invalid email format")
    return True


def validate_password(password):
    if len(password) < 8:
        raise FieldError("password", "Password too short")
    return True


# Test
tests = [
    {"email": "test@example.com", "password": "password123"},
    {"email": "invalid", "password": "password123"},
    {"email": "test@example.com", "password": "short"},
]

for data in tests:
    try:
        validate_email(data["email"])
        validate_password(data["password"])
        print("Valid: " + str(data))
    except FieldError as e:
        print("Invalid " + e.field + ": " + str(e))
