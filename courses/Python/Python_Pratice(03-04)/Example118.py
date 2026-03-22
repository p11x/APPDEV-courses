# Example118.py
# Topic: Bytearray and Memoryview Deep Dive

# Advanced operations with bytearray and memoryview.


# ============================================================
# Example 1: Bytearray Creation
# ============================================================
print("=== Bytearray Creation ===")

# From size
ba = bytearray(10)
print(f"Size 10: {ba}")

# From bytes
ba = bytearray(b"Hello")
print(f"From bytes: {ba}")

# From list
ba = bytearray([72, 101, 108, 108, 111])
print(f"From list: {ba}")


# ============================================================
# Example 2: Bytearray Modification
# ============================================================
print("\n=== Modification ===")

ba = bytearray(b"Hello")

# Change byte
ba[0] = ord("J")
print(f"After change: {ba}")

# Slice assignment
ba[1:3] = b"ai"
print(f"After slice: {ba}")

# Extend
ba.extend(b" World")
print(f"After extend: {ba}")


# ============================================================
# Example 3: Memoryview
# ============================================================
print("\n=== Memoryview ===")

data = bytearray(b"Hello World")

# Create memoryview
mv = memoryview(data)

# Read
print(f"Bytes: {mv.tobytes()}")
print(f"Hex: {mv.hex()}")

# Slice without copy
print(f"Slice: {mv[0:5].tobytes()}")


# ============================================================
# Example 4: Memoryview Modification
# ============================================================
print("\n=== Memoryview Modify ===")

data = bytearray(b"Hello")
mv = memoryview(data)

# Modify through memoryview
mv[0] = ord("J")
print(f"Original after modify: {data}")

# Modify slice
mv[1:3] = b"ai"
print(f"Original after slice: {data}")


# ============================================================
# Example 5: Struct Pack/Unpack
# ============================================================
print("\n=== Struct ===")

import struct

# Pack
packed = struct.pack("iff", 1, 3.14, 2.71)
print(f"Packed: {packed}")

# Unpack
unpacked = struct.unpack("iff", packed)
print(f"Unpacked: {unpacked}")

# Format
print(f"Size: {struct.calcsize('iff')} bytes")
