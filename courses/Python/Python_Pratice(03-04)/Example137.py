# Example137.py
# Topic: Working with UUID and IDs


# ============================================================
# Example 1: UUID Basics
# ============================================================
print("=== UUID Basics ===")

import uuid

u1 = uuid.uuid4()
print(f"UUID: {u1}")
print(f"UUID hex: {u1.hex}")
print(f"UUID string: {str(u1)}")


# ============================================================
# Example 2: UUID Versions
# ============================================================
print("\n=== UUID Versions ===")

import uuid

v1 = uuid.uuid1()
print(f"uuid1 (time-based): {v1}")

v4 = uuid.uuid4()
print(f"uuid4 (random): {v4}")

v5 = uuid.uuid5(uuid.NAMESPACE_DNS, "example.com")
print(f"uuid5 (name-based): {v5}")


# ============================================================
# Example 3: UUID in Collections
# ============================================================
print("\n=== UUID in Collections ===")

import uuid

uuids = [uuid.uuid4() for _ in range(3)]
print(f"UUIDs: {uuids}")

uuid_dict = {str(u): i for i, u in enumerate(uuids)}
print(f"Dict: {uuid_dict}")


# ============================================================
# Example 4: Generate Sequential IDs
# ============================================================
print("\n=== Sequential IDs ===")

import itertools

class IDGenerator:
    def __init__(self, prefix=""):
        self.prefix = prefix
        self.counter = itertools.count(1)
    
    def next(self):
        return f"{self.prefix}{next(self.counter)}"

gen = IDGenerator("USER-")
print(gen.next())
print(gen.next())
print(gen.next())


# ============================================================
# Example 5: Slug Generation
# ============================================================
print("\n=== Slug Generation ===")

import re

def generate_slug(text):
    text = text.lower()
    text = re.sub(r'[^\w\s-]', '', text)
    text = re.sub(r'[\s_-]+', '-', text)
    return text.strip('-')

print(generate_slug("Hello World!"))
print(generate_slug("Python 3.10 - Tutorial"))
print(generate_slug("What's New?"))


# ============================================================
# Example 6: Hash-based IDs
# ============================================================
print("\n=== Hash IDs ===")

import hashlib

def generate_hash_id(data):
    return hashlib.sha256(str(data).encode()).hexdigest()[:8]

print(generate_hash_id("user@example.com"))
print(generate_hash_id("password123"))
print(generate_hash_id(12345))


# ============================================================
# Example 7: Real-World: Unique Entity IDs
# ============================================================
print("\n=== Real-World: Entity IDs ===")

import uuid

class Entity:
    _registry = {}
    
    def __init__(self, entity_type, data):
        self.id = str(uuid.uuid4())[:8]
        self.type = entity_type
        self.data = data
        Entity._registry[self.id] = self
    
    def __repr__(self):
        return f"Entity({self.type}, {self.id})"

e1 = Entity("user", {"name": "Alice"})
e2 = Entity("order", {"amount": 100})
e3 = Entity("product", {"sku": "ABC123"})

print(f"Entities: {list(Entity._registry.values())}")
print(f"Find e1: {Entity._registry[e1.id]}")
