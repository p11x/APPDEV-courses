# Example18.py
# Topic: Type Hints - Advanced (Literal, Final)

print("=" * 50)
print("TYPE HINTS - ADVANCED TYPES")              # TYPE HINTS - ADVANCED TYPES
print("=" * 50)
# Literal Types
print("\n--- Literal Types ---\n")                # \n--- Literal Types ---\n
    
from typing import Literal
    
# Restrict to specific values
status: Literal["pending", "approved", "rejected"] = "pending"
print("status: Literal['pending', 'approved', 'rejected'] = '" + str(status) + "'")# status: Literal['pending', 'approved', 'rejected'] = '" + str(status) + "'
    
status = "approved"
print("status = '" + str(status) + "'")           # status = '" + str(status) + "'
    
status = "rejected"
print("status = '" + str(status) + "'")           # status = '" + str(status) + "'
    
# Literal with integers
direction: Literal[0, 1, 2, 3] = 0
print("direction: Literal[0, 1, 2, 3] = " + str(direction))
# Final Types
print("\n--- Final Types ---\n")                  # \n--- Final Types ---\n
    
from typing import Final
    
# Constants that shouldn't change
MAX_SIZE = 100
VERSION = "1.0.0"
DEFAULT_TIMEOUT = 30
    
print("MAX_SIZE: Final = " + str(MAX_SIZE))
print("VERSION: Final = " + str(VERSION))
print("DEFAULT_TIMEOUT: Final = " + str(DEFAULT_TIMEOUT))
    
# Note: Final doesn't actually prevent reassignment at runtime
# But it tells type checkers this shouldn't change
# MAX_SIZE = 200  # Type checker would warn!
# Practical Examples
print("\n--- Practical Examples ---\n")           # \n--- Practical Examples ---\n
    
# Status with Literal
def process_order(
    order_id: int,
    status: Literal["pending", "processing", "shipped", "delivered"]
) -> dict:
    return {"order_id": order_id, "status": status}
    
order1 = process_order(123, "pending")
order2 = process_order(124, "shipped")
print("process_order(123, 'pending') = " + str(order1))
print("process_order(124, 'shipped') = " + str(order2))
    
# HTTP Methods with Literal
def make_request(
    method: Literal["GET", "POST", "PUT", "DELETE"],
    url: str
) -> str:
    return str(method) + " " + str(url)
    
get_request = make_request("GET", "/users")
post_request = make_request("POST", "/users")
print("\nmake_request('GET', '/users') = " + str(get_request))
print("make_request('POST', '/users') = " + str(post_request))
    
# Final for configuration
class DatabaseConfig:
    HOST = "localhost"
    PORT = 5432
    MAX_CONNECTIONS = 10
        
    def __init__(self, host: str = HOST, port: int = PORT) -> None:
        self.host = host
        self.port = port
    
config = DatabaseConfig()
print("\nDatabaseConfig: " + str(config.host) + ":" + str(config.port))
# Type Aliases
print("\n--- Type Aliases ---\n")                 # \n--- Type Aliases ---\n
    
# Create reusable type names
UserId = int
UserName = str
UserScore = float
    
user_id = 123
user_name = "Alice"
user_score = 95.5
    
print("UserId = " + str(user_id))
print("UserName = " + str(user_name))
print("UserScore = " + str(user_score))
    
# Complex type alias
UserDict = dict[str, int | float | str]
    
user_data = {
    "id": 1,
    "score": 95.5,
    "name": "Alice"
}
print("UserDict: " + str(user_data))
# Combining Advanced Types
print("\n--- Combining Advanced Types ---\n")     # \n--- Combining Advanced Types ---\n
    
from typing import Final
    
# Final Literal combination
HTTP_METHOD: Final[Literal["GET", "POST"]] = "GET"
print("HTTP_METHOD: Final[Literal['GET', 'POST']] = " + str(HTTP_METHOD))
# Summary
print("\n" + "=" * 50)
print("ADVANCED TYPE HINTS SUMMARY")              # ADVANCED TYPE HINTS SUMMARY
print("=" * 50)
print("Key Points:")                              # Key Points:
print("- Literal[T1, T2, ...] - restrict to specific values")# - Literal[T1, T2, ...] - restrict to specific values
print("- Final - mark constants that shouldn't change")# - Final - mark constants that shouldn't change
print("- Type aliases - create reusable type names")# - Type aliases - create reusable type names
print("- Combine types for complex scenarios")    # - Combine types for complex scenarios
print("- Import from typing module")              # - Import from typing module

# Real-world example: