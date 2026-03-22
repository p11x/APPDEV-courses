# Example139.py
# Topic: Copy and Deepcopy


# ============================================================
# Example 1: Shallow Copy
# ============================================================
print("=== Shallow Copy ===")

import copy

original = [1, 2, [3, 4]]
shallow = copy.copy(original)

print(f"Original: {original}")
print(f"Shallow: {shallow}")

shallow[2].append(5)
print(f"Original after modifying shallow: {original}")
print(f"Shallow after modifying: {shallow}")


# ============================================================
# Example 2: Deep Copy
# ============================================================
print("\n=== Deep Copy ===")

import copy

original = [1, 2, [3, 4]]
deep = copy.deepcopy(original)

print(f"Original: {original}")
print(f"Deep: {deep}")

deep[2].append(5)
print(f"Original after modifying deep: {original}")
print(f"Deep after modifying: {deep}")


# ============================================================
# Example 3: Copy Dict
# ============================================================
print("\n=== Dict Copy ===")

import copy

d = {"a": 1, "b": {"c": 2}}

d_copy = d.copy()
d_deep = copy.deepcopy(d)

d["b"]["c"] = 99
print(f"Original: {d}")
print(f"Shallow copy: {d_copy}")
print(f"Deep copy: {d_deep}")


# ============================================================
# Example 4: Copy Objects
# ============================================================
print("\n=== Object Copy ===")

import copy

class Person:
    def __init__(self, name, address):
        self.name = name
        self.address = address
    def __repr__(self):
        return f"Person({self.name}, {self.address})"

original = Person("Alice", {"city": "NYC"})

shallow = copy.copy(original)
deep = copy.deepcopy(original)

original.address["city"] = "LA"
print(f"Original: {original}")
print(f"Shallow: {shallow}")
print(f"Deep: {deep}")


# ============================================================
# Example 5: copy() vs deepcopy()
# ============================================================
print("\n=== copy() vs deepcopy() ===")

import copy

nested = [1, [2, 3]]
s = copy.copy(nested)
d = copy.deepcopy(nested)

print(f"Nested: {nested}")
print(f"Shallow: {s}")
print(f"Deep: {d}")

nested[1][0] = 99
print(f"After change:")
print(f"Nested: {nested}")
print(f"Shallow: {s}")
print(f"Deep: {d}")


# ============================================================
# Example 6: Real-World: Configuration
# ============================================================
print("\n=== Real-World: Config Template ===")

import copy

default_config = {
    "database": {"host": "localhost", "port": 5432},
    "cache": {"enabled": True, "ttl": 3600},
    "features": ["auth", "api"],
}

dev_config = copy.deepcopy(default_config)
dev_config["database"]["host"] = "dev.example.com"
dev_config["features"].append("debug")

prod_config = copy.deepcopy(default_config)
prod_config["database"]["host"] = "prod.example.com"

print(f"Default: {default_config}")
print(f"Dev: {dev_config}")
print(f"Prod: {prod_config}")
