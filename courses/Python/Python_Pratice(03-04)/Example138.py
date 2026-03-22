# Example138.py
# Topic: Random Data Generation and Sampling


# ============================================================
# Example 1: Basic Random
# ============================================================
print("=== Basic Random ===")

import random

print(f"random(): {random.random()}")
print(f"randint(1, 10): {random.randint(1, 10)}")
print(f"randrange(0, 10, 2): {random.randrange(0, 10, 2)}")
print(f"choice([1,2,3]): {random.choice([1, 2, 3])}")


# ============================================================
# Example 2: Sampling
# ============================================================
print("\n=== Sampling ===")

import random

items = list(range(1, 11))
print(f"Original: {items}")

sample3 = random.sample(items, 3)
print(f"Sample 3: {sample3}")

sample5 = random.sample(items, 5)
print(f"Sample 5: {sample5}")


# ============================================================
# Example 3: Shuffling
# ============================================================
print("\n=== Shuffling ===")

import random

deck = list(range(1, 53))
print(f"Original first 5: {deck[:5]}")

random.shuffle(deck)
print(f"Shuffled first 5: {deck[:5]}")


# ============================================================
# Example 4: Random with Seed
# ============================================================
print("\n=== Seed ===")

import random

random.seed(42)
print(f"seed 42: {random.random()}")

random.seed(42)
print(f"seed 42 again: {random.random()}")

random.seed(100)
print(f"seed 100: {random.random()}")


# ============================================================
# Example 5: Weighted Choice
# ============================================================
print("\n=== Weighted Choice ===")

import random

choices = ["low", "medium", "high"]
weights = [0.5, 0.3, 0.2]

for _ in range(10):
    result = random.choices(choices, weights=weights, k=1)[0]
    print(f"  {result}", end="")
print()


# ============================================================
# Example 6: Generate Random Data
# ============================================================
print("\n=== Generate Random Data ===")

import random
import string

def random_string(length):
    return ''.join(random.choices(string.ascii_letters, k=length))

def random_email():
    name = random_string(8).lower()
    domain = random.choice(["gmail.com", "yahoo.com", "example.com"])
    return f"{name}@{domain}"

for _ in range(3):
    print(f"  Email: {random_email()}")


# ============================================================
# Example 7: Real-World: Test Data
# ============================================================
print("\n=== Real-World: Test Data ===")

import random

def generate_users(count):
    first_names = ["Alice", "Bob", "Charlie", "Diana", "Eve"]
    last_names = ["Smith", "Johnson", "Williams", "Brown"]
    
    users = []
    for i in range(count):
        users.append({
            "id": i + 1,
            "name": f"{random.choice(first_names)} {random.choice(last_names)}",
            "age": random.randint(18, 80),
            "active": random.random() > 0.2,
        })
    return users

users = generate_users(5)
for u in users:
    print(f"  {u}")
