# Example94.py
# Topic: Pattern Matching — Rest Patterns with **

# Use ** to capture remaining keys in a dictionary

# === Capture rest with ** ===
# Get specific keys and capture the rest

user = {"name": "Alice", "age": 30, "city": "NYC", "country": "USA"}

match user:
    case {"name": name, **rest}:
        print("Name: " + name)
        print("Other info: " + str(rest))
    case _:
        print("No match")


# === Capture specific keys and rest ===
config = {"host": "localhost", "port": 8080, "debug": True, "log_level": "info"}

match config:
    case {"host": host, "port": port, **options}:
        print("Server: " + host + ":" + str(port))
        print("Options: " + str(options))
    case _:
        print("No match")


# === Capture nothing when only specific keys ===
simple = {"id": 1, "name": "Test"}

match simple:
    case {"id": id, **rest}:
        print("ID: " + str(id))
        print("Rest: " + str(rest))
    case _:
        print("No match")


# === Rest with empty dict ===
empty = {}

match empty:
    case {**rest}:
        print("Empty dict - rest is: " + str(rest))
    case _:
        print("No match")


# === Practical: API request with extra headers ===
request = {
    "method": "GET",
    "url": "/api/users",
    "headers": {
        "Authorization": "Bearer token",
        "Accept": "application/json",
        "Custom-Header": "value"
    }
}

match request:
    case {"method": method, "url": url, **extras}:
        print("Method: " + method)
        print("URL: " + url)
        if "headers" in extras:
            print("Has headers: " + str(extras.get("headers")))
        print("Extra fields: " + str({k: v for k, v in extras.items() if k != "headers"}))


# === Combining ** with specific values ===
data = {"type": "success", "code": 200, "message": "OK", "extra1": "a", "extra2": "b"}

match data:
    case {"type": "success", "code": 200, **rest}:
        print("Success response!")
        print("Extra data: " + str(rest))
    case {"type": "error", **rest}:
        print("Error response!")
        print("Error details: " + str(rest))
    case _:
        print("Unknown type")


# === Rest with nested dicts ===
complex_data = {
    "user": {
        "name": "Alice",
        "age": 30,
        "extra1": "a",
        "extra2": "b"
    }
}

match complex_data:
    case {"user": {"name": name, **user_rest}}:
        print("User: " + name)
        print("User extras: " + str(user_rest))
    case _:
        print("No match")


# === Rest with guards ===
settings = {"theme": "dark", "font_size": 14, "animation": True, "unused": "value"}

match settings:
    case {"theme": theme, **rest} if theme == "dark":
        print("Dark theme with extras: " + str(rest))
    case {"theme": theme, **rest}:
        print("Other theme: " + theme)
        print("Extras: " + str(rest))
    case _:
        print("No settings")


# === Multiple uses of ** in nested patterns ===
nested = {
    "level1": {
        "level2": {
            "key": "value",
            "extra1": "a",
            "extra2": "b"
        }
    }
}

match nested:
    case {"level1": {"level2": {"key": key, **l2_rest}, **l1_rest}}:
        print("Key: " + key)
        print("Level 2 rest: " + str(l2_rest))
        print("Level 1 rest: " + str(l1_rest))
    case _:
        print("No match")


# === Practical: Event handler ===
event = {
    "type": "click",
    "target": "button",
    "timestamp": 1234567890,
    "x": 100,
    "y": 200,
    "ctrlKey": False
}

match event:
    case {"type": "click", "target": target, **event_data}:
        print("Click event on: " + target)
        print("Event data: " + str(event_data))
    case {"type": "keydown", "key": key, **event_data}:
        print("Keydown: " + key)
        print("Event data: " + str(event_data))
    case _:
        print("Unknown event")
