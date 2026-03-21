# Example20.py
# Topic: Complex Type Examples - Lists and Dictionaries

# This file demonstrates complex type hints for lists and dictionaries.
# Python 3.9+ allows using built-in types directly as type hints.


# Function with list type hint (Python 3.9+)
# list[int] indicates a list of integers
def sum_list(numbers: list[int]) -> int:
    total = 0    # int — running sum
    for num in numbers:
        total = total + num
    return total

result = sum_list([1, 2, 3, 4, 5])    # int — sum of 15
print("Sum: " + str(result))    # Sum: 15


# Function with list of strings
def join_words(words: list[str]) -> str:
    result = ""    # str — accumulator for joined words
    for word in words:
        result = result + word + " "
    return result.strip()

words = ["Hello", "World", "Python"]    # list — words to join
joined = join_words(words)    # str — "Hello World Python"
print("Joined: " + joined)    # Joined: Hello World Python


# Function returning list of floats
def get_prices() -> list[float]:
    return [19.99, 29.99, 39.99, 49.99]

prices = get_prices()    # list — [19.99, 29.99, 39.99, 49.99]
print("Prices: " + str(prices))    # Prices: [19.99, 29.99, 39.99, 49.99]


# Function with dict type hint
# dict[str, int] means keys are strings, values are integers
def count_letters(text: str) -> dict[str, int]:
    counts = {}    # dict — letter frequency counter
    for char in text:
        if char.isalpha():
            char_lower = char.lower()
            if char_lower in counts:
                counts[char_lower] = counts[char_lower] + 1
            else:
                counts[char_lower] = 1
    return counts

result = count_letters("Hello World")    # dict — letter counts
print("Counts: " + str(result))    # Counts: {'h': 2, 'e': 1, 'l': 3, 'o': 2, 'w': 1, 'r': 1, 'd': 1}


# Function with dict[str, str] - both keys and values are strings
def create_user_dict(name: str, email: str, city: str) -> dict[str, str]:
    return {
        "name": name,
        "email": email,
        "city": city
    }

user = create_user_dict("Alice", "alice@example.com", "NYC")    # dict — user data
print("User: " + str(user))    # User: {'name': 'Alice', 'email': 'alice@example.com', 'city': 'NYC'}


# Function with dict return type - flexible values
def get_config() -> dict:
    return {
        "host": "localhost",
        "port": 8080,
        "debug": True,
        "timeout": 30
    }

config = get_config()    # dict — configuration dictionary
print("Config: " + str(config))    # Config: {'host': 'localhost', 'port': 8080, 'debug': True, 'timeout': 30}


# Function with nested list type - list of lists
# list[list[int]] means a 2D matrix of integers
def sum_matrix(matrix: list[list[int]]) -> int:
    total = 0    # int — grand total
    for row in matrix:
        for num in row:
            total = total + num
    return total

matrix = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
]
result = sum_matrix(matrix)    # int — sum of all 45
print("Matrix sum: " + str(result))    # Matrix sum: 45


# Function with dict parameter
def find_max_in_dict(scores: dict[str, int]) -> str:
    if not scores:
        return "No scores"
    max_name = ""    # str — name of person with highest score
    max_score = 0    # int — highest score
    for name, score in scores.items():
        if score > max_score:
            max_score = score
            max_name = name
    return max_name + ": " + str(max_score)

scores = {"Alice": 95, "Bob": 87, "Charlie": 92}    # dict — name-score pairs
winner = find_max_in_dict(scores)    # str — "Alice: 95"
print("Winner: " + winner)    # Winner: Alice: 95


# Function with list of dicts
def find_adults(users: list[dict]) -> list[dict]:
    adults = []    # list — filtered list of adult users
    for user in users:
        if user.get("age", 0) >= 18:
            adults.append(user)
    return adults

users = [
    {"name": "Alice", "age": 25},
    {"name": "Bob", "age": 15},
    {"name": "Charlie", "age": 30}
]
adults = find_adults(users)    # list — adults only
print("Adults: " + str(adults))    # Adults: [{'name': 'Alice', 'age': 25}, {'name': 'Charlie', 'age': 30}]


# Function with dict parameter and list return
def get_values_above(data: dict[str, int], threshold: int) -> list:
    result = []    # list — keys with values above threshold
    for key, value in data.items():
        if value > threshold:
            result.append(key)
    return result

data = {"a": 10, "b": 5, "c": 15, "d": 8}
above = get_values_above(data, 7)    # list — ['a', 'c', 'd']
print("Above 7: " + str(above))    # Above 7: ['a', 'c', 'd']


# Complex: list of dictionaries with specific keys
def process_products(products: list[dict]) -> list[dict]:
    discounted = []    # list — products with 10% discount
    for product in products:
        discounted_price = product["price"] * 0.9    # float — 90% of original
        discounted.append({
            "name": product["name"],
            "price": round(discounted_price, 2)
        })
    return discounted

products = [
    {"name": "Laptop", "price": 1000.0},
    {"name": "Phone", "price": 500.0}
]
result = process_products(products)    # list — discounted products
print("Discounted: " + str(result))    # Discounted: [{'name': 'Laptop', 'price': 900.0}, {'name': 'Phone', 'price': 450.0}]
