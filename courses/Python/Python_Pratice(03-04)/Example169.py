# Example169.py
# Topic: Dataclasses - Real-World Applications


# ============================================================
# Example 1: Event Sourcing with Dataclasses
# ============================================================
print("=== Event Sourcing ===")

from dataclasses import dataclass, field
from datetime import datetime
from typing import Any, Dict
from enum import Enum

class EventType(Enum):
    USER_CREATED = "user_created"
    USER_UPDATED = "user_updated"
    USER_DELETED = "user_deleted"
    ORDER_PLACED = "order_placed"

@dataclass
class Event:
    event_type: EventType
    timestamp: datetime = field(default_factory=datetime.now)
    metadata: Dict[str, Any] = field(default_factory=dict)
    
    def __str__(self):
        return f"{self.event_type.value} at {self.timestamp.isoformat()}"

events = [
    Event(EventType.USER_CREATED, metadata={"user_id": 1, "email": "alice@example.com"}),
    Event(EventType.ORDER_PLACED, metadata={"order_id": "ORD-001", "amount": 99.99}),
    Event(EventType.USER_DELETED, metadata={"user_id": 1}),
]

for event in events:
    print(event)


# ============================================================
# Example 2: Command Pattern with Dataclasses
# ============================================================
print("\n=== Command Pattern ===")

from dataclasses import dataclass, field
from typing import Any, Callable, Dict
from datetime import datetime

@dataclass
class Command:
    name: str
    execute: Callable = field(repr=False)
    undo: Callable = field(repr=False)
    timestamp: datetime = field(default_factory=datetime.now)

class CommandHistory:
    def __init__(self):
        self.commands: list[Command] = []
    
    def execute(self, cmd: Command):
        cmd.execute()
        self.commands.append(cmd)
    
    def undo_last(self):
        if self.commands:
            cmd = self.commands.pop()
            cmd.undo()

history = CommandHistory()

def create_user():
    print("Creating user...")

def delete_user():
    print("Deleting user...")

def send_email():
    print("Sending email...")

def unsend_email():
    print("Undoing email...")

history.execute(Command("create_user", create_user, delete_user))
history.execute(Command("send_email", send_email, unsend_email))
print("Undoing last command:")
history.undo_last()


# ============================================================
# Example 3: Builder Pattern with Dataclasses
# ============================================================
print("\n=== Builder Pattern ===")

from dataclasses import dataclass, field
from typing import Optional

@dataclass
class Computer:
    cpu: str = "Intel i5"
    ram: str = "8GB"
    storage: str = "256GB SSD"
    gpu: Optional[str] = None
    display: Optional[str] = None

class ComputerBuilder:
    def __init__(self):
        self._computer = Computer()
    
    def set_cpu(self, cpu: str):
        self._computer.cpu = cpu
        return self
    
    def set_ram(self, ram: str):
        self._computer.ram = ram
        return self
    
    def set_storage(self, storage: str):
        self._computer.storage = storage
        return self
    
    def set_gpu(self, gpu: str):
        self._computer.gpu = gpu
        return self
    
    def set_display(self, display: str):
        self._computer.display = display
        return self
    
    def build(self) -> Computer:
        return self._computer

gaming_pc = (ComputerBuilder()
    .set_cpu("Intel i9")
    .set_ram("32GB")
    .set_storage("1TB SSD")
    .set_gpu("RTX 4090")
    .set_display("27 inch 4K")
    .build())

print(f"Gaming PC: {gaming_pc}")


# ============================================================
# Example 4: Strategy Pattern with Dataclasses
# ============================================================
print("\n=== Strategy Pattern ===")

from dataclasses import dataclass
from typing import Protocol, List

class SortStrategy(Protocol):
    def sort(self, data: List[int]) -> List[int]: ...

