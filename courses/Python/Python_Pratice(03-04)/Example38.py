# Example38.py
# Topic: Accessing Docstrings at Runtime

# This file demonstrates how to access and use docstrings at runtime
# for documentation, help systems, and introspection.


# Basic docstring access
def add(a: int, b: int) -> int:
    """Add two numbers together.
    
    Args:
        a: First number.
        b: Second number.
        
    Returns:
        The sum of a and b.
    """
    return a + b

# Access the docstring via __doc__ attribute
print("=== Function Docstring ===")    # === Function Docstring ===
print(add.__doc__)    # Add two numbers together.
    
# Access function name and module
print(f"\n=== Function Info ===")    # (blank line)
print(f"Name: {add.__name__}")    # Name: add
print(f"Module: {add.__module__}")    # Module: __main__


# Class docstring access
class Calculator:
    """A simple calculator class for basic arithmetic operations.
    
    This class provides methods for addition, subtraction,
    multiplication, and division operations.
    
    Attributes:
        last_result: The result of the last calculation.
    """
    
    def __init__(self):
        """Initialize the calculator with zero as default."""
        self.last_result = 0
    
    def add(self, a: float, b: float) -> float:
        """Add two numbers."""
        self.last_result = a + b
        return self.last_result
    
    def subtract(self, a: float, b: float) -> float:
        """Subtract b from a."""
        self.last_result = a - b
        return self.last_result

print("\n=== Class Docstring ===")    # (blank line)
print(Calculator.__doc__)    # A simple calculator class for basic arithmetic operations.

print("\n=== Method Docstrings ===")    # (blank line)
print(f"add: {Calculator.add.__doc__}")    # add: Add two numbers.
print(f"subtract: {Calculator.subtract.__doc__}")    # subtract: Subtract b from a.


# Module-level docstring access
import datetime

print("\n=== Module Docstrings ===")    # (blank line)
print(f"datetime module: {datetime.__doc__[:50]}...")    # datetime module: Classes for manipulating dates and times...


# Using help() function
def show_help(func):
    """Display help information for a function."""
    print(f"\n=== Help for {func.__name__} ===")    # (blank line)
    help(func)

# Uncomment to see help output:
# show_help(add)


# Dynamic docstring generation
class DataProcessor:
    """Base class for data processing."""
    
    def process(self, data: list) -> list:
        """Process a list of data items."""
        return data
    
    @property
    def description(self) -> str:
        """Generate a description based on class docstring."""
        return self.__doc__ or "No description available"

processor = DataProcessor()
print(f"\n=== Dynamic Description ===")    # (blank line)
print(processor.description)    # Base class for data processing.


# Inspect module for functions with docstrings
import math

print("\n=== Inspecting Math Module Functions ===")    # (blank line)
funcs_with_docs = [(name, func) for name, func in vars(math).items() 
                   if callable(func) and func.__doc__]

for name, func in funcs_with_docs[:3]:  # Show first 3
    print(f"{name}: {func.__doc__[:50]}...")    # acos: acos(x) Return the arc cosine ...


# Real-life Example 1: Auto-generate API documentation
class APIDocumentation:
    """Automatically generates documentation from docstrings."""
    
    @staticmethod
    def generate(function: Callable) -> str:
        """Generate documentation for a function."""
        doc = function.__doc__
        if not doc:
            return f"{function.__name__}: No documentation"
        
        lines = [f"## {function.__name__}"]
        lines.append("")
        
        # Parse docstring
        for line in doc.strip().split("\n"):
            stripped = line.strip()
            if stripped:
                lines.append(stripped)
        
        return "\n".join(lines)
    
    @staticmethod
    def generate_all(functions: List[Callable]) -> str:
        """Generate documentation for multiple functions."""
        docs = [APIDocumentation.generate(func) for func in functions]
        return "\n\n".join(docs)

def create_user(name: str, email: str) -> dict:
    """Create a new user in the system.
    
    Args:
        name: The user's full name.
        email: The user's email address.
        
    Returns:
        Dictionary with user information.
    """
    return {"name": name, "email": email}

def update_user(user_id: int, **kwargs: str) -> dict:
    """Update user information.
    
    Args:
        user_id: The ID of the user to update.
        **kwargs: Fields to update.
        
    Returns:
        Updated user dictionary.
    """
    return {"id": user_id, "updated": True}

print("\n=== Auto-generated API Docs ===")    # (blank line)
print(APIDocumentation.generate(create_user))    # ## create_user...


