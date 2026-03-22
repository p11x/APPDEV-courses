# Example46.py
# Topic: Lambda Functions - Basic Syntax and Usage

# This file demonstrates the basics of lambda functions in Python,
# including syntax, basic operations, and comparison with regular functions.


# Basic lambda syntax
add = lambda a, b: a + b
print("=== Basic Lambda ===")
print(f"add(5, 3) = {add(5, 3)}")  # 8


# Lambda with no parameters
get_random = lambda: 42
print(f"get_random() = {get_random()}")  # 42


# Lambda with single parameter
square = lambda x: x ** 2
print(f"square(5) = {square(5)}")  # 25


# Lambda returning a tuple
get_stats = lambda a, b: (a + b, a - b, a * b)
sum_val, diff, prod = get_stats(10, 5)
print(f"Stats: sum={sum_val}, diff={diff}, prod={prod}")
# Stats: sum=15, diff=5, prod=50


# Lambda with conditional expression (ternary)
abs_value = lambda x: x if x >= 0 else -x
print(f"abs_value(-10) = {abs_value(-10)}")  # 10
print(f"abs_value(10) = {abs_value(10)}")  # 10


# Lambda vs Regular Function Comparison
def multiply_regular(a, b):
    return a * b

multiply_lambda = lambda a, b: a * b

print("\n=== Lambda vs Regular Function ===")
print(f"Regular function: multiply_regular(4, 5) = {multiply_regular(4, 5)}")
print(f"Lambda function: multiply_lambda(4, 5) = {multiply_lambda(4, 5)}")


# Lambda with default arguments
power = lambda x, n=2: x ** n
print(f"\npower(3) = {power(3)}")  # 9
print(f"power(3, 3) = {power(3, 3)}")  # 27


# Lambda with *args (limited)
variadic_sum = lambda *args: sum(args)
print(f"variadic_sum(1, 2, 3, 4, 5) = {variadic_sum(1, 2, 3, 4, 5)}")  # 15


# Lambda with filter() - keep even numbers
numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
print("\n=== Lambda with filter() ===")
evens = list(filter(lambda x: x % 2 == 0, numbers))
print(f"Even numbers: {evens}")  # [2, 4, 6, 8, 10]


# Lambda with filter() - keep odd numbers
odds = list(filter(lambda x: x % 2 != 0, numbers))
print(f"Odd numbers: {odds}")  # [1, 3, 5, 7, 9]


# Lambda with filter() - filter by length
words = ["cat", "elephant", "dog", "hippopotamus", "rat"]
long_words = list(filter(lambda w: len(w) > 4, words))
print(f"Words longer than 4 chars: {long_words}")  # ['elephant', 'hippopotamus']


# Lambda with map() - double all numbers
print("\n=== Lambda with map() ===")
doubled = list(map(lambda x: x * 2, numbers))
print(f"Doubled: {doubled}")  # [2, 4, 6, 8, 10, 12, 14, 16, 18, 20]


# Lambda with map() - convert to strings
nums = [1, 2, 3, 4, 5]
as_strings = list(map(lambda x: str(x), nums))
print(f"As strings: {as_strings}")  # ['1', '2', '3', '4', '5']


# Lambda with map() - add prefix
names = ["Alice", "Bob", "Charlie"]
greeted = list(map(lambda name: f"Hello, {name}!", names))
print(f"Greetings: {greeted}")  # ['Hello, Alice!', 'Hello, Bob!', 'Hello, Charlie!']


# Lambda with sorted() - sort by length
print("\n=== Lambda with sorted() ===")
words = ["cat", "elephant", "dog", "bird", "hippopotamus"]
sorted_by_length = sorted(words, key=lambda w: len(w))
print(f"Sorted by length: {sorted_by_length}")
# ['cat', 'dog', 'bird', 'elephant', 'hippopotamus']


# Lambda with sorted() - sort by last character
sorted_by_last = sorted(words, key=lambda w: w[-1])
print(f"Sorted by last char: {sorted_by_last}")