@dataclass
class QuickSort:
    name: str = "QuickSort"
    
    def sort(self, data: List[int]) -> List[int]:
        if len(data) <= 1:
            return data
        pivot = data[len(data) // 2]
        left = [x for x in data if x < pivot]
        middle = [x for x in data if x == pivot]
        right = [x for x in data if x > pivot]
        return self.sort(left) + middle + self.sort(right)

@dataclass
class BubbleSort:
    name: str = "BubbleSort"
    
    def sort(self, data: List[int]) -> List[int]:
        arr = data.copy()
        n = len(arr)
        for i in range(n):
            for j in range(0, n-i-1):
                if arr[j] > arr[j+1]:
                    arr[j], arr[j+1] = arr[j+1], arr[j]
        return arr

def process_data(data: List[int], strategy: SortStrategy) -> List[int]:
    print(f"Using {strategy.name}")
    return strategy.sort(data)

data = [64, 34, 25, 12, 22, 11, 90]
print(f"Original: {data}")
print(f"QuickSort: {process_data(data, QuickSort())}")
print(f"BubbleSort: {process_data(data, BubbleSort())}")


# ============================================================
# Example 5: ChainMap with Dataclasses for Scoped Config
# ============================================================
print("\n=== ChainMap for Config ===")

from dataclasses import dataclass, field
from collections import ChainMap

@dataclass
class Config:
    debug: bool = False
    log_level: str = "INFO"
    max_retries: int = 3
    timeout: int = 30

@dataclass
class AppContext:
    configs: ChainMap = field(default_factory=lambda: ChainMap(
        {"debug": True, "log_level": "DEBUG"},
        {"max_retries": 5},
        asdict(Config())
    ))
    
    def get(self, key: str, default=None):
        return self.configs.get(key, default)

context = AppContext()
print(f"debug: {context.get('debug')}")
print(f"log_level: {context.get('log_level')}")
print(f"max_retries: {context.get('max_retries')}")
print(f"timeout: {context.get('timeout')}")


# ============================================================
# Example 6: State Machine with Dataclasses
# ============================================================
print("\n=== State Machine ===")

from dataclasses import dataclass, field
from enum import Enum, auto
from typing import Dict, Callable, Optional

class OrderState(Enum):
    DRAFT = auto()
    SUBMITTED = auto()
    CONFIRMED = auto()
    SHIPPED = auto()
    DELIVERED = auto()
    CANCELLED = auto()

@dataclass
class Order:
    order_id: str
    state: OrderState = OrderState.DRAFT
    total: float = 0.0
    
    def transition(self, new_state: OrderState) -> None:
        valid_transitions = {
            OrderState.DRAFT: [OrderState.SUBMITTED, OrderState.CANCELLED],
            OrderState.SUBMITTED: [OrderState.CONFIRMED, OrderState.CANCELLED],
            OrderState.CONFIRMED: [OrderState.SHIPPED, OrderState.CANCELLED],
            OrderState.SHIPPED: [OrderState.DELIVERED],
            OrderState.DELIVERED: [],
            OrderState.CANCELLED: [],
        }
        
        if new_state in valid_transitions.get(self.state, []):
            self.state = new_state
            print(f"Transitioned to {new_state.name}")
        else:
            print(f"Invalid transition from {self.state.name} to {new_state.name}")

order = Order("ORD-001", total=99.99)
print(f"Initial: {order.state.name}")

order.transition(OrderState.SUBMITTED)
order.transition(OrderState.CONFIRMED)
order.transition(OrderState.SHIPPED)
order.transition(OrderState.DELIVERED)

print(f"Final: {order.state.name}")


# ============================================================
# Example 7: Dataclass for JSON Serialization
# ============================================================
print("\n=== JSON Serialization ===")

import json
from dataclasses import dataclass, asdict

@dataclass
class User:
    id: int
    name: str
    email: str
    active: bool = True
    
    def to_json(self) -> str:
        return json.dumps(asdict(self))
    
    @classmethod
    def from_json(cls, json_str: str) -> 'User':
        data = json.loads(json_str)
        return cls(**data)

user = User(1, "Alice", "alice@example.com")
json_str = user.to_json()
print(f"JSON: {json_str}")

restored = User.from_json(json_str)
print(f"Restored: {restored}")


# ============================================================
# Example 8: Registry Pattern with Dataclasses
# ============================================================
print("\n=== Registry Pattern ===")

from dataclasses import dataclass, field
from typing import Dict, Type, List

@dataclass
class Plugin:
    name: str
    version: str
    enabled: bool = True

class PluginRegistry:
    def __init__(self):
        self._plugins: Dict[str, Plugin] = {}
    
    def register(self, plugin: Plugin) -> None:
        self._plugins[plugin.name] = plugin
    
    def unregister(self, name: str) -> None:
        if name in self._plugins:
            del self._plugins[name]
    
    def get(self, name: str) -> Plugin:
        return self._plugins.get(name)
    
    def list_enabled(self) -> List[Plugin]:
        return [p for p in self._plugins.values() if p.enabled]

registry = PluginRegistry()
registry.register(Plugin("auth", "1.0.0"))
registry.register(Plugin("logger", "2.1.0", enabled=False))
registry.register(Plugin("cache", "1.5.0"))

print("Enabled plugins:")
for plugin in registry.list_enabled():
    print(f"  - {plugin.name} v{plugin.version}")
