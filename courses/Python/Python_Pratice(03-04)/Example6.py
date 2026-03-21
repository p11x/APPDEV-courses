# Example6.py
# Topic: Keyword Arguments

# This file demonstrates how to use keyword arguments in function calls.
# Keyword arguments let you specify which parameter each value goes to,
# making code more readable and allowing arguments in any order.


# Takes greeting and name as separate parameters, prints combined message
def greet(greeting: str, name: str) -> None:
    print(greeting + ", " + name + "!")

# Different ways to call greet() - positional and keyword arguments mixed
greet("Hello", "Alice")                      # positional: greeting, then name
greet(name="Bob", greeting="Hi")            # keywords: order doesn't matter
greet("Hey", name="Charlie")                 # mixed: positional then keyword


# Creates a user description with name, age, and city parameters
def create_user(name: str, age: int, city: str) -> str:
    return name + " is " + str(age) + " years old and lives in " + city

# Call with all positional arguments
user1 = create_user("Alice", 25, "NYC")
print(user1)    # Alice is 25 years old and lives in NYC

# Call with all keyword arguments (any order)
user2 = create_user(name="Bob", age=30, city="LA")
print(user2)    # Bob is 30 years old and lives in LA

# Call with mixed positional and keyword arguments
user3 = create_user("Charlie", city="Chicago", age=35)
print(user3)    # Charlie is 35 years old and lives in Chicago

# Call with all keyword arguments in different order
user4 = create_user(age=28, name="Diana", city="Seattle")
print(user4)    # Diana is 28 years old and lives in Seattle


# Performs basic arithmetic operations based on the operation parameter
def calculate(a: float, b: float, operation: str) -> float:
    if operation == "add":
        return a + b
    elif operation == "subtract":
        return a - b
    elif operation == "multiply":
        return a * b
    elif operation == "divide":
        return a / b
    return 0.0

# Call calculate() with different combinations of keyword arguments
result1 = calculate(10, 5, "add")                        # all positional
print("Add: " + str(result1))                           # Add: 15

result2 = calculate(a=10, b=5, operation="subtract")   # all keywords
print("Subtract: " + str(result2))                     # Subtract: 5

result3 = calculate(b=2, a=10, operation="multiply")    # keywords in any order
print("Multiply: " + str(result3))                    # Multiply: 20

result4 = calculate(operation="divide", a=100, b=20)   # operation specified by keyword
print("Divide: " + str(result4))                      # Divide: 5.0


# Describes a pet using its type, name, and age
def describe_pet(pet_type: str, pet_name: str, age: int) -> None:
    print(pet_name + " is a " + pet_type + " that is " + str(age) + " years old")

# Different ways to pass arguments to describe_pet()
describe_pet("dog", "Buddy", 3)                                   # all positional
describe_pet(pet_type="cat", pet_name="Whiskers", age=5)        # all keywords
describe_pet("parrot", age=2, pet_name="Polly")                 # mixed style


# Builds a database connection string with optional host and port
def configure(database: str, host: str = "localhost", port: int = 5432) -> str:
    return database + "://" + host + ":" + str(port)

config1 = configure("postgresql")                              # uses defaults
print(config1)                                               # postgresql://localhost:5432

config2 = configure("mysql", host="db.example.com")          # custom host
print(config2)                                               # mysql://db.example.com:5432

config3 = configure("redis", port=6379, host="cache.example.com")    # custom port and host
print(config3)                                               # redis://cache.example.com:6379


# Sends an email with from, to, subject, and body fields
def send_email(to: str, subject: str, body: str, from_addr: str = "noreply@company.com") -> None:
    print("From: " + from_addr)
    print("To: " + to)
    print("Subject: " + subject)
    print("Body: " + body)

# Various ways to call send_email() with keyword arguments
send_email("user@example.com", "Hello", "Welcome!")                          # positional + default
send_email(to="admin@example.com", subject="Alert", body="System error")    # keywords
send_email(body="Meeting reminder", to="team@example.com", subject="Reminder")    # all keywords any order


# Calculates final price with optional tax and discount rates
def calculate_total(price: float, tax: float = 0.1, discount: float = 0.0) -> float:
    return price * (1 + tax - discount)

# Using keyword arguments to specify only the parameters we need
total1 = calculate_total(100.0)                              # all defaults
print("Total: $" + str(total1))                            # Total: $110.0

total2 = calculate_total(100.0, tax=0.15)                  # custom tax rate
print("Total with tax: $" + str(total2))                  # Total with tax: $115.0

