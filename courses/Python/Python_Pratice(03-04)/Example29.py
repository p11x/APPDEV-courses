# Example29.py
# Topic: Return Values - Combined Examples

# This file demonstrates more advanced uses of return values,
# combining basic returns, None returns, multiple values, and Optional types.


from typing import Optional


# Returns True if the number is prime, False otherwise
def is_prime(n: int) -> bool:
    if n < 2:
        return False
    for i in range(2, int(n ** 0.5) + 1):
        if n % i == 0:
            return False
    return True


# Returns the count of prime numbers and a list of primes found
def find_primes(numbers: list[int]) -> tuple[int, list[int]]:
    primes = []
    for num in numbers:
        if is_prime(num):
            primes.append(num)
    return len(primes), primes


count, primes = find_primes([1, 2, 3, 4, 5, 6, 7, 8, 9, 10])
print("Primes found: " + str(count))    # Primes found: 4
print("Primes: " + str(primes))          # Primes: [2, 3, 5, 7]


# Validates input and returns success status with optional error message
def validate_input(username: str, password: str, email: str) -> tuple[bool, Optional[str]]:
    if not username or len(username) < 3:
        return False, "Username must be at least 3 characters"
    if not password or len(password) < 8:
        return False, "Password must be at least 8 characters"
    if "@" not in email or "." not in email:
        return False, "Invalid email format"
    return True, None


success, error = validate_input("alice", "password123", "alice@example.com")
print("Valid: " + str(success) + ", Error: " + str(error))    # Valid: True, Error: None

success, error = validate_input("ab", "pass", "invalid")
print("Valid: " + str(success) + ", Error: " + str(error))    # Valid: False, Error: Username must be at least 3 characters


# Processes a list and returns statistics
def process_numbers(numbers: list[int]) -> tuple[int, int, int, Optional[int]]:
    if not numbers:
        return 0, 0, 0, None
    
    count = len(numbers)
    total = sum(numbers)
    average = total // count
    even_count = len([n for n in numbers if n % 2 == 0])
    
    return count, total, average, even_count


count, total, avg, evens = process_numbers([1, 2, 3, 4, 5, 6, 7, 8, 9, 10])
print("Count: " + str(count) + ", Total: " + str(total) + ", Avg: " + str(avg) + ", Evens: " + str(evens))
# Count: 10, Total: 55, Avg: 5, Evens: 5


# Simulates a login attempt and returns status and user info
def login(username: str, password: str) -> tuple[bool, Optional[dict]]:
    users = {
        "admin": {"password": "admin123", "role": "administrator"},
        "user": {"password": "pass123", "role": "user"}
    }
    
    if username not in users:
        return False, None
    
    if users[username]["password"] != password:
        return False, None
    
    return True, {"username": username, "role": users[username]["role"]}


success, user = login("admin", "admin123")
if success and user is not None:
    print("Logged in as: " + user["username"] + " (" + user["role"] + ")")
    # Logged in as: admin (administrator)

success, user = login("admin", "wrongpass")
print("Login success: " + str(success) + ", User: " + str(user))    # Login success: False, User: None


# Searches for an item and returns found status with index
def search_item(items: list[str], target: str) -> tuple[bool, Optional[int]]:
    for i, item in enumerate(items):
        if item.lower() == target.lower():
            return True, i
    return False, None


found, index = search_item(["Apple", "Banana", "Cherry"], "banana")
print("Found: " + str(found) + ", Index: " + str(index))    # Found: True, Index: 1


# Calculates shipping cost and returns None for invalid destinations
def calculate_shipping(destination: str, weight: float) -> Optional[float]:
    if weight <= 0:
        return None
    
    rates = {
        "US": 5.0,
        "UK": 10.0,
        "CA": 8.0
    }
    
    if destination not in rates:
        return None
    
    return rates[destination] + (weight * 0.5)


cost = calculate_shipping("US", 10.0)
print("Shipping to US: $" + str(cost))      # Shipping to US: $10.0
print("Shipping to XX: " + str(cost))       # Shipping to XX: None


