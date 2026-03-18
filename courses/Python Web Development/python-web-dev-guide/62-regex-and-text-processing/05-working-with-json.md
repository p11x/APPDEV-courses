# Working with JSON

## What You'll Learn

- JSON basics in Python
- Parsing and generating JSON
- Handling large JSON

## Prerequisites

- Completed `04-input-validation.md`

## JSON Basics

```python
import json

# Parse JSON string
json_string = '{"name": "Alice", "age": 30}'
data = json.loads(json_string)
print(data)  # {'name': 'Alice', 'age': 30}

# Convert to JSON string
data = {"name": "Alice", "age": 30}
json_string = json.dumps(data)
print(json_string)  # {"name": "Alice", "age": 30}
```

## Pretty Printing

```python
data = {
    "name": "Alice",
    "age": 30,
    "skills": ["Python", "JavaScript"]
}

# Pretty print with indentation
print(json.dumps(data, indent=2))

# Sort keys
print(json.dumps(data, sort_keys=True, indent=2))
```

## Working with Files

```python
# Read JSON from file
with open("data.json", "r") as f:
    data = json.load(f)

# Write JSON to file
with open("output.json", "w") as f:
    json.dump(data, f, indent=2)
```

## Custom Encoding

```python
from datetime import datetime
from dataclasses import dataclass

@dataclass
class User:
    name: str
    email: str
    created_at: datetime

class UserEncoder(json.JSONEncoder):
    def default(self, obj):
        if isinstance(obj, datetime):
            return obj.isoformat()
        if isinstance(obj, User):
            return {"name": obj.name, "email": obj.email}
        return super().default(obj)

# Encode custom objects
user = User("Alice", "alice@example.com", datetime.now())
json_string = json.dumps(user, cls=UserEncoder)
```

## Lazy Loading Large JSON

```python
import ijson  # pip install ijson

def parse_large_json(filepath: str):
    """Parse large JSON file lazily."""
    with open(filepath, "rb") as f:
        # Parse objects one at a time
        parser = ijson.items(f, "item")
        for item in parser:
            yield item

# Usage
for item in parse_large_json("large_file.json"):
    # Process each item
    print(item)
```

## Streaming JSON Arrays

```python
import json

def stream_json_objects(filepath: str):
    """Stream JSON objects from array."""
    with open(filepath, "r") as f:
        # Use ijson for large files
        import ijson
        parser = ijson.items(f, "item")
        for obj in parser:
            yield obj
```

## Summary

- Use json.loads and json.dumps
- Use json.load and json.dump for files
- Use ijson for large files

## Next Steps

Continue to `06-csv-processing.md`.
