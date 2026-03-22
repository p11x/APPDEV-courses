# Example25.py
# Topic: Return Values - Returning None

# This file demonstrates functions that return None in Python.
# Functions without an explicit return statement or with empty return
# statements return None by default.


# Prints a message to the console and returns None
def log_message(message: str) -> None:
    print("LOG: " + message)

result = log_message("Application started")    # None
print("log_message returned: " + str(result))    # log_message returned: None


# Doubles all elements in a list in place and returns None
def double_values(numbers: list[int]) -> None:
    for i in range(len(numbers)):
        numbers[i] = numbers[i] * 2

my_list = [1, 2, 3, 4, 5]
double_values(my_list)
print("Doubled list: " + str(my_list))    # Doubled list: [2, 4, 6, 8, 10]


# Updates a dictionary with new key-value pairs and returns None
def update_dict(data: dict, key: str, value: str) -> None:
    data[key] = value

user_info = {"name": "Alice"}
update_dict(user_info, "age", "30")
print("Updated dict: " + str(user_info))    # Updated dict: {'name': 'Alice', 'age': '30'}


# Prints a greeting but returns None (uses empty return)
def greet_user(name: str) -> None:
    print("Welcome, " + name + "!")
    return

result = greet_user("Bob")    # None
print("greet_user returned: " + str(result))    # greet_user returned: None


# Removes negative numbers from a list and returns None
def remove_negatives(numbers: list[int]) -> None:
    numbers[:] = [x for x in numbers if x >= 0]

values = [1, -2, 3, -4, 5, -6]
remove_negatives(values)
print("After removing negatives: " + str(values))    # After removing negatives: [1, 3, 5]


# Appends items to a list and returns None
def add_items(lst: list[str], *items: str) -> None:
    for item in items:
        lst.append(item)

fruits = ["apple"]
add_items(fruits, "banana", "cherry", "date")
print("Fruits list: " + str(fruits))    # Fruits list: ['apple', 'banana', 'cherry', 'date']


# Clears a list and returns None
def clear_list(lst: list) -> None:
    lst.clear()

my_list = [1, 2, 3, 4, 5]
clear_list(my_list)
print("Cleared list: " + str(my_list))    # Cleared list: []


# Prints the contents of a list and returns None
def display_items(items: list[str]) -> None:
    for item in items:
        print("- " + item)

print("Shopping list:")
display_items(["milk", "bread", "eggs"])


# Capitalizes all strings in a list in place and returns None
def capitalize_all(words: list[str]) -> None:
    for i in range(len(words)):
        words[i] = words[i].capitalize()

names = ["alice", "bob", "charlie"]
capitalize_all(names)
print("Capitalized: " + str(names))    # Capitalized: ['Alice', 'Bob', 'Charlie']


# Returns None implicitly (no return statement)
def process(data: list[int]) -> None:
    total = sum(data)
    print("Sum: " + str(total))

result = process([1, 2, 3, 4, 5])    # Sum: 15
print("process returned: " + str(result))    # process returned: None


# Prints a separator line and returns None
def print_separator() -> None:
    print("-" * 30)

print("Before separator")
print_separator()
print("After separator")


# Sets an attribute on an object and returns None
class Person:
    name: str
    age: int
    
    def __init__(self):
        self.name = ""
        self.age = 0

def set_attributes(person: Person, name: str, age: int) -> None:
    person.name = name
    person.age = age

p = Person()
set_attributes(p, "Eve", 25)
print("Person: " + p.name + ", age " + str(p.age))    # Person: Eve, age 25


# Real-life Example 1: Send email notification (side effect only)
def send_email_notification(recipient: str, subject: str, body: str) -> None:
    print("Email sent to: " + recipient)
    print("Subject: " + subject)
    print("Body: " + body)

result = send_email_notification("user@example.com", "Welcome!", "Your account is ready.")
print("send_email returned: " + str(result))    # send_email returned: None


# Real-life Example 2: Update user profile in database (simulated)
def update_user_profile(user_id: int, email: str, phone: str) -> None:
    print("Updating user " + str(user_id) + " profile...")
    print("  Email: " + email)
    print("  Phone: " + phone)
    print("Profile updated successfully!")

result = update_user_profile(123, "newemail@example.com", "555-1234")
print("update_user_profile returned: " + str(result))    # None


# Real-life Example 3: Sort a list of products by price (in place)
def sort_products_by_price(products: list[dict]) -> None:
    products.sort(key=lambda p: p["price"])

products = [
    {"name": "Laptop", "price": 999.99},
    {"name": "Phone", "price": 599.99},
    {"name": "Tablet", "price": 399.99}
]
sort_products_by_price(products)
print("Sorted products:")
for p in products:
    print("  " + p["name"] + ": $" + str(p["price"]))
# Laptop: $999.99, Phone: $599.99, Tablet: $399.99


# Real-life Example 4: Record a transaction in a ledger (simulated)
def record_transaction(transactions: list[dict], date: str, description: str, amount: float) -> None:
    transactions.append({
        "date": date,
        "description": description,
        "amount": amount
    })

ledger = []
record_transaction(ledger, "2024-01-15", "Salary Deposit", 5000.00)
record_transaction(ledger, "2024-01-16", "Grocery Store", -150.50)
record_transaction(ledger, "2024-01-17", "Electric Bill", -85.00)
print("Transaction ledger:")
for t in ledger:
    print("  " + t["date"] + ": " + t["description"] + " $" + str(t["amount"]))


# Real-life Example 5: Normalize a list of email addresses to lowercase
def normalize_emails(emails: list[str]) -> None:
    for i in range(len(emails)):
        emails[i] = emails[i].strip().lower()

emails = ["JOHN@EXAMPLE.COM", "  Alice@Example.com  ", "BOB@EXAMPLE.COM"]
normalize_emails(emails)
print("Normalized emails: " + str(emails))    # ['john@example.com', 'alice@example.com', 'bob@example.com']


# Real-life Example 6: Flush print queue (simulated)
def flush_print_queue(print_jobs: list[str]) -> None:
    while print_jobs:
        job = print_jobs.pop(0)
        print("Printing: " + job)
    print("Print queue empty!")

queue = ["Document1.pdf", "Report.xlsx", "Image.png"]
flush_print_queue(queue)
print("Queue after flush: " + str(queue))    # []
