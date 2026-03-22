# Example267: Bit Manipulation Basics
# Basic bit operations
print("Bit Manipulation:")

# Get bit
def get_bit(num, i):
    return (num >> i) & 1

print(f"Get bit: get_bit(5, 1) = {get_bit(5, 1)}")  # 5 = 101

# Set bit
def set_bit(num, i):
    return num | (1 << i)

print(f"Set bit: set_bit(4, 1) = {set_bit(4, 1)}")  # 4 = 100 -> 110 = 6

# Clear bit
def clear_bit(num, i):
    return num & ~(1 << i)

print(f"Clear bit: clear_bit(5, 0) = {clear_bit(5, 0)}")  # 5 = 101 -> 100 = 4

# Update bit
def update_bit(num, i, bit):
    if bit:
        return num | (1 << i)
    return num & ~(1 << i)

print(f"Update bit: update_bit(4, 0, 1) = {update_bit(4, 0, 1)}")

# Count bits set
def count_bits(n):
    count = 0
    while n:
        n &= n - 1
        count += 1
    return count

print(f"\nCount bits: count_bits(7) = {count_bits(7)}")  # 7 = 111 -> 3

# Power of 2 check
def is_power_of_two(n):
    return n > 0 and (n & (n - 1)) == 0

print(f"Is power of two: is_power_of_two(8) = {is_power_of_two(8)}")
print(f"Is power of two: is_power_of_two(10) = {is_power_of_two(10)}")

# Swap two numbers
def swap(a, b):
    a = a ^ b
    b = a ^ b
    a = a ^ b
    return a, b

print(f"\nSwap: swap(5, 3) = {swap(5, 3)}")

# Reverse bits
def reverse_bits(n):
    result = 0
    for _ in range(32):
        result = (result << 1) | (n & 1)
        n >>= 1
    return result

print(f"Reverse bits: reverse_bits(1) = {reverse_bits(1)}")

# Missing number
def missing_number(nums):
    result = 0
    for i, num in enumerate(nums):
        result ^= num ^ (i + 1)
    return result

print(f"\nMissing number:")
nums = [1, 2, 4, 5, 6]
print(f"Array: {nums}, Missing: {missing_number(nums)}")