# Real-life Example 2: Command-line help generator
class Command:
    """Represents a CLI command with help text."""
    
    def __init__(self, name: str, func: Callable, description: str = ""):
        self.name = name
        self.func = func
        self.description = description or (func.__doc__ or "").strip().split("\n")[0]
    
    def get_help(self) -> str:
        """Get formatted help text for this command."""
        lines = [
            f"Command: {self.name}",
            f"Description: {self.description}",
            ""
        ]
        
        if self.func.__doc__:
            lines.append("Usage:")
            lines.append(self.func.__doc__)
        
        return "\n".join(lines)

def cmd_list_users(limit: int = 10, offset: int = 0) -> list:
    """List users from the database.
    
    Args:
        limit: Maximum number of users to return.
        offset: Number of users to skip.
        
    Returns:
        List of user dictionaries.
    """
    return [{"id": i, "name": f"User {i}"} for i in range(offset, offset + limit)]

def cmd_create_backup(backup_name: str, compress: bool = True) -> str:
    """Create a database backup.
    
    Args:
        backup_name: Name for the backup file.
        compress: Whether to compress the backup.
        
    Returns:
        Status message.
    """
    return f"Backup '{backup_name}' created" + (" (compressed)" if compress else "")

commands = [
    Command("list-users", cmd_list_users),
    Command("create-backup", cmd_create_backup)
]

print("\n=== CLI Help System ===")    # (blank line)
for cmd in commands:
    print(cmd.get_help())    # Command: list-users...
    print("-" * 40)    # ----------------------------------------


# Real-life Example 3: Validation rule registry
class ValidationRule:
    """A validation rule with documentation."""
    
    def __init__(self, name: str, validator: Callable, error_message: str = ""):
        self.name = name
        self.validator = validator
        self.error_message = error_message or (validator.__doc__ or "Validation failed").strip().split("\n")[0]
    
    def validate(self, value: Any) -> bool:
        """Run the validation."""
        return self.validator(value)
    
    def __repr__(self) -> str:
        return f"ValidationRule({self.name})"

def validate_not_empty(value: Any) -> bool:
    """Check that value is not empty.
    
    Returns:
        True if value is not empty, False otherwise.
    """
    if value is None:
        return False
    if isinstance(value, (str, list, dict)):
        return len(value) > 0
    return True

def validate_positive(value: int) -> bool:
    """Check that value is a positive number.
    
    Args:
        value: Number to check.
        
    Returns:
        True if positive, False otherwise.
    """
    return isinstance(value, (int, float)) and value > 0

def validate_email_format(value: str) -> bool:
    """Check that value is a valid email format.
    
    Returns:
        True if valid email format, False otherwise.
    """
    return isinstance(value, str) and "@" in value and "." in value

rules = [
    ValidationRule("not_empty", validate_not_empty),
    ValidationRule("positive", validate_positive),
    ValidationRule("email", validate_email_format)
]

print("\n=== Validation Rules ===")    # (blank line)
for rule in rules:
    print(f"Rule: {rule.name}")    # Rule: not_empty
    print(f"  Error: {rule.error_message}")    # Error: Check that value is not empty.
    print(f"  Test: {rule.validate('test@example.com')}")    # Test: True
    print()    # (blank line)


# Real-life Example 4: Plugin documentation system
class Plugin:
    """A plugin with documentation."""
    
    def __init__(self, name: str, version: str, description: str = ""):
        self.name = name
        self.version = version
        self.description = description
    
    @property
    def docstring(self) -> str:
        """Get the plugin description."""
        return self.description or "No description"

class PluginRegistry:
    """Registry for managing plugins."""
    
    def __init__(self):
        self.plugins: Dict[str, Plugin] = {}
    
    def register(self, plugin: Plugin) -> None:
        """Register a plugin."""
        self.plugins[plugin.name] = plugin
    
    def get_documentation(self) -> str:
        """Generate documentation for all plugins."""
        lines = ["# Plugin Documentation", ""]
        
        for name, plugin in self.plugins.items():
            lines.append(f"## {name} (v{plugin.version})")
            lines.append("")
            lines.append(plugin.description or "No description available")
            lines.append("")
        
        return "\n".join(lines)

class AuthPlugin(Plugin):
    """Provides authentication and authorization services.
    
    Features:
        - User login/logout
        - JWT token generation
        - Password hashing
        - Session management
    """
    def __init__(self):
        super().__init__("auth", "1.0.0", __doc__)

class PaymentPlugin(Plugin):
    """Handles payment processing and transactions.
    
    Supported providers:
        - Stripe
        - PayPal
        - Square
    """
    def __init__(self):
        super().__init__("payment", "2.1.0", __doc__)

registry = PluginRegistry()
registry.register(AuthPlugin())
registry.register(PaymentPlugin())

print("\n=== Plugin Documentation ===")    # (blank line)
print(registry.get_documentation())    # # Plugin Documentation...
