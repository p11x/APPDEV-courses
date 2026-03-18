# ♻️ Refactoring in Practice

> How to improve messy existing code without breaking it.

## 🎯 What You'll Learn

- The refactoring mindset
- Code smells to watch for
- 8 safe refactoring techniques
- Full worked example

## 📦 Prerequisites

- Completion of [02_layered_and_hexagonal_architecture.md](./02_layered_and_hexagonal_architecture.md)

---

## The Refactoring Mindset

> Change structure, not behaviour.

### Golden Rules

1. **Always have tests first** — or write them before refactoring
2. **Make small changes** — one refactoring at a time
3. **Run tests frequently** — after each change
4. **Commit often** — so you can rollback if needed

### Refactoring vs Rewriting

| Refactoring | Rewriting |
|-------------|-----------|
| Improves structure | Starts from scratch |
| Keeps behaviour | Risk of missing features |
| Lower risk | Higher risk |
| Incremental | Big bang |

---

## Code Smells to Watch For

### 1. Long Function (> 20 lines doing multiple things)

```python
# ❌ SMELL: Too long, does validation, processing, saving
def process_order(order_data):
    # 50 lines of mixed responsibilities
    # Validation
    if not order_data.get("customer"):
        raise ValueError("Missing customer")
    
    # Processing
    total = 0
    for item in order_data["items"]:
        # ... complex calculations ...
    
    # Saving to database
    # ... database code ...
    
    # Sending email
    # ... email code ...
    
    return result
```

### 2. God Class (knows and does everything)

```python
# ❌ SMELL: One class handles everything
class OrderManager:
    def validate_order(self): ...
    def calculate_tax(self): ...
    def process_payment(self): ...
    def update_inventory(self): ...
    def send_confirmation(self): ...
    def generate_report(self): ...
    def handle_returns(self): ...
    # ... 20 more methods ...
```

### 3. Feature Envy (method uses another class's data more than its own)

```python
# ❌ SMELL: This method should be on Address class
class User:
    def __init__(self, address):
        self.address = address
    
    def get_formatted_address(self):
        # Uses address data extensively
        return f"{self.address.street}, {self.address.city}, {self.address.state} {self.address.zip}"
```

### 4. Data Clumps (same 3 variables always together)

```python
# ❌ SMELL: These always appear together
def create_user(name, email, password):
    # ... use all three ...

def authenticate_user(name, email, password):
    # ... use all three again ...

def reset_password(name, email, password):
    # ... and again ...
```

### 5. Primitive Obsession (using str for email, int for money)

```python
# ❌ SMELL: Primitives instead of proper types
def send_invoice(customer_email: str, amount: int, due_date: str):
    # No validation that email is valid
    # No protection against negative amounts
    # No date parsing/validation
    pass
```

### 6. Long Parameter List (> 4 params)

```python
# ❌ SMELL: Hard to remember order
def create_user(name, email, password, age, address, phone, newsletter_pref):
    # Which order was it again?
    pass
```

### 7. Duplicate Code (same logic copy-pasted)

```python
# ❌ SMELL: Same validation in multiple places
def process_order(order):
    if "@" not in order.customer.email:
        raise ValueError("Invalid email")

def update_customer(customer):
    if "@" not in customer.email:
        raise ValueError("Invalid email")
```

### 8. Dead Code (functions/vars never used)

```python
# ❌ SMELL: Never called
def legacy_calculation():
    # This code is never used
    return some_old_formula()

# ❌ SMELL: Never referenced
UNUSED_CONSTANT = 42
```

### 9. Magic Numbers (unexplained numeric literals)

```python
# ❌ SMELL: What does 0.15 mean?
def calculate_tax(amount):
    return amount * 0.15

# ❌ SMELL: What does 30 mean?
if user.last_login < datetime.now() - timedelta(days=30):
    # Is this days? seconds?
    pass
```

### 10. Deeply Nested Code (> 3 levels)

