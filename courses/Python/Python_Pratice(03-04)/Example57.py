# Example57.py
# Topic: Built-in Decorators - @property, @staticmethod, @classmethod

# This file demonstrates Python's built-in decorators used in classes.


# ============================================================
# @property Decorator
# ============================================================
print("=== @property Decorator ===")

class Circle:
    """A circle with computed properties."""
    
    def __init__(self, radius):
        self._radius = radius
    
    @property
    def radius(self):
        """Get the radius."""
        return self._radius
    
    @property
    def diameter(self):
        """Calculate diameter (read-only)."""
        return self._radius * 2
    
    @property
    def area(self):
        """Calculate area (read-only)."""
        import math
        return math.pi * self._radius ** 2
    
    @property
    def circumference(self):
        """Calculate circumference (read-only)."""
        import math
        return 2 * math.pi * self._radius

circle = Circle(5)
print(f"Radius: {circle.radius}")
print(f"Diameter: {circle.diameter}")
print(f"Area: {circle.area:.2f}")
print(f"Circumference: {circle.circumference:.2f}")


# ============================================================
# @property with Setter
# ============================================================
print("\n=== @property with Setter ===")

class Temperature:
    """Temperature with Celsius and Fahrenheit."""
    
    def __init__(self, celsius):
        self._celsius = celsius
    
    @property
    def celsius(self):
        """Get temperature in Celsius."""
        return self._celsius
    
    @celsius.setter
    def celsius(self, value):
        """Set temperature in Celsius."""
        self._celsius = value
    
    @property
    def fahrenheit(self):
        """Get temperature in Fahrenheit."""
        return self._celsius * 9/5 + 32
    
    @fahrenheit.setter
    def fahrenheit(self, value):
        """Set temperature from Fahrenheit."""
        self._celsius = (value - 32) * 5/9

temp = Temperature(25)
print(f"Celsius: {temp.celsius}°C")
print(f"Fahrenheit: {temp.fahrenheit}°F")

temp.celsius = 30
print(f"After setting Celsius to 30:")
print(f"Celsius: {temp.celsius}°C")
print(f"Fahrenheit: {temp.fahrenheit}°F")

temp.fahrenheit = 100
print(f"After setting Fahrenheit to 100:")
print(f"Celsius: {temp.celsius}°C")
print(f"Fahrenheit: {temp.fahrenheit}°F")


# ============================================================
# @staticmethod Decorator
# ============================================================
print("\n=== @staticmethod Decorator ===")

class MathUtils:
    """Math utilities using static method."""
    
    @staticmethod
    def add(a, b):
        """Add two numbers."""
        return a + b
    
    @staticmethod
    def multiply(a, b):
        """Multiply two numbers."""
        return a * b
    
    @staticmethod
    def is_even(n):
        """Check if number is even."""
        return n % 2 == 0

# Call without creating instance
result = MathUtils.add(5, 3)
print(f"5 + 3 = {result}")

result = MathUtils.multiply(4, 7)
print(f"4 * 7 = {result}")

print(f"Is 10 even? {MathUtils.is_even(10)}")
print(f"Is 7 even? {MathUtils.is_even(7)}")


# ============================================================
# @classmethod Decorator
# ============================================================
print("\n=== @classmethod Decorator ===")

class Person:
    """Person class with class methods for alternative constructors."""
    
    def __init__(self, name, age):
        self.name = name
        self.age = age
    
    def __str__(self):
        return f"Person({self.name}, {self.age})"
    
    @classmethod
    def from_dict(cls, data):
        """Create Person from dictionary."""
        return cls(data['name'], data['age'])
    
    @classmethod
    def from_string(cls, s):
        """Create Person from 'name,age' string."""
        name, age = s.split(',')
        return cls(name.strip(), int(age.strip()))
    
    @classmethod
    def create_adult(cls, name):
        """Create adult with default age 18."""
        return cls(name, 18)
    
    @classmethod
    def create_child(cls, name, age=5):
        """Create child with default age 5."""
        return cls(name, age)

# Create from dictionary
person1 = Person.from_dict({"name": "Alice", "age": 30})
print(f"From dict: {person1}")

# Create from string
person2 = Person.from_string("Bob,25")
print(f"From string: {person2}")

# Create adult
person3 = Person.create_adult("Charlie")
print(f"Adult: {person3}")

# Create child
person4 = Person.create_child("Diana")
print(f"Child: {person4}")


# ============================================================
# Combining Decorators
# ============================================================
print("\n=== Combining Decorators ===")

class Rectangle:
    """Rectangle with property and class methods."""
    
    def __init__(self, width, height):
        self.width = width
        self.height = height
    
    @property
    def area(self):
        """Calculate area."""
        return self.width * self.height
    
    @property
    def perimeter(self):
        """Calculate perimeter."""
        return 2 * (self.width + self.height)
    
    @staticmethod
    def create_square(side):
        """Create a square (rectangle with equal sides)."""
        return Rectangle(side, side)
    
    @classmethod
    def create_from_area(cls, area):
        """Create rectangle from area (assumes square for simplicity)."""
        import math
        side = int(math.sqrt(area))
        return cls(side, side)

rect = Rectangle(10, 5)
print(f"Area: {rect.area}")
print(f"Perimeter: {rect.perimeter}")

square = Rectangle.create_square(7)
print(f"Square area: {square.area}")

rect2 = Rectangle.create_from_area(36)
print(f"From area 36: {rect2.width}x{rect2.height}")


# ============================================================
# Real-life Example 1: Bank Account
# ============================================================
print("\n=== Real-life: Bank Account ===")

