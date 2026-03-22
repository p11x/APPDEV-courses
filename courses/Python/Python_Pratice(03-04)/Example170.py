# Example170.py
# Topic: Typed Collections - Advanced


# ============================================================
# Example 1: Complex Type Hints with Generics
# ============================================================
print("=== Complex Type Hints ===")

from typing import TypeVar, Generic, List, Dict, Optional

T = TypeVar('T')
K = TypeVar('K')
V = TypeVar('V')

class Repository(Generic[T]):
    def __init__(self):
        self._data: List[T] = []
    
    def add(self, item: T) -> None:
        self._data.append(item)
    
    def get_all(self) -> List[T]:
        return self._data

class User:
    def __init__(self, name: str):
        self.name = name

repo: Repository[User] = Repository()
repo.add(User("Alice"))
repo.add(User("Bob"))

for user in repo.get_all():
    print(f"User: {user.name}")


# ============================================================
# Example 2: Typed Dict
# ============================================================
print("\n=== Typed Dict ===")

from typing import TypedDict, NotRequired, Literal

class UserDict(TypedDict):
    name: str
    age: int
    email: str
    active: bool = True

class ConfigDict(TypedDict):
    host: str
    port: int
    debug: NotRequired[bool]
    mode: NotRequired[Literal["dev", "prod", "test"]]

user: UserDict = {"name": "Alice", "age": 30, "email": "alice@example.com"}
print(f"User: {user}")

config: ConfigDict = {"host": "localhost", "port": 8080}
print(f"Config: {config}")


# ============================================================
# Example 3: NewType
# ============================================================
print("\n=== NewType ===")

from typing import NewType

UserId = NewType('UserId', int)
OrderId = NewType('OrderId', str)

def get_user(user_id: UserId) -> dict:
    return {"id": user_id, "name": "Alice"}

def get_order(order_id: OrderId) -> dict:
    return {"id": order_id, "total": 99.99}

user_id = UserId(123)
order_id = OrderId("ORD-001")

print(f"User: {get_user(user_id)}")
print(f"Order: {get_order(order_id)}")


# ============================================================
# Example 4: Protocol (Structural Subtyping)
# ============================================================
print("\n=== Protocol ===")

from typing import Protocol

class Drawable(Protocol):
    def draw(self) -> str: ...

class Circle:
    def draw(self) -> str:
        return "Drawing circle"

class Square:
    def draw(self) -> str:
        return "Drawing square"

def render(drawable: Drawable) -> None:
    print(drawable.draw())

render(Circle())
render(Square())


# ============================================================
# Example 5: ParamSpec and Concatenate
# ============================================================
print("=== ParamSpec ===")

from typing import Callable, ParamSpec, TypeVar

P = ParamSpec('P')
R = TypeVar('R')

def logger(func: Callable[P, R]) -> Callable[P, R]:
    def wrapper(*args: P.args, **kwargs: P.kwargs) -> R:
        print(f"Calling {func.__name__}")
        result = func(*args, **kwargs)
        print(f"Finished {func.__name__}")
        return result
    return wrapper

@logger
def add(a: int, b: int) -> int:
    return a + b

@logger
def greet(name: str) -> str:
    return f"Hello, {name}!"

print(add(1, 2))
print(greet("World"))


# ============================================================
# Example 6: TypeGuard
# ============================================================
print("\n=== TypeGuard ===")

from typing import TypeGuard, List

def is_string_list(val: List[object]) -> TypeGuard[List[str]]:
    return all(isinstance(x, str) for x in val)

items: List[object] = ["a", "b", "c"]
if is_string_list(items):
    print(f"All strings: {items}")
else:
    print("Not all strings")

mixed: List[object] = ["a", 1, "b"]
if is_string_list(mixed):
    print(f"All strings: {mixed}")
else:
    print("Not all strings")


# ============================================================
# Example 7: Annotated Types
# ============================================================
print("\n=== Annotated ===")

from typing import Annotated

PositiveInt = Annotated[int, "positive"]
NegativeFloat = Annotated[float, "negative"]
Email = Annotated[str, "email"]

def create_user(name: str, age: PositiveInt, email: Email) -> dict:
    return {"name": name, "age": age, "email": email}

user = create_user("Alice", 30, "alice@example.com")
print(f"User: {user}")


# ============================================================
# Example 8: Generic Data Structures
# ============================================================
print("\n=== Generic Data Structures ===")

from typing import Generic, TypeVar, Optional

T = TypeVar('T')

class Stack(Generic[T]):
    def __init__(self):
        self._items: list[T] = []
    
    def push(self, item: T) -> None:
        self._items.append(item)
    
    def pop(self) -> Optional[T]:
        return self._items.pop() if self._items else None
    
    def peek(self) -> Optional[T]:
        return self._items[-1] if self._items else None
    
    def is_empty(self) -> bool:
        return len(self._items) == 0

int_stack: Stack[int] = Stack()
int_stack.push(1)
int_stack.push(2)
int_stack.push(3)

while not int_stack.is_empty():
    print(f"Popped: {int_stack.pop()}")

str_stack: Stack[str] = Stack()
str_stack.push("hello")
str_stack.push("world")
print(f"Peek: {str_stack.peek()}")