```python
# ❌ SMELL: Hard to follow
def process_data(items):
    for item in items:  # Level 1
        if item.valid:  # Level 2
            for subitem in item.subitems:  # Level 3
                if subitem.active:  # Level 4
                    if subitem.value > 0:  # Level 5
                        # ... processing ...
                        pass
```

---

## 8 Safe Refactoring Techniques

### 1. Extract Function

```python
# ❌ BEFORE
def print_order_details(order):
    print(f"Order #{order.id}")
    print(f"Customer: {order.customer.name}")
    print(f"Email: {order.customer.email}")
    total = 0
    for item in order.items:
        total += item.price * item.quantity
    print(f"Total: ${total}")

# ✅ AFTER: Extracted customer details
def print_customer_details(customer):
    print(f"Customer: {customer.name}")
    print(f"Email: {customer.email}")

def print_order_details(order):
    print(f"Order #{order.id}")
    print_customer_details(order.customer)
    total = 0
    for item in order.items:
        total += item.price * item.quantity
    print(f"Total: ${total}")
```

### 2. Extract Class

```python
# ❌ BEFORE: User class doing too much
class User:
    def __init__(self, name, email, street, city, state, zip_code):
        self.name = name
        self.email = email
        self.street = street
        self.city = city
        self.state = state
        self.zip_code = zip_code
    
    def get_full_address(self):
        return f"{self.street}, {self.city}, {self.state} {self.zip_code}"

# ✅ AFTER: Extracted Address class
class Address:
    def __init__(self, street, city, state, zip_code):
        self.street = street
        self.city = city
        self.state = state
        self.zip_code = zip_code
    
    def get_full_address(self):
        return f"{self.street}, {self.city}, {self.state} {self.zip_code}"

class User:
    def __init__(self, name, email, address: Address):
        self.name = name
        self.email = email
        self.address = address
    
    def get_full_address(self):
        return self.address.get_full_address()
```

### 3. Rename Method/Variable

```python
# ❌ BEFORE: Unclear names
def calc(a, b):
    return a * b * 0.15

# ✅ AFTER: Clear names
def calculate_tax(amount, rate=0.15):
    return amount * rate
```

### 4. Move Method

```python
# ❌ BEFORE: Method in wrong class
class Order:
    def __init__(self, customer):
        self.customer = customer
    
    def get_customer_email(self):  # Should be on Customer
        return self.customer.email

# ✅ AFTER: Moved to correct class
class Customer:
    def __init__(self, email):
        self.email = email
    
    def get_email(self):
        return self.email

class Order:
    def __init__(self, customer: Customer):
        self.customer = customer
    
    def get_customer_email(self):
        return self.customer.get_email()
```

### 5. Replace Conditional with Polymorphism

```python
# ❌ BEFORE: Conditional logic based on type
def process_payment(payment_type, amount):
    if payment_type == "credit_card":
        return process_credit_card(amount)
    elif payment_type == "paypal":
        return process_paypal(amount)
    elif payment_type == "bank_transfer":
        return process_bank_transfer(amount)

# ✅ AFTER: Polymorphism
from abc import ABC, abstractmethod

class PaymentProcessor(ABC):
    @abstractmethod
    def process(self, amount): ...

class CreditCardProcessor(PaymentProcessor):
    def process(self, amount):
        return process_credit_card(amount)

class PayPalProcessor(PaymentProcessor):
    def process(self, amount):
        return process_paypal(amount)

def process_payment(processor: PaymentProcessor, amount):
    return processor.process(amount)
```

### 6. Introduce Parameter Object

```python
# ❌ BEFORE: Too many parameters
def create_user(name, email, password, age, address_line1, address_line2, city, state, zip_code):
    pass

# ✅ AFTER: Parameter object
from dataclasses import dataclass

@dataclass
class UserData:
    name: str
    email: str
    password: str
    age: int
    address: str  # Could be another object

def create_user(data: UserData):
    pass
```