# Parses a date string and returns year, month, day as integers
def parse_date(date_str: str) -> tuple[bool, Optional[tuple[int, int, int]]]:
    parts = date_str.split("-")
    
    if len(parts) != 3:
        return False, None
    
    try:
        year = int(parts[0])
        month = int(parts[1])
        day = int(parts[2])
        
        if month < 1 or month > 12:
            return False, None
        if day < 1 or day > 31:
            return False, None
            
        return True, (year, month, day)
    except ValueError:
        return False, None


success, date = parse_date("2024-03-15")
if success:
    print("Date: " + str(date))    # Date: (2024, 3, 15)

success, date = parse_date("invalid")
print("Success: " + str(success) + ", Date: " + str(date))    # Success: False, Date: None


# Returns the result of a mathematical operation
def calculate(operation: str, a: float, b: float) -> tuple[bool, Optional[float]]:
    if operation == "add":
        return True, a + b
    elif operation == "subtract":
        return True, a - b
    elif operation == "multiply":
        return True, a * b
    elif operation == "divide":
        if b == 0:
            return False, None
        return True, a / b
    else:
        return False, None


success, result = calculate("add", 10, 5)
print("10 + 5 = " + str(result))    # 10 + 5 = 15.0

success, result = calculate("divide", 10, 0)
print("Success: " + str(success) + ", Result: " + str(result))    # Success: False, Result: None


# Filters a list of strings by minimum length
def filter_by_length(items: list[str], min_length: int) -> tuple[list[str], list[str]]:
    matching = []
    non_matching = []
    
    for item in items:
        if len(item) >= min_length:
            matching.append(item)
        else:
            non_matching.append(item)
    
    return matching, non_matching


passed, failed = filter_by_length(["cat", "elephant", "dog", "tiger"], 5)
print("Long enough: " + str(passed))    # Long enough: ['elephant', 'tiger']
print("Too short: " + str(failed))      # Too short: ['cat', 'dog']


# Real-life Example 1: Process order and calculate totals
def process_order(items: list[dict], tax_rate: float, shipping_required: bool) -> tuple[bool, Optional[dict], str]:
    if not items:
        return False, None, "Cart is empty"
    
    subtotal = sum(item["price"] * item["quantity"] for item in items)
    tax = subtotal * tax_rate
    
    shipping_cost = 0.0
    if shipping_required:
        total_weight = sum(item.get("weight", 1) * item["quantity"] for item in items)
        shipping_cost = 5.99 + (total_weight * 0.50)
    
    total = subtotal + tax + shipping_cost
    
    return True, {
        "items": len(items),
        "subtotal": round(subtotal, 2),
        "tax": round(tax, 2),
        "shipping": round(shipping_cost, 2),
        "total": round(total, 2)
    }, "Order processed"

order_items = [
    {"name": "Widget", "price": 19.99, "quantity": 2, "weight": 0.5},
    {"name": "Gadget", "price": 49.99, "quantity": 1, "weight": 1.0}
]

success, order, msg = process_order(order_items, 0.08, True)
if success and order is not None:
    print(f"Order: {order['items']} items, Total: ${order['total']}")
    # Order: 2 items, Total: $108.45
else:
    print(f"Error: {msg}")

empty_order = []
success, order, msg = process_order(empty_order, 0.08, True)
print(f"Success: {success}, Message: {msg}")    # Success: False, Message: Cart is empty


# Real-life Example 2: Analyze employee performance
def analyze_performance(employees: list[dict]) -> tuple[bool, Optional[dict], str]:
    if not employees:
        return False, None, "No employee data"
    
    ratings = [e["rating"] for e in employees if "rating" in e]
    if not ratings:
        return False, None, "No ratings available"
    
    avg_rating = sum(ratings) / len(ratings)
    top_performers = [e["name"] for e in employees if e.get("rating", 0) >= 4.5]
    needs_improvement = [e["name"] for e in employees if e.get("rating", 0) < 3.0]
    
    return True, {
        "total_employees": len(employees),
        "average_rating": round(avg_rating, 2),
        "top_performers": top_performers,
        "needs_improvement": needs_improvement
    }, "Analysis complete"

