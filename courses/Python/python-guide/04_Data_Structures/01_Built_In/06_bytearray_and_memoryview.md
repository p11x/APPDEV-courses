# Bytearray and Memoryview

## What You'll Learn

- Understanding mutable byte sequences
- Using memoryview for zero-copy operations
- Working with binary data
- Buffer protocol basics

## Prerequisites

- Read [05_strings_as_sequences.md](./05_strings_as_sequences.md) first

## Bytearray

Bytearray provides a mutable sequence of bytes.

```python
# bytearray_demo.py

# Create bytearray
data: bytearray = bytearray(10)  # 10 zeroed bytes
data2: bytearray = bytearray(b"Hello")

# Mutable operations
data2[0] = ord("J")  # Change first byte
print(data2)        # bytearray(b'Jello')

# Append
data2.extend(b" World")
print(data2)
```

## Memoryview

Memoryview provides zero-copy access to binary data without copying.

```python
# memoryview_demo.py

# Create a bytearray
data: bytearray = bytearray(b"Hello World")

# Create memoryview (no copy!)
mv: memoryview = memoryview(data)

# Read data through memoryview
print(mv[0:5].tobytes())

# Modify through memoryview (affects original!)
mv[0] = ord("J")
print(data)
```

## Working with Binary Data

```python
# binary_data.py

import struct


# Pack binary data
packed: bytes = struct.pack("iff", 1, 3.14, 2.71)
print(f"Packed: {packed}")

# Unpack binary data
unpacked = struct.unpack("iff", packed)
print(f"Unpacked: {unpacked}")
```

## Annotated Full Example

```python
# bytearray_memoryview_demo.py
"""Complete demonstration of bytearray and memoryview."""

import struct


def main() -> None:
    # Bytearray basics
    ba = bytearray(b"Hello")
    ba[0] = ord("J")
    print(f"Modified bytearray: {ba}")
    
    # Memoryview for zero-copy operations
    data = bytearray(b"Binary data here")
    mv = memoryview(data)
    print(f"Memoryview slice: {mv[0:6].tobytes()}")
    
    # Using with struct
    values = (42, 3.14159, 100)
    packed = struct.pack("ifc", *values)
    print(f"Packed struct: {packed}")
    
    unpacked = struct.unpack("ifc", packed)
    print(f"Unpacked: {unpacked}")


if __name__ == "__main__":
    main()
```

## Summary

- Understanding mutable byte sequences
- Using memoryview for zero-copy operations
- Working with binary data

## Next Steps

Continue to **[07_deque_and_defaultdict.md](./07_deque_and_defaultdict.md)**