### 7. Replace Magic Number with Constant

```python
# ❌ BEFORE: Magic numbers everywhere
def calculate_discount(amount):
    if amount > 1000:
        return amount * 0.15
    elif amount > 500:
        return amount * 0.10
    else:
        return amount * 0.05

# ✅ AFTER: Named constants
LARGE_ORDER_THRESHOLD = 1000
MEDIUM_ORDER_THRESHOLD = 500
LARGE_DISCOUNT = 0.15
MEDIUM_DISCOUNT = 0.10
SMALL_DISCOUNT = 0.05

def calculate_discount(amount):
    if amount > LARGE_ORDER_THRESHOLD:
        return amount * LARGE_DISCOUNT
    elif amount > MEDIUM_ORDER_THRESHOLD:
        return amount * MEDIUM_DISCOUNT
    else:
        return amount * SMALL_DISCOUNT
```

### 8. Decompose Conditional

```python
# ❌ BEFORE: Complex conditional
def check_eligibility(age, income, employed, has_dependents):
    if age >= 18 and income >= 30000 and employed and not has_dependents:
        return "Eligible for loan A"
    elif age >= 21 and income >= 50000 and employed:
        return "Eligible for loan B"
    else:
        return "Not eligible"

# ✅ AFTER: Decomposed into clear functions
def is_adult(age):
    return age >= 18

def has_sufficient_income(income, threshold):
    return income >= threshold

def is_employed(employed):
    return employed

def has_no_dependents(has_dependents):
    return not has_dependents

def check_eligibility(age, income, employed, has_dependents):
    if (is_adult(age) and 
        has_sufficient_income(income, 30000) and 
        is_employed(employed) and 
        has_no_dependents(has_dependents)):
        return "Eligible for loan A"
    
    if (age >= 21 and 
        has_sufficient_income(income, 50000) and 
        is_employed(employed)):
        return "Eligible for loan B"
    
    return "Not eligible"
```

---

## Full Worked Example

### Before: 120-line Messy Script

```python
# ❌ BEFORE: Weather reporter script
import requests
import json
from datetime import datetime

def get_weather():
    # Hard-coded API key (security smell!)
    api_key = "12345abcde"
    
    # Magic numbers and strings
    url = "http://api.openweathermap.org/data/2.5/weather"
    
    # Long function doing multiple things
    response = requests.get(f"{url}?q=London&appid={api_key}&units=metric")
    
    if response.status_code == 200:
        data = response.json()
        
        # Deep nesting
        if "main" in data:
            if "temp" in data["main"]:
                temp = data["main"]["temp"]
                
                if "weather" in data and len(data["weather"]) > 0:
                    desc = data["weather"][0]["description"]
                    
                    # Primitive obsession - using raw strings
                    if desc == "clear sky":
                        emoji = "☀️"
                    elif desc == "clouds":
                        emoji = "☁️"
                    elif desc == "rain":
                        emoji = "🌧️"
                    else:
                        emoji = "🌤️"
                    
                    # Magic number for conversion
                    feels_like = data["main"]["feels_like"]
                    
                    # Duplicate code pattern
                    print("Weather in London:")
                    print(f"Temperature: {temp}°C")
                    print(f"Feels like: {feels_like}°C")
                    print(f"Condition: {desc} {emoji}")
                    
                    # More duplication
                    humidity = data["main"]["humidity"]
                    print(f"Humidity: {humidity}%")
                    
                    pressure = data["main"]["pressure"]
                    print(f"Pressure: {pressure} hPa")
                else:
                    print("No weather data")
            else:
                print("No temperature data")
        else:
            print("No main data")
    else:
        print(f"Error: {response.status_code}")

# Called directly - no structure
if __name__ == "__main__":
    get_weather()
```

### After: Refactored with 5 Techniques

