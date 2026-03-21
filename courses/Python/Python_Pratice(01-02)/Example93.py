# Example93.py
# Topic: Pattern Matching — Nested Mapping Patterns

# Match nested dictionaries

# === Basic nested matching ===
company = {"name": "TechCorp", "address": {"city": "NYC", "country": "USA"}}

match company:
    case {"name": name, "address": {"city": city, "country": country}}:
        print(name + " is in " + city + ", " + country)
    case _:
        print("Not a valid company")


# === Matching deeply nested structures ===
api_response = {
    "status": "success",
    "data": {
        "user": {
            "name": "Alice",
            "email": "alice@example.com"
        }
    }
}

match api_response:
    case {"status": "success", "data": {"user": {"name": name, "email": email}}}:
        print("User: " + name + " (" + email + ")")
    case _:
        print("Invalid response")


# === Nested with mixed types ===
product = {
    "id": 123,
    "details": {
        "price": 29.99,
        "in_stock": True
    }
}

match product:
    case {"id": pid, "details": {"price": price, "in_stock": stock}}:
        print("Product #" + str(pid))
        print("Price: $" + str(price))
        print("In stock: " + str(stock))
    case _:
        print("Not a valid product")


# === Multiple levels of nesting ===
config = {
    "app": {
        "server": {
            "host": "localhost",
            "port": 8080
        },
        "debug": True
    }
}

match config:
    case {"app": {"server": {"host": host, "port": port}, "debug": debug}}:
        print("Server: " + host + ":" + str(port))
        print("Debug mode: " + str(debug))
    case _:
        print("Invalid config")


# === Partial nested matching ===
user = {"profile": {"settings": {"theme": "dark"}}}

match user:
    case {"profile": {"settings": {"theme": theme}}}:
        print("Theme: " + theme)
    case _:
        print("No theme found")


# === Nested with list inside ===
employee = {
    "name": "John",
    "department": {
        "name": "Engineering",
        "team": ["Alice", "Bob", "Charlie"]
    }
}

match employee:
    case {"name": name, "department": {"name": dept_name, "team": team}}:
        print(name + " in " + dept_name + " team")
        print("Team members: " + str(len(team)))
    case _:
        print("Invalid employee")


# === Practical: Multi-level API response ===
response = {
    "result": "ok",
    "payload": {
        "page": 1,
        "items": [
            {"id": 1, "title": "Item 1"},
            {"id": 2, "title": "Item 2"}
        ]
    }
}

match response:
    case {"result": "ok", "payload": {"page": page, "items": items}}:
        print("Page " + str(page) + ": " + str(len(items)) + " items")
        for item in items:
            print("  - " + str(item["id"]) + ": " + item["title"])
    case _:
        print("Invalid response")


# === Nested + optional keys ===
data = {"outer": {"inner": {"value": 42}}}

match data:
    case {"outer": {"inner": {"value": val}}}:
        print("Found value: " + str(val))
    case {"outer": {"inner": other}}:
        print("No value key, got: " + str(other))
    case _:
        print("No match")


# === Nested matching with guards ===
request = {
    "method": "POST",
    "headers": {"Content-Type": "application/json"}
}

match request:
    case {"method": "POST", "headers": {"Content-Type": ct}} if "json" in ct:
        print("JSON POST request")
    case {"method": "POST", "headers": {"Content-Type": ct}}:
        print("POST request with Content-Type: " + ct)
    case {"method": "GET"}:
        print("GET request")
    case _:
        print("Unknown request")
