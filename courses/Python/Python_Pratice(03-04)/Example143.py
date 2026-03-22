# Example143.py
# Topic: Array Module


# ============================================================
# Example 1: Array Basics
# ============================================================
print("=== Array Basics ===")

from array import array

arr = array('i', [1, 2, 3, 4, 5])
print(f"Array: {arr}")
print(f"Type: {arr.typecode}")

arr.append(6)
print(f"After append: {arr}")


# ============================================================
# Example 2: Different Types
# ============================================================
print("\n=== Different Types ===")

from array import array

int_arr = array('i', [1, 2, 3])
float_arr = array('d', [1.5, 2.5, 3.5])
char_arr = array('u', 'hello')

print(f"Int: {int_arr}")
print(f"Float: {float_arr}")
print(f"Unicode: {char_arr}")


# ============================================================
# Example 3: Slicing Arrays
# ============================================================
print("\n=== Slicing ===")

from array import array

arr = array('i', range(10))
print(f"Original: {arr}")
print(f"arr[2:5]: {arr[2:5]}")
print(f"arr[::2]: {arr[::2]}")


# ============================================================
# Example 4: Memory Efficiency
# ============================================================
print("\n=== Memory Efficiency ===")

import sys
from array import array

py_list = [1, 2, 3, 4, 5] * 1000
int_array = array('i', [1, 2, 3, 4, 5] * 1000)

print(f"List size: {sys.getsizeof(py_list)} bytes")
print(f"Array size: {sys.getsizeof(int_array)} bytes")


# ============================================================
# Example 5: Convert to/from List
# ============================================================
print("\n=== Convert ===")

from array import array

arr = array('i', [1, 2, 3, 4, 5])
py_list = list(arr)
print(f"To list: {py_list}")

arr2 = array('i', py_list)
print(f"From list: {arr2}")


# ============================================================
# Example 6: Real-World: Sensor Data
# ============================================================
print("\n=== Real-World: Sensor Data ===")

from array import array

sensor_data = array('h')  # signed short

for i in range(10):
    sensor_data.append(i * 10)

print(f"Sensor readings: {sensor_data}")
print(f"Max: {max(sensor_data)}")
print(f"Sum: {sum(sensor_data)}")