total3 = calculate_total(100.0, discount=0.2, tax=0.08)   # custom discount and tax (any order)
print("Total with discount: $" + str(total3))            # Total with discount: $88.0

total4 = calculate_total(tax=0.05, price=200.0, discount=0.1)    # all keywords, any order
print("Total custom: $" + str(total4))                    # Total custom: $190.0


# Creates a product dictionary with name, price, category, and stock status
def create_product(name: str, price: float, category: str = "General", in_stock: bool = True) -> dict:
    return {
        "name": name,
        "price": price,
        "category": category,
        "in_stock": in_stock
    }

product1 = create_product("Laptop", 999.99)                         # uses defaults
print(str(product1))                                              # {'name': 'Laptop', 'price': 999.99, ...}

product2 = create_product("Mouse", 29.99, category="Electronics")  # custom category
print(str(product2))                                             # {'name': 'Mouse', 'category': 'Electronics', ...}

product3 = create_product("Book", 19.99, in_stock=False)          # custom stock status
print(str(product3))                                             # {'name': 'Book', 'in_stock': False, ...}

product4 = create_product(category="Clothing", price=49.99, name="Shirt")    # all keywords
print(str(product4))                                             # {'name': 'Shirt', 'category': 'Clothing', ...}


# Builds a URL from base, path, query string, and fragment
def build_url(base: str, path: str = "/", query: str = "", fragment: str = "") -> str:
    url = base + path    # str — start with base and path
    if query:
        url = url + "?" + query
    if fragment:
        url = url + "#" + fragment
    return url

url1 = build_url("https://example.com")                      # base only
print(url1)                                                  # https://example.com/

url2 = build_url("https://example.com", path="/about")        # with path
print(url2)                                                 # https://example.com/about

url3 = build_url("https://example.com", path="/search", query="q=python")    # with query
print(url3)                                                 # https://example.com/search?q=python

url4 = build_url(path="/docs", base="https://api.example.com", fragment="intro")    # keywords any order
print(url4)                                                 # https://api.example.com/docs#intro


# Processes an order and returns an order summary string
def process_order(customer: str, item: str, quantity: int = 1, express: bool = False) -> str:
    shipping = "Express" if express else "Standard"    # str — shipping type based on flag
    return customer + " ordered " + str(quantity) + " " + item + "(s). Shipping: " + shipping

order1 = process_order("Alice", "Laptop")                 # defaults: qty=1, not express
print(order1)                                            # Alice ordered 1 Laptop(s). Shipping: Standard

order2 = process_order("Bob", "Mouse", quantity=2)        # custom quantity
print(order2)                                            # Bob ordered 2 Mouse(s). Shipping: Standard

order3 = process_order("Charlie", "Phone", express=True)    # express shipping
print(order3)                                            # Charlie ordered 1 Phone(s). Shipping: Express

order4 = process_order(express=True, customer="Diana", item="Tablet", quantity=1)    # all keywords
print(order4)                                            # Diana ordered 1 Tablet(s). Shipping: Express


# Formats a full name with optional middle name and suffix
def format_name(first: str, last: str, middle: str = "", suffix: str = "") -> str:
    name = first    # str — start with first name
    if middle:
        name = name + " " + middle
    name = name + " " + last
    if suffix:
        name = name + " " + suffix
    return name

formatted1 = format_name("John", "Doe")                              # first and last only
print(formatted1)                                                   # John Doe

formatted2 = format_name("John", "Kennedy", middle="F.")            # with middle name
print(formatted2)                                                  # John F. Kennedy

formatted3 = format_name("Robert", "Smith", suffix="Jr.")          # with suffix
print(formatted3)                                                  # Robert Smith Jr.

formatted4 = format_name("George", "Bush", middle="W.", suffix="Bush")    # full name with title
print(formatted4)                                                  # George W. Bush


# Adds three subject scores together to get a total score
def get_score(math: int, science: int, english: int) -> int:
    return math + science + english

# Different ways to call get_score() with keyword arguments
total_score = get_score(90, 85, 88)                    # all positional
print("Total: " + str(total_score))                   # Total: 263

total_score2 = get_score(math=95, science=90, english=85)    # all keywords
print("Total with keywords: " + str(total_score2))    # Total with keywords: 270

total_score3 = get_score(80, english=90, science=85)    # mixed positional and keywords
print("Mixed: " + str(total_score3))                  # Mixed: 255
