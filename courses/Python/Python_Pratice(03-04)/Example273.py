# Example273: JSON Handling
import json

# Basic JSON operations
print("JSON Handling:")

# Create JSON
data = {
    "name": "Alice",
    "age": 30,
    "skills": ["Python", "JavaScript"],
    "active": True,
    "projects": {
        "web": "portfolio",
        "data": "analyzer"
    }
}

# Convert to JSON string
json_str = json.dumps(data, indent=2)
print("JSON string:")
print(json_str)

# Parse JSON
parsed = json.loads(json_str)
print(f"\nParsed name: {parsed['name']}")

# Load from file (simulated)
json_file = '{"user": "bob", "role": "admin"}'
parsed_file = json.loads(json_file)
print(f"Parsed from string: {parsed_file}")

# Pretty print
print("\nPretty print:")
print(json.dumps(data, indent=4, sort_keys=True))

# Handle dates
from datetime import datetime
data_with_date = {
    "event": "party",
    "date": "2024-12-25",
    "timestamp": "2024-12-25T10:30:00"
}
print(f"\nWith dates: {json.dumps(data_with_date)}")

# Custom encoding
class CustomEncoder(json.JSONEncoder):
    def default(self, obj):
        if isinstance(obj, set):
            return list(obj)
        if isinstance(obj, datetime):
            return obj.isoformat()
        return super().default(obj)

data_set = {"colors": {"red", "blue", "green"}}
print(f"\nCustom encoder: {json.dumps(data_set, cls=CustomEncoder)}")

# Validate JSON
def validate_json(json_str):
    try:
        json.loads(json_str)
        return True
    except json.JSONDecodeError:
        return False

print("\nValidate JSON:")
print(f"'{{\"a\": 1}}' valid: {validate_json('{\"a\": 1}')}")
print(f"'{{a: 1}}' valid: {validate_json('{a: 1}')}")