```python
# ✅ AFTER: Clean, testable, maintainable

import requests
from dataclasses import dataclass
from typing import Optional

# ✅ Replace Magic Number with Constant
OPENWEATHER_URL = "http://api.openweathermap.org/data/2.5/weather"
KELVIN_OFFSET = 273.15

# ✅ Extract Class - encapsulate weather data
@dataclass
class WeatherData:
    temperature: float
    feels_like: float
    description: str
    humidity: int
    pressure: int
    
    @property
    def celsius(self) -> float:
        return self.temperature - KELVIN_OFFSET
    
    @property
    def feels_like_celsius(self) -> float:
        return self.feels_like - KELVIN_OFFSET
    
    def get_emoji(self) -> str:
        """Extract Method - moved logic to its own method"""
        weather_map = {
            "clear sky": "☀️",
            "few clouds": "🌤️",
            "scattered clouds": "⛅",
            "broken clouds": "☁️",
            "shower rain": "🌧️",
            "rain": "🌧️",
            "thunderstorm": "⛈️",
            "snow": "❄️",
            "mist": "🌫️"
        }
        return weather_map.get(self.description, "🌤️")

# ✅ Extract Function - separated concerns
def fetch_weather(api_key: str, city: str = "London") -> dict:
    """Fetch raw weather data from API."""
    response = requests.get(
        f"{OPENWEATHER_URL}?q={city}&appid={api_key}&units=metric"
    )
    response.raise_for_status()
    return response.json()

# ✅ Extract Function - transformed data
def parse_weather_data(raw_data: dict) -> Optional[WeatherData]:
    """Parse API response into WeatherData object."""
    try:
        main = raw_data["main"]
        weather = raw_data["weather"][0]
        
        return WeatherData(
            temperature=main["temp"],
            feels_like=main["feels_like"],
            description=weather["description"],
            humidity=main["humidity"],
            pressure=main["pressure"]
        )
    except (KeyError, IndexError):
        return None

# ✅ Extract Function - formatted output
def format_weather_report(weather: WeatherData) -> str:
    """Format weather data for display."""
    lines = [
        f"Weather in London:",
        f"Temperature: {weather.celsius:.1f}°C {weather.get_emoji()}",
        f"Feels like: {weather.feels_like_celsius:.1f}°C",
        f"Condition: {weather.description}",
        f"Humidity: {weather.humidity}%",
        f"Pressure: {weather.pressure} hPa"
    ]
    return "\n".join(lines)

# ✅ Introduce Parameter Object - for configuration
@dataclass
class WeatherConfig:
    api_key: str
    city: str = "London"
    units: str = "metric"

def get_weather_report(config: WeatherConfig) -> str:
    """Main function - orchestrates the process."""
    # Extract
    raw_data = fetch_weather(config.api_key, config.city)
    
    # Transform
    weather = parse_weather_data(raw_data)
    if weather is None:
        return "Failed to parse weather data"
    
    # Load (to console)
    return format_weather_report(weather)

# Usage with dependency injection (easier to test!)
if __name__ == "__main__":
    # In real app, get API key from environment/config
    config = WeatherConfig(api_key="your-api-key-here")
    print(get_weather_report(config))
```

---

## Summary

✅ **Always test first** — write tests before refactoring

✅ **Watch for code smells** — long functions, god classes, duplication

✅ **Use safe techniques** — extract function/class, rename, move method

✅ **Replace conditionals** — with polymorphism when appropriate

✅ **Eliminate magic numbers** — use named constants

✅ **Decompose complex logic** — into clear, named functions

---

## ➡️ Next Steps

Continue to [03_Design_In_Practice/01_designing_a_system_from_scratch.md](../03_Design_In_Practice/01_designing_a_system_from_scratch.md)

---

## 🔗 Further Reading

- [Refactoring: Improving the Design of Existing Code](https://martinfowler.com/books/refactoring.html)
- [Refactoring.Guru](https://refactoring.guru/)
- [Code Complete](https://www.amazon.com/Code-Complete-Practical-Handbook-Construction/dp/0735619670)
