# 🔧 Structural and Behavioural Patterns

> Patterns for composing objects and defining communication.

## 🎯 What You'll Learn

- Adapter — wrap incompatible interfaces
- Decorator — add behavior transparently
- Facade — simple interface over complex systems
- Observer — notify multiple objects
- Strategy — swap algorithms at runtime
- Chain of Responsibility — pass through handler chain

## 📦 Prerequisites

- Completion of [02_creational_patterns.md](./02_creational_patterns.md)

---

## Adapter Pattern

Wrap incompatible interface to match expected one:

```python
# Old interface
class OldPaymentGateway:
    def pay_with_card(self, amount: int, card: str) -> bool:
        print(f"Paying {amount} with card {card}")
        return True


# New expected interface
class PaymentProcessor(Protocol):
    def process_payment(self, amount: float) -> bool:
        ...


# Adapter
class PaymentAdapter(PaymentProcessor):
    """Adapts old gateway to new interface."""
    
    def __init__(self, gateway: OldPaymentGateway, card: str):
        self.gateway = gateway
        self.card = card
    
    def process_payment(self, amount: float) -> bool:
        # Convert float to int cents
        cents = int(amount * 100)
        return self.gateway.pay_with_card(cents, self.card)


# Usage
gateway = OldPaymentGateway()
adapter = PaymentAdapter(gateway, "4111111111111111")
adapter.process_payment(99.99)
```

---

## Facade Pattern

Simple interface over complex subsystem:

```python
class VideoFile:
    def __init__(self, filename: str):
        self.filename = filename
        self.codec = "mp4"


class CodecFactory:
    def extract(self, file: VideoFile) -> str:
        return file.codec


class BitrateReader:
    def read(self, codec: str) -> bytes:
        return b"video data"


class AudioMixer:
    def fix(self, data: bytes) -> bytes:
        return data + b" audio_fixed"


# Complex subsystem
class VideoConverter:
    def convert(self, filename: str, format: str) -> str:
        file = VideoFile(filename)
        
        codec = CodecFactory().extract(file)
        data = BitrateReader().read(codec)
        audio = AudioMixer().fix(data)
        
        return f"converted_{filename}.{format}"


# Simple facade
converter = VideoConverter()
result = converter.convert("movie.mp4", "webm")
print(result)
```

---

## Observer Pattern

Notify multiple objects when state changes:

```python
from typing import Callable


class EventEmitter:
    """Simple observer implementation."""
    
    def __init__(self):
        self._observers: list[Callable] = []
    
    def on(self, callback: Callable) -> None:
        """Subscribe to events."""
        self._observers.append(callback)
    
    def emit(self, event: str, data: dict) -> None:
        """Notify all observers."""
        for observer in self._observers:
            observer(event, data)


# Usage
emitter = EventEmitter()

def logger(event: str, data: dict) -> None:
    print(f"[LOG] {event}: {data}")

def analytics(event: str, data: dict) -> None:
    print(f"[ANALYTICS] Tracking: {event}")

emitter.on(logger)
emitter.on(analytics)

emitter.emit("user_created", {"name": "Alice", "id": 1})
```

---

## Strategy Pattern

Swap algorithms at runtime:

```python
from typing import Protocol


class SortStrategy(Protocol):
    def sort(self, data: list[int]) -> list[int]:
        ...


class QuickSort:
    def sort(self, data: list[int]) -> list[int]:
        if len(data) <= 1:
            return data
        pivot = data[len(data) // 2]
        left = [x for x in data if x < pivot]
        middle = [x for x in data if x == pivot]
        right = [x for x in data if x > pivot]
        return self.sort(left) + middle + self.sort(right)


class BubbleSort:
    def sort(self, data: list[int]) -> list[int]:
        data = data.copy()
        n = len(data)
        for i in range(n):
            for j in range(0, n - i - 1):
                if data[j] > data[j + 1]:
                    data[j], data[j + 1] = data[j + 1], data[j]
        return data


class Sorter:
    def __init__(self, strategy: SortStrategy):
        self.strategy = strategy
    
    def set_strategy(self, strategy: SortStrategy) -> None:
        self.strategy = strategy
    
    def sort(self, data: list[int]) -> list[int]:
        return self.strategy.sort(data)


# Usage
sorter = Sorter(QuickSort())
print(sorter.sort([3, 1, 4, 1, 5]))

sorter.set_strategy(BubbleSort())
print(sorter.sort([3, 1, 4, 1, 5]))
```

---

## Summary

✅ **Adapter** — wrap incompatible interfaces

✅ **Facade** — simple interface over complexity

✅ **Observer** — notify multiple objects

✅ **Strategy** — swap algorithms at runtime

---

## ➡️ Next Steps

Continue to [02_App_Architecture/01_structuring_large_projects.md](../02_App_Architecture/01_structuring_large_projects.md)

---

## 🔗 Further Reading

- [Design Patterns](https://refactoring.guru/design-patterns)
- [Python Design Patterns](https://python-patterns.guide/)
