# Example132.py
# Topic: Working with JSON and Nested Structures


# ============================================================
# Example 1: JSON Basics
# ============================================================
print("=== JSON Basics ===")

import json

data = {"name": "Alice", "age": 30, "skills": ["Python", "Java"]}
json_str = json.dumps(data)
print(f"JSON string: {json_str}")

parsed = json.loads(json_str)
print(f"Parsed: {parsed}")


# ============================================================
# Example 2: Pretty Print JSON
# ============================================================
print("\n=== Pretty Print ===")

import json

data = {
    "users": [
        {"name": "Alice", "age": 30},
        {"name": "Bob", "age": 25},
    ]
}

print(json.dumps(data, indent=2))
print(json.dumps(data, indent=2, sort_keys=True))


# ============================================================
# Example 3: Nested Dict Access
# ============================================================
print("\n=== Nested Dict Access ===")

data = {
    "company": {
        "employees": [
            {"name": "Alice", "address": {"city": "NYC"}},
            {"name": "Bob", "address": {"city": "LA"}},
        ]
    }
}

print(f"First employee: {data['company']['employees'][0]['name']}")
print(f"City: {data['company']['employees'][0]['address']['city']}")


# ============================================================
# Example 4: Safe Nested Get
# ============================================================
print("\n=== Safe Nested Get ===")

data = {"a": {"b": {"c": 1}}}

def nested_get(d, *keys, default=None):
    for key in keys:
        if isinstance(d, dict):
            d = d.get(key, default)
        else:
            return default
    return d

print(nested_get(data, "a", "b", "c"))
print(nested_get(data, "a", "x", "y", default="Not found"))


# ============================================================
# Example 5: Flatten Nested Dict
# ============================================================
print("\n=== Flatten Dict ===")

def flatten_dict(d, parent_key="", sep="_"):
    items = []
    for k, v in d.items():
        new_key = f"{parent_key}{sep}{k}" if parent_key else k
        if isinstance(v, dict):
            items.extend(flatten_dict(v, new_key, sep=sep).items())
        else:
            items.append((new_key, v))
    return dict(items)

nested = {"a": 1, "b": {"c": 2, "d": {"e": 3}}}
print(f"Flattened: {flatten_dict(nested)}")


# ============================================================
# Example 6: Unflatten Dict
# ============================================================
print("\n=== Unflatten Dict ===")

def unflatten_dict(d, sep="_"):
    result = {}
    for key, value in d.items():
        parts = key.split(sep)
        d = result
        for part in parts[:-1]:
            if part not in d:
                d[part] = {}
            d = d[part]
        d[parts[-1]] = value
    return result

flat = {"a": 1, "b_c": 2, "b_d_e": 3}
print(f"Unflattened: {unflatten_dict(flat)}")


# ============================================================
# Example 7: Real-World: Config Management
# ============================================================
print("\n=== Real-World: Config ===")

import json

config = {
    "database": {
        "host": "localhost",
        "port": 5432,
        "credentials": {
            "username": "admin",
            "password": "secret"
        }
    },
    "cache": {
        "enabled": True,
        "ttl": 3600
    }
}

with open("config.json", "w") as f:
    json.dump(config, f, indent=2)

with open("config.json", "r") as f:
    loaded = json.load(f)

print(f"DB Host: {loaded['database']['host']}")
