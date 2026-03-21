# Example10.py
# Topic: Using **kwargs for Variable Keyword Arguments

# This file demonstrates how to use **kwargs to create functions that can
# accept any number of keyword arguments, which are collected into a dictionary.
# This is useful when you want to pass flexible named parameters.


# Takes any number of keyword arguments and prints each as key-value pairs
# The **kwargs parameter collects all keyword arguments into a dictionary
def print_info(**kwargs) -> None:
    for key, value in kwargs.items():    # iterate through dictionary items
        print(key + ": " + str(value))

print_info(name="Alice", age=25)             # name: Alice, age: 25
print_info(city="NYC", country="USA")       # city: NYC, country: USA


# Creates and returns a dictionary from all keyword arguments passed
def create_profile(**info) -> dict:
    return info    # dict — all keyword arguments as a dictionary

profile = create_profile(name="John", age=30, city="LA")    # dict — profile data
print("Profile: " + str(profile))                            # Profile: {'name': 'John', 'age': 30, 'city': 'LA'}


# Prints formatted user information, only displaying fields that exist
def print_user(**user_data) -> None:
    print("=== User Info ===")
    if "name" in user_data:    # check if 'name' key exists
        print("Name: " + user_data["name"])
    if "age" in user_data:    # check if 'age' key exists
        print("Age: " + str(user_data["age"]))
    if "email" in user_data:    # check if 'email' key exists
        print("Email: " + user_data["email"])

print_user(name="Alice", email="alice@example.com")    # Name: Alice, Email: alice@example.com
print_user(name="Bob", age=25, city="Chicago")          # Name: Bob, Age: 25


# Counts and returns the total number of keyword arguments passed
def count_kwargs(**kwargs) -> int:
    return len(kwargs)    # int — number of key-value pairs

count = count_kwargs(a=1, b=2, c=3)    # int — three keyword arguments
print("Count: " + str(count))           # Count: 3


# Retrieves a specific value from kwargs, returns default if not found
def get_value(key, **kwargs) -> str:
    if key in kwargs:    # check if key exists in dictionary
        return kwargs[key]    # str — value for the requested key
    return "Not found"    # str — default message if key not found

value = get_value("name", name="Alice", age=25)    # str — "Alice"
print("Value: " + value)                            # Value: Alice

value2 = get_value("country", name="Alice")    # str — "Not found"
print("Value: " + value2)                        # Value: Not found


# Builds a configuration dictionary with default values that can be overridden
def build_config(**config) -> dict:
    defaults = {"debug": False, "version": "1.0", "port": 8080}    # dict — default settings
    for key, value in config.items():    # override defaults with provided values
        defaults[key] = value
    return defaults

config = build_config(debug=True, port=3000)    # dict — merged config
print("Config: " + str(config))                 # Config: {'debug': True, 'version': '1.0', 'port': 3000}


# Validates that all required keys are present in the keyword arguments
def validate(required_keys, **kwargs) -> bool:
    for key in required_keys:    # loop through each required key
        if key not in kwargs:    # check if key exists in kwargs
            return False    # False — missing a required key
    return True    # True — all required keys present

result = validate(["name", "email"], name="Alice", email="a@b.com")    # bool — True
print("Valid: " + str(result))                                         # Valid: True

result2 = validate(["name", "age"], name="Bob")    # bool — False (missing age)
print("Valid: " + str(result2))                    # Valid: False


# Updates a base dictionary with additional key-value pairs
def update_record(base_record, **updates) -> dict:
    record = base_record.copy()    # dict — copy of base to avoid modifying original
    for key, value in updates.items():    # add each update to the record
        record[key] = value
    return record

original = {"id": 1, "name": "Alice"}    # dict — original record
updated = update_record(original, age=25, city="NYC")    # dict — with updates
print("Updated: " + str(updated))                        # Updated: {'id': 1, 'name': 'Alice', 'age': 25, 'city': 'NYC'}


# Builds a URL query string from keyword arguments as parameters
def build_query(**params) -> str:
    query_parts = []    # list — accumulator for query parts
    for key, value in params.items():    # convert each key-value to key=value
        query_parts.append(key + "=" + str(value))
    return "?" + "&".join(query_parts)    # str — formatted query string

query = build_query(search="python", page=1, limit=10)    # str — query string
print("Query: " + query)                                    # Query: ?search=python&page=1&limit=10


# Prints all keyword arguments with their values converted to strings
def display_all(**kwargs) -> None:
    for key, value in kwargs.items():    # iterate through all keyword arguments
        print(key + " = " + str(value))

display_all(count=5, active=True, rate=3.14)    # count = 5, active = True, rate = 3.14


# Collects database connection parameters and returns formatted connection string
def connect_db(**db_params) -> dict:
    host = db_params.get("host", "localhost")    # str — host with default
    port = db_params.get("port", 5432)          # int — port with default
    database = db_params.get("database", "mydb")    # str — database with default
    return {
        "host": host,
        "port": port,
        "database": database
    }

conn = connect_db(host="db.example.com", database="users")    # dict — connection info
print("Connection: " + str(conn))                          # Connection: {'host': 'db.example.com', 'port': 5432, 'database': 'users'}
