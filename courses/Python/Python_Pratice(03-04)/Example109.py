# Example109.py
# Topic: Bytearray and Memoryview

# This file demonstrates bytearray and memoryview.


# ============================================================
# Example 1: Bytearray
# ============================================================
print("=== Bytearray ===")

# Create bytearray
data = bytearray(10)  # 10 zeroed bytes
print(f"Zeroed: {data}")

# From bytes
data = bytearray(b"Hello")
print(f"From bytes: {data}")

# Mutable - change byte
data[0] = ord("J")
print(f"After change: {data}")

# Append
data.extend(b" World")
print(f"After extend: {data}")


# ============================================================
# Example 2: Memoryview
# ============================================================
print("\n=== Memoryview ===")

# Create from bytearray
data = bytearray(b"Hello World")
mv = memoryview(data)

# Read without copying
print(f"Slice: {mv[0:5].tobytes()}")

# Modify through memoryview (affects original!)
mv[0] = ord("J")
print(f"Original after modify: {data}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
BYTEARRAY:
- Mutable bytes
- bytearray(size) or bytearray(b"...")
- data[index] = value

MEMORYVIEW:
- Zero-copy access to binary data
- mv[start:end].tobytes()
- Modifications affect original
""")
