# Example15.py
# Topic: Type Hints — Container Types (list, dict, set, tuple)

# Container types let you hint what's INSIDE a collection
# Python 3.9+ lets you write list[str] instead of List[str] from typing
# Syntax: variable = value  # type[inner_type] — description

# --- List type hints ---
# list[T] means every item in the list is of type T

names = ["Alice", "Bob", "Charlie"]   # list[str]  — names as strings
ages = [25, 30, 35]                   # list[int]  — ages as whole numbers
prices = [9.99, 19.99, 29.99]        # list[float] — prices with decimals
mixed = [1, "hello", True]            # list (untyped) — mixed types, no hint needed

print(names)    # ['Alice', 'Bob', 'Charlie']
print(ages)     # [25, 30, 35]
print(prices)   # [9.99, 19.99, 29.99]
print(mixed)    # [1, 'hello', True]

# An empty list still gets a hint so your editor knows what will go in it
empty_list = []   # list[str] — will hold strings once populated
print(empty_list) # []


# --- Dict type hints ---
# dict[K, V] means keys are type K and values are type V

user = {"Alice": 25, "Bob": 30}                    # dict[str, int]   — name → age
scores = {"Math": 95.5, "Science": 88.0}           # dict[str, float] — subject → score
config = {"host": "localhost", "port": "8080"}     # dict[str, str]   — setting → value

print(user)    # {'Alice': 25, 'Bob': 30}
print(scores)  # {'Math': 95.5, 'Science': 88.0}
print(config)  # {'host': 'localhost', 'port': '8080'}

# Access a value by its key using square brackets
alice_age = user["Alice"]   # int — looks up Alice's age
print(alice_age)            # 25


# --- Set type hints ---
# set[T] means every element is of type T
# Sets automatically remove duplicates and have no guaranteed order

unique_ids = {1, 2, 3, 4, 5}                    # set[int] — unique whole numbers
tags = {"python", "coding", "tutorial"}          # set[str] — unique tag strings
colors = {"red", "green", "blue"}               # set (untyped) — mixed use

print(unique_ids)  # {1, 2, 3, 4, 5}
print(tags)        # {'python', 'coding', 'tutorial'}
print(colors)      # {'red', 'green', 'blue'}


# --- Tuple type hints ---
# tuple[T1, T2, ...] — fixed length, each position has its own type
# Unlike lists, tuples cannot be changed after creation

point = (10, 20)              # tuple[int, int]         — x, y coordinates
rgb = (255, 128, 0)           # tuple[int, int, int]    — red, green, blue
person = ("Alice", 25, 5.6)  # tuple[str, int, float]  — name, age, height

print(point)   # (10, 20)
print(rgb)     # (255, 128, 0)
print(person)  # ('Alice', 25, 5.6)

# Access tuple items by index, just like a list
x = point[0]   # int — the x coordinate
y = point[1]   # int — the y coordinate
print(x)       # 10
print(y)       # 20


# --- Nested containers ---
# You can nest container hints inside each other for complex structures

# A dict where each key maps to a list of strings
users_by_role = {
    "admin": ["alice", "bob"],
    "user": ["charlie", "david"]
}   # dict[str, list[str]]
print(users_by_role)   # {'admin': ['alice', 'bob'], 'user': ['charlie', 'david']}

# A list where each item is a dict
products = [
    {"name": "Apple", "price": 1.99},
    {"name": "Banana", "price": 0.99}
]   # list[dict[str, float]]
print(products)   # [{'name': 'Apple', 'price': 1.99}, {'name': 'Banana', 'price': 0.99}]

# A list of tuples — common for coordinate pairs
points = [(1, 2), (3, 4), (5, 6)]   # list[tuple[int, int]]
print(points)                         # [(1, 2), (3, 4), (5, 6)]


# --- Type hints on functions ---
# Parameters get hints after their name, return type goes after ->
# This tells anyone reading the code exactly what goes in and what comes out

def process_names(names: list[str]) -> list[int]:
    # Takes a list of name strings, returns a list of their character lengths
    return [len(name) for name in names]

def get_scores(scores: dict[str, int], threshold: int) -> list[str]:
    # Returns only the names whose score is at or above the threshold
    return [name for name, score in scores.items() if score >= threshold]

result = process_names(["Alice", "Bob", "Charlie"])
print(result)   # [5, 3, 7]

scores_dict = {"Alice": 95, "Bob": 85, "Charlie": 75}   # dict[str, int]
high_scorers = get_scores(scores_dict, 80)
print(high_scorers)   # ['Alice', 'Bob']


# --- Real-world example: user database lookup ---
# Simulates fetching user records by ID from a database
# The return type is complex: list[dict[str, str | int]]
# meaning a list of dicts where values can be either str or int

def get_user_info(user_ids: list[int]) -> list[dict[str, str | int]]:
    # Simulated in-memory database — in real apps this would be a DB query
    users_db = {
        1: {"name": "Alice", "email": "alice@example.com"},
        2: {"name": "Bob",   "email": "bob@example.com"},
        3: {"name": "Charlie", "email": "charlie@example.com"}
    }   # dict[int, dict[str, str]]

    result = []   # list[dict[str, str | int]] — will hold matched users
    for user_id in user_ids:
        if user_id in users_db:
            # Merge the id with the user's info into one dict
            user_info = {"id": user_id, **users_db[user_id]}
            result.append(user_info)
    return result

ids = [1, 2, 3]             # list[int] — IDs to look up
users = get_user_info(ids)
print(ids)    # [1, 2, 3]
print(users)  # [{'id': 1, 'name': 'Alice', ...}, ...]