# Example228.py
# Topic: Array Module

# This file demonstrates the array module for memory-efficient arrays.


# ============================================================
# Example 1: Create Array
# ============================================================
print("=== Create Array ===")

from array import array

arr = array('i', [1, 2, 3, 4, 5])
print(f"Array: {arr}")


# ============================================================
# Example 2: Array Types
# ============================================================
print("\n=== Array Types ===")

from array import array

int_arr = array('i', [1, 2, 3])
float_arr = array('f', [1.0, 2.0, 3.0])
char_arr = array('u', 'hello')
print(f"Int: {int_arr}")
print(f"Float: {float_arr}")
print(f"Char: {char_arr}")


# ============================================================
# Example 3: Append and Extend
# ============================================================
print("\n=== Append ===")

from array import array

arr = array('i', [1, 2, 3])
arr.append(4)
arr.extend([5, 6, 7])
print(f"Array: {arr}")


# ============================================================
# Example 4: Index and Slice
# ============================================================
print("\n=== Index ===")

from array import array

arr = array('i', [1, 2, 3, 4, 5])
print(f"First: {arr[0]}")
print(f"Last: {arr[-1]}")
print(f"Slice: {arr[1:4]}")


# ============================================================
# Example 5: Buffer Info
# ============================================================
print("\n=== Buffer Info ===")

from array import array

arr = array('i', [1, 2, 3, 4, 5])
print(f"Buffer: {arr.buffer_info()}")


# ============================================================
# Example 6: Typecode
# ============================================================
print("\n=== Typecode ===")

from array import array

arr = array('i', [1, 2, 3])
print(f"Typecode: {arr.typecode}")


# ============================================================
# Example 7: From Bytes
# ============================================================
print("\n=== From Bytes ===")

from array import array

b = b'\x01\x00\x00\x00\x02\x00\x00\x00'
arr = array('i', b)
print(f"Array: {arr}")


# ============================================================
# Example 8: To File
# ============================================================
print("\n=== To File ===")

from array import array
import tempfile
import os

arr = array('i', [1, 2, 3, 4, 5])
with tempfile.NamedTemporaryFile(delete=False) as f:
    arr.tofile(f)
    fname = f.name

arr2 = array('i')
with open(fname, 'rb') as f:
    arr2.fromfile(f, 3)
print(f"Loaded: {arr2}")
os.unlink(fname)


# ============================================================
# Example 9: Memory Usage
# ============================================================
print("\n=== Memory ===")

import sys
from array import array

arr = array('i', range(1000))
lst = list(range(1000))
print(f"Array size: {sys.getsizeof(arr)}")
print(f"List size: {sys.getsizeof(lst)}")


# ============================================================
# Example 10: Byte Swap
# ============================================================
print("\n=== Byte Swap ===")

from array import array

arr = array('i', [1, 2, 3])
arr.byteswap()
print(f"Swapped: {arr}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
ARRAY MODULE:
- 'i': signed int
- 'f': float
- 'u': unicode
- Memory efficient
- tofile/fromfile
""")