# Lambda with sorted() - reverse sort
sorted_reverse = sorted(words, key=lambda w: len(w), reverse=True)
print(f"Reverse sorted: {sorted_reverse}")


# Lambda with max() - find longest word
print("\n=== Lambda with max() ===")
words = ["cat", "elephant", "dog", "bird"]
longest = max(words, key=lambda w: len(w))
print(f"Longest word: {longest}")  # elephant


# Lambda with min() - find shortest word
shortest = min(words, key=lambda w: len(w))
print(f"Shortest word: {shortest}")  # cat


# Lambda with max() - find highest value
data = [{"name": "A", "score": 85}, {"name": "B", "score": 92}, {"name": "C", "score": 78}]
top_scorer = max(data, key=lambda x: x["score"])
print(f"Top scorer: {top_scorer['name']} with {top_scorer['score']} points")


# Real-life Example 1: Price calculator with discounts
prices = [100, 250, 50, 500, 75]
discount_rate = 0.15

print("\n=== Real-life: Price Calculator ===")
discounted_prices = list(map(lambda p: round(p * (1 - discount_rate), 2), prices))
print(f"Original: {prices}")
print(f"After 15% discount: {discounted_prices}")


# Real-life Example 2: Filter active users
users = [
    {"id": 1, "name": "Alice", "active": True},
    {"id": 2, "name": "Bob", "active": False},
    {"id": 3, "name": "Charlie", "active": True},
    {"id": 4, "name": "Diana", "active": False}
]

print("\n=== Real-life: Filter Active Users ===")
active_users = list(filter(lambda u: u["active"], users))
print(f"Active users: {[u['name'] for u in active_users]}")


# Real-life Example 3: Sort employees by salary
employees = [
    {"name": "Alice", "salary": 75000},
    {"name": "Bob", "salary": 55000},
    {"name": "Charlie", "salary": 82000},
    {"name": "Diana", "salary": 60000}
]

print("\n=== Real-life: Sort Employees by Salary ===")
sorted_employees = sorted(employees, key=lambda e: e["salary"], reverse=True)
for emp in sorted_employees:
    print(f"  {emp['name']}: ${emp['salary']}")


# Real-life Example 4: Process order totals
orders = [
    {"id": 1, "subtotal": 100.00, "tax": 8.00},
    {"id": 2, "subtotal": 250.50, "tax": 20.04},
    {"id": 3, "subtotal": 75.25, "tax": 6.02}
]

print("\n=== Real-life: Calculate Order Totals ===")
totals = list(map(lambda o: {"id": o["id"], "total": round(o["subtotal"] + o["tax"], 2)}, orders))
for order in totals:
    print(f"  Order {order['id']}: ${order['total']}")


# Real-life Example 5: Filter products by category
products = [
    {"name": "Laptop", "category": "electronics", "price": 999},
    {"name": "Apple", "category": "food", "price": 2},
    {"name": "Shirt", "category": "clothing", "price": 29},
    {"name": "Phone", "category": "electronics", "price": 699},
    {"name": "Bread", "category": "food", "price": 3}
]

print("\n=== Real-life: Filter by Category ===")
electronics = list(filter(lambda p: p["category"] == "electronics", products))
food = list(filter(lambda p: p["category"] == "food", products))
print(f"Electronics: {[p['name'] for p in electronics]}")
print(f"Food: {[p['name'] for p in food]}")


# Real-life Example 6: Find highest-rated movie
movies = [
    {"title": "The Matrix", "rating": 8.7, "year": 1999},
    {"title": "Inception", "rating": 8.8, "year": 2010},
    {"title": "Interstellar", "rating": 8.6, "year": 2014},
    {"title": "Tenet", "rating": 7.5, "year": 2020}
]

print("\n=== Real-life: Find Highest-Rated Movie ===")
best_movie = max(movies, key=lambda m: m["rating"])
print(f"Best movie: {best_movie['title']} ({best_movie['rating']})")