staff = [
    {"name": "Alice", "rating": 4.8, "department": "Sales"},
    {"name": "Bob", "rating": 3.2, "department": "Sales"},
    {"name": "Carol", "rating": 4.9, "department": "Engineering"},
    {"name": "David", "rating": 2.8, "department": "Marketing"},
    {"name": "Eve", "rating": 4.5, "department": "Engineering"}
]

success, analysis, msg = analyze_performance(staff)
if success and analysis is not None:
    print(f"Team Analysis: {analysis['total_employees']} employees, Avg Rating: {analysis['average_rating']}")
    print(f"Top Performers: {', '.join(analysis['top_performers'])}")
    # Team Analysis: 5 employees, Avg Rating: 3.84
    # Top Performers: Alice, Carol, Eve


# Real-life Example 3: Calculate ROI (Return on Investment)
def calculate_roi(initial_investment: float, final_value: float, years: float) -> tuple[bool, Optional[dict], str]:
    if initial_investment <= 0:
        return False, None, "Initial investment must be positive"
    if final_value < 0:
        return False, None, "Final value cannot be negative"
    if years <= 0:
        return False, None, "Investment period must be positive"
    
    total_return = ((final_value - initial_investment) / initial_investment) * 100
    annual_return = (((final_value / initial_investment) ** (1 / years)) - 1) * 100
    profit = final_value - initial_investment
    
    return True, {
        "initial_investment": initial_investment,
        "final_value": final_value,
        "profit": round(profit, 2),
        "total_return_pct": round(total_return, 2),
        "annual_return_pct": round(annual_return, 2),
        "years": years
    }, "ROI calculated"

success, roi, msg = calculate_roi(10000, 15000, 2)
if success and roi is not None:
    print(f"Investment: ${roi['initial_investment']} -> ${roi['final_value']}")
    print(f"Profit: ${roi['profit']}, Total Return: {roi['total_return_pct']}%")
    print(f"Annual Return: {roi['annual_return_pct']}%")
    # Investment: $10000 -> $15000
    # Profit: $5000.0, Total Return: 50.0%
    # Annual Return: 22.47%


# Real-life Example 4: Validate and process loan application
def evaluate_loan_application(income: float, credit_score: int, loan_amount: float, term_years: int) -> tuple[bool, Optional[dict], str]:
    if income <= 0:
        return False, None, "Income must be positive"
    if loan_amount <= 0:
        return False, None, "Loan amount must be positive"
    if term_years not in [5, 10, 15, 20, 30]:
        return False, None, "Invalid loan term"
    
    if credit_score < 580:
        return False, None, "Credit score too low (minimum 580)"
    
    if credit_score >= 740:
        rate = 0.065
    elif credit_score >= 670:
        rate = 0.075
    else:
        rate = 0.095
    
    monthly_rate = rate / 12
    num_payments = term_years * 12
    monthly_payment = loan_amount * (monthly_rate * (1 + monthly_rate) ** num_payments) / ((1 + monthly_rate) ** num_payments - 1)
    
    debt_to_income = (monthly_payment * 12) / income
    if debt_to_income > 0.43:
        return False, None, "Debt-to-income ratio too high"
    
    return True, {
        "loan_amount": loan_amount,
        "interest_rate": rate,
        "term_years": term_years,
        "monthly_payment": round(monthly_payment, 2),
        "total_interest": round(monthly_payment * num_payments - loan_amount, 2),
        "debt_to_income": round(debt_to_income * 100, 1)
    }, "Loan approved"

success, loan, msg = evaluate_loan_application(75000, 750, 250000, 30)
if success and loan is not None:
    print(f"Loan Approved: ${loan['loan_amount']} at {loan['interest_rate']*100}%")
    print(f"Monthly Payment: ${loan['monthly_payment']}, Total Interest: ${loan['total_interest']}")
    # Loan Approved: $250000 at 6.5%
    # Monthly Payment: $1580.17, Total Interest: $318861.2

success, loan, msg = evaluate_loan_application(30000, 500, 50000, 15)
print(f"Result: {msg}")    # Result: Credit score too low (minimum 580)