class BankAccount:
    """Bank account with property for balance."""
    
    def __init__(self, account_number, initial_balance=0):
        self._account_number = account_number
        self._balance = initial_balance
        self._transactions = []
    
    @property
    def account_number(self):
        """Get account number (read-only)."""
        return self._account_number
    
    @property
    def balance(self):
        """Get current balance."""
        return self._balance
    
    @property
    def transaction_count(self):
        """Get number of transactions."""
        return len(self._transactions)
    
    def deposit(self, amount):
        """Deposit money."""
        if amount <= 0:
            raise ValueError("Deposit amount must be positive")
        self._balance += amount
        self._transactions.append(f"Deposit: +{amount}")
        return self._balance
    
    def withdraw(self, amount):
        """Withdraw money."""
        if amount <= 0:
            raise ValueError("Withdrawal amount must be positive")
        if amount > self._balance:
            raise ValueError("Insufficient funds")
        self._balance -= amount
        self._transactions.append(f"Withdrawal: -{amount}")
        return self._balance

account = BankAccount("ACC-12345", 1000)
print(f"Account: {account.account_number}")
print(f"Balance: ${account.balance}")

account.deposit(500)
print(f"After deposit: ${account.balance}")

account.withdraw(200)
print(f"After withdrawal: ${account.balance}")
print(f"Transactions: {account.transaction_count}")


# ============================================================
# Real-life Example 2: User Profile
# ============================================================
print("\n=== Real-life: User Profile ===")

class UserProfile:
    """User profile with computed properties."""
    
    def __init__(self, username, email, age):
        self.username = username
        self.email = email
        self.age = age
        self._posts = []
    
    @property
    def display_name(self):
        """Get display name (username with @)."""
        return f"@{self.username}"
    
    @property
    def is_adult(self):
        """Check if user is adult."""
        return self.age >= 18
    
    @property
    def post_count(self):
        """Get number of posts."""
        return len(self._posts)
    
    @property
    def email_domain(self):
        """Get email domain."""
        return self.email.split('@')[1] if '@' in self.email else None
    
    def add_post(self, content):
        """Add a post."""
        self._posts.append(content)

user = UserProfile("alice", "alice@example.com", 25)
print(f"Username: {user.username}")
print(f"Display name: {user.display_name}")
print(f"Email domain: {user.email_domain}")
print(f"Is adult: {user.is_adult}")

user.add_post("Hello world!")
user.add_post("Python is awesome!")
print(f"Post count: {user.post_count}")


# ============================================================
# Real-life Example 3: Product with Class Methods
# ============================================================
print("\n=== Real-life: Product Factory ===")

class Product:
    """Product with class methods for different creation scenarios."""
    
    def __init__(self, name, price, quantity):
        self.name = name
        self.price = price
        self.quantity = quantity
    
    @property
    def total_value(self):
        """Total value of product in inventory."""
        return self.price * self.quantity
    
    @classmethod
    def from_dict(cls, data):
        """Create product from dictionary."""
        return cls(data['name'], data['price'], data['quantity'])
    
    @classmethod
    def create_dummy(cls):
        """Create a dummy product for testing."""
        return cls("Dummy Product", 0, 0)
    
    @classmethod
    def create_with_discount(cls, name, price, quantity, discount):
        """Create product with discount applied."""
        discounted_price = price * (1 - discount)
        return cls(name, discounted_price, quantity)

# Create products
p1 = Product("Laptop", 999, 10)
print(f"Product: {p1.name}, Value: ${p1.total_value}")

p2 = Product.from_dict({"name": "Mouse", "price": 29, "quantity": 50})
print(f"Product: {p2.name}, Value: ${p2.total_value}")

p3 = Product.create_dummy()
print(f"Dummy: {p3.name}")

p4 = Product.create_with_discount("Tablet", 500, 20, 0.1)
print(f"Discounted: {p4.name}, Price: ${p4.price}")


# ============================================================
# Real-life Example 4: Configuration Manager
# ============================================================
print("\n=== Real-life: Configuration Manager ===")

class ConfigManager:
    """Configuration manager with defaults and overrides."""
    
    _defaults = {
        "debug": False,
        "max_connections": 100,
        "timeout": 30,
        "log_level": "INFO"
    }
    
    def __init__(self, config_dict=None):
        self._config = {}
        if config_dict:
            self._config.update(config_dict)
    
    @property
    def debug(self):
        return self._config.get("debug", self._defaults["debug"])
    
    @debug.setter
    def debug(self, value):
        self._config["debug"] = value
    
    @property
    def max_connections(self):
        return self._config.get("max_connections", self._defaults["max_connections"])
    
    @property
    def timeout(self):
        return self._config.get("timeout", self._defaults["timeout"])
    
    @property
    def log_level(self):
        return self._config.get("log_level", self._defaults["log_level"])
    
    @classmethod
    def create_production(cls):
        """Create production config."""
        return cls({"debug": False, "log_level": "ERROR"})
    
    @classmethod
    def create_development(cls):
        """Create development config."""
        return cls({"debug": True, "log_level": "DEBUG"})

# Create configs
prod_config = ConfigManager.create_production()
print(f"Production - Debug: {prod_config.debug}, Log: {prod_config.log_level}")

dev_config = ConfigManager.create_development()
print(f"Development - Debug: {dev_config.debug}, Log: {dev_config.log_level}")

custom = ConfigManager({"timeout": 60})
print(f"Custom - Timeout: {custom.timeout}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("KEY TAKEAWAYS:")
print("=" * 50)
print("""
@property:
  - Creates getter methods
  - Allows computed/derived properties
  - Can have setter for validation
  - Access like regular attributes

@staticmethod:
  - Method doesn't need self
  - Call without creating instance
  - Use for utility functions

@classmethod:
  - Receives class as first argument
  - Alternative constructors (from_dict, from_string)
  - Factory methods
""")
