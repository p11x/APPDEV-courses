# Tuples and Named Tuples

## What You'll Learn

- Understanding tuple immutability
- Tuple packing and unpacking
- Using namedtuple for structured data
- When to use tuples vs lists

## Prerequisites

- Read [01_lists_in_depth.md](./01_lists_in_depth.md) first

## Tuple Basics

Tuples are immutable sequences, perfect for fixed collections of items.

```python
# tuple_basics.py

# Create a tuple
point: tuple[int, int] = (10, 20)
coordinates: tuple[float, float, float] = (1.5, 2.5, 3.5)

# Tuples are immutable - this raises TypeError
# point[0] = 15  # TypeError!

# But you can create new tuples from existing ones
new_point: tuple[int, int] = (point[0] + 5, point[1] + 10)
```

## Tuple Packing and Unpacking

Pack multiple values into a tuple, then unpack them.

```python
# tuple_unpacking.py

# Packing
person = "Alice", 30, "Engineer"

# Unpacking
name, age, job = person
print(f"{name} is {age} and works as {job}")

# Extended unpacking
first, *middle, last = [1, 2, 3, 4, 5]
print(f"First: {first}, Middle: {middle}, Last: {last}")

# Swap values
a, b = 1, 2
a, b = b, a
print(f"Swapped: a={a}, b={b}")
```

## Named Tuples

Named tuples provide named fields while maintaining tuple behavior.

```python
# namedtuple_demo.py

from collections import namedtuple


# Define a named tuple
Point = namedtuple("Point", ["x", "y"])
Person = namedtuple("Person", ["name", "age", "occupation"])

# Create instances
p = Point(10, 20)
person = Person("Alice", 30, "Engineer")

# Access by name or index
print(f"Point: x={p.x}, y={p.y}")
print(f"Point[0]: {p[0]}, Point[1]: {p[1]}")

# Named tuples are immutable but can have defaults
ExtendedPoint = namedtuple("ExtendedPoint", ["x", "y", "z"], defaults=[0, 0])
p2 = ExtendedPoint(5, 10)
print(f"Default z: {p2.z}")
```

## Annotated Full Example

```python
# tuples_namedtuples_demo.py
"""Complete demonstration of tuples and namedtuples."""

from collections import namedtuple
from typing import Tuple


# Define namedtuples for structured data
Product = namedtuple("Product", ["name", "price", "quantity"])
Order = namedtuple("Order", ["order_id", "customer", "products", "total"])


def calculate_order_total(products: list[Product]) -> float:
    """Calculate total price for an order."""
    return sum(p.price * p.quantity for p in products)


def main() -> None:
    # Using regular tuples
    rgb_color: Tuple[int, int, int] = (255, 128, 0)
    r, g, b = rgb_color
    print(f"RGB: r={r}, g={g}, b={b}")
    
    # Using namedtuples
    products = [
        Product("Widget", 9.99, 3),
        Product("Gadget", 19.99, 2),
        Product("Gizmo", 4.99, 5)
    ]
    
    order = Order(
        order_id="ORD-001",
        customer="Alice",
        products=products,
        total=calculate_order_total(products)
    )
    
    print(f"\nOrder {order.order_id} for {order.customer}")
    print(f"Items:")
    for p in order.products:
        print(f"  - {p.name}: ${p.price} x {p.quantity}")
    print(f"Total: ${order.total:.2f}")


if __name__ == "__main__":
    main()
```

## Summary

- Understanding tuple immutability
- Tuple packing and unpacking
- Using namedtuple for structured data

## Next Steps

Continue to **[03_dicts_in_depth.md](./03_dicts_in_depth.md)**
