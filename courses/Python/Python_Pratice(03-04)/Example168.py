# Example168.py
# Topic: Dataclasses - More Advanced Patterns


# ============================================================
# Example 1: Dataclass with Default Factory Functions
# ============================================================
print("=== Default Factory Functions ===")

from dataclasses import dataclass, field
from datetime import datetime
from typing import Dict, List

@dataclass
class Project:
    name: str
    start_date: datetime = field(default_factory=datetime.now)
    tags: List[str] = field(default_factory=list)
    metadata: Dict[str, str] = field(default_factory=dict)

project = Project("My Project", tags=["python", "data"])
project.tags.append("ml")
project.metadata["owner"] = "Alice"

print(f"Project: {project.name}")
print(f"Start date: {project.start_date}")
print(f"Tags: {project.tags}")
print(f"Metadata: {project.metadata}")


# ============================================================
# Example 2: Dataclass with __str__ Override
# ============================================================
print("\n=== Custom __str__ ===")

from dataclasses import dataclass

@dataclass
class User:
    username: str
    email: str
    age: int
    
    def __str__(self) -> str:
        return f"User({self.username}, {self.email})"
    
    def __repr__(self) -> str:
        return f"User(username='{self.username}', email='{self.email}', age={self.age})"

user = User("alice", "alice@example.com", 30)
print(f"str: {str(user)}")
print(f"repr: {repr(user)}")


# ============================================================
# Example 3: Dataclass with asdict(), astuple(), replace()
# ============================================================
print("=== Dataclass Utilities ===")

from dataclasses import dataclass, asdict, astuple, replace

@dataclass
class Point:
    x: int
    y: int
    z: int = 0

p1 = Point(1, 2, 3)
print(f"Original: {p1}")

p_dict = asdict(p1)
print(f"asdict: {p_dict}")

p_tuple = astuple(p1)
print(f"astuple: {p_tuple}")

p2 = replace(p1, z=10)
print(f"replace: {p2}")

p3 = replace(p1, x=100)
print(f"replace x: {p3}")


# ============================================================
# Example 4: Dataclass Order=True
# ============================================================
print("\n=== Order=True ===")

from dataclasses import dataclass

@dataclass(order=True)
class Score:
    points: int
    player: str
    
    def __str__(self):
        return f"{self.player}: {self.points}"

scores = [
    Score(100, "Alice"),
    Score(50, "Bob"),
    Score(75, "Charlie"),
]

sorted_scores = sorted(scores)
print("Sorted by points:")
for s in sorted_scores:
    print(f"  {s}")


# ============================================================
# Example 5: Dataclass with Complex Default Values
# ============================================================
print("\n=== Complex Defaults ===")

from dataclasses import dataclass, field
from typing import Optional

@dataclass
class ServerConfig:
    host: str = "localhost"
    port: int = 8080
    debug: bool = False
    max_connections: int = 100
    timeout: float = 30.0

default_config = ServerConfig()
print(f"Default: {default_config}")

custom_config = ServerConfig(host="0.0.0.0", port=3000, debug=True)
print(f"Custom: {custom_config}")


# ============================================================
# Example 6: Dataclass for API Response
# ============================================================
print("\n=== API Response Dataclass ===")

from dataclasses import dataclass, field
from typing import Any, Optional, Dict
from datetime import datetime

@dataclass
class APIResponse:
    status_code: int
    data: Optional[Any] = None
    error: Optional[str] = None
    headers: Dict[str, str] = field(default_factory=dict)
    timestamp: datetime = field(default_factory=datetime.now)
    
    @property
    def is_success(self) -> bool:
        return 200 <= self.status_code < 300

response = APIResponse(status_code=200, data={"message": "Success"})
print(f"Response: {response.status_code}")
print(f"Success: {response.is_success}")

error_response = APIResponse(status_code=404, error="Not found")
print(f"Error: {error_response.is_success}")


# ============================================================
# Example 7: Nested Dataclasses
# ============================================================
print("\n=== Nested Dataclasses ===")

from dataclasses import dataclass, field
from typing import List

@dataclass
class Address:
    street: str
    city: str
    zip_code: str
    country: str = "USA"

@dataclass
class Person:
    name: str
    age: int
    address: Address

@dataclass
class Company:
    name: str
    employees: List[Person] = field(default_factory=list)
    headquarters: Address = None

addr = Address("123 Main St", "NYC", "10001")
person = Person("Alice", 30, addr)
company = Company("Tech Corp", employees=[person], headquarters=addr)

print(f"Company: {company.name}")
print(f"Employee: {company.employees[0].name}")
print(f"City: {company.headquarters.city}")


# ============================================================
# Example 8: Dataclass Validation Pattern
# ============================================================
print("\n=== Validation Pattern ===")

from dataclasses import dataclass, field
from typing import Optional

class ValidationError(Exception):
    pass

@dataclass
class CreditCard:
    number: str
    cvv: str
    expiry: str
    holder_name: str
    
    def __post_init__(self):
        errors = []
        
        if not self.number.isdigit() or len(self.number) != 16:
            errors.append("Invalid card number")
        
        if not self.cvv.isdigit() or len(self.cvv) not in [3, 4]:
            errors.append("Invalid CVV")
        
        if "/" not in self.expiry:
            errors.append("Invalid expiry format (use MM/YY)")
        
        if errors:
            raise ValidationError(", ".join(errors))

try:
    card = CreditCard("1234567890123456", "123", "12/25", "John Doe")
    print(f"Valid card: {card}")
except ValidationError as e:
    print(f"Error: {e}")

try:
    invalid = CreditCard("invalid", "12", "bad", "John")
except ValidationError as e:
    print(f"Error: {e}")
